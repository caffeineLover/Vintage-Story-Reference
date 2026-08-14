using System;
using System.Runtime.CompilerServices;

namespace QuestPDF.Elements.Text.Items
{
	// Token: 0x020000FF RID: 255
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class TextBlockHyperlink : TextBlockSpan
	{
		// Token: 0x17000213 RID: 531
		// (get) Token: 0x060008AF RID: 2223 RVA: 0x000159AC File Offset: 0x00013BAC
		// (set) Token: 0x060008B0 RID: 2224 RVA: 0x000159B4 File Offset: 0x00013BB4
		public string Url { get; set; }

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x060008B1 RID: 2225 RVA: 0x000159BD File Offset: 0x00013BBD
		// (set) Token: 0x060008B2 RID: 2226 RVA: 0x000159C5 File Offset: 0x00013BC5
		public int ParagraphBeginIndex { get; set; }
	}
}
