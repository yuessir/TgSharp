using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Phone
{
    [TLObject(-1636664659)]
    public class TLGroupCall : TLObject
    {
        public override int Constructor
        {
            get
            {
                return -1636664659;
            }
        }

        public TLAbsGroupCall Call { get; set; }
        public TLVector<TLGroupCallParticipant> Participants { get; set; }
        public string ParticipantsNextOffset { get; set; }
        public TLVector<TLAbsChat> Chats { get; set; }
        public TLVector<TLAbsUser> Users { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Call = (TLAbsGroupCall)ObjectUtils.DeserializeObject(br);
            Participants = (TLVector<TLGroupCallParticipant>)ObjectUtils.DeserializeVector<TLGroupCallParticipant>(br);
            ParticipantsNextOffset = StringUtil.Deserialize(br);
            Chats = (TLVector<TLAbsChat>)ObjectUtils.DeserializeVector<TLAbsChat>(br);
            Users = (TLVector<TLAbsUser>)ObjectUtils.DeserializeVector<TLAbsUser>(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Call, bw);
            ObjectUtils.SerializeObject(Participants, bw);
            StringUtil.Serialize(ParticipantsNextOffset, bw);
            ObjectUtils.SerializeObject(Chats, bw);
            ObjectUtils.SerializeObject(Users, bw);
        }
    }
}
