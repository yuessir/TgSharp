using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Channels
{
    [TLObject(-1699676497)]
    public class TLChannelParticipants : TLAbsChannelParticipants
    {
        public override int Constructor
        {
            get
            {
                return -1699676497;
            }
        }

        public int Count { get; set; }
        public TLVector<TLAbsChannelParticipant> Participants { get; set; }
        public TLVector<TLAbsChat> Chats { get; set; }
        public TLVector<TLAbsUser> Users { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Count = br.ReadInt32();
            Participants = (TLVector<TLAbsChannelParticipant>)ObjectUtils.DeserializeVector<TLAbsChannelParticipant>(br);
            Chats = (TLVector<TLAbsChat>)ObjectUtils.DeserializeVector<TLAbsChat>(br);
            Users = (TLVector<TLAbsUser>)ObjectUtils.DeserializeVector<TLAbsUser>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Count);
            ObjectUtils.SerializeObject(Participants, bw);
            ObjectUtils.SerializeObject(Chats, bw);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}
