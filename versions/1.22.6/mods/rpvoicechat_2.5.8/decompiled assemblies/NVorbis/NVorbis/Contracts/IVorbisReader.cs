using System;
using System.Collections.Generic;
using System.IO;

namespace NVorbis.Contracts
{
	// Token: 0x02000033 RID: 51
	public interface IVorbisReader : IDisposable
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600020C RID: 524
		// (remove) Token: 0x0600020D RID: 525
		event EventHandler<NewStreamEventArgs> NewStream;

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600020E RID: 526
		long ContainerOverheadBits { get; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600020F RID: 527
		long ContainerWasteBits { get; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000210 RID: 528
		IReadOnlyList<IStreamDecoder> Streams { get; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000211 RID: 529
		int StreamIndex { get; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000212 RID: 530
		int Channels { get; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000213 RID: 531
		int SampleRate { get; }

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000214 RID: 532
		int UpperBitrate { get; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000215 RID: 533
		int NominalBitrate { get; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000216 RID: 534
		int LowerBitrate { get; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000217 RID: 535
		TimeSpan TotalTime { get; }

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000218 RID: 536
		long TotalSamples { get; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000219 RID: 537
		// (set) Token: 0x0600021A RID: 538
		bool ClipSamples { get; set; }

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600021B RID: 539
		// (set) Token: 0x0600021C RID: 540
		TimeSpan TimePosition { get; set; }

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600021D RID: 541
		// (set) Token: 0x0600021E RID: 542
		long SamplePosition { get; set; }

		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600021F RID: 543
		bool HasClipped { get; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000220 RID: 544
		bool IsEndOfStream { get; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000221 RID: 545
		IStreamStats StreamStats { get; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000222 RID: 546
		ITagData Tags { get; }

		// Token: 0x06000223 RID: 547
		bool FindNextStream();

		// Token: 0x06000224 RID: 548
		bool SwitchStreams(int index);

		// Token: 0x06000225 RID: 549
		int ReadSamples(float[] buffer, int offset, int count);

		// Token: 0x06000226 RID: 550
		void SeekTo(TimeSpan timePosition, SeekOrigin seekOrigin = 0);

		// Token: 0x06000227 RID: 551
		void SeekTo(long samplePosition, SeekOrigin seekOrigin = 0);
	}
}
