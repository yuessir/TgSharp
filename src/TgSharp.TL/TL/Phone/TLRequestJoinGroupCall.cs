using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Phone
{
    [TLObject(-1322057861)]
    public class TLRequestJoinGroupCall : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return -1322057861;
            }
        }

        public int Flags { get; set; }
        public bool Muted { get; set; }
        public bool VideoStopped { get; set; }
        public TLInputGroupCall Call { get; set; }
        public TLAbsInputPeer JoinAs { get; set; }
        public string InviteHash { get; set; }
        public TLDataJSON Params { get; set; }
        public TLAbsUpdates Response { get; set; }

        public void ComputeFlags()
        {
            Flags = 0;
Flags = Muted ? (Flags | 1) : (Flags & ~1);
Flags = VideoStopped ? (Flags | 4) : (Flags & ~4);
Flags = InviteHash != null ? (Flags | 2) : (Flags & ~2);

        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Muted = (Flags & 1) != 0;
            VideoStopped = (Flags & 4) != 0;
            Call = (TLInputGroupCall)ObjectUtils.DeserializeObject(br);
            JoinAs = (TLAbsInputPeer)ObjectUtils.DeserializeObject(br);
            if ((Flags & 2) != 0)
                InviteHash = StringUtil.Deserialize(br);
            else
                InviteHash = null;

            Params = (TLDataJSON)ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            ObjectUtils.SerializeObject(Call, bw);
            ObjectUtils.SerializeObject(JoinAs, bw);
            if ((Flags & 2) != 0)
                StringUtil.Serialize(InviteHash, bw);
            ObjectUtils.SerializeObject(Params, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TLAbsUpdates)ObjectUtils.DeserializeObject(br);
        }
    }
}
