using System;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
	// Token: 0x02000008 RID: 8
	internal static class HashHelpers
	{
		// Token: 0x06000027 RID: 39 RVA: 0x00002994 File Offset: 0x00000B94
		private static bool IsPrime(int candidate)
		{
			if ((candidate & 1) != 0)
			{
				int num = (int)Math.Sqrt((double)candidate);
				for (int i = 3; i <= num; i += 2)
				{
					if (candidate % i == 0)
					{
						return false;
					}
				}
				return true;
			}
			return candidate == 2;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000029C8 File Offset: 0x00000BC8
		public static int GetPrime(int min)
		{
			foreach (int num in HashHelpers.primes)
			{
				if (num >= min)
				{
					return num;
				}
			}
			for (int j = min | 1; j < 2147483647; j += 2)
			{
				if (HashHelpers.IsPrime(j) && (j - 1) % 101 != 0)
				{
					return j;
				}
			}
			return min;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002A18 File Offset: 0x00000C18
		public static int ExpandPrime(int oldSize)
		{
			int num = 2 * oldSize;
			if (num > 2147483587 && 2147483587 > oldSize)
			{
				return 2147483587;
			}
			return HashHelpers.GetPrime(num);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002A45 File Offset: 0x00000C45
		public static ulong GetFastModMultiplier(uint divisor)
		{
			return ulong.MaxValue / (ulong)divisor + 1UL;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002A4F File Offset: 0x00000C4F
		[MethodImpl(256)]
		public static uint FastMod(uint value, uint divisor, ulong multiplier)
		{
			return (uint)(((multiplier * (ulong)value >> 32) + 1UL) * (ulong)divisor >> 32);
		}

		// Token: 0x0400001E RID: 30
		private const int MaxPrimeArrayLength = 2147483587;

		// Token: 0x0400001F RID: 31
		private const int HashPrime = 101;

		// Token: 0x04000020 RID: 32
		[Nullable(1)]
		private static readonly int[] primes = new int[]
		{
			3,
			7,
			11,
			17,
			23,
			29,
			37,
			47,
			59,
			71,
			89,
			107,
			131,
			163,
			197,
			239,
			293,
			353,
			431,
			521,
			631,
			761,
			919,
			1103,
			1327,
			1597,
			1931,
			2333,
			2801,
			3371,
			4049,
			4861,
			5839,
			7013,
			8419,
			10103,
			12143,
			14591,
			17519,
			21023,
			25229,
			30293,
			36353,
			43627,
			52361,
			62851,
			75431,
			90523,
			108631,
			130363,
			156437,
			187751,
			225307,
			270371,
			324449,
			389357,
			467237,
			560689,
			672827,
			807403,
			968897,
			1162687,
			1395263,
			1674319,
			2009191,
			2411033,
			2893249,
			3471899,
			4166287,
			4999559,
			5999471,
			7199369
		};
	}
}
