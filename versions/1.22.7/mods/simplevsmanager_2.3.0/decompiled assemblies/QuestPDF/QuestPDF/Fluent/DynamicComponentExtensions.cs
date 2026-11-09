using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x02000086 RID: 134
	public static class DynamicComponentExtensions
	{
		// Token: 0x06000392 RID: 914 RVA: 0x00009D34 File Offset: 0x00007F34
		[NullableContext(1)]
		public static void Dynamic(this IContainer element, IDynamicComponent dynamicElement)
		{
			DynamicComponentProxy child = DynamicComponentProxy.CreateFrom(dynamicElement);
			element.Element(new DynamicHost(child));
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00009D58 File Offset: 0x00007F58
		public static void Dynamic<TState>([Nullable(1)] this IContainer element, [Nullable(new byte[]
		{
			1,
			0
		})] IDynamicComponent<TState> dynamicElement) where TState : struct
		{
			DynamicComponentProxy child = DynamicComponentProxy.CreateFrom<TState>(dynamicElement);
			element.DebugPointer(DebugPointerType.Dynamic, dynamicElement.GetType().Name).Element(new DynamicHost(child));
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00009D8A File Offset: 0x00007F8A
		[NullableContext(1)]
		public static void Element(this IContainer element, IDynamicElement child)
		{
			element.Element(child);
		}
	}
}
