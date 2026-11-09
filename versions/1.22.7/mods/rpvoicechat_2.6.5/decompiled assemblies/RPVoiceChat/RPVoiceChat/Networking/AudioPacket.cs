using System;
using OpenTK.Audio.OpenAL;
using ProtoBuf;
using RPVoiceChat.Audio;

namespace RPVoiceChat.Networking
{
	// Token: 0x0200006E RID: 110
	[ProtoContract(ImplicitFields = 1)]
	public class AudioPacket : NetworkPacket
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060002AA RID: 682 RVA: 0x00010C99 File Offset: 0x0000EE99
		// (set) Token: 0x060002AB RID: 683 RVA: 0x00010CA1 File Offset: 0x0000EEA1
		public string PlayerId { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060002AC RID: 684 RVA: 0x00010CAA File Offset: 0x0000EEAA
		// (set) Token: 0x060002AD RID: 685 RVA: 0x00010CB2 File Offset: 0x0000EEB2
		public byte[] AudioData { get; set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060002AE RID: 686 RVA: 0x00010CBB File Offset: 0x0000EEBB
		// (set) Token: 0x060002AF RID: 687 RVA: 0x00010CC3 File Offset: 0x0000EEC3
		public int Length { get; set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x00010CCC File Offset: 0x0000EECC
		// (set) Token: 0x060002B1 RID: 689 RVA: 0x00010CD4 File Offset: 0x0000EED4
		public VoiceLevel VoiceLevel { get; set; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x00010CDD File Offset: 0x0000EEDD
		// (set) Token: 0x060002B3 RID: 691 RVA: 0x00010CE5 File Offset: 0x0000EEE5
		public int Frequency { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x00010CEE File Offset: 0x0000EEEE
		// (set) Token: 0x060002B5 RID: 693 RVA: 0x00010CF6 File Offset: 0x0000EEF6
		public ALFormat Format { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x00010CFF File Offset: 0x0000EEFF
		// (set) Token: 0x060002B7 RID: 695 RVA: 0x00010D07 File Offset: 0x0000EF07
		public long SequenceNumber { get; set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x00010D10 File Offset: 0x0000EF10
		// (set) Token: 0x060002B9 RID: 697 RVA: 0x00010D18 File Offset: 0x0000EF18
		public string Codec { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060002BA RID: 698 RVA: 0x00010D21 File Offset: 0x0000EF21
		// (set) Token: 0x060002BB RID: 699 RVA: 0x00010D29 File Offset: 0x0000EF29
		public int TransmissionRangeBlocks { get; set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060002BC RID: 700 RVA: 0x00010D32 File Offset: 0x0000EF32
		// (set) Token: 0x060002BD RID: 701 RVA: 0x00010D3A File Offset: 0x0000EF3A
		public int EffectiveRange { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060002BE RID: 702 RVA: 0x00010D43 File Offset: 0x0000EF43
		// (set) Token: 0x060002BF RID: 703 RVA: 0x00010D4B File Offset: 0x0000EF4B
		public bool IgnoreDistanceReduction { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x00010D54 File Offset: 0x0000EF54
		// (set) Token: 0x060002C1 RID: 705 RVA: 0x00010D5C File Offset: 0x0000EF5C
		public float WallThicknessOverride { get; set; } = -1f;

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x00010D65 File Offset: 0x0000EF65
		// (set) Token: 0x060002C3 RID: 707 RVA: 0x00010D6D File Offset: 0x0000EF6D
		public bool HasSourcePositionOverride { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x00010D76 File Offset: 0x0000EF76
		// (set) Token: 0x060002C5 RID: 709 RVA: 0x00010D7E File Offset: 0x0000EF7E
		public double SourcePosX { get; set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x00010D87 File Offset: 0x0000EF87
		// (set) Token: 0x060002C7 RID: 711 RVA: 0x00010D8F File Offset: 0x0000EF8F
		public double SourcePosY { get; set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x00010D98 File Offset: 0x0000EF98
		// (set) Token: 0x060002C9 RID: 713 RVA: 0x00010DA0 File Offset: 0x0000EFA0
		public double SourcePosZ { get; set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060002CA RID: 714 RVA: 0x00010DA9 File Offset: 0x0000EFA9
		protected override PacketType Code
		{
			get
			{
				return PacketType.Audio;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060002CB RID: 715 RVA: 0x00010DAD File Offset: 0x0000EFAD
		// (set) Token: 0x060002CC RID: 716 RVA: 0x00010DB5 File Offset: 0x0000EFB5
		public bool IsGlobalBroadcast { get; set; }

		// Token: 0x060002CD RID: 717 RVA: 0x00010DBE File Offset: 0x0000EFBE
		public AudioPacket()
		{
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00010DD4 File Offset: 0x0000EFD4
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
