using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.Client.NoObf;
using Vintagestory.Common;

namespace Vintagestory.ModDb;

public class ModDbUtil
{
	public struct ModInstallInformationResponse
	{
		public string? Error;

		public Dictionary<string, ModInstallInformation>? Data;
	}

	public struct ModInstallInformation
	{
		public enum EnumErrorCode
		{
			SPEC_PARSE_FAILED = 4001,
			MISSING_SPEC_VERSION_NO_GV = 4002,
			FORBIDDEN_IN_HOSTED_MODE = 4031,
			CANNOT_IGNORE_RETRACTION = 4032,
			SPEC_NOT_FOUND = 4041,
			RELEASE_RETRACTED = 4101,
			RELEASE_RETRACTED_FORCED = 4102
		}

		public int ErrorCode;

		public string? FileName;

		public string? FileUrl;

		public string? RetractionReason;

		public string? RecommendedUpgrade;

		public readonly string FormatErrorCode()
		{
			return (EnumErrorCode)ErrorCode switch
			{
				(EnumErrorCode)0 => string.Empty, 
				EnumErrorCode.SPEC_PARSE_FAILED => Lang.Get("modinstall-err-malformed-spec"), 
				EnumErrorCode.MISSING_SPEC_VERSION_NO_GV => Lang.Get("modinstall-err-missing-spec-version-and-gv"), 
				EnumErrorCode.FORBIDDEN_IN_HOSTED_MODE => Lang.Get("modinstall-err-not-allowed-hosted"), 
				EnumErrorCode.CANNOT_IGNORE_RETRACTION => Lang.Get("modinstall-err-cannot-ignore"), 
				EnumErrorCode.SPEC_NOT_FOUND => Lang.Get("modinstall-err-not-found"), 
				EnumErrorCode.RELEASE_RETRACTED => Lang.Get("modinstall-err-retracted"), 
				EnumErrorCode.RELEASE_RETRACTED_FORCED => Lang.Get("modinstall-err-retracted-forced"), 
				_ => $"Unknown Error {ErrorCode}", 
			};
		}
	}

	public enum ModEnableOutcome
	{
		NotFound,
		AlreadyEnabled,
		NewlyEnabled,
		DeletedOlder,
		InstalledIsOlder,
		InstalledIsNewer
	}

	public delegate void DownloadProgressDelegate(long progress, long size);

	private readonly string installPath;

	private readonly Uri modDbUrl;

	private readonly Uri modDbApiUrl;

	private readonly ICoreAPI api;

	private readonly string commandPrefix;

	public static readonly ManualResetEvent ModBlockListMRE = new ManualResetEvent(initialState: false);

	public static Dictionary<string, string>? ModBlockList;

	private GameVersionResponse? gameversions;

	public int selfGameVersionId = -1;

	public int[] sameMinorVersionIds = Array.Empty<int>();

	private List<ModContainer>? mods;

	public ModDbUtil(ICoreAPI api, string modDbUrl, string installPath)
		: this(api, new Uri(modDbUrl), installPath)
	{
	}

	public ModDbUtil(ICoreAPI api, Uri modDbUrl, string installPath)
	{
		this.api = api;
		this.modDbUrl = modDbUrl;
		modDbApiUrl = new Uri(modDbUrl, "api/");
		this.installPath = installPath;
		commandPrefix = ((api.Side == EnumAppSide.Client) ? "." : "/");
	}

	[MemberNotNull("mods")]
	private void ensureModsLoaded()
	{
		if (mods == null)
		{
			mods = ((api.ModLoader is ModLoader loader) ? loader.LoadModInfos() : new List<ModContainer>());
		}
	}

	public string? preConsoleCommand()
	{
		if (gameversions != null)
		{
			return null;
		}
		string error = null;
		modDbRequestAsync(HttpMethod.Get, "gameversions").ContinueWith(delegate(Task<CompletedArgs> t)
		{
			CompletedArgs result = t.Result;
			if (result.State == CompletionState.Good)
			{
				gameversions = parseResponse<GameVersionResponse>(result.Response, out string errorText);
				if (errorText != null)
				{
					error = errorText;
				}
				else if (gameversions != null)
				{
					loadVersionIds(gameversions);
				}
				else
				{
					error = "Bad moddb response - no game versions";
				}
			}
			else
			{
				error = ((result.StatusCode == 0) ? "Mod hub offline" : ("Bad ModDB response - " + result.Response));
			}
		});
		return error;
	}

