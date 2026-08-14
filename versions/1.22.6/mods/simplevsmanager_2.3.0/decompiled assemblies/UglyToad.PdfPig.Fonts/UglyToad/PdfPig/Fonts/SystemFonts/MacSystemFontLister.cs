using System;
using System.Collections.Generic;
using System.IO;

namespace UglyToad.PdfPig.Fonts.SystemFonts
{
	// Token: 0x02000070 RID: 112
	internal class MacSystemFontLister : ISystemFontLister
	{
		// Token: 0x060002AD RID: 685 RVA: 0x0000D689 File Offset: 0x0000B889
		public IEnumerable<SystemFontRecord> GetAllFonts()
		{
			List<string> list = new List<string>();
			list.Add("/Library/Fonts/");
			list.Add("/System/Library/Fonts/");
			list.Add("/Network/Library/Fonts/");
			List<string> list2 = list;
			try
			{
				string environmentVariable = Environment.GetEnvironmentVariable("$HOME");
				if (!string.IsNullOrWhiteSpace(environmentVariable))
				{
					list2.Add(environmentVariable + "/Library/Fonts");
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
					files = Directory.GetFiles(text);
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
