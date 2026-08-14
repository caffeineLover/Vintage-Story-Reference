using System;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x0200017D RID: 381
	internal struct ResourceColorSpace
	{
		// Token: 0x17000275 RID: 629
		// (get) Token: 0x060008D2 RID: 2258 RVA: 0x0002862A File Offset: 0x0002682A
		public readonly NameToken Name { get; }

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x060008D3 RID: 2259 RVA: 0x00028632 File Offset: 0x00026832
		public readonly IToken Data { get; }

		// Token: 0x060008D4 RID: 2260 RVA: 0x0002863A File Offset: 0x0002683A
		public ResourceColorSpace(NameToken name, IToken data)
		{
			this.Name = name;
			this.Data = data;
		}

		// Token: 0x060008D5 RID: 2261 RVA: 0x0002864A File Offset: 0x0002684A
		public ResourceColorSpace(NameToken name)
		{
			this = new ResourceColorSpace(name, null);
		}
	}
}