	public void onInstallCommand(string modId, string forGameVersion, Action<string> onProgressUpdate)
	{
		int num = 1;
		List<string> list = new List<string>(num);
		CollectionsMarshal.SetCount(list, num);
		Span<string> span = CollectionsMarshal.AsSpan(list);
		int index = 0;
		span[index] = modId;
		FetchModInstallInformation(list, forGameVersion, ignoreRetractions: false).ContinueWith(delegate(Task<ModInstallInformationResponse> t)
		{
			ModInstallInformationResponse result = t.Result;
			if (result.Error != null)
			{
				onProgressUpdate(result.Error);
			}
			else if (result.Data != null)
			{
				ensureModsLoaded();
				string[] array = modId.Split('@', 2);
				string text = array[0];
				string text2 = ((array.Length > 1) ? array[1] : null);
				foreach (KeyValuePair<string, ModInstallInformation> datum in result.Data)
				{
					var (identifier, modInstallInformation2) = (KeyValuePair<string, ModInstallInformation>)(ref datum);
					if (modInstallInformation2.ErrorCode != 0)
					{
						onProgressUpdate(identifier + ": " + modInstallInformation2.FormatErrorCode());
					}
					else if (!string.IsNullOrEmpty(modInstallInformation2.RetractionReason))
					{
						onProgressUpdate(identifier + ": Release Retracted. Check log for details.");
						api.Logger.Warning("{0}: Cannot download retracted release: {1}", identifier, modInstallInformation2.RetractionReason);
					}
					else
					{
						string text4 = ((!(text == identifier)) ? modInstallInformation2.RecommendedUpgrade : text2);
						if (IsModBlocked(identifier, text4, out string reason))
						{
							onProgressUpdate(identifier + ": Blocked: " + reason);
						}
						else
						{
							switch (MaybeEnableMod(identifier, text4, deleteOlder: true).Item1)
							{
							case ModEnableOutcome.AlreadyEnabled:
								onProgressUpdate(identifier + ": Already enabled.");
								continue;
							case ModEnableOutcome.NewlyEnabled:
								onProgressUpdate(identifier + ": Enabled.");
								continue;
							case ModEnableOutcome.InstalledIsNewer:
								onProgressUpdate(identifier + ": Newer version already installed.");
								continue;
							case ModEnableOutcome.DeletedOlder:
								onProgressUpdate(identifier + ": Old version deleted. Installing new one ...");
								break;
							}
							if (modInstallInformation2.FileUrl == null)
							{
								onProgressUpdate(identifier + ": Error: no url.");
							}
							else
							{
								DownloadModAsync(modInstallInformation2.FileUrl, modInstallInformation2.FileName, delegate(long progress, long totalSize)
								{
									string text5 = Lang.Get((totalSize == 0L) ? "downloading-progress-1" : "downloading-progress-2", progress, totalSize);
									onProgressUpdate(identifier + ": " + text5);
								}).ContinueWith(delegate(Task<Exception?> task)
								{
									Exception result2 = task.Result;
									if (result2 != null)
									{
										onProgressUpdate(identifier + ": " + result2.Message);
									}
									else
									{
										onProgressUpdate(identifier + ": " + Lang.Get("OK!"));
									}
								});
							}
						}
					}
				}
			}
		});
	}

	public void onRemoveCommand(string modId, Action<string> onProgressUpdate)
	{
		ensureModsLoaded();
		foreach (ModContainer val in mods)
		{
			if (val.Status != ModStatus.Errored && val.Info.ModID == modId)
			{
				File.Delete(val.SourcePath);
				onProgressUpdate("modutil-modremoved");
				return;
			}
		}
		onProgressUpdate("No such mod found.");
	}

	public void onListCommand(Action<string> onProgressUpdate)
	{
		ensureModsLoaded();
		List<string> modIds = new List<string>();
		foreach (ModContainer val in mods)
		{
			if (val.Status != ModStatus.Errored && val.Info.ModID != "game" && val.Info.ModID != "creative" && val.Info.ModID != "survival")
			{
				modIds.Add(val.Info.ModID);
			}
		}
		if (modIds.Count == 0)
		{
			onProgressUpdate(Lang.Get("modutil-list-none"));
			return;
		}
		onProgressUpdate(Lang.Get("modutil-list", modIds.Count, string.Join(", ", modIds)));
	}

