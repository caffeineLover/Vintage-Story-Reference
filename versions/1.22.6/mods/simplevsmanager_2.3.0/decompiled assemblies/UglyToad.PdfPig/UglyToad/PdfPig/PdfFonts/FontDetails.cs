using System;

namespace UglyToad.PdfPig.PdfFonts
{
	// Token: 0x0200008F RID: 143
	public class FontDetails
	{
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000256 RID: 598 RVA: 0x0000FF96 File Offset: 0x0000E196
		public string Name { get; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000257 RID: 599 RVA: 0x0000FF9E File Offset: 0x0000E19E
		public bool IsBold { get; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000258 RID: 600 RVA: 0x0000FFA6 File Offset: 0x0000E1A6
		public int Weight { get; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000FFAE File Offset: 0x0000E1AE
		public bool IsItalic { get; }

		// Token: 0x0600025A RID: 602 RVA: 0x0000FFB6 File Offset: 0x0000E1B6
		public FontDetails(string name, bool isBold, int weight, bool isItalic)
		{
			this.Name = (name ?? string.Empty);
			this.IsBold = isBold;
			this.Weight = weight;
			this.IsItalic = isItalic;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000FFE4 File Offset: 0x0000E1E4
		internal static FontDetails GetDefault(string name = null)
		{
			return new FontDetails(name ?? string.Empty, false, 500, false);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000FFFC File Offset: 0x0000E1FC
		internal FontDetails WithName(string name)
		{
			if (name == null)
			{
				return this;
			}
			return new FontDetails(name, this.IsBold, this.Weight, this.IsItalic);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0001001C File Offset: 0x0000E21C
		public override string ToString()
		{
			string text = this.IsBold ? " (bold)" : string.Empty;
			string text2 = this.IsItalic ? " (italic)" : string.Empty;
			return this.Name + text + text2;
		}

		// Token: 0x04000136 RID: 310
		public const int DefaultWeight = 500;

		// Token: 0x04000137 RID: 311
		public const int BoldWeight = 700;
	}
}
