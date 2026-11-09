using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xabe.FFmpeg.Streams.SubtitleStream;

namespace Xabe.FFmpeg
{
	// Token: 0x0200001C RID: 28
	public class Snippets
	{
		// Token: 0x060000D5 RID: 213 RVA: 0x00004D59 File Offset: 0x00002F59
		internal Snippets()
		{
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00004D64 File Offset: 0x00002F64
		public Task<IConversion> ExtractAudio(string inputPath, string outputPath)
		{
			Snippets.<ExtractAudio>d__1 <ExtractAudio>d__;
			<ExtractAudio>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<ExtractAudio>d__.inputPath = inputPath;
			<ExtractAudio>d__.outputPath = outputPath;
			<ExtractAudio>d__.<>1__state = -1;
			<ExtractAudio>d__.<>t__builder.Start<Snippets.<ExtractAudio>d__1>(ref <ExtractAudio>d__);
			return <ExtractAudio>d__.<>t__builder.Task;
		}

		// Token: 0x060000D7 RID: 215 RVA: 0x00004DB0 File Offset: 0x00002FB0
		public Task<IConversion> AddAudio(string videoPath, string audioPath, string outputPath)
		{
			Snippets.<AddAudio>d__2 <AddAudio>d__;
			<AddAudio>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<AddAudio>d__.videoPath = videoPath;
			<AddAudio>d__.audioPath = audioPath;
			<AddAudio>d__.outputPath = outputPath;
			<AddAudio>d__.<>1__state = -1;
			<AddAudio>d__.<>t__builder.Start<Snippets.<AddAudio>d__2>(ref <AddAudio>d__);
			return <AddAudio>d__.<>t__builder.Task;
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004E04 File Offset: 0x00003004
		public Task<IConversion> ToMp4(string inputPath, string outputPath)
		{
			Snippets.<ToMp4>d__3 <ToMp4>d__;
			<ToMp4>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<ToMp4>d__.inputPath = inputPath;
			<ToMp4>d__.outputPath = outputPath;
			<ToMp4>d__.<>1__state = -1;
			<ToMp4>d__.<>t__builder.Start<Snippets.<ToMp4>d__3>(ref <ToMp4>d__);
			return <ToMp4>d__.<>t__builder.Task;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00004E50 File Offset: 0x00003050
		public Task<IConversion> ToTs(string inputPath, string outputPath)
		{
			Snippets.<ToTs>d__4 <ToTs>d__;
			<ToTs>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<ToTs>d__.inputPath = inputPath;
			<ToTs>d__.outputPath = outputPath;
			<ToTs>d__.<>1__state = -1;
			<ToTs>d__.<>t__builder.Start<Snippets.<ToTs>d__4>(ref <ToTs>d__);
			return <ToTs>d__.<>t__builder.Task;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00004E9C File Offset: 0x0000309C
		public Task<IConversion> ToOgv(string inputPath, string outputPath)
		{
			Snippets.<ToOgv>d__5 <ToOgv>d__;
			<ToOgv>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<ToOgv>d__.inputPath = inputPath;
			<ToOgv>d__.outputPath = outputPath;
			<ToOgv>d__.<>1__state = -1;
			<ToOgv>d__.<>t__builder.Start<Snippets.<ToOgv>d__5>(ref <ToOgv>d__);
			return <ToOgv>d__.<>t__builder.Task;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00004EE8 File Offset: 0x000030E8
		public Task<IConversion> ToWebM(string inputPath, string outputPath)
		{
			Snippets.<ToWebM>d__6 <ToWebM>d__;
			<ToWebM>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<ToWebM>d__.inputPath = inputPath;
			<ToWebM>d__.outputPath = outputPath;
			<ToWebM>d__.<>1__state = -1;
			<ToWebM>d__.<>t__builder.Start<Snippets.<ToWebM>d__6>(ref <ToWebM>d__);
			return <ToWebM>d__.<>t__builder.Task;
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00004F34 File Offset: 0x00003134
		public Task<IConversion> ToGif(string inputPath, string outputPath, int loop, int delay = 0)
		{
			Snippets.<ToGif>d__7 <ToGif>d__;
			<ToGif>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<ToGif>d__.inputPath = inputPath;
			<ToGif>d__.outputPath = outputPath;
			<ToGif>d__.loop = loop;
			<ToGif>d__.delay = delay;
			<ToGif>d__.<>1__state = -1;
			<ToGif>d__.<>t__builder.Start<Snippets.<ToGif>d__7>(ref <ToGif>d__);
			return <ToGif>d__.<>t__builder.Task;
		}

		// Token: 0x060000DD RID: 221 RVA: 0x00004F90 File Offset: 0x00003190
		public Task<IConversion> ConvertWithHardware(string inputFilePath, string outputFilePath, HardwareAccelerator hardwareAccelerator, VideoCodec decoder, VideoCodec encoder, int device = 0)
		{
			Snippets.<ConvertWithHardware>d__8 <ConvertWithHardware>d__;
			<ConvertWithHardware>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<ConvertWithHardware>d__.inputFilePath = inputFilePath;
			<ConvertWithHardware>d__.outputFilePath = outputFilePath;
			<ConvertWithHardware>d__.hardwareAccelerator = hardwareAccelerator;
			<ConvertWithHardware>d__.decoder = decoder;
			<ConvertWithHardware>d__.encoder = encoder;
			<ConvertWithHardware>d__.device = device;
			<ConvertWithHardware>d__.<>1__state = -1;
			<ConvertWithHardware>d__.<>t__builder.Start<Snippets.<ConvertWithHardware>d__8>(ref <ConvertWithHardware>d__);
			return <ConvertWithHardware>d__.<>t__builder.Task;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00005000 File Offset: 0x00003200
		public Task<IConversion> BurnSubtitle(string inputPath, string outputPath, string subtitlesPath)
		{
			Snippets.<BurnSubtitle>d__9 <BurnSubtitle>d__;
			<BurnSubtitle>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<BurnSubtitle>d__.inputPath = inputPath;
			<BurnSubtitle>d__.outputPath = outputPath;
			<BurnSubtitle>d__.subtitlesPath = subtitlesPath;
			<BurnSubtitle>d__.<>1__state = -1;
			<BurnSubtitle>d__.<>t__builder.Start<Snippets.<BurnSubtitle>d__9>(ref <BurnSubtitle>d__);
			return <BurnSubtitle>d__.<>t__builder.Task;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00005054 File Offset: 0x00003254
		public Task<IConversion> AddSubtitle(string inputPath, string outputPath, string subtitlePath, string language = null)
		{
			Snippets.<AddSubtitle>d__10 <AddSubtitle>d__;
			<AddSubtitle>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<AddSubtitle>d__.inputPath = inputPath;
			<AddSubtitle>d__.outputPath = outputPath;
			<AddSubtitle>d__.subtitlePath = subtitlePath;
			<AddSubtitle>d__.language = language;
			<AddSubtitle>d__.<>1__state = -1;
			<AddSubtitle>d__.<>t__builder.Start<Snippets.<AddSubtitle>d__10>(ref <AddSubtitle>d__);
			return <AddSubtitle>d__.<>t__builder.Task;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000050B0 File Offset: 0x000032B0
		public Task<IConversion> AddSubtitle(string inputPath, string outputPath, string subtitlePath, SubtitleCodec subtitleCodec, string language = null)
		{
			Snippets.<AddSubtitle>d__11 <AddSubtitle>d__;
			<AddSubtitle>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<AddSubtitle>d__.inputPath = inputPath;
			<AddSubtitle>d__.outputPath = outputPath;
			<AddSubtitle>d__.subtitlePath = subtitlePath;
			<AddSubtitle>d__.subtitleCodec = subtitleCodec;
			<AddSubtitle>d__.language = language;
			<AddSubtitle>d__.<>1__state = -1;
			<AddSubtitle>d__.<>t__builder.Start<Snippets.<AddSubtitle>d__11>(ref <AddSubtitle>d__);
			return <AddSubtitle>d__.<>t__builder.Task;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00005118 File Offset: 0x00003318
		public Task<IConversion> SetWatermark(string inputPath, string outputPath, string inputImage, Position position)
		{
			Snippets.<SetWatermark>d__12 <SetWatermark>d__;
			<SetWatermark>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<SetWatermark>d__.inputPath = inputPath;
			<SetWatermark>d__.outputPath = outputPath;
			<SetWatermark>d__.inputImage = inputImage;
			<SetWatermark>d__.position = position;
			<SetWatermark>d__.<>1__state = -1;
			<SetWatermark>d__.<>t__builder.Start<Snippets.<SetWatermark>d__12>(ref <SetWatermark>d__);
			return <SetWatermark>d__.<>t__builder.Task;
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00005174 File Offset: 0x00003374
		public Task<IConversion> ExtractVideo(string inputPath, string outputPath)
		{
			Snippets.<ExtractVideo>d__13 <ExtractVideo>d__;
			<ExtractVideo>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<ExtractVideo>d__.inputPath = inputPath;
			<ExtractVideo>d__.outputPath = outputPath;
			<ExtractVideo>d__.<>1__state = -1;
			<ExtractVideo>d__.<>t__builder.Start<Snippets.<ExtractVideo>d__13>(ref <ExtractVideo>d__);
			return <ExtractVideo>d__.<>t__builder.Task;
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x000051C0 File Offset: 0x000033C0
		public Task<IConversion> Snapshot(string inputPath, string outputPath, TimeSpan captureTime)
		{
			Snippets.<Snapshot>d__14 <Snapshot>d__;
			<Snapshot>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<Snapshot>d__.inputPath = inputPath;
			<Snapshot>d__.outputPath = outputPath;
			<Snapshot>d__.captureTime = captureTime;
			<Snapshot>d__.<>1__state = -1;
			<Snapshot>d__.<>t__builder.Start<Snippets.<Snapshot>d__14>(ref <Snapshot>d__);
			return <Snapshot>d__.<>t__builder.Task;
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00005214 File Offset: 0x00003414
		public Task<IConversion> ChangeSize(string inputPath, string outputPath, int width, int height)
		{
			Snippets.<ChangeSize>d__15 <ChangeSize>d__;
			<ChangeSize>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<ChangeSize>d__.inputPath = inputPath;
			<ChangeSize>d__.outputPath = outputPath;
			<ChangeSize>d__.width = width;
			<ChangeSize>d__.height = height;
			<ChangeSize>d__.<>1__state = -1;
			<ChangeSize>d__.<>t__builder.Start<Snippets.<ChangeSize>d__15>(ref <ChangeSize>d__);
			return <ChangeSize>d__.<>t__builder.Task;
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00005270 File Offset: 0x00003470
		public Task<IConversion> ChangeSize(string inputPath, string outputPath, VideoSize size)
		{
			Snippets.<ChangeSize>d__16 <ChangeSize>d__;
			<ChangeSize>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<ChangeSize>d__.inputPath = inputPath;
			<ChangeSize>d__.outputPath = outputPath;
			<ChangeSize>d__.size = size;
			<ChangeSize>d__.<>1__state = -1;
			<ChangeSize>d__.<>t__builder.Start<Snippets.<ChangeSize>d__16>(ref <ChangeSize>d__);
			return <ChangeSize>d__.<>t__builder.Task;
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x000052C4 File Offset: 0x000034C4
		public Task<IConversion> Split(string inputPath, string outputPath, TimeSpan startTime, TimeSpan duration)
		{
			Snippets.<Split>d__17 <Split>d__;
			<Split>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<Split>d__.inputPath = inputPath;
			<Split>d__.outputPath = outputPath;
			<Split>d__.startTime = startTime;
			<Split>d__.duration = duration;
			<Split>d__.<>1__state = -1;
			<Split>d__.<>t__builder.Start<Snippets.<Split>d__17>(ref <Split>d__);
			return <Split>d__.<>t__builder.Task;
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00005320 File Offset: 0x00003520
		public Task<IConversion> SaveM3U8Stream(Uri uri, string outputPath, TimeSpan? duration = null)
		{
			Snippets.<SaveM3U8Stream>d__18 <SaveM3U8Stream>d__;
			<SaveM3U8Stream>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<SaveM3U8Stream>d__.uri = uri;
			<SaveM3U8Stream>d__.outputPath = outputPath;
			<SaveM3U8Stream>d__.duration = duration;
			<SaveM3U8Stream>d__.<>1__state = -1;
			<SaveM3U8Stream>d__.<>t__builder.Start<Snippets.<SaveM3U8Stream>d__18>(ref <SaveM3U8Stream>d__);
			return <SaveM3U8Stream>d__.<>t__builder.Task;
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00005374 File Offset: 0x00003574
		public Task<IConversion> Concatenate(string output, params string[] inputVideos)
		{
			Snippets.<Concatenate>d__19 <Concatenate>d__;
			<Concatenate>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<Concatenate>d__.output = output;
			<Concatenate>d__.inputVideos = inputVideos;
			<Concatenate>d__.<>1__state = -1;
			<Concatenate>d__.<>t__builder.Start<Snippets.<Concatenate>d__19>(ref <Concatenate>d__);
			return <Concatenate>d__.<>t__builder.Task;
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000053C0 File Offset: 0x000035C0
		public Task<IConversion> Convert(string inputFilePath, string outputFilePath, bool keepSubtitles = false)
		{
			Snippets.<Convert>d__20 <Convert>d__;
			<Convert>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<Convert>d__.inputFilePath = inputFilePath;
			<Convert>d__.outputFilePath = outputFilePath;
			<Convert>d__.keepSubtitles = keepSubtitles;
			<Convert>d__.<>1__state = -1;
			<Convert>d__.<>t__builder.Start<Snippets.<Convert>d__20>(ref <Convert>d__);
			return <Convert>d__.<>t__builder.Task;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00005414 File Offset: 0x00003614
		public Task<IConversion> Transcode(string inputFilePath, string outputFilePath, VideoCodec videoCodec, AudioCodec audioCodec, SubtitleCodec subtitleCodec, bool keepSubtitles = false)
		{
			Snippets.<Transcode>d__21 <Transcode>d__;
			<Transcode>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<Transcode>d__.inputFilePath = inputFilePath;
			<Transcode>d__.outputFilePath = outputFilePath;
			<Transcode>d__.videoCodec = videoCodec;
			<Transcode>d__.audioCodec = audioCodec;
			<Transcode>d__.subtitleCodec = subtitleCodec;
			<Transcode>d__.keepSubtitles = keepSubtitles;
			<Transcode>d__.<>1__state = -1;
			<Transcode>d__.<>t__builder.Start<Snippets.<Transcode>d__21>(ref <Transcode>d__);
			return <Transcode>d__.<>t__builder.Task;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00005484 File Offset: 0x00003684
		public Task<IConversion> VisualiseAudio(string inputPath, string outputPath, VideoSize size, PixelFormat pixelFormat = PixelFormat.yuv420p, VisualisationMode mode = VisualisationMode.bar, AmplitudeScale amplitudeScale = AmplitudeScale.lin, FrequencyScale frequencyScale = FrequencyScale.log)
		{
			Snippets.<VisualiseAudio>d__22 <VisualiseAudio>d__;
			<VisualiseAudio>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<VisualiseAudio>d__.inputPath = inputPath;
			<VisualiseAudio>d__.outputPath = outputPath;
			<VisualiseAudio>d__.size = size;
			<VisualiseAudio>d__.pixelFormat = pixelFormat;
			<VisualiseAudio>d__.mode = mode;
			<VisualiseAudio>d__.amplitudeScale = amplitudeScale;
			<VisualiseAudio>d__.frequencyScale = frequencyScale;
			<VisualiseAudio>d__.<>1__state = -1;
			<VisualiseAudio>d__.<>t__builder.Start<Snippets.<VisualiseAudio>d__22>(ref <VisualiseAudio>d__);
			return <VisualiseAudio>d__.<>t__builder.Task;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x000054FC File Offset: 0x000036FC
		public Task<IConversion> SendToRtspServer(string inputFilePath, Uri rtspServerUri)
		{
			Snippets.<SendToRtspServer>d__23 <SendToRtspServer>d__;
			<SendToRtspServer>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<SendToRtspServer>d__.inputFilePath = inputFilePath;
			<SendToRtspServer>d__.rtspServerUri = rtspServerUri;
			<SendToRtspServer>d__.<>1__state = -1;
			<SendToRtspServer>d__.<>t__builder.Start<Snippets.<SendToRtspServer>d__23>(ref <SendToRtspServer>d__);
			return <SendToRtspServer>d__.<>t__builder.Task;
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00005548 File Offset: 0x00003748
		public Task<IConversion> SendDesktopToRtspServer(Uri rtspServerUri)
		{
			Snippets.<SendDesktopToRtspServer>d__24 <SendDesktopToRtspServer>d__;
			<SendDesktopToRtspServer>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<SendDesktopToRtspServer>d__.rtspServerUri = rtspServerUri;
			<SendDesktopToRtspServer>d__.<>1__state = -1;
			<SendDesktopToRtspServer>d__.<>t__builder.Start<Snippets.<SendDesktopToRtspServer>d__24>(ref <SendDesktopToRtspServer>d__);
			return <SendDesktopToRtspServer>d__.<>t__builder.Task;
		}
	}
}
