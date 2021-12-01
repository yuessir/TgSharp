using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(-365344535)]
    public class TLMessageActionChannelMigrateFrom : TLAbsMessageAction
    {
        public override int Constructor
        {
            get
            {
                return -365344535;
            }
        }

        public string Title { get; set; }
        public long ChatId { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Title = StringUtil.Deserialize(br);
            ChatId = br.ReadInt64();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Title, bw);
            bw.Write(ChatId);
        }
    }
}
