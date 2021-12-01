using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Phone
{
    [TLObject(-277077702)]
    public class TLRequestGetGroupCallJoinAs : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return -277077702;
            }
        }

        public TLAbsInputPeer Peer { get; set; }
        public Phone.TLJoinAsPeers Response { get; set; }

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
            Response = (Phone.TLJoinAsPeers)ObjectUtils.DeserializeObject(br);
        }
    }
}
