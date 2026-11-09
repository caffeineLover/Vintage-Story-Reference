using System;

namespace UglyToad.PdfPig.Geometry.ClipperLibrary
{
	// Token: 0x02000197 RID: 407
	internal struct ClipperInt128
	{
		// Token: 0x06000A02 RID: 2562 RVA: 0x00032F33 File Offset: 0x00031133
		public ClipperInt128(long _lo)
		{
			this.lo = (ulong)_lo;
			if (_lo < 0L)
			{
				this.hi = -1L;
				return;
			}
			this.hi = 0L;
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x00032F52 File Offset: 0x00031152
		public ClipperInt128(long _hi, ulong _lo)
		{
			this.lo = _lo;
			this.hi = _hi;
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x00032F62 File Offset: 0x00031162
		public ClipperInt128(ClipperInt128 val)
		{
			this.hi = val.hi;
			this.lo = val.lo;
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x00032F7C File Offset: 0x0003117C
		public bool IsNegative()
		{
			return this.hi < 0L;
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x00032F88 File Offset: 0x00031188
		public static bool operator ==(ClipperInt128 val1, ClipperInt128 val2)
		{
			return val1 == val2 || (val1.hi == val2.hi && val1.lo == val2.lo);
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x00032FB8 File Offset: 0x000311B8
		public static bool operator !=(ClipperInt128 val1, ClipperInt128 val2)
		{
			return !(val1 == val2);
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x00032FC4 File Offset: 0x000311C4
		public override bool Equals(object obj)
		{
			if (obj is ClipperInt128)
			{
				ClipperInt128 clipperInt = (ClipperInt128)obj;
				return clipperInt.hi == this.hi && clipperInt.lo == this.lo;
			}
			return false;
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x00033002 File Offset: 0x00031202
		public override int GetHashCode()
		{
			return this.hi.GetHashCode() ^ this.lo.GetHashCode();
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0003301B File Offset: 0x0003121B
		public static bool operator >(ClipperInt128 val1, ClipperInt128 val2)
		{
			if (val1.hi != val2.hi)
			{
				return val1.hi > val2.hi;
			}
			return val1.lo > val2.lo;
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x00033048 File Offset: 0x00031248
		public static bool operator <(ClipperInt128 val1, ClipperInt128 val2)
		{
			if (val1.hi != val2.hi)
			{
				return val1.hi < val2.hi;
			}
			return val1.lo < val2.lo;
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x00033075 File Offset: 0x00031275
		public static ClipperInt128 operator +(ClipperInt128 lhs, ClipperInt128 rhs)
		{
			lhs.hi += rhs.hi;
			lhs.lo += rhs.lo;
			if (lhs.lo < rhs.lo)
			{
				lhs.hi += 1L;
			}
			return lhs;
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x000330B5 File Offset: 0x000312B5
		public static ClipperInt128 operator -(ClipperInt128 lhs, ClipperInt128 rhs)
		{
			return lhs + -rhs;
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x000330C3 File Offset: 0x000312C3
		public static ClipperInt128 operator -(ClipperInt128 val)
		{
			if (val.lo == 0UL)
			{
				return new ClipperInt128(-val.hi, 0UL);
			}
			return new ClipperInt128(~val.hi, ~val.lo + 1UL);
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x000330F4 File Offset: 0x000312F4
		public static explicit operator double(ClipperInt128 val)
		{
			if (val.hi >= 0L)
			{
				return val.lo + (double)val.hi * 1.8446744073709552E+19;
			}
			if (val.lo == 0UL)
			{
				return (double)val.hi * 1.8446744073709552E+19;
			}
			return -(~val.lo + (double)(~(double)val.hi) * 1.8446744073709552E+19);
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x00033160 File Offset: 0x00031360
		public static ClipperInt128 Int128Mul(long lhs, long rhs)
		{
			bool flag = lhs < 0L != rhs < 0L;
			if (lhs < 0L)
			{
				lhs = -lhs;
			}
			if (rhs < 0L)
			{
				rhs = -rhs;
			}
			ulong num = (ulong)lhs >> 32;
			ulong num2 = (ulong)(lhs & (long)((ulong)-1));
			ulong num3 = (ulong)rhs >> 32;
			ulong num4 = (ulong)(rhs & (long)((ulong)-1));
			ulong num5 = num * num3;
			ulong num6 = num2 * num4;
			ulong num7 = num * num4 + num2 * num3;
			long num8 = (long)(num5 + (num7 >> 32));
			ulong num9 = (num7 << 32) + num6;
			if (num9 < num6)
			{
				num8 += 1L;
			}
			ClipperInt128 clipperInt = new ClipperInt128(num8, num9);
			if (!flag)
			{
				return clipperInt;
			}
			return -clipperInt;
		}

		// Token: 0x040004BF RID: 1215
		private long hi;

		// Token: 0x040004C0 RID: 1216
		private ulong lo;
	}
}
