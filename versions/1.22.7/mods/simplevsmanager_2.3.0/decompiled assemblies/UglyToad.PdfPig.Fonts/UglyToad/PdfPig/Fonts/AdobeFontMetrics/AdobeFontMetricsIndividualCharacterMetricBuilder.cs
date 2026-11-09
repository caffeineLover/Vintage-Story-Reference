using System;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.AdobeFontMetrics
{
	// Token: 0x020000B1 RID: 177
	internal class AdobeFontMetricsIndividualCharacterMetricBuilder
	{
		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x0001E231 File Offset: 0x0001C431
		// (set) Token: 0x06000481 RID: 1153 RVA: 0x0001E239 File Offset: 0x0001C439
		public int CharacterCode { get; set; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x0001E242 File Offset: 0x0001C442
		// (set) Token: 0x06000483 RID: 1155 RVA: 0x0001E24A File Offset: 0x0001C44A
		public double WidthX { get; set; }

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000484 RID: 1156 RVA: 0x0001E253 File Offset: 0x0001C453
		// (set) Token: 0x06000485 RID: 1157 RVA: 0x0001E25B File Offset: 0x0001C45B
		public double WidthY { get; set; }

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000486 RID: 1158 RVA: 0x0001E264 File Offset: 0x0001C464
		// (set) Token: 0x06000487 RID: 1159 RVA: 0x0001E26C File Offset: 0x0001C46C
		public double WidthXDirection0 { get; set; }

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000488 RID: 1160 RVA: 0x0001E275 File Offset: 0x0001C475
		// (set) Token: 0x06000489 RID: 1161 RVA: 0x0001E27D File Offset: 0x0001C47D
		public double WidthYDirection0 { get; set; }

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x0001E286 File Offset: 0x0001C486
		// (set) Token: 0x0600048B RID: 1163 RVA: 0x0001E28E File Offset: 0x0001C48E
		public double WidthXDirection1 { get; set; }

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x0600048C RID: 1164 RVA: 0x0001E297 File Offset: 0x0001C497
		// (set) Token: 0x0600048D RID: 1165 RVA: 0x0001E29F File Offset: 0x0001C49F
		public double WidthYDirection1 { get; set; }

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x0600048E RID: 1166 RVA: 0x0001E2A8 File Offset: 0x0001C4A8
		// (set) Token: 0x0600048F RID: 1167 RVA: 0x0001E2B0 File Offset: 0x0001C4B0
		public string Name { get; set; }

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000490 RID: 1168 RVA: 0x0001E2B9 File Offset: 0x0001C4B9
		// (set) Token: 0x06000491 RID: 1169 RVA: 0x0001E2C1 File Offset: 0x0001C4C1
		public AdobeFontMetricsVector VVector { get; set; }

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000492 RID: 1170 RVA: 0x0001E2CA File Offset: 0x0001C4CA
		// (set) Token: 0x06000493 RID: 1171 RVA: 0x0001E2D2 File Offset: 0x0001C4D2
		public PdfRectangle BoundingBox { get; set; }

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000494 RID: 1172 RVA: 0x0001E2DB File Offset: 0x0001C4DB
		// (set) Token: 0x06000495 RID: 1173 RVA: 0x0001E2E3 File Offset: 0x0001C4E3
		public AdobeFontMetricsLigature Ligature { get; set; }

		// Token: 0x06000496 RID: 1174 RVA: 0x0001E2EC File Offset: 0x0001C4EC
		public AdobeFontMetricsIndividualCharacterMetric Build()
		{
			return new AdobeFontMetricsIndividualCharacterMetric(this.CharacterCode, this.Name, new AdobeFontMetricsVector(this.WidthX, this.WidthY), new AdobeFontMetricsVector(this.WidthXDirection0, this.WidthYDirection0), new AdobeFontMetricsVector(this.WidthXDirection1, this.WidthYDirection1), this.VVector, this.BoundingBox, this.Ligature);
		}
	}
}
