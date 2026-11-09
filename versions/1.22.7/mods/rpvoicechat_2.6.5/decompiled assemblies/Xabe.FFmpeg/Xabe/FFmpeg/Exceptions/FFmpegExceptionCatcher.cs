using System;
using System.Collections.Generic;

namespace Xabe.FFmpeg.Exceptions
{
	// Token: 0x02000044 RID: 68
	internal class FFmpegExceptionCatcher
	{
		// Token: 0x06000213 RID: 531 RVA: 0x00006F78 File Offset: 0x00005178
		static FFmpegExceptionCatcher()
		{
			FFmpegExceptionCatcher._checks.Add(new ExceptionCheck("Invalid NAL unit size", false), delegate(string output, string args)
			{
				throw new ConversionException(output, args);
			});
			FFmpegExceptionCatcher._checks.Add(new ExceptionCheck("Packet mismatch", true), delegate(string output, string args)
			{
				throw new ConversionException(output, args);
			});
			FFmpegExceptionCatcher._checks.Add(new ExceptionCheck("asf_read_pts failed", true), delegate(string output, string args)
			{
				throw new UnknownDecoderException(output, args);
			});
			FFmpegExceptionCatcher._checks.Add(new ExceptionCheck("Missing key frame while searching for timestamp", true), delegate(string output, string args)
			{
				throw new UnknownDecoderException(output, args);
			});
			FFmpegExceptionCatcher._checks.Add(new ExceptionCheck("Old interlaced mode is not supported", true), delegate(string output, string args)
			{
				throw new UnknownDecoderException(output, args);
			});
			FFmpegExceptionCatcher._checks.Add(new ExceptionCheck("mpeg1video", true), delegate(string output, string args)
			{
				throw new UnknownDecoderException(output, args);
			});
			FFmpegExceptionCatcher._checks.Add(new ExceptionCheck("Frame rate very high for a muxer not efficiently supporting it", true), delegate(string output, string args)
			{
				throw new UnknownDecoderException(output, args);
			});
			FFmpegExceptionCatcher._checks.Add(new ExceptionCheck("multiple fourcc not supported", false), delegate(string output, string args)
			{
				throw new UnknownDecoderException(output, args);
			});
			FFmpegExceptionCatcher._checks.Add(new ExceptionCheck("Unknown decoder", false), delegate(string output, string args)
			{
				throw new UnknownDecoderException(output, args);
			});
			FFmpegExceptionCatcher._checks.Add(new ExceptionCheck("Failed to open codec in avformat_find_stream_info", false), delegate(string output, string args)
			{
				throw new UnknownDecoderException(output, args);
			});
			FFmpegExceptionCatcher._checks.Add(new ExceptionCheck("Unrecognized hwaccel: ", false), delegate(string output, string args)
			{
				throw new HardwareAcceleratorNotFoundException(output, args);
			});
			FFmpegExceptionCatcher._checks.Add(new ExceptionCheck("Unable to find a suitable output format", false), delegate(string output, string args)
			{
				throw new FFmpegNoSuitableOutputFormatFoundException(output, args);
			});
			FFmpegExceptionCatcher._checks.Add(new ExceptionCheck("is not supported by the bitstream filter", false), delegate(string output, string args)
			{
				throw new InvalidBitstreamFilterException(output, args);
			});
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00007170 File Offset: 0x00005370
		internal void CatchFFmpegErrors(string output, string args)
		{
			foreach (KeyValuePair<ExceptionCheck, Action<string, string>> keyValuePair in FFmpegExceptionCatcher._checks)
			{
				try
				{
					if (keyValuePair.Key.CheckLog(output))
					{
						keyValuePair.Value.Invoke(output, args);
					}
				}
				catch (ConversionException ex)
				{
					throw new ConversionException(ex.Message, ex, ex.InputParameters);
				}
			}
		}

		// Token: 0x04000515 RID: 1301
		private static readonly Dictionary<ExceptionCheck, Action<string, string>> _checks = new Dictionary<ExceptionCheck, Action<string, string>>();
	}
}
