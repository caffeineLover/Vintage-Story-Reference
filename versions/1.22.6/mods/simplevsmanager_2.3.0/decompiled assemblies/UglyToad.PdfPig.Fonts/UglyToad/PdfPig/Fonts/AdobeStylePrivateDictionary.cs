using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts
{
	// Token: 0x02000002 RID: 2
	public abstract class AdobeStylePrivateDictionary
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public IReadOnlyList<int> BlueValues { get; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public IReadOnlyList<int> OtherBlues { get; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002060 File Offset: 0x00000260
		public IReadOnlyList<int> FamilyBlues { get; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000004 RID: 4 RVA: 0x00002068 File Offset: 0x00000268
		public IReadOnlyList<int> FamilyOtherBlues { get; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002070 File Offset: 0x00000270
		public decimal BlueScale { get; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002078 File Offset: 0x00000278
		public int BlueShift { get; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002080 File Offset: 0x00000280
		public int BlueFuzz { get; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00002088 File Offset: 0x00000288
		public decimal? StandardHorizontalWidth { get; }

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002090 File Offset: 0x00000290
		public decimal? StandardVerticalWidth { get; }

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600000A RID: 10 RVA: 0x00002098 File Offset: 0x00000298
		public IReadOnlyList<decimal> StemSnapHorizontalWidths { get; }

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600000B RID: 11 RVA: 0x000020A0 File Offset: 0x000002A0
		public IReadOnlyList<decimal> StemSnapVerticalWidths { get; }

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600000C RID: 12 RVA: 0x000020A8 File Offset: 0x000002A8
		public bool ForceBold { get; }

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600000D RID: 13 RVA: 0x000020B0 File Offset: 0x000002B0
		public int LanguageGroup { get; }

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000020B8 File Offset: 0x000002B8
		public decimal ExpansionFactor { get; }

		// Token: 0x0600000F RID: 15 RVA: 0x000020C0 File Offset: 0x000002C0
		protected AdobeStylePrivateDictionary(AdobeStylePrivateDictionary.BaseBuilder builder)
		{
			if (builder == null)
			{
				throw new ArgumentNullException("builder");
			}
			this.BlueValues = (builder.BlueValues ?? EmptyArray<int>.Instance);
			this.OtherBlues = (builder.OtherBlues ?? EmptyArray<int>.Instance);
			this.FamilyBlues = (builder.FamilyBlues ?? EmptyArray<int>.Instance);
			this.FamilyOtherBlues = (builder.FamilyOtherBlues ?? EmptyArray<int>.Instance);
			this.BlueScale = (builder.BlueScale ?? AdobeStylePrivateDictionary.DefaultBlueScale);
			this.BlueFuzz = (builder.BlueFuzz ?? 1);
			this.BlueShift = (builder.BlueShift ?? 7);
			this.StandardHorizontalWidth = builder.StandardHorizontalWidth;
			this.StandardVerticalWidth = builder.StandardVerticalWidth;
			this.StemSnapHorizontalWidths = (builder.StemSnapHorizontalWidths ?? EmptyArray<decimal>.Instance);
			this.StemSnapVerticalWidths = (builder.StemSnapVerticalWidths ?? EmptyArray<decimal>.Instance);
			this.ForceBold = builder.ForceBold.GetValueOrDefault();
			this.LanguageGroup = builder.LanguageGroup.GetValueOrDefault();
			this.ExpansionFactor = (builder.ExpansionFactor ?? AdobeStylePrivateDictionary.DefaultExpansionFactor);
		}

		// Token: 0x04000001 RID: 1
		public static readonly decimal DefaultBlueScale = 0.039625m;

		// Token: 0x04000002 RID: 2
		public static readonly decimal DefaultExpansionFactor = 0.06m;

		// Token: 0x04000003 RID: 3
		public const int DefaultBlueFuzz = 1;

		// Token: 0x04000004 RID: 4
		public const int DefaultBlueShift = 7;

		// Token: 0x04000005 RID: 5
		public const int DefaultLanguageGroup = 0;

		// Token: 0x020000B7 RID: 183
		public abstract class BaseBuilder
		{
			// Token: 0x17000213 RID: 531
			// (get) Token: 0x060004A9 RID: 1193 RVA: 0x0001F238 File Offset: 0x0001D438
			// (set) Token: 0x060004AA RID: 1194 RVA: 0x0001F240 File Offset: 0x0001D440
			public IReadOnlyList<int> BlueValues { get; set; }

			// Token: 0x17000214 RID: 532
			// (get) Token: 0x060004AB RID: 1195 RVA: 0x0001F249 File Offset: 0x0001D449
			// (set) Token: 0x060004AC RID: 1196 RVA: 0x0001F251 File Offset: 0x0001D451
			public IReadOnlyList<int> OtherBlues { get; set; }

			// Token: 0x17000215 RID: 533
			// (get) Token: 0x060004AD RID: 1197 RVA: 0x0001F25A File Offset: 0x0001D45A
			// (set) Token: 0x060004AE RID: 1198 RVA: 0x0001F262 File Offset: 0x0001D462
			public IReadOnlyList<int> FamilyBlues { get; set; }

			// Token: 0x17000216 RID: 534
			// (get) Token: 0x060004AF RID: 1199 RVA: 0x0001F26B File Offset: 0x0001D46B
			// (set) Token: 0x060004B0 RID: 1200 RVA: 0x0001F273 File Offset: 0x0001D473
			public IReadOnlyList<int> FamilyOtherBlues { get; set; }

			// Token: 0x17000217 RID: 535
			// (get) Token: 0x060004B1 RID: 1201 RVA: 0x0001F27C File Offset: 0x0001D47C
			// (set) Token: 0x060004B2 RID: 1202 RVA: 0x0001F284 File Offset: 0x0001D484
			public decimal? BlueScale { get; set; }

			// Token: 0x17000218 RID: 536
			// (get) Token: 0x060004B3 RID: 1203 RVA: 0x0001F28D File Offset: 0x0001D48D
			// (set) Token: 0x060004B4 RID: 1204 RVA: 0x0001F295 File Offset: 0x0001D495
			public int? BlueShift { get; set; }

			// Token: 0x17000219 RID: 537
			// (get) Token: 0x060004B5 RID: 1205 RVA: 0x0001F29E File Offset: 0x0001D49E
			// (set) Token: 0x060004B6 RID: 1206 RVA: 0x0001F2A6 File Offset: 0x0001D4A6
			public int? BlueFuzz { get; set; }

			// Token: 0x1700021A RID: 538
			// (get) Token: 0x060004B7 RID: 1207 RVA: 0x0001F2AF File Offset: 0x0001D4AF
			// (set) Token: 0x060004B8 RID: 1208 RVA: 0x0001F2B7 File Offset: 0x0001D4B7
			public decimal? StandardHorizontalWidth { get; set; }

			// Token: 0x1700021B RID: 539
			// (get) Token: 0x060004B9 RID: 1209 RVA: 0x0001F2C0 File Offset: 0x0001D4C0
			// (set) Token: 0x060004BA RID: 1210 RVA: 0x0001F2C8 File Offset: 0x0001D4C8
			public decimal? StandardVerticalWidth { get; set; }

			// Token: 0x1700021C RID: 540
			// (get) Token: 0x060004BB RID: 1211 RVA: 0x0001F2D1 File Offset: 0x0001D4D1
			// (set) Token: 0x060004BC RID: 1212 RVA: 0x0001F2D9 File Offset: 0x0001D4D9
			public IReadOnlyList<decimal> StemSnapHorizontalWidths { get; set; }

			// Token: 0x1700021D RID: 541
			// (get) Token: 0x060004BD RID: 1213 RVA: 0x0001F2E2 File Offset: 0x0001D4E2
			// (set) Token: 0x060004BE RID: 1214 RVA: 0x0001F2EA File Offset: 0x0001D4EA
			public IReadOnlyList<decimal> StemSnapVerticalWidths { get; set; }

			// Token: 0x1700021E RID: 542
			// (get) Token: 0x060004BF RID: 1215 RVA: 0x0001F2F3 File Offset: 0x0001D4F3
			// (set) Token: 0x060004C0 RID: 1216 RVA: 0x0001F2FB File Offset: 0x0001D4FB
			public bool? ForceBold { get; set; }

			// Token: 0x1700021F RID: 543
			// (get) Token: 0x060004C1 RID: 1217 RVA: 0x0001F304 File Offset: 0x0001D504
			// (set) Token: 0x060004C2 RID: 1218 RVA: 0x0001F30C File Offset: 0x0001D50C
			public int? LanguageGroup { get; set; }

			// Token: 0x17000220 RID: 544
			// (get) Token: 0x060004C3 RID: 1219 RVA: 0x0001F315 File Offset: 0x0001D515
			// (set) Token: 0x060004C4 RID: 1220 RVA: 0x0001F31D File Offset: 0x0001D51D
			public decimal? ExpansionFactor { get; set; }
		}
	}
}
