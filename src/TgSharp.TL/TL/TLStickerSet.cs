using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(-673242758)]
    public class TLStickerSet : TLObject
    {
        public override int Constructor
        {
            get
            {
                return -673242758;
            }
        }

        public int Flags { get; set; }
        public bool Archived { get; set; }
        public bool Official { get; set; }
        public bool Masks { get; set; }
        public bool Animated { get; set; }
        public int? InstalledDate { get; set; }
        public long Id { get; set; }
        public long AccessHash { get; set; }
        public string Title { get; set; }
        public string ShortName { get; set; }
        public TLVector<TLAbsPhotoSize> Thumbs { get; set; }
        public int? ThumbDcId { get; set; }
        public int? ThumbVersion { get; set; }
        public int Count { get; set; }
        public int Hash { get; set; }

        public void ComputeFlags()
        {
            Flags = 0;
Flags = Archived ? (Flags | 2) : (Flags & ~2);
Flags = Official ? (Flags | 4) : (Flags & ~4);
Flags = Masks ? (Flags | 8) : (Flags & ~8);
Flags = Animated ? (Flags | 32) : (Flags & ~32);
Flags = InstalledDate != null ? (Flags | 1) : (Flags & ~1);
Flags = Thumbs != null ? (Flags | 16) : (Flags & ~16);
Flags = ThumbDcId != null ? (Flags | 16) : (Flags & ~16);
Flags = ThumbVersion != null ? (Flags | 16) : (Flags & ~16);

        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Archived = (Flags & 2) != 0;
            Official = (Flags & 4) != 0;
            Masks = (Flags & 8) != 0;
            Animated = (Flags & 32) != 0;
            if ((Flags & 1) != 0)
                InstalledDate = br.ReadInt32();
            else
                InstalledDate = null;

            Id = br.ReadInt64();
            AccessHash = br.ReadInt64();
            Title = StringUtil.Deserialize(br);
            ShortName = StringUtil.Deserialize(br);
            if ((Flags & 16) != 0)
                Thumbs = (TLVector<TLAbsPhotoSize>)ObjectUtils.DeserializeVector<TLAbsPhotoSize>(br);
            else
                Thumbs = null;

            if ((Flags & 16) != 0)
                ThumbDcId = br.ReadInt32();
            else
                ThumbDcId = null;

            if ((Flags & 16) != 0)
                ThumbVersion = br.ReadInt32();
            else
                ThumbVersion = null;

            Count = br.ReadInt32();
            Hash = br.ReadInt32();
        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            if ((Flags & 1) != 0)
                bw.Write(InstalledDate.Value);
            bw.Write(Id);
            bw.Write(AccessHash);
            StringUtil.Serialize(Title, bw);
            StringUtil.Serialize(ShortName, bw);
            if ((Flags & 16) != 0)
                ObjectUtils.SerializeObject(Thumbs, bw);
            if ((Flags & 16) != 0)
                bw.Write(ThumbDcId.Value);
            if ((Flags & 16) != 0)
                bw.Write(ThumbVersion.Value);
            bw.Write(Count);
            bw.Write(Hash);
        }
    }
}
