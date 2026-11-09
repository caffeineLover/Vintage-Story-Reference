using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x0200008B RID: 139
	public static class GridExtensions
	{
		// Token: 0x060003D6 RID: 982 RVA: 0x0000A684 File Offset: 0x00008884
		[NullableContext(1)]
		[Obsolete("This element has been deprecated since version 2022.11. Please use the Table element, or the combination of the Row and Column elements.")]
		[ExcludeFromCodeCoverage]
		public static void Grid(this IContainer element, Action<GridDescriptor> handler)
		{
			GridDescriptor gridDescriptor = new GridDescriptor();
			Alignment alignment = element as Alignment;
			if (alignment != null && alignment.Horizontal != null)
			{
				gridDescriptor.Alignment(alignment.Horizontal.Value);
			}
			handler.Invoke(gridDescriptor);
			element.Component(gridDescriptor.Grid);
		}
	}
}
