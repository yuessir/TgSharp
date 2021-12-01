using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(84703944)]
    public class TLChannelAdminLogEventActionChangeLinkedChat : TLAbsChannelAdminLogEventAction
    {
        public override int Constructor
        {
            get
            {
                return 84703944;
            }
        }

        public long PrevValue { get; set; }
        public long NewValue { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            PrevValue = br.ReadInt64();
            NewValue = br.ReadInt64();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(PrevValue);
            bw.Write(NewValue);
        }
    }
}
