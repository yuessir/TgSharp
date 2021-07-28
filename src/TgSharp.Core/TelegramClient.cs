using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TgSharp.TL;
using TgSharp.TL.Account;
using TgSharp.TL.Auth;
using TgSharp.TL.Contacts;
using TgSharp.TL.Help;
using TgSharp.TL.Messages;
using TgSharp.TL.Upload;
using TgSharp.Core.Auth;
using TgSharp.Core.Exceptions;
using TgSharp.Core.MTProto.Crypto;
using TgSharp.Core.Network;
using TgSharp.Core.Network.Exceptions;
using TgSharp.Core.Utils;
using TLAuthorization = TgSharp.TL.Auth.TLAuthorization;

namespace TgSharp.Core
{
    public class TelegramClient : IDisposable
    {
        private MtProtoSender sender;
        private TcpTransport transport;
        private int autoReconnectMaxAttempts;
        private readonly string apiHash;
        private readonly int apiId;
        private readonly string sessionUserId;
        private readonly ISessionStore store;
        private List<TLDcOption> dcOptions;
        private readonly TcpClientConnectionHandler handler;
        private readonly DataCenterIPVersion dcIpVersion;

        public Session Session { get; private set; }

        /// <summary>
        /// Creates a new TelegramClient
        /// </summary>
        /// <param name="apiId">The API ID provided by Telegram. Get one at https://my.telegram.org </param>
        /// <param name="apiHash">The API Hash provided by Telegram. Get one at https://my.telegram.org </param>
        /// <param name="store">An ISessionStore object that will handle the session</param>
        /// <param name="sessionUserId">The name of the session that tracks login info about this TelegramClient connection</param>
        /// <param name="handler">A delegate to invoke when a connection is needed and that will return a TcpClient that will be used to connect</param>
        /// <param name="dcIpVersion">Indicates the preferred IpAddress version to use to connect to a Telegram server</param>
        public TelegramClient(int apiId, string apiHash,
            DataCenterIPVersion dcIpVersion = DataCenterIPVersion.Default,
            ISessionStore store = null,
            string sessionUserId = "session",
            TcpClientConnectionHandler handler = null
            )
        {
            if (apiId == default(int))
                throw new MissingApiConfigurationException("API_ID");
            if (string.IsNullOrEmpty(apiHash))
                throw new MissingApiConfigurationException("API_HASH");

            if (store == null)
            {
                store = JsonFileSessionStore.DefaultSessionStore();
            }

            this.store = store;

            this.apiHash = apiHash;
            this.apiId = apiId;
            this.handler = handler;
            this.dcIpVersion = dcIpVersion;

            this.sessionUserId = sessionUserId;
        }

        public async Task ConnectAsync(int autoReconnectMaxAttempts = 0, CancellationToken token = default(CancellationToken))
        {
            await ConnectInternalAsync(false, autoReconnectMaxAttempts, token);
        }

        private async Task ConnectInternalAsync(bool reconnect = false, int autoReconnectMaxAttempts = 0, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();

            Session = SessionFactory.TryLoadOrCreateNew(store, sessionUserId);
            transport = new TcpTransport(Session.DataCenter.Address, Session.DataCenter.Port, this.handler);
            this.autoReconnectMaxAttempts = autoReconnectMaxAttempts;

            if (Session.AuthKey == null || reconnect)
            {
                var result = await Authenticator.DoAuthentication(transport, token).ConfigureAwait(false);
                Session.AuthKey = result.AuthKey;
                Session.TimeOffset = result.TimeOffset;
            }

            sender = new MtProtoSender(transport, store, Session);

            //set-up layer
            var config = new TLRequestGetConfig();
            var request = new TLRequestInitConnection()
            {
                ApiId = apiId,
                AppVersion = "7.8.4",
                DeviceModel = "iPhone 11",
                LangCode = "nl",
                Query = config,
                SystemVersion = "iOS 14.4",
                SystemLangCode = "nl",
                LangPack = ""
            };
            var invokewithLayer = new TLRequestInvokeWithLayer() { Layer = 108, Query = request };
            await sender.Send(invokewithLayer, token).ConfigureAwait(false);
            await sender.Receive(invokewithLayer, token).ConfigureAwait(false);

            dcOptions = ((TLConfig)invokewithLayer.Response).DcOptions.ToList();
        }

