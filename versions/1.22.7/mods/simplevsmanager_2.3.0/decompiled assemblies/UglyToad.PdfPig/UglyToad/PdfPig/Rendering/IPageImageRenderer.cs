using System;
using UglyToad.PdfPig.Content;

namespace UglyToad.PdfPig.Rendering
{
	// Token: 0x02000087 RID: 135
	public interface IPageImageRenderer
	{
		// Token: 0x0600022C RID: 556
		byte[] Render(Page page, double scale, PdfRendererImageFormat imageFormat);
	}
}
