using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Messages
{
    [TLObject(-183077365)]
    public class TLRequestGetScheduledHistory : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return -183077365;
            }
        }

        public TLAbsInputPeer Peer { get; set; }
        public long Hash { get; set; }
        public Messages.TLAbsMessages Response { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Peer = (TLAbsInputPeer)ObjectUtils.DeserializeObject(br);
            Hash = br.ReadInt64();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Peer, bw);
            bw.Write(Hash);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (Messages.TLAbsMessages)ObjectUtils.DeserializeObject(br);
        }
    }
}
