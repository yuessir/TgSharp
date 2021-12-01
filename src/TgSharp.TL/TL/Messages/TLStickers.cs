using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Messages
{
    [TLObject(816245886)]
    public class TLStickers : TLAbsStickers
    {
        public override int Constructor
        {
            get
            {
                return 816245886;
            }
        }

        public long Hash { get; set; }
        public TLVector<TLAbsDocument> Stickers { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Hash = br.ReadInt64();
            Stickers = (TLVector<TLAbsDocument>)ObjectUtils.DeserializeVector<TLAbsDocument>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Hash);
            ObjectUtils.SerializeObject(Stickers, bw);
        }
    }
}