        private async Task ReconnectToDcAsync(int dcId, CancellationToken token = default(CancellationToken))
        {
            token.ThrowIfCancellationRequested();

            if (dcOptions == null || !dcOptions.Any())
                throw new InvalidOperationException($"Can't reconnect. Establish initial connection first.");

            TLExportedAuthorization exported = null;
            if (Session.AuthenticatedSuccessfully)
            {
                TLRequestExportAuthorization exportAuthorization = new TLRequestExportAuthorization() { DcId = dcId };
                exported = await SendRequestAsync<TLExportedAuthorization>(exportAuthorization, token: token).ConfigureAwait(false);
            }

            IEnumerable<TLDcOption> dcs;
            if (dcIpVersion == DataCenterIPVersion.OnlyIPv6)
                dcs = dcOptions.Where(d => d.Id == dcId && d.Ipv6); // selects only ipv6 addresses 	
            else if (dcIpVersion == DataCenterIPVersion.OnlyIPv4)
                dcs = dcOptions.Where(d => d.Id == dcId && !d.Ipv6); // selects only ipv4 addresses
            else
                dcs = dcOptions.Where(d => d.Id == dcId); // any

            dcs = dcs.Where(d => !d.MediaOnly);

            TLDcOption dc;
            if (dcIpVersion != DataCenterIPVersion.Default)
            {
                if (!dcs.Any())
                    throw new Exception($"Telegram server didn't provide us with any IPAddress that matches your preferences. If you chose OnlyIPvX, try switch to PreferIPvX instead.");
                dcs = dcs.OrderBy(d => d.Ipv6);
                dc = dcIpVersion == DataCenterIPVersion.PreferIPv4 ? dcs.First() : dcs.Last(); // ipv4 addresses are at the beginning of the list because it was ordered
            }
            else
                dc = dcs.First();

            var dataCenter = new DataCenter(dcId, dc.IpAddress, dc.Port);
            Session.DataCenter = dataCenter;
            this.store.Save(Session);

            await ConnectInternalAsync(true, autoReconnectMaxAttempts, token).ConfigureAwait(false);

            if (Session.AuthenticatedSuccessfully)
            {
                TLRequestImportAuthorization importAuthorization = new TLRequestImportAuthorization() { Id = exported.Id, Bytes = exported.Bytes };
                var imported = await SendRequestAsync<TLAuthorization>(importAuthorization, token: token).ConfigureAwait(false);
                OnUserAuthenticated((TLUser)imported.User);
            }
        }

