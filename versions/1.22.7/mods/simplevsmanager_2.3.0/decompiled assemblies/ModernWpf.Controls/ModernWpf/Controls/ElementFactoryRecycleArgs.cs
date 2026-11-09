using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x020000A0 RID: 160
	public sealed class ElementFactoryRecycleArgs
	{
		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060007F6 RID: 2038 RVA: 0x0001C24E File Offset: 0x0001A44E
		// (set) Token: 0x060007F7 RID: 2039 RVA: 0x0001C256 File Offset: 0x0001A456
		public UIElement Parent { get; set; }

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060007F8 RID: 2040 RVA: 0x0001C25F File Offset: 0x0001A45F
		// (set) Token: 0x060007F9 RID: 2041 RVA: 0x0001C267 File Offset: 0x0001A467
		public UIElement Element { get; set; }
	}
}
