using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(1844969806)]
    public class TLChannelParticipantBanned : TLAbsChannelParticipant
    {
        public override int Constructor
        {
            get
            {
                return 1844969806;
            }
        }

        public int Flags { get; set; }
        public bool Left { get; set; }
        public TLAbsPeer Peer { get; set; }
        public long KickedBy { get; set; }
        public int Date { get; set; }
        public TLChatBannedRights BannedRights { get; set; }

        public void ComputeFlags()
        {
            Flags = 0;
Flags = Left ? (Flags | 1) : (Flags & ~1);

        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Left = (Flags & 1) != 0;
            Peer = (TLAbsPeer)ObjectUtils.DeserializeObject(br);
            KickedBy = br.ReadInt64();
            Date = br.ReadInt32();
            BannedRights = (TLChatBannedRights)ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            ObjectUtils.SerializeObject(Peer, bw);
            bw.Write(KickedBy);
            bw.Write(Date);
            ObjectUtils.SerializeObject(BannedRights, bw);
        }
    }
}
