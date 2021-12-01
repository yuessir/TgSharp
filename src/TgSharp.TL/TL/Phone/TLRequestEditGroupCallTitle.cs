using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Phone
{
    [TLObject(480685066)]
    public class TLRequestEditGroupCallTitle : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return 480685066;
            }
        }

        public TLInputGroupCall Call { get; set; }
        public string Title { get; set; }
        public TLAbsUpdates Response { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Call = (TLInputGroupCall)ObjectUtils.DeserializeObject(br);
            Title = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Call, bw);
            StringUtil.Serialize(Title, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TLAbsUpdates)ObjectUtils.DeserializeObject(br);
        }
    }
}
