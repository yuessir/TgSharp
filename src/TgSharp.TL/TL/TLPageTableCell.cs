using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(878078826)]
    public class TLPageTableCell : TLObject
    {
        public override int Constructor
        {
            get
            {
                return 878078826;
            }
        }

        public int Flags { get; set; }
        public bool Header { get; set; }
        public bool AlignCenter { get; set; }
        public bool AlignRight { get; set; }
        public bool ValignMiddle { get; set; }
        public bool ValignBottom { get; set; }
        public TLAbsRichText Text { get; set; }
        public int? Colspan { get; set; }
        public int? Rowspan { get; set; }

        public void ComputeFlags()
        {
            Flags = 0;
Flags = Header ? (Flags | 1) : (Flags & ~1);
Flags = AlignCenter ? (Flags | 8) : (Flags & ~8);
Flags = AlignRight ? (Flags | 16) : (Flags & ~16);
Flags = ValignMiddle ? (Flags | 32) : (Flags & ~32);
Flags = ValignBottom ? (Flags | 64) : (Flags & ~64);
Flags = Text != null ? (Flags | 128) : (Flags & ~128);
Flags = Colspan != null ? (Flags | 2) : (Flags & ~2);
Flags = Rowspan != null ? (Flags | 4) : (Flags & ~4);

        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Header = (Flags & 1) != 0;
            AlignCenter = (Flags & 8) != 0;
            AlignRight = (Flags & 16) != 0;
            ValignMiddle = (Flags & 32) != 0;
            ValignBottom = (Flags & 64) != 0;
            if ((Flags & 128) != 0)
                Text = (TLAbsRichText)ObjectUtils.DeserializeObject(br);
            else
                Text = null;

            if ((Flags & 2) != 0)
                Colspan = br.ReadInt32();
            else
                Colspan = null;

            if ((Flags & 4) != 0)
                Rowspan = br.ReadInt32();
            else
                Rowspan = null;

        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            if ((Flags & 128) != 0)
                ObjectUtils.SerializeObject(Text, bw);
            if ((Flags & 2) != 0)
                bw.Write(Colspan.Value);
            if ((Flags & 4) != 0)
                bw.Write(Rowspan.Value);
        }
    }
}
