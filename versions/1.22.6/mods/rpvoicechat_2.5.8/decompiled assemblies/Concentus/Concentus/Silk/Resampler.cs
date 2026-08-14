using System;
using Concentus.Common;
using Concentus.Common.CPlusPlus;
using Concentus.Silk.Enums;
using Concentus.Silk.Structs;

namespace Concentus.Silk
{
	// Token: 0x02000034 RID: 52
	internal static class Resampler
	{
		// Token: 0x06000106 RID: 262 RVA: 0x0000F984 File Offset: 0x0000DB84
		private static int rateID(int R)
		{
			return ((R >> 12) - ((R > 16000) ? 1 : 0) >> ((R > 24000) ? 1 : 0)) - 1;
		}

		// Token: 0x06000107 RID: 263 RVA: 0x0000F9A4 File Offset: 0x0000DBA4
		internal static int silk_resampler_init(SilkResamplerState S, int Fs_Hz_in, int Fs_Hz_out, int forEnc)
		{
			S.Reset();
			if (forEnc != 0)
			{
				if ((Fs_Hz_in != 8000 && Fs_Hz_in != 12000 && Fs_Hz_in != 16000 && Fs_Hz_in != 24000 && Fs_Hz_in != 48000) || (Fs_Hz_out != 8000 && Fs_Hz_out != 12000 && Fs_Hz_out != 16000))
				{
					return -1;
				}
				S.inputDelay = (int)Tables.delay_matrix_enc[Resampler.rateID(Fs_Hz_in), Resampler.rateID(Fs_Hz_out)];
			}
			else
			{
				if ((Fs_Hz_in != 8000 && Fs_Hz_in != 12000 && Fs_Hz_in != 16000) || (Fs_Hz_out != 8000 && Fs_Hz_out != 12000 && Fs_Hz_out != 16000 && Fs_Hz_out != 24000 && Fs_Hz_out != 48000))
				{
					return -1;
				}
				S.inputDelay = (int)Tables.delay_matrix_dec[Resampler.rateID(Fs_Hz_in), Resampler.rateID(Fs_Hz_out)];
			}
			S.Fs_in_kHz = Inlines.silk_DIV32_16(Fs_Hz_in, 1000);
			S.Fs_out_kHz = Inlines.silk_DIV32_16(Fs_Hz_out, 1000);
			S.batchSize = S.Fs_in_kHz * 10;
			int num = 0;
			if (Fs_Hz_out > Fs_Hz_in)
			{
				if (Fs_Hz_out == Inlines.silk_MUL(Fs_Hz_in, 2))
				{
					S.resampler_function = 1;
				}
				else
				{
					S.resampler_function = 2;
					num = 1;
				}
			}
			else if (Fs_Hz_out < Fs_Hz_in)
			{
				S.resampler_function = 3;
				if (Inlines.silk_MUL(Fs_Hz_out, 4) == Inlines.silk_MUL(Fs_Hz_in, 3))
				{
					S.FIR_Fracs = 3;
					S.FIR_Order = 18;
					S.Coefs = Tables.silk_Resampler_3_4_COEFS;
				}
				else if (Inlines.silk_MUL(Fs_Hz_out, 3) == Inlines.silk_MUL(Fs_Hz_in, 2))
				{
					S.FIR_Fracs = 2;
					S.FIR_Order = 18;
					S.Coefs = Tables.silk_Resampler_2_3_COEFS;
				}
				else if (Inlines.silk_MUL(Fs_Hz_out, 2) == Fs_Hz_in)
				{
					S.FIR_Fracs = 1;
					S.FIR_Order = 24;
					S.Coefs = Tables.silk_Resampler_1_2_COEFS;
				}
				else if (Inlines.silk_MUL(Fs_Hz_out, 3) == Fs_Hz_in)
				{
					S.FIR_Fracs = 1;
					S.FIR_Order = 36;
					S.Coefs = Tables.silk_Resampler_1_3_COEFS;
				}
				else if (Inlines.silk_MUL(Fs_Hz_out, 4) == Fs_Hz_in)
				{
					S.FIR_Fracs = 1;
					S.FIR_Order = 36;
					S.Coefs = Tables.silk_Resampler_1_4_COEFS;
				}
				else
				{
					if (Inlines.silk_MUL(Fs_Hz_out, 6) != Fs_Hz_in)
					{
						return -1;
					}
					S.FIR_Fracs = 1;
					S.FIR_Order = 36;
					S.Coefs = Tables.silk_Resampler_1_6_COEFS;
				}
			}
			else
			{
				S.resampler_function = 0;
			}
			S.invRatio_Q16 = Inlines.silk_LSHIFT32(Inlines.silk_DIV32(Inlines.silk_LSHIFT32(Fs_Hz_in, 14 + num), Fs_Hz_out), 2);
			while (Inlines.silk_SMULWW(S.invRatio_Q16, Fs_Hz_out) < Inlines.silk_LSHIFT32(Fs_Hz_in, num))
			{
				S.invRatio_Q16++;
			}
			return 0;
		}

