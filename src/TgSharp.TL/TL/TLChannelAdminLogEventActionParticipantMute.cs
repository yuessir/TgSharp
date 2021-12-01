using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(-115071790)]
    public class TLChannelAdminLogEventActionParticipantMute : TLAbsChannelAdminLogEventAction
    {
        public override int Constructor
        {
            get
            {
                return -115071790;
            }
        }

        public TLGroupCallParticipant Participant { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Participant = (TLGroupCallParticipant)ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Participant, bw);
        }
    }
}
