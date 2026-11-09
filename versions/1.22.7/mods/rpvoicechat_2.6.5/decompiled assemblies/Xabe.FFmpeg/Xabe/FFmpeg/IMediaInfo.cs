using System;
using System.Collections.Generic;

namespace Xabe.FFmpeg
{
	// Token: 0x02000025 RID: 37
	public interface IMediaInfo
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000105 RID: 261
		IEnumerable<IStream> Streams { get; }

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000106 RID: 262
		string Path { get; }

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000107 RID: 263
		TimeSpan Duration { get; }

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000108 RID: 264
		DateTime? CreationTime { get; }

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000109 RID: 265
		long Size { get; }

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600010A RID: 266
		IEnumerable<IVideoStream> VideoStreams { get; }

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600010B RID: 267
		IEnumerable<IAudioStream> AudioStreams { get; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x0600010C RID: 268
		IEnumerable<ISubtitleStream> SubtitleStreams { get; }
	}
}
