using System;
using Concentus.Common;
using Concentus.Common.CPlusPlus;
using Concentus.Silk.Structs;

namespace Concentus.Silk
{
	// Token: 0x02000030 RID: 48
	internal static class PLC
	{
		// Token: 0x060000FC RID: 252 RVA: 0x0000E9D8 File Offset: 0x0000CBD8
		internal static void silk_PLC_Reset(SilkChannelDecoder psDec)
		{
			psDec.sPLC.pitchL_Q8 = Inlines.silk_LSHIFT(psDec.frame_length, 7);
			psDec.sPLC.prevGain_Q16[0] = 65536;
			psDec.sPLC.prevGain_Q16[1] = 65536;
			psDec.sPLC.subfr_length = 20;
			psDec.sPLC.nb_subfr = 2;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0000EA3C File Offset: 0x0000CC3C
		internal static void silk_PLC(SilkChannelDecoder psDec, SilkDecoderControl psDecCtrl, Span<short> frame, int frame_ptr, int lost)
		{
			if (psDec.fs_kHz != psDec.sPLC.fs_kHz)
			{
				PLC.silk_PLC_Reset(psDec);
				psDec.sPLC.fs_kHz = psDec.fs_kHz;
			}
			if (lost != 0)
			{
				PLC.silk_PLC_conceal(psDec, psDecCtrl, frame, frame_ptr);
				psDec.lossCnt++;
				return;
			}
			PLC.silk_PLC_update(psDec, psDecCtrl);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0000EA98 File Offset: 0x0000CC98
		internal static void silk_PLC_update(SilkChannelDecoder psDec, SilkDecoderControl psDecCtrl)
		{
			PLCStruct sPLC = psDec.sPLC;
			psDec.prevSignalType = (int)psDec.indices.signalType;
			int num = 0;
			if (psDec.indices.signalType == 2)
			{
				int num2 = 0;
				while (num2 * psDec.subfr_length < psDecCtrl.pitchL[psDec.nb_subfr - 1] && num2 != psDec.nb_subfr)
				{
					int num3 = 0;
					for (int i = 0; i < 5; i++)
					{
						num3 += (int)psDecCtrl.LTPCoef_Q14[(psDec.nb_subfr - 1 - num2) * 5 + i];
					}
					if (num3 > num)
					{
						num = num3;
						Arrays.MemCopy(psDecCtrl.LTPCoef_Q14, Inlines.silk_SMULBB(psDec.nb_subfr - 1 - num2, 5), sPLC.LTPCoef_Q14, 0, 5);
						sPLC.pitchL_Q8 = Inlines.silk_LSHIFT(psDecCtrl.pitchL[psDec.nb_subfr - 1 - num2], 8);
					}
					num2++;
				}
				Arrays.MemSetShort(sPLC.LTPCoef_Q14, 0, 5);
				sPLC.LTPCoef_Q14[2] = (short)num;
				if (num < 11469)
				{
					int b = Inlines.silk_DIV32(Inlines.silk_LSHIFT(11469, 10), Inlines.silk_max(num, 1));
					for (int i = 0; i < 5; i++)
					{
						sPLC.LTPCoef_Q14[i] = (short)Inlines.silk_RSHIFT(Inlines.silk_SMULBB((int)sPLC.LTPCoef_Q14[i], b), 10);
					}
				}
				else if (num > 15565)
				{
					int b2 = Inlines.silk_DIV32(Inlines.silk_LSHIFT(15565, 14), Inlines.silk_max(num, 1));
					for (int i = 0; i < 5; i++)
					{
						sPLC.LTPCoef_Q14[i] = (short)Inlines.silk_RSHIFT(Inlines.silk_SMULBB((int)sPLC.LTPCoef_Q14[i], b2), 14);
					}
				}
			}
			else
			{
				sPLC.pitchL_Q8 = Inlines.silk_LSHIFT(Inlines.silk_SMULBB(psDec.fs_kHz, 18), 8);
				Arrays.MemSetShort(sPLC.LTPCoef_Q14, 0, 5);
			}
			Arrays.MemCopy(psDecCtrl.PredCoef_Q12[1], 0, sPLC.prevLPC_Q12, 0, psDec.LPC_order);
			sPLC.prevLTP_scale_Q14 = (short)psDecCtrl.LTP_scale_Q14;
			Arrays.MemCopy(psDecCtrl.Gains_Q16, psDec.nb_subfr - 2, sPLC.prevGain_Q16, 0, 2);
			sPLC.subfr_length = psDec.subfr_length;
			sPLC.nb_subfr = psDec.nb_subfr;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x0000ECB4 File Offset: 0x0000CEB4
		internal static void silk_PLC_energy(out int energy1, out int shift1, out int energy2, out int shift2, int[] exc_Q14, int[] prevGain_Q10, int subfr_length, int nb_subfr)
		{
			int num = 0;
			short[] array = new short[2 * subfr_length];
			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < subfr_length; j++)
				{
					array[num + j] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT(Inlines.silk_SMULWW(exc_Q14[j + (i + nb_subfr - 2) * subfr_length], prevGain_Q10[i]), 8));
				}
				num += subfr_length;
			}
			SumSqrShift.silk_sum_sqr_shift(out energy1, out shift1, array, subfr_length);
			SumSqrShift.silk_sum_sqr_shift(out energy2, out shift2, array, subfr_length, subfr_length);
		}

