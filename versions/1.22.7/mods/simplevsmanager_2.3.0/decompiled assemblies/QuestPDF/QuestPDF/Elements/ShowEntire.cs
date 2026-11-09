using System;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000EC RID: 236
	internal sealed class ShowEntire : ContainerElement
	{
		// Token: 0x060007BB RID: 1979 RVA: 0x00012DE8 File Offset: 0x00010FE8
		internal override SpacePlan Measure(Size availableSpace)
		{
			SpacePlan spacePlan = base.Measure(availableSpace);
			if (spacePlan.Type == SpacePlanType.Wrap)
			{
				return SpacePlan.Wrap("Child element does not fit (even partially) on the page.");
			}
			if (spacePlan.Type == SpacePlanType.PartialRender)
			{
				return SpacePlan.Wrap("Child element fits only partially on the page.");
			}
			return spacePlan;
		}
	}
}
