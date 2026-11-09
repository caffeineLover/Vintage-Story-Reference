using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Content
{
	// Token: 0x020001F3 RID: 499
	internal class PageTreeMembers
	{
		// Token: 0x06000C09 RID: 3081 RVA: 0x0003BC22 File Offset: 0x00039E22
		public CropBox GetCropBox()
		{
			return null;
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000C0A RID: 3082 RVA: 0x0003BC25 File Offset: 0x00039E25
		// (set) Token: 0x06000C0B RID: 3083 RVA: 0x0003BC2D File Offset: 0x00039E2D
		public MediaBox MediaBox { get; set; }

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000C0C RID: 3084 RVA: 0x0003BC36 File Offset: 0x00039E36
		// (set) Token: 0x06000C0D RID: 3085 RVA: 0x0003BC3E File Offset: 0x00039E3E
		public int Rotation { get; set; }

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000C0E RID: 3086 RVA: 0x0003BC47 File Offset: 0x00039E47
		public Queue<DictionaryToken> ParentResources { get; } = new Queue<DictionaryToken>();
	}
}
