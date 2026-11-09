using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000057 RID: 87
	[NullableContext(1)]
	[Nullable(0)]
	internal static class ConfigurationMigrationService
	{
		// Token: 0x060008D1 RID: 2257 RVA: 0x000359F8 File Offset: 0x00033BF8
		[NullableContext(2)]
		public static string GetOldConfigurationDirectory()
		{
			string folder = ConfigurationMigrationService.GetFolder(5);
			if (string.IsNullOrWhiteSpace(folder))
			{
				folder = ConfigurationMigrationService.GetFolder(5);
			}
			if (string.IsNullOrWhiteSpace(folder))
			{
				return null;
			}
			return Path.Combine(folder, "Simple VS Manager");
		}

		// Token: 0x060008D2 RID: 2258 RVA: 0x00035A30 File Offset: 0x00033C30
		[NullableContext(2)]
		public static string GetNewConfigurationDirectory()
		{
			string folder = ConfigurationMigrationService.GetFolder(28);
			if (string.IsNullOrWhiteSpace(folder))
			{
				return null;
			}
			return Path.Combine(folder, "Simple VS Manager");
		}

		// Token: 0x060008D3 RID: 2259 RVA: 0x00035A5C File Offset: 0x00033C5C
		[NullableContext(2)]
		public static bool ShouldOfferMigration(out string oldConfigVersion)
		{
			oldConfigVersion = null;
			string oldConfigurationDirectory = ConfigurationMigrationService.GetOldConfigurationDirectory();
			string newConfigurationDirectory = ConfigurationMigrationService.GetNewConfigurationDirectory();
			if (string.IsNullOrWhiteSpace(oldConfigurationDirectory) || string.IsNullOrWhiteSpace(newConfigurationDirectory))
			{
				return false;
			}
			if (!Directory.Exists(oldConfigurationDirectory))
			{
				return false;
			}
			string text = Path.Combine(oldConfigurationDirectory, DevConfig.ConfigurationFileName);
			if (!File.Exists(text))
			{
				return false;
			}
			oldConfigVersion = ConfigurationMigrationService.TryReadConfigurationVersion(text);
			return !string.IsNullOrWhiteSpace(oldConfigVersion) && ConfigurationMigrationService.IsVersionEligibleForMigration(oldConfigVersion);
		}

		// Token: 0x060008D4 RID: 2260 RVA: 0x00035ACC File Offset: 0x00033CCC
		public static bool PerformMigration()
		{
			string oldConfigurationDirectory = ConfigurationMigrationService.GetOldConfigurationDirectory();
			string newConfigurationDirectory = ConfigurationMigrationService.GetNewConfigurationDirectory();
			if (string.IsNullOrWhiteSpace(oldConfigurationDirectory) || string.IsNullOrWhiteSpace(newConfigurationDirectory))
			{
				return false;
			}
			if (!Directory.Exists(oldConfigurationDirectory))
			{
				return false;
			}
			bool result;
			try
			{
				Directory.CreateDirectory(newConfigurationDirectory);
				ConfigurationMigrationService.CopyDirectory(oldConfigurationDirectory, newConfigurationDirectory, true);
				ConfigurationMigrationService.TryRefreshFirebaseBackup(newConfigurationDirectory);
				result = true;
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is NotSupportedException)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x00035B68 File Offset: 0x00033D68
		private static void CopyDirectory(string sourceDir, string destDir, bool recursive)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(sourceDir);
			if (!directoryInfo.Exists)
			{
				return;
			}
			Directory.CreateDirectory(destDir);
			foreach (FileInfo fileInfo in directoryInfo.GetFiles())
			{
				string text = Path.Combine(destDir, fileInfo.Name);
				if (!File.Exists(text))
				{
					try
					{
						fileInfo.CopyTo(text, false);
					}
					catch (IOException)
					{
					}
				}
			}
			if (recursive)
			{
				foreach (DirectoryInfo directoryInfo2 in directoryInfo.GetDirectories())
				{
					string destDir2 = Path.Combine(destDir, directoryInfo2.Name);
					ConfigurationMigrationService.CopyDirectory(directoryInfo2.FullName, destDir2, true);
				}
			}
		}

		// Token: 0x060008D6 RID: 2262 RVA: 0x00035C1C File Offset: 0x00033E1C
		[return: Nullable(2)]
		private static string TryReadConfigurationVersion(string configPath)
		{
			string result;
			try
			{
				if (!File.Exists(configPath))
				{
					result = null;
				}
				else
				{
					string text = File.ReadAllText(configPath);
					if (string.IsNullOrWhiteSpace(text))
					{
						result = null;
					}
					else
					{
						int num = text.IndexOf("\"configurationVersion\"", 4);
						if (num < 0)
						{
							result = null;
						}
						else
						{
							int num2 = text.IndexOf(':', num);
							if (num2 < 0)
							{
								result = null;
							}
							else
							{
								int num3 = text.IndexOf('"', num2 + 1);
								if (num3 < 0)
								{
									result = null;
								}
								else
								{
									int num4 = text.IndexOf('"', num3 + 1);
									if (num4 < 0)
									{
										result = null;
									}
									else
									{
										string text2 = text.Substring(num3 + 1, num4 - num3 - 1);
										result = (string.IsNullOrWhiteSpace(text2) ? null : text2.Trim());
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060008D7 RID: 2263 RVA: 0x00035D1C File Offset: 0x00033F1C
		private static bool IsVersionEligibleForMigration(string version)
		{
			if (string.IsNullOrWhiteSpace(version))
			{
				return false;
			}
			bool result;
			try
			{
				string[] array = version.Split('.', 0);
				int num;
				int num2;
				if (array.Length < 2)
				{
					result = false;
				}
				else if (!int.TryParse(array[0], ref num))
				{
					result = false;
				}
				else if (!int.TryParse(array[1], ref num2))
				{
					result = false;
				}
				else if (num < 1)
				{
					result = true;
				}
				else if (num == 1 && num2 <= 4)
				{
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060008D8 RID: 2264 RVA: 0x00035D98 File Offset: 0x00033F98
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

		// Token: 0x060008D9 RID: 2265 RVA: 0x00035DD8 File Offset: 0x00033FD8
		private static void TryRefreshFirebaseBackup(string newConfigDirectory)
		{
			try
			{
				string folderPath = Environment.GetFolderPath(28);
				if (!string.IsNullOrWhiteSpace(folderPath))
				{
					string text = Path.Combine(folderPath, DevConfig.FirebaseAuthBackupDirectoryName);
					Directory.CreateDirectory(text);
					string text2 = Path.Combine(text, DevConfig.FirebaseAuthStateFileName);
					string text3 = Path.Combine(text, "OLD_BACKUP_" + DevConfig.FirebaseAuthStateFileName);
					string text4 = Path.Combine(text, "BACKUP_" + DevConfig.FirebaseAuthStateFileName);
					if (File.Exists(text2))
					{
						File.Move(text2, text3, true);
					}
					string text5 = Path.Combine(newConfigDirectory, DevConfig.FirebaseAuthStateFileName);
					if (File.Exists(text5))
					{
						File.Copy(text5, text4, true);
					}
				}
			}
			catch (IOException)
			{
			}
			catch (UnauthorizedAccessException)
			{
			}
			catch (NotSupportedException)
			{
			}
			catch (SecurityException)
			{
			}
		}
	}
}
