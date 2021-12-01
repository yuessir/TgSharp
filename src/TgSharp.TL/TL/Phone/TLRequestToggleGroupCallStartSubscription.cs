using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Phone
{
    [TLObject(563885286)]
    public class TLRequestToggleGroupCallStartSubscription : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return 563885286;
            }
        }

        public TLInputGroupCall Call { get; set; }
        public bool Subscribed { get; set; }
        public TLAbsUpdates Response { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Call = (TLInputGroupCall)ObjectUtils.DeserializeObject(br);
            Subscribed = BoolUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Call, bw);
            BoolUtil.Serialize(Subscribed, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TLAbsUpdates)ObjectUtils.DeserializeObject(br);
        }
    }
}
