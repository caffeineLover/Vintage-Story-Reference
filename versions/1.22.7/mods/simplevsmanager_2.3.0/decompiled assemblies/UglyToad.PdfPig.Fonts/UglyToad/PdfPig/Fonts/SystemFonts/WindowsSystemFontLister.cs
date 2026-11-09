using System;
using System.Collections.Generic;
using System.IO;

namespace UglyToad.PdfPig.Fonts.SystemFonts
{
	// Token: 0x02000074 RID: 116
	internal class WindowsSystemFontLister : ISystemFontLister
	{
		// Token: 0x060002BA RID: 698 RVA: 0x0000DF8D File Offset: 0x0000C18D
		public IEnumerable<SystemFontRecord> GetAllFonts()
		{
			string winDir = Environment.GetFolderPath(36);
			string fonts = Path.Combine(winDir, "Fonts");
			if (Directory.Exists(fonts))
			{
				string[] files = Directory.GetFiles(fonts);
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
			if (Directory.Exists(Path.Combine(winDir, "PSFonts")))
			{
				string[] files2 = Directory.GetFiles(fonts);
				string[] array = files2;
				for (int i = 0; i < array.Length; i++)
				{
					SystemFontRecord systemFontRecord2;
					if (SystemFontRecord.TryCreate(array[i], out systemFontRecord2))
					{
						yield return systemFontRecord2;
					}
				}
				array = null;
			}
			yield break;
		}
	}
}
