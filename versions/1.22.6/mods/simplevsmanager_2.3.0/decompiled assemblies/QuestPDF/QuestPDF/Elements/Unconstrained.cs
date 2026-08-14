using System;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000F9 RID: 249
	internal sealed class Unconstrained : ContainerElement, IContentDirectionAware
	{
		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x0600084A RID: 2122 RVA: 0x000142D1 File Offset: 0x000124D1
		// (set) Token: 0x0600084B RID: 2123 RVA: 0x000142D9 File Offset: 0x000124D9
		public ContentDirection ContentDirection { get; set; }

		// Token: 0x0600084C RID: 2124 RVA: 0x000142E4 File Offset: 0x000124E4
		internal override SpacePlan Measure(Size availableSpace)
		{
			SpacePlan spacePlan = base.Measure(Size.Max);
			if (spacePlan.Type == SpacePlanType.PartialRender)
			{
				return SpacePlan.PartialRender(0f, 0f);
			}
			if (spacePlan.Type == SpacePlanType.FullRender)
			{
				return SpacePlan.FullRender(0f, 0f);
			}
			return spacePlan;
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x00014330 File Offset: 0x00012530
		internal override void Draw(Size availableSpace)
		{
			SpacePlan spacePlan = base.Measure(Size.Max);
			SpacePlanType type = spacePlan.Type;
			bool flag = type <= SpacePlanType.Wrap;
			if (flag)
			{
				return;
			}
			Position vector = (this.ContentDirection == ContentDirection.RightToLeft) ? new Position(-spacePlan.Width, 0f) : Position.Zero;
			base.Canvas.Translate(vector);
			base.Draw(spacePlan);
			base.Canvas.Translate(vector.Reverse());
		}
	}
}
