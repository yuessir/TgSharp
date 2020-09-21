using System;
using System.IO;
using Newtonsoft.Json;
using TgSharp.TL;
using TgSharp.Core.MTProto;
using TgSharp.Core.MTProto.Crypto;

namespace TgSharp.Core
{
    public interface ISessionStore
    {
        void Save(Session session);
        Session Load(string sessionUserId);
    }

    [Obsolete("Use JsonFileSessionStore")]
    public class FileSessionStore : BinaryFileSessionStore
    {
    }

    public class BinaryFileSessionStore : ISessionStore
    {
        private readonly DirectoryInfo basePath;

        public BinaryFileSessionStore(DirectoryInfo basePath = null)
        {
            if (basePath != null && !basePath.Exists)
            {
                throw new ArgumentException("basePath doesn't exist", nameof(basePath));
            }
            this.basePath = basePath;
        }

        public void Save(Session session)
        {
            string sessionFileName = $"{session.SessionUserId}.dat";
            var sessionPath = basePath == null ? sessionFileName :
                Path.Combine(basePath.FullName, sessionFileName);

            using (var stream = new FileStream(sessionPath, FileMode.OpenOrCreate))
            {
                var result = ToBytes(session);
                stream.Write(result, 0, result.Length);
            }
        }

        public Session Load(string sessionUserId)
        {
            string sessionFileName = $"{sessionUserId}.dat";
            var sessionPath = basePath == null ? sessionFileName :
                Path.Combine(basePath.FullName, sessionFileName);

            if (!File.Exists(sessionPath))
                return null;

            using (var stream = new FileStream(sessionPath, FileMode.Open))
            {
                var buffer = new byte[2048];
                stream.Read(buffer, 0, 2048);

                return FromBytes(buffer, sessionUserId);
            }
        }

        public byte [] ToBytes (Session session)
        {
            using (var stream = new MemoryStream ())
            using (var writer = new BinaryWriter (stream)) {
                writer.Write (session.Id);
                writer.Write (session.Sequence);
                writer.Write (session.Salt);
                writer.Write (session.LastMessageId);
                writer.Write (session.TimeOffset);
                Serializers.String.Write (writer, session.DataCenter.Address);
                writer.Write (session.DataCenter.Port);

                if (session.AuthenticatedSuccessfully) {
                    writer.Write (1);
                    writer.Write (session.SessionExpires);
                } else {
                    writer.Write (0);
                }

                Serializers.Bytes.Write (writer, session.AuthKey.Data);

                return stream.ToArray ();
            }
        }

        public static Session FromBytes (byte [] buffer, string sessionUserId)
        {
            using (var stream = new MemoryStream (buffer))
            using (var reader = new BinaryReader (stream)) {
                var id = reader.ReadUInt64 ();
                var sequence = reader.ReadInt32 ();
                var salt = reader.ReadUInt64 ();
                var lastMessageId = reader.ReadInt64 ();
                var timeOffset = reader.ReadInt32 ();
                var serverAddress = Serializers.String.Read (reader);
                var port = reader.ReadInt32 ();

                var authenticatedSuccessfully = reader.ReadInt32 () == 1;
                int sessionExpires = 0;
                if (authenticatedSuccessfully) {
                    sessionExpires = reader.ReadInt32 ();
                }

                var authData = Serializers.Bytes.Read (reader);
                var defaultDataCenter = new DataCenter (null, serverAddress, port);

                return new Session {
                    AuthKey = new AuthKey (authData),
                    Id = id,
                    Salt = salt,
                    Sequence = sequence,
                    LastMessageId = lastMessageId,
                    TimeOffset = timeOffset,
                    SessionExpires = sessionExpires,
                    AuthenticatedSuccessfully = authenticatedSuccessfully,
                    SessionUserId = sessionUserId,
                    DataCenter = defaultDataCenter,
                };
            }
        }
    }

    public class JsonFileSessionStore : ISessionStore
    {
        private readonly DirectoryInfo basePath;
        private readonly JsonSerializerSettings jsonSerializerSettings;

        internal static JsonFileSessionStore DefaultSessionStore ()
        {
            return new JsonFileSessionStore (null, new JsonSerializerSettings () {
                Formatting = Formatting.Indented
            });
        }

        public JsonFileSessionStore(DirectoryInfo basePath = null, JsonSerializerSettings jsonSerializerSettings = null)
        {
            if (basePath != null && !basePath.Exists)
            {
                throw new ArgumentException("basePath doesn't exist", nameof(basePath));
            }

            this.basePath = basePath;
            this.jsonSerializerSettings = jsonSerializerSettings;
        }

        public void Save(Session session)
        {
            File.WriteAllText(GetSessionPath(session.SessionUserId), JsonConvert.SerializeObject(session, jsonSerializerSettings));
        }

        public Session Load(string sessionUserId)
        {
            string sessionPath = GetSessionPath(sessionUserId);

            if (File.Exists(sessionPath))
            {
                return JsonConvert.DeserializeObject<Session> (File.ReadAllText (sessionPath), jsonSerializerSettings);
            }

            return null;
        }

        private string GetSessionPath(string sessionUserId)
        {
            return Path.Combine(basePath?.FullName ?? string.Empty, sessionUserId + ".json");
        }
    }

    public class FakeSessionStore : ISessionStore
    {
        public void Save(Session session)
        {

        }

        public Session Load(string sessionUserId)
        {
            return null;
        }
    }

    internal static class SessionFactory
    {
        internal static Session TryLoadOrCreateNew (ISessionStore store, string sessionUserId)
        {
            var session = store.Load (sessionUserId);
            if (null == session) {
                var defaultDataCenter = new DataCenter (null);
                session = new Session {
                    Id = GenerateRandomUlong (),
                    SessionUserId = sessionUserId,
                    DataCenter = defaultDataCenter,
                };
            }
            return session;
        }

        private static ulong GenerateRandomUlong ()
        {
            var random = new Random ();
            ulong rand = (((ulong)random.Next ()) << 32) | ((ulong)random.Next ());
            return rand;
        }
    }

    public class Session
    {
        public string SessionUserId { get; set; }
        public DataCenter DataCenter { get; set; }
        public AuthKey AuthKey { get; set; }
        public ulong Id { get; set; }
        public int Sequence { get; set; }
        internal object Lock { get; } = new object();
        public ulong Salt { get; set; }
        public int TimeOffset { get; set; }
        public long LastMessageId { get; set; }
        public int SessionExpires { get; set; }
        public bool AuthenticatedSuccessfully { get; set; } = false;
        private readonly Random random = new Random();

        public long GetNewMessageId()
        {
            long time = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds);
            long newMessageId = ((time / 1000 + TimeOffset) << 32) |
                                ((time % 1000) << 22) |
                                (random.Next(524288) << 2); // 2^19
                                                            // [ unix timestamp : 32 bit] [ milliseconds : 10 bit ] [ buffer space : 1 bit ] [ random : 19 bit ] [ msg_id type : 2 bit ] = [ msg_id : 64 bit ]

            if (LastMessageId >= newMessageId)
            {
                newMessageId = LastMessageId + 4;
            }

            LastMessageId = newMessageId;
            return newMessageId;
        }
    }
}
