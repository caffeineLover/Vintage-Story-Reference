using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using VintageStoryModManager.Models;

namespace VintageStoryModManager.Services
{
	// Token: 0x0200006E RID: 110
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModDatabaseService
	{
		// Token: 0x06000A06 RID: 2566 RVA: 0x0003D368 File Offset: 0x0003B568
		private static int CalculateRequestLimit(int maxResults)
		{
			long num = (long)maxResults * 4L;
			if (num < (long)maxResults)
			{
				num = (long)maxResults;
			}
			if (num > 2147483647L)
			{
				return int.MaxValue;
			}
			return (int)num;
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x0003D394 File Offset: 0x0003B594
		public Task PopulateModDatabaseInfoAsync(IEnumerable<ModEntry> mods, [Nullable(2)] string installedGameVersion, bool requireExactVersionMatch = false, CancellationToken cancellationToken = default(CancellationToken))
		{
			ModDatabaseService.<PopulateModDatabaseInfoAsync>d__14 <PopulateModDatabaseInfoAsync>d__;
			<PopulateModDatabaseInfoAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<PopulateModDatabaseInfoAsync>d__.mods = mods;
			<PopulateModDatabaseInfoAsync>d__.installedGameVersion = installedGameVersion;
			<PopulateModDatabaseInfoAsync>d__.requireExactVersionMatch = requireExactVersionMatch;
			<PopulateModDatabaseInfoAsync>d__.cancellationToken = cancellationToken;
			<PopulateModDatabaseInfoAsync>d__.<>1__state = -1;
			<PopulateModDatabaseInfoAsync>d__.<>t__builder.Start<ModDatabaseService.<PopulateModDatabaseInfoAsync>d__14>(ref <PopulateModDatabaseInfoAsync>d__);
			return <PopulateModDatabaseInfoAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x0003D3F0 File Offset: 0x0003B5F0
		private static bool IsCacheSoftExpired(DateTimeOffset? cachedAt)
		{
			return cachedAt == null || DateTimeOffset.Now - cachedAt.Value > ModDatabaseService.ModCacheSoftExpiry;
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x0003D418 File Offset: 0x0003B618
		private static bool IsCacheHardExpired(DateTimeOffset? cachedAt)
		{
			return cachedAt == null || DateTimeOffset.Now - cachedAt.Value > ModDatabaseService.ModCacheHardExpiry;
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x0003D440 File Offset: 0x0003B640
		private Task<bool> CheckIfRefreshNeededAsync(string modId, [Nullable(2)] string normalizedGameVersion, CancellationToken cancellationToken)
		{
			ModDatabaseService.<CheckIfRefreshNeededAsync>d__19 <CheckIfRefreshNeededAsync>d__;
			<CheckIfRefreshNeededAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<CheckIfRefreshNeededAsync>d__.modId = modId;
			<CheckIfRefreshNeededAsync>d__.normalizedGameVersion = normalizedGameVersion;
			<CheckIfRefreshNeededAsync>d__.cancellationToken = cancellationToken;
			<CheckIfRefreshNeededAsync>d__.<>1__state = -1;
			<CheckIfRefreshNeededAsync>d__.<>t__builder.Start<ModDatabaseService.<CheckIfRefreshNeededAsync>d__19>(ref <CheckIfRefreshNeededAsync>d__);
			return <CheckIfRefreshNeededAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x0003D494 File Offset: 0x0003B694
		[NullableContext(2)]
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		public Task<ModDatabaseInfo> TryLoadDatabaseInfoAsync([Nullable(1)] string modId, string modVersion, string installedGameVersion, bool requireExactVersionMatch = false, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (string.IsNullOrWhiteSpace(modId))
			{
				return Task.FromResult<ModDatabaseInfo>(null);
			}
			string normalizedGameVersion = VersionStringUtility.Normalize(installedGameVersion);
			return this.TryLoadDatabaseInfoAsyncCore(modId, modVersion, normalizedGameVersion, requireExactVersionMatch, null, cancellationToken, null);
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x0003D4C8 File Offset: 0x0003B6C8
		[NullableContext(2)]
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		public Task<ModDatabaseInfo> TryLoadDatabaseInfoAsync([Nullable(1)] string modId, string modVersion, string installedGameVersion, bool requireExactVersionMatch, ModDatabaseInfo preloadedCachedInfo, CancellationToken cancellationToken = default(CancellationToken), ModLoadingTimingService timingService = null)
		{
			if (string.IsNullOrWhiteSpace(modId))
			{
				return Task.FromResult<ModDatabaseInfo>(null);
			}
			string normalizedGameVersion = VersionStringUtility.Normalize(installedGameVersion);
			return this.TryLoadDatabaseInfoAsyncCore(modId, modVersion, normalizedGameVersion, requireExactVersionMatch, preloadedCachedInfo, cancellationToken, timingService);
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x0003D4FC File Offset: 0x0003B6FC
		[NullableContext(2)]
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		public Task<ModDatabaseInfo> TryLoadCachedDatabaseInfoAsync([Nullable(1)] string modId, string modVersion, string installedGameVersion, bool requireExactVersionMatch = false, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (string.IsNullOrWhiteSpace(modId))
			{
				return Task.FromResult<ModDatabaseInfo>(null);
			}
			string normalizedGameVersion = VersionStringUtility.Normalize(installedGameVersion);
			return ModDatabaseService.CacheService.TryLoadAsync(modId, normalizedGameVersion, modVersion, false, requireExactVersionMatch, cancellationToken);
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x0003D534 File Offset: 0x0003B734
		[NullableContext(2)]
		[return: TupleElementNames(new string[]
		{
			"Info",
			"NeedsRefresh"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0,
			2
		})]
		public Task<ValueTuple<ModDatabaseInfo, bool>> TryLoadCachedDatabaseInfoWithRefreshCheckAsync([Nullable(1)] string modId, string modVersion, string installedGameVersion, bool requireExactVersionMatch = false, CancellationToken cancellationToken = default(CancellationToken))
		{
			ModDatabaseService.<TryLoadCachedDatabaseInfoWithRefreshCheckAsync>d__23 <TryLoadCachedDatabaseInfoWithRefreshCheckAsync>d__;
			<TryLoadCachedDatabaseInfoWithRefreshCheckAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ValueTuple<ModDatabaseInfo, bool>>.Create();
			<TryLoadCachedDatabaseInfoWithRefreshCheckAsync>d__.<>4__this = this;
			<TryLoadCachedDatabaseInfoWithRefreshCheckAsync>d__.modId = modId;
			<TryLoadCachedDatabaseInfoWithRefreshCheckAsync>d__.modVersion = modVersion;
			<TryLoadCachedDatabaseInfoWithRefreshCheckAsync>d__.installedGameVersion = installedGameVersion;
			<TryLoadCachedDatabaseInfoWithRefreshCheckAsync>d__.requireExactVersionMatch = requireExactVersionMatch;
			<TryLoadCachedDatabaseInfoWithRefreshCheckAsync>d__.cancellationToken = cancellationToken;
			<TryLoadCachedDatabaseInfoWithRefreshCheckAsync>d__.<>1__state = -1;
			<TryLoadCachedDatabaseInfoWithRefreshCheckAsync>d__.<>t__builder.Start<ModDatabaseService.<TryLoadCachedDatabaseInfoWithRefreshCheckAsync>d__23>(ref <TryLoadCachedDatabaseInfoWithRefreshCheckAsync>d__);
			return <TryLoadCachedDatabaseInfoWithRefreshCheckAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x0003D5A4 File Offset: 0x0003B7A4
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		public Task<string> TryFetchLatestReleaseVersionAsync(string modId, CancellationToken cancellationToken)
		{
			ModDatabaseService.<TryFetchLatestReleaseVersionAsync>d__24 <TryFetchLatestReleaseVersionAsync>d__;
			<TryFetchLatestReleaseVersionAsync>d__.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
			<TryFetchLatestReleaseVersionAsync>d__.modId = modId;
			<TryFetchLatestReleaseVersionAsync>d__.cancellationToken = cancellationToken;
			<TryFetchLatestReleaseVersionAsync>d__.<>1__state = -1;
			<TryFetchLatestReleaseVersionAsync>d__.<>t__builder.Start<ModDatabaseService.<TryFetchLatestReleaseVersionAsync>d__24>(ref <TryFetchLatestReleaseVersionAsync>d__);
			return <TryFetchLatestReleaseVersionAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x0003D5F0 File Offset: 0x0003B7F0
		[NullableContext(2)]
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		private Task<ModDatabaseInfo> TryLoadDatabaseInfoAsyncCore([Nullable(1)] string modId, string modVersion, string normalizedGameVersion, bool requireExactVersionMatch, ModDatabaseInfo preloadedCachedInfo, CancellationToken cancellationToken, ModLoadingTimingService timingService = null)
		{
			ModDatabaseService.<TryLoadDatabaseInfoAsyncCore>d__25 <TryLoadDatabaseInfoAsyncCore>d__;
			<TryLoadDatabaseInfoAsyncCore>d__.<>t__builder = AsyncTaskMethodBuilder<ModDatabaseInfo>.Create();
			<TryLoadDatabaseInfoAsyncCore>d__.<>4__this = this;
			<TryLoadDatabaseInfoAsyncCore>d__.modId = modId;
			<TryLoadDatabaseInfoAsyncCore>d__.modVersion = modVersion;
			<TryLoadDatabaseInfoAsyncCore>d__.normalizedGameVersion = normalizedGameVersion;
			<TryLoadDatabaseInfoAsyncCore>d__.requireExactVersionMatch = requireExactVersionMatch;
			<TryLoadDatabaseInfoAsyncCore>d__.preloadedCachedInfo = preloadedCachedInfo;
			<TryLoadDatabaseInfoAsyncCore>d__.cancellationToken = cancellationToken;
			<TryLoadDatabaseInfoAsyncCore>d__.timingService = timingService;
			<TryLoadDatabaseInfoAsyncCore>d__.<>1__state = -1;
			<TryLoadDatabaseInfoAsyncCore>d__.<>t__builder.Start<ModDatabaseService.<TryLoadDatabaseInfoAsyncCore>d__25>(ref <TryLoadDatabaseInfoAsyncCore>d__);
			return <TryLoadDatabaseInfoAsyncCore>d__.<>t__builder.Task;
		}

		// Token: 0x06000A11 RID: 2577 RVA: 0x0003D670 File Offset: 0x0003B870
		public Task<IReadOnlyList<ModDatabaseSearchResult>> SearchModsAsync(string query, int maxResults, CancellationToken cancellationToken = default(CancellationToken))
		{
			ModDatabaseService.<SearchModsAsync>d__26 <SearchModsAsync>d__;
			<SearchModsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IReadOnlyList<ModDatabaseSearchResult>>.Create();
			<SearchModsAsync>d__.<>4__this = this;
			<SearchModsAsync>d__.query = query;
			<SearchModsAsync>d__.maxResults = maxResults;
			<SearchModsAsync>d__.cancellationToken = cancellationToken;
			<SearchModsAsync>d__.<>1__state = -1;
			<SearchModsAsync>d__.<>t__builder.Start<ModDatabaseService.<SearchModsAsync>d__26>(ref <SearchModsAsync>d__);
			return <SearchModsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x0003D6CC File Offset: 0x0003B8CC
		public Task<IReadOnlyList<ModDatabaseSearchResult>> GetMostDownloadedModsAsync(int maxResults, CancellationToken cancellationToken = default(CancellationToken))
		{
			ModDatabaseService.<GetMostDownloadedModsAsync>d__27 <GetMostDownloadedModsAsync>d__;
			<GetMostDownloadedModsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IReadOnlyList<ModDatabaseSearchResult>>.Create();
			<GetMostDownloadedModsAsync>d__.<>4__this = this;
			<GetMostDownloadedModsAsync>d__.maxResults = maxResults;
			<GetMostDownloadedModsAsync>d__.cancellationToken = cancellationToken;
			<GetMostDownloadedModsAsync>d__.<>1__state = -1;
			<GetMostDownloadedModsAsync>d__.<>t__builder.Start<ModDatabaseService.<GetMostDownloadedModsAsync>d__27>(ref <GetMostDownloadedModsAsync>d__);
			return <GetMostDownloadedModsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000A13 RID: 2579 RVA: 0x0003D720 File Offset: 0x0003B920
		public Task<IReadOnlyList<ModDatabaseSearchResult>> GetMostDownloadedModsLastThirtyDaysAsync(int maxResults, CancellationToken cancellationToken = default(CancellationToken))
		{
			ModDatabaseService.<GetMostDownloadedModsLastThirtyDaysAsync>d__28 <GetMostDownloadedModsLastThirtyDaysAsync>d__;
			<GetMostDownloadedModsLastThirtyDaysAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IReadOnlyList<ModDatabaseSearchResult>>.Create();
			<GetMostDownloadedModsLastThirtyDaysAsync>d__.<>4__this = this;
			<GetMostDownloadedModsLastThirtyDaysAsync>d__.maxResults = maxResults;
			<GetMostDownloadedModsLastThirtyDaysAsync>d__.cancellationToken = cancellationToken;
			<GetMostDownloadedModsLastThirtyDaysAsync>d__.<>1__state = -1;
			<GetMostDownloadedModsLastThirtyDaysAsync>d__.<>t__builder.Start<ModDatabaseService.<GetMostDownloadedModsLastThirtyDaysAsync>d__28>(ref <GetMostDownloadedModsLastThirtyDaysAsync>d__);
			return <GetMostDownloadedModsLastThirtyDaysAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000A14 RID: 2580 RVA: 0x0003D774 File Offset: 0x0003B974
		public Task<IReadOnlyList<ModDatabaseSearchResult>> GetMostDownloadedModsLastTenDaysAsync(int maxResults, CancellationToken cancellationToken = default(CancellationToken))
		{
			ModDatabaseService.<GetMostDownloadedModsLastTenDaysAsync>d__29 <GetMostDownloadedModsLastTenDaysAsync>d__;
			<GetMostDownloadedModsLastTenDaysAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IReadOnlyList<ModDatabaseSearchResult>>.Create();
			<GetMostDownloadedModsLastTenDaysAsync>d__.<>4__this = this;
			<GetMostDownloadedModsLastTenDaysAsync>d__.maxResults = maxResults;
			<GetMostDownloadedModsLastTenDaysAsync>d__.cancellationToken = cancellationToken;
			<GetMostDownloadedModsLastTenDaysAsync>d__.<>1__state = -1;
			<GetMostDownloadedModsLastTenDaysAsync>d__.<>t__builder.Start<ModDatabaseService.<GetMostDownloadedModsLastTenDaysAsync>d__29>(ref <GetMostDownloadedModsLastTenDaysAsync>d__);
			return <GetMostDownloadedModsLastTenDaysAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x0003D7C8 File Offset: 0x0003B9C8
		public Task<IReadOnlyList<ModDatabaseSearchResult>> GetMostDownloadedNewModsAsync(int months, int maxResults, CancellationToken cancellationToken = default(CancellationToken))
		{
			ModDatabaseService.<GetMostDownloadedNewModsAsync>d__30 <GetMostDownloadedNewModsAsync>d__;
			<GetMostDownloadedNewModsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IReadOnlyList<ModDatabaseSearchResult>>.Create();
			<GetMostDownloadedNewModsAsync>d__.<>4__this = this;
			<GetMostDownloadedNewModsAsync>d__.months = months;
			<GetMostDownloadedNewModsAsync>d__.maxResults = maxResults;
			<GetMostDownloadedNewModsAsync>d__.cancellationToken = cancellationToken;
			<GetMostDownloadedNewModsAsync>d__.<>1__state = -1;
			<GetMostDownloadedNewModsAsync>d__.<>t__builder.Start<ModDatabaseService.<GetMostDownloadedNewModsAsync>d__30>(ref <GetMostDownloadedNewModsAsync>d__);
			return <GetMostDownloadedNewModsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x0003D824 File Offset: 0x0003BA24
		public Task<IReadOnlyList<ModDatabaseSearchResult>> GetRecentlyUpdatedModsAsync(int maxResults, CancellationToken cancellationToken = default(CancellationToken))
		{
			ModDatabaseService.<GetRecentlyUpdatedModsAsync>d__31 <GetRecentlyUpdatedModsAsync>d__;
			<GetRecentlyUpdatedModsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IReadOnlyList<ModDatabaseSearchResult>>.Create();
			<GetRecentlyUpdatedModsAsync>d__.<>4__this = this;
			<GetRecentlyUpdatedModsAsync>d__.maxResults = maxResults;
			<GetRecentlyUpdatedModsAsync>d__.cancellationToken = cancellationToken;
			<GetRecentlyUpdatedModsAsync>d__.<>1__state = -1;
			<GetRecentlyUpdatedModsAsync>d__.<>t__builder.Start<ModDatabaseService.<GetRecentlyUpdatedModsAsync>d__31>(ref <GetRecentlyUpdatedModsAsync>d__);
			return <GetRecentlyUpdatedModsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x0003D878 File Offset: 0x0003BA78
		public Task<IReadOnlyList<ModDatabaseSearchResult>> GetRecentlyAddedModsAsync(int maxResults, CancellationToken cancellationToken = default(CancellationToken))
		{
			ModDatabaseService.<GetRecentlyAddedModsAsync>d__32 <GetRecentlyAddedModsAsync>d__;
			<GetRecentlyAddedModsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IReadOnlyList<ModDatabaseSearchResult>>.Create();
			<GetRecentlyAddedModsAsync>d__.<>4__this = this;
			<GetRecentlyAddedModsAsync>d__.maxResults = maxResults;
			<GetRecentlyAddedModsAsync>d__.cancellationToken = cancellationToken;
			<GetRecentlyAddedModsAsync>d__.<>1__state = -1;
			<GetRecentlyAddedModsAsync>d__.<>t__builder.Start<ModDatabaseService.<GetRecentlyAddedModsAsync>d__32>(ref <GetRecentlyAddedModsAsync>d__);
			return <GetRecentlyAddedModsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x0003D8CC File Offset: 0x0003BACC
		public Task<IReadOnlyList<ModDatabaseSearchResult>> GetMostTrendingModsAsync(int maxResults, CancellationToken cancellationToken = default(CancellationToken))
		{
			ModDatabaseService.<GetMostTrendingModsAsync>d__33 <GetMostTrendingModsAsync>d__;
			<GetMostTrendingModsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IReadOnlyList<ModDatabaseSearchResult>>.Create();
			<GetMostTrendingModsAsync>d__.<>4__this = this;
			<GetMostTrendingModsAsync>d__.maxResults = maxResults;
			<GetMostTrendingModsAsync>d__.cancellationToken = cancellationToken;
			<GetMostTrendingModsAsync>d__.<>1__state = -1;
			<GetMostTrendingModsAsync>d__.<>t__builder.Start<ModDatabaseService.<GetMostTrendingModsAsync>d__33>(ref <GetMostTrendingModsAsync>d__);
			return <GetMostTrendingModsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x0003D920 File Offset: 0x0003BB20
		public Task<IReadOnlyList<ModDatabaseSearchResult>> GetRandomModsAsync(int maxResults, CancellationToken cancellationToken = default(CancellationToken))
		{
			ModDatabaseService.<GetRandomModsAsync>d__34 <GetRandomModsAsync>d__;
			<GetRandomModsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IReadOnlyList<ModDatabaseSearchResult>>.Create();
			<GetRandomModsAsync>d__.<>4__this = this;
			<GetRandomModsAsync>d__.maxResults = maxResults;
			<GetRandomModsAsync>d__.cancellationToken = cancellationToken;
			<GetRandomModsAsync>d__.<>1__state = -1;
			<GetRandomModsAsync>d__.<>t__builder.Start<ModDatabaseService.<GetRandomModsAsync>d__34>(ref <GetRandomModsAsync>d__);
			return <GetRandomModsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x0003D974 File Offset: 0x0003BB74
		private static bool WasCreatedOnOrAfter(ModDatabaseSearchResult candidate, DateTime thresholdUtc)
		{
			DateTime? dateTime = candidate.CreatedUtc;
			DateTime? dateTime2;
			if (dateTime == null)
			{
				ModDatabaseInfo detailedInfo = candidate.DetailedInfo;
				dateTime2 = ((detailedInfo != null) ? detailedInfo.CreatedUtc : default(DateTime?));
			}
			else
			{
				dateTime2 = dateTime;
			}
			DateTime? dateTime3 = dateTime2;
			if (dateTime3 != null)
			{
				return dateTime3.Value >= thresholdUtc;
			}
			ModDatabaseInfo detailedInfo2 = candidate.DetailedInfo;
			IReadOnlyList<ModReleaseInfo> readOnlyList = (detailedInfo2 != null) ? detailedInfo2.Releases : null;
			if (readOnlyList != null && readOnlyList.Count > 0)
			{
				DateTime? dateTime4 = default(DateTime?);
				foreach (ModReleaseInfo modReleaseInfo in readOnlyList)
				{
					dateTime = ((modReleaseInfo != null) ? modReleaseInfo.CreatedUtc : default(DateTime?));
					if (dateTime != null)
					{
						DateTime valueOrDefault = dateTime.GetValueOrDefault();
						if (dateTime4 == null || valueOrDefault < dateTime4.Value)
						{
							dateTime4 = new DateTime?(valueOrDefault);
						}
					}
				}
				if (dateTime4 != null)
				{
					return dateTime4.Value >= thresholdUtc;
				}
			}
			dateTime = candidate.LastReleasedUtc;
			if (dateTime != null)
			{
				DateTime valueOrDefault2 = dateTime.GetValueOrDefault();
				return valueOrDefault2 >= thresholdUtc;
			}
			return false;
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x0003DAB0 File Offset: 0x0003BCB0
		private Task<IReadOnlyList<ModDatabaseSearchResult>> QueryModsAsync(string requestUri, int maxResults, IReadOnlyList<string> tokens, bool requireTokenMatch, Func<IEnumerable<ModDatabaseSearchResult>, IEnumerable<ModDatabaseSearchResult>> orderResults, CancellationToken cancellationToken)
		{
			ModDatabaseService.<QueryModsAsync>d__36 <QueryModsAsync>d__;
			<QueryModsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IReadOnlyList<ModDatabaseSearchResult>>.Create();
			<QueryModsAsync>d__.requestUri = requestUri;
			<QueryModsAsync>d__.maxResults = maxResults;
			<QueryModsAsync>d__.tokens = tokens;
			<QueryModsAsync>d__.requireTokenMatch = requireTokenMatch;
			<QueryModsAsync>d__.orderResults = orderResults;
			<QueryModsAsync>d__.cancellationToken = cancellationToken;
			<QueryModsAsync>d__.<>1__state = -1;
			<QueryModsAsync>d__.<>t__builder.Start<ModDatabaseService.<QueryModsAsync>d__36>(ref <QueryModsAsync>d__);
			return <QueryModsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x0003DB20 File Offset: 0x0003BD20
		private Task<IReadOnlyList<ModDatabaseSearchResult>> EnrichWithLatestReleaseDownloadsAsync(IReadOnlyList<ModDatabaseSearchResult> candidates, CancellationToken cancellationToken)
		{
			ModDatabaseService.<EnrichWithLatestReleaseDownloadsAsync>d__37 <EnrichWithLatestReleaseDownloadsAsync>d__;
			<EnrichWithLatestReleaseDownloadsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IReadOnlyList<ModDatabaseSearchResult>>.Create();
			<EnrichWithLatestReleaseDownloadsAsync>d__.<>4__this = this;
			<EnrichWithLatestReleaseDownloadsAsync>d__.candidates = candidates;
			<EnrichWithLatestReleaseDownloadsAsync>d__.cancellationToken = cancellationToken;
			<EnrichWithLatestReleaseDownloadsAsync>d__.<>1__state = -1;
			<EnrichWithLatestReleaseDownloadsAsync>d__.<>t__builder.Start<ModDatabaseService.<EnrichWithLatestReleaseDownloadsAsync>d__37>(ref <EnrichWithLatestReleaseDownloadsAsync>d__);
			return <EnrichWithLatestReleaseDownloadsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x0003DB74 File Offset: 0x0003BD74
		private Task<ModDatabaseSearchResult> EnrichCandidateWithLatestReleaseDownloadsAsync(ModDatabaseSearchResult candidate, SemaphoreSlim semaphore, CancellationToken cancellationToken)
		{
			ModDatabaseService.<EnrichCandidateWithLatestReleaseDownloadsAsync>d__38 <EnrichCandidateWithLatestReleaseDownloadsAsync>d__;
			<EnrichCandidateWithLatestReleaseDownloadsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ModDatabaseSearchResult>.Create();
			<EnrichCandidateWithLatestReleaseDownloadsAsync>d__.<>4__this = this;
			<EnrichCandidateWithLatestReleaseDownloadsAsync>d__.candidate = candidate;
			<EnrichCandidateWithLatestReleaseDownloadsAsync>d__.semaphore = semaphore;
			<EnrichCandidateWithLatestReleaseDownloadsAsync>d__.cancellationToken = cancellationToken;
			<EnrichCandidateWithLatestReleaseDownloadsAsync>d__.<>1__state = -1;
			<EnrichCandidateWithLatestReleaseDownloadsAsync>d__.<>t__builder.Start<ModDatabaseService.<EnrichCandidateWithLatestReleaseDownloadsAsync>d__38>(ref <EnrichCandidateWithLatestReleaseDownloadsAsync>d__);
			return <EnrichCandidateWithLatestReleaseDownloadsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x0003DBD0 File Offset: 0x0003BDD0
		private static ModDatabaseSearchResult CloneResultWithDetails(ModDatabaseSearchResult source, [Nullable(2)] ModDatabaseInfo info, int? latestDownloads)
		{
			ModDatabaseSearchResult modDatabaseSearchResult = new ModDatabaseSearchResult();
			modDatabaseSearchResult.ModId = source.ModId;
			modDatabaseSearchResult.Name = source.Name;
			modDatabaseSearchResult.AlternateIds = source.AlternateIds;
			modDatabaseSearchResult.Summary = source.Summary;
			modDatabaseSearchResult.Author = source.Author;
			modDatabaseSearchResult.Tags = source.Tags;
			modDatabaseSearchResult.Downloads = source.Downloads;
			modDatabaseSearchResult.Follows = source.Follows;
			modDatabaseSearchResult.TrendingPoints = source.TrendingPoints;
			modDatabaseSearchResult.Comments = source.Comments;
			modDatabaseSearchResult.AssetId = source.AssetId;
			modDatabaseSearchResult.UrlAlias = source.UrlAlias;
			modDatabaseSearchResult.Side = source.Side;
			modDatabaseSearchResult.LogoUrl = source.LogoUrl;
			modDatabaseSearchResult.LogoUrlSource = source.LogoUrlSource;
			modDatabaseSearchResult.LastReleasedUtc = source.LastReleasedUtc;
			DateTime? dateTime = (info != null) ? info.CreatedUtc : default(DateTime?);
			modDatabaseSearchResult.CreatedUtc = ((dateTime != null) ? dateTime : source.CreatedUtc);
			modDatabaseSearchResult.Score = source.Score;
			modDatabaseSearchResult.LatestReleaseDownloads = latestDownloads;
			modDatabaseSearchResult.DetailedInfo = info;
			return modDatabaseSearchResult;
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x0003DCEC File Offset: 0x0003BEEC
		[NullableContext(2)]
		private static int? ExtractLatestReleaseDownloads(ModDatabaseInfo info)
		{
			if (info == null)
			{
				return default(int?);
			}
			ModReleaseInfo latestRelease = info.LatestRelease;
			int? num = (latestRelease != null) ? latestRelease.Downloads : default(int?);
			if (num != null)
			{
				int valueOrDefault = num.GetValueOrDefault();
				return new int?(valueOrDefault);
			}
			if (info.Releases.Count > 0)
			{
				ModReleaseInfo modReleaseInfo = info.Releases[0];
				num = ((modReleaseInfo != null) ? modReleaseInfo.Downloads : default(int?));
				if (num != null)
				{
					int valueOrDefault2 = num.GetValueOrDefault();
					return new int?(valueOrDefault2);
				}
			}
			return default(int?);
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x0003DD8C File Offset: 0x0003BF8C
		[NullableContext(2)]
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		private static Task<ModDatabaseInfo> TryLoadDatabaseInfoInternalAsync([Nullable(1)] string modId, string modVersion, string normalizedGameVersion, bool requireExactVersionMatch, CancellationToken cancellationToken, ModLoadingTimingService timingService = null)
		{
			ModDatabaseService.<TryLoadDatabaseInfoInternalAsync>d__41 <TryLoadDatabaseInfoInternalAsync>d__;
			<TryLoadDatabaseInfoInternalAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ModDatabaseInfo>.Create();
			<TryLoadDatabaseInfoInternalAsync>d__.modId = modId;
			<TryLoadDatabaseInfoInternalAsync>d__.modVersion = modVersion;
			<TryLoadDatabaseInfoInternalAsync>d__.normalizedGameVersion = normalizedGameVersion;
			<TryLoadDatabaseInfoInternalAsync>d__.requireExactVersionMatch = requireExactVersionMatch;
			<TryLoadDatabaseInfoInternalAsync>d__.cancellationToken = cancellationToken;
			<TryLoadDatabaseInfoInternalAsync>d__.timingService = timingService;
			<TryLoadDatabaseInfoInternalAsync>d__.<>1__state = -1;
			<TryLoadDatabaseInfoInternalAsync>d__.<>t__builder.Start<ModDatabaseService.<TryLoadDatabaseInfoInternalAsync>d__41>(ref <TryLoadDatabaseInfoInternalAsync>d__);
			return <TryLoadDatabaseInfoInternalAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x0003DDFC File Offset: 0x0003BFFC
		[NullableContext(2)]
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		private static Task<ModDatabaseInfo> TryLoadDatabaseInfoFromPageAsync([Nullable(1)] string modId, string normalizedModVersion, string normalizedGameVersion, bool requireExactVersionMatch, CancellationToken cancellationToken)
		{
			ModDatabaseService.<TryLoadDatabaseInfoFromPageAsync>d__42 <TryLoadDatabaseInfoFromPageAsync>d__;
			<TryLoadDatabaseInfoFromPageAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ModDatabaseInfo>.Create();
			<TryLoadDatabaseInfoFromPageAsync>d__.modId = modId;
			<TryLoadDatabaseInfoFromPageAsync>d__.normalizedModVersion = normalizedModVersion;
			<TryLoadDatabaseInfoFromPageAsync>d__.normalizedGameVersion = normalizedGameVersion;
			<TryLoadDatabaseInfoFromPageAsync>d__.requireExactVersionMatch = requireExactVersionMatch;
			<TryLoadDatabaseInfoFromPageAsync>d__.cancellationToken = cancellationToken;
			<TryLoadDatabaseInfoFromPageAsync>d__.<>1__state = -1;
			<TryLoadDatabaseInfoFromPageAsync>d__.<>t__builder.Start<ModDatabaseService.<TryLoadDatabaseInfoFromPageAsync>d__42>(ref <TryLoadDatabaseInfoFromPageAsync>d__);
			return <TryLoadDatabaseInfoFromPageAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x0003DE60 File Offset: 0x0003C060
		private static string CleanHtmlText([Nullable(2)] string value)
		{
			return HtmlEntity.DeEntitize(value ?? string.Empty).Trim();
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x0003DE78 File Offset: 0x0003C078
		private static int? TryParseInt(string value)
		{
			int num;
			if (!int.TryParse(value, 7, CultureInfo.InvariantCulture, ref num))
			{
				return default(int?);
			}
			return new int?(num);
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x0003DEA8 File Offset: 0x0003C0A8
		private static IReadOnlyList<string> GetStringList(JsonElement element, string propertyName)
		{
			JsonElement jsonElement;
			if (!element.TryGetProperty(propertyName, ref jsonElement) || jsonElement.ValueKind != 2)
			{
				return Array.Empty<string>();
			}
			List<string> list = new List<string>();
			foreach (JsonElement jsonElement2 in jsonElement.EnumerateArray())
			{
				if (jsonElement2.ValueKind == 3)
				{
					string @string = jsonElement2.GetString();
					if (!string.IsNullOrWhiteSpace(@string))
					{
						list.Add(@string);
					}
				}
			}
			if (list.Count != 0)
			{
				return list.ToArray();
			}
			return Array.Empty<string>();
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x0003DF54 File Offset: 0x0003C154
		[NullableContext(2)]
		private static string TryGetAssetId(JsonElement element)
		{
			JsonElement jsonElement;
			if (!element.TryGetProperty("assetid", ref jsonElement))
			{
				return null;
			}
			JsonValueKind valueKind = jsonElement.ValueKind;
			string result;
			if (valueKind != 3)
			{
				if (valueKind == 4)
				{
					long num;
					if (jsonElement.TryGetInt64(ref num))
					{
						return num.ToString(CultureInfo.InvariantCulture);
					}
					decimal num2;
					if (jsonElement.TryGetDecimal(ref num2))
					{
						return num2.ToString(CultureInfo.InvariantCulture);
					}
				}
				result = null;
			}
			else
			{
				result = (string.IsNullOrWhiteSpace(jsonElement.GetString()) ? null : jsonElement.GetString());
			}
			return result;
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x0003DFD8 File Offset: 0x0003C1D8
		private static IReadOnlyList<ModReleaseInfo> BuildReleaseInfos(JsonElement modElement, [Nullable(2)] string normalizedGameVersion, bool requireExactVersionMatch)
		{
			JsonElement jsonElement;
			if (!modElement.TryGetProperty("releases", ref jsonElement) || jsonElement.ValueKind != 2)
			{
				return Array.Empty<ModReleaseInfo>();
			}
			List<ModReleaseInfo> list = new List<ModReleaseInfo>();
			foreach (JsonElement releaseElement in jsonElement.EnumerateArray())
			{
				ModReleaseInfo modReleaseInfo;
				if (releaseElement.ValueKind == 1 && ModDatabaseService.TryCreateReleaseInfo(releaseElement, normalizedGameVersion, requireExactVersionMatch, out modReleaseInfo))
				{
					list.Add(modReleaseInfo);
				}
			}
			if (list.Count != 0)
			{
				return list;
			}
			return Array.Empty<ModReleaseInfo>();
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x0003E088 File Offset: 0x0003C288
		private static int? CalculateDownloadsLastThirtyDays(IReadOnlyList<ModReleaseInfo> releases)
		{
			return ModDatabaseService.CalculateDownloadsForPeriod(releases, 30);
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x0003E092 File Offset: 0x0003C292
		private static int? CalculateDownloadsLastTenDays(IReadOnlyList<ModReleaseInfo> releases)
		{
			return ModDatabaseService.CalculateDownloadsForPeriod(releases, 10);
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x0003E09C File Offset: 0x0003C29C
		private static int? CalculateDownloadsForPeriod(IReadOnlyList<ModReleaseInfo> releases, int days)
		{
			if (releases.Count == 0)
			{
				return default(int?);
			}
			DateTime utcNow = DateTime.UtcNow;
			DateTime dateTime = utcNow.AddDays((double)(-(double)days));
			ModReleaseInfo[] array = Enumerable.ToArray<ModReleaseInfo>(Enumerable.OrderByDescending<ModReleaseInfo, DateTime>(Enumerable.Where<ModReleaseInfo>(releases, (ModReleaseInfo release) => release != null && release.CreatedUtc != null && release.Downloads != null), (ModReleaseInfo release) => release.CreatedUtc.Value));
			if (array.Length == 0)
			{
				return default(int?);
			}
			double modDatabaseMinimumIntervalDays = DevConfig.ModDatabaseMinimumIntervalDays;
			double num = 0.0;
			DateTime dateTime2 = utcNow;
			foreach (ModReleaseInfo modReleaseInfo in array)
			{
				if (dateTime2 <= dateTime)
				{
					break;
				}
				DateTime dateTime3 = modReleaseInfo.CreatedUtc.Value;
				if (dateTime3 > dateTime2)
				{
					dateTime3 = dateTime2;
				}
				double totalDays = (dateTime2 - dateTime3).TotalDays;
				if (totalDays <= 0.0)
				{
					dateTime2 = dateTime3;
				}
				else
				{
					double num2 = (double)Math.Max(modReleaseInfo.Downloads.Value, 0) / Math.Max(totalDays, modDatabaseMinimumIntervalDays);
					DateTime dateTime4 = (dateTime3 < dateTime) ? dateTime : dateTime3;
					double totalDays2 = (dateTime2 - dateTime4).TotalDays;
					if (totalDays2 > 0.0)
					{
						num += num2 * totalDays2;
					}
					dateTime2 = dateTime3;
					if (dateTime3 <= dateTime)
					{
						break;
					}
				}
			}
			if (num <= 0.0)
			{
				return new int?(0);
			}
			return new int?((int)Math.Round(num, 1));
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x0003E248 File Offset: 0x0003C448
		private static bool TryCreateReleaseInfo(JsonElement releaseElement, [Nullable(2)] string normalizedGameVersion, bool requireExactVersionMatch, out ModReleaseInfo release)
		{
			release = null;
			string @string = ModDatabaseService.GetString(releaseElement, "mainfile");
			Uri downloadUri;
			if (string.IsNullOrWhiteSpace(@string) || !Uri.TryCreate(@string, 1, ref downloadUri))
			{
				return false;
			}
			string text = ModDatabaseService.ExtractReleaseVersion(releaseElement);
			if (string.IsNullOrWhiteSpace(text))
			{
				return false;
			}
			string normalizedVersion = VersionStringUtility.Normalize(text);
			IReadOnlyList<string> stringList = ModDatabaseService.GetStringList(releaseElement, "tags");
			bool isCompatibleWithInstalledGame = TargetGameVersionReleaseSelector.TagsSupportTarget(stringList, normalizedGameVersion, requireExactVersionMatch);
			string string2 = ModDatabaseService.GetString(releaseElement, "filename");
			string changelog = ModDatabaseService.ConvertChangelogToPlainText(ModDatabaseService.GetString(releaseElement, "changelog"));
			int? nullableInt = ModDatabaseService.GetNullableInt(releaseElement, "downloads");
			DateTime? createdUtc = ModDatabaseService.TryParseDateTime(ModDatabaseService.GetString(releaseElement, "created"));
			release = new ModReleaseInfo
			{
				Version = text,
				NormalizedVersion = normalizedVersion,
				DownloadUri = downloadUri,
				FileName = string2,
				GameVersionTags = stringList,
				IsCompatibleWithInstalledGame = isCompatibleWithInstalledGame,
				Changelog = changelog,
				Downloads = nullableInt,
				CreatedUtc = createdUtc
			};
			return true;
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x0003E334 File Offset: 0x0003C534
		[NullableContext(2)]
		private static bool TryGetLatestReleaseVersion(JsonElement modElement, out string version)
		{
			version = null;
			JsonElement jsonElement;
			if (!modElement.TryGetProperty("releases", ref jsonElement) || jsonElement.ValueKind != 2)
			{
				return false;
			}
			foreach (JsonElement releaseElement in jsonElement.EnumerateArray())
			{
				if (releaseElement.ValueKind == 1)
				{
					string text = ModDatabaseService.ExtractReleaseVersion(releaseElement);
					if (!string.IsNullOrWhiteSpace(text))
					{
						version = text;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x0003E3CC File Offset: 0x0003C5CC
		[return: Nullable(2)]
		private static string GetString(JsonElement element, string propertyName)
		{
			JsonElement jsonElement;
			if (!element.TryGetProperty(propertyName, ref jsonElement) || jsonElement.ValueKind != 3)
			{
				return null;
			}
			string @string = jsonElement.GetString();
			if (!string.IsNullOrWhiteSpace(@string))
			{
				return @string;
			}
			return null;
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x0003E404 File Offset: 0x0003C604
		[NullableContext(2)]
		private static string ConvertChangelogToPlainText(string changelog)
		{
			if (string.IsNullOrWhiteSpace(changelog))
			{
				return null;
			}
			string text = changelog.Trim();
			if (text.Length == 0)
			{
				return null;
			}
			HtmlDocument htmlDocument = new HtmlDocument();
			htmlDocument.OptionFixNestedTags = true;
			htmlDocument.LoadHtml(text);
			StringBuilder stringBuilder = new StringBuilder(text.Length);
			ModDatabaseService.<ConvertChangelogToPlainText>g__AppendNodes|55_0(htmlDocument.DocumentNode.ChildNodes, stringBuilder, 0);
			text = stringBuilder.ToString();
			text = text.Replace("\r\n", "\n", 4);
			text = text.Replace('\r', '\n');
			string[] array = text.Split('\n', 0);
			List<string> list = new List<string>(array.Length);
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text2 = array2[i].TrimEnd();
				if (text2.Length == 0)
				{
					if (list.Count != 0)
					{
						List<string> list2 = list;
						if (list2[list2.Count - 1].Length != 0)
						{
							list.Add(string.Empty);
						}
					}
				}
				else
				{
					string text3 = text2.TrimStart();
					bool flag = false;
					foreach (string text4 in ModDatabaseService.BulletSymbols)
					{
						if (text3.StartsWith(text4, 4))
						{
							int num = text2.Length - text3.Length;
							string text5 = text2.Substring(0, num);
							string text6 = text3.Substring(text4.Length).Trim();
							list.Add(text5 + text4 + text6);
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						list.Add(text2.Trim());
					}
				}
			}
			while (list.Count > 0)
			{
				List<string> list3 = list;
				if (list3[list3.Count - 1].Length != 0)
				{
					break;
				}
				list.RemoveAt(list.Count - 1);
			}
			if (list.Count == 0)
			{
				return null;
			}
			return string.Join(Environment.NewLine, list);
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x0003E5C8 File Offset: 0x0003C7C8
		[return: Nullable(2)]
		private static ModDatabaseSearchResult TryCreateSearchResult(JsonElement element, IReadOnlyList<string> tokens, bool requireTokenMatch)
		{
			string @string = ModDatabaseService.GetString(element, "name");
			if (string.IsNullOrWhiteSpace(@string))
			{
				return null;
			}
			IReadOnlyList<string> stringList = ModDatabaseService.GetStringList(element, "modidstrs");
			string text;
			if ((text = Enumerable.FirstOrDefault<string>(stringList, (string id) => !string.IsNullOrWhiteSpace(id))) == null)
			{
				text = (ModDatabaseService.GetString(element, "urlalias") ?? @string);
			}
			string text2 = text;
			if (string.IsNullOrWhiteSpace(text2))
			{
				return null;
			}
			text2 = text2.Trim();
			string string2 = ModDatabaseService.GetString(element, "summary");
			string string3 = ModDatabaseService.GetString(element, "author");
			string assetId = ModDatabaseService.TryGetAssetId(element);
			string string4 = ModDatabaseService.GetString(element, "urlalias");
			string string5 = ModDatabaseService.GetString(element, "side");
			string string6 = ModDatabaseService.GetString(element, "logofiledb");
			string logoUrlSource = string.IsNullOrWhiteSpace(string6) ? null : "logofiledb";
			IReadOnlyList<string> stringList2 = ModDatabaseService.GetStringList(element, "tags");
			int @int = ModDatabaseService.GetInt(element, "downloads");
			int int2 = ModDatabaseService.GetInt(element, "follows");
			int int3 = ModDatabaseService.GetInt(element, "trendingpoints");
			int int4 = ModDatabaseService.GetInt(element, "comments");
			DateTime? dateTime = ModDatabaseService.TryParseDateTime(ModDatabaseService.GetString(element, "lastreleased"));
			DateTime? createdUtc = ModDatabaseService.TryParseDateTime(ModDatabaseService.GetString(element, "created"));
			IReadOnlyList<string> readOnlyList;
			if (stringList.Count != 0)
			{
				readOnlyList = stringList;
			}
			else
			{
				IReadOnlyList<string> readOnlyList2 = new string[]
				{
					text2
				};
				readOnlyList = readOnlyList2;
			}
			IReadOnlyList<string> alternateIds = readOnlyList;
			double score;
			if (requireTokenMatch)
			{
				if (!ModDatabaseService.TryCalculateSearchScore(@string, text2, string3, string2, alternateIds, stringList2, tokens, @int, int2, int3, int4, dateTime, out score))
				{
					return null;
				}
			}
			else
			{
				score = (double)@int;
			}
			return new ModDatabaseSearchResult
			{
				Name = @string,
				ModId = text2,
				AlternateIds = alternateIds,
				Summary = string2,
				Author = string3,
				Tags = stringList2,
				Downloads = @int,
				Follows = int2,
				TrendingPoints = int3,
				Comments = int4,
				AssetId = assetId,
				UrlAlias = string4,
				Side = string5,
				LogoUrl = string6,
				LogoUrlSource = logoUrlSource,
				LastReleasedUtc = dateTime,
				CreatedUtc = createdUtc,
				Score = score
			};
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x0003E7D8 File Offset: 0x0003C9D8
		private static bool TryCalculateSearchScore(string name, string primaryId, [Nullable(2)] string author, [Nullable(2)] string summary, IReadOnlyList<string> alternateIds, IReadOnlyList<string> tags, IReadOnlyList<string> tokens, int downloads, int follows, int trendingPoints, int comments, DateTime? lastReleased, out double score)
		{
			score = 0.0;
			if (tokens.Count == 0)
			{
				return false;
			}
			int num = 0;
			string text = summary ?? string.Empty;
			IReadOnlyList<string> readOnlyList;
			if (!string.IsNullOrWhiteSpace(author))
			{
				readOnlyList = ModDatabaseService.CreateSearchTokens(author);
			}
			else
			{
				IReadOnlyList<string> readOnlyList2 = Array.Empty<string>();
				readOnlyList = readOnlyList2;
			}
			IReadOnlyList<string> readOnlyList3 = readOnlyList;
			foreach (string text2 in tokens)
			{
				if (!string.IsNullOrWhiteSpace(text2))
				{
					string currentToken = text2.Trim();
					if (currentToken.Length != 0)
					{
						bool flag = false;
						if (string.Equals(name, currentToken, 5))
						{
							score += 12.0;
							flag = true;
						}
						else if (name.Contains(currentToken, 5))
						{
							score += 6.0;
							flag = true;
						}
						if (string.Equals(primaryId, currentToken, 5))
						{
							score += 10.0;
							flag = true;
						}
						else if (primaryId.Contains(currentToken, 5))
						{
							score += 5.0;
							flag = true;
						}
						else if (Enumerable.Any<string>(alternateIds, (string id) => string.Equals(id, currentToken, 5)))
						{
							score += 9.0;
							flag = true;
						}
						else if (Enumerable.Any<string>(alternateIds, (string id) => id.Contains(currentToken, 5)))
						{
							score += 4.0;
							flag = true;
						}
						if (readOnlyList3.Count > 0)
						{
							if (Enumerable.Any<string>(readOnlyList3, (string authorToken) => string.Equals(authorToken, currentToken, 5)))
							{
								score += 4.0;
								flag = true;
							}
							else if (Enumerable.Any<string>(readOnlyList3, (string authorToken) => authorToken.Contains(currentToken, 5)))
							{
								score += 2.5;
								flag = true;
							}
						}
						if (Enumerable.Any<string>(tags, (string tag) => string.Equals(tag, currentToken, 5)))
						{
							score += 3.0;
							flag = true;
						}
						else if (Enumerable.Any<string>(tags, (string tag) => tag.Contains(currentToken, 5)))
						{
							score += 2.0;
							flag = true;
						}
						if (!string.IsNullOrEmpty(text) && text.Contains(currentToken, 5))
						{
							score += 1.5;
							flag = true;
						}
						if (flag)
						{
							num++;
						}
					}
				}
			}
			if (num == 0)
			{
				score = 0.0;
				return false;
			}
			score += (double)num * 1.5;
			score += Math.Log10((double)(downloads + 1)) * 1.2;
			score += Math.Log10((double)(follows + 1)) * 1.5;
			score += Math.Log10((double)(trendingPoints + 1));
			score += Math.Log10((double)(comments + 1)) * 0.5;
			if (lastReleased != null)
			{
				double totalDays = (DateTime.UtcNow - lastReleased.Value).TotalDays;
				if (!double.IsNaN(totalDays))
				{
					score += Math.Max(0.0, 4.0 - totalDays / 45.0);
				}
			}
			return true;
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x0003EB4C File Offset: 0x0003CD4C
		private static IReadOnlyList<string> CreateSearchTokens(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return Array.Empty<string>();
			}
			string text = value.Trim();
			ModDatabaseService.<>c__DisplayClass58_0 CS$<>8__locals1;
			CS$<>8__locals1.tokens = new List<string>();
			CS$<>8__locals1.seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			ModDatabaseService.<CreateSearchTokens>g__AddToken|58_0(text, ref CS$<>8__locals1);
			string[] array = text.Split(new char[]
			{
				' ',
				'\t',
				'\r',
				'\n',
				'-',
				'_',
				'.',
				'/',
				'\\'
			}, 3);
			for (int i = 0; i < array.Length; i++)
			{
				ModDatabaseService.<CreateSearchTokens>g__AddToken|58_0(array[i], ref CS$<>8__locals1);
			}
			if (CS$<>8__locals1.tokens.Count != 0)
			{
				return CS$<>8__locals1.tokens;
			}
			return Array.Empty<string>();
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x0003EBE0 File Offset: 0x0003CDE0
		private static int? GetNullableInt(JsonElement element, string propertyName)
		{
			JsonElement jsonElement;
			if (!element.TryGetProperty(propertyName, ref jsonElement))
			{
				return default(int?);
			}
			JsonValueKind valueKind = jsonElement.ValueKind;
			long num4;
			if (valueKind != 3)
			{
				if (valueKind == 4)
				{
					long num;
					if (jsonElement.TryGetInt64(ref num))
					{
						return new int?((int)Math.Clamp(num, -2147483648L, 2147483647L));
					}
					double num2;
					if (jsonElement.TryGetDouble(ref num2))
					{
						if (double.IsNaN(num2) || double.IsInfinity(num2))
						{
							return default(int?);
						}
						double num3 = Math.Truncate(num2);
						if (num3 < -2147483648.0)
						{
							return new int?(int.MinValue);
						}
						if (num3 > 2147483647.0)
						{
							return new int?(int.MaxValue);
						}
						return new int?((int)num3);
					}
				}
			}
			else if (long.TryParse(jsonElement.GetString(), 7, CultureInfo.InvariantCulture, ref num4))
			{
				return new int?((int)Math.Clamp(num4, -2147483648L, 2147483647L));
			}
			return default(int?);
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x0003ECE4 File Offset: 0x0003CEE4
		private static int GetInt(JsonElement element, string propertyName)
		{
			return ModDatabaseService.GetNullableInt(element, propertyName).GetValueOrDefault();
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x0003ED00 File Offset: 0x0003CF00
		[NullableContext(2)]
		private static DateTime? TryParseDateTime(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return default(DateTime?);
			}
			DateTime dateTime;
			if (DateTime.TryParse(value, CultureInfo.InvariantCulture, 80, ref dateTime))
			{
				return new DateTime?(dateTime);
			}
			if (DateTime.TryParse(value, CultureInfo.InvariantCulture, 0, ref dateTime))
			{
				return new DateTime?(DateTime.SpecifyKind(dateTime, 1));
			}
			return default(DateTime?);
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x0003ED60 File Offset: 0x0003CF60
		private static IReadOnlyList<string> FindRequiredGameVersions(JsonElement modElement, [Nullable(2)] string modVersion)
		{
			if (string.IsNullOrWhiteSpace(modVersion))
			{
				return Array.Empty<string>();
			}
			JsonElement jsonElement;
			if (!modElement.TryGetProperty("releases", ref jsonElement) || jsonElement.ValueKind != 2)
			{
				return Array.Empty<string>();
			}
			string normalizedModVersion = VersionStringUtility.Normalize(modVersion);
			foreach (JsonElement element in jsonElement.EnumerateArray())
			{
				JsonElement jsonElement2;
				if (element.ValueKind == 1 && element.TryGetProperty("modversion", ref jsonElement2) && jsonElement2.ValueKind == 3)
				{
					string @string = jsonElement2.GetString();
					if (!string.IsNullOrWhiteSpace(@string) && ModDatabaseService.ReleaseMatchesModVersion(@string, modVersion, normalizedModVersion))
					{
						IReadOnlyList<string> stringList = ModDatabaseService.GetStringList(element, "tags");
						IReadOnlyList<string> result;
						if (stringList.Count != 0)
						{
							result = stringList;
						}
						else
						{
							IReadOnlyList<string> readOnlyList = Array.Empty<string>();
							result = readOnlyList;
						}
						return result;
					}
				}
			}
			return Array.Empty<string>();
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x0003EE58 File Offset: 0x0003D058
		[NullableContext(2)]
		private static bool ReleaseMatchesModVersion([Nullable(1)] string releaseModVersion, string modVersion, string normalizedModVersion)
		{
			if (modVersion != null && string.Equals(releaseModVersion, modVersion, 5))
			{
				return true;
			}
			string text = VersionStringUtility.Normalize(releaseModVersion);
			return text != null && normalizedModVersion != null && string.Equals(text, normalizedModVersion, 5);
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x0003EE8C File Offset: 0x0003D08C
		[NullableContext(2)]
		private static string ExtractReleaseVersion(JsonElement releaseElement)
		{
			JsonElement jsonElement;
			if (releaseElement.TryGetProperty("modversion", ref jsonElement) && jsonElement.ValueKind == 3)
			{
				string @string = jsonElement.GetString();
				if (!string.IsNullOrWhiteSpace(@string))
				{
					return @string;
				}
			}
			JsonElement jsonElement2;
			if (releaseElement.TryGetProperty("version", ref jsonElement2) && jsonElement2.ValueKind == 3)
			{
				string string2 = jsonElement2.GetString();
				if (!string.IsNullOrWhiteSpace(string2))
				{
					return string2;
				}
			}
			return null;
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x0003EFDC File Offset: 0x0003D1DC
		[CompilerGenerated]
		internal static void <ConvertChangelogToPlainText>g__AppendNodes|55_0(HtmlNodeCollection nodes, StringBuilder output, int listDepth)
		{
			foreach (HtmlNode node in nodes)
			{
				ModDatabaseService.<ConvertChangelogToPlainText>g__AppendNode|55_1(node, output, listDepth);
			}
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x0003F02C File Offset: 0x0003D22C
		[CompilerGenerated]
		internal static void <ConvertChangelogToPlainText>g__AppendNode|55_1(HtmlNode node, StringBuilder output, int listDepth)
		{
			HtmlNodeType nodeType = node.NodeType;
			if (nodeType != 1)
			{
				if (nodeType == 3)
				{
					ModDatabaseService.<ConvertChangelogToPlainText>g__AppendText|55_2(output, HtmlEntity.DeEntitize(node.InnerText));
					return;
				}
			}
			else
			{
				string text = node.Name.ToLowerInvariant();
				if (text != null)
				{
					switch (text.Length)
					{
					case 1:
						if (!(text == "p"))
						{
							goto IL_297;
						}
						ModDatabaseService.<ConvertChangelogToPlainText>g__AppendNodes|55_0(node.ChildNodes, output, listDepth);
						ModDatabaseService.<ConvertChangelogToPlainText>g__EnsureEndsWithNewlines|55_3(output, 2);
						return;
					case 2:
					{
						char c = text.get_Chars(1);
						if (c <= 'i')
						{
							switch (c)
							{
							case '1':
								if (!(text == "h1"))
								{
									goto IL_297;
								}
								break;
							case '2':
								if (!(text == "h2"))
								{
									goto IL_297;
								}
								break;
							case '3':
								if (!(text == "h3"))
								{
									goto IL_297;
								}
								break;
							case '4':
								if (!(text == "h4"))
								{
									goto IL_297;
								}
								break;
							case '5':
								if (!(text == "h5"))
								{
									goto IL_297;
								}
								break;
							case '6':
								if (!(text == "h6"))
								{
									goto IL_297;
								}
								break;
							default:
							{
								if (c != 'i')
								{
									goto IL_297;
								}
								if (!(text == "li"))
								{
									goto IL_297;
								}
								ModDatabaseService.<ConvertChangelogToPlainText>g__EnsureEndsWithNewlines|55_3(output, 1);
								string text2 = new string(' ', Math.Max(0, listDepth - 1) * 2);
								output.Append(text2);
								int num = Math.Clamp(listDepth - 1, 0, ModDatabaseService.BulletSymbols.Length - 1);
								string text3 = ModDatabaseService.BulletSymbols[num];
								output.Append(text3);
								ModDatabaseService.<ConvertChangelogToPlainText>g__AppendNodes|55_0(node.ChildNodes, output, listDepth + 1);
								return;
							}
							}
						}
						else if (c != 'l')
						{
							if (c != 'r')
							{
								goto IL_297;
							}
							if (!(text == "br"))
							{
								goto IL_297;
							}
							ModDatabaseService.<ConvertChangelogToPlainText>g__EnsureEndsWithNewlines|55_3(output, 1);
							return;
						}
						else
						{
							if (!(text == "ul") && !(text == "ol"))
							{
								goto IL_297;
							}
							ModDatabaseService.<ConvertChangelogToPlainText>g__EnsureEndsWithNewlines|55_3(output, 1);
							ModDatabaseService.<ConvertChangelogToPlainText>g__AppendNodes|55_0(node.ChildNodes, output, listDepth + 1);
							ModDatabaseService.<ConvertChangelogToPlainText>g__EnsureEndsWithNewlines|55_3(output, 1);
							return;
						}
						break;
					}
					case 3:
						if (!(text == "div"))
						{
							goto IL_297;
						}
						break;
					case 4:
					case 5:
					case 6:
						goto IL_297;
					case 7:
					{
						char c = text.get_Chars(0);
						if (c != 'a')
						{
							if (c != 's')
							{
								goto IL_297;
							}
							if (!(text == "section"))
							{
								goto IL_297;
							}
						}
						else if (!(text == "article"))
						{
							goto IL_297;
						}
						break;
					}
					default:
						goto IL_297;
					}
					ModDatabaseService.<ConvertChangelogToPlainText>g__AppendNodes|55_0(node.ChildNodes, output, listDepth);
					ModDatabaseService.<ConvertChangelogToPlainText>g__EnsureEndsWithNewlines|55_3(output, 2);
					return;
				}
				IL_297:
				ModDatabaseService.<ConvertChangelogToPlainText>g__AppendNodes|55_0(node.ChildNodes, output, listDepth);
			}
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x0003F2E0 File Offset: 0x0003D4E0
		[CompilerGenerated]
		internal static void <ConvertChangelogToPlainText>g__AppendText|55_2(StringBuilder output, string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return;
			}
			if (string.IsNullOrWhiteSpace(value))
			{
				if (output.Length > 0 && output.get_Chars(output.Length - 1) != ' ' && output.get_Chars(output.Length - 1) != '\n')
				{
					output.Append(' ');
				}
				return;
			}
			output.Append(value);
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x0003F340 File Offset: 0x0003D540
		[CompilerGenerated]
		internal static void <ConvertChangelogToPlainText>g__EnsureEndsWithNewlines|55_3(StringBuilder output, int required)
		{
			int num = 0;
			int num2 = output.Length - 1;
			while (num2 >= 0 && output.get_Chars(num2) == '\n')
			{
				num++;
				num2--;
			}
			int num3 = required - num;
			if (num3 <= 0)
			{
				return;
			}
			for (int i = 0; i < num3; i++)
			{
				output.Append('\n');
			}
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x0003F38F File Offset: 0x0003D58F
		[CompilerGenerated]
		internal static void <CreateSearchTokens>g__AddToken|58_0(string token, ref ModDatabaseService.<>c__DisplayClass58_0 A_1)
		{
			if (string.IsNullOrWhiteSpace(token))
			{
				return;
			}
			token = token.Trim();
			if (A_1.seen.Add(token))
			{
				A_1.tokens.Add(token);
			}
		}

		// Token: 0x040004AC RID: 1196
		private static readonly string ApiEndpointFormat = DevConfig.ModDatabaseApiEndpointFormat;

		// Token: 0x040004AD RID: 1197
		private static readonly string SearchEndpointFormat = DevConfig.ModDatabaseSearchEndpointFormat;

		// Token: 0x040004AE RID: 1198
		private static readonly string MostDownloadedEndpointFormat = DevConfig.ModDatabaseMostDownloadedEndpointFormat;

		// Token: 0x040004AF RID: 1199
		private static readonly string RecentlyCreatedEndpointFormat = DevConfig.ModDatabaseRecentlyCreatedEndpointFormat;

		// Token: 0x040004B0 RID: 1200
		private static readonly string RecentlyUpdatedEndpointFormat = DevConfig.ModDatabaseRecentlyUpdatedEndpointFormat;

		// Token: 0x040004B1 RID: 1201
		private static readonly string ModPageBaseUrl = DevConfig.ModDatabasePageBaseUrl;

		// Token: 0x040004B2 RID: 1202
		private static readonly string ModPageAliasBaseUrl = DevConfig.ModDatabasePageAliasBaseUrl;

		// Token: 0x040004B3 RID: 1203
		private static readonly int MaxConcurrentMetadataRequests = DevConfig.ModDatabaseMaxConcurrentMetadataRequests;

		// Token: 0x040004B4 RID: 1204
		private static readonly int MinimumTotalDownloadsForTrending = DevConfig.ModDatabaseMinimumTotalDownloadsForTrending;

		// Token: 0x040004B5 RID: 1205
		private static readonly int DefaultNewModsMonths = DevConfig.ModDatabaseDefaultNewModsMonths;

		// Token: 0x040004B6 RID: 1206
		private static readonly int MaxNewModsMonths = DevConfig.ModDatabaseMaxNewModsMonths;

		// Token: 0x040004B7 RID: 1207
		private static readonly HttpClient HttpClient = new HttpClient();

		// Token: 0x040004B8 RID: 1208
		private static readonly ModDatabaseCacheService CacheService = new ModDatabaseCacheService();

		// Token: 0x040004B9 RID: 1209
		private static readonly TimeSpan ModCacheSoftExpiry = TimeSpan.FromMinutes(5.0);

		// Token: 0x040004BA RID: 1210
		private static readonly TimeSpan ModCacheHardExpiry = TimeSpan.FromHours(2.0);

		// Token: 0x040004BB RID: 1211
		private static readonly string[] BulletSymbols = new string[]
		{
			"• ",
			"◦ ",
			"▪ ",
			"▫ "
		};
	}
}
