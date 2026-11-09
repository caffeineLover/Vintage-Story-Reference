using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Xabe.FFmpeg
{
	// Token: 0x02000023 RID: 35
	internal sealed class FFprobeWrapper : FFmpeg
	{
		// Token: 0x060000F4 RID: 244 RVA: 0x0000560C File Offset: 0x0000380C
		private Task<ProbeModel.Stream[]> GetStreams(string videoPath, CancellationToken cancellationToken)
		{
			FFprobeWrapper.<GetStreams>d__1 <GetStreams>d__;
			<GetStreams>d__.<>t__builder = AsyncTaskMethodBuilder<ProbeModel.Stream[]>.Create();
			<GetStreams>d__.<>4__this = this;
			<GetStreams>d__.videoPath = videoPath;
			<GetStreams>d__.cancellationToken = cancellationToken;
			<GetStreams>d__.<>1__state = -1;
			<GetStreams>d__.<>t__builder.Start<FFprobeWrapper.<GetStreams>d__1>(ref <GetStreams>d__);
			return <GetStreams>d__.<>t__builder.Task;
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00005660 File Offset: 0x00003860
		private double GetVideoFramerate(ProbeModel.Stream vid)
		{
			long frameCount = this.GetFrameCount(vid);
			double duration = vid.Duration;
			string[] array = vid.RFrameRate.Split(new char[]
			{
				'/'
			});
			if (frameCount > 0L)
			{
				return Math.Round((double)frameCount / duration, 3);
			}
			return Math.Round(double.Parse(array[0]) / double.Parse(array[1]), 3);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x000056BC File Offset: 0x000038BC
		private long GetFrameCount(ProbeModel.Stream vid)
		{
			long result;
			if (!long.TryParse(vid.NbFrames, ref result))
			{
				return 0L;
			}
			return result;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x000056DC File Offset: 0x000038DC
		private string GetVideoAspectRatio(int width, int height)
		{
			int gcd = this.GetGcd(width, height);
			if (gcd <= 0)
			{
				return "0:0";
			}
			return (width / gcd).ToString() + ":" + (height / gcd).ToString();
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000571C File Offset: 0x0000391C
		private Task<FormatModel.Root> GetInfos(string videoPath, CancellationToken cancellationToken)
		{
			FFprobeWrapper.<GetInfos>d__5 <GetInfos>d__;
			<GetInfos>d__.<>t__builder = AsyncTaskMethodBuilder<FormatModel.Root>.Create();
			<GetInfos>d__.<>4__this = this;
			<GetInfos>d__.videoPath = videoPath;
			<GetInfos>d__.cancellationToken = cancellationToken;
			<GetInfos>d__.<>1__state = -1;
			<GetInfos>d__.<>t__builder.Start<FFprobeWrapper.<GetInfos>d__5>(ref <GetInfos>d__);
			return <GetInfos>d__.<>t__builder.Task;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000576F File Offset: 0x0000396F
		private TimeSpan GetAudioDuration(ProbeModel.Stream audio)
		{
			return TimeSpan.FromSeconds(audio.Duration);
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000577C File Offset: 0x0000397C
		private TimeSpan GetVideoDuration(ProbeModel.Stream video, FormatModel.Format format)
		{
			return TimeSpan.FromSeconds((video.Duration > 0.01) ? video.Duration : format.duration);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x000057A2 File Offset: 0x000039A2
		private int GetGcd(int width, int height)
		{
			while (width != 0 && height != 0)
			{
				if (width > height)
				{
					width -= height;
				}
				else
				{
					height -= width;
				}
			}
			if (width != 0)
			{
				return width;
			}
			return height;
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000057C2 File Offset: 0x000039C2
		public Task<string> Start(string args, CancellationToken cancellationToken)
		{
			return this.RunProcess(args, cancellationToken);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x000057CC File Offset: 0x000039CC
		private Task<string> RunProcess(string args, CancellationToken cancellationToken)
		{
			FFprobeWrapper.<RunProcess>d__10 <RunProcess>d__;
			<RunProcess>d__.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
			<RunProcess>d__.<>4__this = this;
			<RunProcess>d__.args = args;
			<RunProcess>d__.cancellationToken = cancellationToken;
			<RunProcess>d__.<>1__state = -1;
			<RunProcess>d__.<>t__builder.Start<FFprobeWrapper.<RunProcess>d__10>(ref <RunProcess>d__);
			return <RunProcess>d__.<>t__builder.Task;
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00005820 File Offset: 0x00003A20
		public Task<MediaInfo> SetProperties(MediaInfo mediaInfo, CancellationToken cancellationToken)
		{
			FFprobeWrapper.<SetProperties>d__11 <SetProperties>d__;
			<SetProperties>d__.<>t__builder = AsyncTaskMethodBuilder<MediaInfo>.Create();
			<SetProperties>d__.<>4__this = this;
			<SetProperties>d__.mediaInfo = mediaInfo;
			<SetProperties>d__.cancellationToken = cancellationToken;
			<SetProperties>d__.<>1__state = -1;
			<SetProperties>d__.<>t__builder.Start<FFprobeWrapper.<SetProperties>d__11>(ref <SetProperties>d__);
			return <SetProperties>d__.<>t__builder.Task;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00005874 File Offset: 0x00003A74
		private static TimeSpan CalculateDuration(IEnumerable<IVideoStream> videoStreams, IEnumerable<IAudioStream> audioStreams)
		{
			double num;
			if (!Enumerable.Any<IAudioStream>(audioStreams))
			{
				num = 0.0;
			}
			else
			{
				num = Enumerable.Max<IAudioStream>(audioStreams, (IAudioStream x) => x.Duration.TotalSeconds);
			}
			double num2;
			if (!Enumerable.Any<IVideoStream>(videoStreams))
			{
				num2 = 0.0;
			}
			else
			{
				num2 = Enumerable.Max<IVideoStream>(videoStreams, (IVideoStream x) => x.Duration.TotalSeconds);
			}
			double num3 = num2;
			return TimeSpan.FromSeconds(Math.Max(num, num3));
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00005900 File Offset: 0x00003B00
		private IEnumerable<IAudioStream> PrepareAudioStreams(string path, IEnumerable<ProbeModel.Stream> audioStreamModels)
		{
			return Enumerable.Select<ProbeModel.Stream, AudioStream>(audioStreamModels, delegate(ProbeModel.Stream model)
			{
				AudioStream audioStream = new AudioStream();
				audioStream.Codec = model.CodecName;
				audioStream.Duration = this.GetAudioDuration(model);
				audioStream.Path = path;
				audioStream.Index = model.Index;
				audioStream.Bitrate = Math.Abs(model.BitRate);
				audioStream.Channels = model.Channels;
				audioStream.SampleRate = model.SampleRate;
				ProbeModel.Tags tags = model.Tags;
				audioStream.Language = ((tags != null) ? tags.Language : null);
				ProbeModel.Disposition disposition = model.Disposition;
				audioStream.Default = ((disposition != null) ? new int?(disposition.Default) : default(int?));
				ProbeModel.Tags tags2 = model.Tags;
				audioStream.Title = ((tags2 != null) ? tags2.Title : null);
				ProbeModel.Disposition disposition2 = model.Disposition;
				audioStream.Forced = ((disposition2 != null) ? new int?(disposition2.Forced) : default(int?));
				return audioStream;
			});
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00005934 File Offset: 0x00003B34
		private static IEnumerable<ISubtitleStream> PrepareSubtitleStreams(string path, IEnumerable<ProbeModel.Stream> subtitleStreamModels)
		{
			return Enumerable.Select<ProbeModel.Stream, SubtitleStream>(subtitleStreamModels, delegate(ProbeModel.Stream model)
			{
				SubtitleStream subtitleStream = new SubtitleStream();
				subtitleStream.Codec = model.CodecName;
				subtitleStream.Path = path;
				subtitleStream.Index = model.Index;
				ProbeModel.Tags tags = model.Tags;
				subtitleStream.Language = ((tags != null) ? tags.Language : null);
				ProbeModel.Tags tags2 = model.Tags;
				subtitleStream.Title = ((tags2 != null) ? tags2.Title : null);
				ProbeModel.Disposition disposition = model.Disposition;
				subtitleStream.Default = ((disposition != null) ? new int?(disposition.Default) : default(int?));
				ProbeModel.Disposition disposition2 = model.Disposition;
				subtitleStream.Forced = ((disposition2 != null) ? new int?(disposition2.Forced) : default(int?));
				return subtitleStream;
			});
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00005960 File Offset: 0x00003B60
		private IEnumerable<IVideoStream> PrepareVideoStreams(string path, IEnumerable<ProbeModel.Stream> videoStreamModels, FormatModel.Format format)
		{
			return Enumerable.Select<ProbeModel.Stream, VideoStream>(videoStreamModels, delegate(ProbeModel.Stream model)
			{
				VideoStream videoStream = new VideoStream();
				videoStream.Codec = model.CodecName;
				videoStream.Duration = this.GetVideoDuration(model, format);
				videoStream.Width = model.Width;
				videoStream.Height = model.Height;
				videoStream.Framerate = this.GetVideoFramerate(model);
				videoStream.Ratio = this.GetVideoAspectRatio(model.Width, model.Height);
				videoStream.Path = path;
				videoStream.Index = model.Index;
				videoStream.Bitrate = (((double)Math.Abs(model.BitRate) > 0.01) ? model.BitRate : format.bit_Rate);
				videoStream.PixelFormat = model.PixFmt;
				ProbeModel.Disposition disposition = model.Disposition;
				videoStream.Default = ((disposition != null) ? new int?(disposition.Default) : default(int?));
				ProbeModel.Disposition disposition2 = model.Disposition;
				videoStream.Forced = ((disposition2 != null) ? new int?(disposition2.Forced) : default(int?));
				ProbeModel.Tags tags = model.Tags;
				videoStream.Rotation = ((tags != null) ? tags.Rotate : default(int?));
				return videoStream;
			});
		}

		// Token: 0x0400029A RID: 666
		private readonly JsonSerializerOptions _defaultSerializerOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
			NumberHandling = 1,
			WriteIndented = true
		};
	}
}
