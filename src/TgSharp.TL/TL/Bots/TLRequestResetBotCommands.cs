using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Bots
{
    [TLObject(1032708345)]
    public class TLRequestResetBotCommands : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return 1032708345;
            }
        }

        public TLAbsBotCommandScope Scope { get; set; }
        public string LangCode { get; set; }
        public bool Response { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Scope = (TLAbsBotCommandScope)ObjectUtils.DeserializeObject(br);
            LangCode = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Scope, bw);
            StringUtil.Serialize(LangCode, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}
