using System;
using Xabe.FFmpeg.Streams.SubtitleStream;

namespace Xabe.FFmpeg
{
	// Token: 0x02000034 RID: 52
	public interface ISubtitleStream : IStream
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000183 RID: 387
		string Language { get; }

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000184 RID: 388
		int? Default { get; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000185 RID: 389
		int? Forced { get; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000186 RID: 390
		string Title { get; }

		// Token: 0x06000187 RID: 391
		ISubtitleStream SetLanguage(string language);

		// Token: 0x06000188 RID: 392
		ISubtitleStream SetCodec(SubtitleCodec codec);

		// Token: 0x06000189 RID: 393
		ISubtitleStream SetCodec(string codec);

		// Token: 0x0600018A RID: 394
		ISubtitleStream UseNativeInputRead(bool readInputAtNativeFrameRate);

		// Token: 0x0600018B RID: 395
		ISubtitleStream SetStreamLoop(int loopCount);
	}
}
