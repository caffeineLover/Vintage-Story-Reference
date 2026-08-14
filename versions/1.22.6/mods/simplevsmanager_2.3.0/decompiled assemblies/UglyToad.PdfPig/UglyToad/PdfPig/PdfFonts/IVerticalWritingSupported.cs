using System;
using UglyToad.PdfPig.Geometry;

namespace UglyToad.PdfPig.PdfFonts
{
	// Token: 0x02000095 RID: 149
	internal interface IVerticalWritingSupported
	{
		// Token: 0x0600026B RID: 619
		PdfVector GetPositionVector(int characterCode);

		// Token: 0x0600026C RID: 620
		PdfVector GetDisplacementVector(int characterCode);
	}
}
