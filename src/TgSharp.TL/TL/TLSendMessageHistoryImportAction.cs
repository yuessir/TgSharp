using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(-606432698)]
    public class TLSendMessageHistoryImportAction : TLAbsSendMessageAction
    {
        public override int Constructor
        {
            get
            {
                return -606432698;
            }
        }

        public int Progress { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Progress = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Progress);
        }
    }
}
