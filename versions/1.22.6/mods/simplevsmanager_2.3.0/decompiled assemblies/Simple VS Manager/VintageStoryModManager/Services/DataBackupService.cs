using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000059 RID: 89
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DataBackupService
	{
		// Token: 0x060008DF RID: 2271 RVA: 0x00035F94 File Offset: 0x00034194
		public DataBackupService(string configurationDirectory, [Nullable(2)] string customBackupLocation = null)
		{
			if (string.IsNullOrWhiteSpace(configurationDirectory))
			{
				throw new ArgumentException("Configuration directory is required.", "configurationDirectory");
			}
			if (!string.IsNullOrWhiteSpace(customBackupLocation))
			{
				this._backupRootDirectory = customBackupLocation;
			}
			else
			{
				this._backupRootDirectory = Path.Combine(configurationDirectory, DevConfig.DataFolderBackupDirectoryName);
			}
			this._savesStoreDirectory = Path.Combine(this._backupRootDirectory, DevConfig.DataFolderBackupSaveStoreName);
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x00035FF8 File Offset: 0x000341F8
		public Task<DataBackupResult> CreateBackupAsync(string dataDirectory, [Nullable(2)] string vintageStoryVersion, [Nullable(2)] IProgress<DataBackupProgress> progress, CancellationToken cancellationToken)
		{
			return Task.Run<DataBackupResult>(() => this.CreateBackupInternal(dataDirectory, vintageStoryVersion, progress, cancellationToken), cancellationToken);
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x00036048 File Offset: 0x00034248
		public Task RestoreBackupAsync(DataBackupSummary summary, string dataDirectory, [Nullable(2)] IProgress<DataBackupProgress> progress, CancellationToken cancellationToken)
		{
			if (summary == null)
			{
				throw new ArgumentNullException("summary");
			}
			return Task.Run(delegate()
			{
				this.RestoreBackupInternal(summary, dataDirectory, progress, cancellationToken);
			}, cancellationToken);
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x000360AC File Offset: 0x000342AC
		public IReadOnlyList<DataBackupSummary> GetAvailableBackups()
		{
			if (!Directory.Exists(this._backupRootDirectory))
			{
				return Array.Empty<DataBackupSummary>();
			}
			List<DataBackupSummary> list = new List<DataBackupSummary>();
			foreach (string text in Directory.EnumerateDirectories(this._backupRootDirectory))
			{
				if (!string.Equals(Path.GetFileName(text), DevConfig.DataFolderBackupSaveStoreName, 5))
				{
					DataBackupService.BackupManifest backupManifest = this.TryLoadManifest(text);
					if (backupManifest != null && !(backupManifest.CreatedOnUtc == default(DateTime)))
					{
						list.Add(new DataBackupSummary(backupManifest.Id ?? Path.GetFileName(text), backupManifest.CreatedOnUtc, text, backupManifest.SourceDataDirectory, backupManifest.VintageStoryVersion));
					}
				}
			}
			foreach (string text2 in Directory.EnumerateFiles(this._backupRootDirectory, "*.zip"))
			{
				DataBackupService.BackupManifest backupManifest2 = this.TryLoadManifestFromZip(text2);
				if (backupManifest2 != null && !(backupManifest2.CreatedOnUtc == default(DateTime)))
				{
					list.Add(new DataBackupSummary(backupManifest2.Id ?? Path.GetFileNameWithoutExtension(text2), backupManifest2.CreatedOnUtc, text2, backupManifest2.SourceDataDirectory, backupManifest2.VintageStoryVersion));
				}
			}
			return Enumerable.ToArray<DataBackupSummary>(Enumerable.OrderByDescending<DataBackupSummary, DateTime>(list, (DataBackupSummary summary) => summary.CreatedOnUtc));
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x00036238 File Offset: 0x00034438
		public int DeleteBackups(string dataDirectory, string vintageStoryVersion)
		{
			if (string.IsNullOrWhiteSpace(dataDirectory))
			{
				throw new ArgumentException("A data directory must be provided.", "dataDirectory");
			}
			if (string.IsNullOrWhiteSpace(vintageStoryVersion))
			{
				throw new ArgumentException("Vintage Story version is required.", "vintageStoryVersion");
			}
			string text = DataBackupService.NormalizeDirectoryPath(dataDirectory);
			if (text == null)
			{
				throw new ArgumentException("A valid data directory must be provided.", "dataDirectory");
			}
			string normalizedTarget = text;
			string text2 = VersionStringUtility.Normalize(vintageStoryVersion);
			if (string.IsNullOrWhiteSpace(text2))
			{
				throw new ArgumentException("A valid Vintage Story version must be provided.", "vintageStoryVersion");
			}
			if (!Directory.Exists(this._backupRootDirectory))
			{
				return 0;
			}
			int num = 0;
			foreach (string text3 in Directory.EnumerateDirectories(this._backupRootDirectory))
			{
				DataBackupService.BackupManifest backupManifest = this.TryLoadManifest(text3);
				if (backupManifest != null && DataBackupService.DirectoriesMatch(backupManifest.SourceDataDirectory, normalizedTarget) && DataBackupService.VersionsMatch(backupManifest.VintageStoryVersion, text2))
				{
					DataBackupService.TryDeleteDirectory(text3);
					num++;
				}
			}
			foreach (string text4 in Directory.EnumerateFiles(this._backupRootDirectory, "*.zip"))
			{
				DataBackupService.BackupManifest backupManifest2 = this.TryLoadManifestFromZip(text4);
				if (backupManifest2 != null && DataBackupService.DirectoriesMatch(backupManifest2.SourceDataDirectory, normalizedTarget) && DataBackupService.VersionsMatch(backupManifest2.VintageStoryVersion, text2))
				{
					DataBackupService.TryDeleteFile(text4);
					num++;
				}
			}
			if (num > 0)
			{
				this.CleanupSaveStore();
			}
			return num;
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x000363B8 File Offset: 0x000345B8
		public int PruneBackups(string dataDirectory, string vintageStoryVersion, int maximumBackups)
		{
			DataBackupService.<>c__DisplayClass12_0 CS$<>8__locals1 = new DataBackupService.<>c__DisplayClass12_0();
			if (maximumBackups <= 0)
			{
				return 0;
			}
			if (string.IsNullOrWhiteSpace(dataDirectory))
			{
				throw new ArgumentException("A data directory must be provided.", "dataDirectory");
			}
			if (string.IsNullOrWhiteSpace(vintageStoryVersion))
			{
				throw new ArgumentException("Vintage Story version is required.", "vintageStoryVersion");
			}
			DataBackupService.<>c__DisplayClass12_0 CS$<>8__locals2 = CS$<>8__locals1;
			string text = DataBackupService.NormalizeDirectoryPath(dataDirectory);
			if (text == null)
			{
				throw new ArgumentException("A valid data directory must be provided.", "dataDirectory");
			}
			CS$<>8__locals2.normalizedSource = text;
			CS$<>8__locals1.normalizedVersion = VersionStringUtility.Normalize(vintageStoryVersion);
			if (string.IsNullOrWhiteSpace(CS$<>8__locals1.normalizedVersion))
			{
				throw new ArgumentException("A valid Vintage Story version must be provided.", "vintageStoryVersion");
			}
			DataBackupSummary[] array = Enumerable.ToArray<DataBackupSummary>(Enumerable.Skip<DataBackupSummary>(Enumerable.OrderByDescending<DataBackupSummary, DateTime>(Enumerable.Where<DataBackupSummary>(Enumerable.Where<DataBackupSummary>(this.GetAvailableBackups(), (DataBackupSummary summary) => DataBackupService.DirectoriesMatch(summary.SourceDataDirectory, CS$<>8__locals1.normalizedSource)), (DataBackupSummary summary) => DataBackupService.VersionsMatch(summary.VintageStoryVersion, CS$<>8__locals1.normalizedVersion)), (DataBackupSummary summary) => summary.CreatedOnUtc), maximumBackups));
			int num = 0;
			DataBackupSummary[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				if (DataBackupService.DeleteBackup(array2[i]))
				{
					num++;
				}
			}
			if (num > 0)
			{
				this.CleanupSaveStore();
			}
			return num;
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x000364CB File Offset: 0x000346CB
		public string GetBackupRootDirectory()
		{
			return this._backupRootDirectory;
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x000364D4 File Offset: 0x000346D4
		private DataBackupResult CreateBackupInternal(string dataDirectory, [Nullable(2)] string vintageStoryVersion, [Nullable(2)] IProgress<DataBackupProgress> progress, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(dataDirectory))
			{
				throw new ArgumentException("A data directory must be provided.", "dataDirectory");
			}
			if (!Directory.Exists(dataDirectory))
			{
				throw new DirectoryNotFoundException("The data directory " + dataDirectory + " does not exist.");
			}
			Directory.CreateDirectory(this._backupRootDirectory);
			Directory.CreateDirectory(this._savesStoreDirectory);
			DateTime utcNow = DateTime.UtcNow;
			string text = this.GenerateBackupIdentifier(utcNow);
			string text2 = DataBackupService.EnsureUniqueDirectory(Path.Combine(this._backupRootDirectory, text));
			string targetDirectory = Path.Combine(text2, "Data");
			string text3 = text2 + ".zip";
			if (progress != null)
			{
				progress.Report(new DataBackupProgress(0.0, "Scanning VintagestoryData..."));
			}
			DataBackupService.BackupPlan plan = this.BuildBackupPlan(dataDirectory, targetDirectory, cancellationToken);
			string text4 = DataBackupService.NormalizeDirectoryPath(dataDirectory);
			string text5 = VersionStringUtility.Normalize(vintageStoryVersion);
			DataBackupService.BackupManifest manifest = new DataBackupService.BackupManifest
			{
				Id = text,
				CreatedOnUtc = utcNow,
				SourceDataDirectory = (text4 ?? dataDirectory),
				VintageStoryVersion = (text5 ?? vintageStoryVersion)
			};
			DataBackupResult result;
			try
			{
				this.ExecuteBackupPlan(plan, manifest, progress, cancellationToken);
				DataBackupService.WriteManifest(Path.Combine(text2, DevConfig.DataFolderBackupManifestFileName), manifest);
				this.CleanupSaveStore();
				try
				{
					if (progress != null)
					{
						progress.Report(new DataBackupProgress(95.0, "Compressing backup..."));
					}
					ZipFile.CreateFromDirectory(text2, text3, 1, false);
					DataBackupService.TryDeleteDirectory(text2);
				}
				catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
				{
					Trace.TraceWarning("Failed to compress backup, keeping uncompressed: {0}", new object[]
					{
						ex.Message
					});
					DataBackupService.TryDeleteFile(text3);
				}
				if (progress != null)
				{
					progress.Report(new DataBackupProgress(100.0, "Backup completed."));
				}
				string directoryPath = File.Exists(text3) ? text3 : text2;
				result = new DataBackupResult(text, utcNow, directoryPath);
			}
			catch
			{
				DataBackupService.TryDeleteDirectory(text2);
				DataBackupService.TryDeleteFile(text3);
				throw;
			}
			return result;
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x000366DC File Offset: 0x000348DC
		private void RestoreBackupInternal(DataBackupSummary summary, string dataDirectory, [Nullable(2)] IProgress<DataBackupProgress> progress, CancellationToken cancellationToken)
		{
			if (string.IsNullOrWhiteSpace(dataDirectory))
			{
				throw new ArgumentException("A destination data directory must be provided.", "dataDirectory");
			}
			bool flag = summary.DirectoryPath.EndsWith(".zip", 5);
			string text;
			if (flag)
			{
				if (!File.Exists(summary.DirectoryPath))
				{
					throw new FileNotFoundException("The backup archive " + summary.DirectoryPath + " could not be found.");
				}
				IProgress<DataBackupProgress> progress2 = progress;
				if (progress2 != null)
				{
					progress2.Report(new DataBackupProgress(0.0, "Extracting backup archive..."));
				}
				text = Path.Combine(Path.GetTempPath(), "svsm-restore-" + Guid.NewGuid().ToString("N"));
				try
				{
					ZipFile.ExtractToDirectory(summary.DirectoryPath, text);
					goto IL_F6;
				}
				catch
				{
					DataBackupService.TryDeleteDirectory(text);
					throw;
				}
			}
			if (!Directory.Exists(summary.DirectoryPath))
			{
				throw new DirectoryNotFoundException("The backup directory " + summary.DirectoryPath + " could not be found.");
			}
			text = summary.DirectoryPath;
			IL_F6:
			try
			{
				DataBackupService.BackupManifest backupManifest = this.TryLoadManifest(text);
				if (backupManifest == null)
				{
					throw new InvalidOperationException("The selected backup is missing required metadata.");
				}
				DataBackupService.BackupManifest backupManifest2 = backupManifest;
				string text2 = Path.Combine(text, "Data");
				if (!Directory.Exists(text2))
				{
					throw new InvalidOperationException("The selected backup does not include any data to restore.");
				}
				Directory.CreateDirectory(dataDirectory);
				IProgress<DataBackupProgress> progress3 = progress;
				if (progress3 != null)
				{
					progress3.Report(new DataBackupProgress(0.0, "Preparing to restore backup..."));
				}
				DataBackupService.RemoveExistingData(dataDirectory);
				long num = Enumerable.Sum<DataBackupService.SaveReference>(backupManifest2.Saves, (DataBackupService.SaveReference save) => Math.Max(1L, save.Size));
				long totalBytes = DataBackupService.CalculateDirectorySize(text2) + num;
				if (totalBytes <= 0L)
				{
					totalBytes = 1L;
				}
				long restoredBytes = 0L;
				DataBackupService.CopyDirectory(text2, dataDirectory, cancellationToken, delegate(string relativePath, long length)
				{
					restoredBytes += Math.Max(1L, length);
					double percent2 = (double)restoredBytes * 100.0 / (double)totalBytes;
					IProgress<DataBackupProgress> progress6 = progress;
					if (progress6 == null)
					{
						return;
					}
					progress6.Report(new DataBackupProgress(percent2, "Restoring " + relativePath));
				});
				if (backupManifest2.Saves.Count > 0)
				{
					string text3 = Path.Combine(dataDirectory, "Saves");
					Action<string, long> <>9__2;
					foreach (DataBackupService.SaveReference saveReference in backupManifest2.Saves)
					{
						cancellationToken.ThrowIfCancellationRequested();
						string saveStorePath = this.GetSaveStorePath(saveReference.Hash, saveReference.IsDirectory);
						if (saveReference.IsDirectory)
						{
							if (!Directory.Exists(saveStorePath))
							{
								throw new InvalidOperationException("The save archive for " + saveReference.RelativePath + " is missing.");
							}
							string sourceDirectory = saveStorePath;
							string destinationDirectory = Path.Combine(text3, saveReference.RelativePath);
							CancellationToken cancellationToken2 = cancellationToken;
							Action<string, long> onFileCopied;
							if ((onFileCopied = <>9__2) == null)
							{
								onFileCopied = (<>9__2 = delegate(string _, long length)
								{
									restoredBytes += Math.Max(1L, length);
								});
							}
							DataBackupService.CopyDirectory(sourceDirectory, destinationDirectory, cancellationToken2, onFileCopied);
						}
						else
						{
							if (!File.Exists(saveStorePath))
							{
								throw new InvalidOperationException("The save archive for " + saveReference.RelativePath + " is missing.");
							}
							DataBackupService.CopyFile(saveStorePath, Path.Combine(text3, saveReference.RelativePath), cancellationToken);
							restoredBytes += Math.Max(1L, saveReference.Size);
						}
						double percent = (double)restoredBytes * 100.0 / (double)totalBytes;
						IProgress<DataBackupProgress> progress4 = progress;
						if (progress4 != null)
						{
							progress4.Report(new DataBackupProgress(percent, "Restoring save " + saveReference.RelativePath));
						}
					}
				}
				DataBackupService.RemoveCacheDirectory(dataDirectory);
				IProgress<DataBackupProgress> progress5 = progress;
				if (progress5 != null)
				{
					progress5.Report(new DataBackupProgress(100.0, "Restore completed."));
				}
			}
			finally
			{
				if (flag)
				{
					DataBackupService.TryDeleteDirectory(text);
				}
			}
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x00036AB0 File Offset: 0x00034CB0
		private static void RemoveExistingData(string dataDirectory)
		{
			foreach (string text in Directory.EnumerateFileSystemEntries(dataDirectory))
			{
				if (string.Equals(Path.GetFileName(text), "Cache", 5))
				{
					DataBackupService.RemoveCacheDirectory(dataDirectory);
				}
				else if (Directory.Exists(text))
				{
					Directory.Delete(text, true);
				}
				else if (File.Exists(text))
				{
					File.Delete(text);
				}
			}
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x00036B30 File Offset: 0x00034D30
		private static void RemoveCacheDirectory(string dataDirectory)
		{
			string text = Path.Combine(dataDirectory, "Cache");
			if (!Directory.Exists(text))
			{
				return;
			}
			Directory.Delete(text, true);
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x00036B5C File Offset: 0x00034D5C
		private void ExecuteBackupPlan(DataBackupService.BackupPlan plan, DataBackupService.BackupManifest manifest, [Nullable(2)] IProgress<DataBackupProgress> progress, CancellationToken cancellationToken)
		{
			foreach (string text in plan.DirectoriesToCreate)
			{
				Directory.CreateDirectory(text);
			}
			long num = Math.Max(plan.TotalBytes, 1L);
			long num2 = 0L;
			foreach (DataBackupService.FileCopyPlanEntry fileCopyPlanEntry in plan.Files)
			{
				cancellationToken.ThrowIfCancellationRequested();
				DataBackupService.CopyFile(fileCopyPlanEntry.SourcePath, fileCopyPlanEntry.DestinationPath, cancellationToken);
				num2 += Math.Max(1L, fileCopyPlanEntry.Length);
				double percent = (double)num2 * 100.0 / (double)num;
				if (progress != null)
				{
					progress.Report(new DataBackupProgress(percent, "Copying " + Path.GetFileName(fileCopyPlanEntry.SourcePath)));
				}
			}
			if (plan.Saves.Count > 0)
			{
				Directory.CreateDirectory(Path.Combine(plan.DataTargetDirectory, "Saves"));
			}
			foreach (DataBackupService.SavePlanEntry savePlanEntry in plan.Saves)
			{
				cancellationToken.ThrowIfCancellationRequested();
				string saveStorePath = this.GetSaveStorePath(savePlanEntry.Hash, savePlanEntry.IsDirectory);
				if (!savePlanEntry.ExistsInStore)
				{
					if (savePlanEntry.IsDirectory)
					{
						DataBackupService.CopyDirectory(savePlanEntry.SourcePath, saveStorePath, cancellationToken, null);
					}
					else
					{
						DataBackupService.CopyFile(savePlanEntry.SourcePath, saveStorePath, cancellationToken);
					}
					num2 += Math.Max(1L, savePlanEntry.Size);
				}
				manifest.Saves.Add(new DataBackupService.SaveReference
				{
					RelativePath = savePlanEntry.RelativePath,
					Hash = savePlanEntry.Hash,
					IsDirectory = savePlanEntry.IsDirectory,
					Size = savePlanEntry.Size
				});
				double percent2 = (double)num2 * 100.0 / (double)num;
				if (progress != null)
				{
					progress.Report(new DataBackupProgress(percent2, "Archiving save " + savePlanEntry.RelativePath));
				}
			}
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x00036DA0 File Offset: 0x00034FA0
		private DataBackupService.BackupPlan BuildBackupPlan(string sourceDirectory, string targetDirectory, CancellationToken cancellationToken)
		{
			DataBackupService.BackupPlan backupPlan = new DataBackupService.BackupPlan(targetDirectory);
			backupPlan.DirectoriesToCreate.Add(targetDirectory);
			foreach (string text in Directory.EnumerateFileSystemEntries(sourceDirectory))
			{
				cancellationToken.ThrowIfCancellationRequested();
				string fileName = Path.GetFileName(text);
				if (!string.Equals(fileName, "Cache", 5))
				{
					if (Directory.Exists(text) && string.Equals(fileName, "Saves", 5))
					{
						this.BuildSavesPlan(text, Path.Combine(targetDirectory, "Saves"), backupPlan, cancellationToken);
					}
					else
					{
						this.BuildGeneralPlan(text, Path.Combine(targetDirectory, fileName), backupPlan, cancellationToken);
					}
				}
			}
			return backupPlan;
		}

		// Token: 0x060008EC RID: 2284 RVA: 0x00036E54 File Offset: 0x00035054
		private void BuildSavesPlan(string sourceDirectory, string targetDirectory, DataBackupService.BackupPlan plan, CancellationToken cancellationToken)
		{
			plan.DirectoriesToCreate.Add(targetDirectory);
			if (!Directory.Exists(sourceDirectory))
			{
				return;
			}
			foreach (string text in Directory.EnumerateFileSystemEntries(sourceDirectory))
			{
				cancellationToken.ThrowIfCancellationRequested();
				string relativePath = Path.GetRelativePath(sourceDirectory, text);
				bool flag = Directory.Exists(text);
				string hash = DataBackupService.ComputeSaveHash(text, flag, cancellationToken);
				string saveStorePath = this.GetSaveStorePath(hash, flag);
				bool flag2 = flag ? Directory.Exists(saveStorePath) : File.Exists(saveStorePath);
				long num = flag ? DataBackupService.CalculateDirectorySize(text) : DataBackupService.GetFileLength(text);
				if (!flag2)
				{
					plan.TotalBytes += Math.Max(1L, num);
				}
				plan.Saves.Add(new DataBackupService.SavePlanEntry(text, relativePath, hash, flag, flag2, num));
			}
		}

		// Token: 0x060008ED RID: 2285 RVA: 0x00036F3C File Offset: 0x0003513C
		private void BuildGeneralPlan(string sourcePath, string targetPath, DataBackupService.BackupPlan plan, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			if (Directory.Exists(sourcePath))
			{
				plan.DirectoriesToCreate.Add(targetPath);
				using (IEnumerator<string> enumerator = Directory.EnumerateFileSystemEntries(sourcePath).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string text = enumerator.Current;
						string fileName = Path.GetFileName(text);
						this.BuildGeneralPlan(text, Path.Combine(targetPath, fileName), plan, cancellationToken);
					}
					return;
				}
			}
			if (File.Exists(sourcePath))
			{
				long fileLength = DataBackupService.GetFileLength(sourcePath);
				plan.Files.Add(new DataBackupService.FileCopyPlanEntry(sourcePath, targetPath, fileLength));
				plan.TotalBytes += Math.Max(1L, fileLength);
			}
		}

		// Token: 0x060008EE RID: 2286 RVA: 0x00036FEC File Offset: 0x000351EC
		private static long CalculateDirectorySize(string directory)
		{
			if (!Directory.Exists(directory))
			{
				return 0L;
			}
			long num = 0L;
			foreach (string path in Directory.EnumerateFiles(directory, "*", 1))
			{
				num += DataBackupService.GetFileLength(path);
			}
			return num;
		}

		// Token: 0x060008EF RID: 2287 RVA: 0x00037050 File Offset: 0x00035250
		private static string ComputeSaveHash(string path, bool isDirectory, CancellationToken cancellationToken)
		{
			if (!isDirectory)
			{
				using (FileStream fileStream = File.Open(path, 3, 1, 1))
				{
					using (SHA256 sha = SHA256.Create())
					{
						return Convert.ToHexString(sha.ComputeHash(fileStream));
					}
				}
			}
			string result;
			using (IncrementalHash incrementalHash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256))
			{
				byte[] array = ArrayPool<byte>.Shared.Rent(81920);
				try
				{
					foreach (string text in Enumerable.OrderBy<string, string>(Directory.EnumerateDirectories(path, "*", 1), (string dir) => dir, StringComparer.OrdinalIgnoreCase))
					{
						cancellationToken.ThrowIfCancellationRequested();
						string text2 = DataBackupService.NormalizeRelativePath(Path.GetRelativePath(path, text));
						DataBackupService.AppendText(incrementalHash, "D:" + text2);
					}
					foreach (string text3 in Enumerable.OrderBy<string, string>(Directory.EnumerateFiles(path, "*", 1), (string file) => file, StringComparer.OrdinalIgnoreCase))
					{
						cancellationToken.ThrowIfCancellationRequested();
						string text4 = DataBackupService.NormalizeRelativePath(Path.GetRelativePath(path, text3));
						DataBackupService.AppendText(incrementalHash, "F:" + text4);
						using (FileStream fileStream2 = File.Open(text3, 3, 1, 1))
						{
							int num;
							while ((num = fileStream2.Read(array, 0, array.Length)) > 0)
							{
								cancellationToken.ThrowIfCancellationRequested();
								incrementalHash.AppendData(array, 0, num);
							}
						}
					}
				}
				finally
				{
					ArrayPool<byte>.Shared.Return(array, false);
				}
				result = Convert.ToHexString(incrementalHash.GetHashAndReset());
			}
			return result;
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x000372D4 File Offset: 0x000354D4
		private static void AppendText(IncrementalHash hash, string text)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(text);
			hash.AppendData(bytes);
		}

		// Token: 0x060008F1 RID: 2289 RVA: 0x000372F4 File Offset: 0x000354F4
		private static string NormalizeRelativePath(string path)
		{
			return path.Replace('\\', '/');
		}

		// Token: 0x060008F2 RID: 2290 RVA: 0x00037300 File Offset: 0x00035500
		private static void CopyFile(string sourcePath, string destinationPath, CancellationToken cancellationToken)
		{
			string directoryName = Path.GetDirectoryName(destinationPath);
			if (!string.IsNullOrWhiteSpace(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			using (FileStream fileStream = File.Open(sourcePath, 3, 1, 1))
			{
				using (FileStream fileStream2 = File.Open(destinationPath, 2, 2, 0))
				{
					byte[] array = ArrayPool<byte>.Shared.Rent(81920);
					try
					{
						int num;
						while ((num = fileStream.Read(array, 0, array.Length)) > 0)
						{
							cancellationToken.ThrowIfCancellationRequested();
							fileStream2.Write(array, 0, num);
						}
					}
					finally
					{
						ArrayPool<byte>.Shared.Return(array, false);
					}
				}
			}
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x000373B8 File Offset: 0x000355B8
		private static void CopyDirectory(string sourceDirectory, string destinationDirectory, CancellationToken cancellationToken, [Nullable(new byte[]
		{
			2,
			1
		})] Action<string, long> onFileCopied = null)
		{
			Directory.CreateDirectory(destinationDirectory);
			foreach (string text in Directory.EnumerateDirectories(sourceDirectory, "*", 1))
			{
				cancellationToken.ThrowIfCancellationRequested();
				string relativePath = Path.GetRelativePath(sourceDirectory, text);
				Directory.CreateDirectory(Path.Combine(destinationDirectory, relativePath));
			}
			foreach (string text2 in Directory.EnumerateFiles(sourceDirectory, "*", 1))
			{
				cancellationToken.ThrowIfCancellationRequested();
				string relativePath2 = Path.GetRelativePath(sourceDirectory, text2);
				string destinationPath = Path.Combine(destinationDirectory, relativePath2);
				DataBackupService.CopyFile(text2, destinationPath, cancellationToken);
				if (onFileCopied != null)
				{
					onFileCopied.Invoke(relativePath2, DataBackupService.GetFileLength(text2));
				}
			}
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x00037498 File Offset: 0x00035698
		private static long GetFileLength(string path)
		{
			long result;
			try
			{
				result = new FileInfo(path).Length;
			}
			catch
			{
				result = 0L;
			}
			return result;
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x000374CC File Offset: 0x000356CC
		private string GenerateBackupIdentifier(DateTime timestampUtc)
		{
			string text = timestampUtc.ToLocalTime().ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) + "-";
			int num = 0;
			if (Directory.Exists(this._backupRootDirectory))
			{
				try
				{
					foreach (string text2 in Directory.EnumerateDirectories(this._backupRootDirectory, text + "*", 0))
					{
						string fileName = Path.GetFileName(text2);
						if (!string.IsNullOrWhiteSpace(fileName) && fileName.Length > text.Length)
						{
							string text3 = fileName.Substring(text.Length);
							int num2 = text3.IndexOf('_');
							if (num2 >= 0)
							{
								text3 = text3.Substring(0, num2);
							}
							int num3;
							if (int.TryParse(text3, 7, CultureInfo.InvariantCulture, ref num3) && num3 > num)
							{
								num = num3;
							}
						}
					}
					foreach (string text4 in Directory.EnumerateFiles(this._backupRootDirectory, text + "*.zip", 0))
					{
						string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text4);
						if (!string.IsNullOrWhiteSpace(fileNameWithoutExtension) && fileNameWithoutExtension.Length > text.Length)
						{
							string text5 = fileNameWithoutExtension.Substring(text.Length);
							int num4 = text5.IndexOf('_');
							if (num4 >= 0)
							{
								text5 = text5.Substring(0, num4);
							}
							int num5;
							if (int.TryParse(text5, 7, CultureInfo.InvariantCulture, ref num5) && num5 > num)
							{
								num = num5;
							}
						}
					}
				}
				catch
				{
				}
			}
			int num6 = num + 1;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(0, 2);
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendFormatted<int>(num6, "000");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x000376CC File Offset: 0x000358CC
		private static string EnsureUniqueDirectory(string candidate)
		{
			if (!Directory.Exists(candidate) && !File.Exists(candidate + ".zip"))
			{
				return candidate;
			}
			int num = 1;
			string text;
			do
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(1, 2);
				defaultInterpolatedStringHandler.AppendFormatted(candidate);
				defaultInterpolatedStringHandler.AppendLiteral("_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(num++);
				text = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			while (Directory.Exists(text) || File.Exists(text + ".zip"));
			return text;
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x00037744 File Offset: 0x00035944
		private static void WriteManifest(string manifestPath, DataBackupService.BackupManifest manifest)
		{
			JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
			{
				WriteIndented = true
			};
			string directoryName = Path.GetDirectoryName(manifestPath);
			if (!string.IsNullOrWhiteSpace(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			File.WriteAllText(manifestPath, JsonSerializer.Serialize<DataBackupService.BackupManifest>(manifest, jsonSerializerOptions));
		}

		// Token: 0x060008F8 RID: 2296 RVA: 0x00037784 File Offset: 0x00035984
		[return: Nullable(2)]
		private DataBackupService.BackupManifest TryLoadManifest(string directory)
		{
			DataBackupService.BackupManifest result;
			try
			{
				string text = Path.Combine(directory, DevConfig.DataFolderBackupManifestFileName);
				if (!File.Exists(text))
				{
					result = null;
				}
				else
				{
					result = JsonSerializer.Deserialize<DataBackupService.BackupManifest>(File.ReadAllText(text), null);
				}
			}
			catch (JsonException)
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

		// Token: 0x060008F9 RID: 2297 RVA: 0x000377F0 File Offset: 0x000359F0
		[return: Nullable(2)]
		private DataBackupService.BackupManifest TryLoadManifestFromZip(string zipPath)
		{
			DataBackupService.BackupManifest result;
			try
			{
				using (ZipArchive zipArchive = ZipFile.OpenRead(zipPath))
				{
					ZipArchiveEntry entry = zipArchive.GetEntry(DevConfig.DataFolderBackupManifestFileName);
					if (entry == null)
					{
						result = null;
					}
					else
					{
						using (Stream stream = entry.Open())
						{
							result = JsonSerializer.Deserialize<DataBackupService.BackupManifest>(stream, null);
						}
					}
				}
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is InvalidDataException || ex is JsonException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060008FA RID: 2298 RVA: 0x000378AC File Offset: 0x00035AAC
		private void CleanupSaveStore()
		{
			if (!Directory.Exists(this._savesStoreDirectory))
			{
				return;
			}
			HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
			if (Directory.Exists(this._backupRootDirectory))
			{
				foreach (string directory in Directory.EnumerateDirectories(this._backupRootDirectory))
				{
					DataBackupService.BackupManifest backupManifest = this.TryLoadManifest(directory);
					if (backupManifest != null)
					{
						foreach (DataBackupService.SaveReference saveReference in backupManifest.Saves)
						{
							if (!string.IsNullOrWhiteSpace(saveReference.Hash))
							{
								hashSet.Add(saveReference.Hash);
							}
						}
					}
				}
				foreach (string zipPath in Directory.EnumerateFiles(this._backupRootDirectory, "*.zip"))
				{
					DataBackupService.BackupManifest backupManifest2 = this.TryLoadManifestFromZip(zipPath);
					if (backupManifest2 != null)
					{
						foreach (DataBackupService.SaveReference saveReference2 in backupManifest2.Saves)
						{
							if (!string.IsNullOrWhiteSpace(saveReference2.Hash))
							{
								hashSet.Add(saveReference2.Hash);
							}
						}
					}
				}
			}
			foreach (string path in Directory.EnumerateFileSystemEntries(this._savesStoreDirectory))
			{
				string text = DataBackupService.ExtractHashFromStoreEntry(path);
				if (text != null && !hashSet.Contains(text))
				{
					DataBackupService.TryDeleteEntry(path);
				}
			}
		}

		// Token: 0x060008FB RID: 2299 RVA: 0x00037A88 File Offset: 0x00035C88
		[return: Nullable(2)]
		private static string ExtractHashFromStoreEntry(string path)
		{
			string fileName = Path.GetFileName(path);
			if (fileName.EndsWith(".dir", 5))
			{
				string text = fileName;
				int length = ".dir".Length;
				return text.Substring(0, text.Length - length);
			}
			if (fileName.EndsWith(".file", 5))
			{
				string text = fileName;
				int length = ".file".Length;
				return text.Substring(0, text.Length - length);
			}
			return null;
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x00037AF4 File Offset: 0x00035CF4
		[NullableContext(2)]
		private static void TryDeleteDirectory(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return;
			}
			try
			{
				if (Directory.Exists(path))
				{
					Directory.Delete(path, true);
				}
			}
			catch (IOException ex)
			{
				Trace.TraceWarning("Failed to delete directory {0}: {1}", new object[]
				{
					path,
					ex.Message
				});
			}
			catch (UnauthorizedAccessException ex2)
			{
				Trace.TraceWarning("Failed to delete directory {0}: {1}", new object[]
				{
					path,
					ex2.Message
				});
			}
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x00037B78 File Offset: 0x00035D78
		private static void TryDeleteFile(string path)
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
				Trace.TraceWarning("Failed to delete file {0}: {1}", new object[]
				{
					path,
					ex.Message
				});
			}
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x00037BF0 File Offset: 0x00035DF0
		private static bool DeleteBackup(DataBackupSummary summary)
		{
			if (summary.DirectoryPath.EndsWith(".zip", 5))
			{
				bool flag = File.Exists(summary.DirectoryPath);
				DataBackupService.TryDeleteFile(summary.DirectoryPath);
				return flag && !File.Exists(summary.DirectoryPath);
			}
			bool flag2 = Directory.Exists(summary.DirectoryPath);
			DataBackupService.TryDeleteDirectory(summary.DirectoryPath);
			return flag2 && !Directory.Exists(summary.DirectoryPath);
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x00037C64 File Offset: 0x00035E64
		private static void TryDeleteEntry(string path)
		{
			try
			{
				if (Directory.Exists(path))
				{
					Directory.Delete(path, true);
				}
				else if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			catch (IOException ex)
			{
				Trace.TraceWarning("Failed to delete backup store entry {0}: {1}", new object[]
				{
					path,
					ex.Message
				});
			}
			catch (UnauthorizedAccessException ex2)
			{
				Trace.TraceWarning("Failed to delete backup store entry {0}: {1}", new object[]
				{
					path,
					ex2.Message
				});
			}
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x00037CF0 File Offset: 0x00035EF0
		private string GetSaveStorePath(string hash, bool isDirectory)
		{
			string text = isDirectory ? ".dir" : ".file";
			return Path.Combine(this._savesStoreDirectory, hash + text);
		}

		// Token: 0x06000901 RID: 2305 RVA: 0x00037D20 File Offset: 0x00035F20
		[NullableContext(2)]
		private static string NormalizeDirectoryPath(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return null;
			}
			string result;
			try
			{
				result = Path.TrimEndingDirectorySeparator(Path.GetFullPath(path));
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x06000902 RID: 2306 RVA: 0x00037D5C File Offset: 0x00035F5C
		private static bool DirectoriesMatch([Nullable(2)] string candidate, string normalizedTarget)
		{
			if (string.IsNullOrWhiteSpace(candidate))
			{
				return false;
			}
			string text = DataBackupService.NormalizeDirectoryPath(candidate);
			if (text == null)
			{
				return false;
			}
			StringComparison stringComparison = OperatingSystem.IsWindows() ? 5 : 4;
			return string.Equals(text, normalizedTarget, stringComparison);
		}

		// Token: 0x06000903 RID: 2307 RVA: 0x00037D94 File Offset: 0x00035F94
		private static bool VersionsMatch([Nullable(2)] string candidate, string normalizedTarget)
		{
			string text = VersionStringUtility.Normalize(candidate);
			return !string.IsNullOrWhiteSpace(text) && string.Equals(text, normalizedTarget, 5);
		}

		// Token: 0x04000459 RID: 1113
		private const string DataDirectoryName = "Data";

		// Token: 0x0400045A RID: 1114
		private const string SavesFolderName = "Saves";

		// Token: 0x0400045B RID: 1115
		private const string CacheFolderName = "Cache";

		// Token: 0x0400045C RID: 1116
		private const string DirectoryStoreSuffix = ".dir";

		// Token: 0x0400045D RID: 1117
		private const string FileStoreSuffix = ".file";

		// Token: 0x0400045E RID: 1118
		private readonly string _backupRootDirectory;

		// Token: 0x0400045F RID: 1119
		private readonly string _savesStoreDirectory;

		// Token: 0x0200026B RID: 619
		[Nullable(0)]
		private sealed class BackupPlan
		{
			// Token: 0x0600157B RID: 5499 RVA: 0x0007E6D9 File Offset: 0x0007C8D9
			public BackupPlan(string dataTargetDirectory)
			{
				this.DataTargetDirectory = dataTargetDirectory;
			}

			// Token: 0x17000471 RID: 1137
			// (get) Token: 0x0600157C RID: 5500 RVA: 0x0007E709 File Offset: 0x0007C909
			public string DataTargetDirectory { get; }

			// Token: 0x17000472 RID: 1138
			// (get) Token: 0x0600157D RID: 5501 RVA: 0x0007E711 File Offset: 0x0007C911
			public List<string> DirectoriesToCreate { get; } = new List<string>();

			// Token: 0x17000473 RID: 1139
			// (get) Token: 0x0600157E RID: 5502 RVA: 0x0007E719 File Offset: 0x0007C919
			public List<DataBackupService.FileCopyPlanEntry> Files { get; } = new List<DataBackupService.FileCopyPlanEntry>();

			// Token: 0x17000474 RID: 1140
			// (get) Token: 0x0600157F RID: 5503 RVA: 0x0007E721 File Offset: 0x0007C921
			public List<DataBackupService.SavePlanEntry> Saves { get; } = new List<DataBackupService.SavePlanEntry>();

			// Token: 0x17000475 RID: 1141
			// (get) Token: 0x06001580 RID: 5504 RVA: 0x0007E729 File Offset: 0x0007C929
			// (set) Token: 0x06001581 RID: 5505 RVA: 0x0007E731 File Offset: 0x0007C931
			public long TotalBytes { get; set; }
		}

		// Token: 0x0200026C RID: 620
		[Nullable(0)]
		private sealed class FileCopyPlanEntry : IEquatable<DataBackupService.FileCopyPlanEntry>
		{
			// Token: 0x06001582 RID: 5506 RVA: 0x0007E73A File Offset: 0x0007C93A
			public FileCopyPlanEntry(string SourcePath, string DestinationPath, long Length)
			{
				this.SourcePath = SourcePath;
				this.DestinationPath = DestinationPath;
				this.Length = Length;
				base..ctor();
			}

			// Token: 0x17000476 RID: 1142
			// (get) Token: 0x06001583 RID: 5507 RVA: 0x0007E757 File Offset: 0x0007C957
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(DataBackupService.FileCopyPlanEntry);
				}
			}

			// Token: 0x17000477 RID: 1143
			// (get) Token: 0x06001584 RID: 5508 RVA: 0x0007E763 File Offset: 0x0007C963
			// (set) Token: 0x06001585 RID: 5509 RVA: 0x0007E76B File Offset: 0x0007C96B
			public string SourcePath { get; set; }

			// Token: 0x17000478 RID: 1144
			// (get) Token: 0x06001586 RID: 5510 RVA: 0x0007E774 File Offset: 0x0007C974
			// (set) Token: 0x06001587 RID: 5511 RVA: 0x0007E77C File Offset: 0x0007C97C
			public string DestinationPath { get; set; }

			// Token: 0x17000479 RID: 1145
			// (get) Token: 0x06001588 RID: 5512 RVA: 0x0007E785 File Offset: 0x0007C985
			// (set) Token: 0x06001589 RID: 5513 RVA: 0x0007E78D File Offset: 0x0007C98D
			public long Length { get; set; }

			// Token: 0x0600158A RID: 5514 RVA: 0x0007E798 File Offset: 0x0007C998
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("FileCopyPlanEntry");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x0600158B RID: 5515 RVA: 0x0007E7E4 File Offset: 0x0007C9E4
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("SourcePath = ");
				builder.Append(this.SourcePath);
				builder.Append(", DestinationPath = ");
				builder.Append(this.DestinationPath);
				builder.Append(", Length = ");
				builder.Append(this.Length.ToString());
				return true;
			}

			// Token: 0x0600158C RID: 5516 RVA: 0x0007E850 File Offset: 0x0007CA50
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(DataBackupService.FileCopyPlanEntry left, DataBackupService.FileCopyPlanEntry right)
			{
				return !(left == right);
			}

			// Token: 0x0600158D RID: 5517 RVA: 0x0007E85C File Offset: 0x0007CA5C
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(DataBackupService.FileCopyPlanEntry left, DataBackupService.FileCopyPlanEntry right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x0600158E RID: 5518 RVA: 0x0007E870 File Offset: 0x0007CA70
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<SourcePath>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<DestinationPath>k__BackingField)) * -1521134295 + EqualityComparer<long>.Default.GetHashCode(this.<Length>k__BackingField);
			}

			// Token: 0x0600158F RID: 5519 RVA: 0x0007E8D2 File Offset: 0x0007CAD2
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as DataBackupService.FileCopyPlanEntry);
			}

			// Token: 0x06001590 RID: 5520 RVA: 0x0007E8E0 File Offset: 0x0007CAE0
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(DataBackupService.FileCopyPlanEntry other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<SourcePath>k__BackingField, other.<SourcePath>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<DestinationPath>k__BackingField, other.<DestinationPath>k__BackingField) && EqualityComparer<long>.Default.Equals(this.<Length>k__BackingField, other.<Length>k__BackingField));
			}

			// Token: 0x06001592 RID: 5522 RVA: 0x0007E959 File Offset: 0x0007CB59
			[CompilerGenerated]
			private FileCopyPlanEntry(DataBackupService.FileCopyPlanEntry original)
			{
				this.SourcePath = original.<SourcePath>k__BackingField;
				this.DestinationPath = original.<DestinationPath>k__BackingField;
				this.Length = original.<Length>k__BackingField;
			}

			// Token: 0x06001593 RID: 5523 RVA: 0x0007E985 File Offset: 0x0007CB85
			[CompilerGenerated]
			public void Deconstruct(out string SourcePath, out string DestinationPath, out long Length)
			{
				SourcePath = this.SourcePath;
				DestinationPath = this.DestinationPath;
				Length = this.Length;
			}
		}

		// Token: 0x0200026D RID: 621
		[Nullable(0)]
		private sealed class SavePlanEntry : IEquatable<DataBackupService.SavePlanEntry>
		{
			// Token: 0x06001594 RID: 5524 RVA: 0x0007E99F File Offset: 0x0007CB9F
			public SavePlanEntry(string SourcePath, string RelativePath, string Hash, bool IsDirectory, bool ExistsInStore, long Size)
			{
				this.SourcePath = SourcePath;
				this.RelativePath = RelativePath;
				this.Hash = Hash;
				this.IsDirectory = IsDirectory;
				this.ExistsInStore = ExistsInStore;
				this.Size = Size;
				base..ctor();
			}

			// Token: 0x1700047A RID: 1146
			// (get) Token: 0x06001595 RID: 5525 RVA: 0x0007E9D4 File Offset: 0x0007CBD4
			[CompilerGenerated]
			private Type EqualityContract
			{
				[CompilerGenerated]
				get
				{
					return typeof(DataBackupService.SavePlanEntry);
				}
			}

			// Token: 0x1700047B RID: 1147
			// (get) Token: 0x06001596 RID: 5526 RVA: 0x0007E9E0 File Offset: 0x0007CBE0
			// (set) Token: 0x06001597 RID: 5527 RVA: 0x0007E9E8 File Offset: 0x0007CBE8
			public string SourcePath { get; set; }

			// Token: 0x1700047C RID: 1148
			// (get) Token: 0x06001598 RID: 5528 RVA: 0x0007E9F1 File Offset: 0x0007CBF1
			// (set) Token: 0x06001599 RID: 5529 RVA: 0x0007E9F9 File Offset: 0x0007CBF9
			public string RelativePath { get; set; }

			// Token: 0x1700047D RID: 1149
			// (get) Token: 0x0600159A RID: 5530 RVA: 0x0007EA02 File Offset: 0x0007CC02
			// (set) Token: 0x0600159B RID: 5531 RVA: 0x0007EA0A File Offset: 0x0007CC0A
			public string Hash { get; set; }

			// Token: 0x1700047E RID: 1150
			// (get) Token: 0x0600159C RID: 5532 RVA: 0x0007EA13 File Offset: 0x0007CC13
			// (set) Token: 0x0600159D RID: 5533 RVA: 0x0007EA1B File Offset: 0x0007CC1B
			public bool IsDirectory { get; set; }

			// Token: 0x1700047F RID: 1151
			// (get) Token: 0x0600159E RID: 5534 RVA: 0x0007EA24 File Offset: 0x0007CC24
			// (set) Token: 0x0600159F RID: 5535 RVA: 0x0007EA2C File Offset: 0x0007CC2C
			public bool ExistsInStore { get; set; }

			// Token: 0x17000480 RID: 1152
			// (get) Token: 0x060015A0 RID: 5536 RVA: 0x0007EA35 File Offset: 0x0007CC35
			// (set) Token: 0x060015A1 RID: 5537 RVA: 0x0007EA3D File Offset: 0x0007CC3D
			public long Size { get; set; }

			// Token: 0x060015A2 RID: 5538 RVA: 0x0007EA48 File Offset: 0x0007CC48
			[CompilerGenerated]
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("SavePlanEntry");
				stringBuilder.Append(" { ");
				if (this.PrintMembers(stringBuilder))
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append('}');
				return stringBuilder.ToString();
			}

			// Token: 0x060015A3 RID: 5539 RVA: 0x0007EA94 File Offset: 0x0007CC94
			[CompilerGenerated]
			private bool PrintMembers(StringBuilder builder)
			{
				RuntimeHelpers.EnsureSufficientExecutionStack();
				builder.Append("SourcePath = ");
				builder.Append(this.SourcePath);
				builder.Append(", RelativePath = ");
				builder.Append(this.RelativePath);
				builder.Append(", Hash = ");
				builder.Append(this.Hash);
				builder.Append(", IsDirectory = ");
				builder.Append(this.IsDirectory.ToString());
				builder.Append(", ExistsInStore = ");
				builder.Append(this.ExistsInStore.ToString());
				builder.Append(", Size = ");
				builder.Append(this.Size.ToString());
				return true;
			}

			// Token: 0x060015A4 RID: 5540 RVA: 0x0007EB67 File Offset: 0x0007CD67
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator !=(DataBackupService.SavePlanEntry left, DataBackupService.SavePlanEntry right)
			{
				return !(left == right);
			}

			// Token: 0x060015A5 RID: 5541 RVA: 0x0007EB73 File Offset: 0x0007CD73
			[NullableContext(2)]
			[CompilerGenerated]
			public static bool operator ==(DataBackupService.SavePlanEntry left, DataBackupService.SavePlanEntry right)
			{
				return left == right || (left != null && left.Equals(right));
			}

			// Token: 0x060015A6 RID: 5542 RVA: 0x0007EB88 File Offset: 0x0007CD88
			[CompilerGenerated]
			public override int GetHashCode()
			{
				return (((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<SourcePath>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<RelativePath>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.<Hash>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<IsDirectory>k__BackingField)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.<ExistsInStore>k__BackingField)) * -1521134295 + EqualityComparer<long>.Default.GetHashCode(this.<Size>k__BackingField);
			}

			// Token: 0x060015A7 RID: 5543 RVA: 0x0007EC2F File Offset: 0x0007CE2F
			[NullableContext(2)]
			[CompilerGenerated]
			public override bool Equals(object obj)
			{
				return this.Equals(obj as DataBackupService.SavePlanEntry);
			}

			// Token: 0x060015A8 RID: 5544 RVA: 0x0007EC40 File Offset: 0x0007CE40
			[NullableContext(2)]
			[CompilerGenerated]
			public bool Equals(DataBackupService.SavePlanEntry other)
			{
				return this == other || (other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.<SourcePath>k__BackingField, other.<SourcePath>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<RelativePath>k__BackingField, other.<RelativePath>k__BackingField) && EqualityComparer<string>.Default.Equals(this.<Hash>k__BackingField, other.<Hash>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<IsDirectory>k__BackingField, other.<IsDirectory>k__BackingField) && EqualityComparer<bool>.Default.Equals(this.<ExistsInStore>k__BackingField, other.<ExistsInStore>k__BackingField) && EqualityComparer<long>.Default.Equals(this.<Size>k__BackingField, other.<Size>k__BackingField));
			}

			// Token: 0x060015AA RID: 5546 RVA: 0x0007ED0C File Offset: 0x0007CF0C
			[CompilerGenerated]
			private SavePlanEntry(DataBackupService.SavePlanEntry original)
			{
				this.SourcePath = original.<SourcePath>k__BackingField;
				this.RelativePath = original.<RelativePath>k__BackingField;
				this.Hash = original.<Hash>k__BackingField;
				this.IsDirectory = original.<IsDirectory>k__BackingField;
				this.ExistsInStore = original.<ExistsInStore>k__BackingField;
				this.Size = original.<Size>k__BackingField;
			}

			// Token: 0x060015AB RID: 5547 RVA: 0x0007ED67 File Offset: 0x0007CF67
			[CompilerGenerated]
			public void Deconstruct(out string SourcePath, out string RelativePath, out string Hash, out bool IsDirectory, out bool ExistsInStore, out long Size)
			{
				SourcePath = this.SourcePath;
				RelativePath = this.RelativePath;
				Hash = this.Hash;
				IsDirectory = this.IsDirectory;
				ExistsInStore = this.ExistsInStore;
				Size = this.Size;
			}
		}

		// Token: 0x0200026E RID: 622
		[NullableContext(2)]
		[Nullable(0)]
		private sealed class BackupManifest
		{
			// Token: 0x17000481 RID: 1153
			// (get) Token: 0x060015AC RID: 5548 RVA: 0x0007ED9C File Offset: 0x0007CF9C
			// (set) Token: 0x060015AD RID: 5549 RVA: 0x0007EDA4 File Offset: 0x0007CFA4
			public string Id { get; set; }

			// Token: 0x17000482 RID: 1154
			// (get) Token: 0x060015AE RID: 5550 RVA: 0x0007EDAD File Offset: 0x0007CFAD
			// (set) Token: 0x060015AF RID: 5551 RVA: 0x0007EDB5 File Offset: 0x0007CFB5
			public DateTime CreatedOnUtc { get; set; }

			// Token: 0x17000483 RID: 1155
			// (get) Token: 0x060015B0 RID: 5552 RVA: 0x0007EDBE File Offset: 0x0007CFBE
			// (set) Token: 0x060015B1 RID: 5553 RVA: 0x0007EDC6 File Offset: 0x0007CFC6
			public string SourceDataDirectory { get; set; }

			// Token: 0x17000484 RID: 1156
			// (get) Token: 0x060015B2 RID: 5554 RVA: 0x0007EDCF File Offset: 0x0007CFCF
			// (set) Token: 0x060015B3 RID: 5555 RVA: 0x0007EDD7 File Offset: 0x0007CFD7
			public string VintageStoryVersion { get; set; }

			// Token: 0x17000485 RID: 1157
			// (get) Token: 0x060015B4 RID: 5556 RVA: 0x0007EDE0 File Offset: 0x0007CFE0
			// (set) Token: 0x060015B5 RID: 5557 RVA: 0x0007EDE8 File Offset: 0x0007CFE8
			[Nullable(1)]
			public List<DataBackupService.SaveReference> Saves { [NullableContext(1)] get; [NullableContext(1)] set; } = new List<DataBackupService.SaveReference>();
		}

		// Token: 0x0200026F RID: 623
		[Nullable(0)]
		private sealed class SaveReference
		{
			// Token: 0x17000486 RID: 1158
			// (get) Token: 0x060015B7 RID: 5559 RVA: 0x0007EE04 File Offset: 0x0007D004
			// (set) Token: 0x060015B8 RID: 5560 RVA: 0x0007EE0C File Offset: 0x0007D00C
			public string RelativePath { get; set; } = string.Empty;

			// Token: 0x17000487 RID: 1159
			// (get) Token: 0x060015B9 RID: 5561 RVA: 0x0007EE15 File Offset: 0x0007D015
			// (set) Token: 0x060015BA RID: 5562 RVA: 0x0007EE1D File Offset: 0x0007D01D
			public string Hash { get; set; } = string.Empty;

			// Token: 0x17000488 RID: 1160
			// (get) Token: 0x060015BB RID: 5563 RVA: 0x0007EE26 File Offset: 0x0007D026
			// (set) Token: 0x060015BC RID: 5564 RVA: 0x0007EE2E File Offset: 0x0007D02E
			public bool IsDirectory { get; set; }

			// Token: 0x17000489 RID: 1161
			// (get) Token: 0x060015BD RID: 5565 RVA: 0x0007EE37 File Offset: 0x0007D037
			// (set) Token: 0x060015BE RID: 5566 RVA: 0x0007EE3F File Offset: 0x0007D03F
			public long Size { get; set; }
		}
	}
}
