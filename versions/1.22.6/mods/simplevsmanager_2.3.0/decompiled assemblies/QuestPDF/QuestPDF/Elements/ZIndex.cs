using System;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000FA RID: 250
	internal sealed class ZIndex : ContainerElement
	{
		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x0600084F RID: 2127 RVA: 0x000143B2 File Offset: 0x000125B2
		// (set) Token: 0x06000850 RID: 2128 RVA: 0x000143BA File Offset: 0x000125BA
		public int Depth { get; set; }

		// Token: 0x06000851 RID: 2129 RVA: 0x000143C4 File Offset: 0x000125C4
		internal override void Draw(Size availableSpace)
		{
			int zindex = base.Canvas.GetZIndex();
			base.Canvas.SetZIndex(this.Depth);
			base.Draw(availableSpace);
			base.Canvas.SetZIndex(zindex);
		}
	}
}
