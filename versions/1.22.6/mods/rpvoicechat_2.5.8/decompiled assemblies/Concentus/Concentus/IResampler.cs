using System;

namespace Concentus
{
	// Token: 0x02000006 RID: 6
	public interface IResampler : IDisposable
	{
		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000064 RID: 100
		int InputLatency { get; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000065 RID: 101
		// (set) Token: 0x06000066 RID: 102
		int InputStride { get; set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000067 RID: 103
		int OutputLatencySamples { get; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000068 RID: 104
		TimeSpan OutputLatency { get; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000069 RID: 105
		// (set) Token: 0x0600006A RID: 106
		int OutputStride { get; set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600006B RID: 107
		// (set) Token: 0x0600006C RID: 108
		int Quality { get; set; }

		// Token: 0x0600006D RID: 109
		void GetRateFraction(out int ratio_num, out int ratio_den);

		// Token: 0x0600006E RID: 110
		void GetRates(out int in_rate, out int out_rate);

		// Token: 0x0600006F RID: 111
		void ResetMem();

		// Token: 0x06000070 RID: 112
		void SkipZeroes();

		// Token: 0x06000071 RID: 113
		void Process(int channel_index, Span<float> input, ref int in_len, Span<float> output, ref int out_len);

		// Token: 0x06000072 RID: 114
		void Process(int channel_index, Span<short> input, ref int in_len, Span<short> output, ref int out_len);

		// Token: 0x06000073 RID: 115
		void ProcessInterleaved(Span<float> input, ref int in_len, Span<float> output, ref int out_len);

		// Token: 0x06000074 RID: 116
		void ProcessInterleaved(Span<short> input, ref int in_len, Span<short> output, ref int out_len);

		// Token: 0x06000075 RID: 117
		void SetRateFraction(int ratio_num, int ratio_den, int in_rate, int out_rate);

		// Token: 0x06000076 RID: 118
		void SetRates(int in_rate, int out_rate);
	}
}
