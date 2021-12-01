using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(-440664550)]
    public class TLInputMediaPhotoExternal : TLAbsInputMedia
    {
        public override int Constructor
        {
            get
            {
                return -440664550;
            }
        }

        public int Flags { get; set; }
        public string Url { get; set; }
        public int? TtlSeconds { get; set; }

        public void ComputeFlags()
        {
            Flags = 0;
Flags = TtlSeconds != null ? (Flags | 1) : (Flags & ~1);

        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Url = StringUtil.Deserialize(br);
            if ((Flags & 1) != 0)
                TtlSeconds = br.ReadInt32();
            else
                TtlSeconds = null;

        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            StringUtil.Serialize(Url, bw);
            if ((Flags & 1) != 0)
                bw.Write(TtlSeconds.Value);
        }
    }
}
