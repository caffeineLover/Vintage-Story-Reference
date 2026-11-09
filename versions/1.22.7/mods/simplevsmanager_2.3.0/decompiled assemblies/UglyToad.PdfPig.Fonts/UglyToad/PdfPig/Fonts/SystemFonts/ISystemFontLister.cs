using System;
using System.Collections.Generic;

namespace UglyToad.PdfPig.Fonts.SystemFonts
{
	// Token: 0x0200006E RID: 110
	internal interface ISystemFontLister
	{
		// Token: 0x060002AA RID: 682
		IEnumerable<SystemFontRecord> GetAllFonts();
	}
}
