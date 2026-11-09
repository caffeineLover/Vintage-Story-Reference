using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Xabe.FFmpeg.Extensions
{
	// Token: 0x02000041 RID: 65
	public static class StreamExtensions
	{
		// Token: 0x0600020D RID: 525 RVA: 0x00006EA4 File Offset: 0x000050A4
		public static Task CopyToAsync(this Stream source, Stream destination, long contentLength, int bufferSize, IProgress<ProgressInfo> progress = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			StreamExtensions.<CopyToAsync>d__0 <CopyToAsync>d__;
			<CopyToAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<CopyToAsync>d__.source = source;
			<CopyToAsync>d__.destination = destination;
			<CopyToAsync>d__.contentLength = contentLength;
			<CopyToAsync>d__.bufferSize = bufferSize;
			<CopyToAsync>d__.progress = progress;
			<CopyToAsync>d__.cancellationToken = cancellationToken;
			<CopyToAsync>d__.<>1__state = -1;
			<CopyToAsync>d__.<>t__builder.Start<StreamExtensions.<CopyToAsync>d__0>(ref <CopyToAsync>d__);
			return <CopyToAsync>d__.<>t__builder.Task;
		}
	}
}
