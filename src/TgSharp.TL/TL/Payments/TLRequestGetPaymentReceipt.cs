using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Payments
{
    [TLObject(611897804)]
    public class TLRequestGetPaymentReceipt : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return 611897804;
            }
        }

        public TLAbsInputPeer Peer { get; set; }
        public int MsgId { get; set; }
        public Payments.TLPaymentReceipt Response { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Peer = (TLAbsInputPeer)ObjectUtils.DeserializeObject(br);
            MsgId = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Peer, bw);
            bw.Write(MsgId);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (Payments.TLPaymentReceipt)ObjectUtils.DeserializeObject(br);
        }
    }
}
