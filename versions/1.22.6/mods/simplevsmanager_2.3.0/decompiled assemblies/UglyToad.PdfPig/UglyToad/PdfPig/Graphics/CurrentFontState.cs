using System;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Graphics
{
	// Token: 0x02000105 RID: 261
	public class CurrentFontState : IDeepCloneable<CurrentFontState>
	{
		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000538 RID: 1336 RVA: 0x0001F68C File Offset: 0x0001D88C
		// (set) Token: 0x06000539 RID: 1337 RVA: 0x0001F694 File Offset: 0x0001D894
		public bool FromExtendedGraphicsState { get; set; }

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600053A RID: 1338 RVA: 0x0001F69D File Offset: 0x0001D89D
		// (set) Token: 0x0600053B RID: 1339 RVA: 0x0001F6A5 File Offset: 0x0001D8A5
		public double CharacterSpacing { get; set; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x0600053C RID: 1340 RVA: 0x0001F6AE File Offset: 0x0001D8AE
		// (set) Token: 0x0600053D RID: 1341 RVA: 0x0001F6B6 File Offset: 0x0001D8B6
		public double WordSpacing { get; set; }

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600053E RID: 1342 RVA: 0x0001F6BF File Offset: 0x0001D8BF
		// (set) Token: 0x0600053F RID: 1343 RVA: 0x0001F6C7 File Offset: 0x0001D8C7
		public double HorizontalScaling { get; set; } = 100.0;

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000540 RID: 1344 RVA: 0x0001F6D0 File Offset: 0x0001D8D0
		// (set) Token: 0x06000541 RID: 1345 RVA: 0x0001F6D8 File Offset: 0x0001D8D8
		public double Leading { get; set; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06000542 RID: 1346 RVA: 0x0001F6E1 File Offset: 0x0001D8E1
		// (set) Token: 0x06000543 RID: 1347 RVA: 0x0001F6E9 File Offset: 0x0001D8E9
		public NameToken FontName { get; set; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06000544 RID: 1348 RVA: 0x0001F6F2 File Offset: 0x0001D8F2
		// (set) Token: 0x06000545 RID: 1349 RVA: 0x0001F6FA File Offset: 0x0001D8FA
		public double FontSize { get; set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x06000546 RID: 1350 RVA: 0x0001F703 File Offset: 0x0001D903
		// (set) Token: 0x06000547 RID: 1351 RVA: 0x0001F70B File Offset: 0x0001D90B
		public TextRenderingMode TextRenderingMode { get; set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x0001F714 File Offset: 0x0001D914
		// (set) Token: 0x06000549 RID: 1353 RVA: 0x0001F71C File Offset: 0x0001D91C
		public double Rise { get; set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x0001F725 File Offset: 0x0001D925
		// (set) Token: 0x0600054B RID: 1355 RVA: 0x0001F72D File Offset: 0x0001D92D
		public bool Knockout { get; set; }

		// Token: 0x0600054C RID: 1356 RVA: 0x0001F738 File Offset: 0x0001D938
		public CurrentFontState DeepClone()
		{
			return new CurrentFontState
			{
				CharacterSpacing = this.CharacterSpacing,
				TextRenderingMode = this.TextRenderingMode,
				Rise = this.Rise,
				Leading = this.Leading,
				WordSpacing = this.WordSpacing,
				FontName = this.FontName,
				FontSize = this.FontSize,
				HorizontalScaling = this.HorizontalScaling,
				Knockout = this.Knockout
			};
		}
	}
}
