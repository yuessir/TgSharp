using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(-1227287081)]
    public class TLInputBotInlineMessageID64 : TLAbsInputBotInlineMessageID
    {
        public override int Constructor
        {
            get
            {
                return -1227287081;
            }
        }

        public int DcId { get; set; }
        public long OwnerId { get; set; }
        public int Id { get; set; }
        public long AccessHash { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            DcId = br.ReadInt32();
            OwnerId = br.ReadInt64();
            Id = br.ReadInt32();
            AccessHash = br.ReadInt64();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(DcId);
            bw.Write(OwnerId);
            bw.Write(Id);
            bw.Write(AccessHash);
        }
    }
}
