using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Bots
{
    [TLObject(85399130)]
    public class TLRequestSetBotCommands : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return 85399130;
            }
        }

        public TLAbsBotCommandScope Scope { get; set; }
        public string LangCode { get; set; }
        public TLVector<TLBotCommand> Commands { get; set; }
        public bool Response { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Scope = (TLAbsBotCommandScope)ObjectUtils.DeserializeObject(br);
            LangCode = StringUtil.Deserialize(br);
            Commands = (TLVector<TLBotCommand>)ObjectUtils.DeserializeVector<TLBotCommand>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Scope, bw);
            StringUtil.Serialize(LangCode, bw);
            ObjectUtils.SerializeObject(Commands, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}
