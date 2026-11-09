using System;
using System.Windows.Media;

namespace ModernWpf.Media.ColorPalette
{
	// Token: 0x02000240 RID: 576
	internal class ColorPaletteEntry : IColorPaletteEntry
	{
		// Token: 0x060010DD RID: 4317 RVA: 0x000314C5 File Offset: 0x0002F6C5
		public ColorPaletteEntry(Color color)
		{
			this.ActiveColor = color;
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x060010DE RID: 4318 RVA: 0x000314D4 File Offset: 0x0002F6D4
		public Color ActiveColor { get; }
	}
}
