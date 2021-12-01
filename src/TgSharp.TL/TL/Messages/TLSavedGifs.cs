using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Messages
{
    [TLObject(-2069878259)]
    public class TLSavedGifs : TLAbsSavedGifs
    {
        public override int Constructor
        {
            get
            {
                return -2069878259;
            }
        }

        public long Hash { get; set; }
        public TLVector<TLAbsDocument> Gifs { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Hash = br.ReadInt64();
            Gifs = (TLVector<TLAbsDocument>)ObjectUtils.DeserializeVector<TLAbsDocument>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Hash);
            ObjectUtils.SerializeObject(Gifs, bw);
        }
    }
}
