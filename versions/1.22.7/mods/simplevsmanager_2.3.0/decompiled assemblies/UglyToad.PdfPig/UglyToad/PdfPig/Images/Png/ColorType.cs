using System;

namespace UglyToad.PdfPig.Images.Png
{
	// Token: 0x020000EF RID: 239
	[Flags]
	internal enum ColorType : byte
	{
		// Token: 0x04000263 RID: 611
		None = 0,
		// Token: 0x04000264 RID: 612
		PaletteUsed = 1,
		// Token: 0x04000265 RID: 613
		ColorUsed = 2,
		// Token: 0x04000266 RID: 614
		AlphaChannelUsed = 4
	}
}
