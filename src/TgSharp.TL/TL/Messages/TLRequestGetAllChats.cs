using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Messages
{
    [TLObject(-2023787330)]
    public class TLRequestGetAllChats : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return -2023787330;
            }
        }

        public TLVector<long> ExceptIds { get; set; }
        public Messages.TLAbsChats Response { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ExceptIds = (TLVector<long>)ObjectUtils.DeserializeVector<long>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(ExceptIds, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (Messages.TLAbsChats)ObjectUtils.DeserializeObject(br);
        }
    }
}
