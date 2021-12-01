using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(-1770029977)]
    public class TLPhoneCall : TLAbsPhoneCall
    {
        public override int Constructor
        {
            get
            {
                return -1770029977;
            }
        }

        public int Flags { get; set; }
        public bool P2pAllowed { get; set; }
        public bool Video { get; set; }
        public long Id { get; set; }
        public long AccessHash { get; set; }
        public int Date { get; set; }
        public long AdminId { get; set; }
        public long ParticipantId { get; set; }
        public byte[] GAOrB { get; set; }
        public long KeyFingerprint { get; set; }
        public TLPhoneCallProtocol Protocol { get; set; }
        public TLVector<TLAbsPhoneConnection> Connections { get; set; }
        public int StartDate { get; set; }

        public void ComputeFlags()
        {
            Flags = 0;
Flags = P2pAllowed ? (Flags | 32) : (Flags & ~32);
Flags = Video ? (Flags | 64) : (Flags & ~64);

        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            P2pAllowed = (Flags & 32) != 0;
            Video = (Flags & 64) != 0;
            Id = br.ReadInt64();
            AccessHash = br.ReadInt64();
            Date = br.ReadInt32();
            AdminId = br.ReadInt64();
            ParticipantId = br.ReadInt64();
            GAOrB = BytesUtil.Deserialize(br);
            KeyFingerprint = br.ReadInt64();
            Protocol = (TLPhoneCallProtocol)ObjectUtils.DeserializeObject(br);
            Connections = (TLVector<TLAbsPhoneConnection>)ObjectUtils.DeserializeVector<TLAbsPhoneConnection>(br);
            StartDate = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            bw.Write(Id);
            bw.Write(AccessHash);
            bw.Write(Date);
            bw.Write(AdminId);
            bw.Write(ParticipantId);
            BytesUtil.Serialize(GAOrB, bw);
            bw.Write(KeyFingerprint);
            ObjectUtils.SerializeObject(Protocol, bw);
            ObjectUtils.SerializeObject(Connections, bw);
            bw.Write(StartDate);
        }
    }
}
