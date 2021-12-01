using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(1153291573)]
    public class TLUpdateChannelReadMessagesContents : TLAbsUpdate
    {
        public override int Constructor
        {
            get
            {
                return 1153291573;
            }
        }

        public long ChannelId { get; set; }
        public TLVector<int> Messages { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ChannelId = br.ReadInt64();
            Messages = (TLVector<int>)ObjectUtils.DeserializeVector<int>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ChannelId);
            ObjectUtils.SerializeObject(Messages, bw);
        }
    }
}
