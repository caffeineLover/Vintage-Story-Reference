using System;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000C6 RID: 198
	public sealed class GenerateDynamicImageDelegatePayload
	{
		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000603 RID: 1539 RVA: 0x0000EEC4 File Offset: 0x0000D0C4
		// (set) Token: 0x06000604 RID: 1540 RVA: 0x0000EECC File Offset: 0x0000D0CC
		public Size AvailableSpace { get; set; }

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000605 RID: 1541 RVA: 0x0000EED5 File Offset: 0x0000D0D5
		// (set) Token: 0x06000606 RID: 1542 RVA: 0x0000EEDD File Offset: 0x0000D0DD
		public ImageSize ImageSize { get; set; }

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000607 RID: 1543 RVA: 0x0000EEE6 File Offset: 0x0000D0E6
		// (set) Token: 0x06000608 RID: 1544 RVA: 0x0000EEEE File Offset: 0x0000D0EE
		public int Dpi { get; set; }
	}
}
