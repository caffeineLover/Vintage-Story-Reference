using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using VintageStoryModManager.Models;

namespace VintageStoryModManager.Services
{
	// Token: 0x0200006D RID: 109
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class ModDatabaseCacheService
	{
		// Token: 0x060009E8 RID: 2536 RVA: 0x0003C40C File Offset: 0x0003A60C
		internal static void ClearCacheDirectory()
		{
			string modDatabaseCacheDirectory = ModCacheLocator.GetModDatabaseCacheDirectory();
			if (string.IsNullOrWhiteSpace(modDatabaseCacheDirectory) || !Directory.Exists(modDatabaseCacheDirectory))
			{
				return;
			}
			try
			{
				Directory.Delete(modDatabaseCacheDirectory, true);
			}
			catch (Exception ex)
			{
				throw new IOException("Failed to delete the mod database cache at " + modDatabaseCacheDirectory + ".", ex);
			}
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x0003C464 File Offset: 0x0003A664
		internal void ClearInMemoryCache()
		{
			this._inMemoryCache.Clear();
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x0003C474 File Offset: 0x0003A674
		[NullableContext(2)]
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		public Task<ModDatabaseInfo> TryLoadAsync([Nullable(1)] string modId, string normalizedGameVersion, string installedModVersion, bool allowExpiredEntryRefresh, bool requireExactVersionMatch, CancellationToken cancellationToken)
		{
			ModDatabaseCacheService.<TryLoadAsync>d__10 <TryLoadAsync>d__;
			<TryLoadAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ModDatabaseInfo>.Create();
			<TryLoadAsync>d__.<>4__this = this;
			<TryLoadAsync>d__.modId = modId;
			<TryLoadAsync>d__.normalizedGameVersion = normalizedGameVersion;
			<TryLoadAsync>d__.installedModVersion = installedModVersion;
			<TryLoadAsync>d__.requireExactVersionMatch = requireExactVersionMatch;
			<TryLoadAsync>d__.cancellationToken = cancellationToken;
			<TryLoadAsync>d__.<>1__state = -1;
			<TryLoadAsync>d__.<>t__builder.Start<ModDatabaseCacheService.<TryLoadAsync>d__10>(ref <TryLoadAsync>d__);
			return <TryLoadAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x0003C4E4 File Offset: 0x0003A6E4
		[NullableContext(2)]
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		public Task<ModDatabaseInfo> TryLoadWithoutExpiryAsync([Nullable(1)] string modId, string normalizedGameVersion, string installedModVersion, bool requireExactVersionMatch, CancellationToken cancellationToken)
		{
			ModDatabaseCacheService.<TryLoadWithoutExpiryAsync>d__11 <TryLoadWithoutExpiryAsync>d__;
			<TryLoadWithoutExpiryAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ModDatabaseInfo>.Create();
			<TryLoadWithoutExpiryAsync>d__.<>4__this = this;
			<TryLoadWithoutExpiryAsync>d__.modId = modId;
			<TryLoadWithoutExpiryAsync>d__.normalizedGameVersion = normalizedGameVersion;
			<TryLoadWithoutExpiryAsync>d__.installedModVersion = installedModVersion;
			<TryLoadWithoutExpiryAsync>d__.requireExactVersionMatch = requireExactVersionMatch;
			<TryLoadWithoutExpiryAsync>d__.cancellationToken = cancellationToken;
			<TryLoadWithoutExpiryAsync>d__.<>1__state = -1;
			<TryLoadWithoutExpiryAsync>d__.<>t__builder.Start<ModDatabaseCacheService.<TryLoadWithoutExpiryAsync>d__11>(ref <TryLoadWithoutExpiryAsync>d__);
			return <TryLoadWithoutExpiryAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x0003C554 File Offset: 0x0003A754
		[NullableContext(2)]
		[return: TupleElementNames(new string[]
		{
			"Info",
			"LastModifiedApiValue",
			"CachedAt"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0,
			2,
			2
		})]
		public Task<ValueTuple<ModDatabaseInfo, string, DateTimeOffset?>> TryLoadWithLastModifiedAsync([Nullable(1)] string modId, string normalizedGameVersion, string installedModVersion, bool requireExactVersionMatch, CancellationToken cancellationToken)
		{
			ModDatabaseCacheService.<TryLoadWithLastModifiedAsync>d__12 <TryLoadWithLastModifiedAsync>d__;
			<TryLoadWithLastModifiedAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ValueTuple<ModDatabaseInfo, string, DateTimeOffset?>>.Create();
			<TryLoadWithLastModifiedAsync>d__.<>4__this = this;
			<TryLoadWithLastModifiedAsync>d__.modId = modId;
			<TryLoadWithLastModifiedAsync>d__.normalizedGameVersion = normalizedGameVersion;
			<TryLoadWithLastModifiedAsync>d__.installedModVersion = installedModVersion;
			<TryLoadWithLastModifiedAsync>d__.requireExactVersionMatch = requireExactVersionMatch;
			<TryLoadWithLastModifiedAsync>d__.cancellationToken = cancellationToken;
			<TryLoadWithLastModifiedAsync>d__.<>1__state = -1;
			<TryLoadWithLastModifiedAsync>d__.<>t__builder.Start<ModDatabaseCacheService.<TryLoadWithLastModifiedAsync>d__12>(ref <TryLoadWithLastModifiedAsync>d__);
			return <TryLoadWithLastModifiedAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x0003C5C4 File Offset: 0x0003A7C4
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		public Task<string> GetCachedLatestVersionAsync(string modId, [Nullable(2)] string normalizedGameVersion, CancellationToken cancellationToken)
		{
			ModDatabaseCacheService.<GetCachedLatestVersionAsync>d__13 <GetCachedLatestVersionAsync>d__;
			<GetCachedLatestVersionAsync>d__.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
			<GetCachedLatestVersionAsync>d__.<>4__this = this;
			<GetCachedLatestVersionAsync>d__.modId = modId;
			<GetCachedLatestVersionAsync>d__.normalizedGameVersion = normalizedGameVersion;
			<GetCachedLatestVersionAsync>d__.cancellationToken = cancellationToken;
			<GetCachedLatestVersionAsync>d__.<>1__state = -1;
			<GetCachedLatestVersionAsync>d__.<>t__builder.Start<ModDatabaseCacheService.<GetCachedLatestVersionAsync>d__13>(ref <GetCachedLatestVersionAsync>d__);
			return <GetCachedLatestVersionAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x0003C620 File Offset: 0x0003A820
		[return: TupleElementNames(new string[]
		{
			"LastModifiedApiValue",
			"CachedAt"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0,
			2
		})]
		public Task<ValueTuple<string, DateTimeOffset?>> GetCachedLastModifiedAsync(string modId, [Nullable(2)] string normalizedGameVersion, CancellationToken cancellationToken)
		{
			ModDatabaseCacheService.<GetCachedLastModifiedAsync>d__14 <GetCachedLastModifiedAsync>d__;
			<GetCachedLastModifiedAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ValueTuple<string, DateTimeOffset?>>.Create();
			<GetCachedLastModifiedAsync>d__.<>4__this = this;
			<GetCachedLastModifiedAsync>d__.modId = modId;
			<GetCachedLastModifiedAsync>d__.normalizedGameVersion = normalizedGameVersion;
			<GetCachedLastModifiedAsync>d__.cancellationToken = cancellationToken;
			<GetCachedLastModifiedAsync>d__.<>1__state = -1;
			<GetCachedLastModifiedAsync>d__.<>t__builder.Start<ModDatabaseCacheService.<GetCachedLastModifiedAsync>d__14>(ref <GetCachedLastModifiedAsync>d__);
			return <GetCachedLastModifiedAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x0003C67C File Offset: 0x0003A87C
		[return: TupleElementNames(new string[]
		{
			"LastModified",
			"ETag",
			"CachedAt"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0,
			2,
			2
		})]
		public Task<ValueTuple<string, string, DateTimeOffset?>> GetCachedHttpHeadersAsync(string modId, [Nullable(2)] string normalizedGameVersion, CancellationToken cancellationToken)
		{
			ModDatabaseCacheService.<GetCachedHttpHeadersAsync>d__15 <GetCachedHttpHeadersAsync>d__;
			<GetCachedHttpHeadersAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ValueTuple<string, string, DateTimeOffset?>>.Create();
			<GetCachedHttpHeadersAsync>d__.<>4__this = this;
			<GetCachedHttpHeadersAsync>d__.modId = modId;
			<GetCachedHttpHeadersAsync>d__.normalizedGameVersion = normalizedGameVersion;
			<GetCachedHttpHeadersAsync>d__.cancellationToken = cancellationToken;
			<GetCachedHttpHeadersAsync>d__.<>1__state = -1;
			<GetCachedHttpHeadersAsync>d__.<>t__builder.Start<ModDatabaseCacheService.<GetCachedHttpHeadersAsync>d__15>(ref <GetCachedHttpHeadersAsync>d__);
			return <GetCachedHttpHeadersAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x0003C6D8 File Offset: 0x0003A8D8
		public Task StoreAsync(string modId, [Nullable(2)] string normalizedGameVersion, ModDatabaseInfo info, [Nullable(2)] string installedModVersion, CancellationToken cancellationToken)
		{
			return this.StoreAsync(modId, normalizedGameVersion, info, installedModVersion, null, null, null, cancellationToken);
		}

		// Token: 0x060009F1 RID: 2545 RVA: 0x0003C6F8 File Offset: 0x0003A8F8
		[NullableContext(2)]
		[return: Nullable(1)]
		public Task StoreAsync([Nullable(1)] string modId, string normalizedGameVersion, [Nullable(1)] ModDatabaseInfo info, string installedModVersion, string lastModifiedHeader, string etag, CancellationToken cancellationToken)
		{
			ModDatabaseCacheService.<StoreAsync>d__17 <StoreAsync>d__;
			<StoreAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<StoreAsync>d__.<>4__this = this;
			<StoreAsync>d__.modId = modId;
			<StoreAsync>d__.normalizedGameVersion = normalizedGameVersion;
			<StoreAsync>d__.info = info;
			<StoreAsync>d__.installedModVersion = installedModVersion;
			<StoreAsync>d__.lastModifiedHeader = lastModifiedHeader;
			<StoreAsync>d__.etag = etag;
			<StoreAsync>d__.cancellationToken = cancellationToken;
			<StoreAsync>d__.<>1__state = -1;
			<StoreAsync>d__.<>t__builder.Start<ModDatabaseCacheService.<StoreAsync>d__17>(ref <StoreAsync>d__);
			return <StoreAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x0003C778 File Offset: 0x0003A978
		[NullableContext(2)]
		[return: Nullable(1)]
		public Task StoreAsync([Nullable(1)] string modId, string normalizedGameVersion, [Nullable(1)] ModDatabaseInfo info, string installedModVersion, string lastModifiedHeader, string etag, string lastModifiedApiValue, CancellationToken cancellationToken)
		{
			ModDatabaseCacheService.<StoreAsync>d__18 <StoreAsync>d__;
			<StoreAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<StoreAsync>d__.<>4__this = this;
			<StoreAsync>d__.modId = modId;
			<StoreAsync>d__.normalizedGameVersion = normalizedGameVersion;
			<StoreAsync>d__.info = info;
			<StoreAsync>d__.installedModVersion = installedModVersion;
			<StoreAsync>d__.lastModifiedHeader = lastModifiedHeader;
			<StoreAsync>d__.etag = etag;
			<StoreAsync>d__.lastModifiedApiValue = lastModifiedApiValue;
			<StoreAsync>d__.cancellationToken = cancellationToken;
			<StoreAsync>d__.<>1__state = -1;
			<StoreAsync>d__.<>t__builder.Start<ModDatabaseCacheService.<StoreAsync>d__18>(ref <StoreAsync>d__);
			return <StoreAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x0003C800 File Offset: 0x0003AA00
		private Task<Dictionary<string, string[]>> LoadExistingTagsByVersionAsync(string cachePath, CancellationToken cancellationToken)
		{
			ModDatabaseCacheService.<LoadExistingTagsByVersionAsync>d__19 <LoadExistingTagsByVersionAsync>d__;
			<LoadExistingTagsByVersionAsync>d__.<>t__builder = AsyncTaskMethodBuilder<Dictionary<string, string[]>>.Create();
			<LoadExistingTagsByVersionAsync>d__.cachePath = cachePath;
			<LoadExistingTagsByVersionAsync>d__.cancellationToken = cancellationToken;
			<LoadExistingTagsByVersionAsync>d__.<>1__state = -1;
			<LoadExistingTagsByVersionAsync>d__.<>t__builder.Start<ModDatabaseCacheService.<LoadExistingTagsByVersionAsync>d__19>(ref <LoadExistingTagsByVersionAsync>d__);
			return <LoadExistingTagsByVersionAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x0003C84B File Offset: 0x0003AA4B
		[NullableContext(2)]
		private static string NormalizeModVersion(string version)
		{
			if (string.IsNullOrWhiteSpace(version))
			{
				return null;
			}
			return VersionStringUtility.Normalize(version);
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x0003C860 File Offset: 0x0003AA60
		private static ModDatabaseCacheService.CachedModDatabaseInfo CreateCacheModel(string modId, [Nullable(2)] string normalizedGameVersion, ModDatabaseInfo info, IReadOnlyDictionary<string, string[]> tagsByModVersion, [Nullable(2)] string lastModifiedHeader = null, [Nullable(2)] string etag = null, [Nullable(2)] string lastModifiedApiValue = null)
		{
			IReadOnlyList<ModReleaseInfo> readOnlyList = info.Releases ?? Array.Empty<ModReleaseInfo>();
			List<ModDatabaseCacheService.CachedModRelease> list = new List<ModDatabaseCacheService.CachedModRelease>(readOnlyList.Count);
			foreach (ModReleaseInfo modReleaseInfo in readOnlyList)
			{
				Uri uri = (modReleaseInfo != null) ? modReleaseInfo.DownloadUri : null;
				if (uri != null)
				{
					List<ModDatabaseCacheService.CachedModRelease> list2 = list;
					ModDatabaseCacheService.CachedModRelease cachedModRelease = new ModDatabaseCacheService.CachedModRelease();
					cachedModRelease.Version = modReleaseInfo.Version;
					cachedModRelease.NormalizedVersion = modReleaseInfo.NormalizedVersion;
					cachedModRelease.DownloadUrl = uri.ToString();
					cachedModRelease.FileName = modReleaseInfo.FileName;
					IReadOnlyList<string> gameVersionTags = modReleaseInfo.GameVersionTags;
					cachedModRelease.GameVersionTags = (((gameVersionTags != null) ? Enumerable.ToArray<string>(gameVersionTags) : null) ?? Array.Empty<string>());
					cachedModRelease.Changelog = modReleaseInfo.Changelog;
					cachedModRelease.Downloads = modReleaseInfo.Downloads;
					cachedModRelease.CreatedUtc = modReleaseInfo.CreatedUtc;
					list2.Add(cachedModRelease);
				}
			}
			ModDatabaseCacheService.CachedModDatabaseInfo cachedModDatabaseInfo = new ModDatabaseCacheService.CachedModDatabaseInfo();
			cachedModDatabaseInfo.SchemaVersion = ModDatabaseCacheService.CacheSchemaVersion;
			cachedModDatabaseInfo.CachedAt = DateTimeOffset.Now;
			cachedModDatabaseInfo.ModId = modId;
			cachedModDatabaseInfo.GameVersion = (string.IsNullOrWhiteSpace(normalizedGameVersion) ? ModDatabaseCacheService.AnyGameVersionToken : normalizedGameVersion);
			cachedModDatabaseInfo.LastModifiedHeader = lastModifiedHeader;
			cachedModDatabaseInfo.ETag = etag;
			cachedModDatabaseInfo.LastModifiedApiValue = lastModifiedApiValue;
			IReadOnlyList<string> tags = info.Tags;
			cachedModDatabaseInfo.Tags = (((tags != null) ? Enumerable.ToArray<string>(tags) : null) ?? Array.Empty<string>());
			cachedModDatabaseInfo.TagsByModVersion = ((tagsByModVersion.Count == 0) ? new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase) : new Dictionary<string, string[]>(tagsByModVersion, StringComparer.OrdinalIgnoreCase));
			cachedModDatabaseInfo.AssetId = info.AssetId;
			cachedModDatabaseInfo.ModPageUrl = info.ModPageUrl;
			cachedModDatabaseInfo.Downloads = info.Downloads;
			cachedModDatabaseInfo.Comments = info.Comments;
			cachedModDatabaseInfo.Follows = info.Follows;
			cachedModDatabaseInfo.TrendingPoints = info.TrendingPoints;
			cachedModDatabaseInfo.LogoUrl = info.LogoUrl;
			cachedModDatabaseInfo.LogoUrlSource = info.LogoUrlSource;
			cachedModDatabaseInfo.DownloadsLastThirtyDays = info.DownloadsLastThirtyDays;
			cachedModDatabaseInfo.DownloadsLastTenDays = info.DownloadsLastTenDays;
			cachedModDatabaseInfo.LastReleasedUtc = info.LastReleasedUtc;
			cachedModDatabaseInfo.CreatedUtc = info.CreatedUtc;
			IReadOnlyList<string> requiredGameVersions = info.RequiredGameVersions;
			cachedModDatabaseInfo.RequiredGameVersions = (((requiredGameVersions != null) ? Enumerable.ToArray<string>(requiredGameVersions) : null) ?? Array.Empty<string>());
			cachedModDatabaseInfo.Releases = list.ToArray();
			cachedModDatabaseInfo.Side = info.Side;
			return cachedModDatabaseInfo;
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x0003CAB4 File Offset: 0x0003ACB4
		[NullableContext(2)]
		private static ModDatabaseInfo ConvertToDatabaseInfo([Nullable(1)] ModDatabaseCacheService.CachedModDatabaseInfo cached, string normalizedGameVersion, string installedModVersion, bool requireExactVersionMatch)
		{
			string normalizedInstalledVersion = ModDatabaseCacheService.NormalizeModVersion(installedModVersion);
			string cachedTagsVersion;
			IReadOnlyList<string> tagsForInstalledVersion = ModDatabaseCacheService.GetTagsForInstalledVersion(cached, normalizedInstalledVersion, out cachedTagsVersion);
			IReadOnlyList<ModReleaseInfo> readOnlyList = ModDatabaseCacheService.BuildReleases(cached.Releases, normalizedGameVersion, requireExactVersionMatch);
			ModReleaseInfo modReleaseInfo = (readOnlyList.Count > 0) ? readOnlyList[0] : null;
			ModReleaseInfo modReleaseInfo2 = TargetGameVersionReleaseSelector.SelectLatestCompatibleRelease(readOnlyList, normalizedGameVersion, requireExactVersionMatch);
			IReadOnlyList<string> requiredGameVersions = ModDatabaseCacheService.DetermineRequiredGameVersions(cached.Releases, installedModVersion);
			return new ModDatabaseInfo
			{
				Tags = tagsForInstalledVersion,
				CachedTagsVersion = cachedTagsVersion,
				AssetId = cached.AssetId,
				ModPageUrl = cached.ModPageUrl,
				LatestCompatibleVersion = ((modReleaseInfo2 != null) ? modReleaseInfo2.Version : null),
				LatestVersion = ((modReleaseInfo != null) ? modReleaseInfo.Version : null),
				RequiredGameVersions = requiredGameVersions,
				Downloads = cached.Downloads,
				Comments = cached.Comments,
				Follows = cached.Follows,
				TrendingPoints = cached.TrendingPoints,
				LogoUrl = cached.LogoUrl,
				LogoUrlSource = cached.LogoUrlSource,
				DownloadsLastThirtyDays = cached.DownloadsLastThirtyDays,
				DownloadsLastTenDays = cached.DownloadsLastTenDays,
				LastReleasedUtc = cached.LastReleasedUtc,
				CreatedUtc = cached.CreatedUtc,
				LatestRelease = modReleaseInfo,
				LatestCompatibleRelease = modReleaseInfo2,
				Releases = readOnlyList,
				Side = cached.Side
			};
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x0003CC04 File Offset: 0x0003AE04
		private static IReadOnlyList<string> GetTagsForInstalledVersion(ModDatabaseCacheService.CachedModDatabaseInfo cached, [Nullable(2)] string normalizedInstalledVersion, [Nullable(2)] out string cachedTagsVersion)
		{
			cachedTagsVersion = null;
			Dictionary<string, string[]> tagsByModVersion;
			if (!string.IsNullOrWhiteSpace(normalizedInstalledVersion))
			{
				tagsByModVersion = cached.TagsByModVersion;
				string[] array;
				if (tagsByModVersion != null && tagsByModVersion.Count > 0 && cached.TagsByModVersion.TryGetValue(normalizedInstalledVersion, ref array))
				{
					cachedTagsVersion = normalizedInstalledVersion;
					if (array == null || array.Length <= 0)
					{
						return Array.Empty<string>();
					}
					return array;
				}
			}
			string[] array2 = cached.Tags;
			if (array2 != null && array2.Length > 0)
			{
				return cached.Tags;
			}
			tagsByModVersion = cached.TagsByModVersion;
			if (tagsByModVersion != null && tagsByModVersion.Count > 0)
			{
				foreach (KeyValuePair<string, string[]> keyValuePair in cached.TagsByModVersion)
				{
					array2 = keyValuePair.Value;
					if (array2 != null && array2.Length > 0)
					{
						return keyValuePair.Value;
					}
				}
			}
			return Array.Empty<string>();
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0003CCE4 File Offset: 0x0003AEE4
		private static IReadOnlyList<ModReleaseInfo> BuildReleases([Nullable(new byte[]
		{
			2,
			1
		})] IReadOnlyList<ModDatabaseCacheService.CachedModRelease> cachedReleases, [Nullable(2)] string normalizedGameVersion, bool requireExactVersionMatch)
		{
			if (cachedReleases == null || cachedReleases.Count == 0)
			{
				return Array.Empty<ModReleaseInfo>();
			}
			List<ModReleaseInfo> list = new List<ModReleaseInfo>(cachedReleases.Count);
			foreach (ModDatabaseCacheService.CachedModRelease cachedModRelease in cachedReleases)
			{
				Uri downloadUri;
				if (!string.IsNullOrWhiteSpace(cachedModRelease.Version) && !string.IsNullOrWhiteSpace(cachedModRelease.DownloadUrl) && Uri.TryCreate(cachedModRelease.DownloadUrl, 1, ref downloadUri))
				{
					bool isCompatibleWithInstalledGame = TargetGameVersionReleaseSelector.TagsSupportTarget(cachedModRelease.GameVersionTags, normalizedGameVersion, requireExactVersionMatch);
					list.Add(new ModReleaseInfo
					{
						Version = cachedModRelease.Version,
						NormalizedVersion = cachedModRelease.NormalizedVersion,
						DownloadUri = downloadUri,
						FileName = cachedModRelease.FileName,
						GameVersionTags = (cachedModRelease.GameVersionTags ?? Array.Empty<string>()),
						IsCompatibleWithInstalledGame = isCompatibleWithInstalledGame,
						Changelog = cachedModRelease.Changelog,
						Downloads = cachedModRelease.Downloads,
						CreatedUtc = cachedModRelease.CreatedUtc
					});
				}
			}
			if (list.Count != 0)
			{
				return list;
			}
			return Array.Empty<ModReleaseInfo>();
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x0003CE18 File Offset: 0x0003B018
		private static IReadOnlyList<string> DetermineRequiredGameVersions([Nullable(new byte[]
		{
			2,
			1
		})] IReadOnlyList<ModDatabaseCacheService.CachedModRelease> cachedReleases, [Nullable(2)] string installedModVersion)
		{
			if (string.IsNullOrWhiteSpace(installedModVersion) || cachedReleases == null || cachedReleases.Count == 0)
			{
				return Array.Empty<string>();
			}
			string normalizedInstalledVersion = VersionStringUtility.Normalize(installedModVersion);
			foreach (ModDatabaseCacheService.CachedModRelease cachedModRelease in cachedReleases)
			{
				if (!string.IsNullOrWhiteSpace(cachedModRelease.Version) && ModDatabaseCacheService.ReleaseMatchesInstalledVersion(cachedModRelease.Version, cachedModRelease.NormalizedVersion, installedModVersion, normalizedInstalledVersion))
				{
					string[] array = cachedModRelease.GameVersionTags ?? Array.Empty<string>();
					return (array.Length == 0) ? Array.Empty<string>() : array;
				}
			}
			return Array.Empty<string>();
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x0003CEC4 File Offset: 0x0003B0C4
		private static bool ReleaseMatchesInstalledVersion(string releaseVersion, [Nullable(2)] string normalizedReleaseVersion, string installedVersion, [Nullable(2)] string normalizedInstalledVersion)
		{
			return string.Equals(releaseVersion, installedVersion, 5) || (!string.IsNullOrWhiteSpace(normalizedReleaseVersion) && !string.IsNullOrWhiteSpace(normalizedInstalledVersion) && string.Equals(normalizedReleaseVersion, normalizedInstalledVersion, 5));
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x0003CEEC File Offset: 0x0003B0EC
		private Task<SemaphoreSlim> AcquireLockAsync(string path, CancellationToken cancellationToken)
		{
			ModDatabaseCacheService.<AcquireLockAsync>d__27 <AcquireLockAsync>d__;
			<AcquireLockAsync>d__.<>t__builder = AsyncTaskMethodBuilder<SemaphoreSlim>.Create();
			<AcquireLockAsync>d__.<>4__this = this;
			<AcquireLockAsync>d__.path = path;
			<AcquireLockAsync>d__.cancellationToken = cancellationToken;
			<AcquireLockAsync>d__.<>1__state = -1;
			<AcquireLockAsync>d__.<>t__builder.Start<ModDatabaseCacheService.<AcquireLockAsync>d__27>(ref <AcquireLockAsync>d__);
			return <AcquireLockAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x0003CF3F File Offset: 0x0003B13F
		private static bool IsSupportedSchemaVersion(int schemaVersion)
		{
			return schemaVersion >= ModDatabaseCacheService.MinimumSupportedCacheSchemaVersion && schemaVersion <= ModDatabaseCacheService.CacheSchemaVersion;
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x0003CF58 File Offset: 0x0003B158
		[NullableContext(2)]
		private static string GetCacheFilePath([Nullable(1)] string modId, string normalizedGameVersion)
		{
			if (string.IsNullOrWhiteSpace(modId))
			{
				return null;
			}
			string modDatabaseCacheDirectory = ModCacheLocator.GetModDatabaseCacheDirectory();
			if (string.IsNullOrWhiteSpace(modDatabaseCacheDirectory))
			{
				return null;
			}
			string text = ModCacheLocator.SanitizeFileName(modId, "mod");
			string text2 = string.IsNullOrWhiteSpace(normalizedGameVersion) ? ModDatabaseCacheService.AnyGameVersionToken : ModCacheLocator.SanitizeFileName(normalizedGameVersion, "game");
			string text3 = text + "__" + text2 + ".json";
			return Path.Combine(modDatabaseCacheDirectory, text3);
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x0003CFC0 File Offset: 0x0003B1C0
		[NullableContext(2)]
		private static bool IsGameVersionMatch(string cachedGameVersion, string normalizedGameVersion)
		{
			string text = string.IsNullOrWhiteSpace(cachedGameVersion) ? ModDatabaseCacheService.AnyGameVersionToken : cachedGameVersion;
			string text2 = string.IsNullOrWhiteSpace(normalizedGameVersion) ? ModDatabaseCacheService.AnyGameVersionToken : normalizedGameVersion;
			return string.Equals(text, text2, 5);
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x0003CFF8 File Offset: 0x0003B1F8
		private static string BuildInMemoryCacheKey(string cachePath, [Nullable(2)] string installedModVersion, bool requireExactVersionMatch)
		{
			string text = string.IsNullOrWhiteSpace(installedModVersion) ? "_" : installedModVersion;
			string text2 = requireExactVersionMatch ? "e" : "p";
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(2, 3);
			defaultInterpolatedStringHandler.AppendFormatted(cachePath);
			defaultInterpolatedStringHandler.AppendLiteral("|");
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendLiteral("|");
			defaultInterpolatedStringHandler.AppendFormatted(text2);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x0003D066 File Offset: 0x0003B266
		private static bool IsInMemoryCacheEntryExpired(ModDatabaseCacheService.InMemoryCacheEntry entry)
		{
			return DateTimeOffset.Now - entry.CreatedAt > ModDatabaseCacheService.InMemoryCacheMaxAge;
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x0003D084 File Offset: 0x0003B284
		private void TryAddToInMemoryCache(string key, [Nullable(2)] ModDatabaseInfo result)
		{
			if (this._inMemoryCache.Count >= 500)
			{
				this.EvictOldestCacheEntries();
			}
			ModDatabaseCacheService.InMemoryCacheEntry inMemoryCacheEntry = new ModDatabaseCacheService.InMemoryCacheEntry
			{
				Result = result,
				CreatedAt = DateTimeOffset.Now
			};
			this._inMemoryCache.TryAdd(key, inMemoryCacheEntry);
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x0003D0D0 File Offset: 0x0003B2D0
		private void EvictOldestCacheEntries()
		{
			DateTimeOffset now = DateTimeOffset.Now;
			DateTimeOffset? dateTimeOffset = default(DateTimeOffset?);
			string text = null;
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, ModDatabaseCacheService.InMemoryCacheEntry> keyValuePair in this._inMemoryCache)
			{
				if (now - keyValuePair.Value.CreatedAt > ModDatabaseCacheService.InMemoryCacheMaxAge)
				{
					list.Add(keyValuePair.Key);
				}
				else if (dateTimeOffset == null || keyValuePair.Value.CreatedAt < dateTimeOffset)
				{
					dateTimeOffset = new DateTimeOffset?(keyValuePair.Value.CreatedAt);
					text = keyValuePair.Key;
				}
			}
			ModDatabaseCacheService.InMemoryCacheEntry inMemoryCacheEntry;
			foreach (string text2 in list)
			{
				this._inMemoryCache.TryRemove(text2, ref inMemoryCacheEntry);
			}
			if (this._inMemoryCache.Count >= 500 && text != null)
			{
				this._inMemoryCache.TryRemove(text, ref inMemoryCacheEntry);
			}
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x0003D224 File Offset: 0x0003B424
		private void InvalidateInMemoryCacheForMod(string cachePath)
		{
			string text = cachePath + "|";
			List<string> list = new List<string>();
			foreach (string text2 in this._inMemoryCache.Keys)
			{
				if (text2.StartsWith(text, 5))
				{
					list.Add(text2);
				}
			}
			foreach (string text3 in list)
			{
				ModDatabaseCacheService.InMemoryCacheEntry inMemoryCacheEntry;
				this._inMemoryCache.TryRemove(text3, ref inMemoryCacheEntry);
			}
		}

		// Token: 0x040004A4 RID: 1188
		private static readonly int CacheSchemaVersion = DevConfig.ModDatabaseCacheSchemaVersion;

		// Token: 0x040004A5 RID: 1189
		private static readonly int MinimumSupportedCacheSchemaVersion = DevConfig.ModDatabaseMinimumSupportedCacheSchemaVersion;

		// Token: 0x040004A6 RID: 1190
		private static readonly string AnyGameVersionToken = DevConfig.ModDatabaseAnyGameVersionToken;

		// Token: 0x040004A7 RID: 1191
		private const int MaxInMemoryCacheSize = 500;

		// Token: 0x040004A8 RID: 1192
		private static readonly TimeSpan InMemoryCacheMaxAge = TimeSpan.FromMinutes(5.0);

		// Token: 0x040004A9 RID: 1193
		private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = false,
			DefaultIgnoreCondition = 3
		};

		// Token: 0x040004AA RID: 1194
		private readonly ConcurrentDictionary<string, SemaphoreSlim> _fileLocks = new ConcurrentDictionary<string, SemaphoreSlim>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040004AB RID: 1195
		private readonly ConcurrentDictionary<string, ModDatabaseCacheService.InMemoryCacheEntry> _inMemoryCache = new ConcurrentDictionary<string, ModDatabaseCacheService.InMemoryCacheEntry>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0200029D RID: 669
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class CachedModDatabaseInfo
		{
			// Token: 0x170004C5 RID: 1221
			// (get) Token: 0x060016E7 RID: 5863 RVA: 0x00084672 File Offset: 0x00082872
			// (set) Token: 0x060016E8 RID: 5864 RVA: 0x0008467A File Offset: 0x0008287A
			public int SchemaVersion { get; set; }

			// Token: 0x170004C6 RID: 1222
			// (get) Token: 0x060016E9 RID: 5865 RVA: 0x00084683 File Offset: 0x00082883
			// (set) Token: 0x060016EA RID: 5866 RVA: 0x0008468B File Offset: 0x0008288B
			[Nullable(1)]
			public string ModId { [NullableContext(1)] get; [NullableContext(1)] set; } = string.Empty;

			// Token: 0x170004C7 RID: 1223
			// (get) Token: 0x060016EB RID: 5867 RVA: 0x00084694 File Offset: 0x00082894
			// (set) Token: 0x060016EC RID: 5868 RVA: 0x0008469C File Offset: 0x0008289C
			[Nullable(1)]
			public string GameVersion { [NullableContext(1)] get; [NullableContext(1)] set; } = ModDatabaseCacheService.AnyGameVersionToken;

			// Token: 0x170004C8 RID: 1224
			// (get) Token: 0x060016ED RID: 5869 RVA: 0x000846A5 File Offset: 0x000828A5
			// (set) Token: 0x060016EE RID: 5870 RVA: 0x000846AD File Offset: 0x000828AD
			public DateTimeOffset CachedAt { get; set; }

			// Token: 0x170004C9 RID: 1225
			// (get) Token: 0x060016EF RID: 5871 RVA: 0x000846B6 File Offset: 0x000828B6
			// (set) Token: 0x060016F0 RID: 5872 RVA: 0x000846BE File Offset: 0x000828BE
			public string LastModifiedHeader { get; set; }

			// Token: 0x170004CA RID: 1226
			// (get) Token: 0x060016F1 RID: 5873 RVA: 0x000846C7 File Offset: 0x000828C7
			// (set) Token: 0x060016F2 RID: 5874 RVA: 0x000846CF File Offset: 0x000828CF
			public string ETag { get; set; }

			// Token: 0x170004CB RID: 1227
			// (get) Token: 0x060016F3 RID: 5875 RVA: 0x000846D8 File Offset: 0x000828D8
			// (set) Token: 0x060016F4 RID: 5876 RVA: 0x000846E0 File Offset: 0x000828E0
			public string LastModifiedApiValue { get; set; }

			// Token: 0x170004CC RID: 1228
			// (get) Token: 0x060016F5 RID: 5877 RVA: 0x000846E9 File Offset: 0x000828E9
			// (set) Token: 0x060016F6 RID: 5878 RVA: 0x000846F1 File Offset: 0x000828F1
			[Nullable(1)]
			public string[] Tags { [NullableContext(1)] get; [NullableContext(1)] set; } = Array.Empty<string>();

			// Token: 0x170004CD RID: 1229
			// (get) Token: 0x060016F7 RID: 5879 RVA: 0x000846FA File Offset: 0x000828FA
			// (set) Token: 0x060016F8 RID: 5880 RVA: 0x00084702 File Offset: 0x00082902
			[Nullable(1)]
			public Dictionary<string, string[]> TagsByModVersion { [NullableContext(1)] get; [NullableContext(1)] set; } = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase);

			// Token: 0x170004CE RID: 1230
			// (get) Token: 0x060016F9 RID: 5881 RVA: 0x0008470B File Offset: 0x0008290B
			// (set) Token: 0x060016FA RID: 5882 RVA: 0x00084713 File Offset: 0x00082913
			public string AssetId { get; set; }

			// Token: 0x170004CF RID: 1231
			// (get) Token: 0x060016FB RID: 5883 RVA: 0x0008471C File Offset: 0x0008291C
			// (set) Token: 0x060016FC RID: 5884 RVA: 0x00084724 File Offset: 0x00082924
			public string ModPageUrl { get; set; }

			// Token: 0x170004D0 RID: 1232
			// (get) Token: 0x060016FD RID: 5885 RVA: 0x0008472D File Offset: 0x0008292D
			// (set) Token: 0x060016FE RID: 5886 RVA: 0x00084735 File Offset: 0x00082935
			public int? Downloads { get; set; }

			// Token: 0x170004D1 RID: 1233
			// (get) Token: 0x060016FF RID: 5887 RVA: 0x0008473E File Offset: 0x0008293E
			// (set) Token: 0x06001700 RID: 5888 RVA: 0x00084746 File Offset: 0x00082946
			public int? Comments { get; set; }

			// Token: 0x170004D2 RID: 1234
			// (get) Token: 0x06001701 RID: 5889 RVA: 0x0008474F File Offset: 0x0008294F
			// (set) Token: 0x06001702 RID: 5890 RVA: 0x00084757 File Offset: 0x00082957
			public int? Follows { get; set; }

			// Token: 0x170004D3 RID: 1235
			// (get) Token: 0x06001703 RID: 5891 RVA: 0x00084760 File Offset: 0x00082960
			// (set) Token: 0x06001704 RID: 5892 RVA: 0x00084768 File Offset: 0x00082968
			public int? TrendingPoints { get; set; }

			// Token: 0x170004D4 RID: 1236
			// (get) Token: 0x06001705 RID: 5893 RVA: 0x00084771 File Offset: 0x00082971
			// (set) Token: 0x06001706 RID: 5894 RVA: 0x00084779 File Offset: 0x00082979
			public string LogoUrl { get; set; }

			// Token: 0x170004D5 RID: 1237
			// (get) Token: 0x06001707 RID: 5895 RVA: 0x00084782 File Offset: 0x00082982
			// (set) Token: 0x06001708 RID: 5896 RVA: 0x0008478A File Offset: 0x0008298A
			public string LogoUrlSource { get; set; }

			// Token: 0x170004D6 RID: 1238
			// (get) Token: 0x06001709 RID: 5897 RVA: 0x00084793 File Offset: 0x00082993
			// (set) Token: 0x0600170A RID: 5898 RVA: 0x0008479B File Offset: 0x0008299B
			public int? DownloadsLastThirtyDays { get; set; }

			// Token: 0x170004D7 RID: 1239
			// (get) Token: 0x0600170B RID: 5899 RVA: 0x000847A4 File Offset: 0x000829A4
			// (set) Token: 0x0600170C RID: 5900 RVA: 0x000847AC File Offset: 0x000829AC
			public int? DownloadsLastTenDays { get; set; }

			// Token: 0x170004D8 RID: 1240
			// (get) Token: 0x0600170D RID: 5901 RVA: 0x000847B5 File Offset: 0x000829B5
			// (set) Token: 0x0600170E RID: 5902 RVA: 0x000847BD File Offset: 0x000829BD
			public DateTime? LastReleasedUtc { get; set; }

			// Token: 0x170004D9 RID: 1241
			// (get) Token: 0x0600170F RID: 5903 RVA: 0x000847C6 File Offset: 0x000829C6
			// (set) Token: 0x06001710 RID: 5904 RVA: 0x000847CE File Offset: 0x000829CE
			public DateTime? CreatedUtc { get; set; }

			// Token: 0x170004DA RID: 1242
			// (get) Token: 0x06001711 RID: 5905 RVA: 0x000847D7 File Offset: 0x000829D7
			// (set) Token: 0x06001712 RID: 5906 RVA: 0x000847DF File Offset: 0x000829DF
			[Nullable(1)]
			public string[] RequiredGameVersions { [NullableContext(1)] get; [NullableContext(1)] set; } = Array.Empty<string>();

			// Token: 0x170004DB RID: 1243
			// (get) Token: 0x06001713 RID: 5907 RVA: 0x000847E8 File Offset: 0x000829E8
			// (set) Token: 0x06001714 RID: 5908 RVA: 0x000847F0 File Offset: 0x000829F0
			[Nullable(1)]
			public ModDatabaseCacheService.CachedModRelease[] Releases { [NullableContext(1)] get; [NullableContext(1)] set; } = Array.Empty<ModDatabaseCacheService.CachedModRelease>();

			// Token: 0x170004DC RID: 1244
			// (get) Token: 0x06001715 RID: 5909 RVA: 0x000847F9 File Offset: 0x000829F9
			// (set) Token: 0x06001716 RID: 5910 RVA: 0x00084801 File Offset: 0x00082A01
			public string Side { get; set; }
		}

		// Token: 0x0200029E RID: 670
		[Nullable(0)]
		private sealed class CachedModRelease
		{
			// Token: 0x170004DD RID: 1245
			// (get) Token: 0x06001718 RID: 5912 RVA: 0x00084866 File Offset: 0x00082A66
			// (set) Token: 0x06001719 RID: 5913 RVA: 0x0008486E File Offset: 0x00082A6E
			public string Version { get; set; } = string.Empty;

			// Token: 0x170004DE RID: 1246
			// (get) Token: 0x0600171A RID: 5914 RVA: 0x00084877 File Offset: 0x00082A77
			// (set) Token: 0x0600171B RID: 5915 RVA: 0x0008487F File Offset: 0x00082A7F
			[Nullable(2)]
			public string NormalizedVersion { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x170004DF RID: 1247
			// (get) Token: 0x0600171C RID: 5916 RVA: 0x00084888 File Offset: 0x00082A88
			// (set) Token: 0x0600171D RID: 5917 RVA: 0x00084890 File Offset: 0x00082A90
			public string DownloadUrl { get; set; } = string.Empty;

			// Token: 0x170004E0 RID: 1248
			// (get) Token: 0x0600171E RID: 5918 RVA: 0x00084899 File Offset: 0x00082A99
			// (set) Token: 0x0600171F RID: 5919 RVA: 0x000848A1 File Offset: 0x00082AA1
			[Nullable(2)]
			public string FileName { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x170004E1 RID: 1249
			// (get) Token: 0x06001720 RID: 5920 RVA: 0x000848AA File Offset: 0x00082AAA
			// (set) Token: 0x06001721 RID: 5921 RVA: 0x000848B2 File Offset: 0x00082AB2
			public string[] GameVersionTags { get; set; } = Array.Empty<string>();

			// Token: 0x170004E2 RID: 1250
			// (get) Token: 0x06001722 RID: 5922 RVA: 0x000848BB File Offset: 0x00082ABB
			// (set) Token: 0x06001723 RID: 5923 RVA: 0x000848C3 File Offset: 0x00082AC3
			[Nullable(2)]
			public string Changelog { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x170004E3 RID: 1251
			// (get) Token: 0x06001724 RID: 5924 RVA: 0x000848CC File Offset: 0x00082ACC
			// (set) Token: 0x06001725 RID: 5925 RVA: 0x000848D4 File Offset: 0x00082AD4
			public int? Downloads { get; set; }

			// Token: 0x170004E4 RID: 1252
			// (get) Token: 0x06001726 RID: 5926 RVA: 0x000848DD File Offset: 0x00082ADD
			// (set) Token: 0x06001727 RID: 5927 RVA: 0x000848E5 File Offset: 0x00082AE5
			public DateTime? CreatedUtc { get; set; }
		}

		// Token: 0x0200029F RID: 671
		[NullableContext(2)]
		[Nullable(0)]
		[RequiredMember]
		private sealed class InMemoryCacheEntry
		{
			// Token: 0x170004E5 RID: 1253
			// (get) Token: 0x06001729 RID: 5929 RVA: 0x00084917 File Offset: 0x00082B17
			// (set) Token: 0x0600172A RID: 5930 RVA: 0x0008491F File Offset: 0x00082B1F
			[RequiredMember]
			public ModDatabaseInfo Result { get; set; }

			// Token: 0x170004E6 RID: 1254
			// (get) Token: 0x0600172B RID: 5931 RVA: 0x00084928 File Offset: 0x00082B28
			// (set) Token: 0x0600172C RID: 5932 RVA: 0x00084930 File Offset: 0x00082B30
			[RequiredMember]
			public DateTimeOffset CreatedAt { get; set; }

			// Token: 0x0600172D RID: 5933 RVA: 0x00084939 File Offset: 0x00082B39
			[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
			[CompilerFeatureRequired("RequiredMembers")]
			public InMemoryCacheEntry()
			{
			}
		}
	}
}
