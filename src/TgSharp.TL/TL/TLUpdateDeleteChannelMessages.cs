using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(-1020437742)]
    public class TLUpdateDeleteChannelMessages : TLAbsUpdate
    {
        public override int Constructor
        {
            get
            {
                return -1020437742;
            }
        }

        public long ChannelId { get; set; }
        public TLVector<int> Messages { get; set; }
        public int Pts { get; set; }
        public int PtsCount { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ChannelId = br.ReadInt64();
            Messages = (TLVector<int>)ObjectUtils.DeserializeVector<int>(br);
            Pts = br.ReadInt32();
            PtsCount = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ChannelId);
            ObjectUtils.SerializeObject(Messages, bw);
            bw.Write(Pts);
            bw.Write(PtsCount);
        }
    }
}
