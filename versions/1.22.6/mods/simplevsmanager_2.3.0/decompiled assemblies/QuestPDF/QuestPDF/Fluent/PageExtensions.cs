using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x0200009A RID: 154
	public static class PageExtensions
	{
		// Token: 0x0600044B RID: 1099 RVA: 0x0000B420 File Offset: 0x00009620
		[NullableContext(1)]
		public static IDocumentContainer Page(this IDocumentContainer document, Action<PageDescriptor> handler)
		{
			PageDescriptor pageDescriptor = new PageDescriptor();
			handler.Invoke(pageDescriptor);
			(document as DocumentContainer).Pages.Add(pageDescriptor.Page);
			return document;
		}
	}
}
