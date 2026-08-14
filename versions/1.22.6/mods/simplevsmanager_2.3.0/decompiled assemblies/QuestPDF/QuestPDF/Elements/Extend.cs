using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000CD RID: 205
	internal sealed class Extend : ContainerElement
	{
		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600064A RID: 1610 RVA: 0x0000F74A File Offset: 0x0000D94A
		// (set) Token: 0x0600064B RID: 1611 RVA: 0x0000F752 File Offset: 0x0000D952
		public bool ExtendVertical { get; set; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600064C RID: 1612 RVA: 0x0000F75B File Offset: 0x0000D95B
		// (set) Token: 0x0600064D RID: 1613 RVA: 0x0000F763 File Offset: 0x0000D963
		public bool ExtendHorizontal { get; set; }

		// Token: 0x0600064E RID: 1614 RVA: 0x0000F76C File Offset: 0x0000D96C
		internal override SpacePlan Measure(Size availableSpace)
		{
			SpacePlan spacePlan = base.Measure(availableSpace);
			SpacePlanType type = spacePlan.Type;
			bool flag = type <= SpacePlanType.Wrap;
			if (flag)
			{
				return spacePlan;
			}
			Size targetSize = this.GetTargetSize(availableSpace, spacePlan);
			if (spacePlan.Type == SpacePlanType.PartialRender)
			{
				return SpacePlan.PartialRender(targetSize);
			}
			if (spacePlan.Type == SpacePlanType.FullRender)
			{
				return SpacePlan.FullRender(targetSize);
			}
			throw new NotSupportedException();
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x0000F7CA File Offset: 0x0000D9CA
		private Size GetTargetSize(Size availableSpace, Size childSize)
		{
			return new Size(this.ExtendHorizontal ? availableSpace.Width : childSize.Width, this.ExtendVertical ? availableSpace.Height : childSize.Height);
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x0000F800 File Offset: 0x0000DA00
		[NullableContext(2)]
		internal override string GetCompanionHint()
		{
			bool extendVertical = this.ExtendVertical;
			bool extendHorizontal = this.ExtendHorizontal;
			string result;
			if (extendVertical)
			{
				if (!extendHorizontal)
				{
					result = "Vertical axis";
				}
				else
				{
					result = "Both axes";
				}
			}
			else if (!extendHorizontal)
			{
				result = null;
			}
			else
			{
				result = "Horizontal axis";
			}
			return result;
		}
	}
}
