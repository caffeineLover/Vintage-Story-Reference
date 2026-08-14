using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x02000090 RID: 144
	public static class InlinedExtensions
	{
		// Token: 0x06000403 RID: 1027 RVA: 0x0000AB98 File Offset: 0x00008D98
		[NullableContext(1)]
		public static void Inlined(this IContainer element, Action<InlinedDescriptor> handler)
		{
			InlinedDescriptor inlinedDescriptor = new InlinedDescriptor();
			handler.Invoke(inlinedDescriptor);
			element.Element(inlinedDescriptor.Inlined);
		}
	}
}
