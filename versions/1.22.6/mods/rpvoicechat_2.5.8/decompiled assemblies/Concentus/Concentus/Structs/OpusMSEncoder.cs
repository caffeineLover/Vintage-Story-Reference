using System;
using System.Runtime.CompilerServices;
using Concentus.Celt;
using Concentus.Celt.Structs;
using Concentus.Common;
using Concentus.Common.CPlusPlus;
using Concentus.Enums;

namespace Concentus.Structs
{
	// Token: 0x0200005D RID: 93
	public class OpusMSEncoder : IOpusMultiStreamEncoder, IDisposable
	{
		// Token: 0x060001E5 RID: 485 RVA: 0x0001D2A4 File Offset: 0x0001B4A4
		private OpusMSEncoder(int nb_streams, int nb_coupled_streams)
		{
			if (nb_streams < 1 || nb_coupled_streams > nb_streams || nb_coupled_streams < 0)
			{
				throw new ArgumentException("Invalid channel count in MS encoder");
			}
			this.encoders = new OpusEncoder[nb_streams];
			for (int i = 0; i < nb_streams; i++)
			{
				this.encoders[i] = new OpusEncoder();
			}
			int num = nb_coupled_streams * 2 + (nb_streams - nb_coupled_streams);
			this.window_mem = new int[num * 120];
			this.preemph_mem = new int[num];
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0001D338 File Offset: 0x0001B538
		public void ResetState()
		{
			this.subframe_mem[0] = (this.subframe_mem[1] = (this.subframe_mem[2] = 0f));
			if (this.surround != 0)
			{
				Arrays.MemSetInt(this.preemph_mem, 0, this.layout.nb_channels);
				Arrays.MemSetInt(this.window_mem, 0, this.layout.nb_channels * 120);
			}
			int num = 0;
			for (int i = 0; i < this.layout.nb_streams; i++)
			{
				this.encoders[num++].ResetState();
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0001D3CC File Offset: 0x0001B5CC
		internal static int validate_encoder_layout(ChannelLayout layout)
		{
			for (int i = 0; i < layout.nb_streams; i++)
			{
				if (i < layout.nb_coupled_streams)
				{
					if (OpusMultistream.get_left_channel(layout, i, -1) == -1)
					{
						return 0;
					}
					if (OpusMultistream.get_right_channel(layout, i, -1) == -1)
					{
						return 0;
					}
				}
				else if (OpusMultistream.get_mono_channel(layout, i, -1) == -1)
				{
					return 0;
				}
			}
			return 1;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0001D41C File Offset: 0x0001B61C
		internal static void channel_pos(int channels, int[] pos)
		{
			if (channels == 4)
			{
				pos[0] = 1;
				pos[1] = 3;
				pos[2] = 1;
				pos[3] = 3;
				return;
			}
			if (channels == 3 || channels == 5 || channels == 6)
			{
				pos[0] = 1;
				pos[1] = 2;
				pos[2] = 3;
				pos[3] = 1;
				pos[4] = 3;
				pos[5] = 0;
				return;
			}
			if (channels == 7)
			{
				pos[0] = 1;
				pos[1] = 2;
				pos[2] = 3;
				pos[3] = 1;
				pos[4] = 3;
				pos[5] = 2;
				pos[6] = 0;
				return;
			}
			if (channels == 8)
			{
				pos[0] = 1;
				pos[1] = 2;
				pos[2] = 3;
				pos[3] = 1;
				pos[4] = 3;
				pos[5] = 1;
				pos[6] = 3;
				pos[7] = 0;
			}
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0001D4A8 File Offset: 0x0001B6A8
		internal static int logSum(int a, int b)
		{
			int num;
			int num2;
			if (a > b)
			{
				num = a;
				num2 = Inlines.SUB32(Inlines.EXTEND32(a), Inlines.EXTEND32(b));
			}
			else
			{
				num = b;
				num2 = Inlines.SUB32(Inlines.EXTEND32(b), Inlines.EXTEND32(a));
			}
			if (num2 >= 8192)
			{
				return num;
			}
			int num3 = Inlines.SHR32(num2, 9);
			int a2 = Inlines.SHL16(num2 - Inlines.SHL16(num3, 9), 6);
			return num + OpusMSEncoder.diff_table[num3] + Inlines.MULT16_16_Q15(a2, Inlines.SUB16(OpusMSEncoder.diff_table[num3 + 1], OpusMSEncoder.diff_table[num3]));
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0001D52C File Offset: 0x0001B72C
		internal static void surround_analysis<T>(CeltMode celt_mode, ReadOnlySpan<T> pcm, int[] bandLogE, int[] mem, int[] preemph_mem, int len, int overlap, int channels, int rate, OpusMSEncoder.opus_copy_channel_in_func<T> copy_channel_in)
		{
			int[] array = new int[8];
			int[][] array2 = Arrays.InitTwoDimensionalArray<int>(1, 21);
			int[][] array3 = Arrays.InitTwoDimensionalArray<int>(3, 21);
			int num = CeltCommon.resampling_factor(rate);
			int num2 = len * num;
			int num3 = 0;
			while (num3 < celt_mode.maxLM && celt_mode.shortMdctSize << num3 != num2)
			{
				num3++;
			}
			int[] array4 = new int[num2 + overlap];
			short[] array5 = new short[len];
			int[][] array6 = Arrays.InitTwoDimensionalArray<int>(1, num2);
			OpusMSEncoder.channel_pos(channels, array);
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 21; j++)
				{
					array3[i][j] = -28672;
				}
			}
			for (int i = 0; i < channels; i++)
			{
				Arrays.MemCopy(mem, i * overlap, array4, 0, overlap);
				copy_channel_in.Invoke(array5, 0, 1, pcm, channels, i, len);
				BoxedValueInt boxedValueInt = new BoxedValueInt(preemph_mem[i]);
				CeltCommon.celt_preemphasis(array5, array4, overlap, num2, 1, num, celt_mode.preemph, boxedValueInt, 0);
				preemph_mem[i] = boxedValueInt.Val;
				MDCT.clt_mdct_forward(celt_mode.mdct, array4, 0, array6[0], 0, celt_mode.window, overlap, celt_mode.maxLM - num3, 1);
				if (num != 1)
				{
					int j;
					for (j = 0; j < len; j++)
					{
						array6[0][j] *= num;
					}
					while (j < num2)
					{
						array6[0][j] = 0;
						j++;
					}
				}
				Bands.compute_band_energies(celt_mode, array6, array2, 21, 1, num3);
				QuantizeBands.amp2Log2(celt_mode, 21, 21, array2[0], bandLogE, 21 * i, 1);
				for (int j = 1; j < 21; j++)
				{
					bandLogE[21 * i + j] = Inlines.MAX16(bandLogE[21 * i + j], bandLogE[21 * i + j - 1] - 1024);
				}
				for (int j = 19; j >= 0; j--)
				{
					bandLogE[21 * i + j] = Inlines.MAX16(bandLogE[21 * i + j], bandLogE[21 * i + j + 1] - 2048);
				}
				if (array[i] == 1)
				{
					for (int j = 0; j < 21; j++)
					{
						array3[0][j] = OpusMSEncoder.logSum(array3[0][j], bandLogE[21 * i + j]);
					}
				}
				else if (array[i] == 3)
				{
					for (int j = 0; j < 21; j++)
					{
						array3[2][j] = OpusMSEncoder.logSum(array3[2][j], bandLogE[21 * i + j]);
					}
				}
				else if (array[i] == 2)
				{
					for (int j = 0; j < 21; j++)
					{
						array3[0][j] = OpusMSEncoder.logSum(array3[0][j], bandLogE[21 * i + j] - 512);
						array3[2][j] = OpusMSEncoder.logSum(array3[2][j], bandLogE[21 * i + j] - 512);
					}
				}
				Arrays.MemCopy(array4, num2, mem, i * overlap, overlap);
			}
			for (int j = 0; j < 21; j++)
			{
				array3[1][j] = Inlines.MIN32(array3[0][j], array3[2][j]);
			}
			int num4 = Inlines.HALF16(Inlines.celt_log2(32768 / (channels - 1)));
			for (int i = 0; i < 3; i++)
			{
				for (int j = 0; j < 21; j++)
				{
					array3[i][j] += num4;
				}
			}
			for (int i = 0; i < channels; i++)
			{
				if (array[i] != 0)
				{
					int[] array7 = array3[array[i] - 1];
					for (int j = 0; j < 21; j++)
					{
						bandLogE[21 * i + j] = bandLogE[21 * i + j] - array7[j];
					}
				}
				else
				{
					for (int j = 0; j < 21; j++)
					{
						bandLogE[21 * i + j] = 0;
					}
				}
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0001D8A4 File Offset: 0x0001BAA4
		internal int opus_multistream_encoder_init(int Fs, int channels, int streams, int coupled_streams, byte[] mapping, OpusApplication application, int surround)
		{
			if (channels > 255 || channels < 1 || coupled_streams > streams || streams < 1 || coupled_streams < 0 || streams > 255 - coupled_streams)
			{
				return -1;
			}
			this.layout.nb_channels = channels;
			this.layout.nb_streams = streams;
			this.layout.nb_coupled_streams = coupled_streams;
			this.subframe_mem[0] = (this.subframe_mem[1] = (this.subframe_mem[2] = 0f));
			if (surround == 0)
			{
				this.lfe_stream = -1;
			}
			this.bitrate_bps = -1000;
			this.application = application;
			this.variable_duration = OpusFramesize.OPUS_FRAMESIZE_ARG;
			int i;
			for (i = 0; i < this.layout.nb_channels; i++)
			{
				this.layout.mapping[i] = mapping[i];
			}
			if (OpusMultistream.validate_layout(this.layout) == 0 || OpusMSEncoder.validate_encoder_layout(this.layout) == 0)
			{
				return -1;
			}
			int num = 0;
			for (i = 0; i < this.layout.nb_coupled_streams; i++)
			{
				int num2 = this.encoders[num].opus_init_encoder(Fs, 2, application);
				if (num2 != 0)
				{
					return num2;
				}
				if (i == this.lfe_stream)
				{
					this.encoders[num].IsLFE = true;
				}
				num++;
			}
			while (i < this.layout.nb_streams)
			{
				int num2 = this.encoders[num].opus_init_encoder(Fs, 1, application);
				if (i == this.lfe_stream)
				{
					this.encoders[num].IsLFE = true;
				}
				if (num2 != 0)
				{
					return num2;
				}
				num++;
				i++;
			}
			if (surround != 0)
			{
				Arrays.MemSetInt(this.preemph_mem, 0, channels);
				Arrays.MemSetInt(this.window_mem, 0, channels * 120);
			}
			this.surround = surround;
			return 0;
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0001DA48 File Offset: 0x0001BC48
		internal int opus_multistream_surround_encoder_init(int Fs, int channels, int mapping_family, out int streams, out int coupled_streams, byte[] mapping, OpusApplication application)
		{
			streams = 0;
			coupled_streams = 0;
			if (channels > 255 || channels < 1)
			{
				return -1;
			}
			this.lfe_stream = -1;
			if (mapping_family == 0)
			{
				if (channels == 1)
				{
					streams = 1;
					coupled_streams = 0;
					mapping[0] = 0;
				}
				else
				{
					if (channels != 2)
					{
						return -5;
					}
					streams = 1;
					coupled_streams = 1;
					mapping[0] = 0;
					mapping[1] = 1;
				}
			}
			else if (mapping_family == 1 && channels <= 8 && channels >= 1)
			{
				streams = VorbisLayout.vorbis_mappings[channels - 1].nb_streams;
				coupled_streams = VorbisLayout.vorbis_mappings[channels - 1].nb_coupled_streams;
				for (int i = 0; i < channels; i++)
				{
					mapping[i] = VorbisLayout.vorbis_mappings[channels - 1].mapping[i];
				}
				if (channels >= 6)
				{
					this.lfe_stream = streams - 1;
				}
			}
			else
			{
				if (mapping_family != 255)
				{
					return -5;
				}
				streams = channels;
				coupled_streams = 0;
				byte b = 0;
				while ((int)b < channels)
				{
					mapping[(int)b] = b;
					b += 1;
				}
			}
			return this.opus_multistream_encoder_init(Fs, channels, streams, coupled_streams, mapping, application, (channels > 2 && mapping_family == 1) ? 1 : 0);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0001DB50 File Offset: 0x0001BD50
		[Obsolete("Use OpusCodecFactory methods which can give you native code if supported by your platform")]
		public static OpusMSEncoder Create(int Fs, int channels, int streams, int coupled_streams, byte[] mapping, OpusApplication application)
		{
			if (channels > 255 || channels < 1 || coupled_streams > streams || streams < 1 || coupled_streams < 0 || streams > 255 - coupled_streams)
			{
				throw new ArgumentException("Invalid channel / stream configuration");
			}
			OpusMSEncoder opusMSEncoder = new OpusMSEncoder(streams, coupled_streams);
			int num = opusMSEncoder.opus_multistream_encoder_init(Fs, channels, streams, coupled_streams, mapping, application, 0);
			if (num == 0)
			{
				return opusMSEncoder;
			}
			if (num == -1)
			{
				throw new ArgumentException("OPUS_BAD_ARG when creating MS encoder");
			}
			throw new OpusException("Could not create MS encoder: " + CodecHelpers.opus_strerror(num), num);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0001DBCC File Offset: 0x0001BDCC
		internal static void GetStreamCount(int channels, int mapping_family, BoxedValueInt nb_streams, BoxedValueInt nb_coupled_streams)
		{
			if (mapping_family == 0)
			{
				if (channels == 1)
				{
					nb_streams.Val = 1;
					nb_coupled_streams.Val = 0;
					return;
				}
				if (channels == 2)
				{
					nb_streams.Val = 1;
					nb_coupled_streams.Val = 1;
					return;
				}
				throw new ArgumentException("More than 2 channels requires custom mappings");
			}
			else
			{
				if (mapping_family == 1 && channels <= 8 && channels >= 1)
				{
					nb_streams.Val = VorbisLayout.vorbis_mappings[channels - 1].nb_streams;
					nb_coupled_streams.Val = VorbisLayout.vorbis_mappings[channels - 1].nb_coupled_streams;
					return;
				}
				if (mapping_family == 255)
				{
					nb_streams.Val = channels;
					nb_coupled_streams.Val = 0;
					return;
				}
				throw new ArgumentException("Invalid mapping family");
			}
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0001DC64 File Offset: 0x0001BE64
		[Obsolete("Use OpusCodecFactory methods which can give you native code if supported by your platform")]
		public static OpusMSEncoder CreateSurround(int Fs, int channels, int mapping_family, out int streams, out int coupled_streams, byte[] mapping, OpusApplication application)
		{
			if (channels > 255 || channels < 1 || application == OpusApplication.OPUS_APPLICATION_UNIMPLEMENTED)
			{
				throw new ArgumentException("Invalid channel count or application");
			}
			BoxedValueInt boxedValueInt = new BoxedValueInt(0);
			BoxedValueInt boxedValueInt2 = new BoxedValueInt(0);
			OpusMSEncoder.GetStreamCount(channels, mapping_family, boxedValueInt, boxedValueInt2);
			OpusMSEncoder opusMSEncoder = new OpusMSEncoder(boxedValueInt.Val, boxedValueInt2.Val);
			int num = opusMSEncoder.opus_multistream_surround_encoder_init(Fs, channels, mapping_family, out streams, out coupled_streams, mapping, application);
			if (num == 0)
			{
				return opusMSEncoder;
			}
			if (num == -1)
			{
				throw new ArgumentException("Bad argument passed to CreateSurround");
			}
			throw new OpusException("Could not create multistream encoder: " + CodecHelpers.opus_strerror(num), num);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0001DCF0 File Offset: 0x0001BEF0
		internal int surround_rate_allocation(int[] out_rates, int frame_size)
		{
			int num = 0;
			int sampleRate = this.encoders[0].SampleRate;
			int num2;
			if (this.bitrate_bps > this.layout.nb_channels * 40000)
			{
				num2 = 20000;
			}
			else
			{
				num2 = this.bitrate_bps / this.layout.nb_channels / 2;
			}
			num2 += 60 * (sampleRate / frame_size - 50);
			int num3 = 3500 + 60 * (sampleRate / frame_size - 50);
			int num4 = 512;
			int num5 = 32;
			int num6;
			if (this.bitrate_bps == -1000)
			{
				num6 = sampleRate + 60 * sampleRate / frame_size;
			}
			else if (this.bitrate_bps == -1)
			{
				num6 = 300000;
			}
			else
			{
				int num7 = (this.lfe_stream != -1) ? 1 : 0;
				int nb_coupled_streams = this.layout.nb_coupled_streams;
				int num8 = this.layout.nb_streams - nb_coupled_streams - num7;
				int num9 = (num8 << 8) + num4 * nb_coupled_streams + num7 * num5;
				num6 = 256 * (this.bitrate_bps - num3 * num7 - num2 * (nb_coupled_streams + num8)) / num9;
			}
			for (int i = 0; i < this.layout.nb_streams; i++)
			{
				if (i < this.layout.nb_coupled_streams)
				{
					out_rates[i] = num2 + (num6 * num4 >> 8);
				}
				else if (i != this.lfe_stream)
				{
					out_rates[i] = num2 + num6;
				}
				else
				{
					out_rates[i] = num3 + (num6 * num5 >> 8);
				}
				out_rates[i] = Inlines.IMAX(out_rates[i], 500);
				num += out_rates[i];
			}
			return num;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0001DE5C File Offset: 0x0001C05C
		internal int opus_multistream_encode_native<T>(OpusMSEncoder.opus_copy_channel_in_func<T> copy_channel_in, ReadOnlySpan<T> pcm, int analysis_frame_size, Span<byte> data, int max_data_bytes, int lsb_depth, Downmix.downmix_func<T> downmix, int float_api)
		{
			byte[] array = new byte[3832];
			OpusRepacketizer opusRepacketizer = new OpusRepacketizer();
			int[] array2 = new int[256];
			int[] array3 = new int[42];
			int[] mem = null;
			int[] array4 = null;
			int num = 0;
			if (this.surround != 0)
			{
				array4 = this.preemph_mem;
				mem = this.window_mem;
			}
			int num2 = 0;
			int sampleRate = this.encoders[num2].SampleRate;
			int num3 = (this.encoders[num2].UseVBR > false) ? 1 : 0;
			CeltMode celtMode = this.encoders[num2].GetCeltMode();
			int c = this.layout.nb_streams + this.layout.nb_coupled_streams;
			int num4 = this.encoders[num2].Lookahead;
			num4 -= sampleRate / 400;
			int num5 = CodecHelpers.compute_frame_size<T>(pcm, analysis_frame_size, this.variable_duration, c, sampleRate, this.bitrate_bps, num4, downmix, this.subframe_mem, this.encoders[num2].analysis.enabled);
			if (400 * num5 < sampleRate)
			{
				return -1;
			}
			if (400 * num5 != sampleRate && 200 * num5 != sampleRate && 100 * num5 != sampleRate && 50 * num5 != sampleRate && 25 * num5 != sampleRate && 50 * num5 != 3 * sampleRate)
			{
				return -1;
			}
			int num6 = this.layout.nb_streams * 2 - 1;
			if (max_data_bytes < num6)
			{
				return -2;
			}
			short[] array5 = new short[2 * num5];
			int[] array6 = new int[21 * this.layout.nb_channels];
			if (this.surround != 0)
			{
				OpusMSEncoder.surround_analysis<T>(celtMode, pcm, array6, mem, array4, num5, 120, this.layout.nb_channels, sampleRate, copy_channel_in);
			}
			int num7 = this.surround_rate_allocation(array2, num5);
			if (num3 == 0)
			{
				if (this.bitrate_bps == -1000)
				{
					max_data_bytes = Inlines.IMIN(max_data_bytes, 3 * num7 / (24 * sampleRate / num5));
				}
				else if (this.bitrate_bps != -1)
				{
					max_data_bytes = Inlines.IMIN(max_data_bytes, Inlines.IMAX(num6, 3 * this.bitrate_bps / (24 * sampleRate / num5)));
				}
			}
			for (int i = 0; i < this.layout.nb_streams; i++)
			{
				OpusEncoder opusEncoder = this.encoders[num2];
				num2++;
				opusEncoder.Bitrate = array2[i];
				if (this.surround != 0)
				{
					int num8 = this.bitrate_bps;
					if (num5 * 50 < sampleRate)
					{
						num8 -= 60 * (sampleRate / num5 - 50) * this.layout.nb_channels;
					}
					if (num8 > 10000 * this.layout.nb_channels)
					{
						opusEncoder.Bandwidth = OpusBandwidth.OPUS_BANDWIDTH_FULLBAND;
					}
					else if (num8 > 7000 * this.layout.nb_channels)
					{
						opusEncoder.Bandwidth = OpusBandwidth.OPUS_BANDWIDTH_SUPERWIDEBAND;
					}
					else if (num8 > 5000 * this.layout.nb_channels)
					{
						opusEncoder.Bandwidth = OpusBandwidth.OPUS_BANDWIDTH_WIDEBAND;
					}
					else
					{
						opusEncoder.Bandwidth = OpusBandwidth.OPUS_BANDWIDTH_NARROWBAND;
					}
					if (i < this.layout.nb_coupled_streams)
					{
						opusEncoder.ForceMode = OpusMode.MODE_CELT_ONLY;
						opusEncoder.ForceChannels = 2;
					}
				}
			}
			num2 = 0;
			int num9 = 0;
			for (int i = 0; i < this.layout.nb_streams; i++)
			{
				opusRepacketizer.Reset();
				OpusEncoder opusEncoder2 = this.encoders[num2];
				int c2;
				int c3;
				if (i < this.layout.nb_coupled_streams)
				{
					int num10 = OpusMultistream.get_left_channel(this.layout, i, -1);
					int num11 = OpusMultistream.get_right_channel(this.layout, i, -1);
					copy_channel_in.Invoke(array5, 0, 2, pcm, this.layout.nb_channels, num10, num5);
					copy_channel_in.Invoke(array5, 1, 2, pcm, this.layout.nb_channels, num11, num5);
					num2++;
					if (this.surround != 0)
					{
						for (int j = 0; j < 21; j++)
						{
							array3[j] = array6[21 * num10 + j];
							array3[21 + j] = array6[21 * num11 + j];
						}
					}
					c2 = num10;
					c3 = num11;
				}
				else
				{
					int num12 = OpusMultistream.get_mono_channel(this.layout, i, -1);
					copy_channel_in.Invoke(array5, 0, 1, pcm, this.layout.nb_channels, num12, num5);
					num2++;
					if (this.surround != 0)
					{
						for (int k = 0; k < 21; k++)
						{
							array3[k] = array6[21 * num12 + k];
						}
					}
					c2 = num12;
					c3 = -1;
				}
				if (this.surround != 0)
				{
					opusEncoder2.SetEnergyMask(array3);
				}
				int num13 = max_data_bytes - num9;
				num13 -= Inlines.IMAX(0, 2 * (this.layout.nb_streams - i - 1) - 1);
				num13 = Inlines.IMIN(num13, 3832);
				if (i != this.layout.nb_streams - 1)
				{
					num13 -= ((num13 > 253) ? 2 : 1);
				}
				if (num3 == 0 && i == this.layout.nb_streams - 1)
				{
					opusEncoder2.Bitrate = num13 * (8 * sampleRate / num5);
				}
				int num14 = opusEncoder2.opus_encode_native<T>(array5, 0, num5, array, 0, num13, lsb_depth, pcm, analysis_frame_size, c2, c3, this.layout.nb_channels, downmix, float_api);
				if (num14 < 0)
				{
					return num14;
				}
				opusRepacketizer.AddPacket(array, 0, num14);
				num14 = opusRepacketizer.opus_repacketizer_out_range_impl(0, opusRepacketizer.GetNumFrames(), data, num, max_data_bytes - num9, (i != this.layout.nb_streams - 1) ? 1 : 0, (num3 == 0 && i == this.layout.nb_streams - 1) ? 1 : 0);
				num += num14;
				num9 += num14;
			}
			return num9;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0001E3C4 File Offset: 0x0001C5C4
		internal unsafe static void opus_copy_channel_in_float(Span<short> dst, int dst_offset, int dst_stride, ReadOnlySpan<float> src, int src_stride, int src_channel, int frame_size)
		{
			for (int i = 0; i < frame_size; i++)
			{
				*dst[i * dst_stride + dst_offset] = Inlines.FLOAT2INT16(*src[i * src_stride + src_channel]);
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0001E400 File Offset: 0x0001C600
		internal unsafe static void opus_copy_channel_in_short(Span<short> dst, int dst_offset, int dst_stride, ReadOnlySpan<short> src, int src_stride, int src_channel, int frame_size)
		{
			for (int i = 0; i < frame_size; i++)
			{
				*dst[i * dst_stride + dst_offset] = *src[i * src_stride + src_channel];
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0001E436 File Offset: 0x0001C636
		[Obsolete("Use Span<> overrides if possible")]
		public int EncodeMultistream(short[] pcm, int pcm_offset, int frame_size, byte[] outputBuffer, int outputBuffer_offset, int max_data_bytes)
		{
			return this.EncodeMultistream(MemoryExtensions.AsSpan<short>(pcm, pcm_offset), frame_size, MemoryExtensions.AsSpan<byte>(outputBuffer, outputBuffer_offset), max_data_bytes);
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0001E458 File Offset: 0x0001C658
		public int EncodeMultistream(ReadOnlySpan<short> pcm, int frame_size, Span<byte> outputBuffer, int max_data_bytes)
		{
			OpusMSEncoder.opus_copy_channel_in_func<short> copy_channel_in;
			if ((copy_channel_in = OpusMSEncoder.<>O.<0>__opus_copy_channel_in_short) == null)
			{
				copy_channel_in = (OpusMSEncoder.<>O.<0>__opus_copy_channel_in_short = new OpusMSEncoder.opus_copy_channel_in_func<short>(OpusMSEncoder.opus_copy_channel_in_short));
			}
			int lsb_depth = 16;
			Downmix.downmix_func<short> downmix;
			if ((downmix = OpusMSEncoder.<>O.<1>__downmix_int) == null)
			{
				downmix = (OpusMSEncoder.<>O.<1>__downmix_int = new Downmix.downmix_func<short>(Downmix.downmix_int));
			}
			int num = this.opus_multistream_encode_native<short>(copy_channel_in, pcm, frame_size, outputBuffer, max_data_bytes, lsb_depth, downmix, 0);
			if (num >= 0)
			{
				return num;
			}
			if (num == -1)
			{
				throw new ArgumentException("OPUS_BAD_ARG while encoding");
			}
			throw new OpusException("An error occurred during encoding: " + CodecHelpers.opus_strerror(num), num);
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0001E4D5 File Offset: 0x0001C6D5
		[Obsolete("Use Span<> overrides if possible")]
		public int EncodeMultistream(float[] pcm, int pcm_offset, int frame_size, byte[] outputBuffer, int outputBuffer_offset, int max_data_bytes)
		{
			return this.EncodeMultistream(MemoryExtensions.AsSpan<float>(pcm, pcm_offset), frame_size, MemoryExtensions.AsSpan<byte>(outputBuffer, outputBuffer_offset), max_data_bytes);
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0001E4F8 File Offset: 0x0001C6F8
		public int EncodeMultistream(ReadOnlySpan<float> pcm, int frame_size, Span<byte> outputBuffer, int max_data_bytes)
		{
			OpusMSEncoder.opus_copy_channel_in_func<float> copy_channel_in;
			if ((copy_channel_in = OpusMSEncoder.<>O.<2>__opus_copy_channel_in_float) == null)
			{
				copy_channel_in = (OpusMSEncoder.<>O.<2>__opus_copy_channel_in_float = new OpusMSEncoder.opus_copy_channel_in_func<float>(OpusMSEncoder.opus_copy_channel_in_float));
			}
			int lsb_depth = 16;
			Downmix.downmix_func<float> downmix;
			if ((downmix = OpusMSEncoder.<>O.<3>__downmix_float) == null)
			{
				downmix = (OpusMSEncoder.<>O.<3>__downmix_float = new Downmix.downmix_func<float>(Downmix.downmix_float));
			}
			int num = this.opus_multistream_encode_native<float>(copy_channel_in, pcm, frame_size, outputBuffer, max_data_bytes, lsb_depth, downmix, 1);
			if (num >= 0)
			{
				return num;
			}
			if (num == -1)
			{
				throw new ArgumentException("OPUS_BAD_ARG while encoding");
			}
			throw new OpusException("An error occurred during encoding: " + CodecHelpers.opus_strerror(num), num);
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x0001E578 File Offset: 0x0001C778
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x0001E5B6 File Offset: 0x0001C7B6
		public int Bitrate
		{
			get
			{
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < this.layout.nb_streams; i++)
				{
					OpusEncoder opusEncoder = this.encoders[num2++];
					num += opusEncoder.Bitrate;
				}
				return num;
			}
			set
			{
				if (value < 0 && value != -1000 && value != -1)
				{
					throw new ArgumentException("Invalid bitrate");
				}
				this.bitrate_bps = value;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001FA RID: 506 RVA: 0x0001E5DA File Offset: 0x0001C7DA
		// (set) Token: 0x060001FB RID: 507 RVA: 0x0001E5EC File Offset: 0x0001C7EC
		public OpusApplication Application
		{
			get
			{
				return this.encoders[0].Application;
			}
			set
			{
				for (int i = 0; i < this.layout.nb_streams; i++)
				{
					this.encoders[i].Application = value;
				}
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0001E61D File Offset: 0x0001C81D
		// (set) Token: 0x060001FD RID: 509 RVA: 0x0001E62C File Offset: 0x0001C82C
		public int ForceChannels
		{
			get
			{
				return this.encoders[0].ForceChannels;
			}
			set
			{
				for (int i = 0; i < this.layout.nb_streams; i++)
				{
					this.encoders[i].ForceChannels = value;
				}
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001FE RID: 510 RVA: 0x0001E65D File Offset: 0x0001C85D
		public int NumChannels
		{
			get
			{
				return this.layout.nb_channels;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001FF RID: 511 RVA: 0x0001E66A File Offset: 0x0001C86A
		// (set) Token: 0x06000200 RID: 512 RVA: 0x0001E67C File Offset: 0x0001C87C
		public OpusBandwidth MaxBandwidth
		{
			get
			{
				return this.encoders[0].MaxBandwidth;
			}
			set
			{
				for (int i = 0; i < this.layout.nb_streams; i++)
				{
					this.encoders[i].MaxBandwidth = value;
				}
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000201 RID: 513 RVA: 0x0001E6AD File Offset: 0x0001C8AD
		// (set) Token: 0x06000202 RID: 514 RVA: 0x0001E6BC File Offset: 0x0001C8BC
		public OpusBandwidth Bandwidth
		{
			get
			{
				return this.encoders[0].Bandwidth;
			}
			set
			{
				for (int i = 0; i < this.layout.nb_streams; i++)
				{
					this.encoders[i].Bandwidth = value;
				}
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000203 RID: 515 RVA: 0x0001E6ED File Offset: 0x0001C8ED
		// (set) Token: 0x06000204 RID: 516 RVA: 0x0001E6FC File Offset: 0x0001C8FC
		public bool UseDTX
		{
			get
			{
				return this.encoders[0].UseDTX;
			}
			set
			{
				for (int i = 0; i < this.layout.nb_streams; i++)
				{
					this.encoders[i].UseDTX = value;
				}
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000205 RID: 517 RVA: 0x0001E72D File Offset: 0x0001C92D
		// (set) Token: 0x06000206 RID: 518 RVA: 0x0001E73C File Offset: 0x0001C93C
		public int Complexity
		{
			get
			{
				return this.encoders[0].Complexity;
			}
			set
			{
				for (int i = 0; i < this.layout.nb_streams; i++)
				{
					this.encoders[i].Complexity = value;
				}
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000207 RID: 519 RVA: 0x0001E76D File Offset: 0x0001C96D
		// (set) Token: 0x06000208 RID: 520 RVA: 0x0001E77C File Offset: 0x0001C97C
		public OpusMode ForceMode
		{
			get
			{
				return this.encoders[0].ForceMode;
			}
			set
			{
				for (int i = 0; i < this.layout.nb_streams; i++)
				{
					this.encoders[i].ForceMode = value;
				}
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000209 RID: 521 RVA: 0x0001E7AD File Offset: 0x0001C9AD
		// (set) Token: 0x0600020A RID: 522 RVA: 0x0001E7BC File Offset: 0x0001C9BC
		public bool UseInbandFEC
		{
			get
			{
				return this.encoders[0].UseInbandFEC;
			}
			set
			{
				for (int i = 0; i < this.layout.nb_streams; i++)
				{
					this.encoders[i].UseInbandFEC = value;
				}
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x0600020B RID: 523 RVA: 0x0001E7ED File Offset: 0x0001C9ED
		// (set) Token: 0x0600020C RID: 524 RVA: 0x0001E7FC File Offset: 0x0001C9FC
		public int PacketLossPercent
		{
			get
			{
				return this.encoders[0].PacketLossPercent;
			}
			set
			{
				for (int i = 0; i < this.layout.nb_streams; i++)
				{
					this.encoders[i].PacketLossPercent = value;
				}
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600020D RID: 525 RVA: 0x0001E82D File Offset: 0x0001CA2D
		// (set) Token: 0x0600020E RID: 526 RVA: 0x0001E83C File Offset: 0x0001CA3C
		public bool UseVBR
		{
			get
			{
				return this.encoders[0].UseVBR;
			}
			set
			{
				for (int i = 0; i < this.layout.nb_streams; i++)
				{
					this.encoders[i].UseVBR = value;
				}
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600020F RID: 527 RVA: 0x0001E86D File Offset: 0x0001CA6D
		// (set) Token: 0x06000210 RID: 528 RVA: 0x0001E87C File Offset: 0x0001CA7C
		public bool UseConstrainedVBR
		{
			get
			{
				return this.encoders[0].UseConstrainedVBR;
			}
			set
			{
				for (int i = 0; i < this.layout.nb_streams; i++)
				{
					this.encoders[i].UseConstrainedVBR = value;
				}
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0001E8AD File Offset: 0x0001CAAD
		// (set) Token: 0x06000212 RID: 530 RVA: 0x0001E8BC File Offset: 0x0001CABC
		public OpusSignal SignalType
		{
			get
			{
				return this.encoders[0].SignalType;
			}
			set
			{
				for (int i = 0; i < this.layout.nb_streams; i++)
				{
					this.encoders[i].SignalType = value;
				}
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000213 RID: 531 RVA: 0x0001E8ED File Offset: 0x0001CAED
		public int Lookahead
		{
			get
			{
				return this.encoders[0].Lookahead;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000214 RID: 532 RVA: 0x0001E8FC File Offset: 0x0001CAFC
		public int SampleRate
		{
			get
			{
				return this.encoders[0].SampleRate;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000215 RID: 533 RVA: 0x0001E90C File Offset: 0x0001CB0C
		public uint FinalRange
		{
			get
			{
				uint num = 0U;
				int num2 = 0;
				for (int i = 0; i < this.layout.nb_streams; i++)
				{
					num ^= this.encoders[num2++].FinalRange;
				}
				return num;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0001E948 File Offset: 0x0001CB48
		// (set) Token: 0x06000217 RID: 535 RVA: 0x0001E958 File Offset: 0x0001CB58
		public int LSBDepth
		{
			get
			{
				return this.encoders[0].LSBDepth;
			}
			set
			{
				for (int i = 0; i < this.layout.nb_streams; i++)
				{
					this.encoders[i].LSBDepth = value;
				}
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x06000218 RID: 536 RVA: 0x0001E989 File Offset: 0x0001CB89
		// (set) Token: 0x06000219 RID: 537 RVA: 0x0001E998 File Offset: 0x0001CB98
		public bool PredictionDisabled
		{
			get
			{
				return this.encoders[0].PredictionDisabled;
			}
			set
			{
				for (int i = 0; i < this.layout.nb_streams; i++)
				{
					this.encoders[i].PredictionDisabled = value;
				}
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600021A RID: 538 RVA: 0x0001E9C9 File Offset: 0x0001CBC9
		// (set) Token: 0x0600021B RID: 539 RVA: 0x0001E9D1 File Offset: 0x0001CBD1
		public OpusFramesize ExpertFrameDuration
		{
			get
			{
				return this.variable_duration;
			}
			set
			{
				this.variable_duration = value;
			}
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0001E9DA File Offset: 0x0001CBDA
		public OpusEncoder GetMultistreamEncoderState(int streamId)
		{
			if (streamId >= this.layout.nb_streams)
			{
				throw new ArgumentException("Requested stream doesn't exist");
			}
			return this.encoders[streamId];
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0001E9FD File Offset: 0x0001CBFD
		public string GetVersionString()
		{
			return CodecHelpers.GetVersionString();
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0001EA04 File Offset: 0x0001CC04
		public void Dispose()
		{
		}

		// Token: 0x040002F4 RID: 756
		internal readonly ChannelLayout layout = new ChannelLayout();

		// Token: 0x040002F5 RID: 757
		internal int lfe_stream;

		// Token: 0x040002F6 RID: 758
		internal OpusApplication application = OpusApplication.OPUS_APPLICATION_AUDIO;

		// Token: 0x040002F7 RID: 759
		internal OpusFramesize variable_duration;

		// Token: 0x040002F8 RID: 760
		internal int surround;

		// Token: 0x040002F9 RID: 761
		internal int bitrate_bps;

		// Token: 0x040002FA RID: 762
		internal readonly float[] subframe_mem = new float[3];

		// Token: 0x040002FB RID: 763
		internal readonly OpusEncoder[] encoders;

		// Token: 0x040002FC RID: 764
		internal readonly int[] window_mem;

		// Token: 0x040002FD RID: 765
		internal readonly int[] preemph_mem;

		// Token: 0x040002FE RID: 766
		private static readonly int[] diff_table = new int[]
		{
			512,
			300,
			165,
			87,
			45,
			23,
			11,
			6,
			3,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0
		};

		// Token: 0x040002FF RID: 767
		private const int MS_FRAME_TMP = 3832;

		// Token: 0x020000A2 RID: 162
		// (Invoke) Token: 0x06000549 RID: 1353
		internal delegate void opus_copy_channel_in_func<T>(Span<short> dst, int dst_ptr, int dst_stride, ReadOnlySpan<T> src, int src_stride, int src_channel, int frame_size);

		// Token: 0x020000A3 RID: 163
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040005EF RID: 1519
			public static OpusMSEncoder.opus_copy_channel_in_func<short> <0>__opus_copy_channel_in_short;

			// Token: 0x040005F0 RID: 1520
			public static Downmix.downmix_func<short> <1>__downmix_int;

			// Token: 0x040005F1 RID: 1521
			public static OpusMSEncoder.opus_copy_channel_in_func<float> <2>__opus_copy_channel_in_float;

			// Token: 0x040005F2 RID: 1522
			public static Downmix.downmix_func<float> <3>__downmix_float;
		}
	}
}
