using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(-318022605)]
    public class TLChatTheme : TLObject
    {
        public override int Constructor
        {
            get
            {
                return -318022605;
            }
        }

        public string Emoticon { get; set; }
        public TLTheme Theme { get; set; }
        public TLTheme DarkTheme { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Emoticon = StringUtil.Deserialize(br);
            Theme = (TLTheme)ObjectUtils.DeserializeObject(br);
            DarkTheme = (TLTheme)ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Emoticon, bw);
            ObjectUtils.SerializeObject(Theme, bw);
            ObjectUtils.SerializeObject(DarkTheme, bw);
        }
    }
}
