using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(-528465642)]
    public class TLWallPaperNoFile : TLAbsWallPaper
    {
        public override int Constructor
        {
            get
            {
                return -528465642;
            }
        }

        public long Id { get; set; }
        public int Flags { get; set; }
        public bool Default { get; set; }
        public bool Dark { get; set; }
        public TLWallPaperSettings Settings { get; set; }

        public void ComputeFlags()
        {
            Flags = 0;
Flags = Default ? (Flags | 2) : (Flags & ~2);
Flags = Dark ? (Flags | 16) : (Flags & ~16);
Flags = Settings != null ? (Flags | 4) : (Flags & ~4);

        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = br.ReadInt64();
            Flags = br.ReadInt32();
            Default = (Flags & 2) != 0;
            Dark = (Flags & 16) != 0;
            if ((Flags & 4) != 0)
                Settings = (TLWallPaperSettings)ObjectUtils.DeserializeObject(br);
            else
                Settings = null;

        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            bw.Write(Id);
            if ((Flags & 4) != 0)
                ObjectUtils.SerializeObject(Settings, bw);
        }
    }
}
