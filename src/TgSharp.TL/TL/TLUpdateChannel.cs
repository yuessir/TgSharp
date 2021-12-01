using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(1666927625)]
    public class TLUpdateChannel : TLAbsUpdate
    {
        public override int Constructor
        {
            get
            {
                return 1666927625;
            }
        }

        public long ChannelId { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ChannelId = br.ReadInt64();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ChannelId);
        }
    }
}
