using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Messages
{
    [TLObject(-1489903017)]
    public class TLRequestToggleDialogPin : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return -1489903017;
            }
        }

        public int Flags { get; set; }
        public bool Pinned { get; set; }
        public TLAbsInputDialogPeer Peer { get; set; }
        public bool Response { get; set; }

        public void ComputeFlags()
        {
            Flags = 0;
Flags = Pinned ? (Flags | 1) : (Flags & ~1);

        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Pinned = (Flags & 1) != 0;
            Peer = (TLAbsInputDialogPeer)ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            ObjectUtils.SerializeObject(Peer, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = BoolUtil.Deserialize(br);
        }
    }
}
