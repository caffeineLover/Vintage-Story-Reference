using System;
using Concentus.Enums;

namespace Concentus
{
	// Token: 0x02000005 RID: 5
	public interface IOpusMultiStreamEncoder : IDisposable
	{
		// Token: 0x0600003F RID: 63
		int EncodeMultistream(ReadOnlySpan<float> in_pcm, int frame_size, Span<byte> out_data, int max_data_bytes);

		// Token: 0x06000040 RID: 64
		int EncodeMultistream(ReadOnlySpan<short> in_pcm, int frame_size, Span<byte> out_data, int max_data_bytes);

		// Token: 0x06000041 RID: 65
		void ResetState();

		// Token: 0x06000042 RID: 66
		string GetVersionString();

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000043 RID: 67
		// (set) Token: 0x06000044 RID: 68
		OpusApplication Application { get; set; }

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000045 RID: 69
		// (set) Token: 0x06000046 RID: 70
		OpusBandwidth Bandwidth { get; set; }

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000047 RID: 71
		// (set) Token: 0x06000048 RID: 72
		int Bitrate { get; set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000049 RID: 73
		// (set) Token: 0x0600004A RID: 74
		int Complexity { get; set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x0600004B RID: 75
		int NumChannels { get; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600004C RID: 76
		// (set) Token: 0x0600004D RID: 77
		OpusFramesize ExpertFrameDuration { get; set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600004E RID: 78
		uint FinalRange { get; }

		// Token: 0x17000029 RID: 41
		// (set) Token: 0x0600004F RID: 79
		OpusMode ForceMode { set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000050 RID: 80
		int Lookahead { get; }

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000051 RID: 81
		// (set) Token: 0x06000052 RID: 82
		int LSBDepth { get; set; }

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000053 RID: 83
		// (set) Token: 0x06000054 RID: 84
		OpusBandwidth MaxBandwidth { get; set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000055 RID: 85
		// (set) Token: 0x06000056 RID: 86
		int PacketLossPercent { get; set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000057 RID: 87
		// (set) Token: 0x06000058 RID: 88
		bool PredictionDisabled { get; set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000059 RID: 89
		int SampleRate { get; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600005A RID: 90
		// (set) Token: 0x0600005B RID: 91
		OpusSignal SignalType { get; set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x0600005C RID: 92
		// (set) Token: 0x0600005D RID: 93
		bool UseConstrainedVBR { get; set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x0600005E RID: 94
		// (set) Token: 0x0600005F RID: 95
		bool UseDTX { get; set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000060 RID: 96
		// (set) Token: 0x06000061 RID: 97
		bool UseInbandFEC { get; set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000062 RID: 98
		// (set) Token: 0x06000063 RID: 99
		bool UseVBR { get; set; }
	}
}
