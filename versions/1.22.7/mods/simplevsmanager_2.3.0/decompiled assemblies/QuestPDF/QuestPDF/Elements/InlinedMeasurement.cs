using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000D3 RID: 211
	[NullableContext(1)]
	[Nullable(0)]
	internal struct InlinedMeasurement
	{
		// Token: 0x17000174 RID: 372
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x0000FDCC File Offset: 0x0000DFCC
		// (set) Token: 0x0600068B RID: 1675 RVA: 0x0000FDD4 File Offset: 0x0000DFD4
		public Element Element { readonly get; set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x0600068C RID: 1676 RVA: 0x0000FDDD File Offset: 0x0000DFDD
		// (set) Token: 0x0600068D RID: 1677 RVA: 0x0000FDE5 File Offset: 0x0000DFE5
		public SpacePlan Size { readonly get; set; }
	}
}
