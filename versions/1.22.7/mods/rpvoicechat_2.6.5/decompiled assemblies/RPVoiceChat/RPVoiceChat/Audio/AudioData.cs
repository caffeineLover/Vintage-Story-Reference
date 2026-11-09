using System;
using OpenTK.Audio.OpenAL;
using RPVoiceChat.Config;
using RPVoiceChat.Networking;
using Vintagestory.API.MathTools;

namespace RPVoiceChat.Audio
{
	// Token: 0x020000FB RID: 251
	public class AudioData
	{
		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060008EB RID: 2283 RVA: 0x0003245E File Offset: 0x0003065E
		// (set) Token: 0x060008EC RID: 2284 RVA: 0x00032466 File Offset: 0x00030666
		public bool ignoreDistanceReduction { get; set; }

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060008ED RID: 2285 RVA: 0x0003246F File Offset: 0x0003066F
		// (set) Token: 0x060008EE RID: 2286 RVA: 0x00032477 File Offset: 0x00030677
		public float wallThicknessOverride { get; set; } = -1f;

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x060008EF RID: 2287 RVA: 0x00032480 File Offset: 0x00030680
		// (set) Token: 0x060008F0 RID: 2288 RVA: 0x00032488 File Offset: 0x00030688
		public bool isGlobalBroadcast { get; set; }

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x060008F1 RID: 2289 RVA: 0x00032491 File Offset: 0x00030691
		// (set) Token: 0x060008F2 RID: 2290 RVA: 0x00032499 File Offset: 0x00030699
		public Vec3d sourcePosOverride { get; set; }

		// Token: 0x060008F4 RID: 2292 RVA: 0x000324B8 File Offset: 0x000306B8
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

		// Token: 0x040003E8 RID: 1000
		public byte[] data;

		// Token: 0x040003E9 RID: 1001
		public int frequency;

		// Token: 0x040003EA RID: 1002
		public ALFormat format;

		// Token: 0x040003EB RID: 1003
		public double amplitude;

		// Token: 0x040003EC RID: 1004
		public VoiceLevel voiceLevel;

		// Token: 0x040003ED RID: 1005
		public string codec;

		// Token: 0x040003EE RID: 1006
		public int transmissionRangeBlocks;

		// Token: 0x040003EF RID: 1007
		public int effectiveRange;
	}
}
