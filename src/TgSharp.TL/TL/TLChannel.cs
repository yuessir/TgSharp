using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TgSharp.TL;

namespace TgSharp.TL
{
    [TLObject(-2107528095)]
    public class TLChannel : TLAbsChat
    {
        public override int Constructor
        {
            get
            {
                return -2107528095;
            }
        }

        public int Flags { get; set; }
        public bool Creator { get; set; }
        public bool Left { get; set; }
        public bool Broadcast { get; set; }
        public bool Verified { get; set; }
        public bool Megagroup { get; set; }
        public bool Restricted { get; set; }
        public bool Signatures { get; set; }
        public bool Min { get; set; }
        public bool Scam { get; set; }
        public bool HasLink { get; set; }
        public bool HasGeo { get; set; }
        public bool SlowmodeEnabled { get; set; }
        public bool CallActive { get; set; }
        public bool CallNotEmpty { get; set; }
        public bool Fake { get; set; }
        public bool Gigagroup { get; set; }
        public bool Noforwards { get; set; }
        public long Id { get; set; }
        public long? AccessHash { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
        public TLAbsChatPhoto Photo { get; set; }
        public int Date { get; set; }
        public TLVector<TLRestrictionReason> RestrictionReason { get; set; }
        public TLChatAdminRights AdminRights { get; set; }
        public TLChatBannedRights BannedRights { get; set; }
        public TLChatBannedRights DefaultBannedRights { get; set; }
        public int? ParticipantsCount { get; set; }

        public void ComputeFlags()
        {
            Flags = 0;
Flags = Creator ? (Flags | 1) : (Flags & ~1);
Flags = Left ? (Flags | 4) : (Flags & ~4);
Flags = Broadcast ? (Flags | 32) : (Flags & ~32);
Flags = Verified ? (Flags | 128) : (Flags & ~128);
Flags = Megagroup ? (Flags | 256) : (Flags & ~256);
Flags = Restricted ? (Flags | 512) : (Flags & ~512);
Flags = Signatures ? (Flags | 2048) : (Flags & ~2048);
Flags = Min ? (Flags | 4096) : (Flags & ~4096);
Flags = Scam ? (Flags | 524288) : (Flags & ~524288);
Flags = HasLink ? (Flags | 1048576) : (Flags & ~1048576);
Flags = HasGeo ? (Flags | 2097152) : (Flags & ~2097152);
Flags = SlowmodeEnabled ? (Flags | 4194304) : (Flags & ~4194304);
Flags = CallActive ? (Flags | 8388608) : (Flags & ~8388608);
Flags = CallNotEmpty ? (Flags | 16777216) : (Flags & ~16777216);
Flags = Fake ? (Flags | 33554432) : (Flags & ~33554432);
Flags = Gigagroup ? (Flags | 67108864) : (Flags & ~67108864);
Flags = Noforwards ? (Flags | 134217728) : (Flags & ~134217728);
Flags = AccessHash != null ? (Flags | 8192) : (Flags & ~8192);
Flags = Username != null ? (Flags | 64) : (Flags & ~64);
Flags = RestrictionReason != null ? (Flags | 512) : (Flags & ~512);
Flags = AdminRights != null ? (Flags | 16384) : (Flags & ~16384);
Flags = BannedRights != null ? (Flags | 32768) : (Flags & ~32768);
Flags = DefaultBannedRights != null ? (Flags | 262144) : (Flags & ~262144);
Flags = ParticipantsCount != null ? (Flags | 131072) : (Flags & ~131072);

        }

        public override void DeserializeBody(BinaryReader br)
        {
            Flags = br.ReadInt32();
            Creator = (Flags & 1) != 0;
            Left = (Flags & 4) != 0;
            Broadcast = (Flags & 32) != 0;
            Verified = (Flags & 128) != 0;
            Megagroup = (Flags & 256) != 0;
            Restricted = (Flags & 512) != 0;
            Signatures = (Flags & 2048) != 0;
            Min = (Flags & 4096) != 0;
            Scam = (Flags & 524288) != 0;
            HasLink = (Flags & 1048576) != 0;
            HasGeo = (Flags & 2097152) != 0;
            SlowmodeEnabled = (Flags & 4194304) != 0;
            CallActive = (Flags & 8388608) != 0;
            CallNotEmpty = (Flags & 16777216) != 0;
            Fake = (Flags & 33554432) != 0;
            Gigagroup = (Flags & 67108864) != 0;
            Noforwards = (Flags & 134217728) != 0;
            Id = br.ReadInt64();
            if ((Flags & 8192) != 0)
                AccessHash = br.ReadInt64();
            else
                AccessHash = null;

            Title = StringUtil.Deserialize(br);
            if ((Flags & 64) != 0)
                Username = StringUtil.Deserialize(br);
            else
                Username = null;

            Photo = (TLAbsChatPhoto)ObjectUtils.DeserializeObject(br);
            Date = br.ReadInt32();
            if ((Flags & 512) != 0)
                RestrictionReason = (TLVector<TLRestrictionReason>)ObjectUtils.DeserializeVector<TLRestrictionReason>(br);
            else
                RestrictionReason = null;

            if ((Flags & 16384) != 0)
                AdminRights = (TLChatAdminRights)ObjectUtils.DeserializeObject(br);
            else
                AdminRights = null;

            if ((Flags & 32768) != 0)
                BannedRights = (TLChatBannedRights)ObjectUtils.DeserializeObject(br);
            else
                BannedRights = null;

            if ((Flags & 262144) != 0)
                DefaultBannedRights = (TLChatBannedRights)ObjectUtils.DeserializeObject(br);
            else
                DefaultBannedRights = null;

            if ((Flags & 131072) != 0)
                ParticipantsCount = br.ReadInt32();
            else
                ParticipantsCount = null;

        }

        public override void SerializeBody(BinaryWriter bw)
        {
            bw.Write(Constructor);
            ComputeFlags();
            bw.Write(Flags);
            bw.Write(Id);
            if ((Flags & 8192) != 0)
                bw.Write(AccessHash.Value);
            StringUtil.Serialize(Title, bw);
            if ((Flags & 64) != 0)
                StringUtil.Serialize(Username, bw);
            ObjectUtils.SerializeObject(Photo, bw);
            bw.Write(Date);
            if ((Flags & 512) != 0)
                ObjectUtils.SerializeObject(RestrictionReason, bw);
            if ((Flags & 16384) != 0)
                ObjectUtils.SerializeObject(AdminRights, bw);
            if ((Flags & 32768) != 0)
                ObjectUtils.SerializeObject(BannedRights, bw);
            if ((Flags & 262144) != 0)
                ObjectUtils.SerializeObject(DefaultBannedRights, bw);
            if ((Flags & 131072) != 0)
                bw.Write(ParticipantsCount.Value);
        }
    }
}
