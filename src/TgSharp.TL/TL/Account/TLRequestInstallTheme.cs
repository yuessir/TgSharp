using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Account
{
    [TLObject(2061776695)]
    public class TLRequestInstallTheme : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return 2061776695;
            }
        }

        public int Flags { get; set; }
        public bool Dark { get; set; }
        public string Format { get; set; }
        public TLAbsInputTheme Theme { get; set; }
        public bool Response { get; set; }

        public void ComputeFlags()
        {
            Flags = 0;
Flags = Dark ? (Flags | 1) : (Flags & ~1);
Flags = Format != null ? (Flags | 2) : (Flags & ~2);
Flags = Theme != null ? (Flags | 2) : (Flags & ~2);

        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Dark = (Flags & 1) != 0;
            if ((Flags & 2) != 0)
                Format = StringUtil.Deserialize(br);
            else
                Format = null;

            if ((Flags & 2) != 0)
                Theme = (TLAbsInputTheme)ObjectUtils.DeserializeObject(br);
            else
                Theme = null;

        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            if ((Flags & 2) != 0)
                StringUtil.Serialize(Format, bw);
            if ((Flags & 2) != 0)
                ObjectUtils.SerializeObject(Theme, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}
