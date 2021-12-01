using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(1456906823)]
    public class TLChannelAdminLogEventActionToggleGroupCallSetting : TLAbsChannelAdminLogEventAction
    {
        public override int Constructor
        {
            get
            {
                return 1456906823;
            }
        }

        public bool JoinMuted { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            JoinMuted = BoolUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            BoolUtil.Serialize(JoinMuted, bw);
        }
    }
}
