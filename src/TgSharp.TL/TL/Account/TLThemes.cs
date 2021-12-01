using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Account
{
    [TLObject(-1707242387)]
    public class TLThemes : TLAbsThemes
    {
        public override int Constructor
        {
            get
            {
                return -1707242387;
            }
        }

        public long Hash { get; set; }
        public TLVector<TLTheme> Themes { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Hash = br.ReadInt64();
            Themes = (TLVector<TLTheme>)ObjectUtils.DeserializeVector<TLTheme>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Hash);
            ObjectUtils.SerializeObject(Themes, bw);
        }
    }
}
