using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x0200009B RID: 155
	[NullableContext(1)]
	[Nullable(0)]
	public static class RotateExtensions
	{
		// Token: 0x0600044C RID: 1100 RVA: 0x0000B454 File Offset: 0x00009654
		private static IContainer SimpleRotate(this IContainer element, int turnDirection)
		{
			SimpleRotate simpleRotate = (element as SimpleRotate) ?? new SimpleRotate();
			simpleRotate.TurnCount += turnDirection;
			return element.Element(simpleRotate);
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x0000B486 File Offset: 0x00009686
		public static IContainer RotateLeft(this IContainer element)
		{
			return element.SimpleRotate(-1);
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x0000B48F File Offset: 0x0000968F
		public static IContainer RotateRight(this IContainer element)
		{
			return element.SimpleRotate(1);
		}

		// Token: 0x0600044F RID: 1103 RVA: 0x0000B498 File Offset: 0x00009698
		public static IContainer Rotate(this IContainer element, float angle)
		{
			Rotate rotate = (element as Rotate) ?? new Rotate();
			rotate.Angle += angle;
			return element.Element(rotate);
		}
	}
}
