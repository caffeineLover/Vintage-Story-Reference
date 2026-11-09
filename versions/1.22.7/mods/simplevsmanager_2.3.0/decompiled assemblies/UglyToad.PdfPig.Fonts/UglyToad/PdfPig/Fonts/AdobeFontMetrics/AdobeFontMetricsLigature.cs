using System;

namespace UglyToad.PdfPig.Fonts.AdobeFontMetrics
{
	// Token: 0x020000B2 RID: 178
	public class AdobeFontMetricsLigature
	{
		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000498 RID: 1176 RVA: 0x0001E357 File Offset: 0x0001C557
		public string Successor { get; }

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x0001E35F File Offset: 0x0001C55F
		public string Value { get; }

		// Token: 0x0600049A RID: 1178 RVA: 0x0001E367 File Offset: 0x0001C567
		public AdobeFontMetricsLigature(string successor, string value)
		{
			this.Successor = successor;
			this.Value = value;
		}

		// Token: 0x0600049B RID: 1179 RVA: 0x0001E37D File Offset: 0x0001C57D
		public override string ToString()
		{
			return "Ligature: " + this.Value + " -> Successor: " + this.Successor;
		}
	}
}
