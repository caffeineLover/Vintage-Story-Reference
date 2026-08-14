using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using SimpleVsManager.Cloud;
using VintageStoryModManager.Models;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000080 RID: 128
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModVersionVoteService : IDisposable
	{
		// Token: 0x06000B51 RID: 2897 RVA: 0x0004654A File Offset: 0x0004474A
		public static string GetVoteCachePath()
		{
			return ModVersionVoteService.VoteCachePath;
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x00046551 File Offset: 0x00044751
		public ModVersionVoteService() : this(ModVersionVoteService.DefaultDbUrl, new FirebaseAnonymousAuthenticator())
		{
			this._ownsAuthenticator = true;
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x0004656C File Offset: 0x0004476C
		public ModVersionVoteService(string databaseUrl, FirebaseAnonymousAuthenticator authenticator)
		{
			if (string.IsNullOrWhiteSpace(databaseUrl))
			{
				throw new ArgumentException("A Firebase database URL must be provided.", "databaseUrl");
			}
			this._dbUrl = databaseUrl.TrimEnd('/');
			if (authenticator == null)
			{
				throw new ArgumentNullException("authenticator");
			}
			this._authenticator = authenticator;
			this._ownsAuthenticator = false;
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x00046608 File Offset: 0x00044808
		public Task<ModVersionVoteSummary> GetVoteSummaryAsync(string modId, string modVersion, string vintageStoryVersion, CancellationToken cancellationToken = default(CancellationToken))
		{
			ModVersionVoteService.<GetVoteSummaryAsync>d__19 <GetVoteSummaryAsync>d__;
			<GetVoteSummaryAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ModVersionVoteSummary>.Create();
			<GetVoteSummaryAsync>d__.<>4__this = this;
			<GetVoteSummaryAsync>d__.modId = modId;
			<GetVoteSummaryAsync>d__.modVersion = modVersion;
			<GetVoteSummaryAsync>d__.vintageStoryVersion = vintageStoryVersion;
			<GetVoteSummaryAsync>d__.cancellationToken = cancellationToken;
			<GetVoteSummaryAsync>d__.<>1__state = -1;
			<GetVoteSummaryAsync>d__.<>t__builder.Start<ModVersionVoteService.<GetVoteSummaryAsync>d__19>(ref <GetVoteSummaryAsync>d__);
			return <GetVoteSummaryAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x0004666C File Offset: 0x0004486C
		public Task<ModVersionVoteService.VoteSummaryResult> GetVoteSummaryIfChangedAsync(string modId, string modVersion, string vintageStoryVersion, [Nullable(2)] string knownEtag, CancellationToken cancellationToken = default(CancellationToken))
		{
			ModVersionVoteService.<GetVoteSummaryIfChangedAsync>d__20 <GetVoteSummaryIfChangedAsync>d__;
			<GetVoteSummaryIfChangedAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ModVersionVoteService.VoteSummaryResult>.Create();
			<GetVoteSummaryIfChangedAsync>d__.<>4__this = this;
			<GetVoteSummaryIfChangedAsync>d__.modId = modId;
			<GetVoteSummaryIfChangedAsync>d__.modVersion = modVersion;
			<GetVoteSummaryIfChangedAsync>d__.vintageStoryVersion = vintageStoryVersion;
			<GetVoteSummaryIfChangedAsync>d__.knownEtag = knownEtag;
			<GetVoteSummaryIfChangedAsync>d__.cancellationToken = cancellationToken;
			<GetVoteSummaryIfChangedAsync>d__.<>1__state = -1;
			<GetVoteSummaryIfChangedAsync>d__.<>t__builder.Start<ModVersionVoteService.<GetVoteSummaryIfChangedAsync>d__20>(ref <GetVoteSummaryIfChangedAsync>d__);
			return <GetVoteSummaryIfChangedAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x000466DC File Offset: 0x000448DC
		[return: TupleElementNames(new string[]
		{
			"Summary",
			"ETag"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0,
			1,
			2
		})]
		public Task<ValueTuple<ModVersionVoteSummary, string>> GetVoteSummaryWithEtagAsync(string modId, string modVersion, string vintageStoryVersion, CancellationToken cancellationToken = default(CancellationToken))
		{
			ModVersionVoteService.<GetVoteSummaryWithEtagAsync>d__21 <GetVoteSummaryWithEtagAsync>d__;
			<GetVoteSummaryWithEtagAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ValueTuple<ModVersionVoteSummary, string>>.Create();
			<GetVoteSummaryWithEtagAsync>d__.<>4__this = this;
			<GetVoteSummaryWithEtagAsync>d__.modId = modId;
			<GetVoteSummaryWithEtagAsync>d__.modVersion = modVersion;
			<GetVoteSummaryWithEtagAsync>d__.vintageStoryVersion = vintageStoryVersion;
			<GetVoteSummaryWithEtagAsync>d__.cancellationToken = cancellationToken;
			<GetVoteSummaryWithEtagAsync>d__.<>1__state = -1;
			<GetVoteSummaryWithEtagAsync>d__.<>t__builder.Start<ModVersionVoteService.<GetVoteSummaryWithEtagAsync>d__21>(ref <GetVoteSummaryWithEtagAsync>d__);
			return <GetVoteSummaryWithEtagAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x00046740 File Offset: 0x00044940
		[return: TupleElementNames(new string[]
		{
			"Summary",
			"ETag"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0,
			1,
			2
		})]
		public Task<ValueTuple<ModVersionVoteSummary, string>> SubmitVoteAsync(string modId, string modVersion, string vintageStoryVersion, ModVersionVoteOption option, [Nullable(2)] string comment, CancellationToken cancellationToken = default(CancellationToken))
		{
			ModVersionVoteService.<SubmitVoteAsync>d__22 <SubmitVoteAsync>d__;
			<SubmitVoteAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ValueTuple<ModVersionVoteSummary, string>>.Create();
			<SubmitVoteAsync>d__.<>4__this = this;
			<SubmitVoteAsync>d__.modId = modId;
			<SubmitVoteAsync>d__.modVersion = modVersion;
			<SubmitVoteAsync>d__.vintageStoryVersion = vintageStoryVersion;
			<SubmitVoteAsync>d__.option = option;
			<SubmitVoteAsync>d__.comment = comment;
			<SubmitVoteAsync>d__.cancellationToken = cancellationToken;
			<SubmitVoteAsync>d__.<>1__state = -1;
			<SubmitVoteAsync>d__.<>t__builder.Start<ModVersionVoteService.<SubmitVoteAsync>d__22>(ref <SubmitVoteAsync>d__);
			return <SubmitVoteAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x000467B8 File Offset: 0x000449B8
		[return: TupleElementNames(new string[]
		{
			"Summary",
			"ETag"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0,
			1,
			2
		})]
		public Task<ValueTuple<ModVersionVoteSummary, string>> RemoveVoteAsync(string modId, string modVersion, string vintageStoryVersion, CancellationToken cancellationToken = default(CancellationToken))
		{
			ModVersionVoteService.<RemoveVoteAsync>d__23 <RemoveVoteAsync>d__;
			<RemoveVoteAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ValueTuple<ModVersionVoteSummary, string>>.Create();
			<RemoveVoteAsync>d__.<>4__this = this;
			<RemoveVoteAsync>d__.modId = modId;
			<RemoveVoteAsync>d__.modVersion = modVersion;
			<RemoveVoteAsync>d__.vintageStoryVersion = vintageStoryVersion;
			<RemoveVoteAsync>d__.cancellationToken = cancellationToken;
			<RemoveVoteAsync>d__.<>1__state = -1;
			<RemoveVoteAsync>d__.<>t__builder.Start<ModVersionVoteService.<RemoveVoteAsync>d__23>(ref <RemoveVoteAsync>d__);
			return <RemoveVoteAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x0004681C File Offset: 0x00044A1C
		[return: TupleElementNames(new string[]
		{
			"Summary",
			"ETag"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0,
			1,
			2
		})]
		private Task<ValueTuple<ModVersionVoteSummary, string>> GetVoteSummaryWithEtagAsync(FirebaseAnonymousAuthenticator.FirebaseAuthSession? session, ModVersionVoteService.VoteCacheKey cacheKey, CancellationToken cancellationToken, bool bypassCache = false)
		{
			ModVersionVoteService.<GetVoteSummaryWithEtagAsync>d__24 <GetVoteSummaryWithEtagAsync>d__;
			<GetVoteSummaryWithEtagAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ValueTuple<ModVersionVoteSummary, string>>.Create();
			<GetVoteSummaryWithEtagAsync>d__.<>4__this = this;
			<GetVoteSummaryWithEtagAsync>d__.session = session;
			<GetVoteSummaryWithEtagAsync>d__.cacheKey = cacheKey;
			<GetVoteSummaryWithEtagAsync>d__.cancellationToken = cancellationToken;
			<GetVoteSummaryWithEtagAsync>d__.bypassCache = bypassCache;
			<GetVoteSummaryWithEtagAsync>d__.<>1__state = -1;
			<GetVoteSummaryWithEtagAsync>d__.<>t__builder.Start<ModVersionVoteService.<GetVoteSummaryWithEtagAsync>d__24>(ref <GetVoteSummaryWithEtagAsync>d__);
			return <GetVoteSummaryWithEtagAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x00046880 File Offset: 0x00044A80
		private Task<ModVersionVoteService.VoteSummaryResult> GetVoteSummaryInternalAsync(FirebaseAnonymousAuthenticator.FirebaseAuthSession? session, ModVersionVoteService.VoteCacheKey cacheKey, [Nullable(2)] string knownEtag, bool bypassCache, CancellationToken cancellationToken)
		{
			ModVersionVoteService.<GetVoteSummaryInternalAsync>d__25 <GetVoteSummaryInternalAsync>d__;
			<GetVoteSummaryInternalAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ModVersionVoteService.VoteSummaryResult>.Create();
			<GetVoteSummaryInternalAsync>d__.<>4__this = this;
			<GetVoteSummaryInternalAsync>d__.session = session;
			<GetVoteSummaryInternalAsync>d__.cacheKey = cacheKey;
			<GetVoteSummaryInternalAsync>d__.knownEtag = knownEtag;
			<GetVoteSummaryInternalAsync>d__.bypassCache = bypassCache;
			<GetVoteSummaryInternalAsync>d__.cancellationToken = cancellationToken;
			<GetVoteSummaryInternalAsync>d__.<>1__state = -1;
			<GetVoteSummaryInternalAsync>d__.<>t__builder.Start<ModVersionVoteService.<GetVoteSummaryInternalAsync>d__25>(ref <GetVoteSummaryInternalAsync>d__);
			return <GetVoteSummaryInternalAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x000468F0 File Offset: 0x00044AF0
		[return: TupleElementNames(new string[]
		{
			"IndexAvailable",
			"HasVotes"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0
		})]
		private Task<ValueTuple<bool, bool>> TryHasVotesAsync(FirebaseAnonymousAuthenticator.FirebaseAuthSession? session, string modKey, string versionKey, CancellationToken cancellationToken)
		{
			ModVersionVoteService.<TryHasVotesAsync>d__26 <TryHasVotesAsync>d__;
			<TryHasVotesAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ValueTuple<bool, bool>>.Create();
			<TryHasVotesAsync>d__.<>4__this = this;
			<TryHasVotesAsync>d__.session = session;
			<TryHasVotesAsync>d__.modKey = modKey;
			<TryHasVotesAsync>d__.versionKey = versionKey;
			<TryHasVotesAsync>d__.cancellationToken = cancellationToken;
			<TryHasVotesAsync>d__.<>1__state = -1;
			<TryHasVotesAsync>d__.<>t__builder.Start<ModVersionVoteService.<TryHasVotesAsync>d__26>(ref <TryHasVotesAsync>d__);
			return <TryHasVotesAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x00046954 File Offset: 0x00044B54
		private Task EnsureVoteIndexLoadedAsync(FirebaseAnonymousAuthenticator.FirebaseAuthSession? session, CancellationToken cancellationToken)
		{
			ModVersionVoteService.<EnsureVoteIndexLoadedAsync>d__27 <EnsureVoteIndexLoadedAsync>d__;
			<EnsureVoteIndexLoadedAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<EnsureVoteIndexLoadedAsync>d__.<>4__this = this;
			<EnsureVoteIndexLoadedAsync>d__.session = session;
			<EnsureVoteIndexLoadedAsync>d__.cancellationToken = cancellationToken;
			<EnsureVoteIndexLoadedAsync>d__.<>1__state = -1;
			<EnsureVoteIndexLoadedAsync>d__.<>t__builder.Start<ModVersionVoteService.<EnsureVoteIndexLoadedAsync>d__27>(ref <EnsureVoteIndexLoadedAsync>d__);
			return <EnsureVoteIndexLoadedAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x000469A8 File Offset: 0x00044BA8
		private Task InvalidateVoteIndexAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			ModVersionVoteService.<InvalidateVoteIndexAsync>d__28 <InvalidateVoteIndexAsync>d__;
			<InvalidateVoteIndexAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<InvalidateVoteIndexAsync>d__.<>4__this = this;
			<InvalidateVoteIndexAsync>d__.cancellationToken = cancellationToken;
			<InvalidateVoteIndexAsync>d__.<>1__state = -1;
			<InvalidateVoteIndexAsync>d__.<>t__builder.Start<ModVersionVoteService.<InvalidateVoteIndexAsync>d__28>(ref <InvalidateVoteIndexAsync>d__);
			return <InvalidateVoteIndexAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x000469F4 File Offset: 0x00044BF4
		private Task<Dictionary<string, HashSet<string>>> FetchVoteIndexAsync(FirebaseAnonymousAuthenticator.FirebaseAuthSession? session, CancellationToken cancellationToken)
		{
			ModVersionVoteService.<FetchVoteIndexAsync>d__29 <FetchVoteIndexAsync>d__;
			<FetchVoteIndexAsync>d__.<>t__builder = AsyncTaskMethodBuilder<Dictionary<string, HashSet<string>>>.Create();
			<FetchVoteIndexAsync>d__.<>4__this = this;
			<FetchVoteIndexAsync>d__.session = session;
			<FetchVoteIndexAsync>d__.cancellationToken = cancellationToken;
			<FetchVoteIndexAsync>d__.<>1__state = -1;
			<FetchVoteIndexAsync>d__.<>t__builder.Start<ModVersionVoteService.<FetchVoteIndexAsync>d__29>(ref <FetchVoteIndexAsync>d__);
			return <FetchVoteIndexAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x00046A48 File Offset: 0x00044C48
		private static ModVersionVoteService.VoteRecord? TryDeserializeRecord(JsonElement element)
		{
			ModVersionVoteService.VoteRecord? result;
			try
			{
				result = new ModVersionVoteService.VoteRecord?(JsonSerializer.Deserialize<ModVersionVoteService.VoteRecord>(element, ModVersionVoteService.SerializerOptions));
			}
			catch
			{
				result = default(ModVersionVoteService.VoteRecord?);
			}
			return result;
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x00046A88 File Offset: 0x00044C88
		private static ModVersionVoteSummary CreateEmptySummary(ModVersionVoteService.VoteCacheKey cacheKey)
		{
			return new ModVersionVoteSummary(cacheKey.ModId, cacheKey.ModVersion, cacheKey.VintageStoryVersion, ModVersionVoteCounts.Empty, ModVersionVoteComments.Empty, default(ModVersionVoteOption?), null);
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x00046AC4 File Offset: 0x00044CC4
		private string BuildVotesUrl(FirebaseAnonymousAuthenticator.FirebaseAuthSession? session, [Nullable(2)] string query, params string[] segments)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this._dbUrl);
			foreach (string text in segments)
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					stringBuilder.Append('/');
					stringBuilder.Append(text);
				}
			}
			stringBuilder.Append(".json");
			bool flag = false;
			if (!string.IsNullOrWhiteSpace(query))
			{
				stringBuilder.Append('?');
				stringBuilder.Append(query);
				flag = true;
			}
			if (session != null)
			{
				stringBuilder.Append(flag ? '&' : '?');
				stringBuilder.Append("auth=");
				stringBuilder.Append(Uri.EscapeDataString(session.Value.IdToken));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x00046B84 File Offset: 0x00044D84
		private static Task EnsureOkAsync(HttpResponseMessage response, string operation)
		{
			ModVersionVoteService.<EnsureOkAsync>d__33 <EnsureOkAsync>d__;
			<EnsureOkAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<EnsureOkAsync>d__.response = response;
			<EnsureOkAsync>d__.operation = operation;
			<EnsureOkAsync>d__.<>1__state = -1;
			<EnsureOkAsync>d__.<>t__builder.Start<ModVersionVoteService.<EnsureOkAsync>d__33>(ref <EnsureOkAsync>d__);
			return <EnsureOkAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x00046BD0 File Offset: 0x00044DD0
		private static void ValidateIdentifiers(string modId, string modVersion, string vintageStoryVersion)
		{
			if (string.IsNullOrWhiteSpace(modId))
			{
				throw new ArgumentException("Mod identifier is required.", "modId");
			}
			if (string.IsNullOrWhiteSpace(modVersion))
			{
				throw new ArgumentException("Mod version is required.", "modVersion");
			}
			if (string.IsNullOrWhiteSpace(vintageStoryVersion))
			{
				throw new ArgumentException("Vintage Story version is required.", "vintageStoryVersion");
			}
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x00046C28 File Offset: 0x00044E28
		private static string SanitizeKey(string value)
		{
			string text = value.Trim();
			if (text.Length == 0)
			{
				return "_";
			}
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(text)).Replace('+', '-').Replace('/', '_').TrimEnd('=');
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x00046C74 File Offset: 0x00044E74
		private static void AddComment(List<string> target, [Nullable(2)] string comment)
		{
			string text = ModVersionVoteService.NormalizeComment(comment);
			if (string.IsNullOrEmpty(text))
			{
				return;
			}
			target.Add(text);
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x00046C98 File Offset: 0x00044E98
		private static IReadOnlyList<string> ToReadOnlyList(List<string> source)
		{
			if (source.Count != 0)
			{
				return source.ToArray();
			}
			return Array.Empty<string>();
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x00046CB0 File Offset: 0x00044EB0
		[NullableContext(2)]
		private static string NormalizeComment(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}
			string text = value.Trim();
			if (text.Length != 0)
			{
				return text;
			}
			return null;
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x00046CD9 File Offset: 0x00044ED9
		[NullableContext(2)]
		private static string NormalizeVersion(string value)
		{
			if (!string.IsNullOrWhiteSpace(value))
			{
				return value.Trim();
			}
			return null;
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x00046CEB File Offset: 0x00044EEB
		private static string Truncate(string value, int maxLength)
		{
			if (string.IsNullOrEmpty(value) || value.Length <= maxLength)
			{
				return value;
			}
			return value.Substring(0, maxLength);
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x00046D08 File Offset: 0x00044F08
		[return: TupleElementNames(new string[]
		{
			"Found",
			"Entry"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0
		})]
		private Task<ValueTuple<bool, ModVersionVoteService.VoteCacheEntry>> TryGetCachedSummaryAsync(ModVersionVoteService.VoteCacheKey key, CancellationToken cancellationToken = default(CancellationToken))
		{
			ModVersionVoteService.<TryGetCachedSummaryAsync>d__48 <TryGetCachedSummaryAsync>d__;
			<TryGetCachedSummaryAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ValueTuple<bool, ModVersionVoteService.VoteCacheEntry>>.Create();
			<TryGetCachedSummaryAsync>d__.<>4__this = this;
			<TryGetCachedSummaryAsync>d__.key = key;
			<TryGetCachedSummaryAsync>d__.cancellationToken = cancellationToken;
			<TryGetCachedSummaryAsync>d__.<>1__state = -1;
			<TryGetCachedSummaryAsync>d__.<>t__builder.Start<ModVersionVoteService.<TryGetCachedSummaryAsync>d__48>(ref <TryGetCachedSummaryAsync>d__);
			return <TryGetCachedSummaryAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x00046D5C File Offset: 0x00044F5C
		private Task StoreCachedSummaryAsync(ModVersionVoteService.VoteCacheKey key, ModVersionVoteSummary summary, [Nullable(2)] string eTag, CancellationToken cancellationToken = default(CancellationToken), bool persist = true)
		{
			ModVersionVoteService.<StoreCachedSummaryAsync>d__49 <StoreCachedSummaryAsync>d__;
			<StoreCachedSummaryAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<StoreCachedSummaryAsync>d__.<>4__this = this;
			<StoreCachedSummaryAsync>d__.key = key;
			<StoreCachedSummaryAsync>d__.summary = summary;
			<StoreCachedSummaryAsync>d__.eTag = eTag;
			<StoreCachedSummaryAsync>d__.cancellationToken = cancellationToken;
			<StoreCachedSummaryAsync>d__.persist = persist;
			<StoreCachedSummaryAsync>d__.<>1__state = -1;
			<StoreCachedSummaryAsync>d__.<>t__builder.Start<ModVersionVoteService.<StoreCachedSummaryAsync>d__49>(ref <StoreCachedSummaryAsync>d__);
			return <StoreCachedSummaryAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x00046DCC File Offset: 0x00044FCC
		private Task EnsureVoteCacheLoadedAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			ModVersionVoteService.<EnsureVoteCacheLoadedAsync>d__50 <EnsureVoteCacheLoadedAsync>d__;
			<EnsureVoteCacheLoadedAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<EnsureVoteCacheLoadedAsync>d__.<>4__this = this;
			<EnsureVoteCacheLoadedAsync>d__.cancellationToken = cancellationToken;
			<EnsureVoteCacheLoadedAsync>d__.<>1__state = -1;
			<EnsureVoteCacheLoadedAsync>d__.<>t__builder.Start<ModVersionVoteService.<EnsureVoteCacheLoadedAsync>d__50>(ref <EnsureVoteCacheLoadedAsync>d__);
			return <EnsureVoteCacheLoadedAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x00046E18 File Offset: 0x00045018
		private void TryLoadVoteCacheFromDisk()
		{
			try
			{
				if (File.Exists(ModVersionVoteService.VoteCachePath))
				{
					string text = File.ReadAllText(ModVersionVoteService.VoteCachePath);
					if (!string.IsNullOrWhiteSpace(text))
					{
						Dictionary<string, ModVersionVoteService.VoteCacheFileEntry> dictionary = JsonSerializer.Deserialize<Dictionary<string, ModVersionVoteService.VoteCacheFileEntry>>(text, ModVersionVoteService.SerializerOptions);
						if (dictionary != null)
						{
							foreach (KeyValuePair<string, ModVersionVoteService.VoteCacheFileEntry> keyValuePair in dictionary)
							{
								ModVersionVoteService.VoteCacheKey? voteCacheKey = ModVersionVoteService.TryParseCacheKey(keyValuePair.Key);
								if (voteCacheKey != null)
								{
									ModVersionVoteService.VoteCacheFileEntry value = keyValuePair.Value;
									if (((value != null) ? value.Summary : null) != null && value.Summary.Counts.Total != 0)
									{
										this._voteCache[voteCacheKey.Value] = new ModVersionVoteService.VoteCacheEntry(value.Summary, value.ETag);
									}
								}
							}
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x00046F18 File Offset: 0x00045118
		private Task PersistVoteCacheAsync(CancellationToken cancellationToken = default(CancellationToken))
		{
			ModVersionVoteService.<PersistVoteCacheAsync>d__52 <PersistVoteCacheAsync>d__;
			<PersistVoteCacheAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<PersistVoteCacheAsync>d__.<>4__this = this;
			<PersistVoteCacheAsync>d__.cancellationToken = cancellationToken;
			<PersistVoteCacheAsync>d__.<>1__state = -1;
			<PersistVoteCacheAsync>d__.<>t__builder.Start<ModVersionVoteService.<PersistVoteCacheAsync>d__52>(ref <PersistVoteCacheAsync>d__);
			return <PersistVoteCacheAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x00046F63 File Offset: 0x00045163
		private static string BuildCacheKey(ModVersionVoteService.VoteCacheKey key)
		{
			return string.Join("||", new string[]
			{
				key.ModId,
				key.ModVersion,
				key.VintageStoryVersion
			});
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x00046F94 File Offset: 0x00045194
		private static ModVersionVoteService.VoteCacheKey? TryParseCacheKey(string key)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				return default(ModVersionVoteService.VoteCacheKey?);
			}
			string[] array = key.Split("||", 0);
			if (array.Length != 3)
			{
				return default(ModVersionVoteService.VoteCacheKey?);
			}
			return new ModVersionVoteService.VoteCacheKey?(new ModVersionVoteService.VoteCacheKey(array[0], array[1], array[2]));
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x00046FE4 File Offset: 0x000451E4
		public void Dispose()
		{
			object disposeLock = this._disposeLock;
			lock (disposeLock)
			{
				if (this._disposed)
				{
					return;
				}
				this._disposed = true;
			}
			this._cacheLock.Dispose();
			this._voteIndexLock.Dispose();
			if (this._ownsAuthenticator)
			{
				this._authenticator.Dispose();
			}
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x00047058 File Offset: 0x00045258
		// Note: this type is marked as 'beforefieldinit'.
		static ModVersionVoteService()
		{
			JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
			jsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
			jsonSerializerOptions.DefaultIgnoreCondition = 3;
			jsonSerializerOptions.Converters.Add(new ModVersionVoteService.ModVersionVoteOptionJsonConverter());
			ModVersionVoteService.SerializerOptions = jsonSerializerOptions;
		}

		// Token: 0x0400052A RID: 1322
		private static readonly string DefaultDbUrl = DevConfig.ModVersionVoteDefaultDbUrl;

		// Token: 0x0400052B RID: 1323
		private static readonly string VotesRootPath = DevConfig.ModVersionVoteRootPath;

		// Token: 0x0400052C RID: 1324
		private static readonly string VoteCachePath = Path.Combine(DevConfig.FirebaseBackupDirectory, "compat-votes-cache.json");

		// Token: 0x0400052D RID: 1325
		private static readonly HttpClient HttpClient = new HttpClient
		{
			Timeout = TimeSpan.FromSeconds(15.0)
		};

		// Token: 0x0400052E RID: 1326
		private static readonly JsonSerializerOptions SerializerOptions;

		// Token: 0x0400052F RID: 1327
		private readonly FirebaseAnonymousAuthenticator _authenticator;

		// Token: 0x04000530 RID: 1328
		private readonly string _dbUrl;

		// Token: 0x04000531 RID: 1329
		private readonly ConcurrentDictionary<ModVersionVoteService.VoteCacheKey, ModVersionVoteService.VoteCacheEntry> _voteCache = new ConcurrentDictionary<ModVersionVoteService.VoteCacheKey, ModVersionVoteService.VoteCacheEntry>(ModVersionVoteService.VoteCacheKeyComparer.Instance);

		// Token: 0x04000532 RID: 1330
		private readonly SemaphoreSlim _voteIndexLock = new SemaphoreSlim(1, 1);

		// Token: 0x04000533 RID: 1331
		private Dictionary<string, HashSet<string>> _voteIndex = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000534 RID: 1332
		private bool _voteIndexLoaded;

		// Token: 0x04000535 RID: 1333
		private readonly SemaphoreSlim _cacheLock = new SemaphoreSlim(1, 1);

		// Token: 0x04000536 RID: 1334
		private readonly object _disposeLock = new object();

		// Token: 0x04000537 RID: 1335
		private readonly bool _ownsAuthenticator;

		// Token: 0x04000538 RID: 1336
		private bool _voteCacheLoaded;

		// Token: 0x04000539 RID: 1337
		private bool _disposed;

		// Token: 0x020002E5 RID: 741
		[NullableContext(2)]
		[Nullable(0)]
		public readonly struct VoteSummaryResult
		{
			// Token: 0x06001875 RID: 6261 RVA: 0x0008DED2 File Offset: 0x0008C0D2
			public VoteSummaryResult(ModVersionVoteSummary summary, string eTag, bool isNotModified)
			{
				this.Summary = summary;
				this.ETag = eTag;
				this.IsNotModified = isNotModified;
			}

			// Token: 0x17000517 RID: 1303
			// (get) Token: 0x06001876 RID: 6262 RVA: 0x0008DEE9 File Offset: 0x0008C0E9
			public ModVersionVoteSummary Summary { get; }

			// Token: 0x17000518 RID: 1304
			// (get) Token: 0x06001877 RID: 6263 RVA: 0x0008DEF1 File Offset: 0x0008C0F1
			public string ETag { get; }

			// Token: 0x17000519 RID: 1305
			// (get) Token: 0x06001878 RID: 6264 RVA: 0x0008DEF9 File Offset: 0x0008C0F9
			public bool IsNotModified { get; }
		}

		// Token: 0x020002E6 RID: 742
		[Nullable(0)]
		private sealed class ModVersionVoteOptionJsonConverter : JsonConverter<ModVersionVoteOption>
		{
			// Token: 0x06001879 RID: 6265 RVA: 0x0008DF04 File Offset: 0x0008C104
			public override ModVersionVoteOption Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				if (reader.TokenType != 7)
				{
					throw new JsonException("Expected string value for mod version vote option.");
				}
				string @string = reader.GetString();
				if (string.IsNullOrWhiteSpace(@string))
				{
					throw new JsonException("Vote option value was empty.");
				}
				string text = @string;
				ModVersionVoteOption result;
				if (text.Equals("fullyFunctional", 5))
				{
					result = ModVersionVoteOption.FullyFunctional;
				}
				else if (text.Equals("workingPerfectly", 5))
				{
					result = ModVersionVoteOption.FullyFunctional;
				}
				else if (text.Equals("noIssuesSoFar", 5))
				{
					result = ModVersionVoteOption.NoIssuesSoFar;
				}
				else if (text.Equals("someIssuesButWorks", 5))
				{
					result = ModVersionVoteOption.SomeIssuesButWorks;
				}
				else if (text.Equals("notFunctional", 5))
				{
					result = ModVersionVoteOption.NotFunctional;
				}
				else if (text.Equals("notWorking", 5))
				{
					result = ModVersionVoteOption.NotFunctional;
				}
				else
				{
					if (!text.Equals("crashesOrFreezesGame", 5))
					{
						throw new JsonException("Unrecognized vote option '" + @string + "'.");
					}
					result = ModVersionVoteOption.CrashesOrFreezesGame;
				}
				return result;
			}

			// Token: 0x0600187A RID: 6266 RVA: 0x0008DFDC File Offset: 0x0008C1DC
			public override void Write(Utf8JsonWriter writer, ModVersionVoteOption value, JsonSerializerOptions options)
			{
				string text;
				switch (value)
				{
				case ModVersionVoteOption.FullyFunctional:
					text = "fullyFunctional";
					break;
				case ModVersionVoteOption.NoIssuesSoFar:
					text = "noIssuesSoFar";
					break;
				case ModVersionVoteOption.SomeIssuesButWorks:
					text = "someIssuesButWorks";
					break;
				case ModVersionVoteOption.NotFunctional:
					text = "notFunctional";
					break;
				case ModVersionVoteOption.CrashesOrFreezesGame:
					text = "crashesOrFreezesGame";
					break;
				default:
					text = (value.ToString() ?? string.Empty);
					break;
				}
				string text2 = text;
				writer.WriteStringValue(text2);
			}
		}

		// Token: 0x020002E7 RID: 743
		[NullableContext(2)]
		[Nullable(0)]
		private struct VoteRecord
		{
			// Token: 0x1700051A RID: 1306
			// (get) Token: 0x0600187C RID: 6268 RVA: 0x0008E055 File Offset: 0x0008C255
			// (set) Token: 0x0600187D RID: 6269 RVA: 0x0008E05D File Offset: 0x0008C25D
			[JsonPropertyName("option")]
			public ModVersionVoteOption Option { readonly get; set; }

			// Token: 0x1700051B RID: 1307
			// (get) Token: 0x0600187E RID: 6270 RVA: 0x0008E066 File Offset: 0x0008C266
			// (set) Token: 0x0600187F RID: 6271 RVA: 0x0008E06E File Offset: 0x0008C26E
			[JsonPropertyName("vintageStoryVersion")]
			public string VintageStoryVersion { readonly get; set; }

			// Token: 0x1700051C RID: 1308
			// (get) Token: 0x06001880 RID: 6272 RVA: 0x0008E077 File Offset: 0x0008C277
			// (set) Token: 0x06001881 RID: 6273 RVA: 0x0008E07F File Offset: 0x0008C27F
			[JsonPropertyName("updatedUtc")]
			public string UpdatedUtc { readonly get; set; }

			// Token: 0x1700051D RID: 1309
			// (get) Token: 0x06001882 RID: 6274 RVA: 0x0008E088 File Offset: 0x0008C288
			// (set) Token: 0x06001883 RID: 6275 RVA: 0x0008E090 File Offset: 0x0008C290
			[JsonPropertyName("comment")]
			public string Comment { readonly get; set; }
		}

		// Token: 0x020002E8 RID: 744
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class VoteCacheFileEntry : IEquatable<ModVersionVoteService.VoteCacheFileEntry>
		{
			// Token: 0x06001884 RID: 6276 RVA: 0x0008E099 File Offset: 0x0008C299
			public VoteCacheFileEntry(ModVersionVoteSummary Summary, string ETag)
			{
				this.Summary = Summary;
				this.ETag = ETag;
				base..ctor();
			}

			// Token: 0x1700051E RID: 1310
			// (get) Token: 0x06001885 RID: 6277 RVA: 0x0008E0AF File Offset: 0x0008C2AF
			[Nullable(1)]
			[CompilerGenerated]
			private Type EqualityContract
			{
				[NullableContext(1)]
				[CompilerGenerated]
				get
				{
					return typeof(ModVersionVoteService.VoteCacheFileEntry);
				}
			}

			// Token: 0x1700051F RID: 1311
			// (get) Token: 0x06001886 RID: 6278 RVA: 0x0008E0BB File Offset: 0x0008C2BB
			// (set) Token: 0x06001887 RID: 6279 RVA: 0x0008E0C3 File Offset: 0x0008C2C3
			public ModVersionVoteSummary Summary { get; set; }

			// Token: 0x17000520 RID: 1312
			// (get) Token: 0x06001888 RID: 6280 RVA: 0x0008E0CC File Offset: 0x0008C2CC
			// (set) Token: 0x06001889 RID: 6281 RVA: 0x0008E0D4 File Offset: 0x0008C2D4
			public string ETag { get; set; }

			// Token: 0x0600188A RID: 6282 RVA: 0x0008E0E0 File Offset: 0x0008C2E0
			[NullableContext(1)]
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("VoteCacheFileEntry");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x0600188B RID: 6283 RVA: 0x0008E12C File Offset: 0x0008C32C
			[NullableContext(1)]
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Summary = ");
				builder.Append(this.Summary);
				builder.Append(", ETag = ");
				builder.Append(this.ETag);
				return true;
			}

			// Token: 0x0600188C RID: 6284 RVA: 0x0008E166 File Offset: 0x0008C366
			[CompilerGenerated]
			public static bool operator !=(ModVersionVoteService.VoteCacheFileEntry left, ModVersionVoteService.VoteCacheFileEntry right)
			{
				return !(left == right);
			}

			// Token: 0x0600188D RID: 6285 RVA: 0x0008E172 File Offset: 0x0008C372
			[CompilerGenerated]
			public static bool operator ==(ModVersionVoteService.VoteCacheFileEntry left, ModVersionVoteService.VoteCacheFileEntry right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x0600188E RID: 6286 RVA: 0x0008E186 File Offset: 0x0008C386
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<ModVersionVoteSummary>.Default.GetHashCode(this.<Summary>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ETag>k__BackingField);
			}

			// Token: 0x0600188F RID: 6287 RVA: 0x0008E1C6 File Offset: 0x0008C3C6
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as ModVersionVoteService.VoteCacheFileEntry);
			}

			// Token: 0x06001890 RID: 6288 RVA: 0x0008E1D4 File Offset: 0x0008C3D4
			[CompilerGenerated]
			public bool Equals(ModVersionVoteService.VoteCacheFileEntry other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<ModVersionVoteSummary>.Default.Equals(this.<Summary>k__BackingField, other.<Summary>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<ETag>k__BackingField, other.<ETag>k__BackingField));
			}

			// Token: 0x06001892 RID: 6290 RVA: 0x0008E235 File Offset: 0x0008C435
			[CompilerGenerated]
			private VoteCacheFileEntry([Nullable(1)] ModVersionVoteService.VoteCacheFileEntry original)
			{
				this.Summary = original.<Summary>k__BackingField;
				this.ETag = original.<ETag>k__BackingField;
			}

			// Token: 0x06001893 RID: 6291 RVA: 0x0008E255 File Offset: 0x0008C455
			[CompilerGenerated]
			public void Deconstruct(out ModVersionVoteSummary Summary, out string ETag)
			{
				Summary = this.Summary;
				ETag = this.ETag;
			}
		}

		// Token: 0x020002E9 RID: 745
		[Nullable(0)]
		private readonly struct VoteCacheKey : IEquatable<ModVersionVoteService.VoteCacheKey>
		{
			// Token: 0x06001894 RID: 6292 RVA: 0x0008E267 File Offset: 0x0008C467
			public VoteCacheKey(string ModId, string ModVersion, string VintageStoryVersion)
			{
				this.ModId = ModId;
				this.ModVersion = ModVersion;
				this.VintageStoryVersion = VintageStoryVersion;
			}

			// Token: 0x17000521 RID: 1313
			// (get) Token: 0x06001895 RID: 6293 RVA: 0x0008E27E File Offset: 0x0008C47E
			// (set) Token: 0x06001896 RID: 6294 RVA: 0x0008E286 File Offset: 0x0008C486
			public string ModId { get; set; }

			// Token: 0x17000522 RID: 1314
			// (get) Token: 0x06001897 RID: 6295 RVA: 0x0008E28F File Offset: 0x0008C48F
			// (set) Token: 0x06001898 RID: 6296 RVA: 0x0008E297 File Offset: 0x0008C497
			public string ModVersion { get; set; }

			// Token: 0x17000523 RID: 1315
			// (get) Token: 0x06001899 RID: 6297 RVA: 0x0008E2A0 File Offset: 0x0008C4A0
			// (set) Token: 0x0600189A RID: 6298 RVA: 0x0008E2A8 File Offset: 0x0008C4A8
			public string VintageStoryVersion { get; set; }

			// Token: 0x0600189B RID: 6299 RVA: 0x0008E2B4 File Offset: 0x0008C4B4
			[NullableContext(0)]
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("VoteCacheKey");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x0600189C RID: 6300 RVA: 0x0008E300 File Offset: 0x0008C500
			[NullableContext(0)]
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				builder.Append("ModId = ");
				builder.Append(this.ModId);
				builder.Append(", ModVersion = ");
				builder.Append(this.ModVersion);
				builder.Append(", VintageStoryVersion = ");
				builder.Append(this.VintageStoryVersion);
				return true;
			}

			// Token: 0x0600189D RID: 6301 RVA: 0x0008E359 File Offset: 0x0008C559
			[CompilerGenerated]
			public static bool operator !=(ModVersionVoteService.VoteCacheKey left, ModVersionVoteService.VoteCacheKey right)
			{
				return !(left == right);
			}

			// Token: 0x0600189E RID: 6302 RVA: 0x0008E365 File Offset: 0x0008C565
			[CompilerGenerated]
			public static bool operator ==(ModVersionVoteService.VoteCacheKey left, ModVersionVoteService.VoteCacheKey right)
			{
				return left.Equals(right);
			}

			// Token: 0x0600189F RID: 6303 RVA: 0x0008E36F File Offset: 0x0008C56F
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (EqualityComparer<string>.Default.GetHashCode(this.<ModId>k__BackingField) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ModVersion>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<VintageStoryVersion>k__BackingField);
			}

			// Token: 0x060018A0 RID: 6304 RVA: 0x0008E3AF File Offset: 0x0008C5AF
			[NullableContext(0)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return obj is ModVersionVoteService.VoteCacheKey && this.Equals((ModVersionVoteService.VoteCacheKey)obj);
			}

			// Token: 0x060018A1 RID: 6305 RVA: 0x0008E3C8 File Offset: 0x0008C5C8
			[CompilerGenerated]
			public bool Equals(ModVersionVoteService.VoteCacheKey other)
			{
				return EqualityComparer<string>.Default.Equals(this.<ModId>k__BackingField, other.<ModId>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<ModVersion>k__BackingField, other.<ModVersion>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<VintageStoryVersion>k__BackingField, other.<VintageStoryVersion>k__BackingField);
			}

			// Token: 0x060018A2 RID: 6306 RVA: 0x0008E41D File Offset: 0x0008C61D
			[CompilerGenerated]
			public void Deconstruct(out string ModId, out string ModVersion, out string VintageStoryVersion)
			{
				ModId = this.ModId;
				ModVersion = this.ModVersion;
				VintageStoryVersion = this.VintageStoryVersion;
			}
		}

		// Token: 0x020002EA RID: 746
		[Nullable(0)]
		private readonly struct VoteCacheEntry : IEquatable<ModVersionVoteService.VoteCacheEntry>
		{
			// Token: 0x060018A3 RID: 6307 RVA: 0x0008E437 File Offset: 0x0008C637
			public VoteCacheEntry(ModVersionVoteSummary Summary, [Nullable(2)] string ETag)
			{
				this.Summary = Summary;
				this.ETag = ETag;
			}

			// Token: 0x17000524 RID: 1316
			// (get) Token: 0x060018A4 RID: 6308 RVA: 0x0008E447 File Offset: 0x0008C647
			// (set) Token: 0x060018A5 RID: 6309 RVA: 0x0008E44F File Offset: 0x0008C64F
			public ModVersionVoteSummary Summary { get; set; }

			// Token: 0x17000525 RID: 1317
			// (get) Token: 0x060018A6 RID: 6310 RVA: 0x0008E458 File Offset: 0x0008C658
			// (set) Token: 0x060018A7 RID: 6311 RVA: 0x0008E460 File Offset: 0x0008C660
			[Nullable(2)]
			public string ETag { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x060018A8 RID: 6312 RVA: 0x0008E469 File Offset: 0x0008C669
			[return: TupleElementNames(new string[]
			{
				"Summary",
				"ETag"
			})]
			[return: Nullable(new byte[]
			{
				0,
				1,
				2
			})]
			public ValueTuple<ModVersionVoteSummary, string> ToSummaryResult()
			{
				return new ValueTuple<ModVersionVoteSummary, string>(this.Summary, this.ETag);
			}

			// Token: 0x060018A9 RID: 6313 RVA: 0x0008E47C File Offset: 0x0008C67C
			[NullableContext(0)]
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("VoteCacheEntry");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x060018AA RID: 6314 RVA: 0x0008E4C8 File Offset: 0x0008C6C8
			[NullableContext(0)]
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				builder.Append("Summary = ");
				builder.Append(this.Summary);
				builder.Append(", ETag = ");
				builder.Append(this.ETag);
				return true;
			}

			// Token: 0x060018AB RID: 6315 RVA: 0x0008E4FD File Offset: 0x0008C6FD
			[CompilerGenerated]
			public static bool operator !=(ModVersionVoteService.VoteCacheEntry left, ModVersionVoteService.VoteCacheEntry right)
			{
				return !(left == right);
			}

			// Token: 0x060018AC RID: 6316 RVA: 0x0008E509 File Offset: 0x0008C709
			[CompilerGenerated]
			public static bool operator ==(ModVersionVoteService.VoteCacheEntry left, ModVersionVoteService.VoteCacheEntry right)
			{
				return left.Equals(right);
			}

			// Token: 0x060018AD RID: 6317 RVA: 0x0008E513 File Offset: 0x0008C713
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return EqualityComparer<ModVersionVoteSummary>.Default.GetHashCode(this.<Summary>k__BackingField) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ETag>k__BackingField);
			}

			// Token: 0x060018AE RID: 6318 RVA: 0x0008E53C File Offset: 0x0008C73C
			[NullableContext(0)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return obj is ModVersionVoteService.VoteCacheEntry && this.Equals((ModVersionVoteService.VoteCacheEntry)obj);
			}

			// Token: 0x060018AF RID: 6319 RVA: 0x0008E554 File Offset: 0x0008C754
			[CompilerGenerated]
			public bool Equals(ModVersionVoteService.VoteCacheEntry other)
			{
				return EqualityComparer<ModVersionVoteSummary>.Default.Equals(this.<Summary>k__BackingField, other.<Summary>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<ETag>k__BackingField, other.<ETag>k__BackingField);
			}

			// Token: 0x060018B0 RID: 6320 RVA: 0x0008E586 File Offset: 0x0008C786
			[CompilerGenerated]
			public void Deconstruct(out ModVersionVoteSummary Summary, [Nullable(2)] out string ETag)
			{
				Summary = this.Summary;
				ETag = this.ETag;
			}
		}

		// Token: 0x020002EB RID: 747
		[NullableContext(0)]
		private sealed class VoteCacheKeyComparer : IEqualityComparer<ModVersionVoteService.VoteCacheKey>
		{
			// Token: 0x060018B1 RID: 6321 RVA: 0x0008E598 File Offset: 0x0008C798
			public bool Equals(ModVersionVoteService.VoteCacheKey x, ModVersionVoteService.VoteCacheKey y)
			{
				return string.Equals(x.ModId, y.ModId, 5) && string.Equals(x.ModVersion, y.ModVersion, 5) && string.Equals(x.VintageStoryVersion, y.VintageStoryVersion, 5);
			}

			// Token: 0x060018B2 RID: 6322 RVA: 0x0008E5E8 File Offset: 0x0008C7E8
			public int GetHashCode(ModVersionVoteService.VoteCacheKey obj)
			{
				HashCode hashCode = default(HashCode);
				hashCode.Add<string>(obj.ModId, StringComparer.OrdinalIgnoreCase);
				hashCode.Add<string>(obj.ModVersion, StringComparer.OrdinalIgnoreCase);
				hashCode.Add<string>(obj.VintageStoryVersion, StringComparer.OrdinalIgnoreCase);
				return hashCode.ToHashCode();
			}

			// Token: 0x04001289 RID: 4745
			[Nullable(1)]
			public static readonly ModVersionVoteService.VoteCacheKeyComparer Instance = new ModVersionVoteService.VoteCacheKeyComparer();
		}
	}
}
