using System;
using System.Runtime.CompilerServices;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements.Text.Items
{
	// Token: 0x02000103 RID: 259
	[NullableContext(1)]
	[Nullable(0)]
	internal class TextBlockSpan : ITextBlockItem
	{
		// Token: 0x1700021A RID: 538
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x00015A81 File Offset: 0x00013C81
		// (set) Token: 0x060008C1 RID: 2241 RVA: 0x00015A89 File Offset: 0x00013C89
		public string Text { get; set; }

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x060008C2 RID: 2242 RVA: 0x00015A92 File Offset: 0x00013C92
		// (set) Token: 0x060008C3 RID: 2243 RVA: 0x00015A9A File Offset: 0x00013C9A
		public TextStyle Style { get; set; } = TextStyle.Default;
	}
}
