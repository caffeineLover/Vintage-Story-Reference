using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000CB RID: 203
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class Empty : Element
	{
		// Token: 0x1700015D RID: 349
		// (get) Token: 0x0600063A RID: 1594 RVA: 0x0000F5AF File Offset: 0x0000D7AF
		internal static Empty Instance { get; } = new Empty();

		// Token: 0x0600063B RID: 1595 RVA: 0x0000F5B6 File Offset: 0x0000D7B6
		internal override SpacePlan Measure(Size availableSpace)
		{
			if (!availableSpace.IsNegative())
			{
				return SpacePlan.FullRender(0f, 0f);
			}
			return SpacePlan.Wrap("The available space is negative.");
		}

		// Token: 0x0600063C RID: 1596 RVA: 0x0000F5DA File Offset: 0x0000D7DA
		internal override void Draw(Size availableSpace)
		{
		}
	}
}
