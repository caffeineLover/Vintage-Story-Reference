using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Xabe.FFmpeg
{
	// Token: 0x02000026 RID: 38
	public class MediaInfo : IMediaInfo
	{
		// Token: 0x0600010D RID: 269 RVA: 0x000059C6 File Offset: 0x00003BC6
		private MediaInfo(string path)
		{
			this.Path = path;
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600010E RID: 270 RVA: 0x000059D5 File Offset: 0x00003BD5
		public IEnumerable<IStream> Streams
		{
			get
			{
				return Enumerable.Concat<IStream>(Enumerable.Concat<IStream>(this.VideoStreams, this.AudioStreams), this.SubtitleStreams);
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x0600010F RID: 271 RVA: 0x000059F3 File Offset: 0x00003BF3
		// (set) Token: 0x06000110 RID: 272 RVA: 0x000059FB File Offset: 0x00003BFB
		public TimeSpan Duration { get; internal set; }

		// Token: 0x17000025 RID: 37
		// (get) Token: 0x06000111 RID: 273 RVA: 0x00005A04 File Offset: 0x00003C04
		// (set) Token: 0x06000112 RID: 274 RVA: 0x00005A0C File Offset: 0x00003C0C
		public long Size { get; internal set; }

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x06000113 RID: 275 RVA: 0x00005A15 File Offset: 0x00003C15
		// (set) Token: 0x06000114 RID: 276 RVA: 0x00005A1D File Offset: 0x00003C1D
		public DateTime? CreationTime { get; internal set; }

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000115 RID: 277 RVA: 0x00005A26 File Offset: 0x00003C26
		// (set) Token: 0x06000116 RID: 278 RVA: 0x00005A2E File Offset: 0x00003C2E
		public IEnumerable<IVideoStream> VideoStreams { get; internal set; }

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000117 RID: 279 RVA: 0x00005A37 File Offset: 0x00003C37
		// (set) Token: 0x06000118 RID: 280 RVA: 0x00005A3F File Offset: 0x00003C3F
		public IEnumerable<IAudioStream> AudioStreams { get; internal set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000119 RID: 281 RVA: 0x00005A48 File Offset: 0x00003C48
		// (set) Token: 0x0600011A RID: 282 RVA: 0x00005A50 File Offset: 0x00003C50
		public IEnumerable<ISubtitleStream> SubtitleStreams { get; internal set; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600011B RID: 283 RVA: 0x00005A59 File Offset: 0x00003C59
		public string Path { get; }

		// Token: 0x0600011C RID: 284 RVA: 0x00005A64 File Offset: 0x00003C64
		internal static Task<IMediaInfo> Get(string filePath)
		{
			MediaInfo.<Get>d__30 <Get>d__;
			<Get>d__.<>t__builder = AsyncTaskMethodBuilder<IMediaInfo>.Create();
			<Get>d__.filePath = filePath;
			<Get>d__.<>1__state = -1;
			<Get>d__.<>t__builder.Start<MediaInfo.<Get>d__30>(ref <Get>d__);
			return <Get>d__.<>t__builder.Task;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005AA8 File Offset: 0x00003CA8
		internal static Task<IMediaInfo> Get(string filePath, CancellationToken cancellationToken)
		{
			MediaInfo.<Get>d__31 <Get>d__;
			<Get>d__.<>t__builder = AsyncTaskMethodBuilder<IMediaInfo>.Create();
			<Get>d__.filePath = filePath;
			<Get>d__.cancellationToken = cancellationToken;
			<Get>d__.<>1__state = -1;
			<Get>d__.<>t__builder.Start<MediaInfo.<Get>d__31>(ref <Get>d__);
			return <Get>d__.<>t__builder.Task;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00005AF4 File Offset: 0x00003CF4
		internal static Task<IMediaInfo> Get(FileInfo fileInfo)
		{
			MediaInfo.<Get>d__32 <Get>d__;
			<Get>d__.<>t__builder = AsyncTaskMethodBuilder<IMediaInfo>.Create();
			<Get>d__.fileInfo = fileInfo;
			<Get>d__.<>1__state = -1;
			<Get>d__.<>t__builder.Start<MediaInfo.<Get>d__32>(ref <Get>d__);
			return <Get>d__.<>t__builder.Task;
		}
	}
}
