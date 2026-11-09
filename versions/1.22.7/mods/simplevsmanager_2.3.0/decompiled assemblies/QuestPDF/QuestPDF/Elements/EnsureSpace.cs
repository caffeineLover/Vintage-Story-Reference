using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000CC RID: 204
	internal sealed class EnsureSpace : ContainerElement, IStateful
	{
		// Token: 0x1700015E RID: 350
		// (get) Token: 0x0600063F RID: 1599 RVA: 0x0000F5F0 File Offset: 0x0000D7F0
		// (set) Token: 0x06000640 RID: 1600 RVA: 0x0000F5F8 File Offset: 0x0000D7F8
		public float MinHeight { get; set; } = 150f;

		// Token: 0x06000641 RID: 1601 RVA: 0x0000F604 File Offset: 0x0000D804
		internal override SpacePlan Measure(Size availableSpace)
		{
			SpacePlan spacePlan = base.Measure(availableSpace);
			if (this.IsFirstPageRendered)
			{
				return spacePlan;
			}
			SpacePlanType type = spacePlan.Type;
			bool flag = type == SpacePlanType.Empty || type == SpacePlanType.FullRender;
			if (flag)
			{
				return spacePlan;
			}
			if (spacePlan.Type == SpacePlanType.PartialRender && this.MinHeight <= spacePlan.Height)
			{
				return spacePlan;
			}
			return SpacePlan.PartialRender(Size.Zero);
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x0000F660 File Offset: 0x0000D860
		internal override void Draw(Size availableSpace)
		{
			if (this.IsFirstPageRendered)
			{
				base.Draw(availableSpace);
				return;
			}
			SpacePlan spacePlan = base.Measure(availableSpace);
			SpacePlanType type = spacePlan.Type;
			bool flag = type == SpacePlanType.Empty || type == SpacePlanType.FullRender;
			if (flag)
			{
				base.Draw(availableSpace);
			}
			if (spacePlan.Type == SpacePlanType.PartialRender && this.MinHeight <= spacePlan.Height)
			{
				base.Draw(availableSpace);
			}
			this.IsFirstPageRendered = true;
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x0000F6C8 File Offset: 0x0000D8C8
		[NullableContext(2)]
		internal override string GetCompanionHint()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(9, 1);
			defaultInterpolatedStringHandler.AppendLiteral("at least ");
			defaultInterpolatedStringHandler.AppendFormatted<float>(this.MinHeight);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000644 RID: 1604 RVA: 0x0000F6FF File Offset: 0x0000D8FF
		// (set) Token: 0x06000645 RID: 1605 RVA: 0x0000F707 File Offset: 0x0000D907
		private bool IsFirstPageRendered { get; set; }

		// Token: 0x06000646 RID: 1606 RVA: 0x0000F710 File Offset: 0x0000D910
		public void ResetState(bool hardReset = false)
		{
			if (hardReset)
			{
				this.IsFirstPageRendered = false;
			}
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x0000F71C File Offset: 0x0000D91C
		[NullableContext(1)]
		public object GetState()
		{
			return this.IsFirstPageRendered;
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x0000F729 File Offset: 0x0000D929
		[NullableContext(1)]
		public void SetState(object state)
		{
			this.IsFirstPageRendered = (bool)state;
		}

		// Token: 0x040002E8 RID: 744
		public const float DefaultMinHeight = 150f;
	}
}
