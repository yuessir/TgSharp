using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Messages
{
    [TLObject(-1111085620)]
    public class TLExportedChatInvites : TLObject
    {
        public override int Constructor
        {
            get
            {
                return -1111085620;
            }
        }

        public int Count { get; set; }
        public TLVector<TLAbsExportedChatInvite> Invites { get; set; }
        public TLVector<TLAbsUser> Users { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Count = br.ReadInt32();
            Invites = (TLVector<TLAbsExportedChatInvite>)ObjectUtils.DeserializeVector<TLAbsExportedChatInvite>(br);
            Users = (TLVector<TLAbsUser>)ObjectUtils.DeserializeVector<TLAbsUser>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Count);
            ObjectUtils.SerializeObject(Invites, bw);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}
