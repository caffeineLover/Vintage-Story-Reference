using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xabe.FFmpeg.Events;
using Xabe.FFmpeg.Streams;
using Xabe.FFmpeg.Streams.SubtitleStream;

namespace Xabe.FFmpeg
{
	// Token: 0x02000005 RID: 5
	public class Conversion : IConversion
	{
		// Token: 0x06000006 RID: 6 RVA: 0x00002228 File Offset: 0x00000428
		public Conversion()
		{
			this._userDefinedParameters[ParameterPosition.PostInput] = new List<string>();
			this._userDefinedParameters[ParameterPosition.PreInput] = new List<string>();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002294 File Offset: 0x00000494
		public string Build()
		{
			object builderLock = this._builderLock;
			string result;
			lock (builderLock)
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (this._buildOutputFileName == null)
				{
					this._buildOutputFileName = ((string number) => this._output);
				}
				stringBuilder.Append(string.Join(" ", Enumerable.Select<string, string>(this._userDefinedParameters[ParameterPosition.PreInput], (string x) => x.Trim())) + " ");
				stringBuilder.Append(this.GetParameters(ParameterPosition.PreInput));
				stringBuilder.Append(this.GetStreamsPreInputs());
				if (this._buildInputFileName == null)
				{
					stringBuilder.Append(this.GetInputs());
				}
				else
				{
					this._hasInputBuilder = true;
					stringBuilder.Append(this._buildInputFileName.Invoke("_%03d"));
					stringBuilder.Append(this.GetInputs());
				}
				stringBuilder.Append(this.GetStreamsPostInputs());
				stringBuilder.Append(this.GetFilters());
				stringBuilder.Append(this.GetMap());
				stringBuilder.Append(this.GetParameters(ParameterPosition.PostInput));
				stringBuilder.Append(string.Join(" ", Enumerable.Select<string, string>(this._userDefinedParameters[ParameterPosition.PostInput], (string x) => x.Trim())) + " ");
				stringBuilder.Append(this._buildOutputFileName.Invoke("_%03d"));
				result = stringBuilder.ToString();
			}
			return result;
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000008 RID: 8 RVA: 0x00002444 File Offset: 0x00000644
		// (remove) Token: 0x06000009 RID: 9 RVA: 0x0000247C File Offset: 0x0000067C
		public event ConversionProgressEventHandler OnProgress;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x0600000A RID: 10 RVA: 0x000024B4 File Offset: 0x000006B4
		// (remove) Token: 0x0600000B RID: 11 RVA: 0x000024EC File Offset: 0x000006EC
		public event DataReceivedEventHandler OnDataReceived;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600000C RID: 12 RVA: 0x00002524 File Offset: 0x00000724
		// (remove) Token: 0x0600000D RID: 13 RVA: 0x0000255C File Offset: 0x0000075C
		public event VideoDataEventHandler OnVideoDataReceived;

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x0600000E RID: 14 RVA: 0x00002591 File Offset: 0x00000791
		// (set) Token: 0x0600000F RID: 15 RVA: 0x00002599 File Offset: 0x00000799
		public string OutputFilePath { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000010 RID: 16 RVA: 0x000025A2 File Offset: 0x000007A2
		// (set) Token: 0x06000011 RID: 17 RVA: 0x000025AA File Offset: 0x000007AA
		public PipeDescriptor? OutputPipeDescriptor { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000012 RID: 18 RVA: 0x000025B3 File Offset: 0x000007B3
		public IEnumerable<IStream> Streams
		{
			get
			{
				return this._streams;
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x000025BB File Offset: 0x000007BB
		public Task<IConversionResult> Start()
		{
			return this.Start(this.Build());
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000025C9 File Offset: 0x000007C9
		public Task<IConversionResult> Start(CancellationToken cancellationToken)
		{
			return this.Start(this.Build(), cancellationToken);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000025D8 File Offset: 0x000007D8
		public Task<IConversionResult> Start(string parameters)
		{
			return this.Start(parameters, default(CancellationToken));
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000025F8 File Offset: 0x000007F8
		public Task<IConversionResult> Start(string parameters, CancellationToken cancellationToken)
		{
			Conversion.<Start>d__35 <Start>d__;
			<Start>d__.<>t__builder = AsyncTaskMethodBuilder<IConversionResult>.Create();
			<Start>d__.<>4__this = this;
			<Start>d__.parameters = parameters;
			<Start>d__.cancellationToken = cancellationToken;
			<Start>d__.<>1__state = -1;
			<Start>d__.<>t__builder.Start<Conversion.<Start>d__35>(ref <Start>d__);
			return <Start>d__.<>t__builder.Task;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000264C File Offset: 0x0000084C
		private void CreateOutputDirectoryIfNotExists()
		{
			if (this.OutputFilePath == null || this.OutputPipeDescriptor != null)
			{
				return;
			}
			try
			{
				if (!Directory.Exists(Path.GetDirectoryName(this.OutputFilePath.Unescape())))
				{
					Directory.CreateDirectory(Path.GetDirectoryName(this.OutputFilePath.Unescape()));
				}
			}
			catch (IOException)
			{
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000026B8 File Offset: 0x000008B8
		public IConversion AddParameter(string parameter, ParameterPosition parameterPosition = ParameterPosition.PostInput)
		{
			this._userDefinedParameters[parameterPosition].Add(parameter);
			return this;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x000026D0 File Offset: 0x000008D0
		public IConversion AddStream<T>(params T[] streams) where T : IStream
		{
			foreach (T t in streams)
			{
				if (t != null)
				{
					this._streams.Add(t);
				}
			}
			return this;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002710 File Offset: 0x00000910
		public IConversion AddStream(IEnumerable<IStream> streams)
		{
			foreach (IStream stream in streams)
			{
				this.AddStream<IStream>(new IStream[]
				{
					stream
				});
			}
			return this;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002764 File Offset: 0x00000964
		public IConversion SetHashFormat(Hash hashFormat = Hash.SHA256)
		{
			string hashFormat2 = hashFormat.ToString();
			if (hashFormat == Hash.SHA512_256)
			{
				hashFormat2 = "SHA512/256";
			}
			else if (hashFormat == Hash.SHA512_224)
			{
				hashFormat2 = "SHA512/224";
			}
			this.SetOutputFormat(Format.hash);
			return this.SetHashFormat(hashFormat2);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x000027A7 File Offset: 0x000009A7
		public IConversion SetHashFormat(string hashFormat)
		{
			this._parameters.Add(new ConversionParameter("-hash " + hashFormat, ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x0600001D RID: 29 RVA: 0x000027C6 File Offset: 0x000009C6
		public IConversion SetPreset(ConversionPreset preset)
		{
			this._parameters.Add(new ConversionParameter("-preset " + preset.ToString().ToLower(), ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000027F6 File Offset: 0x000009F6
		public IConversion SetSeek(TimeSpan? seek)
		{
			if (seek != null)
			{
				this._parameters.Add(new ConversionParameter("-ss " + seek.Value.ToFFmpeg(), ParameterPosition.PostInput));
			}
			return this;
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002829 File Offset: 0x00000A29
		public IConversion SetInputTime(TimeSpan? time)
		{
			if (time != null)
			{
				this._parameters.Add(new ConversionParameter("-t " + time.Value.ToFFmpeg(), ParameterPosition.PreInput));
			}
			return this;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x0000285C File Offset: 0x00000A5C
		public IConversion SetOutputTime(TimeSpan? time)
		{
			if (time != null)
			{
				this._parameters.Add(new ConversionParameter("-t " + time.Value.ToFFmpeg(), ParameterPosition.PostInput));
			}
			return this;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002890 File Offset: 0x00000A90
		public IConversion UseMultiThread(bool multiThread)
		{
			int num = multiThread ? Environment.ProcessorCount : 1;
			this._parameters.Add(new ConversionParameter(string.Format("-threads {0}", Math.Min(num, 16)), ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000028D2 File Offset: 0x00000AD2
		public IConversion UseMultiThread(int threadsCount)
		{
			this._parameters.Add(new ConversionParameter(string.Format("-threads {0}", threadsCount), ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000028F6 File Offset: 0x00000AF6
		public IConversion SetOutput(string outputPath)
		{
			this.OutputFilePath = new FileInfo(outputPath).FullName;
			this._output = outputPath.Escape();
			return this;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002916 File Offset: 0x00000B16
		public IConversion PipeOutput(PipeDescriptor descriptor = PipeDescriptor.stdout)
		{
			this.SetOutput(string.Format("pipe:{0}", (int)descriptor));
			this.OutputPipeDescriptor = new PipeDescriptor?(descriptor);
			return this;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000293C File Offset: 0x00000B3C
		public IConversion SetVideoBitrate(long bitrate)
		{
			this._parameters.Add(new ConversionParameter(string.Format("-b:v {0}", bitrate), ParameterPosition.PostInput));
			this._parameters.Add(new ConversionParameter(string.Format("-minrate {0}", bitrate), ParameterPosition.PostInput));
			this._parameters.Add(new ConversionParameter(string.Format("-maxrate {0}", bitrate), ParameterPosition.PostInput));
			this._parameters.Add(new ConversionParameter(string.Format("-bufsize {0}", bitrate), ParameterPosition.PostInput));
			if (this.HasH264Stream())
			{
				this._parameters.Add(new ConversionParameter("-x264opts nal-hrd=cbr:force-cfr=1", ParameterPosition.PostInput));
			}
			return this;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000029EC File Offset: 0x00000BEC
		public IConversion SetAudioBitrate(long bitrate)
		{
			this._parameters.Add(new ConversionParameter(string.Format("-b:a {0}", bitrate), ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002A10 File Offset: 0x00000C10
		public IConversion UseShortest(bool useShortest)
		{
			if (useShortest)
			{
				this._parameters.Add(new ConversionParameter("-shortest", ParameterPosition.PostInput));
			}
			else
			{
				this._parameters.Remove(new ConversionParameter("-shortest", ParameterPosition.PostInput));
			}
			return this;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002A44 File Offset: 0x00000C44
		public IConversion SetPriority(ProcessPriorityClass? priority)
		{
			this._priority = priority;
			return this;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002A50 File Offset: 0x00000C50
		public IConversion ExtractEveryNthFrame(int frameNo, Func<string, string> buildOutputFileName)
		{
			this._buildOutputFileName = buildOutputFileName;
			this.OutputFilePath = buildOutputFileName.Invoke("");
			this._parameters.Add(new ConversionParameter(string.Format("-vf select='not(mod(n\\,{0}))'", frameNo), ParameterPosition.PostInput));
			this.SetVideoSyncMethod(VideoSyncMethod.vfr);
			return this;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002AA0 File Offset: 0x00000CA0
		public IConversion ExtractNthFrame(int frameNo, Func<string, string> buildOutputFileName)
		{
			this._buildOutputFileName = buildOutputFileName;
			this._parameters.Add(new ConversionParameter(string.Format("-vf select='eq(n\\,{0})'", frameNo), ParameterPosition.PostInput));
			this.OutputFilePath = buildOutputFileName.Invoke("");
			this.SetVideoSyncMethod(VideoSyncMethod.passthrough);
			return this;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002AEF File Offset: 0x00000CEF
		public IConversion BuildVideoFromImages(int startNumber, Func<string, string> buildInputFileName)
		{
			this._buildInputFileName = buildInputFileName;
			this._parameters.Add(new ConversionParameter(string.Format("-start_number {0}", startNumber), ParameterPosition.PreInput));
			return this;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002B1C File Offset: 0x00000D1C
		public IConversion BuildVideoFromImages(IEnumerable<string> imageFiles)
		{
			InputBuilder inputBuilder = new InputBuilder();
			string text;
			this._buildInputFileName = inputBuilder.PrepareInputFiles(Enumerable.ToList<string>(imageFiles), out text);
			return this;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002B44 File Offset: 0x00000D44
		public IConversion SetInputFrameRate(double frameRate)
		{
			this._parameters.Add(new ConversionParameter("-framerate " + frameRate.ToFFmpegFormat(3), ParameterPosition.PreInput));
			this._parameters.Add(new ConversionParameter("-r " + frameRate.ToFFmpegFormat(3), ParameterPosition.PreInput));
			return this;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002B98 File Offset: 0x00000D98
		public IConversion SetFrameRate(double frameRate)
		{
			this._parameters.Add(new ConversionParameter("-framerate " + frameRate.ToFFmpegFormat(3), ParameterPosition.PostInput));
			this._parameters.Add(new ConversionParameter("-r " + frameRate.ToFFmpegFormat(3), ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002BEC File Offset: 0x00000DEC
		private string GetStreamsPostInputs()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (IStream stream in this._streams)
			{
				stringBuilder.Append(stream.BuildParameters(ParameterPosition.PostInput));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002C54 File Offset: 0x00000E54
		private string GetStreamsPreInputs()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (IStream stream in this._streams)
			{
				stringBuilder.Append(stream.BuildParameters(ParameterPosition.PreInput));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002CBC File Offset: 0x00000EBC
		private string GetFilters()
		{
			StringBuilder stringBuilder = new StringBuilder();
			List<IFilterConfiguration> list = new List<IFilterConfiguration>();
			foreach (IStream stream in this._streams)
			{
				IFilterable filterable = stream as IFilterable;
				if (filterable != null)
				{
					list.AddRange(filterable.GetFilters());
				}
			}
			using (IEnumerator<IGrouping<string, IFilterConfiguration>> enumerator2 = Enumerable.GroupBy<IFilterConfiguration, string>(list, (IFilterConfiguration configuration) => configuration.FilterType).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					IGrouping<string, IFilterConfiguration> filterGroup = enumerator2.Current;
					stringBuilder.Append(filterGroup.Key + " \"");
					IEnumerable<IFilterConfiguration> enumerable = list;
					Func<IFilterConfiguration, bool> func;
					Func<IFilterConfiguration, bool> <>9__1;
					if ((func = <>9__1) == null)
					{
						func = (<>9__1 = ((IFilterConfiguration x) => x.FilterType == filterGroup.Key));
					}
					foreach (IFilterConfiguration filterConfiguration in Enumerable.Where<IFilterConfiguration>(enumerable, func))
					{
						List<string> list2 = new List<string>();
						foreach (KeyValuePair<string, string> keyValuePair in filterConfiguration.Filters)
						{
							string text = string.Format("[{0}]", filterConfiguration.StreamNumber);
							string text2 = string.IsNullOrEmpty(keyValuePair.Value) ? (keyValuePair.Key + " ") : (keyValuePair.Key + "=" + keyValuePair.Value);
							list2.Add(text + " " + text2 + " ");
						}
						stringBuilder.Append(string.Join(";", list2));
					}
					stringBuilder.Append("\" ");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002F20 File Offset: 0x00001120
		private string GetMap()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (IStream stream in this._streams)
			{
				if (this._hasInputBuilder)
				{
					stringBuilder.Append("-map 0:0 ");
				}
				foreach (string text in stream.GetSource())
				{
					if (this._hasInputBuilder)
					{
						stringBuilder.Append(string.Format("-map {0}:{1} ", this._inputFileMap[text] + 1, stream.Index));
					}
					else
					{
						stringBuilder.Append(string.Format("-map {0}:{1} ", this._inputFileMap[text], stream.Index));
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003030 File Offset: 0x00001230
		private string GetParameters(ParameterPosition forPosition)
		{
			ParametersList<ConversionParameter> parameters = this._parameters;
			IEnumerable<ConversionParameter> enumerable = (parameters != null) ? Enumerable.Where<ConversionParameter>(parameters, (ConversionParameter x) => x.Position == forPosition) : null;
			if (enumerable != null && Enumerable.Any<ConversionParameter>(enumerable))
			{
				return string.Join(string.Empty, Enumerable.Select<ConversionParameter, string>(enumerable, (ConversionParameter x) => x.Parameter));
			}
			return string.Empty;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000030AC File Offset: 0x000012AC
		private string GetInputs()
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (string text in Enumerable.Distinct<string>(Enumerable.SelectMany<IStream, string>(this._streams, (IStream x) => x.GetSource())))
			{
				this._inputFileMap[text] = num++;
				stringBuilder.Append("-i " + text.Escape() + " ");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00003158 File Offset: 0x00001358
		private bool HasH264Stream()
		{
			foreach (IStream stream in this._streams)
			{
				IVideoStream videoStream = stream as IVideoStream;
				if (videoStream != null && (videoStream.Codec == "libx264" || videoStream.Codec == VideoCodec.h264.ToString()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000031E4 File Offset: 0x000013E4
		internal static IConversion New()
		{
			return new Conversion().SetOverwriteOutput(false);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000031F1 File Offset: 0x000013F1
		public IConversion UseHardwareAcceleration(HardwareAccelerator hardwareAccelerator, VideoCodec decoder, VideoCodec encoder, int device = 0)
		{
			return this.UseHardwareAcceleration(string.Format("{0}", hardwareAccelerator), decoder.ToString(), encoder.ToString(), device);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003228 File Offset: 0x00001428
		public IConversion UseHardwareAcceleration(string hardwareAccelerator, string decoder, string encoder, int device = 0)
		{
			this._parameters.Add(new ConversionParameter("-hwaccel " + hardwareAccelerator, ParameterPosition.PreInput));
			this._parameters.Add(new ConversionParameter("-c:v " + decoder, ParameterPosition.PreInput));
			this._parameters.Add(new ConversionParameter("-c:v " + ((encoder != null) ? encoder.ToString() : null), ParameterPosition.PostInput));
			if (device != 0)
			{
				this._parameters.Add(new ConversionParameter(string.Format("-hwaccel_device {0}", device), ParameterPosition.PreInput));
			}
			this.UseMultiThread(false);
			return this;
		}

		// Token: 0x06000039 RID: 57 RVA: 0x000032C4 File Offset: 0x000014C4
		public IConversion SetOverwriteOutput(bool overwrite)
		{
			if (overwrite)
			{
				this._parameters.Add(new ConversionParameter("-y", ParameterPosition.PostInput));
				this._parameters.Remove(new ConversionParameter("-n", ParameterPosition.PostInput));
			}
			else
			{
				this._parameters.Remove(new ConversionParameter("-y", ParameterPosition.PostInput));
				this._parameters.Add(new ConversionParameter("-n", ParameterPosition.PostInput));
			}
			return this;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003330 File Offset: 0x00001530
		public IConversion SetInputFormat(Format inputFormat)
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

		// Token: 0x0600003B RID: 59 RVA: 0x00003388 File Offset: 0x00001588
		public IConversion SetInputFormat(string format)
		{
			if (format != null)
			{
				this._parameters.Add(new ConversionParameter("-f " + format, ParameterPosition.PreInput));
			}
			return this;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000033AC File Offset: 0x000015AC
		public IConversion SetOutputFormat(Format outputFormat)
		{
			string outputFormat2 = outputFormat.ToString();
			switch (outputFormat)
			{
			case Format._3dostr:
				outputFormat2 = "3dostr";
				break;
			case Format._3g2:
				outputFormat2 = "3g2";
				break;
			case Format._3gp:
				outputFormat2 = "3gp";
				break;
			case Format._4xm:
				outputFormat2 = "4xm";
				break;
			}
			return this.SetOutputFormat(outputFormat2);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003404 File Offset: 0x00001604
		public IConversion SetOutputFormat(string format)
		{
			if (format != null)
			{
				this._parameters.Add(new ConversionParameter("-f " + format, ParameterPosition.PostInput));
			}
			return this;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00003428 File Offset: 0x00001628
		public IConversion SetPixelFormat(PixelFormat pixelFormat)
		{
			string pixelFormat2 = pixelFormat.ToString();
			if (pixelFormat != PixelFormat._0rgb)
			{
				if (pixelFormat == PixelFormat._0bgr)
				{
					pixelFormat2 = "0bgr";
				}
			}
			else
			{
				pixelFormat2 = "0rgb";
			}
			return this.SetPixelFormat(pixelFormat2);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003462 File Offset: 0x00001662
		public IConversion SetPixelFormat(string pixelFormat)
		{
			if (pixelFormat != null)
			{
				this._parameters.Add(new ConversionParameter("-pix_fmt " + pixelFormat, ParameterPosition.PostInput));
			}
			return this;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003484 File Offset: 0x00001684
		public IConversion SetVideoSyncMethod(VideoSyncMethod method)
		{
			if (method == VideoSyncMethod.auto)
			{
				this._parameters.Add(new ConversionParameter("-vsync -1", ParameterPosition.PostInput));
			}
			else
			{
				this._parameters.Add(new ConversionParameter(string.Format("-vsync {0}", method), ParameterPosition.PostInput));
			}
			return this;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x000034C4 File Offset: 0x000016C4
		public IConversion AddDesktopStream(string videoSize = null, double framerate = 30.0, int xOffset = 0, int yOffset = 0)
		{
			VideoStream videoStream = new VideoStream();
			int index;
			if (!Enumerable.Any<IStream>(this._streams))
			{
				index = 0;
			}
			else
			{
				index = Enumerable.Max<IStream>(this._streams, (IStream x) => x.Index) + 1;
			}
			videoStream.Index = index;
			VideoStream videoStream2 = videoStream;
			videoStream2.AddParameter("-framerate " + framerate.ToFFmpegFormat(4), ParameterPosition.PreInput);
			videoStream2.AddParameter(string.Format("-offset_x {0}", xOffset), ParameterPosition.PreInput);
			videoStream2.AddParameter(string.Format("-offset_y {0}", yOffset), ParameterPosition.PreInput);
			if (videoSize != null)
			{
				videoStream2.AddParameter("-video_size " + videoSize, ParameterPosition.PreInput);
			}
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				videoStream2.SetInputFormat(Format.gdigrab);
				videoStream2.Path = "desktop";
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				videoStream2.SetInputFormat(Format.avfoundation);
				videoStream2.Path = "1:1";
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				videoStream2.SetInputFormat(Format.x11grab);
				videoStream2.Path = ":0.0+0,0";
			}
			this._streams.Add(videoStream2);
			return this;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000035F0 File Offset: 0x000017F0
		public IConversion AddDesktopStream(VideoSize videoSize, double framerate = 30.0, int xOffset = 0, int yOffset = 0)
		{
			return this.AddDesktopStream(videoSize.ToFFmpegFormat(), framerate, xOffset, yOffset);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003604 File Offset: 0x00001804
		internal static Task<IConversion> ExtractAudio(string inputPath, string outputPath)
		{
			Conversion.<ExtractAudio>d__80 <ExtractAudio>d__;
			<ExtractAudio>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<ExtractAudio>d__.inputPath = inputPath;
			<ExtractAudio>d__.outputPath = outputPath;
			<ExtractAudio>d__.<>1__state = -1;
			<ExtractAudio>d__.<>t__builder.Start<Conversion.<ExtractAudio>d__80>(ref <ExtractAudio>d__);
			return <ExtractAudio>d__.<>t__builder.Task;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003650 File Offset: 0x00001850
		internal static Task<IConversion> AddAudio(string videoPath, string audioPath, string outputPath)
		{
			Conversion.<AddAudio>d__81 <AddAudio>d__;
			<AddAudio>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<AddAudio>d__.videoPath = videoPath;
			<AddAudio>d__.audioPath = audioPath;
			<AddAudio>d__.outputPath = outputPath;
			<AddAudio>d__.<>1__state = -1;
			<AddAudio>d__.<>t__builder.Start<Conversion.<AddAudio>d__81>(ref <AddAudio>d__);
			return <AddAudio>d__.<>t__builder.Task;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000036A4 File Offset: 0x000018A4
		internal static Task<IConversion> VisualiseAudio(string inputPath, string outputPath, VideoSize size, PixelFormat pixelFormat = PixelFormat.yuv420p, VisualisationMode mode = VisualisationMode.bar, AmplitudeScale amplitudeScale = AmplitudeScale.lin, FrequencyScale frequencyScale = FrequencyScale.log)
		{
			Conversion.<VisualiseAudio>d__82 <VisualiseAudio>d__;
			<VisualiseAudio>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<VisualiseAudio>d__.inputPath = inputPath;
			<VisualiseAudio>d__.outputPath = outputPath;
			<VisualiseAudio>d__.size = size;
			<VisualiseAudio>d__.pixelFormat = pixelFormat;
			<VisualiseAudio>d__.mode = mode;
			<VisualiseAudio>d__.amplitudeScale = amplitudeScale;
			<VisualiseAudio>d__.frequencyScale = frequencyScale;
			<VisualiseAudio>d__.<>1__state = -1;
			<VisualiseAudio>d__.<>t__builder.Start<Conversion.<VisualiseAudio>d__82>(ref <VisualiseAudio>d__);
			return <VisualiseAudio>d__.<>t__builder.Task;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x0000371C File Offset: 0x0000191C
		internal static Task<IConversion> ToMp4(string inputPath, string outputPath)
		{
			Conversion.<ToMp4>d__83 <ToMp4>d__;
			<ToMp4>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<ToMp4>d__.inputPath = inputPath;
			<ToMp4>d__.outputPath = outputPath;
			<ToMp4>d__.<>1__state = -1;
			<ToMp4>d__.<>t__builder.Start<Conversion.<ToMp4>d__83>(ref <ToMp4>d__);
			return <ToMp4>d__.<>t__builder.Task;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003768 File Offset: 0x00001968
		internal static Task<IConversion> ToTs(string inputPath, string outputPath)
		{
			Conversion.<ToTs>d__84 <ToTs>d__;
			<ToTs>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<ToTs>d__.inputPath = inputPath;
			<ToTs>d__.outputPath = outputPath;
			<ToTs>d__.<>1__state = -1;
			<ToTs>d__.<>t__builder.Start<Conversion.<ToTs>d__84>(ref <ToTs>d__);
			return <ToTs>d__.<>t__builder.Task;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x000037B4 File Offset: 0x000019B4
		internal static Task<IConversion> ToOgv(string inputPath, string outputPath)
		{
			Conversion.<ToOgv>d__85 <ToOgv>d__;
			<ToOgv>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<ToOgv>d__.inputPath = inputPath;
			<ToOgv>d__.outputPath = outputPath;
			<ToOgv>d__.<>1__state = -1;
			<ToOgv>d__.<>t__builder.Start<Conversion.<ToOgv>d__85>(ref <ToOgv>d__);
			return <ToOgv>d__.<>t__builder.Task;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003800 File Offset: 0x00001A00
		internal static Task<IConversion> ToWebM(string inputPath, string outputPath)
		{
			Conversion.<ToWebM>d__86 <ToWebM>d__;
			<ToWebM>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<ToWebM>d__.inputPath = inputPath;
			<ToWebM>d__.outputPath = outputPath;
			<ToWebM>d__.<>1__state = -1;
			<ToWebM>d__.<>t__builder.Start<Conversion.<ToWebM>d__86>(ref <ToWebM>d__);
			return <ToWebM>d__.<>t__builder.Task;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x0000384C File Offset: 0x00001A4C
		internal static Task<IConversion> ToGif(string inputPath, string outputPath, int loop, int delay = 0)
		{
			Conversion.<ToGif>d__87 <ToGif>d__;
			<ToGif>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<ToGif>d__.inputPath = inputPath;
			<ToGif>d__.outputPath = outputPath;
			<ToGif>d__.loop = loop;
			<ToGif>d__.delay = delay;
			<ToGif>d__.<>1__state = -1;
			<ToGif>d__.<>t__builder.Start<Conversion.<ToGif>d__87>(ref <ToGif>d__);
			return <ToGif>d__.<>t__builder.Task;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000038A8 File Offset: 0x00001AA8
		internal static Task<Device[]> GetAvailableDevices()
		{
			Conversion.<GetAvailableDevices>d__88 <GetAvailableDevices>d__;
			<GetAvailableDevices>d__.<>t__builder = AsyncTaskMethodBuilder<Device[]>.Create();
			<GetAvailableDevices>d__.<>1__state = -1;
			<GetAvailableDevices>d__.<>t__builder.Start<Conversion.<GetAvailableDevices>d__88>(ref <GetAvailableDevices>d__);
			return <GetAvailableDevices>d__.<>t__builder.Task;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x000038E4 File Offset: 0x00001AE4
		internal static Task<IConversion> ConvertWithHardwareAsync(string inputFilePath, string outputFilePath, HardwareAccelerator hardwareAccelerator, VideoCodec decoder, VideoCodec encoder, int device = 0)
		{
			Conversion.<ConvertWithHardwareAsync>d__89 <ConvertWithHardwareAsync>d__;
			<ConvertWithHardwareAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<ConvertWithHardwareAsync>d__.inputFilePath = inputFilePath;
			<ConvertWithHardwareAsync>d__.outputFilePath = outputFilePath;
			<ConvertWithHardwareAsync>d__.hardwareAccelerator = hardwareAccelerator;
			<ConvertWithHardwareAsync>d__.decoder = decoder;
			<ConvertWithHardwareAsync>d__.encoder = encoder;
			<ConvertWithHardwareAsync>d__.device = device;
			<ConvertWithHardwareAsync>d__.<>1__state = -1;
			<ConvertWithHardwareAsync>d__.<>t__builder.Start<Conversion.<ConvertWithHardwareAsync>d__89>(ref <ConvertWithHardwareAsync>d__);
			return <ConvertWithHardwareAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003954 File Offset: 0x00001B54
		internal static Task<IConversion> SendToRtspServer(string inputFilePath, Uri rtspServerUri)
		{
			Conversion.<SendToRtspServer>d__90 <SendToRtspServer>d__;
			<SendToRtspServer>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<SendToRtspServer>d__.inputFilePath = inputFilePath;
			<SendToRtspServer>d__.rtspServerUri = rtspServerUri;
			<SendToRtspServer>d__.<>1__state = -1;
			<SendToRtspServer>d__.<>t__builder.Start<Conversion.<SendToRtspServer>d__90>(ref <SendToRtspServer>d__);
			return <SendToRtspServer>d__.<>t__builder.Task;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000039A0 File Offset: 0x00001BA0
		internal static IConversion SendDesktopToRtspServer(Uri rtspServerUri)
		{
			return FFmpeg.Conversions.New().AddDesktopStream("800x600", 30.0, 0, 0).AddParameter("-tune zerolatency", ParameterPosition.PostInput).SetOutputFormat(Format.rtsp).SetOutput(rtspServerUri.OriginalString);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000039EC File Offset: 0x00001BEC
		internal static Task<IConversion> AddSubtitlesAsync(string inputPath, string outputPath, string subtitlesPath)
		{
			Conversion.<AddSubtitlesAsync>d__92 <AddSubtitlesAsync>d__;
			<AddSubtitlesAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<AddSubtitlesAsync>d__.inputPath = inputPath;
			<AddSubtitlesAsync>d__.outputPath = outputPath;
			<AddSubtitlesAsync>d__.subtitlesPath = subtitlesPath;
			<AddSubtitlesAsync>d__.<>1__state = -1;
			<AddSubtitlesAsync>d__.<>t__builder.Start<Conversion.<AddSubtitlesAsync>d__92>(ref <AddSubtitlesAsync>d__);
			return <AddSubtitlesAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003A40 File Offset: 0x00001C40
		internal static Task<IConversion> AddSubtitleAsync(string inputPath, string outputPath, string subtitlePath, string language = null)
		{
			Conversion.<AddSubtitleAsync>d__93 <AddSubtitleAsync>d__;
			<AddSubtitleAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<AddSubtitleAsync>d__.inputPath = inputPath;
			<AddSubtitleAsync>d__.outputPath = outputPath;
			<AddSubtitleAsync>d__.subtitlePath = subtitlePath;
			<AddSubtitleAsync>d__.language = language;
			<AddSubtitleAsync>d__.<>1__state = -1;
			<AddSubtitleAsync>d__.<>t__builder.Start<Conversion.<AddSubtitleAsync>d__93>(ref <AddSubtitleAsync>d__);
			return <AddSubtitleAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003A9C File Offset: 0x00001C9C
		internal static Task<IConversion> AddSubtitleAsync(string inputPath, string outputPath, string subtitlePath, SubtitleCodec subtitleCodec, string language = null)
		{
			Conversion.<AddSubtitleAsync>d__94 <AddSubtitleAsync>d__;
			<AddSubtitleAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<AddSubtitleAsync>d__.inputPath = inputPath;
			<AddSubtitleAsync>d__.outputPath = outputPath;
			<AddSubtitleAsync>d__.subtitlePath = subtitlePath;
			<AddSubtitleAsync>d__.subtitleCodec = subtitleCodec;
			<AddSubtitleAsync>d__.language = language;
			<AddSubtitleAsync>d__.<>1__state = -1;
			<AddSubtitleAsync>d__.<>t__builder.Start<Conversion.<AddSubtitleAsync>d__94>(ref <AddSubtitleAsync>d__);
			return <AddSubtitleAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003B00 File Offset: 0x00001D00
		internal static Task<IConversion> SetWatermarkAsync(string inputPath, string outputPath, string inputImage, Position position)
		{
			Conversion.<SetWatermarkAsync>d__95 <SetWatermarkAsync>d__;
			<SetWatermarkAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<SetWatermarkAsync>d__.inputPath = inputPath;
			<SetWatermarkAsync>d__.outputPath = outputPath;
			<SetWatermarkAsync>d__.inputImage = inputImage;
			<SetWatermarkAsync>d__.position = position;
			<SetWatermarkAsync>d__.<>1__state = -1;
			<SetWatermarkAsync>d__.<>t__builder.Start<Conversion.<SetWatermarkAsync>d__95>(ref <SetWatermarkAsync>d__);
			return <SetWatermarkAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003B5C File Offset: 0x00001D5C
		internal static Task<IConversion> ExtractVideoAsync(string inputPath, string outputPath)
		{
			Conversion.<ExtractVideoAsync>d__96 <ExtractVideoAsync>d__;
			<ExtractVideoAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<ExtractVideoAsync>d__.inputPath = inputPath;
			<ExtractVideoAsync>d__.outputPath = outputPath;
			<ExtractVideoAsync>d__.<>1__state = -1;
			<ExtractVideoAsync>d__.<>t__builder.Start<Conversion.<ExtractVideoAsync>d__96>(ref <ExtractVideoAsync>d__);
			return <ExtractVideoAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003BA8 File Offset: 0x00001DA8
		internal static Task<IConversion> SnapshotAsync(string inputPath, string outputPath, TimeSpan captureTime)
		{
			Conversion.<SnapshotAsync>d__97 <SnapshotAsync>d__;
			<SnapshotAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<SnapshotAsync>d__.inputPath = inputPath;
			<SnapshotAsync>d__.outputPath = outputPath;
			<SnapshotAsync>d__.captureTime = captureTime;
			<SnapshotAsync>d__.<>1__state = -1;
			<SnapshotAsync>d__.<>t__builder.Start<Conversion.<SnapshotAsync>d__97>(ref <SnapshotAsync>d__);
			return <SnapshotAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003BFC File Offset: 0x00001DFC
		internal static Task<IConversion> ChangeSizeAsync(string inputPath, string outputPath, int width, int height)
		{
			Conversion.<ChangeSizeAsync>d__98 <ChangeSizeAsync>d__;
			<ChangeSizeAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<ChangeSizeAsync>d__.inputPath = inputPath;
			<ChangeSizeAsync>d__.outputPath = outputPath;
			<ChangeSizeAsync>d__.width = width;
			<ChangeSizeAsync>d__.height = height;
			<ChangeSizeAsync>d__.<>1__state = -1;
			<ChangeSizeAsync>d__.<>t__builder.Start<Conversion.<ChangeSizeAsync>d__98>(ref <ChangeSizeAsync>d__);
			return <ChangeSizeAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003C58 File Offset: 0x00001E58
		internal static Task<IConversion> ChangeSizeAsync(string inputPath, string outputPath, VideoSize size)
		{
			Conversion.<ChangeSizeAsync>d__99 <ChangeSizeAsync>d__;
			<ChangeSizeAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<ChangeSizeAsync>d__.inputPath = inputPath;
			<ChangeSizeAsync>d__.outputPath = outputPath;
			<ChangeSizeAsync>d__.size = size;
			<ChangeSizeAsync>d__.<>1__state = -1;
			<ChangeSizeAsync>d__.<>t__builder.Start<Conversion.<ChangeSizeAsync>d__99>(ref <ChangeSizeAsync>d__);
			return <ChangeSizeAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00003CAC File Offset: 0x00001EAC
		internal static Task<IConversion> SplitAsync(string inputPath, string outputPath, TimeSpan startTime, TimeSpan duration)
		{
			Conversion.<SplitAsync>d__100 <SplitAsync>d__;
			<SplitAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<SplitAsync>d__.inputPath = inputPath;
			<SplitAsync>d__.outputPath = outputPath;
			<SplitAsync>d__.startTime = startTime;
			<SplitAsync>d__.duration = duration;
			<SplitAsync>d__.<>1__state = -1;
			<SplitAsync>d__.<>t__builder.Start<Conversion.<SplitAsync>d__100>(ref <SplitAsync>d__);
			return <SplitAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00003D08 File Offset: 0x00001F08
		internal static Task<IConversion> SaveM3U8StreamAsync(Uri uri, string outputPath, TimeSpan? duration = null)
		{
			Conversion.<SaveM3U8StreamAsync>d__101 <SaveM3U8StreamAsync>d__;
			<SaveM3U8StreamAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<SaveM3U8StreamAsync>d__.uri = uri;
			<SaveM3U8StreamAsync>d__.outputPath = outputPath;
			<SaveM3U8StreamAsync>d__.duration = duration;
			<SaveM3U8StreamAsync>d__.<>1__state = -1;
			<SaveM3U8StreamAsync>d__.<>t__builder.Start<Conversion.<SaveM3U8StreamAsync>d__101>(ref <SaveM3U8StreamAsync>d__);
			return <SaveM3U8StreamAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00003D5C File Offset: 0x00001F5C
		internal static Task<IConversion> Concatenate(string output, params string[] inputVideos)
		{
			Conversion.<Concatenate>d__102 <Concatenate>d__;
			<Concatenate>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<Concatenate>d__.output = output;
			<Concatenate>d__.inputVideos = inputVideos;
			<Concatenate>d__.<>1__state = -1;
			<Concatenate>d__.<>t__builder.Start<Conversion.<Concatenate>d__102>(ref <Concatenate>d__);
			return <Concatenate>d__.<>t__builder.Task;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00003DA8 File Offset: 0x00001FA8
		internal static Task<IConversion> ConvertAsync(string inputFilePath, string outputFilePath, bool keepSubtitles = false)
		{
			Conversion.<ConvertAsync>d__103 <ConvertAsync>d__;
			<ConvertAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<ConvertAsync>d__.inputFilePath = inputFilePath;
			<ConvertAsync>d__.outputFilePath = outputFilePath;
			<ConvertAsync>d__.keepSubtitles = keepSubtitles;
			<ConvertAsync>d__.<>1__state = -1;
			<ConvertAsync>d__.<>t__builder.Start<Conversion.<ConvertAsync>d__103>(ref <ConvertAsync>d__);
			return <ConvertAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00003DFC File Offset: 0x00001FFC
		internal static Task<IConversion> TranscodeAsync(string inputFilePath, string outputFilePath, VideoCodec videoCodec, AudioCodec audioCodec, SubtitleCodec subtitleCodec, bool keepSubtitles = false)
		{
			Conversion.<TranscodeAsync>d__104 <TranscodeAsync>d__;
			<TranscodeAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IConversion>.Create();
			<TranscodeAsync>d__.inputFilePath = inputFilePath;
			<TranscodeAsync>d__.outputFilePath = outputFilePath;
			<TranscodeAsync>d__.videoCodec = videoCodec;
			<TranscodeAsync>d__.audioCodec = audioCodec;
			<TranscodeAsync>d__.subtitleCodec = subtitleCodec;
			<TranscodeAsync>d__.keepSubtitles = keepSubtitles;
			<TranscodeAsync>d__.<>1__state = -1;
			<TranscodeAsync>d__.<>t__builder.Start<Conversion.<TranscodeAsync>d__104>(ref <TranscodeAsync>d__);
			return <TranscodeAsync>d__.<>t__builder.Task;
		}

		// Token: 0x04000001 RID: 1
		private readonly object _builderLock = new object();

		// Token: 0x04000002 RID: 2
		private readonly Dictionary<string, int> _inputFileMap = new Dictionary<string, int>();

		// Token: 0x04000003 RID: 3
		private readonly ParametersList<ConversionParameter> _parameters = new ParametersList<ConversionParameter>();

		// Token: 0x04000004 RID: 4
		private readonly IDictionary<ParameterPosition, List<string>> _userDefinedParameters = new Dictionary<ParameterPosition, List<string>>();

		// Token: 0x04000005 RID: 5
		private readonly List<IStream> _streams = new List<IStream>();

		// Token: 0x04000006 RID: 6
		private string _output;

		// Token: 0x04000007 RID: 7
		private bool _hasInputBuilder;

		// Token: 0x04000008 RID: 8
		private ProcessPriorityClass? _priority;

		// Token: 0x04000009 RID: 9
		private FFmpegWrapper _ffmpeg;

		// Token: 0x0400000A RID: 10
		private Func<string, string> _buildInputFileName;

		// Token: 0x0400000B RID: 11
		private Func<string, string> _buildOutputFileName;
	}
}
