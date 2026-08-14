using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Concentus.Enums;

namespace Concentus.Native
{
	// Token: 0x02000072 RID: 114
	internal class NativeOpusMultistreamDecoder : IOpusMultiStreamDecoder, IDisposable
	{
		// Token: 0x060002C2 RID: 706 RVA: 0x00020B13 File Offset: 0x0001ED13
		internal NativeOpusMultistreamDecoder(IntPtr handle)
		{
			this._handle = handle;
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00020B30 File Offset: 0x0001ED30
		~NativeOpusMultistreamDecoder()
		{
			this.Dispose(false);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x00020B60 File Offset: 0x0001ED60
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00020B6F File Offset: 0x0001ED6F
		protected virtual void Dispose(bool disposing)
		{
			if (Interlocked.CompareExchange(ref this._disposed, 1, 0) != 0)
			{
				return;
			}
			if (this._handle != IntPtr.Zero)
			{
				NativeOpus.opus_multistream_decoder_destroy(this._handle);
			}
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x00020B9C File Offset: 0x0001ED9C
		public unsafe static NativeOpusMultistreamDecoder Create(int sampleRate, int channelCount, int streams, int coupledStreams, byte[] channelMapping)
		{
			byte* mapping;
			if (channelMapping == null || channelMapping.Length == 0)
			{
				mapping = null;
			}
			else
			{
				mapping = &channelMapping[0];
			}
			int num;
			IntPtr intPtr = NativeOpus.opus_multistream_decoder_create(sampleRate, channelCount, streams, coupledStreams, mapping, out num);
			if (num != 0)
			{
				if (intPtr != IntPtr.Zero)
				{
					NativeOpus.opus_multistream_decoder_destroy(intPtr);
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(40, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to create opus MS decoder: error ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return new NativeOpusMultistreamDecoder(intPtr)
			{
				_sampleRate = sampleRate,
				_numChannels = channelCount
			};
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x00020C1F File Offset: 0x0001EE1F
		private IntPtr NativeHandle
		{
			get
			{
				return this._handle;
			}
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00020C28 File Offset: 0x0001EE28
		public unsafe int DecodeMultistream(ReadOnlySpan<byte> data, Span<float> out_pcm, int frame_size, bool decode_fec)
		{
			fixed (float* pinnableReference = out_pcm.GetPinnableReference())
			{
				float* pcm = pinnableReference;
				if (data.Length == 0)
				{
					return NativeOpus.opus_multistream_decode_float(this.NativeHandle, null, 0, pcm, frame_size, (decode_fec > false) ? 1 : 0);
				}
				fixed (byte* pinnableReference2 = data.GetPinnableReference())
				{
					byte* data2 = pinnableReference2;
					return NativeOpus.opus_multistream_decode_float(this.NativeHandle, data2, data.Length, pcm, frame_size, (decode_fec > false) ? 1 : 0);
				}
			}
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00020C84 File Offset: 0x0001EE84
		public unsafe int DecodeMultistream(ReadOnlySpan<byte> data, Span<short> out_pcm, int frame_size, bool decode_fec)
		{
			fixed (short* pinnableReference = out_pcm.GetPinnableReference())
			{
				short* pcm = pinnableReference;
				if (data.Length == 0)
				{
					return NativeOpus.opus_multistream_decode(this.NativeHandle, null, 0, pcm, frame_size, (decode_fec > false) ? 1 : 0);
				}
				fixed (byte* pinnableReference2 = data.GetPinnableReference())
				{
					byte* data2 = pinnableReference2;
					return NativeOpus.opus_multistream_decode(this.NativeHandle, data2, data.Length, pcm, frame_size, (decode_fec > false) ? 1 : 0);
				}
			}
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00020CE0 File Offset: 0x0001EEE0
		public void ResetState()
		{
			NativeOpus.opus_multistream_decoder_ctl(this.NativeHandle, 4028, 0);
		}

		// Token: 0x060002CB RID: 715 RVA: 0x00020CF4 File Offset: 0x0001EEF4
		public string GetVersionString()
		{
			return Marshal.PtrToStringAnsi(NativeOpus.opus_get_version_string());
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060002CC RID: 716 RVA: 0x00020D00 File Offset: 0x0001EF00
		public int SampleRate
		{
			get
			{
				return this._sampleRate;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060002CD RID: 717 RVA: 0x00020D08 File Offset: 0x0001EF08
		public int NumChannels
		{
			get
			{
				return this._numChannels;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060002CE RID: 718 RVA: 0x00020D10 File Offset: 0x0001EF10
		public OpusBandwidth Bandwidth
		{
			get
			{
				int result;
				NativeOpus.opus_multistream_decoder_ctl(this.NativeHandle, 4009, out result);
				return (OpusBandwidth)result;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060002CF RID: 719 RVA: 0x00020D34 File Offset: 0x0001EF34
		public uint FinalRange
		{
			get
			{
				int result;
				NativeOpus.opus_multistream_decoder_ctl(this.NativeHandle, 4031, out result);
				return (uint)result;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x00020D58 File Offset: 0x0001EF58
		// (set) Token: 0x060002D1 RID: 721 RVA: 0x00020D79 File Offset: 0x0001EF79
		public int Gain
		{
			get
			{
				int result;
				NativeOpus.opus_multistream_decoder_ctl(this.NativeHandle, 4045, out result);
				return result;
			}
			set
			{
				NativeOpus.opus_multistream_decoder_ctl(this.NativeHandle, 4034, value);
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x00020D90 File Offset: 0x0001EF90
		public int LastPacketDuration
		{
			get
			{
				int result;
				NativeOpus.opus_multistream_decoder_ctl(this.NativeHandle, 4039, out result);
				return result;
			}
		}

		// Token: 0x040003C4 RID: 964
		private readonly IntPtr _handle = IntPtr.Zero;

		// Token: 0x040003C5 RID: 965
		private int _disposed;

		// Token: 0x040003C6 RID: 966
		private int _sampleRate;

		// Token: 0x040003C7 RID: 967
		private int _numChannels;
	}
}
