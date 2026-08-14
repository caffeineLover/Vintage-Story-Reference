using System;

namespace UglyToad.PdfPig.Images
{
	// Token: 0x020000EA RID: 234
	internal class JpegInformation
	{
		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000475 RID: 1141 RVA: 0x0001B1CE File Offset: 0x000193CE
		public int Width { get; }

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x06000476 RID: 1142 RVA: 0x0001B1D6 File Offset: 0x000193D6
		public int Height { get; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x0001B1DE File Offset: 0x000193DE
		public int BitsPerComponent { get; }

		// Token: 0x06000478 RID: 1144 RVA: 0x0001B1E6 File Offset: 0x000193E6
		public JpegInformation(int width, int height, int bitsPerComponent)
		{
			this.Width = width;
			this.Height = height;
			this.BitsPerComponent = bitsPerComponent;
		}
	}
}
