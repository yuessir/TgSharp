using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Messages
{
    [TLObject(410107472)]
    public class TLExportedChatInvite : TLAbsExportedChatInvite
    {
        public override int Constructor
        {
            get
            {
                return 410107472;
            }
        }

        public TLAbsExportedChatInvite Invite { get; set; }
        public TLVector<TLAbsUser> Users { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Invite = (TLAbsExportedChatInvite)ObjectUtils.DeserializeObject(br);
            Users = (TLVector<TLAbsUser>)ObjectUtils.DeserializeVector<TLAbsUser>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Invite, bw);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}
