using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Content;

namespace UglyToad.PdfPig.Util
{
	// Token: 0x02000031 RID: 49
	public interface IWordExtractor
	{
		// Token: 0x06000138 RID: 312
		IEnumerable<Word> GetWords(IReadOnlyList<Letter> letters);
	}
}
