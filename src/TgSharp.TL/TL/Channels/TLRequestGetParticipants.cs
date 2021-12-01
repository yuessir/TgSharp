using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Channels
{
    [TLObject(2010044880)]
    public class TLRequestGetParticipants : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return 2010044880;
            }
        }

        public TLAbsInputChannel Channel { get; set; }
        public TLAbsChannelParticipantsFilter Filter { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public long Hash { get; set; }
        public Channels.TLAbsChannelParticipants Response { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Channel = (TLAbsInputChannel)ObjectUtils.DeserializeObject(br);
            Filter = (TLAbsChannelParticipantsFilter)ObjectUtils.DeserializeObject(br);
            Offset = br.ReadInt32();
            Limit = br.ReadInt32();
            Hash = br.ReadInt64();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ObjectUtils.SerializeObject(Channel, bw);
            ObjectUtils.SerializeObject(Filter, bw);
            bw.Write(Offset);
            bw.Write(Limit);
            bw.Write(Hash);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (Channels.TLAbsChannelParticipants)ObjectUtils.DeserializeObject(br);
        }
    }
}
