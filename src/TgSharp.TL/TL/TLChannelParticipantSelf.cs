using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(682146919)]
    public class TLChannelParticipantSelf : TLAbsChannelParticipant
    {
        public override int Constructor
        {
            get
            {
                return 682146919;
            }
        }

        public long UserId { get; set; }
        public long InviterId { get; set; }
        public int Date { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            UserId = br.ReadInt64();
            InviterId = br.ReadInt64();
            Date = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(UserId);
            bw.Write(InviterId);
            bw.Write(Date);
        }
    }
}
