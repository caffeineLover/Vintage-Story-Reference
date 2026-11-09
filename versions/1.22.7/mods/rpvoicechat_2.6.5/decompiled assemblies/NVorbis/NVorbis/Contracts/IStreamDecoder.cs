using System;
using System.IO;

namespace NVorbis.Contracts
{
	// Token: 0x02000030 RID: 48
	public interface IStreamDecoder : IDisposable
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060001DD RID: 477
		int Channels { get; }

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060001DE RID: 478
		int SampleRate { get; }

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060001DF RID: 479
		int UpperBitrate { get; }

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001E0 RID: 480
		int NominalBitrate { get; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001E1 RID: 481
		int LowerBitrate { get; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001E2 RID: 482
		ITagData Tags { get; }

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001E3 RID: 483
		TimeSpan TotalTime { get; }

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001E4 RID: 484
		long TotalSamples { get; }

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001E5 RID: 485
		// (set) Token: 0x060001E6 RID: 486
		TimeSpan TimePosition { get; set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060001E7 RID: 487
		// (set) Token: 0x060001E8 RID: 488
		long SamplePosition { get; set; }

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060001E9 RID: 489
		// (set) Token: 0x060001EA RID: 490
		bool ClipSamples { get; set; }

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060001EB RID: 491
		bool HasClipped { get; }

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060001EC RID: 492
		bool IsEndOfStream { get; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060001ED RID: 493
		IStreamStats Stats { get; }

		// Token: 0x060001EE RID: 494
		void SeekTo(TimeSpan timePosition, SeekOrigin seekOrigin = 0);

		// Token: 0x060001EF RID: 495
		void SeekTo(long samplePosition, SeekOrigin seekOrigin = 0);

		// Token: 0x060001F0 RID: 496
		int Read(Span<float> buffer, int offset, int count);
	}
}
