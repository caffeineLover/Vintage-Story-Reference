using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000DE RID: 222
	internal sealed class PageBreak : Element, IStateful
	{
		// Token: 0x0600073B RID: 1851 RVA: 0x000117A3 File Offset: 0x0000F9A3
		internal override SpacePlan Measure(Size availableSpace)
		{
			if (availableSpace.IsNegative())
			{
				return SpacePlan.Wrap("The available space is negative.");
			}
			if (this.IsRendered)
			{
				return SpacePlan.Empty();
			}
			return SpacePlan.PartialRender(Size.Zero);
		}

		// Token: 0x0600073C RID: 1852 RVA: 0x000117D0 File Offset: 0x0000F9D0
		internal override void Draw(Size availableSpace)
		{
			this.IsRendered = true;
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x0600073D RID: 1853 RVA: 0x000117D9 File Offset: 0x0000F9D9
		// (set) Token: 0x0600073E RID: 1854 RVA: 0x000117E1 File Offset: 0x0000F9E1
		private bool IsRendered { get; set; }

		// Token: 0x0600073F RID: 1855 RVA: 0x000117EA File Offset: 0x0000F9EA
		public void ResetState(bool hardReset = false)
		{
			this.IsRendered = false;
		}

		// Token: 0x06000740 RID: 1856 RVA: 0x000117F3 File Offset: 0x0000F9F3
		[NullableContext(1)]
		public object GetState()
		{
			return this.IsRendered;
		}

		// Token: 0x06000741 RID: 1857 RVA: 0x00011800 File Offset: 0x0000FA00
		[NullableContext(1)]
		public void SetState(object state)
		{
			this.IsRendered = (bool)state;
		}
	}
}
