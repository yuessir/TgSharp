using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(-1652231205)]
    public class TLInputStickerSetThumb : TLAbsInputFileLocation
    {
        public override int Constructor
        {
            get
            {
                return -1652231205;
            }
        }

        public TLAbsInputStickerSet Stickerset { get; set; }
        public int ThumbVersion { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Stickerset = (TLAbsInputStickerSet)ObjectUtils.DeserializeObject(br);
            ThumbVersion = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Stickerset, bw);
            bw.Write(ThumbVersion);
        }
    }
}
