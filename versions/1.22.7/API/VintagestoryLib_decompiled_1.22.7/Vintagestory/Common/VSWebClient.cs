using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Vintagestory.Client.NoObf;

namespace Vintagestory.Common;

public class VSWebClient : HttpClient
{
	public delegate void PostCompleteHandler(CompletedArgs args);

	public static readonly VSWebClient Inst = new VSWebClient
	{
		Timeout = TimeSpan.FromSeconds(ClientSettings.WebRequestTimeout)
	};

	public void PostAsync(Uri uri, FormUrlEncodedContent? postData, PostCompleteHandler onFinished)
	{
		PostAsyncSafe(uri, postData).ContinueWith(delegate(Task<CompletedArgs> t)
		{
			onFinished(t.Result);
		}, TaskContinuationOptions.AttachedToParent);
	}

	public Task<CompletedArgs> PostAsyncSafe(Uri uri, FormUrlEncodedContent? postData, CancellationToken cancellationToken = default(CancellationToken))
	{
		return SendAsyncSafe(new HttpRequestMessage(HttpMethod.Post, uri)
		{
			Content = postData
		}, HttpCompletionOption.ResponseContentRead, cancellationToken);
	}

	public Task<CompletedArgs> GetAsyncSafe(Uri uri, CancellationToken cancellationToken = default(CancellationToken))
	{
		return SendAsyncSafe(new HttpRequestMessage(HttpMethod.Get, uri), HttpCompletionOption.ResponseContentRead, cancellationToken);
	}

	public async Task<CompletedArgs> SendAsyncSafe(HttpRequestMessage request, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead, CancellationToken cancellationToken = default(CancellationToken))
	{
		_ = 1;
		try
		{
			HttpResponseMessage res = await SendAsync(request, cancellationToken);
			string data = await res.Content.ReadAsStringAsync(cancellationToken);
			return new CompletedArgs
			{
				State = ((!res.IsSuccessStatusCode) ? CompletionState.Error : CompletionState.Good),
				StatusCode = (int)res.StatusCode,
				Response = data,
				ErrorMessage = (res.ReasonPhrase ?? string.Empty)
			};
		}
		catch (Exception ex)
		{
			return new CompletedArgs
			{
				State = CompletionState.Error,
				ErrorMessage = ex.ToString()
			};
		}
	}

	public string Post(Uri uri, FormUrlEncodedContent postData)
	{
		try
		{
			return PostAsync(uri, postData).Result.Content.ReadAsStringAsync().Result;
		}
		catch (Exception)
		{
			return string.Empty;
		}
	}

	public async Task DownloadAsync(string requestUri, Stream destination, IProgress<Tuple<int, long>>? progress = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		using HttpResponseMessage response = await GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
		long? contentLength = response.Content.Headers.ContentLength;
		await using Stream download = await response.Content.ReadAsStreamAsync(cancellationToken);
		if (progress == null || !contentLength.HasValue)
		{
			await download.CopyToAsync(destination, cancellationToken);
			return;
		}
		Progress<int> relativeProgress = new Progress<int>(delegate(int totalBytes)
		{
			progress.Report(new Tuple<int, long>(totalBytes, contentLength.Value));
		});
		await download.CopyToAsync(destination, 81920, relativeProgress, cancellationToken);
		download.Close();
	}
}
