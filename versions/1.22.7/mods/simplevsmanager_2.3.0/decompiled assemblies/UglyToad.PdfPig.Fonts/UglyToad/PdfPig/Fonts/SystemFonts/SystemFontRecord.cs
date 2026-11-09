using System;

namespace UglyToad.PdfPig.Fonts.SystemFonts
{
	// Token: 0x02000072 RID: 114
	internal struct SystemFontRecord
	{
		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000DEE4 File Offset: 0x0000C0E4
		public readonly string Path { get; }

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x0000DEEC File Offset: 0x0000C0EC
		public readonly SystemFontType Type { get; }

		// Token: 0x060002B8 RID: 696 RVA: 0x0000DEF4 File Offset: 0x0000C0F4
		public SystemFontRecord(string path, SystemFontType type)
		{
			if (path == null)
			{
				throw new ArgumentNullException("path");
			}
			this.Path = path;
			this.Type = type;
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000DF14 File Offset: 0x0000C114
		public static bool TryCreate(string path, out SystemFontRecord type)
		{
			type = default(SystemFontRecord);
			SystemFontType type2;
			if (path.EndsWith(".ttf"))
			{
				type2 = SystemFontType.TrueType;
			}
			else if (path.EndsWith(".otf"))
			{
				type2 = SystemFontType.OpenType;
			}
			else if (path.EndsWith(".ttc"))
			{
				type2 = SystemFontType.TrueTypeCollection;
			}
			else if (path.EndsWith(".otc"))
			{
				type2 = SystemFontType.OpenTypeCollection;
			}
			else
			{
				if (!path.EndsWith(".pfb"))
				{
					return false;
				}
				type2 = SystemFontType.Type1;
			}
			type = new SystemFontRecord(path, type2);
			return true;
		}
	}
}
