using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Account
{
    [TLObject(127302966)]
    public class TLRequestGetWallPapers : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return 127302966;
            }
        }

        public long Hash { get; set; }
        public Account.TLAbsWallPapers Response { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Hash = br.ReadInt64();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Hash);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (Account.TLAbsWallPapers)ObjectUtils.DeserializeObject(br);
        }
    }
}
