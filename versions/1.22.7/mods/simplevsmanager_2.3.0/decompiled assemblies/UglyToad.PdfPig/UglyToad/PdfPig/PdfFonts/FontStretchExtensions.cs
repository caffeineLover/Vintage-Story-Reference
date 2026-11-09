using System;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.PdfFonts
{
	// Token: 0x02000092 RID: 146
	internal static class FontStretchExtensions
	{
		// Token: 0x06000260 RID: 608 RVA: 0x00010178 File Offset: 0x0000E378
		public static FontStretch ConvertToFontStretch(this NameToken name)
		{
			string data = name.Data;
			uint num = <PrivateImplementationDetails>.ComputeStringHash(data);
			if (num <= 1490954098U)
			{
				if (num <= 448860242U)
				{
					if (num != 70267786U)
					{
						if (num == 448860242U)
						{
							if (data == "Condensed")
							{
								return FontStretch.Condensed;
							}
						}
					}
					else if (data == "SemiExpanded")
					{
						return FontStretch.SemiExpanded;
					}
				}
				else if (num != 1254241640U)
				{
					if (num == 1490954098U)
					{
						if (data == "Normal")
						{
							return FontStretch.Normal;
						}
					}
				}
				else if (data == "UltraExpanded")
				{
					return FontStretch.UltraExpanded;
				}
			}
			else if (num <= 1816842402U)
			{
				if (num != 1785505622U)
				{
					if (num == 1816842402U)
					{
						if (data == "UltraCondensed")
						{
							return FontStretch.UltraCondensed;
						}
					}
				}
				else if (data == "ExtraCondensed")
				{
					return FontStretch.ExtraCondensed;
				}
			}
			else if (num != 2079068312U)
			{
				if (num == 4067999284U)
				{
					if (data == "ExtraExpanded")
					{
						return FontStretch.ExtraExpanded;
					}
				}
			}
			else if (data == "Expanded")
			{
				return FontStretch.Expanded;
			}
			return FontStretch.Unknown;
		}
	}
}
