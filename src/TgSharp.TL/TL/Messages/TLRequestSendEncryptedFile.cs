using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Messages
{
    [TLObject(1431914525)]
    public class TLRequestSendEncryptedFile : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return 1431914525;
            }
        }

        public int Flags { get; set; }
        public bool Silent { get; set; }
        public TLInputEncryptedChat Peer { get; set; }
        public long RandomId { get; set; }
        public byte[] Data { get; set; }
        public TLAbsInputEncryptedFile File { get; set; }
        public Messages.TLAbsSentEncryptedMessage Response { get; set; }

        public void ComputeFlags()
        {
            Flags = 0;
Flags = Silent ? (Flags | 1) : (Flags & ~1);

        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Silent = (Flags & 1) != 0;
            Peer = (TLInputEncryptedChat)ObjectUtils.DeserializeObject(br);
            RandomId = br.ReadInt64();
            Data = BytesUtil.Deserialize(br);
            File = (TLAbsInputEncryptedFile)ObjectUtils.DeserializeObject(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            ObjectUtils.SerializeObject(Peer, bw);
            bw.Write(RandomId);
            BytesUtil.Serialize(Data, bw);
            ObjectUtils.SerializeObject(File, bw);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (Messages.TLAbsSentEncryptedMessage)ObjectUtils.DeserializeObject(br);
        }
    }
}
