using System;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000C5 RID: 197
	internal sealed class DynamicElement : ContainerElement, IDynamicElement, IElement
	{
		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000600 RID: 1536 RVA: 0x0000EEAB File Offset: 0x0000D0AB
		// (set) Token: 0x06000601 RID: 1537 RVA: 0x0000EEB3 File Offset: 0x0000D0B3
		public Size Size { get; internal set; }
	}
}
