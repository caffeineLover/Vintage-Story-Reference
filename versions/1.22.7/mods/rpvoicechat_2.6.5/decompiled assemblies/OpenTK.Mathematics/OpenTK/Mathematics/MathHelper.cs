using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace OpenTK.Mathematics
{
	// Token: 0x0200000F RID: 15
	public static class MathHelper
	{
		// Token: 0x0600022C RID: 556 RVA: 0x00008A19 File Offset: 0x00006C19
		public static decimal Abs(decimal n)
		{
			return Math.Abs(n);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x00008A21 File Offset: 0x00006C21
		public static double Abs(double n)
		{
			return Math.Abs(n);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00008A29 File Offset: 0x00006C29
		public static short Abs(short n)
		{
			return Math.Abs(n);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x00008A31 File Offset: 0x00006C31
		public static int Abs(int n)
		{
			return Math.Abs(n);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00008A39 File Offset: 0x00006C39
		public static long Abs(long n)
		{
			return Math.Abs(n);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00008A41 File Offset: 0x00006C41
		public static sbyte Abs(sbyte n)
		{
			return Math.Abs(n);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00008A49 File Offset: 0x00006C49
		public static float Abs(float n)
		{
			return Math.Abs(n);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00008A51 File Offset: 0x00006C51
		public static double Sin(double radians)
		{
			return Math.Sin(radians);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00008A59 File Offset: 0x00006C59
		public static double Sinh(double radians)
		{
			return Math.Sinh(radians);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00008A61 File Offset: 0x00006C61
		public static double Asin(double radians)
		{
			return Math.Asin(radians);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00008A69 File Offset: 0x00006C69
		public static double Cos(double radians)
		{
			return Math.Cos(radians);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00008A71 File Offset: 0x00006C71
		public static double Cosh(double radians)
		{
			return Math.Cosh(radians);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00008A79 File Offset: 0x00006C79
		public static double Acos(double radians)
		{
			return Math.Acos(radians);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00008A81 File Offset: 0x00006C81
		public static double Tan(double radians)
		{
			return Math.Tan(radians);
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00008A89 File Offset: 0x00006C89
		public static double Tanh(double radians)
		{
			return Math.Tanh(radians);
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00008A91 File Offset: 0x00006C91
		public static double Atan(double radians)
		{
			return Math.Atan(radians);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00008A99 File Offset: 0x00006C99
		public static double Atan2(double y, double x)
		{
			return Math.Atan2(y, x);
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00008AA2 File Offset: 0x00006CA2
		public static long BigMul(int a, int b)
		{
			return Math.BigMul(a, b);
		}

		// Token: 0x0600023E RID: 574 RVA: 0x00008AAB File Offset: 0x00006CAB
		public static double Sqrt(double n)
		{
			return Math.Sqrt(n);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00008AB3 File Offset: 0x00006CB3
		public static double Pow(double x, double y)
		{
			return Math.Pow(x, y);
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00008ABC File Offset: 0x00006CBC
		public static decimal Ceiling(decimal n)
		{
			return Math.Ceiling(n);
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00008AC4 File Offset: 0x00006CC4
		public static double Ceiling(double n)
		{
			return Math.Ceiling(n);
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00008ACC File Offset: 0x00006CCC
		public static decimal Floor(decimal n)
		{
			return Math.Floor(n);
		}

		// Token: 0x06000243 RID: 579 RVA: 0x00008AD4 File Offset: 0x00006CD4
		public static double Floor(double n)
		{
			return Math.Floor(n);
		}

		// Token: 0x06000244 RID: 580 RVA: 0x00008ADC File Offset: 0x00006CDC
		public static int DivRem(int a, int b, out int result)
		{
			return Math.DivRem(a, b, ref result);
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00008AE6 File Offset: 0x00006CE6
		public static long DivRem(long a, long b, out long result)
		{
			return Math.DivRem(a, b, ref result);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00008AF0 File Offset: 0x00006CF0
		public static double Log(double n)
		{
			return Math.Log(n);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00008AF8 File Offset: 0x00006CF8
		public static double Log(double n, double newBase)
		{
			return Math.Log(n, newBase);
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00008B01 File Offset: 0x00006D01
		public static double Log10(double n)
		{
			return Math.Log10(n);
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00008B09 File Offset: 0x00006D09
		public static double Log2(double n)
		{
			return Math.Log(n, 2.0);
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00008B1A File Offset: 0x00006D1A
		public static double Exp(double n)
		{
			return Math.Exp(n);
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00008B22 File Offset: 0x00006D22
		public static double IEEERemainder(double a, double b)
		{
			return Math.IEEERemainder(a, b);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x00008B2B File Offset: 0x00006D2B
		public static byte Max(byte a, byte b)
		{
			return Math.Max(a, b);
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00008B34 File Offset: 0x00006D34
		public static sbyte Max(sbyte a, sbyte b)
		{
			return Math.Max(a, b);
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00008B3D File Offset: 0x00006D3D
		public static short Max(short a, short b)
		{
			return Math.Max(a, b);
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00008B46 File Offset: 0x00006D46
		public static ushort Max(ushort a, ushort b)
		{
			return Math.Max(a, b);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00008B4F File Offset: 0x00006D4F
		public static decimal Max(decimal a, decimal b)
		{
			return Math.Max(a, b);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00008B58 File Offset: 0x00006D58
		public static int Max(int a, int b)
		{
			return Math.Max(a, b);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00008B61 File Offset: 0x00006D61
		public static uint Max(uint a, uint b)
		{
			return Math.Max(a, b);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00008B6A File Offset: 0x00006D6A
		public static float Max(float a, float b)
		{
			return Math.Max(a, b);
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00008B73 File Offset: 0x00006D73
		public static long Max(long a, long b)
		{
			return Math.Max(a, b);
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00008B7C File Offset: 0x00006D7C
		public static ulong Max(ulong a, ulong b)
		{
			return Math.Max(a, b);
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00008B85 File Offset: 0x00006D85
		public static byte Min(byte a, byte b)
		{
			return Math.Min(a, b);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x00008B8E File Offset: 0x00006D8E
		public static sbyte Min(sbyte a, sbyte b)
		{
			return Math.Min(a, b);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00008B97 File Offset: 0x00006D97
		public static short Min(short a, short b)
		{
			return Math.Min(a, b);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00008BA0 File Offset: 0x00006DA0
		public static ushort Min(ushort a, ushort b)
		{
			return Math.Min(a, b);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x00008BA9 File Offset: 0x00006DA9
		public static decimal Min(decimal a, decimal b)
		{
			return Math.Min(a, b);
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00008BB2 File Offset: 0x00006DB2
		public static int Min(int a, int b)
		{
			return Math.Min(a, b);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00008BBB File Offset: 0x00006DBB
		public static uint Min(uint a, uint b)
		{
			return Math.Min(a, b);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00008BC4 File Offset: 0x00006DC4
		public static float Min(float a, float b)
		{
			return Math.Min(a, b);
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00008BCD File Offset: 0x00006DCD
		public static double Min(double a, double b)
		{
			return Math.Min(a, b);
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00008BD6 File Offset: 0x00006DD6
		public static long Min(long a, long b)
		{
			return Math.Min(a, b);
		}

		// Token: 0x06000260 RID: 608 RVA: 0x00008BDF File Offset: 0x00006DDF
		public static ulong Min(ulong a, ulong b)
		{
			return Math.Min(a, b);
		}

		// Token: 0x06000261 RID: 609 RVA: 0x00008BE8 File Offset: 0x00006DE8
		public static decimal Round(decimal d, int digits, MidpointRounding mode)
		{
			return Math.Round(d, digits, mode);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00008BF2 File Offset: 0x00006DF2
		public static double Round(double d, int digits, MidpointRounding mode)
		{
			return Math.Round(d, digits, mode);
		}

		// Token: 0x06000263 RID: 611 RVA: 0x00008BFC File Offset: 0x00006DFC
		public static decimal Round(decimal d, MidpointRounding mode)
		{
			return Math.Round(d, mode);
		}

		// Token: 0x06000264 RID: 612 RVA: 0x00008C05 File Offset: 0x00006E05
		public static double Round(double d, MidpointRounding mode)
		{
			return Math.Round(d, mode);
		}

		// Token: 0x06000265 RID: 613 RVA: 0x00008C0E File Offset: 0x00006E0E
		public static decimal Round(decimal d, int digits)
		{
			return Math.Round(d, digits);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x00008C17 File Offset: 0x00006E17
		public static double Round(double d, int digits)
		{
			return Math.Round(d, digits);
		}

		// Token: 0x06000267 RID: 615 RVA: 0x00008C20 File Offset: 0x00006E20
		public static decimal Round(decimal d)
		{
			return Math.Round(d);
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00008C28 File Offset: 0x00006E28
		public static double Round(double d)
		{
			return Math.Round(d);
		}

		// Token: 0x06000269 RID: 617 RVA: 0x00008C30 File Offset: 0x00006E30
		public static decimal Truncate(decimal d)
		{
			return Math.Truncate(d);
		}

		// Token: 0x0600026A RID: 618 RVA: 0x00008C38 File Offset: 0x00006E38
		public static double Truncate(double d)
		{
			return Math.Truncate(d);
		}

		// Token: 0x0600026B RID: 619 RVA: 0x00008C40 File Offset: 0x00006E40
		public static int Sign(sbyte d)
		{
			return Math.Sign(d);
		}

		// Token: 0x0600026C RID: 620 RVA: 0x00008C48 File Offset: 0x00006E48
		public static int Sign(short d)
		{
			return Math.Sign(d);
		}

		// Token: 0x0600026D RID: 621 RVA: 0x00008C50 File Offset: 0x00006E50
		public static int Sign(int d)
		{
			return Math.Sign(d);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x00008C58 File Offset: 0x00006E58
		public static int Sign(float d)
		{
			return Math.Sign(d);
		}

		// Token: 0x0600026F RID: 623 RVA: 0x00008C60 File Offset: 0x00006E60
		public static int Sign(decimal d)
		{
			return Math.Sign(d);
		}

		// Token: 0x06000270 RID: 624 RVA: 0x00008C68 File Offset: 0x00006E68
		public static int Sign(double d)
		{
			return Math.Sign(d);
		}

		// Token: 0x06000271 RID: 625 RVA: 0x00008C70 File Offset: 0x00006E70
		public static int Sign(long d)
		{
			return Math.Sign(d);
		}

		// Token: 0x06000272 RID: 626 RVA: 0x00008C78 File Offset: 0x00006E78
		public static long NextPowerOfTwo(long n)
		{
			if (n < 0L)
			{
				throw new ArgumentOutOfRangeException("n", "Must be positive.");
			}
			return (long)Math.Pow(2.0, Math.Ceiling(Math.Log((double)n, 2.0)));
		}

		// Token: 0x06000273 RID: 627 RVA: 0x00008CB3 File Offset: 0x00006EB3
		public static int NextPowerOfTwo(int n)
		{
			if (n < 0)
			{
				throw new ArgumentOutOfRangeException("n", "Must be positive.");
			}
			return (int)Math.Pow(2.0, Math.Ceiling(Math.Log((double)n, 2.0)));
		}

		// Token: 0x06000274 RID: 628 RVA: 0x00008CED File Offset: 0x00006EED
		public static float NextPowerOfTwo(float n)
		{
			if (n < 0f)
			{
				throw new ArgumentOutOfRangeException("n", "Must be positive.");
			}
			return MathF.Pow(2f, MathF.Ceiling(MathF.Log(n, 2f)));
		}

		// Token: 0x06000275 RID: 629 RVA: 0x00008D21 File Offset: 0x00006F21
		public static double NextPowerOfTwo(double n)
		{
			if (n < 0.0)
			{
				throw new ArgumentOutOfRangeException("n", "Must be positive.");
			}
			return Math.Pow(2.0, Math.Ceiling(Math.Log(n, 2.0)));
		}

		// Token: 0x06000276 RID: 630 RVA: 0x00008D64 File Offset: 0x00006F64
		public static long Factorial(int n)
		{
			long num = 1L;
			while (n > 1)
			{
				num *= (long)n;
				n--;
			}
			return num;
		}

		// Token: 0x06000277 RID: 631 RVA: 0x00008D85 File Offset: 0x00006F85
		public static long BinomialCoefficient(int n, int k)
		{
			return MathHelper.Factorial(n) / (MathHelper.Factorial(k) * MathHelper.Factorial(n - k));
		}

		// Token: 0x06000278 RID: 632 RVA: 0x00008DA0 File Offset: 0x00006FA0
		public unsafe static float InverseSqrtFast(float x)
		{
			float num = 0.5f * x;
			int num2 = *(int*)(&x);
			num2 = 1597463174 - (num2 >> 1);
			x = *(float*)(&num2);
			x *= 1.5f - num * x * x;
			return x;
		}

		// Token: 0x06000279 RID: 633 RVA: 0x00008DDC File Offset: 0x00006FDC
		public unsafe static double InverseSqrtFast(double x)
		{
			double num = 0.5 * x;
			long num2 = *(long*)(&x);
			num2 = 6910469410427058089L - (num2 >> 1);
			x = *(double*)(&num2);
			x *= 1.5 - num * x * x;
			return x;
		}

		// Token: 0x0600027A RID: 634 RVA: 0x00008E22 File Offset: 0x00007022
		public static float DegreesToRadians(float degrees)
		{
			return degrees * 0.017453292f;
		}

		// Token: 0x0600027B RID: 635 RVA: 0x00008E2B File Offset: 0x0000702B
		public static float RadiansToDegrees(float radians)
		{
			return radians * 57.295776f;
		}

		// Token: 0x0600027C RID: 636 RVA: 0x00008E34 File Offset: 0x00007034
		public static double DegreesToRadians(double degrees)
		{
			return degrees * 0.017453292519943295;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x00008E41 File Offset: 0x00007041
		public static double RadiansToDegrees(double radians)
		{
			return radians * 57.29577951308232;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x00008E50 File Offset: 0x00007050
		public static void Swap<T>(ref T a, ref T b)
		{
			T t = b;
			T t2 = a;
			a = t;
			b = t2;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x00008E79 File Offset: 0x00007079
		public static int Clamp(int n, int min, int max)
		{
			return Math.Max(Math.Min(n, max), min);
		}

		// Token: 0x06000280 RID: 640 RVA: 0x00008E88 File Offset: 0x00007088
		public static float Clamp(float n, float min, float max)
		{
			return Math.Max(Math.Min(n, max), min);
		}

		// Token: 0x06000281 RID: 641 RVA: 0x00008E97 File Offset: 0x00007097
		public static double Clamp(double n, double min, double max)
		{
			return Math.Max(Math.Min(n, max), min);
		}

		// Token: 0x06000282 RID: 642 RVA: 0x00008EA8 File Offset: 0x000070A8
		public static int MapRange(int value, int valueMin, int valueMax, int resultMin, int resultMax)
		{
			int num = valueMax - valueMin;
			int num2 = resultMax - resultMin;
			return resultMin + num2 * ((value - valueMin) / num);
		}

		// Token: 0x06000283 RID: 643 RVA: 0x00008EC8 File Offset: 0x000070C8
		public static float MapRange(float value, float valueMin, float valueMax, float resultMin, float resultMax)
		{
			float num = valueMax - valueMin;
			float num2 = resultMax - resultMin;
			return resultMin + num2 * ((value - valueMin) / num);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00008EE8 File Offset: 0x000070E8
		public static double MapRange(double value, double valueMin, double valueMax, double resultMin, double resultMax)
		{
			double num = valueMax - valueMin;
			double num2 = resultMax - resultMin;
			return resultMin + num2 * ((value - valueMin) / num);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00008F08 File Offset: 0x00007108
		public static bool ApproximatelyEqual(float a, float b, int maxDeltaBits)
		{
			long num = (long)BitConverter.SingleToInt32Bits(a);
			if (num < 0L)
			{
				num = -2147483648L - num;
			}
			long num2 = (long)BitConverter.SingleToInt32Bits(b);
			if (num2 < 0L)
			{
				num2 = -2147483648L - num2;
			}
			return Math.Abs(num - num2) <= 1L << (maxDeltaBits & 31);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00008F58 File Offset: 0x00007158
		public static bool ApproximatelyEqualEpsilon(double a, double b, double epsilon)
		{
			double num = Math.Abs(a);
			double num2 = Math.Abs(b);
			double num3 = Math.Abs(a - b);
			if (a == b)
			{
				return true;
			}
			if (a == 0.0 || b == 0.0 || num3 < 2.2250738585072014E-308)
			{
				return num3 < epsilon * 2.2250738585072014E-308;
			}
			return num3 / Math.Min(num + num2, double.MaxValue) < epsilon;
		}

		// Token: 0x06000287 RID: 647 RVA: 0x00008FCC File Offset: 0x000071CC
		public static bool ApproximatelyEqualEpsilon(float a, float b, float epsilon)
		{
			float num = Math.Abs(a);
			float num2 = Math.Abs(b);
			float num3 = Math.Abs(a - b);
			if (a == b)
			{
				return true;
			}
			if (a == 0f || b == 0f || num3 < 1.1754944E-38f)
			{
				return num3 < epsilon * 1.1754944E-38f;
			}
			return num3 / Math.Min(num + num2, float.MaxValue) < epsilon;
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0000902B File Offset: 0x0000722B
		public static bool ApproximatelyEquivalent(float a, float b, float tolerance)
		{
			return a == b || Math.Abs(a - b) <= tolerance;
		}

		// Token: 0x06000289 RID: 649 RVA: 0x00009041 File Offset: 0x00007241
		public static bool ApproximatelyEquivalent(double a, double b, double tolerance)
		{
			return a == b || Math.Abs(a - b) <= tolerance;
		}

		// Token: 0x0600028A RID: 650 RVA: 0x00009057 File Offset: 0x00007257
		public static float Lerp(float start, float end, float t)
		{
			return start + t * (end - start);
		}

		// Token: 0x0600028B RID: 651 RVA: 0x00009060 File Offset: 0x00007260
		public static double Lerp(double start, double end, double t)
		{
			return start + t * (end - start);
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00009069 File Offset: 0x00007269
		public static float Elerp(float start, float end, float t)
		{
			return start + MathF.Pow(end / start, t);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00009076 File Offset: 0x00007276
		public static double Elerp(double start, double end, double t)
		{
			return start + Math.Pow(end / start, t);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00009083 File Offset: 0x00007283
		public static float NormalizeAngle(float angle)
		{
			angle = MathHelper.ClampAngle(angle);
			if (angle > 180f)
			{
				angle -= 360f;
			}
			return angle;
		}

		// Token: 0x0600028F RID: 655 RVA: 0x0000909F File Offset: 0x0000729F
		public static double NormalizeAngle(double angle)
		{
			angle = MathHelper.ClampAngle(angle);
			if (angle > 180.0)
			{
				angle -= 360.0;
			}
			return angle;
		}

		// Token: 0x06000290 RID: 656 RVA: 0x000090C3 File Offset: 0x000072C3
		public static float NormalizeRadians(float angle)
		{
			angle = MathHelper.ClampRadians(angle);
			if (angle > 3.1415927f)
			{
				angle -= 6.2831855f;
			}
			return angle;
		}

		// Token: 0x06000291 RID: 657 RVA: 0x000090DF File Offset: 0x000072DF
		public static double NormalizeRadians(double angle)
		{
			angle = MathHelper.ClampRadians(angle);
			if (angle > 3.1415927410125732)
			{
				angle -= 6.2831854820251465;
			}
			return angle;
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00009103 File Offset: 0x00007303
		public static float ClampAngle(float angle)
		{
			angle %= 360f;
			if (angle < 0f)
			{
				angle += 360f;
			}
			return angle;
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00009120 File Offset: 0x00007320
		public static double ClampAngle(double angle)
		{
			angle %= 360.0;
			if (angle < 0.0)
			{
				angle += 360.0;
			}
			return angle;
		}

		// Token: 0x06000294 RID: 660 RVA: 0x00009149 File Offset: 0x00007349
		public static float ClampRadians(float angle)
		{
			angle %= 6.2831855f;
			if (angle < 0f)
			{
				angle += 6.2831855f;
			}
			return angle;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00009166 File Offset: 0x00007366
		public static double ClampRadians(double angle)
		{
			angle %= 6.283185307179586;
			if (angle < 0.0)
			{
				angle += 6.283185307179586;
			}
			return angle;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00009190 File Offset: 0x00007390
		internal static string GetListSeparator(IFormatProvider formatProvider)
		{
			CultureInfo cultureInfo = formatProvider as CultureInfo;
			if (cultureInfo != null)
			{
				return cultureInfo.TextInfo.ListSeparator;
			}
			TextInfo textInfo = ((formatProvider != null) ? formatProvider.GetFormat(typeof(TextInfo)) : null) as TextInfo;
			if (textInfo != null)
			{
				return textInfo.ListSeparator;
			}
			return CultureInfo.CurrentCulture.TextInfo.ListSeparator;
		}

		// Token: 0x06000297 RID: 663 RVA: 0x000091E8 File Offset: 0x000073E8
		[DoesNotReturn]
		internal static void ThrowOutOfRangeException(string message)
		{
			throw new IndexOutOfRangeException(message);
		}

		// Token: 0x0400002A RID: 42
		public const float Pi = 3.1415927f;

		// Token: 0x0400002B RID: 43
		public const float PiOver2 = 1.5707964f;

		// Token: 0x0400002C RID: 44
		public const float PiOver3 = 1.0471976f;

		// Token: 0x0400002D RID: 45
		public const float PiOver4 = 0.7853982f;

		// Token: 0x0400002E RID: 46
		public const float PiOver6 = 0.5235988f;

		// Token: 0x0400002F RID: 47
		public const float TwoPi = 6.2831855f;

		// Token: 0x04000030 RID: 48
		public const float ThreePiOver2 = 4.712389f;

		// Token: 0x04000031 RID: 49
		public const float E = 2.7182817f;

		// Token: 0x04000032 RID: 50
		public const float Log10E = 0.4342945f;

		// Token: 0x04000033 RID: 51
		public const float Log2E = 1.442695f;

		// Token: 0x04000034 RID: 52
		public const float RadToDeg = 57.295776f;

		// Token: 0x04000035 RID: 53
		public const float DegToRad = 0.017453292f;

		// Token: 0x04000036 RID: 54
		public const double RadToDegd = 57.29577951308232;

		// Token: 0x04000037 RID: 55
		public const double DegToRadd = 0.017453292519943295;
	}
}
