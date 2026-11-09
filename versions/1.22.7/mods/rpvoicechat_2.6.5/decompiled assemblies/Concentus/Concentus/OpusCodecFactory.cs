using System;
using System.IO;
using System.Runtime.CompilerServices;
using Concentus.Enums;
using Concentus.Native;
using Concentus.Structs;

namespace Concentus
{
	// Token: 0x02000007 RID: 7
	public static class OpusCodecFactory
	{
		// Token: 0x06000077 RID: 119 RVA: 0x00002050 File Offset: 0x00000250
		public static IOpusEncoder CreateEncoder(int sampleRate, int numChannels, OpusApplication application = OpusApplication.OPUS_APPLICATION_AUDIO, TextWriter messageLogger = null)
		{
			if (OpusCodecFactory._userAllowNativeLib && OpusCodecFactory.NativeLibraryAvailable(messageLogger))
			{
				return NativeOpusEncoder.Create(sampleRate, numChannels, application);
			}
			return new OpusEncoder(sampleRate, numChannels, application);
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00002072 File Offset: 0x00000272
		public static IOpusDecoder CreateDecoder(int sampleRate, int numChannels, TextWriter messageLogger = null)
		{
			if (OpusCodecFactory._userAllowNativeLib && OpusCodecFactory.NativeLibraryAvailable(messageLogger))
			{
				return NativeOpusDecoder.Create(sampleRate, numChannels);
			}
			return new OpusDecoder(sampleRate, numChannels);
		}

		// Token: 0x06000079 RID: 121 RVA: 0x00002092 File Offset: 0x00000292
		public static IOpusMultiStreamEncoder CreateMultiStreamEncoder(int sampleRate, int numChannels, int mappingFamily, out int streams, out int coupledStreams, byte[] mapping, OpusApplication application, TextWriter messageLogger = null)
		{
			if (OpusCodecFactory._userAllowNativeLib && OpusCodecFactory.NativeLibraryAvailable(messageLogger))
			{
				return NativeOpusMultistreamEncoder.Create(sampleRate, numChannels, mappingFamily, out streams, out coupledStreams, mapping, application);
			}
			return OpusMSEncoder.CreateSurround(sampleRate, numChannels, mappingFamily, out streams, out coupledStreams, mapping, application);
		}

		// Token: 0x0600007A RID: 122 RVA: 0x000020C3 File Offset: 0x000002C3
		public static IOpusMultiStreamDecoder CreateMultiStreamDecoder(int sampleRate, int numChannels, int streams, int coupledStreams, byte[] mapping, TextWriter messageLogger = null)
		{
			if (OpusCodecFactory._userAllowNativeLib && OpusCodecFactory.NativeLibraryAvailable(messageLogger))
			{
				return NativeOpusMultistreamDecoder.Create(sampleRate, numChannels, streams, coupledStreams, mapping);
			}
			return new OpusMSDecoder(sampleRate, numChannels, streams, coupledStreams, mapping);
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600007B RID: 123 RVA: 0x000020EC File Offset: 0x000002EC
		// (set) Token: 0x0600007C RID: 124 RVA: 0x000020F3 File Offset: 0x000002F3
		public static bool AttemptToUseNativeLibrary
		{
			get
			{
				return OpusCodecFactory._userAllowNativeLib;
			}
			set
			{
				OpusCodecFactory._userAllowNativeLib = value;
			}
		}

		// Token: 0x0600007D RID: 125 RVA: 0x000020FC File Offset: 0x000002FC
		private static bool NativeLibraryAvailable(TextWriter messageLogger)
		{
			object mutex = OpusCodecFactory._mutex;
			bool isNativeLibAvailable;
			lock (mutex)
			{
				if (!OpusCodecFactory._nativeLibInitialized)
				{
					try
					{
						OpusCodecFactory._isNativeLibAvailable = NativeOpus.Initialize(messageLogger);
						if (messageLogger != null)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
							defaultInterpolatedStringHandler.AppendLiteral("Is native opus available? ");
							defaultInterpolatedStringHandler.AppendFormatted<bool>(OpusCodecFactory._isNativeLibAvailable);
							messageLogger.WriteLine(defaultInterpolatedStringHandler.ToStringAndClear());
						}
					}
					catch (Exception ex)
					{
						if (messageLogger != null)
						{
							messageLogger.WriteLine(ex.ToString());
						}
					}
					OpusCodecFactory._nativeLibInitialized = true;
				}
				isNativeLibAvailable = OpusCodecFactory._isNativeLibAvailable;
			}
			return isNativeLibAvailable;
		}

		// Token: 0x04000001 RID: 1
		private static readonly object _mutex = new object();

		// Token: 0x04000002 RID: 2
		private static bool _nativeLibInitialized = false;

		// Token: 0x04000003 RID: 3
		private static bool _isNativeLibAvailable = false;

		// Token: 0x04000004 RID: 4
		private static bool _userAllowNativeLib = true;
	}
}
