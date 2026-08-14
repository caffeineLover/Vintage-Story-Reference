using System;

namespace UglyToad.PdfPig.PdfFonts.Cmap
{
	// Token: 0x020000AF RID: 175
	internal class CharacterIdentifierSystemInfoBuilder
	{
		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000306 RID: 774 RVA: 0x00014819 File Offset: 0x00012A19
		// (set) Token: 0x06000307 RID: 775 RVA: 0x00014821 File Offset: 0x00012A21
		public string Registry
		{
			get
			{
				return this.registry;
			}
			set
			{
				this.registry = value;
				this.HasRegistry = true;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000308 RID: 776 RVA: 0x00014831 File Offset: 0x00012A31
		// (set) Token: 0x06000309 RID: 777 RVA: 0x00014839 File Offset: 0x00012A39
		public bool HasRegistry { get; private set; }

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x0600030A RID: 778 RVA: 0x00014842 File Offset: 0x00012A42
		// (set) Token: 0x0600030B RID: 779 RVA: 0x0001484A File Offset: 0x00012A4A
		public string Ordering
		{
			get
			{
				return this.ordering;
			}
			set
			{
				this.ordering = value;
				this.HasOrdering = true;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600030C RID: 780 RVA: 0x0001485A File Offset: 0x00012A5A
		// (set) Token: 0x0600030D RID: 781 RVA: 0x00014862 File Offset: 0x00012A62
		public bool HasOrdering { get; private set; }

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600030E RID: 782 RVA: 0x0001486B File Offset: 0x00012A6B
		// (set) Token: 0x0600030F RID: 783 RVA: 0x00014873 File Offset: 0x00012A73
		public int Supplement
		{
			get
			{
				return this.supplement;
			}
			set
			{
				this.supplement = value;
				this.HasSupplement = true;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x06000310 RID: 784 RVA: 0x00014883 File Offset: 0x00012A83
		// (set) Token: 0x06000311 RID: 785 RVA: 0x0001488B File Offset: 0x00012A8B
		public bool HasSupplement { get; private set; }

		// Token: 0x0400019D RID: 413
		private string registry;

		// Token: 0x0400019F RID: 415
		private string ordering;

		// Token: 0x040001A1 RID: 417
		private int supplement;
	}
}
