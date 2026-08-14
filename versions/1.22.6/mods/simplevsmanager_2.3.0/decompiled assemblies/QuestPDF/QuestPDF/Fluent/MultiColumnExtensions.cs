using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x02000097 RID: 151
	public static class MultiColumnExtensions
	{
		// Token: 0x06000429 RID: 1065 RVA: 0x0000AFC8 File Offset: 0x000091C8
		[NullableContext(1)]
		public static void MultiColumn(this IContainer element, Action<MultiColumnDescriptor> handler)
		{
			MultiColumnDescriptor descriptor = new MultiColumnDescriptor();
			handler.Invoke(descriptor);
			element.Element(delegate(IContainer x)
			{
				if (!descriptor.MultiColumn.BalanceHeight)
				{
					return x;
				}
				return x.ShrinkVertical();
			}, "x => descriptor.MultiColumn.BalanceHeight ? x.ShrinkVertical() : x", "MultiColumn", "/_/Source/QuestPDF/Fluent/MultiColumnExtensions.cs", 113).Element(descriptor.MultiColumn);
		}
	}
}
