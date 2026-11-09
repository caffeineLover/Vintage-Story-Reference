using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x0200007E RID: 126
	[NullableContext(1)]
	[Nullable(0)]
	public static class ColumnExtensions
	{
		// Token: 0x0600035C RID: 860 RVA: 0x00008FD9 File Offset: 0x000071D9
		[Obsolete("This element has been renamed since version 2022.2. Please use the 'Column' method.")]
		[ExcludeFromCodeCoverage]
		public static void Stack(this IContainer element, Action<ColumnDescriptor> handler)
		{
			element.Column(handler);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00008FE4 File Offset: 0x000071E4
		public static void Column(this IContainer element, Action<ColumnDescriptor> handler)
		{
			ColumnDescriptor columnDescriptor = new ColumnDescriptor();
			handler.Invoke(columnDescriptor);
			element.Element(columnDescriptor.Column);
		}
	}
}
