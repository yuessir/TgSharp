using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Phone
{
    [TLObject(-425040769)]
    public class TLRequestExportGroupCallInvite : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return -425040769;
            }
        }

        public int Flags { get; set; }
        public bool CanSelfUnmute { get; set; }
        public TLInputGroupCall Call { get; set; }
        public Phone.TLExportedGroupCallInvite Response { get; set; }

        public void ComputeFlags()
        {
            Flags = 0;
Flags = CanSelfUnmute ? (Flags | 1) : (Flags & ~1);

        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            CanSelfUnmute = (Flags & 1) != 0;
            Call = (TLInputGroupCall)ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            ObjectUtils.SerializeObject(Call, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (Phone.TLExportedGroupCallInvite)ObjectUtils.DeserializeObject(br);
        }
    }
}
