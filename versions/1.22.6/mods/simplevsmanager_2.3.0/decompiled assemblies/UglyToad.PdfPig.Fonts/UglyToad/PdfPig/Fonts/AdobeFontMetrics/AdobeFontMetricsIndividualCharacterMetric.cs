using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;

namespace UglyToad.PdfPig.Fonts.AdobeFontMetrics
{
	// Token: 0x020000B0 RID: 176
	public class AdobeFontMetricsIndividualCharacterMetric
	{
		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000476 RID: 1142 RVA: 0x0001E12C File Offset: 0x0001C32C
		public int CharacterCode { get; }

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000477 RID: 1143 RVA: 0x0001E134 File Offset: 0x0001C334
		public string Name { get; }

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000478 RID: 1144 RVA: 0x0001E13C File Offset: 0x0001C33C
		public AdobeFontMetricsVector Width { get; }

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000479 RID: 1145 RVA: 0x0001E144 File Offset: 0x0001C344
		public AdobeFontMetricsVector WidthDirection0 { get; }

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x0600047A RID: 1146 RVA: 0x0001E14C File Offset: 0x0001C34C
		public AdobeFontMetricsVector WidthDirection1 { get; }

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600047B RID: 1147 RVA: 0x0001E154 File Offset: 0x0001C354
		public AdobeFontMetricsVector VVector { get; }

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600047C RID: 1148 RVA: 0x0001E15C File Offset: 0x0001C35C
		public PdfRectangle BoundingBox { get; }

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x0600047D RID: 1149 RVA: 0x0001E164 File Offset: 0x0001C364
		public AdobeFontMetricsLigature Ligature { get; }

		// Token: 0x0600047E RID: 1150 RVA: 0x0001E16C File Offset: 0x0001C36C
		public AdobeFontMetricsIndividualCharacterMetric(int characterCode, string name, AdobeFontMetricsVector width, AdobeFontMetricsVector widthDirection0, AdobeFontMetricsVector widthDirection1, AdobeFontMetricsVector vVector, PdfRectangle boundingBox, AdobeFontMetricsLigature ligature)
		{
			this.CharacterCode = characterCode;
			this.Name = name;
			this.Width = width;
			this.WidthDirection0 = widthDirection0;
			this.WidthDirection1 = widthDirection1;
			this.VVector = vVector;
			this.BoundingBox = boundingBox;
			this.Ligature = ligature;
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0001E1BC File Offset: 0x0001C3BC
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(12, 3);
			defaultInterpolatedStringHandler.AppendLiteral("[");
			defaultInterpolatedStringHandler.AppendFormatted<int>(this.CharacterCode);
			defaultInterpolatedStringHandler.AppendLiteral("] ");
			defaultInterpolatedStringHandler.AppendFormatted(this.Name);
			defaultInterpolatedStringHandler.AppendLiteral(" Width: ");
			defaultInterpolatedStringHandler.AppendFormatted<AdobeFontMetricsVector>(this.Width);
			defaultInterpolatedStringHandler.AppendLiteral(".");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
