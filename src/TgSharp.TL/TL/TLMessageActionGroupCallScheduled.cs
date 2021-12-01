using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(-1281329567)]
    public class TLMessageActionGroupCallScheduled : TLAbsMessageAction
    {
        public override int Constructor
        {
            get
            {
                return -1281329567;
            }
        }

        public TLInputGroupCall Call { get; set; }
        public int ScheduleDate { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Call = (TLInputGroupCall)ObjectUtils.DeserializeObject(br);
            ScheduleDate = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Call, bw);
            bw.Write(ScheduleDate);
        }
    }
}
