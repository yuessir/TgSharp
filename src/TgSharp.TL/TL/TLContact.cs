using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(341499403)]
    public class TLContact : TLObject
    {
        public override int Constructor
        {
            get
            {
                return 341499403;
            }
        }

        public long UserId { get; set; }
        public bool Mutual { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            UserId = br.ReadInt64();
            Mutual = BoolUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(UserId);
            BoolUtil.Serialize(Mutual, bw);
        }
    }
}
