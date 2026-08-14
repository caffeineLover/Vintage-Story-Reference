using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x0200009F RID: 159
	public sealed class ElementFactoryGetArgs
	{
		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060007EF RID: 2031 RVA: 0x0001C213 File Offset: 0x0001A413
		// (set) Token: 0x060007F0 RID: 2032 RVA: 0x0001C21B File Offset: 0x0001A41B
		public UIElement Parent { get; set; }

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060007F1 RID: 2033 RVA: 0x0001C224 File Offset: 0x0001A424
		// (set) Token: 0x060007F2 RID: 2034 RVA: 0x0001C22C File Offset: 0x0001A42C
		public object Data { get; set; }

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060007F3 RID: 2035 RVA: 0x0001C235 File Offset: 0x0001A435
		// (set) Token: 0x060007F4 RID: 2036 RVA: 0x0001C23D File Offset: 0x0001A43D
		internal int Index { get; set; }
	}
}
