using System;
using System.Windows.Media;

namespace ModernWpf.Media.Utils
{
	// Token: 0x02000235 RID: 565
	internal readonly struct ColorScaleStop
	{
		// Token: 0x06001097 RID: 4247 RVA: 0x0002FA33 File Offset: 0x0002DC33
		public ColorScaleStop(Color color, double position)
		{
			this.Color = color;
			this.Position = position;
		}

		// Token: 0x06001098 RID: 4248 RVA: 0x0002FA43 File Offset: 0x0002DC43
		public ColorScaleStop(ColorScaleStop source)
		{
			this.Color = source.Color;
			this.Position = source.Position;
		}

		// Token: 0x0400045D RID: 1117
		public readonly Color Color;

		// Token: 0x0400045E RID: 1118
		public readonly double Position;
	}
}
