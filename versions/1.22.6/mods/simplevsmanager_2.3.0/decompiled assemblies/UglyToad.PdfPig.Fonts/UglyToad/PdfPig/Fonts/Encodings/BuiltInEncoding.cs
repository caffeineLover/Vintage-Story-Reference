using System;
using System.Collections.Generic;

namespace UglyToad.PdfPig.Fonts.Encodings
{
	// Token: 0x02000077 RID: 119
	public class BuiltInEncoding : Encoding
	{
		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x0000E4BC File Offset: 0x0000C6BC
		public override string EncodingName
		{
			get
			{
				return "built-in (TTF)";
			}
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000E4C4 File Offset: 0x0000C6C4
		public BuiltInEncoding(IReadOnlyDictionary<int, string> codeToName)
		{
			foreach (KeyValuePair<int, string> keyValuePair in codeToName)
			{
				base.Add(keyValuePair.Key, keyValuePair.Value);
			}
		}
	}
}
