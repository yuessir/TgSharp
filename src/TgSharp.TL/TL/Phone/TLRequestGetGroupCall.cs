using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Phone
{
    [TLObject(68699611)]
    public class TLRequestGetGroupCall : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return 68699611;
            }
        }

        public TLInputGroupCall Call { get; set; }
        public int Limit { get; set; }
        public Phone.TLGroupCall Response { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Call = (TLInputGroupCall)ObjectUtils.DeserializeObject(br);
            Limit = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Call, bw);
            bw.Write(Limit);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (Phone.TLGroupCall)ObjectUtils.DeserializeObject(br);
        }
    }
}
