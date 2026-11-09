using System;
using Concentus.Celt.Structs;
using Concentus.Common;
using Concentus.Common.CPlusPlus;

namespace Concentus.Celt
{
	// Token: 0x0200008E RID: 142
	internal static class QuantizeBands
	{
		// Token: 0x060004EB RID: 1259 RVA: 0x00030890 File Offset: 0x0002EA90
		internal static int loss_distortion(int[][] eBands, int[][] oldEBands, int start, int end, int len, int C)
		{
			int num = 0;
			int num2 = 0;
			do
			{
				for (int i = start; i < end; i++)
				{
					int num3 = Inlines.SUB16(Inlines.SHR16(eBands[num2][i], 3), Inlines.SHR16(oldEBands[num2][i], 3));
					num = Inlines.MAC16_16(num, num3, num3);
				}
			}
			while (++num2 < C);
			return Inlines.MIN32(200, Inlines.SHR32(num, 14));
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x000308F0 File Offset: 0x0002EAF0
		internal static int quant_coarse_energy_impl(CeltMode m, int start, int end, int[][] eBands, int[][] oldEBands, int budget, int tell, byte[] prob_model, int[][] error, EntropyCoder enc, Span<byte> encodedData, int C, int LM, int intra, int max_decay, int lfe)
		{
			int num = 0;
			int[] array = new int[2];
			if (tell + 3 <= budget)
			{
				enc.enc_bit_logp(encodedData, intra, 3U);
			}
			int a;
			int a2;
			if (intra != 0)
			{
				a = 0;
				a2 = QuantizeBands.beta_intra;
			}
			else
			{
				a2 = QuantizeBands.beta_coef[LM];
				a = QuantizeBands.pred_coef[LM];
			}
			for (int i = start; i < end; i++)
			{
				int num2 = 0;
				do
				{
					int num3 = eBands[num2][i];
					int b = Inlines.MAX16(-9216, oldEBands[num2][i]);
					int num4 = Inlines.SHL32(Inlines.EXTEND32(num3), 7) - Inlines.PSHR32(Inlines.MULT16_16(a, b), 8) - array[num2];
					int num5 = num4 + 65536 >> 17;
					int num6 = (int)Inlines.EXTRACT16(Inlines.MAX32(-28672, Inlines.SUB32(oldEBands[num2][i], max_decay)));
					if (num5 < 0 && num3 < num6)
					{
						num5 += Inlines.SHR16(Inlines.SUB16(num6, num3), 10);
						if (num5 > 0)
						{
							num5 = 0;
						}
					}
					int num7 = num5;
					tell = enc.tell();
					int num8 = budget - tell - 3 * C * (end - i);
					if (i != start && num8 < 30)
					{
						if (num8 < 24)
						{
							num5 = Inlines.IMIN(1, num5);
						}
						if (num8 < 16)
						{
							num5 = Inlines.IMAX(-1, num5);
						}
					}
					if (lfe != 0 && i >= 2)
					{
						num5 = Inlines.IMIN(num5, 0);
					}
					if (budget - tell >= 15)
					{
						int num9 = 2 * Inlines.IMIN(i, 20);
						Laplace.ec_laplace_encode(enc, encodedData, ref num5, (uint)((uint)prob_model[num9] << 7), (int)prob_model[num9 + 1] << 6);
					}
					else if (budget - tell >= 2)
					{
						num5 = Inlines.IMAX(-1, Inlines.IMIN(num5, 1));
						enc.enc_icdf(encodedData, 2 * num5 ^ ((false - (num5 < 0)) ? 1 : 0), QuantizeBands.small_energy_icdf, 2U);
					}
					else if (budget - tell >= 1)
					{
						num5 = Inlines.IMIN(0, num5);
						enc.enc_bit_logp(encodedData, -num5, 1U);
					}
					else
					{
						num5 = -1;
					}
					error[num2][i] = Inlines.PSHR32(num4, 7) - Inlines.SHL16(num5, 10);
					num += Inlines.abs(num7 - num5);
					int a3 = Inlines.SHL32(num5, 10);
					int num10 = Inlines.PSHR32(Inlines.MULT16_16(a, b), 8) + array[num2] + Inlines.SHL32(a3, 7);
					num10 = Inlines.MAX32(-3670016, num10);
					oldEBands[num2][i] = Inlines.PSHR32(num10, 7);
					array[num2] = array[num2] + Inlines.SHL32(a3, 7) - Inlines.MULT16_16(a2, Inlines.PSHR32(a3, 8));
				}
				while (++num2 < C);
			}
			if (lfe == 0)
			{
				return num;
			}
			return 0;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x00030B64 File Offset: 0x0002ED64
		internal static void quant_coarse_energy(CeltMode m, int start, int end, int effEnd, int[][] eBands, int[][] oldEBands, uint budget, int[][] error, EntropyCoder enc, Span<byte> encodedData, int C, int LM, int nbAvailableBytes, int force_intra, ref int delayedIntra, int two_pass, int loss_rate, int lfe)
		{
			EntropyCoder entropyCoder = new EntropyCoder();
			int num = 0;
			int num2 = (force_intra != 0 || (two_pass == 0 && delayedIntra > 2 * C * (end - start) && nbAvailableBytes > (end - start) * C)) ? 1 : 0;
			int num3 = (int)((ulong)budget * (ulong)((long)delayedIntra) * (ulong)((long)loss_rate) / (ulong)((long)(C * 512)));
			int num4 = QuantizeBands.loss_distortion(eBands, oldEBands, start, effEnd, m.nbEBands, C);
			uint num5 = (uint)enc.tell();
			if (num5 + 3U > budget)
			{
				num2 = (two_pass = 0);
			}
			int num6 = 16384;
			if (end - start > 10)
			{
				num6 = Inlines.MIN32(num6, Inlines.SHL32(nbAvailableBytes, 7));
			}
			if (lfe != 0)
			{
				num6 = 3072;
			}
			entropyCoder.Assign(enc);
			int[][] array = Arrays.InitTwoDimensionalArray<int>(C, m.nbEBands);
			int[][] array2 = Arrays.InitTwoDimensionalArray<int>(C, m.nbEBands);
			Arrays.MemCopy(oldEBands[0], 0, array[0], 0, m.nbEBands);
			if (C == 2)
			{
				Arrays.MemCopy(oldEBands[1], 0, array[1], 0, m.nbEBands);
			}
			if (two_pass != 0 || num2 != 0)
			{
				num = QuantizeBands.quant_coarse_energy_impl(m, start, end, eBands, array, (int)budget, (int)num5, Tables.e_prob_model[LM][1], array2, enc, encodedData, C, LM, 1, num6, lfe);
			}
			if (num2 == 0)
			{
				EntropyCoder entropyCoder2 = new EntropyCoder();
				byte[] array3 = null;
				int num7 = (int)enc.tell_frac();
				entropyCoder2.Assign(enc);
				uint num8 = entropyCoder.range_bytes();
				uint num9 = entropyCoder2.range_bytes();
				int num10 = (int)num8;
				uint num11 = num9 - num8;
				if (num11 != 0U)
				{
					array3 = new byte[num11];
					encodedData.Slice(num10, (int)num11).CopyTo(array3);
				}
				enc.Assign(entropyCoder);
				int num12 = QuantizeBands.quant_coarse_energy_impl(m, start, end, eBands, oldEBands, (int)budget, (int)num5, Tables.e_prob_model[LM][num2], error, enc, encodedData, C, LM, 0, num6, lfe);
				if (two_pass != 0 && (num < num12 || (num == num12 && enc.tell_frac() + (uint)num3 > (uint)num7)))
				{
					enc.Assign(entropyCoder2);
					if (array3 != null)
					{
						MemoryExtensions.AsSpan<byte>(array3, 0, (int)(num9 - num8)).CopyTo(encodedData.Slice(num10));
					}
					Arrays.MemCopy(array[0], 0, oldEBands[0], 0, m.nbEBands);
					Arrays.MemCopy(array2[0], 0, error[0], 0, m.nbEBands);
					if (C == 2)
					{
						Arrays.MemCopy(array[1], 0, oldEBands[1], 0, m.nbEBands);
						Arrays.MemCopy(array2[1], 0, error[1], 0, m.nbEBands);
					}
					num2 = 1;
				}
			}
			else
			{
				Arrays.MemCopy(array[0], 0, oldEBands[0], 0, m.nbEBands);
				Arrays.MemCopy(array2[0], 0, error[0], 0, m.nbEBands);
				if (C == 2)
				{
					Arrays.MemCopy(array[1], 0, oldEBands[1], 0, m.nbEBands);
					Arrays.MemCopy(array2[1], 0, error[1], 0, m.nbEBands);
				}
			}
			if (num2 != 0)
			{
				delayedIntra = num4;
				return;
			}
			delayedIntra = Inlines.ADD32(Inlines.MULT16_32_Q15(Inlines.MULT16_16_Q15(QuantizeBands.pred_coef[LM], QuantizeBands.pred_coef[LM]), delayedIntra), num4);
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00030E58 File Offset: 0x0002F058
		internal static void quant_fine_energy(CeltMode m, int start, int end, int[][] oldEBands, int[][] error, int[] fine_quant, EntropyCoder enc, Span<byte> encodedData, int C)
		{
			for (int i = start; i < end; i++)
			{
				int num = 1 << fine_quant[i];
				if (fine_quant[i] > 0)
				{
					int num2 = 0;
					do
					{
						int num3 = error[num2][i] + 512 >> 10 - fine_quant[i];
						if (num3 > num - 1)
						{
							num3 = num - 1;
						}
						if (num3 < 0)
						{
							num3 = 0;
						}
						enc.enc_bits(encodedData, (uint)num3, (uint)fine_quant[i]);
						int num4 = Inlines.SUB16(Inlines.SHR32(Inlines.SHL32(num3, 10) + 512, fine_quant[i]), 512);
						oldEBands[num2][i] += num4;
						error[num2][i] -= num4;
					}
					while (++num2 < C);
				}
			}
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00030F14 File Offset: 0x0002F114
		internal static void quant_energy_finalise(CeltMode m, int start, int end, int[][] oldEBands, int[][] error, int[] fine_quant, int[] fine_priority, int bits_left, EntropyCoder enc, Span<byte> encodedData, int C)
		{
			for (int i = 0; i < 2; i++)
			{
				int num = start;
				while (num < end && bits_left >= C)
				{
					if (fine_quant[num] < 8 && fine_priority[num] == i)
					{
						int num2 = 0;
						do
						{
							int num3 = (error[num2][num] >= 0) ? 1 : 0;
							enc.enc_bits(encodedData, (uint)num3, 1U);
							int num4 = Inlines.SHR16(Inlines.SHL16(num3, 10) - 512, fine_quant[num] + 1);
							oldEBands[num2][num] += num4;
							bits_left--;
						}
						while (++num2 < C);
					}
					num++;
				}
			}
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00030FA0 File Offset: 0x0002F1A0
		internal static void unquant_coarse_energy(CeltMode m, int start, int end, int[] oldEBands, int intra, EntropyCoder dec, ReadOnlySpan<byte> encodedData, int C, int LM)
		{
			byte[] array = Tables.e_prob_model[LM][intra];
			int[] array2 = new int[2];
			int a;
			int a2;
			if (intra != 0)
			{
				a = 0;
				a2 = QuantizeBands.beta_intra;
			}
			else
			{
				a2 = QuantizeBands.beta_coef[LM];
				a = QuantizeBands.pred_coef[LM];
			}
			int num = (int)(dec.storage * 8U);
			for (int i = start; i < end; i++)
			{
				int num2 = 0;
				do
				{
					int num3 = dec.tell();
					int num5;
					if (num - num3 >= 15)
					{
						int num4 = 2 * Inlines.IMIN(i, 20);
						num5 = Laplace.ec_laplace_decode(dec, encodedData, (uint)((uint)array[num4] << 7), (int)array[num4 + 1] << 6);
					}
					else if (num - num3 >= 2)
					{
						num5 = dec.dec_icdf(encodedData, QuantizeBands.small_energy_icdf, 2U);
						num5 = (num5 >> 1 ^ -(num5 & 1));
					}
					else if (num - num3 >= 1)
					{
						num5 = 0 - dec.dec_bit_logp(encodedData, 1U);
					}
					else
					{
						num5 = -1;
					}
					int a3 = Inlines.SHL32(num5, 10);
					oldEBands[i + num2 * m.nbEBands] = Inlines.MAX16(-9216, oldEBands[i + num2 * m.nbEBands]);
					int num6 = Inlines.PSHR32(Inlines.MULT16_16(a, oldEBands[i + num2 * m.nbEBands]), 8) + array2[num2] + Inlines.SHL32(a3, 7);
					num6 = Inlines.MAX32(-3670016, num6);
					oldEBands[i + num2 * m.nbEBands] = Inlines.PSHR32(num6, 7);
					array2[num2] = array2[num2] + Inlines.SHL32(a3, 7) - Inlines.MULT16_16(a2, Inlines.PSHR32(a3, 8));
				}
				while (++num2 < C);
			}
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00031120 File Offset: 0x0002F320
		internal static void unquant_fine_energy(CeltMode m, int start, int end, int[] oldEBands, int[] fine_quant, EntropyCoder dec, ReadOnlySpan<byte> encodedData, int C)
		{
			for (int i = start; i < end; i++)
			{
				if (fine_quant[i] > 0)
				{
					int num = 0;
					do
					{
						int num2 = Inlines.SUB16(Inlines.SHR32(Inlines.SHL32((int)dec.dec_bits(encodedData, (uint)fine_quant[i]), 10) + 512, fine_quant[i]), 512);
						oldEBands[i + num * m.nbEBands] += num2;
					}
					while (++num < C);
				}
			}
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00031190 File Offset: 0x0002F390
		internal static void unquant_energy_finalise(CeltMode m, int start, int end, int[] oldEBands, int[] fine_quant, int[] fine_priority, int bits_left, EntropyCoder dec, ReadOnlySpan<byte> encodedData, int C)
		{
			for (int i = 0; i < 2; i++)
			{
				int num = start;
				while (num < end && bits_left >= C)
				{
					if (fine_quant[num] < 8 && fine_priority[num] == i)
					{
						int num2 = 0;
						do
						{
							int num3 = Inlines.SHR16(Inlines.SHL16((int)dec.dec_bits(encodedData, 1U), 10) - 512, fine_quant[num] + 1);
							oldEBands[num + num2 * m.nbEBands] += num3;
							bits_left--;
						}
						while (++num2 < C);
					}
					num++;
				}
			}
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00031214 File Offset: 0x0002F414
		internal static void amp2Log2(CeltMode m, int effEnd, int end, int[][] bandE, int[][] bandLogE, int C)
		{
			int num = 0;
			do
			{
				for (int i = 0; i < effEnd; i++)
				{
					bandLogE[num][i] = Inlines.celt_log2(Inlines.SHL32(bandE[num][i], 2)) - Inlines.SHL16((int)Tables.eMeans[i], 6);
				}
				for (int i = effEnd; i < end; i++)
				{
					bandLogE[num][i] = -14336;
				}
			}
			while (++num < C);
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00031274 File Offset: 0x0002F474
		internal unsafe static void amp2Log2(CeltMode m, int effEnd, int end, int[] bandE, Span<int> bandLogE, int bandLogE_ptr, int C)
		{
			int num = 0;
			do
			{
				for (int i = 0; i < effEnd; i++)
				{
					*bandLogE[bandLogE_ptr + num * m.nbEBands + i] = Inlines.celt_log2(Inlines.SHL32(bandE[i + num * m.nbEBands], 2)) - Inlines.SHL16((int)Tables.eMeans[i], 6);
				}
				for (int i = effEnd; i < end; i++)
				{
					*bandLogE[bandLogE_ptr + num * m.nbEBands + i] = -14336;
				}
			}
			while (++num < C);
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x000312F8 File Offset: 0x0002F4F8
		// Note: this type is marked as 'beforefieldinit'.
		static QuantizeBands()
		{
			byte[] array = new byte[3];
			array[0] = 2;
			array[1] = 1;
			QuantizeBands.small_energy_icdf = array;
		}

		// Token: 0x04000456 RID: 1110
		private static readonly int[] pred_coef = new int[]
		{
			29440,
			26112,
			21248,
			16384
		};

		// Token: 0x04000457 RID: 1111
		private static readonly int[] beta_coef = new int[]
		{
			30147,
			22282,
			12124,
			6554
		};

		// Token: 0x04000458 RID: 1112
		private static readonly int beta_intra = 4915;

		// Token: 0x04000459 RID: 1113
		private static byte[] small_energy_icdf;
	}
}
