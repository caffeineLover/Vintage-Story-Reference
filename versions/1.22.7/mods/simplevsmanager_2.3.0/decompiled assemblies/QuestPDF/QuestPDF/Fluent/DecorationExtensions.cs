using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x02000084 RID: 132
	public static class DecorationExtensions
	{
		// Token: 0x0600037C RID: 892 RVA: 0x00009580 File Offset: 0x00007780
		[NullableContext(1)]
		public static void Decoration(this IContainer element, Action<DecorationDescriptor> handler)
		{
			DecorationDescriptor decorationDescriptor = new DecorationDescriptor();
			handler.Invoke(decorationDescriptor);
			element.Element(decorationDescriptor.Decoration);
		}
	}
}
