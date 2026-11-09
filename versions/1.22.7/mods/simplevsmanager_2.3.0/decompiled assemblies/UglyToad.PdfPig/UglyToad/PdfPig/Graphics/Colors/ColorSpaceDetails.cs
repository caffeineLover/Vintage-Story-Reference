using System;
using System.Collections.Generic;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x02000166 RID: 358
	public abstract class ColorSpaceDetails
	{
		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000817 RID: 2071 RVA: 0x000261D1 File Offset: 0x000243D1
		public ColorSpace Type { get; }

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000818 RID: 2072
		public abstract int NumberOfColorComponents { get; }

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000819 RID: 2073 RVA: 0x000261D9 File Offset: 0x000243D9
		// (set) Token: 0x0600081A RID: 2074 RVA: 0x000261E1 File Offset: 0x000243E1
		public ColorSpace BaseType { get; protected set; }

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x0600081B RID: 2075
		internal abstract int BaseNumberOfColorComponents { get; }

		// Token: 0x0600081C RID: 2076 RVA: 0x000261EA File Offset: 0x000243EA
		protected internal ColorSpaceDetails(ColorSpace type)
		{
			this.Type = type;
			this.BaseType = type;
		}

		// Token: 0x0600081D RID: 2077
		public abstract IColor GetColor(params double[] values);

		// Token: 0x0600081E RID: 2078
		internal abstract double[] Process(params double[] values);

		// Token: 0x0600081F RID: 2079
		public abstract IColor GetInitializeColor();

		// Token: 0x06000820 RID: 2080
		internal abstract IReadOnlyList<byte> Transform(IReadOnlyList<byte> decoded);

		// Token: 0x06000821 RID: 2081 RVA: 0x00026200 File Offset: 0x00024400
		protected static byte ConvertToByte(double componentValue)
		{
			return (byte)Math.Round(componentValue * 255.0, 1);
		}
	}
}
