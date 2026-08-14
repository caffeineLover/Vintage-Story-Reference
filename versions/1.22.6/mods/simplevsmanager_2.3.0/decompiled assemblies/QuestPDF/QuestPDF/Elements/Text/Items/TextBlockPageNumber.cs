using System;
using System.Runtime.CompilerServices;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements.Text.Items
{
	// Token: 0x02000100 RID: 256
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class TextBlockPageNumber : TextBlockSpan
	{
		// Token: 0x17000215 RID: 533
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x000159D6 File Offset: 0x00013BD6
		// (set) Token: 0x060008B5 RID: 2229 RVA: 0x000159DE File Offset: 0x00013BDE
		public Func<IPageContext, string> Source { get; set; } = (IPageContext _) => "123";

		// Token: 0x060008B6 RID: 2230 RVA: 0x000159E7 File Offset: 0x00013BE7
		public void UpdatePageNumberText(IPageContext context)
		{
			base.Text = (this.Source.Invoke(context) ?? "123");
		}

		// Token: 0x040003A6 RID: 934
		public const string PageNumberPlaceholder = "123";
	}
}
