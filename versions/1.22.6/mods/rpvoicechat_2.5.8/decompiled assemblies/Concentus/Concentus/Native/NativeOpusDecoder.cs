using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Concentus.Enums;

namespace Concentus.Native
{
	// Token: 0x02000070 RID: 112
	internal class NativeOpusDecoder : IOpusDecoder, IDisposable
	{
		// Token: 0x06000283 RID: 643 RVA: 0x00020327 File Offset: 0x0001E527
		internal NativeOpusDecoder(IntPtr handle)
		{
			this._handle = handle;
		}

		// Token: 0x06000284 RID: 644 RVA: 0x00020344 File Offset: 0x0001E544
		~NativeOpusDecoder()
		{
			this.Dispose(false);
		}

		// Token: 0x06000285 RID: 645 RVA: 0x00020374 File Offset: 0x0001E574
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000286 RID: 646 RVA: 0x00020383 File Offset: 0x0001E583
		protected virtual void Dispose(bool disposing)
		{
			if (Interlocked.CompareExchange(ref this._disposed, 1, 0) != 0)
			{
				return;
			}
			if (this._handle != IntPtr.Zero)
			{
				NativeOpus.opus_decoder_destroy(this._handle);
			}
		}

		// Token: 0x06000287 RID: 647 RVA: 0x000203B0 File Offset: 0x0001E5B0
		public static NativeOpusDecoder Create(int sampleRate, int channelCount)
		{
			int num;
			IntPtr intPtr = NativeOpus.opus_decoder_create(sampleRate, channelCount, out num);
			if (num != 0)
			{
				if (intPtr != IntPtr.Zero)
				{
					NativeOpus.opus_decoder_destroy(intPtr);
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(37, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to create opus decoder: error ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return new NativeOpusDecoder(intPtr)
			{
				_sampleRate = sampleRate,
				_numChannels = channelCount
			};
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000288 RID: 648 RVA: 0x00020417 File Offset: 0x0001E617
		private IntPtr NativeHandle
		{
			get
			{
				return this._handle;
			}
		}

		// Token: 0x06000289 RID: 649 RVA: 0x00020420 File Offset: 0x0001E620
		public unsafe int Decode(ReadOnlySpan<byte> in_data, Span<float> out_pcm, int frame_size, bool decode_fec = false)
		{
			fixed (float* pinnableReference = out_pcm.GetPinnableReference())
			{
				float* pcm = pinnableReference;
				if (in_data.Length == 0)
				{
					return NativeOpus.opus_decode_float(this.NativeHandle, null, 0, pcm, frame_size, (decode_fec > false) ? 1 : 0);
				}
				fixed (byte* pinnableReference2 = in_data.GetPinnableReference())
				{
					byte* data = pinnableReference2;
					return NativeOpus.opus_decode_float(this.NativeHandle, data, in_data.Length, pcm, frame_size, (decode_fec > false) ? 1 : 0);
				}
			}
		}

		// Token: 0x0600028A RID: 650 RVA: 0x0002047C File Offset: 0x0001E67C
		public unsafe int Decode(ReadOnlySpan<byte> in_data, Span<short> out_pcm, int frame_size, bool decode_fec = false)
		{
			fixed (short* pinnableReference = out_pcm.GetPinnableReference())
			{
				short* pcm = pinnableReference;
				if (in_data.Length == 0)
				{
					return NativeOpus.opus_decode(this.NativeHandle, null, 0, pcm, frame_size, (decode_fec > false) ? 1 : 0);
				}
				fixed (byte* pinnableReference2 = in_data.GetPinnableReference())
				{
					byte* data = pinnableReference2;
					return NativeOpus.opus_decode(this.NativeHandle, data, in_data.Length, pcm, frame_size, (decode_fec > false) ? 1 : 0);
				}
			}
		}

		// Token: 0x0600028B RID: 651 RVA: 0x000204D8 File Offset: 0x0001E6D8
		public void ResetState()
		{
			NativeOpus.opus_decoder_ctl(this.NativeHandle, 4028, 0);
		}

		// Token: 0x0600028C RID: 652 RVA: 0x000204EC File Offset: 0x0001E6EC
		public string GetVersionString()
		{
			return Marshal.PtrToStringAnsi(NativeOpus.opus_get_version_string());
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600028D RID: 653 RVA: 0x000204F8 File Offset: 0x0001E6F8
		public int SampleRate
		{
			get
			{
				return this._sampleRate;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600028E RID: 654 RVA: 0x00020500 File Offset: 0x0001E700
		public int NumChannels
		{
			get
			{
				return this._numChannels;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600028F RID: 655 RVA: 0x00020508 File Offset: 0x0001E708
		public OpusBandwidth Bandwidth
		{
			get
			{
				int result;
				NativeOpus.opus_decoder_ctl(this.NativeHandle, 4009, out result);
				return (OpusBandwidth)result;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000290 RID: 656 RVA: 0x0002052C File Offset: 0x0001E72C
		public uint FinalRange
		{
			get
			{
				int result;
				NativeOpus.opus_decoder_ctl(this.NativeHandle, 4031, out result);
				return (uint)result;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000291 RID: 657 RVA: 0x00020550 File Offset: 0x0001E750
		// (set) Token: 0x06000292 RID: 658 RVA: 0x00020571 File Offset: 0x0001E771
		public int Gain
		{
			get
			{
				int result;
				NativeOpus.opus_decoder_ctl(this.NativeHandle, 4045, out result);
				return result;
			}
			set
			{
				NativeOpus.opus_decoder_ctl(this.NativeHandle, 4034, value);
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000293 RID: 659 RVA: 0x00020588 File Offset: 0x0001E788
		public int LastPacketDuration
		{
			get
			{
				int result;
				NativeOpus.opus_decoder_ctl(this.NativeHandle, 4039, out result);
				return result;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000294 RID: 660 RVA: 0x000205AC File Offset: 0x0001E7AC
		public int Pitch
		{
			get
			{
				int result;
				NativeOpus.opus_decoder_ctl(this.NativeHandle, 4033, out result);
				return result;
			}
		}

		// Token: 0x040003BC RID: 956
		private readonly IntPtr _handle = IntPtr.Zero;

		// Token: 0x040003BD RID: 957
		private int _disposed;

		// Token: 0x040003BE RID: 958
		private int _sampleRate;

		// Token: 0x040003BF RID: 959
		private int _numChannels;
	}
}
