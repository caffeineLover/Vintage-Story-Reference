using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000F2 RID: 242
	internal sealed class SkipOnce : ContainerElement, IStateful
	{
		// Token: 0x060007E0 RID: 2016 RVA: 0x000132B0 File Offset: 0x000114B0
		internal override SpacePlan Measure(Size availableSpace)
		{
			if (!this.FirstPageWasSkipped)
			{
				return SpacePlan.Empty();
			}
			return base.Measure(availableSpace);
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x000132C7 File Offset: 0x000114C7
		internal override void Draw(Size availableSpace)
		{
			if (this.FirstPageWasSkipped)
			{
				base.Child.Draw(availableSpace);
			}
			this.FirstPageWasSkipped = true;
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060007E2 RID: 2018 RVA: 0x000132E4 File Offset: 0x000114E4
		// (set) Token: 0x060007E3 RID: 2019 RVA: 0x000132EC File Offset: 0x000114EC
		private bool FirstPageWasSkipped { get; set; }

		// Token: 0x060007E4 RID: 2020 RVA: 0x000132F5 File Offset: 0x000114F5
		public void ResetState(bool hardReset = false)
		{
			if (hardReset)
			{
				this.FirstPageWasSkipped = false;
			}
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00013301 File Offset: 0x00011501
		[NullableContext(1)]
		public object GetState()
		{
			return this.FirstPageWasSkipped;
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x0001330E File Offset: 0x0001150E
		[NullableContext(1)]
		public void SetState(object state)
		{
			this.FirstPageWasSkipped = (bool)state;
		}
	}
}
