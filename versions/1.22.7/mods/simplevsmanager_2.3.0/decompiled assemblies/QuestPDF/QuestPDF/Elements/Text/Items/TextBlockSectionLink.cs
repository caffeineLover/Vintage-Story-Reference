using System;
using System.Runtime.CompilerServices;

namespace QuestPDF.Elements.Text.Items
{
	// Token: 0x02000102 RID: 258
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class TextBlockSectionLink : TextBlockSpan
	{
		// Token: 0x17000218 RID: 536
		// (get) Token: 0x060008BB RID: 2235 RVA: 0x00015A57 File Offset: 0x00013C57
		// (set) Token: 0x060008BC RID: 2236 RVA: 0x00015A5F File Offset: 0x00013C5F
		public string SectionName { get; set; }

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x060008BD RID: 2237 RVA: 0x00015A68 File Offset: 0x00013C68
		// (set) Token: 0x060008BE RID: 2238 RVA: 0x00015A70 File Offset: 0x00013C70
		public int ParagraphBeginIndex { get; set; }
	}
}
