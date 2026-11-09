using System;

namespace ModernWpf.Media.Utils
{
	// Token: 0x0200023E RID: 574
	internal static class MathUtils
	{
		// Token: 0x060010D3 RID: 4307 RVA: 0x0003102C File Offset: 0x0002F22C
		public static byte ClampToByte(double c)
		{
			if (double.IsNaN(c))
			{
				return 0;
			}
			if (double.IsPositiveInfinity(c))
			{
				return byte.MaxValue;
			}
			if (double.IsNegativeInfinity(c))
			{
				return 0;
			}
			c = Math.Round(c);
			if (c <= 0.0)
			{
				return 0;
			}
			if (c >= 255.0)
			{
				return byte.MaxValue;
			}
			return (byte)c;
		}

		// Token: 0x060010D4 RID: 4308 RVA: 0x00031088 File Offset: 0x0002F288
		public static double ClampToUnit(double c)
		{
			if (double.IsNaN(c))
			{
				return 0.0;
			}
			if (double.IsPositiveInfinity(c))
			{
				return 1.0;
			}
			if (double.IsNegativeInfinity(c))
			{
				return 0.0;
			}
			if (c <= 0.0)
			{
				return 0.0;
			}
			if (c >= 1.0)
			{
				return 1.0;
			}
			return c;
		}

		// Token: 0x060010D5 RID: 4309 RVA: 0x000310F8 File Offset: 0x0002F2F8
		public static double DegreesToRadians(double degrees)
		{
			return degrees * 0.017453292519943295;
		}

		// Token: 0x060010D6 RID: 4310 RVA: 0x00031105 File Offset: 0x0002F305
		public static double RadiansToDegrees(double radians)
		{
			return radians * 57.29577951308232;
		}

		// Token: 0x060010D7 RID: 4311 RVA: 0x00031112 File Offset: 0x0002F312
		public static double Lerp(double left, double right, double scale)
		{
			if (scale <= 0.0)
			{
				return left;
			}
			if (scale >= 1.0)
			{
				return right;
			}
			return left + scale * (right - left);
		}

		// Token: 0x060010D8 RID: 4312 RVA: 0x00031138 File Offset: 0x0002F338
		public static byte Lerp(byte left, byte right, double scale)
		{
			if (scale <= 0.0)
			{
				return left;
			}
			if (scale >= 1.0)
			{
				return right;
			}
			if (left == right)
			{
				return left;
			}
			double num = (double)left;
			double num2 = (double)right;
			return (byte)Math.Round(num + scale * (num2 - num));
		}
	}
}
