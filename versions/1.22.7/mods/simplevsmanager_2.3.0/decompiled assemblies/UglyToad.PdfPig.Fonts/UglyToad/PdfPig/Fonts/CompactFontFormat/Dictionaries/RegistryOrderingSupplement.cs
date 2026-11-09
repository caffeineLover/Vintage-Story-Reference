using System;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat.Dictionaries
{
	// Token: 0x0200009B RID: 155
	internal class RegistryOrderingSupplement
	{
		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060003B7 RID: 951 RVA: 0x00019F94 File Offset: 0x00018194
		// (set) Token: 0x060003B8 RID: 952 RVA: 0x00019F9C File Offset: 0x0001819C
		public string Registry { get; set; }

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x00019FA5 File Offset: 0x000181A5
		// (set) Token: 0x060003BA RID: 954 RVA: 0x00019FAD File Offset: 0x000181AD
		public string Ordering { get; set; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060003BB RID: 955 RVA: 0x00019FB6 File Offset: 0x000181B6
		// (set) Token: 0x060003BC RID: 956 RVA: 0x00019FBE File Offset: 0x000181BE
		public decimal Supplement { get; set; }
	}
}
