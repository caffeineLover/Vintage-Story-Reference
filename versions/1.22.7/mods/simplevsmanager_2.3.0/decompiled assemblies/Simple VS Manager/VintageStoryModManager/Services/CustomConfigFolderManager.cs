using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace VintageStoryModManager.Services
{
	// Token: 0x02000058 RID: 88
	[NullableContext(1)]
	[Nullable(0)]
	public static class CustomConfigFolderManager
	{
		// Token: 0x060008DA RID: 2266 RVA: 0x00035EB8 File Offset: 0x000340B8
		private static string GetCustomFolderPathFile()
		{
			return Path.Combine(AppContext.BaseDirectory, "CustomConfigFolder.txt");
		}

		// Token: 0x060008DB RID: 2267 RVA: 0x00035ECC File Offset: 0x000340CC
		[NullableContext(2)]
		public static string GetCustomConfigFolder()
		{
			string customFolderPathFile = CustomConfigFolderManager.GetCustomFolderPathFile();
			if (!File.Exists(customFolderPathFile))
			{
				return null;
			}
			string result;
			try
			{
				string text = File.ReadAllText(customFolderPathFile).Trim();
				if (string.IsNullOrWhiteSpace(text))
				{
					result = null;
				}
				else if (!Directory.Exists(text))
				{
					result = null;
				}
				else
				{
					result = text;
				}
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060008DC RID: 2268 RVA: 0x00035F28 File Offset: 0x00034128
		public static void SetCustomConfigFolder(string folderPath)
		{
			if (string.IsNullOrWhiteSpace(folderPath))
			{
				throw new ArgumentException("Folder path cannot be null or empty.", "folderPath");
			}
			File.WriteAllText(CustomConfigFolderManager.GetCustomFolderPathFile(), folderPath);
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x00035F50 File Offset: 0x00034150
		public static void ClearCustomConfigFolder()
		{
			string customFolderPathFile = CustomConfigFolderManager.GetCustomFolderPathFile();
			if (File.Exists(customFolderPathFile))
			{
				try
				{
					File.Delete(customFolderPathFile);
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x00035F88 File Offset: 0x00034188
		public static bool HasCustomConfigFolder()
		{
			return CustomConfigFolderManager.GetCustomConfigFolder() != null;
		}

		// Token: 0x04000458 RID: 1112
		private const string CustomFolderPathFileName = "CustomConfigFolder.txt";
	}
}
