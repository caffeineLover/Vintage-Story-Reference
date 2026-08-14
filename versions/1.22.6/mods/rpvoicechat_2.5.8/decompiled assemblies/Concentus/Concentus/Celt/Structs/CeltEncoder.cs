using System;
using Concentus.Common;
using Concentus.Common.CPlusPlus;
using Concentus.Enums;

namespace Concentus.Celt.Structs
{
	// Token: 0x02000094 RID: 148
	internal class CeltEncoder
	{
		// Token: 0x0600051C RID: 1308 RVA: 0x00034410 File Offset: 0x00032610
		private void Reset()
		{
			this.mode = null;
			this.channels = 0;
			this.stream_channels = 0;
			this.force_intra = 0;
			this.clip = 0;
			this.disable_pf = 0;
			this.complexity = 0;
			this.upsample = 0;
			this.start = 0;
			this.end = 0;
			this.bitrate = 0;
			this.vbr = 0;
			this.signalling = 0;
			this.constrained_vbr = 0;
			this.loss_rate = 0;
			this.lsb_depth = 0;
			this.variable_duration = (OpusFramesize)0;
			this.lfe = 0;
			this.PartialReset();
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x000344A4 File Offset: 0x000326A4
		private void PartialReset()
		{
			this.rng = 0U;
			this.spread_decision = 0;
			this.delayedIntra = 0;
			this.tonal_average = 0;
			this.lastCodedBands = 0;
			this.hf_average = 0;
			this.tapset_decision = 0;
			this.prefilter_period = 0;
			this.prefilter_gain = 0;
			this.prefilter_tapset = 0;
			this.consec_transient = 0;
			this.analysis.Reset();
			this.preemph_memE[0] = 0;
			this.preemph_memE[1] = 0;
			this.preemph_memD[0] = 0;
			this.preemph_memD[1] = 0;
			this.vbr_reservoir = 0;
			this.vbr_drift = 0;
			this.vbr_offset = 0;
			this.vbr_count = 0;
			this.overlap_max = 0;
			this.stereo_saving = 0;
			this.intensity = 0;
			this.energy_mask = null;
			this.spec_avg = 0;
			this.in_mem = null;
			this.prefilter_mem = null;
			this.oldBandE = null;
			this.oldLogE = null;
			this.oldLogE2 = null;
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x00034590 File Offset: 0x00032790
		internal void ResetState()
		{
			this.PartialReset();
			this.in_mem = Arrays.InitTwoDimensionalArray<int>(this.channels, this.mode.overlap);
			this.prefilter_mem = Arrays.InitTwoDimensionalArray<int>(this.channels, 1024);
			this.oldBandE = Arrays.InitTwoDimensionalArray<int>(this.channels, this.mode.nbEBands);
			this.oldLogE = Arrays.InitTwoDimensionalArray<int>(this.channels, this.mode.nbEBands);
			this.oldLogE2 = Arrays.InitTwoDimensionalArray<int>(this.channels, this.mode.nbEBands);
			for (int i = 0; i < this.mode.nbEBands; i++)
			{
				this.oldLogE[0][i] = (this.oldLogE2[0][i] = -28672);
			}
			if (this.channels == 2)
			{
				for (int i = 0; i < this.mode.nbEBands; i++)
				{
					this.oldLogE[1][i] = (this.oldLogE2[1][i] = -28672);
				}
			}
			this.vbr_offset = 0;
			this.delayedIntra = 1;
			this.spread_decision = 2;
			this.tonal_average = 256;
			this.hf_average = 0;
			this.tapset_decision = 0;
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x000346C4 File Offset: 0x000328C4
		internal int opus_custom_encoder_init_arch(CeltMode mode, int channels)
		{
			if (channels < 0 || channels > 2)
			{
				return -1;
			}
			if (this == null || mode == null)
			{
				return -7;
			}
			this.Reset();
			this.mode = mode;
			this.channels = channels;
			this.stream_channels = channels;
			this.upsample = 1;
			this.start = 0;
			this.end = this.mode.effEBands;
			this.signalling = 1;
			this.constrained_vbr = 1;
			this.clip = 1;
			this.bitrate = -1;
			this.vbr = 0;
			this.force_intra = 0;
			this.complexity = 5;
			this.lsb_depth = 24;
			this.ResetState();
			return 0;
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x00034760 File Offset: 0x00032960
		internal int celt_encoder_init(int sampling_rate, int channels)
		{
			int num = this.opus_custom_encoder_init_arch(CeltMode.mode48000_960_120, channels);
			if (num != 0)
			{
				return num;
			}
			this.upsample = CeltCommon.resampling_factor(sampling_rate);
			return 0;
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0003478C File Offset: 0x0003298C
		internal int run_prefilter(int[][] input, int[][] prefilter_mem, int CC, int N, int prefilter_tapset, out int pitch, out int gain, out int qgain, int enabled, int nbAvailableBytes)
		{
			int[][] array = new int[CC][];
			CeltMode celtMode = this.mode;
			int overlap = celtMode.overlap;
			for (int i = 0; i < CC; i++)
			{
				array[i] = new int[N + 1024];
			}
			int num = 0;
			do
			{
				Arrays.MemCopy(prefilter_mem[num], 0, array[num], 0, 1024);
				Arrays.MemCopy(input[num], overlap, array[num], 1024, N);
			}
			while (++num < CC);
			int num2;
			int num3;
			if (enabled != 0)
			{
				int[] array2 = new int[1024 + N >> 1];
				Pitch.pitch_downsample(array, array2, 1024 + N, CC);
				Pitch.pitch_search(array2, 512, array2, N, 979, out num2);
				num2 = 1024 - num2;
				num3 = Pitch.remove_doubling(array2, 1024, 15, N, ref num2, this.prefilter_period, this.prefilter_gain);
				if (num2 > 1022)
				{
					num2 = 1022;
				}
				num3 = Inlines.MULT16_16_Q15(22938, num3);
				if (this.loss_rate > 2)
				{
					num3 = Inlines.HALF32(num3);
				}
				if (this.loss_rate > 4)
				{
					num3 = Inlines.HALF32(num3);
				}
				if (this.loss_rate > 8)
				{
					num3 = 0;
				}
			}
			else
			{
				num3 = 0;
				num2 = 15;
			}
			int num4 = 6554;
			if (Inlines.abs(num2 - this.prefilter_period) * 10 > num2)
			{
				num4 += 6554;
			}
			if (nbAvailableBytes < 25)
			{
				num4 += 3277;
			}
			if (nbAvailableBytes < 35)
			{
				num4 += 3277;
			}
			if (this.prefilter_gain > 13107)
			{
				num4 -= 3277;
			}
			if (this.prefilter_gain > 18022)
			{
				num4 -= 3277;
			}
			num4 = Inlines.MAX16(num4, 6554);
			int result;
			int num5;
			if (num3 < num4)
			{
				num3 = 0;
				result = 0;
				num5 = 0;
			}
			else
			{
				if (Inlines.ABS32(num3 - this.prefilter_gain) < 3277)
				{
					num3 = this.prefilter_gain;
				}
				num5 = (num3 + 1536 >> 10) / 3 - 1;
				num5 = Inlines.IMAX(0, Inlines.IMIN(7, num5));
				num3 = 3072 * (num5 + 1);
				result = 1;
			}
			num = 0;
			do
			{
				int num6 = celtMode.shortMdctSize - overlap;
				this.prefilter_period = Inlines.IMAX(this.prefilter_period, 15);
				Arrays.MemCopy(this.in_mem[num], 0, input[num], 0, overlap);
				if (num6 != 0)
				{
					CeltCommon.comb_filter(input[num], overlap, array[num], 1024, this.prefilter_period, this.prefilter_period, num6, -this.prefilter_gain, -this.prefilter_gain, this.prefilter_tapset, this.prefilter_tapset, null, 0);
				}
				CeltCommon.comb_filter(input[num], overlap + num6, array[num], 1024 + num6, this.prefilter_period, num2, N - num6, -this.prefilter_gain, -num3, this.prefilter_tapset, prefilter_tapset, celtMode.window, overlap);
				Arrays.MemCopy(input[num], N, this.in_mem[num], 0, overlap);
				if (N > 1024)
				{
					Arrays.MemCopy(array[num], N, prefilter_mem[num], 0, 1024);
				}
				else
				{
					Arrays.MemMoveInt(prefilter_mem[num], N, 0, 1024 - N);
					Arrays.MemCopy(array[num], 1024, prefilter_mem[num], 1024 - N, N);
				}
			}
			while (++num < CC);
			gain = num3;
			pitch = num2;
			qgain = num5;
			return result;
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x00034AE0 File Offset: 0x00032CE0
		internal int celt_encode_with_ec(Span<short> pcm, int pcm_ptr, int frame_size, Span<byte> compressed, int compressed_ptr, int nbCompressedBytes, EntropyCoder enc)
		{
			int num = this.channels;
			int num2 = this.stream_channels;
			int num3 = 15;
			int num4 = 0;
			int dual_stereo = 0;
			int tf_chan = 0;
			int pitch_change = 0;
			int num5 = 0;
			int surround_masking = 0;
			int num6 = 0;
			int surround_trim = 0;
			int num7 = 510000;
			CeltMode celtMode = this.mode;
			int nbEBands = celtMode.nbEBands;
			int overlap = celtMode.overlap;
			short[] eBands = celtMode.eBands;
			int num8 = this.start;
			int num9 = this.end;
			int tf_estimate = 0;
			if (nbCompressedBytes < 2 || pcm.IsEmpty)
			{
				return -1;
			}
			frame_size *= this.upsample;
			int num10 = 0;
			while (num10 <= celtMode.maxLM && celtMode.shortMdctSize << num10 != frame_size)
			{
				num10++;
			}
			if (num10 > celtMode.maxLM)
			{
				return -1;
			}
			int num11 = 1 << num10;
			int num12 = num11 * celtMode.shortMdctSize;
			int num13;
			int num14;
			if (enc == null)
			{
				num13 = 1;
				num14 = 0;
			}
			else
			{
				num13 = enc.tell();
				num14 = num13 + 4 >> 3;
			}
			nbCompressedBytes = Inlines.IMIN(nbCompressedBytes, 1275);
			int num15 = nbCompressedBytes - num14;
			int num17;
			int num18;
			if (this.vbr != 0 && this.bitrate != -1)
			{
				int num16 = celtMode.Fs >> 3;
				num17 = (this.bitrate * frame_size + (num16 >> 1)) / num16;
				num18 = num17 >> 6;
			}
			else
			{
				num17 = 0;
				int num19 = this.bitrate * frame_size;
				if (num13 > 1)
				{
					num19 += num13;
				}
				if (this.bitrate != -1)
				{
					nbCompressedBytes = Inlines.IMAX(2, Inlines.IMIN(nbCompressedBytes, (num19 + 4 * celtMode.Fs) / (8 * celtMode.Fs) - ((this.signalling != 0) ? 1 : 0)));
				}
				num18 = nbCompressedBytes;
			}
			if (this.bitrate != -1)
			{
				num7 = this.bitrate - (40 * num2 + 20) * ((400 >> num10) - 50);
			}
			if (enc == null)
			{
				enc = new EntropyCoder();
				enc.enc_init((uint)nbCompressedBytes);
			}
			if (num17 > 0 && this.constrained_vbr != 0)
			{
				int num20 = num17;
				int num21 = Inlines.IMIN(Inlines.IMAX((num13 == 1) ? 2 : 0, num17 + num20 - this.vbr_reservoir >> 6), num15);
				if (num21 < num15)
				{
					nbCompressedBytes = num14 + num21;
					num15 = num21;
					enc.enc_shrink(compressed.Slice(compressed_ptr), (uint)nbCompressedBytes);
				}
			}
			int num22 = nbCompressedBytes * 8;
			int num23 = num9;
			if (num23 > celtMode.effEBands)
			{
				num23 = celtMode.effEBands;
			}
			int[][] array = Arrays.InitTwoDimensionalArray<int>(num, num12 + overlap);
			int a = Inlines.MAX32(this.overlap_max, (int)Inlines.celt_maxabs32(pcm, pcm_ptr, num2 * (num12 - overlap) / this.upsample));
			this.overlap_max = (int)Inlines.celt_maxabs32(pcm, pcm_ptr + num2 * (num12 - overlap) / this.upsample, num2 * overlap / this.upsample);
			int num24 = (Inlines.MAX32(a, this.overlap_max) == 0) ? 1 : 0;
			if (num13 == 1)
			{
				enc.enc_bit_logp(compressed.Slice(compressed_ptr), num24, 15U);
			}
			else
			{
				num24 = 0;
			}
			if (num24 != 0)
			{
				if (num17 > 0)
				{
					nbCompressedBytes = (num18 = Inlines.IMIN(nbCompressedBytes, num14 + 2));
					num22 = nbCompressedBytes * 8;
					num15 = 2;
					enc.enc_shrink(compressed.Slice(compressed_ptr), (uint)nbCompressedBytes);
				}
				num13 = nbCompressedBytes * 8;
				enc.nbits_total += num13 - enc.tell();
			}
			int i = 0;
			do
			{
				int num25 = 0;
				CeltCommon.celt_preemphasis(pcm, pcm_ptr + i, array[i], overlap, num12, num, this.upsample, celtMode.preemph, ref this.preemph_memE[i], num25);
			}
			while (++i < num);
			int enabled = (((this.lfe != 0 && num15 > 3) || num15 > 12 * num2) && num8 == 0 && num24 == 0 && this.disable_pf == 0 && this.complexity >= 5 && (this.consec_transient == 0 || num10 == 3 || this.variable_duration != OpusFramesize.OPUS_FRAMESIZE_VARIABLE)) ? 1 : 0;
			int s = this.tapset_decision;
			int fl;
			int num26 = this.run_prefilter(array, this.prefilter_mem, num, num12, s, out num3, out num4, out fl, enabled, num15);
			if ((num4 > 13107 || this.prefilter_gain > 13107) && (this.analysis.valid == 0 || (double)this.analysis.tonality > 0.3) && ((double)num3 > 1.26 * (double)this.prefilter_period || (double)num3 < 0.79 * (double)this.prefilter_period))
			{
				pitch_change = 1;
			}
			if (num26 == 0)
			{
				if (num8 == 0 && num13 + 16 <= num22)
				{
					enc.enc_bit_logp(compressed.Slice(compressed_ptr), 0, 1U);
				}
			}
			else
			{
				enc.enc_bit_logp(compressed.Slice(compressed_ptr), 1, 1U);
				num3++;
				int num27 = Inlines.EC_ILOG((uint)num3) - 5;
				enc.enc_uint(compressed.Slice(compressed_ptr), (uint)num27, 6U);
				enc.enc_bits(compressed.Slice(compressed_ptr), (uint)(num3 - (16 << num27)), (uint)(4 + num27));
				num3--;
				enc.enc_bits(compressed.Slice(compressed_ptr), (uint)fl, 3U);
				enc.enc_icdf(compressed.Slice(compressed_ptr), s, Concentus.Celt.Tables.tapset_icdf, 2U);
			}
			int num28 = 0;
			int num29 = 0;
			if (this.complexity >= 1 && this.lfe == 0)
			{
				num28 = CeltCommon.transient_analysis(array, num12 + overlap, num, out tf_estimate, out tf_chan);
			}
			if (num10 > 0 && enc.tell() + 3 <= num22)
			{
				if (num28 != 0)
				{
					num29 = num11;
				}
			}
			else
			{
				num28 = 0;
				num5 = 1;
			}
			int[][] array2 = Arrays.InitTwoDimensionalArray<int>(num, num12);
			int[][] array3 = Arrays.InitTwoDimensionalArray<int>(num, nbEBands);
			int[][] array4 = Arrays.InitTwoDimensionalArray<int>(num, nbEBands);
			int num30 = (num29 != 0 && this.complexity >= 8) ? 1 : 0;
			int[][] array5 = Arrays.InitTwoDimensionalArray<int>(num, nbEBands);
			if (num30 != 0)
			{
				CeltCommon.compute_mdcts(celtMode, 0, array, array2, num2, num, num10, this.upsample);
				Bands.compute_band_energies(celtMode, array2, array3, num23, num2, num10);
				QuantizeBands.amp2Log2(celtMode, num23, num9, array3, array5, num2);
				for (int j = 0; j < nbEBands; j++)
				{
					array5[0][j] += Inlines.HALF16(Inlines.SHL16(num10, 10));
				}
				if (num2 == 2)
				{
					for (int j = 0; j < nbEBands; j++)
					{
						array5[1][j] += Inlines.HALF16(Inlines.SHL16(num10, 10));
					}
				}
			}
			CeltCommon.compute_mdcts(celtMode, num29, array, array2, num2, num, num10, this.upsample);
			if (num == 2 && num2 == 1)
			{
				tf_chan = 0;
			}
			Bands.compute_band_energies(celtMode, array2, array3, num23, num2, num10);
			if (this.lfe != 0)
			{
				for (int j = 2; j < num9; j++)
				{
					array3[0][j] = Inlines.IMIN(array3[0][j], Inlines.MULT16_32_Q15(3, array3[0][0]));
					array3[0][j] = Inlines.MAX32(array3[0][j], 1);
				}
			}
			QuantizeBands.amp2Log2(celtMode, num23, num9, array3, array4, num2);
			int[] array6 = new int[num2 * nbEBands];
			if (num8 == 0 && this.energy_mask != null && this.lfe == 0)
			{
				int num31 = 0;
				int num32 = 0;
				int num33 = 0;
				int num34 = Inlines.IMAX(2, this.lastCodedBands);
				for (i = 0; i < num2; i++)
				{
					for (int j = 0; j < num34; j++)
					{
						int num35 = Inlines.MAX16(Inlines.MIN16(this.energy_mask[nbEBands * i + j], 256), -2048);
						if (num35 > 0)
						{
							num35 = Inlines.HALF16(num35);
						}
						num31 += Inlines.MULT16_16(num35, (int)(eBands[j + 1] - eBands[j]));
						num33 += (int)(eBands[j + 1] - eBands[j]);
						num32 += Inlines.MULT16_16(num35, 1 + 2 * j - num34);
					}
				}
				num31 = Inlines.DIV32_16(num31, num33);
				num31 += 205;
				num32 = num32 * 6 / (num2 * (num34 - 1) * (num34 + 1) * num34);
				num32 = Inlines.HALF32(num32);
				num32 = Inlines.MAX32(Inlines.MIN32(num32, 32), -32);
				int num36 = 0;
				while (eBands[num36 + 1] < eBands[num34] / 2)
				{
					num36++;
				}
				int num37 = 0;
				for (int j = 0; j < num34; j++)
				{
					int num38 = num31 + num32 * (j - num36);
					int num39;
					if (num2 == 2)
					{
						num39 = Inlines.MAX16(this.energy_mask[j], this.energy_mask[nbEBands + j]);
					}
					else
					{
						num39 = this.energy_mask[j];
					}
					num39 = Inlines.MIN16(num39, 0);
					num39 -= num38;
					if (num39 > 256)
					{
						array6[j] = num39 - 256;
						num37++;
					}
				}
				if (num37 >= 3)
				{
					num31 += 256;
					if (num31 > 0)
					{
						num31 = 0;
						num32 = 0;
						Arrays.MemSetInt(array6, 0, num34);
					}
					else
					{
						for (int j = 0; j < num34; j++)
						{
							array6[j] = Inlines.MAX16(0, array6[j] - 256);
						}
					}
				}
				num31 += 205;
				surround_trim = 64 * num32;
				surround_masking = num31;
			}
			if (this.lfe == 0)
			{
				int num40 = -10240;
				int num41 = 0;
				int num42 = (num29 != 0) ? Inlines.HALF16(Inlines.SHL16(num10, 10)) : 0;
				for (int j = num8; j < num9; j++)
				{
					num40 = Inlines.MAX16(num40 - 1024, array4[0][j] - num42);
					if (num2 == 2)
					{
						num40 = Inlines.MAX16(num40, array4[1][j] - num42);
					}
					num41 += num40;
				}
				num41 /= num9 - num8;
				num6 = Inlines.SUB16(num41, this.spec_avg);
				num6 = Inlines.MIN16(3072, Inlines.MAX16(-1536, num6));
				this.spec_avg += (int)((short)Inlines.MULT16_16_Q15(655, num6));
			}
			if (num30 == 0)
			{
				Arrays.MemCopy(array4[0], 0, array5[0], 0, nbEBands);
				if (num2 == 2)
				{
					Arrays.MemCopy(array4[1], 0, array5[1], 0, nbEBands);
				}
			}
			if (num10 > 0 && enc.tell() + 3 <= num22 && num28 == 0 && this.complexity >= 5 && this.lfe == 0 && CeltCommon.patch_transient_decision(array4, this.oldBandE, nbEBands, num8, num9, num2) != 0)
			{
				num28 = 1;
				num29 = num11;
				CeltCommon.compute_mdcts(celtMode, num29, array, array2, num2, num, num10, this.upsample);
				Bands.compute_band_energies(celtMode, array2, array3, num23, num2, num10);
				QuantizeBands.amp2Log2(celtMode, num23, num9, array3, array4, num2);
				for (int j = 0; j < nbEBands; j++)
				{
					array5[0][j] += Inlines.HALF16(Inlines.SHL16(num10, 10));
				}
				if (num2 == 2)
				{
					for (int j = 0; j < nbEBands; j++)
					{
						array5[1][j] += Inlines.HALF16(Inlines.SHL16(num10, 10));
					}
				}
				tf_estimate = 3277;
			}
			if (num10 > 0 && enc.tell() + 3 <= num22)
			{
				enc.enc_bit_logp(compressed.Slice(compressed_ptr), num28, 3U);
			}
			int[][] array7 = Arrays.InitTwoDimensionalArray<int>(num2, num12);
			Bands.normalise_bands(celtMode, array2, array7, array3, num23, num2, num11);
			int[] array8 = new int[nbEBands];
			int tf_select;
			if (num18 >= 15 * num2 && num8 == 0 && this.complexity >= 2 && this.lfe == 0)
			{
				int num43;
				if (num18 < 40)
				{
					num43 = 12;
				}
				else if (num18 < 60)
				{
					num43 = 6;
				}
				else if (num18 < 100)
				{
					num43 = 4;
				}
				else
				{
					num43 = 3;
				}
				num43 *= 2;
				int num44;
				tf_select = CeltCommon.tf_analysis(celtMode, num23, num28, array8, num43, array7, num12, num10, out num44, tf_estimate, tf_chan);
				for (int j = num23; j < num9; j++)
				{
					array8[j] = array8[num23 - 1];
				}
			}
			else
			{
				int num44 = 0;
				for (int j = 0; j < num9; j++)
				{
					array8[j] = num28;
				}
				tf_select = 0;
			}
			int[][] error = Arrays.InitTwoDimensionalArray<int>(num2, nbEBands);
			QuantizeBands.quant_coarse_energy(celtMode, num8, num9, num23, array4, this.oldBandE, (uint)num22, error, enc, compressed.Slice(compressed_ptr), num2, num10, num15, this.force_intra, ref this.delayedIntra, (this.complexity >= 4) ? 1 : 0, this.loss_rate, this.lfe);
			CeltCommon.tf_encode(num8, num9, num28, array8, num10, tf_select, enc, compressed.Slice(compressed_ptr));
			if (enc.tell() + 4 <= num22)
			{
				if (this.lfe != 0)
				{
					this.tapset_decision = 0;
					this.spread_decision = 2;
				}
				else if (num29 != 0 || this.complexity < 3 || num15 < 10 * num2 || num8 != 0)
				{
					if (this.complexity == 0)
					{
						this.spread_decision = 0;
					}
					else
					{
						this.spread_decision = 2;
					}
				}
				else
				{
					this.spread_decision = Bands.spreading_decision(celtMode, array7, ref this.tonal_average, this.spread_decision, ref this.hf_average, ref this.tapset_decision, (num26 != 0 && num29 == 0) ? 1 : 0, num23, num2, num11);
				}
				enc.enc_icdf(compressed.Slice(compressed_ptr), this.spread_decision, Concentus.Celt.Tables.spread_icdf, 5U);
			}
			int[] array9 = new int[nbEBands];
			int tot_boost;
			int maxDepth = CeltCommon.dynalloc_analysis(array4, array5, nbEBands, num8, num9, num2, array9, this.lsb_depth, celtMode.logN, num28, this.vbr, this.constrained_vbr, eBands, num10, num18, out tot_boost, this.lfe, array6);
			if (this.lfe != 0)
			{
				array9[0] = Inlines.IMIN(8, num18 / 3);
			}
			int[] array10 = new int[nbEBands];
			CeltCommon.init_caps(celtMode, array10, num10, num2);
			int num45 = 6;
			num22 <<= 3;
			int num46 = 0;
			num13 = (int)enc.tell_frac();
			for (int j = num8; j < num9; j++)
			{
				int num47 = num2 * (int)(eBands[j + 1] - eBands[j]) << num10;
				int num48 = Inlines.IMIN(num47 << 3, Inlines.IMAX(48, num47));
				int num49 = num45;
				int num50 = 0;
				int num51 = 0;
				while (num13 + (num49 << 3) < num22 - num46 && num50 < array10[j])
				{
					int num52 = (num51 < array9[j]) ? 1 : 0;
					enc.enc_bit_logp(compressed.Slice(compressed_ptr), num52, (uint)num49);
					num13 = (int)enc.tell_frac();
					if (num52 == 0)
					{
						break;
					}
					num50 += num48;
					num46 += num48;
					num49 = 1;
					num51++;
				}
				if (num51 != 0)
				{
					num45 = Inlines.IMAX(2, num45 - 1);
				}
				array9[j] = num50;
			}
			if (num2 == 2)
			{
				if (num10 != 0)
				{
					dual_stereo = CeltCommon.stereo_analysis(celtMode, array7, num10);
				}
				this.intensity = Bands.hysteresis_decision(num7 / 1000, Concentus.Celt.Tables.intensity_thresholds, Concentus.Celt.Tables.intensity_histeresis, 21, this.intensity);
				this.intensity = Inlines.IMIN(num9, Inlines.IMAX(num8, this.intensity));
			}
			int num53 = 5;
			if (num13 + 48 <= num22 - num46)
			{
				if (this.lfe != 0)
				{
					num53 = 5;
				}
				else
				{
					num53 = CeltCommon.alloc_trim_analysis(celtMode, array7, array4, num9, num10, num2, this.analysis, ref this.stereo_saving, tf_estimate, this.intensity, surround_trim);
				}
				enc.enc_icdf(compressed.Slice(compressed_ptr), num53, Concentus.Celt.Tables.trim_icdf, 7U);
				num13 = (int)enc.tell_frac();
			}
			if (num17 > 0)
			{
				int num54 = celtMode.maxLM - num10;
				nbCompressedBytes = Inlines.IMIN(nbCompressedBytes, 1275 >> 3 - num10);
				int num55 = num17 - (40 * num2 + 20 << 3);
				if (this.constrained_vbr != 0)
				{
					num55 += this.vbr_offset >> num54;
				}
				int num56 = CeltCommon.compute_vbr(celtMode, this.analysis, num55, num10, num7, this.lastCodedBands, num2, this.intensity, this.constrained_vbr, this.stereo_saving, tot_boost, tf_estimate, pitch_change, maxDepth, this.variable_duration, this.lfe, (this.energy_mask != null) ? 1 : 0, surround_masking, num6);
				num56 += num13;
				int a2 = (num13 + num46 + 64 - 1 >> 6) + 2 - num14;
				num15 = num56 + 32 >> 6;
				num15 = Inlines.IMAX(a2, num15);
				num15 = Inlines.IMIN(nbCompressedBytes, num15 + num14) - num14;
				int num57 = num56 - num17;
				num56 = num15 << 6;
				if (num24 != 0)
				{
					num15 = 2;
					num56 = 128;
					num57 = 0;
				}
				int a3;
				if (this.vbr_count < 970)
				{
					this.vbr_count++;
					a3 = Inlines.celt_rcp(Inlines.SHL32(this.vbr_count + 20, 16));
				}
				else
				{
					a3 = 33;
				}
				if (this.constrained_vbr != 0)
				{
					this.vbr_reservoir += num56 - num17;
				}
				if (this.constrained_vbr != 0)
				{
					this.vbr_drift += Inlines.MULT16_32_Q15(a3, num57 * (1 << num54) - this.vbr_offset - this.vbr_drift);
					this.vbr_offset = -this.vbr_drift;
				}
				if (this.constrained_vbr != 0 && this.vbr_reservoir < 0)
				{
					int num58 = -this.vbr_reservoir / 64;
					num15 += ((num24 != 0) ? 0 : num58);
					this.vbr_reservoir = 0;
				}
				nbCompressedBytes = Inlines.IMIN(nbCompressedBytes, num15 + num14);
				enc.enc_shrink(compressed.Slice(compressed_ptr), (uint)nbCompressedBytes);
			}
			int[] array11 = new int[nbEBands];
			int[] pulses = new int[nbEBands];
			int[] fine_priority = new int[nbEBands];
			int num59 = (nbCompressedBytes * 8 << 3) - (int)enc.tell_frac() - 1;
			int num60 = (num28 != 0 && num10 >= 2 && num59 >= num10 + 2 << 3) ? 8 : 0;
			num59 -= num60;
			int signalBandwidth = num9 - 1;
			if (this.analysis.valid != 0)
			{
				int b;
				if (num7 < 32000 * num2)
				{
					b = 13;
				}
				else if (num7 < 48000 * num2)
				{
					b = 16;
				}
				else if (num7 < 60000 * num2)
				{
					b = 18;
				}
				else if (num7 < 80000 * num2)
				{
					b = 19;
				}
				else
				{
					b = 20;
				}
				signalBandwidth = Inlines.IMAX(this.analysis.bandwidth, b);
			}
			if (this.lfe != 0)
			{
				signalBandwidth = 1;
			}
			int balance;
			int num61 = Rate.compute_allocation_encode(celtMode, num8, num9, array9, array10, num53, ref this.intensity, ref dual_stereo, num59, out balance, pulses, array11, fine_priority, num2, num10, enc, compressed.Slice(compressed_ptr), this.lastCodedBands, signalBandwidth);
			if (this.lastCodedBands != 0)
			{
				this.lastCodedBands = Inlines.IMIN(this.lastCodedBands + 1, Inlines.IMAX(this.lastCodedBands - 1, num61));
			}
			else
			{
				this.lastCodedBands = num61;
			}
			QuantizeBands.quant_fine_energy(celtMode, num8, num9, this.oldBandE, error, array11, enc, compressed.Slice(compressed_ptr), num2);
			byte[] collapse_masks = new byte[num2 * nbEBands];
			Bands.quant_all_bands_encode(1, celtMode, num8, num9, array7[0], (num2 == 2) ? array7[1] : null, collapse_masks, array3, pulses, num29, this.spread_decision, dual_stereo, this.intensity, array8, nbCompressedBytes * 64 - num60, balance, enc, compressed.Slice(compressed_ptr), num10, num61, ref this.rng);
			if (num60 > 0)
			{
				int fl2 = (this.consec_transient < 2) ? 1 : 0;
				enc.enc_bits(compressed.Slice(compressed_ptr), (uint)fl2, 1U);
			}
			QuantizeBands.quant_energy_finalise(celtMode, num8, num9, this.oldBandE, error, array11, fine_priority, nbCompressedBytes * 8 - enc.tell(), enc, compressed.Slice(compressed_ptr), num2);
			if (num24 != 0)
			{
				for (int j = 0; j < nbEBands; j++)
				{
					this.oldBandE[0][j] = -28672;
				}
				if (num2 == 2)
				{
					for (int j = 0; j < nbEBands; j++)
					{
						this.oldBandE[1][j] = -28672;
					}
				}
			}
			this.prefilter_period = num3;
			this.prefilter_gain = num4;
			this.prefilter_tapset = s;
			if (num == 2 && num2 == 1)
			{
				Arrays.MemCopy(this.oldBandE[0], 0, this.oldBandE[1], 0, nbEBands);
			}
			if (num28 == 0)
			{
				Arrays.MemCopy(this.oldLogE[0], 0, this.oldLogE2[0], 0, nbEBands);
				Arrays.MemCopy(this.oldBandE[0], 0, this.oldLogE[0], 0, nbEBands);
				if (num == 2)
				{
					Arrays.MemCopy(this.oldLogE[1], 0, this.oldLogE2[1], 0, nbEBands);
					Arrays.MemCopy(this.oldBandE[1], 0, this.oldLogE[1], 0, nbEBands);
				}
			}
			else
			{
				for (int j = 0; j < nbEBands; j++)
				{
					this.oldLogE[0][j] = Inlines.MIN16(this.oldLogE[0][j], this.oldBandE[0][j]);
				}
				if (num == 2)
				{
					for (int j = 0; j < nbEBands; j++)
					{
						this.oldLogE[1][j] = Inlines.MIN16(this.oldLogE[1][j], this.oldBandE[1][j]);
					}
				}
			}
			i = 0;
			do
			{
				for (int j = 0; j < num8; j++)
				{
					this.oldBandE[i][j] = 0;
					this.oldLogE[i][j] = (this.oldLogE2[i][j] = -28672);
				}
				for (int j = num9; j < nbEBands; j++)
				{
					this.oldBandE[i][j] = 0;
					this.oldLogE[i][j] = (this.oldLogE2[i][j] = -28672);
				}
			}
			while (++i < num);
			if (num28 != 0 || num5 != 0)
			{
				this.consec_transient++;
			}
			else
			{
				this.consec_transient = 0;
			}
			this.rng = enc.rng;
			enc.enc_done(compressed.Slice(compressed_ptr));
			if (enc.get_error() != 0)
			{
				return -3;
			}
			return nbCompressedBytes;
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x00035F62 File Offset: 0x00034162
		internal void SetComplexity(int value)
		{
			if (value < 0 || value > 10)
			{
				throw new ArgumentException("Complexity must be between 0 and 10 inclusive");
			}
			this.complexity = value;
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x00035F7F File Offset: 0x0003417F
		internal void SetStartBand(int value)
		{
			if (value < 0 || value >= this.mode.nbEBands)
			{
				throw new ArgumentException("Start band above max number of ebands (or negative)");
			}
			this.start = value;
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x00035FA5 File Offset: 0x000341A5
		internal void SetEndBand(int value)
		{
			if (value < 1 || value > this.mode.nbEBands)
			{
				throw new ArgumentException("End band above max number of ebands (or less than 1)");
			}
			this.end = value;
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x00035FCB File Offset: 0x000341CB
		internal void SetPacketLossPercent(int value)
		{
			if (value < 0 || value > 100)
			{
				throw new ArgumentException("Packet loss must be between 0 and 100");
			}
			this.loss_rate = value;
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x00035FE8 File Offset: 0x000341E8
		internal void SetPrediction(int value)
		{
			if (value < 0 || value > 2)
			{
				throw new ArgumentException("CELT prediction mode must be 0, 1, or 2");
			}
			this.disable_pf = ((value <= 1) ? 1 : 0);
			this.force_intra = ((value == 0) ? 1 : 0);
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00036014 File Offset: 0x00034214
		internal void SetVBRConstraint(bool value)
		{
			this.constrained_vbr = ((value > false) ? 1 : 0);
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00036020 File Offset: 0x00034220
		internal void SetVBR(bool value)
		{
			this.vbr = ((value > false) ? 1 : 0);
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0003602C File Offset: 0x0003422C
		internal void SetBitrate(int value)
		{
			if (value <= 500 && value != -1)
			{
				throw new ArgumentException("Bitrate out of range");
			}
			value = Inlines.IMIN(value, 260000 * this.channels);
			this.bitrate = value;
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x00036060 File Offset: 0x00034260
		internal void SetChannels(int value)
		{
			if (value < 1 || value > 2)
			{
				throw new ArgumentException("Channel count must be 1 or 2");
			}
			this.stream_channels = value;
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0003607C File Offset: 0x0003427C
		internal void SetLSBDepth(int value)
		{
			if (value < 8 || value > 24)
			{
				throw new ArgumentException("Bit depth must be between 8 and 24");
			}
			this.lsb_depth = value;
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x00036099 File Offset: 0x00034299
		internal int GetLSBDepth()
		{
			return this.lsb_depth;
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x000360A1 File Offset: 0x000342A1
		internal void SetExpertFrameDuration(OpusFramesize value)
		{
			this.variable_duration = value;
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x000360AA File Offset: 0x000342AA
		internal void SetSignalling(int value)
		{
			this.signalling = value;
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x000360B3 File Offset: 0x000342B3
		internal void SetAnalysis(AnalysisInfo value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("AnalysisInfo");
			}
			this.analysis.Assign(value);
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x000360CF File Offset: 0x000342CF
		internal CeltMode GetMode()
		{
			return this.mode;
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x000360D7 File Offset: 0x000342D7
		internal uint GetFinalRange()
		{
			return this.rng;
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x000360DF File Offset: 0x000342DF
		internal void SetLFE(int value)
		{
			this.lfe = value;
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x000360E8 File Offset: 0x000342E8
		internal void SetEnergyMask(int[] value)
		{
			this.energy_mask = value;
		}

		// Token: 0x04000498 RID: 1176
		internal CeltMode mode;

		// Token: 0x04000499 RID: 1177
		internal int channels;

		// Token: 0x0400049A RID: 1178
		internal int stream_channels;

		// Token: 0x0400049B RID: 1179
		internal int force_intra;

		// Token: 0x0400049C RID: 1180
		internal int clip;

		// Token: 0x0400049D RID: 1181
		internal int disable_pf;

		// Token: 0x0400049E RID: 1182
		internal int complexity;

		// Token: 0x0400049F RID: 1183
		internal int upsample;

		// Token: 0x040004A0 RID: 1184
		internal int start;

		// Token: 0x040004A1 RID: 1185
		internal int end;

		// Token: 0x040004A2 RID: 1186
		internal int bitrate;

		// Token: 0x040004A3 RID: 1187
		internal int vbr;

		// Token: 0x040004A4 RID: 1188
		internal int signalling;

		// Token: 0x040004A5 RID: 1189
		internal int constrained_vbr;

		// Token: 0x040004A6 RID: 1190
		internal int loss_rate;

		// Token: 0x040004A7 RID: 1191
		internal int lsb_depth;

		// Token: 0x040004A8 RID: 1192
		internal OpusFramesize variable_duration;

		// Token: 0x040004A9 RID: 1193
		internal int lfe;

		// Token: 0x040004AA RID: 1194
		internal uint rng;

		// Token: 0x040004AB RID: 1195
		internal int spread_decision;

		// Token: 0x040004AC RID: 1196
		internal int delayedIntra;

		// Token: 0x040004AD RID: 1197
		internal int tonal_average;

		// Token: 0x040004AE RID: 1198
		internal int lastCodedBands;

		// Token: 0x040004AF RID: 1199
		internal int hf_average;

		// Token: 0x040004B0 RID: 1200
		internal int tapset_decision;

		// Token: 0x040004B1 RID: 1201
		internal int prefilter_period;

		// Token: 0x040004B2 RID: 1202
		internal int prefilter_gain;

		// Token: 0x040004B3 RID: 1203
		internal int prefilter_tapset;

		// Token: 0x040004B4 RID: 1204
		internal int consec_transient;

		// Token: 0x040004B5 RID: 1205
		internal AnalysisInfo analysis = new AnalysisInfo();

		// Token: 0x040004B6 RID: 1206
		internal readonly int[] preemph_memE = new int[2];

		// Token: 0x040004B7 RID: 1207
		internal readonly int[] preemph_memD = new int[2];

		// Token: 0x040004B8 RID: 1208
		internal int vbr_reservoir;

		// Token: 0x040004B9 RID: 1209
		internal int vbr_drift;

		// Token: 0x040004BA RID: 1210
		internal int vbr_offset;

		// Token: 0x040004BB RID: 1211
		internal int vbr_count;

		// Token: 0x040004BC RID: 1212
		internal int overlap_max;

		// Token: 0x040004BD RID: 1213
		internal int stereo_saving;

		// Token: 0x040004BE RID: 1214
		internal int intensity;

		// Token: 0x040004BF RID: 1215
		internal int[] energy_mask;

		// Token: 0x040004C0 RID: 1216
		internal int spec_avg;

		// Token: 0x040004C1 RID: 1217
		internal int[][] in_mem;

		// Token: 0x040004C2 RID: 1218
		internal int[][] prefilter_mem;

		// Token: 0x040004C3 RID: 1219
		internal int[][] oldBandE;

		// Token: 0x040004C4 RID: 1220
		internal int[][] oldLogE;

		// Token: 0x040004C5 RID: 1221
		internal int[][] oldLogE2;
	}
}
