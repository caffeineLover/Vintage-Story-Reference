using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing.Proxy;
using QuestPDF.Elements;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x02000082 RID: 130
	[NullableContext(1)]
	[Nullable(0)]
	public static class DebugExtensions
	{
		// Token: 0x0600036C RID: 876 RVA: 0x00009320 File Offset: 0x00007520
		public static IContainer DebugArea(this IContainer parent, [Nullable(2)] string text = null, Color? color = null)
		{
			Container container = new Container();
			parent.Component(new DebugArea
			{
				Child = container,
				Text = (text ?? string.Empty),
				Color = (color ?? Colors.Red.Medium)
			});
			return container;
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00009375 File Offset: 0x00007575
		public static IContainer DebugPointer(this IContainer parent, string label)
		{
			return parent.DebugPointer(DebugPointerType.UserDefined, label);
		}

		// Token: 0x0600036E RID: 878 RVA: 0x0000937F File Offset: 0x0000757F
		internal static IContainer DebugPointer(this IContainer parent, DebugPointerType type, string label)
		{
			return parent.Element(new DebugPointer
			{
				Type = type,
				Label = label
			});
		}

		// Token: 0x0600036F RID: 879 RVA: 0x0000939A File Offset: 0x0000759A
		internal static IContainer LayoutOverflowVisualization(this IContainer parent)
		{
			return parent.Element(new LayoutOverflowVisualization());
		}
	}
}
