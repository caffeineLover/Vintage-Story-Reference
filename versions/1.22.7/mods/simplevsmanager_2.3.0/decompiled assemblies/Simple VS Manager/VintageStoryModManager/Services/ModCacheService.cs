using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using VintageStoryModManager.Models;

namespace VintageStoryModManager.Services
{
	// Token: 0x0200006A RID: 106
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ModCacheService
	{
		// Token: 0x060009C8 RID: 2504 RVA: 0x0003AE08 File Offset: 0x00039008
		[NullableContext(2)]
		public static void EnsureModCached(string modId, string version, string sourcePath, ModSourceKind sourceKind)
		{
			if (string.IsNullOrWhiteSpace(modId) || string.IsNullOrWhiteSpace(version) || string.IsNullOrWhiteSpace(sourcePath))
			{
				return;
			}
			string fileName = Path.GetFileName(sourcePath);
			string modCachePath = ModCacheLocator.GetModCachePath(modId, version, fileName);
			if (string.IsNullOrWhiteSpace(modCachePath))
			{
				return;
			}
			if (ModCacheLocator.TryPromoteLegacyCacheFile(modId, version, fileName, modCachePath) && File.Exists(modCachePath))
			{
				return;
			}
			if (string.Equals(sourcePath, modCachePath, 5))
			{
				return;
			}
			if (File.Exists(modCachePath))
			{
				return;
			}
			string text;
			if (ModCacheLocator.TryLocateCachedModFile(modId, version, fileName, out text) && text != null)
			{
				string directoryName = Path.GetDirectoryName(text);
				string directoryName2 = Path.GetDirectoryName(modCachePath);
				if (!string.IsNullOrWhiteSpace(directoryName) && !string.IsNullOrWhiteSpace(directoryName2) && string.Equals(directoryName, directoryName2, 5))
				{
					return;
				}
			}
			try
			{
				if (sourceKind != ModSourceKind.Folder)
				{
					if (sourceKind - ModSourceKind.ZipArchive > 2)
					{
					}
					ModCacheService.CacheFile(sourcePath, modCachePath);
				}
				else
				{
					ModCacheService.CacheDirectory(sourcePath, modCachePath);
				}
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is NotSupportedException || ex is InvalidDataException)
			{
				Trace.TraceWarning("Failed to cache mod {0} {1}: {2}", new object[]
				{
					modId,
					version,
					ex.Message
				});
				ModCacheService.TryDelete(modCachePath);
			}
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x0003AF44 File Offset: 0x00039144
		private static void CacheFile(string sourcePath, string cachePath)
		{
			if (!File.Exists(sourcePath))
			{
				return;
			}
			string directoryName = Path.GetDirectoryName(cachePath);
			if (!string.IsNullOrWhiteSpace(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			try
			{
				File.Copy(sourcePath, cachePath, false);
			}
			catch (IOException ex)
			{
				if (!File.Exists(cachePath))
				{
					throw new IOException(ex.Message, ex);
				}
			}
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x0003AFA4 File Offset: 0x000391A4
		private static void CacheDirectory(string sourceDirectory, string cachePath)
		{
			if (!Directory.Exists(sourceDirectory))
			{
				return;
			}
			string directoryName = Path.GetDirectoryName(cachePath);
			if (!string.IsNullOrWhiteSpace(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			try
			{
				ZipFile.CreateFromDirectory(sourceDirectory, cachePath, 0, false);
			}
			catch (IOException ex)
			{
				if (!File.Exists(cachePath))
				{
					throw new IOException(ex.Message, ex);
				}
			}
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x0003B004 File Offset: 0x00039204
		private static void TryDelete(string path)
		{
			try
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
			{
				Trace.TraceWarning("Failed to clean up cache file {0}: {1}", new object[]
				{
					path,
					ex.Message
				});
			}
		}
	}
}
