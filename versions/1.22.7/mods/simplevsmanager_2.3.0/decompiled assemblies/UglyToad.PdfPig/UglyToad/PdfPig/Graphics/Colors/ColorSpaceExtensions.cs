using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x02000173 RID: 371
	public static class ColorSpaceExtensions
	{
		// Token: 0x060008A2 RID: 2210 RVA: 0x00027DD0 File Offset: 0x00025FD0
		public static ColorSpaceFamily GetFamily(this ColorSpace colorSpace)
		{
			switch (colorSpace)
			{
			case ColorSpace.DeviceGray:
			case ColorSpace.DeviceRGB:
			case ColorSpace.DeviceCMYK:
				return ColorSpaceFamily.Device;
			case ColorSpace.CalGray:
			case ColorSpace.CalRGB:
			case ColorSpace.Lab:
			case ColorSpace.ICCBased:
				return ColorSpaceFamily.CIEBased;
			case ColorSpace.Indexed:
			case ColorSpace.Pattern:
			case ColorSpace.Separation:
			case ColorSpace.DeviceN:
				return ColorSpaceFamily.Special;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(26, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unrecognized colorspace: ");
				defaultInterpolatedStringHandler.AppendFormatted<ColorSpace>(colorSpace);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x00027E50 File Offset: 0x00026050
		public static bool TryMapToColorSpace(this NameToken name, out ColorSpace colorspace)
		{
			colorspace = ColorSpace.DeviceGray;
			if (name.Data == NameToken.Devicegray.Data || name.Data == "G")
			{
				colorspace = ColorSpace.DeviceGray;
			}
			else if (name.Data == NameToken.Devicergb.Data || name.Data == "RGB")
			{
				colorspace = ColorSpace.DeviceRGB;
			}
			else if (name.Data == NameToken.Devicecmyk.Data || name.Data == "CMYK")
			{
				colorspace = ColorSpace.DeviceCMYK;
			}
			else if (name.Data == NameToken.Calgray.Data)
			{
				colorspace = ColorSpace.CalGray;
			}
			else if (name.Data == NameToken.Calrgb.Data)
			{
				colorspace = ColorSpace.CalRGB;
			}
			else if (name.Data == NameToken.Lab.Data)
			{
				colorspace = ColorSpace.Lab;
			}
			else if (name.Data == NameToken.Iccbased.Data)
			{
				colorspace = ColorSpace.ICCBased;
			}
			else if (name.Data == NameToken.Indexed.Data || name.Data == "I")
			{
				colorspace = ColorSpace.Indexed;
			}
			else if (name.Data == NameToken.Pattern.Data)
			{
				colorspace = ColorSpace.Pattern;
			}
			else if (name.Data == NameToken.Separation.Data)
			{
				colorspace = ColorSpace.Separation;
			}
			else
			{
				if (!(name.Data == NameToken.Devicen.Data))
				{
					return false;
				}
				colorspace = ColorSpace.DeviceN;
			}
			return true;
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x00027FF8 File Offset: 0x000261F8
		public static NameToken ToNameToken(this ColorSpace colorSpace)
		{
			switch (colorSpace)
			{
			case ColorSpace.DeviceGray:
				return NameToken.Devicegray;
			case ColorSpace.DeviceRGB:
				return NameToken.Devicergb;
			case ColorSpace.DeviceCMYK:
				return NameToken.Devicecmyk;
			case ColorSpace.CalGray:
				return NameToken.Calgray;
			case ColorSpace.CalRGB:
				return NameToken.Calrgb;
			case ColorSpace.Lab:
				return NameToken.Lab;
			case ColorSpace.ICCBased:
				return NameToken.Iccbased;
			case ColorSpace.Indexed:
				return NameToken.Indexed;
			case ColorSpace.Pattern:
				return NameToken.Pattern;
			case ColorSpace.Separation:
				return NameToken.Separation;
			case ColorSpace.DeviceN:
				return NameToken.Devicen;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(26, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unrecognized colorspace: ");
				defaultInterpolatedStringHandler.AppendFormatted<ColorSpace>(colorSpace);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}
	}
}
