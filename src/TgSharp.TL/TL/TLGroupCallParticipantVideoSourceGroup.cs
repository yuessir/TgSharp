using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(-592373577)]
    public class TLGroupCallParticipantVideoSourceGroup : TLObject
    {
        public override int Constructor
        {
            get
            {
                return -592373577;
            }
        }

        public string Semantics { get; set; }
        public TLVector<int> Sources { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Semantics = StringUtil.Deserialize(br);
            Sources = (TLVector<int>)ObjectUtils.DeserializeVector<int>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            StringUtil.Serialize(Semantics, bw);
            ObjectUtils.SerializeObject(Sources, bw);
        }
    }
}
