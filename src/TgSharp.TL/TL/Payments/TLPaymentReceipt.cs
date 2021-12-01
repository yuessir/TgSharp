using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Payments
{
    [TLObject(1891958275)]
    public class TLPaymentReceipt : TLObject
    {
        public override int Constructor
        {
            get
            {
                return 1891958275;
            }
        }

        public int Flags { get; set; }
        public int Date { get; set; }
        public long BotId { get; set; }
        public long ProviderId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TLAbsWebDocument Photo { get; set; }
        public TLInvoice Invoice { get; set; }
        public TLPaymentRequestedInfo Info { get; set; }
        public TLShippingOption Shipping { get; set; }
        public long? TipAmount { get; set; }
        public string Currency { get; set; }
        public long TotalAmount { get; set; }
        public string CredentialsTitle { get; set; }
        public TLVector<TLAbsUser> Users { get; set; }

        public void ComputeFlags()
        {
            Flags = 0;
Flags = Photo != null ? (Flags | 4) : (Flags & ~4);
Flags = Info != null ? (Flags | 1) : (Flags & ~1);
Flags = Shipping != null ? (Flags | 2) : (Flags & ~2);
Flags = TipAmount != null ? (Flags | 8) : (Flags & ~8);

        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Date = br.ReadInt32();
            BotId = br.ReadInt64();
            ProviderId = br.ReadInt64();
            Title = StringUtil.Deserialize(br);
            Description = StringUtil.Deserialize(br);
            if ((Flags & 4) != 0)
                Photo = (TLAbsWebDocument)ObjectUtils.DeserializeObject(br);
            else
                Photo = null;

            Invoice = (TLInvoice)ObjectUtils.DeserializeObject(br);
            if ((Flags & 1) != 0)
                Info = (TLPaymentRequestedInfo)ObjectUtils.DeserializeObject(br);
            else
                Info = null;

            if ((Flags & 2) != 0)
                Shipping = (TLShippingOption)ObjectUtils.DeserializeObject(br);
            else
                Shipping = null;

            if ((Flags & 8) != 0)
                TipAmount = br.ReadInt64();
            else
                TipAmount = null;

            Currency = StringUtil.Deserialize(br);
            TotalAmount = br.ReadInt64();
            CredentialsTitle = StringUtil.Deserialize(br);
            Users = (TLVector<TLAbsUser>)ObjectUtils.DeserializeVector<TLAbsUser>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            bw.Write(Date);
            bw.Write(BotId);
            bw.Write(ProviderId);
            StringUtil.Serialize(Title, bw);
            StringUtil.Serialize(Description, bw);
            if ((Flags & 4) != 0)
                ObjectUtils.SerializeObject(Photo, bw);
            ObjectUtils.SerializeObject(Invoice, bw);
            if ((Flags & 1) != 0)
                ObjectUtils.SerializeObject(Info, bw);
            if ((Flags & 2) != 0)
                ObjectUtils.SerializeObject(Shipping, bw);
            if ((Flags & 8) != 0)
                bw.Write(TipAmount.Value);
            StringUtil.Serialize(Currency, bw);
            bw.Write(TotalAmount);
            StringUtil.Serialize(CredentialsTitle, bw);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}
