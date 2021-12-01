using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Phone
{
    [TLObject(-248985848)]
    public class TLRequestToggleGroupCallRecord : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return -248985848;
            }
        }

        public int Flags { get; set; }
        public bool Start { get; set; }
        public bool Video { get; set; }
        public TLInputGroupCall Call { get; set; }
        public string Title { get; set; }
        public bool? VideoPortrait { get; set; }
        public TLAbsUpdates Response { get; set; }

        public void ComputeFlags()
        {
            Flags = 0;
Flags = Start ? (Flags | 1) : (Flags & ~1);
Flags = Video ? (Flags | 4) : (Flags & ~4);
Flags = Title != null ? (Flags | 2) : (Flags & ~2);
Flags = VideoPortrait != null ? (Flags | 4) : (Flags & ~4);

        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Start = (Flags & 1) != 0;
            Video = (Flags & 4) != 0;
            Call = (TLInputGroupCall)ObjectUtils.DeserializeObject(br);
            if ((Flags & 2) != 0)
                Title = StringUtil.Deserialize(br);
            else
                Title = null;

            if ((Flags & 4) != 0)
                VideoPortrait = BoolUtil.Deserialize(br);
            else
                VideoPortrait = null;

        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            ObjectUtils.SerializeObject(Call, bw);
            if ((Flags & 2) != 0)
                StringUtil.Serialize(Title, bw);
            if ((Flags & 4) != 0)
                BoolUtil.Serialize(VideoPortrait.Value, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (TLAbsUpdates)ObjectUtils.DeserializeObject(br);
        }
    }
}
