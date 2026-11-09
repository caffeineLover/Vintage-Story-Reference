using System;
using System.Collections.Generic;

namespace UglyToad.PdfPig.Fonts.TrueType.Subsetting
{
	// Token: 0x02000052 RID: 82
	public class TrueTypeSubsetEncoding
	{
		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000245 RID: 581 RVA: 0x0000B7C8 File Offset: 0x000099C8
		public IReadOnlyList<char> Characters { get; }

		// Token: 0x06000246 RID: 582 RVA: 0x0000B7D0 File Offset: 0x000099D0
		public TrueTypeSubsetEncoding(IReadOnlyList<char> characters)
		{
			this.Characters = characters;
		}
	}
}
