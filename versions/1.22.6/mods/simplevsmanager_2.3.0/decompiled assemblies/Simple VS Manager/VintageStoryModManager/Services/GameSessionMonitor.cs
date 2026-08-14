using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000063 RID: 99
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GameSessionMonitor : IDisposable
	{
		// Token: 0x06000973 RID: 2419 RVA: 0x000395C4 File Offset: 0x000377C4
		public GameSessionMonitor(string logsDirectory, Dispatcher dispatcher, UserConfigurationService configuration, Func<IReadOnlyList<ModUsageTrackingEntry>> activeModProvider)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(logsDirectory, "logsDirectory");
			if (dispatcher == null)
			{
				throw new ArgumentNullException("dispatcher");
			}
			this._dispatcher = dispatcher;
			if (configuration == null)
			{
				throw new ArgumentNullException("configuration");
			}
			this._configuration = configuration;
			if (activeModProvider == null)
			{
				throw new ArgumentNullException("activeModProvider");
			}
			this._activeModProvider = activeModProvider;
			this._logsDirectory = Path.GetFullPath(logsDirectory);
			try
			{
				Directory.CreateDirectory(this._logsDirectory);
				this.InitializeWatchers();
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is ArgumentException)
			{
				StatusLogService.AppendStatus(string.Format(CultureInfo.CurrentCulture, "Failed to initialize log monitor: {0}", ex.Message), true);
			}
			this._lastPromptState = this._configuration.HasPendingModUsagePrompt;
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x000396EC File Offset: 0x000378EC
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
			FileSystemWatcher clientWatcher = this._clientWatcher;
			if (clientWatcher != null)
			{
				clientWatcher.Dispose();
			}
			this._clientWatcher = null;
			FileSystemWatcher serverWatcher = this._serverWatcher;
			if (serverWatcher != null)
			{
				serverWatcher.Dispose();
			}
			this._serverWatcher = null;
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000975 RID: 2421 RVA: 0x0003973C File Offset: 0x0003793C
		// (remove) Token: 0x06000976 RID: 2422 RVA: 0x00039774 File Offset: 0x00037974
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event EventHandler PromptRequired;

		// Token: 0x06000977 RID: 2423 RVA: 0x000397A9 File Offset: 0x000379A9
		public void RefreshPromptState()
		{
			this._lastPromptState = this._configuration.HasPendingModUsagePrompt;
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x000397BC File Offset: 0x000379BC
		private void InitializeWatchers()
		{
			this._clientWatcher = this.CreateWatcher("client-main*");
			if (this._clientWatcher != null)
			{
				this._clientWatcher.Changed += delegate(object _, FileSystemEventArgs e)
				{
					this.HandleLogEvent(e.FullPath, GameSessionMonitor.LogCategory.Client);
				};
				this._clientWatcher.Created += delegate(object _, FileSystemEventArgs e)
				{
					this.HandleLogEvent(e.FullPath, GameSessionMonitor.LogCategory.Client);
				};
				this._clientWatcher.Renamed += delegate(object _, RenamedEventArgs e)
				{
					this.HandleLogEvent(e.FullPath, GameSessionMonitor.LogCategory.Client);
				};
				this._clientWatcher.Renamed += delegate(object _, RenamedEventArgs e)
				{
					this.RemoveLogState(e.OldFullPath);
				};
				this._clientWatcher.Deleted += delegate(object _, FileSystemEventArgs e)
				{
					this.RemoveLogState(e.FullPath);
				};
				this._clientWatcher.EnableRaisingEvents = true;
				this.InitializeExistingLogs(GameSessionMonitor.LogCategory.Client);
			}
			this._serverWatcher = this.CreateWatcher("server-main*");
			if (this._serverWatcher != null)
			{
				this._serverWatcher.Changed += delegate(object _, FileSystemEventArgs e)
				{
					this.HandleLogEvent(e.FullPath, GameSessionMonitor.LogCategory.Server);
				};
				this._serverWatcher.Created += delegate(object _, FileSystemEventArgs e)
				{
					this.HandleLogEvent(e.FullPath, GameSessionMonitor.LogCategory.Server);
				};
				this._serverWatcher.Renamed += delegate(object _, RenamedEventArgs e)
				{
					this.HandleLogEvent(e.FullPath, GameSessionMonitor.LogCategory.Server);
				};
				this._serverWatcher.Renamed += delegate(object _, RenamedEventArgs e)
				{
					this.RemoveLogState(e.OldFullPath);
				};
				this._serverWatcher.Deleted += delegate(object _, FileSystemEventArgs e)
				{
					this.RemoveLogState(e.FullPath);
				};
				this._serverWatcher.EnableRaisingEvents = true;
				this.InitializeExistingLogs(GameSessionMonitor.LogCategory.Server);
			}
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x00039910 File Offset: 0x00037B10
		[return: Nullable(2)]
		private FileSystemWatcher CreateWatcher(string filter)
		{
			FileSystemWatcher result;
			try
			{
				result = new FileSystemWatcher(this._logsDirectory, filter)
				{
					NotifyFilter = 81,
					IncludeSubdirectories = false,
					EnableRaisingEvents = false
				};
			}
			catch (ArgumentException)
			{
				result = null;
			}
			catch (IOException)
			{
				result = null;
			}
			catch (UnauthorizedAccessException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x0003997C File Offset: 0x00037B7C
		private void InitializeExistingLogs(GameSessionMonitor.LogCategory category)
		{
			try
			{
				foreach (string path in this.EnumerateCategoryFiles(category))
				{
					this.EnsureLogState(path);
				}
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
			{
				StatusLogService.AppendStatus(string.Format(CultureInfo.CurrentCulture, "Failed to enumerate existing logs: {0}", ex.Message), true);
			}
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x00039A20 File Offset: 0x00037C20
		private IEnumerable<string> EnumerateCategoryFiles(GameSessionMonitor.LogCategory category)
		{
			GameSessionMonitor.<EnumerateCategoryFiles>d__29 <EnumerateCategoryFiles>d__ = new GameSessionMonitor.<EnumerateCategoryFiles>d__29(-2);
			<EnumerateCategoryFiles>d__.<>4__this = this;
			<EnumerateCategoryFiles>d__.<>3__category = category;
			return <EnumerateCategoryFiles>d__;
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x00039A38 File Offset: 0x00037C38
		[NullableContext(2)]
		private void HandleLogEvent(string path, GameSessionMonitor.LogCategory category)
		{
			GameSessionMonitor.<>c__DisplayClass30_0 CS$<>8__locals1 = new GameSessionMonitor.<>c__DisplayClass30_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.category = category;
			if (this._disposed || string.IsNullOrWhiteSpace(path))
			{
				return;
			}
			try
			{
				CS$<>8__locals1.fullPath = Path.GetFullPath(path);
			}
			catch (Exception obj) when (path != null)
			{
				return;
			}
			if (!GameSessionMonitor.IsSupportedLog(CS$<>8__locals1.fullPath))
			{
				return;
			}
			this._inFlightProcessing.GetOrAdd(CS$<>8__locals1.fullPath, delegate(string key)
			{
				Func<Task> func;
				if ((func = CS$<>8__locals1.<>9__1) == null)
				{
					func = (CS$<>8__locals1.<>9__1 = delegate()
					{
						GameSessionMonitor.<>c__DisplayClass30_0.<<HandleLogEvent>b__1>d <<HandleLogEvent>b__1>d;
						<<HandleLogEvent>b__1>d.<>t__builder = AsyncTaskMethodBuilder.Create();
						<<HandleLogEvent>b__1>d.<>4__this = CS$<>8__locals1;
						<<HandleLogEvent>b__1>d.<>1__state = -1;
						<<HandleLogEvent>b__1>d.<>t__builder.Start<GameSessionMonitor.<>c__DisplayClass30_0.<<HandleLogEvent>b__1>d>(ref <<HandleLogEvent>b__1>d);
						return <<HandleLogEvent>b__1>d.<>t__builder.Task;
					});
				}
				return Task.Run(func);
			});
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x00039AD0 File Offset: 0x00037CD0
		[NullableContext(2)]
		private void RemoveLogState(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return;
			}
			GameSessionMonitor.LogTailState logTailState;
			this._logStates.TryRemove(Path.GetFullPath(path), ref logTailState);
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x00039AFC File Offset: 0x00037CFC
		private Task ProcessLogChangesAsync(GameSessionMonitor.LogCategory category, string path)
		{
			GameSessionMonitor.<ProcessLogChangesAsync>d__32 <ProcessLogChangesAsync>d__;
			<ProcessLogChangesAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<ProcessLogChangesAsync>d__.<>4__this = this;
			<ProcessLogChangesAsync>d__.category = category;
			<ProcessLogChangesAsync>d__.path = path;
			<ProcessLogChangesAsync>d__.<>1__state = -1;
			<ProcessLogChangesAsync>d__.<>t__builder.Start<GameSessionMonitor.<ProcessLogChangesAsync>d__32>(ref <ProcessLogChangesAsync>d__);
			return <ProcessLogChangesAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x00039B50 File Offset: 0x00037D50
		private static bool IsSupportedLog(string path)
		{
			string extension = Path.GetExtension(path);
			if (!extension.Equals(".log", 5) && !extension.Equals(".txt", 5) && !extension.Equals(".json", 5))
			{
				return false;
			}
			string fileName = Path.GetFileName(path);
			return fileName.Contains("client-main", 5) || fileName.Contains("server-main", 5);
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x00039BB4 File Offset: 0x00037DB4
		private GameSessionMonitor.LogTailState EnsureLogState(string path)
		{
			string fullPath = Path.GetFullPath(path);
			return this._logStates.GetOrAdd(fullPath, new Func<string, GameSessionMonitor.LogTailState>(this.CreateInitialLogState));
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x00039BE0 File Offset: 0x00037DE0
		private GameSessionMonitor.LogTailState CreateInitialLogState(string path)
		{
			GameSessionMonitor.LogTailState result;
			try
			{
				FileInfo fileInfo = new FileInfo(path);
				result = new GameSessionMonitor.LogTailState(fileInfo.Exists ? fileInfo.Length : 0L);
			}
			catch (IOException)
			{
				result = new GameSessionMonitor.LogTailState(0L);
			}
			catch (UnauthorizedAccessException)
			{
				result = new GameSessionMonitor.LogTailState(0L);
			}
			return result;
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x00039C44 File Offset: 0x00037E44
		[return: Nullable(2)]
		private static string TryReadFirstLine(string path)
		{
			try
			{
				using (FileStream fileStream = new FileStream(path, 3, 1, 3))
				{
					using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8, true))
					{
						while (!streamReader.EndOfStream)
						{
							string text = streamReader.ReadLine();
							if (text == null)
							{
								break;
							}
							if (text.Length != 0)
							{
								return text;
							}
						}
					}
				}
			}
			catch (IOException)
			{
				return null;
			}
			catch (UnauthorizedAccessException)
			{
				return null;
			}
			return null;
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x00039CE4 File Offset: 0x00037EE4
		private static bool TryExtractTimestamp(string line, out DateTimeOffset timestamp)
		{
			timestamp = default(DateTimeOffset);
			if (string.IsNullOrWhiteSpace(line))
			{
				return false;
			}
			string text = line.Trim();
			if (text.Length == 0)
			{
				return false;
			}
			if (text.StartsWith("{", 4))
			{
				try
				{
					using (JsonDocument jsonDocument = JsonDocument.Parse(text, default(JsonDocumentOptions)))
					{
						if (GameSessionMonitor.TryGetJsonTimestamp(jsonDocument.RootElement, out timestamp))
						{
							return true;
						}
					}
				}
				catch (JsonException)
				{
				}
			}
			Match match = GameSessionMonitor.VintageStoryTimestampRegex.Match(text);
			if (match.Success)
			{
				try
				{
					int num = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
					int num2 = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
					int num3 = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);
					int num4 = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);
					int num5 = int.Parse(match.Groups[5].Value, CultureInfo.InvariantCulture);
					int num6 = int.Parse(match.Groups[6].Value, CultureInfo.InvariantCulture);
					DateTime dateTime;
					dateTime..ctor(num3, num2, num, num4, num5, num6, 0);
					TimeSpan utcOffset = TimeZoneInfo.Local.GetUtcOffset(dateTime);
					timestamp = new DateTimeOffset(dateTime, utcOffset);
					return true;
				}
				catch (ArgumentOutOfRangeException)
				{
				}
				catch (FormatException)
				{
				}
			}
			Match match2 = GameSessionMonitor.Iso8601TimestampRegex.Match(text);
			return match2.Success && DateTimeOffset.TryParse(match2.Value, CultureInfo.InvariantCulture, 128, ref timestamp);
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x00039EC0 File Offset: 0x000380C0
		private static bool TryGetJsonTimestamp(JsonElement element, out DateTimeOffset timestamp)
		{
			string text;
			if ((GameSessionMonitor.TryReadJsonString(element, "@t", out text) || GameSessionMonitor.TryReadJsonString(element, "time", out text) || GameSessionMonitor.TryReadJsonString(element, "timestamp", out text) || GameSessionMonitor.TryReadJsonString(element, "Time", out text) || GameSessionMonitor.TryReadJsonString(element, "Timestamp", out text)) && !string.IsNullOrEmpty(text) && DateTimeOffset.TryParse(text, CultureInfo.InvariantCulture, 128, ref timestamp))
			{
				return true;
			}
			timestamp = default(DateTimeOffset);
			return false;
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x00039F40 File Offset: 0x00038140
		private static bool TryReadJsonString(JsonElement element, string propertyName, [Nullable(2)] out string value)
		{
			JsonElement jsonElement;
			if (element.ValueKind == 1 && element.TryGetProperty(propertyName, ref jsonElement) && jsonElement.ValueKind == 3)
			{
				value = jsonElement.GetString();
				return !string.IsNullOrEmpty(value);
			}
			value = null;
			return false;
		}

		// Token: 0x06000986 RID: 2438 RVA: 0x00039F88 File Offset: 0x00038188
		private Task<bool> HandleLogLineAsync(GameSessionMonitor.LogCategory category, string line, GameSessionMonitor.LogTailState state)
		{
			GameSessionMonitor.<HandleLogLineAsync>d__40 <HandleLogLineAsync>d__;
			<HandleLogLineAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<HandleLogLineAsync>d__.<>4__this = this;
			<HandleLogLineAsync>d__.category = category;
			<HandleLogLineAsync>d__.line = line;
			<HandleLogLineAsync>d__.state = state;
			<HandleLogLineAsync>d__.<>1__state = -1;
			<HandleLogLineAsync>d__.<>t__builder.Start<GameSessionMonitor.<HandleLogLineAsync>d__40>(ref <HandleLogLineAsync>d__);
			return <HandleLogLineAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000987 RID: 2439 RVA: 0x00039FE4 File Offset: 0x000381E4
		private static bool ContainsMarker(string line, IReadOnlyList<string> markers)
		{
			foreach (string text in markers)
			{
				if (line.IndexOf(text, 5) >= 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000988 RID: 2440 RVA: 0x0003A038 File Offset: 0x00038238
		private Task StartSessionAsync(DateTimeOffset? logTimestamp)
		{
			GameSessionMonitor.<StartSessionAsync>d__42 <StartSessionAsync>d__;
			<StartSessionAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<StartSessionAsync>d__.<>4__this = this;
			<StartSessionAsync>d__.logTimestamp = logTimestamp;
			<StartSessionAsync>d__.<>1__state = -1;
			<StartSessionAsync>d__.<>t__builder.Start<GameSessionMonitor.<StartSessionAsync>d__42>(ref <StartSessionAsync>d__);
			return <StartSessionAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000989 RID: 2441 RVA: 0x0003A084 File Offset: 0x00038284
		private Task<bool> CompleteSessionAsync(string source, DateTimeOffset? logTimestamp)
		{
			GameSessionMonitor.<CompleteSessionAsync>d__43 <CompleteSessionAsync>d__;
			<CompleteSessionAsync>d__.<>t__builder = AsyncTaskMethodBuilder<bool>.Create();
			<CompleteSessionAsync>d__.<>4__this = this;
			<CompleteSessionAsync>d__.source = source;
			<CompleteSessionAsync>d__.logTimestamp = logTimestamp;
			<CompleteSessionAsync>d__.<>1__state = -1;
			<CompleteSessionAsync>d__.<>t__builder.Start<GameSessionMonitor.<CompleteSessionAsync>d__43>(ref <CompleteSessionAsync>d__);
			return <CompleteSessionAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x0003A0D8 File Offset: 0x000382D8
		private static List<ModUsageTrackingEntry> NormalizeEntries([Nullable(2)] IReadOnlyList<ModUsageTrackingEntry> entries)
		{
			if (entries == null || entries.Count == 0)
			{
				return new List<ModUsageTrackingEntry>();
			}
			HashSet<ModUsageTrackingKey> hashSet = new HashSet<ModUsageTrackingKey>();
			List<ModUsageTrackingEntry> list = new List<ModUsageTrackingEntry>(entries.Count);
			foreach (ModUsageTrackingEntry modUsageTrackingEntry in entries)
			{
				if (!string.IsNullOrEmpty(modUsageTrackingEntry.ModId) && !string.IsNullOrEmpty(modUsageTrackingEntry.ModVersion) && !string.IsNullOrEmpty(modUsageTrackingEntry.GameVersion) && modUsageTrackingEntry.CanSubmitVote && !modUsageTrackingEntry.HasUserVote)
				{
					ModUsageTrackingKey modUsageTrackingKey = new ModUsageTrackingKey(modUsageTrackingEntry.ModId, modUsageTrackingEntry.ModVersion, modUsageTrackingEntry.GameVersion);
					if (hashSet.Add(modUsageTrackingKey))
					{
						list.Add(new ModUsageTrackingEntry(modUsageTrackingEntry.ModId, modUsageTrackingEntry.ModVersion, modUsageTrackingEntry.GameVersion, modUsageTrackingEntry.CanSubmitVote, modUsageTrackingEntry.HasUserVote));
					}
				}
			}
			return list;
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x0003A2DA File Offset: 0x000384DA
		[CompilerGenerated]
		private IEnumerable<string> <EnumerateCategoryFiles>g__Enumerate|29_0(string pattern)
		{
			return Directory.EnumerateFiles(this._logsDirectory, pattern, 0);
		}

		// Token: 0x0400047F RID: 1151
		private static readonly TimeSpan MinimumSessionDuration = DevConfig.MinimumSessionDuration;

		// Token: 0x04000480 RID: 1152
		private static readonly Regex VintageStoryTimestampRegex = new Regex("^(\\d{1,2})\\.(\\d{1,2})\\.(\\d{4})\\s+(\\d{1,2}):(\\d{2}):(\\d{2})", 8);

		// Token: 0x04000481 RID: 1153
		private static readonly Regex Iso8601TimestampRegex = new Regex("\\d{4}-\\d{2}-\\d{2}T\\d{2}:\\d{2}:\\d{2}(?:\\.\\d+)?(?:Z|[+-]\\d{2}:\\d{2})", 8);

		// Token: 0x04000482 RID: 1154
		private static readonly string[] ClientStartMarkers = new string[]
		{
			"Received level finalize"
		};

		// Token: 0x04000483 RID: 1155
		private static readonly string[] ClientEndMarkers = new string[]
		{
			"Exiting current game"
		};

		// Token: 0x04000484 RID: 1156
		private static readonly string[] ServerEndMarkers = new string[]
		{
			"server shutting down"
		};

		// Token: 0x04000485 RID: 1157
		private readonly Func<IReadOnlyList<ModUsageTrackingEntry>> _activeModProvider;

		// Token: 0x04000486 RID: 1158
		private readonly UserConfigurationService _configuration;

		// Token: 0x04000487 RID: 1159
		private readonly Dispatcher _dispatcher;

		// Token: 0x04000488 RID: 1160
		private readonly string _logsDirectory;

		// Token: 0x04000489 RID: 1161
		private readonly ConcurrentDictionary<string, Task> _inFlightProcessing = new ConcurrentDictionary<string, Task>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400048A RID: 1162
		private readonly ConcurrentDictionary<string, GameSessionMonitor.LogTailState> _logStates = new ConcurrentDictionary<string, GameSessionMonitor.LogTailState>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x0400048B RID: 1163
		private readonly object _sessionLock = new object();

		// Token: 0x0400048C RID: 1164
		[Nullable(2)]
		private FileSystemWatcher _clientWatcher;

		// Token: 0x0400048D RID: 1165
		private bool _disposed;

		// Token: 0x0400048E RID: 1166
		private bool _lastPromptState;

		// Token: 0x0400048F RID: 1167
		[Nullable(2)]
		private FileSystemWatcher _serverWatcher;

		// Token: 0x04000490 RID: 1168
		private bool _sessionActive;

		// Token: 0x04000491 RID: 1169
		private List<ModUsageTrackingEntry> _sessionModEntries = new List<ModUsageTrackingEntry>();

		// Token: 0x04000492 RID: 1170
		private DateTimeOffset _sessionStartUtc;

		// Token: 0x02000280 RID: 640
		[NullableContext(0)]
		private enum LogCategory
		{
			// Token: 0x04000F49 RID: 3913
			Client,
			// Token: 0x04000F4A RID: 3914
			Server
		}

		// Token: 0x02000281 RID: 641
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class LogTailState
		{
			// Token: 0x06001626 RID: 5670 RVA: 0x000803FF File Offset: 0x0007E5FF
			public LogTailState(long initialPosition)
			{
				this.Position = ((initialPosition < 0L) ? 0L : initialPosition);
			}

			// Token: 0x1700049C RID: 1180
			// (get) Token: 0x06001627 RID: 5671 RVA: 0x00080422 File Offset: 0x0007E622
			// (set) Token: 0x06001628 RID: 5672 RVA: 0x0008042A File Offset: 0x0007E62A
			public long Position { get; set; }

			// Token: 0x1700049D RID: 1181
			// (get) Token: 0x06001629 RID: 5673 RVA: 0x00080433 File Offset: 0x0007E633
			[Nullable(1)]
			public object SyncRoot { [NullableContext(1)] get; } = new object();

			// Token: 0x1700049E RID: 1182
			// (get) Token: 0x0600162A RID: 5674 RVA: 0x0008043B File Offset: 0x0007E63B
			// (set) Token: 0x0600162B RID: 5675 RVA: 0x00080443 File Offset: 0x0007E643
			public string FirstLineReference { get; set; }

			// Token: 0x1700049F RID: 1183
			// (get) Token: 0x0600162C RID: 5676 RVA: 0x0008044C File Offset: 0x0007E64C
			// (set) Token: 0x0600162D RID: 5677 RVA: 0x00080454 File Offset: 0x0007E654
			public bool HasProcessedReference { get; set; }
		}
	}
}
