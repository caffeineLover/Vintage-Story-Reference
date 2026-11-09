using System;
using Concentus.Common;
using Concentus.Common.CPlusPlus;
using Concentus.Silk.Structs;

namespace Concentus.Silk
{
	// Token: 0x02000019 RID: 25
	internal static class DecodeCore
	{
		// Token: 0x060000B6 RID: 182 RVA: 0x00006FDC File Offset: 0x000051DC
		internal unsafe static void silk_decode_core(SilkChannelDecoder psDec, SilkDecoderControl psDecCtrl, Span<short> xq, int xq_ptr, short[] pulses)
		{
			int num = 0;
			short[] ltpcoef_Q = psDecCtrl.LTPCoef_Q14;
			short[] array = new short[psDec.ltp_mem_length];
			int[] array2 = new int[psDec.ltp_mem_length + psDec.frame_length];
			int[] array3 = new int[psDec.subfr_length];
			int[] array4 = new int[psDec.subfr_length + 16];
			int num2 = (int)Tables.silk_Quantization_Offsets_Q10[psDec.indices.signalType >> 1][(int)psDec.indices.quantOffsetType];
			int num3;
			if (psDec.indices.NLSFInterpCoef_Q2 < 4)
			{
				num3 = 1;
			}
			else
			{
				num3 = 0;
			}
			int num4 = (int)psDec.indices.Seed;
			for (int i = 0; i < psDec.frame_length; i++)
			{
				num4 = Inlines.silk_RAND(num4);
				psDec.exc_Q14[i] = Inlines.silk_LSHIFT((int)pulses[i], 14);
				if (psDec.exc_Q14[i] > 0)
				{
					psDec.exc_Q14[i] -= 1280;
				}
				else if (psDec.exc_Q14[i] < 0)
				{
					psDec.exc_Q14[i] += 1280;
				}
				psDec.exc_Q14[i] += num2 << 4;
				if (num4 < 0)
				{
					psDec.exc_Q14[i] = -psDec.exc_Q14[i];
				}
				num4 = Inlines.silk_ADD32_ovflw(num4, (int)pulses[i]);
			}
			Arrays.MemCopy(psDec.sLPC_Q14_buf, 0, array4, 0, 16);
			int num5 = 0;
			int num6 = xq_ptr;
			int num7 = psDec.ltp_mem_length;
			for (int j = 0; j < psDec.nb_subfr; j++)
			{
				int[] array5 = array3;
				int num8 = 0;
				short[] array6 = psDecCtrl.PredCoef_Q12[j >> 1];
				int num9 = j * 5;
				int num10 = (int)psDec.indices.signalType;
				int b = Inlines.silk_RSHIFT(psDecCtrl.Gains_Q16[j], 6);
				int a = Inlines.silk_INVERSE32_varQ(psDecCtrl.Gains_Q16[j], 47);
				int num11;
				if (psDecCtrl.Gains_Q16[j] != psDec.prev_gain_Q16)
				{
					num11 = Inlines.silk_DIV32_varQ(psDec.prev_gain_Q16, psDecCtrl.Gains_Q16[j], 16);
					for (int i = 0; i < 16; i++)
					{
						array4[i] = Inlines.silk_SMULWW(num11, array4[i]);
					}
				}
				else
				{
					num11 = 65536;
				}
				psDec.prev_gain_Q16 = psDecCtrl.Gains_Q16[j];
				if (psDec.lossCnt != 0 && psDec.prevSignalType == 2 && psDec.indices.signalType != 2 && j < 2)
				{
					Arrays.MemSetWithOffset<short>(ltpcoef_Q, 0, num9, 5);
					ltpcoef_Q[num9 + 2] = 4096;
					num10 = 2;
					psDecCtrl.pitchL[j] = psDec.lagPrev;
				}
				if (num10 == 2)
				{
					num = psDecCtrl.pitchL[j];
					if (j == 0 || (j == 2 && num3 != 0))
					{
						int num12 = psDec.ltp_mem_length - num - psDec.LPC_order - 2;
						if (j == 2)
						{
							xq.Slice(xq_ptr, 2 * psDec.subfr_length).CopyTo(MemoryExtensions.AsSpan<short>(psDec.outBuf, psDec.ltp_mem_length));
						}
						Filters.silk_LPC_analysis_filter(array, num12, psDec.outBuf, num12 + j * psDec.subfr_length, array6, 0, psDec.ltp_mem_length - num12, psDec.LPC_order);
						if (j == 0)
						{
							a = Inlines.silk_LSHIFT(Inlines.silk_SMULWB(a, psDecCtrl.LTP_scale_Q14), 2);
						}
						for (int i = 0; i < num + 2; i++)
						{
							array2[num7 - i - 1] = Inlines.silk_SMULWB(a, (int)array[psDec.ltp_mem_length - i - 1]);
						}
					}
					else if (num11 != 65536)
					{
						for (int i = 0; i < num + 2; i++)
						{
							array2[num7 - i - 1] = Inlines.silk_SMULWW(num11, array2[num7 - i - 1]);
						}
					}
				}
				if (num10 == 2)
				{
					int num13 = num7 - num + 2;
					for (int i = 0; i < psDec.subfr_length; i++)
					{
						int num14 = 2;
						num14 = Inlines.silk_SMLAWB(num14, array2[num13], (int)ltpcoef_Q[num9]);
						num14 = Inlines.silk_SMLAWB(num14, array2[num13 - 1], (int)ltpcoef_Q[num9 + 1]);
						num14 = Inlines.silk_SMLAWB(num14, array2[num13 - 2], (int)ltpcoef_Q[num9 + 2]);
						num14 = Inlines.silk_SMLAWB(num14, array2[num13 - 3], (int)ltpcoef_Q[num9 + 3]);
						num14 = Inlines.silk_SMLAWB(num14, array2[num13 - 4], (int)ltpcoef_Q[num9 + 4]);
						num13++;
						array5[num8 + i] = Inlines.silk_ADD_LSHIFT32(psDec.exc_Q14[num5 + i], num14, 1);
						array2[num7] = Inlines.silk_LSHIFT(array5[num8 + i], 1);
						num7++;
					}
				}
				else
				{
					array5 = psDec.exc_Q14;
					num8 = num5;
				}
				for (int i = 0; i < psDec.subfr_length; i++)
				{
					int num15 = Inlines.silk_RSHIFT(psDec.LPC_order, 1);
					num15 = Inlines.silk_SMLAWB(num15, array4[16 + i - 1], (int)array6[0]);
					num15 = Inlines.silk_SMLAWB(num15, array4[16 + i - 2], (int)array6[1]);
					num15 = Inlines.silk_SMLAWB(num15, array4[16 + i - 3], (int)array6[2]);
					num15 = Inlines.silk_SMLAWB(num15, array4[16 + i - 4], (int)array6[3]);
					num15 = Inlines.silk_SMLAWB(num15, array4[16 + i - 5], (int)array6[4]);
					num15 = Inlines.silk_SMLAWB(num15, array4[16 + i - 6], (int)array6[5]);
					num15 = Inlines.silk_SMLAWB(num15, array4[16 + i - 7], (int)array6[6]);
					num15 = Inlines.silk_SMLAWB(num15, array4[16 + i - 8], (int)array6[7]);
					num15 = Inlines.silk_SMLAWB(num15, array4[16 + i - 9], (int)array6[8]);
					num15 = Inlines.silk_SMLAWB(num15, array4[16 + i - 10], (int)array6[9]);
					if (psDec.LPC_order == 16)
					{
						num15 = Inlines.silk_SMLAWB(num15, array4[16 + i - 11], (int)array6[10]);
						num15 = Inlines.silk_SMLAWB(num15, array4[16 + i - 12], (int)array6[11]);
						num15 = Inlines.silk_SMLAWB(num15, array4[16 + i - 13], (int)array6[12]);
						num15 = Inlines.silk_SMLAWB(num15, array4[16 + i - 14], (int)array6[13]);
						num15 = Inlines.silk_SMLAWB(num15, array4[16 + i - 15], (int)array6[14]);
						num15 = Inlines.silk_SMLAWB(num15, array4[16 + i - 16], (int)array6[15]);
					}
					array4[16 + i] = Inlines.silk_ADD_LSHIFT32(array5[num8 + i], num15, 4);
					*xq[num6 + i] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(Inlines.silk_SMULWW(array4[16 + i], b), 8));
				}
				Arrays.MemCopy(array4, psDec.subfr_length, array4, 0, 16);
				num5 += psDec.subfr_length;
				num6 += psDec.subfr_length;
			}
			Arrays.MemCopy(array4, 0, psDec.sLPC_Q14_buf, 0, 16);
		}
	}
}
