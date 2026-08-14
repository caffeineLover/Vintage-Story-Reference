using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace QuestPDF.Helpers
{
	// Token: 0x02000076 RID: 118
	[NullableContext(1)]
	[Nullable(0)]
	internal static class NativeDependencyProvider
	{
		// Token: 0x060002EB RID: 747 RVA: 0x00007AF4 File Offset: 0x00005CF4
		public static void EnsureNativeFileAvailability()
		{
			string nativeFileSourcePath = NativeDependencyProvider.GetNativeFileSourcePath();
			if (nativeFileSourcePath == null)
			{
				return;
			}
			foreach (string text in Directory.GetFiles(nativeFileSourcePath))
			{
				string targetPath = Path.Combine(new FileInfo(text).Directory.Parent.Parent.Parent.FullName, Path.GetFileName(text));
				NativeDependencyProvider.CopyFileIfNewer(text, targetPath);
			}
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00007B58 File Offset: 0x00005D58
		public static bool IsCurrentPlatformSupported()
		{
			string runtimePlatform = NativeDependencyProvider.GetRuntimePlatform();
			return Enumerable.Contains<string>(NativeDependencyProvider.SupportedPlatforms, runtimePlatform);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00007B78 File Offset: 0x00005D78
		[NullableContext(2)]
		private static string GetNativeFileSourcePath()
		{
			string runtimePlatform = NativeDependencyProvider.GetRuntimePlatform();
			string[] array = new string[6];
			array[0] = AppDomain.CurrentDomain.RelativeSearchPath;
			array[1] = AppDomain.CurrentDomain.BaseDirectory;
			array[2] = Environment.CurrentDirectory;
			array[3] = AppContext.BaseDirectory;
			array[4] = Directory.GetCurrentDirectory();
			int num = 5;
			DirectoryInfo directory = new FileInfo(typeof(NativeDependencyProvider).Assembly.Location).Directory;
			array[num] = ((directory != null) ? directory.FullName : null);
			foreach (string text in array)
			{
				if (!string.IsNullOrEmpty(text))
				{
					string text2 = Path.Combine(text, "runtimes", runtimePlatform, "native");
					if (Directory.Exists(text2))
					{
						return text2;
					}
				}
			}
			return null;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00007C2B File Offset: 0x00005E2B
		public static string GetRuntimePlatform()
		{
			if (RuntimeInformation.ProcessArchitecture == 4)
			{
				return "browser-wasm";
			}
			return NativeDependencyProvider.<GetRuntimePlatform>g__GetSystemIdentifier|4_0() + "-" + NativeDependencyProvider.<GetRuntimePlatform>g__GetProcessArchitecture|4_1();
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00007C50 File Offset: 0x00005E50
		[NullableContext(2)]
		private static string ExecuteLddCommand()
		{
			if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				return null;
			}
			string result;
			try
			{
				using (Process process = Process.Start(new ProcessStartInfo
				{
					FileName = "ldd",
					Arguments = "--version",
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					CreateNoWindow = true
				}))
				{
					if (process == null)
					{
						result = null;
					}
					else
					{
						string text = process.StandardOutput.ReadToEnd();
						string text2 = process.StandardError.ReadToEnd();
						process.WaitForExit();
						result = text + text2;
					}
				}
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00007D04 File Offset: 0x00005F04
		private static bool IsLinuxMusl()
		{
			return (NativeDependencyProvider.ExecuteLddCommand() ?? string.Empty).IndexOf("musl", 3) >= 0;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00007D28 File Offset: 0x00005F28
		[NullableContext(2)]
		public static Version GetGlibcVersion()
		{
			Match match = Regex.Match(NativeDependencyProvider.ExecuteLddCommand() ?? string.Empty, "ldd \\(.+\\) (?<version>[1-9]\\.[0-9]{2})");
			if (!match.Success)
			{
				return null;
			}
			Group group = match.Groups["version"];
			if (!group.Success)
			{
				return null;
			}
			Version result;
			if (!Version.TryParse(group.Value, ref result))
			{
				return null;
			}
			return result;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00007D85 File Offset: 0x00005F85
		private static void CopyFileIfNewer(string sourcePath, string targetPath)
		{
			if (!File.Exists(sourcePath))
			{
				throw new FileNotFoundException("Source file not found: " + sourcePath);
			}
			if (!File.Exists(targetPath) || File.GetLastWriteTime(sourcePath) > File.GetLastWriteTime(targetPath))
			{
				File.Copy(sourcePath, targetPath, true);
			}
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00007E14 File Offset: 0x00006014
		[CompilerGenerated]
		internal static string <GetRuntimePlatform>g__GetSystemIdentifier|4_0()
		{
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				return "win";
			}
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				if (!NativeDependencyProvider.IsLinuxMusl())
				{
					return "linux";
				}
				return "linux-musl";
			}
			else
			{
				if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
				{
					return "osx";
				}
				return "other";
			}
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00007E6C File Offset: 0x0000606C
		[CompilerGenerated]
		internal static string <GetRuntimePlatform>g__GetProcessArchitecture|4_1()
		{
			return RuntimeInformation.ProcessArchitecture.ToString().ToLower();
		}

		// Token: 0x04000233 RID: 563
		public static readonly string[] SupportedPlatforms = new string[]
		{
			"win-x86",
			"win-x64",
			"linux-x64",
			"linux-arm64",
			"linux-musl-x64",
			"osx-x64",
			"osx-arm64"
		};
	}
}
