using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Fonts.TrueType.Names;

namespace UglyToad.PdfPig.Fonts.TrueType.Tables
{
	// Token: 0x02000040 RID: 64
	public class NameTable : ITrueTypeTable
	{
		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x00009F02 File Offset: 0x00008102
		public string Tag
		{
			get
			{
				return "name";
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00009F09 File Offset: 0x00008109
		public TrueTypeHeaderTable DirectoryTable { get; }

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060001CA RID: 458 RVA: 0x00009F11 File Offset: 0x00008111
		public string FontName { get; }

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060001CB RID: 459 RVA: 0x00009F19 File Offset: 0x00008119
		public string FontFamilyName { get; }

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060001CC RID: 460 RVA: 0x00009F21 File Offset: 0x00008121
		public string FontSubFamilyName { get; }

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00009F29 File Offset: 0x00008129
		public IReadOnlyList<TrueTypeNameRecord> NameRecords { get; }

		// Token: 0x060001CE RID: 462 RVA: 0x00009F31 File Offset: 0x00008131
		public NameTable(TrueTypeHeaderTable directoryTable, string fontName, string fontFamilyName, string fontSubFamilyName, IReadOnlyList<TrueTypeNameRecord> nameRecords)
		{
			this.DirectoryTable = directoryTable;
			this.FontName = fontName;
			this.FontFamilyName = fontFamilyName;
			this.FontSubFamilyName = fontSubFamilyName;
			if (nameRecords == null)
			{
				throw new ArgumentNullException("nameRecords");
			}
			this.NameRecords = nameRecords;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00009F70 File Offset: 0x00008170
		public string GetPostscriptName()
		{
			string result = null;
			foreach (TrueTypeNameRecord trueTypeNameRecord in this.NameRecords)
			{
				if (trueTypeNameRecord != null && trueTypeNameRecord.NameId == 6)
				{
					if (trueTypeNameRecord.PlatformId == TrueTypePlatformIdentifier.Windows)
					{
						return trueTypeNameRecord.Value;
					}
					result = trueTypeNameRecord.Value;
				}
			}
			return result;
		}
	}
}
