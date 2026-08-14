using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000062 RID: 98
	[NullableContext(1)]
	[Nullable(0)]
	public static class GameDirectoryLocator
	{
		// Token: 0x06000969 RID: 2409 RVA: 0x000393F4 File Offset: 0x000375F4
		public static string Resolve()
		{
			foreach (string text in GameDirectoryLocator.EnumerateCandidates())
			{
				if (GameDirectoryLocator.IsValidGameDirectory(text))
				{
					return text;
				}
			}
			return string.Empty;
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0003944C File Offset: 0x0003764C
		[NullableContext(2)]
		public static string FindExecutable(string gameDirectory)
		{
			if (string.IsNullOrWhiteSpace(gameDirectory))
			{
				return null;
			}
			string text = GameDirectoryLocator.NormalizePath(gameDirectory);
			if (string.IsNullOrWhiteSpace(text) || !Directory.Exists(text))
			{
				return null;
			}
			foreach (string text2 in GameDirectoryLocator.ExecutableCandidates)
			{
				string text3 = Path.Combine(text, text2);
				if (File.Exists(text3))
				{
					return Path.GetFullPath(text3);
				}
			}
			return null;
		}

		// Token: 0x0600096B RID: 2411 RVA: 0x000394AF File Offset: 0x000376AF
		private static IEnumerable<string> EnumerateCandidates()
		{
			return new GameDirectoryLocator.<EnumerateCandidates>d__3(-2);
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x000394B8 File Offset: 0x000376B8
		private static IEnumerable<string> EnumerateDefaultInstallPaths()
		{
			return new GameDirectoryLocator.<EnumerateDefaultInstallPaths>d__4(-2);
		}

		// Token: 0x0600096D RID: 2413 RVA: 0x000394C4 File Offset: 0x000376C4
		[NullableContext(2)]
		private static bool IsValidGameDirectory(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return false;
			}
			string text = GameDirectoryLocator.NormalizePath(path);
			return !string.IsNullOrWhiteSpace(text) && Directory.Exists(text) && GameDirectoryLocator.FindExecutable(text) != null;
		}

		// Token: 0x0600096E RID: 2414 RVA: 0x00039500 File Offset: 0x00037700
		[return: Nullable(2)]
		private static string NormalizePath(string path)
		{
			string result;
			try
			{
				result = Path.GetFullPath(path);
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x0003952C File Offset: 0x0003772C
		[NullableContext(2)]
		private static string TryNormalize(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return null;
			}
			return GameDirectoryLocator.NormalizePath(path);
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x00039540 File Offset: 0x00037740
		[NullableContext(2)]
		private static string TryGetFolder(Environment.SpecialFolder folder)
		{
			try
			{
				string folderPath = Environment.GetFolderPath(folder, 16384);
				if (!string.IsNullOrWhiteSpace(folderPath))
				{
					return folderPath;
				}
			}
			catch (PlatformNotSupportedException)
			{
				return null;
			}
			return null;
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x00039580 File Offset: 0x00037780
		private static IEnumerable<string> EnumerateAdditionalWindowsRoots()
		{
			return new GameDirectoryLocator.<EnumerateAdditionalWindowsRoots>d__9(-2);
		}

		// Token: 0x0400047E RID: 1150
		private static readonly string[] ExecutableCandidates = new string[]
		{
			"Vintagestory.exe",
			"Vintagestory",
			Path.Combine("Vintagestory.app", "Contents", "MacOS", "Vintagestory")
		};
	}
}
