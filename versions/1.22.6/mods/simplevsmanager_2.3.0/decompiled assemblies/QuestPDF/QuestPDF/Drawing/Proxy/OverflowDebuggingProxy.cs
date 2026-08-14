using System;
using System.Runtime.CompilerServices;
using QuestPDF.Infrastructure;

namespace QuestPDF.Drawing.Proxy
{
	// Token: 0x02000118 RID: 280
	internal sealed class OverflowDebuggingProxy : ElementProxy
	{
		// Token: 0x17000253 RID: 595
		// (get) Token: 0x060009AC RID: 2476 RVA: 0x00019B9B File Offset: 0x00017D9B
		// (set) Token: 0x060009AD RID: 2477 RVA: 0x00019BA3 File Offset: 0x00017DA3
		public bool IsMeasuring { get; private set; } = true;

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x060009AE RID: 2478 RVA: 0x00019BAC File Offset: 0x00017DAC
		// (set) Token: 0x060009AF RID: 2479 RVA: 0x00019BB4 File Offset: 0x00017DB4
		public Size? AvailableSpace { get; private set; }

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x060009B0 RID: 2480 RVA: 0x00019BBD File Offset: 0x00017DBD
		// (set) Token: 0x060009B1 RID: 2481 RVA: 0x00019BC5 File Offset: 0x00017DC5
		public SpacePlan? SpacePlan { get; private set; }

		// Token: 0x060009B2 RID: 2482 RVA: 0x00019BCE File Offset: 0x00017DCE
		[NullableContext(1)]
		public OverflowDebuggingProxy(Element child)
		{
			base.Child = child;
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x00019BE4 File Offset: 0x00017DE4
		internal override SpacePlan Measure(Size availableSpace)
		{
			SpacePlan spacePlan = base.Child.Measure(availableSpace);
			if (this.IsMeasuring)
			{
				this.AvailableSpace = new Size?(availableSpace);
				this.SpacePlan = new SpacePlan?(spacePlan);
			}
			return spacePlan;
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x00019C1F File Offset: 0x00017E1F
		public void StopMeasuring()
		{
			this.IsMeasuring = false;
		}
	}
}
