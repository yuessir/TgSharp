using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Photos
{
    [TLObject(1926525996)]
    public class TLRequestUpdateProfilePhoto : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return 1926525996;
            }
        }

        public TLAbsInputPhoto Id { get; set; }
        public Photos.TLPhoto Response { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = (TLAbsInputPhoto)ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Id, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (Photos.TLPhoto)ObjectUtils.DeserializeObject(br);
        }
    }
}
