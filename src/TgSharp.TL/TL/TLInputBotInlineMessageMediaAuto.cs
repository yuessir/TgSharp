using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(864077702)]
    public class TLInputBotInlineMessageMediaAuto : TLAbsInputBotInlineMessage
    {
        public override int Constructor
        {
            get
            {
                return 864077702;
            }
        }

        public int Flags { get; set; }
        public string Message { get; set; }
        public TLVector<TLAbsMessageEntity> Entities { get; set; }
        public TLAbsReplyMarkup ReplyMarkup { get; set; }

        public void ComputeFlags()
        {
            Flags = 0;
Flags = Entities != null ? (Flags | 2) : (Flags & ~2);
Flags = ReplyMarkup != null ? (Flags | 4) : (Flags & ~4);

        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Message = StringUtil.Deserialize(br);
            if ((Flags & 2) != 0)
                Entities = (TLVector<TLAbsMessageEntity>)ObjectUtils.DeserializeVector<TLAbsMessageEntity>(br);
            else
                Entities = null;

            if ((Flags & 4) != 0)
                ReplyMarkup = (TLAbsReplyMarkup)ObjectUtils.DeserializeObject(br);
            else
                ReplyMarkup = null;

        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            StringUtil.Serialize(Message, bw);
            if ((Flags & 2) != 0)
                ObjectUtils.SerializeObject(Entities, bw);
            if ((Flags & 4) != 0)
                ObjectUtils.SerializeObject(ReplyMarkup, bw);
        }
    }
}
