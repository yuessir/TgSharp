using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(-761649164)]
    public class TLUpdateChannelMessageForwards : TLAbsUpdate
    {
        public override int Constructor
        {
            get
            {
                return -761649164;
            }
        }

        public long ChannelId { get; set; }
        public int Id { get; set; }
        public int Forwards { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ChannelId = br.ReadInt64();
            Id = br.ReadInt32();
            Forwards = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(ChannelId);
            bw.Write(Id);
            bw.Write(Forwards);
        }
    }
}
