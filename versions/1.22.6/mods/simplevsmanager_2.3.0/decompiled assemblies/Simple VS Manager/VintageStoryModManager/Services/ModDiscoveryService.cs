using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Resources;
using VintageStoryModManager.Models;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000071 RID: 113
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModDiscoveryService
	{
		// Token: 0x06000A50 RID: 2640 RVA: 0x0003FC08 File Offset: 0x0003DE08
		public ModDiscoveryService(ClientSettingsStore settingsStore)
		{
			if (settingsStore == null)
			{
				throw new ArgumentNullException("settingsStore");
			}
			this._settingsStore = settingsStore;
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x0003FC58 File Offset: 0x0003DE58
		public IReadOnlyList<ModEntry> LoadMods()
		{
			List<ValueTuple<int, FileSystemInfo>> list = this.CollectModSources();
			if (list.Count == 0)
			{
				return Array.Empty<ModEntry>();
			}
			int maxDegreeOfParallelism = Math.Clamp(Environment.ProcessorCount, 1, 16);
			ConcurrentBag<ValueTuple<int, ModEntry>> collected = new ConcurrentBag<ValueTuple<int, ModEntry>>();
			ParallelOptions parallelOptions = new ParallelOptions
			{
				MaxDegreeOfParallelism = maxDegreeOfParallelism
			};
			Parallel.ForEach<ValueTuple<int, FileSystemInfo>[]>(Enumerable.Chunk<ValueTuple<int, FileSystemInfo>>(list, ModDiscoveryService.DiscoveryBatchSize), parallelOptions, delegate([TupleElementNames(new string[]
			{
				"Order",
				"Entry"
			})] ValueTuple<int, FileSystemInfo>[] batch)
			{
				foreach (ValueTuple<int, FileSystemInfo> valueTuple in batch)
				{
					int item = valueTuple.Item1;
					FileSystemInfo item2 = valueTuple.Item2;
					ModEntry modEntry = this.ProcessEntry(item2);
					if (modEntry != null)
					{
						collected.Add(new ValueTuple<int, ModEntry>(item, modEntry));
					}
				}
			});
			List<ModEntry> list2 = Enumerable.ToList<ModEntry>(Enumerable.Select<ValueTuple<int, ModEntry>, ModEntry>(Enumerable.OrderBy<ValueTuple<int, ModEntry>, int>(collected, ([TupleElementNames(new string[]
			{
				"Order",
				"Entry"
			})] ValueTuple<int, ModEntry> tuple) => tuple.Item1), ([TupleElementNames(new string[]
			{
				"Order",
				"Entry"
			})] ValueTuple<int, ModEntry> tuple) => tuple.Item2));
			this.ApplyLoadStatuses(list2);
			return list2;
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x0003FD2A File Offset: 0x0003DF2A
		[AsyncIteratorStateMachine(typeof(ModDiscoveryService.<LoadModsIncrementallyAsync>d__14))]
		public IAsyncEnumerable<IReadOnlyList<ModEntry>> LoadModsIncrementallyAsync(int batchSize, [EnumeratorCancellation] CancellationToken cancellationToken = default(CancellationToken))
		{
			ModDiscoveryService.<LoadModsIncrementallyAsync>d__14 <LoadModsIncrementallyAsync>d__ = new ModDiscoveryService.<LoadModsIncrementallyAsync>d__14(-2);
			<LoadModsIncrementallyAsync>d__.<>4__this = this;
			<LoadModsIncrementallyAsync>d__.<>3__batchSize = batchSize;
			<LoadModsIncrementallyAsync>d__.<>3__cancellationToken = cancellationToken;
			return <LoadModsIncrementallyAsync>d__;
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x0003FD48 File Offset: 0x0003DF48
		public IReadOnlyList<string> GetSearchPaths()
		{
			return Enumerable.ToArray<string>(this.BuildSearchPaths());
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x0003FD58 File Offset: 0x0003DF58
		[return: Nullable(2)]
		public ModEntry LoadModFromPath(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return null;
			}
			string fullPath;
			try
			{
				fullPath = Path.GetFullPath(path);
			}
			catch (Exception)
			{
				return null;
			}
			ModEntry modEntry = null;
			if (Directory.Exists(fullPath))
			{
				modEntry = this.TryLoadFromDirectory(new DirectoryInfo(fullPath));
			}
			else if (File.Exists(fullPath))
			{
				FileInfo fileInfo = new FileInfo(fullPath);
				if (string.Equals(fileInfo.Extension, ".zip", 5))
				{
					modEntry = this.TryLoadFromZip(fileInfo);
				}
				else if (string.Equals(fileInfo.Extension, ".cs", 5) || string.Equals(fileInfo.Extension, ".dll", 5))
				{
					modEntry = this.CreateUnsupportedCodeModEntry(fileInfo);
				}
			}
			if (modEntry != null && ModDiscoveryService.IsBaseGameMod(modEntry.ModId))
			{
				return null;
			}
			return modEntry;
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x0003FE1C File Offset: 0x0003E01C
		[return: TupleElementNames(new string[]
		{
			"Order",
			"Entry"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		private List<ValueTuple<int, FileSystemInfo>> CollectModSources()
		{
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			List<ValueTuple<int, FileSystemInfo>> list = new List<ValueTuple<int, FileSystemInfo>>();
			int num = 0;
			foreach (string text in this.BuildSearchPaths())
			{
				if (Directory.Exists(text) && !ModDiscoveryService.IsDefaultGameModsDirectory(text))
				{
					IEnumerable<FileSystemInfo> enumerable;
					try
					{
						enumerable = new DirectoryInfo(text).EnumerateFileSystemInfos();
					}
					catch (Exception)
					{
						continue;
					}
					foreach (FileSystemInfo fileSystemInfo in enumerable)
					{
						if (hashSet.Add(fileSystemInfo.FullName))
						{
							list.Add(new ValueTuple<int, FileSystemInfo>(num++, fileSystemInfo));
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x0003FF04 File Offset: 0x0003E104
		[return: Nullable(2)]
		private ModEntry ProcessEntry(FileSystemInfo entry)
		{
			DirectoryInfo directoryInfo = entry as DirectoryInfo;
			ModEntry modEntry;
			if (directoryInfo == null)
			{
				FileInfo fileInfo = entry as FileInfo;
				if (fileInfo != null)
				{
					if (string.Equals(fileInfo.Extension, ".zip", 5))
					{
						modEntry = this.TryLoadFromZip(fileInfo);
						goto IL_7A;
					}
					FileInfo fileInfo2 = fileInfo;
					if (string.Equals(fileInfo2.Extension, ".cs", 5) || string.Equals(fileInfo2.Extension, ".dll", 5))
					{
						modEntry = this.CreateUnsupportedCodeModEntry(fileInfo2);
						goto IL_7A;
					}
				}
				modEntry = null;
			}
			else
			{
				modEntry = this.TryLoadFromDirectory(directoryInfo);
			}
			IL_7A:
			ModEntry modEntry2 = modEntry;
			if (modEntry2 != null && ModDiscoveryService.IsBaseGameMod(modEntry2.ModId))
			{
				return null;
			}
			return modEntry2;
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x0003FFA1 File Offset: 0x0003E1A1
		private static bool IsBaseGameMod(string modId)
		{
			return !string.IsNullOrWhiteSpace(modId) && (string.Equals(modId, "VSCreativeMod", 5) || string.Equals(modId, "VSEssentials", 5) || string.Equals(modId, "VSSurvivalMod", 5));
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x0003FFD7 File Offset: 0x0003E1D7
		public void ApplyLoadStatuses(IList<ModEntry> mods)
		{
			this.ApplyLoadStatusesInternal(mods, mods, null);
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x0003FFE3 File Offset: 0x0003E1E3
		public IReadOnlyCollection<ModEntry> ApplyLoadStatusesIncremental(IList<ModEntry> mods, [Nullable(new byte[]
		{
			2,
			1
		})] ICollection<ModEntry> changedMods, [Nullable(new byte[]
		{
			2,
			1
		})] ICollection<string> removedModIds = null)
		{
			return this.ApplyLoadStatusesInternal(mods, changedMods, removedModIds);
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x0003FFF0 File Offset: 0x0003E1F0
		private IReadOnlyCollection<ModEntry> ApplyLoadStatusesInternal(IList<ModEntry> mods, [Nullable(new byte[]
		{
			2,
			1
		})] ICollection<ModEntry> changedMods, [Nullable(new byte[]
		{
			2,
			1
		})] ICollection<string> removedModIds)
		{
			if (mods == null || mods.Count == 0)
			{
				return Array.Empty<ModEntry>();
			}
			if ((changedMods == null || changedMods.Count <= 0) && (removedModIds == null || removedModIds.Count <= 0))
			{
				changedMods = mods;
			}
			Dictionary<string, List<ModEntry>> dictionary = new Dictionary<string, List<ModEntry>>(StringComparer.OrdinalIgnoreCase);
			Dictionary<string, HashSet<ModEntry>> dictionary2 = new Dictionary<string, HashSet<ModEntry>>(StringComparer.OrdinalIgnoreCase);
			foreach (ModEntry modEntry in mods)
			{
				if (!string.IsNullOrWhiteSpace(modEntry.ModId))
				{
					List<ModEntry> list;
					if (!dictionary.TryGetValue(modEntry.ModId, ref list))
					{
						list = new List<ModEntry>();
						dictionary[modEntry.ModId] = list;
					}
					list.Add(modEntry);
				}
				if (modEntry.Dependencies.Count != 0)
				{
					foreach (ModDependencyInfo modDependencyInfo in modEntry.Dependencies)
					{
						if (!modDependencyInfo.IsGameOrCoreDependency && !string.IsNullOrWhiteSpace(modDependencyInfo.ModId))
						{
							HashSet<ModEntry> hashSet;
							if (!dictionary2.TryGetValue(modDependencyInfo.ModId, ref hashSet))
							{
								hashSet = new HashSet<ModEntry>();
								dictionary2[modDependencyInfo.ModId] = hashSet;
							}
							hashSet.Add(modEntry);
						}
					}
				}
			}
			if (dictionary.Count == 0)
			{
				return Array.Empty<ModEntry>();
			}
			Dictionary<string, ModEntry> dictionary3 = new Dictionary<string, ModEntry>(StringComparer.OrdinalIgnoreCase);
			foreach (KeyValuePair<string, List<ModEntry>> keyValuePair in dictionary)
			{
				ModEntry modEntry2 = null;
				foreach (ModEntry modEntry3 in keyValuePair.Value)
				{
					if (modEntry2 == null && !modEntry3.HasErrors)
					{
						modEntry2 = modEntry3;
					}
				}
				if (modEntry2 == null)
				{
					modEntry2 = keyValuePair.Value[0];
				}
				dictionary3[keyValuePair.Key] = modEntry2;
			}
			HashSet<ModEntry> hashSet2 = new HashSet<ModEntry>();
			if (changedMods != null)
			{
				foreach (ModEntry modEntry4 in changedMods)
				{
					List<ModEntry> list2;
					if (modEntry4 != null && !string.IsNullOrWhiteSpace(modEntry4.ModId) && dictionary.TryGetValue(modEntry4.ModId, ref list2))
					{
						foreach (ModEntry modEntry5 in list2)
						{
							hashSet2.Add(modEntry5);
						}
					}
				}
			}
			if (removedModIds != null && removedModIds.Count > 0)
			{
				foreach (string text in removedModIds)
				{
					List<ModEntry> list3;
					if (!string.IsNullOrWhiteSpace(text) && dictionary.TryGetValue(text, ref list3))
					{
						foreach (ModEntry modEntry6 in list3)
						{
							hashSet2.Add(modEntry6);
						}
					}
				}
			}
			Queue<string> queue = new Queue<string>();
			HashSet<string> hashSet3 = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			if (changedMods != null)
			{
				foreach (ModEntry modEntry7 in changedMods)
				{
					if (!string.IsNullOrWhiteSpace((modEntry7 != null) ? modEntry7.ModId : null))
					{
						queue.Enqueue(modEntry7.ModId);
					}
				}
			}
			if (removedModIds == null || removedModIds.Count <= 0)
			{
				goto IL_46F;
			}
			using (IEnumerator<string> enumerator5 = removedModIds.GetEnumerator())
			{
				while (enumerator5.MoveNext())
				{
					string text2 = enumerator5.Current;
					if (!string.IsNullOrWhiteSpace(text2))
					{
						queue.Enqueue(text2);
					}
				}
				goto IL_46F;
			}
			IL_3AF:
			string text3 = queue.Dequeue();
			HashSet<ModEntry> hashSet4;
			if (hashSet3.Add(text3) && dictionary2.TryGetValue(text3, ref hashSet4))
			{
				foreach (ModEntry modEntry8 in hashSet4)
				{
					if (hashSet2.Add(modEntry8) && !string.IsNullOrWhiteSpace(modEntry8.ModId))
					{
						queue.Enqueue(modEntry8.ModId);
						List<ModEntry> list4;
						if (dictionary.TryGetValue(modEntry8.ModId, ref list4))
						{
							foreach (ModEntry modEntry9 in list4)
							{
								hashSet2.Add(modEntry9);
							}
						}
					}
				}
			}
			IL_46F:
			if (queue.Count > 0)
			{
				goto IL_3AF;
			}
			if (hashSet2.Count == 0)
			{
				return Array.Empty<ModEntry>();
			}
			foreach (ModEntry modEntry10 in hashSet2)
			{
				modEntry10.LoadError = null;
				modEntry10.DependencyHasErrors = false;
				modEntry10.MissingDependencies = Array.Empty<ModDependencyInfo>();
			}
			foreach (KeyValuePair<string, List<ModEntry>> keyValuePair2 in dictionary)
			{
				ModEntry modEntry11 = dictionary3[keyValuePair2.Key];
				foreach (ModEntry modEntry12 in keyValuePair2.Value)
				{
					if (hashSet2.Contains(modEntry12) && modEntry12 != modEntry11)
					{
						modEntry12.LoadError = ModDiscoveryService.GeneralLoadErrorMessage;
					}
				}
			}
			foreach (ModEntry modEntry13 in hashSet2)
			{
				if (!modEntry13.HasErrors && modEntry13.Dependencies.Count != 0)
				{
					bool flag = false;
					List<ModDependencyInfo> list5 = null;
					foreach (ModDependencyInfo modDependencyInfo2 in modEntry13.Dependencies)
					{
						if (!modDependencyInfo2.IsGameOrCoreDependency)
						{
							ModEntry modEntry14;
							if (!dictionary3.TryGetValue(modDependencyInfo2.ModId, ref modEntry14))
							{
								if (list5 == null)
								{
									list5 = new List<ModDependencyInfo>();
								}
								list5.Add(modDependencyInfo2);
							}
							else
							{
								if (modEntry14.HasErrors || modEntry14.HasLoadError || this._settingsStore.IsDisabled(modEntry14.ModId, modEntry14.Version))
								{
									flag = true;
									break;
								}
								if (!VersionStringUtility.SatisfiesMinimumVersion(modDependencyInfo2.Version, modEntry14.Version))
								{
									if (list5 == null)
									{
										list5 = new List<ModDependencyInfo>();
									}
									list5.Add(modDependencyInfo2);
								}
							}
						}
					}
					if (flag)
					{
						modEntry13.DependencyHasErrors = true;
						modEntry13.LoadError = ModDiscoveryService.DependencyErrorMessage;
					}
					else if (list5 != null && list5.Count > 0)
					{
						ModDependencyInfo[] array = list5.ToArray();
						modEntry13.MissingDependencies = array;
						modEntry13.LoadError = ModDiscoveryService.BuildMissingDependencyMessage(array);
					}
				}
			}
			return Enumerable.ToArray<ModEntry>(hashSet2);
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x00040858 File Offset: 0x0003EA58
		private static string BuildMissingDependencyMessage(IReadOnlyList<ModDependencyInfo> dependencies)
		{
			if (dependencies.Count == 1)
			{
				return "Unable to load mod. Requires dependency " + ModDiscoveryService.<BuildMissingDependencyMessage>g__Format|23_0(dependencies[0]);
			}
			string text = "Unable to load mod. Requires dependencies ";
			string text2 = ", ";
			Func<ModDependencyInfo, string> func;
			if ((func = ModDiscoveryService.<>O.<0>__Format) == null)
			{
				func = (ModDiscoveryService.<>O.<0>__Format = new Func<ModDependencyInfo, string>(ModDiscoveryService.<BuildMissingDependencyMessage>g__Format|23_0));
			}
			return text + string.Join(text2, Enumerable.Select<ModDependencyInfo, string>(dependencies, func));
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x000408BC File Offset: 0x0003EABC
		public string GetModsStateFingerprint()
		{
			string result;
			using (SHA256 sha = SHA256.Create())
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (string text in this.BuildSearchPaths())
				{
					stringBuilder.AppendLine(text);
					if (!Directory.Exists(text))
					{
						stringBuilder.AppendLine("<missing>");
					}
					else
					{
						try
						{
							foreach (FileSystemInfo entry in Enumerable.OrderBy<FileSystemInfo, string>(new DirectoryInfo(text).EnumerateFileSystemInfos(), (FileSystemInfo info) => info.Name, StringComparer.OrdinalIgnoreCase))
							{
								ModDiscoveryService.AppendEntrySignature(stringBuilder, entry);
							}
						}
						catch (Exception ex)
						{
							stringBuilder.Append("<error>|").Append(ex.GetType().FullName).Append('|').Append(ex.Message).AppendLine();
						}
					}
				}
				result = Convert.ToHexString(sha.ComputeHash(Encoding.UTF8.GetBytes(stringBuilder.ToString())));
			}
			return result;
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x00040A58 File Offset: 0x0003EC58
		private static void AppendEntrySignature(StringBuilder builder, FileSystemInfo entry)
		{
			builder.Append(entry.Name).Append('|').Append((entry is DirectoryInfo) ? 'D' : 'F').Append('|').Append(entry.LastWriteTimeUtc.Ticks.ToString(CultureInfo.InvariantCulture));
			FileInfo fileInfo = entry as FileInfo;
			if (fileInfo != null)
			{
				builder.Append('|').Append(fileInfo.Length.ToString(CultureInfo.InvariantCulture));
			}
			builder.AppendLine();
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x00040AE8 File Offset: 0x0003ECE8
		private IEnumerable<string> BuildSearchPaths()
		{
			ModDiscoveryService.<>c__DisplayClass26_0 CS$<>8__locals1;
			CS$<>8__locals1.ordered = new List<string>();
			CS$<>8__locals1.seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			ModDiscoveryService.<BuildSearchPaths>g__AddPath|26_0(Path.Combine(this._settingsStore.DataDirectory, "Mods"), ref CS$<>8__locals1);
			ModDiscoveryService.<BuildSearchPaths>g__AddPath|26_0(Path.Combine(this._settingsStore.DataDirectory, "ModsByServer"), ref CS$<>8__locals1);
			foreach (string rawPath in this._settingsStore.ModPaths)
			{
				foreach (string candidate in this.ResolvePathCandidates(rawPath))
				{
					ModDiscoveryService.<BuildSearchPaths>g__AddPath|26_0(candidate, ref CS$<>8__locals1);
				}
			}
			foreach (string candidate2 in this.LoadPathsFromLog())
			{
				ModDiscoveryService.<BuildSearchPaths>g__AddPath|26_0(candidate2, ref CS$<>8__locals1);
			}
			return CS$<>8__locals1.ordered;
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x00040C08 File Offset: 0x0003EE08
		private static bool IsDefaultGameModsDirectory(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return false;
			}
			bool result;
			try
			{
				string text = Path.GetFullPath(path).Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar).TrimEnd(Path.DirectorySeparatorChar);
				char directorySeparatorChar = Path.DirectorySeparatorChar;
				ReadOnlySpan<char> readOnlySpan = new ReadOnlySpan<char>(ref directorySeparatorChar);
				ReadOnlySpan<char> readOnlySpan2 = "Vintagestory";
				char directorySeparatorChar2 = Path.DirectorySeparatorChar;
				string text2 = readOnlySpan + readOnlySpan2 + new ReadOnlySpan<char>(ref directorySeparatorChar2) + "Mods";
				result = text.Contains(text2, 5);
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x00040C94 File Offset: 0x0003EE94
		private IEnumerable<string> LoadPathsFromLog()
		{
			ModDiscoveryService.<LoadPathsFromLog>d__28 <LoadPathsFromLog>d__ = new ModDiscoveryService.<LoadPathsFromLog>d__28(-2);
			<LoadPathsFromLog>d__.<>4__this = this;
			return <LoadPathsFromLog>d__;
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x00040CA4 File Offset: 0x0003EEA4
		private static string ExtractLogMessage(string line)
		{
			if (string.IsNullOrWhiteSpace(line))
			{
				return string.Empty;
			}
			int num = line.IndexOf(']');
			if (num >= 0 && num + 1 < line.Length)
			{
				return line.Substring(num + 1).TrimStart();
			}
			return line.TrimStart();
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x00040CEC File Offset: 0x0003EEEC
		private static bool LooksLikePath(string text)
		{
			return !string.IsNullOrWhiteSpace(text) && (text.IndexOfAny(ModDiscoveryService.PathSeparators) >= 0 || (text.Length >= 2 && char.IsLetter(text.get_Chars(0)) && text.get_Chars(1) == ':'));
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x00040D38 File Offset: 0x0003EF38
		private IEnumerable<string> ResolvePathCandidates(string rawPath)
		{
			ModDiscoveryService.<ResolvePathCandidates>d__31 <ResolvePathCandidates>d__ = new ModDiscoveryService.<ResolvePathCandidates>d__31(-2);
			<ResolvePathCandidates>d__.<>4__this = this;
			<ResolvePathCandidates>d__.<>3__rawPath = rawPath;
			return <ResolvePathCandidates>d__;
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x00040D50 File Offset: 0x0003EF50
		[return: Nullable(2)]
		private ModEntry TryLoadFromDirectory(DirectoryInfo directory)
		{
			string text = Path.Combine(directory.FullName, "modinfo.json");
			if (!File.Exists(text))
			{
				this.InvalidateDirectoryCache(directory.FullName);
				return null;
			}
			DateTime manifestLastWriteUtc;
			long manifestLength;
			try
			{
				FileInfo fileInfo = new FileInfo(text);
				manifestLastWriteUtc = fileInfo.LastWriteTimeUtc;
				manifestLength = fileInfo.Length;
			}
			catch (Exception)
			{
				manifestLastWriteUtc = DateTime.MinValue;
				manifestLength = -1L;
			}
			ModDiscoveryService.CachedDirectoryEntry cachedDirectoryEntry;
			if (this.TryGetCachedDirectoryEntry(directory.FullName, manifestLastWriteUtc, manifestLength, out cachedDirectoryEntry))
			{
				byte[] iconBytes = cachedDirectoryEntry.IconBytes ?? this.LoadDefaultIcon();
				return this.CreateEntry(cachedDirectoryEntry.Info, directory.FullName, ModSourceKind.Folder, iconBytes);
			}
			ModEntry result;
			try
			{
				using (JsonDocument jsonDocument = JsonDocument.Parse(File.ReadAllText(text), ModDiscoveryService.DocumentOptions))
				{
					ModDiscoveryService.RawModInfo rawModInfo = ModDiscoveryService.ParseModInfo(jsonDocument.RootElement, directory.Name);
					ModDiscoveryService.DirectoryIconCache directoryIconCache = this.LoadDirectoryIcon(directory, rawModInfo.IconPath);
					byte[] array = directoryIconCache.Bytes;
					if (array == null)
					{
						array = this.LoadDefaultIcon();
					}
					this.CacheDirectoryEntry(directory.FullName, rawModInfo, manifestLastWriteUtc, manifestLength, directoryIconCache);
					result = this.CreateEntry(rawModInfo, directory.FullName, ModSourceKind.Folder, array);
				}
			}
			catch (Exception ex)
			{
				this.InvalidateDirectoryCache(directory.FullName);
				result = this.CreateErrorEntry(directory.Name, directory.FullName, ModSourceKind.Folder, ex.Message);
			}
			return result;
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x00040EB8 File Offset: 0x0003F0B8
		[return: Nullable(2)]
		private ModEntry TryLoadFromZip(FileInfo archiveFile)
		{
			DateTime lastWriteTimeUtc = DateTime.MinValue;
			long length = 0L;
			try
			{
				lastWriteTimeUtc = archiveFile.LastWriteTimeUtc;
				length = archiveFile.Length;
			}
			catch (Exception)
			{
			}
			string text;
			byte[] array;
			if (ModManifestCacheService.TryGetManifest(archiveFile.FullName, lastWriteTimeUtc, length, out text, out array))
			{
				try
				{
					using (JsonDocument jsonDocument = JsonDocument.Parse(text, ModDiscoveryService.DocumentOptions))
					{
						ModDiscoveryService.RawModInfo info = ModDiscoveryService.ParseModInfo(jsonDocument.RootElement, Path.GetFileNameWithoutExtension(archiveFile.Name));
						byte[] array2 = array;
						if (array2 == null)
						{
							array2 = this.LoadDefaultIcon();
						}
						return this.CreateEntry(info, archiveFile.FullName, ModSourceKind.ZipArchive, array2);
					}
				}
				catch (Exception)
				{
					ModManifestCacheService.Invalidate(archiveFile.FullName);
				}
			}
			ModEntry result;
			try
			{
				using (ZipArchive zipArchive = ZipFile.OpenRead(archiveFile.FullName))
				{
					ModDiscoveryService.ZipArchiveLookup lookup = ModDiscoveryService.BuildArchiveLookup(zipArchive);
					ZipArchiveEntry zipArchiveEntry = ModDiscoveryService.FindEntry(lookup, "modinfo.json");
					if (zipArchiveEntry == null)
					{
						result = this.CreateErrorEntry(Path.GetFileNameWithoutExtension(archiveFile.Name), archiveFile.FullName, ModSourceKind.ZipArchive, "Missing modinfo.json");
					}
					else
					{
						string text2;
						using (Stream stream = zipArchiveEntry.Open())
						{
							using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8, true))
							{
								text2 = streamReader.ReadToEnd();
							}
						}
						using (JsonDocument jsonDocument2 = JsonDocument.Parse(text2, ModDiscoveryService.DocumentOptions))
						{
							ModDiscoveryService.RawModInfo rawModInfo = ModDiscoveryService.ParseModInfo(jsonDocument2.RootElement, Path.GetFileNameWithoutExtension(archiveFile.Name));
							byte[] array3 = ModDiscoveryService.LoadIconFromArchive(lookup, rawModInfo.IconPath);
							byte[] array4 = array3;
							if (array4 == null)
							{
								array4 = this.LoadDefaultIcon();
							}
							ModManifestCacheService.StoreManifest(archiveFile.FullName, lastWriteTimeUtc, length, rawModInfo.ModId, rawModInfo.Version, text2, array3);
							result = this.CreateEntry(rawModInfo, archiveFile.FullName, ModSourceKind.ZipArchive, array4);
						}
					}
				}
			}
			catch (Exception ex)
			{
				result = this.CreateErrorEntry(Path.GetFileNameWithoutExtension(archiveFile.Name), archiveFile.FullName, ModSourceKind.ZipArchive, ex.Message);
			}
			return result;
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x00041164 File Offset: 0x0003F364
		private static ModDiscoveryService.ZipArchiveLookup BuildArchiveLookup(ZipArchive archive)
		{
			Dictionary<string, ZipArchiveEntry> dictionary = new Dictionary<string, ZipArchiveEntry>(StringComparer.OrdinalIgnoreCase);
			Dictionary<string, ZipArchiveEntry> dictionary2 = new Dictionary<string, ZipArchiveEntry>(StringComparer.OrdinalIgnoreCase);
			foreach (ZipArchiveEntry zipArchiveEntry in archive.Entries)
			{
				string text = ModDiscoveryService.NormalizeZipEntryPath(zipArchiveEntry.FullName);
				if (!dictionary.ContainsKey(text))
				{
					dictionary[text] = zipArchiveEntry;
				}
				string text2 = ModDiscoveryService.NormalizeZipEntryPath(Path.GetFileName(zipArchiveEntry.FullName));
				if (!string.IsNullOrEmpty(text2) && !dictionary2.ContainsKey(text2))
				{
					dictionary2[text2] = zipArchiveEntry;
				}
			}
			return new ModDiscoveryService.ZipArchiveLookup(dictionary, dictionary2);
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x00041218 File Offset: 0x0003F418
		[return: Nullable(2)]
		private static ZipArchiveEntry FindEntry(ModDiscoveryService.ZipArchiveLookup lookup, string entryName)
		{
			if (string.IsNullOrWhiteSpace(entryName))
			{
				return null;
			}
			string normalizedPath = ModDiscoveryService.NormalizeZipEntryPath(entryName);
			ZipArchiveEntry result;
			if (lookup.TryGetEntry(normalizedPath, out result))
			{
				return result;
			}
			string normalizedFileName = ModDiscoveryService.NormalizeZipEntryPath(Path.GetFileName(entryName));
			ZipArchiveEntry result2;
			if (lookup.TryGetEntryByFileName(normalizedFileName, out result2))
			{
				return result2;
			}
			return null;
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x00041260 File Offset: 0x0003F460
		private ModEntry CreateUnsupportedCodeModEntry(FileInfo file)
		{
			string modId = ModDiscoveryService.ToModId(Path.GetFileNameWithoutExtension(file.Name));
			return new ModEntry
			{
				ModId = modId,
				Name = Path.GetFileNameWithoutExtension(file.Name),
				ManifestName = null,
				Version = null,
				NetworkVersion = null,
				Description = "This code mod is not packaged with a modinfo.json file. Pack the mod into a zip or folder with modinfo.json so it can be managed.",
				Website = null,
				Authors = Array.Empty<string>(),
				Contributors = Array.Empty<string>(),
				Dependencies = Array.Empty<ModDependencyInfo>(),
				SourcePath = file.FullName,
				SourceKind = (string.Equals(file.Extension, ".cs", 5) ? ModSourceKind.SourceCode : ModSourceKind.Assembly),
				IconBytes = null,
				Error = "Metadata unavailable for code-only mods.",
				LoadError = null,
				Side = null,
				RequiredOnClient = default(bool?),
				RequiredOnServer = default(bool?)
			};
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x0004134C File Offset: 0x0003F54C
		private ModEntry CreateEntry(ModDiscoveryService.RawModInfo info, string sourcePath, ModSourceKind kind, [Nullable(2)] byte[] iconBytes)
		{
			return new ModEntry
			{
				ModId = info.ModId,
				Name = info.Name,
				ManifestName = info.ManifestName,
				Version = info.Version,
				NetworkVersion = info.NetworkVersion,
				Description = info.Description,
				Website = info.Website,
				Authors = info.Authors,
				Contributors = info.Contributors,
				Dependencies = info.Dependencies,
				SourcePath = sourcePath,
				SourceKind = kind,
				IconBytes = iconBytes,
				Error = null,
				LoadError = null,
				Side = info.Side,
				RequiredOnClient = info.RequiredOnClient,
				RequiredOnServer = info.RequiredOnServer
			};
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x00041420 File Offset: 0x0003F620
		private ModEntry CreateErrorEntry(string hintName, string sourcePath, ModSourceKind kind, string message)
		{
			string name = string.IsNullOrWhiteSpace(hintName) ? Path.GetFileName(sourcePath) : hintName;
			return new ModEntry
			{
				ModId = ModDiscoveryService.ToModId(name),
				Name = name,
				ManifestName = null,
				Version = null,
				NetworkVersion = null,
				Description = null,
				Website = null,
				Authors = Array.Empty<string>(),
				Contributors = Array.Empty<string>(),
				Dependencies = Array.Empty<ModDependencyInfo>(),
				SourcePath = sourcePath,
				SourceKind = kind,
				IconBytes = null,
				Error = message,
				LoadError = null,
				Side = null,
				RequiredOnClient = default(bool?),
				RequiredOnServer = default(bool?)
			};
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x000414E4 File Offset: 0x0003F6E4
		private static ModDiscoveryService.RawModInfo ParseModInfo(JsonElement root, string fallbackName)
		{
			string text;
			if ((text = ModDiscoveryService.GetString(root, "modid")) == null)
			{
				text = (ModDiscoveryService.GetString(root, "modID") ?? string.Empty);
			}
			string text2 = text;
			fallbackName = (string.IsNullOrWhiteSpace(fallbackName) ? string.Empty : fallbackName.Trim());
			string text3 = ModDiscoveryService.GetString(root, "name");
			text3 = (string.IsNullOrWhiteSpace(text3) ? null : text3.Trim());
			string text4 = text3 ?? fallbackName;
			if (string.IsNullOrWhiteSpace(text2))
			{
				text2 = ModDiscoveryService.ToModId(text4);
			}
			string text5 = ModDiscoveryService.GetString(root, "version");
			if (text5 == null)
			{
				text5 = ModDiscoveryService.TryResolveVersionFromMap(root);
			}
			IReadOnlyList<string> stringList = ModDiscoveryService.GetStringList(root, "authors");
			IReadOnlyList<string> stringList2 = ModDiscoveryService.GetStringList(root, "contributors");
			IReadOnlyList<ModDependencyInfo> dependencies = ModDiscoveryService.GetDependencies(root);
			string name = string.IsNullOrWhiteSpace(text4) ? text2 : text4;
			return new ModDiscoveryService.RawModInfo
			{
				ModId = text2,
				Name = name,
				ManifestName = text3,
				Version = (string.IsNullOrWhiteSpace(text5) ? null : text5),
				NetworkVersion = (ModDiscoveryService.GetString(root, "networkversion") ?? ModDiscoveryService.GetString(root, "networkVersion")),
				Description = ModDiscoveryService.GetString(root, "description"),
				Website = ModDiscoveryService.GetString(root, "website"),
				Authors = stringList,
				Contributors = stringList2,
				Dependencies = dependencies,
				IconPath = (ModDiscoveryService.GetString(root, "iconpath") ?? ModDiscoveryService.GetString(root, "iconPath")),
				Side = ModDiscoveryService.GetString(root, "side"),
				RequiredOnClient = ModDiscoveryService.GetNullableBool(root, "requiredonclient"),
				RequiredOnServer = ModDiscoveryService.GetNullableBool(root, "requiredonserver")
			};
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x00041680 File Offset: 0x0003F880
		private static IReadOnlyList<ModDependencyInfo> GetDependencies(JsonElement root)
		{
			JsonElement jsonElement;
			if (!ModDiscoveryService.TryGetProperty(root, "dependencies", out jsonElement) || jsonElement.ValueKind != 1)
			{
				return Array.Empty<ModDependencyInfo>();
			}
			List<ModDependencyInfo> list = new List<ModDependencyInfo>();
			foreach (JsonProperty jsonProperty in jsonElement.EnumerateObject())
			{
				string version = (jsonProperty.Value.ValueKind == 3) ? (jsonProperty.Value.GetString() ?? string.Empty) : string.Empty;
				list.Add(new ModDependencyInfo(jsonProperty.Name, version));
			}
			if (list.Count != 0)
			{
				return list.ToArray();
			}
			return Array.Empty<ModDependencyInfo>();
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x00041754 File Offset: 0x0003F954
		[NullableContext(2)]
		private static string TryResolveVersionFromMap(JsonElement root)
		{
			JsonElement jsonElement;
			if (!ModDiscoveryService.TryGetProperty(root, "versionmap", out jsonElement) && !ModDiscoveryService.TryGetProperty(root, "VersionMap", out jsonElement))
			{
				return null;
			}
			if (jsonElement.ValueKind != 1)
			{
				return null;
			}
			string text = null;
			string text2 = null;
			foreach (JsonProperty jsonProperty in jsonElement.EnumerateObject())
			{
				string @string = jsonProperty.Value.GetString();
				if (@string != null)
				{
					text2 = @string;
					if (jsonProperty.Name.Contains("1.21", 5))
					{
						text = @string;
					}
				}
			}
			return text ?? text2;
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x0004180C File Offset: 0x0003FA0C
		private ModDiscoveryService.DirectoryIconCache LoadDirectoryIcon(DirectoryInfo directory, [Nullable(2)] string iconPath)
		{
			string text = ModDiscoveryService.ResolveSafePath(directory.FullName, iconPath);
			if (text == null)
			{
				string text2 = Path.Combine(directory.FullName, "modicon.png");
				if (File.Exists(text2))
				{
					text = text2;
				}
			}
			if (text != null)
			{
				try
				{
					FileInfo fileInfo = new FileInfo(text);
					if (fileInfo.Exists)
					{
						byte[] bytes = File.ReadAllBytes(text);
						return new ModDiscoveryService.DirectoryIconCache
						{
							Bytes = bytes,
							Metadata = new ModDiscoveryService.DirectoryIconMetadata
							{
								IconPath = text,
								LastWriteTimeUtc = new DateTime?(fileInfo.LastWriteTimeUtc),
								Length = new long?(fileInfo.Length),
								WasMissing = false
							}
						};
					}
					return new ModDiscoveryService.DirectoryIconCache
					{
						Bytes = null,
						Metadata = new ModDiscoveryService.DirectoryIconMetadata
						{
							IconPath = text,
							WasMissing = true
						}
					};
				}
				catch (Exception)
				{
					return new ModDiscoveryService.DirectoryIconCache
					{
						Bytes = null,
						Metadata = new ModDiscoveryService.DirectoryIconMetadata
						{
							IconPath = text,
							WasMissing = true
						}
					};
				}
			}
			return new ModDiscoveryService.DirectoryIconCache
			{
				Bytes = null,
				Metadata = null
			};
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x00041924 File Offset: 0x0003FB24
		private bool TryGetCachedDirectoryEntry(string directoryPath, DateTime manifestLastWriteUtc, long manifestLength, out ModDiscoveryService.CachedDirectoryEntry entry)
		{
			object directoryCacheLock = this._directoryCacheLock;
			lock (directoryCacheLock)
			{
				ModDiscoveryService.CachedDirectoryEntry cachedDirectoryEntry;
				if (this._directoryManifestCache.TryGetValue(directoryPath, ref cachedDirectoryEntry))
				{
					if (cachedDirectoryEntry.ManifestLength == manifestLength && cachedDirectoryEntry.ManifestLastWriteTimeUtc == manifestLastWriteUtc && ModDiscoveryService.IconMetadataMatches(cachedDirectoryEntry.IconMetadata))
					{
						entry = cachedDirectoryEntry;
						return true;
					}
					this._directoryManifestCache.Remove(directoryPath);
				}
			}
			entry = null;
			return false;
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x000419B0 File Offset: 0x0003FBB0
		private void CacheDirectoryEntry(string directoryPath, ModDiscoveryService.RawModInfo info, DateTime manifestLastWriteUtc, long manifestLength, ModDiscoveryService.DirectoryIconCache icon)
		{
			ModDiscoveryService.CachedDirectoryEntry cachedDirectoryEntry = new ModDiscoveryService.CachedDirectoryEntry
			{
				Info = info,
				ManifestLastWriteTimeUtc = manifestLastWriteUtc,
				ManifestLength = manifestLength,
				IconBytes = icon.Bytes,
				IconMetadata = icon.Metadata
			};
			object directoryCacheLock = this._directoryCacheLock;
			lock (directoryCacheLock)
			{
				this._directoryManifestCache[directoryPath] = cachedDirectoryEntry;
			}
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x00041A30 File Offset: 0x0003FC30
		private void InvalidateDirectoryCache(string directoryPath)
		{
			object directoryCacheLock = this._directoryCacheLock;
			lock (directoryCacheLock)
			{
				this._directoryManifestCache.Remove(directoryPath);
			}
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00041A78 File Offset: 0x0003FC78
		[NullableContext(2)]
		private static bool IconMetadataMatches(ModDiscoveryService.DirectoryIconMetadata metadata)
		{
			if (metadata == null || string.IsNullOrWhiteSpace(metadata.IconPath))
			{
				return true;
			}
			bool result;
			try
			{
				FileInfo fileInfo = new FileInfo(metadata.IconPath);
				if (!fileInfo.Exists)
				{
					result = metadata.WasMissing;
				}
				else if (metadata.WasMissing)
				{
					result = false;
				}
				else
				{
					bool flag;
					if (fileInfo.LastWriteTimeUtc == metadata.LastWriteTimeUtc)
					{
						long length = fileInfo.Length;
						long? length2 = metadata.Length;
						flag = (length == length2.GetValueOrDefault() & length2 != null);
					}
					else
					{
						flag = false;
					}
					result = flag;
				}
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x00041B28 File Offset: 0x0003FD28
		[NullableContext(2)]
		private static byte[] LoadIconFromArchive([Nullable(1)] ModDiscoveryService.ZipArchiveLookup lookup, string iconPath)
		{
			ZipArchiveEntry zipArchiveEntry = null;
			if (!string.IsNullOrWhiteSpace(iconPath))
			{
				zipArchiveEntry = ModDiscoveryService.FindEntry(lookup, iconPath);
			}
			if (zipArchiveEntry == null)
			{
				zipArchiveEntry = ModDiscoveryService.FindEntry(lookup, "modicon.png");
			}
			if (zipArchiveEntry == null)
			{
				return null;
			}
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (Stream stream = zipArchiveEntry.Open())
				{
					stream.CopyTo(memoryStream);
					result = memoryStream.ToArray();
				}
			}
			return result;
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x00041BAC File Offset: 0x0003FDAC
		[NullableContext(2)]
		private byte[] LoadDefaultIcon()
		{
			object defaultIconLock = this._defaultIconLock;
			byte[] defaultIconBytes;
			lock (defaultIconLock)
			{
				if (!this._defaultIconResolved)
				{
					this._defaultIconBytes = (ModDiscoveryService.TryLoadBundledDefaultIcon() ?? ModDiscoveryService.TryLoadBundledIconFromBaseDirectory());
					if (this._defaultIconBytes == null)
					{
						foreach (string text in this.EnumerateDefaultIconCandidates())
						{
							try
							{
								if (File.Exists(text))
								{
									this._defaultIconBytes = File.ReadAllBytes(text);
									break;
								}
							}
							catch (IOException)
							{
							}
							catch (UnauthorizedAccessException)
							{
							}
							catch (NotSupportedException)
							{
							}
						}
					}
					this._defaultIconResolved = true;
				}
				defaultIconBytes = this._defaultIconBytes;
			}
			return defaultIconBytes;
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x00041C98 File Offset: 0x0003FE98
		[NullableContext(2)]
		private static byte[] TryLoadBundledDefaultIcon()
		{
			try
			{
				StreamResourceInfo resourceStream = Application.GetResourceStream(new Uri("Resources/mod-default.png", 2));
				if (((resourceStream != null) ? resourceStream.Stream : null) != null)
				{
					using (Stream stream = resourceStream.Stream)
					{
						using (MemoryStream memoryStream = new MemoryStream())
						{
							stream.CopyTo(memoryStream);
							return memoryStream.ToArray();
						}
					}
				}
			}
			catch (IOException)
			{
			}
			catch (NotSupportedException)
			{
			}
			catch (SecurityException)
			{
			}
			catch (InvalidOperationException)
			{
			}
			catch (UriFormatException)
			{
			}
			return null;
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x00041D64 File Offset: 0x0003FF64
		[NullableContext(2)]
		private static byte[] TryLoadBundledIconFromBaseDirectory()
		{
			string text = Path.Combine(AppContext.BaseDirectory, "mod-default.png");
			try
			{
				if (File.Exists(text))
				{
					return File.ReadAllBytes(text);
				}
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			catch (NotSupportedException)
			{
			}
			return null;
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x00041DC8 File Offset: 0x0003FFC8
		private IEnumerable<string> EnumerateDefaultIconCandidates()
		{
			ModDiscoveryService.<EnumerateDefaultIconCandidates>d__51 <EnumerateDefaultIconCandidates>d__ = new ModDiscoveryService.<EnumerateDefaultIconCandidates>d__51(-2);
			<EnumerateDefaultIconCandidates>d__.<>4__this = this;
			return <EnumerateDefaultIconCandidates>d__;
		}

		// Token: 0x06000A78 RID: 2680 RVA: 0x00041DD8 File Offset: 0x0003FFD8
		[NullableContext(2)]
		private static string ResolveSafePath([Nullable(1)] string baseDirectory, string relative)
		{
			if (string.IsNullOrWhiteSpace(relative))
			{
				return null;
			}
			try
			{
				string fullPath = Path.GetFullPath(Path.Combine(baseDirectory, relative));
				if (fullPath.StartsWith(baseDirectory, 5))
				{
					return fullPath;
				}
			}
			catch (Exception)
			{
			}
			return null;
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x00041E24 File Offset: 0x00040024
		private static string NormalizeZipEntryPath(string path)
		{
			string text = path.Trim().Replace('\\', '/');
			while (text.StartsWith("./", 4))
			{
				text = text.Substring(2);
			}
			if (text.Length > 0 && text.get_Chars(0) == '/')
			{
				text = text.Substring(1);
			}
			return text;
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x00041E78 File Offset: 0x00040078
		private static string ToModId([Nullable(2)] string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				return "mod";
			}
			StringBuilder stringBuilder = new StringBuilder(name.Length);
			for (int i = 0; i < name.Length; i++)
			{
				char c = name.get_Chars(i);
				if (char.IsLetter(c))
				{
					stringBuilder.Append(char.ToLowerInvariant(c));
				}
				else if (char.IsDigit(c))
				{
					if (stringBuilder.Length == 0)
					{
						stringBuilder.Append('m');
					}
					stringBuilder.Append(c);
				}
			}
			if (stringBuilder.Length != 0)
			{
				return stringBuilder.ToString();
			}
			return "mod";
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x00041F08 File Offset: 0x00040108
		[return: Nullable(2)]
		private static string GetString(JsonElement element, string propertyName)
		{
			JsonElement jsonElement;
			if (ModDiscoveryService.TryGetProperty(element, propertyName, out jsonElement) && jsonElement.ValueKind == 3)
			{
				return jsonElement.GetString();
			}
			return null;
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x00041F34 File Offset: 0x00040134
		private static bool? GetNullableBool(JsonElement element, string propertyName)
		{
			JsonElement jsonElement;
			if (ModDiscoveryService.TryGetProperty(element, propertyName, out jsonElement))
			{
				if (jsonElement.ValueKind == 5)
				{
					return new bool?(true);
				}
				if (jsonElement.ValueKind == 6)
				{
					return new bool?(false);
				}
			}
			return default(bool?);
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x00041F78 File Offset: 0x00040178
		private static IReadOnlyList<string> GetStringList(JsonElement element, string propertyName)
		{
			JsonElement jsonElement;
			if (!ModDiscoveryService.TryGetProperty(element, propertyName, out jsonElement) || jsonElement.ValueKind != 2)
			{
				return Array.Empty<string>();
			}
			List<string> list = new List<string>();
			foreach (JsonElement jsonElement2 in jsonElement.EnumerateArray())
			{
				if (jsonElement2.ValueKind == 3)
				{
					list.Add(jsonElement2.GetString() ?? string.Empty);
				}
			}
			if (list.Count != 0)
			{
				return list.ToArray();
			}
			return Array.Empty<string>();
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x00042020 File Offset: 0x00040220
		private static bool TryGetProperty(JsonElement element, string propertyName, out JsonElement value)
		{
			foreach (JsonProperty jsonProperty in element.EnumerateObject())
			{
				if (string.Equals(jsonProperty.Name, propertyName, 5))
				{
					value = jsonProperty.Value;
					return true;
				}
			}
			value = default(JsonElement);
			return false;
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x0004209C File Offset: 0x0004029C
		// Note: this type is marked as 'beforefieldinit'.
		static ModDiscoveryService()
		{
			JsonDocumentOptions documentOptions = default(JsonDocumentOptions);
			documentOptions.AllowTrailingCommas = true;
			documentOptions.CommentHandling = 1;
			ModDiscoveryService.DocumentOptions = documentOptions;
			ModDiscoveryService.PathSeparators = new char[]
			{
				Path.DirectorySeparatorChar,
				Path.AltDirectorySeparatorChar
			};
			ModDiscoveryService.DefaultIconRelativePaths = new string[]
			{
				Path.Combine(new string[]
				{
					"assets",
					"game",
					"textures",
					"gui",
					"3rdpartymodicon.png"
				}),
				Path.Combine("game", "textures", "gui", "3rdpartymodicon.png")
			};
			ModDiscoveryService.GeneralLoadErrorMessage = DevConfig.ModDiscoveryGeneralLoadErrorMessage;
			ModDiscoveryService.DependencyErrorMessage = DevConfig.ModDiscoveryDependencyErrorMessage;
		}

		// Token: 0x06000A80 RID: 2688 RVA: 0x00042160 File Offset: 0x00040360
		[CompilerGenerated]
		internal static string <BuildMissingDependencyMessage>g__Format|23_0(ModDependencyInfo dependency)
		{
			string version = dependency.Version;
			string text = ((version != null) ? version.Trim() : null) ?? string.Empty;
			if (string.IsNullOrEmpty(text) || text == "*")
			{
				return dependency.ModId;
			}
			return dependency.ModId + " v" + text;
		}

		// Token: 0x06000A81 RID: 2689 RVA: 0x000421B6 File Offset: 0x000403B6
		[CompilerGenerated]
		internal static void <BuildSearchPaths>g__AddPath|26_0(string candidate, ref ModDiscoveryService.<>c__DisplayClass26_0 A_1)
		{
			if (!string.IsNullOrWhiteSpace(candidate) && A_1.seen.Add(candidate))
			{
				A_1.ordered.Add(candidate);
			}
		}

		// Token: 0x040004C8 RID: 1224
		private static readonly int DiscoveryBatchSize = DevConfig.ModDiscoveryBatchSize;

		// Token: 0x040004C9 RID: 1225
		private static readonly JsonDocumentOptions DocumentOptions;

		// Token: 0x040004CA RID: 1226
		private static readonly char[] PathSeparators;

		// Token: 0x040004CB RID: 1227
		private static readonly string[] DefaultIconRelativePaths;

		// Token: 0x040004CC RID: 1228
		private static readonly string GeneralLoadErrorMessage;

		// Token: 0x040004CD RID: 1229
		private static readonly string DependencyErrorMessage;

		// Token: 0x040004CE RID: 1230
		private readonly object _defaultIconLock = new object();

		// Token: 0x040004CF RID: 1231
		private readonly object _directoryCacheLock = new object();

		// Token: 0x040004D0 RID: 1232
		private readonly Dictionary<string, ModDiscoveryService.CachedDirectoryEntry> _directoryManifestCache = new Dictionary<string, ModDiscoveryService.CachedDirectoryEntry>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x040004D1 RID: 1233
		private readonly ClientSettingsStore _settingsStore;

		// Token: 0x040004D2 RID: 1234
		[Nullable(2)]
		private byte[] _defaultIconBytes;

		// Token: 0x040004D3 RID: 1235
		private bool _defaultIconResolved;

		// Token: 0x020002C3 RID: 707
		[NullableContext(2)]
		[Nullable(0)]
		[RequiredMember]
		private sealed class CachedDirectoryEntry
		{
			// Token: 0x170004E7 RID: 1255
			// (get) Token: 0x060017A5 RID: 6053 RVA: 0x0008A8F2 File Offset: 0x00088AF2
			// (set) Token: 0x060017A6 RID: 6054 RVA: 0x0008A8FA File Offset: 0x00088AFA
			[Nullable(1)]
			[RequiredMember]
			public ModDiscoveryService.RawModInfo Info { [NullableContext(1)] get; [NullableContext(1)] set; }

			// Token: 0x170004E8 RID: 1256
			// (get) Token: 0x060017A7 RID: 6055 RVA: 0x0008A903 File Offset: 0x00088B03
			// (set) Token: 0x060017A8 RID: 6056 RVA: 0x0008A90B File Offset: 0x00088B0B
			[RequiredMember]
			public DateTime ManifestLastWriteTimeUtc { get; set; }

			// Token: 0x170004E9 RID: 1257
			// (get) Token: 0x060017A9 RID: 6057 RVA: 0x0008A914 File Offset: 0x00088B14
			// (set) Token: 0x060017AA RID: 6058 RVA: 0x0008A91C File Offset: 0x00088B1C
			[RequiredMember]
			public long ManifestLength { get; set; }

			// Token: 0x170004EA RID: 1258
			// (get) Token: 0x060017AB RID: 6059 RVA: 0x0008A925 File Offset: 0x00088B25
			// (set) Token: 0x060017AC RID: 6060 RVA: 0x0008A92D File Offset: 0x00088B2D
			public byte[] IconBytes { get; set; }

			// Token: 0x170004EB RID: 1259
			// (get) Token: 0x060017AD RID: 6061 RVA: 0x0008A936 File Offset: 0x00088B36
			// (set) Token: 0x060017AE RID: 6062 RVA: 0x0008A93E File Offset: 0x00088B3E
			public ModDiscoveryService.DirectoryIconMetadata IconMetadata { get; set; }

			// Token: 0x060017AF RID: 6063 RVA: 0x0008A947 File Offset: 0x00088B47
			[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
			[CompilerFeatureRequired("RequiredMembers")]
			public CachedDirectoryEntry()
			{
			}
		}

		// Token: 0x020002C4 RID: 708
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class DirectoryIconCache
		{
			// Token: 0x170004EC RID: 1260
			// (get) Token: 0x060017B0 RID: 6064 RVA: 0x0008A94F File Offset: 0x00088B4F
			// (set) Token: 0x060017B1 RID: 6065 RVA: 0x0008A957 File Offset: 0x00088B57
			public byte[] Bytes { get; set; }

			// Token: 0x170004ED RID: 1261
			// (get) Token: 0x060017B2 RID: 6066 RVA: 0x0008A960 File Offset: 0x00088B60
			// (set) Token: 0x060017B3 RID: 6067 RVA: 0x0008A968 File Offset: 0x00088B68
			public ModDiscoveryService.DirectoryIconMetadata Metadata { get; set; }
		}

		// Token: 0x020002C5 RID: 709
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class DirectoryIconMetadata
		{
			// Token: 0x170004EE RID: 1262
			// (get) Token: 0x060017B5 RID: 6069 RVA: 0x0008A979 File Offset: 0x00088B79
			// (set) Token: 0x060017B6 RID: 6070 RVA: 0x0008A981 File Offset: 0x00088B81
			public string IconPath { get; set; }

			// Token: 0x170004EF RID: 1263
			// (get) Token: 0x060017B7 RID: 6071 RVA: 0x0008A98A File Offset: 0x00088B8A
			// (set) Token: 0x060017B8 RID: 6072 RVA: 0x0008A992 File Offset: 0x00088B92
			public DateTime? LastWriteTimeUtc { get; set; }

			// Token: 0x170004F0 RID: 1264
			// (get) Token: 0x060017B9 RID: 6073 RVA: 0x0008A99B File Offset: 0x00088B9B
			// (set) Token: 0x060017BA RID: 6074 RVA: 0x0008A9A3 File Offset: 0x00088BA3
			public long? Length { get; set; }

			// Token: 0x170004F1 RID: 1265
			// (get) Token: 0x060017BB RID: 6075 RVA: 0x0008A9AC File Offset: 0x00088BAC
			// (set) Token: 0x060017BC RID: 6076 RVA: 0x0008A9B4 File Offset: 0x00088BB4
			public bool WasMissing { get; set; }
		}

		// Token: 0x020002C6 RID: 710
		[Nullable(0)]
		private sealed class ZipArchiveLookup
		{
			// Token: 0x060017BE RID: 6078 RVA: 0x0008A9C5 File Offset: 0x00088BC5
			public ZipArchiveLookup([Nullable(new byte[]
			{
				1,
				1,
				2
			})] Dictionary<string, ZipArchiveEntry> byPath, [Nullable(new byte[]
			{
				1,
				1,
				2
			})] Dictionary<string, ZipArchiveEntry> byFileName)
			{
				this._byPath = byPath;
				this._byFileName = byFileName;
			}

			// Token: 0x060017BF RID: 6079 RVA: 0x0008A9DC File Offset: 0x00088BDC
			public bool TryGetEntry(string normalizedPath, [Nullable(2)] out ZipArchiveEntry entry)
			{
				ZipArchiveEntry zipArchiveEntry;
				if (this._byPath.TryGetValue(normalizedPath, ref zipArchiveEntry) && zipArchiveEntry != null)
				{
					entry = zipArchiveEntry;
					return true;
				}
				entry = null;
				return false;
			}

			// Token: 0x060017C0 RID: 6080 RVA: 0x0008AA08 File Offset: 0x00088C08
			public bool TryGetEntryByFileName(string normalizedFileName, [Nullable(2)] out ZipArchiveEntry entry)
			{
				ZipArchiveEntry zipArchiveEntry;
				if (this._byFileName.TryGetValue(normalizedFileName, ref zipArchiveEntry) && zipArchiveEntry != null)
				{
					entry = zipArchiveEntry;
					return true;
				}
				entry = null;
				return false;
			}

			// Token: 0x040011B8 RID: 4536
			[Nullable(new byte[]
			{
				1,
				1,
				2
			})]
			private readonly Dictionary<string, ZipArchiveEntry> _byFileName;

			// Token: 0x040011B9 RID: 4537
			[Nullable(new byte[]
			{
				1,
				1,
				2
			})]
			private readonly Dictionary<string, ZipArchiveEntry> _byPath;
		}

		// Token: 0x020002C7 RID: 711
		[NullableContext(2)]
		[Nullable(0)]
		[RequiredMember]
		private sealed class RawModInfo
		{
			// Token: 0x170004F2 RID: 1266
			// (get) Token: 0x060017C1 RID: 6081 RVA: 0x0008AA31 File Offset: 0x00088C31
			// (set) Token: 0x060017C2 RID: 6082 RVA: 0x0008AA39 File Offset: 0x00088C39
			[Nullable(1)]
			[RequiredMember]
			public string ModId { [NullableContext(1)] get; [NullableContext(1)] set; }

			// Token: 0x170004F3 RID: 1267
			// (get) Token: 0x060017C3 RID: 6083 RVA: 0x0008AA42 File Offset: 0x00088C42
			// (set) Token: 0x060017C4 RID: 6084 RVA: 0x0008AA4A File Offset: 0x00088C4A
			[Nullable(1)]
			[RequiredMember]
			public string Name { [NullableContext(1)] get; [NullableContext(1)] set; }

			// Token: 0x170004F4 RID: 1268
			// (get) Token: 0x060017C5 RID: 6085 RVA: 0x0008AA53 File Offset: 0x00088C53
			// (set) Token: 0x060017C6 RID: 6086 RVA: 0x0008AA5B File Offset: 0x00088C5B
			public string ManifestName { get; set; }

			// Token: 0x170004F5 RID: 1269
			// (get) Token: 0x060017C7 RID: 6087 RVA: 0x0008AA64 File Offset: 0x00088C64
			// (set) Token: 0x060017C8 RID: 6088 RVA: 0x0008AA6C File Offset: 0x00088C6C
			public string Version { get; set; }

			// Token: 0x170004F6 RID: 1270
			// (get) Token: 0x060017C9 RID: 6089 RVA: 0x0008AA75 File Offset: 0x00088C75
			// (set) Token: 0x060017CA RID: 6090 RVA: 0x0008AA7D File Offset: 0x00088C7D
			public string NetworkVersion { get; set; }

			// Token: 0x170004F7 RID: 1271
			// (get) Token: 0x060017CB RID: 6091 RVA: 0x0008AA86 File Offset: 0x00088C86
			// (set) Token: 0x060017CC RID: 6092 RVA: 0x0008AA8E File Offset: 0x00088C8E
			public string Description { get; set; }

			// Token: 0x170004F8 RID: 1272
			// (get) Token: 0x060017CD RID: 6093 RVA: 0x0008AA97 File Offset: 0x00088C97
			// (set) Token: 0x060017CE RID: 6094 RVA: 0x0008AA9F File Offset: 0x00088C9F
			public string Website { get; set; }

			// Token: 0x170004F9 RID: 1273
			// (get) Token: 0x060017CF RID: 6095 RVA: 0x0008AAA8 File Offset: 0x00088CA8
			// (set) Token: 0x060017D0 RID: 6096 RVA: 0x0008AAB0 File Offset: 0x00088CB0
			[Nullable(1)]
			public IReadOnlyList<string> Authors { [NullableContext(1)] get; [NullableContext(1)] set; } = Array.Empty<string>();

			// Token: 0x170004FA RID: 1274
			// (get) Token: 0x060017D1 RID: 6097 RVA: 0x0008AAB9 File Offset: 0x00088CB9
			// (set) Token: 0x060017D2 RID: 6098 RVA: 0x0008AAC1 File Offset: 0x00088CC1
			[Nullable(1)]
			public IReadOnlyList<string> Contributors { [NullableContext(1)] get; [NullableContext(1)] set; } = Array.Empty<string>();

			// Token: 0x170004FB RID: 1275
			// (get) Token: 0x060017D3 RID: 6099 RVA: 0x0008AACA File Offset: 0x00088CCA
			// (set) Token: 0x060017D4 RID: 6100 RVA: 0x0008AAD2 File Offset: 0x00088CD2
			[Nullable(1)]
			public IReadOnlyList<ModDependencyInfo> Dependencies { [NullableContext(1)] get; [NullableContext(1)] set; } = Array.Empty<ModDependencyInfo>();

			// Token: 0x170004FC RID: 1276
			// (get) Token: 0x060017D5 RID: 6101 RVA: 0x0008AADB File Offset: 0x00088CDB
			// (set) Token: 0x060017D6 RID: 6102 RVA: 0x0008AAE3 File Offset: 0x00088CE3
			public string IconPath { get; set; }

			// Token: 0x170004FD RID: 1277
			// (get) Token: 0x060017D7 RID: 6103 RVA: 0x0008AAEC File Offset: 0x00088CEC
			// (set) Token: 0x060017D8 RID: 6104 RVA: 0x0008AAF4 File Offset: 0x00088CF4
			public string Side { get; set; }

			// Token: 0x170004FE RID: 1278
			// (get) Token: 0x060017D9 RID: 6105 RVA: 0x0008AAFD File Offset: 0x00088CFD
			// (set) Token: 0x060017DA RID: 6106 RVA: 0x0008AB05 File Offset: 0x00088D05
			public bool? RequiredOnClient { get; set; }

			// Token: 0x170004FF RID: 1279
			// (get) Token: 0x060017DB RID: 6107 RVA: 0x0008AB0E File Offset: 0x00088D0E
			// (set) Token: 0x060017DC RID: 6108 RVA: 0x0008AB16 File Offset: 0x00088D16
			public bool? RequiredOnServer { get; set; }

			// Token: 0x060017DD RID: 6109 RVA: 0x0008AB1F File Offset: 0x00088D1F
			[Obsolete("Constructors of types with required members are not supported in this version of your compiler.", true)]
			[CompilerFeatureRequired("RequiredMembers")]
			public RawModInfo()
			{
			}
		}

		// Token: 0x020002C8 RID: 712
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040011C8 RID: 4552
			[Nullable(0)]
			public static Func<ModDependencyInfo, string> <0>__Format;
		}
	}
}
