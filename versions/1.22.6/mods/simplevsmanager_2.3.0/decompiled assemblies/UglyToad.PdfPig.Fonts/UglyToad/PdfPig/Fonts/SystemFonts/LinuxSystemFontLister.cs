using System;
using System.Collections.Generic;
using System.IO;

namespace UglyToad.PdfPig.Fonts.SystemFonts
{
	// Token: 0x0200006F RID: 111
	internal class LinuxSystemFontLister : ISystemFontLister
	{
		// Token: 0x060002AB RID: 683 RVA: 0x0000D678 File Offset: 0x0000B878
		public IEnumerable<SystemFontRecord> GetAllFonts()
		{
			List<string> list = new List<string>();
			list.Add("/usr/local/fonts");
			list.Add("/usr/local/share/fonts");
			list.Add("/usr/share/fonts");
			list.Add("/usr/X11R6/lib/X11/fonts");
			List<string> list2 = list;
			try
			{
				string environmentVariable = Environment.GetEnvironmentVariable("$HOME");
				if (string.IsNullOrWhiteSpace(environmentVariable))
				{
					environmentVariable = Environment.GetEnvironmentVariable("HOME");
				}
				if (!string.IsNullOrWhiteSpace(environmentVariable))
				{
					list2.Add(environmentVariable + "/.fonts");
				}
			}
			catch
			{
			}
			foreach (string text in list2)
			{
				try
				{
					if (!Directory.Exists(text))
					{
						continue;
					}
				}
				catch
				{
					continue;
				}
				string[] files;
				try
				{
					files = Directory.GetFiles(text, "*.*", 1);
				}
				catch
				{
					continue;
				}
				string[] array = files;
				for (int i = 0; i < array.Length; i++)
				{
					SystemFontRecord systemFontRecord;
					if (SystemFontRecord.TryCreate(array[i], out systemFontRecord))
					{
						yield return systemFontRecord;
					}
				}
				array = null;
			}
			List<string>.Enumerator enumerator = default(List<string>.Enumerator);
			yield break;
			yield break;
		}
	}
}
