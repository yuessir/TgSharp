using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Help
{
    [TLObject(505748629)]
    public class TLRequestHidePromoData : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return 505748629;
            }
        }

        public TLAbsInputPeer Peer { get; set; }
        public bool Response { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Peer = (TLAbsInputPeer)ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Peer, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}
