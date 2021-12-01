using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(1855199800)]
    public class TLChannelAdminLogEventActionChangeHistoryTTL : TLAbsChannelAdminLogEventAction
    {
        public override int Constructor
        {
            get
            {
                return 1855199800;
            }
        }

        public int PrevValue { get; set; }
        public int NewValue { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            PrevValue = br.ReadInt32();
            NewValue = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(PrevValue);
            bw.Write(NewValue);
        }
    }
}
