using System;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat.Dictionaries
{
	// Token: 0x02000097 RID: 151
	internal class CompactFontFormatPrivateDictionary : AdobeStylePrivateDictionary
	{
		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000364 RID: 868 RVA: 0x000199B0 File Offset: 0x00017BB0
		public decimal InitialRandomSeed { get; }

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000365 RID: 869 RVA: 0x000199B8 File Offset: 0x00017BB8
		public int? LocalSubroutineOffset { get; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000366 RID: 870 RVA: 0x000199C0 File Offset: 0x00017BC0
		public decimal DefaultWidthX { get; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000367 RID: 871 RVA: 0x000199C8 File Offset: 0x00017BC8
		public decimal NominalWidthX { get; }

		// Token: 0x06000368 RID: 872 RVA: 0x000199D0 File Offset: 0x00017BD0
		public CompactFontFormatPrivateDictionary(CompactFontFormatPrivateDictionary.Builder builder) : base(builder)
		{
			this.InitialRandomSeed = builder.InitialRandomSeed;
			this.LocalSubroutineOffset = builder.LocalSubroutineOffset;
			this.DefaultWidthX = builder.DefaultWidthX;
			this.NominalWidthX = builder.NominalWidthX;
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00019A09 File Offset: 0x00017C09
		public static CompactFontFormatPrivateDictionary GetDefault()
		{
			return new CompactFontFormatPrivateDictionary.Builder().Build();
		}

		// Token: 0x020000DD RID: 221
		public class Builder : AdobeStylePrivateDictionary.BaseBuilder
		{
			// Token: 0x1700026D RID: 621
			// (get) Token: 0x0600057B RID: 1403 RVA: 0x000203E9 File Offset: 0x0001E5E9
			// (set) Token: 0x0600057C RID: 1404 RVA: 0x000203F1 File Offset: 0x0001E5F1
			public decimal InitialRandomSeed { get; set; }

			// Token: 0x1700026E RID: 622
			// (get) Token: 0x0600057D RID: 1405 RVA: 0x000203FA File Offset: 0x0001E5FA
			// (set) Token: 0x0600057E RID: 1406 RVA: 0x00020402 File Offset: 0x0001E602
			public int? LocalSubroutineOffset { get; set; }

			// Token: 0x1700026F RID: 623
			// (get) Token: 0x0600057F RID: 1407 RVA: 0x0002040B File Offset: 0x0001E60B
			// (set) Token: 0x06000580 RID: 1408 RVA: 0x00020413 File Offset: 0x0001E613
			public decimal DefaultWidthX { get; set; }

			// Token: 0x17000270 RID: 624
			// (get) Token: 0x06000581 RID: 1409 RVA: 0x0002041C File Offset: 0x0001E61C
			// (set) Token: 0x06000582 RID: 1410 RVA: 0x00020424 File Offset: 0x0001E624
			public decimal NominalWidthX { get; set; }

			// Token: 0x06000583 RID: 1411 RVA: 0x0002042D File Offset: 0x0001E62D
			public CompactFontFormatPrivateDictionary Build()
			{
				return new CompactFontFormatPrivateDictionary(this);
			}
		}
	}
}
