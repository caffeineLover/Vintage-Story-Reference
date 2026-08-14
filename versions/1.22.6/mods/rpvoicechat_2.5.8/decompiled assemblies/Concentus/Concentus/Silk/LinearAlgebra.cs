using System;
using Concentus.Common;

namespace Concentus.Silk
{
	// Token: 0x02000029 RID: 41
	internal static class LinearAlgebra
	{
		// Token: 0x060000DB RID: 219 RVA: 0x0000B47C File Offset: 0x0000967C
		internal static void silk_solve_LDL(int[] A, int A_ptr, int M, int[] b, int[] x_Q16)
		{
			int[] l_Q = new int[M * M];
			int[] array = new int[16];
			int[] inv_D = new int[32];
			LinearAlgebra.silk_LDL_factorize(A, A_ptr, M, l_Q, inv_D);
			LinearAlgebra.silk_LS_SolveFirst(l_Q, M, b, array);
			LinearAlgebra.silk_LS_divide_Q16(array, inv_D, M);
			LinearAlgebra.silk_LS_SolveLast(l_Q, M, array, x_Q16);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000B4C8 File Offset: 0x000096C8
		private static void silk_LDL_factorize(int[] A, int A_ptr, int M, int[] L_Q16, int[] inv_D)
		{
			int[] array = new int[M];
			int[] array2 = new int[M];
			int num = 1;
			int num2 = Inlines.silk_max_32(Inlines.silk_SMMUL(Inlines.silk_ADD_SAT32(A[A_ptr], A[A_ptr + Inlines.silk_SMULBB(M, M) - 1]), 21475), 512);
			int num3 = 0;
			while (num3 < M && num == 1)
			{
				num = 0;
				for (int i = 0; i < M; i++)
				{
					int num4 = Inlines.MatrixGetPointer(i, 0, M);
					int num5 = 0;
					for (int j = 0; j < i; j++)
					{
						array[j] = Inlines.silk_SMULWW(array2[j], L_Q16[num4 + j]);
						num5 = Inlines.silk_SMLAWW(num5, array[j], L_Q16[num4 + j]);
					}
					num5 = Inlines.silk_SUB32(Inlines.MatrixGet<int>(A, A_ptr, i, i, M), num5);
					if (num5 < num2)
					{
						num5 = Inlines.silk_SUB32(Inlines.silk_SMULBB(num3 + 1, num2), num5);
						for (int j = 0; j < M; j++)
						{
							Inlines.MatrixSet<int>(A, A_ptr, j, j, M, Inlines.silk_ADD32(Inlines.MatrixGet<int>(A, A_ptr, j, j, M), num5));
						}
						num = 1;
						break;
					}
					array2[i] = num5;
					int num6 = Inlines.silk_INVERSE32_varQ(num5, 36);
					int b = Inlines.silk_LSHIFT(num6, 4);
					int num7 = Inlines.silk_SMULWW(Inlines.silk_SUB32(16777216, Inlines.silk_SMULWW(num5, b)), b);
					inv_D[i * 2] = num6;
					inv_D[i * 2 + 1] = num7;
					Inlines.MatrixSet<int>(L_Q16, i, i, M, 65536);
					num4 = Inlines.MatrixGetPointer(i, 0, M) + A_ptr;
					int num8 = Inlines.MatrixGetPointer(i + 1, 0, M);
					for (int j = i + 1; j < M; j++)
					{
						num5 = 0;
						for (int k = 0; k < i; k++)
						{
							num5 = Inlines.silk_SMLAWW(num5, array[k], L_Q16[num8 + k]);
						}
						num5 = Inlines.silk_SUB32(A[num4 + j], num5);
						Inlines.MatrixSet<int>(L_Q16, j, i, M, Inlines.silk_ADD32(Inlines.silk_SMMUL(num5, num7), Inlines.silk_RSHIFT(Inlines.silk_SMULWW(num5, num6), 4)));
						num8 += M;
					}
				}
				num3++;
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000B6C4 File Offset: 0x000098C4
		private static void silk_LS_divide_Q16(int[] T, int[] inv_D, int M)
		{
			for (int i = 0; i < M; i++)
			{
				int b = inv_D[i * 2];
				int b2 = inv_D[i * 2 + 1];
				int a = T[i];
				T[i] = Inlines.silk_ADD32(Inlines.silk_SMMUL(a, b2), Inlines.silk_RSHIFT(Inlines.silk_SMULWW(a, b), 4));
			}
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000B70C File Offset: 0x0000990C
		private static void silk_LS_SolveFirst(int[] L_Q16, int M, int[] b, int[] x_Q16)
		{
			for (int i = 0; i < M; i++)
			{
				int num = Inlines.MatrixGetPointer(i, 0, M);
				int num2 = 0;
				for (int j = 0; j < i; j++)
				{
					num2 = Inlines.silk_SMLAWW(num2, L_Q16[num + j], x_Q16[j]);
				}
				x_Q16[i] = Inlines.silk_SUB32(b[i], num2);
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000B758 File Offset: 0x00009958
		private static void silk_LS_SolveLast(int[] L_Q16, int M, int[] b, int[] x_Q16)
		{
			for (int i = M - 1; i >= 0; i--)
			{
				int num = Inlines.MatrixGetPointer(0, i, M);
				int num2 = 0;
				for (int j = M - 1; j > i; j--)
				{
					num2 = Inlines.silk_SMLAWW(num2, L_Q16[num + Inlines.silk_SMULBB(j, M)], x_Q16[j]);
				}
				x_Q16[i] = Inlines.silk_SUB32(b[i], num2);
			}
		}
	}
}
