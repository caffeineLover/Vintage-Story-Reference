using System;
using System.Collections.Generic;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x02000172 RID: 370
	public sealed class UnsupportedColorSpaceDetails : ColorSpaceDetails
	{
		// Token: 0x17000260 RID: 608
		// (get) Token: 0x0600089A RID: 2202 RVA: 0x00027D77 File Offset: 0x00025F77
		public override int NumberOfColorComponents
		{
			get
			{
				throw new InvalidOperationException("UnsupportedColorSpaceDetails");
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x0600089B RID: 2203 RVA: 0x00027D83 File Offset: 0x00025F83
		internal override int BaseNumberOfColorComponents
		{
			get
			{
				return this.NumberOfColorComponents;
			}
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x00027D8B File Offset: 0x00025F8B
		private UnsupportedColorSpaceDetails() : base(ColorSpace.DeviceGray)
		{
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x00027D94 File Offset: 0x00025F94
		internal override double[] Process(params double[] values)
		{
			throw new InvalidOperationException("UnsupportedColorSpaceDetails");
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x00027DA0 File Offset: 0x00025FA0
		public override IColor GetColor(params double[] values)
		{
			throw new InvalidOperationException("UnsupportedColorSpaceDetails");
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x00027DAC File Offset: 0x00025FAC
		public override IColor GetInitializeColor()
		{
			throw new InvalidOperationException("UnsupportedColorSpaceDetails");
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x00027DB8 File Offset: 0x00025FB8
		internal override IReadOnlyList<byte> Transform(IReadOnlyList<byte> decoded)
		{
			throw new InvalidOperationException("UnsupportedColorSpaceDetails");
		}

		// Token: 0x04000411 RID: 1041
		public static readonly UnsupportedColorSpaceDetails Instance = new UnsupportedColorSpaceDetails();
	}
}
