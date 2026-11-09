using System;
using System.Collections.Generic;
using System.Linq;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.AdobeFontMetrics
{
	// Token: 0x020000AD RID: 173
	internal class AdobeFontMetricsBuilder
	{
		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000434 RID: 1076 RVA: 0x0001DD2A File Offset: 0x0001BF2A
		public decimal AfmVersion { get; }

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000435 RID: 1077 RVA: 0x0001DD32 File Offset: 0x0001BF32
		public List<string> Comments { get; }

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x0001DD3A File Offset: 0x0001BF3A
		public List<AdobeFontMetricsIndividualCharacterMetric> CharacterMetrics { get; } = new List<AdobeFontMetricsIndividualCharacterMetric>();

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x0001DD42 File Offset: 0x0001BF42
		// (set) Token: 0x06000438 RID: 1080 RVA: 0x0001DD4A File Offset: 0x0001BF4A
		public string FontName { get; set; }

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000439 RID: 1081 RVA: 0x0001DD53 File Offset: 0x0001BF53
		// (set) Token: 0x0600043A RID: 1082 RVA: 0x0001DD5B File Offset: 0x0001BF5B
		public string FullName { get; set; }

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x0600043B RID: 1083 RVA: 0x0001DD64 File Offset: 0x0001BF64
		// (set) Token: 0x0600043C RID: 1084 RVA: 0x0001DD6C File Offset: 0x0001BF6C
		public string FamilyName { get; set; }

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x0001DD75 File Offset: 0x0001BF75
		// (set) Token: 0x0600043E RID: 1086 RVA: 0x0001DD7D File Offset: 0x0001BF7D
		public string Weight { get; set; }

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x0001DD86 File Offset: 0x0001BF86
		// (set) Token: 0x06000440 RID: 1088 RVA: 0x0001DD8E File Offset: 0x0001BF8E
		public decimal ItalicAngle { get; set; }

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x0001DD97 File Offset: 0x0001BF97
		// (set) Token: 0x06000442 RID: 1090 RVA: 0x0001DD9F File Offset: 0x0001BF9F
		public bool IsFixedPitch { get; set; }

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x06000443 RID: 1091 RVA: 0x0001DDA8 File Offset: 0x0001BFA8
		// (set) Token: 0x06000444 RID: 1092 RVA: 0x0001DDB0 File Offset: 0x0001BFB0
		public PdfRectangle PdfBoundingBox { get; private set; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x06000445 RID: 1093 RVA: 0x0001DDB9 File Offset: 0x0001BFB9
		// (set) Token: 0x06000446 RID: 1094 RVA: 0x0001DDC1 File Offset: 0x0001BFC1
		public decimal UnderlinePosition { get; set; }

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x06000447 RID: 1095 RVA: 0x0001DDCA File Offset: 0x0001BFCA
		// (set) Token: 0x06000448 RID: 1096 RVA: 0x0001DDD2 File Offset: 0x0001BFD2
		public decimal UnderlineThickness { get; set; }

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x06000449 RID: 1097 RVA: 0x0001DDDB File Offset: 0x0001BFDB
		// (set) Token: 0x0600044A RID: 1098 RVA: 0x0001DDE3 File Offset: 0x0001BFE3
		public string Version { get; set; }

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x0600044B RID: 1099 RVA: 0x0001DDEC File Offset: 0x0001BFEC
		// (set) Token: 0x0600044C RID: 1100 RVA: 0x0001DDF4 File Offset: 0x0001BFF4
		public string Notice { get; set; }

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x0600044D RID: 1101 RVA: 0x0001DDFD File Offset: 0x0001BFFD
		// (set) Token: 0x0600044E RID: 1102 RVA: 0x0001DE05 File Offset: 0x0001C005
		public string EncodingScheme { get; set; }

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x0001DE0E File Offset: 0x0001C00E
		// (set) Token: 0x06000450 RID: 1104 RVA: 0x0001DE16 File Offset: 0x0001C016
		public int MappingScheme { get; set; }

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x06000451 RID: 1105 RVA: 0x0001DE1F File Offset: 0x0001C01F
		// (set) Token: 0x06000452 RID: 1106 RVA: 0x0001DE27 File Offset: 0x0001C027
		public string CharacterSet { get; set; }

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x06000453 RID: 1107 RVA: 0x0001DE30 File Offset: 0x0001C030
		// (set) Token: 0x06000454 RID: 1108 RVA: 0x0001DE38 File Offset: 0x0001C038
		public bool IsBaseFont { get; set; } = true;

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x06000455 RID: 1109 RVA: 0x0001DE41 File Offset: 0x0001C041
		// (set) Token: 0x06000456 RID: 1110 RVA: 0x0001DE49 File Offset: 0x0001C049
		public decimal CapHeight { get; set; }

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x06000457 RID: 1111 RVA: 0x0001DE52 File Offset: 0x0001C052
		// (set) Token: 0x06000458 RID: 1112 RVA: 0x0001DE5A File Offset: 0x0001C05A
		public decimal XHeight { get; set; }

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x0001DE63 File Offset: 0x0001C063
		// (set) Token: 0x0600045A RID: 1114 RVA: 0x0001DE6B File Offset: 0x0001C06B
		public decimal Ascender { get; set; }

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x0001DE74 File Offset: 0x0001C074
		// (set) Token: 0x0600045C RID: 1116 RVA: 0x0001DE7C File Offset: 0x0001C07C
		public decimal Descender { get; set; }

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x0001DE85 File Offset: 0x0001C085
		// (set) Token: 0x0600045E RID: 1118 RVA: 0x0001DE8D File Offset: 0x0001C08D
		public decimal StdHw { get; set; }

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x0001DE96 File Offset: 0x0001C096
		// (set) Token: 0x06000460 RID: 1120 RVA: 0x0001DE9E File Offset: 0x0001C09E
		public decimal StdVw { get; set; }

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x0001DEA7 File Offset: 0x0001C0A7
		// (set) Token: 0x06000462 RID: 1122 RVA: 0x0001DEAF File Offset: 0x0001C0AF
		public int EscapeCharacter { get; set; }

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x0001DEB8 File Offset: 0x0001C0B8
		// (set) Token: 0x06000464 RID: 1124 RVA: 0x0001DEC0 File Offset: 0x0001C0C0
		public AdobeFontMetricsCharacterSize CharacterWidth { get; private set; }

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x0001DEC9 File Offset: 0x0001C0C9
		// (set) Token: 0x06000466 RID: 1126 RVA: 0x0001DED1 File Offset: 0x0001C0D1
		public int Characters { get; set; }

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x0001DEDA File Offset: 0x0001C0DA
		// (set) Token: 0x06000468 RID: 1128 RVA: 0x0001DEE2 File Offset: 0x0001C0E2
		public AdobeFontMetricsVector VVector { get; private set; }

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x0001DEEB File Offset: 0x0001C0EB
		// (set) Token: 0x0600046A RID: 1130 RVA: 0x0001DEF3 File Offset: 0x0001C0F3
		public bool IsFixedV { get; set; }

		// Token: 0x0600046B RID: 1131 RVA: 0x0001DEFC File Offset: 0x0001C0FC
		public AdobeFontMetricsBuilder(decimal afmVersion)
		{
			this.AfmVersion = afmVersion;
			this.Comments = new List<string>();
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0001DF28 File Offset: 0x0001C128
		public void SetBoundingBox(double x1, double y1, double x2, double y2)
		{
			this.PdfBoundingBox = new PdfRectangle(x1, y1, x2, y2);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0001DF3A File Offset: 0x0001C13A
		public void SetCharacterWidth(double x, double y)
		{
			this.CharacterWidth = new AdobeFontMetricsCharacterSize(x, y);
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0001DF49 File Offset: 0x0001C149
		public void SetVVector(double x, double y)
		{
			this.VVector = new AdobeFontMetricsVector(x, y);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0001DF58 File Offset: 0x0001C158
		public AdobeFontMetrics Build()
		{
			Dictionary<string, AdobeFontMetricsIndividualCharacterMetric> characterMetrics = Enumerable.ToDictionary<AdobeFontMetricsIndividualCharacterMetric, string>(this.CharacterMetrics, (AdobeFontMetricsIndividualCharacterMetric x) => x.Name);
			return new AdobeFontMetrics(this.AfmVersion, this.Comments, 0, this.FontName, this.FullName, this.FamilyName, this.Weight, this.PdfBoundingBox, this.Version, this.Notice, this.EncodingScheme, this.MappingScheme, this.EscapeCharacter, this.CharacterSet, this.Characters, this.IsBaseFont, this.VVector, this.IsFixedV, this.CapHeight, this.XHeight, this.Ascender, this.Descender, this.UnderlinePosition, this.UnderlineThickness, this.ItalicAngle, this.CharacterWidth, this.StdHw, this.StdVw, characterMetrics);
		}
	}
}
