using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Account
{
    [TLObject(517647042)]
    public class TLRequestSetGlobalPrivacySettings : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return 517647042;
            }
        }

        public TLGlobalPrivacySettings Settings { get; set; }
        public TLGlobalPrivacySettings Response { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Settings = (TLGlobalPrivacySettings)ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Settings, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TLGlobalPrivacySettings)ObjectUtils.DeserializeObject(br);
        }
    }
}