	public void onSearchForCommand(string version, string modId, Action<string> onProgressUpdate)
	{
		ensureModsLoaded();
		if (gameversions == null)
		{
			onProgressUpdate("Game versions not (yet) loaded.");
			return;
		}
		int versionId = -1;
		GameVersionEntry[] gameVersions = gameversions.GameVersions;
		foreach (GameVersionEntry val in gameVersions)
		{
			if (val.Name == "v" + version)
			{
				versionId = val.TagId;
			}
		}
		if (versionId <= 0)
		{
			onProgressUpdate("No such version is listed on the moddb");
			return;
		}
		search(modId, onProgressUpdate, new int[1] { versionId });
	}

	public void onSearchForAndCompatibleCommand(string version, string modId, Action<string> onProgressUpdate)
	{
		ensureModsLoaded();
		if (gameversions == null)
		{
			onProgressUpdate("Game versions not (yet) loaded.");
			return;
		}
		string[] array = version.Split('.');
		string major = array[0];
		string minor = array[1];
		string minorVersionPrefix = "v" + major + "." + minor;
		List<int> sameMinVersionIds = new List<int>();
		GameVersionEntry[] gameVersions = gameversions.GameVersions;
		foreach (GameVersionEntry val in gameVersions)
		{
			if (val.Name.StartsWithOrdinal(minorVersionPrefix))
			{
				sameMinVersionIds.Add(val.TagId);
			}
		}
		search(modId, onProgressUpdate, sameMinVersionIds.ToArray());
	}

	public void onSearchCommand(string modId, Action<string> onProgressUpdate)
	{
		ensureModsLoaded();
		search(modId, onProgressUpdate, new int[1] { selfGameVersionId });
	}

	public void onSearchCompatibleCommand(string modId, Action<string> onProgressUpdate)
	{
		ensureModsLoaded();
		search(modId, onProgressUpdate, sameMinorVersionIds);
	}

	public async Task<ModInstallInformationResponse> FetchModInstallInformation(List<string> modSpecs, string forGameVersion, bool ignoreRetractions, CancellationToken cancellationToken = default(CancellationToken))
	{
		StringBuilder sb = new StringBuilder("v2/mods/install-information?gv=");
		sb.Append(forGameVersion);
		if (api is ICoreServerAPI sApi && sApi.Server.Config.HostedMode)
		{
			sb.Append("&hosted-mode=1");
		}
		if (ignoreRetractions)
		{
			sb.Append("&ignore-retractions=1");
		}
		sb.Append("&ids=").AppendJoin(',', modSpecs);
		CompletedArgs response = await modDbRequestAsync(HttpMethod.Get, sb.ToString(), null, cancellationToken);
		if (response.Response == null || response.StatusCode < 200 || response.StatusCode >= 400)
		{
			return new ModInstallInformationResponse
			{
				Error = ((response.State == CompletionState.Timeout) ? Lang.Get("Mod hub offline") : response.ErrorMessage)
			};
		}
		return JsonConvert.DeserializeObject<ModInstallInformationResponse>(response.Response);
	}

	public (ModEnableOutcome, string? installedVersion) MaybeEnableMod(string modIdentifier, string? desiredVersion, bool deleteOlder)
	{
		ensureModsLoaded();
		foreach (ModContainer mod in mods)
		{
			if (mod.Info.ModID != modIdentifier)
			{
				continue;
			}
			if (mod.Info.Version == desiredVersion || desiredVersion == null)
			{
				if (mod.Enabled)
				{
					return (ModEnableOutcome.AlreadyEnabled, installedVersion: mod.Info.Version);
				}
				List<string> disabledMods = ClientSettings.DisabledMods;
				disabledMods.Remove(mod.Info.ModID + "@" + mod.Info.Version);
				ClientSettings.DisabledMods = disabledMods;
				ClientSettings.Inst.Save(force: true);
				return (ModEnableOutcome.NewlyEnabled, installedVersion: mod.Info.Version);
			}
			if (GameVersion.IsNewerVersionThan(mod.Info.Version, desiredVersion))
			{
				return (ModEnableOutcome.InstalledIsNewer, installedVersion: mod.Info.Version);
			}
			if (deleteOlder)
			{
				File.Delete(mod.SourcePath);
				return (ModEnableOutcome.DeletedOlder, installedVersion: mod.Info.Version);
			}
			return (ModEnableOutcome.InstalledIsOlder, installedVersion: mod.Info.Version);
		}
		return (ModEnableOutcome.NotFound, installedVersion: null);
	}

