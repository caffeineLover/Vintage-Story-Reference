using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x0200007F RID: 127
	[NullableContext(1)]
	[Nullable(0)]
	public static class ComponentExtensions
	{
		// Token: 0x0600035E RID: 862 RVA: 0x0000900C File Offset: 0x0000720C
		public static void Component<[Nullable(0)] T>(this IContainer element, T component) where T : IComponent
		{
			IContainer container = element.Container().DebugPointer(DebugPointerType.Component, component.GetType().Name);
			component.Compose(container);
		}

		// Token: 0x0600035F RID: 863 RVA: 0x00009046 File Offset: 0x00007246
		public static void Component<[Nullable(0)] T>(this IContainer element) where T : IComponent, new()
		{
			element.Component(Activator.CreateInstance<T>());
		}
	}
}
