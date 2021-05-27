using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(-2122045747)]
    public class TLPeerSettings : TLObject
    {
        public override int Constructor
        {
            get
            {
                return -2122045747;
            }
        }

        public int Flags { get; set; }
        public bool ReportSpam { get; set; }
        public bool AddContact { get; set; }
        public bool BlockContact { get; set; }
        public bool ShareContact { get; set; }
        public bool NeedContactsException { get; set; }
        public bool ReportGeo { get; set; }

        public void ComputeFlags()
        {
            Flags = 0;
            Flags = ReportSpam ? (Flags | 1) : (Flags & ~1);
            Flags = AddContact ? (Flags | 2) : (Flags & ~2);
            Flags = BlockContact ? (Flags | 4) : (Flags & ~4);
            Flags = ShareContact ? (Flags | 8) : (Flags & ~8);
            Flags = NeedContactsException ? (Flags | 16) : (Flags & ~16);
            Flags = ReportGeo ? (Flags | 32) : (Flags & ~32);

        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            ReportSpam = (Flags & 1) != 0;
            AddContact = (Flags & 2) != 0;
            BlockContact = (Flags & 4) != 0;
            ShareContact = (Flags & 8) != 0;
            NeedContactsException = (Flags & 16) != 0;
            ReportGeo = (Flags & 32) != 0;
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
        }
    }
}
