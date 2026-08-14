using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x0200009D RID: 157
	public static class RowExtensions
	{
		// Token: 0x06000459 RID: 1113 RVA: 0x0000B5B4 File Offset: 0x000097B4
		[NullableContext(1)]
		public static void Row(this IContainer element, Action<RowDescriptor> handler)
		{
			RowDescriptor rowDescriptor = new RowDescriptor();
			handler.Invoke(rowDescriptor);
			element.Element(rowDescriptor.Row);
		}
	}
}
