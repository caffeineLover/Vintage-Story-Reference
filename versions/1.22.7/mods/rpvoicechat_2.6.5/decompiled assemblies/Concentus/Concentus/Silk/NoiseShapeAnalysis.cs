using System;
using Concentus.Common;
using Concentus.Common.CPlusPlus;
using Concentus.Silk.Structs;

namespace Concentus.Silk
{
	// Token: 0x0200002E RID: 46
	internal static class NoiseShapeAnalysis
	{
		// Token: 0x060000F6 RID: 246 RVA: 0x0000D16C File Offset: 0x0000B36C
		internal static int warped_gain(int[] coefs_Q24, int lambda_Q16, int order)
		{
			lambda_Q16 = -lambda_Q16;
			int b = coefs_Q24[order - 1];
			for (int i = order - 2; i >= 0; i--)
			{
				b = Inlines.silk_SMLAWB(coefs_Q24[i], b, lambda_Q16);
			}
			b = Inlines.silk_SMLAWB(16777216, b, -lambda_Q16);
			return Inlines.silk_INVERSE32_varQ(b, 40);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000D1B4 File Offset: 0x0000B3B4
		internal static void limit_warped_coefs(int[] coefs_syn_Q24, int[] coefs_ana_Q24, int lambda_Q16, int limit_Q24, int order)
		{
			int num = 0;
			lambda_Q16 = -lambda_Q16;
			for (int i = order - 1; i > 0; i--)
			{
				coefs_syn_Q24[i - 1] = Inlines.silk_SMLAWB(coefs_syn_Q24[i - 1], coefs_syn_Q24[i], lambda_Q16);
				coefs_ana_Q24[i - 1] = Inlines.silk_SMLAWB(coefs_ana_Q24[i - 1], coefs_ana_Q24[i], lambda_Q16);
			}
			lambda_Q16 = -lambda_Q16;
			int a = Inlines.silk_SMLAWB(65536, -lambda_Q16, lambda_Q16);
			int b = Inlines.silk_SMLAWB(16777216, coefs_syn_Q24[0], lambda_Q16);
			int num2 = Inlines.silk_DIV32_varQ(a, b, 24);
			b = Inlines.silk_SMLAWB(16777216, coefs_ana_Q24[0], lambda_Q16);
			int num3 = Inlines.silk_DIV32_varQ(a, b, 24);
			for (int i = 0; i < order; i++)
			{
				coefs_syn_Q24[i] = Inlines.silk_SMULWW(num2, coefs_syn_Q24[i]);
				coefs_ana_Q24[i] = Inlines.silk_SMULWW(num3, coefs_ana_Q24[i]);
			}
			for (int j = 0; j < 10; j++)
			{
				int num4 = -1;
				for (int i = 0; i < order; i++)
				{
					int num5 = Inlines.silk_max(Inlines.silk_abs_int32(coefs_syn_Q24[i]), Inlines.silk_abs_int32(coefs_ana_Q24[i]));
					if (num5 > num4)
					{
						num4 = num5;
						num = i;
					}
				}
				if (num4 <= limit_Q24)
				{
					return;
				}
				for (int i = 1; i < order; i++)
				{
					coefs_syn_Q24[i - 1] = Inlines.silk_SMLAWB(coefs_syn_Q24[i - 1], coefs_syn_Q24[i], lambda_Q16);
					coefs_ana_Q24[i - 1] = Inlines.silk_SMLAWB(coefs_ana_Q24[i - 1], coefs_ana_Q24[i], lambda_Q16);
				}
				num2 = Inlines.silk_INVERSE32_varQ(num2, 32);
				num3 = Inlines.silk_INVERSE32_varQ(num3, 32);
				for (int i = 0; i < order; i++)
				{
					coefs_syn_Q24[i] = Inlines.silk_SMULWW(num2, coefs_syn_Q24[i]);
					coefs_ana_Q24[i] = Inlines.silk_SMULWW(num3, coefs_ana_Q24[i]);
				}
				int chirp_Q = 64881 - Inlines.silk_DIV32_varQ(Inlines.silk_SMULWB(num4 - limit_Q24, Inlines.silk_SMLABB(819, 102, j)), Inlines.silk_MUL(num4, num + 1), 22);
				BWExpander.silk_bwexpander_32(coefs_syn_Q24, order, chirp_Q);
				BWExpander.silk_bwexpander_32(coefs_ana_Q24, order, chirp_Q);
				lambda_Q16 = -lambda_Q16;
				for (int i = order - 1; i > 0; i--)
				{
					coefs_syn_Q24[i - 1] = Inlines.silk_SMLAWB(coefs_syn_Q24[i - 1], coefs_syn_Q24[i], lambda_Q16);
					coefs_ana_Q24[i - 1] = Inlines.silk_SMLAWB(coefs_ana_Q24[i - 1], coefs_ana_Q24[i], lambda_Q16);
				}
				lambda_Q16 = -lambda_Q16;
				int a2 = Inlines.silk_SMLAWB(65536, -lambda_Q16, lambda_Q16);
				b = Inlines.silk_SMLAWB(16777216, coefs_syn_Q24[0], lambda_Q16);
				num2 = Inlines.silk_DIV32_varQ(a2, b, 24);
				b = Inlines.silk_SMLAWB(16777216, coefs_ana_Q24[0], lambda_Q16);
				num3 = Inlines.silk_DIV32_varQ(a2, b, 24);
				for (int i = 0; i < order; i++)
				{
					coefs_syn_Q24[i] = Inlines.silk_SMULWW(num2, coefs_syn_Q24[i]);
					coefs_ana_Q24[i] = Inlines.silk_SMULWW(num3, coefs_ana_Q24[i]);
				}
			}
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000D410 File Offset: 0x0000B610
		internal static void silk_noise_shape_analysis(SilkChannelEncoder psEnc, SilkEncoderControl psEncCtrl, short[] pitch_res, int pitch_res_ptr, short[] x, int x_ptr)
		{
			SilkShapeState sShape = psEnc.sShape;
			int num = 0;
			int[] array = new int[17];
			int[] rc_Q = new int[16];
			int[] array2 = new int[16];
			int[] array3 = new int[16];
			int num2 = x_ptr - psEnc.la_shape;
			int num3 = psEnc.SNR_dB_Q7;
			psEncCtrl.input_quality_Q14 = Inlines.silk_RSHIFT(psEnc.input_quality_bands_Q15[0] + psEnc.input_quality_bands_Q15[1], 2);
			psEncCtrl.coding_quality_Q14 = Inlines.silk_RSHIFT(Sigmoid.silk_sigm_Q15(Inlines.silk_RSHIFT_ROUND(num3 - 2560, 4)), 1);
			if (psEnc.useCBR == 0)
			{
				int num4 = 256 - psEnc.speech_activity_Q8;
				num4 = Inlines.silk_SMULWB(Inlines.silk_LSHIFT(num4, 8), num4);
				num3 = Inlines.silk_SMLAWB(num3, Inlines.silk_SMULBB(-8, num4), Inlines.silk_SMULWB(16384 + psEncCtrl.input_quality_Q14, psEncCtrl.coding_quality_Q14));
			}
			if (psEnc.indices.signalType == 2)
			{
				num3 = Inlines.silk_SMLAWB(num3, 512, psEnc.LTPCorr_Q15);
			}
			else
			{
				num3 = Inlines.silk_SMLAWB(num3, Inlines.silk_SMLAWB(3072, -104858, psEnc.SNR_dB_Q7), 16384 - psEncCtrl.input_quality_Q14);
			}
			if (psEnc.indices.signalType == 2)
			{
				psEnc.indices.quantOffsetType = 0;
				psEncCtrl.sparseness_Q8 = 0;
			}
			else
			{
				int num5 = Inlines.silk_LSHIFT(psEnc.fs_kHz, 1);
				int num6 = 0;
				int num7 = 0;
				int num8 = pitch_res_ptr;
				for (int i = 0; i < Inlines.silk_SMULBB(5, psEnc.nb_subfr) / 2; i++)
				{
					int num9;
					SumSqrShift.silk_sum_sqr_shift(out num9, out num, pitch_res, num8, num5);
					num9 += Inlines.silk_RSHIFT(num5, num);
					int num10 = Inlines.silk_lin2log(num9);
					if (i > 0)
					{
						num6 += Inlines.silk_abs(num10 - num7);
					}
					num7 = num10;
					num8 += num5;
				}
				psEncCtrl.sparseness_Q8 = Inlines.silk_RSHIFT(Sigmoid.silk_sigm_Q15(Inlines.silk_SMULWB(num6 - 640, 6554)), 7);
				if (psEncCtrl.sparseness_Q8 > 192)
				{
					psEnc.indices.quantOffsetType = 0;
				}
				else
				{
					psEnc.indices.quantOffsetType = 1;
				}
				num3 = Inlines.silk_SMLAWB(num3, 65536, psEncCtrl.sparseness_Q8 - 128);
			}
			int num11 = Inlines.silk_SMULWB(psEncCtrl.predGain_Q16, 66);
			int num13;
			int num12 = num13 = Inlines.silk_DIV32_varQ(62259, Inlines.silk_SMLAWW(65536, num11, num11), 16);
			int b = Inlines.silk_SMULWB(65536 - Inlines.silk_SMULBB(3, psEncCtrl.coding_quality_Q14), 655);
			num13 = Inlines.silk_SUB32(num13, b);
			num12 = Inlines.silk_ADD32(num12, b);
			num13 = Inlines.silk_DIV32_16(Inlines.silk_LSHIFT(num13, 14), Inlines.silk_RSHIFT(num12, 2));
			int num14;
			if (psEnc.warping_Q16 > 0)
			{
				num14 = Inlines.silk_SMLAWB(psEnc.warping_Q16, psEncCtrl.coding_quality_Q14, 2621);
			}
			else
			{
				num14 = 0;
			}
			short[] array4 = new short[psEnc.shapeWinLength];
			int num19;
			for (int i = 0; i < psEnc.nb_subfr; i++)
			{
				int num15 = psEnc.fs_kHz * 3;
				int num16 = Inlines.silk_RSHIFT(psEnc.shapeWinLength - num15, 1);
				ApplySineWindow.silk_apply_sine_window(array4, 0, x, num2, 1, num16);
				int num17 = num16;
				Arrays.MemCopy(x, num2 + num17, array4, num17, num15);
				num17 += num15;
				ApplySineWindow.silk_apply_sine_window(array4, num17, x, num2 + num17, 2, num16);
				num2 += psEnc.subfr_length;
				if (psEnc.warping_Q16 > 0)
				{
					Autocorrelation.silk_warped_autocorrelation(array, out num, array4, num14, psEnc.shapeWinLength, psEnc.shapingLPCOrder);
				}
				else
				{
					Autocorrelation.silk_autocorr(array, out num, array4, psEnc.shapeWinLength, psEnc.shapingLPCOrder + 1);
				}
				array[0] = Inlines.silk_ADD32(array[0], Inlines.silk_max_32(Inlines.silk_SMULWB(Inlines.silk_RSHIFT(array[0], 4), 52), 1));
				int num9 = Schur.silk_schur64(rc_Q, array, psEnc.shapingLPCOrder);
				K2A.silk_k2a_Q16(array3, rc_Q, psEnc.shapingLPCOrder);
				int num18 = -num;
				if ((num18 & 1) != 0)
				{
					num18--;
					num9 >>= 1;
				}
				int a = Inlines.silk_SQRT_APPROX(num9);
				num18 >>= 1;
				psEncCtrl.Gains_Q16[i] = Inlines.silk_LSHIFT_SAT32(a, 16 - num18);
				if (psEnc.warping_Q16 > 0)
				{
					num19 = NoiseShapeAnalysis.warped_gain(array3, num14, psEnc.shapingLPCOrder);
					if (Inlines.silk_SMULWW(Inlines.silk_RSHIFT_ROUND(psEncCtrl.Gains_Q16[i], 1), num19) >= 1073741823)
					{
						psEncCtrl.Gains_Q16[i] = int.MaxValue;
					}
					else
					{
						psEncCtrl.Gains_Q16[i] = Inlines.silk_SMULWW(psEncCtrl.Gains_Q16[i], num19);
					}
				}
				BWExpander.silk_bwexpander_32(array3, psEnc.shapingLPCOrder, num12);
				Arrays.MemCopy(array3, 0, array2, 0, psEnc.shapingLPCOrder);
				BWExpander.silk_bwexpander_32(array2, psEnc.shapingLPCOrder, num13);
				int a2 = LPCInversePredGain.silk_LPC_inverse_pred_gain_Q24(array3, psEnc.shapingLPCOrder);
				num9 = LPCInversePredGain.silk_LPC_inverse_pred_gain_Q24(array2, psEnc.shapingLPCOrder);
				a2 = Inlines.silk_LSHIFT32(Inlines.silk_SMULWB(a2, 22938), 1);
				psEncCtrl.GainsPre_Q14[i] = 4915 + Inlines.silk_DIV32_varQ(a2, num9, 14);
				NoiseShapeAnalysis.limit_warped_coefs(array3, array2, num14, 67092088, psEnc.shapingLPCOrder);
				for (int j = 0; j < psEnc.shapingLPCOrder; j++)
				{
					psEncCtrl.AR1_Q13[i * 16 + j] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(array2[j], 11));
					psEncCtrl.AR2_Q13[i * 16 + j] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(array3[j], 11));
				}
			}
			num19 = Inlines.silk_log2lin(-Inlines.silk_SMLAWB(-2048, num3, 10486));
			int b2 = Inlines.silk_log2lin(Inlines.silk_SMLAWB(2048, 256, 10486));
			for (int i = 0; i < psEnc.nb_subfr; i++)
			{
				psEncCtrl.Gains_Q16[i] = Inlines.silk_SMULWW(psEncCtrl.Gains_Q16[i], num19);
				psEncCtrl.Gains_Q16[i] = Inlines.silk_ADD_POS_SAT32(psEncCtrl.Gains_Q16[i], b2);
			}
			num19 = 65536 + Inlines.silk_RSHIFT_ROUND(Inlines.silk_MLA(3355443, psEncCtrl.coding_quality_Q14, 410), 10);
			for (int i = 0; i < psEnc.nb_subfr; i++)
			{
				psEncCtrl.GainsPre_Q14[i] = Inlines.silk_SMULWB(num19, psEncCtrl.GainsPre_Q14[i]);
			}
			num11 = Inlines.silk_MUL(64, Inlines.silk_SMLAWB(4096, 4096, psEnc.input_quality_bands_Q15[0] - 32768));
			num11 = Inlines.silk_RSHIFT(Inlines.silk_MUL(num11, psEnc.speech_activity_Q8), 8);
			int num22;
			if (psEnc.indices.signalType == 2)
			{
				int num20 = Inlines.silk_DIV32_16(3277, psEnc.fs_kHz);
				for (int i = 0; i < psEnc.nb_subfr; i++)
				{
					int num21 = num20 + Inlines.silk_DIV32_16(49152, psEncCtrl.pitchL[i]);
					psEncCtrl.LF_shp_Q14[i] = Inlines.silk_LSHIFT(16384 - num21 - Inlines.silk_SMULWB(num11, num21), 16);
					psEncCtrl.LF_shp_Q14[i] |= (num21 - 16384 & 65535);
				}
				num22 = -16384 - Inlines.silk_SMULWB(49152, Inlines.silk_SMULWB(5872026, psEnc.speech_activity_Q8));
			}
			else
			{
				int num21 = Inlines.silk_DIV32_16(21299, psEnc.fs_kHz);
				psEncCtrl.LF_shp_Q14[0] = Inlines.silk_LSHIFT(16384 - num21 - Inlines.silk_SMULWB(num11, Inlines.silk_SMULWB(39322, num21)), 16);
				psEncCtrl.LF_shp_Q14[0] |= (num21 - 16384 & 65535);
				for (int i = 1; i < psEnc.nb_subfr; i++)
				{
					psEncCtrl.LF_shp_Q14[i] = psEncCtrl.LF_shp_Q14[0];
				}
				num22 = -16384;
			}
			int num23 = Inlines.silk_SMULWB(Inlines.silk_SMULWB(131072 - Inlines.silk_LSHIFT(psEncCtrl.coding_quality_Q14, 3), psEnc.LTPCorr_Q15), 6554);
			num23 = Inlines.silk_SMLAWB(num23, 65536 - Inlines.silk_LSHIFT(psEncCtrl.input_quality_Q14, 2), 6554);
			int num24;
			if (psEnc.indices.signalType == 2)
			{
				num24 = Inlines.silk_SMLAWB(19661, 65536 - Inlines.silk_SMULWB(262144 - Inlines.silk_LSHIFT(psEncCtrl.coding_quality_Q14, 4), psEncCtrl.input_quality_Q14), 13107);
				num24 = Inlines.silk_SMULWB(Inlines.silk_LSHIFT(num24, 1), Inlines.silk_SQRT_APPROX(Inlines.silk_LSHIFT(psEnc.LTPCorr_Q15, 15)));
			}
			else
			{
				num24 = 0;
			}
			for (int i = 0; i < 4; i++)
			{
				sShape.HarmBoost_smth_Q16 = Inlines.silk_SMLAWB(sShape.HarmBoost_smth_Q16, num23 - sShape.HarmBoost_smth_Q16, 26214);
				sShape.HarmShapeGain_smth_Q16 = Inlines.silk_SMLAWB(sShape.HarmShapeGain_smth_Q16, num24 - sShape.HarmShapeGain_smth_Q16, 26214);
				sShape.Tilt_smth_Q16 = Inlines.silk_SMLAWB(sShape.Tilt_smth_Q16, num22 - sShape.Tilt_smth_Q16, 26214);
				psEncCtrl.HarmBoost_Q14[i] = Inlines.silk_RSHIFT_ROUND(sShape.HarmBoost_smth_Q16, 2);
				psEncCtrl.HarmShapeGain_Q14[i] = Inlines.silk_RSHIFT_ROUND(sShape.HarmShapeGain_smth_Q16, 2);
				psEncCtrl.Tilt_Q14[i] = Inlines.silk_RSHIFT_ROUND(sShape.Tilt_smth_Q16, 2);
			}
		}
	}
}
