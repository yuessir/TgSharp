using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Messages
{
    [TLObject(1143203525)]
    public class TLRequestGetHistory : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return 1143203525;
            }
        }

        public TLAbsInputPeer Peer { get; set; }
        public int OffsetId { get; set; }
        public int OffsetDate { get; set; }
        public int AddOffset { get; set; }
        public int Limit { get; set; }
        public int MaxId { get; set; }
        public int MinId { get; set; }
        public long Hash { get; set; }
        public Messages.TLAbsMessages Response { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Peer = (TLAbsInputPeer)ObjectUtils.DeserializeObject(br);
            OffsetId = br.ReadInt32();
            OffsetDate = br.ReadInt32();
            AddOffset = br.ReadInt32();
            Limit = br.ReadInt32();
            MaxId = br.ReadInt32();
            MinId = br.ReadInt32();
            Hash = br.ReadInt64();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Peer, bw);
            bw.Write(OffsetId);
            bw.Write(OffsetDate);
            bw.Write(AddOffset);
            bw.Write(Limit);
            bw.Write(MaxId);
            bw.Write(MinId);
            bw.Write(Hash);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (Messages.TLAbsMessages)ObjectUtils.DeserializeObject(br);
        }
    }
}
