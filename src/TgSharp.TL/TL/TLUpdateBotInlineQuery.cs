using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(1232025500)]
    public class TLUpdateBotInlineQuery : TLAbsUpdate
    {
        public override int Constructor
        {
            get
            {
                return 1232025500;
            }
        }

        public int Flags { get; set; }
        public long QueryId { get; set; }
        public long UserId { get; set; }
        public string Query { get; set; }
        public TLAbsGeoPoint Geo { get; set; }
        public TLAbsInlineQueryPeerType PeerType { get; set; }
        public string Offset { get; set; }

        public void ComputeFlags()
        {
            Flags = 0;
Flags = Geo != null ? (Flags | 1) : (Flags & ~1);
Flags = PeerType != null ? (Flags | 2) : (Flags & ~2);

        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            QueryId = br.ReadInt64();
            UserId = br.ReadInt64();
            Query = StringUtil.Deserialize(br);
            if ((Flags & 1) != 0)
                Geo = (TLAbsGeoPoint)ObjectUtils.DeserializeObject(br);
            else
                Geo = null;

            if ((Flags & 2) != 0)
                PeerType = (TLAbsInlineQueryPeerType)ObjectUtils.DeserializeObject(br);
            else
                PeerType = null;

            Offset = StringUtil.Deserialize(br);
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            bw.Write(QueryId);
            bw.Write(UserId);
            StringUtil.Serialize(Query, bw);
            if ((Flags & 1) != 0)
                ObjectUtils.SerializeObject(Geo, bw);
            if ((Flags & 2) != 0)
                ObjectUtils.SerializeObject(PeerType, bw);
            StringUtil.Serialize(Offset, bw);
        }
    }
}
