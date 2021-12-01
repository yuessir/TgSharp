using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Messages
{
    [TLObject(1140726259)]
    public class TLRequestCheckHistoryImport : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return 1140726259;
            }
        }

        public string ImportHead { get; set; }
        public Messages.TLHistoryImportParsed Response { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            ImportHead = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(ImportHead, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (Messages.TLHistoryImportParsed)ObjectUtils.DeserializeObject(br);
        }
    }
}
