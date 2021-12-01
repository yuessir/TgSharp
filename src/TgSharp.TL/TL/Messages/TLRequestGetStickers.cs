using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Messages
{
    [TLObject(-710552671)]
    public class TLRequestGetStickers : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return -710552671;
            }
        }

        public string Emoticon { get; set; }
        public long Hash { get; set; }
        public Messages.TLAbsStickers Response { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Emoticon = StringUtil.Deserialize(br);
            Hash = br.ReadInt64();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Emoticon, bw);
            bw.Write(Hash);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (Messages.TLAbsStickers)ObjectUtils.DeserializeObject(br);
        }
    }
}
