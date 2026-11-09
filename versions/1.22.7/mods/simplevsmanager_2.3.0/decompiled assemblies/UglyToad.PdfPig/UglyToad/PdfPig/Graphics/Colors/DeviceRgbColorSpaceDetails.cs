using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x02000168 RID: 360
	public sealed class DeviceRgbColorSpaceDetails : ColorSpaceDetails
	{
		// Token: 0x17000233 RID: 563
		// (get) Token: 0x0600082A RID: 2090 RVA: 0x000262D6 File Offset: 0x000244D6
		public override int NumberOfColorComponents
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x0600082B RID: 2091 RVA: 0x000262D9 File Offset: 0x000244D9
		internal override int BaseNumberOfColorComponents
		{
			get
			{
				return this.NumberOfColorComponents;
			}
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x000262E1 File Offset: 0x000244E1
		private DeviceRgbColorSpaceDetails() : base(ColorSpace.DeviceRGB)
		{
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x000262EA File Offset: 0x000244EA
		internal override double[] Process(params double[] values)
		{
			return values;
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x000262F0 File Offset: 0x000244F0
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
			if (num == 0.0 && num2 == 0.0 && num3 == 0.0)
			{
				return RGBColor.Black;
			}
			if (num == 1.0 && num2 == 1.0 && num3 == 1.0)
			{
				return RGBColor.White;
			}
			return new RGBColor(num, num2, num3);
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x000263BE File Offset: 0x000245BE
		public override IColor GetInitializeColor()
		{
			return RGBColor.Black;
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x000263C5 File Offset: 0x000245C5
		internal override IReadOnlyList<byte> Transform(IReadOnlyList<byte> decoded)
		{
			return decoded;
		}

		// Token: 0x040003EF RID: 1007
		public static readonly DeviceRgbColorSpaceDetails Instance = new DeviceRgbColorSpaceDetails();
	}
}
