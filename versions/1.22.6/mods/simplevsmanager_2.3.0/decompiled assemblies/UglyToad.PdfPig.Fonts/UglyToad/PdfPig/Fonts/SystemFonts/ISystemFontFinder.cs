using System;
using UglyToad.PdfPig.Fonts.TrueType;

namespace UglyToad.PdfPig.Fonts.SystemFonts
{
	// Token: 0x0200006D RID: 109
	public interface ISystemFontFinder
	{
		// Token: 0x060002A9 RID: 681
		TrueTypeFont GetTrueTypeFont(string name);
	}
}
