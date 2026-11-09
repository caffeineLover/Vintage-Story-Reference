using System;
using System.Collections.Generic;
using System.Linq;
using Xabe.FFmpeg.Streams;

namespace Xabe.FFmpeg
{
	// Token: 0x0200002C RID: 44
	public class AudioStream : IAudioStream, IStream, IFilterable
	{
		// Token: 0x0600012C RID: 300 RVA: 0x00005BB5 File Offset: 0x00003DB5
		internal AudioStream()
		{
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00005BD3 File Offset: 0x00003DD3
		public IAudioStream Reverse()
		{
			this._parameters.Add(new ConversionParameter("-af areverse", ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00005BEC File Offset: 0x00003DEC
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

		// Token: 0x0600012F RID: 303 RVA: 0x00005C68 File Offset: 0x00003E68
		public IAudioStream Split(TimeSpan startTime, TimeSpan duration)
		{
			this._parameters.Add(new ConversionParameter("-ss " + startTime.ToFFmpeg(), ParameterPosition.PostInput));
			this._parameters.Add(new ConversionParameter("-t " + duration.ToFFmpeg(), ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00005CB8 File Offset: 0x00003EB8
		public IAudioStream CopyStream()
		{
			return this.SetCodec(AudioCodec.copy);
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000131 RID: 305 RVA: 0x00005CC5 File Offset: 0x00003EC5
		public StreamType StreamType
		{
			get
			{
				return StreamType.Audio;
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00005CC8 File Offset: 0x00003EC8
		public IAudioStream SetChannels(int channels)
		{
			this._parameters.Add(new ConversionParameter(string.Format("-ac:{0} {1}", this.Index, channels), ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00005CF7 File Offset: 0x00003EF7
		public IAudioStream SetBitstreamFilter(BitstreamFilter filter)
		{
			return this.SetBitstreamFilter(string.Format("{0}", filter));
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00005D0F File Offset: 0x00003F0F
		public IAudioStream SetBitstreamFilter(string filter)
		{
			this._parameters.Add(new ConversionParameter("-bsf:a " + filter, ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00005D2E File Offset: 0x00003F2E
		public IAudioStream SetBitrate(long bitRate)
		{
			this._parameters.Add(new ConversionParameter(string.Format("-b:a:{0} {1}", this.Index, bitRate), ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00005D60 File Offset: 0x00003F60
		public IAudioStream SetBitrate(long minBitrate, long maxBitrate, long bufferSize)
		{
			this._parameters.Add(new ConversionParameter(string.Format("-b:a:{0} {1}", this.Index, minBitrate), ParameterPosition.PostInput));
			this._parameters.Add(new ConversionParameter(string.Format("-maxrate {0}", maxBitrate), ParameterPosition.PostInput));
			this._parameters.Add(new ConversionParameter(string.Format("-bufsize {0}", bufferSize), ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00005DDC File Offset: 0x00003FDC
		public IAudioStream SetSampleRate(int sampleRate)
		{
			this._parameters.Add(new ConversionParameter(string.Format("-ar:{0} {1}", this.Index, sampleRate), ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00005E0B File Offset: 0x0000400B
		public IAudioStream ChangeSpeed(double multiplication)
		{
			this._audioFilters["atempo"] = (this.GetAudioSpeed(multiplication) ?? "");
			return this;
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00005E2E File Offset: 0x0000402E
		private string GetAudioSpeed(double multiplication)
		{
			if (multiplication < 0.5 || multiplication > 2.0)
			{
				throw new ArgumentOutOfRangeException("multiplication", "Value has to be greater than 0.5 and less than 2.0.");
			}
			return multiplication.ToFFmpegFormat(1) + " ";
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00005E6C File Offset: 0x0000406C
		public IAudioStream SetCodec(AudioCodec codec)
		{
			string text = codec.ToString();
			if (codec == AudioCodec._4gv)
			{
				text = "4gv";
			}
			else if (codec == AudioCodec._8svx_exp)
			{
				text = "8svx_exp";
			}
			else if (codec == AudioCodec._8svx_fib)
			{
				text = "8svx_fib";
			}
			return this.SetCodec(text ?? "");
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00005EB8 File Offset: 0x000040B8
		public IAudioStream SetCodec(string codec)
		{
			this._parameters.Add(new ConversionParameter("-c:a " + codec + " ", ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600013C RID: 316 RVA: 0x00005EDC File Offset: 0x000040DC
		// (set) Token: 0x0600013D RID: 317 RVA: 0x00005EE4 File Offset: 0x000040E4
		public int Index { get; internal set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600013E RID: 318 RVA: 0x00005EED File Offset: 0x000040ED
		// (set) Token: 0x0600013F RID: 319 RVA: 0x00005EF5 File Offset: 0x000040F5
		public TimeSpan Duration { get; internal set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000140 RID: 320 RVA: 0x00005EFE File Offset: 0x000040FE
		// (set) Token: 0x06000141 RID: 321 RVA: 0x00005F06 File Offset: 0x00004106
		public string Codec { get; internal set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000142 RID: 322 RVA: 0x00005F0F File Offset: 0x0000410F
		// (set) Token: 0x06000143 RID: 323 RVA: 0x00005F17 File Offset: 0x00004117
		public long Bitrate { get; internal set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000144 RID: 324 RVA: 0x00005F20 File Offset: 0x00004120
		// (set) Token: 0x06000145 RID: 325 RVA: 0x00005F28 File Offset: 0x00004128
		public int Channels { get; internal set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000146 RID: 326 RVA: 0x00005F31 File Offset: 0x00004131
		// (set) Token: 0x06000147 RID: 327 RVA: 0x00005F39 File Offset: 0x00004139
		public int SampleRate { get; internal set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000148 RID: 328 RVA: 0x00005F42 File Offset: 0x00004142
		// (set) Token: 0x06000149 RID: 329 RVA: 0x00005F4A File Offset: 0x0000414A
		public string Language { get; internal set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600014A RID: 330 RVA: 0x00005F53 File Offset: 0x00004153
		// (set) Token: 0x0600014B RID: 331 RVA: 0x00005F5B File Offset: 0x0000415B
		public string Title { get; internal set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600014C RID: 332 RVA: 0x00005F64 File Offset: 0x00004164
		// (set) Token: 0x0600014D RID: 333 RVA: 0x00005F6C File Offset: 0x0000416C
		public int? Default { get; internal set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x0600014E RID: 334 RVA: 0x00005F75 File Offset: 0x00004175
		// (set) Token: 0x0600014F RID: 335 RVA: 0x00005F7D File Offset: 0x0000417D
		public int? Forced { get; internal set; }

		// Token: 0x06000150 RID: 336 RVA: 0x00005F86 File Offset: 0x00004186
		public IEnumerable<string> GetSource()
		{
			return new string[]
			{
				this.Path
			};
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000151 RID: 337 RVA: 0x00005F97 File Offset: 0x00004197
		// (set) Token: 0x06000152 RID: 338 RVA: 0x00005F9F File Offset: 0x0000419F
		public string Path { get; set; }

		// Token: 0x06000153 RID: 339 RVA: 0x00005FA8 File Offset: 0x000041A8
		public IAudioStream SetSeek(TimeSpan? seek)
		{
			this._parameters.Add(new ConversionParameter("-ss " + seek.Value.ToFFmpeg(), ParameterPosition.PreInput));
			return this;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00005FD2 File Offset: 0x000041D2
		public IEnumerable<IFilterConfiguration> GetFilters()
		{
			if (Enumerable.Any<KeyValuePair<string, string>>(this._audioFilters))
			{
				yield return new FilterConfiguration
				{
					FilterType = "-filter:a",
					StreamNumber = this.Index,
					Filters = this._audioFilters
				};
			}
			yield break;
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00005FE2 File Offset: 0x000041E2
		public IAudioStream SetInputFormat(string inputFormat)
		{
			this._parameters.Add(new ConversionParameter("-f " + inputFormat, ParameterPosition.PreInput));
			return this;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00006001 File Offset: 0x00004201
		public IAudioStream SetInputFormat(Format inputFormat)
		{
			return this.SetInputFormat(inputFormat.ToString());
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00006016 File Offset: 0x00004216
		public IAudioStream UseNativeInputRead(bool readInputAtNativeFrameRate)
		{
			this._parameters.Add(new ConversionParameter("-re", ParameterPosition.PreInput));
			return this;
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000602F File Offset: 0x0000422F
		public IAudioStream SetStreamLoop(int loopCount)
		{
			this._parameters.Add(new ConversionParameter(string.Format("-stream_loop {0}", loopCount), ParameterPosition.PreInput));
			return this;
		}

		// Token: 0x0400035B RID: 859
		private readonly ParametersList<ConversionParameter> _parameters = new ParametersList<ConversionParameter>();

		// Token: 0x0400035C RID: 860
		private readonly Dictionary<string, string> _audioFilters = new Dictionary<string, string>();
	}
}
