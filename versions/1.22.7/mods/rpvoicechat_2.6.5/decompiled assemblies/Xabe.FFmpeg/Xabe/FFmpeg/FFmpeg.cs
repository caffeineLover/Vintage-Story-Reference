using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Xabe.FFmpeg.Exceptions;

namespace Xabe.FFmpeg
{
	// Token: 0x0200001A RID: 26
	public abstract class FFmpeg
	{
		// Token: 0x060000BD RID: 189 RVA: 0x000045B8 File Offset: 0x000027B8
		protected FFmpeg()
		{
			this.FindAndValidateExecutables();
		}

		// Token: 0x060000BE RID: 190 RVA: 0x000045C8 File Offset: 0x000027C8
		private void FindAndValidateExecutables()
		{
			if (!string.IsNullOrWhiteSpace(this.FFprobePath) && !string.IsNullOrWhiteSpace(this.FFmpegPath) && FFmpeg._lastExecutablePath.Equals(FFmpeg.ExecutablesPath))
			{
				return;
			}
			if (!string.IsNullOrWhiteSpace(FFmpeg.ExecutablesPath))
			{
				FileInfo[] files = new DirectoryInfo(FFmpeg.ExecutablesPath).GetFiles();
				Func<string, string, IFormatProvider, bool> compareMethod;
				switch (FFmpeg.FilterMethod)
				{
				case FileNameFilterMethod.Contains:
					compareMethod = ((string path, string exec, IFormatProvider provider) => path.ToString(provider).Contains(exec));
					break;
				case FileNameFilterMethod.Exact:
					compareMethod = ((string path, string exec, IFormatProvider provider) => path.ToString(provider).Equals(exec));
					break;
				case FileNameFilterMethod.StartWith:
					compareMethod = ((string path, string exec, IFormatProvider provider) => path.ToString(provider).StartsWith(exec));
					break;
				default:
					compareMethod = ((string path, string exec, IFormatProvider provider) => path.ToString(provider).Contains(exec));
					break;
				}
				FileInfo fileInfo = Enumerable.FirstOrDefault<FileInfo>(files, (FileInfo x) => compareMethod.Invoke(x.Name, FFmpeg._ffprobeExecutableName, FFmpeg.FormatProvider) && this.IsExecutable(x.FullName, null, null));
				this.FFprobePath = ((fileInfo != null) ? fileInfo.FullName : null);
				FileInfo fileInfo2 = Enumerable.FirstOrDefault<FileInfo>(files, (FileInfo x) => compareMethod.Invoke(x.Name, FFmpeg._ffmpegExecutableName, FFmpeg.FormatProvider) && this.IsExecutable(x.FullName, null, null));
				this.FFmpegPath = ((fileInfo2 != null) ? fileInfo2.FullName : null);
				this.ValidateExecutables();
				FFmpeg._lastExecutablePath = FFmpeg.ExecutablesPath;
				return;
			}
			Assembly entryAssembly = Assembly.GetEntryAssembly();
			if (entryAssembly != null)
			{
				string directoryName = Path.GetDirectoryName(entryAssembly.Location);
				this.FindProgramsFromPath(directoryName);
				if (this.FFmpegPath != null && this.FFprobePath != null)
				{
					return;
				}
			}
			foreach (string path2 in Environment.GetEnvironmentVariable("PATH").Split(new char[]
			{
				Path.PathSeparator
			}))
			{
				this.FindProgramsFromPath(path2);
				if (this.FFmpegPath != null && this.FFprobePath != null)
				{
					break;
				}
			}
			this.ValidateExecutables();
		}

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x060000BF RID: 191 RVA: 0x000047C8 File Offset: 0x000029C8
		// (set) Token: 0x060000C0 RID: 192 RVA: 0x00004808 File Offset: 0x00002A08
		private protected string FFmpegPath
		{
			protected get
			{
				object ffmpegPathLock = FFmpeg._ffmpegPathLock;
				string ffmpegPath;
				lock (ffmpegPathLock)
				{
					ffmpegPath = FFmpeg._ffmpegPath;
				}
				return ffmpegPath;
			}
			private set
			{
				object ffmpegPathLock = FFmpeg._ffmpegPathLock;
				lock (ffmpegPathLock)
				{
					FFmpeg._ffmpegPath = value;
				}
			}
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00004848 File Offset: 0x00002A48
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00004888 File Offset: 0x00002A88
		private protected string FFprobePath
		{
			protected get
			{
				object ffprobePathLock = FFmpeg._ffprobePathLock;
				string ffprobePath;
				lock (ffprobePathLock)
				{
					ffprobePath = FFmpeg._ffprobePath;
				}
				return ffprobePath;
			}
			private set
			{
				object ffprobePathLock = FFmpeg._ffprobePathLock;
				lock (ffprobePathLock)
				{
					FFmpeg._ffprobePath = value;
				}
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000048C8 File Offset: 0x00002AC8
		private void ValidateExecutables()
		{
			if (this.FFmpegPath != null && this.FFprobePath != null)
			{
				return;
			}
			string text = string.IsNullOrWhiteSpace(FFmpeg.ExecutablesPath) ? string.Empty : string.Format(FFmpeg.ExecutablesPath + " or ", Array.Empty<object>());
			throw new FFmpegNotFoundException("Cannot find FFmpeg in " + text + "PATH. This package needs installed FFmpeg. Please add it to your PATH variable or specify path to DIRECTORY with FFmpeg executables in FFmpeg.ExecutablesPath");
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00004928 File Offset: 0x00002B28
		private bool IsExecutable(string file, OperatingSystemProvider systemProvider = null, OperatingSystemArchitectureProvider architectureProvider = null)
		{
			try
			{
				using (FileStream fileStream = File.OpenRead(file))
				{
					byte[] array = new byte[4];
					byte[] array2 = new byte[4];
					fileStream.Read(array, 0, 4);
					OperatingSystemProvider operatingSystemProvider = systemProvider ?? new OperatingSystemProvider();
					OperatingSystemArchitecture architecture = (architectureProvider ?? new OperatingSystemArchitectureProvider()).GetArchitecture();
					switch (operatingSystemProvider.GetOperatingSystem())
					{
					case OperatingSystem.Windows:
						return array[0] == 77 && array[1] == 90;
					case OperatingSystem.Osx:
						return array[0] == 206 && array[1] == 250 && array[2] == 237 && array[3] == 254;
					case OperatingSystem.Linux:
						if (architecture == OperatingSystemArchitecture.X86 || architecture == OperatingSystemArchitecture.X64)
						{
							return array[0] == 127 && array[1] == 69 && array[2] == 76 && array[3] == 70;
						}
						fileStream.Seek(48L, 0);
						fileStream.Read(array2, 0, 4);
						return array2[0] == 80 && array2[1] == 75 && array2[2] == 3 && array2[3] == 4;
					}
				}
			}
			catch (Exception)
			{
			}
			return false;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00004A80 File Offset: 0x00002C80
		private void FindProgramsFromPath(string path)
		{
			if (!Directory.Exists(path))
			{
				return;
			}
			IEnumerable<FileInfo> files = new DirectoryInfo(path).GetFiles();
			this.FFprobePath = FFmpeg.GetFullName(files, FFmpeg._ffprobeExecutableName);
			this.FFmpegPath = FFmpeg.GetFullName(files, FFmpeg._ffmpegExecutableName);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00004AC4 File Offset: 0x00002CC4
		internal static string GetFullName(IEnumerable<FileInfo> files, string fileName)
		{
			FileInfo fileInfo = Enumerable.FirstOrDefault<FileInfo>(files, (FileInfo x) => x.Name.Equals(fileName, 3) || x.Name.Equals(fileName + ".exe", 3));
			if (fileInfo == null)
			{
				return null;
			}
			return fileInfo.FullName;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00004AFC File Offset: 0x00002CFC
		protected Process RunProcess(string args, string processPath, ProcessPriorityClass? priority, bool standardInput = false, bool standardOutput = false, bool standardError = false)
		{
			Process process = new Process
			{
				StartInfo = 
				{
					FileName = processPath,
					Arguments = args,
					UseShellExecute = false,
					CreateNoWindow = true,
					RedirectStandardInput = standardInput,
					RedirectStandardOutput = standardOutput,
					RedirectStandardError = standardError
				},
				EnableRaisingEvents = true
			};
			process.Start();
			try
			{
				process.PriorityClass = (priority ?? Process.GetCurrentProcess().PriorityClass);
			}
			catch (Exception)
			{
			}
			return process;
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00004BB0 File Offset: 0x00002DB0
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00004BB7 File Offset: 0x00002DB7
		public static string ExecutablesPath { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00004BBF File Offset: 0x00002DBF
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00004BC6 File Offset: 0x00002DC6
		public static FileNameFilterMethod FilterMethod { get; private set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00004BCE File Offset: 0x00002DCE
		// (set) Token: 0x060000CD RID: 205 RVA: 0x00004BD5 File Offset: 0x00002DD5
		public static IFormatProvider FormatProvider { get; private set; }

		// Token: 0x060000CE RID: 206 RVA: 0x00004BE0 File Offset: 0x00002DE0
		public static Task<IMediaInfo> GetMediaInfo(string fileName)
		{
			FFmpeg.<GetMediaInfo>d__33 <GetMediaInfo>d__;
			<GetMediaInfo>d__.<>t__builder = AsyncTaskMethodBuilder<IMediaInfo>.Create();
			<GetMediaInfo>d__.fileName = fileName;
			<GetMediaInfo>d__.<>1__state = -1;
			<GetMediaInfo>d__.<>t__builder.Start<FFmpeg.<GetMediaInfo>d__33>(ref <GetMediaInfo>d__);
			return <GetMediaInfo>d__.<>t__builder.Task;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00004C24 File Offset: 0x00002E24
		public static Task<IMediaInfo> GetMediaInfo(string fileName, CancellationToken token)
		{
			FFmpeg.<GetMediaInfo>d__34 <GetMediaInfo>d__;
			<GetMediaInfo>d__.<>t__builder = AsyncTaskMethodBuilder<IMediaInfo>.Create();
			<GetMediaInfo>d__.fileName = fileName;
			<GetMediaInfo>d__.token = token;
			<GetMediaInfo>d__.<>1__state = -1;
			<GetMediaInfo>d__.<>t__builder.Start<FFmpeg.<GetMediaInfo>d__34>(ref <GetMediaInfo>d__);
			return <GetMediaInfo>d__.<>t__builder.Task;
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00004C6F File Offset: 0x00002E6F
		public static void SetExecutablesPath(string directoryWithFFmpegAndFFprobe, string ffmpegExeutableName = "ffmpeg", string ffprobeExecutableName = "ffprobe", FileNameFilterMethod filteringMethod = FileNameFilterMethod.Contains, IFormatProvider formatprovider = null)
		{
			FFmpeg.ExecutablesPath = ((directoryWithFFmpegAndFFprobe == null) ? null : new DirectoryInfo(directoryWithFFmpegAndFFprobe).FullName);
			FFmpeg.FilterMethod = filteringMethod;
			FFmpeg.FormatProvider = (formatprovider ?? CultureInfo.CurrentCulture);
			FFmpeg._ffmpegExecutableName = ffmpegExeutableName;
			FFmpeg._ffprobeExecutableName = ffprobeExecutableName;
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00004CAC File Offset: 0x00002EAC
		internal static Task<Device[]> GetAvailableDevices()
		{
			FFmpeg.<GetAvailableDevices>d__36 <GetAvailableDevices>d__;
			<GetAvailableDevices>d__.<>t__builder = AsyncTaskMethodBuilder<Device[]>.Create();
			<GetAvailableDevices>d__.<>1__state = -1;
			<GetAvailableDevices>d__.<>t__builder.Start<FFmpeg.<GetAvailableDevices>d__36>(ref <GetAvailableDevices>d__);
			return <GetAvailableDevices>d__.<>t__builder.Task;
		}

		// Token: 0x04000285 RID: 645
		private static string _ffmpegPath;

		// Token: 0x04000286 RID: 646
		private static string _ffprobePath;

		// Token: 0x04000287 RID: 647
		private static string _lastExecutablePath = Guid.NewGuid().ToString();

		// Token: 0x04000288 RID: 648
		private static readonly object _ffmpegPathLock = new object();

		// Token: 0x04000289 RID: 649
		private static readonly object _ffprobePathLock = new object();

		// Token: 0x0400028A RID: 650
		private static string _ffmpegExecutableName = "ffmpeg";

		// Token: 0x0400028B RID: 651
		private static string _ffprobeExecutableName = "ffprobe";

		// Token: 0x0400028F RID: 655
		public static Conversions Conversions = new Conversions();
	}
}
