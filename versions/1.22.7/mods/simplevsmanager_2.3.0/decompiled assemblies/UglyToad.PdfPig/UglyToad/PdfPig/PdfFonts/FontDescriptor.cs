using System;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;
using UglyToad.PdfPig.Util.JetBrains.Annotations;

namespace UglyToad.PdfPig.PdfFonts
{
	// Token: 0x0200008D RID: 141
	public class FontDescriptor
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000241 RID: 577 RVA: 0x0000FD8C File Offset: 0x0000DF8C
		public NameToken FontName { get; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000242 RID: 578 RVA: 0x0000FD94 File Offset: 0x0000DF94
		public string FontFamily { get; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000243 RID: 579 RVA: 0x0000FD9C File Offset: 0x0000DF9C
		public FontStretch Stretch { get; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000244 RID: 580 RVA: 0x0000FDA4 File Offset: 0x0000DFA4
		public decimal FontWeight { get; }

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000245 RID: 581 RVA: 0x0000FDAC File Offset: 0x0000DFAC
		public FontDescriptorFlags Flags { get; }

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000246 RID: 582 RVA: 0x0000FDB4 File Offset: 0x0000DFB4
		public PdfRectangle BoundingBox { get; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000FDBC File Offset: 0x0000DFBC
		public decimal ItalicAngle { get; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000FDC4 File Offset: 0x0000DFC4
		public decimal Ascent { get; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000249 RID: 585 RVA: 0x0000FDCC File Offset: 0x0000DFCC
		public decimal Descent { get; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600024A RID: 586 RVA: 0x0000FDD4 File Offset: 0x0000DFD4
		public decimal Leading { get; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600024B RID: 587 RVA: 0x0000FDDC File Offset: 0x0000DFDC
		public decimal CapHeight { get; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600024C RID: 588 RVA: 0x0000FDE4 File Offset: 0x0000DFE4
		public decimal XHeight { get; }

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600024D RID: 589 RVA: 0x0000FDEC File Offset: 0x0000DFEC
		public decimal StemVertical { get; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600024E RID: 590 RVA: 0x0000FDF4 File Offset: 0x0000DFF4
		public decimal StemHorizontal { get; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x0600024F RID: 591 RVA: 0x0000FDFC File Offset: 0x0000DFFC
		public decimal AverageWidth { get; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000250 RID: 592 RVA: 0x0000FE04 File Offset: 0x0000E004
		public decimal MaxWidth { get; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000251 RID: 593 RVA: 0x0000FE0C File Offset: 0x0000E00C
		public decimal MissingWidth { get; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0000FE14 File Offset: 0x0000E014
		[CanBeNull]
		public DescriptorFontFile FontFile { get; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000253 RID: 595 RVA: 0x0000FE1C File Offset: 0x0000E01C
		[CanBeNull]
		public string CharSet { get; }

		// Token: 0x06000254 RID: 596 RVA: 0x0000FE24 File Offset: 0x0000E024
		public FontDescriptor(FontDescriptor.Builder builder)
		{
			this.FontName = builder.FontName;
			this.FontFamily = builder.FontFamily;
			this.Stretch = builder.Stretch;
			this.FontWeight = builder.FontWeight;
			this.Flags = builder.Flags;
			this.BoundingBox = builder.BoundingBox;
			this.ItalicAngle = builder.ItalicAngle;
			this.Ascent = builder.Ascent;
			this.Descent = builder.Descent;
			this.Leading = builder.Leading;
			this.CapHeight = builder.CapHeight;
			this.XHeight = builder.XHeight;
			this.StemVertical = builder.StemVertical;
			this.StemHorizontal = builder.StemHorizontal;
			this.AverageWidth = builder.AverageWidth;
			this.MaxWidth = builder.MaxWidth;
			this.MissingWidth = builder.MissingWidth;
			this.FontFile = builder.FontFile;
			this.CharSet = builder.CharSet;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000FF1C File Offset: 0x0000E11C
		internal FontDetails ToDetails(string name = null)
		{
			string name2 = name;
			if (name == null)
			{
				NameToken fontName = this.FontName;
				name2 = ((fontName != null) ? fontName : string.Empty);
			}
			return new FontDetails(name2, this.FontWeight > 500m, (int)this.FontWeight, this.Flags.HasFlag(FontDescriptorFlags.Italic) || this.ItalicAngle != 0m);
		}

		// Token: 0x02000252 RID: 594
		public class Builder
		{
			// Token: 0x170003F4 RID: 1012
			// (get) Token: 0x06000D93 RID: 3475 RVA: 0x00041022 File Offset: 0x0003F222
			// (set) Token: 0x06000D94 RID: 3476 RVA: 0x0004102A File Offset: 0x0003F22A
			public NameToken FontName { get; set; }

			// Token: 0x170003F5 RID: 1013
			// (get) Token: 0x06000D95 RID: 3477 RVA: 0x00041033 File Offset: 0x0003F233
			// (set) Token: 0x06000D96 RID: 3478 RVA: 0x0004103B File Offset: 0x0003F23B
			public string FontFamily { get; set; }

			// Token: 0x170003F6 RID: 1014
			// (get) Token: 0x06000D97 RID: 3479 RVA: 0x00041044 File Offset: 0x0003F244
			// (set) Token: 0x06000D98 RID: 3480 RVA: 0x0004104C File Offset: 0x0003F24C
			public FontStretch Stretch { get; set; } = FontStretch.Normal;

			// Token: 0x170003F7 RID: 1015
			// (get) Token: 0x06000D99 RID: 3481 RVA: 0x00041055 File Offset: 0x0003F255
			// (set) Token: 0x06000D9A RID: 3482 RVA: 0x0004105D File Offset: 0x0003F25D
			public decimal FontWeight { get; set; } = 400m;

			// Token: 0x170003F8 RID: 1016
			// (get) Token: 0x06000D9B RID: 3483 RVA: 0x00041066 File Offset: 0x0003F266
			// (set) Token: 0x06000D9C RID: 3484 RVA: 0x0004106E File Offset: 0x0003F26E
			public FontDescriptorFlags Flags { get; set; }

			// Token: 0x170003F9 RID: 1017
			// (get) Token: 0x06000D9D RID: 3485 RVA: 0x00041077 File Offset: 0x0003F277
			// (set) Token: 0x06000D9E RID: 3486 RVA: 0x0004107F File Offset: 0x0003F27F
			public PdfRectangle BoundingBox { get; set; }

			// Token: 0x170003FA RID: 1018
			// (get) Token: 0x06000D9F RID: 3487 RVA: 0x00041088 File Offset: 0x0003F288
			// (set) Token: 0x06000DA0 RID: 3488 RVA: 0x00041090 File Offset: 0x0003F290
			public decimal ItalicAngle { get; set; }

			// Token: 0x170003FB RID: 1019
			// (get) Token: 0x06000DA1 RID: 3489 RVA: 0x00041099 File Offset: 0x0003F299
			// (set) Token: 0x06000DA2 RID: 3490 RVA: 0x000410A1 File Offset: 0x0003F2A1
			public decimal Ascent { get; set; }

			// Token: 0x170003FC RID: 1020
			// (get) Token: 0x06000DA3 RID: 3491 RVA: 0x000410AA File Offset: 0x0003F2AA
			// (set) Token: 0x06000DA4 RID: 3492 RVA: 0x000410B2 File Offset: 0x0003F2B2
			public decimal Descent { get; set; }

			// Token: 0x170003FD RID: 1021
			// (get) Token: 0x06000DA5 RID: 3493 RVA: 0x000410BB File Offset: 0x0003F2BB
			// (set) Token: 0x06000DA6 RID: 3494 RVA: 0x000410C3 File Offset: 0x0003F2C3
			public decimal Leading { get; set; }

			// Token: 0x170003FE RID: 1022
			// (get) Token: 0x06000DA7 RID: 3495 RVA: 0x000410CC File Offset: 0x0003F2CC
			// (set) Token: 0x06000DA8 RID: 3496 RVA: 0x000410D4 File Offset: 0x0003F2D4
			public decimal CapHeight { get; set; }

			// Token: 0x170003FF RID: 1023
			// (get) Token: 0x06000DA9 RID: 3497 RVA: 0x000410DD File Offset: 0x0003F2DD
			// (set) Token: 0x06000DAA RID: 3498 RVA: 0x000410E5 File Offset: 0x0003F2E5
			public decimal XHeight { get; set; }

			// Token: 0x17000400 RID: 1024
			// (get) Token: 0x06000DAB RID: 3499 RVA: 0x000410EE File Offset: 0x0003F2EE
			// (set) Token: 0x06000DAC RID: 3500 RVA: 0x000410F6 File Offset: 0x0003F2F6
			public decimal StemVertical { get; set; }

			// Token: 0x17000401 RID: 1025
			// (get) Token: 0x06000DAD RID: 3501 RVA: 0x000410FF File Offset: 0x0003F2FF
			// (set) Token: 0x06000DAE RID: 3502 RVA: 0x00041107 File Offset: 0x0003F307
			public decimal StemHorizontal { get; set; }

			// Token: 0x17000402 RID: 1026
			// (get) Token: 0x06000DAF RID: 3503 RVA: 0x00041110 File Offset: 0x0003F310
			// (set) Token: 0x06000DB0 RID: 3504 RVA: 0x00041118 File Offset: 0x0003F318
			public decimal AverageWidth { get; set; }

			// Token: 0x17000403 RID: 1027
			// (get) Token: 0x06000DB1 RID: 3505 RVA: 0x00041121 File Offset: 0x0003F321
			// (set) Token: 0x06000DB2 RID: 3506 RVA: 0x00041129 File Offset: 0x0003F329
			public decimal MaxWidth { get; set; }

			// Token: 0x17000404 RID: 1028
			// (get) Token: 0x06000DB3 RID: 3507 RVA: 0x00041132 File Offset: 0x0003F332
			// (set) Token: 0x06000DB4 RID: 3508 RVA: 0x0004113A File Offset: 0x0003F33A
			public decimal MissingWidth { get; set; }

			// Token: 0x17000405 RID: 1029
			// (get) Token: 0x06000DB5 RID: 3509 RVA: 0x00041143 File Offset: 0x0003F343
			// (set) Token: 0x06000DB6 RID: 3510 RVA: 0x0004114B File Offset: 0x0003F34B
			[CanBeNull]
			public DescriptorFontFile FontFile { get; set; }

			// Token: 0x17000406 RID: 1030
			// (get) Token: 0x06000DB7 RID: 3511 RVA: 0x00041154 File Offset: 0x0003F354
			// (set) Token: 0x06000DB8 RID: 3512 RVA: 0x0004115C File Offset: 0x0003F35C
			[CanBeNull]
			public string CharSet { get; set; }

			// Token: 0x06000DB9 RID: 3513 RVA: 0x00041165 File Offset: 0x0003F365
			public Builder(NameToken fontName, FontDescriptorFlags flags)
			{
				this.FontName = fontName;
				this.Flags = flags;
			}

			// Token: 0x06000DBA RID: 3514 RVA: 0x00041192 File Offset: 0x0003F392
			public FontDescriptor Build()
			{
				return new FontDescriptor(this);
			}
		}
	}
}
