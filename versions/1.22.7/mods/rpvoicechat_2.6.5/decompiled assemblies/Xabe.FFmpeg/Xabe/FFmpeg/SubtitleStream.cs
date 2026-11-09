using System;
using System.Collections.Generic;
using System.Linq;
using Xabe.FFmpeg.Streams;
using Xabe.FFmpeg.Streams.SubtitleStream;

namespace Xabe.FFmpeg
{
	// Token: 0x02000035 RID: 53
	public class SubtitleStream : ISubtitleStream, IStream
	{
		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600018C RID: 396 RVA: 0x00006099 File Offset: 0x00004299
		// (set) Token: 0x0600018D RID: 397 RVA: 0x000060A1 File Offset: 0x000042A1
		public string Codec { get; internal set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600018E RID: 398 RVA: 0x000060AA File Offset: 0x000042AA
		// (set) Token: 0x0600018F RID: 399 RVA: 0x000060B2 File Offset: 0x000042B2
		public string Path { get; internal set; }

		// Token: 0x06000190 RID: 400 RVA: 0x000060BB File Offset: 0x000042BB
		internal SubtitleStream()
		{
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000060D0 File Offset: 0x000042D0
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

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00006149 File Offset: 0x00004349
		// (set) Token: 0x06000193 RID: 403 RVA: 0x00006151 File Offset: 0x00004351
		public int Index { get; internal set; }

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000194 RID: 404 RVA: 0x0000615A File Offset: 0x0000435A
		// (set) Token: 0x06000195 RID: 405 RVA: 0x00006162 File Offset: 0x00004362
		public string Language { get; internal set; }

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000196 RID: 406 RVA: 0x0000616B File Offset: 0x0000436B
		// (set) Token: 0x06000197 RID: 407 RVA: 0x00006173 File Offset: 0x00004373
		public int? Default { get; internal set; }

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000198 RID: 408 RVA: 0x0000617C File Offset: 0x0000437C
		// (set) Token: 0x06000199 RID: 409 RVA: 0x00006184 File Offset: 0x00004384
		public int? Forced { get; internal set; }

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600019A RID: 410 RVA: 0x0000618D File Offset: 0x0000438D
		// (set) Token: 0x0600019B RID: 411 RVA: 0x00006195 File Offset: 0x00004395
		public string Title { get; internal set; }

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600019C RID: 412 RVA: 0x0000619E File Offset: 0x0000439E
		public StreamType StreamType
		{
			get
			{
				return StreamType.Subtitle;
			}
		}

		// Token: 0x0600019D RID: 413 RVA: 0x000061A4 File Offset: 0x000043A4
		public ISubtitleStream SetLanguage(string lang)
		{
			string text = (!string.IsNullOrEmpty(lang)) ? lang : this.Language;
			if (!string.IsNullOrEmpty(text))
			{
				text = string.Format("-metadata:s:s:{0} language={1}", this.Index, text);
				this._parameters.Add(new ConversionParameter(text, ParameterPosition.PostInput));
			}
			return this;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x000061F5 File Offset: 0x000043F5
		public IEnumerable<string> GetSource()
		{
			return new string[]
			{
				this.Path
			};
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00006206 File Offset: 0x00004406
		public ISubtitleStream SetCodec(SubtitleCodec codec)
		{
			return this.SetCodec(codec.ToString());
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000621B File Offset: 0x0000441B
		public ISubtitleStream SetCodec(string codec)
		{
			this._parameters.Add(new ConversionParameter("-c:s " + codec, ParameterPosition.PostInput));
			return this;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000623A File Offset: 0x0000443A
		public ISubtitleStream UseNativeInputRead(bool readInputAtNativeFrameRate)
		{
			this._parameters.Add(new ConversionParameter("-re", ParameterPosition.PreInput));
			return this;
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00006253 File Offset: 0x00004453
		public ISubtitleStream SetStreamLoop(int loopCount)
		{
			this._parameters.Add(new ConversionParameter(string.Format("-stream_loop {0}", loopCount), ParameterPosition.PreInput));
			return this;
		}

		// Token: 0x0400038F RID: 911
		private readonly ParametersList<ConversionParameter> _parameters = new ParametersList<ConversionParameter>();
	}
}
