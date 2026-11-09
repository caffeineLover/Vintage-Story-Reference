using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x02000167 RID: 359
	public sealed class DeviceGrayColorSpaceDetails : ColorSpaceDetails
	{
		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000822 RID: 2082 RVA: 0x00026214 File Offset: 0x00024414
		public override int NumberOfColorComponents
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000823 RID: 2083 RVA: 0x00026217 File Offset: 0x00024417
		internal override int BaseNumberOfColorComponents
		{
			get
			{
				return this.NumberOfColorComponents;
			}
		}

		// Token: 0x06000824 RID: 2084 RVA: 0x0002621F File Offset: 0x0002441F
		private DeviceGrayColorSpaceDetails() : base(ColorSpace.DeviceGray)
		{
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x00026228 File Offset: 0x00024428
		internal override double[] Process(params double[] values)
		{
			return values;
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x0002622C File Offset: 0x0002442C
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
			if (num == 0.0)
			{
				return GrayColor.Black;
			}
			if (num == 1.0)
			{
				return GrayColor.White;
			}
			return new GrayColor(num);
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x000262C0 File Offset: 0x000244C0
		public override IColor GetInitializeColor()
		{
			return GrayColor.Black;
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x000262C7 File Offset: 0x000244C7
		internal override IReadOnlyList<byte> Transform(IReadOnlyList<byte> decoded)
		{
			return decoded;
		}

		// Token: 0x040003EE RID: 1006
		public static readonly DeviceGrayColorSpaceDetails Instance = new DeviceGrayColorSpaceDetails();
	}
}
