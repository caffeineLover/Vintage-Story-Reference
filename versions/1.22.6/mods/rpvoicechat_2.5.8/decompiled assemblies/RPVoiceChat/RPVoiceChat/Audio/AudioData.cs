using System;
using OpenTK.Audio.OpenAL;
using RPVoiceChat.Config;
using RPVoiceChat.Networking;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Audio
{
	// Token: 0x020000C2 RID: 194
	public class AudioData
	{
		// Token: 0x17000128 RID: 296
		// (get) Token: 0x060006D3 RID: 1747 RVA: 0x00026FCA File Offset: 0x000251CA
		// (set) Token: 0x060006D4 RID: 1748 RVA: 0x00026FD2 File Offset: 0x000251D2
		public bool ignoreDistanceReduction { get; set; }

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x060006D5 RID: 1749 RVA: 0x00026FDB File Offset: 0x000251DB
		// (set) Token: 0x060006D6 RID: 1750 RVA: 0x00026FE3 File Offset: 0x000251E3
		public float wallThicknessOverride { get; set; } = -1f;

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x060006D7 RID: 1751 RVA: 0x00026FEC File Offset: 0x000251EC
		// (set) Token: 0x060006D8 RID: 1752 RVA: 0x00026FF4 File Offset: 0x000251F4
		public bool isGlobalBroadcast { get; set; }

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x060006D9 RID: 1753 RVA: 0x00026FFD File Offset: 0x000251FD
		// (set) Token: 0x060006DA RID: 1754 RVA: 0x00027005 File Offset: 0x00025205
		public Vec3d sourcePosOverride { get; set; }

		// Token: 0x060006DC RID: 1756 RVA: 0x00027024 File Offset: 0x00025224
		public static AudioData FromPacket(AudioPacket audioPacket)
		{
			return new AudioData
			{
				data = audioPacket.AudioData,
				frequency = audioPacket.Frequency,
				format = audioPacket.Format,
				voiceLevel = audioPacket.VoiceLevel,
				codec = audioPacket.Codec,
				transmissionRangeBlocks = audioPacket.TransmissionRangeBlocks,
				effectiveRange = ((audioPacket.TransmissionRangeBlocks > 0) ? audioPacket.TransmissionRangeBlocks : WorldConfig.GetInt(audioPacket.VoiceLevel, default(int?))),
				ignoreDistanceReduction = audioPacket.IgnoreDistanceReduction,
				wallThicknessOverride = audioPacket.WallThicknessOverride,
				isGlobalBroadcast = audioPacket.IsGlobalBroadcast,
				sourcePosOverride = (audioPacket.HasSourcePositionOverride ? new Vec3d(audioPacket.SourcePosX, audioPacket.SourcePosY, audioPacket.SourcePosZ) : null)
			};
		}

		// Token: 0x04000309 RID: 777
		public byte[] data;

		// Token: 0x0400030A RID: 778
		public int frequency;

		// Token: 0x0400030B RID: 779
		public ALFormat format;

		// Token: 0x0400030C RID: 780
		public double amplitude;

		// Token: 0x0400030D RID: 781
		public VoiceLevel voiceLevel;

		// Token: 0x0400030E RID: 782
		public string codec;

		// Token: 0x0400030F RID: 783
		public int transmissionRangeBlocks;

		// Token: 0x04000310 RID: 784
		public int effectiveRange;
	}
}
