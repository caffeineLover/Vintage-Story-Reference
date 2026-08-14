using System;

namespace NVorbis.Contracts.Ogg
{
	// Token: 0x02000038 RID: 56
	internal interface IPageData : IPageReader, IDisposable
	{
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000232 RID: 562
		long PageOffset { get; }

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000233 RID: 563
		int StreamSerial { get; }

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000234 RID: 564
		int SequenceNumber { get; }

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000235 RID: 565
		PageFlags PageFlags { get; }

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000236 RID: 566
		long GranulePosition { get; }

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000237 RID: 567
		short PacketCount { get; }

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000238 RID: 568
		bool? IsResync { get; }

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000239 RID: 569
		bool IsContinued { get; }

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600023A RID: 570
		int PageOverhead { get; }

		// Token: 0x0600023B RID: 571
		Memory<byte>[] GetPackets();
	}
}
