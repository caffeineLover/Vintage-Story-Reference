using System;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000F4 RID: 244
	internal sealed class StopPaging : ContainerElement
	{
		// Token: 0x060007F3 RID: 2035 RVA: 0x000133DC File Offset: 0x000115DC
		internal override SpacePlan Measure(Size availableSpace)
		{
			SpacePlan spacePlan = base.Measure(availableSpace);
			SpacePlan result;
			switch (spacePlan.Type)
			{
			case SpacePlanType.Empty:
				result = spacePlan;
				break;
			case SpacePlanType.Wrap:
				result = SpacePlan.FullRender(Size.Zero);
				break;
			case SpacePlanType.PartialRender:
				result = SpacePlan.FullRender(spacePlan);
				break;
			case SpacePlanType.FullRender:
				result = spacePlan;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return result;
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x0001343A File Offset: 0x0001163A
		internal override void Draw(Size availableSpace)
		{
			if (base.Measure(availableSpace).Type == SpacePlanType.Wrap)
			{
				return;
			}
			base.Draw(availableSpace);
		}
	}
}
