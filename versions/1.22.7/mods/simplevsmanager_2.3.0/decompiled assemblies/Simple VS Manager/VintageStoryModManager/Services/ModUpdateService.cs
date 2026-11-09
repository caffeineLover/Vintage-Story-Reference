using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000079 RID: 121
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ModUpdateService
	{
		// Token: 0x06000AF0 RID: 2800 RVA: 0x00044D0C File Offset: 0x00042F0C
		public Task<ModUpdateResult> UpdateAsync(ModUpdateDescriptor descriptor, bool cacheDownloads, [Nullable(2)] IProgress<ModUpdateProgress> progress = null, CancellationToken cancellationToken = default(CancellationToken))
		{
			ModUpdateService.<UpdateAsync>d__1 <UpdateAsync>d__;
			<UpdateAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ModUpdateResult>.Create();
			<UpdateAsync>d__.descriptor = descriptor;
			<UpdateAsync>d__.cacheDownloads = cacheDownloads;
			<UpdateAsync>d__.progress = progress;
			<UpdateAsync>d__.cancellationToken = cancellationToken;
			<UpdateAsync>d__.<>1__state = -1;
			<UpdateAsync>d__.<>t__builder.Start<ModUpdateService.<UpdateAsync>d__1>(ref <UpdateAsync>d__);
			return <UpdateAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000AF1 RID: 2801 RVA: 0x00044D68 File Offset: 0x00042F68
		private static Task<ModUpdateService.DownloadResult> DownloadAsync(ModUpdateDescriptor descriptor, [Nullable(2)] IProgress<ModUpdateProgress> progress, CancellationToken cancellationToken)
		{
			ModUpdateService.<DownloadAsync>d__2 <DownloadAsync>d__;
			<DownloadAsync>d__.<>t__builder = AsyncTaskMethodBuilder<ModUpdateService.DownloadResult>.Create();
			<DownloadAsync>d__.descriptor = descriptor;
			<DownloadAsync>d__.progress = progress;
			<DownloadAsync>d__.cancellationToken = cancellationToken;
			<DownloadAsync>d__.<>1__state = -1;
			<DownloadAsync>d__.<>t__builder.Start<ModUpdateService.<DownloadAsync>d__2>(ref <DownloadAsync>d__);
			return <DownloadAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000AF2 RID: 2802 RVA: 0x00044DBC File Offset: 0x00042FBC
		private static void ValidateArchive(string downloadPath)
		{
			try
			{
				using (ZipArchive zipArchive = ZipFile.OpenRead(downloadPath))
				{
					using (IEnumerator<ZipArchiveEntry> enumerator = zipArchive.Entries.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (string.Equals(Path.GetFileName(enumerator.Current.FullName), "modinfo.json", 5))
							{
								return;
							}
						}
					}
				}
			}
			catch (InvalidDataException ex)
			{
				throw new InvalidDataException("The downloaded file is not a valid Vintage Story mod archive.", ex);
			}
			throw new InvalidDataException("The downloaded file does not contain a modinfo.json manifest.");
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x00044E60 File Offset: 0x00043060
		private static Task<ModUpdateResult> InstallAsync(ModUpdateDescriptor descriptor, string downloadPath, bool treatAsDirectory, [Nullable(2)] IProgress<ModUpdateProgress> progress, CancellationToken cancellationToken)
		{
			string targetPath = descriptor.TargetPath;
			bool flag = treatAsDirectory ? Directory.Exists(targetPath) : File.Exists(targetPath);
			bool flag2 = string.IsNullOrWhiteSpace(descriptor.InstalledVersion) || !flag;
			string text = flag2 ? "Mod installed." : "Update installed.";
			if (treatAsDirectory)
			{
				ModUpdateService.ReportProgress(progress, ModUpdateStage.Preparing, "Preparing extracted files...", default(double?), default(double?));
				return Task.FromResult<ModUpdateResult>(ModUpdateService.InstallToDirectory(descriptor, targetPath, downloadPath, text, progress, cancellationToken));
			}
			string message = flag2 ? "Installing mod archive..." : "Replacing mod archive...";
			ModUpdateService.ReportProgress(progress, ModUpdateStage.Replacing, message, default(double?), default(double?));
			ModUpdateService.InstallToFile(descriptor, downloadPath);
			ModUpdateService.ReportProgress(progress, ModUpdateStage.Completed, text, default(double?), default(double?));
			return Task.FromResult<ModUpdateResult>(new ModUpdateResult(true, null));
		}

		// Token: 0x06000AF4 RID: 2804 RVA: 0x00044F40 File Offset: 0x00043140
		private static ModUpdateResult InstallToDirectory(ModUpdateDescriptor descriptor, string targetDirectory, string downloadPath, string completionMessage, [Nullable(2)] IProgress<ModUpdateProgress> progress, CancellationToken cancellationToken)
		{
			string text = ModUpdateService.CreateUniquePath(targetDirectory, ".immbackup");
			string text2 = ModUpdateService.CreateTemporaryDirectory();
			bool flag = false;
			ModUpdateResult result;
			try
			{
				if (Directory.Exists(targetDirectory))
				{
					if (Directory.Exists(text))
					{
						Directory.Delete(text, true);
					}
					Directory.Move(targetDirectory, text);
					flag = true;
					ModUpdateService.TryCacheDirectoryBackup(descriptor, text);
				}
				ZipFile.ExtractToDirectory(downloadPath, text2);
				ModUpdateService.CopyDirectory(ModUpdateService.DeterminePayloadRoot(text2), targetDirectory, cancellationToken);
				ModUpdateService.ReportProgress(progress, ModUpdateStage.Completed, completionMessage, default(double?), default(double?));
				ModUpdateService.TryDelete(text);
				result = new ModUpdateResult(true, null);
			}
			catch (OperationCanceledException)
			{
				ModUpdateService.TryDelete(targetDirectory);
				if (flag)
				{
					ModUpdateService.TryRestoreDirectoryBackup(text, targetDirectory);
				}
				throw;
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is InvalidDataException || ex is NotSupportedException)
			{
				ModUpdateService.TryDelete(targetDirectory);
				if (flag)
				{
					ModUpdateService.TryRestoreDirectoryBackup(text, targetDirectory);
				}
				result = new ModUpdateResult(false, ex.Message);
			}
			catch
			{
				ModUpdateService.TryDelete(targetDirectory);
				if (flag)
				{
					ModUpdateService.TryRestoreDirectoryBackup(text, targetDirectory);
				}
				throw;
			}
			finally
			{
				ModUpdateService.TryDelete(text2);
			}
			return result;
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x00045094 File Offset: 0x00043294
		private static void InstallToFile(ModUpdateDescriptor descriptor, string downloadPath)
		{
			string targetPath = descriptor.TargetPath;
			string text = string.IsNullOrWhiteSpace(descriptor.ExistingPath) ? targetPath : descriptor.ExistingPath;
			string directoryName = Path.GetDirectoryName(targetPath);
			if (!string.IsNullOrWhiteSpace(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			string text2 = ModUpdateService.CreateUniquePath(text, ".immbackup");
			string text3 = null;
			if (File.Exists(text))
			{
				if (File.Exists(text2))
				{
					File.Delete(text2);
				}
				File.Move(text, text2);
				text3 = ModUpdateService.TryMoveBackupToCache(descriptor, text2);
			}
			try
			{
				if (File.Exists(targetPath))
				{
					File.Delete(targetPath);
				}
				File.Copy(downloadPath, targetPath, true);
				if (!string.Equals(text, targetPath, 5))
				{
					ModUpdateService.TryDelete(text);
				}
				if (text3 == null)
				{
					ModUpdateService.TryDelete(text2);
				}
			}
			catch (Exception)
			{
				ModUpdateService.TryDelete(targetPath);
				if (!string.Equals(text, targetPath, 5))
				{
					ModUpdateService.TryDelete(text);
				}
				if (text3 != null && File.Exists(text3))
				{
					try
					{
						File.Copy(text3, text, true);
						goto IL_1AC;
					}
					catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is NotSupportedException)
					{
						Trace.TraceWarning("Failed to restore mod from cache {0}: {1}", new object[]
						{
							text3,
							ex.Message
						});
						try
						{
							File.Move(text3, text);
							text3 = null;
						}
						catch (Exception ex2) when (ex2 is IOException || ex2 is UnauthorizedAccessException || ex2 is NotSupportedException)
						{
							Trace.TraceWarning("Failed to move cached mod back to {0}: {1}", new object[]
							{
								text,
								ex2.Message
							});
						}
						goto IL_1AC;
					}
				}
				if (File.Exists(text2))
				{
					ModUpdateService.TryDelete(text);
					File.Move(text2, text);
				}
				IL_1AC:
				throw;
			}
			finally
			{
				if (text3 == null)
				{
					ModUpdateService.TryDelete(text2);
				}
			}
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x000452C0 File Offset: 0x000434C0
		private static string DeterminePayloadRoot(string extractDirectory)
		{
			string[] directories = Directory.GetDirectories(extractDirectory, "*", 0);
			string[] files = Directory.GetFiles(extractDirectory, "*", 0);
			if (directories.Length == 1 && files.Length == 0)
			{
				return directories[0];
			}
			return extractDirectory;
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x000452F8 File Offset: 0x000434F8
		[return: Nullable(2)]
		private static string TryMoveBackupToCache(ModUpdateDescriptor descriptor, string backupPath)
		{
			if (string.IsNullOrWhiteSpace(descriptor.InstalledVersion))
			{
				return null;
			}
			if (!File.Exists(backupPath))
			{
				return null;
			}
			string text = (descriptor.ExistingPath != null) ? Path.GetFileName(descriptor.ExistingPath) : null;
			if (string.IsNullOrWhiteSpace(text))
			{
				text = Path.GetFileName(descriptor.TargetPath);
			}
			if (string.IsNullOrWhiteSpace(text))
			{
				text = descriptor.ReleaseFileName;
			}
			string modCachePath = ModCacheLocator.GetModCachePath(descriptor.ModId, descriptor.InstalledVersion, text);
			if (modCachePath == null)
			{
				return null;
			}
			if (ModCacheLocator.TryPromoteLegacyCacheFile(descriptor.ModId, descriptor.InstalledVersion, text, modCachePath) && File.Exists(modCachePath))
			{
				return null;
			}
			if (File.Exists(modCachePath))
			{
				return null;
			}
			string text2;
			if (ModCacheLocator.TryLocateCachedModFile(descriptor.ModId, descriptor.InstalledVersion, text, out text2) && text2 != null)
			{
				string directoryName = Path.GetDirectoryName(text2);
				string directoryName2 = Path.GetDirectoryName(modCachePath);
				if (!string.IsNullOrWhiteSpace(directoryName) && !string.IsNullOrWhiteSpace(directoryName2) && string.Equals(directoryName, directoryName2, 5))
				{
					return null;
				}
			}
			string result;
			try
			{
				string directoryName3 = Path.GetDirectoryName(modCachePath);
				if (!string.IsNullOrWhiteSpace(directoryName3))
				{
					Directory.CreateDirectory(directoryName3);
				}
				File.Move(backupPath, modCachePath);
				result = modCachePath;
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is NotSupportedException)
			{
				Trace.TraceWarning("Failed to cache existing mod version {0}: {1}", new object[]
				{
					modCachePath,
					ex.Message
				});
				try
				{
					if (!File.Exists(backupPath) && File.Exists(modCachePath))
					{
						File.Move(modCachePath, backupPath);
					}
				}
				catch (Exception ex2) when (ex2 is IOException || ex2 is UnauthorizedAccessException || ex2 is NotSupportedException)
				{
					Trace.TraceWarning("Failed to restore backup after cache failure {0}: {1}", new object[]
					{
						modCachePath,
						ex2.Message
					});
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x000454F4 File Offset: 0x000436F4
		private static void TryCacheDirectoryBackup(ModUpdateDescriptor descriptor, string backupPath)
		{
			if (string.IsNullOrWhiteSpace(descriptor.InstalledVersion))
			{
				return;
			}
			if (!Directory.Exists(backupPath))
			{
				return;
			}
			string fileName = descriptor.ReleaseFileName ?? Path.GetFileName(descriptor.TargetPath);
			string modCachePath = ModCacheLocator.GetModCachePath(descriptor.ModId, descriptor.InstalledVersion, fileName);
			if (modCachePath == null)
			{
				return;
			}
			if (ModCacheLocator.TryPromoteLegacyCacheFile(descriptor.ModId, descriptor.InstalledVersion, fileName, modCachePath) && File.Exists(modCachePath))
			{
				return;
			}
			if (File.Exists(modCachePath))
			{
				return;
			}
			string text;
			if (ModCacheLocator.TryLocateCachedModFile(descriptor.ModId, descriptor.InstalledVersion, fileName, out text) && text != null)
			{
				string directoryName = Path.GetDirectoryName(text);
				string directoryName2 = Path.GetDirectoryName(modCachePath);
				if (!string.IsNullOrWhiteSpace(directoryName) && !string.IsNullOrWhiteSpace(directoryName2) && string.Equals(directoryName, directoryName2, 5))
				{
					return;
				}
			}
			string text2 = ModUpdateService.CreateTemporaryDirectory();
			string text3 = Path.Combine(text2, Path.GetFileName(modCachePath));
			try
			{
				string directoryName3 = Path.GetDirectoryName(modCachePath);
				if (!string.IsNullOrWhiteSpace(directoryName3))
				{
					Directory.CreateDirectory(directoryName3);
				}
				ZipFile.CreateFromDirectory(backupPath, text3, 0, false);
				if (File.Exists(modCachePath))
				{
					File.Delete(modCachePath);
				}
				File.Move(text3, modCachePath);
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is NotSupportedException || ex is InvalidDataException)
			{
				Trace.TraceWarning("Failed to cache directory mod backup {0}: {1}", new object[]
				{
					backupPath,
					ex.Message
				});
				ModUpdateService.TryDelete(text3);
			}
			finally
			{
				ModUpdateService.TryDelete(text2);
			}
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x00045690 File Offset: 0x00043890
		private static void CopyDirectory(string sourceDirectory, string destinationDirectory, CancellationToken cancellationToken)
		{
			Stack<string> stack = new Stack<string>();
			stack.Push(sourceDirectory);
			while (stack.Count > 0)
			{
				cancellationToken.ThrowIfCancellationRequested();
				string text = stack.Pop();
				string relativePath = Path.GetRelativePath(sourceDirectory, text);
				Directory.CreateDirectory((relativePath == ".") ? destinationDirectory : Path.Combine(destinationDirectory, relativePath));
				foreach (string text2 in Directory.GetFiles(text))
				{
					string relativePath2 = Path.GetRelativePath(sourceDirectory, text2);
					string text3 = Path.Combine(destinationDirectory, relativePath2);
					Directory.CreateDirectory(Path.GetDirectoryName(text3));
					File.Copy(text2, text3, true);
				}
				foreach (string text4 in Directory.GetDirectories(text))
				{
					stack.Push(text4);
				}
			}
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x00045760 File Offset: 0x00043960
		private static string CreateTemporaryDirectory()
		{
			string text = Path.Combine(Path.GetTempPath(), "IMM", Guid.NewGuid().ToString("N"));
			Directory.CreateDirectory(text);
			return text;
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x00045798 File Offset: 0x00043998
		private static string CreateUniquePath(string basePath, string suffix)
		{
			string text = basePath + suffix;
			int num = 1;
			while (File.Exists(text) || Directory.Exists(text))
			{
				text = basePath + suffix + num++.ToString();
			}
			return text;
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x000457D8 File Offset: 0x000439D8
		private static void TryCacheDownload(string sourcePath, string cachePath)
		{
			try
			{
				if (!string.Equals(sourcePath, cachePath, 5))
				{
					string directoryName = Path.GetDirectoryName(cachePath);
					if (!string.IsNullOrWhiteSpace(directoryName))
					{
						Directory.CreateDirectory(directoryName);
					}
					File.Copy(sourcePath, cachePath, true);
				}
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is NotSupportedException)
			{
				Trace.TraceWarning("Failed to cache mod archive {0}: {1}", new object[]
				{
					cachePath,
					ex.Message
				});
			}
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x00045874 File Offset: 0x00043A74
		private static void TryDelete(string path)
		{
			try
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				else if (Directory.Exists(path))
				{
					Directory.Delete(path, true);
				}
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
			{
				Trace.TraceWarning("Failed to delete temporary resource {0}: {1}", new object[]
				{
					path,
					ex.Message
				});
			}
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x000458FC File Offset: 0x00043AFC
		private static void TryRestoreDirectoryBackup(string backupPath, string targetPath)
		{
			if (!Directory.Exists(backupPath))
			{
				return;
			}
			try
			{
				if (Directory.Exists(targetPath))
				{
					Directory.Delete(targetPath, true);
				}
				Directory.Move(backupPath, targetPath);
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
			{
				Trace.TraceWarning("Failed to restore mod directory backup {0}: {1}", new object[]
				{
					backupPath,
					ex.Message
				});
			}
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x00045984 File Offset: 0x00043B84
		private static void ReportProgress([Nullable(2)] IProgress<ModUpdateProgress> progress, ModUpdateStage stage, string message, double? percent = null, double? bytesPerSecond = null)
		{
			if (progress != null)
			{
				progress.Report(new ModUpdateProgress(stage, message, percent, bytesPerSecond));
			}
		}

		// Token: 0x0400050C RID: 1292
		private static readonly HttpClient HttpClient = new HttpClient();

		// Token: 0x020002E2 RID: 738
		[Nullable(0)]
		private sealed class DownloadResult : IEquatable<ModUpdateService.DownloadResult>
		{
			// Token: 0x0600185D RID: 6237 RVA: 0x0008CB11 File Offset: 0x0008AD11
			public DownloadResult(string Path, bool IsTemporary, [Nullable(2)] string CachePath, bool IsCacheHit)
			{
				this.Path = Path;
				this.IsTemporary = IsTemporary;
				this.CachePath = CachePath;
				this.IsCacheHit = IsCacheHit;
				base..ctor();
			}

			// Token: 0x17000512 RID: 1298
			// (get) Token: 0x0600185E RID: 6238 RVA: 0x0008CB36 File Offset: 0x0008AD36
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(ModUpdateService.DownloadResult);
				}
			}

			// Token: 0x17000513 RID: 1299
			// (get) Token: 0x0600185F RID: 6239 RVA: 0x0008CB42 File Offset: 0x0008AD42
			// (set) Token: 0x06001860 RID: 6240 RVA: 0x0008CB4A File Offset: 0x0008AD4A
			public string Path { get; set; }

			// Token: 0x17000514 RID: 1300
			// (get) Token: 0x06001861 RID: 6241 RVA: 0x0008CB53 File Offset: 0x0008AD53
			// (set) Token: 0x06001862 RID: 6242 RVA: 0x0008CB5B File Offset: 0x0008AD5B
			public bool IsTemporary { get; set; }

			// Token: 0x17000515 RID: 1301
			// (get) Token: 0x06001863 RID: 6243 RVA: 0x0008CB64 File Offset: 0x0008AD64
			// (set) Token: 0x06001864 RID: 6244 RVA: 0x0008CB6C File Offset: 0x0008AD6C
			[Nullable(2)]
			public string CachePath { [NullableContext(2)] get; [NullableContext(2)] set; }

			// Token: 0x17000516 RID: 1302
			// (get) Token: 0x06001865 RID: 6245 RVA: 0x0008CB75 File Offset: 0x0008AD75
			// (set) Token: 0x06001866 RID: 6246 RVA: 0x0008CB7D File Offset: 0x0008AD7D
			public bool IsCacheHit { get; set; }

			// Token: 0x06001867 RID: 6247 RVA: 0x0008CB88 File Offset: 0x0008AD88
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("DownloadResult");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x06001868 RID: 6248 RVA: 0x0008CBD4 File Offset: 0x0008ADD4
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("Path = ");
				builder.Append(this.Path);
				builder.Append(", IsTemporary = ");
				builder.Append(this.IsTemporary.ToString());
				builder.Append(", CachePath = ");
				builder.Append(this.CachePath);
				builder.Append(", IsCacheHit = ");
				builder.Append(this.IsCacheHit.ToString());
				return true;
			}

			// Token: 0x06001869 RID: 6249 RVA: 0x0008CC67 File Offset: 0x0008AE67
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(ModUpdateService.DownloadResult left, ModUpdateService.DownloadResult right)
			{
				return !(left == right);
			}

			// Token: 0x0600186A RID: 6250 RVA: 0x0008CC73 File Offset: 0x0008AE73
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(ModUpdateService.DownloadResult left, ModUpdateService.DownloadResult right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x0600186B RID: 6251 RVA: 0x0008CC88 File Offset: 0x0008AE88
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Path>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<IsTemporary>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<CachePath>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<IsCacheHit>k__BackingField);
			}

			// Token: 0x0600186C RID: 6252 RVA: 0x0008CD01 File Offset: 0x0008AF01
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as ModUpdateService.DownloadResult);
			}

			// Token: 0x0600186D RID: 6253 RVA: 0x0008CD10 File Offset: 0x0008AF10
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(ModUpdateService.DownloadResult other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<Path>k__BackingField, other.<Path>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<IsTemporary>k__BackingField, other.<IsTemporary>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<CachePath>k__BackingField, other.<CachePath>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<IsCacheHit>k__BackingField, other.<IsCacheHit>k__BackingField));
			}

			// Token: 0x0600186F RID: 6255 RVA: 0x0008CDA1 File Offset: 0x0008AFA1
			[CompilerGenerated]
			private DownloadResult(ModUpdateService.DownloadResult original)
			{
				this.Path = original.<Path>k__BackingField;
				this.IsTemporary = original.<IsTemporary>k__BackingField;
				this.CachePath = original.<CachePath>k__BackingField;
				this.IsCacheHit = original.<IsCacheHit>k__BackingField;
			}

			// Token: 0x06001870 RID: 6256 RVA: 0x0008CDD9 File Offset: 0x0008AFD9
			[CompilerGenerated]
			public void Deconstruct(out string Path, out bool IsTemporary, [Nullable(2)] out string CachePath, out bool IsCacheHit)
			{
				Path = this.Path;
				IsTemporary = this.IsTemporary;
				CachePath = this.CachePath;
				IsCacheHit = this.IsCacheHit;
			}
		}
	}
}
