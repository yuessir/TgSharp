using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;
using TgSharp.TL.Messages;

namespace TgSharp.TL
{
    [TLObject(-384910503)]
    public class TLChannelAdminLogEventActionExportedInviteEdit : TLAbsChannelAdminLogEventAction
    {
        public override int Constructor
        {
            get
            {
                return -384910503;
            }
        }

        public TLAbsExportedChatInvite PrevInvite { get; set; }
        public TLAbsExportedChatInvite NewInvite { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            PrevInvite = (TLAbsExportedChatInvite)ObjectUtils.DeserializeObject(br);
            NewInvite = (TLAbsExportedChatInvite)ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(PrevInvite, bw);
            ObjectUtils.SerializeObject(NewInvite, bw);
        }
    }
}
