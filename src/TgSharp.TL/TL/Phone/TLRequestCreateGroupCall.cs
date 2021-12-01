using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Phone
{
    [TLObject(1221445336)]
    public class TLRequestCreateGroupCall : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return 1221445336;
            }
        }

        public int Flags { get; set; }
        public TLAbsInputPeer Peer { get; set; }
        public int RandomId { get; set; }
        public string Title { get; set; }
        public int? ScheduleDate { get; set; }
        public TLAbsUpdates Response { get; set; }

        public void ComputeFlags()
        {
            Flags = 0;
Flags = Title != null ? (Flags | 1) : (Flags & ~1);
Flags = ScheduleDate != null ? (Flags | 2) : (Flags & ~2);

        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Peer = (TLAbsInputPeer)ObjectUtils.DeserializeObject(br);
            RandomId = br.ReadInt32();
            if ((Flags & 1) != 0)
                Title = StringUtil.Deserialize(br);
            else
                Title = null;

            if ((Flags & 2) != 0)
                ScheduleDate = br.ReadInt32();
            else
                ScheduleDate = null;

        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            ObjectUtils.SerializeObject(Peer, bw);
            bw.Write(RandomId);
            if ((Flags & 1) != 0)
                StringUtil.Serialize(Title, bw);
            if ((Flags & 2) != 0)
                bw.Write(ScheduleDate.Value);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TLAbsUpdates)ObjectUtils.DeserializeObject(br);
        }
    }
}
