using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(1421174295)]
    public class TLWebPageAttributeTheme : TLObject
    {
        public override int Constructor
        {
            get
            {
                return 1421174295;
            }
        }

        public int Flags { get; set; }
        public TLVector<TLAbsDocument> Documents { get; set; }
        public TLThemeSettings Settings { get; set; }

        public void ComputeFlags()
        {
            Flags = 0;
Flags = Documents != null ? (Flags | 1) : (Flags & ~1);
Flags = Settings != null ? (Flags | 2) : (Flags & ~2);

        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            if ((Flags & 1) != 0)
                Documents = (TLVector<TLAbsDocument>)ObjectUtils.DeserializeVector<TLAbsDocument>(br);
            else
                Documents = null;

            if ((Flags & 2) != 0)
                Settings = (TLThemeSettings)ObjectUtils.DeserializeObject(br);
            else
                Settings = null;

        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            if ((Flags & 1) != 0)
                ObjectUtils.SerializeObject(Documents, bw);
            if ((Flags & 2) != 0)
                ObjectUtils.SerializeObject(Settings, bw);
        }
    }
}
