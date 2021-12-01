using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Payments
{
    [TLObject(818134173)]
    public class TLRequestSendPaymentForm : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return 818134173;
            }
        }

        public int Flags { get; set; }
        public long FormId { get; set; }
        public TLAbsInputPeer Peer { get; set; }
        public int MsgId { get; set; }
        public string RequestedInfoId { get; set; }
        public string ShippingOptionId { get; set; }
        public TLAbsInputPaymentCredentials Credentials { get; set; }
        public long? TipAmount { get; set; }
        public Payments.TLAbsPaymentResult Response { get; set; }

        public void ComputeFlags()
        {
            Flags = 0;
Flags = RequestedInfoId != null ? (Flags | 1) : (Flags & ~1);
Flags = ShippingOptionId != null ? (Flags | 2) : (Flags & ~2);
Flags = TipAmount != null ? (Flags | 4) : (Flags & ~4);

        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            FormId = br.ReadInt64();
            Peer = (TLAbsInputPeer)ObjectUtils.DeserializeObject(br);
            MsgId = br.ReadInt32();
            if ((Flags & 1) != 0)
                RequestedInfoId = StringUtil.Deserialize(br);
            else
                RequestedInfoId = null;

            if ((Flags & 2) != 0)
                ShippingOptionId = StringUtil.Deserialize(br);
            else
                ShippingOptionId = null;

            Credentials = (TLAbsInputPaymentCredentials)ObjectUtils.DeserializeObject(br);
            if ((Flags & 4) != 0)
                TipAmount = br.ReadInt64();
            else
                TipAmount = null;

        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            bw.Write(FormId);
            ObjectUtils.SerializeObject(Peer, bw);
            bw.Write(MsgId);
            if ((Flags & 1) != 0)
                StringUtil.Serialize(RequestedInfoId, bw);
            if ((Flags & 2) != 0)
                StringUtil.Serialize(ShippingOptionId, bw);
            ObjectUtils.SerializeObject(Credentials, bw);
            if ((Flags & 4) != 0)
                bw.Write(TipAmount.Value);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (Payments.TLAbsPaymentResult)ObjectUtils.DeserializeObject(br);
        }
    }
}
