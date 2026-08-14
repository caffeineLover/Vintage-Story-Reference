using System;
using System.Buffers;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using VintageStoryModManager;
using VintageStoryModManager.Models;
using VintageStoryModManager.Services;

namespace SimpleVsManager.Cloud
{
	// Token: 0x02000010 RID: 16
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class FirebaseModlistStore : IDisposable
	{
		// Token: 0x0600008A RID: 138 RVA: 0x00004068 File Offset: 0x00002268
		public FirebaseModlistStore() : this(FirebaseModlistStore.DefaultDbUrl, new FirebaseAnonymousAuthenticator())
		{
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000407C File Offset: 0x0000227C
		public FirebaseModlistStore(string dbUrl, FirebaseAnonymousAuthenticator authenticator)
		{
			if (dbUrl == null)
			{
				throw new ArgumentNullException("dbUrl");
			}
			this._dbUrl = dbUrl.TrimEnd('/');
			if (authenticator == null)
			{
				throw new ArgumentNullException("authenticator");
			}
			this.Authenticator = authenticator;
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600008C RID: 140 RVA: 0x000040F7 File Offset: 0x000022F7
		internal FirebaseAnonymousAuthenticator Authenticator { get; }

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600008D RID: 141 RVA: 0x000040FF File Offset: 0x000022FF
		public static IReadOnlyList<string> SlotKeys
		{
			get
			{
				return FirebaseModlistStore.KnownSlots;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00004106 File Offset: 0x00002306
		[Nullable(2)]
		public string CurrentUserId
		{
			[NullableContext(2)]
			get
			{
				if (!string.IsNullOrWhiteSpace(this._playerUid))
				{
					return this._playerUid;
				}
				return null;
			}
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004120 File Offset: 0x00002320
		private static string BuildSlotNodeJson(string contentJson, [Nullable(2)] string registryId, string dateAdded)
		{
			string result;
			using (JsonDocument jsonDocument = JsonDocument.Parse(contentJson, default(JsonDocumentOptions)))
			{
				result = JsonSerializer.Serialize<FirebaseModlistStore.SlotNode>(new FirebaseModlistStore.SlotNode
				{
					RegistryId = registryId,
					Content = jsonDocument.RootElement.Clone(),
					DateAdded = dateAdded
				}, FirebaseModlistStore.JsonOpts);
			}
			return result;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00004198 File Offset: 0x00002398
		private Task<FirebaseModlistStore.SlotNode?> TryReadSlotNodeAsync(string uid, string slotKey, CancellationToken ct)
		{
			FirebaseModlistStore.<TryReadSlotNodeAsync>d__24 <TryReadSlotNodeAsync>d__;
			<TryReadSlotNodeAsync>d__.<>t__builder = AsyncTaskMethodBuilder<FirebaseModlistStore.SlotNode?>.Create();
			<TryReadSlotNodeAsync>d__.<>4__this = this;
			<TryReadSlotNodeAsync>d__.uid = uid;
			<TryReadSlotNodeAsync>d__.slotKey = slotKey;
			<TryReadSlotNodeAsync>d__.ct = ct;
			<TryReadSlotNodeAsync>d__.<>1__state = -1;
			<TryReadSlotNodeAsync>d__.<>t__builder.Start<FirebaseModlistStore.<TryReadSlotNodeAsync>d__24>(ref <TryReadSlotNodeAsync>d__);
			return <TryReadSlotNodeAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000041F4 File Offset: 0x000023F4
		private Task RegisterPlayerIdentityAsync(FirebaseModlistStore.PlayerIdentity identity, string firebaseUid, CancellationToken ct)
		{
			FirebaseModlistStore.<RegisterPlayerIdentityAsync>d__25 <RegisterPlayerIdentityAsync>d__;
			<RegisterPlayerIdentityAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<RegisterPlayerIdentityAsync>d__.<>4__this = this;
			<RegisterPlayerIdentityAsync>d__.identity = identity;
			<RegisterPlayerIdentityAsync>d__.ct = ct;
			<RegisterPlayerIdentityAsync>d__.<>1__state = -1;
			<RegisterPlayerIdentityAsync>d__.<>t__builder.Start<FirebaseModlistStore.<RegisterPlayerIdentityAsync>d__25>(ref <RegisterPlayerIdentityAsync>d__);
			return <RegisterPlayerIdentityAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00004248 File Offset: 0x00002448
		private static string GenerateEntryId()
		{
			return Guid.NewGuid().ToString("n");
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00004268 File Offset: 0x00002468
		[NullableContext(2)]
		public void SetPlayerIdentity(string playerUid, string playerName)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("FirebaseModlistStore");
			}
			this._playerUid = FirebaseModlistStore.Normalize(playerUid);
			this._playerName = FirebaseModlistStore.Normalize(playerName);
			this._sanitizedPlayerUid = (string.IsNullOrWhiteSpace(this._playerUid) ? null : FirebaseModlistStore.SanitizePlayerUidForFirebase(this._playerUid));
			if (!string.Equals(this._ownershipClaimedForUid, this._sanitizedPlayerUid, 4))
			{
				this._ownershipClaimedForUid = null;
			}
		}

		// Token: 0x06000094 RID: 148 RVA: 0x000042DC File Offset: 0x000024DC
		public Task SaveAsync(string slotKey, string modlistJson, CancellationToken ct = default(CancellationToken))
		{
			FirebaseModlistStore.<SaveAsync>d__28 <SaveAsync>d__;
			<SaveAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SaveAsync>d__.<>4__this = this;
			<SaveAsync>d__.slotKey = slotKey;
			<SaveAsync>d__.modlistJson = modlistJson;
			<SaveAsync>d__.ct = ct;
			<SaveAsync>d__.<>1__state = -1;
			<SaveAsync>d__.<>t__builder.Start<FirebaseModlistStore.<SaveAsync>d__28>(ref <SaveAsync>d__);
			return <SaveAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000095 RID: 149 RVA: 0x00004338 File Offset: 0x00002538
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		public Task<string> LoadAsync(string slotKey, CancellationToken ct = default(CancellationToken))
		{
			FirebaseModlistStore.<LoadAsync>d__29 <LoadAsync>d__;
			<LoadAsync>d__.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
			<LoadAsync>d__.<>4__this = this;
			<LoadAsync>d__.slotKey = slotKey;
			<LoadAsync>d__.ct = ct;
			<LoadAsync>d__.<>1__state = -1;
			<LoadAsync>d__.<>t__builder.Start<FirebaseModlistStore.<LoadAsync>d__29>(ref <LoadAsync>d__);
			return <LoadAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000096 RID: 150 RVA: 0x0000438C File Offset: 0x0000258C
		public Task<IReadOnlyList<string>> ListSlotsAsync(CancellationToken ct = default(CancellationToken))
		{
			FirebaseModlistStore.<ListSlotsAsync>d__30 <ListSlotsAsync>d__;
			<ListSlotsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IReadOnlyList<string>>.Create();
			<ListSlotsAsync>d__.<>4__this = this;
			<ListSlotsAsync>d__.ct = ct;
			<ListSlotsAsync>d__.<>1__state = -1;
			<ListSlotsAsync>d__.<>t__builder.Start<FirebaseModlistStore.<ListSlotsAsync>d__30>(ref <ListSlotsAsync>d__);
			return <ListSlotsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x000043D8 File Offset: 0x000025D8
		public Task DeleteAsync(string slotKey, CancellationToken ct = default(CancellationToken))
		{
			FirebaseModlistStore.<DeleteAsync>d__31 <DeleteAsync>d__;
			<DeleteAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<DeleteAsync>d__.<>4__this = this;
			<DeleteAsync>d__.slotKey = slotKey;
			<DeleteAsync>d__.ct = ct;
			<DeleteAsync>d__.<>1__state = -1;
			<DeleteAsync>d__.<>t__builder.Start<FirebaseModlistStore.<DeleteAsync>d__31>(ref <DeleteAsync>d__);
			return <DeleteAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x0000442C File Offset: 0x0000262C
		public Task DeleteAllUserDataAsync(CancellationToken ct = default(CancellationToken))
		{
			FirebaseModlistStore.<DeleteAllUserDataAsync>d__32 <DeleteAllUserDataAsync>d__;
			<DeleteAllUserDataAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<DeleteAllUserDataAsync>d__.<>4__this = this;
			<DeleteAllUserDataAsync>d__.ct = ct;
			<DeleteAllUserDataAsync>d__.<>1__state = -1;
			<DeleteAllUserDataAsync>d__.<>t__builder.Start<FirebaseModlistStore.<DeleteAllUserDataAsync>d__32>(ref <DeleteAllUserDataAsync>d__);
			return <DeleteAllUserDataAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004478 File Offset: 0x00002678
		public Task<IReadOnlyList<CloudModlistRegistryEntry>> GetRegistryEntriesAsync(CancellationToken ct = default(CancellationToken))
		{
			FirebaseModlistStore.<GetRegistryEntriesAsync>d__33 <GetRegistryEntriesAsync>d__;
			<GetRegistryEntriesAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IReadOnlyList<CloudModlistRegistryEntry>>.Create();
			<GetRegistryEntriesAsync>d__.<>4__this = this;
			<GetRegistryEntriesAsync>d__.ct = ct;
			<GetRegistryEntriesAsync>d__.<>1__state = -1;
			<GetRegistryEntriesAsync>d__.<>t__builder.Start<FirebaseModlistStore.<GetRegistryEntriesAsync>d__33>(ref <GetRegistryEntriesAsync>d__);
			return <GetRegistryEntriesAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000044C4 File Offset: 0x000026C4
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		public Task<CloudModlistRegistryEntry> GetRegistryEntryAsync(string registryId, CancellationToken ct = default(CancellationToken))
		{
			FirebaseModlistStore.<GetRegistryEntryAsync>d__34 <GetRegistryEntryAsync>d__;
			<GetRegistryEntryAsync>d__.<>t__builder = AsyncTaskMethodBuilder<CloudModlistRegistryEntry>.Create();
			<GetRegistryEntryAsync>d__.<>4__this = this;
			<GetRegistryEntryAsync>d__.registryId = registryId;
			<GetRegistryEntryAsync>d__.ct = ct;
			<GetRegistryEntryAsync>d__.<>1__state = -1;
			<GetRegistryEntryAsync>d__.<>t__builder.Start<FirebaseModlistStore.<GetRegistryEntryAsync>d__34>(ref <GetRegistryEntryAsync>d__);
			return <GetRegistryEntryAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004518 File Offset: 0x00002718
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		public Task<string> GetFirstFreeSlotAsync(CancellationToken ct = default(CancellationToken))
		{
			FirebaseModlistStore.<GetFirstFreeSlotAsync>d__35 <GetFirstFreeSlotAsync>d__;
			<GetFirstFreeSlotAsync>d__.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
			<GetFirstFreeSlotAsync>d__.<>4__this = this;
			<GetFirstFreeSlotAsync>d__.ct = ct;
			<GetFirstFreeSlotAsync>d__.<>1__state = -1;
			<GetFirstFreeSlotAsync>d__.<>t__builder.Start<FirebaseModlistStore.<GetFirstFreeSlotAsync>d__35>(ref <GetFirstFreeSlotAsync>d__);
			return <GetFirstFreeSlotAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004564 File Offset: 0x00002764
		private Task<IReadOnlyList<string>> ListSlotsAsync(FirebaseModlistStore.PlayerIdentity identity, CancellationToken ct)
		{
			FirebaseModlistStore.<ListSlotsAsync>d__36 <ListSlotsAsync>d__;
			<ListSlotsAsync>d__.<>t__builder = AsyncTaskMethodBuilder<IReadOnlyList<string>>.Create();
			<ListSlotsAsync>d__.<>4__this = this;
			<ListSlotsAsync>d__.identity = identity;
			<ListSlotsAsync>d__.ct = ct;
			<ListSlotsAsync>d__.<>1__state = -1;
			<ListSlotsAsync>d__.<>t__builder.Start<FirebaseModlistStore.<ListSlotsAsync>d__36>(ref <ListSlotsAsync>d__);
			return <ListSlotsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000045B8 File Offset: 0x000027B8
		private Task<List<CloudModlistRegistryEntry>> TryFetchRegistryEntriesAsync(string path, bool isContentComplete, CancellationToken ct)
		{
			FirebaseModlistStore.<TryFetchRegistryEntriesAsync>d__37 <TryFetchRegistryEntriesAsync>d__;
			<TryFetchRegistryEntriesAsync>d__.<>t__builder = AsyncTaskMethodBuilder<List<CloudModlistRegistryEntry>>.Create();
			<TryFetchRegistryEntriesAsync>d__.<>4__this = this;
			<TryFetchRegistryEntriesAsync>d__.path = path;
			<TryFetchRegistryEntriesAsync>d__.isContentComplete = isContentComplete;
			<TryFetchRegistryEntriesAsync>d__.ct = ct;
			<TryFetchRegistryEntriesAsync>d__.<>1__state = -1;
			<TryFetchRegistryEntriesAsync>d__.<>t__builder.Start<FirebaseModlistStore.<TryFetchRegistryEntriesAsync>d__37>(ref <TryFetchRegistryEntriesAsync>d__);
			return <TryFetchRegistryEntriesAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004614 File Offset: 0x00002814
		private static CloudModlistSummary BuildSummary(string contentJson)
		{
			CloudModlistSummary result;
			using (JsonDocument jsonDocument = JsonDocument.Parse(contentJson, default(JsonDocumentOptions)))
			{
				result = CloudModlistSummary.FromJsonElement(jsonDocument.RootElement);
			}
			return result;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x0000465C File Offset: 0x0000285C
		private static string BuildSummaryJson(string contentJson, string dateAddedIso)
		{
			return JsonSerializer.Serialize<object>(FirebaseModlistStore.BuildSummary(contentJson).ToFirebasePayload(dateAddedIso), FirebaseModlistStore.JsonOpts);
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00004674 File Offset: 0x00002874
		private static string GetRegistryCachePath(string path)
		{
			string firebaseBackupDirectory = DevConfig.FirebaseBackupDirectory;
			Directory.CreateDirectory(firebaseBackupDirectory);
			string text = path.Replace('/', '_');
			return Path.Combine(firebaseBackupDirectory, "registry-" + text + "-cache.json");
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x000046AD File Offset: 0x000028AD
		private static string GetRegistryEntryCachePath(string registryId)
		{
			string text = Path.Combine(DevConfig.FirebaseBackupDirectory, "registry-entries");
			Directory.CreateDirectory(text);
			return Path.Combine(text, registryId + ".json");
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000046D8 File Offset: 0x000028D8
		private static List<CloudModlistRegistryEntry> ParseRegistryEntries(JsonDocument document, bool isContentComplete)
		{
			if (document.RootElement.ValueKind != 1)
			{
				return new List<CloudModlistRegistryEntry>();
			}
			List<CloudModlistRegistryEntry> list = new List<CloudModlistRegistryEntry>();
			foreach (JsonProperty jsonProperty in document.RootElement.EnumerateObject())
			{
				JsonElement jsonElement;
				if (jsonProperty.Value.ValueKind == 1 && jsonProperty.Value.TryGetProperty("content", ref jsonElement))
				{
					JsonValueKind valueKind = jsonElement.ValueKind;
					if (valueKind != null && valueKind != 7)
					{
						string rawText = jsonElement.GetRawText();
						DateTimeOffset? dateAdded = default(DateTimeOffset?);
						JsonElement jsonElement2;
						if (jsonProperty.Value.TryGetProperty("dateAdded", ref jsonElement2) && jsonElement2.ValueKind == 3)
						{
							string @string = jsonElement2.GetString();
							DateTimeOffset dateTimeOffset;
							if (!string.IsNullOrWhiteSpace(@string) && DateTimeOffset.TryParse(@string, CultureInfo.InvariantCulture, 128, ref dateTimeOffset))
							{
								dateAdded = new DateTimeOffset?(dateTimeOffset);
							}
						}
						list.Add(new CloudModlistRegistryEntry(jsonProperty.Name, "public", rawText, dateAdded, isContentComplete));
					}
				}
			}
			return list;
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00004824 File Offset: 0x00002A24
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		private Task<FirebaseModlistStore.RegistryCache> TryReadRegistryCacheAsync(string cachePath, CancellationToken ct)
		{
			FirebaseModlistStore.<TryReadRegistryCacheAsync>d__43 <TryReadRegistryCacheAsync>d__;
			<TryReadRegistryCacheAsync>d__.<>t__builder = AsyncTaskMethodBuilder<FirebaseModlistStore.RegistryCache>.Create();
			<TryReadRegistryCacheAsync>d__.<>4__this = this;
			<TryReadRegistryCacheAsync>d__.cachePath = cachePath;
			<TryReadRegistryCacheAsync>d__.ct = ct;
			<TryReadRegistryCacheAsync>d__.<>1__state = -1;
			<TryReadRegistryCacheAsync>d__.<>t__builder.Start<FirebaseModlistStore.<TryReadRegistryCacheAsync>d__43>(ref <TryReadRegistryCacheAsync>d__);
			return <TryReadRegistryCacheAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00004878 File Offset: 0x00002A78
		private static Task WriteRegistryCacheAsync(string cachePath, IReadOnlyCollection<CloudModlistRegistryEntry> entries, [Nullable(2)] string eTag, bool isContentComplete, CancellationToken ct)
		{
			FirebaseModlistStore.<WriteRegistryCacheAsync>d__44 <WriteRegistryCacheAsync>d__;
			<WriteRegistryCacheAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WriteRegistryCacheAsync>d__.cachePath = cachePath;
			<WriteRegistryCacheAsync>d__.entries = entries;
			<WriteRegistryCacheAsync>d__.eTag = eTag;
			<WriteRegistryCacheAsync>d__.isContentComplete = isContentComplete;
			<WriteRegistryCacheAsync>d__.ct = ct;
			<WriteRegistryCacheAsync>d__.<>1__state = -1;
			<WriteRegistryCacheAsync>d__.<>t__builder.Start<FirebaseModlistStore.<WriteRegistryCacheAsync>d__44>(ref <WriteRegistryCacheAsync>d__);
			return <WriteRegistryCacheAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000048DC File Offset: 0x00002ADC
		[return: Nullable(2)]
		private static CloudModlistRegistryEntry ParseRegistryEntry(string registryId, string json)
		{
			if (string.IsNullOrWhiteSpace(json) || json == "null")
			{
				return null;
			}
			CloudModlistRegistryEntry result;
			using (JsonDocument jsonDocument = JsonDocument.Parse(json, default(JsonDocumentOptions)))
			{
				JsonElement jsonElement;
				if (jsonDocument.RootElement.ValueKind != 1)
				{
					result = null;
				}
				else if (!jsonDocument.RootElement.TryGetProperty("content", ref jsonElement))
				{
					result = null;
				}
				else
				{
					JsonValueKind valueKind = jsonElement.ValueKind;
					bool flag = valueKind == null || valueKind == 7;
					if (flag)
					{
						result = null;
					}
					else
					{
						DateTimeOffset? dateAdded = default(DateTimeOffset?);
						JsonElement jsonElement2;
						if (jsonDocument.RootElement.TryGetProperty("dateAdded", ref jsonElement2) && jsonElement2.ValueKind == 3)
						{
							string @string = jsonElement2.GetString();
							DateTimeOffset dateTimeOffset;
							if (!string.IsNullOrWhiteSpace(@string) && DateTimeOffset.TryParse(@string, CultureInfo.InvariantCulture, 128, ref dateTimeOffset))
							{
								dateAdded = new DateTimeOffset?(dateTimeOffset);
							}
						}
						string rawText = jsonElement.GetRawText();
						result = new CloudModlistRegistryEntry(registryId, "public", rawText, dateAdded, true);
					}
				}
			}
			return result;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00004A00 File Offset: 0x00002C00
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		private static Task<FirebaseModlistStore.RegistryEntryCache> TryReadRegistryEntryCacheAsync(string cachePath, CancellationToken ct)
		{
			FirebaseModlistStore.<TryReadRegistryEntryCacheAsync>d__46 <TryReadRegistryEntryCacheAsync>d__;
			<TryReadRegistryEntryCacheAsync>d__.<>t__builder = AsyncTaskMethodBuilder<FirebaseModlistStore.RegistryEntryCache>.Create();
			<TryReadRegistryEntryCacheAsync>d__.cachePath = cachePath;
			<TryReadRegistryEntryCacheAsync>d__.ct = ct;
			<TryReadRegistryEntryCacheAsync>d__.<>1__state = -1;
			<TryReadRegistryEntryCacheAsync>d__.<>t__builder.Start<FirebaseModlistStore.<TryReadRegistryEntryCacheAsync>d__46>(ref <TryReadRegistryEntryCacheAsync>d__);
			return <TryReadRegistryEntryCacheAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00004A4C File Offset: 0x00002C4C
		private static Task WriteRegistryEntryCacheAsync(string cachePath, CloudModlistRegistryEntry entry, [Nullable(2)] string eTag, CancellationToken ct)
		{
			FirebaseModlistStore.<WriteRegistryEntryCacheAsync>d__47 <WriteRegistryEntryCacheAsync>d__;
			<WriteRegistryEntryCacheAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<WriteRegistryEntryCacheAsync>d__.cachePath = cachePath;
			<WriteRegistryEntryCacheAsync>d__.entry = entry;
			<WriteRegistryEntryCacheAsync>d__.eTag = eTag;
			<WriteRegistryEntryCacheAsync>d__.ct = ct;
			<WriteRegistryEntryCacheAsync>d__.<>1__state = -1;
			<WriteRegistryEntryCacheAsync>d__.<>t__builder.Start<FirebaseModlistStore.<WriteRegistryEntryCacheAsync>d__47>(ref <WriteRegistryEntryCacheAsync>d__);
			return <WriteRegistryEntryCacheAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00004AA8 File Offset: 0x00002CA8
		private static Task DeleteRegistryEntryCacheAsync(string cachePath)
		{
			FirebaseModlistStore.<DeleteRegistryEntryCacheAsync>d__48 <DeleteRegistryEntryCacheAsync>d__;
			<DeleteRegistryEntryCacheAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<DeleteRegistryEntryCacheAsync>d__.cachePath = cachePath;
			<DeleteRegistryEntryCacheAsync>d__.<>1__state = -1;
			<DeleteRegistryEntryCacheAsync>d__.<>t__builder.Start<FirebaseModlistStore.<DeleteRegistryEntryCacheAsync>d__48>(ref <DeleteRegistryEntryCacheAsync>d__);
			return <DeleteRegistryEntryCacheAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00004AEC File Offset: 0x00002CEC
		private FirebaseModlistStore.PlayerIdentity GetIdentityComponents()
		{
			string text = this._playerUid;
			string text2 = this._sanitizedPlayerUid;
			string text3 = this._playerName;
			if (string.IsNullOrWhiteSpace(text))
			{
				throw new InvalidOperationException("The Vintage Story clientsettings.json file does not contain a playeruid value. Start the game once to generate it.");
			}
			if (string.IsNullOrWhiteSpace(text3))
			{
				throw new InvalidOperationException("The Vintage Story clientsettings.json file does not contain a playername value. Set a player name in the game before using cloud modlists.");
			}
			text = text.Trim();
			text3 = text3.Trim();
			text2 = (string.IsNullOrWhiteSpace(text2) ? FirebaseModlistStore.SanitizePlayerUidForFirebase(text) : text2);
			return new FirebaseModlistStore.PlayerIdentity(text, text2, text3);
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00004B5C File Offset: 0x00002D5C
		private static Task EnsureOk(HttpResponseMessage response, string operation)
		{
			FirebaseModlistStore.<EnsureOk>d__50 <EnsureOk>d__;
			<EnsureOk>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<EnsureOk>d__.response = response;
			<EnsureOk>d__.operation = operation;
			<EnsureOk>d__.<>1__state = -1;
			<EnsureOk>d__.<>t__builder.Start<FirebaseModlistStore.<EnsureOk>d__50>(ref <EnsureOk>d__);
			return <EnsureOk>d__.<>t__builder.Task;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00004BA7 File Offset: 0x00002DA7
		private static void LogRestFailure(string message)
		{
			if (string.IsNullOrWhiteSpace(message))
			{
				return;
			}
			StatusLogService.AppendStatus(message, true);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00004BBC File Offset: 0x00002DBC
		[return: TupleElementNames(new string[]
		{
			"Response",
			"Session"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		private Task<ValueTuple<HttpResponseMessage, FirebaseAnonymousAuthenticator.FirebaseAuthSession>> SendWithAuthRetryAsync(Func<FirebaseAnonymousAuthenticator.FirebaseAuthSession, Task<HttpResponseMessage>> operation, CancellationToken ct)
		{
			FirebaseModlistStore.<SendWithAuthRetryAsync>d__52 <SendWithAuthRetryAsync>d__;
			<SendWithAuthRetryAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ValueTuple<HttpResponseMessage, FirebaseAnonymousAuthenticator.FirebaseAuthSession>>.Create();
			<SendWithAuthRetryAsync>d__.<>4__this = this;
			<SendWithAuthRetryAsync>d__.operation = operation;
			<SendWithAuthRetryAsync>d__.ct = ct;
			<SendWithAuthRetryAsync>d__.<>1__state = -1;
			<SendWithAuthRetryAsync>d__.<>t__builder.Start<FirebaseModlistStore.<SendWithAuthRetryAsync>d__52>(ref <SendWithAuthRetryAsync>d__);
			return <SendWithAuthRetryAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00004C10 File Offset: 0x00002E10
		private Task EnsureOwnershipAsync(FirebaseModlistStore.PlayerIdentity identity, CancellationToken ct)
		{
			FirebaseModlistStore.<EnsureOwnershipAsync>d__53 <EnsureOwnershipAsync>d__;
			<EnsureOwnershipAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<EnsureOwnershipAsync>d__.<>4__this = this;
			<EnsureOwnershipAsync>d__.identity = identity;
			<EnsureOwnershipAsync>d__.ct = ct;
			<EnsureOwnershipAsync>d__.<>1__state = -1;
			<EnsureOwnershipAsync>d__.<>t__builder.Start<FirebaseModlistStore.<EnsureOwnershipAsync>d__53>(ref <EnsureOwnershipAsync>d__);
			return <EnsureOwnershipAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00004C64 File Offset: 0x00002E64
		private Task DeleteOwnershipAsync(FirebaseModlistStore.PlayerIdentity identity, CancellationToken ct)
		{
			FirebaseModlistStore.<DeleteOwnershipAsync>d__54 <DeleteOwnershipAsync>d__;
			<DeleteOwnershipAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<DeleteOwnershipAsync>d__.<>4__this = this;
			<DeleteOwnershipAsync>d__.identity = identity;
			<DeleteOwnershipAsync>d__.ct = ct;
			<DeleteOwnershipAsync>d__.<>1__state = -1;
			<DeleteOwnershipAsync>d__.<>t__builder.Start<FirebaseModlistStore.<DeleteOwnershipAsync>d__54>(ref <DeleteOwnershipAsync>d__);
			return <DeleteOwnershipAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00004CB8 File Offset: 0x00002EB8
		[return: Nullable(2)]
		private static string ParseOwnerIdentifier(string json)
		{
			if (string.IsNullOrWhiteSpace(json) || string.Equals(json.Trim(), "null", 4))
			{
				return null;
			}
			string result;
			try
			{
				string text = JsonSerializer.Deserialize<string>(json, FirebaseModlistStore.JsonOpts);
				result = (string.IsNullOrWhiteSpace(text) ? null : text.Trim());
			}
			catch (JsonException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00004D1C File Offset: 0x00002F1C
		private string BuildAuthenticatedUrl(string authToken, [Nullable(2)] string query, params string[] segments)
		{
			StringBuilder stringBuilder = new StringBuilder(this._dbUrl.Length + 64);
			stringBuilder.Append(this._dbUrl);
			stringBuilder.Append('/');
			for (int i = 0; i < segments.Length; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append('/');
				}
				stringBuilder.Append(Uri.EscapeDataString(segments[i]));
			}
			stringBuilder.Append(".json?auth=");
			stringBuilder.Append(Uri.EscapeDataString(authToken));
			if (!string.IsNullOrWhiteSpace(query))
			{
				stringBuilder.Append('&');
				stringBuilder.Append(query);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00004DB8 File Offset: 0x00002FB8
		private static bool IsAuthError(HttpStatusCode statusCode)
		{
			return statusCode == 401 || statusCode == 403;
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00004DDC File Offset: 0x00002FDC
		private static string ReplaceUploader(JsonElement root, FirebaseModlistStore.PlayerIdentity identity)
		{
			ArrayBufferWriter<byte> arrayBufferWriter = new ArrayBufferWriter<byte>();
			using (Utf8JsonWriter utf8JsonWriter = new Utf8JsonWriter(arrayBufferWriter, default(JsonWriterOptions)))
			{
				utf8JsonWriter.WriteStartObject();
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				foreach (JsonProperty jsonProperty in root.EnumerateObject())
				{
					if (jsonProperty.NameEquals("uploader"))
					{
						utf8JsonWriter.WriteString("uploader", identity.Name);
						flag = true;
					}
					else if (jsonProperty.NameEquals("uploaderName"))
					{
						utf8JsonWriter.WriteString("uploaderName", identity.Name);
						flag2 = true;
					}
					else if (jsonProperty.NameEquals("uploaderId"))
					{
						utf8JsonWriter.WriteString("uploaderId", identity.SanitizedUid);
						flag3 = true;
					}
					else
					{
						jsonProperty.WriteTo(utf8JsonWriter);
					}
				}
				if (!flag)
				{
					utf8JsonWriter.WriteString("uploader", identity.Name);
				}
				if (!flag2)
				{
					utf8JsonWriter.WriteString("uploaderName", identity.Name);
				}
				if (!flag3)
				{
					utf8JsonWriter.WriteString("uploaderId", identity.SanitizedUid);
				}
				utf8JsonWriter.WriteEndObject();
			}
			return Encoding.UTF8.GetString(arrayBufferWriter.WrittenSpan);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00004F54 File Offset: 0x00003154
		private static string BuildNodeJson(string contentJson)
		{
			string result;
			using (JsonDocument jsonDocument = JsonDocument.Parse(contentJson, default(JsonDocumentOptions)))
			{
				result = JsonSerializer.Serialize<FirebaseModlistStore.ModlistNode>(new FirebaseModlistStore.ModlistNode
				{
					Content = jsonDocument.RootElement.Clone()
				}, FirebaseModlistStore.JsonOpts);
			}
			return result;
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x00004FBC File Offset: 0x000031BC
		private static void ValidateSlotKey(string slotKey)
		{
			if (Array.IndexOf<string>(FirebaseModlistStore.KnownSlots, slotKey) < 0)
			{
				throw new ArgumentException("Slot must be one of: slot1, slot2, slot3, slot4, slot5.", "slotKey");
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00004FDC File Offset: 0x000031DC
		[NullableContext(2)]
		private static string Normalize(string value)
		{
			if (!string.IsNullOrWhiteSpace(value))
			{
				return value.Trim();
			}
			return null;
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00004FF0 File Offset: 0x000031F0
		private static string SanitizePlayerUidForFirebase(string playerUid)
		{
			if (string.IsNullOrWhiteSpace(playerUid))
			{
				throw new ArgumentException("Player UID cannot be null or whitespace.", "playerUid");
			}
			StringBuilder stringBuilder = new StringBuilder(playerUid.Length);
			for (int i = 0; i < playerUid.Length; i++)
			{
				char c = playerUid.get_Chars(i);
				if (c == '.' || c == '$' || c == '#' || c == '[' || c == ']' || c == '/' || c < ' ' || c == '\u007f')
				{
					stringBuilder.Append('_');
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000507D File Offset: 0x0000327D
		private static string Truncate(string value, int max)
		{
			if (value.Length > max)
			{
				return value.Substring(0, max) + "...";
			}
			return value;
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000509C File Offset: 0x0000329C
		private static bool IsKnownSlot(string slotKey)
		{
			return Array.IndexOf<string>(FirebaseModlistStore.KnownSlots, slotKey) >= 0;
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x000050B0 File Offset: 0x000032B0
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
			this._ownershipClaimLock.Dispose();
			this._registryCacheLock.Dispose();
		}

		// Token: 0x04000066 RID: 102
		private static readonly HttpClient HttpClient = new HttpClient
		{
			Timeout = TimeSpan.FromSeconds(15.0)
		};

		// Token: 0x04000067 RID: 103
		private static readonly string DefaultDbUrl = DevConfig.FirebaseModlistDefaultDbUrl;

		// Token: 0x04000068 RID: 104
		private static readonly string[] KnownSlots = new string[]
		{
			"slot1",
			"slot2",
			"slot3",
			"slot4",
			"slot5"
		};

		// Token: 0x04000069 RID: 105
		private static readonly JsonSerializerOptions JsonOpts = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			DefaultIgnoreCondition = 3
		};

		// Token: 0x0400006A RID: 106
		private readonly string _dbUrl;

		// Token: 0x0400006B RID: 107
		private readonly SemaphoreSlim _ownershipClaimLock = new SemaphoreSlim(1, 1);

		// Token: 0x0400006C RID: 108
		private readonly SemaphoreSlim _registryCacheLock = new SemaphoreSlim(1, 1);

		// Token: 0x0400006D RID: 109
		private readonly object _disposeLock = new object();

		// Token: 0x0400006E RID: 110
		[Nullable(2)]
		private string _ownershipClaimedForUid;

		// Token: 0x0400006F RID: 111
		[Nullable(2)]
		private string _playerName;

		// Token: 0x04000070 RID: 112
		private readonly Dictionary<string, FirebaseModlistStore.RegistryCache> _registryCache = new Dictionary<string, FirebaseModlistStore.RegistryCache>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04000071 RID: 113
		[Nullable(2)]
		private string _playerUid;

		// Token: 0x04000072 RID: 114
		[Nullable(2)]
		private string _sanitizedPlayerUid;

		// Token: 0x04000073 RID: 115
		private bool _disposed;

		// Token: 0x020000EF RID: 239
		[Nullable(0)]
		private sealed class RegistryCache : IEquatable<FirebaseModlistStore.RegistryCache>
		{
			// Token: 0x060010C5 RID: 4293 RVA: 0x000593AE File Offset: 0x000575AE
			public RegistryCache(IReadOnlyList<CloudModlistRegistryEntry> Entries, [Nullable(2)] string ETag, bool IsContentComplete)
			{
				this.Entries = Entries;
				this.ETag = ETag;
				this.IsContentComplete = IsContentComplete;
				base..ctor();
			}

			// Token: 0x17000408 RID: 1032
			// (get) Token: 0x060010C6 RID: 4294 RVA: 0x000593CB File Offset: 0x000575CB
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(FirebaseModlistStore.RegistryCache);
				}
			}

			// Token: 0x17000409 RID: 1033
			// (get) Token: 0x060010C7 RID: 4295 RVA: 0x000593D7 File Offset: 0x000575D7
			// (set) Token: 0x060010C8 RID: 4296 RVA: 0x000593DF File Offset: 0x000575DF
			public IReadOnlyList<CloudModlistRegistryEntry> Entries { get; set; }

			// Token: 0x1700040A RID: 1034
			// (get) Token: 0x060010C9 RID: 4297 RVA: 0x000593E8 File Offset: 0x000575E8
			// (set) Token: 0x060010CA RID: 4298 RVA: 0x000593F0 File Offset: 0x000575F0
			[Nullable(2)]
			public string ETag { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x1700040B RID: 1035
			// (get) Token: 0x060010CB RID: 4299 RVA: 0x000593F9 File Offset: 0x000575F9
			// (set) Token: 0x060010CC RID: 4300 RVA: 0x00059401 File Offset: 0x00057601
			public bool IsContentComplete { get; set; }

			// Token: 0x060010CD RID: 4301 RVA: 0x0005940C File Offset: 0x0005760C
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("RegistryCache");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x060010CE RID: 4302 RVA: 0x00059458 File Offset: 0x00057658
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Entries = ");
				builder.Append(this.Entries);
				builder.Append(", ETag = ");
				builder.Append(this.ETag);
				builder.Append(", IsContentComplete = ");
				builder.Append(this.IsContentComplete.ToString());
				return true;
			}

			// Token: 0x060010CF RID: 4303 RVA: 0x000594C4 File Offset: 0x000576C4
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(FirebaseModlistStore.RegistryCache left, FirebaseModlistStore.RegistryCache right)
			{
				return !(left == right);
			}

			// Token: 0x060010D0 RID: 4304 RVA: 0x000594D0 File Offset: 0x000576D0
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(FirebaseModlistStore.RegistryCache left, FirebaseModlistStore.RegistryCache right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x060010D1 RID: 4305 RVA: 0x000594E4 File Offset: 0x000576E4
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<IReadOnlyList<CloudModlistRegistryEntry>>.Default.GetHashCode(this.<Entries>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ETag>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<IsContentComplete>k__BackingField);
			}

			// Token: 0x060010D2 RID: 4306 RVA: 0x00059546 File Offset: 0x00057746
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as FirebaseModlistStore.RegistryCache);
			}

			// Token: 0x060010D3 RID: 4307 RVA: 0x00059554 File Offset: 0x00057754
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(FirebaseModlistStore.RegistryCache other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<IReadOnlyList<CloudModlistRegistryEntry>>.Default.Equals(this.<Entries>k__BackingField, other.<Entries>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<ETag>k__BackingField, other.<ETag>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<IsContentComplete>k__BackingField, other.<IsContentComplete>k__BackingField));
			}

			// Token: 0x060010D5 RID: 4309 RVA: 0x000595CD File Offset: 0x000577CD
			[CompilerGenerated]
			private RegistryCache(FirebaseModlistStore.RegistryCache original)
			{
				this.Entries = original.<Entries>k__BackingField;
				this.ETag = original.<ETag>k__BackingField;
				this.IsContentComplete = original.<IsContentComplete>k__BackingField;
			}

			// Token: 0x060010D6 RID: 4310 RVA: 0x000595F9 File Offset: 0x000577F9
			[CompilerGenerated]
			public void Deconstruct(out IReadOnlyList<CloudModlistRegistryEntry> Entries, [Nullable(2)] out string ETag, out bool IsContentComplete)
			{
				Entries = this.Entries;
				ETag = this.ETag;
				IsContentComplete = this.IsContentComplete;
			}
		}

		// Token: 0x020000F0 RID: 240
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class RegistryCachePayload
		{
			// Token: 0x1700040C RID: 1036
			// (get) Token: 0x060010D7 RID: 4311 RVA: 0x00059613 File Offset: 0x00057813
			// (set) Token: 0x060010D8 RID: 4312 RVA: 0x0005961B File Offset: 0x0005781B
			public string ETag { get; set; }

			// Token: 0x1700040D RID: 1037
			// (get) Token: 0x060010D9 RID: 4313 RVA: 0x00059624 File Offset: 0x00057824
			// (set) Token: 0x060010DA RID: 4314 RVA: 0x0005962C File Offset: 0x0005782C
			public bool IsContentComplete { get; set; }

			// Token: 0x1700040E RID: 1038
			// (get) Token: 0x060010DB RID: 4315 RVA: 0x00059635 File Offset: 0x00057835
			// (set) Token: 0x060010DC RID: 4316 RVA: 0x0005963D File Offset: 0x0005783D
			[Nullable(new byte[]
			{
				2,
				1
			})]
			public List<FirebaseModlistStore.RegistryCacheEntry> Entries { [return: Nullable(new byte[]
			{
				2,
				1
			})] get; [param: Nullable(new byte[]
			{
				2,
				1
			})] set; }
		}

		// Token: 0x020000F1 RID: 241
		[Nullable(0)]
		private sealed class RegistryCacheEntry : IEquatable<FirebaseModlistStore.RegistryCacheEntry>
		{
			// Token: 0x060010DE RID: 4318 RVA: 0x0005964E File Offset: 0x0005784E
			public RegistryCacheEntry(string OwnerId, string SlotKey, string ContentJson, DateTimeOffset? DateAdded)
			{
				this.OwnerId = OwnerId;
				this.SlotKey = SlotKey;
				this.ContentJson = ContentJson;
				this.DateAdded = DateAdded;
				base..ctor();
			}

			// Token: 0x1700040F RID: 1039
			// (get) Token: 0x060010DF RID: 4319 RVA: 0x00059673 File Offset: 0x00057873
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(FirebaseModlistStore.RegistryCacheEntry);
				}
			}

			// Token: 0x17000410 RID: 1040
			// (get) Token: 0x060010E0 RID: 4320 RVA: 0x0005967F File Offset: 0x0005787F
			// (set) Token: 0x060010E1 RID: 4321 RVA: 0x00059687 File Offset: 0x00057887
			public string OwnerId { get; set; }

			// Token: 0x17000411 RID: 1041
			// (get) Token: 0x060010E2 RID: 4322 RVA: 0x00059690 File Offset: 0x00057890
			// (set) Token: 0x060010E3 RID: 4323 RVA: 0x00059698 File Offset: 0x00057898
			public string SlotKey { get; set; }

			// Token: 0x17000412 RID: 1042
			// (get) Token: 0x060010E4 RID: 4324 RVA: 0x000596A1 File Offset: 0x000578A1
			// (set) Token: 0x060010E5 RID: 4325 RVA: 0x000596A9 File Offset: 0x000578A9
			public string ContentJson { get; set; }

			// Token: 0x17000413 RID: 1043
			// (get) Token: 0x060010E6 RID: 4326 RVA: 0x000596B2 File Offset: 0x000578B2
			// (set) Token: 0x060010E7 RID: 4327 RVA: 0x000596BA File Offset: 0x000578BA
			public DateTimeOffset? DateAdded { get; set; }

			// Token: 0x060010E8 RID: 4328 RVA: 0x000596C4 File Offset: 0x000578C4
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("RegistryCacheEntry");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x060010E9 RID: 4329 RVA: 0x00059710 File Offset: 0x00057910
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("OwnerId = ");
				builder.Append(this.OwnerId);
				builder.Append(", SlotKey = ");
				builder.Append(this.SlotKey);
				builder.Append(", ContentJson = ");
				builder.Append(this.ContentJson);
				builder.Append(", DateAdded = ");
				builder.Append(this.DateAdded.ToString());
				return true;
			}

			// Token: 0x060010EA RID: 4330 RVA: 0x00059795 File Offset: 0x00057995
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(FirebaseModlistStore.RegistryCacheEntry left, FirebaseModlistStore.RegistryCacheEntry right)
			{
				return !(left == right);
			}

			// Token: 0x060010EB RID: 4331 RVA: 0x000597A1 File Offset: 0x000579A1
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(FirebaseModlistStore.RegistryCacheEntry left, FirebaseModlistStore.RegistryCacheEntry right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x060010EC RID: 4332 RVA: 0x000597B8 File Offset: 0x000579B8
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<OwnerId>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<SlotKey>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ContentJson>k__BackingField)) * -1521134295 + EqualityComparer<DateTimeOffset?>.Default.GetHashCode(this.<DateAdded>k__BackingField);
			}

			// Token: 0x060010ED RID: 4333 RVA: 0x00059831 File Offset: 0x00057A31
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as FirebaseModlistStore.RegistryCacheEntry);
			}

			// Token: 0x060010EE RID: 4334 RVA: 0x00059840 File Offset: 0x00057A40
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(FirebaseModlistStore.RegistryCacheEntry other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<OwnerId>k__BackingField, other.<OwnerId>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<SlotKey>k__BackingField, other.<SlotKey>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<ContentJson>k__BackingField, other.<ContentJson>k__BackingField) && EqualityComparer<DateTimeOffset?>.Default.Equals(this.<DateAdded>k__BackingField, other.<DateAdded>k__BackingField));
			}

			// Token: 0x060010F0 RID: 4336 RVA: 0x000598D1 File Offset: 0x00057AD1
			[CompilerGenerated]
			private RegistryCacheEntry(FirebaseModlistStore.RegistryCacheEntry original)
			{
				this.OwnerId = original.<OwnerId>k__BackingField;
				this.SlotKey = original.<SlotKey>k__BackingField;
				this.ContentJson = original.<ContentJson>k__BackingField;
				this.DateAdded = original.<DateAdded>k__BackingField;
			}

			// Token: 0x060010F1 RID: 4337 RVA: 0x00059909 File Offset: 0x00057B09
			[CompilerGenerated]
			public void Deconstruct(out string OwnerId, out string SlotKey, out string ContentJson, out DateTimeOffset? DateAdded)
			{
				OwnerId = this.OwnerId;
				SlotKey = this.SlotKey;
				ContentJson = this.ContentJson;
				DateAdded = this.DateAdded;
			}
		}

		// Token: 0x020000F2 RID: 242
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class RegistryEntryCache : IEquatable<FirebaseModlistStore.RegistryEntryCache>
		{
			// Token: 0x060010F2 RID: 4338 RVA: 0x00059930 File Offset: 0x00057B30
			public RegistryEntryCache(CloudModlistRegistryEntry Entry, string ETag)
			{
				this.Entry = Entry;
				this.ETag = ETag;
				base..ctor();
			}

			// Token: 0x17000414 RID: 1044
			// (get) Token: 0x060010F3 RID: 4339 RVA: 0x00059946 File Offset: 0x00057B46
			[Nullable(1)]
			[CompilerGenerated]
			private Type EqualityContract
			{
				[NullableContext(1)]
				[CompilerGenerated]
				get
				{
					return typeof(FirebaseModlistStore.RegistryEntryCache);
				}
			}

			// Token: 0x17000415 RID: 1045
			// (get) Token: 0x060010F4 RID: 4340 RVA: 0x00059952 File Offset: 0x00057B52
			// (set) Token: 0x060010F5 RID: 4341 RVA: 0x0005995A File Offset: 0x00057B5A
			public CloudModlistRegistryEntry Entry { get; set; }

			// Token: 0x17000416 RID: 1046
			// (get) Token: 0x060010F6 RID: 4342 RVA: 0x00059963 File Offset: 0x00057B63
			// (set) Token: 0x060010F7 RID: 4343 RVA: 0x0005996B File Offset: 0x00057B6B
			public string ETag { get; set; }

			// Token: 0x060010F8 RID: 4344 RVA: 0x00059974 File Offset: 0x00057B74
			[NullableContext(1)]
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("RegistryEntryCache");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x060010F9 RID: 4345 RVA: 0x000599C0 File Offset: 0x00057BC0
			[NullableContext(1)]
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Entry = ");
				builder.Append(this.Entry);
				builder.Append(", ETag = ");
				builder.Append(this.ETag);
				return true;
			}

			// Token: 0x060010FA RID: 4346 RVA: 0x000599FA File Offset: 0x00057BFA
			[CompilerGenerated]
			public static bool operator !=(FirebaseModlistStore.RegistryEntryCache left, FirebaseModlistStore.RegistryEntryCache right)
			{
				return !(left == right);
			}

			// Token: 0x060010FB RID: 4347 RVA: 0x00059A06 File Offset: 0x00057C06
			[CompilerGenerated]
			public static bool operator ==(FirebaseModlistStore.RegistryEntryCache left, FirebaseModlistStore.RegistryEntryCache right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x060010FC RID: 4348 RVA: 0x00059A1A File Offset: 0x00057C1A
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<CloudModlistRegistryEntry>.Default.GetHashCode(this.<Entry>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<ETag>k__BackingField);
			}

			// Token: 0x060010FD RID: 4349 RVA: 0x00059A5A File Offset: 0x00057C5A
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as FirebaseModlistStore.RegistryEntryCache);
			}

			// Token: 0x060010FE RID: 4350 RVA: 0x00059A68 File Offset: 0x00057C68
			[CompilerGenerated]
			public bool Equals(FirebaseModlistStore.RegistryEntryCache other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<CloudModlistRegistryEntry>.Default.Equals(this.<Entry>k__BackingField, other.<Entry>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<ETag>k__BackingField, other.<ETag>k__BackingField));
			}

			// Token: 0x06001100 RID: 4352 RVA: 0x00059AC9 File Offset: 0x00057CC9
			[CompilerGenerated]
			private RegistryEntryCache([Nullable(1)] FirebaseModlistStore.RegistryEntryCache original)
			{
				this.Entry = original.<Entry>k__BackingField;
				this.ETag = original.<ETag>k__BackingField;
			}

			// Token: 0x06001101 RID: 4353 RVA: 0x00059AE9 File Offset: 0x00057CE9
			[CompilerGenerated]
			public void Deconstruct(out CloudModlistRegistryEntry Entry, out string ETag)
			{
				Entry = this.Entry;
				ETag = this.ETag;
			}
		}

		// Token: 0x020000F3 RID: 243
		[NullableContext(2)]
		[Nullable(0)]
		private struct SlotNode
		{
			// Token: 0x17000417 RID: 1047
			// (get) Token: 0x06001102 RID: 4354 RVA: 0x00059AFB File Offset: 0x00057CFB
			// (set) Token: 0x06001103 RID: 4355 RVA: 0x00059B03 File Offset: 0x00057D03
			[JsonPropertyName("registryId")]
			public string RegistryId { readonly get; set; }

			// Token: 0x17000418 RID: 1048
			// (get) Token: 0x06001104 RID: 4356 RVA: 0x00059B0C File Offset: 0x00057D0C
			// (set) Token: 0x06001105 RID: 4357 RVA: 0x00059B14 File Offset: 0x00057D14
			[JsonPropertyName("content")]
			public JsonElement Content { readonly get; set; }

			// Token: 0x17000419 RID: 1049
			// (get) Token: 0x06001106 RID: 4358 RVA: 0x00059B1D File Offset: 0x00057D1D
			// (set) Token: 0x06001107 RID: 4359 RVA: 0x00059B25 File Offset: 0x00057D25
			[JsonPropertyName("dateAdded")]
			public string DateAdded { readonly get; set; }
		}

		// Token: 0x020000F4 RID: 244
		[Nullable(0)]
		private readonly struct PlayerIdentity
		{
			// Token: 0x06001108 RID: 4360 RVA: 0x00059B2E File Offset: 0x00057D2E
			public PlayerIdentity(string originalUid, string sanitizedUid, string name)
			{
				this.OriginalUid = originalUid;
				this.SanitizedUid = sanitizedUid;
				this.Name = name;
			}

			// Token: 0x1700041A RID: 1050
			// (get) Token: 0x06001109 RID: 4361 RVA: 0x00059B45 File Offset: 0x00057D45
			public string OriginalUid { get; }

			// Token: 0x1700041B RID: 1051
			// (get) Token: 0x0600110A RID: 4362 RVA: 0x00059B4D File Offset: 0x00057D4D
			public string SanitizedUid { get; }

			// Token: 0x1700041C RID: 1052
			// (get) Token: 0x0600110B RID: 4363 RVA: 0x00059B55 File Offset: 0x00057D55
			public string Name { get; }
		}

		// Token: 0x020000F5 RID: 245
		[NullableContext(0)]
		private struct ModlistNode
		{
			// Token: 0x1700041D RID: 1053
			// (get) Token: 0x0600110C RID: 4364 RVA: 0x00059B5D File Offset: 0x00057D5D
			// (set) Token: 0x0600110D RID: 4365 RVA: 0x00059B65 File Offset: 0x00057D65
			[JsonPropertyName("content")]
			public JsonElement Content { readonly get; set; }
		}
	}
}