		// Token: 0x06000108 RID: 264 RVA: 0x0000FC24 File Offset: 0x0000DE24
		internal static int silk_resampler(SilkResamplerState S, Span<short> output, int output_ptr, Span<short> input, int input_ptr, int inLen)
		{
			int num = S.Fs_in_kHz - S.inputDelay;
			short[] delayBuf = S.delayBuf;
			input.Slice(input_ptr, num).CopyTo(MemoryExtensions.AsSpan<short>(delayBuf, S.inputDelay));
			switch (S.resampler_function)
			{
			case 1:
				Resampler.silk_resampler_private_up2_HQ(S.sIIR, output, output_ptr, delayBuf, 0, S.Fs_in_kHz);
				Resampler.silk_resampler_private_up2_HQ(S.sIIR, output, output_ptr + S.Fs_out_kHz, input, input_ptr + num, inLen - S.Fs_in_kHz);
				break;
			case 2:
				Resampler.silk_resampler_private_IIR_FIR(S, output, output_ptr, delayBuf, 0, S.Fs_in_kHz);
				Resampler.silk_resampler_private_IIR_FIR(S, output, output_ptr + S.Fs_out_kHz, input, input_ptr + num, inLen - S.Fs_in_kHz);
				break;
			case 3:
				Resampler.silk_resampler_private_down_FIR(S, output, output_ptr, delayBuf, 0, S.Fs_in_kHz);
				Resampler.silk_resampler_private_down_FIR(S, output, output_ptr + S.Fs_out_kHz, input, input_ptr + num, inLen - S.Fs_in_kHz);
				break;
			default:
				MemoryExtensions.AsSpan<short>(delayBuf, 0, S.Fs_in_kHz).CopyTo(output.Slice(output_ptr));
				input.Slice(input_ptr + num, inLen - S.Fs_in_kHz).CopyTo(output.Slice(output_ptr + S.Fs_out_kHz));
				break;
			}
			input.Slice(input_ptr + inLen - S.inputDelay, S.inputDelay).CopyTo(delayBuf);
			return SilkError.SILK_NO_ERROR;
		}

		// Token: 0x06000109 RID: 265 RVA: 0x0000FDA4 File Offset: 0x0000DFA4
		internal static void silk_resampler_down2(int[] S, short[] output, short[] input, int inLen)
		{
			int num = Inlines.silk_RSHIFT32(inLen, 1);
			for (int i = 0; i < num; i++)
			{
				int a = Inlines.silk_LSHIFT((int)input[2 * i], 10);
				int num2 = Inlines.silk_SUB32(a, S[0]);
				int b = Inlines.silk_SMLAWB(num2, num2, -25727);
				int a2 = Inlines.silk_ADD32(S[0], b);
				S[0] = Inlines.silk_ADD32(a, b);
				a = Inlines.silk_LSHIFT((int)input[2 * i + 1], 10);
				b = Inlines.silk_SMULWB(Inlines.silk_SUB32(a, S[1]), 9872);
				a2 = Inlines.silk_ADD32(a2, S[1]);
				a2 = Inlines.silk_ADD32(a2, b);
				S[1] = Inlines.silk_ADD32(a, b);
				output[i] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(a2, 11));
			}
		}

