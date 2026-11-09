using System;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Xabe.FFmpeg.Extensions
{
	// Token: 0x02000040 RID: 64
	public static class HttpClientExtensions
	{
		// Token: 0x0600020C RID: 524 RVA: 0x00006E40 File Offset: 0x00005040
		public static Task DownloadAsync(this HttpClient client, string requestUri, Stream destination, IProgress<ProgressInfo> progress = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			HttpClientExtensions.<DownloadAsync>d__0 <DownloadAsync>d__;
			<DownloadAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<DownloadAsync>d__.client = client;
			<DownloadAsync>d__.requestUri = requestUri;
			<DownloadAsync>d__.destination = destination;
			<DownloadAsync>d__.progress = progress;
			<DownloadAsync>d__.cancellationToken = cancellationToken;
			<DownloadAsync>d__.<>1__state = -1;
			<DownloadAsync>d__.<>t__builder.Start<HttpClientExtensions.<DownloadAsync>d__0>(ref <DownloadAsync>d__);
			return <DownloadAsync>d__.<>t__builder.Task;
		}
	}
}
