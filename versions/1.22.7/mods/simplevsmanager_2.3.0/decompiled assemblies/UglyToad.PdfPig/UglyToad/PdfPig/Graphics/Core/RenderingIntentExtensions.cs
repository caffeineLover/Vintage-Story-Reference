using System;

namespace UglyToad.PdfPig.Graphics.Core
{
	// Token: 0x02000160 RID: 352
	internal static class RenderingIntentExtensions
	{
		// Token: 0x060007F4 RID: 2036 RVA: 0x000259B4 File Offset: 0x00023BB4
		public static RenderingIntent ToRenderingIntent(this string s)
		{
			if (s == "AbsoluteColorimetric")
			{
				return RenderingIntent.AbsoluteColorimetric;
			}
			if (s == "RelativeColorimetric")
			{
				return RenderingIntent.RelativeColorimetric;
			}
			if (s == "Saturation")
			{
				return RenderingIntent.Saturation;
			}
			if (!(s == "Perceptual"))
			{
				return RenderingIntent.RelativeColorimetric;
			}
			return RenderingIntent.Perceptual;
		}
	}
}