        private async Task RequestWithDcMigration(TLMethod request, CancellationToken token = default(CancellationToken))
        {
            if (sender == null)
                throw new InvalidOperationException("Not connected!");

            bool completed = false;
            int attempts = 0;

            while (!completed)
            {
                try
                {
                    await sender.Send(request, token).ConfigureAwait(false);
                    await sender.Receive(request, token).ConfigureAwait(false);

                    completed = true;
                }
                catch (DataCenterMigrationException e)
                {
                    if (Session.DataCenter.DataCenterId.HasValue &&
                        Session.DataCenter.DataCenterId.Value == e.DC)
                    {
                        throw new Exception($"Telegram server replied requesting a migration to DataCenter {e.DC} when this connection was already using this DataCenter", e);
                    }

                    await ReconnectToDcAsync(e.DC, token).ConfigureAwait(false);

                    // prepare the request for another try
                    request.ConfirmReceived = false;
                }
                catch (IOException)
                {
                    if (attempts++ < autoReconnectMaxAttempts)
                    {
                        await ConnectInternalAsync(false, autoReconnectMaxAttempts, token).ConfigureAwait(false);

                        // prepare the request for another try
                        request.ConfirmReceived = false;
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        public bool IsUserAuthorized()
        {
            return Session.AuthenticatedSuccessfully;
        }

        public async Task<TLSentCode> SendCodeRequestAsync(string phoneNumber, CancellationToken token = default(CancellationToken))
        {
            if (String.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentNullException(nameof(phoneNumber));

            var request = new TLRequestSendCode() { PhoneNumber = phoneNumber, ApiId = apiId, ApiHash = apiHash, Settings = new TLCodeSettings { } };

            return await SendRequestAsync<TLSentCode>(request, token: token).ConfigureAwait(false);
        }

        public async Task<TLSentCode> ResendCodeRequestAsync(string phoneNumber, string phoneCodeHash, CancellationToken token = default(CancellationToken))
        {
            if (String.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentNullException(nameof(phoneNumber));

            if (String.IsNullOrWhiteSpace(phoneCodeHash))
                throw new ArgumentNullException(nameof(phoneCodeHash));

            var request = new TLRequestResendCode() { PhoneNumber = phoneNumber, PhoneCodeHash = phoneCodeHash };

            return await SendRequestAsync<TLSentCode>(request, token: token).ConfigureAwait(false);
        }

        public async Task<TLUser> MakeAuthAsync(string phoneNumber, string phoneCodeHash, string code, string password = "", string firstName = "", string lastName = "", CancellationToken token = default(CancellationToken))
        {
            if (String.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentNullException(nameof(phoneNumber));

            if (String.IsNullOrWhiteSpace(phoneCodeHash))
                throw new ArgumentNullException(nameof(phoneCodeHash));

            if (String.IsNullOrWhiteSpace(code))
                throw new ArgumentNullException(nameof(code));

            var request = new TLRequestSignIn() { PhoneNumber = phoneNumber, PhoneCodeHash = phoneCodeHash, PhoneCode = code };
            TLRequestCheckPassword requestCheckPassword = null;

            try
            {
                await RequestWithDcMigration(request, token).ConfigureAwait(false);
            }
            catch (CloudPasswordNeededException)
            {
                if (password != "")
                {
                    requestCheckPassword = new TLRequestCheckPassword { Password = await this.CheckPassword(password, token) };
                    await RequestWithDcMigration(requestCheckPassword, token).ConfigureAwait(false);
                }
                else throw;
            }

            if (requestCheckPassword == null && request.Response is TLAuthorization authorization1)
            {
                OnUserAuthenticated(((TLUser)authorization1.User));
                return ((TLUser)authorization1.User);
            }

            if (requestCheckPassword?.Response is TLAuthorization authorization2)
            {
                OnUserAuthenticated(((TLUser)authorization2.User));
                return ((TLUser)authorization2.User);
            }

            while (true)
            {
                try
                {
                    var signUpRequest = new TLRequestSignUp() { PhoneNumber = phoneNumber, PhoneCodeHash = phoneCodeHash, FirstName = firstName, LastName = lastName };
                    await RequestWithDcMigration(signUpRequest, token).ConfigureAwait(false);
                    OnUserAuthenticated((TLUser)signUpRequest.Response.User);
                    return (TLUser)signUpRequest.Response.User;
                }
                catch (FloodException ex)
                {
                    await Task.Delay(ex.TimeToWait, token);
                }
            }
        }

        /// <param name="retryOnFloodException">Whether to automatically wait and resend the request when a FloodException shorter than 60 seconds is thrown</param>
        public async Task<T> SendRequestAsync<T>(TLMethod methodToExecute, bool retryOnFloodException = true, CancellationToken token = default(CancellationToken))
        {
            while (true)
            {
                try
                {
                    await RequestWithDcMigration(methodToExecute, token).ConfigureAwait(false);

                    break;
                }
                catch (FloodException ex)
                {
                    if (!retryOnFloodException || ex.TimeToWait > TimeSpan.FromSeconds(60))
                    {
                        throw;
                    }

                    await Task.Delay(ex.TimeToWait.Add(TimeSpan.FromSeconds(1)), token);
                }
            }

            var result = methodToExecute.GetType().GetProperty("Response").GetValue(methodToExecute);

            return (T)result;
        }

        /// <param name="retryOnFloodException">Whether to automatically wait and resend the request when a FloodException shorter than 60 seconds is thrown</param>
        public async Task<T> SendAuthenticatedRequestAsync<T>(TLMethod methodToExecute, bool retryOnFloodException = true, CancellationToken token = default(CancellationToken))
        {
            if (!IsUserAuthorized())
                throw new InvalidOperationException("Authorize user first!");

            return await SendRequestAsync<T>(methodToExecute, retryOnFloodException, token)
                .ConfigureAwait(false);
        }

        public async Task<TLUser> UpdateUsernameAsync(string username, CancellationToken token = default(CancellationToken))
        {
            var req = new TLRequestUpdateUsername { Username = username };

            return await SendAuthenticatedRequestAsync<TLUser>(req, token: token)
                .ConfigureAwait(false);
        }

        public async Task<bool> CheckUsernameAsync(string username, CancellationToken token = default(CancellationToken))
        {
            var req = new TLRequestCheckUsername { Username = username };

            return await SendAuthenticatedRequestAsync<bool>(req, token: token)
                .ConfigureAwait(false);
        }

        public async Task<TLImportedContacts> ImportContactsAsync(IReadOnlyList<TLInputPhoneContact> contacts, CancellationToken token = default(CancellationToken))
        {
            var req = new TLRequestImportContacts { Contacts = new TLVector<TLInputPhoneContact>(contacts) };

            return await SendAuthenticatedRequestAsync<TLImportedContacts>(req, token: token)
                .ConfigureAwait(false);
        }

        public async Task<bool> DeleteContactsAsync(IReadOnlyList<TLAbsInputUser> users, CancellationToken token = default(CancellationToken))
        {
            var req = new TLRequestDeleteContacts { Id = new TLVector<TLAbsInputUser>(users) };

            return await SendAuthenticatedRequestAsync<bool>(req, token: token)
                .ConfigureAwait(false);
        }

        public async Task<TLContacts> GetContactsAsync(CancellationToken token = default(CancellationToken))
        {
            var req = new TLRequestGetContacts() { Hash = 0 };

            return await SendAuthenticatedRequestAsync<TLContacts>(req, token: token)
                .ConfigureAwait(false);
        }

        public async Task<TLAbsUpdates> SendMessageAsync(TLAbsInputPeer peer, string message, CancellationToken token = default(CancellationToken))
        {
            return await SendAuthenticatedRequestAsync<TLAbsUpdates>(
                    new TLRequestSendMessage()
                    {
                        Peer = peer,
                        Message = message,
                        RandomId = Helpers.GenerateRandomLong()
                    }, token: token)
                .ConfigureAwait(false);
        }

        public async Task<Boolean> SendTypingAsync(TLAbsInputPeer peer, CancellationToken token = default(CancellationToken))
        {
            var req = new TLRequestSetTyping()
            {
                Action = new TLSendMessageTypingAction(),
                Peer = peer
            };
            return await SendAuthenticatedRequestAsync<Boolean>(req, token: token)
                .ConfigureAwait(false);
        }

        public async Task<TLAbsDialogs> GetUserDialogsAsync(int offsetDate = 0, int offsetId = 0, TLAbsInputPeer offsetPeer = null, int limit = 100, CancellationToken token = default(CancellationToken))
        {
            if (offsetPeer == null)
                offsetPeer = new TLInputPeerSelf();

            var req = new TLRequestGetDialogs()
            {
                OffsetDate = offsetDate,
                OffsetId = offsetId,
                OffsetPeer = offsetPeer,
                Limit = limit
            };
            return await SendAuthenticatedRequestAsync<TLAbsDialogs>(req, token: token)
                .ConfigureAwait(false);
        }

        public async Task<TLAbsUpdates> SendUploadedPhoto(TLAbsInputPeer peer, TLAbsInputFile file, CancellationToken token = default(CancellationToken))
        {
            return await SendAuthenticatedRequestAsync<TLAbsUpdates>(new TLRequestSendMedia()
            {
                RandomId = Helpers.GenerateRandomLong(),
                Background = false,
                ClearDraft = false,
                Media = new TLInputMediaUploadedPhoto() { File = file },
                Peer = peer
            }, token: token)
                .ConfigureAwait(false);
        }

        public async Task<TLAbsUpdates> SendUploadedDocument(
            TLAbsInputPeer peer, TLAbsInputFile file, string mimeType, TLVector<TLAbsDocumentAttribute> attributes, CancellationToken token = default(CancellationToken))
        {
            return await SendAuthenticatedRequestAsync<TLAbsUpdates>(new TLRequestSendMedia()
            {
                RandomId = Helpers.GenerateRandomLong(),
                Background = false,
                ClearDraft = false,
                Media = new TLInputMediaUploadedDocument()
                {
                    File = file,
                    MimeType = mimeType,
                    Attributes = attributes
                },
                Peer = peer
            }, token: token)
                .ConfigureAwait(false);
        }

        public async Task<TLFile> GetFile(TLAbsInputFileLocation location, int filePartSize, int offset = 0, CancellationToken token = default(CancellationToken))
        {
            TLFile result = await SendAuthenticatedRequestAsync<TLFile>(new TLRequestGetFile
            {
                Location = location,
                Limit = filePartSize,
                Offset = offset
            }, token: token)
                .ConfigureAwait(false);
            return result;
        }

        public async Task SendPingAsync(CancellationToken token = default(CancellationToken))
        {
            await sender.SendPingAsync(token)
                .ConfigureAwait(false);
        }

        public async Task<TLAbsMessages> GetHistoryAsync(TLAbsInputPeer peer, int offsetId = 0, int offsetDate = 0, int addOffset = 0, int limit = 100, int maxId = 0, int minId = 0, CancellationToken token = default(CancellationToken))
        {
            var req = new TLRequestGetHistory()
            {
                Peer = peer,
                OffsetId = offsetId,
                OffsetDate = offsetDate,
                AddOffset = addOffset,
                Limit = limit,
                MaxId = maxId,
                MinId = minId
            };
            return await SendAuthenticatedRequestAsync<TLAbsMessages>(req, token: token)
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Serch user or chat. API: contacts.search#11f812d8 q:string limit:int = contacts.Found;
        /// </summary>
        /// <param name="q">User or chat name</param>
        /// <param name="limit">Max result count</param>
        /// <returns></returns>
        public async Task<TLFound> SearchUserAsync(string q, int limit = 10, CancellationToken token = default(CancellationToken))
        {
            var r = new TL.Contacts.TLRequestSearch
            {
                Q = q,
                Limit = limit
            };

            return await SendAuthenticatedRequestAsync<TLFound>(r, token: token)
                .ConfigureAwait(false);
        }

        private void OnUserAuthenticated(TLUser TLUser)
        {
            Session.AuthenticatedSuccessfully = true;
            Session.SessionExpires = int.MaxValue;

            this.store.Save(Session);
        }

        public bool IsConnected
        {
            get
            {
                if (transport == null)
                    return false;
                return transport.IsConnected;
            }
        }

        public void Dispose()
        {
            if (transport != null)
            {
                transport.Dispose();
                transport = null;
            }
        }
    }
}
