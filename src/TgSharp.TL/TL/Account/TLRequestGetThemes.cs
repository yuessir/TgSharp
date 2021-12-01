using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Account
{
    [TLObject(1913054296)]
    public class TLRequestGetThemes : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return 1913054296;
            }
        }

        public string Format { get; set; }
        public long Hash { get; set; }
        public Account.TLAbsThemes Response { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Format = StringUtil.Deserialize(br);
            Hash = br.ReadInt64();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Format, bw);
            bw.Write(Hash);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (Account.TLAbsThemes)ObjectUtils.DeserializeObject(br);
        }
    }
}
