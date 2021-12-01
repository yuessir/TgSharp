using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(-1966921727)]
    public class TLInputPaymentCredentialsGooglePay : TLAbsInputPaymentCredentials
    {
        public override int Constructor
        {
            get
            {
                return -1966921727;
            }
        }

        public TLDataJSON PaymentToken { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            PaymentToken = (TLDataJSON)ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(PaymentToken, bw);
        }
    }
}
