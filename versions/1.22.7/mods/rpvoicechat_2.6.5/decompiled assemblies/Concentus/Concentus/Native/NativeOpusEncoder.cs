using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Concentus.Enums;

namespace Concentus.Native
{
	// Token: 0x02000071 RID: 113
	internal class NativeOpusEncoder : IOpusEncoder, IDisposable
	{
		// Token: 0x06000295 RID: 661 RVA: 0x000205CD File Offset: 0x0001E7CD
		internal NativeOpusEncoder(IntPtr handle)
		{
			this._handle = handle;
		}

		// Token: 0x06000296 RID: 662 RVA: 0x000205E8 File Offset: 0x0001E7E8
		~NativeOpusEncoder()
		{
			this.Dispose(false);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00020618 File Offset: 0x0001E818
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00020627 File Offset: 0x0001E827
		protected virtual void Dispose(bool disposing)
		{
			if (Interlocked.CompareExchange(ref this._disposed, 1, 0) != 0)
			{
				return;
			}
			if (this._handle != IntPtr.Zero)
			{
				NativeOpus.opus_encoder_destroy(this._handle);
			}
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00020654 File Offset: 0x0001E854
		public static NativeOpusEncoder Create(int sampleRate, int channelCount, OpusApplication application)
		{
			int num;
			IntPtr intPtr = NativeOpus.opus_encoder_create(sampleRate, channelCount, (int)application, out num);
			if (num != 0)
			{
				if (intPtr != IntPtr.Zero)
				{
					NativeOpus.opus_encoder_destroy(intPtr);
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(37, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to create opus encoder: error ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return new NativeOpusEncoder(intPtr)
			{
				_sampleRate = sampleRate,
				_numChannels = channelCount
			};
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x0600029A RID: 666 RVA: 0x000206BC File Offset: 0x0001E8BC
		private IntPtr NativeHandle
		{
			get
			{
				return this._handle;
			}
		}

		// Token: 0x0600029B RID: 667 RVA: 0x000206C4 File Offset: 0x0001E8C4
		public unsafe int Encode(ReadOnlySpan<short> in_pcm, int frame_size, Span<byte> out_data, int max_data_bytes)
		{
			fixed (short* pinnableReference = in_pcm.GetPinnableReference())
			{
				short* pcm = pinnableReference;
				fixed (byte* pinnableReference2 = out_data.GetPinnableReference())
				{
					byte* data = pinnableReference2;
					return NativeOpus.opus_encode(this.NativeHandle, pcm, frame_size, data, max_data_bytes);
				}
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x000206F8 File Offset: 0x0001E8F8
		public unsafe int Encode(ReadOnlySpan<float> in_pcm, int frame_size, Span<byte> out_data, int max_data_bytes)
		{
			fixed (float* pinnableReference = in_pcm.GetPinnableReference())
			{
				float* pcm = pinnableReference;
				fixed (byte* pinnableReference2 = out_data.GetPinnableReference())
				{
					byte* data = pinnableReference2;
					return NativeOpus.opus_encode_float(this.NativeHandle, pcm, frame_size, data, max_data_bytes);
				}
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0002072B File Offset: 0x0001E92B
		public void ResetState()
		{
			NativeOpus.opus_encoder_ctl(this.NativeHandle, 4028, 0);
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0002073F File Offset: 0x0001E93F
		public string GetVersionString()
		{
			return Marshal.PtrToStringAnsi(NativeOpus.opus_get_version_string());
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x0600029F RID: 671 RVA: 0x0002074B File Offset: 0x0001E94B
		public int SampleRate
		{
			get
			{
				return this._sampleRate;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x00020753 File Offset: 0x0001E953
		public int NumChannels
		{
			get
			{
				return this._numChannels;
			}
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x0002075C File Offset: 0x0001E95C
		// (set) Token: 0x060002A2 RID: 674 RVA: 0x0002077D File Offset: 0x0001E97D
		public int Complexity
		{
			get
			{
				int result;
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4011, out result);
				return result;
			}
			set
			{
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4010, value);
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x00020794 File Offset: 0x0001E994
		// (set) Token: 0x060002A4 RID: 676 RVA: 0x000207B8 File Offset: 0x0001E9B8
		public bool UseDTX
		{
			get
			{
				int num;
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4017, out num);
				return num != 0;
			}
			set
			{
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4016, (value > false) ? 1 : 0);
			}
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x000207D0 File Offset: 0x0001E9D0
		// (set) Token: 0x060002A6 RID: 678 RVA: 0x000207F1 File Offset: 0x0001E9F1
		public int Bitrate
		{
			get
			{
				int result;
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4003, out result);
				return result;
			}
			set
			{
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4002, value);
			}
		}

		// Token: 0x1700008A RID: 138
		// (set) Token: 0x060002A7 RID: 679 RVA: 0x00020805 File Offset: 0x0001EA05
		public OpusMode ForceMode
		{
			set
			{
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 11002, (int)value);
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x0002081C File Offset: 0x0001EA1C
		// (set) Token: 0x060002A9 RID: 681 RVA: 0x00020840 File Offset: 0x0001EA40
		public bool UseVBR
		{
			get
			{
				int num;
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4007, out num);
				return num != 0;
			}
			set
			{
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4006, (value > false) ? 1 : 0);
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060002AA RID: 682 RVA: 0x00020858 File Offset: 0x0001EA58
		// (set) Token: 0x060002AB RID: 683 RVA: 0x00020879 File Offset: 0x0001EA79
		public OpusApplication Application
		{
			get
			{
				int result;
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4001, out result);
				return (OpusApplication)result;
			}
			set
			{
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4000, (int)value);
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060002AC RID: 684 RVA: 0x00020890 File Offset: 0x0001EA90
		// (set) Token: 0x060002AD RID: 685 RVA: 0x000208B1 File Offset: 0x0001EAB1
		public int ForceChannels
		{
			get
			{
				int result;
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4023, out result);
				return result;
			}
			set
			{
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4022, value);
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060002AE RID: 686 RVA: 0x000208C8 File Offset: 0x0001EAC8
		// (set) Token: 0x060002AF RID: 687 RVA: 0x000208E9 File Offset: 0x0001EAE9
		public OpusBandwidth MaxBandwidth
		{
			get
			{
				int result;
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4005, out result);
				return (OpusBandwidth)result;
			}
			set
			{
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4004, (int)value);
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060002B0 RID: 688 RVA: 0x00020900 File Offset: 0x0001EB00
		// (set) Token: 0x060002B1 RID: 689 RVA: 0x00020921 File Offset: 0x0001EB21
		public OpusBandwidth Bandwidth
		{
			get
			{
				int result;
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4009, out result);
				return (OpusBandwidth)result;
			}
			set
			{
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4008, (int)value);
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x00020938 File Offset: 0x0001EB38
		// (set) Token: 0x060002B3 RID: 691 RVA: 0x0002095C File Offset: 0x0001EB5C
		public bool UseInbandFEC
		{
			get
			{
				int num;
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4013, out num);
				return num != 0;
			}
			set
			{
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4012, (value > false) ? 1 : 0);
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x00020974 File Offset: 0x0001EB74
		// (set) Token: 0x060002B5 RID: 693 RVA: 0x00020995 File Offset: 0x0001EB95
		public int PacketLossPercent
		{
			get
			{
				int result;
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4015, out result);
				return result;
			}
			set
			{
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4014, value);
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x000209AC File Offset: 0x0001EBAC
		// (set) Token: 0x060002B7 RID: 695 RVA: 0x000209D0 File Offset: 0x0001EBD0
		public bool UseConstrainedVBR
		{
			get
			{
				int num;
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4021, out num);
				return num != 0;
			}
			set
			{
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4020, (value > false) ? 1 : 0);
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x000209E8 File Offset: 0x0001EBE8
		// (set) Token: 0x060002B9 RID: 697 RVA: 0x00020A09 File Offset: 0x0001EC09
		public OpusSignal SignalType
		{
			get
			{
				int result;
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4025, out result);
				return (OpusSignal)result;
			}
			set
			{
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4024, (int)value);
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060002BA RID: 698 RVA: 0x00020A20 File Offset: 0x0001EC20
		public int Lookahead
		{
			get
			{
				int result;
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4027, out result);
				return result;
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060002BB RID: 699 RVA: 0x00020A44 File Offset: 0x0001EC44
		public uint FinalRange
		{
			get
			{
				int result;
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4031, out result);
				return (uint)result;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060002BC RID: 700 RVA: 0x00020A68 File Offset: 0x0001EC68
		// (set) Token: 0x060002BD RID: 701 RVA: 0x00020A89 File Offset: 0x0001EC89
		public int LSBDepth
		{
			get
			{
				int result;
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4037, out result);
				return result;
			}
			set
			{
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4036, value);
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060002BE RID: 702 RVA: 0x00020AA0 File Offset: 0x0001ECA0
		// (set) Token: 0x060002BF RID: 703 RVA: 0x00020AC1 File Offset: 0x0001ECC1
		public OpusFramesize ExpertFrameDuration
		{
			get
			{
				int result;
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4041, out result);
				return (OpusFramesize)result;
			}
			set
			{
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4040, (int)value);
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x00020AD8 File Offset: 0x0001ECD8
		// (set) Token: 0x060002C1 RID: 705 RVA: 0x00020AFC File Offset: 0x0001ECFC
		public bool PredictionDisabled
		{
			get
			{
				int num;
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4043, out num);
				return num != 0;
			}
			set
			{
				NativeOpus.opus_encoder_ctl(this.NativeHandle, 4042, (value > false) ? 1 : 0);
			}
		}

		// Token: 0x040003C0 RID: 960
		private readonly IntPtr _handle = IntPtr.Zero;

		// Token: 0x040003C1 RID: 961
		private int _disposed;

		// Token: 0x040003C2 RID: 962
		private int _sampleRate;

		// Token: 0x040003C3 RID: 963
		private int _numChannels;
	}
}