	public async Task<Exception?> DownloadModAsync(string fileUrl, string fileName, DownloadProgressDelegate progressCallback, CancellationToken cancellationToken = default(CancellationToken))
	{
		GamePaths.EnsurePathExists(installPath);
		string filePath = Path.Combine(installPath, fileName);
		try
		{
			using Stream networkStream = await VSWebClient.Inst.GetStreamAsync(new Uri(modDbUrl, fileUrl));
			using FileStream fileStream = new FileStream(filePath, FileMode.Create);
			long sizeRead = 0L;
			long sizeTotal = (networkStream.CanSeek ? networkStream.Length : 0);
			if (sizeTotal > 0)
			{
				progressCallback(0L, sizeTotal);
			}
			int bufferSize = 81920;
			if (networkStream.CanSeek)
			{
				long length = networkStream.Length;
				long position = networkStream.Position;
				if (length <= position)
				{
					bufferSize = 1;
				}
				else
				{
					long remaining = length - position;
					if (remaining > 0)
					{
						bufferSize = (int)Math.Min(bufferSize, remaining);
					}
				}
			}
			byte[] buffer = ArrayPool<byte>.Shared.Rent(bufferSize);
			try
			{
				while (true)
				{
					int num;
					int bytesRead = (num = await networkStream.ReadAsync(new Memory<byte>(buffer), cancellationToken).ConfigureAwait(continueOnCapturedContext: false));
					if (num == 0)
					{
						break;
					}
					await fileStream.WriteAsync(new ReadOnlyMemory<byte>(buffer, 0, bytesRead), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
					sizeRead += bytesRead;
					progressCallback(sizeRead, sizeTotal);
				}
			}
			finally
			{
				ArrayPool<byte>.Shared.Return(buffer);
			}
			return null;
		}
		catch (Exception result)
		{
			return result;
		}
	}

	private void loadVersionIds(GameVersionResponse gameVersions)
	{
		List<int> sameMinorVersionIds = new List<int>();
		string[] array = "1.22.6".Split('.');
		string major = array[0];
		string minor = array[1];
		string versionExact = "v1.22.6";
		string versionMinor = "v" + major + "." + minor;
		GameVersionEntry[] gameVersions2 = gameVersions.GameVersions;
		foreach (GameVersionEntry val in gameVersions2)
		{
			if (val.Name == versionExact)
			{
				selfGameVersionId = val.TagId;
			}
			if (val.Name.StartsWithOrdinal(versionMinor))
			{
				sameMinorVersionIds.Add(val.TagId);
			}
		}
		this.sameMinorVersionIds = sameMinorVersionIds.ToArray();
	}

	private void search(string searchText, Action<string> onDone, int[] gameVersionIds)
	{
		if (searchText == null)
		{
			onDone("Syntax: " + commandPrefix + "moddb search [text]");
			return;
		}
		Search(searchText, delegate(ModSearchResult searchResult)
		{
			if (searchResult.Mods == null)
			{
				onDone(searchResult.StatusMessage);
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (searchResult.Mods.Length == 0)
				{
					stringBuilder.AppendLine(Lang.Get("Found no mods compatible for your game version"));
				}
				else
				{
					stringBuilder.AppendLine(Lang.Get("Found {0} compatible mods. Name and mod id:", searchResult.Mods.Length));
				}
				int num = 0;
				ModDbEntrySearchResponse[] array = searchResult.Mods;
				foreach (ModDbEntrySearchResponse modDbEntrySearchResponse in array)
				{
					stringBuilder.AppendLine(Lang.Get("{0}: <strong>{1}</strong>", modDbEntrySearchResponse.Name, modDbEntrySearchResponse.ModIdStrs[0]));
					num++;
					if (num > 10)
					{
						stringBuilder.AppendLine("and more...");
						break;
					}
				}
				onDone(stringBuilder.ToString());
			}
		}, gameVersionIds);
	}

	public void Search(string searchText, Action<ModSearchResult> onDone, int[] gameVersionIds, string? mv = null, string? sortBy = null, int limit = 100)
	{
		StringBuilder urlBuilder = new StringBuilder("mods?");
		urlBuilder.Append("text=").Append(Uri.EscapeDataString(searchText).Replace("%20", "+"));
		foreach (int tagId in gameVersionIds)
		{
			if (tagId != -1)
			{
				urlBuilder.Append("&gv[]=").Append(tagId);
			}
		}
		if (mv != null)
		{
			urlBuilder.Append("&mv=").Append(mv);
		}
		if (sortBy != null)
		{
			urlBuilder.Append("&sortBy=").Append(sortBy);
		}
		urlBuilder.Append("&limit=").Append(limit);
		modDbRequestAsync(HttpMethod.Get, urlBuilder.ToString()).ContinueWith(delegate(Task<CompletedArgs> res)
		{
			string response = res.Result.Response;
			switch (res.Result.State)
			{
			case CompletionState.Good:
			{
				string errorText;
				ModSearchResult modSearchResult = parseResponse<ModSearchResult>(response, out errorText);
				if (errorText != null)
				{
					onDone(new ModSearchResult
					{
						StatusCode = 500,
						StatusMessage = errorText
					});
				}
				else
				{
					modSearchResult.Mods = modSearchResult.Mods.Where((ModDbEntrySearchResponse m) => m.Type.Equals("mod")).ToArray();
					onDone(modSearchResult);
				}
				break;
			}
			case CompletionState.Timeout:
				onDone(new ModSearchResult
				{
					StatusCode = 500,
					StatusMessage = Lang.Get("Mod hub offline")
				});
				break;
			default:
				onDone(new ModSearchResult
				{
					StatusCode = 500,
					StatusMessage = Lang.Get("Bad moddb response - {0}", response)
				});
				break;
			}
		});
	}

	private Task<CompletedArgs> modDbRequestAsync(HttpMethod method, string action, FormUrlEncodedContent? postData = null, CancellationToken cancellationToken = default(CancellationToken))
	{
		api.Logger.Notification("Send request: {0}", action);
		return VSWebClient.Inst.SendAsyncSafe(new HttpRequestMessage(method, new Uri(modDbApiUrl, action))
		{
			Content = postData
		}, HttpCompletionOption.ResponseContentRead, cancellationToken);
	}

	public T? parseResponse<T>(string text, out string? errorText) where T : ModDbResponse
	{
		errorText = null;
		T response;
		try
		{
			response = JsonConvert.DeserializeObject<T>(text);
		}
		catch (Exception ex)
		{
			api.Logger.Notification("{0}", ex);
			errorText = LoggerBase.CleanStackTrace(ex.ToString());
			return null;
		}
		if (response == null || response.StatusCode != 200)
		{
			errorText = "Invalid request - " + response?.StatusCode;
		}
		return response;
	}

	public static async Task LoadBlockedModsAsync(ILogger logger, bool disabled = false)
	{
		if (ModBlockList != null)
		{
			return;
		}
		if (disabled)
		{
			ModBlockList = new Dictionary<string, string>();
			ModBlockListMRE.Set();
			return;
		}
		for (int tries = 0; tries < 3; tries++)
		{
			if (tries > 0)
			{
				logger.Notification("Trying again to get blocked mods list in {0}ms ...", tries * 100);
				await Task.Delay(tries * 100);
			}
			try
			{
				ModBlockList = JsonConvert.DeserializeObject<ModBlock[]>(await VSWebClient.Inst.GetStringAsync("https://cdn.vintagestory.at/api/blockedmods.json")).ToDictionary((ModBlock b) => b.Id, (ModBlock b) => b.reason);
				ModBlockListMRE.Set();
				return;
			}
			catch (Exception ex)
			{
				logger.Warning("Could not get blocked mods from api: {0}", ex.Message);
			}
		}
		ModBlockList = new Dictionary<string, string>();
		ModBlockListMRE.Set();
	}

	public static bool IsModBlocked(string modId, string? version, [NotNullWhen(true)] out string? reason)
	{
		if (ModBlockList == null)
		{
			reason = "Mod blocklist not yet loaded.";
			return true;
		}
		if (version != null && ModBlockList.TryGetValue(modId + "@" + version, out reason))
		{
			return true;
		}
		return ModBlockList.TryGetValue(modId ?? "", out reason);
	}
}
