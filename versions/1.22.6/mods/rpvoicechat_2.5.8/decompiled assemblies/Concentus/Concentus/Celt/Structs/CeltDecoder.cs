using System;
using Concentus.Common;
using Concentus.Common.CPlusPlus;

namespace Concentus.Celt.Structs
{
	// Token: 0x02000093 RID: 147
	internal class CeltDecoder
	{
		// Token: 0x0600050B RID: 1291 RVA: 0x00033136 File Offset: 0x00031336
		private void Reset()
		{
			this.mode = null;
			this.overlap = 0;
			this.channels = 0;
			this.stream_channels = 0;
			this.downsample = 0;
			this.start = 0;
			this.end = 0;
			this.signalling = 0;
			this.PartialReset();
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x00033178 File Offset: 0x00031378
		private void PartialReset()
		{
			this.rng = 0U;
			this.error = 0;
			this.last_pitch_index = 0;
			this.loss_count = 0;
			this.postfilter_period = 0;
			this.postfilter_period_old = 0;
			this.postfilter_gain = 0;
			this.postfilter_gain_old = 0;
			this.postfilter_tapset = 0;
			this.postfilter_tapset_old = 0;
			Arrays.MemSetInt(this.preemph_memD, 0, 2);
			this.decode_mem = null;
			this.lpc = null;
			this.oldEBands = null;
			this.oldLogE = null;
			this.oldLogE2 = null;
			this.backgroundLogE = null;
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x00033204 File Offset: 0x00031404
		internal void ResetState()
		{
			this.PartialReset();
			this.decode_mem = new int[this.channels][];
			this.lpc = new int[this.channels][];
			for (int i = 0; i < this.channels; i++)
			{
				this.decode_mem[i] = new int[2048 + this.mode.overlap];
				this.lpc[i] = new int[24];
			}
			this.oldEBands = new int[2 * this.mode.nbEBands];
			this.oldLogE = new int[2 * this.mode.nbEBands];
			this.oldLogE2 = new int[2 * this.mode.nbEBands];
			this.backgroundLogE = new int[2 * this.mode.nbEBands];
			for (int j = 0; j < 2 * this.mode.nbEBands; j++)
			{
				this.oldLogE[j] = (this.oldLogE2[j] = -28672);
			}
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00033308 File Offset: 0x00031508
		internal int celt_decoder_init(int sampling_rate, int channels)
		{
			int num = this.opus_custom_decoder_init(CeltMode.mode48000_960_120, channels);
			if (num != 0)
			{
				return num;
			}
			this.downsample = CeltCommon.resampling_factor(sampling_rate);
			if (this.downsample == 0)
			{
				return -1;
			}
			return 0;
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x00033340 File Offset: 0x00031540
		private int opus_custom_decoder_init(CeltMode mode, int channels)
		{
			if (channels < 0 || channels > 2)
			{
				return -1;
			}
			if (this == null)
			{
				return -7;
			}
			this.Reset();
			this.mode = mode;
			this.overlap = mode.overlap;
			this.channels = channels;
			this.stream_channels = channels;
			this.downsample = 1;
			this.start = 0;
			this.end = this.mode.effEBands;
			this.signalling = 1;
			this.loss_count = 0;
			this.ResetState();
			return 0;
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x000333BC File Offset: 0x000315BC
		internal unsafe void celt_decode_lost(int N, int LM)
		{
			int num = this.channels;
			int[][] array = new int[2][];
			int[] array2 = new int[2];
			CeltMode celtMode = this.mode;
			int nbEBands = celtMode.nbEBands;
			int num2 = celtMode.overlap;
			short[] eBands = celtMode.eBands;
			int i = 0;
			do
			{
				array[i] = this.decode_mem[i];
				array2[i] = 2048 - N;
			}
			while (++i < num);
			if (this.loss_count >= 5 || this.start != 0)
			{
				int num3 = this.end;
				int num4 = Inlines.IMAX(this.start, Inlines.IMIN(num3, celtMode.effEBands));
				int[][] array3 = Arrays.InitTwoDimensionalArray<int>(num, N);
				int num5 = (this.loss_count == 0) ? 1536 : 512;
				i = 0;
				do
				{
					for (int j = this.start; j < num3; j++)
					{
						this.oldEBands[i * nbEBands + j] = Inlines.MAX16(this.backgroundLogE[i * nbEBands + j], this.oldEBands[i * nbEBands + j] - num5);
					}
				}
				while (++i < num);
				uint num6 = this.rng;
				for (i = 0; i < num; i++)
				{
					for (int j = this.start; j < num4; j++)
					{
						int num7 = (int)eBands[j] << LM;
						int num8 = (int)(eBands[j + 1] - eBands[j]) << LM;
						for (int k = 0; k < num8; k++)
						{
							num6 = Bands.celt_lcg_rand(num6);
							array3[i][num7 + k] = (int)num6 >> 20;
						}
						VQ.renormalise_vector(MemoryExtensions.AsSpan<int>(array3[i]), num8, 32767);
					}
				}
				this.rng = num6;
				i = 0;
				do
				{
					Arrays.MemMoveInt(this.decode_mem[i], N, 0, 2048 - N + (num2 >> 1));
				}
				while (++i < num);
				CeltCommon.celt_synthesis(celtMode, array3, array, array2, this.oldEBands, this.start, num4, num, num, 0, LM, this.downsample, 0);
			}
			else
			{
				int a = 32767;
				int num9;
				if (this.loss_count == 0)
				{
					num9 = (this.last_pitch_index = CeltCommon.celt_plc_pitch_search(this.decode_mem, num));
				}
				else
				{
					num9 = this.last_pitch_index;
					a = 26214;
				}
				int[] array4 = new int[num2];
				int[] array5 = new int[1024];
				Span<int> mem = new Span<int>(stackalloc byte[(UIntPtr)96], 24);
				int[] window = celtMode.window;
				i = 0;
				do
				{
					int num10 = 0;
					int[] array6 = this.decode_mem[i];
					int j;
					for (j = 0; j < 1024; j++)
					{
						array5[j] = Inlines.ROUND16(array6[1024 + j], 12);
					}
					if (this.loss_count == 0)
					{
						int[] array7 = new int[25];
						Autocorrelation._celt_autocorr(array5, array7, window, num2, 24, 1024);
						array7[0] += Inlines.SHR32(array7[0], 13);
						for (j = 1; j <= 24; j++)
						{
							array7[j] -= Inlines.MULT16_32_Q15(2 * j * j, array7[j]);
						}
						CeltLPC.celt_lpc(this.lpc[i], array7, 24);
					}
					int num11 = Inlines.IMIN(2 * num9, 1024);
					for (j = 0; j < 24; j++)
					{
						*mem[j] = Inlines.ROUND16(array6[2048 - num11 - 1 - j], 12);
					}
					Kernels.celt_fir(MemoryExtensions.AsSpan<int>(array5).Slice(1024 - num11), MemoryExtensions.AsSpan<int>(this.lpc[i]), MemoryExtensions.AsSpan<int>(array5).Slice(1024 - num11), num11, 24, mem);
					int num12 = 1;
					int num13 = 1;
					int shift = Inlines.IMAX(0, 2 * Inlines.celt_zlog2(Inlines.celt_maxabs16(array5, 1024 - num11, num11)) - 20);
					int num14 = num11 >> 1;
					for (j = 0; j < num14; j++)
					{
						int num15 = array5[1024 - num14 + j];
						num12 += Inlines.SHR32(Inlines.MULT16_16(num15, num15), shift);
						num15 = array5[1024 - 2 * num14 + j];
						num13 += Inlines.SHR32(Inlines.MULT16_16(num15, num15), shift);
					}
					num12 = Inlines.MIN32(num12, num13);
					int b = Inlines.celt_sqrt(Inlines.frac_div32(Inlines.SHR32(num12, 1), num13));
					Arrays.MemMoveInt(array6, N, 0, 2048 - N);
					int num16 = 1024 - num9;
					int num17 = N + num2;
					int a2 = Inlines.MULT16_16_Q15(a, b);
					int num18 = j = 0;
					while (j < num17)
					{
						if (num18 >= num9)
						{
							num18 -= num9;
							a2 = Inlines.MULT16_16_Q15(a2, b);
						}
						array6[2048 - N + j] = Inlines.SHL32(Inlines.MULT16_16_Q15(a2, array5[num16 + num18]), 12);
						int num19 = Inlines.ROUND16(array6[1024 - N + num16 + num18], 12);
						num10 += Inlines.SHR32(Inlines.MULT16_16(num19, num19), 8);
						j++;
						num18++;
					}
					for (j = 0; j < 24; j++)
					{
						*mem[j] = Inlines.ROUND16(array6[2048 - N - 1 - j], 12);
					}
					CeltLPC.celt_iir(MemoryExtensions.AsSpan<int>(array6).Slice(2048 - N), this.lpc[i], MemoryExtensions.AsSpan<int>(array6).Slice(2048 - N), num17, 24, mem);
					int num20 = 0;
					for (j = 0; j < num17; j++)
					{
						int num21 = Inlines.ROUND16(array6[2048 - N + j], 12);
						num20 += Inlines.SHR32(Inlines.MULT16_16(num21, num21), 8);
					}
					if (num10 <= Inlines.SHR32(num20, 2))
					{
						for (j = 0; j < num17; j++)
						{
							array6[2048 - N + j] = 0;
						}
					}
					else if (num10 < num20)
					{
						int num22 = Inlines.celt_sqrt(Inlines.frac_div32(Inlines.SHR32(num10, 1) + 1, num20 + 1));
						for (j = 0; j < num2; j++)
						{
							int a3 = 32767 - Inlines.MULT16_16_Q15(window[j], 32767 - num22);
							array6[2048 - N + j] = Inlines.MULT16_32_Q15(a3, array6[2048 - N + j]);
						}
						for (j = num2; j < num17; j++)
						{
							array6[2048 - N + j] = Inlines.MULT16_32_Q15(num22, array6[2048 - N + j]);
						}
					}
					CeltCommon.comb_filter(array4, 0, array6, 2048, this.postfilter_period, this.postfilter_period, num2, -this.postfilter_gain, -this.postfilter_gain, this.postfilter_tapset, this.postfilter_tapset, null, 0);
					for (j = 0; j < num2 / 2; j++)
					{
						array6[2048 + j] = Inlines.MULT16_32_Q15(window[j], array4[num2 - 1 - j]) + Inlines.MULT16_32_Q15(window[num2 - j - 1], array4[j]);
					}
				}
				while (++i < num);
			}
			this.loss_count++;
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x00033A84 File Offset: 0x00031C84
		internal int celt_decode_with_ec(ReadOnlySpan<byte> data, int data_ptr, int len, Span<short> pcm, int pcm_ptr, int frame_size, EntropyCoder dec, int accum)
		{
			int[][] array = new int[2][];
			int[] array2 = new int[2];
			int num = this.channels;
			int intensity = 0;
			int dual_stereo = 0;
			int num2 = 0;
			int num3 = this.stream_channels;
			CeltMode celtMode = this.mode;
			int nbEBands = celtMode.nbEBands;
			int num4 = celtMode.overlap;
			short[] eBands = celtMode.eBands;
			int num5 = this.start;
			int num6 = this.end;
			frame_size *= this.downsample;
			int[] array3 = this.oldEBands;
			int[] array4 = this.oldLogE;
			int[] array5 = this.oldLogE2;
			int[] array6 = this.backgroundLogE;
			int num7 = 0;
			while (num7 <= celtMode.maxLM && celtMode.shortMdctSize << num7 != frame_size)
			{
				num7++;
			}
			if (num7 > celtMode.maxLM)
			{
				return -1;
			}
			int num8 = 1 << num7;
			if (len < 0 || len > 1275 || pcm.IsEmpty)
			{
				return -1;
			}
			int num9 = num8 * celtMode.shortMdctSize;
			int num10 = 0;
			do
			{
				array[num10] = this.decode_mem[num10];
				array2[num10] = 2048 - num9;
			}
			while (++num10 < num);
			int num11 = num6;
			if (num11 > celtMode.effEBands)
			{
				num11 = celtMode.effEBands;
			}
			if (data.IsEmpty || len <= 1)
			{
				this.celt_decode_lost(num9, num7);
				CeltCommon.deemphasis(array, array2, pcm, pcm_ptr, num9, num, this.downsample, celtMode.preemph, this.preemph_memD, accum);
				return frame_size / this.downsample;
			}
			if (dec == null)
			{
				dec = new EntropyCoder();
				dec.dec_init(data.Slice(data_ptr), (uint)len);
			}
			if (num3 == 1)
			{
				for (int i = 0; i < nbEBands; i++)
				{
					array3[i] = Inlines.MAX16(array3[i], array3[nbEBands + i]);
				}
			}
			int num12 = len * 8;
			int num13 = dec.tell();
			int num14;
			if (num13 >= num12)
			{
				num14 = 1;
			}
			else if (num13 == 1)
			{
				num14 = dec.dec_bit_logp(data.Slice(data_ptr), 15U);
			}
			else
			{
				num14 = 0;
			}
			if (num14 != 0)
			{
				num13 = len * 8;
				dec.nbits_total += num13 - dec.tell();
			}
			int g = 0;
			int t = 0;
			int tapset = 0;
			if (num5 == 0 && num13 + 16 <= num12)
			{
				if (dec.dec_bit_logp(data.Slice(data_ptr), 1U) != 0)
				{
					int num15 = (int)dec.dec_uint(data.Slice(data_ptr), 6U);
					t = (int)((16U << num15) + dec.dec_bits(data.Slice(data_ptr), (uint)(4 + num15)) - 1U);
					int num16 = (int)dec.dec_bits(data.Slice(data_ptr), 3U);
					if (dec.tell() + 2 <= num12)
					{
						tapset = dec.dec_icdf(data.Slice(data_ptr), Concentus.Celt.Tables.tapset_icdf, 2U);
					}
					g = 3072 * (num16 + 1);
				}
				num13 = dec.tell();
			}
			int num17;
			if (num7 > 0 && num13 + 3 <= num12)
			{
				num17 = dec.dec_bit_logp(data.Slice(data_ptr), 3U);
				num13 = dec.tell();
			}
			else
			{
				num17 = 0;
			}
			int shortBlocks;
			if (num17 != 0)
			{
				shortBlocks = num8;
			}
			else
			{
				shortBlocks = 0;
			}
			int intra = (num13 + 3 <= num12) ? dec.dec_bit_logp(data.Slice(data_ptr), 3U) : 0;
			QuantizeBands.unquant_coarse_energy(celtMode, num5, num6, array3, intra, dec, data.Slice(data_ptr), num3, num7);
			int[] tf_res = new int[nbEBands];
			CeltCommon.tf_decode(num5, num6, num17, tf_res, num7, dec, data.Slice(data_ptr));
			num13 = dec.tell();
			int spread = 2;
			if (num13 + 4 <= num12)
			{
				spread = dec.dec_icdf(data.Slice(data_ptr), Concentus.Celt.Tables.spread_icdf, 5U);
			}
			int[] array7 = new int[nbEBands];
			CeltCommon.init_caps(celtMode, array7, num7, num3);
			int[] array8 = new int[nbEBands];
			int num18 = 6;
			num12 <<= 3;
			num13 = (int)dec.tell_frac();
			for (int i = num5; i < num6; i++)
			{
				int num19 = num3 * (int)(eBands[i + 1] - eBands[i]) << num7;
				int num20 = Inlines.IMIN(num19 << 3, Inlines.IMAX(48, num19));
				int num21 = num18;
				int num22 = 0;
				while (num13 + (num21 << 3) < num12 && num22 < array7[i])
				{
					bool flag = dec.dec_bit_logp(data.Slice(data_ptr), (uint)num21) != 0;
					num13 = (int)dec.tell_frac();
					if (!flag)
					{
						break;
					}
					num22 += num20;
					num12 -= num20;
					num21 = 1;
				}
				array8[i] = num22;
				if (num22 > 0)
				{
					num18 = Inlines.IMAX(2, num18 - 1);
				}
			}
			int[] array9 = new int[nbEBands];
			int alloc_trim = (num13 + 48 <= num12) ? dec.dec_icdf(data.Slice(data_ptr), Concentus.Celt.Tables.trim_icdf, 7U) : 5;
			int num23 = (len * 8 << 3) - (int)dec.tell_frac() - 1;
			int num24 = (num17 != 0 && num7 >= 2 && num23 >= num7 + 2 << 3) ? 8 : 0;
			num23 -= num24;
			int[] pulses = new int[nbEBands];
			int[] fine_priority = new int[nbEBands];
			int balance;
			int codedBands = Rate.compute_allocation_decode(celtMode, num5, num6, array8, array7, alloc_trim, ref intensity, ref dual_stereo, num23, out balance, pulses, array9, fine_priority, num3, num7, dec, data.Slice(data_ptr), 0, 0);
			QuantizeBands.unquant_fine_energy(celtMode, num5, num6, array3, array9, dec, data.Slice(data_ptr), num3);
			num10 = 0;
			do
			{
				Arrays.MemMoveInt(this.decode_mem[num10], num9, 0, 2048 - num9 + num4 / 2);
			}
			while (++num10 < num);
			byte[] collapse_masks = new byte[num3 * nbEBands];
			int[][] array10 = Arrays.InitTwoDimensionalArray<int>(num3, num9);
			Bands.quant_all_bands_decode(0, celtMode, num5, num6, array10[0], (num3 == 2) ? array10[1] : null, collapse_masks, null, pulses, shortBlocks, spread, dual_stereo, intensity, tf_res, len * 64 - num24, balance, dec, data.Slice(data_ptr), num7, codedBands, ref this.rng);
			if (num24 > 0)
			{
				num2 = (int)dec.dec_bits(data.Slice(data_ptr), 1U);
			}
			QuantizeBands.unquant_energy_finalise(celtMode, num5, num6, array3, array9, fine_priority, len * 8 - dec.tell(), dec, data.Slice(data_ptr), num3);
			if (num2 != 0)
			{
				Bands.anti_collapse(celtMode, array10, collapse_masks, num7, num3, num9, num5, num6, array3, array4, array5, pulses, this.rng);
			}
			if (num14 != 0)
			{
				for (int i = 0; i < num3 * nbEBands; i++)
				{
					array3[i] = -28672;
				}
			}
			CeltCommon.celt_synthesis(celtMode, array10, array, array2, array3, num5, num11, num3, num, num17, num7, this.downsample, num14);
			num10 = 0;
			do
			{
				this.postfilter_period = Inlines.IMAX(this.postfilter_period, 15);
				this.postfilter_period_old = Inlines.IMAX(this.postfilter_period_old, 15);
				CeltCommon.comb_filter(array[num10], array2[num10], array[num10], array2[num10], this.postfilter_period_old, this.postfilter_period, celtMode.shortMdctSize, this.postfilter_gain_old, this.postfilter_gain, this.postfilter_tapset_old, this.postfilter_tapset, celtMode.window, num4);
				if (num7 != 0)
				{
					CeltCommon.comb_filter(array[num10], array2[num10] + celtMode.shortMdctSize, array[num10], array2[num10] + celtMode.shortMdctSize, this.postfilter_period, t, num9 - celtMode.shortMdctSize, this.postfilter_gain, g, this.postfilter_tapset, tapset, celtMode.window, num4);
				}
			}
			while (++num10 < num);
			this.postfilter_period_old = this.postfilter_period;
			this.postfilter_gain_old = this.postfilter_gain;
			this.postfilter_tapset_old = this.postfilter_tapset;
			this.postfilter_period = t;
			this.postfilter_gain = g;
			this.postfilter_tapset = tapset;
			if (num7 != 0)
			{
				this.postfilter_period_old = this.postfilter_period;
				this.postfilter_gain_old = this.postfilter_gain;
				this.postfilter_tapset_old = this.postfilter_tapset;
			}
			if (num3 == 1)
			{
				Arrays.MemCopy(array3, 0, array3, nbEBands, nbEBands);
			}
			if (num17 == 0)
			{
				Arrays.MemCopy(array4, 0, array5, 0, 2 * nbEBands);
				Arrays.MemCopy(array3, 0, array4, 0, 2 * nbEBands);
				int num25;
				if (this.loss_count < 10)
				{
					num25 = num8;
				}
				else
				{
					num25 = 1024;
				}
				for (int i = 0; i < 2 * nbEBands; i++)
				{
					array6[i] = Inlines.MIN16(array6[i] + num25, array3[i]);
				}
			}
			else
			{
				for (int i = 0; i < 2 * nbEBands; i++)
				{
					array4[i] = Inlines.MIN16(array4[i], array3[i]);
				}
			}
			num10 = 0;
			do
			{
				for (int i = 0; i < num5; i++)
				{
					array3[num10 * nbEBands + i] = 0;
					array4[num10 * nbEBands + i] = (array5[num10 * nbEBands + i] = -28672);
				}
				for (int i = num6; i < nbEBands; i++)
				{
					array3[num10 * nbEBands + i] = 0;
					array4[num10 * nbEBands + i] = (array5[num10 * nbEBands + i] = -28672);
				}
			}
			while (++num10 < 2);
			this.rng = dec.rng;
			CeltCommon.deemphasis(array, array2, pcm, pcm_ptr, num9, num, this.downsample, celtMode.preemph, this.preemph_memD, accum);
			this.loss_count = 0;
			if (dec.tell() > 8 * len)
			{
				return -3;
			}
			if (dec.get_error() != 0)
			{
				this.error = 1;
			}
			return frame_size / this.downsample;
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x00034355 File Offset: 0x00032555
		internal void SetStartBand(int value)
		{
			if (value < 0 || value >= this.mode.nbEBands)
			{
				throw new ArgumentException("Start band above max number of ebands (or negative)");
			}
			this.start = value;
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0003437B File Offset: 0x0003257B
		internal void SetEndBand(int value)
		{
			if (value < 1 || value > this.mode.nbEBands)
			{
				throw new ArgumentException("End band above max number of ebands (or less than 1)");
			}
			this.end = value;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x000343A1 File Offset: 0x000325A1
		internal void SetChannels(int value)
		{
			if (value < 1 || value > 2)
			{
				throw new ArgumentException("Channel count must be 1 or 2");
			}
			this.stream_channels = value;
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x000343BD File Offset: 0x000325BD
		internal int GetAndClearError()
		{
			int result = this.error;
			this.error = 0;
			return result;
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x000343CC File Offset: 0x000325CC
		internal int GetLookahead()
		{
			return this.overlap / this.downsample;
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x000343DB File Offset: 0x000325DB
		internal int GetPitch()
		{
			return this.postfilter_period;
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x000343E3 File Offset: 0x000325E3
		internal CeltMode GetMode()
		{
			return this.mode;
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x000343EB File Offset: 0x000325EB
		internal void SetSignalling(int value)
		{
			this.signalling = value;
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x000343F4 File Offset: 0x000325F4
		internal uint GetFinalRange()
		{
			return this.rng;
		}

		// Token: 0x0400047F RID: 1151
		internal CeltMode mode;

		// Token: 0x04000480 RID: 1152
		internal int overlap;

		// Token: 0x04000481 RID: 1153
		internal int channels;

		// Token: 0x04000482 RID: 1154
		internal int stream_channels;

		// Token: 0x04000483 RID: 1155
		internal int downsample;

		// Token: 0x04000484 RID: 1156
		internal int start;

		// Token: 0x04000485 RID: 1157
		internal int end;

		// Token: 0x04000486 RID: 1158
		internal int signalling;

		// Token: 0x04000487 RID: 1159
		internal uint rng;

		// Token: 0x04000488 RID: 1160
		internal int error;

		// Token: 0x04000489 RID: 1161
		internal int last_pitch_index;

		// Token: 0x0400048A RID: 1162
		internal int loss_count;

		// Token: 0x0400048B RID: 1163
		internal int postfilter_period;

		// Token: 0x0400048C RID: 1164
		internal int postfilter_period_old;

		// Token: 0x0400048D RID: 1165
		internal int postfilter_gain;

		// Token: 0x0400048E RID: 1166
		internal int postfilter_gain_old;

		// Token: 0x0400048F RID: 1167
		internal int postfilter_tapset;

		// Token: 0x04000490 RID: 1168
		internal int postfilter_tapset_old;

		// Token: 0x04000491 RID: 1169
		internal readonly int[] preemph_memD = new int[2];

		// Token: 0x04000492 RID: 1170
		internal int[][] decode_mem;

		// Token: 0x04000493 RID: 1171
		internal int[][] lpc;

		// Token: 0x04000494 RID: 1172
		internal int[] oldEBands;

		// Token: 0x04000495 RID: 1173
		internal int[] oldLogE;

		// Token: 0x04000496 RID: 1174
		internal int[] oldLogE2;

		// Token: 0x04000497 RID: 1175
		internal int[] backgroundLogE;
	}
}
