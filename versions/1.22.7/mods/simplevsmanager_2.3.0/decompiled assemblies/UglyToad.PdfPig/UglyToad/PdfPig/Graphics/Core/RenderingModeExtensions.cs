using System;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Graphics.Core
{
	// Token: 0x02000161 RID: 353
	internal static class RenderingModeExtensions
	{
		// Token: 0x060007F5 RID: 2037 RVA: 0x00025A00 File Offset: 0x00023C00
		public static bool IsFill(this TextRenderingMode mode)
		{
			return mode == null || mode == 2 || mode == 4 || mode == 6;
		}

		// Token: 0x060007F6 RID: 2038 RVA: 0x00025A13 File Offset: 0x00023C13
		public static bool IsStroke(this TextRenderingMode mode)
		{
			return mode == 1 || mode == 2 || mode == 5 || mode == 6;
		}

		// Token: 0x060007F7 RID: 2039 RVA: 0x00025A27 File Offset: 0x00023C27
		public static bool IsClip(this TextRenderingMode mode)
		{
			return mode == 4 || mode == 5 || mode == 6 || mode == 7;
		}
	}
}
