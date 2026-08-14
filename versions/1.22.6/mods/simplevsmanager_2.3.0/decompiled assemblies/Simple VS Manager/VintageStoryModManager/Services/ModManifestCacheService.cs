using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000078 RID: 120
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ModManifestCacheService
	{
		// Token: 0x06000ADF RID: 2783 RVA: 0x00044608 File Offset: 0x00042808
		public static bool TryGetManifest(string sourcePath, DateTime lastWriteTimeUtc, long length, out string manifestJson, [Nullable(2)] out byte[] iconBytes)
		{
			manifestJson = string.Empty;
			iconBytes = null;
			string text = ModManifestCacheService.NormalizePath(sourcePath);
			long num = ModManifestCacheService.ToUniversalTicks(lastWriteTimeUtc);
			object cacheLock = ModManifestCacheService.CacheLock;
			bool result;
			lock (cacheLock)
			{
				ModManifestCacheService.UnifiedMetadataCache unifiedMetadataCache = ModManifestCacheService.EnsureCacheLocked();
				ModManifestCacheService.CachedMetadataEntry cachedMetadataEntry;
				if (!unifiedMetadataCache.Entries.TryGetValue(text, ref cachedMetadataEntry))
				{
					result = false;
				}
				else if (cachedMetadataEntry.Length != length || cachedMetadataEntry.LastWriteTimeUtcTicks != num)
				{
					unifiedMetadataCache.Entries.Remove(text);
					ModManifestCacheService.SaveCacheLocked(unifiedMetadataCache);
					result = false;
				}
				else
				{
					try
					{
						manifestJson = (cachedMetadataEntry.ManifestJson ?? string.Empty);
						string iconPath = ModManifestCacheService.GetIconPath(cachedMetadataEntry.ModId, cachedMetadataEntry.Version);
						if (!string.IsNullOrWhiteSpace(iconPath) && File.Exists(iconPath))
						{
							iconBytes = File.ReadAllBytes(iconPath);
						}
						result = true;
					}
					catch (Exception)
					{
						unifiedMetadataCache.Entries.Remove(text);
						ModManifestCacheService.SaveCacheLocked(unifiedMetadataCache);
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x06000AE0 RID: 2784 RVA: 0x00044718 File Offset: 0x00042918
		public static void ClearCache()
		{
			object cacheLock = ModManifestCacheService.CacheLock;
			lock (cacheLock)
			{
				ModManifestCacheService._cache = null;
			}
			string metadataRoot = ModManifestCacheService.GetMetadataRoot();
			if (!string.IsNullOrWhiteSpace(metadataRoot) && Directory.Exists(metadataRoot))
			{
				try
				{
					Directory.Delete(metadataRoot, true);
				}
				catch (Exception ex)
				{
					throw new IOException("Failed to delete the mod metadata cache at " + metadataRoot + ".", ex);
				}
			}
			string iconCacheRoot = ModManifestCacheService.GetIconCacheRoot();
			if (!string.IsNullOrWhiteSpace(iconCacheRoot) && Directory.Exists(iconCacheRoot))
			{
				try
				{
					Directory.Delete(iconCacheRoot, true);
				}
				catch (Exception ex2)
				{
					throw new IOException("Failed to delete the mod icon cache at " + iconCacheRoot + ".", ex2);
				}
			}
		}

		// Token: 0x06000AE1 RID: 2785 RVA: 0x000447E4 File Offset: 0x000429E4
		public static void StoreManifest(string sourcePath, DateTime lastWriteTimeUtc, long length, string modId, [Nullable(2)] string version, string manifestJson, [Nullable(2)] byte[] iconBytes)
		{
			string text = ModManifestCacheService.NormalizePath(sourcePath);
			long lastWriteTimeUtcTicks = ModManifestCacheService.ToUniversalTicks(lastWriteTimeUtc);
			try
			{
				if (iconBytes != null && iconBytes.Length > 0)
				{
					string iconPath = ModManifestCacheService.GetIconPath(modId, version);
					if (!string.IsNullOrWhiteSpace(iconPath))
					{
						string directoryName = Path.GetDirectoryName(iconPath);
						if (!string.IsNullOrWhiteSpace(directoryName))
						{
							Directory.CreateDirectory(directoryName);
							File.WriteAllBytes(iconPath, iconBytes);
						}
					}
				}
				object cacheLock = ModManifestCacheService.CacheLock;
				lock (cacheLock)
				{
					ModManifestCacheService.UnifiedMetadataCache unifiedMetadataCache = ModManifestCacheService.EnsureCacheLocked();
					ModManifestCacheService.CachedMetadataEntry cachedMetadataEntry = new ModManifestCacheService.CachedMetadataEntry
					{
						ModId = modId,
						Version = version,
						ManifestJson = manifestJson,
						Length = length,
						LastWriteTimeUtcTicks = lastWriteTimeUtcTicks,
						Tags = Array.Empty<string>()
					};
					unifiedMetadataCache.Entries[text] = cachedMetadataEntry;
					unifiedMetadataCache.InvalidateIndex();
					ModManifestCacheService.SaveCacheLocked(unifiedMetadataCache);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000AE2 RID: 2786 RVA: 0x000448D0 File Offset: 0x00042AD0
		public static void Invalidate(string sourcePath)
		{
			string text = ModManifestCacheService.NormalizePath(sourcePath);
			object cacheLock = ModManifestCacheService.CacheLock;
			lock (cacheLock)
			{
				ModManifestCacheService.UnifiedMetadataCache unifiedMetadataCache = ModManifestCacheService.EnsureCacheLocked();
				if (unifiedMetadataCache.Entries.Remove(text))
				{
					unifiedMetadataCache.InvalidateIndex();
					ModManifestCacheService.SaveCacheLocked(unifiedMetadataCache);
				}
			}
		}

		// Token: 0x06000AE3 RID: 2787 RVA: 0x00044930 File Offset: 0x00042B30
		public static void UpdateTags(string modId, [Nullable(2)] string version, IReadOnlyList<string> tags)
		{
			if (string.IsNullOrWhiteSpace(modId) || tags == null || tags.Count == 0)
			{
				return;
			}
			object cacheLock = ModManifestCacheService.CacheLock;
			lock (cacheLock)
			{
				ModManifestCacheService.UnifiedMetadataCache unifiedMetadataCache = ModManifestCacheService.EnsureCacheLocked();
				bool flag2 = false;
				string modVersionKey = ModManifestCacheService.GetModVersionKey(modId, version);
				List<string> list;
				if (unifiedMetadataCache.ModVersionIndex.TryGetValue(modVersionKey, ref list))
				{
					foreach (string text in list)
					{
						ModManifestCacheService.CachedMetadataEntry cachedMetadataEntry;
						if (unifiedMetadataCache.Entries.TryGetValue(text, ref cachedMetadataEntry))
						{
							cachedMetadataEntry.Tags = Enumerable.ToArray<string>(tags);
							flag2 = true;
						}
					}
				}
				if (flag2)
				{
					ModManifestCacheService.SaveCacheLocked(unifiedMetadataCache);
				}
			}
		}

		// Token: 0x06000AE4 RID: 2788 RVA: 0x00044A04 File Offset: 0x00042C04
		public static IReadOnlyList<string> GetTags(string modId, [Nullable(2)] string version)
		{
			if (string.IsNullOrWhiteSpace(modId))
			{
				return Array.Empty<string>();
			}
			object cacheLock = ModManifestCacheService.CacheLock;
			IReadOnlyList<string> result;
			lock (cacheLock)
			{
				ModManifestCacheService.UnifiedMetadataCache unifiedMetadataCache = ModManifestCacheService.EnsureCacheLocked();
				string modVersionKey = ModManifestCacheService.GetModVersionKey(modId, version);
				List<string> list;
				ModManifestCacheService.CachedMetadataEntry cachedMetadataEntry;
				if (unifiedMetadataCache.ModVersionIndex.TryGetValue(modVersionKey, ref list) && list.Count > 0 && unifiedMetadataCache.Entries.TryGetValue(list[0], ref cachedMetadataEntry))
				{
					result = (cachedMetadataEntry.Tags ?? Array.Empty<string>());
				}
				else
				{
					result = Array.Empty<string>();
				}
			}
			return result;
		}

		// Token: 0x06000AE5 RID: 2789 RVA: 0x00044AA8 File Offset: 0x00042CA8
		private static string GetModVersionKey(string modId, [Nullable(2)] string version)
		{
			if (!string.IsNullOrWhiteSpace(version))
			{
				return modId.ToLowerInvariant() + ":" + version.ToLowerInvariant();
			}
			return modId.ToLowerInvariant();
		}

		// Token: 0x06000AE6 RID: 2790 RVA: 0x00044ACF File Offset: 0x00042CCF
		private static ModManifestCacheService.UnifiedMetadataCache EnsureCacheLocked()
		{
			ModManifestCacheService.UnifiedMetadataCache result;
			if ((result = ModManifestCacheService._cache) == null)
			{
				result = (ModManifestCacheService._cache = ModManifestCacheService.LoadCache());
			}
			return result;
		}

		// Token: 0x06000AE7 RID: 2791 RVA: 0x00044AE8 File Offset: 0x00042CE8
		private static ModManifestCacheService.UnifiedMetadataCache LoadCache()
		{
			string unifiedCachePath = ModManifestCacheService.GetUnifiedCachePath();
			if (unifiedCachePath == null)
			{
				return new ModManifestCacheService.UnifiedMetadataCache();
			}
			ModManifestCacheService.UnifiedMetadataCache result;
			try
			{
				if (!File.Exists(unifiedCachePath))
				{
					result = new ModManifestCacheService.UnifiedMetadataCache();
				}
				else
				{
					result = (JsonSerializer.Deserialize<ModManifestCacheService.UnifiedMetadataCache>(File.ReadAllText(unifiedCachePath), ModManifestCacheService.SerializerOptions) ?? new ModManifestCacheService.UnifiedMetadataCache());
				}
			}
			catch (Exception)
			{
				result = new ModManifestCacheService.UnifiedMetadataCache();
			}
			return result;
		}

		// Token: 0x06000AE8 RID: 2792 RVA: 0x00044B4C File Offset: 0x00042D4C
		private static void SaveCacheLocked(ModManifestCacheService.UnifiedMetadataCache cache)
		{
			string unifiedCachePath = ModManifestCacheService.GetUnifiedCachePath();
			if (unifiedCachePath == null)
			{
				return;
			}
			try
			{
				string directoryName = Path.GetDirectoryName(unifiedCachePath);
				if (!string.IsNullOrEmpty(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				string text = JsonSerializer.Serialize<ModManifestCacheService.UnifiedMetadataCache>(cache, ModManifestCacheService.SerializerOptions);
				File.WriteAllText(unifiedCachePath, text);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000AE9 RID: 2793 RVA: 0x00044BA4 File Offset: 0x00042DA4
		[NullableContext(2)]
		private static string GetMetadataRoot()
		{
			string managerDataDirectory = ModCacheLocator.GetManagerDataDirectory();
			if (managerDataDirectory != null)
			{
				return Path.Combine(managerDataDirectory, ModManifestCacheService.MetadataFolderName);
			}
			return null;
		}

		// Token: 0x06000AEA RID: 2794 RVA: 0x00044BC8 File Offset: 0x00042DC8
		[NullableContext(2)]
		private static string GetIconCacheRoot()
		{
			string managerDataDirectory = ModCacheLocator.GetManagerDataDirectory();
			if (managerDataDirectory != null)
			{
				return Path.Combine(managerDataDirectory, ModManifestCacheService.IconCacheFolderName);
			}
			return null;
		}

		// Token: 0x06000AEB RID: 2795 RVA: 0x00044BEC File Offset: 0x00042DEC
		[NullableContext(2)]
		private static string GetUnifiedCachePath()
		{
			string metadataRoot = ModManifestCacheService.GetMetadataRoot();
			if (metadataRoot != null)
			{
				return Path.Combine(metadataRoot, ModManifestCacheService.UnifiedCacheFileName);
			}
			return null;
		}

		// Token: 0x06000AEC RID: 2796 RVA: 0x00044C10 File Offset: 0x00042E10
		[NullableContext(2)]
		private static string GetIconPath([Nullable(1)] string modId, string version)
		{
			string iconCacheRoot = ModManifestCacheService.GetIconCacheRoot();
			if (iconCacheRoot == null)
			{
				return null;
			}
			string text = ModCacheLocator.SanitizeFileName(modId, "mod");
			string text2 = ModCacheLocator.SanitizeFileName(version, "noversion");
			string text3 = text + "_" + text2 + ".icon";
			return Path.Combine(iconCacheRoot, text3);
		}

		// Token: 0x06000AED RID: 2797 RVA: 0x00044C58 File Offset: 0x00042E58
		private static string NormalizePath(string sourcePath)
		{
			string result;
			try
			{
				result = Path.GetFullPath(sourcePath);
			}
			catch (Exception)
			{
				result = sourcePath;
			}
			return result;
		}

		// Token: 0x06000AEE RID: 2798 RVA: 0x00044C84 File Offset: 0x00042E84
		private static long ToUniversalTicks(DateTime value)
		{
			if (value.Kind == null)
			{
				value = DateTime.SpecifyKind(value, 2);
			}
			return value.ToUniversalTime().Ticks;
		}

		// Token: 0x04000506 RID: 1286
		private static readonly string MetadataFolderName = DevConfig.MetadataFolderName;

		// Token: 0x04000507 RID: 1287
		private static readonly string UnifiedCacheFileName = "metadata-cache.json";

		// Token: 0x04000508 RID: 1288
		private static readonly string IconCacheFolderName = DevConfig.IconCacheFolderName;

		// Token: 0x04000509 RID: 1289
		private static readonly object CacheLock = new object();

		// Token: 0x0400050A RID: 1290
		[Nullable(2)]
		private static ModManifestCacheService.UnifiedMetadataCache _cache;

		// Token: 0x0400050B RID: 1291
		private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			WriteIndented = false,
			DefaultIgnoreCondition = 3
		};

		// Token: 0x020002E0 RID: 736
		[Nullable(0)]
		private sealed class UnifiedMetadataCache
		{
			// Token: 0x1700050A RID: 1290
			// (get) Token: 0x0600184B RID: 6219 RVA: 0x0008C99C File Offset: 0x0008AB9C
			// (set) Token: 0x0600184C RID: 6220 RVA: 0x0008C9A4 File Offset: 0x0008ABA4
			public Dictionary<string, ModManifestCacheService.CachedMetadataEntry> Entries { get; set; } = new Dictionary<string, ModManifestCacheService.CachedMetadataEntry>(StringComparer.OrdinalIgnoreCase);

			// Token: 0x1700050B RID: 1291
			// (get) Token: 0x0600184D RID: 6221 RVA: 0x0008C9B0 File Offset: 0x0008ABB0
			[JsonIgnore]
			public Dictionary<string, List<string>> ModVersionIndex
			{
				get
				{
					if (this._modVersionIndex == null)
					{
						this._modVersionIndex = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
						foreach (KeyValuePair<string, ModManifestCacheService.CachedMetadataEntry> keyValuePair in this.Entries)
						{
							string modVersionKey = ModManifestCacheService.GetModVersionKey(keyValuePair.Value.ModId, keyValuePair.Value.Version);
							List<string> list;
							if (!this._modVersionIndex.TryGetValue(modVersionKey, ref list))
							{
								list = new List<string>();
								this._modVersionIndex[modVersionKey] = list;
							}
							list.Add(keyValuePair.Key);
						}
					}
					return this._modVersionIndex;
				}
			}

			// Token: 0x0600184E RID: 6222 RVA: 0x0008CA6C File Offset: 0x0008AC6C
			public void InvalidateIndex()
			{
				this._modVersionIndex = null;
			}

			// Token: 0x04001249 RID: 4681
			[Nullable(new byte[]
			{
				2,
				1,
				1,
				1
			})]
			[JsonIgnore]
			private Dictionary<string, List<string>> _modVersionIndex;
		}

		// Token: 0x020002E1 RID: 737
		[Nullable(0)]
		private sealed class CachedMetadataEntry
		{
			// Token: 0x1700050C RID: 1292
			// (get) Token: 0x06001850 RID: 6224 RVA: 0x0008CA8D File Offset: 0x0008AC8D
			// (set) Token: 0x06001851 RID: 6225 RVA: 0x0008CA95 File Offset: 0x0008AC95
			public string ModId { get; set; } = string.Empty;

			// Token: 0x1700050D RID: 1293
			// (get) Token: 0x06001852 RID: 6226 RVA: 0x0008CA9E File Offset: 0x0008AC9E
			// (set) Token: 0x06001853 RID: 6227 RVA: 0x0008CAA6 File Offset: 0x0008ACA6
			[Nullable(2)]
			public string Version { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x1700050E RID: 1294
			// (get) Token: 0x06001854 RID: 6228 RVA: 0x0008CAAF File Offset: 0x0008ACAF
			// (set) Token: 0x06001855 RID: 6229 RVA: 0x0008CAB7 File Offset: 0x0008ACB7
			[Nullable(2)]
			public string ManifestJson { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x1700050F RID: 1295
			// (get) Token: 0x06001856 RID: 6230 RVA: 0x0008CAC0 File Offset: 0x0008ACC0
			// (set) Token: 0x06001857 RID: 6231 RVA: 0x0008CAC8 File Offset: 0x0008ACC8
			public long Length { get; set; }

			// Token: 0x17000510 RID: 1296
			// (get) Token: 0x06001858 RID: 6232 RVA: 0x0008CAD1 File Offset: 0x0008ACD1
			// (set) Token: 0x06001859 RID: 6233 RVA: 0x0008CAD9 File Offset: 0x0008ACD9
			public long LastWriteTimeUtcTicks { get; set; }

			// Token: 0x17000511 RID: 1297
			// (get) Token: 0x0600185A RID: 6234 RVA: 0x0008CAE2 File Offset: 0x0008ACE2
			// (set) Token: 0x0600185B RID: 6235 RVA: 0x0008CAEA File Offset: 0x0008ACEA
			public string[] Tags { get; set; } = Array.Empty<string>();
		}
	}
}
