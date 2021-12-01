using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL.Contacts
{
    [TLObject(1574346258)]
    public class TLRequestGetContacts : TLMethod
    {
        public override int Constructor
        {
            get
            {
                return 1574346258;
            }
        }

        public long Hash { get; set; }
        public Contacts.TLAbsContacts Response { get; set; }

        public void ComputeFlags()
        {
            // do nothing
        }

        public override void DeserializeBody(BinaryReader br)
        {
            Hash = br.ReadInt64();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            bw.Write(Hash);
        }

        public override void DeserializeResponse(BinaryReader br)
        {
            Response = (Contacts.TLAbsContacts)ObjectUtils.DeserializeObject(br);
        }
    }
}
