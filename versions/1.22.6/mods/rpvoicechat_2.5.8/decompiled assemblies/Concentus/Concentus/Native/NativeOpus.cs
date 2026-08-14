using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Concentus.Native
{
	// Token: 0x0200006F RID: 111
	internal class NativeOpus
	{
		// Token: 0x06000260 RID: 608 RVA: 0x000201A8 File Offset: 0x0001E3A8
		internal static bool Initialize(TextWriter logger)
		{
			return NativePlatformUtils.PrepareNativeLibrary("opus", logger) == NativeLibraryStatus.Available && NativeOpus.opus_get_version_string() != IntPtr.Zero;
		}

		// Token: 0x06000261 RID: 609 RVA: 0x000201CC File Offset: 0x0001E3CC
		[LibraryImport("opus")]
		[GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "8.0.10.6711")]
		[SkipLocalsInit]
		internal unsafe static IntPtr opus_decoder_create(int Fs, int channels, out int error)
		{
			Unsafe.SkipInit<int>(ref error);
			IntPtr result;
			fixed (int* ptr = &error)
			{
				int* _error_native = ptr;
				result = NativeOpus.<opus_decoder_create>g____PInvoke|41_0(Fs, channels, _error_native);
			}
			return result;
		}

		// Token: 0x06000262 RID: 610 RVA: 0x000201F0 File Offset: 0x0001E3F0
		[LibraryImport("opus")]
		[GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "8.0.10.6711")]
		[SkipLocalsInit]
		internal unsafe static IntPtr opus_multistream_decoder_create(int Fs, int channels, int streams, int coupled_streams, byte* mapping, out int error)
		{
			Unsafe.SkipInit<int>(ref error);
			IntPtr result;
			fixed (int* ptr = &error)
			{
				int* _error_native = ptr;
				result = NativeOpus.<opus_multistream_decoder_create>g____PInvoke|42_0(Fs, channels, streams, coupled_streams, mapping, _error_native);
			}
			return result;
		}

		// Token: 0x06000263 RID: 611
		[LibraryImport("opus")]
		[DllImport("opus", ExactSpelling = true)]
		internal unsafe static extern int opus_decode(IntPtr st, byte* data, int len, short* pcm, int frame_size, int decode_fec);

		// Token: 0x06000264 RID: 612
		[LibraryImport("opus")]
		[DllImport("opus", ExactSpelling = true)]
		internal unsafe static extern int opus_decode_float(IntPtr st, byte* data, int len, float* pcm, int frame_size, int decode_fec);

		// Token: 0x06000265 RID: 613
		[LibraryImport("opus")]
		[DllImport("opus", ExactSpelling = true)]
		internal unsafe static extern int opus_multistream_decode(IntPtr st, byte* data, int len, short* pcm, int frame_size, int decode_fec);

		// Token: 0x06000266 RID: 614
		[LibraryImport("opus")]
		[DllImport("opus", ExactSpelling = true)]
		internal unsafe static extern int opus_multistream_decode_float(IntPtr st, byte* data, int len, float* pcm, int frame_size, int decode_fec);

		// Token: 0x06000267 RID: 615 RVA: 0x0002021C File Offset: 0x0001E41C
		[LibraryImport("opus")]
		[GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "8.0.10.6711")]
		[SkipLocalsInit]
		internal unsafe static IntPtr opus_encoder_create(int Fs, int channels, int application, out int error)
		{
			Unsafe.SkipInit<int>(ref error);
			IntPtr result;
			fixed (int* ptr = &error)
			{
				int* _error_native = ptr;
				result = NativeOpus.<opus_encoder_create>g____PInvoke|47_0(Fs, channels, application, _error_native);
			}
			return result;
		}

		// Token: 0x06000268 RID: 616 RVA: 0x00020240 File Offset: 0x0001E440
		[LibraryImport("opus")]
		[GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "8.0.10.6711")]
		[SkipLocalsInit]
		internal unsafe static IntPtr opus_multistream_surround_encoder_create(int Fs, int channels, int mapping_family, out int streams, out int coupled_streams, byte* mapping, int application, out int error)
		{
			Unsafe.SkipInit<int>(ref streams);
			Unsafe.SkipInit<int>(ref coupled_streams);
			Unsafe.SkipInit<int>(ref error);
			IntPtr result;
			fixed (int* ptr = &error)
			{
				int* _error_native = ptr;
				fixed (int* ptr2 = &coupled_streams)
				{
					int* _coupled_streams_native = ptr2;
					fixed (int* ptr3 = &streams)
					{
						int* _streams_native = ptr3;
						result = NativeOpus.<opus_multistream_surround_encoder_create>g____PInvoke|48_0(Fs, channels, mapping_family, _streams_native, _coupled_streams_native, mapping, application, _error_native);
					}
				}
			}
			return result;
		}

		// Token: 0x06000269 RID: 617
		[LibraryImport("opus")]
		[DllImport("opus", ExactSpelling = true)]
		internal unsafe static extern int opus_encode(IntPtr st, short* pcm, int frame_size, byte* data, int max_data_bytes);

		// Token: 0x0600026A RID: 618
		[LibraryImport("opus")]
		[DllImport("opus", ExactSpelling = true)]
		internal unsafe static extern int opus_encode_float(IntPtr st, float* pcm, int frame_size, byte* data, int max_data_bytes);

		// Token: 0x0600026B RID: 619
		[LibraryImport("opus")]
		[DllImport("opus", ExactSpelling = true)]
		internal unsafe static extern int opus_multistream_encode(IntPtr st, short* pcm, int frame_size, byte* data, int max_data_bytes);

		// Token: 0x0600026C RID: 620
		[LibraryImport("opus")]
		[DllImport("opus", ExactSpelling = true)]
		internal unsafe static extern int opus_multistream_encode_float(IntPtr st, float* pcm, int frame_size, byte* data, int max_data_bytes);

		// Token: 0x0600026D RID: 621
		[LibraryImport("opus")]
		[DllImport("opus", ExactSpelling = true)]
		internal static extern int opus_decoder_ctl(IntPtr st, int request, int value);

		// Token: 0x0600026E RID: 622 RVA: 0x00020290 File Offset: 0x0001E490
		[LibraryImport("opus")]
		[GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "8.0.10.6711")]
		[SkipLocalsInit]
		internal unsafe static int opus_decoder_ctl(IntPtr st, int request, out int value)
		{
			Unsafe.SkipInit<int>(ref value);
			int result;
			fixed (int* ptr = &value)
			{
				int* _value_native = ptr;
				result = NativeOpus.<opus_decoder_ctl>g____PInvoke|54_0(st, request, _value_native);
			}
			return result;
		}

		// Token: 0x0600026F RID: 623
		[LibraryImport("opus")]
		[DllImport("opus", ExactSpelling = true)]
		internal static extern int opus_encoder_ctl(IntPtr st, int request, int value);

		// Token: 0x06000270 RID: 624 RVA: 0x000202B4 File Offset: 0x0001E4B4
		[LibraryImport("opus")]
		[GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "8.0.10.6711")]
		[SkipLocalsInit]
		internal unsafe static int opus_encoder_ctl(IntPtr st, int request, out int value)
		{
			Unsafe.SkipInit<int>(ref value);
			int result;
			fixed (int* ptr = &value)
			{
				int* _value_native = ptr;
				result = NativeOpus.<opus_encoder_ctl>g____PInvoke|56_0(st, request, _value_native);
			}
			return result;
		}

		// Token: 0x06000271 RID: 625
		[LibraryImport("opus")]
		[DllImport("opus", ExactSpelling = true)]
		internal static extern int opus_multistream_decoder_ctl(IntPtr st, int request, int value);

		// Token: 0x06000272 RID: 626 RVA: 0x000202D8 File Offset: 0x0001E4D8
		[LibraryImport("opus")]
		[GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "8.0.10.6711")]
		[SkipLocalsInit]
		internal unsafe static int opus_multistream_decoder_ctl(IntPtr st, int request, out int value)
		{
			Unsafe.SkipInit<int>(ref value);
			int result;
			fixed (int* ptr = &value)
			{
				int* _value_native = ptr;
				result = NativeOpus.<opus_multistream_decoder_ctl>g____PInvoke|58_0(st, request, _value_native);
			}
			return result;
		}

		// Token: 0x06000273 RID: 627
		[LibraryImport("opus")]
		[DllImport("opus", ExactSpelling = true)]
		internal static extern int opus_multistream_encoder_ctl(IntPtr st, int request, int value);

		// Token: 0x06000274 RID: 628 RVA: 0x000202FC File Offset: 0x0001E4FC
		[LibraryImport("opus")]
		[GeneratedCode("Microsoft.Interop.LibraryImportGenerator", "8.0.10.6711")]
		[SkipLocalsInit]
		internal unsafe static int opus_multistream_encoder_ctl(IntPtr st, int request, out int value)
		{
			Unsafe.SkipInit<int>(ref value);
			int result;
			fixed (int* ptr = &value)
			{
				int* _value_native = ptr;
				result = NativeOpus.<opus_multistream_encoder_ctl>g____PInvoke|60_0(st, request, _value_native);
			}
			return result;
		}

		// Token: 0x06000275 RID: 629
		[LibraryImport("opus")]
		[DllImport("opus", ExactSpelling = true)]
		internal static extern IntPtr opus_get_version_string();

		// Token: 0x06000276 RID: 630
		[LibraryImport("opus")]
		[DllImport("opus", ExactSpelling = true)]
		internal static extern void opus_encoder_destroy(IntPtr encoder);

		// Token: 0x06000277 RID: 631
		[LibraryImport("opus")]
		[DllImport("opus", ExactSpelling = true)]
		internal static extern void opus_multistream_encoder_destroy(IntPtr encoder);

		// Token: 0x06000278 RID: 632
		[LibraryImport("opus")]
		[DllImport("opus", ExactSpelling = true)]
		internal static extern void opus_decoder_destroy(IntPtr decoder);

		// Token: 0x06000279 RID: 633
		[LibraryImport("opus")]
		[DllImport("opus", ExactSpelling = true)]
		internal static extern void opus_multistream_decoder_destroy(IntPtr decoder);

		// Token: 0x0600027B RID: 635
		[CompilerGenerated]
		[DllImport("opus", EntryPoint = "opus_decoder_create", ExactSpelling = true)]
		internal unsafe static extern IntPtr <opus_decoder_create>g____PInvoke|41_0(int __Fs_native, int __channels_native, int* __error_native);

		// Token: 0x0600027C RID: 636
		[CompilerGenerated]
		[DllImport("opus", EntryPoint = "opus_multistream_decoder_create", ExactSpelling = true)]
		internal unsafe static extern IntPtr <opus_multistream_decoder_create>g____PInvoke|42_0(int __Fs_native, int __channels_native, int __streams_native, int __coupled_streams_native, byte* __mapping_native, int* __error_native);

		// Token: 0x0600027D RID: 637
		[CompilerGenerated]
		[DllImport("opus", EntryPoint = "opus_encoder_create", ExactSpelling = true)]
		internal unsafe static extern IntPtr <opus_encoder_create>g____PInvoke|47_0(int __Fs_native, int __channels_native, int __application_native, int* __error_native);

		// Token: 0x0600027E RID: 638
		[CompilerGenerated]
		[DllImport("opus", EntryPoint = "opus_multistream_surround_encoder_create", ExactSpelling = true)]
		internal unsafe static extern IntPtr <opus_multistream_surround_encoder_create>g____PInvoke|48_0(int __Fs_native, int __channels_native, int __mapping_family_native, int* __streams_native, int* __coupled_streams_native, byte* __mapping_native, int __application_native, int* __error_native);

		// Token: 0x0600027F RID: 639
		[CompilerGenerated]
		[DllImport("opus", EntryPoint = "opus_decoder_ctl", ExactSpelling = true)]
		internal unsafe static extern int <opus_decoder_ctl>g____PInvoke|54_0(IntPtr __st_native, int __request_native, int* __value_native);

		// Token: 0x06000280 RID: 640
		[CompilerGenerated]
		[DllImport("opus", EntryPoint = "opus_encoder_ctl", ExactSpelling = true)]
		internal unsafe static extern int <opus_encoder_ctl>g____PInvoke|56_0(IntPtr __st_native, int __request_native, int* __value_native);

		// Token: 0x06000281 RID: 641
		[CompilerGenerated]
		[DllImport("opus", EntryPoint = "opus_multistream_decoder_ctl", ExactSpelling = true)]
		internal unsafe static extern int <opus_multistream_decoder_ctl>g____PInvoke|58_0(IntPtr __st_native, int __request_native, int* __value_native);

		// Token: 0x06000282 RID: 642
		[CompilerGenerated]
		[DllImport("opus", EntryPoint = "opus_multistream_encoder_ctl", ExactSpelling = true)]
		internal unsafe static extern int <opus_multistream_encoder_ctl>g____PInvoke|60_0(IntPtr __st_native, int __request_native, int* __value_native);

		// Token: 0x04000394 RID: 916
		private const string LIBRARY_NAME = "opus";

		// Token: 0x04000395 RID: 917
		internal const int OPUS_SET_APPLICATION_REQUEST = 4000;

		// Token: 0x04000396 RID: 918
		internal const int OPUS_GET_APPLICATION_REQUEST = 4001;

		// Token: 0x04000397 RID: 919
		internal const int OPUS_SET_BITRATE_REQUEST = 4002;

		// Token: 0x04000398 RID: 920
		internal const int OPUS_GET_BITRATE_REQUEST = 4003;

		// Token: 0x04000399 RID: 921
		internal const int OPUS_SET_MAX_BANDWIDTH_REQUEST = 4004;

		// Token: 0x0400039A RID: 922
		internal const int OPUS_GET_MAX_BANDWIDTH_REQUEST = 4005;

		// Token: 0x0400039B RID: 923
		internal const int OPUS_SET_VBR_REQUEST = 4006;

		// Token: 0x0400039C RID: 924
		internal const int OPUS_GET_VBR_REQUEST = 4007;

		// Token: 0x0400039D RID: 925
		internal const int OPUS_SET_BANDWIDTH_REQUEST = 4008;

		// Token: 0x0400039E RID: 926
		internal const int OPUS_GET_BANDWIDTH_REQUEST = 4009;

		// Token: 0x0400039F RID: 927
		internal const int OPUS_SET_COMPLEXITY_REQUEST = 4010;

		// Token: 0x040003A0 RID: 928
		internal const int OPUS_GET_COMPLEXITY_REQUEST = 4011;

		// Token: 0x040003A1 RID: 929
		internal const int OPUS_SET_INBAND_FEC_REQUEST = 4012;

		// Token: 0x040003A2 RID: 930
		internal const int OPUS_GET_INBAND_FEC_REQUEST = 4013;

		// Token: 0x040003A3 RID: 931
		internal const int OPUS_SET_PACKET_LOSS_PERC_REQUEST = 4014;

		// Token: 0x040003A4 RID: 932
		internal const int OPUS_GET_PACKET_LOSS_PERC_REQUEST = 4015;

		// Token: 0x040003A5 RID: 933
		internal const int OPUS_SET_DTX_REQUEST = 4016;

		// Token: 0x040003A6 RID: 934
		internal const int OPUS_GET_DTX_REQUEST = 4017;

		// Token: 0x040003A7 RID: 935
		internal const int OPUS_SET_VBR_CONSTRAINT_REQUEST = 4020;

		// Token: 0x040003A8 RID: 936
		internal const int OPUS_GET_VBR_CONSTRAINT_REQUEST = 4021;

		// Token: 0x040003A9 RID: 937
		internal const int OPUS_SET_FORCE_CHANNELS_REQUEST = 4022;

		// Token: 0x040003AA RID: 938
		internal const int OPUS_GET_FORCE_CHANNELS_REQUEST = 4023;

		// Token: 0x040003AB RID: 939
		internal const int OPUS_SET_SIGNAL_REQUEST = 4024;

		// Token: 0x040003AC RID: 940
		internal const int OPUS_GET_SIGNAL_REQUEST = 4025;

		// Token: 0x040003AD RID: 941
		internal const int OPUS_GET_LOOKAHEAD_REQUEST = 4027;

		// Token: 0x040003AE RID: 942
		internal const int OPUS_GET_SAMPLE_RATE_REQUEST = 4029;

		// Token: 0x040003AF RID: 943
		internal const int OPUS_GET_FINAL_RANGE_REQUEST = 4031;

		// Token: 0x040003B0 RID: 944
		internal const int OPUS_GET_PITCH_REQUEST = 4033;

		// Token: 0x040003B1 RID: 945
		internal const int OPUS_SET_GAIN_REQUEST = 4034;

		// Token: 0x040003B2 RID: 946
		internal const int OPUS_GET_GAIN_REQUEST = 4045;

		// Token: 0x040003B3 RID: 947
		internal const int OPUS_SET_LSB_DEPTH_REQUEST = 4036;

		// Token: 0x040003B4 RID: 948
		internal const int OPUS_GET_LSB_DEPTH_REQUEST = 4037;

		// Token: 0x040003B5 RID: 949
		internal const int OPUS_GET_LAST_PACKET_DURATION_REQUEST = 4039;

		// Token: 0x040003B6 RID: 950
		internal const int OPUS_SET_EXPERT_FRAME_DURATION_REQUEST = 4040;

		// Token: 0x040003B7 RID: 951
		internal const int OPUS_GET_EXPERT_FRAME_DURATION_REQUEST = 4041;

		// Token: 0x040003B8 RID: 952
		internal const int OPUS_SET_PREDICTION_DISABLED_REQUEST = 4042;

		// Token: 0x040003B9 RID: 953
		internal const int OPUS_GET_PREDICTION_DISABLED_REQUEST = 4043;

		// Token: 0x040003BA RID: 954
		internal const int OPUS_SET_FORCE_MODE_REQUEST = 11002;

		// Token: 0x040003BB RID: 955
		internal const int OPUS_RESET_STATE = 4028;
	}
}
