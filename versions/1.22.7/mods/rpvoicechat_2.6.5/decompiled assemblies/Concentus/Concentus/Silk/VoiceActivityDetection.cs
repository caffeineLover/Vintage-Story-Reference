using System;
using Concentus.Common;
using Concentus.Silk.Structs;

namespace Concentus.Silk
{
	// Token: 0x0200003F RID: 63
	internal static class VoiceActivityDetection
	{
		// Token: 0x0600012D RID: 301 RVA: 0x0001391C File Offset: 0x00011B1C
		internal static int silk_VAD_Init(SilkVADState psSilk_VAD)
		{
			int result = 0;
			psSilk_VAD.Reset();
			for (int i = 0; i < 4; i++)
			{
				psSilk_VAD.NoiseLevelBias[i] = Inlines.silk_max_32(Inlines.silk_DIV32_16(50, (int)((short)(i + 1))), 1);
			}
			for (int i = 0; i < 4; i++)
			{
				psSilk_VAD.NL[i] = Inlines.silk_MUL(100, psSilk_VAD.NoiseLevelBias[i]);
				psSilk_VAD.inv_NL[i] = Inlines.silk_DIV32(int.MaxValue, psSilk_VAD.NL[i]);
			}
			psSilk_VAD.counter = 15;
			for (int i = 0; i < 4; i++)
			{
				psSilk_VAD.NrgRatioSmth_Q8[i] = 25600;
			}
			return result;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x000139B8 File Offset: 0x00011BB8
		internal static int silk_VAD_GetSA_Q8(SilkChannelEncoder psEncC, Span<short> pIn, int pIn_ptr)
		{
			int num = 0;
			int[] array = new int[4];
			int[] array2 = new int[4];
			int[] array3 = new int[4];
			int result = 0;
			SilkVADState sVAD = psEncC.sVAD;
			int num2 = Inlines.silk_RSHIFT(psEncC.frame_length, 1);
			int num3 = Inlines.silk_RSHIFT(psEncC.frame_length, 2);
			int num4 = Inlines.silk_RSHIFT(psEncC.frame_length, 3);
			array3[0] = 0;
			array3[1] = num4 + num3;
			array3[2] = array3[1] + num4;
			array3[3] = array3[2] + num3;
			short[] array4 = new short[array3[3] + num2];
			Filters.silk_ana_filt_bank_1(pIn, pIn_ptr, sVAD.AnaState, array4, array4, array3[3], psEncC.frame_length);
			Filters.silk_ana_filt_bank_1(array4, 0, sVAD.AnaState1, array4, array4, array3[2], num2);
			Filters.silk_ana_filt_bank_1(array4, 0, sVAD.AnaState2, array4, array4, array3[1], num3);
			array4[num4 - 1] = (short)Inlines.silk_RSHIFT((int)array4[num4 - 1], 1);
			short hpstate = array4[num4 - 1];
			for (int i = num4 - 1; i > 0; i--)
			{
				array4[i - 1] = (short)Inlines.silk_RSHIFT((int)array4[i - 1], 1);
				short[] array5 = array4;
				int num5 = i;
				array5[num5] -= array4[i - 1];
			}
			short[] array6 = array4;
			int num6 = 0;
			array6[num6] -= sVAD.HPstate;
			sVAD.HPstate = hpstate;
			for (int j = 0; j < 4; j++)
			{
				num4 = Inlines.silk_RSHIFT(psEncC.frame_length, Inlines.silk_min_int(4 - j, 3));
				int num7 = Inlines.silk_RSHIFT(num4, 2);
				int num8 = 0;
				array[j] = sVAD.XnrgSubfr[j];
				for (int k = 0; k < 4; k++)
				{
					num = 0;
					for (int i = 0; i < num7; i++)
					{
						int num9 = Inlines.silk_RSHIFT((int)array4[array3[j] + i + num8], 3);
						num = Inlines.silk_SMLABB(num, num9, num9);
					}
					if (k < 3)
					{
						array[j] = Inlines.silk_ADD_POS_SAT32(array[j], num);
					}
					else
					{
						array[j] = Inlines.silk_ADD_POS_SAT32(array[j], Inlines.silk_RSHIFT(num, 1));
					}
					num8 += num7;
				}
				sVAD.XnrgSubfr[j] = num;
			}
			VoiceActivityDetection.silk_VAD_GetNoiseLevels(array, sVAD);
			num = 0;
			int num10 = 0;
			int num11;
			for (int j = 0; j < 4; j++)
			{
				num11 = array[j] - sVAD.NL[j];
				if (num11 > 0)
				{
					if (((long)array[j] & (long)((ulong)-8388608)) == 0L)
					{
						array2[j] = Inlines.silk_DIV32(Inlines.silk_LSHIFT(array[j], 8), sVAD.NL[j] + 1);
					}
					else
					{
						array2[j] = Inlines.silk_DIV32(array[j], Inlines.silk_RSHIFT(sVAD.NL[j], 8) + 1);
					}
					int num12 = Inlines.silk_lin2log(array2[j]) - 1024;
					num = Inlines.silk_SMLABB(num, num12, num12);
					if (num11 < 1048576)
					{
						num12 = Inlines.silk_SMULWB(Inlines.silk_LSHIFT(Inlines.silk_SQRT_APPROX(num11), 6), num12);
					}
					num10 = Inlines.silk_SMLAWB(num10, VoiceActivityDetection.tiltWeights[j], num12);
				}
				else
				{
					array2[j] = 256;
				}
			}
			num = Inlines.silk_DIV32_16(num, 4);
			int b = (int)((short)(3 * Inlines.silk_SQRT_APPROX(num)));
			int num13 = Sigmoid.silk_sigm_Q15(Inlines.silk_SMULWB(45000, b) - 128);
			psEncC.input_tilt_Q15 = Inlines.silk_LSHIFT(Sigmoid.silk_sigm_Q15(num10) - 16384, 1);
			num11 = 0;
			for (int j = 0; j < 4; j++)
			{
				num11 += (j + 1) * Inlines.silk_RSHIFT(array[j] - sVAD.NL[j], 4);
			}
			if (num11 <= 0)
			{
				num13 = Inlines.silk_RSHIFT(num13, 1);
			}
			else if (num11 < 32768)
			{
				if (psEncC.frame_length == 10 * psEncC.fs_kHz)
				{
					num11 = Inlines.silk_LSHIFT_SAT32(num11, 16);
				}
				else
				{
					num11 = Inlines.silk_LSHIFT_SAT32(num11, 15);
				}
				num11 = Inlines.silk_SQRT_APPROX(num11);
				num13 = Inlines.silk_SMULWB(32768 + num11, num13);
			}
			psEncC.speech_activity_Q8 = Inlines.silk_min_int(Inlines.silk_RSHIFT(num13, 7), 255);
			int num14 = Inlines.silk_SMULWB(4096, Inlines.silk_SMULWB(num13, num13));
			if (psEncC.frame_length == 10 * psEncC.fs_kHz)
			{
				num14 >>= 1;
			}
			for (int j = 0; j < 4; j++)
			{
				sVAD.NrgRatioSmth_Q8[j] = Inlines.silk_SMLAWB(sVAD.NrgRatioSmth_Q8[j], array2[j] - sVAD.NrgRatioSmth_Q8[j], num14);
				int num12 = 3 * (Inlines.silk_lin2log(sVAD.NrgRatioSmth_Q8[j]) - 1024);
				psEncC.input_quality_bands_Q15[j] = Sigmoid.silk_sigm_Q15(Inlines.silk_RSHIFT(num12 - 2048, 4));
			}
			return result;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00013E64 File Offset: 0x00012064
		internal static void silk_VAD_GetNoiseLevels(int[] pX, SilkVADState psSilk_VAD)
		{
			int b;
			if (psSilk_VAD.counter < 1000)
			{
				b = Inlines.silk_DIV32_16(32767, (int)((short)(Inlines.silk_RSHIFT(psSilk_VAD.counter, 4) + 1)));
			}
			else
			{
				b = 0;
			}
			for (int i = 0; i < 4; i++)
			{
				int num = psSilk_VAD.NL[i];
				int num2 = Inlines.silk_ADD_POS_SAT32(pX[i], psSilk_VAD.NoiseLevelBias[i]);
				int num3 = Inlines.silk_DIV32(int.MaxValue, num2);
				int num4;
				if (num2 > Inlines.silk_LSHIFT(num, 3))
				{
					num4 = 128;
				}
				else if (num2 < num)
				{
					num4 = 1024;
				}
				else
				{
					num4 = Inlines.silk_SMULWB(Inlines.silk_SMULWW(num3, num), 2048);
				}
				num4 = Inlines.silk_max_int(num4, b);
				psSilk_VAD.inv_NL[i] = Inlines.silk_SMLAWB(psSilk_VAD.inv_NL[i], num3 - psSilk_VAD.inv_NL[i], num4);
				num = Inlines.silk_DIV32(int.MaxValue, psSilk_VAD.inv_NL[i]);
				num = Inlines.silk_min(num, 16777215);
				psSilk_VAD.NL[i] = num;
			}
			psSilk_VAD.counter++;
		}

		// Token: 0x0400017F RID: 383
		private static readonly int[] tiltWeights = new int[]
		{
			30000,
			6000,
			-12000,
			-12000
		};
	}
}
