using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Payments
{
    [TLObject(-1976353651)]
    public class TLRequestGetPaymentForm : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return -1976353651;
            }
        }

        public int Flags { get; set; }
        public TLAbsInputPeer Peer { get; set; }
        public int MsgId { get; set; }
        public TLDataJSON ThemeParams { get; set; }
        public Payments.TLPaymentForm Response { get; set; }

        public void ComputeFlags()
        {
            Flags = 0;
Flags = ThemeParams != null ? (Flags | 1) : (Flags & ~1);

        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Peer = (TLAbsInputPeer)ObjectUtils.DeserializeObject(br);
            MsgId = br.ReadInt32();
            if ((Flags & 1) != 0)
                ThemeParams = (TLDataJSON)ObjectUtils.DeserializeObject(br);
            else
                ThemeParams = null;

        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            ObjectUtils.SerializeObject(Peer, bw);
            bw.Write(MsgId);
            if ((Flags & 1) != 0)
                ObjectUtils.SerializeObject(ThemeParams, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (Payments.TLPaymentForm)ObjectUtils.DeserializeObject(br);
        }
    }
}
