using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(383348795)]
    public class TLContactStatus : TLObject
    {
        public override int Constructor
        {
            get
            {
                return 383348795;
            }
        }

        public long UserId { get; set; }
        public TLAbsUserStatus Status { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            UserId = br.ReadInt64();
            Status = (TLAbsUserStatus)ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(UserId);
            ObjectUtils.SerializeObject(Status, bw);
        }
    }
}
