using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace QuestPDF.Helpers
{
	// Token: 0x0200007B RID: 123
	[NullableContext(2)]
	[Nullable(0)]
	internal static class TemporaryStorage
	{
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x0600034B RID: 843 RVA: 0x00008D7E File Offset: 0x00006F7E
		// (set) Token: 0x0600034C RID: 844 RVA: 0x00008D85 File Offset: 0x00006F85
		private static string TemporaryStoragePath { get; set; }

		// Token: 0x0600034D RID: 845 RVA: 0x00008D8D File Offset: 0x00006F8D
		[NullableContext(1)]
		internal static string GetPath()
		{
			string text = TemporaryStorage.TryGetPath();
			if (text == null)
			{
				throw new InvalidOperationException("Unable to find a suitable temporary storage location. Please specify it using the Settings.TemporaryStoragePath setting and ensure that the application has permissions to read and write to that location.");
			}
			return text;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x00008DA4 File Offset: 0x00006FA4
		internal static string TryGetPath()
		{
			if (TemporaryStorage.TemporaryStoragePath != null)
			{
				return TemporaryStorage.TemporaryStoragePath;
			}
			IEnumerable<string> enumerable = Enumerable.Where<string>(new string[]
			{
				Settings.TemporaryStoragePath,
				Path.Combine(Path.GetTempPath(), "QuestPDF", "temp"),
				Path.Combine(Environment.GetFolderPath(28), "QuestPDF", "temp"),
				Path.Combine(Environment.GetFolderPath(35), "QuestPDF", "temp")
			}, (string x) => x != null);
			Func<string, bool> func;
			if ((func = TemporaryStorage.<>O.<0>__HasPermissionsToAlterPath) == null)
			{
				func = (TemporaryStorage.<>O.<0>__HasPermissionsToAlterPath = new Func<string, bool>(TemporaryStorage.HasPermissionsToAlterPath));
			}
			TemporaryStorage.TemporaryStoragePath = Enumerable.FirstOrDefault<string>(enumerable, func);
			return TemporaryStorage.TemporaryStoragePath;
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00008E64 File Offset: 0x00007064
		[NullableContext(1)]
		private static bool HasPermissionsToAlterPath(string path)
		{
			bool result;
			try
			{
				if (!Directory.Exists(path))
				{
					Directory.CreateDirectory(path);
				}
				string text = Path.Combine(path, Path.GetRandomFileName());
				using (FileStream fileStream = File.Create(text))
				{
					fileStream.WriteByte(123);
				}
				File.Delete(text);
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x02000185 RID: 389
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040005C0 RID: 1472
			[Nullable(0)]
			public static Func<string, bool> <0>__HasPermissionsToAlterPath;
		}
	}
}
