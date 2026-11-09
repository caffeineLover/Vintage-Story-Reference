using System;
using System.Collections.Generic;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat
{
	// Token: 0x02000082 RID: 130
	internal abstract class CompactFontFormatBuiltInEncoding : CompactFontFormatBaseEncoding
	{
		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x00015124 File Offset: 0x00013324
		public IReadOnlyList<CompactFontFormatBuiltInEncoding.Supplement> Supplements { get; }

		// Token: 0x060002F3 RID: 755 RVA: 0x0001512C File Offset: 0x0001332C
		protected CompactFontFormatBuiltInEncoding(IReadOnlyList<CompactFontFormatBuiltInEncoding.Supplement> supplements)
		{
			this.Supplements = supplements;
			foreach (CompactFontFormatBuiltInEncoding.Supplement supplement in supplements)
			{
				base.Add(supplement.Code, supplement.Sid, supplement.Name);
			}
		}

		// Token: 0x020000D9 RID: 217
		public class Supplement
		{
			// Token: 0x17000264 RID: 612
			// (get) Token: 0x0600056B RID: 1387 RVA: 0x000202BF File Offset: 0x0001E4BF
			public int Code { get; }

			// Token: 0x17000265 RID: 613
			// (get) Token: 0x0600056C RID: 1388 RVA: 0x000202C7 File Offset: 0x0001E4C7
			public int Sid { get; }

			// Token: 0x17000266 RID: 614
			// (get) Token: 0x0600056D RID: 1389 RVA: 0x000202CF File Offset: 0x0001E4CF
			public string Name { get; }

			// Token: 0x0600056E RID: 1390 RVA: 0x000202D7 File Offset: 0x0001E4D7
			public Supplement(int code, int sid, string name)
			{
				this.Code = code;
				this.Sid = sid;
				this.Name = name;
			}
		}
	}
}
