using System;
using System.Collections.Generic;
using System.Linq;
using Xabe.FFmpeg.Streams;

namespace Xabe.FFmpeg
{
	// Token: 0x0200003D RID: 61
	public class VideoStream : IVideoStream, IStream, IFilterable
	{
		// Token: 0x060001C9 RID: 457 RVA: 0x000064A7 File Offset: 0x000046A7
		internal VideoStream()
		{
		}

		// Token: 0x060001CA RID: 458 RVA: 0x000064C5 File Offset: 0x000046C5
		public IEnumerable<IFilterConfiguration> GetFilters()
		{
			if (Enumerable.Any<KeyValuePair<string, string>>(this._videoFilters))
			{
				yield return new FilterConfiguration
				{
					FilterType = "-filter_complex",
					StreamNumber = this.Index,
					Filters = this._videoFilters
				};
			}
			yield break;
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001CB RID: 459 RVA: 0x000064D5 File Offset: 0x000046D5
		// (set) Token: 0x060001CC RID: 460 RVA: 0x000064DD File Offset: 0x000046DD
		public int Width { get; internal set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001CD RID: 461 RVA: 0x000064E6 File Offset: 0x000046E6
		// (set) Token: 0x060001CE RID: 462 RVA: 0x000064EE File Offset: 0x000046EE
		public int Height { get; internal set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001CF RID: 463 RVA: 0x000064F7 File Offset: 0x000046F7
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x000064FF File Offset: 0x000046FF
		public double Framerate { get; internal set; }

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x00006508 File Offset: 0x00004708
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x00006510 File Offset: 0x00004710
		public string Ratio { get; internal set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x00006519 File Offset: 0x00004719
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x00006521 File Offset: 0x00004721
		public TimeSpan Duration { get; internal set; }

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x0000652A File Offset: 0x0000472A
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x00006532 File Offset: 0x00004732
		public string Codec { get; internal set; }

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x0000653B File Offset: 0x0000473B
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x00006543 File Offset: 0x00004743
		public int Index { get; internal set; }

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x0000654C File Offset: 0x0000474C
		// (set) Token: 0x060001DA RID: 474 RVA: 0x00006554 File Offset: 0x00004754
		public string Path { get; internal set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001DB RID: 475 RVA: 0x0000655D File Offset: 0x0000475D
		// (set) Token: 0x060001DC RID: 476 RVA: 0x00006565 File Offset: 0x00004765
		public int? Default { get; internal set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001DD RID: 477 RVA: 0x0000656E File Offset: 0x0000476E
		// (set) Token: 0x060001DE RID: 478 RVA: 0x00006576 File Offset: 0x00004776
		public int? Forced { get; internal set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001DF RID: 479 RVA: 0x0000657F File Offset: 0x0000477F
		// (set) Token: 0x060001E0 RID: 480 RVA: 0x00006587 File Offset: 0x00004787
		public string PixelFormat { get; internal set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x00006590 File Offset: 0x00004790
		// (set) Token: 0x060001E2 RID: 482 RVA: 0x00006598 File Offset: 0x00004798
		public int? Rotation { get; internal set; }

		// Token: 0x060001E3 RID: 483 RVA: 0x000065A4 File Offset: 0x000047A4
		public string BuildParameters(ParameterPosition forPosition)
		{
			ParametersList<ConversionParameter> parameters = this._parameters;
			IEnumerable<ConversionParameter> enumerable = (parameters != null) ? Enumerable.Where<ConversionParameter>(parameters, (ConversionParameter x) => x.Position == forPosition) : null;
			if (enumerable != null && Enumerable.Any<ConversionParameter>(enumerable))
			{
				return string.Join(string.Empty, Enumerable.Select<ConversionParameter, string>(enumerable, (ConversionParameter x) => x.Parameter));
			}
			return string.Empty;
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000661D File Offset: 0x0000481D
		public IVideoStream ChangeSpeed(double multiplication)
		{
			this._videoFilters["setpts"] = this.GetVideoSpeedFilter(multiplication);
			return this;
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x00006638 File Offset: 0x00004838
		private string GetVideoSpeedFilter(double multiplication)
		{
			if (multiplication < 0.5 || multiplication > 2.0)
			{
				throw new ArgumentOutOfRangeException("multiplication", "Value has to be greater than 0.5 and less than 2.0.");
			}
			return ((multiplication >= 1.0) ? (1.0 - (multiplication - 1.0) / 2.0) : (1.0 + (multiplication - 1.0) * -2.0)).ToFFmpegFormat(1) + "*PTS ";
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x000066CC File Offset: 0x000048CC
		public IVideoStream Rotate(RotateDegrees rotateDegrees)
		{
			string parameter = (rotateDegrees == RotateDegrees.Invert) ? "-vf \"transpose=2,transpose=2\" " : string.Format("-vf \"transpose={0}\" ", (int)rotateDegrees);
			this._parameters.Add(new ConversionParameter(parameter, ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00006708 File Offset: 0x00004908
		public IVideoStream Pad(int width, int height)
		{
			string parameter = string.Format("-vf \"scale={0}:{1}:force_original_aspect_ratio=decrease,pad={2}:{3}:-1:-1:color=black\"", new object[]
			{
				width,
				height,
				width,
				height
			});
			this._parameters.Add(new ConversionParameter(parameter, ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000675D File Offset: 0x0000495D
		public StreamType StreamType
		{
			get
			{
				return StreamType.Video;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001E9 RID: 489 RVA: 0x00006760 File Offset: 0x00004960
		// (set) Token: 0x060001EA RID: 490 RVA: 0x00006768 File Offset: 0x00004968
		public long Bitrate { get; internal set; }

		// Token: 0x060001EB RID: 491 RVA: 0x00006771 File Offset: 0x00004971
		public IVideoStream CopyStream()
		{
			return this.SetCodec(VideoCodec.copy);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x00006780 File Offset: 0x00004980
		public IVideoStream SetLoop(int count, int delay)
		{
			this._parameters.Add(new ConversionParameter(string.Format("-loop {0}", count), ParameterPosition.PostInput));
			if (delay > 0)
			{
				this._parameters.Add(new ConversionParameter(string.Format("-final_delay {0}", delay / 100), ParameterPosition.PostInput));
			}
			return this;
		}

		// Token: 0x060001ED RID: 493 RVA: 0x000067D7 File Offset: 0x000049D7
		public IVideoStream AddSubtitles(string subtitlePath, VideoSize originalSize, string encode, string style)
		{
			return this.BuildSubtitleFilter(subtitlePath, new VideoSize?(originalSize), encode, style);
		}

		// Token: 0x060001EE RID: 494 RVA: 0x000067EC File Offset: 0x000049EC
		public IVideoStream AddSubtitles(string subtitlePath, string encode, string style)
		{
			return this.BuildSubtitleFilter(subtitlePath, default(VideoSize?), encode, style);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000680C File Offset: 0x00004A0C
		private IVideoStream BuildSubtitleFilter(string subtitlePath, VideoSize? originalSize, string encode, string style)
		{
			string text = ("'" + subtitlePath + "'").Replace("\\", "\\\\").Replace(":", "\\:");
			if (!string.IsNullOrEmpty(encode))
			{
				text = text + ":charenc=" + encode;
			}
			if (!string.IsNullOrEmpty(style))
			{
				text = text + ":force_style='" + style + "'";
			}
			if (originalSize != null)
			{
				text = text + ":original_size=" + originalSize.Value.ToFFmpegFormat();
			}
			this._videoFilters.Add("subtitles", text);
			return this;
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x000068AC File Offset: 0x00004AAC
		public IVideoStream Reverse()
		{
			this._parameters.Add(new ConversionParameter("-vf reverse", ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x000068C5 File Offset: 0x00004AC5
		public IVideoStream SetBitrate(long bitrate)
		{
			this._parameters.Add(new ConversionParameter(string.Format("-b:v {0}", bitrate), ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x000068EC File Offset: 0x00004AEC
		public IVideoStream SetBitrate(long minBitrate, long maxBitrate, long bufferSize)
		{
			this._parameters.Add(new ConversionParameter(string.Format("-b:v {0}", minBitrate), ParameterPosition.PostInput));
			this._parameters.Add(new ConversionParameter(string.Format("-maxrate {0}", maxBitrate), ParameterPosition.PostInput));
			this._parameters.Add(new ConversionParameter(string.Format("-bufsize {0}", bufferSize), ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000695D File Offset: 0x00004B5D
		public IVideoStream SetFlags(params Flag[] flags)
		{
			return this.SetFlags(Enumerable.ToArray<string>(Enumerable.Select<Flag, string>(flags, (Flag x) => x.ToString())));
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x00006990 File Offset: 0x00004B90
		public IVideoStream SetFlags(params string[] flags)
		{
			string text = string.Join("+", flags);
			if (text.get_Chars(0) != '+')
			{
				text = "+" + text;
			}
			this._parameters.Add(new ConversionParameter("-flags " + text, ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x000069DD File Offset: 0x00004BDD
		public IVideoStream SetFramerate(double framerate)
		{
			this._parameters.Add(new ConversionParameter("-r " + framerate.ToFFmpegFormat(3), ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x00006A02 File Offset: 0x00004C02
		public IVideoStream SetSize(VideoSize size)
		{
			this._parameters.Add(new ConversionParameter("-s " + size.ToFFmpegFormat(), ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x00006A26 File Offset: 0x00004C26
		public IVideoStream SetSize(int width, int height)
		{
			this._parameters.Add(new ConversionParameter(string.Format("-s {0}x{1}", width, height), ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x00006A50 File Offset: 0x00004C50
		public IVideoStream SetCodec(VideoCodec codec)
		{
			string text = codec.ToString();
			if (codec == VideoCodec._8bps)
			{
				text = "8bps";
			}
			else if (codec == VideoCodec._4xm)
			{
				text = "4xm";
			}
			else if (codec == VideoCodec._012v)
			{
				text = "012v";
			}
			return this.SetCodec(text ?? "");
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x00006A9C File Offset: 0x00004C9C
		public IVideoStream SetCodec(string codec)
		{
			this._parameters.Add(new ConversionParameter("-c:v " + codec, ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00006ABB File Offset: 0x00004CBB
		public IVideoStream SetBitstreamFilter(BitstreamFilter filter)
		{
			return this.SetBitstreamFilter(string.Format("{0}", filter));
		}

		// Token: 0x060001FB RID: 507 RVA: 0x00006AD3 File Offset: 0x00004CD3
		public IVideoStream SetBitstreamFilter(string filter)
		{
			this._parameters.Add(new ConversionParameter("-bsf:v " + filter, ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x00006AF4 File Offset: 0x00004CF4
		public IVideoStream SetSeek(TimeSpan seek)
		{
			if (seek > this.Duration)
			{
				throw new ArgumentException("Seek can not be greater than video duration. Seek: " + seek.TotalSeconds.ToString() + " Duration: " + this.Duration.TotalSeconds.ToString());
			}
			this._parameters.Add(new ConversionParameter("-ss " + seek.ToFFmpeg(), ParameterPosition.PreInput));
			return this;
		}

		// Token: 0x060001FD RID: 509 RVA: 0x00006B6B File Offset: 0x00004D6B
		public IVideoStream SetOutputFramesCount(int number)
		{
			this._parameters.Add(new ConversionParameter(string.Format("-frames:v {0}", number), ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x060001FE RID: 510 RVA: 0x00006B90 File Offset: 0x00004D90
		public IVideoStream SetWatermark(string imagePath, Position position)
		{
			this._watermarkSource = imagePath;
			string text = string.Empty;
			switch (position)
			{
			case Position.UpperLeft:
				text += "5:5";
				break;
			case Position.UpperRight:
				text += "(main_w-overlay_w):5";
				break;
			case Position.Right:
				text += "(main_w-overlay_w-5):(main_h-overlay_h)/2";
				break;
			case Position.Left:
				text += "5:(main_h-overlay_h)/2";
				break;
			case Position.Up:
				text += "(main_w-overlay_w)/2:5";
				break;
			case Position.BottomRight:
				text += "(main_w-overlay_w):main_h-overlay_h";
				break;
			case Position.BottomLeft:
				text += "5:main_h-overlay_h";
				break;
			case Position.Center:
				text += "x=(main_w-overlay_w)/2:y=(main_h-overlay_h)/2";
				break;
			case Position.Bottom:
				text += "(main_w-overlay_w)/2:main_h-overlay_h";
				break;
			}
			this._videoFilters["overlay"] = text;
			return this;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x00006C64 File Offset: 0x00004E64
		public IVideoStream Split(TimeSpan startTime, TimeSpan duration)
		{
			this._parameters.Add(new ConversionParameter("-ss " + startTime.ToFFmpeg(), ParameterPosition.PostInput));
			this._parameters.Add(new ConversionParameter("-t " + duration.ToFFmpeg(), ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x00006CB4 File Offset: 0x00004EB4
		public IEnumerable<string> GetSource()
		{
			if (!string.IsNullOrWhiteSpace(this._watermarkSource))
			{
				return new string[]
				{
					this.Path,
					this._watermarkSource
				};
			}
			return new string[]
			{
				this.Path
			};
		}

		// Token: 0x06000201 RID: 513 RVA: 0x00006CEC File Offset: 0x00004EEC
		public IVideoStream SetInputFormat(Format inputFormat)
		{
			string inputFormat2 = inputFormat.ToString();
			switch (inputFormat)
			{
			case Format._3dostr:
				inputFormat2 = "3dostr";
				break;
			case Format._3g2:
				inputFormat2 = "3g2";
				break;
			case Format._3gp:
				inputFormat2 = "3gp";
				break;
			case Format._4xm:
				inputFormat2 = "4xm";
				break;
			}
			return this.SetInputFormat(inputFormat2);
		}

		// Token: 0x06000202 RID: 514 RVA: 0x00006D44 File Offset: 0x00004F44
		public IVideoStream SetInputFormat(string format)
		{
			if (format != null)
			{
				this._parameters.Add(new ConversionParameter("-f " + format, ParameterPosition.PreInput));
			}
			return this;
		}

		// Token: 0x06000203 RID: 515 RVA: 0x00006D66 File Offset: 0x00004F66
		public IVideoStream AddParameter(string parameter, ParameterPosition parameterPosition = ParameterPosition.PostInput)
		{
			this._parameters.Add(new ConversionParameter(parameter, parameterPosition));
			return this;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x00006D7B File Offset: 0x00004F7B
		public IVideoStream UseNativeInputRead(bool readInputAtNativeFrameRate)
		{
			this._parameters.Add(new ConversionParameter("-re", ParameterPosition.PreInput));
			return this;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x00006D94 File Offset: 0x00004F94
		public IVideoStream SetStreamLoop(int loopCount)
		{
			this._parameters.Add(new ConversionParameter(string.Format("-stream_loop {0}", loopCount), ParameterPosition.PreInput));
			return this;
		}

		// Token: 0x040004EB RID: 1259
		private readonly ParametersList<ConversionParameter> _parameters = new ParametersList<ConversionParameter>();

		// Token: 0x040004EC RID: 1260
		private readonly Dictionary<string, string> _videoFilters = new Dictionary<string, string>();

		// Token: 0x040004ED RID: 1261
		private string _watermarkSource;
	}
}
