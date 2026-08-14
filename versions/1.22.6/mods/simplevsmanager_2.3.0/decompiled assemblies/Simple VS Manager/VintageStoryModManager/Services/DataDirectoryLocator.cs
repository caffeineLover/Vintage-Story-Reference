using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Services
{
	// Token: 0x0200005D RID: 93
	[NullableContext(1)]
	[Nullable(0)]
	public static class DataDirectoryLocator
	{
		// Token: 0x06000939 RID: 2361 RVA: 0x000384F4 File Offset: 0x000366F4
		public static string Resolve()
		{
			foreach (string text in DataDirectoryLocator.EnumerateCandidates())
			{
				if (!string.IsNullOrWhiteSpace(text))
				{
					return text;
				}
			}
			return string.Empty;
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x0003854C File Offset: 0x0003674C
		private static IEnumerable<string> EnumerateCandidates()
		{
			return new DataDirectoryLocator.<EnumerateCandidates>d__2(-2);
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x00038558 File Offset: 0x00036758
		[NullableContext(2)]
		private static string GetFolder(Environment.SpecialFolder folder)
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

		// Token: 0x0600093C RID: 2364 RVA: 0x00038598 File Offset: 0x00036798
		[NullableContext(2)]
		private static string GetPortableCandidate()
		{
			string baseDirectory = AppContext.BaseDirectory;
			foreach (string relative in new string[]
			{
				DataDirectoryLocator.DataFolderName,
				"data"
			})
			{
				string text = DataDirectoryLocator.TryCombine(baseDirectory, relative);
				if (!string.IsNullOrWhiteSpace(text) && Directory.Exists(text))
				{
					return text;
				}
			}
			return null;
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x000385F3 File Offset: 0x000367F3
		private static IEnumerable<string> EnumerateAdditionalWindowsRoots()
		{
			return new DataDirectoryLocator.<EnumerateAdditionalWindowsRoots>d__5(-2);
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x000385FC File Offset: 0x000367FC
		[NullableContext(2)]
		private static string TryCombine(string basePath, [Nullable(1)] string relative)
		{
			if (string.IsNullOrWhiteSpace(basePath))
			{
				return null;
			}
			string result;
			try
			{
				result = Path.GetFullPath(Path.Combine(basePath, relative));
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x0003863C File Offset: 0x0003683C
		[NullableContext(2)]
		private static string TryNormalize(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return null;
			}
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

		// Token: 0x0400046A RID: 1130
		private static readonly string DataFolderName = DevConfig.DataFolderName;
	}
}
