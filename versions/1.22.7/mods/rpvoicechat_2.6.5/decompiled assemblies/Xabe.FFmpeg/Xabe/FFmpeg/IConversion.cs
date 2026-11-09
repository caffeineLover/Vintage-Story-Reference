using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xabe.FFmpeg.Events;

namespace Xabe.FFmpeg
{
	// Token: 0x02000012 RID: 18
	public interface IConversion
	{
		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600006C RID: 108
		string OutputFilePath { get; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600006D RID: 109
		PipeDescriptor? OutputPipeDescriptor { get; }

		// Token: 0x0600006E RID: 110
		IConversion SetPriority(ProcessPriorityClass? priority);

		// Token: 0x0600006F RID: 111
		IConversion ExtractEveryNthFrame(int frameNo, Func<string, string> buildOutputFileName);

		// Token: 0x06000070 RID: 112
		IConversion ExtractNthFrame(int frameNo, Func<string, string> buildOutputFileName);

		// Token: 0x06000071 RID: 113
		IConversion BuildVideoFromImages(int startNumber, Func<string, string> buildInputFileName);

		// Token: 0x06000072 RID: 114
		IConversion BuildVideoFromImages(IEnumerable<string> imageFiles);

		// Token: 0x06000073 RID: 115
		IConversion SetFrameRate(double frameRate);

		// Token: 0x06000074 RID: 116
		IConversion SetInputFrameRate(double frameRate);

		// Token: 0x06000075 RID: 117
		IConversion SetSeek(TimeSpan? seek);

		// Token: 0x06000076 RID: 118
		IConversion SetInputTime(TimeSpan? seek);

		// Token: 0x06000077 RID: 119
		IConversion SetOutputTime(TimeSpan? seek);

		// Token: 0x06000078 RID: 120
		IConversion SetPreset(ConversionPreset preset);

		// Token: 0x06000079 RID: 121
		IConversion SetHashFormat(Hash format = Hash.SHA256);

		// Token: 0x0600007A RID: 122
		IConversion SetHashFormat(string format);

		// Token: 0x0600007B RID: 123
		IConversion SetVideoBitrate(long bitrate);

		// Token: 0x0600007C RID: 124
		IConversion SetAudioBitrate(long bitrate);

		// Token: 0x0600007D RID: 125
		IConversion UseMultiThread(int threadCount);

		// Token: 0x0600007E RID: 126
		IConversion UseMultiThread(bool multiThread);

		// Token: 0x0600007F RID: 127
		IConversion SetOutput(string outputPath);

		// Token: 0x06000080 RID: 128
		IConversion PipeOutput(PipeDescriptor descriptor = PipeDescriptor.stdout);

		// Token: 0x06000081 RID: 129
		IConversion SetOverwriteOutput(bool overwrite);

		// Token: 0x06000082 RID: 130
		IConversion SetInputFormat(string inputFormat);

		// Token: 0x06000083 RID: 131
		IConversion SetInputFormat(Format inputFormat);

		// Token: 0x06000084 RID: 132
		IConversion SetOutputFormat(Format outputFormat);

		// Token: 0x06000085 RID: 133
		IConversion SetOutputFormat(string outputFormat);

		// Token: 0x06000086 RID: 134
		IConversion SetPixelFormat(string pixelFormat);

		// Token: 0x06000087 RID: 135
		IConversion SetPixelFormat(PixelFormat pixelFormat);

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000088 RID: 136
		// (remove) Token: 0x06000089 RID: 137
		event ConversionProgressEventHandler OnProgress;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600008A RID: 138
		// (remove) Token: 0x0600008B RID: 139
		event DataReceivedEventHandler OnDataReceived;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x0600008C RID: 140
		// (remove) Token: 0x0600008D RID: 141
		event VideoDataEventHandler OnVideoDataReceived;

		// Token: 0x0600008E RID: 142
		IConversion UseShortest(bool useShortest);

		// Token: 0x0600008F RID: 143
		string Build();

		// Token: 0x06000090 RID: 144
		Task<IConversionResult> Start();

		// Token: 0x06000091 RID: 145
		Task<IConversionResult> Start(CancellationToken cancellationToken);

		// Token: 0x06000092 RID: 146
		Task<IConversionResult> Start(string parameters);

		// Token: 0x06000093 RID: 147
		Task<IConversionResult> Start(string parameters, CancellationToken cancellationToken);

		// Token: 0x06000094 RID: 148
		IConversion AddParameter(string parameter, ParameterPosition parameterPosition = ParameterPosition.PostInput);

		// Token: 0x06000095 RID: 149
		IConversion AddStream<T>(params T[] streams) where T : IStream;

		// Token: 0x06000096 RID: 150
		IConversion AddStream(IEnumerable<IStream> streams);

		// Token: 0x06000097 RID: 151
		IConversion UseHardwareAcceleration(HardwareAccelerator hardwareAccelerator, VideoCodec decoder, VideoCodec encoder, int device = 0);

		// Token: 0x06000098 RID: 152
		IConversion UseHardwareAcceleration(string hardwareAccelerator, string decoder, string encoder, int device = 0);

		// Token: 0x06000099 RID: 153
		IConversion SetVideoSyncMethod(VideoSyncMethod method);

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600009A RID: 154
		IEnumerable<IStream> Streams { get; }

		// Token: 0x0600009B RID: 155
		IConversion AddDesktopStream(string videoSize = null, double framerate = 30.0, int xOffset = 0, int yOffset = 0);

		// Token: 0x0600009C RID: 156
		IConversion AddDesktopStream(VideoSize videoSize, double framerate = 30.0, int xOffset = 0, int yOffset = 0);
	}
}
