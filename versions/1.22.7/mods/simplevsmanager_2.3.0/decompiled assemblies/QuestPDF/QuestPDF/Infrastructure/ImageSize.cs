using System;

namespace QuestPDF.Infrastructure
{
	// Token: 0x02000057 RID: 87
	public readonly struct ImageSize
	{
		// Token: 0x0600020A RID: 522 RVA: 0x00005427 File Offset: 0x00003627
		public ImageSize(int width, int height)
		{
			this.Width = width;
			this.Height = height;
		}

		// Token: 0x0400011A RID: 282
		public readonly int Width;

		// Token: 0x0400011B RID: 283
		public readonly int Height;
	}
}
