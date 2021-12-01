using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Phone
{
    [TLObject(-1524155713)]
    public class TLRequestEditGroupCallParticipant : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return -1524155713;
            }
        }

        public int Flags { get; set; }
        public TLInputGroupCall Call { get; set; }
        public TLAbsInputPeer Participant { get; set; }
        public bool? Muted { get; set; }
        public int? Volume { get; set; }
        public bool? RaiseHand { get; set; }
        public bool? VideoStopped { get; set; }
        public bool? VideoPaused { get; set; }
        public bool? PresentationPaused { get; set; }
        public TLAbsUpdates Response { get; set; }

        public void ComputeFlags()
        {
            Flags = 0;
Flags = Muted != null ? (Flags | 1) : (Flags & ~1);
Flags = Volume != null ? (Flags | 2) : (Flags & ~2);
Flags = RaiseHand != null ? (Flags | 4) : (Flags & ~4);
Flags = VideoStopped != null ? (Flags | 8) : (Flags & ~8);
Flags = VideoPaused != null ? (Flags | 16) : (Flags & ~16);
Flags = PresentationPaused != null ? (Flags | 32) : (Flags & ~32);

        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Call = (TLInputGroupCall)ObjectUtils.DeserializeObject(br);
            Participant = (TLAbsInputPeer)ObjectUtils.DeserializeObject(br);
            if ((Flags & 1) != 0)
                Muted = BoolUtil.Deserialize(br);
            else
                Muted = null;

            if ((Flags & 2) != 0)
                Volume = br.ReadInt32();
            else
                Volume = null;

            if ((Flags & 4) != 0)
                RaiseHand = BoolUtil.Deserialize(br);
            else
                RaiseHand = null;

            if ((Flags & 8) != 0)
                VideoStopped = BoolUtil.Deserialize(br);
            else
                VideoStopped = null;

            if ((Flags & 16) != 0)
                VideoPaused = BoolUtil.Deserialize(br);
            else
                VideoPaused = null;

            if ((Flags & 32) != 0)
                PresentationPaused = BoolUtil.Deserialize(br);
            else
                PresentationPaused = null;

        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            ObjectUtils.SerializeObject(Call, bw);
            ObjectUtils.SerializeObject(Participant, bw);
            if ((Flags & 1) != 0)
                BoolUtil.Serialize(Muted.Value, bw);
            if ((Flags & 2) != 0)
                bw.Write(Volume.Value);
            if ((Flags & 4) != 0)
                BoolUtil.Serialize(RaiseHand.Value, bw);
            if ((Flags & 8) != 0)
                BoolUtil.Serialize(VideoStopped.Value, bw);
            if ((Flags & 16) != 0)
                BoolUtil.Serialize(VideoPaused.Value, bw);
            if ((Flags & 32) != 0)
                BoolUtil.Serialize(PresentationPaused.Value, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TLAbsUpdates)ObjectUtils.DeserializeObject(br);
        }
    }
}
