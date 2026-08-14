using System;

namespace NVorbis.Contracts.Ogg
{
	// Token: 0x0200003A RID: 58
	internal interface IStreamPageReader
	{
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000242 RID: 578
		NVorbis.Contracts.IPacketProvider PacketProvider { get; }

		// Token: 0x06000243 RID: 579
		void AddPage();

		// Token: 0x06000244 RID: 580
		Memory<byte>[] GetPagePackets(int pageIndex);

		// Token: 0x06000245 RID: 581
		int FindPage(long granulePos);

		// Token: 0x06000246 RID: 582
		bool GetPage(int pageIndex, out long granulePos, out bool isResync, out bool isContinuation, out bool isContinued, out int packetCount, out int pageOverhead);

		// Token: 0x06000247 RID: 583
		void SetEndOfStream();

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000248 RID: 584
		int PageCount { get; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x06000249 RID: 585
		bool HasAllPages { get; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600024A RID: 586
		long? MaxGranulePosition { get; }

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600024B RID: 587
		int FirstDataPageIndex { get; }
	}
}
