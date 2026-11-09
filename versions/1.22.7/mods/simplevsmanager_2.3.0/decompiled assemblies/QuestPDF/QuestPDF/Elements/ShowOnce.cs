using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000EF RID: 239
	internal sealed class ShowOnce : ContainerElement, IStateful
	{
		// Token: 0x060007C8 RID: 1992 RVA: 0x00012EFA File Offset: 0x000110FA
		internal override SpacePlan Measure(Size availableSpace)
		{
			if (this.IsRendered)
			{
				return SpacePlan.Empty();
			}
			return base.Measure(availableSpace);
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00012F14 File Offset: 0x00011114
		internal override void Draw(Size availableSpace)
		{
			if (this.IsRendered)
			{
				return;
			}
			SpacePlanType type = base.Measure(availableSpace).Type;
			bool flag = type == SpacePlanType.Empty || type == SpacePlanType.FullRender;
			if (flag)
			{
				this.IsRendered = true;
			}
			base.Draw(availableSpace);
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060007CA RID: 1994 RVA: 0x00012F55 File Offset: 0x00011155
		// (set) Token: 0x060007CB RID: 1995 RVA: 0x00012F5D File Offset: 0x0001115D
		private bool IsRendered { get; set; }

		// Token: 0x060007CC RID: 1996 RVA: 0x00012F66 File Offset: 0x00011166
		public void ResetState(bool hardReset = false)
		{
			if (hardReset)
			{
				this.IsRendered = false;
			}
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x00012F72 File Offset: 0x00011172
		[NullableContext(1)]
		public object GetState()
		{
			return this.IsRendered;
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x00012F7F File Offset: 0x0001117F
		[NullableContext(1)]
		public void SetState(object state)
		{
			this.IsRendered = (bool)state;
		}
	}
}
