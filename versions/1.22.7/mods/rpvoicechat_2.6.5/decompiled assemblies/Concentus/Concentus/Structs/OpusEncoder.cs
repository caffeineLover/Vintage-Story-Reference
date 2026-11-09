using System;
using System.Runtime.CompilerServices;
using Concentus.Celt.Structs;
using Concentus.Common;
using Concentus.Common.CPlusPlus;
using Concentus.Enums;
using Concentus.Silk;
using Concentus.Silk.Structs;

namespace Concentus.Structs
{
	// Token: 0x0200005B RID: 91
	public class OpusEncoder : IOpusEncoder, IDisposable
	{
		// Token: 0x06000192 RID: 402 RVA: 0x00019DFC File Offset: 0x00017FFC
		internal OpusEncoder()
		{
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00019E64 File Offset: 0x00018064
		internal void Reset()
		{
			this.silk_mode.Reset();
			this.application = OpusApplication.OPUS_APPLICATION_UNIMPLEMENTED;
			this.channels = 0;
			this.delay_compensation = 0;
			this.force_channels = 0;
			this.signal_type = (OpusSignal)0;
			this.user_bandwidth = (OpusBandwidth)0;
			this.max_bandwidth = (OpusBandwidth)0;
			this.user_forced_mode = (OpusMode)0;
			this.voice_ratio = 0;
			this.Fs = 0;
			this.use_vbr = 0;
			this.vbr_constraint = 0;
			this.variable_duration = (OpusFramesize)0;
			this.bitrate_bps = 0;
			this.user_bitrate_bps = 0;
			this.lsb_depth = 0;
			this.encoder_buffer = 0;
			this.lfe = 0;
			this.analysis.Reset();
			this.PartialReset();
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00019F0C File Offset: 0x0001810C
		internal void PartialReset()
		{
			this.stream_channels = 0;
			this.hybrid_stereo_width_Q14 = 0;
			this.variable_HP_smth2_Q15 = 0;
			this.prev_HB_gain = 0;
			Arrays.MemSetInt(this.hp_mem, 0, 4);
			this.mode = (OpusMode)0;
			this.prev_mode = (OpusMode)0;
			this.prev_channels = 0;
			this.prev_framesize = 0;
			this.bandwidth = (OpusBandwidth)0;
			this.silk_bw_switch = 0;
			this.first = 0;
			this.energy_masking = null;
			this.width_mem.Reset();
			Arrays.MemSetShort(this.delay_buffer, 0, 960);
			this.detected_bandwidth = (OpusBandwidth)0;
			this.rangeFinal = 0U;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00019FA4 File Offset: 0x000181A4
		public void ResetState()
		{
			EncControlState encStatus = new EncControlState();
			this.analysis.Reset();
			this.PartialReset();
			this.Celt_Encoder.ResetState();
			EncodeAPI.silk_InitEncoder(this.SilkEncoder, encStatus);
			this.stream_channels = this.channels;
			this.hybrid_stereo_width_Q14 = 16384;
			this.prev_HB_gain = 32767;
			this.first = 1;
			this.mode = OpusMode.MODE_HYBRID;
			this.bandwidth = OpusBandwidth.OPUS_BANDWIDTH_FULLBAND;
			this.variable_HP_smth2_Q15 = Inlines.silk_LSHIFT(Inlines.silk_lin2log(60), 8);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0001A034 File Offset: 0x00018234
		[Obsolete("Use OpusCodecFactory methods which can give you native code if supported by your platform")]
		public OpusEncoder(int Fs, int channels, OpusApplication application)
		{
			if (Fs != 48000 && Fs != 24000 && Fs != 16000 && Fs != 12000 && Fs != 8000)
			{
				throw new ArgumentException("Sample rate is invalid (must be 8/12/16/24/48 Khz)");
			}
			if (channels != 1 && channels != 2)
			{
				throw new ArgumentException("Number of channels must be 1 or 2");
			}
			int num = this.opus_init_encoder(Fs, channels, application);
			if (num == 0)
			{
				return;
			}
			if (num == -1)
			{
				throw new ArgumentException("OPUS_BAD_ARG when creating encoder");
			}
			throw new OpusException("Error while initializing encoder: " + CodecHelpers.opus_strerror(num), num);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0001A114 File Offset: 0x00018314
		internal int opus_init_encoder(int Fs, int channels, OpusApplication application)
		{
			if ((Fs != 48000 && Fs != 24000 && Fs != 16000 && Fs != 12000 && Fs != 8000) || (channels != 1 && channels != 2) || application == OpusApplication.OPUS_APPLICATION_UNIMPLEMENTED)
			{
				return -1;
			}
			this.Reset();
			SilkEncoder silkEncoder = this.SilkEncoder;
			CeltEncoder celt_Encoder = this.Celt_Encoder;
			this.channels = channels;
			this.stream_channels = channels;
			this.Fs = Fs;
			if (EncodeAPI.silk_InitEncoder(silkEncoder, this.silk_mode) != 0)
			{
				return -3;
			}
			this.silk_mode.nChannelsAPI = channels;
			this.silk_mode.nChannelsInternal = channels;
			this.silk_mode.API_sampleRate = this.Fs;
			this.silk_mode.maxInternalSampleRate = 16000;
			this.silk_mode.minInternalSampleRate = 8000;
			this.silk_mode.desiredInternalSampleRate = 16000;
			this.silk_mode.payloadSize_ms = 20;
			this.silk_mode.bitRate = 25000;
			this.silk_mode.packetLossPercentage = 0;
			this.silk_mode.complexity = 9;
			this.silk_mode.useInBandFEC = 0;
			this.silk_mode.useDTX = 0;
			this.silk_mode.useCBR = 0;
			this.silk_mode.reducedDependency = 0;
			if (celt_Encoder.celt_encoder_init(Fs, channels) != 0)
			{
				return -3;
			}
			celt_Encoder.SetSignalling(0);
			celt_Encoder.SetComplexity(this.silk_mode.complexity);
			this.use_vbr = 1;
			this.vbr_constraint = 1;
			this.user_bitrate_bps = -1000;
			this.bitrate_bps = 3000 + Fs * channels;
			this.application = application;
			this.signal_type = OpusSignal.OPUS_SIGNAL_AUTO;
			this.user_bandwidth = OpusBandwidth.OPUS_BANDWIDTH_AUTO;
			this.max_bandwidth = OpusBandwidth.OPUS_BANDWIDTH_FULLBAND;
			this.force_channels = -1000;
			this.user_forced_mode = OpusMode.MODE_AUTO;
			this.voice_ratio = -1;
			this.encoder_buffer = this.Fs / 100;
			this.lsb_depth = 24;
			this.variable_duration = OpusFramesize.OPUS_FRAMESIZE_ARG;
			this.delay_compensation = this.Fs / 250;
			this.hybrid_stereo_width_Q14 = 16384;
			this.prev_HB_gain = 32767;
			this.variable_HP_smth2_Q15 = Inlines.silk_LSHIFT(Inlines.silk_lin2log(60), 8);
			this.first = 1;
			this.mode = OpusMode.MODE_HYBRID;
			this.bandwidth = OpusBandwidth.OPUS_BANDWIDTH_FULLBAND;
			Analysis.tonality_analysis_init(this.analysis);
			return 0;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0001A368 File Offset: 0x00018568
		internal int user_bitrate_to_bitrate(int user_bitrate, int frame_size, int max_data_bytes)
		{
			if (frame_size == 0)
			{
				frame_size = this.Fs / 400;
			}
			if (user_bitrate == -1000)
			{
				return 60 * this.Fs / frame_size + this.Fs * this.channels;
			}
			if (user_bitrate == -1)
			{
				return max_data_bytes * 8 * this.Fs / frame_size;
			}
			return user_bitrate;
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0001A3BC File Offset: 0x000185BC
		internal unsafe int opus_encode_native<T>(ReadOnlySpan<short> pcm, int pcm_ptr, int frame_size, Span<byte> data, int data_ptr, int out_data_bytes, int lsb_depth, ReadOnlySpan<T> analysis_pcm, int analysis_size, int c1, int c2, int analysis_channels, Downmix.downmix_func<T> downmix, int float_api)
		{
			int num = 0;
			EntropyCoder entropyCoder = new EntropyCoder();
			int num2 = 0;
			int startBand = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			uint num7 = 0U;
			AnalysisInfo analysisInfo = new AnalysisInfo();
			int num8 = -1;
			int read_subframe = -1;
			int num9 = Inlines.IMIN(1276, out_data_bytes);
			this.rangeFinal = 0U;
			if ((this.variable_duration == (OpusFramesize)0 && 400 * frame_size != this.Fs && 200 * frame_size != this.Fs && 100 * frame_size != this.Fs && 50 * frame_size != this.Fs && 25 * frame_size != this.Fs && 50 * frame_size != 3 * this.Fs) || 400 * frame_size < this.Fs || num9 <= 0)
			{
				return -1;
			}
			SilkEncoder silkEncoder = this.SilkEncoder;
			CeltEncoder celt_Encoder = this.Celt_Encoder;
			int num10;
			if (this.application == OpusApplication.OPUS_APPLICATION_RESTRICTED_LOWDELAY)
			{
				num10 = 0;
			}
			else
			{
				num10 = this.delay_compensation;
			}
			lsb_depth = Inlines.IMIN(lsb_depth, this.lsb_depth);
			CeltMode celtMode = celt_Encoder.GetMode();
			this.voice_ratio = -1;
			if (this.analysis.enabled)
			{
				analysisInfo.valid = 0;
				if ((this._vqLevel != null || this.silk_mode.complexity >= 7) && this.Fs == 48000)
				{
					num8 = this.analysis.read_pos;
					read_subframe = this.analysis.read_subframe;
					Analysis.run_analysis<T>(this.analysis, celtMode, analysis_pcm, analysis_size, frame_size, c1, c2, analysis_channels, this.Fs, lsb_depth, downmix, analysisInfo);
				}
				this.detected_bandwidth = (OpusBandwidth)0;
				if (analysisInfo.valid != 0)
				{
					if (this.signal_type == OpusSignal.OPUS_SIGNAL_AUTO)
					{
						this.voice_ratio = (int)Math.Floor((double)(0.5f + 100f * (1f - analysisInfo.music_prob)));
					}
					int num11 = analysisInfo.bandwidth;
					if (num11 <= 12)
					{
						this.detected_bandwidth = OpusBandwidth.OPUS_BANDWIDTH_NARROWBAND;
					}
					else if (num11 <= 14)
					{
						this.detected_bandwidth = OpusBandwidth.OPUS_BANDWIDTH_MEDIUMBAND;
					}
					else if (num11 <= 16)
					{
						this.detected_bandwidth = OpusBandwidth.OPUS_BANDWIDTH_WIDEBAND;
					}
					else if (num11 <= 18)
					{
						this.detected_bandwidth = OpusBandwidth.OPUS_BANDWIDTH_SUPERWIDEBAND;
					}
					else
					{
						this.detected_bandwidth = OpusBandwidth.OPUS_BANDWIDTH_FULLBAND;
					}
					if (this._vqLevel != null)
					{
						this.user_bitrate_bps = OpusEncoder.GetVariableQualityBitrate(this._vqLevel.Value, this.analysis.music_prob);
					}
				}
			}
			int num12;
			if (this.channels == 2 && this.force_channels != 1)
			{
				num12 = CodecHelpers.compute_stereo_width(pcm, pcm_ptr, frame_size, this.Fs, this.width_mem);
			}
			else
			{
				num12 = 0;
			}
			int num13 = num10;
			this.bitrate_bps = this.user_bitrate_to_bitrate(this.user_bitrate_bps, frame_size, num9);
			int num14 = this.Fs / frame_size;
			if (this.use_vbr == 0)
			{
				int num15 = 3 * this.Fs / frame_size;
				int num16 = Inlines.IMIN((3 * this.bitrate_bps / 8 + num15 / 2) / num15, num9);
				this.bitrate_bps = num16 * num15 * 8 / 3;
				num9 = num16;
			}
			if (num9 < 3 || this.bitrate_bps < 3 * num14 * 8 || (num14 < 50 && (num9 * num14 < 300 || this.bitrate_bps < 2400)))
			{
				OpusMode opusMode = this.mode;
				OpusBandwidth opusBandwidth = (this.bandwidth == (OpusBandwidth)0) ? OpusBandwidth.OPUS_BANDWIDTH_NARROWBAND : this.bandwidth;
				if (opusMode == (OpusMode)0)
				{
					opusMode = OpusMode.MODE_SILK_ONLY;
				}
				if (num14 > 100)
				{
					opusMode = OpusMode.MODE_CELT_ONLY;
				}
				if (num14 < 50)
				{
					opusMode = OpusMode.MODE_SILK_ONLY;
				}
				if (opusMode == OpusMode.MODE_SILK_ONLY && opusBandwidth > OpusBandwidth.OPUS_BANDWIDTH_WIDEBAND)
				{
					opusBandwidth = OpusBandwidth.OPUS_BANDWIDTH_WIDEBAND;
				}
				else if (opusMode == OpusMode.MODE_CELT_ONLY && opusBandwidth == OpusBandwidth.OPUS_BANDWIDTH_MEDIUMBAND)
				{
					opusBandwidth = OpusBandwidth.OPUS_BANDWIDTH_NARROWBAND;
				}
				else if (opusMode == OpusMode.MODE_HYBRID && opusBandwidth <= OpusBandwidth.OPUS_BANDWIDTH_SUPERWIDEBAND)
				{
					opusBandwidth = OpusBandwidth.OPUS_BANDWIDTH_SUPERWIDEBAND;
				}
				*data[data_ptr] = CodecHelpers.gen_toc(opusMode, num14, opusBandwidth, this.stream_channels);
				num = 1;
				if (this.use_vbr == 0)
				{
					num = OpusRepacketizer.PadPacket(data, data_ptr, num, num9);
					if (num == 0)
					{
						num = num9;
					}
				}
				return num;
			}
			int num17 = num14 * num9 * 8;
			int num18 = this.bitrate_bps - (40 * this.channels + 20) * (this.Fs / frame_size - 50);
			int num19;
			if (this.signal_type == OpusSignal.OPUS_SIGNAL_VOICE)
			{
				num19 = 127;
			}
			else if (this.signal_type == OpusSignal.OPUS_SIGNAL_MUSIC)
			{
				num19 = 0;
			}
			else if (this.voice_ratio >= 0)
			{
				num19 = this.voice_ratio * 327 >> 8;
				if (this.application == OpusApplication.OPUS_APPLICATION_AUDIO)
				{
					num19 = Inlines.IMIN(num19, 115);
				}
			}
			else if (this.application == OpusApplication.OPUS_APPLICATION_VOIP)
			{
				num19 = 115;
			}
			else
			{
				num19 = 48;
			}
			if (this.force_channels != -1000 && this.channels == 2)
			{
				this.stream_channels = this.force_channels;
			}
			else if (this.channels == 2)
			{
				int num20 = 30000 + (0 >> 14);
				if (this.stream_channels == 2)
				{
					num20 -= 1000;
				}
				else
				{
					num20 += 1000;
				}
				this.stream_channels = ((num18 > num20) ? 2 : 1);
			}
			else
			{
				this.stream_channels = this.channels;
			}
			num18 = this.bitrate_bps - (40 * this.stream_channels + 20) * (this.Fs / frame_size - 50);
			if (this.application == OpusApplication.OPUS_APPLICATION_RESTRICTED_LOWDELAY)
			{
				this.mode = OpusMode.MODE_CELT_ONLY;
			}
			else if (this.user_forced_mode == OpusMode.MODE_AUTO)
			{
				int num21 = Inlines.MULT16_32_Q15(32767 - num12, Concentus.Tables.mode_thresholds[0][0]) + Inlines.MULT16_32_Q15(num12, Concentus.Tables.mode_thresholds[1][0]);
				int num22 = Inlines.MULT16_32_Q15(32767 - num12, Concentus.Tables.mode_thresholds[1][1]) + Inlines.MULT16_32_Q15(num12, Concentus.Tables.mode_thresholds[1][1]);
				int num23 = num22 + (num19 * num19 * (num21 - num22) >> 14);
				if (this.application == OpusApplication.OPUS_APPLICATION_VOIP)
				{
					num23 += 8000;
				}
				if (this.prev_mode == OpusMode.MODE_CELT_ONLY)
				{
					num23 -= 4000;
				}
				else if (this.prev_mode > (OpusMode)0)
				{
					num23 += 4000;
				}
				this.mode = ((num18 >= num23) ? OpusMode.MODE_CELT_ONLY : OpusMode.MODE_SILK_ONLY);
				if (this.silk_mode.useInBandFEC != 0 && this.silk_mode.packetLossPercentage > 128 - num19 >> 4)
				{
					this.mode = OpusMode.MODE_SILK_ONLY;
				}
				if (this.silk_mode.useDTX != 0 && num19 > 100)
				{
					this.mode = OpusMode.MODE_SILK_ONLY;
				}
			}
			else
			{
				this.mode = this.user_forced_mode;
			}
			if (this.mode != OpusMode.MODE_CELT_ONLY && frame_size < this.Fs / 100)
			{
				this.mode = OpusMode.MODE_CELT_ONLY;
			}
			if (this.lfe != 0)
			{
				this.mode = OpusMode.MODE_CELT_ONLY;
			}
			if (num9 < ((num14 > 50) ? 12000 : 8000) * frame_size / (this.Fs * 8))
			{
				this.mode = OpusMode.MODE_CELT_ONLY;
			}
			if (this.stream_channels == 1 && this.prev_channels == 2 && this.silk_mode.toMono == 0 && this.mode != OpusMode.MODE_CELT_ONLY && this.prev_mode != OpusMode.MODE_CELT_ONLY)
			{
				this.silk_mode.toMono = 1;
				this.stream_channels = 2;
			}
			else
			{
				this.silk_mode.toMono = 0;
			}
			if (this.prev_mode > (OpusMode)0 && ((this.mode != OpusMode.MODE_CELT_ONLY && this.prev_mode == OpusMode.MODE_CELT_ONLY) || (this.mode == OpusMode.MODE_CELT_ONLY && this.prev_mode != OpusMode.MODE_CELT_ONLY)))
			{
				num3 = 1;
				num5 = ((this.mode != OpusMode.MODE_CELT_ONLY) ? 1 : 0);
				if (num5 == 0)
				{
					if (frame_size >= this.Fs / 100)
					{
						this.mode = this.prev_mode;
						num6 = 1;
					}
					else
					{
						num3 = 0;
					}
				}
			}
			if (this.silk_bw_switch != 0)
			{
				num3 = 1;
				num5 = 1;
				this.silk_bw_switch = 0;
				num2 = 1;
			}
			if (num3 != 0)
			{
				num4 = Inlines.IMIN(257, num9 * (this.Fs / 200) / (frame_size + this.Fs / 200));
				if (this.use_vbr != 0)
				{
					num4 = Inlines.IMIN(num4, this.bitrate_bps / 1600);
				}
			}
			if (this.mode != OpusMode.MODE_CELT_ONLY && this.prev_mode == OpusMode.MODE_CELT_ONLY)
			{
				EncControlState encStatus = new EncControlState();
				EncodeAPI.silk_InitEncoder(silkEncoder, encStatus);
				num2 = 1;
			}
			if (this.mode == OpusMode.MODE_CELT_ONLY || this.first != 0 || this.silk_mode.allowBandwidthSwitch != 0)
			{
				int[] array = new int[8];
				OpusBandwidth opusBandwidth2 = OpusBandwidth.OPUS_BANDWIDTH_FULLBAND;
				int num24 = num18;
				if (this.mode != OpusMode.MODE_CELT_ONLY)
				{
					num24 = num24 * (45 + this.silk_mode.complexity) / 50;
					if (this.use_vbr == 0)
					{
						num24 -= 1000;
					}
				}
				int[] array2;
				int[] array3;
				if (this.channels == 2 && this.force_channels != 1)
				{
					array2 = Concentus.Tables.stereo_voice_bandwidth_thresholds;
					array3 = Concentus.Tables.stereo_music_bandwidth_thresholds;
				}
				else
				{
					array2 = Concentus.Tables.mono_voice_bandwidth_thresholds;
					array3 = Concentus.Tables.mono_music_bandwidth_thresholds;
				}
				for (int i = 0; i < 8; i++)
				{
					array[i] = array3[i] + (num19 * num19 * (array2[i] - array3[i]) >> 14);
				}
				int num25;
				do
				{
					num25 = array[2 * (opusBandwidth2 - OpusBandwidth.OPUS_BANDWIDTH_MEDIUMBAND)];
					int num26 = array[2 * (opusBandwidth2 - OpusBandwidth.OPUS_BANDWIDTH_MEDIUMBAND) + 1];
					if (this.first == 0)
					{
						if (this.bandwidth >= opusBandwidth2)
						{
							num25 -= num26;
						}
						else
						{
							num25 += num26;
						}
					}
				}
				while (num24 < num25 && --opusBandwidth2 > OpusBandwidth.OPUS_BANDWIDTH_NARROWBAND);
				this.bandwidth = opusBandwidth2;
				if (this.first == 0 && this.mode != OpusMode.MODE_CELT_ONLY && this.silk_mode.inWBmodeWithoutVariableLP == 0 && this.bandwidth > OpusBandwidth.OPUS_BANDWIDTH_WIDEBAND)
				{
					this.bandwidth = OpusBandwidth.OPUS_BANDWIDTH_WIDEBAND;
				}
			}
			if (this.bandwidth > this.max_bandwidth)
			{
				this.bandwidth = this.max_bandwidth;
			}
			if (this.user_bandwidth != OpusBandwidth.OPUS_BANDWIDTH_AUTO)
			{
				this.bandwidth = this.user_bandwidth;
			}
			if (this.mode != OpusMode.MODE_CELT_ONLY && num17 < 15000)
			{
				this.bandwidth = OpusBandwidthHelpers.MIN(this.bandwidth, OpusBandwidth.OPUS_BANDWIDTH_WIDEBAND);
			}
			if (this.Fs <= 24000 && this.bandwidth > OpusBandwidth.OPUS_BANDWIDTH_SUPERWIDEBAND)
			{
				this.bandwidth = OpusBandwidth.OPUS_BANDWIDTH_SUPERWIDEBAND;
			}
			if (this.Fs <= 16000 && this.bandwidth > OpusBandwidth.OPUS_BANDWIDTH_WIDEBAND)
			{
				this.bandwidth = OpusBandwidth.OPUS_BANDWIDTH_WIDEBAND;
			}
			if (this.Fs <= 12000 && this.bandwidth > OpusBandwidth.OPUS_BANDWIDTH_MEDIUMBAND)
			{
				this.bandwidth = OpusBandwidth.OPUS_BANDWIDTH_MEDIUMBAND;
			}
			if (this.Fs <= 8000 && this.bandwidth > OpusBandwidth.OPUS_BANDWIDTH_NARROWBAND)
			{
				this.bandwidth = OpusBandwidth.OPUS_BANDWIDTH_NARROWBAND;
			}
			if (this.detected_bandwidth != (OpusBandwidth)0 && this.user_bandwidth == OpusBandwidth.OPUS_BANDWIDTH_AUTO)
			{
				OpusBandwidth b;
				if (num18 <= 18000 * this.stream_channels && this.mode == OpusMode.MODE_CELT_ONLY)
				{
					b = OpusBandwidth.OPUS_BANDWIDTH_NARROWBAND;
				}
				else if (num18 <= 24000 * this.stream_channels && this.mode == OpusMode.MODE_CELT_ONLY)
				{
					b = OpusBandwidth.OPUS_BANDWIDTH_MEDIUMBAND;
				}
				else if (num18 <= 30000 * this.stream_channels)
				{
					b = OpusBandwidth.OPUS_BANDWIDTH_WIDEBAND;
				}
				else if (num18 <= 44000 * this.stream_channels)
				{
					b = OpusBandwidth.OPUS_BANDWIDTH_SUPERWIDEBAND;
				}
				else
				{
					b = OpusBandwidth.OPUS_BANDWIDTH_FULLBAND;
				}
				this.detected_bandwidth = OpusBandwidthHelpers.MAX(this.detected_bandwidth, b);
				this.bandwidth = OpusBandwidthHelpers.MIN(this.bandwidth, this.detected_bandwidth);
			}
			celt_Encoder.SetLSBDepth(lsb_depth);
			if (this.mode == OpusMode.MODE_CELT_ONLY && this.bandwidth == OpusBandwidth.OPUS_BANDWIDTH_MEDIUMBAND)
			{
				this.bandwidth = OpusBandwidth.OPUS_BANDWIDTH_WIDEBAND;
			}
			if (this.lfe != 0)
			{
				this.bandwidth = OpusBandwidth.OPUS_BANDWIDTH_NARROWBAND;
			}
			if (frame_size <= this.Fs / 50 || (this.mode != OpusMode.MODE_CELT_ONLY && this.bandwidth <= OpusBandwidth.OPUS_BANDWIDTH_WIDEBAND))
			{
				OpusBandwidth opusBandwidth3 = this.bandwidth;
				if (this.mode == OpusMode.MODE_SILK_ONLY && opusBandwidth3 > OpusBandwidth.OPUS_BANDWIDTH_WIDEBAND)
				{
					this.mode = OpusMode.MODE_HYBRID;
				}
				if (this.mode == OpusMode.MODE_HYBRID && opusBandwidth3 <= OpusBandwidth.OPUS_BANDWIDTH_WIDEBAND)
				{
					this.mode = OpusMode.MODE_SILK_ONLY;
				}
				int num27 = Inlines.IMIN(num9 - num4, this.bitrate_bps * frame_size / (this.Fs * 8)) - 1;
				data_ptr++;
				entropyCoder.enc_init((uint)(num9 - 1));
				short[] array4 = new short[(num13 + frame_size) * this.channels];
				Arrays.MemCopy(this.delay_buffer, (this.encoder_buffer - num13) * this.channels, array4, 0, num13 * this.channels);
				int num28;
				if (this.mode == OpusMode.MODE_CELT_ONLY)
				{
					num28 = Inlines.silk_LSHIFT(Inlines.silk_lin2log(60), 8);
				}
				else
				{
					num28 = silkEncoder.state_Fxx[0].variable_HP_smth1_Q15;
				}
				this.variable_HP_smth2_Q15 = Inlines.silk_SMLAWB(this.variable_HP_smth2_Q15, num28 - this.variable_HP_smth2_Q15, 983);
				int cutoff_Hz = Inlines.silk_log2lin(Inlines.silk_RSHIFT(this.variable_HP_smth2_Q15, 8));
				if (this.application == OpusApplication.OPUS_APPLICATION_VOIP)
				{
					CodecHelpers.hp_cutoff(pcm, pcm_ptr, cutoff_Hz, array4, num13 * this.channels, this.hp_mem, frame_size, this.channels, this.Fs);
				}
				else
				{
					CodecHelpers.dc_reject(pcm, pcm_ptr, 3, array4, num13 * this.channels, this.hp_mem, frame_size, this.channels, this.Fs);
				}
				int num29 = 32767;
				if (this.mode != OpusMode.MODE_CELT_ONLY)
				{
					short[] array5 = new short[this.channels * frame_size];
					int num30 = 8 * num27 * num14;
					if (this.mode == OpusMode.MODE_HYBRID)
					{
						this.silk_mode.bitRate = this.stream_channels * (5000 + ((this.Fs == 100 * frame_size) ? 1000 : 0));
						if (opusBandwidth3 == OpusBandwidth.OPUS_BANDWIDTH_SUPERWIDEBAND)
						{
							this.silk_mode.bitRate += (num30 - this.silk_mode.bitRate) * 2 / 3;
						}
						else
						{
							this.silk_mode.bitRate += (num30 - this.silk_mode.bitRate) * 3 / 5;
						}
						if (this.silk_mode.bitRate > num30 * 4 / 5)
						{
							this.silk_mode.bitRate = num30 * 4 / 5;
						}
						if (this.energy_masking == null)
						{
							int num31 = num30 - this.silk_mode.bitRate;
							int num32 = (opusBandwidth3 == OpusBandwidth.OPUS_BANDWIDTH_SUPERWIDEBAND) ? 3000 : 3600;
							num29 = Inlines.SHL32(num31, 9) / Inlines.SHR32(num31 + this.stream_channels * num32, 6);
							num29 = ((num29 < 28086) ? (num29 + 4681) : 32767);
						}
					}
					else
					{
						this.silk_mode.bitRate = num30;
					}
					if (this.energy_masking != null && this.use_vbr != 0 && this.lfe == 0)
					{
						int num33 = 0;
						int num34 = 17;
						short a = 16000;
						if (this.bandwidth == OpusBandwidth.OPUS_BANDWIDTH_NARROWBAND)
						{
							num34 = 13;
							a = 8000;
						}
						else if (this.bandwidth == OpusBandwidth.OPUS_BANDWIDTH_MEDIUMBAND)
						{
							num34 = 15;
							a = 12000;
						}
						for (int j = 0; j < this.channels; j++)
						{
							for (int i = 0; i < num34; i++)
							{
								int num35 = Inlines.MAX16(Inlines.MIN16(this.energy_masking[21 * j + i], 512), -2048);
								if (num35 > 0)
								{
									num35 = Inlines.HALF16(num35);
								}
								num33 += num35;
							}
						}
						int num36 = num33 / num34 * this.channels;
						num36 += 205;
						int num37 = Inlines.PSHR32(Inlines.MULT16_16((int)a, num36), 10);
						num37 = Inlines.MAX32(num37, -2 * this.silk_mode.bitRate / 3);
						if (this.bandwidth == OpusBandwidth.OPUS_BANDWIDTH_SUPERWIDEBAND || this.bandwidth == OpusBandwidth.OPUS_BANDWIDTH_FULLBAND)
						{
							this.silk_mode.bitRate += 3 * num37 / 5;
						}
						else
						{
							this.silk_mode.bitRate += num37;
						}
						num27 += num37 * frame_size / (8 * this.Fs);
					}
					this.silk_mode.payloadSize_ms = 1000 * frame_size / this.Fs;
					this.silk_mode.nChannelsAPI = this.channels;
					this.silk_mode.nChannelsInternal = this.stream_channels;
					if (opusBandwidth3 == OpusBandwidth.OPUS_BANDWIDTH_NARROWBAND)
					{
						this.silk_mode.desiredInternalSampleRate = 8000;
					}
					else if (opusBandwidth3 == OpusBandwidth.OPUS_BANDWIDTH_MEDIUMBAND)
					{
						this.silk_mode.desiredInternalSampleRate = 12000;
					}
					else
					{
						this.silk_mode.desiredInternalSampleRate = 16000;
					}
					if (this.mode == OpusMode.MODE_HYBRID)
					{
						this.silk_mode.minInternalSampleRate = 16000;
					}
					else
					{
						this.silk_mode.minInternalSampleRate = 8000;
					}
					if (this.mode == OpusMode.MODE_SILK_ONLY)
					{
						int num38 = num17;
						this.silk_mode.maxInternalSampleRate = 16000;
						if (num14 > 50)
						{
							num38 = num38 * 2 / 3;
						}
						if (num38 < 13000)
						{
							this.silk_mode.maxInternalSampleRate = 12000;
							this.silk_mode.desiredInternalSampleRate = Inlines.IMIN(12000, this.silk_mode.desiredInternalSampleRate);
						}
						if (num38 < 9600)
						{
							this.silk_mode.maxInternalSampleRate = 8000;
							this.silk_mode.desiredInternalSampleRate = Inlines.IMIN(8000, this.silk_mode.desiredInternalSampleRate);
						}
					}
					else
					{
						this.silk_mode.maxInternalSampleRate = 16000;
					}
					this.silk_mode.useCBR = ((this.use_vbr == 0) ? 1 : 0);
					int num39 = Inlines.IMIN(1275, num9 - 1 - num4);
					this.silk_mode.maxBits = num39 * 8;
					if (this.mode == OpusMode.MODE_HYBRID)
					{
						this.silk_mode.maxBits = this.silk_mode.maxBits * 9 / 10;
					}
					if (this.silk_mode.useCBR != 0)
					{
						this.silk_mode.maxBits = this.silk_mode.bitRate * frame_size / (this.Fs * 8) * 8;
						this.silk_mode.bitRate = Inlines.IMAX(1, this.silk_mode.bitRate - 2000);
					}
					if (num2 != 0)
					{
						BoxedValueInt nBytesOut = new BoxedValueInt(0);
						int num40 = this.channels * (this.encoder_buffer - this.delay_compensation - this.Fs / 400);
						CodecHelpers.gain_fade(this.delay_buffer, num40, 0, 32767, celtMode.overlap, this.Fs / 400, this.channels, celtMode.window, this.Fs);
						Arrays.MemSetShort(this.delay_buffer, 0, num40);
						Arrays.MemCopy(this.delay_buffer, 0, array5, 0, this.encoder_buffer * this.channels);
						EncodeAPI.silk_Encode(silkEncoder, this.silk_mode, array5, this.encoder_buffer, null, data.Slice(data_ptr), nBytesOut, 1);
					}
					Arrays.MemCopy(array4, num13 * this.channels, array5, 0, frame_size * this.channels);
					BoxedValueInt boxedValueInt = new BoxedValueInt(num39);
					num = EncodeAPI.silk_Encode(silkEncoder, this.silk_mode, array5, frame_size, entropyCoder, data.Slice(data_ptr), boxedValueInt, 0);
					num39 = boxedValueInt.Val;
					if (num != 0)
					{
						return -3;
					}
					if (num39 == 0)
					{
						this.rangeFinal = 0U;
						*data[data_ptr - 1] = CodecHelpers.gen_toc(this.mode, this.Fs / frame_size, opusBandwidth3, this.stream_channels);
						return 1;
					}
					if (this.mode == OpusMode.MODE_SILK_ONLY)
					{
						if (this.silk_mode.internalSampleRate == 8000)
						{
							opusBandwidth3 = OpusBandwidth.OPUS_BANDWIDTH_NARROWBAND;
						}
						else if (this.silk_mode.internalSampleRate == 12000)
						{
							opusBandwidth3 = OpusBandwidth.OPUS_BANDWIDTH_MEDIUMBAND;
						}
						else if (this.silk_mode.internalSampleRate == 16000)
						{
							opusBandwidth3 = OpusBandwidth.OPUS_BANDWIDTH_WIDEBAND;
						}
					}
					this.silk_mode.opusCanSwitch = this.silk_mode.switchReady;
					if (this.silk_mode.opusCanSwitch != 0)
					{
						num3 = 1;
						num5 = 0;
						this.silk_bw_switch = 1;
					}
				}
				int endBand = 21;
				switch (opusBandwidth3)
				{
				case OpusBandwidth.OPUS_BANDWIDTH_NARROWBAND:
					endBand = 13;
					break;
				case OpusBandwidth.OPUS_BANDWIDTH_MEDIUMBAND:
				case OpusBandwidth.OPUS_BANDWIDTH_WIDEBAND:
					endBand = 17;
					break;
				case OpusBandwidth.OPUS_BANDWIDTH_SUPERWIDEBAND:
					endBand = 19;
					break;
				case OpusBandwidth.OPUS_BANDWIDTH_FULLBAND:
					endBand = 21;
					break;
				}
				celt_Encoder.SetEndBand(endBand);
				celt_Encoder.SetChannels(this.stream_channels);
				celt_Encoder.SetBitrate(-1);
				int num42;
				if (this.mode != OpusMode.MODE_SILK_ONLY)
				{
					int prediction = 2;
					celt_Encoder.SetVBR(false);
					if (this.silk_mode.reducedDependency != 0)
					{
						prediction = 0;
					}
					celt_Encoder.SetPrediction(prediction);
					if (this.mode == OpusMode.MODE_HYBRID)
					{
						int num41 = entropyCoder.tell() + 7 >> 3;
						if (num3 != 0)
						{
							num41 += ((this.mode == OpusMode.MODE_HYBRID) ? 3 : 1);
						}
						if (this.use_vbr != 0)
						{
							num42 = num41 + num27 - this.silk_mode.bitRate * frame_size / (8 * this.Fs);
						}
						else
						{
							num42 = ((num41 > num27) ? num41 : num27);
						}
					}
					else if (this.use_vbr != 0)
					{
						int num43 = 0;
						if (this.analysis.enabled && this.variable_duration == OpusFramesize.OPUS_FRAMESIZE_VARIABLE && frame_size != this.Fs / 50)
						{
							num43 = (60 * this.stream_channels + 40) * (this.Fs / frame_size - 50);
							if (analysisInfo.valid != 0)
							{
								num43 = (int)((float)num43 * (1f + 0.5f * analysisInfo.tonality));
							}
						}
						celt_Encoder.SetVBR(true);
						celt_Encoder.SetVBRConstraint(this.vbr_constraint != 0);
						celt_Encoder.SetBitrate(this.bitrate_bps + num43);
						num42 = num9 - 1 - num4;
					}
					else
					{
						num42 = num27;
					}
				}
				else
				{
					num42 = 0;
				}
				short[] array6 = new short[this.channels * this.Fs / 400];
				if (this.mode != OpusMode.MODE_SILK_ONLY && this.mode != this.prev_mode && this.prev_mode > (OpusMode)0)
				{
					Arrays.MemCopy(this.delay_buffer, (this.encoder_buffer - num13 - this.Fs / 400) * this.channels, array6, 0, this.channels * this.Fs / 400);
				}
				if (this.channels * (this.encoder_buffer - (frame_size + num13)) > 0)
				{
					Arrays.MemMoveShort(this.delay_buffer, this.channels * frame_size, 0, this.channels * (this.encoder_buffer - frame_size - num13));
					Arrays.MemCopy(array4, 0, this.delay_buffer, this.channels * (this.encoder_buffer - frame_size - num13), (frame_size + num13) * this.channels);
				}
				else
				{
					Arrays.MemCopy(array4, (frame_size + num13 - this.encoder_buffer) * this.channels, this.delay_buffer, 0, this.encoder_buffer * this.channels);
				}
				if (this.prev_HB_gain < 32767 || num29 < 32767)
				{
					CodecHelpers.gain_fade(array4, 0, this.prev_HB_gain, num29, celtMode.overlap, frame_size, this.channels, celtMode.window, this.Fs);
				}
				this.prev_HB_gain = num29;
				if (this.mode != OpusMode.MODE_HYBRID || this.stream_channels == 1)
				{
					this.silk_mode.stereoWidth_Q14 = Inlines.IMIN(16384, 2 * Inlines.IMAX(0, num18 - 30000));
				}
				if (this.energy_masking == null && this.channels == 2 && (this.hybrid_stereo_width_Q14 < 16384 || this.silk_mode.stereoWidth_Q14 < 16384))
				{
					int num44 = (int)this.hybrid_stereo_width_Q14;
					int num45 = this.silk_mode.stereoWidth_Q14;
					num44 = ((num44 == 16384) ? 32767 : Inlines.SHL16(num44, 1));
					num45 = ((num45 == 16384) ? 32767 : Inlines.SHL16(num45, 1));
					CodecHelpers.stereo_fade(array4, num44, num45, celtMode.overlap, frame_size, this.channels, celtMode.window, this.Fs);
					this.hybrid_stereo_width_Q14 = (short)this.silk_mode.stereoWidth_Q14;
				}
				if (this.mode != OpusMode.MODE_CELT_ONLY && entropyCoder.tell() + 17 + 20 * ((this.mode == OpusMode.MODE_HYBRID) ? 1 : 0) <= 8 * (num9 - 1))
				{
					if (this.mode == OpusMode.MODE_HYBRID && (num3 != 0 || entropyCoder.tell() + 37 <= 8 * num42))
					{
						entropyCoder.enc_bit_logp(data.Slice(data_ptr), num3, 12U);
					}
					if (num3 != 0)
					{
						entropyCoder.enc_bit_logp(data.Slice(data_ptr), num5, 1U);
						int a2;
						if (this.mode == OpusMode.MODE_HYBRID)
						{
							a2 = num9 - 1 - num42;
						}
						else
						{
							a2 = num9 - 1 - (entropyCoder.tell() + 7 >> 3);
						}
						num4 = Inlines.IMIN(a2, this.bitrate_bps / 1600);
						num4 = Inlines.IMIN(257, Inlines.IMAX(2, num4));
						if (this.mode == OpusMode.MODE_HYBRID)
						{
							entropyCoder.enc_uint(data.Slice(data_ptr), (uint)(num4 - 2), 256U);
						}
					}
				}
				else
				{
					num3 = 0;
				}
				if (num3 == 0)
				{
					this.silk_bw_switch = 0;
					num4 = 0;
				}
				if (this.mode != OpusMode.MODE_CELT_ONLY)
				{
					startBand = 17;
				}
				if (this.mode == OpusMode.MODE_SILK_ONLY)
				{
					num = entropyCoder.tell() + 7 >> 3;
					entropyCoder.enc_done(data.Slice(data_ptr));
					num42 = num;
				}
				else
				{
					num42 = Inlines.IMIN(num9 - 1 - num4, num42);
					entropyCoder.enc_shrink(data.Slice(data_ptr), (uint)num42);
				}
				if ((this.analysis.enabled && num3 != 0) || this.mode != OpusMode.MODE_SILK_ONLY)
				{
					celt_Encoder.SetAnalysis(analysisInfo);
				}
				if (num3 != 0 && num5 != 0)
				{
					celt_Encoder.SetStartBand(0);
					celt_Encoder.SetVBR(false);
					if (celt_Encoder.celt_encode_with_ec(array4, 0, this.Fs / 200, data, data_ptr + num42, num4, null) < 0)
					{
						return -3;
					}
					num7 = celt_Encoder.GetFinalRange();
					celt_Encoder.ResetState();
				}
				celt_Encoder.SetStartBand(startBand);
				if (this.mode != OpusMode.MODE_SILK_ONLY)
				{
					if (this.mode != this.prev_mode && this.prev_mode > (OpusMode)0)
					{
						Span<byte> compressed = new Span<byte>(stackalloc byte[(UIntPtr)2], 2);
						celt_Encoder.ResetState();
						celt_Encoder.celt_encode_with_ec(array6, 0, this.Fs / 400, compressed, 0, 2, null);
						celt_Encoder.SetPrediction(0);
					}
					if (entropyCoder.tell() <= 8 * num42)
					{
						num = celt_Encoder.celt_encode_with_ec(array4, 0, frame_size, data.Slice(data_ptr), 0, num42, entropyCoder);
						if (num < 0)
						{
							return -3;
						}
					}
				}
				if (num3 != 0 && num5 == 0)
				{
					Span<byte> compressed2 = new Span<byte>(stackalloc byte[(UIntPtr)2], 2);
					int num46 = this.Fs / 200;
					int num47 = this.Fs / 400;
					celt_Encoder.ResetState();
					celt_Encoder.SetStartBand(0);
					celt_Encoder.SetPrediction(0);
					celt_Encoder.celt_encode_with_ec(array4, this.channels * (frame_size - num46 - num47), num47, compressed2, 0, 2, null);
					if (celt_Encoder.celt_encode_with_ec(array4, this.channels * (frame_size - num46), num46, data, data_ptr + num42, num4, null) < 0)
					{
						return -3;
					}
					num7 = celt_Encoder.GetFinalRange();
				}
				data_ptr--;
				*data[data_ptr] = CodecHelpers.gen_toc(this.mode, this.Fs / frame_size, opusBandwidth3, this.stream_channels);
				this.rangeFinal = (entropyCoder.rng ^ num7);
				if (num6 != 0)
				{
					this.prev_mode = OpusMode.MODE_CELT_ONLY;
				}
				else
				{
					this.prev_mode = this.mode;
				}
				this.prev_channels = this.stream_channels;
				this.prev_framesize = frame_size;
				this.first = 0;
				if (entropyCoder.tell() > (num9 - 1) * 8)
				{
					if (num9 < 2)
					{
						return -2;
					}
					*data[data_ptr + 1] = 0;
					num = 1;
					this.rangeFinal = 0U;
				}
				else if (this.mode == OpusMode.MODE_SILK_ONLY && num3 == 0)
				{
					while (num > 2 && *data[data_ptr + num] == 0)
					{
						num--;
					}
				}
				num += 1 + num4;
				if (this.use_vbr == 0)
				{
					if (OpusRepacketizer.PadPacket(data, data_ptr, num, num9) != 0)
					{
						return -3;
					}
					num = num9;
				}
				return num;
			}
			if (this.analysis.enabled && num8 != -1)
			{
				this.analysis.read_pos = num8;
				this.analysis.read_subframe = read_subframe;
			}
			int num48 = (frame_size > this.Fs / 25) ? 3 : 2;
			int num49 = Inlines.IMIN(1276, (out_data_bytes - 3) / num48);
			byte[] array7 = new byte[num48 * num49];
			OpusRepacketizer opusRepacketizer = new OpusRepacketizer();
			OpusMode opusMode2 = this.user_forced_mode;
			OpusBandwidth opusBandwidth4 = this.user_bandwidth;
			int num50 = this.force_channels;
			this.user_forced_mode = this.mode;
			this.user_bandwidth = this.bandwidth;
			this.force_channels = this.stream_channels;
			int toMono = this.silk_mode.toMono;
			if (toMono != 0)
			{
				this.force_channels = 1;
			}
			else
			{
				this.prev_channels = this.stream_channels;
			}
			for (int i = 0; i < num48; i++)
			{
				this.silk_mode.toMono = 0;
				if (num6 != 0 && i == num48 - 1)
				{
					this.user_forced_mode = OpusMode.MODE_CELT_ONLY;
				}
				int num51 = this.opus_encode_native<T>(pcm, pcm_ptr + i * (this.channels * this.Fs / 50), this.Fs / 50, array7, i * num49, num49, lsb_depth, null, 0, c1, c2, analysis_channels, downmix, float_api);
				if (num51 < 0)
				{
					return -3;
				}
				num = opusRepacketizer.AddPacket(array7, i * num49, num51);
				if (num < 0)
				{
					return -3;
				}
			}
			int maxlen;
			if (this.use_vbr != 0)
			{
				maxlen = out_data_bytes;
			}
			else
			{
				maxlen = Inlines.IMIN(3 * this.bitrate_bps / (1200 / num48), out_data_bytes);
			}
			num = opusRepacketizer.opus_repacketizer_out_range_impl(0, num48, data, data_ptr, maxlen, 0, (this.use_vbr == 0) ? 1 : 0);
			if (num < 0)
			{
				return -3;
			}
			this.user_forced_mode = opusMode2;
			this.user_bandwidth = opusBandwidth4;
			this.force_channels = num50;
			this.silk_mode.toMono = toMono;
			return num;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0001C148 File Offset: 0x0001A348
		private static int GetVariableQualityBitrate(int vqLevel, float music_prob)
		{
			float num = (float)OpusEncoder.vqTable[vqLevel][0];
			float num2 = (float)OpusEncoder.vqTable[vqLevel][1];
			float num3 = (float)Math.Sqrt((double)music_prob);
			return (int)(num2 * num3 + num * (1f - num3));
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0001C180 File Offset: 0x0001A380
		[Obsolete("Use Span<> overrides if possible")]
		public int Encode(short[] in_pcm, int pcm_offset, int frame_size, byte[] out_data, int out_data_offset, int max_data_bytes)
		{
			return this.Encode(MemoryExtensions.AsSpan<short>(in_pcm, pcm_offset), frame_size, MemoryExtensions.AsSpan<byte>(out_data, out_data_offset), max_data_bytes);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0001C1A0 File Offset: 0x0001A3A0
		public int Encode(ReadOnlySpan<short> in_pcm, int frame_size, Span<byte> out_data, int max_data_bytes)
		{
			if (max_data_bytes > out_data.Length)
			{
				throw new ArgumentException(string.Format("Output buffer is too small: Stated size is {0} bytes, actual size is {1} bytes", max_data_bytes, out_data.Length));
			}
			int num;
			if (this.application == OpusApplication.OPUS_APPLICATION_RESTRICTED_LOWDELAY)
			{
				num = 0;
			}
			else
			{
				num = this.delay_compensation;
			}
			ReadOnlySpan<short> analysis_pcm = in_pcm;
			OpusFramesize opusFramesize = this.variable_duration;
			int c = this.channels;
			int fs = this.Fs;
			int num2 = this.bitrate_bps;
			int num3 = num;
			Downmix.downmix_func<short> downmix;
			if ((downmix = OpusEncoder.<>O.<0>__downmix_int) == null)
			{
				downmix = (OpusEncoder.<>O.<0>__downmix_int = new Downmix.downmix_func<short>(Downmix.downmix_int));
			}
			int num4 = CodecHelpers.compute_frame_size<short>(analysis_pcm, frame_size, opusFramesize, c, fs, num2, num3, downmix, this.analysis.subframe_mem, this.analysis.enabled);
			if (num4 > in_pcm.Length)
			{
				throw new ArgumentException(string.Format("Not enough samples provided in input signal: Expected {0} samples, found {1}", num4, in_pcm.Length));
			}
			int result;
			try
			{
				ReadOnlySpan<short> pcm = in_pcm;
				int pcm_ptr = 0;
				int frame_size2 = num4;
				Span<byte> data = out_data;
				int data_ptr = 0;
				int num5 = 16;
				ReadOnlySpan<short> analysis_pcm2 = in_pcm;
				int c2 = 0;
				int c3 = -2;
				int analysis_channels = this.channels;
				Downmix.downmix_func<short> downmix2;
				if ((downmix2 = OpusEncoder.<>O.<0>__downmix_int) == null)
				{
					downmix2 = (OpusEncoder.<>O.<0>__downmix_int = new Downmix.downmix_func<short>(Downmix.downmix_int));
				}
				int num6 = this.opus_encode_native<short>(pcm, pcm_ptr, frame_size2, data, data_ptr, max_data_bytes, num5, analysis_pcm2, frame_size, c2, c3, analysis_channels, downmix2, 0);
				if (num6 < 0)
				{
					if (num6 == -1)
					{
						throw new ArgumentException("OPUS_BAD_ARG while encoding");
					}
					throw new OpusException("An error occurred during encoding: " + CodecHelpers.opus_strerror(num6), num6);
				}
				else
				{
					result = num6;
				}
			}
			catch (ArgumentException ex)
			{
				throw new OpusException("public error during encoding: " + ex.Message, -1);
			}
			return result;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0001C304 File Offset: 0x0001A504
		[Obsolete("Use Span<> overrides if possible")]
		public int Encode(float[] in_pcm, int in_pcm_offset, int frame_size, byte[] out_data, int out_data_offset, int max_data_bytes)
		{
			return this.Encode(MemoryExtensions.AsSpan<float>(in_pcm, in_pcm_offset), frame_size, MemoryExtensions.AsSpan<byte>(out_data, out_data_offset), max_data_bytes);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0001C324 File Offset: 0x0001A524
		public unsafe int Encode(ReadOnlySpan<float> in_pcm, int frame_size, Span<byte> out_data, int max_data_bytes)
		{
			if (max_data_bytes > out_data.Length)
			{
				throw new ArgumentException(string.Format("Output buffer is too small: Stated size is {0} bytes, actual size is {1} bytes", max_data_bytes, out_data.Length));
			}
			int num;
			if (this.application == OpusApplication.OPUS_APPLICATION_RESTRICTED_LOWDELAY)
			{
				num = 0;
			}
			else
			{
				num = this.delay_compensation;
			}
			ReadOnlySpan<float> analysis_pcm = in_pcm;
			OpusFramesize opusFramesize = this.variable_duration;
			int c = this.channels;
			int fs = this.Fs;
			int num2 = this.bitrate_bps;
			int num3 = num;
			Downmix.downmix_func<float> downmix;
			if ((downmix = OpusEncoder.<>O.<1>__downmix_float) == null)
			{
				downmix = (OpusEncoder.<>O.<1>__downmix_float = new Downmix.downmix_func<float>(Downmix.downmix_float));
			}
			int num4 = CodecHelpers.compute_frame_size<float>(analysis_pcm, frame_size, opusFramesize, c, fs, num2, num3, downmix, this.analysis.subframe_mem, this.analysis.enabled);
			if (num4 > in_pcm.Length)
			{
				throw new ArgumentException(string.Format("Not enough samples provided in input signal: Expected {0} samples, found {1}", num4, in_pcm.Length));
			}
			short[] array = new short[num4 * this.channels];
			for (int i = 0; i < num4 * this.channels; i++)
			{
				array[i] = Inlines.FLOAT2INT16(*in_pcm[i]);
			}
			int result;
			try
			{
				ReadOnlySpan<short> pcm = array;
				int pcm_ptr = 0;
				int frame_size2 = num4;
				Span<byte> data = out_data;
				int data_ptr = 0;
				int num5 = 16;
				ReadOnlySpan<float> analysis_pcm2 = in_pcm;
				int c2 = 0;
				int c3 = -2;
				int analysis_channels = this.channels;
				Downmix.downmix_func<float> downmix2;
				if ((downmix2 = OpusEncoder.<>O.<1>__downmix_float) == null)
				{
					downmix2 = (OpusEncoder.<>O.<1>__downmix_float = new Downmix.downmix_func<float>(Downmix.downmix_float));
				}
				int num6 = this.opus_encode_native<float>(pcm, pcm_ptr, frame_size2, data, data_ptr, max_data_bytes, num5, analysis_pcm2, frame_size, c2, c3, analysis_channels, downmix2, 1);
				if (num6 < 0)
				{
					if (num6 == -1)
					{
						throw new ArgumentException("OPUS_BAD_ARG while decoding");
					}
					throw new OpusException("An error occurred during encoding: " + CodecHelpers.opus_strerror(num6), num6);
				}
				else
				{
					result = num6;
				}
			}
			catch (ArgumentException ex)
			{
				throw new OpusException("public error during encoding: " + ex.Message, -1);
			}
			return result;
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600019F RID: 415 RVA: 0x0001C4C4 File Offset: 0x0001A6C4
		// (set) Token: 0x060001A0 RID: 416 RVA: 0x0001C4CC File Offset: 0x0001A6CC
		public OpusApplication Application
		{
			get
			{
				return this.application;
			}
			set
			{
				if (this.first == 0 && this.application != value)
				{
					throw new ArgumentException("Application cannot be changed after encoding has started");
				}
				this.application = value;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x060001A1 RID: 417 RVA: 0x0001C4F1 File Offset: 0x0001A6F1
		// (set) Token: 0x060001A2 RID: 418 RVA: 0x0001C50C File Offset: 0x0001A70C
		public int Bitrate
		{
			get
			{
				return this.user_bitrate_to_bitrate(this.user_bitrate_bps, this.prev_framesize, 1276);
			}
			set
			{
				if (this.ConstantQuality != null)
				{
					throw new ArgumentException("Bitrate is read-only while the ConstantQuality parameter is set");
				}
				if (value != -1000 && value != -1)
				{
					if (value <= 0)
					{
						throw new ArgumentException("Bitrate must be positive");
					}
					if (value <= 500)
					{
						value = 500;
					}
					else if (value > 300000 * this.channels)
					{
						value = 300000 * this.channels;
					}
				}
				this.user_bitrate_bps = value;
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x060001A3 RID: 419 RVA: 0x0001C584 File Offset: 0x0001A784
		// (set) Token: 0x060001A4 RID: 420 RVA: 0x0001C58C File Offset: 0x0001A78C
		public int ForceChannels
		{
			get
			{
				return this.force_channels;
			}
			set
			{
				if ((value < 1 || value > this.channels) && value != -1000)
				{
					throw new ArgumentException("Force channels must be <= num. of channels");
				}
				this.force_channels = value;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x0001C5B5 File Offset: 0x0001A7B5
		// (set) Token: 0x060001A6 RID: 422 RVA: 0x0001C5C0 File Offset: 0x0001A7C0
		public OpusBandwidth MaxBandwidth
		{
			get
			{
				return this.max_bandwidth;
			}
			set
			{
				this.max_bandwidth = value;
				if (this.max_bandwidth == OpusBandwidth.OPUS_BANDWIDTH_NARROWBAND)
				{
					this.silk_mode.maxInternalSampleRate = 8000;
					return;
				}
				if (this.max_bandwidth == OpusBandwidth.OPUS_BANDWIDTH_MEDIUMBAND)
				{
					this.silk_mode.maxInternalSampleRate = 12000;
					return;
				}
				this.silk_mode.maxInternalSampleRate = 16000;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x0001C620 File Offset: 0x0001A820
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x0001C628 File Offset: 0x0001A828
		public OpusBandwidth Bandwidth
		{
			get
			{
				return this.bandwidth;
			}
			set
			{
				this.user_bandwidth = value;
				if (this.user_bandwidth == OpusBandwidth.OPUS_BANDWIDTH_NARROWBAND)
				{
					this.silk_mode.maxInternalSampleRate = 8000;
					return;
				}
				if (this.user_bandwidth == OpusBandwidth.OPUS_BANDWIDTH_MEDIUMBAND)
				{
					this.silk_mode.maxInternalSampleRate = 12000;
					return;
				}
				this.silk_mode.maxInternalSampleRate = 16000;
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x060001A9 RID: 425 RVA: 0x0001C688 File Offset: 0x0001A888
		// (set) Token: 0x060001AA RID: 426 RVA: 0x0001C698 File Offset: 0x0001A898
		public bool UseDTX
		{
			get
			{
				return this.silk_mode.useDTX != 0;
			}
			set
			{
				this.silk_mode.useDTX = ((value > false) ? 1 : 0);
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060001AB RID: 427 RVA: 0x0001C6A9 File Offset: 0x0001A8A9
		// (set) Token: 0x060001AC RID: 428 RVA: 0x0001C6B6 File Offset: 0x0001A8B6
		public int Complexity
		{
			get
			{
				return this.silk_mode.complexity;
			}
			set
			{
				if (value < 0 || value > 10)
				{
					throw new ArgumentException("Complexity must be between 0 and 10");
				}
				this.silk_mode.complexity = value;
				this.Celt_Encoder.SetComplexity(value);
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x060001AD RID: 429 RVA: 0x0001C6E4 File Offset: 0x0001A8E4
		// (set) Token: 0x060001AE RID: 430 RVA: 0x0001C6F4 File Offset: 0x0001A8F4
		public bool UseInbandFEC
		{
			get
			{
				return this.silk_mode.useInBandFEC != 0;
			}
			set
			{
				this.silk_mode.useInBandFEC = ((value > false) ? 1 : 0);
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x060001AF RID: 431 RVA: 0x0001C705 File Offset: 0x0001A905
		// (set) Token: 0x060001B0 RID: 432 RVA: 0x0001C712 File Offset: 0x0001A912
		public int PacketLossPercent
		{
			get
			{
				return this.silk_mode.packetLossPercentage;
			}
			set
			{
				if (value < 0 || value > 100)
				{
					throw new ArgumentException("Packet loss must be between 0 and 100");
				}
				this.silk_mode.packetLossPercentage = value;
				this.Celt_Encoder.SetPacketLossPercent(value);
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001B1 RID: 433 RVA: 0x0001C740 File Offset: 0x0001A940
		// (set) Token: 0x060001B2 RID: 434 RVA: 0x0001C74B File Offset: 0x0001A94B
		public bool UseVBR
		{
			get
			{
				return this.use_vbr != 0;
			}
			set
			{
				this.use_vbr = ((value > false) ? 1 : 0);
				this.silk_mode.useCBR = ((!value) ? 1 : 0);
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x0001C766 File Offset: 0x0001A966
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x0001C771 File Offset: 0x0001A971
		public bool UseConstrainedVBR
		{
			get
			{
				return this.vbr_constraint != 0;
			}
			set
			{
				this.vbr_constraint = ((value > false) ? 1 : 0);
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001B5 RID: 437 RVA: 0x0001C77D File Offset: 0x0001A97D
		// (set) Token: 0x060001B6 RID: 438 RVA: 0x0001C785 File Offset: 0x0001A985
		public OpusSignal SignalType
		{
			get
			{
				return this.signal_type;
			}
			set
			{
				this.signal_type = value;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x0001C790 File Offset: 0x0001A990
		public int Lookahead
		{
			get
			{
				int num = this.Fs / 400;
				if (this.application != OpusApplication.OPUS_APPLICATION_RESTRICTED_LOWDELAY)
				{
					num += this.delay_compensation;
				}
				return num;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x0001C7C1 File Offset: 0x0001A9C1
		public int SampleRate
		{
			get
			{
				return this.Fs;
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001B9 RID: 441 RVA: 0x0001C7C9 File Offset: 0x0001A9C9
		public int NumChannels
		{
			get
			{
				return this.channels;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001BA RID: 442 RVA: 0x0001C7D1 File Offset: 0x0001A9D1
		public uint FinalRange
		{
			get
			{
				return this.rangeFinal;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001BB RID: 443 RVA: 0x0001C7D9 File Offset: 0x0001A9D9
		// (set) Token: 0x060001BC RID: 444 RVA: 0x0001C7E1 File Offset: 0x0001A9E1
		public int LSBDepth
		{
			get
			{
				return this.lsb_depth;
			}
			set
			{
				if (value < 8 || value > 24)
				{
					throw new ArgumentException("LSB depth must be between 8 and 24");
				}
				this.lsb_depth = value;
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001BD RID: 445 RVA: 0x0001C7FE File Offset: 0x0001A9FE
		// (set) Token: 0x060001BE RID: 446 RVA: 0x0001C806 File Offset: 0x0001AA06
		public OpusFramesize ExpertFrameDuration
		{
			get
			{
				return this.variable_duration;
			}
			set
			{
				this.variable_duration = value;
				this.Celt_Encoder.SetExpertFrameDuration(value);
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001BF RID: 447 RVA: 0x0001C81B File Offset: 0x0001AA1B
		// (set) Token: 0x060001C0 RID: 448 RVA: 0x0001C823 File Offset: 0x0001AA23
		public OpusMode ForceMode
		{
			get
			{
				return this.user_forced_mode;
			}
			set
			{
				this.user_forced_mode = value;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x0001C82C File Offset: 0x0001AA2C
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x0001C837 File Offset: 0x0001AA37
		public bool IsLFE
		{
			get
			{
				return this.lfe != 0;
			}
			set
			{
				this.lfe = ((value > false) ? 1 : 0);
				this.Celt_Encoder.SetLFE((value > false) ? 1 : 0);
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x0001C852 File Offset: 0x0001AA52
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x0001C862 File Offset: 0x0001AA62
		public bool PredictionDisabled
		{
			get
			{
				return this.silk_mode.reducedDependency != 0;
			}
			set
			{
				this.silk_mode.reducedDependency = ((value > false) ? 1 : 0);
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x0001C873 File Offset: 0x0001AA73
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x0001C880 File Offset: 0x0001AA80
		public bool EnableAnalysis
		{
			get
			{
				return this.analysis.enabled;
			}
			set
			{
				if (!value && this._vqLevel != null)
				{
					throw new ArgumentException("You cannot disable analysis while also specifying a ConstantQuality parameter");
				}
				if (value && this.Fs != 48000)
				{
					throw new ArgumentException("EnableAnalysis only works if the encoder is in 48000Khz mode");
				}
				this.analysis.enabled = value;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x0001C8CF File Offset: 0x0001AACF
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x0001C8D8 File Offset: 0x0001AAD8
		public int? ConstantQuality
		{
			get
			{
				return this._vqLevel;
			}
			set
			{
				if (value != null && (value.Value < 0 || value.Value > 10))
				{
					throw new ArgumentException("Constant quality VBR level must be either null (disabled) or between 0 and 10, inclusive.");
				}
				if (value != null && this.Fs != 48000)
				{
					throw new ArgumentException("ConstantQuality only works if the encoder is in 48000Khz mode");
				}
				this.EnableAnalysis = true;
				this._vqLevel = value;
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x0001C93D File Offset: 0x0001AB3D
		public float MusicProbability
		{
			get
			{
				return this.analysis.music_prob;
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0001C94A File Offset: 0x0001AB4A
		internal void SetEnergyMask(int[] value)
		{
			this.energy_masking = value;
			this.Celt_Encoder.SetEnergyMask(value);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0001C95F File Offset: 0x0001AB5F
		internal CeltMode GetCeltMode()
		{
			return this.Celt_Encoder.GetMode();
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0001C96C File Offset: 0x0001AB6C
		public string GetVersionString()
		{
			return CodecHelpers.GetVersionString();
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0001C973 File Offset: 0x0001AB73
		public void Dispose()
		{
		}

		// Token: 0x040002C9 RID: 713
		internal readonly EncControlState silk_mode = new EncControlState();

		// Token: 0x040002CA RID: 714
		internal OpusApplication application;

		// Token: 0x040002CB RID: 715
		internal int channels;

		// Token: 0x040002CC RID: 716
		internal int delay_compensation;

		// Token: 0x040002CD RID: 717
		internal int force_channels;

		// Token: 0x040002CE RID: 718
		internal OpusSignal signal_type;

		// Token: 0x040002CF RID: 719
		internal OpusBandwidth user_bandwidth;

		// Token: 0x040002D0 RID: 720
		internal OpusBandwidth max_bandwidth;

		// Token: 0x040002D1 RID: 721
		internal OpusMode user_forced_mode;

		// Token: 0x040002D2 RID: 722
		internal int voice_ratio;

		// Token: 0x040002D3 RID: 723
		internal int Fs;

		// Token: 0x040002D4 RID: 724
		internal int use_vbr;

		// Token: 0x040002D5 RID: 725
		internal int vbr_constraint;

		// Token: 0x040002D6 RID: 726
		internal OpusFramesize variable_duration;

		// Token: 0x040002D7 RID: 727
		internal int bitrate_bps;

		// Token: 0x040002D8 RID: 728
		internal int user_bitrate_bps;

		// Token: 0x040002D9 RID: 729
		internal int lsb_depth;

		// Token: 0x040002DA RID: 730
		internal int encoder_buffer;

		// Token: 0x040002DB RID: 731
		internal int lfe;

		// Token: 0x040002DC RID: 732
		internal readonly TonalityAnalysisState analysis = new TonalityAnalysisState();

		// Token: 0x040002DD RID: 733
		internal int stream_channels;

		// Token: 0x040002DE RID: 734
		internal short hybrid_stereo_width_Q14;

		// Token: 0x040002DF RID: 735
		internal int variable_HP_smth2_Q15;

		// Token: 0x040002E0 RID: 736
		internal int prev_HB_gain;

		// Token: 0x040002E1 RID: 737
		internal readonly int[] hp_mem = new int[4];

		// Token: 0x040002E2 RID: 738
		internal OpusMode mode;

		// Token: 0x040002E3 RID: 739
		internal OpusMode prev_mode;

		// Token: 0x040002E4 RID: 740
		internal int prev_channels;

		// Token: 0x040002E5 RID: 741
		internal int prev_framesize;

		// Token: 0x040002E6 RID: 742
		internal OpusBandwidth bandwidth;

		// Token: 0x040002E7 RID: 743
		internal int silk_bw_switch;

		// Token: 0x040002E8 RID: 744
		internal int first;

		// Token: 0x040002E9 RID: 745
		internal int[] energy_masking;

		// Token: 0x040002EA RID: 746
		internal readonly StereoWidthState width_mem = new StereoWidthState();

		// Token: 0x040002EB RID: 747
		internal readonly short[] delay_buffer = new short[960];

		// Token: 0x040002EC RID: 748
		internal OpusBandwidth detected_bandwidth;

		// Token: 0x040002ED RID: 749
		internal uint rangeFinal;

		// Token: 0x040002EE RID: 750
		private int? _vqLevel;

		// Token: 0x040002EF RID: 751
		internal readonly SilkEncoder SilkEncoder = new SilkEncoder();

		// Token: 0x040002F0 RID: 752
		internal readonly CeltEncoder Celt_Encoder = new CeltEncoder();

		// Token: 0x040002F1 RID: 753
		private static readonly int[][] vqTable = new int[][]
		{
			new int[]
			{
				7000,
				16000
			},
			new int[]
			{
				10000,
				24000
			},
			new int[]
			{
				13000,
				32000
			},
			new int[]
			{
				17000,
				48000
			},
			new int[]
			{
				20000,
				64000
			},
			new int[]
			{
				24000,
				80000
			},
			new int[]
			{
				28000,
				96000
			},
			new int[]
			{
				32000,
				112000
			},
			new int[]
			{
				38000,
				128000
			},
			new int[]
			{
				48000,
				192000
			},
			new int[]
			{
				64000,
				256000
			}
		};

		// Token: 0x0200009F RID: 159
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040005EB RID: 1515
			public static Downmix.downmix_func<short> <0>__downmix_int;

			// Token: 0x040005EC RID: 1516
			public static Downmix.downmix_func<float> <1>__downmix_float;
		}
	}
}
