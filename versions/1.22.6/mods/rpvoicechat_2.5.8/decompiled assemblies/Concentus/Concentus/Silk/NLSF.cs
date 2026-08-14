using System;
using Concentus.Common;
using Concentus.Common.CPlusPlus;
using Concentus.Silk.Structs;

namespace Concentus.Silk
{
	// Token: 0x0200002D RID: 45
	internal static class NLSF
	{
		// Token: 0x060000E6 RID: 230 RVA: 0x0000BAF8 File Offset: 0x00009CF8
		internal static void silk_NLSF_VQ(int[] err_Q26, short[] in_Q15, byte[] pCB_Q8, int K, int LPC_order)
		{
			int num = 0;
			for (int i = 0; i < K; i++)
			{
				int num2 = 0;
				for (int j = 0; j < LPC_order; j += 2)
				{
					int num3 = Inlines.silk_SUB_LSHIFT32((int)in_Q15[j], (int)pCB_Q8[num++], 7);
					int num4 = Inlines.silk_SMULBB(num3, num3);
					num3 = Inlines.silk_SUB_LSHIFT32((int)in_Q15[j + 1], (int)pCB_Q8[num++], 7);
					num4 = Inlines.silk_SMLABB(num4, num3, num3);
					num2 = Inlines.silk_ADD_RSHIFT32(num2, num4, 4);
				}
				err_Q26[i] = num2;
			}
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0000BB70 File Offset: 0x00009D70
		internal unsafe static void silk_NLSF_VQ_weights_laroia(Span<short> pNLSFW_Q_OUT, Span<short> pNLSF_Q15, int D)
		{
			int num = Inlines.silk_max_int((int)(*pNLSF_Q15[0]), 1);
			num = Inlines.silk_DIV32(131072, num);
			int num2 = Inlines.silk_max_int((int)(*pNLSF_Q15[1] - *pNLSF_Q15[0]), 1);
			num2 = Inlines.silk_DIV32(131072, num2);
			*pNLSFW_Q_OUT[0] = (short)Inlines.silk_min_int(num + num2, 32767);
			for (int i = 1; i < D - 1; i += 2)
			{
				num = Inlines.silk_max_int((int)(*pNLSF_Q15[i + 1] - *pNLSF_Q15[i]), 1);
				num = Inlines.silk_DIV32(131072, num);
				*pNLSFW_Q_OUT[i] = (short)Inlines.silk_min_int(num + num2, 32767);
				num2 = Inlines.silk_max_int((int)(*pNLSF_Q15[i + 2] - *pNLSF_Q15[i + 1]), 1);
				num2 = Inlines.silk_DIV32(131072, num2);
				*pNLSFW_Q_OUT[i + 1] = (short)Inlines.silk_min_int(num + num2, 32767);
			}
			num = Inlines.silk_max_int(32768 - (int)(*pNLSF_Q15[D - 1]), 1);
			num = Inlines.silk_DIV32(131072, num);
			*pNLSFW_Q_OUT[D - 1] = (short)Inlines.silk_min_int(num + num2, 32767);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0000BCAC File Offset: 0x00009EAC
		internal unsafe static void silk_NLSF_residual_dequant(Span<short> x_Q10, Span<sbyte> indices, Span<byte> pred_coef_Q8, int quant_step_size_Q16, short order)
		{
			short num = 0;
			for (int i = (int)(order - 1); i >= 0; i--)
			{
				int a = Inlines.silk_RSHIFT(Inlines.silk_SMULBB((int)num, (int)(*pred_coef_Q8[i])), 8);
				num = Inlines.silk_LSHIFT16((short)(*indices[i]), 10);
				if (num > 0)
				{
					num = Inlines.silk_SUB16(num, 102);
				}
				else if (num < 0)
				{
					num = Inlines.silk_ADD16(num, 102);
				}
				num = (short)Inlines.silk_SMLAWB(a, (int)num, quant_step_size_Q16);
				*x_Q10[i] = num;
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0000BD20 File Offset: 0x00009F20
		internal unsafe static void silk_NLSF_unpack(Span<short> ec_ix, Span<byte> pred_Q8, NLSFCodebook psNLSF_CB, int CB1_index)
		{
			byte[] ec_sel = psNLSF_CB.ec_sel;
			int num = CB1_index * (int)psNLSF_CB.order / 2;
			for (int i = 0; i < (int)psNLSF_CB.order; i += 2)
			{
				byte b = ec_sel[num];
				num++;
				*ec_ix[i] = (short)Inlines.silk_SMULBB(Inlines.silk_RSHIFT((int)b, 1) & 7, 9);
				*pred_Q8[i] = psNLSF_CB.pred_Q8[i + (int)((short)(b & 1) * (psNLSF_CB.order - 1))];
				*ec_ix[i + 1] = (short)Inlines.silk_SMULBB(Inlines.silk_RSHIFT((int)b, 5) & 7, 9);
				*pred_Q8[i + 1] = psNLSF_CB.pred_Q8[i + (Inlines.silk_RSHIFT((int)b, 4) & 1) * (int)(psNLSF_CB.order - 1) + 1];
			}
		}

		// Token: 0x060000EA RID: 234 RVA: 0x0000BDDC File Offset: 0x00009FDC
		internal static void silk_NLSF_stabilize(short[] NLSF_Q15, short[] NDeltaMin_Q15, int L)
		{
			int i;
			for (i = 0; i < 20; i++)
			{
				int num = (int)(NLSF_Q15[0] - NDeltaMin_Q15[0]);
				int num2 = 0;
				int num3;
				for (int j = 1; j <= L - 1; j++)
				{
					num3 = (int)(NLSF_Q15[j] - (NLSF_Q15[j - 1] + NDeltaMin_Q15[j]));
					if (num3 < num)
					{
						num = num3;
						num2 = j;
					}
				}
				num3 = 32768 - (int)(NLSF_Q15[L - 1] + NDeltaMin_Q15[L]);
				if (num3 < num)
				{
					num = num3;
					num2 = L;
				}
				if (num >= 0)
				{
					return;
				}
				if (num2 == 0)
				{
					NLSF_Q15[0] = NDeltaMin_Q15[0];
				}
				else if (num2 == L)
				{
					NLSF_Q15[L - 1] = (short)(32768 - (int)NDeltaMin_Q15[L]);
				}
				else
				{
					int num4 = 0;
					for (int k = 0; k < num2; k++)
					{
						num4 += (int)NDeltaMin_Q15[k];
					}
					num4 += Inlines.silk_RSHIFT((int)NDeltaMin_Q15[num2], 1);
					int num5 = 32768;
					for (int k = L; k > num2; k--)
					{
						num5 -= (int)NDeltaMin_Q15[k];
					}
					num5 -= Inlines.silk_RSHIFT((int)NDeltaMin_Q15[num2], 1);
					short num6 = (short)Inlines.silk_LIMIT_32(Inlines.silk_RSHIFT_ROUND((int)(NLSF_Q15[num2 - 1] + NLSF_Q15[num2]), 1), num4, num5);
					NLSF_Q15[num2 - 1] = (short)((int)num6 - Inlines.silk_RSHIFT((int)NDeltaMin_Q15[num2], 1));
					NLSF_Q15[num2] = NLSF_Q15[num2 - 1] + NDeltaMin_Q15[num2];
				}
			}
			if (i == 20)
			{
				Sort.silk_insertion_sort_increasing_all_values_int16(NLSF_Q15, L);
				NLSF_Q15[0] = (short)Inlines.silk_max_int((int)NLSF_Q15[0], (int)NDeltaMin_Q15[0]);
				for (int j = 1; j < L; j++)
				{
					NLSF_Q15[j] = (short)Inlines.silk_max_int((int)NLSF_Q15[j], (int)(NLSF_Q15[j - 1] + NDeltaMin_Q15[j]));
				}
				NLSF_Q15[L - 1] = (short)Inlines.silk_min_int((int)NLSF_Q15[L - 1], 32768 - (int)NDeltaMin_Q15[L]);
				for (int j = L - 2; j >= 0; j--)
				{
					NLSF_Q15[j] = (short)Inlines.silk_min_int((int)NLSF_Q15[j], (int)(NLSF_Q15[j + 1] - NDeltaMin_Q15[j + 1]));
				}
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0000BF80 File Offset: 0x0000A180
		internal unsafe static void silk_NLSF_decode(short[] pNLSF_Q15, sbyte[] NLSFIndices, NLSFCodebook psNLSF_CB)
		{
			Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)16], 16);
			Span<short> ec_ix = new Span<short>(stackalloc byte[(UIntPtr)32], 16);
			Span<short> x_Q = new Span<short>(stackalloc byte[(UIntPtr)32], 16);
			Span<short> pNLSFW_Q_OUT = new Span<short>(stackalloc byte[(UIntPtr)32], 16);
			byte[] cb1_NLSF_Q = psNLSF_CB.CB1_NLSF_Q8;
			int num = (int)((short)NLSFIndices[0] * psNLSF_CB.order);
			for (int i = 0; i < (int)psNLSF_CB.order; i++)
			{
				pNLSF_Q15[i] = Inlines.silk_LSHIFT16((short)cb1_NLSF_Q[num + i], 7);
			}
			NLSF.silk_NLSF_unpack(ec_ix, span, psNLSF_CB, (int)NLSFIndices[0]);
			NLSF.silk_NLSF_residual_dequant(x_Q, MemoryExtensions.AsSpan<sbyte>(NLSFIndices).Slice(1), span, (int)psNLSF_CB.quantStepSize_Q16, psNLSF_CB.order);
			NLSF.silk_NLSF_VQ_weights_laroia(pNLSFW_Q_OUT, pNLSF_Q15, (int)psNLSF_CB.order);
			for (int i = 0; i < (int)psNLSF_CB.order; i++)
			{
				int num2 = Inlines.silk_SQRT_APPROX(Inlines.silk_LSHIFT((int)(*pNLSFW_Q_OUT[i]), 16));
				int a = Inlines.silk_ADD32((int)pNLSF_Q15[i], Inlines.silk_DIV32_16(Inlines.silk_LSHIFT((int)(*x_Q[i]), 14), (int)((short)num2)));
				pNLSF_Q15[i] = (short)Inlines.silk_LIMIT(a, 0, 32767);
			}
			NLSF.silk_NLSF_stabilize(pNLSF_Q15, psNLSF_CB.deltaMin_Q15, (int)psNLSF_CB.order);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0000C0A8 File Offset: 0x0000A2A8
		internal unsafe static int silk_NLSF_del_dec_quant(sbyte[] indices, Span<short> x_Q10, Span<short> w_Q5, Span<byte> pred_coef_Q8, Span<short> ec_ix, byte[] ec_rates_Q5, int quant_step_size_Q16, short inv_quant_step_size_Q6, int mu_Q20, short order)
		{
			Span<int> span = new Span<int>(stackalloc byte[(UIntPtr)16], 4);
			sbyte[][] array = new sbyte[4][];
			int i;
			for (i = 0; i < 4; i++)
			{
				array[i] = new sbyte[16];
			}
			Span<short> span2 = new Span<short>(stackalloc byte[(UIntPtr)16], 8);
			Span<int> span3 = new Span<int>(stackalloc byte[(UIntPtr)32], 8);
			Span<int> span4 = new Span<int>(stackalloc byte[(UIntPtr)16], 4);
			Span<int> span5 = new Span<int>(stackalloc byte[(UIntPtr)16], 4);
			Span<int> span6 = new Span<int>(stackalloc byte[(UIntPtr)80], 20);
			Span<int> span7 = new Span<int>(stackalloc byte[(UIntPtr)80], 20);
			for (i = -10; i <= 9; i++)
			{
				int num = Inlines.silk_LSHIFT(i, 10);
				int num2 = (int)Inlines.silk_ADD16((short)num, 1024);
				if (i > 0)
				{
					num = (int)Inlines.silk_SUB16((short)num, 102);
					num2 = (int)Inlines.silk_SUB16((short)num2, 102);
				}
				else if (i == 0)
				{
					num2 = (int)Inlines.silk_SUB16((short)num2, 102);
				}
				else if (i == -1)
				{
					num = (int)Inlines.silk_ADD16((short)num, 102);
				}
				else
				{
					num = (int)Inlines.silk_ADD16((short)num, 102);
					num2 = (int)Inlines.silk_ADD16((short)num2, 102);
				}
				*span6[i + 10] = Inlines.silk_SMULWB(num, quant_step_size_Q16);
				*span7[i + 10] = Inlines.silk_SMULWB(num2, quant_step_size_Q16);
			}
			int num3 = 1;
			*span3[0] = 0;
			*span2[0] = 0;
			i = (int)(order - 1);
			int num6;
			for (;;)
			{
				int a = Inlines.silk_LSHIFT((int)(*pred_coef_Q8[i]), 8);
				int num4 = (int)(*x_Q10[i]);
				for (int j = 0; j < num3; j++)
				{
					int num5 = Inlines.silk_SMULWB(a, (int)(*span2[j]));
					int b = (int)Inlines.silk_SUB16((short)num4, (short)num5);
					num6 = Inlines.silk_SMULWB((int)inv_quant_step_size_Q6, b);
					num6 = Inlines.silk_LIMIT(num6, -10, 9);
					array[j][i] = (sbyte)num6;
					int num7 = (int)(*ec_ix[i]) + num6;
					int num = *span6[num6 + 10];
					int num2 = *span7[num6 + 10];
					num = (int)Inlines.silk_ADD16((short)num, (short)num5);
					num2 = (int)Inlines.silk_ADD16((short)num2, (short)num5);
					*span2[j] = (short)num;
					*span2[j + num3] = (short)num2;
					int num8;
					int c;
					if (num6 + 1 >= 4)
					{
						if (num6 + 1 == 4)
						{
							num8 = (int)ec_rates_Q5[num7 + 4];
							c = 280;
						}
						else
						{
							num8 = Inlines.silk_SMLABB(108, 43, num6);
							c = (int)Inlines.silk_ADD16((short)num8, 43);
						}
					}
					else if (num6 <= -4)
					{
						if (num6 == -4)
						{
							num8 = 280;
							c = (int)ec_rates_Q5[num7 + 1 + 4];
						}
						else
						{
							num8 = Inlines.silk_SMLABB(108, -43, num6);
							c = (int)Inlines.silk_SUB16((short)num8, 43);
						}
					}
					else
					{
						num8 = (int)ec_rates_Q5[num7 + 4];
						c = (int)ec_rates_Q5[num7 + 1 + 4];
					}
					int a2 = *span3[j];
					int num9 = (int)Inlines.silk_SUB16((short)num4, (short)num);
					*span3[j] = Inlines.silk_SMLABB(Inlines.silk_MLA(a2, Inlines.silk_SMULBB(num9, num9), (int)(*w_Q5[i])), mu_Q20, num8);
					num9 = (int)Inlines.silk_SUB16((short)num4, (short)num2);
					*span3[j + num3] = Inlines.silk_SMLABB(Inlines.silk_MLA(a2, Inlines.silk_SMULBB(num9, num9), (int)(*w_Q5[i])), mu_Q20, c);
				}
				if (num3 <= 2)
				{
					for (int j = 0; j < num3; j++)
					{
						array[j + num3][i] = array[j][i] + 1;
					}
					num3 = Inlines.silk_LSHIFT(num3, 1);
					for (int j = num3; j < 4; j++)
					{
						array[j][i] = array[j - num3][i];
					}
				}
				else
				{
					if (i <= 0)
					{
						break;
					}
					for (int j = 0; j < 4; j++)
					{
						if (*span3[j] > *span3[j + 4])
						{
							*span5[j] = *span3[j];
							*span4[j] = *span3[j + 4];
							*span3[j] = *span4[j];
							*span3[j + 4] = *span5[j];
							int num = (int)(*span2[j]);
							*span2[j] = *span2[j + 4];
							*span2[j + 4] = (short)num;
							*span[j] = j + 4;
						}
						else
						{
							*span4[j] = *span3[j];
							*span5[j] = *span3[j + 4];
							*span[j] = j;
						}
					}
					for (;;)
					{
						int num10 = int.MaxValue;
						int num11 = 0;
						int num12 = 0;
						int num13 = 0;
						for (int j = 0; j < 4; j++)
						{
							if (num10 > *span5[j])
							{
								num10 = *span5[j];
								num12 = j;
							}
							if (num11 < *span4[j])
							{
								num11 = *span4[j];
								num13 = j;
							}
						}
						if (num10 >= num11)
						{
							break;
						}
						*span[num13] = (*span[num12] ^ 4);
						*span3[num13] = *span3[num12 + 4];
						*span2[num13] = *span2[num12 + 4];
						*span4[num13] = 0;
						*span5[num12] = int.MaxValue;
						Buffer.BlockCopy(array[num12], 0, array[num13], 0, (int)order);
					}
					for (int j = 0; j < 4; j++)
					{
						sbyte b2 = (sbyte)Inlines.silk_RSHIFT(*span[j], 2);
						sbyte[] array2 = array[j];
						int num14 = i;
						array2[num14] += b2;
					}
				}
				i--;
			}
			num6 = 0;
			int num15 = int.MaxValue;
			for (int j = 0; j < 8; j++)
			{
				if (num15 > *span3[j])
				{
					num15 = *span3[j];
					num6 = j;
				}
			}
			for (int j = 0; j < (int)order; j++)
			{
				indices[j] = array[num6 & 3][j];
			}
			indices[0] = (sbyte)((int)indices[0] + Inlines.silk_RSHIFT(num6, 2));
			return num15;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x0000C654 File Offset: 0x0000A854
		internal unsafe static int silk_NLSF_encode(sbyte[] NLSFIndices, short[] pNLSF_Q15, NLSFCodebook psNLSF_CB, short[] pW_QW, int NLSF_mu_Q20, int nSurvivors, int signalType)
		{
			Span<short> span = new Span<short>(stackalloc byte[(UIntPtr)32], 16);
			Span<short> x_Q = new Span<short>(stackalloc byte[(UIntPtr)32], 16);
			Span<short> pNLSF_Q16 = new Span<short>(stackalloc byte[(UIntPtr)32], 16);
			Span<short> pNLSFW_Q_OUT = new Span<short>(stackalloc byte[(UIntPtr)32], 16);
			Span<short> w_Q = new Span<short>(stackalloc byte[(UIntPtr)32], 16);
			Span<byte> span2 = new Span<byte>(stackalloc byte[(UIntPtr)16], 16);
			Span<short> ec_ix = new Span<short>(stackalloc byte[(UIntPtr)32], 16);
			byte[] cb1_NLSF_Q = psNLSF_CB.CB1_NLSF_Q8;
			NLSF.silk_NLSF_stabilize(pNLSF_Q15, psNLSF_CB.deltaMin_Q15, (int)psNLSF_CB.order);
			int[] array = new int[(int)psNLSF_CB.nVectors];
			NLSF.silk_NLSF_VQ(array, pNLSF_Q15, psNLSF_CB.CB1_NLSF_Q8, (int)psNLSF_CB.nVectors, (int)psNLSF_CB.order);
			int[] array2 = new int[nSurvivors];
			Sort.silk_insertion_sort_increasing(array, array2, (int)psNLSF_CB.nVectors, nSurvivors);
			int[] array3 = new int[nSurvivors];
			sbyte[][] array4 = Arrays.InitTwoDimensionalArray<sbyte>(nSurvivors, 16);
			for (int i = 0; i < nSurvivors; i++)
			{
				int num = array2[i];
				int num2 = num * (int)psNLSF_CB.order;
				for (int j = 0; j < (int)psNLSF_CB.order; j++)
				{
					*pNLSF_Q16[j] = Inlines.silk_LSHIFT16((short)cb1_NLSF_Q[num2 + j], 7);
					*span[j] = pNLSF_Q15[j] - *pNLSF_Q16[j];
				}
				NLSF.silk_NLSF_VQ_weights_laroia(pNLSFW_Q_OUT, pNLSF_Q16, (int)psNLSF_CB.order);
				for (int j = 0; j < (int)psNLSF_CB.order; j++)
				{
					int b = Inlines.silk_SQRT_APPROX(Inlines.silk_LSHIFT((int)(*pNLSFW_Q_OUT[j]), 16));
					*x_Q[j] = (short)Inlines.silk_RSHIFT(Inlines.silk_SMULBB((int)(*span[j]), b), 14);
				}
				for (int j = 0; j < (int)psNLSF_CB.order; j++)
				{
					*w_Q[j] = (short)Inlines.silk_DIV32_16(Inlines.silk_LSHIFT((int)pW_QW[j], 5), (int)(*pNLSFW_Q_OUT[j]));
				}
				NLSF.silk_NLSF_unpack(ec_ix, span2, psNLSF_CB, num);
				array3[i] = NLSF.silk_NLSF_del_dec_quant(array4[i], x_Q, w_Q, span2, ec_ix, psNLSF_CB.ec_Rates_Q5, (int)psNLSF_CB.quantStepSize_Q16, psNLSF_CB.invQuantStepSize_Q6, NLSF_mu_Q20, psNLSF_CB.order);
				int num3 = (signalType >> 1) * (int)psNLSF_CB.nVectors;
				int inLin;
				if (num == 0)
				{
					inLin = 256 - (int)psNLSF_CB.CB1_iCDF[num3 + num];
				}
				else
				{
					inLin = (int)(psNLSF_CB.CB1_iCDF[num3 + num - 1] - psNLSF_CB.CB1_iCDF[num3 + num]);
				}
				int b2 = 1024 - Inlines.silk_lin2log(inLin);
				array3[i] = Inlines.silk_SMLABB(array3[i], b2, Inlines.silk_RSHIFT(NLSF_mu_Q20, 2));
			}
			int[] array5 = new int[1];
			Sort.silk_insertion_sort_increasing(array3, array5, nSurvivors, 1);
			NLSFIndices[0] = (sbyte)array2[array5[0]];
			Arrays.MemCopy(array4[array5[0]], 0, NLSFIndices, 1, (int)psNLSF_CB.order);
			NLSF.silk_NLSF_decode(pNLSF_Q15, NLSFIndices, psNLSF_CB);
			return array3[0];
		}

		// Token: 0x060000EE RID: 238 RVA: 0x0000C8F4 File Offset: 0x0000AAF4
		internal unsafe static void silk_NLSF2A_find_poly(int[] o, Span<int> cLSF, int dd)
		{
			o[0] = Inlines.silk_LSHIFT(1, 16);
			o[1] = 0 - *cLSF[0];
			for (int i = 1; i < dd; i++)
			{
				int num = *cLSF[2 * i];
				o[i + 1] = Inlines.silk_LSHIFT(o[i - 1], 1) - (int)Inlines.silk_RSHIFT_ROUND64(Inlines.silk_SMULL(num, o[i]), 16);
				for (int j = i; j > 1; j--)
				{
					o[j] += o[j - 2] - (int)Inlines.silk_RSHIFT_ROUND64(Inlines.silk_SMULL(num, o[j - 1]), 16);
				}
				o[1] -= num;
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0000C990 File Offset: 0x0000AB90
		internal static void silk_NLSF2A(short[] a_Q12, short[] NLSF, int d)
		{
			int[] array = new int[d];
			int[] array2 = new int[d / 2 + 1];
			int[] array3 = new int[d / 2 + 1];
			int[] array4 = new int[d];
			int num = 0;
			byte[] array5 = (d == 16) ? NLSF.ordering16 : NLSF.ordering10;
			for (int i = 0; i < d; i++)
			{
				int num2 = Inlines.silk_RSHIFT((int)NLSF[i], 8);
				int b = (int)NLSF[i] - Inlines.silk_LSHIFT(num2, 8);
				int num3 = (int)Tables.silk_LSFCosTab_Q12[num2];
				int a = (int)Tables.silk_LSFCosTab_Q12[num2 + 1] - num3;
				array[(int)array5[i]] = Inlines.silk_RSHIFT_ROUND(Inlines.silk_LSHIFT(num3, 8) + Inlines.silk_MUL(a, b), 4);
			}
			int num4 = Inlines.silk_RSHIFT(d, 1);
			NLSF.silk_NLSF2A_find_poly(array2, MemoryExtensions.AsSpan<int>(array), num4);
			NLSF.silk_NLSF2A_find_poly(array3, MemoryExtensions.AsSpan<int>(array).Slice(1), num4);
			for (int i = 0; i < num4; i++)
			{
				int num5 = array2[i + 1] + array2[i];
				int num6 = array3[i + 1] - array3[i];
				array4[i] = -num6 - num5;
				array4[d - i - 1] = num6 - num5;
			}
			int j;
			for (j = 0; j < 10; j++)
			{
				int num7 = 0;
				for (int i = 0; i < d; i++)
				{
					int num8 = Inlines.silk_abs(array4[i]);
					if (num8 > num7)
					{
						num7 = num8;
						num = i;
					}
				}
				num7 = Inlines.silk_RSHIFT_ROUND(num7, 5);
				if (num7 <= 32767)
				{
					break;
				}
				num7 = Inlines.silk_min(num7, 163838);
				int chirp_Q = 65470 - Inlines.silk_DIV32(Inlines.silk_LSHIFT(num7 - 32767, 14), Inlines.silk_RSHIFT32(Inlines.silk_MUL(num7, num + 1), 2));
				Filters.silk_bwexpander_32(array4, d, chirp_Q);
			}
			if (j == 10)
			{
				for (int i = 0; i < d; i++)
				{
					a_Q12[i] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(array4[i], 5));
					array4[i] = Inlines.silk_LSHIFT((int)a_Q12[i], 5);
				}
			}
			else
			{
				for (int i = 0; i < d; i++)
				{
					a_Q12[i] = (short)Inlines.silk_RSHIFT_ROUND(array4[i], 5);
				}
			}
			j = 0;
			while (j < 16 && Filters.silk_LPC_inverse_pred_gain(a_Q12, d) < 107374)
			{
				Filters.silk_bwexpander_32(array4, d, 65536 - Inlines.silk_LSHIFT(2, j));
				for (int i = 0; i < d; i++)
				{
					a_Q12[i] = (short)Inlines.silk_RSHIFT_ROUND(array4[i], 5);
				}
				j++;
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0000CBCC File Offset: 0x0000ADCC
		internal static void silk_A2NLSF_trans_poly(int[] p, int dd)
		{
			for (int i = 2; i <= dd; i++)
			{
				for (int j = dd; j > i; j--)
				{
					p[j - 2] -= p[j];
				}
				p[i - 2] -= Inlines.silk_LSHIFT(p[i], 1);
			}
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0000CC18 File Offset: 0x0000AE18
		internal static int silk_A2NLSF_eval_poly(int[] p, int x, int dd)
		{
			int num = p[dd];
			int c = Inlines.silk_LSHIFT(x, 4);
			if (8 == dd)
			{
				num = Inlines.silk_SMLAWW(p[7], num, c);
				num = Inlines.silk_SMLAWW(p[6], num, c);
				num = Inlines.silk_SMLAWW(p[5], num, c);
				num = Inlines.silk_SMLAWW(p[4], num, c);
				num = Inlines.silk_SMLAWW(p[3], num, c);
				num = Inlines.silk_SMLAWW(p[2], num, c);
				num = Inlines.silk_SMLAWW(p[1], num, c);
				num = Inlines.silk_SMLAWW(p[0], num, c);
			}
			else
			{
				for (int i = dd - 1; i >= 0; i--)
				{
					num = Inlines.silk_SMLAWW(p[i], num, c);
				}
			}
			return num;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0000CCAC File Offset: 0x0000AEAC
		internal static void silk_A2NLSF_init(int[] a_Q16, int[] P, int[] Q, int dd)
		{
			P[dd] = Inlines.silk_LSHIFT(1, 16);
			Q[dd] = Inlines.silk_LSHIFT(1, 16);
			for (int i = 0; i < dd; i++)
			{
				P[i] = -a_Q16[dd - i - 1] - a_Q16[dd + i];
				Q[i] = -a_Q16[dd - i - 1] + a_Q16[dd + i];
			}
			for (int i = dd; i > 0; i--)
			{
				P[i - 1] -= P[i];
				Q[i - 1] += Q[i];
			}
			NLSF.silk_A2NLSF_trans_poly(P, dd);
			NLSF.silk_A2NLSF_trans_poly(Q, dd);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000CD38 File Offset: 0x0000AF38
		internal static void silk_A2NLSF(short[] NLSF, int[] a_Q16, int d)
		{
			int[] array = new int[9];
			int[] array2 = new int[9];
			int[][] array3 = new int[][]
			{
				array,
				array2
			};
			int dd = Inlines.silk_RSHIFT(d, 1);
			NLSF.silk_A2NLSF_init(a_Q16, array, array2, dd);
			int[] p = array;
			int num = (int)Tables.silk_LSFCosTab_Q12[0];
			int num2 = NLSF.silk_A2NLSF_eval_poly(p, num, dd);
			int num3;
			if (num2 < 0)
			{
				NLSF[0] = 0;
				p = array2;
				num2 = NLSF.silk_A2NLSF_eval_poly(p, num, dd);
				num3 = 1;
			}
			else
			{
				num3 = 0;
			}
			int i = 1;
			int num4 = 0;
			int num5 = 0;
			for (;;)
			{
				int num6 = (int)Tables.silk_LSFCosTab_Q12[i];
				int num7 = NLSF.silk_A2NLSF_eval_poly(p, num6, dd);
				if ((num2 <= 0 && num7 >= num5) || (num2 >= 0 && num7 <= -num5))
				{
					if (num7 == 0)
					{
						num5 = 1;
					}
					else
					{
						num5 = 0;
					}
					int num8 = -256;
					for (int j = 0; j < 3; j++)
					{
						int num9 = Inlines.silk_RSHIFT_ROUND(num + num6, 1);
						int num10 = NLSF.silk_A2NLSF_eval_poly(p, num9, dd);
						if ((num2 <= 0 && num10 >= 0) || (num2 >= 0 && num10 <= 0))
						{
							num6 = num9;
							num7 = num10;
						}
						else
						{
							num = num9;
							num2 = num10;
							num8 = Inlines.silk_ADD_RSHIFT(num8, 128, j);
						}
					}
					if (Inlines.silk_abs(num2) < 65536)
					{
						int num11 = num2 - num7;
						int a = Inlines.silk_LSHIFT(num2, 5) + Inlines.silk_RSHIFT(num11, 1);
						if (num11 != 0)
						{
							num8 += Inlines.silk_DIV32(a, num11);
						}
					}
					else
					{
						num8 += Inlines.silk_DIV32(num2, Inlines.silk_RSHIFT(num2 - num7, 5));
					}
					NLSF[num3] = (short)Inlines.silk_min_32(Inlines.silk_LSHIFT(i, 8) + num8, 32767);
					num3++;
					if (num3 >= d)
					{
						return;
					}
					p = array3[num3 & 1];
					num = (int)Tables.silk_LSFCosTab_Q12[i - 1];
					num2 = Inlines.silk_LSHIFT(1 - (num3 & 2), 12);
				}
				else
				{
					i++;
					num = num6;
					num2 = num7;
					num5 = 0;
					if (i > 128)
					{
						num4++;
						if (num4 > 30)
						{
							break;
						}
						Filters.silk_bwexpander_32(a_Q16, d, 65536 - Inlines.silk_SMULBB(10 + num4, num4));
						NLSF.silk_A2NLSF_init(a_Q16, array, array2, dd);
						p = array;
						num = (int)Tables.silk_LSFCosTab_Q12[0];
						num2 = NLSF.silk_A2NLSF_eval_poly(p, num, dd);
						if (num2 < 0)
						{
							NLSF[0] = 0;
							p = array2;
							num2 = NLSF.silk_A2NLSF_eval_poly(p, num, dd);
							num3 = 1;
						}
						else
						{
							num3 = 0;
						}
						i = 1;
					}
				}
			}
			NLSF[0] = (short)Inlines.silk_DIV32_16(32768, (int)((short)(d + 1)));
			for (i = 1; i < d; i++)
			{
				NLSF[i] = (short)Inlines.silk_SMULBB(i + 1, (int)NLSF[0]);
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0000CFB4 File Offset: 0x0000B1B4
		internal static void silk_process_NLSFs(SilkChannelEncoder psEncC, short[][] PredCoef_Q12, short[] pNLSF_Q15, short[] prev_NLSFq_Q15)
		{
			short[] array = new short[16];
			short[] array2 = new short[16];
			short[] array3 = new short[16];
			int num = Inlines.silk_SMLAWB(3146, -268434, psEncC.speech_activity_Q8);
			if (psEncC.nb_subfr == 2)
			{
				num = Inlines.silk_ADD_RSHIFT(num, num, 1);
			}
			NLSF.silk_NLSF_VQ_weights_laroia(array2, pNLSF_Q15, psEncC.predictLPCOrder);
			bool flag = psEncC.useInterpolatedNLSFs == 1 && psEncC.indices.NLSFInterpCoef_Q2 < 4;
			if (flag)
			{
				Inlines.silk_interpolate(array, prev_NLSFq_Q15, pNLSF_Q15, (int)psEncC.indices.NLSFInterpCoef_Q2, psEncC.predictLPCOrder);
				NLSF.silk_NLSF_VQ_weights_laroia(array3, array, psEncC.predictLPCOrder);
				int c = Inlines.silk_LSHIFT(Inlines.silk_SMULBB((int)psEncC.indices.NLSFInterpCoef_Q2, (int)psEncC.indices.NLSFInterpCoef_Q2), 11);
				for (int i = 0; i < psEncC.predictLPCOrder; i++)
				{
					array2[i] = (short)Inlines.silk_SMLAWB(Inlines.silk_RSHIFT((int)array2[i], 1), (int)array3[i], c);
				}
			}
			NLSF.silk_NLSF_encode(psEncC.indices.NLSFIndices, pNLSF_Q15, psEncC.psNLSF_CB, array2, num, psEncC.NLSF_MSVQ_Survivors, (int)psEncC.indices.signalType);
			NLSF.silk_NLSF2A(PredCoef_Q12[1], pNLSF_Q15, psEncC.predictLPCOrder);
			if (flag)
			{
				Inlines.silk_interpolate(array, prev_NLSFq_Q15, pNLSF_Q15, (int)psEncC.indices.NLSFInterpCoef_Q2, psEncC.predictLPCOrder);
				NLSF.silk_NLSF2A(PredCoef_Q12[0], array, psEncC.predictLPCOrder);
				return;
			}
			Arrays.MemCopy(PredCoef_Q12[1], 0, PredCoef_Q12[0], 0, psEncC.predictLPCOrder);
		}

		// Token: 0x0400003C RID: 60
		private const int MAX_LPC_ORDER = 16;

		// Token: 0x0400003D RID: 61
		private const int MAX_STABILIZE_LOOPS = 20;

		// Token: 0x0400003E RID: 62
		private const int QA = 16;

		// Token: 0x0400003F RID: 63
		private const int BIN_DIV_STEPS_A2NLSF = 3;

		// Token: 0x04000040 RID: 64
		private const int MAX_ITERATIONS_A2NLSF = 30;

		// Token: 0x04000041 RID: 65
		private static readonly byte[] ordering16 = new byte[]
		{
			0,
			15,
			8,
			7,
			4,
			11,
			12,
			3,
			2,
			13,
			10,
			5,
			6,
			9,
			14,
			1
		};

		// Token: 0x04000042 RID: 66
		private static readonly byte[] ordering10 = new byte[]
		{
			0,
			9,
			6,
			3,
			4,
			5,
			8,
			1,
			2,
			7
		};
	}
}
