using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Account
{
    [TLObject(-370148227)]
    public class TLResetPasswordRequestedWait : TLAbsResetPasswordResult
    {
        public override int Constructor
        {
            get
            {
                return -370148227;
            }
        }

        public int UntilDate { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            UntilDate = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(UntilDate);
        }
    }
}
