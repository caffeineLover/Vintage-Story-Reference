using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000094 RID: 148
	[NullableContext(1)]
	[Nullable(0)]
	public static class VintageStoryVersionLocator
	{
		// Token: 0x06000D2D RID: 3373 RVA: 0x00050580 File Offset: 0x0004E780
		[NullableContext(2)]
		public static string GetInstalledVersion(string configuredGameDirectory = null)
		{
			foreach (string path in VintageStoryVersionLocator.EnumerateCandidates(configuredGameDirectory))
			{
				string text = VintageStoryVersionLocator.TryGetVersionFromFile(path);
				if (!string.IsNullOrWhiteSpace(text))
				{
					return text;
				}
			}
			return null;
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x000505DC File Offset: 0x0004E7DC
		private static IEnumerable<string> EnumerateCandidates([Nullable(2)] string configuredGameDirectory)
		{
			VintageStoryVersionLocator.<EnumerateCandidates>d__2 <EnumerateCandidates>d__ = new VintageStoryVersionLocator.<EnumerateCandidates>d__2(-2);
			<EnumerateCandidates>d__.<>3__configuredGameDirectory = configuredGameDirectory;
			return <EnumerateCandidates>d__;
		}

		// Token: 0x06000D2F RID: 3375 RVA: 0x000505EC File Offset: 0x0004E7EC
		private static IEnumerable<string> EnumerateRoots([Nullable(2)] string configuredGameDirectory)
		{
			VintageStoryVersionLocator.<EnumerateRoots>d__3 <EnumerateRoots>d__ = new VintageStoryVersionLocator.<EnumerateRoots>d__3(-2);
			<EnumerateRoots>d__.<>3__configuredGameDirectory = configuredGameDirectory;
			return <EnumerateRoots>d__;
		}

		// Token: 0x06000D30 RID: 3376 RVA: 0x000505FC File Offset: 0x0004E7FC
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

		// Token: 0x06000D31 RID: 3377 RVA: 0x00050634 File Offset: 0x0004E834
		[return: Nullable(2)]
		private static string TryGetVersionFromFile(string path)
		{
			string result;
			try
			{
				if (!File.Exists(path))
				{
					result = null;
				}
				else
				{
					FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(path);
					string text = VersionStringUtility.Normalize(versionInfo.FileVersion);
					if (!string.IsNullOrWhiteSpace(text))
					{
						result = text;
					}
					else
					{
						string text2 = VersionStringUtility.Normalize(versionInfo.ProductVersion);
						if (!string.IsNullOrWhiteSpace(text2))
						{
							result = text2;
						}
						else
						{
							Version version = AssemblyName.GetAssemblyName(path).Version;
							result = VersionStringUtility.Normalize((version != null) ? version.ToString() : null);
						}
					}
				}
			}
			catch (FileNotFoundException)
			{
				result = null;
			}
			catch (BadImageFormatException)
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

		// Token: 0x040005D0 RID: 1488
		private static readonly string[] CandidateRelativePaths = new string[]
		{
			"Vintagestory.exe",
			"Vintagestory",
			Path.Combine("Vintagestory.app", "Contents", "MacOS", "Vintagestory"),
			"VintagestoryServer.exe",
			"VintagestoryServer",
			"VintagestoryAPI.dll"
		};
	}
}
