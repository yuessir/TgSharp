using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Account
{
    [TLObject(-842824308)]
    public class TLWallPapers : TLAbsWallPapers
    {
        public override int Constructor
        {
            get
            {
                return -842824308;
            }
        }

        public long Hash { get; set; }
        public TLVector<TLAbsWallPaper> Wallpapers { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Hash = br.ReadInt64();
            Wallpapers = (TLVector<TLAbsWallPaper>)ObjectUtils.DeserializeVector<TLAbsWallPaper>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Hash);
            ObjectUtils.SerializeObject(Wallpapers, bw);
        }
    }
}
