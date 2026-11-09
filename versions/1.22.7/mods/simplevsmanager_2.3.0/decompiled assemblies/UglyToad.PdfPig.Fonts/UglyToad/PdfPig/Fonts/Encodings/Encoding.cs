using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Fonts.Encodings
{
	// Token: 0x02000079 RID: 121
	public abstract class Encoding
	{
		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x0000E630 File Offset: 0x0000C830
		public IReadOnlyDictionary<int, string> CodeToNameMap
		{
			get
			{
				return this.CodeToName;
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060002C9 RID: 713 RVA: 0x0000E638 File Offset: 0x0000C838
		public IReadOnlyDictionary<string, int> NameToCodeMap
		{
			get
			{
				return this.NameToCode;
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060002CA RID: 714
		public abstract string EncodingName { get; }

		// Token: 0x060002CB RID: 715 RVA: 0x0000E640 File Offset: 0x0000C840
		public bool ContainsName(string name)
		{
			return this.NameToCode.ContainsKey(name);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000E64E File Offset: 0x0000C84E
		public bool ContainsCode(int code)
		{
			return this.CodeToName.ContainsKey(code);
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000E65C File Offset: 0x0000C85C
		public virtual string GetName(int code)
		{
			string result;
			if (!this.CodeToName.TryGetValue(code, ref result))
			{
				return ".notdef";
			}
			return result;
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0000E680 File Offset: 0x0000C880
		public virtual int GetCode(string name)
		{
			int result;
			if (!this.NameToCode.TryGetValue(name, ref result))
			{
				return -1;
			}
			return result;
		}

		// Token: 0x060002CF RID: 719 RVA: 0x0000E6A0 File Offset: 0x0000C8A0
		protected void Add(int code, string name)
		{
			this.CodeToName[code] = name;
			if (!this.NameToCode.ContainsKey(name))
			{
				this.NameToCode[name] = code;
			}
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0000E6CC File Offset: 0x0000C8CC
		public static bool TryGetNamedEncoding(NameToken name, out Encoding encoding)
		{
			encoding = null;
			if (name == null)
			{
				return false;
			}
			if (name.Equals(NameToken.StandardEncoding))
			{
				encoding = StandardEncoding.Instance;
				return true;
			}
			if (name.Equals(NameToken.WinAnsiEncoding))
			{
				encoding = WinAnsiEncoding.Instance;
				return true;
			}
			if (name.Equals(NameToken.MacExpertEncoding))
			{
				encoding = MacExpertEncoding.Instance;
				return true;
			}
			if (name.Equals(NameToken.MacRomanEncoding))
			{
				encoding = MacRomanEncoding.Instance;
				return true;
			}
			return false;
		}

		// Token: 0x040002ED RID: 749
		protected readonly Dictionary<int, string> CodeToName = new Dictionary<int, string>(250);

		// Token: 0x040002EE RID: 750
		protected readonly Dictionary<string, int> NameToCode = new Dictionary<string, int>(250);
	}
}
