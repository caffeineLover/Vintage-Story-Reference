using System;
using Concentus.Common;
using Concentus.Common.CPlusPlus;

namespace Concentus.Silk.Structs
{
	// Token: 0x0200004E RID: 78
	internal class SilkNSQState
	{
		// Token: 0x0600015F RID: 351 RVA: 0x00016954 File Offset: 0x00014B54
		internal void Reset()
		{
			Arrays.MemSetShort(this.xq, 0, 640);
			Arrays.MemSetInt(this.sLTP_shp_Q14, 0, 640);
			Arrays.MemSetInt(this.sLPC_Q14, 0, 80 + SilkConstants.NSQ_LPC_BUF_LENGTH);
			Arrays.MemSetInt(this.sAR2_Q14, 0, 16);
			this.sLF_AR_shp_Q14 = 0;
			this.lagPrev = 0;
			this.sLTP_buf_idx = 0;
			this.sLTP_shp_buf_idx = 0;
			this.rand_seed = 0;
			this.prev_gain_Q16 = 0;
			this.rewhite_flag = 0;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x000169D8 File Offset: 0x00014BD8
		internal void Assign(SilkNSQState other)
		{
			this.sLF_AR_shp_Q14 = other.sLF_AR_shp_Q14;
			this.lagPrev = other.lagPrev;
			this.sLTP_buf_idx = other.sLTP_buf_idx;
			this.sLTP_shp_buf_idx = other.sLTP_shp_buf_idx;
			this.rand_seed = other.rand_seed;
			this.prev_gain_Q16 = other.prev_gain_Q16;
			this.rewhite_flag = other.rewhite_flag;
			Arrays.MemCopy(other.xq, 0, this.xq, 0, 640);
			Arrays.MemCopy(other.sLTP_shp_Q14, 0, this.sLTP_shp_Q14, 0, 640);
			Arrays.MemCopy(other.sLPC_Q14, 0, this.sLPC_Q14, 0, 80 + SilkConstants.NSQ_LPC_BUF_LENGTH);
			Arrays.MemCopy(other.sAR2_Q14, 0, this.sAR2_Q14, 0, 16);
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00016A9C File Offset: 0x00014C9C
		internal void silk_NSQ(SilkChannelEncoder psEncC, SideInfoIndices psIndices, int[] x_Q3, sbyte[] pulses, short[][] PredCoef_Q12, short[] LTPCoef_Q14, short[] AR2_Q13, int[] HarmShapeGain_Q14, int[] Tilt_Q14, int[] LF_shp_Q14, int[] Gains_Q16, int[] pitchL, int Lambda_Q10, int LTP_scale_Q14)
		{
			int num = 0;
			int num2 = 0;
			this.rand_seed = (int)psIndices.Seed;
			int num3 = this.lagPrev;
			int offset_Q = (int)Concentus.Silk.Tables.silk_Quantization_Offsets_Q10[psIndices.signalType >> 1][(int)psIndices.quantOffsetType];
			int num4;
			if (psIndices.NLSFInterpCoef_Q2 == 4)
			{
				num4 = 0;
			}
			else
			{
				num4 = 1;
			}
			int[] sLTP_Q = new int[psEncC.ltp_mem_length + psEncC.frame_length];
			short[] array = new short[psEncC.ltp_mem_length + psEncC.frame_length];
			int[] x_sc_Q = new int[psEncC.subfr_length];
			this.sLTP_shp_buf_idx = psEncC.ltp_mem_length;
			this.sLTP_buf_idx = psEncC.ltp_mem_length;
			int num5 = psEncC.ltp_mem_length;
			for (int i = 0; i < psEncC.nb_subfr; i++)
			{
				int num6 = i >> 1 | 1 - num4;
				int b_Q14_ptr = i * 5;
				int ar_shp_Q13_ptr = i * 16;
				int num7 = Inlines.silk_RSHIFT(HarmShapeGain_Q14[i], 2);
				num7 |= Inlines.silk_LSHIFT(Inlines.silk_RSHIFT(HarmShapeGain_Q14[i], 1), 16);
				this.rewhite_flag = 0;
				if (psIndices.signalType == 2)
				{
					num3 = pitchL[i];
					if ((i & 3 - Inlines.silk_LSHIFT(num4, 1)) == 0)
					{
						int num8 = psEncC.ltp_mem_length - num3 - psEncC.predictLPCOrder - 2;
						Filters.silk_LPC_analysis_filter(array, num8, this.xq, num8 + i * psEncC.subfr_length, PredCoef_Q12[num6], 0, psEncC.ltp_mem_length - num8, psEncC.predictLPCOrder);
						this.rewhite_flag = 1;
						this.sLTP_buf_idx = psEncC.ltp_mem_length;
					}
				}
				this.silk_nsq_scale_states(psEncC, x_Q3, num2, x_sc_Q, array, sLTP_Q, i, LTP_scale_Q14, Gains_Q16, pitchL, (int)psIndices.signalType);
				this.silk_noise_shape_quantizer((int)psIndices.signalType, x_sc_Q, pulses, num, this.xq, num5, sLTP_Q, PredCoef_Q12[num6], LTPCoef_Q14, b_Q14_ptr, AR2_Q13, ar_shp_Q13_ptr, num3, num7, Tilt_Q14[i], LF_shp_Q14[i], Gains_Q16[i], Lambda_Q10, offset_Q, psEncC.subfr_length, psEncC.shapingLPCOrder, psEncC.predictLPCOrder);
				num2 += psEncC.subfr_length;
				num += psEncC.subfr_length;
				num5 += psEncC.subfr_length;
			}
			this.lagPrev = pitchL[psEncC.nb_subfr - 1];
			Arrays.MemMoveShort(this.xq, psEncC.frame_length, 0, psEncC.ltp_mem_length);
			Arrays.MemMoveInt(this.sLTP_shp_Q14, psEncC.frame_length, 0, psEncC.ltp_mem_length);
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00016CDC File Offset: 0x00014EDC
		private void silk_noise_shape_quantizer(int signalType, int[] x_sc_Q10, sbyte[] pulses, int pulses_ptr, short[] xq, int xq_ptr, int[] sLTP_Q15, short[] a_Q12, short[] b_Q14, int b_Q14_ptr, short[] AR_shp_Q13, int AR_shp_Q13_ptr, int lag, int HarmShapeFIRPacked_Q14, int Tilt_Q14, int LF_shp_Q14, int Gain_Q16, int Lambda_Q10, int offset_Q10, int length, int shapingLPCOrder, int predictLPCOrder)
		{
			int num = this.sLTP_shp_buf_idx - lag + 1;
			int num2 = this.sLTP_buf_idx - lag + 2;
			int b = Inlines.silk_RSHIFT(Gain_Q16, 6);
			int num3 = SilkConstants.NSQ_LPC_BUF_LENGTH - 1;
			for (int i = 0; i < length; i++)
			{
				this.rand_seed = Inlines.silk_RAND(this.rand_seed);
				int num4 = Inlines.silk_RSHIFT(predictLPCOrder, 1);
				num4 = Inlines.silk_SMLAWB(num4, this.sLPC_Q14[num3], (int)a_Q12[0]);
				num4 = Inlines.silk_SMLAWB(num4, this.sLPC_Q14[num3 - 1], (int)a_Q12[1]);
				num4 = Inlines.silk_SMLAWB(num4, this.sLPC_Q14[num3 - 2], (int)a_Q12[2]);
				num4 = Inlines.silk_SMLAWB(num4, this.sLPC_Q14[num3 - 3], (int)a_Q12[3]);
				num4 = Inlines.silk_SMLAWB(num4, this.sLPC_Q14[num3 - 4], (int)a_Q12[4]);
				num4 = Inlines.silk_SMLAWB(num4, this.sLPC_Q14[num3 - 5], (int)a_Q12[5]);
				num4 = Inlines.silk_SMLAWB(num4, this.sLPC_Q14[num3 - 6], (int)a_Q12[6]);
				num4 = Inlines.silk_SMLAWB(num4, this.sLPC_Q14[num3 - 7], (int)a_Q12[7]);
				num4 = Inlines.silk_SMLAWB(num4, this.sLPC_Q14[num3 - 8], (int)a_Q12[8]);
				num4 = Inlines.silk_SMLAWB(num4, this.sLPC_Q14[num3 - 9], (int)a_Q12[9]);
				if (predictLPCOrder == 16)
				{
					num4 = Inlines.silk_SMLAWB(num4, this.sLPC_Q14[num3 - 10], (int)a_Q12[10]);
					num4 = Inlines.silk_SMLAWB(num4, this.sLPC_Q14[num3 - 11], (int)a_Q12[11]);
					num4 = Inlines.silk_SMLAWB(num4, this.sLPC_Q14[num3 - 12], (int)a_Q12[12]);
					num4 = Inlines.silk_SMLAWB(num4, this.sLPC_Q14[num3 - 13], (int)a_Q12[13]);
					num4 = Inlines.silk_SMLAWB(num4, this.sLPC_Q14[num3 - 14], (int)a_Q12[14]);
					num4 = Inlines.silk_SMLAWB(num4, this.sLPC_Q14[num3 - 15], (int)a_Q12[15]);
				}
				int num5;
				if (signalType == 2)
				{
					num5 = 2;
					num5 = Inlines.silk_SMLAWB(num5, sLTP_Q15[num2], (int)b_Q14[b_Q14_ptr]);
					num5 = Inlines.silk_SMLAWB(num5, sLTP_Q15[num2 - 1], (int)b_Q14[b_Q14_ptr + 1]);
					num5 = Inlines.silk_SMLAWB(num5, sLTP_Q15[num2 - 2], (int)b_Q14[b_Q14_ptr + 2]);
					num5 = Inlines.silk_SMLAWB(num5, sLTP_Q15[num2 - 3], (int)b_Q14[b_Q14_ptr + 3]);
					num5 = Inlines.silk_SMLAWB(num5, sLTP_Q15[num2 - 4], (int)b_Q14[b_Q14_ptr + 4]);
					num2++;
				}
				else
				{
					num5 = 0;
				}
				int num6 = this.sLPC_Q14[num3];
				int num7 = this.sAR2_Q14[0];
				this.sAR2_Q14[0] = num6;
				int num8 = Inlines.silk_RSHIFT(shapingLPCOrder, 1);
				num8 = Inlines.silk_SMLAWB(num8, num6, (int)AR_shp_Q13[AR_shp_Q13_ptr]);
				for (int j = 2; j < shapingLPCOrder; j += 2)
				{
					num6 = this.sAR2_Q14[j - 1];
					this.sAR2_Q14[j - 1] = num7;
					num8 = Inlines.silk_SMLAWB(num8, num7, (int)AR_shp_Q13[AR_shp_Q13_ptr + j - 1]);
					num7 = this.sAR2_Q14[j];
					this.sAR2_Q14[j] = num6;
					num8 = Inlines.silk_SMLAWB(num8, num6, (int)AR_shp_Q13[AR_shp_Q13_ptr + j]);
				}
				this.sAR2_Q14[shapingLPCOrder - 1] = num7;
				num8 = Inlines.silk_SMLAWB(num8, num7, (int)AR_shp_Q13[AR_shp_Q13_ptr + shapingLPCOrder - 1]);
				num8 = Inlines.silk_LSHIFT32(num8, 1);
				num8 = Inlines.silk_SMLAWB(num8, this.sLF_AR_shp_Q14, Tilt_Q14);
				int num9 = Inlines.silk_SMULWB(this.sLTP_shp_Q14[this.sLTP_shp_buf_idx - 1], LF_shp_Q14);
				num9 = Inlines.silk_SMLAWT(num9, this.sLF_AR_shp_Q14, LF_shp_Q14);
				num7 = Inlines.silk_SUB32(Inlines.silk_LSHIFT32(num4, 2), num8);
				num7 = Inlines.silk_SUB32(num7, num9);
				if (lag > 0)
				{
					int num10 = Inlines.silk_SMULWB(Inlines.silk_ADD32(this.sLTP_shp_Q14[num], this.sLTP_shp_Q14[num - 2]), HarmShapeFIRPacked_Q14);
					num10 = Inlines.silk_SMLAWT(num10, this.sLTP_shp_Q14[num - 1], HarmShapeFIRPacked_Q14);
					num10 = Inlines.silk_LSHIFT(num10, 1);
					num++;
					num6 = Inlines.silk_SUB32(num5, num10);
					num7 = Inlines.silk_ADD_LSHIFT32(num6, num7, 1);
					num7 = Inlines.silk_RSHIFT_ROUND(num7, 3);
				}
				else
				{
					num7 = Inlines.silk_RSHIFT_ROUND(num7, 2);
				}
				int num11 = Inlines.silk_SUB32(x_sc_Q10[i], num7);
				if (this.rand_seed < 0)
				{
					num11 = -num11;
				}
				num11 = Inlines.silk_LIMIT_32(num11, -31744, 30720);
				int num12 = Inlines.silk_SUB32(num11, offset_Q10);
				int num13 = Inlines.silk_RSHIFT(num12, 10);
				int num14;
				int num15;
				int num16;
				if (num13 > 0)
				{
					num12 = Inlines.silk_SUB32(Inlines.silk_LSHIFT(num13, 10), 80);
					num12 = Inlines.silk_ADD32(num12, offset_Q10);
					num14 = Inlines.silk_ADD32(num12, 1024);
					num15 = Inlines.silk_SMULBB(num12, Lambda_Q10);
					num16 = Inlines.silk_SMULBB(num14, Lambda_Q10);
				}
				else if (num13 == 0)
				{
					num12 = offset_Q10;
					num14 = Inlines.silk_ADD32(num12, 944);
					num15 = Inlines.silk_SMULBB(num12, Lambda_Q10);
					num16 = Inlines.silk_SMULBB(num14, Lambda_Q10);
				}
				else if (num13 == -1)
				{
					num14 = offset_Q10;
					num12 = Inlines.silk_SUB32(num14, 944);
					num15 = Inlines.silk_SMULBB(-num12, Lambda_Q10);
					num16 = Inlines.silk_SMULBB(num14, Lambda_Q10);
				}
				else
				{
					num12 = Inlines.silk_ADD32(Inlines.silk_LSHIFT(num13, 10), 80);
					num12 = Inlines.silk_ADD32(num12, offset_Q10);
					num14 = Inlines.silk_ADD32(num12, 1024);
					num15 = Inlines.silk_SMULBB(-num12, Lambda_Q10);
					num16 = Inlines.silk_SMULBB(-num14, Lambda_Q10);
				}
				int num17 = Inlines.silk_SUB32(num11, num12);
				num15 = Inlines.silk_SMLABB(num15, num17, num17);
				num17 = Inlines.silk_SUB32(num11, num14);
				num16 = Inlines.silk_SMLABB(num16, num17, num17);
				if (num16 < num15)
				{
					num12 = num14;
				}
				pulses[pulses_ptr + i] = (sbyte)Inlines.silk_RSHIFT_ROUND(num12, 10);
				int num18 = Inlines.silk_LSHIFT(num12, 4);
				if (this.rand_seed < 0)
				{
					num18 = -num18;
				}
				int a = Inlines.silk_ADD_LSHIFT32(num18, num5, 1);
				int num19 = Inlines.silk_ADD_LSHIFT32(a, num4, 4);
				xq[xq_ptr + i] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(Inlines.silk_SMULWW(num19, b), 8));
				num3++;
				this.sLPC_Q14[num3] = num19;
				int a2 = Inlines.silk_SUB_LSHIFT32(num19, num8, 2);
				this.sLF_AR_shp_Q14 = a2;
				this.sLTP_shp_Q14[this.sLTP_shp_buf_idx] = Inlines.silk_SUB_LSHIFT32(a2, num9, 2);
				sLTP_Q15[this.sLTP_buf_idx] = Inlines.silk_LSHIFT(a, 1);
				this.sLTP_shp_buf_idx++;
				this.sLTP_buf_idx++;
				this.rand_seed = Inlines.silk_ADD32_ovflw(this.rand_seed, (int)pulses[pulses_ptr + i]);
			}
			Arrays.MemCopy(this.sLPC_Q14, length, this.sLPC_Q14, 0, SilkConstants.NSQ_LPC_BUF_LENGTH);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0001731C File Offset: 0x0001551C
		private void silk_nsq_scale_states(SilkChannelEncoder psEncC, int[] x_Q3, int x_Q3_ptr, int[] x_sc_Q10, short[] sLTP, int[] sLTP_Q15, int subfr, int LTP_scale_Q14, int[] Gains_Q16, int[] pitchL, int signal_type)
		{
			int num = pitchL[subfr];
			int num2 = Inlines.silk_INVERSE32_varQ(Inlines.silk_max(Gains_Q16[subfr], 1), 47);
			int num3;
			if (Gains_Q16[subfr] != this.prev_gain_Q16)
			{
				num3 = Inlines.silk_DIV32_varQ(this.prev_gain_Q16, Gains_Q16[subfr], 16);
			}
			else
			{
				num3 = 65536;
			}
			int b = Inlines.silk_RSHIFT_ROUND(num2, 8);
			for (int i = 0; i < psEncC.subfr_length; i++)
			{
				x_sc_Q10[i] = Inlines.silk_SMULWW(x_Q3[x_Q3_ptr + i], b);
			}
			this.prev_gain_Q16 = Gains_Q16[subfr];
			if (this.rewhite_flag != 0)
			{
				if (subfr == 0)
				{
					num2 = Inlines.silk_LSHIFT(Inlines.silk_SMULWB(num2, LTP_scale_Q14), 2);
				}
				for (int i = this.sLTP_buf_idx - num - 2; i < this.sLTP_buf_idx; i++)
				{
					sLTP_Q15[i] = Inlines.silk_SMULWB(num2, (int)sLTP[i]);
				}
			}
			if (num3 != 65536)
			{
				for (int i = this.sLTP_shp_buf_idx - psEncC.ltp_mem_length; i < this.sLTP_shp_buf_idx; i++)
				{
					this.sLTP_shp_Q14[i] = Inlines.silk_SMULWW(num3, this.sLTP_shp_Q14[i]);
				}
				if (signal_type == 2 && this.rewhite_flag == 0)
				{
					for (int i = this.sLTP_buf_idx - num - 2; i < this.sLTP_buf_idx; i++)
					{
						sLTP_Q15[i] = Inlines.silk_SMULWW(num3, sLTP_Q15[i]);
					}
				}
				this.sLF_AR_shp_Q14 = Inlines.silk_SMULWW(num3, this.sLF_AR_shp_Q14);
				for (int i = 0; i < SilkConstants.NSQ_LPC_BUF_LENGTH; i++)
				{
					this.sLPC_Q14[i] = Inlines.silk_SMULWW(num3, this.sLPC_Q14[i]);
				}
				for (int i = 0; i < 16; i++)
				{
					this.sAR2_Q14[i] = Inlines.silk_SMULWW(num3, this.sAR2_Q14[i]);
				}
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x000174B0 File Offset: 0x000156B0
		internal void silk_NSQ_del_dec(SilkChannelEncoder psEncC, SideInfoIndices psIndices, int[] x_Q3, sbyte[] pulses, short[][] PredCoef_Q12, short[] LTPCoef_Q14, short[] AR2_Q13, int[] HarmShapeGain_Q14, int[] Tilt_Q14, int[] LF_shp_Q14, int[] Gains_Q16, int[] pitchL, int Lambda_Q10, int LTP_scale_Q14)
		{
			int num = 0;
			int num2 = 0;
			int num3 = this.lagPrev;
			SilkNSQState.NSQ_del_dec_struct[] array = new SilkNSQState.NSQ_del_dec_struct[psEncC.nStatesDelayedDecision];
			for (int i = 0; i < psEncC.nStatesDelayedDecision; i++)
			{
				array[i] = new SilkNSQState.NSQ_del_dec_struct(psEncC.shapingLPCOrder);
			}
			SilkNSQState.NSQ_del_dec_struct nsq_del_dec_struct;
			for (int j = 0; j < psEncC.nStatesDelayedDecision; j++)
			{
				nsq_del_dec_struct = array[j];
				nsq_del_dec_struct.Seed = (j + (int)psIndices.Seed & 3);
				nsq_del_dec_struct.SeedInit = nsq_del_dec_struct.Seed;
				nsq_del_dec_struct.RD_Q10 = 0;
				nsq_del_dec_struct.LF_AR_Q14 = this.sLF_AR_shp_Q14;
				nsq_del_dec_struct.Shape_Q14[0] = this.sLTP_shp_Q14[psEncC.ltp_mem_length - 1];
				Arrays.MemCopy(this.sLPC_Q14, 0, nsq_del_dec_struct.sLPC_Q14, 0, SilkConstants.NSQ_LPC_BUF_LENGTH);
				Arrays.MemCopy(this.sAR2_Q14, 0, nsq_del_dec_struct.sAR2_Q14, 0, psEncC.shapingLPCOrder);
			}
			int offset_Q = (int)Concentus.Silk.Tables.silk_Quantization_Offsets_Q10[psIndices.signalType >> 1][(int)psIndices.quantOffsetType];
			int num4 = 0;
			int num5 = Inlines.silk_min_int(32, psEncC.subfr_length);
			if (psIndices.signalType == 2)
			{
				for (int j = 0; j < psEncC.nb_subfr; j++)
				{
					num5 = Inlines.silk_min_int(num5, pitchL[j] - 2 - 1);
				}
			}
			else if (num3 > 0)
			{
				num5 = Inlines.silk_min_int(num5, num3 - 2 - 1);
			}
			int num6;
			if (psIndices.NLSFInterpCoef_Q2 == 4)
			{
				num6 = 0;
			}
			else
			{
				num6 = 1;
			}
			int[] sLTP_Q = new int[psEncC.ltp_mem_length + psEncC.frame_length];
			short[] array2 = new short[psEncC.ltp_mem_length + psEncC.frame_length];
			int[] array3 = new int[psEncC.subfr_length];
			int[] delayedGain_Q = new int[32];
			int num7 = psEncC.ltp_mem_length;
			this.sLTP_shp_buf_idx = psEncC.ltp_mem_length;
			this.sLTP_buf_idx = psEncC.ltp_mem_length;
			int num8 = 0;
			int rd_Q;
			int num11;
			int num12;
			for (int j = 0; j < psEncC.nb_subfr; j++)
			{
				int num9 = j >> 1 | 1 - num6;
				int num10 = Inlines.silk_RSHIFT(HarmShapeGain_Q14[j], 2);
				num10 |= Inlines.silk_LSHIFT(Inlines.silk_RSHIFT(HarmShapeGain_Q14[j], 1), 16);
				this.rewhite_flag = 0;
				if (psIndices.signalType == 2)
				{
					num3 = pitchL[j];
					if ((j & 3 - Inlines.silk_LSHIFT(num6, 1)) == 0)
					{
						if (j == 2)
						{
							rd_Q = array[0].RD_Q10;
							num11 = 0;
							for (int k = 1; k < psEncC.nStatesDelayedDecision; k++)
							{
								if (array[k].RD_Q10 < rd_Q)
								{
									rd_Q = array[k].RD_Q10;
									num11 = k;
								}
							}
							for (int k = 0; k < psEncC.nStatesDelayedDecision; k++)
							{
								if (k != num11)
								{
									array[k].RD_Q10 += 134217727;
								}
							}
							nsq_del_dec_struct = array[num11];
							num12 = num4 + num5;
							for (int k = 0; k < num5; k++)
							{
								num12 = (num12 - 1 & 31);
								pulses[num + k - num5] = (sbyte)Inlines.silk_RSHIFT_ROUND(nsq_del_dec_struct.Q_Q10[num12], 10);
								this.xq[num7 + k - num5] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(Inlines.silk_SMULWW(nsq_del_dec_struct.Xq_Q14[num12], Gains_Q16[1]), 14));
								this.sLTP_shp_Q14[this.sLTP_shp_buf_idx - num5 + k] = nsq_del_dec_struct.Shape_Q14[num12];
							}
							num8 = 0;
						}
						int num13 = psEncC.ltp_mem_length - num3 - psEncC.predictLPCOrder - 2;
						Filters.silk_LPC_analysis_filter(array2, num13, this.xq, num13 + j * psEncC.subfr_length, PredCoef_Q12[num9], 0, psEncC.ltp_mem_length - num13, psEncC.predictLPCOrder);
						this.sLTP_buf_idx = psEncC.ltp_mem_length;
						this.rewhite_flag = 1;
					}
				}
				this.silk_nsq_del_dec_scale_states(psEncC, array, x_Q3, num2, array3, array2, sLTP_Q, j, psEncC.nStatesDelayedDecision, LTP_scale_Q14, Gains_Q16, pitchL, (int)psIndices.signalType, num5);
				BoxedValueInt boxedValueInt = new BoxedValueInt(num4);
				this.silk_noise_shape_quantizer_del_dec(array, (int)psIndices.signalType, array3, pulses, num, this.xq, num7, sLTP_Q, delayedGain_Q, PredCoef_Q12[num9], LTPCoef_Q14, j * 5, AR2_Q13, j * 16, num3, num10, Tilt_Q14[j], LF_shp_Q14[j], Gains_Q16[j], Lambda_Q10, offset_Q, psEncC.subfr_length, num8++, psEncC.shapingLPCOrder, psEncC.predictLPCOrder, psEncC.warping_Q16, psEncC.nStatesDelayedDecision, boxedValueInt, num5);
				num4 = boxedValueInt.Val;
				num2 += psEncC.subfr_length;
				num += psEncC.subfr_length;
				num7 += psEncC.subfr_length;
			}
			rd_Q = array[0].RD_Q10;
			num11 = 0;
			for (int j = 1; j < psEncC.nStatesDelayedDecision; j++)
			{
				if (array[j].RD_Q10 < rd_Q)
				{
					rd_Q = array[j].RD_Q10;
					num11 = j;
				}
			}
			nsq_del_dec_struct = array[num11];
			psIndices.Seed = (sbyte)nsq_del_dec_struct.SeedInit;
			num12 = num4 + num5;
			int b = Inlines.silk_RSHIFT32(Gains_Q16[psEncC.nb_subfr - 1], 6);
			for (int k = 0; k < num5; k++)
			{
				num12 = (num12 - 1 & 31);
				pulses[num + k - num5] = (sbyte)Inlines.silk_RSHIFT_ROUND(nsq_del_dec_struct.Q_Q10[num12], 10);
				this.xq[num7 + k - num5] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(Inlines.silk_SMULWW(nsq_del_dec_struct.Xq_Q14[num12], b), 8));
				this.sLTP_shp_Q14[this.sLTP_shp_buf_idx - num5 + k] = nsq_del_dec_struct.Shape_Q14[num12];
			}
			Arrays.MemCopy(nsq_del_dec_struct.sLPC_Q14, psEncC.subfr_length, this.sLPC_Q14, 0, SilkConstants.NSQ_LPC_BUF_LENGTH);
			Arrays.MemCopy(nsq_del_dec_struct.sAR2_Q14, 0, this.sAR2_Q14, 0, psEncC.shapingLPCOrder);
			this.sLF_AR_shp_Q14 = nsq_del_dec_struct.LF_AR_Q14;
			this.lagPrev = pitchL[psEncC.nb_subfr - 1];
			Arrays.MemMoveShort(this.xq, psEncC.frame_length, 0, psEncC.ltp_mem_length);
			Arrays.MemMoveInt(this.sLTP_shp_Q14, psEncC.frame_length, 0, psEncC.ltp_mem_length);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x00017A68 File Offset: 0x00015C68
		private void silk_noise_shape_quantizer_del_dec(SilkNSQState.NSQ_del_dec_struct[] psDelDec, int signalType, int[] x_Q10, sbyte[] pulses, int pulses_ptr, short[] xq, int xq_ptr, int[] sLTP_Q15, int[] delayedGain_Q10, short[] a_Q12, short[] b_Q14, int b_Q14_ptr, short[] AR_shp_Q13, int AR_shp_Q13_ptr, int lag, int HarmShapeFIRPacked_Q14, int Tilt_Q14, int LF_shp_Q14, int Gain_Q16, int Lambda_Q10, int offset_Q10, int length, int subfr, int shapingLPCOrder, int predictLPCOrder, int warping_Q16, int nStatesDelayedDecision, BoxedValueInt smpl_buf_idx, int decisionDelay)
		{
			SilkNSQState.NSQ_sample_struct[] array = new SilkNSQState.NSQ_sample_struct[2 * nStatesDelayedDecision];
			for (int i = 0; i < 2 * nStatesDelayedDecision; i++)
			{
				array[i] = default(SilkNSQState.NSQ_sample_struct);
			}
			int num = this.sLTP_shp_buf_idx - lag + 1;
			int num2 = this.sLTP_buf_idx - lag + 2;
			int num3 = Inlines.silk_RSHIFT(Gain_Q16, 6);
			for (int j = 0; j < length; j++)
			{
				int num4;
				if (signalType == 2)
				{
					num4 = 2;
					num4 = Inlines.silk_SMLAWB(num4, sLTP_Q15[num2], (int)b_Q14[b_Q14_ptr]);
					num4 = Inlines.silk_SMLAWB(num4, sLTP_Q15[num2 - 1], (int)b_Q14[b_Q14_ptr + 1]);
					num4 = Inlines.silk_SMLAWB(num4, sLTP_Q15[num2 - 2], (int)b_Q14[b_Q14_ptr + 2]);
					num4 = Inlines.silk_SMLAWB(num4, sLTP_Q15[num2 - 3], (int)b_Q14[b_Q14_ptr + 3]);
					num4 = Inlines.silk_SMLAWB(num4, sLTP_Q15[num2 - 4], (int)b_Q14[b_Q14_ptr + 4]);
					num4 = Inlines.silk_LSHIFT(num4, 1);
					num2++;
				}
				else
				{
					num4 = 0;
				}
				int num5;
				if (lag > 0)
				{
					num5 = Inlines.silk_SMULWB(Inlines.silk_ADD32(this.sLTP_shp_Q14[num], this.sLTP_shp_Q14[num - 2]), HarmShapeFIRPacked_Q14);
					num5 = Inlines.silk_SMLAWT(num5, this.sLTP_shp_Q14[num - 1], HarmShapeFIRPacked_Q14);
					num5 = Inlines.silk_SUB_LSHIFT32(num4, num5, 2);
					num++;
				}
				else
				{
					num5 = 0;
				}
				SilkNSQState.NSQ_del_dec_struct nsq_del_dec_struct;
				for (int k = 0; k < nStatesDelayedDecision; k++)
				{
					nsq_del_dec_struct = psDelDec[k];
					int[] array2 = nsq_del_dec_struct.sAR2_Q14;
					int num6 = 2 * k;
					int num7 = num6 + 1;
					nsq_del_dec_struct.Seed = Inlines.silk_RAND(nsq_del_dec_struct.Seed);
					int num8 = SilkConstants.NSQ_LPC_BUF_LENGTH - 1 + j;
					int num9 = Inlines.silk_RSHIFT(predictLPCOrder, 1);
					num9 = Inlines.silk_SMLAWB(num9, nsq_del_dec_struct.sLPC_Q14[num8], (int)a_Q12[0]);
					num9 = Inlines.silk_SMLAWB(num9, nsq_del_dec_struct.sLPC_Q14[num8 - 1], (int)a_Q12[1]);
					num9 = Inlines.silk_SMLAWB(num9, nsq_del_dec_struct.sLPC_Q14[num8 - 2], (int)a_Q12[2]);
					num9 = Inlines.silk_SMLAWB(num9, nsq_del_dec_struct.sLPC_Q14[num8 - 3], (int)a_Q12[3]);
					num9 = Inlines.silk_SMLAWB(num9, nsq_del_dec_struct.sLPC_Q14[num8 - 4], (int)a_Q12[4]);
					num9 = Inlines.silk_SMLAWB(num9, nsq_del_dec_struct.sLPC_Q14[num8 - 5], (int)a_Q12[5]);
					num9 = Inlines.silk_SMLAWB(num9, nsq_del_dec_struct.sLPC_Q14[num8 - 6], (int)a_Q12[6]);
					num9 = Inlines.silk_SMLAWB(num9, nsq_del_dec_struct.sLPC_Q14[num8 - 7], (int)a_Q12[7]);
					num9 = Inlines.silk_SMLAWB(num9, nsq_del_dec_struct.sLPC_Q14[num8 - 8], (int)a_Q12[8]);
					num9 = Inlines.silk_SMLAWB(num9, nsq_del_dec_struct.sLPC_Q14[num8 - 9], (int)a_Q12[9]);
					if (predictLPCOrder == 16)
					{
						num9 = Inlines.silk_SMLAWB(num9, nsq_del_dec_struct.sLPC_Q14[num8 - 10], (int)a_Q12[10]);
						num9 = Inlines.silk_SMLAWB(num9, nsq_del_dec_struct.sLPC_Q14[num8 - 11], (int)a_Q12[11]);
						num9 = Inlines.silk_SMLAWB(num9, nsq_del_dec_struct.sLPC_Q14[num8 - 12], (int)a_Q12[12]);
						num9 = Inlines.silk_SMLAWB(num9, nsq_del_dec_struct.sLPC_Q14[num8 - 13], (int)a_Q12[13]);
						num9 = Inlines.silk_SMLAWB(num9, nsq_del_dec_struct.sLPC_Q14[num8 - 14], (int)a_Q12[14]);
						num9 = Inlines.silk_SMLAWB(num9, nsq_del_dec_struct.sLPC_Q14[num8 - 15], (int)a_Q12[15]);
					}
					num9 = Inlines.silk_LSHIFT(num9, 4);
					int num10 = Inlines.silk_SMLAWB(nsq_del_dec_struct.sLPC_Q14[num8], array2[0], warping_Q16);
					int num11 = Inlines.silk_SMLAWB(array2[0], array2[1] - num10, warping_Q16);
					array2[0] = num10;
					int num12 = Inlines.silk_RSHIFT(shapingLPCOrder, 1);
					num12 = Inlines.silk_SMLAWB(num12, num10, (int)AR_shp_Q13[AR_shp_Q13_ptr]);
					for (int l = 2; l < shapingLPCOrder; l += 2)
					{
						num10 = Inlines.silk_SMLAWB(array2[l - 1], array2[l] - num11, warping_Q16);
						array2[l - 1] = num11;
						num12 = Inlines.silk_SMLAWB(num12, num11, (int)AR_shp_Q13[AR_shp_Q13_ptr + l - 1]);
						num11 = Inlines.silk_SMLAWB(array2[l], array2[l + 1] - num10, warping_Q16);
						array2[l] = num10;
						num12 = Inlines.silk_SMLAWB(num12, num10, (int)AR_shp_Q13[AR_shp_Q13_ptr + l]);
					}
					array2[shapingLPCOrder - 1] = num11;
					num12 = Inlines.silk_SMLAWB(num12, num11, (int)AR_shp_Q13[AR_shp_Q13_ptr + shapingLPCOrder - 1]);
					num12 = Inlines.silk_LSHIFT(num12, 1);
					num12 = Inlines.silk_SMLAWB(num12, nsq_del_dec_struct.LF_AR_Q14, Tilt_Q14);
					num12 = Inlines.silk_LSHIFT(num12, 2);
					int num13 = Inlines.silk_SMULWB(nsq_del_dec_struct.Shape_Q14[smpl_buf_idx.Val], LF_shp_Q14);
					num13 = Inlines.silk_SMLAWT(num13, nsq_del_dec_struct.LF_AR_Q14, LF_shp_Q14);
					num13 = Inlines.silk_LSHIFT(num13, 2);
					num11 = Inlines.silk_ADD32(num12, num13);
					num10 = Inlines.silk_ADD32(num5, num9);
					num11 = Inlines.silk_SUB32(num10, num11);
					num11 = Inlines.silk_RSHIFT_ROUND(num11, 4);
					int num14 = Inlines.silk_SUB32(x_Q10[j], num11);
					if (nsq_del_dec_struct.Seed < 0)
					{
						num14 = -num14;
					}
					num14 = Inlines.silk_LIMIT_32(num14, -31744, 30720);
					int num15 = Inlines.silk_SUB32(num14, offset_Q10);
					int num16 = Inlines.silk_RSHIFT(num15, 10);
					int num17;
					int num18;
					int num19;
					if (num16 > 0)
					{
						num15 = Inlines.silk_SUB32(Inlines.silk_LSHIFT(num16, 10), 80);
						num15 = Inlines.silk_ADD32(num15, offset_Q10);
						num17 = Inlines.silk_ADD32(num15, 1024);
						num18 = Inlines.silk_SMULBB(num15, Lambda_Q10);
						num19 = Inlines.silk_SMULBB(num17, Lambda_Q10);
					}
					else if (num16 == 0)
					{
						num15 = offset_Q10;
						num17 = Inlines.silk_ADD32(num15, 944);
						num18 = Inlines.silk_SMULBB(num15, Lambda_Q10);
						num19 = Inlines.silk_SMULBB(num17, Lambda_Q10);
					}
					else if (num16 == -1)
					{
						num17 = offset_Q10;
						num15 = Inlines.silk_SUB32(num17, 944);
						num18 = Inlines.silk_SMULBB(-num15, Lambda_Q10);
						num19 = Inlines.silk_SMULBB(num17, Lambda_Q10);
					}
					else
					{
						num15 = Inlines.silk_ADD32(Inlines.silk_LSHIFT(num16, 10), 80);
						num15 = Inlines.silk_ADD32(num15, offset_Q10);
						num17 = Inlines.silk_ADD32(num15, 1024);
						num18 = Inlines.silk_SMULBB(-num15, Lambda_Q10);
						num19 = Inlines.silk_SMULBB(-num17, Lambda_Q10);
					}
					int num20 = Inlines.silk_SUB32(num14, num15);
					num18 = Inlines.silk_RSHIFT(Inlines.silk_SMLABB(num18, num20, num20), 10);
					num20 = Inlines.silk_SUB32(num14, num17);
					num19 = Inlines.silk_RSHIFT(Inlines.silk_SMLABB(num19, num20, num20), 10);
					if (num18 < num19)
					{
						array[num6].RD_Q10 = Inlines.silk_ADD32(nsq_del_dec_struct.RD_Q10, num18);
						array[num7].RD_Q10 = Inlines.silk_ADD32(nsq_del_dec_struct.RD_Q10, num19);
						array[num6].Q_Q10 = num15;
						array[num7].Q_Q10 = num17;
					}
					else
					{
						array[num6].RD_Q10 = Inlines.silk_ADD32(nsq_del_dec_struct.RD_Q10, num19);
						array[num7].RD_Q10 = Inlines.silk_ADD32(nsq_del_dec_struct.RD_Q10, num18);
						array[num6].Q_Q10 = num17;
						array[num7].Q_Q10 = num15;
					}
					int num21 = Inlines.silk_LSHIFT32(array[num6].Q_Q10, 4);
					if (nsq_del_dec_struct.Seed < 0)
					{
						num21 = -num21;
					}
					int num22 = Inlines.silk_ADD32(num21, num4);
					int num23 = Inlines.silk_ADD32(num22, num9);
					int num24 = Inlines.silk_SUB32(num23, num12);
					array[num6].sLTP_shp_Q14 = Inlines.silk_SUB32(num24, num13);
					array[num6].LF_AR_Q14 = num24;
					array[num6].LPC_exc_Q14 = num22;
					array[num6].xq_Q14 = num23;
					num21 = Inlines.silk_LSHIFT32(array[num7].Q_Q10, 4);
					if (nsq_del_dec_struct.Seed < 0)
					{
						num21 = -num21;
					}
					num22 = Inlines.silk_ADD32(num21, num4);
					num23 = Inlines.silk_ADD32(num22, num9);
					num24 = Inlines.silk_SUB32(num23, num12);
					array[num7].sLTP_shp_Q14 = Inlines.silk_SUB32(num24, num13);
					array[num7].LF_AR_Q14 = num24;
					array[num7].LPC_exc_Q14 = num22;
					array[num7].xq_Q14 = num23;
				}
				smpl_buf_idx.Val = (smpl_buf_idx.Val - 1 & 31);
				int num25 = smpl_buf_idx.Val + decisionDelay & 31;
				int rd_Q = array[0].RD_Q10;
				int num26 = 0;
				for (int k = 1; k < nStatesDelayedDecision; k++)
				{
					if (array[k * 2].RD_Q10 < rd_Q)
					{
						rd_Q = array[k * 2].RD_Q10;
						num26 = k;
					}
				}
				int num27 = psDelDec[num26].RandState[num25];
				for (int k = 0; k < nStatesDelayedDecision; k++)
				{
					if (psDelDec[k].RandState[num25] != num27)
					{
						int num28 = k * 2;
						array[num28].RD_Q10 = Inlines.silk_ADD32(array[num28].RD_Q10, 134217727);
						array[num28 + 1].RD_Q10 = Inlines.silk_ADD32(array[num28 + 1].RD_Q10, 134217727);
					}
				}
				int rd_Q2 = array[0].RD_Q10;
				rd_Q = array[1].RD_Q10;
				int num29 = 0;
				int num30 = 0;
				for (int k = 1; k < nStatesDelayedDecision; k++)
				{
					int num31 = k * 2;
					if (array[num31].RD_Q10 > rd_Q2)
					{
						rd_Q2 = array[num31].RD_Q10;
						num29 = k;
					}
					if (array[num31 + 1].RD_Q10 < rd_Q)
					{
						rd_Q = array[num31 + 1].RD_Q10;
						num30 = k;
					}
				}
				if (rd_Q < rd_Q2)
				{
					psDelDec[num29].PartialCopyFrom(psDelDec[num30], j);
					array[num29 * 2] = array[num30 * 2 + 1];
				}
				nsq_del_dec_struct = psDelDec[num26];
				if (subfr > 0 || j >= decisionDelay)
				{
					pulses[pulses_ptr + j - decisionDelay] = (sbyte)Inlines.silk_RSHIFT_ROUND(nsq_del_dec_struct.Q_Q10[num25], 10);
					xq[xq_ptr + j - decisionDelay] = (short)Inlines.silk_SAT16(Inlines.silk_RSHIFT_ROUND(Inlines.silk_SMULWW(nsq_del_dec_struct.Xq_Q14[num25], delayedGain_Q10[num25]), 8));
					this.sLTP_shp_Q14[this.sLTP_shp_buf_idx - decisionDelay] = nsq_del_dec_struct.Shape_Q14[num25];
					sLTP_Q15[this.sLTP_buf_idx - decisionDelay] = nsq_del_dec_struct.Pred_Q15[num25];
				}
				this.sLTP_shp_buf_idx++;
				this.sLTP_buf_idx++;
				for (int k = 0; k < nStatesDelayedDecision; k++)
				{
					nsq_del_dec_struct = psDelDec[k];
					int num6 = k * 2;
					nsq_del_dec_struct.LF_AR_Q14 = array[num6].LF_AR_Q14;
					nsq_del_dec_struct.sLPC_Q14[SilkConstants.NSQ_LPC_BUF_LENGTH + j] = array[num6].xq_Q14;
					nsq_del_dec_struct.Xq_Q14[smpl_buf_idx.Val] = array[num6].xq_Q14;
					nsq_del_dec_struct.Q_Q10[smpl_buf_idx.Val] = array[num6].Q_Q10;
					nsq_del_dec_struct.Pred_Q15[smpl_buf_idx.Val] = Inlines.silk_LSHIFT32(array[num6].LPC_exc_Q14, 1);
					nsq_del_dec_struct.Shape_Q14[smpl_buf_idx.Val] = array[num6].sLTP_shp_Q14;
					nsq_del_dec_struct.Seed = Inlines.silk_ADD32_ovflw(nsq_del_dec_struct.Seed, Inlines.silk_RSHIFT_ROUND(array[num6].Q_Q10, 10));
					nsq_del_dec_struct.RandState[smpl_buf_idx.Val] = nsq_del_dec_struct.Seed;
					nsq_del_dec_struct.RD_Q10 = array[num6].RD_Q10;
				}
				delayedGain_Q10[smpl_buf_idx.Val] = num3;
			}
			for (int k = 0; k < nStatesDelayedDecision; k++)
			{
				SilkNSQState.NSQ_del_dec_struct nsq_del_dec_struct = psDelDec[k];
				Buffer.BlockCopy(nsq_del_dec_struct.sLPC_Q14, length * 4, nsq_del_dec_struct.sLPC_Q14, 0, SilkConstants.NSQ_LPC_BUF_LENGTH * 4);
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x000185E8 File Offset: 0x000167E8
		private void silk_nsq_del_dec_scale_states(SilkChannelEncoder psEncC, SilkNSQState.NSQ_del_dec_struct[] psDelDec, int[] x_Q3, int x_Q3_ptr, int[] x_sc_Q10, short[] sLTP, int[] sLTP_Q15, int subfr, int nStatesDelayedDecision, int LTP_scale_Q14, int[] Gains_Q16, int[] pitchL, int signal_type, int decisionDelay)
		{
			int num = pitchL[subfr];
			int num2 = Inlines.silk_INVERSE32_varQ(Inlines.silk_max(Gains_Q16[subfr], 1), 47);
			int num3;
			if (Gains_Q16[subfr] != this.prev_gain_Q16)
			{
				num3 = Inlines.silk_DIV32_varQ(this.prev_gain_Q16, Gains_Q16[subfr], 16);
			}
			else
			{
				num3 = 65536;
			}
			int b = Inlines.silk_RSHIFT_ROUND(num2, 8);
			for (int i = 0; i < psEncC.subfr_length; i++)
			{
				x_sc_Q10[i] = Inlines.silk_SMULWW(x_Q3[x_Q3_ptr + i], b);
			}
			this.prev_gain_Q16 = Gains_Q16[subfr];
			if (this.rewhite_flag != 0)
			{
				if (subfr == 0)
				{
					num2 = Inlines.silk_LSHIFT(Inlines.silk_SMULWB(num2, LTP_scale_Q14), 2);
				}
				for (int i = this.sLTP_buf_idx - num - 2; i < this.sLTP_buf_idx; i++)
				{
					sLTP_Q15[i] = Inlines.silk_SMULWB(num2, (int)sLTP[i]);
				}
			}
			if (num3 != 65536)
			{
				for (int i = this.sLTP_shp_buf_idx - psEncC.ltp_mem_length; i < this.sLTP_shp_buf_idx; i++)
				{
					this.sLTP_shp_Q14[i] = Inlines.silk_SMULWW(num3, this.sLTP_shp_Q14[i]);
				}
				if (signal_type == 2 && this.rewhite_flag == 0)
				{
					for (int i = this.sLTP_buf_idx - num - 2; i < this.sLTP_buf_idx - decisionDelay; i++)
					{
						sLTP_Q15[i] = Inlines.silk_SMULWW(num3, sLTP_Q15[i]);
					}
				}
				for (int j = 0; j < nStatesDelayedDecision; j++)
				{
					SilkNSQState.NSQ_del_dec_struct nsq_del_dec_struct = psDelDec[j];
					nsq_del_dec_struct.LF_AR_Q14 = Inlines.silk_SMULWW(num3, nsq_del_dec_struct.LF_AR_Q14);
					for (int i = 0; i < SilkConstants.NSQ_LPC_BUF_LENGTH; i++)
					{
						nsq_del_dec_struct.sLPC_Q14[i] = Inlines.silk_SMULWW(num3, nsq_del_dec_struct.sLPC_Q14[i]);
					}
					for (int i = 0; i < psEncC.shapingLPCOrder; i++)
					{
						nsq_del_dec_struct.sAR2_Q14[i] = Inlines.silk_SMULWW(num3, nsq_del_dec_struct.sAR2_Q14[i]);
					}
					for (int i = 0; i < 32; i++)
					{
						nsq_del_dec_struct.Pred_Q15[i] = Inlines.silk_SMULWW(num3, nsq_del_dec_struct.Pred_Q15[i]);
						nsq_del_dec_struct.Shape_Q14[i] = Inlines.silk_SMULWW(num3, nsq_del_dec_struct.Shape_Q14[i]);
					}
				}
			}
		}

		// Token: 0x04000263 RID: 611
		internal readonly short[] xq = new short[640];

		// Token: 0x04000264 RID: 612
		internal readonly int[] sLTP_shp_Q14 = new int[640];

		// Token: 0x04000265 RID: 613
		internal readonly int[] sLPC_Q14 = new int[80 + SilkConstants.NSQ_LPC_BUF_LENGTH];

		// Token: 0x04000266 RID: 614
		internal readonly int[] sAR2_Q14 = new int[16];

		// Token: 0x04000267 RID: 615
		internal int sLF_AR_shp_Q14;

		// Token: 0x04000268 RID: 616
		internal int lagPrev;

		// Token: 0x04000269 RID: 617
		internal int sLTP_buf_idx;

		// Token: 0x0400026A RID: 618
		internal int sLTP_shp_buf_idx;

		// Token: 0x0400026B RID: 619
		internal int rand_seed;

		// Token: 0x0400026C RID: 620
		internal int prev_gain_Q16;

		// Token: 0x0400026D RID: 621
		internal int rewhite_flag;

		// Token: 0x0200009D RID: 157
		private class NSQ_del_dec_struct
		{
			// Token: 0x06000541 RID: 1345 RVA: 0x000362D8 File Offset: 0x000344D8
			internal NSQ_del_dec_struct(int shapingOrder)
			{
				this.sAR2_Q14 = new int[shapingOrder];
			}

			// Token: 0x06000542 RID: 1346 RVA: 0x0003634C File Offset: 0x0003454C
			internal void PartialCopyFrom(SilkNSQState.NSQ_del_dec_struct other, int q14Offset)
			{
				Buffer.BlockCopy(other.sLPC_Q14, q14Offset * 4, this.sLPC_Q14, q14Offset * 4, (80 + SilkConstants.NSQ_LPC_BUF_LENGTH - q14Offset) * 4);
				Buffer.BlockCopy(other.RandState, 0, this.RandState, 0, 128);
				Buffer.BlockCopy(other.Q_Q10, 0, this.Q_Q10, 0, 128);
				Buffer.BlockCopy(other.Xq_Q14, 0, this.Xq_Q14, 0, 128);
				Buffer.BlockCopy(other.Pred_Q15, 0, this.Pred_Q15, 0, 128);
				Buffer.BlockCopy(other.Shape_Q14, 0, this.Shape_Q14, 0, 128);
				Buffer.BlockCopy(other.sAR2_Q14, 0, this.sAR2_Q14, 0, this.sAR2_Q14.Length * 4);
				this.LF_AR_Q14 = other.LF_AR_Q14;
				this.Seed = other.Seed;
				this.SeedInit = other.SeedInit;
				this.RD_Q10 = other.RD_Q10;
			}

			// Token: 0x06000543 RID: 1347 RVA: 0x00036441 File Offset: 0x00034641
			internal void Assign(SilkNSQState.NSQ_del_dec_struct other)
			{
				this.PartialCopyFrom(other, 0);
			}

			// Token: 0x040005DA RID: 1498
			internal readonly int[] sLPC_Q14 = new int[80 + SilkConstants.NSQ_LPC_BUF_LENGTH];

			// Token: 0x040005DB RID: 1499
			internal readonly int[] RandState = new int[32];

			// Token: 0x040005DC RID: 1500
			internal readonly int[] Q_Q10 = new int[32];

			// Token: 0x040005DD RID: 1501
			internal readonly int[] Xq_Q14 = new int[32];

			// Token: 0x040005DE RID: 1502
			internal readonly int[] Pred_Q15 = new int[32];

			// Token: 0x040005DF RID: 1503
			internal readonly int[] Shape_Q14 = new int[32];

			// Token: 0x040005E0 RID: 1504
			internal int[] sAR2_Q14;

			// Token: 0x040005E1 RID: 1505
			internal int LF_AR_Q14;

			// Token: 0x040005E2 RID: 1506
			internal int Seed;

			// Token: 0x040005E3 RID: 1507
			internal int SeedInit;

			// Token: 0x040005E4 RID: 1508
			internal int RD_Q10;
		}

		// Token: 0x0200009E RID: 158
		private struct NSQ_sample_struct
		{
			// Token: 0x040005E5 RID: 1509
			internal int Q_Q10;

			// Token: 0x040005E6 RID: 1510
			internal int RD_Q10;

			// Token: 0x040005E7 RID: 1511
			internal int xq_Q14;

			// Token: 0x040005E8 RID: 1512
			internal int LF_AR_Q14;

			// Token: 0x040005E9 RID: 1513
			internal int sLTP_shp_Q14;

			// Token: 0x040005EA RID: 1514
			internal int LPC_exc_Q14;
		}
	}
}
