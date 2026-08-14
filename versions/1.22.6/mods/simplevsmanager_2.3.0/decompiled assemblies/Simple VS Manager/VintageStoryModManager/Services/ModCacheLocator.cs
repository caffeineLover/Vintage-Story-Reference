using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000069 RID: 105
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ModCacheLocator
	{
		// Token: 0x060009B9 RID: 2489 RVA: 0x0003A914 File Offset: 0x00038B14
		[NullableContext(2)]
		public static string GetManagerDataDirectory()
		{
			string customConfigFolder = CustomConfigFolderManager.GetCustomConfigFolder();
			if (!string.IsNullOrWhiteSpace(customConfigFolder))
			{
				return customConfigFolder;
			}
			return ModCacheLocator.GetDefaultManagerDataDirectory();
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x0003A938 File Offset: 0x00038B38
		[NullableContext(2)]
		public static string GetDefaultManagerDataDirectory()
		{
			string folderPath = Environment.GetFolderPath(28);
			if (string.IsNullOrWhiteSpace(folderPath))
			{
				folderPath = Environment.GetFolderPath(26);
			}
			if (string.IsNullOrWhiteSpace(folderPath))
			{
				folderPath = Environment.GetFolderPath(5);
			}
			if (string.IsNullOrWhiteSpace(folderPath))
			{
				folderPath = Environment.GetFolderPath(5);
			}
			if (!string.IsNullOrWhiteSpace(folderPath))
			{
				return Path.Combine(folderPath, "Simple VS Manager");
			}
			return null;
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x0003A990 File Offset: 0x00038B90
		[NullableContext(2)]
		public static string GetCachedModsDirectory()
		{
			string managerDataDirectory = ModCacheLocator.GetManagerDataDirectory();
			if (managerDataDirectory != null)
			{
				return Path.Combine(managerDataDirectory, "Temp Cache", "Cached Mods");
			}
			return null;
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x0003A9B8 File Offset: 0x00038BB8
		[return: Nullable(2)]
		public static string GetModCacheDirectory(string modId)
		{
			string cachedModsDirectory = ModCacheLocator.GetCachedModsDirectory();
			if (string.IsNullOrWhiteSpace(cachedModsDirectory))
			{
				return null;
			}
			string text = ModCacheLocator.SanitizeFileName(modId, "mod");
			return Path.Combine(cachedModsDirectory, text);
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x0003A9E8 File Offset: 0x00038BE8
		[NullableContext(2)]
		public static string GetModDatabaseCacheDirectory()
		{
			string managerDataDirectory = ModCacheLocator.GetManagerDataDirectory();
			if (managerDataDirectory != null)
			{
				return Path.Combine(managerDataDirectory, "Temp Cache", "Mod Database Cache");
			}
			return null;
		}

		// Token: 0x060009BE RID: 2494 RVA: 0x0003AA10 File Offset: 0x00038C10
		[NullableContext(2)]
		public static string GetModDatabaseImageCacheDirectory()
		{
			string modDatabaseCacheDirectory = ModCacheLocator.GetModDatabaseCacheDirectory();
			if (modDatabaseCacheDirectory != null)
			{
				return Path.Combine(modDatabaseCacheDirectory, "Images");
			}
			return null;
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0003AA34 File Offset: 0x00038C34
		[NullableContext(2)]
		public static string GetModCachePath([Nullable(1)] string modId, string version, string fileName)
		{
			if (string.IsNullOrWhiteSpace(version))
			{
				return null;
			}
			string modCacheDirectory = ModCacheLocator.GetModCacheDirectory(modId);
			if (string.IsNullOrWhiteSpace(modCacheDirectory))
			{
				return null;
			}
			string text = ModCacheLocator.SanitizeFileName(version, "version");
			string text2 = Path.Combine(modCacheDirectory, text);
			string text3 = ModCacheLocator.SanitizeCacheFileName(fileName, modId, text);
			return Path.Combine(text2, text3);
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x0003AA80 File Offset: 0x00038C80
		[NullableContext(2)]
		public static bool TryLocateCachedModFile([Nullable(1)] string modId, string version, string fileName, out string cacheFilePath)
		{
			cacheFilePath = null;
			if (string.IsNullOrWhiteSpace(version))
			{
				return false;
			}
			string modCacheDirectory = ModCacheLocator.GetModCacheDirectory(modId);
			if (string.IsNullOrWhiteSpace(modCacheDirectory))
			{
				return false;
			}
			string text = ModCacheLocator.SanitizeFileName(version, "version");
			string text2 = Path.Combine(modCacheDirectory, text);
			string text3 = ModCacheLocator.SanitizeCacheFileName(fileName, modId, text);
			string text4 = Path.Combine(text2, text3);
			if (File.Exists(text4))
			{
				cacheFilePath = text4;
				return true;
			}
			if (Directory.Exists(text2))
			{
				try
				{
					using (IEnumerator<string> enumerator = Directory.EnumerateFiles(text2, "*", 0).GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							string text5 = enumerator.Current;
							cacheFilePath = text5;
							return true;
						}
					}
				}
				catch (Exception)
				{
				}
			}
			string legacyModCachePath = ModCacheLocator.GetLegacyModCachePath(modCacheDirectory, text, fileName);
			if (legacyModCachePath != null && File.Exists(legacyModCachePath))
			{
				cacheFilePath = legacyModCachePath;
				return true;
			}
			return false;
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x0003AB68 File Offset: 0x00038D68
		public static bool TryPromoteLegacyCacheFile(string modId, [Nullable(2)] string version, [Nullable(2)] string fileName, string targetPath)
		{
			if (string.IsNullOrWhiteSpace(version))
			{
				return false;
			}
			string modCacheDirectory = ModCacheLocator.GetModCacheDirectory(modId);
			if (string.IsNullOrWhiteSpace(modCacheDirectory))
			{
				return false;
			}
			string versionSegment = ModCacheLocator.SanitizeFileName(version, "version");
			string legacyModCachePath = ModCacheLocator.GetLegacyModCachePath(modCacheDirectory, versionSegment, fileName);
			if (legacyModCachePath == null || !File.Exists(legacyModCachePath))
			{
				return false;
			}
			if (string.Equals(legacyModCachePath, targetPath, 5))
			{
				return true;
			}
			bool result;
			try
			{
				string directoryName = Path.GetDirectoryName(targetPath);
				if (!string.IsNullOrWhiteSpace(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				File.Move(legacyModCachePath, targetPath);
				result = true;
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is NotSupportedException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0003AC34 File Offset: 0x00038E34
		public static IEnumerable<string> EnumerateCachedFiles(string modId)
		{
			ModCacheLocator.<EnumerateCachedFiles>d__9 <EnumerateCachedFiles>d__ = new ModCacheLocator.<EnumerateCachedFiles>d__9(-2);
			<EnumerateCachedFiles>d__.<>3__modId = modId;
			return <EnumerateCachedFiles>d__;
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x0003AC44 File Offset: 0x00038E44
		public static string SanitizeFileName([Nullable(2)] string input, string fallback)
		{
			if (string.IsNullOrWhiteSpace(input))
			{
				return fallback;
			}
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			StringBuilder stringBuilder = new StringBuilder(input.Length);
			for (int i = 0; i < input.Length; i++)
			{
				char c = input.get_Chars(i);
				if (Array.IndexOf<char>(invalidFileNameChars, c) >= 0)
				{
					stringBuilder.Append('_');
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			string text = stringBuilder.ToString().Trim();
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			return fallback;
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0003ACC4 File Offset: 0x00038EC4
		private static string SanitizeCacheFileName([Nullable(2)] string fileName, string modId, string versionSegment)
		{
			if (!string.IsNullOrWhiteSpace(fileName))
			{
				string text = ModCacheLocator.ReplaceInvalidCacheFileCharacters(fileName);
				if (!string.IsNullOrWhiteSpace(text))
				{
					if (string.IsNullOrWhiteSpace(Path.GetExtension(text)))
					{
						text += ".zip";
					}
					return text;
				}
			}
			return ModCacheLocator.BuildFallbackFileName(modId, versionSegment);
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x0003AD0C File Offset: 0x00038F0C
		private static string BuildFallbackFileName(string modId, string versionSegment)
		{
			string text = ModCacheLocator.SanitizeFileName(modId, "mod");
			return string.Concat(new object[]
			{
				text,
				'-',
				versionSegment,
				".zip"
			});
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x0003AD4C File Offset: 0x00038F4C
		private static string ReplaceInvalidCacheFileCharacters(string value)
		{
			char[] invalidFileNameChars = Path.GetInvalidFileNameChars();
			StringBuilder stringBuilder = new StringBuilder(value.Length);
			string text = value.Trim();
			for (int i = 0; i < text.Length; i++)
			{
				char c = text.get_Chars(i);
				stringBuilder.Append((Array.IndexOf<char>(invalidFileNameChars, c) >= 0) ? '_' : c);
			}
			string text2 = stringBuilder.ToString().TrimEnd('.');
			if (!string.IsNullOrWhiteSpace(text2))
			{
				return text2;
			}
			return string.Empty;
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x0003ADC8 File Offset: 0x00038FC8
		[return: Nullable(2)]
		private static string GetLegacyModCachePath(string modCacheDirectory, string versionSegment, [Nullable(2)] string fileName)
		{
			string text = string.IsNullOrWhiteSpace(fileName) ? ".zip" : Path.GetExtension(fileName);
			if (string.IsNullOrWhiteSpace(text))
			{
				text = ".zip";
			}
			return Path.Combine(modCacheDirectory, versionSegment + text);
		}
	}
}
