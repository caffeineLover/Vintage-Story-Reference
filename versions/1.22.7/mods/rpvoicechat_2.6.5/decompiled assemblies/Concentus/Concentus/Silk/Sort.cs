using System;

namespace Concentus.Silk
{
	// Token: 0x0200003A RID: 58
	internal static class Sort
	{
		// Token: 0x0600011E RID: 286 RVA: 0x00011520 File Offset: 0x0000F720
		internal static void silk_insertion_sort_increasing(int[] a, int[] idx, int L, int K)
		{
			for (int i = 0; i < K; i++)
			{
				idx[i] = i;
			}
			for (int i = 1; i < K; i++)
			{
				int num = a[i];
				int num2 = i - 1;
				while (num2 >= 0 && num < a[num2])
				{
					a[num2 + 1] = a[num2];
					idx[num2 + 1] = idx[num2];
					num2--;
				}
				a[num2 + 1] = num;
				idx[num2 + 1] = i;
			}
			for (int i = K; i < L; i++)
			{
				int num = a[i];
				if (num < a[K - 1])
				{
					int num2 = K - 2;
					while (num2 >= 0 && num < a[num2])
					{
						a[num2 + 1] = a[num2];
						idx[num2 + 1] = idx[num2];
						num2--;
					}
					a[num2 + 1] = num;
					idx[num2 + 1] = i;
				}
			}
		}

		// Token: 0x0600011F RID: 287 RVA: 0x000115C8 File Offset: 0x0000F7C8
		internal static void silk_insertion_sort_increasing_all_values_int16(short[] a, int L)
		{
			for (int i = 1; i < L; i++)
			{
				short num = a[i];
				int num2 = i - 1;
				while (num2 >= 0 && num < a[num2])
				{
					a[num2 + 1] = a[num2];
					num2--;
				}
				a[num2 + 1] = num;
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00011608 File Offset: 0x0000F808
		internal static void silk_insertion_sort_decreasing_int16(short[] a, int[] idx, int L, int K)
		{
			for (int i = 0; i < K; i++)
			{
				idx[i] = i;
			}
			for (int i = 1; i < K; i++)
			{
				short num = a[i];
				int num2 = i - 1;
				while (num2 >= 0 && num > a[num2])
				{
					a[num2 + 1] = a[num2];
					idx[num2 + 1] = idx[num2];
					num2--;
				}
				a[num2 + 1] = num;
				idx[num2 + 1] = i;
			}
			for (int i = K; i < L; i++)
			{
				short num = a[i];
				if (num > a[K - 1])
				{
					int num2 = K - 2;
					while (num2 >= 0 && num > a[num2])
					{
						a[num2 + 1] = a[num2];
						idx[num2 + 1] = idx[num2];
						num2--;
					}
					a[num2 + 1] = num;
					idx[num2 + 1] = i;
				}
			}
		}
	}
}