		// Token: 0x0600010A RID: 266 RVA: 0x0000FE58 File Offset: 0x0000E058
		internal static void silk_resampler_down2_3(int[] S, short[] output, short[] input, int inLen)
		{
			int[] array = new int[484];
			int num = 0;
			int num2 = 0;
			Arrays.MemCopy(S, 0, array, 0, 4);
			int num3;
			for (;;)
			{
				num3 = Inlines.silk_min(inLen, 480);
				Resampler.silk_resampler_private_AR2(S, 4, array, 4, input, num, Tables.silk_Resampler_2_3_COEFS_LQ, num3);
				int num4 = 0;
				for (int i = num3; i > 2; i -= 3)
				{
					int num5 = Inlines.silk_SMULWB(array[num4], (int)Tables.silk_Resampler_2_3_COEFS_LQ[2]);
					num5 = Inlines.silk_SMLAWB(num5, array[num4 + 1], (int)Tables.silk_Resampler_2_3_COEFS_LQ[3]);
					num5 = Inlines.silk_SMLAWB(num5, array[num4 + 2], (int)Tables.silk_Resampler_2_3_COEFS_LQ[5]);
					num5 = Inlines.silk_SMLAWB(num5, array[num4 + 3], (int)Tables.silk_Resampler_2_3_COEFS_LQ[4]);
					output[num2++] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(num5, 6));
					num5 = Inlines.silk_SMULWB(array[num4 + 1], (int)Tables.silk_Resampler_2_3_COEFS_LQ[4]);
					num5 = Inlines.silk_SMLAWB(num5, array[num4 + 2], (int)Tables.silk_Resampler_2_3_COEFS_LQ[5]);
					num5 = Inlines.silk_SMLAWB(num5, array[num4 + 3], (int)Tables.silk_Resampler_2_3_COEFS_LQ[3]);
					num5 = Inlines.silk_SMLAWB(num5, array[num4 + 4], (int)Tables.silk_Resampler_2_3_COEFS_LQ[2]);
					output[num2++] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(num5, 6));
					num4 += 3;
				}
				num += num3;
				inLen -= num3;
				if (inLen <= 0)
				{
					break;
				}
				Arrays.MemCopy(array, num3, array, 0, 4);
			}
			Arrays.MemCopy(array, num3, S, 0, 4);
		}

		// Token: 0x0600010B RID: 267 RVA: 0x0000FFB8 File Offset: 0x0000E1B8
		internal unsafe static void silk_resampler_private_AR2(Span<int> S, int S_ptr, Span<int> out_Q8, int out_Q8_ptr, Span<short> input, int input_ptr, short[] A_Q14, int len)
		{
			for (int i = 0; i < len; i++)
			{
				int num = Inlines.silk_ADD_LSHIFT32(*S[S_ptr], (int)(*input[input_ptr + i]), 8);
				*out_Q8[out_Q8_ptr + i] = num;
				num = Inlines.silk_LSHIFT(num, 2);
				*S[S_ptr] = Inlines.silk_SMLAWB(*S[S_ptr + 1], num, (int)A_Q14[0]);
				*S[S_ptr + 1] = Inlines.silk_SMULWB(num, (int)A_Q14[1]);
			}
		}

		// Token: 0x0600010C RID: 268 RVA: 0x00010038 File Offset: 0x0000E238
		internal unsafe static int silk_resampler_private_down_FIR_INTERPOL(Span<short> output, int output_ptr, int[] buf, Span<short> FIR_Coefs, int FIR_Coefs_ptr, int FIR_Order, int FIR_Fracs, int max_index_Q16, int index_increment_Q16)
		{
			if (FIR_Order != 18)
			{
				if (FIR_Order != 24)
				{
					if (FIR_Order == 36)
					{
						for (int i = 0; i < max_index_Q16; i += index_increment_Q16)
						{
							int num = Inlines.silk_RSHIFT(i, 16);
							int num2 = Inlines.silk_SMULWB(Inlines.silk_ADD32(buf[num], buf[num + 35]), (int)(*FIR_Coefs[FIR_Coefs_ptr]));
							num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 1], buf[num + 34]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 1]));
							num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 2], buf[num + 33]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 2]));
							num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 3], buf[num + 32]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 3]));
							num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 4], buf[num + 31]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 4]));
							num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 5], buf[num + 30]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 5]));
							num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 6], buf[num + 29]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 6]));
							num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 7], buf[num + 28]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 7]));
							num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 8], buf[num + 27]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 8]));
							num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 9], buf[num + 26]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 9]));
							num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 10], buf[num + 25]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 10]));
							num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 11], buf[num + 24]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 11]));
							num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 12], buf[num + 23]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 12]));
							num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 13], buf[num + 22]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 13]));
							num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 14], buf[num + 21]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 14]));
							num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 15], buf[num + 20]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 15]));
							num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 16], buf[num + 19]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 16]));
							num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 17], buf[num + 18]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 17]));
							*output[output_ptr++] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(num2, 6));
						}
					}
				}
				else
				{
					for (int i = 0; i < max_index_Q16; i += index_increment_Q16)
					{
						int num = Inlines.silk_RSHIFT(i, 16);
						int num2 = Inlines.silk_SMULWB(Inlines.silk_ADD32(buf[num], buf[num + 23]), (int)(*FIR_Coefs[FIR_Coefs_ptr]));
						num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 1], buf[num + 22]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 1]));
						num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 2], buf[num + 21]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 2]));
						num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 3], buf[num + 20]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 3]));
						num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 4], buf[num + 19]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 4]));
						num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 5], buf[num + 18]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 5]));
						num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 6], buf[num + 17]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 6]));
						num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 7], buf[num + 16]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 7]));
						num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 8], buf[num + 15]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 8]));
						num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 9], buf[num + 14]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 9]));
						num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 10], buf[num + 13]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 10]));
						num2 = Inlines.silk_SMLAWB(num2, Inlines.silk_ADD32(buf[num + 11], buf[num + 12]), (int)(*FIR_Coefs[FIR_Coefs_ptr + 11]));
						*output[output_ptr++] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(num2, 6));
					}
				}
			}
			else
			{
				for (int i = 0; i < max_index_Q16; i += index_increment_Q16)
				{
					int num = Inlines.silk_RSHIFT(i, 16);
					int num3 = Inlines.silk_SMULWB(i & 65535, FIR_Fracs);
					int num4 = FIR_Coefs_ptr + 9 * num3;
					int num2 = Inlines.silk_SMULWB(buf[num], (int)(*FIR_Coefs[num4]));
					num2 = Inlines.silk_SMLAWB(num2, buf[num + 1], (int)(*FIR_Coefs[num4 + 1]));
					num2 = Inlines.silk_SMLAWB(num2, buf[num + 2], (int)(*FIR_Coefs[num4 + 2]));
					num2 = Inlines.silk_SMLAWB(num2, buf[num + 3], (int)(*FIR_Coefs[num4 + 3]));
					num2 = Inlines.silk_SMLAWB(num2, buf[num + 4], (int)(*FIR_Coefs[num4 + 4]));
					num2 = Inlines.silk_SMLAWB(num2, buf[num + 5], (int)(*FIR_Coefs[num4 + 5]));
					num2 = Inlines.silk_SMLAWB(num2, buf[num + 6], (int)(*FIR_Coefs[num4 + 6]));
					num2 = Inlines.silk_SMLAWB(num2, buf[num + 7], (int)(*FIR_Coefs[num4 + 7]));
					num2 = Inlines.silk_SMLAWB(num2, buf[num + 8], (int)(*FIR_Coefs[num4 + 8]));
					num4 = FIR_Coefs_ptr + 9 * (FIR_Fracs - 1 - num3);
					num2 = Inlines.silk_SMLAWB(num2, buf[num + 17], (int)(*FIR_Coefs[num4]));
					num2 = Inlines.silk_SMLAWB(num2, buf[num + 16], (int)(*FIR_Coefs[num4 + 1]));
					num2 = Inlines.silk_SMLAWB(num2, buf[num + 15], (int)(*FIR_Coefs[num4 + 2]));
					num2 = Inlines.silk_SMLAWB(num2, buf[num + 14], (int)(*FIR_Coefs[num4 + 3]));
					num2 = Inlines.silk_SMLAWB(num2, buf[num + 13], (int)(*FIR_Coefs[num4 + 4]));
					num2 = Inlines.silk_SMLAWB(num2, buf[num + 12], (int)(*FIR_Coefs[num4 + 5]));
					num2 = Inlines.silk_SMLAWB(num2, buf[num + 11], (int)(*FIR_Coefs[num4 + 6]));
					num2 = Inlines.silk_SMLAWB(num2, buf[num + 10], (int)(*FIR_Coefs[num4 + 7]));
					num2 = Inlines.silk_SMLAWB(num2, buf[num + 9], (int)(*FIR_Coefs[num4 + 8]));
					*output[output_ptr++] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(num2, 6));
				}
			}
			return output_ptr;
		}

		// Token: 0x0600010D RID: 269 RVA: 0x00010718 File Offset: 0x0000E918
		internal static void silk_resampler_private_down_FIR(SilkResamplerState S, Span<short> output, int output_ptr, Span<short> input, int input_ptr, int inLen)
		{
			int[] array = new int[S.batchSize + S.FIR_Order];
			Arrays.MemCopy(S.sFIR_i32, 0, array, 0, S.FIR_Order);
			int invRatio_Q = S.invRatio_Q16;
			int num;
			for (;;)
			{
				num = Inlines.silk_min(inLen, S.batchSize);
				Resampler.silk_resampler_private_AR2(S.sIIR, 0, array, S.FIR_Order, input, input_ptr, S.Coefs, num);
				int max_index_Q = Inlines.silk_LSHIFT32(num, 16);
				output_ptr = Resampler.silk_resampler_private_down_FIR_INTERPOL(output, output_ptr, array, S.Coefs, 2, S.FIR_Order, S.FIR_Fracs, max_index_Q, invRatio_Q);
				input_ptr += num;
				inLen -= num;
				if (inLen <= 1)
				{
					break;
				}
				Arrays.MemCopy(array, num, array, 0, S.FIR_Order);
			}
			Arrays.MemCopy(array, num, S.sFIR_i32, 0, S.FIR_Order);
		}

		// Token: 0x0600010E RID: 270 RVA: 0x000107F0 File Offset: 0x0000E9F0
		internal unsafe static int silk_resampler_private_IIR_FIR_INTERPOL(Span<short> output, int output_ptr, short[] buf, int max_index_Q16, int index_increment_Q16)
		{
			for (int i = 0; i < max_index_Q16; i += index_increment_Q16)
			{
				int num = Inlines.silk_SMULWB(i & 65535, 12);
				int num2 = i >> 16;
				int num3 = Inlines.silk_SMULBB((int)buf[num2], (int)Tables.silk_resampler_frac_FIR_12[num, 0]);
				num3 = Inlines.silk_SMLABB(num3, (int)buf[num2 + 1], (int)Tables.silk_resampler_frac_FIR_12[num, 1]);
				num3 = Inlines.silk_SMLABB(num3, (int)buf[num2 + 2], (int)Tables.silk_resampler_frac_FIR_12[num, 2]);
				num3 = Inlines.silk_SMLABB(num3, (int)buf[num2 + 3], (int)Tables.silk_resampler_frac_FIR_12[num, 3]);
				num3 = Inlines.silk_SMLABB(num3, (int)buf[num2 + 4], (int)Tables.silk_resampler_frac_FIR_12[11 - num, 3]);
				num3 = Inlines.silk_SMLABB(num3, (int)buf[num2 + 5], (int)Tables.silk_resampler_frac_FIR_12[11 - num, 2]);
				num3 = Inlines.silk_SMLABB(num3, (int)buf[num2 + 6], (int)Tables.silk_resampler_frac_FIR_12[11 - num, 1]);
				num3 = Inlines.silk_SMLABB(num3, (int)buf[num2 + 7], (int)Tables.silk_resampler_frac_FIR_12[11 - num, 0]);
				*output[output_ptr++] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(num3, 15));
			}
			return output_ptr;
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0001090C File Offset: 0x0000EB0C
		internal static void silk_resampler_private_IIR_FIR(SilkResamplerState S, Span<short> output, int output_ptr, Span<short> input, int input_ptr, int inLen)
		{
			short[] array = new short[2 * S.batchSize + 8];
			Arrays.MemCopy(S.sFIR_i16, 0, array, 0, 8);
			int invRatio_Q = S.invRatio_Q16;
			int num;
			for (;;)
			{
				num = Inlines.silk_min(inLen, S.batchSize);
				Resampler.silk_resampler_private_up2_HQ(S.sIIR, array, 8, input, input_ptr, num);
				int max_index_Q = Inlines.silk_LSHIFT32(num, 17);
				output_ptr = Resampler.silk_resampler_private_IIR_FIR_INTERPOL(output, output_ptr, array, max_index_Q, invRatio_Q);
				input_ptr += num;
				inLen -= num;
				if (inLen <= 0)
				{
					break;
				}
				Arrays.MemCopy(array, num << 1, array, 0, 8);
			}
			Arrays.MemCopy(array, num << 1, S.sFIR_i16, 0, 8);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x000109A8 File Offset: 0x0000EBA8
		internal unsafe static void silk_resampler_private_up2_HQ(int[] S, Span<short> output, int output_ptr, Span<short> input, int input_ptr, int len)
		{
			for (int i = 0; i < len; i++)
			{
				int a = Inlines.silk_LSHIFT((int)(*input[input_ptr + i]), 10);
				int b = Inlines.silk_SMULWB(Inlines.silk_SUB32(a, S[0]), (int)Tables.silk_resampler_up2_hq_0[0]);
				int a2 = Inlines.silk_ADD32(S[0], b);
				S[0] = Inlines.silk_ADD32(a, b);
				b = Inlines.silk_SMULWB(Inlines.silk_SUB32(a2, S[1]), (int)Tables.silk_resampler_up2_hq_0[1]);
				int a3 = Inlines.silk_ADD32(S[1], b);
				S[1] = Inlines.silk_ADD32(a2, b);
				int num = Inlines.silk_SUB32(a3, S[2]);
				b = Inlines.silk_SMLAWB(num, num, (int)Tables.silk_resampler_up2_hq_0[2]);
				a2 = Inlines.silk_ADD32(S[2], b);
				S[2] = Inlines.silk_ADD32(a3, b);
				*output[output_ptr + 2 * i] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(a2, 10));
				b = Inlines.silk_SMULWB(Inlines.silk_SUB32(a, S[3]), (int)Tables.silk_resampler_up2_hq_1[0]);
				a2 = Inlines.silk_ADD32(S[3], b);
				S[3] = Inlines.silk_ADD32(a, b);
				b = Inlines.silk_SMULWB(Inlines.silk_SUB32(a2, S[4]), (int)Tables.silk_resampler_up2_hq_1[1]);
				a3 = Inlines.silk_ADD32(S[4], b);
				S[4] = Inlines.silk_ADD32(a2, b);
				int num2 = Inlines.silk_SUB32(a3, S[5]);
				b = Inlines.silk_SMLAWB(num2, num2, (int)Tables.silk_resampler_up2_hq_1[2]);
				a2 = Inlines.silk_ADD32(S[5], b);
				S[5] = Inlines.silk_ADD32(a3, b);
				*output[output_ptr + 2 * i + 1] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(a2, 10));
			}
		}

		// Token: 0x04000053 RID: 83
		private const int USE_silk_resampler_copy = 0;

		// Token: 0x04000054 RID: 84
		private const int USE_silk_resampler_private_up2_HQ_wrapper = 1;

		// Token: 0x04000055 RID: 85
		private const int USE_silk_resampler_private_IIR_FIR = 2;

		// Token: 0x04000056 RID: 86
		private const int USE_silk_resampler_private_down_FIR = 3;

		// Token: 0x04000057 RID: 87
		private const int ORDER_FIR = 4;
	}
}
