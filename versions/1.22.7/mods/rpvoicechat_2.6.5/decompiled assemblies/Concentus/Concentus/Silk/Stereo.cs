using System;
using Concentus.Common;
using Concentus.Common.CPlusPlus;
using Concentus.Silk.Structs;

namespace Concentus.Silk
{
	// Token: 0x0200003B RID: 59
	internal static class Stereo
	{
		// Token: 0x06000121 RID: 289 RVA: 0x000116B0 File Offset: 0x0000F8B0
		internal static void silk_stereo_decode_pred(EntropyCoder psRangeDec, ReadOnlySpan<byte> encodedData, int[] pred_Q13)
		{
			int[][] array = Arrays.InitTwoDimensionalArray<int>(2, 3);
			int i = psRangeDec.dec_icdf(encodedData, Tables.silk_stereo_pred_joint_iCDF, 8U);
			array[0][2] = Inlines.silk_DIV32_16(i, 5);
			array[1][2] = i - 5 * array[0][2];
			for (i = 0; i < 2; i++)
			{
				array[i][0] = psRangeDec.dec_icdf(encodedData, Tables.silk_uniform3_iCDF, 8U);
				array[i][1] = psRangeDec.dec_icdf(encodedData, Tables.silk_uniform5_iCDF, 8U);
			}
			for (i = 0; i < 2; i++)
			{
				array[i][0] += 3 * array[i][2];
				int num = (int)Tables.silk_stereo_pred_quant_Q13[array[i][0]];
				int b = Inlines.silk_SMULWB((int)Tables.silk_stereo_pred_quant_Q13[array[i][0] + 1] - num, 6554);
				pred_Q13[i] = Inlines.silk_SMLABB(num, b, 2 * array[i][1] + 1);
			}
			pred_Q13[0] -= pred_Q13[1];
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00011784 File Offset: 0x0000F984
		internal static void silk_stereo_decode_mid_only(EntropyCoder psRangeDec, ReadOnlySpan<byte> encodedData, BoxedValueInt decode_only_mid)
		{
			decode_only_mid.Val = psRangeDec.dec_icdf(encodedData, Tables.silk_stereo_only_code_mid_iCDF, 8U);
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0001179C File Offset: 0x0000F99C
		internal static void silk_stereo_encode_pred(EntropyCoder psRangeEnc, Span<byte> encodedData, sbyte[][] ix)
		{
			int i = (int)(5 * ix[0][2] + ix[1][2]);
			psRangeEnc.enc_icdf(encodedData, i, Tables.silk_stereo_pred_joint_iCDF, 8U);
			for (i = 0; i < 2; i++)
			{
				psRangeEnc.enc_icdf(encodedData, (int)ix[i][0], Tables.silk_uniform3_iCDF, 8U);
				psRangeEnc.enc_icdf(encodedData, (int)ix[i][1], Tables.silk_uniform5_iCDF, 8U);
			}
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000117F5 File Offset: 0x0000F9F5
		internal static void silk_stereo_encode_mid_only(EntropyCoder psRangeEnc, Span<byte> encodedData, sbyte mid_only_flag)
		{
			psRangeEnc.enc_icdf(encodedData, (int)mid_only_flag, Tables.silk_stereo_only_code_mid_iCDF, 8U);
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00011808 File Offset: 0x0000FA08
		internal unsafe static int silk_stereo_find_predictor(BoxedValueInt ratio_Q14, short[] x, short[] y, Span<int> mid_res_amp_Q0, int mid_res_amp_Q0_ptr, int length, int smooth_coef_Q16)
		{
			int num;
			int num2;
			SumSqrShift.silk_sum_sqr_shift(out num, out num2, x, length);
			int num3;
			int num4;
			SumSqrShift.silk_sum_sqr_shift(out num3, out num4, y, length);
			int num5 = Inlines.silk_max_int(num2, num4);
			num5 += (num5 & 1);
			num3 = Inlines.silk_RSHIFT32(num3, num5 - num4);
			num = Inlines.silk_RSHIFT32(num, num5 - num2);
			num = Inlines.silk_max_int(num, 1);
			int a = Inlines.silk_inner_prod_aligned_scale(x, y, num5, length);
			int num6 = Inlines.silk_DIV32_varQ(a, num, 13);
			num6 = Inlines.silk_LIMIT(num6, -16384, 16384);
			int num7 = Inlines.silk_SMULWB(num6, num6);
			smooth_coef_Q16 = Inlines.silk_max_int(smooth_coef_Q16, Inlines.silk_abs(num7));
			num5 = Inlines.silk_RSHIFT(num5, 1);
			*mid_res_amp_Q0[mid_res_amp_Q0_ptr] = Inlines.silk_SMLAWB(*mid_res_amp_Q0[mid_res_amp_Q0_ptr], Inlines.silk_LSHIFT(Inlines.silk_SQRT_APPROX(num), num5) - *mid_res_amp_Q0[mid_res_amp_Q0_ptr], smooth_coef_Q16);
			num3 = Inlines.silk_SUB_LSHIFT32(num3, Inlines.silk_SMULWB(a, num6), 4);
			num3 = Inlines.silk_ADD_LSHIFT32(num3, Inlines.silk_SMULWB(num, num7), 6);
			*mid_res_amp_Q0[mid_res_amp_Q0_ptr + 1] = Inlines.silk_SMLAWB(*mid_res_amp_Q0[mid_res_amp_Q0_ptr + 1], Inlines.silk_LSHIFT(Inlines.silk_SQRT_APPROX(num3), num5) - *mid_res_amp_Q0[mid_res_amp_Q0_ptr + 1], smooth_coef_Q16);
			ratio_Q14.Val = Inlines.silk_DIV32_varQ(*mid_res_amp_Q0[mid_res_amp_Q0_ptr + 1], Inlines.silk_max(*mid_res_amp_Q0[mid_res_amp_Q0_ptr], 1), 14);
			ratio_Q14.Val = Inlines.silk_LIMIT(ratio_Q14.Val, 0, 32767);
			return num6;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x00011974 File Offset: 0x0000FB74
		internal unsafe static void silk_stereo_LR_to_MS(StereoEncodeState state, Span<short> x1, int x1_ptr, Span<short> x2, int x2_ptr, sbyte[][] ix, BoxedValueSbyte mid_only_flag, int[] mid_side_rates_bps, int total_rate_bps, int prev_speech_act_Q8, int toMono, int fs_kHz, int frame_length)
		{
			int[] array = new int[2];
			BoxedValueInt boxedValueInt = new BoxedValueInt(0);
			BoxedValueInt boxedValueInt2 = new BoxedValueInt(0);
			int num = x1_ptr - 2;
			short[] array2 = new short[frame_length + 2];
			for (int i = 0; i < frame_length + 2; i++)
			{
				int num2 = (int)(*x1[x1_ptr + i - 2] + *x2[x2_ptr + i - 2]);
				int a = (int)(*x1[x1_ptr + i - 2] - *x2[x2_ptr + i - 2]);
				*x1[num + i] = (short)Inlines.silk_RSHIFT_ROUND(num2, 1);
				array2[i] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(a, 1));
			}
			MemoryExtensions.AsSpan<short>(state.sMid, 0, 2).CopyTo(x1.Slice(num));
			Arrays.MemCopy(state.sSide, 0, array2, 0, 2);
			x1.Slice(num + frame_length, 2).CopyTo(state.sMid);
			Arrays.MemCopy(array2, frame_length, state.sSide, 0, 2);
			short[] array3 = new short[frame_length];
			short[] array4 = new short[frame_length];
			for (int i = 0; i < frame_length; i++)
			{
				int num2 = Inlines.silk_RSHIFT_ROUND(Inlines.silk_ADD_LSHIFT32((int)(*x1[num + i] + *x1[num + i + 2]), (int)(*x1[num + i + 1]), 1), 2);
				array3[i] = (short)num2;
				array4[i] = (short)((int)(*x1[num + i + 1]) - num2);
			}
			short[] array5 = new short[frame_length];
			short[] array6 = new short[frame_length];
			for (int i = 0; i < frame_length; i++)
			{
				int num2 = Inlines.silk_RSHIFT_ROUND(Inlines.silk_ADD_LSHIFT32((int)(array2[i] + array2[i + 2]), (int)array2[i + 1], 1), 2);
				array5[i] = (short)num2;
				array6[i] = (short)((int)array2[i + 1] - num2);
			}
			int num3 = (frame_length == 10 * fs_kHz) ? 1 : 0;
			int num4 = (num3 != 0) ? 328 : 655;
			num4 = Inlines.silk_SMULWB(Inlines.silk_SMULBB(prev_speech_act_Q8, prev_speech_act_Q8), num4);
			array[0] = Stereo.silk_stereo_find_predictor(boxedValueInt, array3, array5, state.mid_side_amp_Q0, 0, frame_length, num4);
			array[1] = Stereo.silk_stereo_find_predictor(boxedValueInt2, array4, array6, state.mid_side_amp_Q0, 2, frame_length, num4);
			int num5 = Inlines.silk_SMLABB(boxedValueInt2.Val, boxedValueInt.Val, 3);
			num5 = Inlines.silk_min(num5, 65536);
			total_rate_bps -= ((num3 != 0) ? 1200 : 600);
			if (total_rate_bps < 1)
			{
				total_rate_bps = 1;
			}
			int num6 = Inlines.silk_SMLABB(2000, fs_kHz, 900);
			int num7 = Inlines.silk_MUL(3, num5);
			mid_side_rates_bps[0] = Inlines.silk_DIV32_varQ(total_rate_bps, 851968 + num7, 19);
			int num8;
			if (mid_side_rates_bps[0] < num6)
			{
				mid_side_rates_bps[0] = num6;
				mid_side_rates_bps[1] = total_rate_bps - mid_side_rates_bps[0];
				num8 = Inlines.silk_DIV32_varQ(Inlines.silk_LSHIFT(mid_side_rates_bps[1], 1) - num6, Inlines.silk_SMULWB(65536 + num7, num6), 16);
				num8 = Inlines.silk_LIMIT(num8, 0, 16384);
			}
			else
			{
				mid_side_rates_bps[1] = total_rate_bps - mid_side_rates_bps[0];
				num8 = 16384;
			}
			state.smth_width_Q14 = (short)Inlines.silk_SMLAWB((int)state.smth_width_Q14, num8 - (int)state.smth_width_Q14, num4);
			mid_only_flag.Val = 0;
			if (toMono != 0)
			{
				num8 = 0;
				array[0] = 0;
				array[1] = 0;
				Stereo.silk_stereo_quant_pred(array, ix);
			}
			else if (state.width_prev_Q14 == 0 && (8 * total_rate_bps < 13 * num6 || Inlines.silk_SMULWB(num5, (int)state.smth_width_Q14) < 819))
			{
				array[0] = Inlines.silk_RSHIFT(Inlines.silk_SMULBB((int)state.smth_width_Q14, array[0]), 14);
				array[1] = Inlines.silk_RSHIFT(Inlines.silk_SMULBB((int)state.smth_width_Q14, array[1]), 14);
				Stereo.silk_stereo_quant_pred(array, ix);
				num8 = 0;
				array[0] = 0;
				array[1] = 0;
				mid_side_rates_bps[0] = total_rate_bps;
				mid_side_rates_bps[1] = 0;
				mid_only_flag.Val = 1;
			}
			else if (state.width_prev_Q14 != 0 && (8 * total_rate_bps < 11 * num6 || Inlines.silk_SMULWB(num5, (int)state.smth_width_Q14) < 328))
			{
				array[0] = Inlines.silk_RSHIFT(Inlines.silk_SMULBB((int)state.smth_width_Q14, array[0]), 14);
				array[1] = Inlines.silk_RSHIFT(Inlines.silk_SMULBB((int)state.smth_width_Q14, array[1]), 14);
				Stereo.silk_stereo_quant_pred(array, ix);
				num8 = 0;
				array[0] = 0;
				array[1] = 0;
			}
			else if (state.smth_width_Q14 > 15565)
			{
				Stereo.silk_stereo_quant_pred(array, ix);
				num8 = 16384;
			}
			else
			{
				array[0] = Inlines.silk_RSHIFT(Inlines.silk_SMULBB((int)state.smth_width_Q14, array[0]), 14);
				array[1] = Inlines.silk_RSHIFT(Inlines.silk_SMULBB((int)state.smth_width_Q14, array[1]), 14);
				Stereo.silk_stereo_quant_pred(array, ix);
				num8 = (int)state.smth_width_Q14;
			}
			if (mid_only_flag.Val == 1)
			{
				state.silent_side_len += (short)(frame_length - 8 * fs_kHz);
				if ((int)state.silent_side_len < 5 * fs_kHz)
				{
					mid_only_flag.Val = 0;
				}
				else
				{
					state.silent_side_len = 10000;
				}
			}
			else
			{
				state.silent_side_len = 0;
			}
			if (mid_only_flag.Val == 0 && mid_side_rates_bps[1] < 1)
			{
				mid_side_rates_bps[1] = 1;
				mid_side_rates_bps[0] = Inlines.silk_max_int(1, total_rate_bps - mid_side_rates_bps[1]);
			}
			int num9 = (int)(-(int)state.pred_prev_Q13[0]);
			int num10 = (int)(-(int)state.pred_prev_Q13[1]);
			int num11 = Inlines.silk_LSHIFT((int)state.width_prev_Q14, 10);
			int b = Inlines.silk_DIV32_16(65536, 8 * fs_kHz);
			int num12 = 0 - Inlines.silk_RSHIFT_ROUND(Inlines.silk_SMULBB(array[0] - (int)state.pred_prev_Q13[0], b), 16);
			int num13 = 0 - Inlines.silk_RSHIFT_ROUND(Inlines.silk_SMULBB(array[1] - (int)state.pred_prev_Q13[1], b), 16);
			int num14 = Inlines.silk_LSHIFT(Inlines.silk_SMULWB(num8 - (int)state.width_prev_Q14, b), 10);
			for (int i = 0; i < 8 * fs_kHz; i++)
			{
				num9 += num12;
				num10 += num13;
				num11 += num14;
				int num2 = Inlines.silk_LSHIFT(Inlines.silk_ADD_LSHIFT((int)(*x1[num + i] + *x1[num + i + 2]), (int)(*x1[num + i + 1]), 1), 9);
				num2 = Inlines.silk_SMLAWB(Inlines.silk_SMULWB(num11, (int)array2[i + 1]), num2, num9);
				num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_LSHIFT((int)(*x1[num + i + 1]), 11), num10);
				*x2[x2_ptr + i - 1] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(num2, 8));
			}
			num9 = 0 - array[0];
			num10 = 0 - array[1];
			num11 = Inlines.silk_LSHIFT(num8, 10);
			for (int i = 8 * fs_kHz; i < frame_length; i++)
			{
				int num2 = Inlines.silk_LSHIFT(Inlines.silk_ADD_LSHIFT((int)(*x1[num + i] + *x1[num + i + 2]), (int)(*x1[num + i + 1]), 1), 9);
				num2 = Inlines.silk_SMLAWB(Inlines.silk_SMULWB(num11, (int)array2[i + 1]), num2, num9);
				num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_LSHIFT((int)(*x1[num + i + 1]), 11), num10);
				*x2[x2_ptr + i - 1] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(num2, 8));
			}
			state.pred_prev_Q13[0] = (short)array[0];
			state.pred_prev_Q13[1] = (short)array[1];
			state.width_prev_Q14 = (short)num8;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000120B4 File Offset: 0x000102B4
		internal unsafe static void silk_stereo_MS_to_LR(StereoDecodeState state, Span<short> x1, int x1_ptr, Span<short> x2, int x2_ptr, int[] pred_Q13, int fs_kHz, int frame_length)
		{
			MemoryExtensions.AsSpan<short>(state.sMid, 0, 2).CopyTo(x1.Slice(x1_ptr));
			MemoryExtensions.AsSpan<short>(state.sSide, 0, 2).CopyTo(x2.Slice(x2_ptr));
			x1.Slice(x1_ptr + frame_length, 2).CopyTo(state.sMid);
			x2.Slice(x2_ptr + frame_length, 2).CopyTo(state.sSide);
			int num = (int)state.pred_prev_Q13[0];
			int num2 = (int)state.pred_prev_Q13[1];
			int b = Inlines.silk_DIV32_16(65536, 8 * fs_kHz);
			int num3 = Inlines.silk_RSHIFT_ROUND(Inlines.silk_SMULBB(pred_Q13[0] - (int)state.pred_prev_Q13[0], b), 16);
			int num4 = Inlines.silk_RSHIFT_ROUND(Inlines.silk_SMULBB(pred_Q13[1] - (int)state.pred_prev_Q13[1], b), 16);
			for (int i = 0; i < 8 * fs_kHz; i++)
			{
				num += num3;
				num2 += num4;
				int num5 = Inlines.silk_LSHIFT(Inlines.silk_ADD_LSHIFT((int)(*x1[x1_ptr + i] + *x1[x1_ptr + i + 2]), (int)(*x1[x1_ptr + i + 1]), 1), 9);
				num5 = Inlines.silk_SMLAWB(Inlines.silk_LSHIFT((int)(*x2[x2_ptr + i + 1]), 8), num5, num);
				num5 = Inlines.silk_SMLAWB(num5, Inlines.silk_LSHIFT((int)(*x1[x1_ptr + i + 1]), 11), num2);
				*x2[x2_ptr + i + 1] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(num5, 8));
			}
			num = pred_Q13[0];
			num2 = pred_Q13[1];
			for (int i = 8 * fs_kHz; i < frame_length; i++)
			{
				int num5 = Inlines.silk_LSHIFT(Inlines.silk_ADD_LSHIFT((int)(*x1[x1_ptr + i] + *x1[x1_ptr + i + 2]), (int)(*x1[x1_ptr + i + 1]), 1), 9);
				num5 = Inlines.silk_SMLAWB(Inlines.silk_LSHIFT((int)(*x2[x2_ptr + i + 1]), 8), num5, num);
				num5 = Inlines.silk_SMLAWB(num5, Inlines.silk_LSHIFT((int)(*x1[x1_ptr + i + 1]), 11), num2);
				*x2[x2_ptr + i + 1] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(num5, 8));
			}
			state.pred_prev_Q13[0] = (short)pred_Q13[0];
			state.pred_prev_Q13[1] = (short)pred_Q13[1];
			for (int i = 0; i < frame_length; i++)
			{
				int num5 = (int)(*x1[x1_ptr + i + 1] + *x2[x2_ptr + i + 1]);
				int a = (int)(*x1[x1_ptr + i + 1] - *x2[x2_ptr + i + 1]);
				*x1[x1_ptr + i + 1] = (short)Inlines.silk_SAT16(num5);
				*x2[x2_ptr + i + 1] = (short)Inlines.silk_SAT16(a);
			}
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00012388 File Offset: 0x00010588
		internal static void silk_stereo_quant_pred(int[] pred_Q13, sbyte[][] ix)
		{
			int num = 0;
			Arrays.MemSetSbyte(ix[0], 0, 3);
			Arrays.MemSetSbyte(ix[1], 0, 3);
			for (int i = 0; i < 2; i++)
			{
				int num2 = int.MaxValue;
				for (sbyte b = 0; b < 15; b += 1)
				{
					int num3 = (int)Tables.silk_stereo_pred_quant_Q13[(int)b];
					int b2 = Inlines.silk_SMULWB((int)Tables.silk_stereo_pred_quant_Q13[(int)(b + 1)] - num3, 6554);
					for (sbyte b3 = 0; b3 < 5; b3 += 1)
					{
						int num4 = Inlines.silk_SMLABB(num3, b2, (int)(2 * b3 + 1));
						int num5 = Inlines.silk_abs(pred_Q13[i] - num4);
						if (num5 >= num2)
						{
							goto IL_95;
						}
						num2 = num5;
						num = num4;
						ix[i][0] = b;
						ix[i][1] = b3;
					}
				}
				IL_95:
				ix[i][2] = (sbyte)Inlines.silk_DIV32_16((int)ix[i][0], 3);
				ix[i][0] = ix[i][0] - ix[i][2] * 3;
				pred_Q13[i] = num;
			}
			pred_Q13[0] -= pred_Q13[1];
		}
	}
}
