using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Fonts.Encodings;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat
{
	// Token: 0x02000081 RID: 129
	internal abstract class CompactFontFormatBaseEncoding : Encoding
	{
		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060002ED RID: 749 RVA: 0x00015094 File Offset: 0x00013294
		public override string EncodingName { get; } = "CFF";

		// Token: 0x060002EE RID: 750 RVA: 0x0001509C File Offset: 0x0001329C
		public override string GetName(int code)
		{
			string result;
			if (!this.codeToNameMap.TryGetValue(code, ref result))
			{
				return ".notdef";
			}
			return result;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x000150C0 File Offset: 0x000132C0
		public void Add(int code, int sid, string name)
		{
			this.codeToNameMap[code] = name;
			base.Add(code, name);
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x000150D8 File Offset: 0x000132D8
		protected void Add(int code, int sid)
		{
			string name = CompactFontFormatStandardStrings.GetName(sid);
			this.codeToNameMap[code] = name;
			base.Add(code, name);
		}

		// Token: 0x040002FE RID: 766
		private readonly Dictionary<int, string> codeToNameMap = new Dictionary<int, string>(250);
	}
}
