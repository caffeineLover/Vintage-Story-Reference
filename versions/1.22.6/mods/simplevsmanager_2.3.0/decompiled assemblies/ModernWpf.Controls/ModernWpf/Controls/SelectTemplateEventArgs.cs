using System;
using System.Windows;

namespace ModernWpf.Controls
{
	// Token: 0x020000A4 RID: 164
	public sealed class SelectTemplateEventArgs : EventArgs
	{
		// Token: 0x06000819 RID: 2073 RVA: 0x0001C846 File Offset: 0x0001AA46
		internal SelectTemplateEventArgs()
		{
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x0600081A RID: 2074 RVA: 0x0001C84E File Offset: 0x0001AA4E
		// (set) Token: 0x0600081B RID: 2075 RVA: 0x0001C856 File Offset: 0x0001AA56
		public string TemplateKey { get; set; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x0600081C RID: 2076 RVA: 0x0001C85F File Offset: 0x0001AA5F
		// (set) Token: 0x0600081D RID: 2077 RVA: 0x0001C867 File Offset: 0x0001AA67
		public object DataContext { get; internal set; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x0600081E RID: 2078 RVA: 0x0001C870 File Offset: 0x0001AA70
		// (set) Token: 0x0600081F RID: 2079 RVA: 0x0001C878 File Offset: 0x0001AA78
		public UIElement Owner { get; internal set; }
	}
}
