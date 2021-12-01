using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(1722964307)]
    public class TLEncryptedChatWaiting : TLAbsEncryptedChat
    {
        public override int Constructor
        {
            get
            {
                return 1722964307;
            }
        }

        public int Id { get; set; }
        public long AccessHash { get; set; }
        public int Date { get; set; }
        public long AdminId { get; set; }
        public long ParticipantId { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Id = br.ReadInt32();
            AccessHash = br.ReadInt64();
            Date = br.ReadInt32();
            AdminId = br.ReadInt64();
            ParticipantId = br.ReadInt64();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Id);
            bw.Write(AccessHash);
            bw.Write(Date);
            bw.Write(AdminId);
            bw.Write(ParticipantId);
        }
    }
}
