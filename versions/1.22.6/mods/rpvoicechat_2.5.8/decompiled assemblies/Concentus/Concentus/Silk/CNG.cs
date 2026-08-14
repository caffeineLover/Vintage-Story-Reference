using System;
using Concentus.Common;
using Concentus.Common.CPlusPlus;
using Concentus.Silk.Structs;

namespace Concentus.Silk
{
	// Token: 0x02000015 RID: 21
	internal static class CNG
	{
		// Token: 0x060000AB RID: 171 RVA: 0x00005D24 File Offset: 0x00003F24
		internal unsafe static void silk_CNG_exc(Span<int> exc_Q10, int exc_Q10_ptr, int[] exc_buf_Q14, int Gain_Q16, int length, ref int rand_seed)
		{
			int i;
			for (i = 255; i > length; i = Inlines.silk_RSHIFT(i, 1))
			{
			}
			int num = rand_seed;
			for (int j = exc_Q10_ptr; j < exc_Q10_ptr + length; j++)
			{
				num = Inlines.silk_RAND(num);
				int num2 = Inlines.silk_RSHIFT(num, 24) & i;
				*exc_Q10[j] = (int)((short)Inlines.silk_SAT16(Inlines.silk_SMULWW(exc_buf_Q14[num2], Gain_Q16 >> 4)));
			}
			rand_seed = num;
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00005D8C File Offset: 0x00003F8C
		internal static void silk_CNG_Reset(SilkChannelDecoder psDec)
		{
			int num = Inlines.silk_DIV32_16(32767, (int)((short)(psDec.LPC_order + 1)));
			int num2 = 0;
			for (int i = 0; i < psDec.LPC_order; i++)
			{
				num2 += num;
				psDec.sCNG.CNG_smth_NLSF_Q15[i] = (short)num2;
			}
			psDec.sCNG.CNG_smth_Gain_Q16 = 0;
			psDec.sCNG.rand_seed = 3176576;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00005DF0 File Offset: 0x00003FF0
		internal unsafe static void silk_CNG(SilkChannelDecoder psDec, SilkDecoderControl psDecCtrl, Span<short> frame, int frame_ptr, int length)
		{
			short[] array = new short[psDec.LPC_order];
			CNGState sCNG = psDec.sCNG;
			if (psDec.fs_kHz != sCNG.fs_kHz)
			{
				CNG.silk_CNG_Reset(psDec);
				sCNG.fs_kHz = psDec.fs_kHz;
			}
			if (psDec.lossCnt == 0 && psDec.prevSignalType == 0)
			{
				for (int i = 0; i < psDec.LPC_order; i++)
				{
					short[] cng_smth_NLSF_Q = sCNG.CNG_smth_NLSF_Q15;
					int num = i;
					cng_smth_NLSF_Q[num] += (short)Inlines.silk_SMULWB((int)(psDec.prevNLSF_Q15[i] - sCNG.CNG_smth_NLSF_Q15[i]), 16348);
				}
				int num2 = 0;
				for (int i = 0; i < psDec.nb_subfr; i++)
				{
					if (psDecCtrl.Gains_Q16[i] > num2)
					{
						num2 = psDecCtrl.Gains_Q16[i];
					}
				}
				Arrays.MemMoveInt(sCNG.CNG_exc_buf_Q14, 0, psDec.subfr_length, (psDec.nb_subfr - 1) * psDec.subfr_length);
				for (int i = 0; i < psDec.nb_subfr; i++)
				{
					sCNG.CNG_smth_Gain_Q16 += Inlines.silk_SMULWB(psDecCtrl.Gains_Q16[i] - sCNG.CNG_smth_Gain_Q16, 4634);
				}
			}
			if (psDec.lossCnt != 0)
			{
				int[] array2 = new int[length + 16];
				int num3 = Inlines.silk_SMULWW((int)psDec.sPLC.randScale_Q14, psDec.sPLC.prevGain_Q16[1]);
				if (num3 >= 2097152 || sCNG.CNG_smth_Gain_Q16 > 8388608)
				{
					num3 = Inlines.silk_SMULTT(num3, num3);
					num3 = Inlines.silk_SUB_LSHIFT32(Inlines.silk_SMULTT(sCNG.CNG_smth_Gain_Q16, sCNG.CNG_smth_Gain_Q16), num3, 5);
					num3 = Inlines.silk_LSHIFT32(Inlines.silk_SQRT_APPROX(num3), 16);
				}
				else
				{
					num3 = Inlines.silk_SMULWW(num3, num3);
					num3 = Inlines.silk_SUB_LSHIFT32(Inlines.silk_SMULWW(sCNG.CNG_smth_Gain_Q16, sCNG.CNG_smth_Gain_Q16), num3, 5);
					num3 = Inlines.silk_LSHIFT32(Inlines.silk_SQRT_APPROX(num3), 8);
				}
				CNG.silk_CNG_exc(array2, 16, sCNG.CNG_exc_buf_Q14, num3, length, ref sCNG.rand_seed);
				NLSF.silk_NLSF2A(array, sCNG.CNG_smth_NLSF_Q15, psDec.LPC_order);
				Arrays.MemCopy(sCNG.CNG_synth_state, 0, array2, 0, 16);
				for (int i = 0; i < length; i++)
				{
					int num4 = 16 + i;
					int num5 = Inlines.silk_RSHIFT(psDec.LPC_order, 1);
					num5 = Inlines.silk_SMLAWB(num5, array2[num4 - 1], (int)array[0]);
					num5 = Inlines.silk_SMLAWB(num5, array2[num4 - 2], (int)array[1]);
					num5 = Inlines.silk_SMLAWB(num5, array2[num4 - 3], (int)array[2]);
					num5 = Inlines.silk_SMLAWB(num5, array2[num4 - 4], (int)array[3]);
					num5 = Inlines.silk_SMLAWB(num5, array2[num4 - 5], (int)array[4]);
					num5 = Inlines.silk_SMLAWB(num5, array2[num4 - 6], (int)array[5]);
					num5 = Inlines.silk_SMLAWB(num5, array2[num4 - 7], (int)array[6]);
					num5 = Inlines.silk_SMLAWB(num5, array2[num4 - 8], (int)array[7]);
					num5 = Inlines.silk_SMLAWB(num5, array2[num4 - 9], (int)array[8]);
					num5 = Inlines.silk_SMLAWB(num5, array2[num4 - 10], (int)array[9]);
					if (psDec.LPC_order == 16)
					{
						num5 = Inlines.silk_SMLAWB(num5, array2[num4 - 11], (int)array[10]);
						num5 = Inlines.silk_SMLAWB(num5, array2[num4 - 12], (int)array[11]);
						num5 = Inlines.silk_SMLAWB(num5, array2[num4 - 13], (int)array[12]);
						num5 = Inlines.silk_SMLAWB(num5, array2[num4 - 14], (int)array[13]);
						num5 = Inlines.silk_SMLAWB(num5, array2[num4 - 15], (int)array[14]);
						num5 = Inlines.silk_SMLAWB(num5, array2[num4 - 16], (int)array[15]);
					}
					array2[num4] = Inlines.silk_ADD_LSHIFT(array2[num4], num5, 4);
					*frame[frame_ptr + i] = Inlines.silk_ADD_SAT16(*frame[frame_ptr + i], (short)Inlines.silk_RSHIFT_ROUND(array2[num4], 10));
				}
				Arrays.MemCopy(array2, length, sCNG.CNG_synth_state, 0, 16);
				return;
			}
			Arrays.MemSetInt(sCNG.CNG_synth_state, 0, psDec.LPC_order);
		}
	}
}
