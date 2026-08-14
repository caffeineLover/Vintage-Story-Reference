using System;
using OpenTK.Audio.OpenAL;
using ProtoBuf;
using RPVoiceChat.Audio;

namespace RPVoiceChat.Networking
{
	// Token: 0x02000051 RID: 81
	[ProtoContract(ImplicitFields = 1)]
	public class AudioPacket : NetworkPacket
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600020F RID: 527 RVA: 0x0000D1CD File Offset: 0x0000B3CD
		// (set) Token: 0x06000210 RID: 528 RVA: 0x0000D1D5 File Offset: 0x0000B3D5
		public string PlayerId { get; set; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0000D1DE File Offset: 0x0000B3DE
		// (set) Token: 0x06000212 RID: 530 RVA: 0x0000D1E6 File Offset: 0x0000B3E6
		public byte[] AudioData { get; set; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000213 RID: 531 RVA: 0x0000D1EF File Offset: 0x0000B3EF
		// (set) Token: 0x06000214 RID: 532 RVA: 0x0000D1F7 File Offset: 0x0000B3F7
		public int Length { get; set; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000215 RID: 533 RVA: 0x0000D200 File Offset: 0x0000B400
		// (set) Token: 0x06000216 RID: 534 RVA: 0x0000D208 File Offset: 0x0000B408
		public VoiceLevel VoiceLevel { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0000D211 File Offset: 0x0000B411
		// (set) Token: 0x06000218 RID: 536 RVA: 0x0000D219 File Offset: 0x0000B419
		public int Frequency { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000219 RID: 537 RVA: 0x0000D222 File Offset: 0x0000B422
		// (set) Token: 0x0600021A RID: 538 RVA: 0x0000D22A File Offset: 0x0000B42A
		public ALFormat Format { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x0600021B RID: 539 RVA: 0x0000D233 File Offset: 0x0000B433
		// (set) Token: 0x0600021C RID: 540 RVA: 0x0000D23B File Offset: 0x0000B43B
		public long SequenceNumber { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600021D RID: 541 RVA: 0x0000D244 File Offset: 0x0000B444
		// (set) Token: 0x0600021E RID: 542 RVA: 0x0000D24C File Offset: 0x0000B44C
		public string Codec { get; set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600021F RID: 543 RVA: 0x0000D255 File Offset: 0x0000B455
		// (set) Token: 0x06000220 RID: 544 RVA: 0x0000D25D File Offset: 0x0000B45D
		public int TransmissionRangeBlocks { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000221 RID: 545 RVA: 0x0000D266 File Offset: 0x0000B466
		// (set) Token: 0x06000222 RID: 546 RVA: 0x0000D26E File Offset: 0x0000B46E
		public int EffectiveRange { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000223 RID: 547 RVA: 0x0000D277 File Offset: 0x0000B477
		// (set) Token: 0x06000224 RID: 548 RVA: 0x0000D27F File Offset: 0x0000B47F
		public bool IgnoreDistanceReduction { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000225 RID: 549 RVA: 0x0000D288 File Offset: 0x0000B488
		// (set) Token: 0x06000226 RID: 550 RVA: 0x0000D290 File Offset: 0x0000B490
		public float WallThicknessOverride { get; set; } = -1f;

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000227 RID: 551 RVA: 0x0000D299 File Offset: 0x0000B499
		// (set) Token: 0x06000228 RID: 552 RVA: 0x0000D2A1 File Offset: 0x0000B4A1
		public bool HasSourcePositionOverride { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000229 RID: 553 RVA: 0x0000D2AA File Offset: 0x0000B4AA
		// (set) Token: 0x0600022A RID: 554 RVA: 0x0000D2B2 File Offset: 0x0000B4B2
		public double SourcePosX { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0000D2BB File Offset: 0x0000B4BB
		// (set) Token: 0x0600022C RID: 556 RVA: 0x0000D2C3 File Offset: 0x0000B4C3
		public double SourcePosY { get; set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600022D RID: 557 RVA: 0x0000D2CC File Offset: 0x0000B4CC
		// (set) Token: 0x0600022E RID: 558 RVA: 0x0000D2D4 File Offset: 0x0000B4D4
		public double SourcePosZ { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0000D2DD File Offset: 0x0000B4DD
		protected override PacketType Code
		{
			get
			{
				return PacketType.Audio;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000230 RID: 560 RVA: 0x0000D2E1 File Offset: 0x0000B4E1
		// (set) Token: 0x06000231 RID: 561 RVA: 0x0000D2E9 File Offset: 0x0000B4E9
		public bool IsGlobalBroadcast { get; set; }

		// Token: 0x06000232 RID: 562 RVA: 0x0000D2F2 File Offset: 0x0000B4F2
		public AudioPacket()
		{
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000D308 File Offset: 0x0000B508
		public AudioPacket(string playerId, AudioData audioData, long sequenceNumber)
		{
			this.PlayerId = playerId;
			this.AudioData = audioData.data;
			this.Length = audioData.data.Length;
			this.VoiceLevel = audioData.voiceLevel;
			this.Frequency = audioData.frequency;
			this.Format = audioData.format;
			this.SequenceNumber = sequenceNumber;
			this.Codec = audioData.codec;
			this.TransmissionRangeBlocks = audioData.transmissionRangeBlocks;
			this.IgnoreDistanceReduction = audioData.ignoreDistanceReduction;
			this.WallThicknessOverride = audioData.wallThicknessOverride;
			this.IsGlobalBroadcast = audioData.isGlobalBroadcast;
		}
	}
}
