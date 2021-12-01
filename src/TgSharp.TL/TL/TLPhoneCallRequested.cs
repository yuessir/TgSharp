using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(347139340)]
    public class TLPhoneCallRequested : TLAbsPhoneCall
    {
        public override int Constructor
        {
            get
            {
                return 347139340;
            }
        }

        public int Flags { get; set; }
        public bool Video { get; set; }
        public long Id { get; set; }
        public long AccessHash { get; set; }
        public int Date { get; set; }
        public long AdminId { get; set; }
        public long ParticipantId { get; set; }
        public byte[] GAHash { get; set; }
        public TLPhoneCallProtocol Protocol { get; set; }

        public void ComputeFlags()
        {
            Flags = 0;
Flags = Video ? (Flags | 64) : (Flags & ~64);

        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Video = (Flags & 64) != 0;
            Id = br.ReadInt64();
            AccessHash = br.ReadInt64();
            Date = br.ReadInt32();
            AdminId = br.ReadInt64();
            ParticipantId = br.ReadInt64();
            GAHash = BytesUtil.Deserialize(br);
            Protocol = (TLPhoneCallProtocol)ObjectUtils.DeserializeObject(br);
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
            BytesUtil.Serialize(GAHash, bw);
            ObjectUtils.SerializeObject(Protocol, bw);
        }
    }
}
