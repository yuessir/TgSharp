using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Auth
{
    [TLObject(923364464)]
    public class TLRequestRecoverPassword : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return 923364464;
            }
        }

        public int Flags { get; set; }
        public string Code { get; set; }
        public Account.TLPasswordInputSettings NewSettings { get; set; }
        public Auth.TLAbsAuthorization Response { get; set; }

        public void ComputeFlags()
        {
            Flags = 0;
Flags = NewSettings != null ? (Flags | 1) : (Flags & ~1);

        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Code = StringUtil.Deserialize(br);
            if ((Flags & 1) != 0)
                NewSettings = (Account.TLPasswordInputSettings)ObjectUtils.DeserializeObject(br);
            else
                NewSettings = null;

        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            StringUtil.Serialize(Code, bw);
            if ((Flags & 1) != 0)
                ObjectUtils.SerializeObject(NewSettings, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (Auth.TLAbsAuthorization)ObjectUtils.DeserializeObject(br);
        }
    }
}
