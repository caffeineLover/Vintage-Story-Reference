using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000D6 RID: 214
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class Layers : Element
	{
		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060006B0 RID: 1712 RVA: 0x00010616 File Offset: 0x0000E816
		// (set) Token: 0x060006B1 RID: 1713 RVA: 0x0001061E File Offset: 0x0000E81E
		public List<Layer> Children { get; set; } = new List<Layer>();

		// Token: 0x060006B2 RID: 1714 RVA: 0x00010627 File Offset: 0x0000E827
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		internal override IEnumerable<Element> GetChildren()
		{
			return this.Children;
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00010630 File Offset: 0x0000E830
		internal override SpacePlan Measure(Size availableSpace)
		{
			SpacePlan spacePlan = Enumerable.Single<Layer>(this.Children, (Layer x) => x.IsPrimary).Measure(availableSpace);
			if (spacePlan.Type == SpacePlanType.Wrap)
			{
				return SpacePlan.Wrap("The content of the primary layer does not fit (even partially) the available space.");
			}
			return spacePlan;
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x00010684 File Offset: 0x0000E884
		internal override void Draw(Size availableSpace)
		{
			Enumerable.ToList<Layer>(Enumerable.Where<Layer>(this.Children, (Layer x) => x.Measure(availableSpace).Type != SpacePlanType.Wrap)).ForEach(delegate(Layer x)
			{
				x.Draw(availableSpace);
			});
		}
	}
}
