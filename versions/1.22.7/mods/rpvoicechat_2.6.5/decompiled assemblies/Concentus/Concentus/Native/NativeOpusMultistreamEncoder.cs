using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Concentus.Enums;

namespace Concentus.Native
{
	// Token: 0x02000073 RID: 115
	internal class NativeOpusMultistreamEncoder : IOpusMultiStreamEncoder, IDisposable
	{
		// Token: 0x060002D3 RID: 723 RVA: 0x00020DB1 File Offset: 0x0001EFB1
		internal NativeOpusMultistreamEncoder(IntPtr handle)
		{
			this._handle = handle;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00020DCC File Offset: 0x0001EFCC
		~NativeOpusMultistreamEncoder()
		{
			this.Dispose(false);
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00020DFC File Offset: 0x0001EFFC
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00020E0B File Offset: 0x0001F00B
		protected virtual void Dispose(bool disposing)
		{
			if (Interlocked.CompareExchange(ref this._disposed, 1, 0) != 0)
			{
				return;
			}
			if (this._handle != IntPtr.Zero)
			{
				NativeOpus.opus_multistream_encoder_destroy(this._handle);
			}
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00020E38 File Offset: 0x0001F038
		public unsafe static NativeOpusMultistreamEncoder Create(int sampleRate, int channelCount, int mapping_family, out int streams, out int coupled_streams, byte[] mapping, OpusApplication application)
		{
			byte* mapping2;
			if (mapping == null || mapping.Length == 0)
			{
				mapping2 = null;
			}
			else
			{
				mapping2 = &mapping[0];
			}
			int num;
			IntPtr intPtr = NativeOpus.opus_multistream_surround_encoder_create(sampleRate, channelCount, mapping_family, out streams, out coupled_streams, mapping2, (int)application, out num);
			if (num != 0)
			{
				if (intPtr != IntPtr.Zero)
				{
					NativeOpus.opus_multistream_encoder_destroy(intPtr);
				}
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(40, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to create opus MS encoder: error ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return new NativeOpusMultistreamEncoder(intPtr)
			{
				_sampleRate = sampleRate,
				_numChannels = channelCount
			};
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060002D8 RID: 728 RVA: 0x00020EBF File Offset: 0x0001F0BF
		private IntPtr NativeHandle
		{
			get
			{
				return this._handle;
			}
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00020EC8 File Offset: 0x0001F0C8
		public unsafe int EncodeMultistream(ReadOnlySpan<float> in_pcm, int frame_size, Span<byte> out_data, int max_data_bytes)
		{
			fixed (float* pinnableReference = in_pcm.GetPinnableReference())
			{
				float* pcm = pinnableReference;
				fixed (byte* pinnableReference2 = out_data.GetPinnableReference())
				{
					byte* data = pinnableReference2;
					return NativeOpus.opus_multistream_encode_float(this.NativeHandle, pcm, frame_size, data, max_data_bytes);
				}
			}
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00020EFC File Offset: 0x0001F0FC
		public unsafe int EncodeMultistream(ReadOnlySpan<short> in_pcm, int frame_size, Span<byte> out_data, int max_data_bytes)
		{
			fixed (short* pinnableReference = in_pcm.GetPinnableReference())
			{
				short* pcm = pinnableReference;
				fixed (byte* pinnableReference2 = out_data.GetPinnableReference())
				{
					byte* data = pinnableReference2;
					return NativeOpus.opus_multistream_encode(this.NativeHandle, pcm, frame_size, data, max_data_bytes);
				}
			}
		}

		// Token: 0x060002DB RID: 731 RVA: 0x00020F2F File Offset: 0x0001F12F
		public void ResetState()
		{
			NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4028, 0);
		}

		// Token: 0x060002DC RID: 732 RVA: 0x00020F43 File Offset: 0x0001F143
		public string GetVersionString()
		{
			return Marshal.PtrToStringAnsi(NativeOpus.opus_get_version_string());
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060002DD RID: 733 RVA: 0x00020F4F File Offset: 0x0001F14F
		public int SampleRate
		{
			get
			{
				return this._sampleRate;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060002DE RID: 734 RVA: 0x00020F57 File Offset: 0x0001F157
		public int NumChannels
		{
			get
			{
				return this._numChannels;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060002DF RID: 735 RVA: 0x00020F60 File Offset: 0x0001F160
		// (set) Token: 0x060002E0 RID: 736 RVA: 0x00020F81 File Offset: 0x0001F181
		public int Complexity
		{
			get
			{
				int result;
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4011, out result);
				return result;
			}
			set
			{
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4010, value);
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x00020F98 File Offset: 0x0001F198
		// (set) Token: 0x060002E2 RID: 738 RVA: 0x00020FBC File Offset: 0x0001F1BC
		public bool UseDTX
		{
			get
			{
				int num;
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4017, out num);
				return num != 0;
			}
			set
			{
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4016, (value > false) ? 1 : 0);
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x00020FD4 File Offset: 0x0001F1D4
		// (set) Token: 0x060002E4 RID: 740 RVA: 0x00020FF5 File Offset: 0x0001F1F5
		public int Bitrate
		{
			get
			{
				int result;
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4003, out result);
				return result;
			}
			set
			{
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4002, value);
			}
		}

		// Token: 0x170000A6 RID: 166
		// (set) Token: 0x060002E5 RID: 741 RVA: 0x00021009 File Offset: 0x0001F209
		public OpusMode ForceMode
		{
			set
			{
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 11002, (int)value);
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060002E6 RID: 742 RVA: 0x00021020 File Offset: 0x0001F220
		// (set) Token: 0x060002E7 RID: 743 RVA: 0x00021044 File Offset: 0x0001F244
		public bool UseVBR
		{
			get
			{
				int num;
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4007, out num);
				return num != 0;
			}
			set
			{
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4006, (value > false) ? 1 : 0);
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x0002105C File Offset: 0x0001F25C
		// (set) Token: 0x060002E9 RID: 745 RVA: 0x0002107D File Offset: 0x0001F27D
		public OpusApplication Application
		{
			get
			{
				int result;
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4001, out result);
				return (OpusApplication)result;
			}
			set
			{
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4000, (int)value);
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060002EA RID: 746 RVA: 0x00021094 File Offset: 0x0001F294
		// (set) Token: 0x060002EB RID: 747 RVA: 0x000210B5 File Offset: 0x0001F2B5
		public OpusBandwidth Bandwidth
		{
			get
			{
				int result;
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4009, out result);
				return (OpusBandwidth)result;
			}
			set
			{
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4008, (int)value);
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060002EC RID: 748 RVA: 0x000210CC File Offset: 0x0001F2CC
		// (set) Token: 0x060002ED RID: 749 RVA: 0x000210ED File Offset: 0x0001F2ED
		public OpusFramesize ExpertFrameDuration
		{
			get
			{
				int result;
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4041, out result);
				return (OpusFramesize)result;
			}
			set
			{
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4040, (int)value);
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060002EE RID: 750 RVA: 0x00021104 File Offset: 0x0001F304
		public uint FinalRange
		{
			get
			{
				int result;
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4031, out result);
				return (uint)result;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060002EF RID: 751 RVA: 0x00021128 File Offset: 0x0001F328
		public int Lookahead
		{
			get
			{
				int result;
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4027, out result);
				return result;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0002114C File Offset: 0x0001F34C
		// (set) Token: 0x060002F1 RID: 753 RVA: 0x0002116D File Offset: 0x0001F36D
		public int LSBDepth
		{
			get
			{
				int result;
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4037, out result);
				return result;
			}
			set
			{
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4036, value);
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x00021184 File Offset: 0x0001F384
		// (set) Token: 0x060002F3 RID: 755 RVA: 0x000211A5 File Offset: 0x0001F3A5
		public OpusBandwidth MaxBandwidth
		{
			get
			{
				int result;
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4005, out result);
				return (OpusBandwidth)result;
			}
			set
			{
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4004, (int)value);
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x000211BC File Offset: 0x0001F3BC
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x000211DD File Offset: 0x0001F3DD
		public int PacketLossPercent
		{
			get
			{
				int result;
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4015, out result);
				return result;
			}
			set
			{
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4014, value);
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x000211F4 File Offset: 0x0001F3F4
		// (set) Token: 0x060002F7 RID: 759 RVA: 0x00021218 File Offset: 0x0001F418
		public bool PredictionDisabled
		{
			get
			{
				int num;
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4043, out num);
				return num != 0;
			}
			set
			{
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4042, (value > false) ? 1 : 0);
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x00021230 File Offset: 0x0001F430
		// (set) Token: 0x060002F9 RID: 761 RVA: 0x00021251 File Offset: 0x0001F451
		public OpusSignal SignalType
		{
			get
			{
				int result;
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4025, out result);
				return (OpusSignal)result;
			}
			set
			{
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4024, (int)value);
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060002FA RID: 762 RVA: 0x00021268 File Offset: 0x0001F468
		// (set) Token: 0x060002FB RID: 763 RVA: 0x0002128C File Offset: 0x0001F48C
		public bool UseConstrainedVBR
		{
			get
			{
				int num;
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4021, out num);
				return num != 0;
			}
			set
			{
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4020, (value > false) ? 1 : 0);
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060002FC RID: 764 RVA: 0x000212A4 File Offset: 0x0001F4A4
		// (set) Token: 0x060002FD RID: 765 RVA: 0x000212C8 File Offset: 0x0001F4C8
		public bool UseInbandFEC
		{
			get
			{
				int num;
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4013, out num);
				return num != 0;
			}
			set
			{
				NativeOpus.opus_multistream_encoder_ctl(this.NativeHandle, 4012, (value > false) ? 1 : 0);
			}
		}

		// Token: 0x040003C8 RID: 968
		private readonly IntPtr _handle = IntPtr.Zero;

		// Token: 0x040003C9 RID: 969
		private int _disposed;

		// Token: 0x040003CA RID: 970
		private int _sampleRate;

		// Token: 0x040003CB RID: 971
		private int _numChannels;
	}
}
