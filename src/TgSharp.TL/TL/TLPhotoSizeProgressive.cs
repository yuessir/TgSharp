using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(-96535659)]
    public class TLPhotoSizeProgressive : TLAbsPhotoSize
    {
        public override int Constructor
        {
            get
            {
                return -96535659;
            }
        }

        public string Type { get; set; }
        public int W { get; set; }
        public int H { get; set; }
        public TLVector<int> Sizes { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Type = StringUtil.Deserialize(br);
            W = br.ReadInt32();
            H = br.ReadInt32();
            Sizes = (TLVector<int>)ObjectUtils.DeserializeVector<int>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Type, bw);
            bw.Write(W);
            bw.Write(H);
            ObjectUtils.SerializeObject(Sizes, bw);
        }
    }
}
