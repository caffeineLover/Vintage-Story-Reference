using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UglyToad.PdfPig.Fonts.Encodings;

namespace UglyToad.PdfPig.Fonts
{
	// Token: 0x02000005 RID: 5
	public class GlyphList
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600001E RID: 30 RVA: 0x000023A3 File Offset: 0x000005A3
		public static GlyphList AdobeGlyphList
		{
			get
			{
				return GlyphList.LazyAdobeGlyphList.Value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600001F RID: 31 RVA: 0x000023AF File Offset: 0x000005AF
		public static GlyphList AdditionalGlyphList
		{
			get
			{
				return GlyphList.LazyAdditionalGlyphList.Value;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000020 RID: 32 RVA: 0x000023BB File Offset: 0x000005BB
		public static GlyphList ZapfDingbats
		{
			get
			{
				return GlyphList.LazyZapfDingbatsGlyphList.Value;
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000023C8 File Offset: 0x000005C8
		internal GlyphList(IReadOnlyDictionary<string, string> namesToUnicode)
		{
			this.nameToUnicode = namesToUnicode;
			Dictionary<string, string> dictionary = new Dictionary<string, string>(namesToUnicode.Count);
			foreach (KeyValuePair<string, string> keyValuePair in namesToUnicode)
			{
				bool flag = WinAnsiEncoding.Instance.ContainsName(keyValuePair.Key) || MacRomanEncoding.Instance.ContainsName(keyValuePair.Key) || MacExpertEncoding.Instance.ContainsName(keyValuePair.Key) || SymbolEncoding.Instance.ContainsName(keyValuePair.Key) || ZapfDingbatsEncoding.Instance.ContainsName(keyValuePair.Key);
				if (!dictionary.ContainsKey(keyValuePair.Value) || flag)
				{
					dictionary[keyValuePair.Value] = keyValuePair.Key;
				}
			}
			this.unicodeToName = dictionary;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000024C4 File Offset: 0x000006C4
		public string UnicodeCodePointToName(int unicodeValue)
		{
			string text = char.ConvertFromUtf32(unicodeValue);
			string result;
			if (this.unicodeToName.TryGetValue(text, ref result))
			{
				return result;
			}
			return ".notdef";
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000024F0 File Offset: 0x000006F0
		public string NameToUnicode(string name)
		{
			if (name == null)
			{
				return null;
			}
			string result;
			if (this.nameToUnicode.TryGetValue(name, ref result))
			{
				return result;
			}
			string result2;
			if (this.oddNameToUnicodeCache.TryGetValue(name, ref result2))
			{
				return result2;
			}
			string text;
			if (name.IndexOf('.') > 0)
			{
				text = this.NameToUnicode(name.Substring(0, name.IndexOf('.')));
			}
			else if (name.StartsWith("uni") && name.Length == 7)
			{
				int length = name.Length;
				StringBuilder stringBuilder = new StringBuilder();
				bool flag = true;
				int num = 3;
				while (num + 4 <= length)
				{
					int num2;
					if (!int.TryParse(name.Substring(num, 4), 515, CultureInfo.InvariantCulture, ref num2))
					{
						flag = false;
						break;
					}
					if (num2 > 55295 && num2 < 57344)
					{
						throw new InvalidFontFormatException("Unicode character name with disallowed code area: " + name);
					}
					stringBuilder.Append((char)num2);
					num += 4;
				}
				if (!flag)
				{
					return null;
				}
				text = stringBuilder.ToString();
			}
			else
			{
				if (!name.StartsWith("u") || name.Length != 5)
				{
					return null;
				}
				int num3 = int.Parse(name.Substring(1), 515, CultureInfo.InvariantCulture);
				if (num3 > 55295 && num3 < 57344)
				{
					throw new InvalidFontFormatException("Unicode character name with disallowed code area: " + name);
				}
				text = char.ConvertFromUtf32(num3);
			}
			this.oddNameToUnicodeCache[name] = text;
			return text;
		}

		// Token: 0x04000015 RID: 21
		private const string NotDefined = ".notdef";

		// Token: 0x04000016 RID: 22
		private readonly IReadOnlyDictionary<string, string> nameToUnicode;

		// Token: 0x04000017 RID: 23
		private readonly IReadOnlyDictionary<string, string> unicodeToName;

		// Token: 0x04000018 RID: 24
		private readonly Dictionary<string, string> oddNameToUnicodeCache = new Dictionary<string, string>();

		// Token: 0x04000019 RID: 25
		private static readonly Lazy<GlyphList> LazyAdobeGlyphList = new Lazy<GlyphList>(() => GlyphListFactory.Get("glyphlist"));

		// Token: 0x0400001A RID: 26
		private static readonly Lazy<GlyphList> LazyAdditionalGlyphList = new Lazy<GlyphList>(() => GlyphListFactory.Get("additional"));

		// Token: 0x0400001B RID: 27
		private static readonly Lazy<GlyphList> LazyZapfDingbatsGlyphList = new Lazy<GlyphList>(() => GlyphListFactory.Get("zapfdingbats"));
	}
}
