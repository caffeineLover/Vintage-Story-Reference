using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using RPVoiceChat.Util;

namespace RPVoiceChat.Audio.Input
{
	// Token: 0x02000108 RID: 264
	public static class FfmpegLocator
	{
		// Token: 0x060009C2 RID: 2498 RVA: 0x00036239 File Offset: 0x00034439
		public static void SetModFolder(string folderPath)
		{
			FfmpegLocator.modFolder = folderPath;
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x00036241 File Offset: 0x00034441
		public static string ResolveExecutable()
		{
			string result;
			if ((result = FfmpegLocator.ResolveBundledExecutable()) == null)
			{
				result = (FfmpegLocator.ResolveAutoInstalledExecutable() ?? FfmpegLocator.ResolveFromPath());
			}
			return result;
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0003625A File Offset: 0x0003445A
		public static bool IsAvailable()
		{
			return !string.IsNullOrWhiteSpace(FfmpegLocator.ResolveExecutable());
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x00036269 File Offset: 0x00034469
		public static void BeginEnsureAvailable()
		{
			FfmpegLocator.EnsureAvailableAsync();
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x00036274 File Offset: 0x00034474
		public static Task EnsureAvailableAsync()
		{
			object obj = FfmpegLocator.ensureLock;
			Task completedTask;
			lock (obj)
			{
				if (FfmpegLocator.IsAvailable())
				{
					completedTask = Task.CompletedTask;
				}
				else
				{
					if (FfmpegLocator.ensureTask == null || FfmpegLocator.ensureTask.IsFaulted || (FfmpegLocator.ensureTask.IsCompleted && !FfmpegLocator.IsAvailable()))
					{
						FfmpegLocator.ensureTask = FfmpegLocator.DownloadIfNeededAsync();
					}
					completedTask = FfmpegLocator.ensureTask;
				}
			}
			return completedTask;
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x000362F4 File Offset: 0x000344F4
		public static bool TryEnsureAvailable(TimeSpan timeout)
		{
			if (FfmpegLocator.IsAvailable())
			{
				return true;
			}
			bool result;
			try
			{
				if (!FfmpegLocator.EnsureAvailableAsync().Wait(timeout))
				{
					FfmpegLocator.lastFailureMessage = "FFmpeg download timed out.";
					result = false;
				}
				else
				{
					result = FfmpegLocator.IsAvailable();
				}
			}
			catch (Exception ex)
			{
				FfmpegLocator.lastFailureMessage = FfmpegLocator.UnwrapMessage(ex);
				result = FfmpegLocator.IsAvailable();
			}
			return result;
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x00036354 File Offset: 0x00034554
		public static string GetMissingMessage()
		{
			if (!string.IsNullOrWhiteSpace(FfmpegLocator.lastFailureMessage))
			{
				return "FFmpeg not found (" + FfmpegLocator.lastFailureMessage + "). Install FFmpeg on the server PATH, place a binary in Lib/ffmpeg/{win|linux|osx}/, or allow the server to download it into Lib/ffmpeg/auto/.";
			}
			return "FFmpeg not found. It will be downloaded automatically into Lib/ffmpeg/auto/ when the server has internet access. Alternatively install FFmpeg on PATH or place a binary in Lib/ffmpeg/{win|linux|osx}/.";
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x0003637C File Offset: 0x0003457C
		private static Task DownloadIfNeededAsync()
		{
			FfmpegLocator.<DownloadIfNeededAsync>d__11 <DownloadIfNeededAsync>d__;
			<DownloadIfNeededAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<DownloadIfNeededAsync>d__.<>1__state = -1;
			<DownloadIfNeededAsync>d__.<>t__builder.Start<FfmpegLocator.<DownloadIfNeededAsync>d__11>(ref <DownloadIfNeededAsync>d__);
			return <DownloadIfNeededAsync>d__.<>t__builder.Task;
		}

		// Token: 0x060009CA RID: 2506 RVA: 0x000363B7 File Offset: 0x000345B7
		private static string GetAutoInstallDirectory()
		{
			if (string.IsNullOrWhiteSpace(FfmpegLocator.modFolder))
			{
				return null;
			}
			return Path.Combine(FfmpegLocator.modFolder, "Lib", "ffmpeg", "auto");
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x000363E0 File Offset: 0x000345E0
		private static string ResolveBundledExecutable()
		{
			if (string.IsNullOrWhiteSpace(FfmpegLocator.modFolder))
			{
				return null;
			}
			string platformFolder = FfmpegLocator.GetPlatformFolderName();
			if (platformFolder == null)
			{
				return null;
			}
			string executableName = FfmpegLocator.GetExecutableFileName();
			foreach (string candidate in new string[]
			{
				Path.Combine(new string[]
				{
					FfmpegLocator.modFolder,
					"Lib",
					"ffmpeg",
					platformFolder,
					executableName
				}),
				Path.Combine(new string[]
				{
					FfmpegLocator.modFolder,
					"lib",
					"ffmpeg",
					platformFolder,
					executableName
				})
			})
			{
				if (File.Exists(candidate))
				{
					return candidate;
				}
			}
			return null;
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x00036490 File Offset: 0x00034690
		private static string ResolveAutoInstalledExecutable()
		{
			string installDir = FfmpegLocator.GetAutoInstallDirectory();
			if (string.IsNullOrWhiteSpace(installDir) || !Directory.Exists(installDir))
			{
				return null;
			}
			string executableName = FfmpegLocator.GetExecutableFileName();
			string direct = Path.Combine(installDir, executableName);
			if (File.Exists(direct))
			{
				return direct;
			}
			try
			{
				using (IEnumerator<string> enumerator = Directory.EnumerateFiles(installDir, executableName, 1).GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						return enumerator.Current;
					}
				}
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x00036524 File Offset: 0x00034724
		private static string ResolveFromPath()
		{
			if (OperatingSystem.IsWindows())
			{
				return FfmpegLocator.TryResolveFromPath("ffmpeg.exe") ?? FfmpegLocator.TryResolveFromPath("ffmpeg");
			}
			return FfmpegLocator.TryResolveFromPath("ffmpeg");
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x00036550 File Offset: 0x00034750
		private static string GetPlatformFolderName()
		{
			if (OperatingSystem.IsWindows())
			{
				return "win";
			}
			if (OperatingSystem.IsLinux())
			{
				return "linux";
			}
			if (OperatingSystem.IsMacOS())
			{
				return "osx";
			}
			return null;
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x0003657A File Offset: 0x0003477A
		private static string GetExecutableFileName()
		{
			if (!OperatingSystem.IsWindows())
			{
				return "ffmpeg";
			}
			return "ffmpeg.exe";
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x00036590 File Offset: 0x00034790
		private static string TryResolveFromPath(string executable)
		{
			string pathEnv = Environment.GetEnvironmentVariable("PATH");
			if (string.IsNullOrWhiteSpace(pathEnv))
			{
				return null;
			}
			foreach (string folder in pathEnv.Split(Path.PathSeparator, 0))
			{
				if (!string.IsNullOrWhiteSpace(folder))
				{
					try
					{
						string candidate = Path.Combine(folder.Trim(), executable);
						if (File.Exists(candidate))
						{
							return candidate;
						}
					}
					catch
					{
					}
				}
			}
			return null;
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x00036610 File Offset: 0x00034810
		private static void MakeUnixExecutable(string path)
		{
			if (string.IsNullOrWhiteSpace(path) || OperatingSystem.IsWindows() || !File.Exists(path))
			{
				return;
			}
			try
			{
				UnixFileMode mode = File.GetUnixFileMode(path);
				mode |= 73;
				File.SetUnixFileMode(path, mode);
			}
			catch (Exception ex)
			{
				Logger server = Logger.server;
				if (server != null)
				{
					server.Warning("Could not set execute bit on FFmpeg: " + ex.Message);
				}
			}
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x00036680 File Offset: 0x00034880
		private static string UnwrapMessage(Exception ex)
		{
			AggregateException aggregate = ex as AggregateException;
			if (aggregate != null && aggregate.InnerException != null)
			{
				return aggregate.InnerException.Message;
			}
			return ((ex != null) ? ex.Message : null) ?? "unknown error";
		}

		// Token: 0x04000469 RID: 1129
		private static string modFolder;

		// Token: 0x0400046A RID: 1130
		private static readonly object ensureLock = new object();

		// Token: 0x0400046B RID: 1131
		private static Task ensureTask;

		// Token: 0x0400046C RID: 1132
		private static string lastFailureMessage;
	}
}
