using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x02000169 RID: 361
	public sealed class DeviceCmykColorSpaceDetails : ColorSpaceDetails
	{
		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000832 RID: 2098 RVA: 0x000263D4 File Offset: 0x000245D4
		public override int NumberOfColorComponents
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x000263D7 File Offset: 0x000245D7
		internal override int BaseNumberOfColorComponents
		{
			get
			{
				return this.NumberOfColorComponents;
			}
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x000263DF File Offset: 0x000245DF
		private DeviceCmykColorSpaceDetails() : base(ColorSpace.DeviceCMYK)
		{
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x000263E8 File Offset: 0x000245E8
		internal override double[] Process(params double[] values)
		{
			return values;
		}

		// Token: 0x06000836 RID: 2102 RVA: 0x000263EC File Offset: 0x000245EC
		public override IColor GetColor(params double[] values)
		{
			if (values == null || values.Length != this.NumberOfColorComponents)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(45, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid number of imputs, expecting ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.NumberOfColorComponents);
				defaultInterpolatedStringHandler.AppendLiteral(" but got ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(values.Length);
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear(), "values");
			}
			double num = values[0];
			double num2 = values[1];
			double num3 = values[2];
			double num4 = values[3];
			if (num == 0.0 && num2 == 0.0 && num3 == 0.0 && num4 == 1.0)
			{
				return CMYKColor.Black;
			}
			if (num == 0.0 && num2 == 0.0 && num3 == 0.0 && num4 == 0.0)
			{
				return CMYKColor.White;
			}
			return new CMYKColor(num, num2, num3, num4);
		}

		// Token: 0x06000837 RID: 2103 RVA: 0x000264D7 File Offset: 0x000246D7
		public override IColor GetInitializeColor()
		{
			return CMYKColor.Black;
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x000264DE File Offset: 0x000246DE
		internal override IReadOnlyList<byte> Transform(IReadOnlyList<byte> decoded)
		{
			return decoded;
		}

		// Token: 0x040003F0 RID: 1008
		public static readonly DeviceCmykColorSpaceDetails Instance = new DeviceCmykColorSpaceDetails();
	}
}