		// Token: 0x06000100 RID: 256 RVA: 0x0000ED30 File Offset: 0x0000CF30
		internal unsafe static void silk_PLC_conceal(SilkChannelDecoder psDec, SilkDecoderControl psDecCtrl, Span<short> frame, int frame_ptr)
		{
			short[] array = new short[psDec.ltp_mem_length];
			int[] array2 = new int[psDec.ltp_mem_length + psDec.frame_length];
			PLCStruct sPLC = psDec.sPLC;
			int[] array3 = new int[]
			{
				Inlines.silk_RSHIFT(sPLC.prevGain_Q16[0], 6),
				Inlines.silk_RSHIFT(sPLC.prevGain_Q16[1], 6)
			};
			if (psDec.first_frame_after_reset != 0)
			{
				Arrays.MemSetShort(sPLC.prevLPC_Q12, 0, 16);
			}
			int a;
			int shift;
			int a2;
			int shift2;
			PLC.silk_PLC_energy(out a, out shift, out a2, out shift2, psDec.exc_Q14, array3, psDec.subfr_length, psDec.nb_subfr);
			int num;
			if (Inlines.silk_RSHIFT(a, shift2) < Inlines.silk_RSHIFT(a2, shift))
			{
				num = Inlines.silk_max_int(0, (sPLC.nb_subfr - 1) * sPLC.subfr_length - 128);
			}
			else
			{
				num = Inlines.silk_max_int(0, sPLC.nb_subfr * sPLC.subfr_length - 128);
			}
			short[] ltpcoef_Q = sPLC.LTPCoef_Q14;
			short num2 = sPLC.randScale_Q14;
			int a3 = (int)PLC.HARM_ATT_Q15[Inlines.silk_min_int(1, psDec.lossCnt)];
			int b;
			if (psDec.prevSignalType == 2)
			{
				b = (int)PLC.PLC_RAND_ATTENUATE_V_Q15[Inlines.silk_min_int(1, psDec.lossCnt)];
			}
			else
			{
				b = (int)PLC.PLC_RAND_ATTENUATE_UV_Q15[Inlines.silk_min_int(1, psDec.lossCnt)];
			}
			BWExpander.silk_bwexpander(sPLC.prevLPC_Q12, psDec.LPC_order, 64881);
			if (psDec.lossCnt == 0)
			{
				num2 = 16384;
				if (psDec.prevSignalType == 2)
				{
					for (int i = 0; i < 5; i++)
					{
						num2 -= ltpcoef_Q[i];
					}
					num2 = Inlines.silk_max_16(3277, num2);
					num2 = (short)Inlines.silk_RSHIFT(Inlines.silk_SMULBB((int)num2, (int)sPLC.prevLTP_scale_Q14), 14);
				}
				else
				{
					int b2 = LPCInversePredGain.silk_LPC_inverse_pred_gain(sPLC.prevLPC_Q12, psDec.LPC_order);
					int num3 = Inlines.silk_min_32(Inlines.silk_RSHIFT(1073741824, 3), b2);
					num3 = Inlines.silk_max_32(Inlines.silk_RSHIFT(1073741824, 8), num3);
					num3 = Inlines.silk_LSHIFT(num3, 3);
					b = Inlines.silk_RSHIFT(Inlines.silk_SMULWB(num3, b), 14);
				}
			}
			int num4 = sPLC.rand_seed;
			int num5 = Inlines.silk_RSHIFT_ROUND(sPLC.pitchL_Q8, 8);
			int num6 = psDec.ltp_mem_length;
			int num7 = psDec.ltp_mem_length - num5 - psDec.LPC_order - 2;
			Filters.silk_LPC_analysis_filter(array, num7, psDec.outBuf, num7, sPLC.prevLPC_Q12, 0, psDec.ltp_mem_length - num7, psDec.LPC_order);
			int num8 = Inlines.silk_INVERSE32_varQ(sPLC.prevGain_Q16[1], 46);
			num8 = Inlines.silk_min(num8, 1073741823);
			for (int i = num7 + psDec.LPC_order; i < psDec.ltp_mem_length; i++)
			{
				array2[i] = Inlines.silk_SMULWB(num8, (int)array[i]);
			}
			for (int j = 0; j < psDec.nb_subfr; j++)
			{
				int num9 = num6 - num5 + 2;
				for (int i = 0; i < psDec.subfr_length; i++)
				{
					int a4 = 2;
					a4 = Inlines.silk_SMLAWB(a4, array2[num9], (int)ltpcoef_Q[0]);
					a4 = Inlines.silk_SMLAWB(a4, array2[num9 - 1], (int)ltpcoef_Q[1]);
					a4 = Inlines.silk_SMLAWB(a4, array2[num9 - 2], (int)ltpcoef_Q[2]);
					a4 = Inlines.silk_SMLAWB(a4, array2[num9 - 3], (int)ltpcoef_Q[3]);
					a4 = Inlines.silk_SMLAWB(a4, array2[num9 - 4], (int)ltpcoef_Q[4]);
					num9++;
					num4 = Inlines.silk_RAND(num4);
					num7 = (Inlines.silk_RSHIFT(num4, 25) & 127);
					array2[num6] = Inlines.silk_LSHIFT32(Inlines.silk_SMLAWB(a4, psDec.exc_Q14[num + num7], (int)num2), 2);
					num6++;
				}
				for (int k = 0; k < 5; k++)
				{
					ltpcoef_Q[k] = (short)Inlines.silk_RSHIFT(Inlines.silk_SMULBB(a3, (int)ltpcoef_Q[k]), 15);
				}
				num2 = (short)Inlines.silk_RSHIFT(Inlines.silk_SMULBB((int)num2, b), 15);
				sPLC.pitchL_Q8 = Inlines.silk_SMLAWB(sPLC.pitchL_Q8, sPLC.pitchL_Q8, 655);
				sPLC.pitchL_Q8 = Inlines.silk_min_32(sPLC.pitchL_Q8, Inlines.silk_LSHIFT(Inlines.silk_SMULBB(18, psDec.fs_kHz), 8));
				num5 = Inlines.silk_RSHIFT_ROUND(sPLC.pitchL_Q8, 8);
			}
			int num10 = psDec.ltp_mem_length - 16;
			Arrays.MemCopy(psDec.sLPC_Q14_buf, 0, array2, num10, 16);
			for (int i = 0; i < psDec.frame_length; i++)
			{
				int num11 = num10 + 16 + i;
				int num12 = Inlines.silk_RSHIFT(psDec.LPC_order, 1);
				num12 = Inlines.silk_SMLAWB(num12, array2[num11 - 1], (int)sPLC.prevLPC_Q12[0]);
				num12 = Inlines.silk_SMLAWB(num12, array2[num11 - 2], (int)sPLC.prevLPC_Q12[1]);
				num12 = Inlines.silk_SMLAWB(num12, array2[num11 - 3], (int)sPLC.prevLPC_Q12[2]);
				num12 = Inlines.silk_SMLAWB(num12, array2[num11 - 4], (int)sPLC.prevLPC_Q12[3]);
				num12 = Inlines.silk_SMLAWB(num12, array2[num11 - 5], (int)sPLC.prevLPC_Q12[4]);
				num12 = Inlines.silk_SMLAWB(num12, array2[num11 - 6], (int)sPLC.prevLPC_Q12[5]);
				num12 = Inlines.silk_SMLAWB(num12, array2[num11 - 7], (int)sPLC.prevLPC_Q12[6]);
				num12 = Inlines.silk_SMLAWB(num12, array2[num11 - 8], (int)sPLC.prevLPC_Q12[7]);
				num12 = Inlines.silk_SMLAWB(num12, array2[num11 - 9], (int)sPLC.prevLPC_Q12[8]);
				num12 = Inlines.silk_SMLAWB(num12, array2[num11 - 10], (int)sPLC.prevLPC_Q12[9]);
				for (int k = 10; k < psDec.LPC_order; k++)
				{
					num12 = Inlines.silk_SMLAWB(num12, array2[num11 - k - 1], (int)sPLC.prevLPC_Q12[k]);
				}
				array2[num11] = Inlines.silk_ADD_LSHIFT32(array2[num11], num12, 4);
				*frame[frame_ptr + i] = (short)Inlines.silk_SAT16(Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(Inlines.silk_SMULWW(array2[num11], array3[1]), 8)));
			}
			Arrays.MemCopy(array2, num10 + psDec.frame_length, psDec.sLPC_Q14_buf, 0, 16);
			sPLC.rand_seed = num4;
			sPLC.randScale_Q14 = num2;
			for (int i = 0; i < 4; i++)
			{
				psDecCtrl.pitchL[i] = num5;
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0000F348 File Offset: 0x0000D548
		internal unsafe static void silk_PLC_glue_frames(SilkChannelDecoder psDec, Span<short> frame, int frame_ptr, int length)
		{
			PLCStruct sPLC = psDec.sPLC;
			if (psDec.lossCnt != 0)
			{
				SumSqrShift.silk_sum_sqr_shift(out sPLC.conc_energy, out sPLC.conc_energy_shift, frame, frame_ptr, length);
				sPLC.last_frame_lost = 1;
				return;
			}
			if (psDec.sPLC.last_frame_lost != 0)
			{
				int num;
				int num2;
				SumSqrShift.silk_sum_sqr_shift(out num, out num2, frame, frame_ptr, length);
				if (num2 > sPLC.conc_energy_shift)
				{
					sPLC.conc_energy = Inlines.silk_RSHIFT(sPLC.conc_energy, num2 - sPLC.conc_energy_shift);
				}
				else if (num2 < sPLC.conc_energy_shift)
				{
					num = Inlines.silk_RSHIFT(num, sPLC.conc_energy_shift - num2);
				}
				if (num > sPLC.conc_energy)
				{
					int num3 = Inlines.silk_CLZ32(sPLC.conc_energy);
					num3--;
					sPLC.conc_energy = Inlines.silk_LSHIFT(sPLC.conc_energy, num3);
					num = Inlines.silk_RSHIFT(num, Inlines.silk_max_32(24 - num3, 0));
					int num4 = Inlines.silk_LSHIFT(Inlines.silk_SQRT_APPROX(Inlines.silk_DIV32(sPLC.conc_energy, Inlines.silk_max(num, 1))), 4);
					int num5 = Inlines.silk_DIV32_16(65536 - num4, length);
					num5 = Inlines.silk_LSHIFT(num5, 2);
					for (int i = frame_ptr; i < frame_ptr + length; i++)
					{
						*frame[i] = (short)Inlines.silk_SMULWB(num4, (int)(*frame[i]));
						num4 += num5;
						if (num4 > 65536)
						{
							break;
						}
					}
				}
			}
			sPLC.last_frame_lost = 0;
		}

		// Token: 0x0400004F RID: 79
		private const int NB_ATT = 2;

		// Token: 0x04000050 RID: 80
		private static readonly short[] HARM_ATT_Q15 = new short[]
		{
			32440,
			31130
		};

		// Token: 0x04000051 RID: 81
		private static readonly short[] PLC_RAND_ATTENUATE_V_Q15 = new short[]
		{
			31130,
			26214
		};

		// Token: 0x04000052 RID: 82
		private static readonly short[] PLC_RAND_ATTENUATE_UV_Q15 = new short[]
		{
			32440,
			29491
		};
	}
}
