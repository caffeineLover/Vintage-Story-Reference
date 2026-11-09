using System;

namespace QuestPDF.Elements.Text.Items
{
	// Token: 0x02000101 RID: 257
	internal sealed class TextBlockParagraphSpacing : ITextBlockItem
	{
		// Token: 0x17000216 RID: 534
		// (get) Token: 0x060008B8 RID: 2232 RVA: 0x00015A31 File Offset: 0x00013C31
		public float Width { get; }

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x060008B9 RID: 2233 RVA: 0x00015A39 File Offset: 0x00013C39
		public float Height { get; }

		// Token: 0x060008BA RID: 2234 RVA: 0x00015A41 File Offset: 0x00013C41
		public TextBlockParagraphSpacing(float width, float height)
		{
			this.Width = width;
			this.Height = height;
		}
	}
}
