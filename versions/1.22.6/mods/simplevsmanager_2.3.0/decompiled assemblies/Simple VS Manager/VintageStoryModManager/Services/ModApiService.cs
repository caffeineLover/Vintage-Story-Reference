using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using VintageStoryModManager.Models;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000068 RID: 104
	[NullableContext(1)]
	[Nullable(0)]
	public class ModApiService : IModApiService
	{
		// Token: 0x060009B1 RID: 2481 RVA: 0x0003A683 File Offset: 0x00038883
		public ModApiService(HttpClient httpClient)
		{
			this._httpClient = httpClient;
			this._jsonOptions = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};
		}

		// Token: 0x060009B2 RID: 2482 RVA: 0x0003A6A4 File Offset: 0x000388A4
		public Task<List<DownloadableModOnList>> QueryModsAsync([Nullable(2)] string textFilter = null, [Nullable(2)] ModAuthor authorFilter = null, [Nullable(new byte[]
		{
			2,
			1
		})] IEnumerable<GameVersion> versionsFilter = null, [Nullable(new byte[]
		{
			2,
			1
		})] IEnumerable<ModTag> tagsFilter = null, string orderBy = "follows", string orderByOrder = "desc", CancellationToken cancellationToken = default(CancellationToken))
		{
			ModApiService.<QueryModsAsync>d__4 <QueryModsAsync>d__;
			<QueryModsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<List<DownloadableModOnList>>.Create();
			<QueryModsAsync>d__.<>4__this = this;
			<QueryModsAsync>d__.textFilter = textFilter;
			<QueryModsAsync>d__.authorFilter = authorFilter;
			<QueryModsAsync>d__.versionsFilter = versionsFilter;
			<QueryModsAsync>d__.tagsFilter = tagsFilter;
			<QueryModsAsync>d__.orderBy = orderBy;
			<QueryModsAsync>d__.orderByOrder = orderByOrder;
			<QueryModsAsync>d__.cancellationToken = cancellationToken;
			<QueryModsAsync>d__.<>1__state = -1;
			<QueryModsAsync>d__.<>t__builder.Start<ModApiService.<QueryModsAsync>d__4>(ref <QueryModsAsync>d__);
			return <QueryModsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060009B3 RID: 2483 RVA: 0x0003A724 File Offset: 0x00038924
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		public Task<DownloadableMod> GetModAsync(int modId, CancellationToken cancellationToken = default(CancellationToken))
		{
			ModApiService.<GetModAsync>d__5 <GetModAsync>d__;
			<GetModAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DownloadableMod>.Create();
			<GetModAsync>d__.<>4__this = this;
			<GetModAsync>d__.modId = modId;
			<GetModAsync>d__.cancellationToken = cancellationToken;
			<GetModAsync>d__.<>1__state = -1;
			<GetModAsync>d__.<>t__builder.Start<ModApiService.<GetModAsync>d__5>(ref <GetModAsync>d__);
			return <GetModAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060009B4 RID: 2484 RVA: 0x0003A778 File Offset: 0x00038978
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		public Task<DownloadableMod> GetModAsync(string modIdStr, CancellationToken cancellationToken = default(CancellationToken))
		{
			ModApiService.<GetModAsync>d__6 <GetModAsync>d__;
			<GetModAsync>d__.<>t__builder = AsyncTaskMethodBuilder<DownloadableMod>.Create();
			<GetModAsync>d__.<>4__this = this;
			<GetModAsync>d__.modIdStr = modIdStr;
			<GetModAsync>d__.cancellationToken = cancellationToken;
			<GetModAsync>d__.<>1__state = -1;
			<GetModAsync>d__.<>t__builder.Start<ModApiService.<GetModAsync>d__6>(ref <GetModAsync>d__);
			return <GetModAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060009B5 RID: 2485 RVA: 0x0003A7CC File Offset: 0x000389CC
		public Task<List<ModAuthor>> GetAuthorsAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			ModApiService.<GetAuthorsAsync>d__7 <GetAuthorsAsync>d__;
			<GetAuthorsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<List<ModAuthor>>.Create();
			<GetAuthorsAsync>d__.<>4__this = this;
			<GetAuthorsAsync>d__.cancellationToken = cancellationToken;
			<GetAuthorsAsync>d__.<>1__state = -1;
			<GetAuthorsAsync>d__.<>t__builder.Start<ModApiService.<GetAuthorsAsync>d__7>(ref <GetAuthorsAsync>d__);
			return <GetAuthorsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060009B6 RID: 2486 RVA: 0x0003A818 File Offset: 0x00038A18
		public Task<List<GameVersion>> GetGameVersionsAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			ModApiService.<GetGameVersionsAsync>d__8 <GetGameVersionsAsync>d__;
			<GetGameVersionsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<List<GameVersion>>.Create();
			<GetGameVersionsAsync>d__.<>4__this = this;
			<GetGameVersionsAsync>d__.cancellationToken = cancellationToken;
			<GetGameVersionsAsync>d__.<>1__state = -1;
			<GetGameVersionsAsync>d__.<>t__builder.Start<ModApiService.<GetGameVersionsAsync>d__8>(ref <GetGameVersionsAsync>d__);
			return <GetGameVersionsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060009B7 RID: 2487 RVA: 0x0003A864 File Offset: 0x00038A64
		public Task<List<ModTag>> GetTagsAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			ModApiService.<GetTagsAsync>d__9 <GetTagsAsync>d__;
			<GetTagsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<List<ModTag>>.Create();
			<GetTagsAsync>d__.<>4__this = this;
			<GetTagsAsync>d__.cancellationToken = cancellationToken;
			<GetTagsAsync>d__.<>1__state = -1;
			<GetTagsAsync>d__.<>t__builder.Start<ModApiService.<GetTagsAsync>d__9>(ref <GetTagsAsync>d__);
			return <GetTagsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x0003A8B0 File Offset: 0x00038AB0
		public Task<bool> DownloadModAsync(string downloadUrl, string destinationPath, [Nullable(2)] IProgress<double> progress = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			ModApiService.<DownloadModAsync>d__10 <DownloadModAsync>d__;
			<DownloadModAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<DownloadModAsync>d__.<>4__this = this;
			<DownloadModAsync>d__.downloadUrl = downloadUrl;
			<DownloadModAsync>d__.destinationPath = destinationPath;
			<DownloadModAsync>d__.progress = progress;
			<DownloadModAsync>d__.cancellationToken = cancellationToken;
			<DownloadModAsync>d__.<>1__state = -1;
			<DownloadModAsync>d__.<>t__builder.Start<ModApiService.<DownloadModAsync>d__10>(ref <DownloadModAsync>d__);
			return <DownloadModAsync>d__.<>t__builder.Task;
		}

		// Token: 0x04000499 RID: 1177
		private const string BaseUrl = "https://mods.vintagestory.at";

		// Token: 0x0400049A RID: 1178
		private readonly HttpClient _httpClient;

		// Token: 0x0400049B RID: 1179
		private readonly JsonSerializerOptions _jsonOptions;
	}
}
