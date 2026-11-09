using System;
using Concentus.Celt.Structs;
using Concentus.Common;
using Concentus.Common.CPlusPlus;
using Concentus.Enums;
using Concentus.Silk;
using Concentus.Silk.Structs;

namespace Concentus.Structs
{
	// Token: 0x0200005A RID: 90
	public class OpusDecoder : IOpusDecoder, IDisposable
	{
		// Token: 0x0600017B RID: 379 RVA: 0x00018DAD File Offset: 0x00016FAD
		internal OpusDecoder()
		{
		}

		// Token: 0x0600017C RID: 380 RVA: 0x00018DD6 File Offset: 0x00016FD6
		internal void Reset()
		{
			this.channels = 0;
			this.Fs = 0;
			this.DecControl.Reset();
			this.decode_gain = 0;
			this.PartialReset();
		}

		// Token: 0x0600017D RID: 381 RVA: 0x00018DFE File Offset: 0x00016FFE
		internal void PartialReset()
		{
			this.stream_channels = 0;
			this.bandwidth = (OpusBandwidth)0;
			this.mode = (OpusMode)0;
			this.prev_mode = (OpusMode)0;
			this.frame_size = 0;
			this.prev_redundancy = 0;
			this.last_packet_duration = 0;
			this.rangeFinal = 0U;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x00018E38 File Offset: 0x00017038
		[Obsolete("Use OpusCodecFactory methods which can give you native code if supported by your platform")]
		public OpusDecoder(int Fs, int channels)
		{
			if (Fs != 48000 && Fs != 24000 && Fs != 16000 && Fs != 12000 && Fs != 8000)
			{
				throw new ArgumentException("Sample rate is invalid (must be 8/12/16/24/48 Khz)");
			}
			if (channels != 1 && channels != 2)
			{
				throw new ArgumentException("Number of channels must be 1 or 2");
			}
			int num = this.opus_decoder_init(Fs, channels);
			if (num == 0)
			{
				return;
			}
			if (num == -1)
			{
				throw new ArgumentException("OPUS_BAD_ARG when creating decoder");
			}
			throw new OpusException("Error while initializing decoder: " + CodecHelpers.opus_strerror(num), num);
		}

		// Token: 0x0600017F RID: 383 RVA: 0x00018EE4 File Offset: 0x000170E4
		internal int opus_decoder_init(int Fs, int channels)
		{
			if ((Fs != 48000 && Fs != 24000 && Fs != 16000 && Fs != 12000 && Fs != 8000) || (channels != 1 && channels != 2))
			{
				return -1;
			}
			this.Reset();
			SilkDecoder silkDecoder = this.SilkDecoder;
			CeltDecoder celt_Decoder = this.Celt_Decoder;
			this.channels = channels;
			this.stream_channels = channels;
			this.Fs = Fs;
			this.DecControl.API_sampleRate = this.Fs;
			this.DecControl.nChannelsAPI = this.channels;
			if (DecodeAPI.silk_InitDecoder(silkDecoder) != 0)
			{
				return -3;
			}
			if (celt_Decoder.celt_decoder_init(Fs, channels) != 0)
			{
				return -3;
			}
			celt_Decoder.SetSignalling(0);
			this.prev_mode = (OpusMode)0;
			this.frame_size = Fs / 400;
			return 0;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00018FA4 File Offset: 0x000171A4
		internal unsafe int opus_decode_frame(ReadOnlySpan<byte> data, int data_ptr, int len, Span<short> pcm, int pcm_ptr, int frame_size, int decode_fec)
		{
			int num = 0;
			EntropyCoder entropyCoder = new EntropyCoder();
			short[] array = null;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			uint num6 = 0U;
			SilkDecoder silkDecoder = this.SilkDecoder;
			CeltDecoder celt_Decoder = this.Celt_Decoder;
			int num7 = this.Fs / 50;
			int num8 = num7 >> 1;
			int num9 = num8 >> 1;
			int num10 = num9 >> 1;
			if (frame_size < num10)
			{
				return -2;
			}
			frame_size = Inlines.IMIN(frame_size, this.Fs / 25 * 3);
			if (len <= 1)
			{
				data = null;
				frame_size = Inlines.IMIN(frame_size, this.frame_size);
			}
			int num11;
			OpusMode opusMode;
			if (!data.IsEmpty)
			{
				num11 = this.frame_size;
				opusMode = this.mode;
				entropyCoder.dec_init(data.Slice(data_ptr), (uint)len);
			}
			else
			{
				num11 = frame_size;
				opusMode = this.prev_mode;
				if (opusMode == (OpusMode)0)
				{
					for (int i = pcm_ptr; i < pcm_ptr + num11 * this.channels; i++)
					{
						*pcm[i] = 0;
					}
					return num11;
				}
				if (num11 > num7)
				{
					int num12;
					for (;;)
					{
						num12 = this.opus_decode_frame(null, 0, 0, pcm, pcm_ptr, Inlines.IMIN(num11, num7), 0);
						if (num12 < 0)
						{
							break;
						}
						pcm_ptr += num12 * this.channels;
						num11 -= num12;
						if (num11 <= 0)
						{
							return frame_size;
						}
					}
					return num12;
				}
				if (num11 < num7)
				{
					if (num11 > num8)
					{
						num11 = num8;
					}
					else if (opusMode != OpusMode.MODE_SILK_ONLY && num11 > num9 && num11 < num8)
					{
						num11 = num9;
					}
				}
			}
			int num13 = (opusMode != OpusMode.MODE_CELT_ONLY && frame_size >= num8) ? 1 : 0;
			int num14 = 0;
			int num15 = 0;
			if (!data.IsEmpty && this.prev_mode > (OpusMode)0 && ((opusMode == OpusMode.MODE_CELT_ONLY && this.prev_mode != OpusMode.MODE_CELT_ONLY && this.prev_redundancy == 0) || (opusMode != OpusMode.MODE_CELT_ONLY && this.prev_mode == OpusMode.MODE_CELT_ONLY)))
			{
				num2 = 1;
				if (opusMode == OpusMode.MODE_CELT_ONLY)
				{
					num15 = num9 * this.channels;
				}
				else
				{
					num14 = num9 * this.channels;
				}
			}
			short[] array2 = new short[num15];
			if (num2 != 0 && opusMode == OpusMode.MODE_CELT_ONLY)
			{
				array = array2;
				this.opus_decode_frame(null, 0, 0, array, 0, Inlines.IMIN(num9, num11), 0);
			}
			if (num11 > frame_size)
			{
				return -1;
			}
			frame_size = num11;
			short[] array3 = new short[(opusMode != OpusMode.MODE_CELT_ONLY && num13 == 0) ? (Inlines.IMAX(num8, frame_size) * this.channels) : 0];
			if (opusMode != OpusMode.MODE_CELT_ONLY)
			{
				Span<short> span;
				int num16;
				if (num13 != 0)
				{
					span = pcm;
					num16 = pcm_ptr;
				}
				else
				{
					span = array3;
					num16 = 0;
				}
				if (this.prev_mode == OpusMode.MODE_CELT_ONLY)
				{
					DecodeAPI.silk_InitDecoder(silkDecoder);
				}
				this.DecControl.payloadSize_ms = Inlines.IMAX(10, 1000 * num11 / this.Fs);
				if (!data.IsEmpty)
				{
					this.DecControl.nChannelsInternal = this.stream_channels;
					if (opusMode == OpusMode.MODE_SILK_ONLY)
					{
						if (this.bandwidth == OpusBandwidth.OPUS_BANDWIDTH_NARROWBAND)
						{
							this.DecControl.internalSampleRate = 8000;
						}
						else if (this.bandwidth == OpusBandwidth.OPUS_BANDWIDTH_MEDIUMBAND)
						{
							this.DecControl.internalSampleRate = 12000;
						}
						else if (this.bandwidth == OpusBandwidth.OPUS_BANDWIDTH_WIDEBAND)
						{
							this.DecControl.internalSampleRate = 16000;
						}
						else
						{
							this.DecControl.internalSampleRate = 16000;
						}
					}
					else
					{
						this.DecControl.internalSampleRate = 16000;
					}
				}
				int num17 = data.IsEmpty ? 1 : (2 * decode_fec);
				int num18 = 0;
				for (;;)
				{
					int newPacketFlag = (num18 == 0) ? 1 : 0;
					int num19;
					if (DecodeAPI.silk_Decode(silkDecoder, this.DecControl, data.Slice(data_ptr), num17, newPacketFlag, entropyCoder, span, num16, out num19) != 0)
					{
						if (num17 == 0)
						{
							break;
						}
						num19 = frame_size;
						Arrays.MemSetWithOffset<short>(span, 0, num16, frame_size * this.channels);
					}
					num16 += num19 * this.channels;
					num18 += num19;
					if (num18 >= frame_size)
					{
						goto IL_3C4;
					}
				}
				return -3;
			}
			IL_3C4:
			int startBand = 0;
			if (decode_fec == 0 && opusMode != OpusMode.MODE_CELT_ONLY && !data.IsEmpty && entropyCoder.tell() + 17 + 20 * ((this.mode == OpusMode.MODE_HYBRID) ? 1 : 0) <= 8 * len)
			{
				if (opusMode == OpusMode.MODE_HYBRID)
				{
					num3 = entropyCoder.dec_bit_logp(data.Slice(data_ptr), 12U);
				}
				else
				{
					num3 = 1;
				}
				if (num3 != 0)
				{
					num5 = entropyCoder.dec_bit_logp(data.Slice(data_ptr), 1U);
					num4 = (int)((opusMode == OpusMode.MODE_HYBRID) ? (entropyCoder.dec_uint(data.Slice(data_ptr), 256U) + 2U) : ((uint)(len - (entropyCoder.tell() + 7 >> 3))));
					len -= num4;
					if (len * 8 < entropyCoder.tell())
					{
						len = 0;
						num4 = 0;
						num3 = 0;
					}
					entropyCoder.storage = (uint)((ulong)entropyCoder.storage - (ulong)((long)num4));
				}
			}
			if (opusMode != OpusMode.MODE_CELT_ONLY)
			{
				startBand = 17;
			}
			int endBand = 21;
			switch (this.bandwidth)
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
			celt_Decoder.SetEndBand(endBand);
			celt_Decoder.SetChannels(this.stream_channels);
			if (num3 != 0)
			{
				num2 = 0;
				num14 = 0;
			}
			short[] array4 = new short[num14];
			if (num2 != 0 && opusMode != OpusMode.MODE_CELT_ONLY)
			{
				array = array4;
				this.opus_decode_frame(null, 0, 0, array, 0, Inlines.IMIN(num9, num11), 0);
			}
			short[] array5 = new short[(num3 != 0) ? (num9 * this.channels) : 0];
			if (num3 != 0 && num5 != 0)
			{
				celt_Decoder.SetStartBand(0);
				celt_Decoder.celt_decode_with_ec(data, data_ptr + len, num4, array5, 0, num9, null, 0);
				num6 = celt_Decoder.GetFinalRange();
			}
			celt_Decoder.SetStartBand(startBand);
			if (opusMode != OpusMode.MODE_SILK_ONLY)
			{
				int num20 = Inlines.IMIN(num7, frame_size);
				if (opusMode != this.prev_mode && this.prev_mode > (OpusMode)0 && this.prev_redundancy == 0)
				{
					celt_Decoder.ResetState();
				}
				num = celt_Decoder.celt_decode_with_ec((decode_fec != 0) ? null : data, data_ptr, len, pcm, pcm_ptr, num20, entropyCoder, num13);
			}
			else
			{
				if (num13 == 0)
				{
					for (int i = pcm_ptr; i < frame_size * this.channels + pcm_ptr; i++)
					{
						*pcm[i] = 0;
					}
				}
				if (this.prev_mode == OpusMode.MODE_HYBRID && (num3 == 0 || num5 == 0 || this.prev_redundancy == 0))
				{
					celt_Decoder.SetStartBand(0);
					celt_Decoder.celt_decode_with_ec(OpusDecoder.SILENCE, 0, 2, pcm, pcm_ptr, num10, null, num13);
				}
			}
			if (opusMode != OpusMode.MODE_CELT_ONLY && num13 == 0)
			{
				for (int i = 0; i < frame_size * this.channels; i++)
				{
					*pcm[pcm_ptr + i] = Inlines.SAT16(Inlines.ADD32((int)(*pcm[pcm_ptr + i]), (int)array3[i]));
				}
			}
			int[] window = celt_Decoder.GetMode().window;
			if (num3 != 0 && num5 == 0)
			{
				celt_Decoder.ResetState();
				celt_Decoder.SetStartBand(0);
				celt_Decoder.celt_decode_with_ec(data, data_ptr + len, num4, array5, 0, num9, null, 0);
				num6 = celt_Decoder.GetFinalRange();
				CodecHelpers.smooth_fade(pcm, pcm_ptr + this.channels * (frame_size - num10), array5, this.channels * num10, pcm, pcm_ptr + this.channels * (frame_size - num10), num10, this.channels, window, this.Fs);
			}
			if (num3 != 0 && num5 != 0)
			{
				for (int j = 0; j < this.channels; j++)
				{
					for (int i = 0; i < num10; i++)
					{
						*pcm[this.channels * i + j + pcm_ptr] = array5[this.channels * i + j];
					}
				}
				CodecHelpers.smooth_fade(array5, this.channels * num10, pcm, pcm_ptr + this.channels * num10, pcm, pcm_ptr + this.channels * num10, num10, this.channels, window, this.Fs);
			}
			if (num2 != 0)
			{
				if (num11 >= num9)
				{
					for (int i = 0; i < this.channels * num10; i++)
					{
						*pcm[i] = array[i];
					}
					CodecHelpers.smooth_fade(array, this.channels * num10, pcm, pcm_ptr + this.channels * num10, pcm, pcm_ptr + this.channels * num10, num10, this.channels, window, this.Fs);
				}
				else
				{
					CodecHelpers.smooth_fade(array, 0, pcm, pcm_ptr, pcm, pcm_ptr, num10, this.channels, window, this.Fs);
				}
			}
			if (this.decode_gain != 0)
			{
				int b = Inlines.celt_exp2(Inlines.MULT16_16_P15(21771, this.decode_gain));
				for (int i = pcm_ptr; i < pcm_ptr + frame_size * this.channels; i++)
				{
					int x = Inlines.MULT16_32_P16(*pcm[i], b);
					*pcm[i] = (short)Inlines.SATURATE(x, 32767);
				}
			}
			if (len <= 1)
			{
				this.rangeFinal = 0U;
			}
			else
			{
				this.rangeFinal = (entropyCoder.rng ^ num6);
			}
			this.prev_mode = opusMode;
			this.prev_redundancy = ((num3 != 0 && num5 == 0) ? 1 : 0);
			if (num >= 0)
			{
				return num11;
			}
			return num;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x000198A4 File Offset: 0x00017AA4
		internal int opus_decode_native(ReadOnlySpan<byte> data, int data_ptr, int len, Span<short> pcm_out, int pcm_out_ptr, int frame_size, int decode_fec, int self_delimited, out int packet_offset, int soft_clip)
		{
			packet_offset = 0;
			short[] array = new short[48];
			if (decode_fec < 0 || decode_fec > 1)
			{
				return -1;
			}
			if ((decode_fec != 0 || len == 0 || data.IsEmpty) && frame_size % (this.Fs / 400) != 0)
			{
				return -1;
			}
			if (len == 0 || data.IsEmpty)
			{
				int num = 0;
				int num2;
				for (;;)
				{
					num2 = this.opus_decode_frame(null, 0, 0, pcm_out, pcm_out_ptr + num * this.channels, frame_size - num, 0);
					if (num2 < 0)
					{
						break;
					}
					num += num2;
					if (num >= frame_size)
					{
						goto Block_7;
					}
				}
				return num2;
				Block_7:
				this.last_packet_duration = num;
				return num;
			}
			if (len < 0)
			{
				return -1;
			}
			OpusMode encoderMode = OpusPacketInfo.GetEncoderMode(data.Slice(data_ptr));
			OpusBandwidth opusBandwidth = OpusPacketInfo.GetBandwidth(data.Slice(data_ptr));
			int numSamplesPerFrame = OpusPacketInfo.GetNumSamplesPerFrame(data.Slice(data_ptr), this.Fs);
			int numEncodedChannels = OpusPacketInfo.GetNumEncodedChannels(data.Slice(data_ptr));
			byte b;
			int num4;
			int num3 = OpusPacketInfo.opus_packet_parse_impl(data, data_ptr, len, self_delimited, out b, null, null, 0, array, 0, out num4, out packet_offset);
			if (num3 < 0)
			{
				return num3;
			}
			data_ptr += num4;
			if (decode_fec != 0)
			{
				if (frame_size < numSamplesPerFrame || encoderMode == OpusMode.MODE_CELT_ONLY || this.mode == OpusMode.MODE_CELT_ONLY)
				{
					int num5;
					return this.opus_decode_native(null, 0, 0, pcm_out, pcm_out_ptr, frame_size, 0, 0, out num5, soft_clip);
				}
				int num6 = this.last_packet_duration;
				int num7;
				if (frame_size - numSamplesPerFrame != 0)
				{
					int num5;
					num7 = this.opus_decode_native(null, 0, 0, pcm_out, pcm_out_ptr, frame_size - numSamplesPerFrame, 0, 0, out num5, soft_clip);
					if (num7 < 0)
					{
						this.last_packet_duration = num6;
						return num7;
					}
				}
				this.mode = encoderMode;
				this.bandwidth = opusBandwidth;
				this.frame_size = numSamplesPerFrame;
				this.stream_channels = numEncodedChannels;
				num7 = this.opus_decode_frame(data, data_ptr, (int)array[0], pcm_out, pcm_out_ptr + this.channels * (frame_size - numSamplesPerFrame), numSamplesPerFrame, 1);
				if (num7 < 0)
				{
					return num7;
				}
				this.last_packet_duration = frame_size;
				return frame_size;
			}
			else
			{
				if (num3 * numSamplesPerFrame > frame_size)
				{
					return -2;
				}
				this.mode = encoderMode;
				this.bandwidth = opusBandwidth;
				this.frame_size = numSamplesPerFrame;
				this.stream_channels = numEncodedChannels;
				int num8 = 0;
				for (int i = 0; i < num3; i++)
				{
					int num9 = this.opus_decode_frame(data, data_ptr, (int)array[i], pcm_out, pcm_out_ptr + num8 * this.channels, frame_size - num8, 0);
					if (num9 < 0)
					{
						return num9;
					}
					data_ptr += (int)array[i];
					num8 += num9;
				}
				this.last_packet_duration = num8;
				return num8;
			}
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00019AFB File Offset: 0x00017CFB
		[Obsolete("Use Span<> overrides if possible")]
		public int Decode(byte[] in_data, int in_data_offset, int len, short[] out_pcm, int out_pcm_offset, int frame_size, bool decode_fec = false)
		{
			if (in_data == null)
			{
				return this.Decode(ReadOnlySpan<byte>.Empty, MemoryExtensions.AsSpan<short>(out_pcm, out_pcm_offset), frame_size, decode_fec);
			}
			return this.Decode(MemoryExtensions.AsSpan<byte>(in_data, in_data_offset, len), MemoryExtensions.AsSpan<short>(out_pcm, out_pcm_offset), frame_size, decode_fec);
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00019B3C File Offset: 0x00017D3C
		public int Decode(ReadOnlySpan<byte> in_data, Span<short> out_pcm, int frame_size, bool decode_fec = false)
		{
			if (frame_size <= 0)
			{
				throw new ArgumentException("Frame size must be > 0");
			}
			int result;
			try
			{
				int num2;
				int num = this.opus_decode_native(in_data, 0, in_data.Length, out_pcm, 0, frame_size, (decode_fec > false) ? 1 : 0, 0, out num2, 0);
				if (num < 0)
				{
					if (num == -1)
					{
						throw new ArgumentException("OPUS_BAD_ARG while decoding");
					}
					throw new OpusException("An error occurred during decoding: " + CodecHelpers.opus_strerror(num), num);
				}
				else
				{
					result = num;
				}
			}
			catch (ArgumentException ex)
			{
				throw new OpusException("public error during decoding: " + ex.Message, -1);
			}
			return result;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00019BCC File Offset: 0x00017DCC
		[Obsolete("Use Span<> overrides if possible")]
		public int Decode(byte[] in_data, int in_data_offset, int len, float[] out_pcm, int out_pcm_offset, int frame_size, bool decode_fec = false)
		{
			if (in_data == null)
			{
				return this.Decode(ReadOnlySpan<byte>.Empty, MemoryExtensions.AsSpan<float>(out_pcm, out_pcm_offset), frame_size, decode_fec);
			}
			return this.Decode(MemoryExtensions.AsSpan<byte>(in_data, in_data_offset, len), MemoryExtensions.AsSpan<float>(out_pcm, out_pcm_offset), frame_size, decode_fec);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00019C0C File Offset: 0x00017E0C
		public unsafe int Decode(ReadOnlySpan<byte> in_data, Span<float> out_pcm, int frame_size, bool decode_fec = false)
		{
			if (frame_size <= 0)
			{
				throw new ArgumentException("Frame size must be > 0");
			}
			if (!in_data.IsEmpty && in_data.Length > 0 && !decode_fec)
			{
				int numSamples = OpusPacketInfo.GetNumSamples(in_data, this.Fs);
				if (numSamples <= 0)
				{
					throw new OpusException("An invalid packet was provided (unable to parse # of samples)", -4);
				}
				frame_size = Inlines.IMIN(frame_size, numSamples);
			}
			short[] array = new short[frame_size * this.channels];
			int result;
			try
			{
				int num2;
				int num = this.opus_decode_native(in_data, 0, in_data.Length, array, 0, frame_size, (decode_fec > false) ? 1 : 0, 0, out num2, 0);
				if (num < 0)
				{
					if (num == -1)
					{
						throw new ArgumentException("OPUS_BAD_ARG when decoding");
					}
					throw new OpusException("An error occurred during decoding: " + CodecHelpers.opus_strerror(num), num);
				}
				else
				{
					if (num > 0)
					{
						for (int i = 0; i < num * this.channels; i++)
						{
							*out_pcm[i] = 3.0517578E-05f * (float)array[i];
						}
					}
					result = num;
				}
			}
			catch (ArgumentException ex)
			{
				throw new OpusException("public error during decoding: " + ex.Message, -1);
			}
			return result;
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00019D1C File Offset: 0x00017F1C
		public OpusBandwidth Bandwidth
		{
			get
			{
				return this.bandwidth;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000187 RID: 391 RVA: 0x00019D24 File Offset: 0x00017F24
		public uint FinalRange
		{
			get
			{
				return this.rangeFinal;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00019D2C File Offset: 0x00017F2C
		public int SampleRate
		{
			get
			{
				return this.Fs;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000189 RID: 393 RVA: 0x00019D34 File Offset: 0x00017F34
		public int NumChannels
		{
			get
			{
				return this.channels;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00019D3C File Offset: 0x00017F3C
		public int Pitch
		{
			get
			{
				if (this.prev_mode == OpusMode.MODE_CELT_ONLY)
				{
					return this.Celt_Decoder.GetPitch();
				}
				return this.DecControl.prevPitchLag;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600018B RID: 395 RVA: 0x00019D62 File Offset: 0x00017F62
		// (set) Token: 0x0600018C RID: 396 RVA: 0x00019D6A File Offset: 0x00017F6A
		public int Gain
		{
			get
			{
				return this.decode_gain;
			}
			set
			{
				if (value < -32768 || value > 32767)
				{
					throw new ArgumentException("Gain must be within the range of a signed int16");
				}
				this.decode_gain = value;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x0600018D RID: 397 RVA: 0x00019D8E File Offset: 0x00017F8E
		public int LastPacketDuration
		{
			get
			{
				return this.last_packet_duration;
			}
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00019D96 File Offset: 0x00017F96
		public void ResetState()
		{
			this.PartialReset();
			this.Celt_Decoder.ResetState();
			DecodeAPI.silk_InitDecoder(this.SilkDecoder);
			this.stream_channels = this.channels;
			this.frame_size = this.Fs / 400;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x00019DD3 File Offset: 0x00017FD3
		public string GetVersionString()
		{
			return CodecHelpers.GetVersionString();
		}

		// Token: 0x06000190 RID: 400 RVA: 0x00019DDA File Offset: 0x00017FDA
		public void Dispose()
		{
		}

		// Token: 0x040002BA RID: 698
		internal int channels;

		// Token: 0x040002BB RID: 699
		internal int Fs;

		// Token: 0x040002BC RID: 700
		internal readonly DecControlState DecControl = new DecControlState();

		// Token: 0x040002BD RID: 701
		internal int decode_gain;

		// Token: 0x040002BE RID: 702
		internal int stream_channels;

		// Token: 0x040002BF RID: 703
		internal OpusBandwidth bandwidth;

		// Token: 0x040002C0 RID: 704
		internal OpusMode mode;

		// Token: 0x040002C1 RID: 705
		internal OpusMode prev_mode;

		// Token: 0x040002C2 RID: 706
		internal int frame_size;

		// Token: 0x040002C3 RID: 707
		internal int prev_redundancy;

		// Token: 0x040002C4 RID: 708
		internal int last_packet_duration;

		// Token: 0x040002C5 RID: 709
		internal uint rangeFinal;

		// Token: 0x040002C6 RID: 710
		internal SilkDecoder SilkDecoder = new SilkDecoder();

		// Token: 0x040002C7 RID: 711
		internal CeltDecoder Celt_Decoder = new CeltDecoder();

		// Token: 0x040002C8 RID: 712
		private static readonly byte[] SILENCE = new byte[]
		{
			byte.MaxValue,
			byte.MaxValue
		};
	}
}
