using System;
using System.Runtime.CompilerServices;
using Concentus.Common;
using Concentus.Enums;

namespace Concentus.Structs
{
	// Token: 0x0200005C RID: 92
	public class OpusMSDecoder : IOpusMultiStreamDecoder, IDisposable
	{
		// Token: 0x060001CF RID: 463 RVA: 0x0001CAA8 File Offset: 0x0001ACA8
		private OpusMSDecoder(int nb_streams, int nb_coupled_streams)
		{
			this.decoders = new OpusDecoder[nb_streams];
			for (int i = 0; i < nb_streams; i++)
			{
				this.decoders[i] = new OpusDecoder();
			}
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0001CAEC File Offset: 0x0001ACEC
		internal int opus_multistream_decoder_init(int Fs, int channels, int streams, int coupled_streams, byte[] mapping)
		{
			int num = 0;
			if (channels > 255 || channels < 1 || coupled_streams > streams || streams < 1 || coupled_streams < 0 || streams > 255 - coupled_streams)
			{
				throw new ArgumentException("Invalid channel or coupled stream count");
			}
			this.layout.nb_channels = channels;
			this.layout.nb_streams = streams;
			this.layout.nb_coupled_streams = coupled_streams;
			int i;
			for (i = 0; i < this.layout.nb_channels; i++)
			{
				this.layout.mapping[i] = mapping[i];
			}
			if (OpusMultistream.validate_layout(this.layout) == 0)
			{
				throw new ArgumentException("Invalid surround channel layout");
			}
			for (i = 0; i < this.layout.nb_coupled_streams; i++)
			{
				int num2 = this.decoders[num].opus_decoder_init(Fs, 2);
				if (num2 != 0)
				{
					return num2;
				}
				num++;
			}
			while (i < this.layout.nb_streams)
			{
				int num2 = this.decoders[num].opus_decoder_init(Fs, 1);
				if (num2 != 0)
				{
					return num2;
				}
				num++;
				i++;
			}
			return 0;
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0001CBEC File Offset: 0x0001ADEC
		[Obsolete("Use OpusCodecFactory methods which can give you native code if supported by your platform")]
		public OpusMSDecoder(int Fs, int channels, int streams, int coupled_streams, byte[] mapping) : this(streams, coupled_streams)
		{
			if (channels > 255 || channels < 1 || coupled_streams > streams || streams < 1 || coupled_streams < 0 || streams > 255 - coupled_streams)
			{
				throw new ArgumentException("Invalid channel / stream configuration");
			}
			int num = this.opus_multistream_decoder_init(Fs, channels, streams, coupled_streams, mapping);
			if (num == 0)
			{
				return;
			}
			if (num == -1)
			{
				throw new ArgumentException("Bad argument while creating MS decoder");
			}
			throw new OpusException("Could not create MS decoder: " + CodecHelpers.opus_strerror(num), num);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0001CC6C File Offset: 0x0001AE6C
		internal static int opus_multistream_packet_validate(ReadOnlySpan<byte> data, int nb_streams, int Fs)
		{
			short[] array = new short[48];
			int num = 0;
			int num2 = 0;
			int num3 = data.Length;
			for (int i = 0; i < nb_streams; i++)
			{
				if (data.Length <= 0)
				{
					return -4;
				}
				byte b;
				int num5;
				int num6;
				int num4 = OpusPacketInfo.opus_packet_parse_impl(data, num2, num3, (i != nb_streams - 1) ? 1 : 0, out b, null, null, 0, array, 0, out num5, out num6);
				if (num4 < 0)
				{
					return num4;
				}
				int numSamples = OpusPacketInfo.GetNumSamples(data.Slice(num2, num6), Fs);
				if (i != 0 && num != numSamples)
				{
					return -4;
				}
				num = numSamples;
				num2 += num6;
				num3 -= num6;
			}
			return num;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x0001CD0C File Offset: 0x0001AF0C
		internal int opus_multistream_decode_native<T>(ReadOnlySpan<byte> data, Span<T> pcm, OpusMSDecoder.opus_copy_channel_out_func<T> copy_channel_out, int frame_size, int decode_fec, int soft_clip)
		{
			int num = 0;
			int sampleRate = this.SampleRate;
			frame_size = Inlines.IMIN(frame_size, sampleRate / 25 * 3);
			short[] array = new short[2 * frame_size];
			int num2 = 0;
			if (data.Length == 0)
			{
				num = 1;
			}
			if (data.Length < 0)
			{
				return -1;
			}
			if (num == 0 && data.Length < 2 * this.layout.nb_streams - 1)
			{
				return -4;
			}
			if (num == 0)
			{
				int num3 = OpusMSDecoder.opus_multistream_packet_validate(data, this.layout.nb_streams, sampleRate);
				if (num3 < 0)
				{
					return num3;
				}
				if (num3 > frame_size)
				{
					return -2;
				}
			}
			int num4 = 0;
			int num5 = data.Length;
			for (int i = 0; i < this.layout.nb_streams; i++)
			{
				OpusDecoder opusDecoder = this.decoders[num2++];
				if (num == 0 && data.Length <= 0)
				{
					return -3;
				}
				int num7;
				int num6 = opusDecoder.opus_decode_native(data, num4, num5, array, 0, frame_size, decode_fec, (i != this.layout.nb_streams - 1) ? 1 : 0, out num7, soft_clip);
				num4 += num7;
				num5 -= num7;
				if (num6 <= 0)
				{
					return num6;
				}
				frame_size = num6;
				if (i < this.layout.nb_coupled_streams)
				{
					int prev = -1;
					int num8;
					while ((num8 = OpusMultistream.get_left_channel(this.layout, i, prev)) != -1)
					{
						copy_channel_out.Invoke(pcm, this.layout.nb_channels, num8, array, 0, 2, frame_size);
						prev = num8;
					}
					prev = -1;
					while ((num8 = OpusMultistream.get_right_channel(this.layout, i, prev)) != -1)
					{
						copy_channel_out.Invoke(pcm, this.layout.nb_channels, num8, array, 1, 2, frame_size);
						prev = num8;
					}
				}
				else
				{
					int prev2 = -1;
					int num9;
					while ((num9 = OpusMultistream.get_mono_channel(this.layout, i, prev2)) != -1)
					{
						copy_channel_out.Invoke(pcm, this.layout.nb_channels, num9, array, 0, 1, frame_size);
						prev2 = num9;
					}
				}
			}
			for (int j = 0; j < this.layout.nb_channels; j++)
			{
				if (this.layout.mapping[j] == 255)
				{
					copy_channel_out.Invoke(pcm, this.layout.nb_channels, j, null, 0, 0, frame_size);
				}
			}
			return frame_size;
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0001CF40 File Offset: 0x0001B140
		internal unsafe static void opus_copy_channel_out_float(Span<float> dst, int dst_stride, int dst_channel, Span<short> src, int src_ptr, int src_stride, int frame_size)
		{
			if (!src.IsEmpty)
			{
				for (int i = 0; i < frame_size; i++)
				{
					*dst[i * dst_stride + dst_channel] = 3.0517578E-05f * (float)(*src[i * src_stride + src_ptr]);
				}
				return;
			}
			for (int i = 0; i < frame_size; i++)
			{
				*dst[i * dst_stride + dst_channel] = 0f;
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0001CFA8 File Offset: 0x0001B1A8
		internal unsafe static void opus_copy_channel_out_short(Span<short> dst, int dst_stride, int dst_channel, Span<short> src, int src_ptr, int src_stride, int frame_size)
		{
			if (!src.IsEmpty)
			{
				for (int i = 0; i < frame_size; i++)
				{
					*dst[i * dst_stride + dst_channel] = *src[i * src_stride + src_ptr];
				}
				return;
			}
			for (int i = 0; i < frame_size; i++)
			{
				*dst[i * dst_stride + dst_channel] = 0;
			}
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0001D003 File Offset: 0x0001B203
		[Obsolete("Use Span<> overrides if possible")]
		public int DecodeMultistream(byte[] data, int data_offset, int len, short[] out_pcm, int out_pcm_offset, int frame_size, bool decode_fec)
		{
			if (data == null)
			{
				return this.DecodeMultistream(ReadOnlySpan<byte>.Empty, MemoryExtensions.AsSpan<short>(out_pcm, out_pcm_offset), frame_size, decode_fec);
			}
			return this.DecodeMultistream(MemoryExtensions.AsSpan<byte>(data, data_offset, len), MemoryExtensions.AsSpan<short>(out_pcm, out_pcm_offset), frame_size, decode_fec);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0001D044 File Offset: 0x0001B244
		public int DecodeMultistream(ReadOnlySpan<byte> data, Span<short> out_pcm, int frame_size, bool decode_fec)
		{
			OpusMSDecoder.opus_copy_channel_out_func<short> copy_channel_out;
			if ((copy_channel_out = OpusMSDecoder.<>O.<0>__opus_copy_channel_out_short) == null)
			{
				copy_channel_out = (OpusMSDecoder.<>O.<0>__opus_copy_channel_out_short = new OpusMSDecoder.opus_copy_channel_out_func<short>(OpusMSDecoder.opus_copy_channel_out_short));
			}
			int num = this.opus_multistream_decode_native<short>(data, out_pcm, copy_channel_out, frame_size, (decode_fec > false) ? 1 : 0, 0);
			if (num >= 0)
			{
				return num;
			}
			if (num == -1)
			{
				throw new ArgumentException("OPUS_BAD_ARG while decoding");
			}
			throw new OpusException("An error occurred during decoding: " + CodecHelpers.opus_strerror(num), num);
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0001D0A7 File Offset: 0x0001B2A7
		[Obsolete("Use Span<> overrides if possible")]
		public int DecodeMultistream(byte[] data, int data_offset, int len, float[] out_pcm, int out_pcm_offset, int frame_size, bool decode_fec)
		{
			if (data == null)
			{
				return this.DecodeMultistream(ReadOnlySpan<byte>.Empty, MemoryExtensions.AsSpan<float>(out_pcm, out_pcm_offset), frame_size, decode_fec);
			}
			return this.DecodeMultistream(MemoryExtensions.AsSpan<byte>(data, data_offset, len), MemoryExtensions.AsSpan<float>(out_pcm, out_pcm_offset), frame_size, decode_fec);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0001D0E8 File Offset: 0x0001B2E8
		public int DecodeMultistream(ReadOnlySpan<byte> data, Span<float> out_pcm, int frame_size, bool decode_fec)
		{
			OpusMSDecoder.opus_copy_channel_out_func<float> copy_channel_out;
			if ((copy_channel_out = OpusMSDecoder.<>O.<1>__opus_copy_channel_out_float) == null)
			{
				copy_channel_out = (OpusMSDecoder.<>O.<1>__opus_copy_channel_out_float = new OpusMSDecoder.opus_copy_channel_out_func<float>(OpusMSDecoder.opus_copy_channel_out_float));
			}
			int num = this.opus_multistream_decode_native<float>(data, out_pcm, copy_channel_out, frame_size, (decode_fec > false) ? 1 : 0, 0);
			if (num >= 0)
			{
				return num;
			}
			if (num == -1)
			{
				throw new ArgumentException("OPUS_BAD_ARG while decoding");
			}
			throw new OpusException("An error occurred during decoding: " + CodecHelpers.opus_strerror(num), num);
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001DA RID: 474 RVA: 0x0001D14B File Offset: 0x0001B34B
		public OpusBandwidth Bandwidth
		{
			get
			{
				if (this.decoders == null || this.decoders.Length == 0)
				{
					throw new InvalidOperationException("Decoder not initialized");
				}
				return this.decoders[0].Bandwidth;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001DB RID: 475 RVA: 0x0001D176 File Offset: 0x0001B376
		public int SampleRate
		{
			get
			{
				if (this.decoders == null || this.decoders.Length == 0)
				{
					throw new InvalidOperationException("Decoder not initialized");
				}
				return this.decoders[0].SampleRate;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001DC RID: 476 RVA: 0x0001D1A1 File Offset: 0x0001B3A1
		public int NumChannels
		{
			get
			{
				return this.layout.nb_channels;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001DD RID: 477 RVA: 0x0001D1AE File Offset: 0x0001B3AE
		// (set) Token: 0x060001DE RID: 478 RVA: 0x0001D1D4 File Offset: 0x0001B3D4
		public int Gain
		{
			get
			{
				if (this.decoders == null || this.decoders.Length == 0)
				{
					return -6;
				}
				return this.decoders[0].Gain;
			}
			set
			{
				for (int i = 0; i < this.layout.nb_streams; i++)
				{
					this.decoders[i].Gain = value;
				}
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001DF RID: 479 RVA: 0x0001D205 File Offset: 0x0001B405
		public int LastPacketDuration
		{
			get
			{
				if (this.decoders == null || this.decoders.Length == 0)
				{
					return -6;
				}
				return this.decoders[0].LastPacketDuration;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x0001D228 File Offset: 0x0001B428
		public uint FinalRange
		{
			get
			{
				uint num = 0U;
				for (int i = 0; i < this.layout.nb_streams; i++)
				{
					num ^= this.decoders[i].FinalRange;
				}
				return num;
			}
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0001D260 File Offset: 0x0001B460
		public void ResetState()
		{
			for (int i = 0; i < this.layout.nb_streams; i++)
			{
				this.decoders[i].ResetState();
			}
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0001D290 File Offset: 0x0001B490
		public string GetVersionString()
		{
			return CodecHelpers.GetVersionString();
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0001D297 File Offset: 0x0001B497
		public OpusDecoder GetMultistreamDecoderState(int streamId)
		{
			return this.decoders[streamId];
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0001D2A1 File Offset: 0x0001B4A1
		public void Dispose()
		{
		}

		// Token: 0x040002F2 RID: 754
		internal ChannelLayout layout = new ChannelLayout();

		// Token: 0x040002F3 RID: 755
		internal OpusDecoder[] decoders;

		// Token: 0x020000A0 RID: 160
		// (Invoke) Token: 0x06000545 RID: 1349
		internal delegate void opus_copy_channel_out_func<T>(Span<T> dst, int dst_stride, int dst_channel, Span<short> src, int src_ptr, int src_stride, int frame_size);

		// Token: 0x020000A1 RID: 161
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040005ED RID: 1517
			public static OpusMSDecoder.opus_copy_channel_out_func<short> <0>__opus_copy_channel_out_short;

			// Token: 0x040005EE RID: 1518
			public static OpusMSDecoder.opus_copy_channel_out_func<float> <1>__opus_copy_channel_out_float;
		}
	}
}
