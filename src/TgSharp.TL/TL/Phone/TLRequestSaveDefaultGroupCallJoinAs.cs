using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Phone
{
    [TLObject(1465786252)]
    public class TLRequestSaveDefaultGroupCallJoinAs : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return 1465786252;
            }
        }

        public TLAbsInputPeer Peer { get; set; }
        public TLAbsInputPeer JoinAs { get; set; }
        public bool Response { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Peer = (TLAbsInputPeer)ObjectUtils.DeserializeObject(br);
            JoinAs = (TLAbsInputPeer)ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Peer, bw);
            ObjectUtils.SerializeObject(JoinAs, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}
