using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Messages
{
    [TLObject(-2091549254)]
    public class TLRequestEditInlineBotMessage : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return -2091549254;
            }
        }

        public int Flags { get; set; }
        public bool NoWebpage { get; set; }
        public TLAbsInputBotInlineMessageID Id { get; set; }
        public string Message { get; set; }
        public TLAbsInputMedia Media { get; set; }
        public TLAbsReplyMarkup ReplyMarkup { get; set; }
        public TLVector<TLAbsMessageEntity> Entities { get; set; }
        public bool Response { get; set; }

        public void ComputeFlags()
        {
            Flags = 0;
Flags = NoWebpage ? (Flags | 2) : (Flags & ~2);
Flags = Message != null ? (Flags | 2048) : (Flags & ~2048);
Flags = Media != null ? (Flags | 16384) : (Flags & ~16384);
Flags = ReplyMarkup != null ? (Flags | 4) : (Flags & ~4);
Flags = Entities != null ? (Flags | 8) : (Flags & ~8);

        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            NoWebpage = (Flags & 2) != 0;
            Id = (TLAbsInputBotInlineMessageID)ObjectUtils.DeserializeObject(br);
            if ((Flags & 2048) != 0)
                Message = StringUtil.Deserialize(br);
            else
                Message = null;

            if ((Flags & 16384) != 0)
                Media = (TLAbsInputMedia)ObjectUtils.DeserializeObject(br);
            else
                Media = null;

            if ((Flags & 4) != 0)
                ReplyMarkup = (TLAbsReplyMarkup)ObjectUtils.DeserializeObject(br);
            else
                ReplyMarkup = null;

            if ((Flags & 8) != 0)
                Entities = (TLVector<TLAbsMessageEntity>)ObjectUtils.DeserializeVector<TLAbsMessageEntity>(br);
            else
                Entities = null;

        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            ObjectUtils.SerializeObject(Id, bw);
            if ((Flags & 2048) != 0)
                StringUtil.Serialize(Message, bw);
            if ((Flags & 16384) != 0)
                ObjectUtils.SerializeObject(Media, bw);
            if ((Flags & 4) != 0)
                ObjectUtils.SerializeObject(ReplyMarkup, bw);
            if ((Flags & 8) != 0)
                ObjectUtils.SerializeObject(Entities, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}
