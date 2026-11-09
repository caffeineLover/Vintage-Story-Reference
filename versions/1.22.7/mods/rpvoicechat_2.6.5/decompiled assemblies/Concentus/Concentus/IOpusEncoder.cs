using System;
using Concentus.Enums;

namespace Concentus
{
	// Token: 0x02000003 RID: 3
	public interface IOpusEncoder : IDisposable
	{
		// Token: 0x0600000D RID: 13
		int Encode(ReadOnlySpan<short> in_pcm, int frame_size, Span<byte> out_data, int max_data_bytes);

		// Token: 0x0600000E RID: 14
		int Encode(ReadOnlySpan<float> in_pcm, int frame_size, Span<byte> out_data, int max_data_bytes);

		// Token: 0x0600000F RID: 15
		void ResetState();

		// Token: 0x06000010 RID: 16
		string GetVersionString();

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000011 RID: 17
		// (set) Token: 0x06000012 RID: 18
		OpusApplication Application { get; set; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000013 RID: 19
		// (set) Token: 0x06000014 RID: 20
		int Bitrate { get; set; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000015 RID: 21
		// (set) Token: 0x06000016 RID: 22
		int ForceChannels { get; set; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000017 RID: 23
		// (set) Token: 0x06000018 RID: 24
		OpusBandwidth MaxBandwidth { get; set; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000019 RID: 25
		// (set) Token: 0x0600001A RID: 26
		OpusBandwidth Bandwidth { get; set; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600001B RID: 27
		// (set) Token: 0x0600001C RID: 28
		bool UseDTX { get; set; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600001D RID: 29
		// (set) Token: 0x0600001E RID: 30
		int Complexity { get; set; }

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600001F RID: 31
		// (set) Token: 0x06000020 RID: 32
		bool UseInbandFEC { get; set; }

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000021 RID: 33
		// (set) Token: 0x06000022 RID: 34
		int PacketLossPercent { get; set; }

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000023 RID: 35
		// (set) Token: 0x06000024 RID: 36
		bool UseVBR { get; set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000025 RID: 37
		// (set) Token: 0x06000026 RID: 38
		bool UseConstrainedVBR { get; set; }

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000027 RID: 39
		// (set) Token: 0x06000028 RID: 40
		OpusSignal SignalType { get; set; }

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000029 RID: 41
		int Lookahead { get; }

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600002A RID: 42
		int SampleRate { get; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600002B RID: 43
		int NumChannels { get; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600002C RID: 44
		uint FinalRange { get; }

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600002D RID: 45
		// (set) Token: 0x0600002E RID: 46
		int LSBDepth { get; set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600002F RID: 47
		// (set) Token: 0x06000030 RID: 48
		OpusFramesize ExpertFrameDuration { get; set; }

		// Token: 0x1700001A RID: 26
		// (set) Token: 0x06000031 RID: 49
		OpusMode ForceMode { set; }

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000032 RID: 50
		// (set) Token: 0x06000033 RID: 51
		bool PredictionDisabled { get; set; }
	}
}
