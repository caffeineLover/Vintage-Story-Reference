using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000E0 RID: 224
	internal sealed class PreventPageBreak : ContainerElement, IStateful
	{
		// Token: 0x06000748 RID: 1864 RVA: 0x000118D8 File Offset: 0x0000FAD8
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
			return SpacePlan.PartialRender(Size.Zero);
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0001191C File Offset: 0x0000FB1C
		internal override void Draw(Size availableSpace)
		{
			if (this.IsFirstPageRendered)
			{
				base.Draw(availableSpace);
				return;
			}
			SpacePlanType type = base.Measure(availableSpace).Type;
			bool flag = type == SpacePlanType.Empty || type == SpacePlanType.FullRender;
			if (flag)
			{
				base.Draw(availableSpace);
			}
			this.IsFirstPageRendered = true;
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x0600074A RID: 1866 RVA: 0x00011964 File Offset: 0x0000FB64
		// (set) Token: 0x0600074B RID: 1867 RVA: 0x0001196C File Offset: 0x0000FB6C
		private bool IsFirstPageRendered { get; set; }

		// Token: 0x0600074C RID: 1868 RVA: 0x00011975 File Offset: 0x0000FB75
		public void ResetState(bool hardReset = false)
		{
			if (hardReset)
			{
				this.IsFirstPageRendered = false;
			}
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x00011981 File Offset: 0x0000FB81
		[NullableContext(1)]
		public object GetState()
		{
			return this.IsFirstPageRendered;
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0001198E File Offset: 0x0000FB8E
		[NullableContext(1)]
		public void SetState(object state)
		{
			this.IsFirstPageRendered = (bool)state;
		}
	}
}
