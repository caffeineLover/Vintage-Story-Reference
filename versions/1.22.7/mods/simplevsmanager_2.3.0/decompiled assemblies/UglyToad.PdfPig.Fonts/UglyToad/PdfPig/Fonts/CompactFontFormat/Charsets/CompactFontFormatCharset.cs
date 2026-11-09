using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat.Charsets
{
	// Token: 0x020000A3 RID: 163
	internal abstract class CompactFontFormatCharset : ICompactFontFormatCharset
	{
		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x0001B7F5 File Offset: 0x000199F5
		public bool IsCidCharset { get; }

		// Token: 0x060003EA RID: 1002 RVA: 0x0001B800 File Offset: 0x00019A00
		protected CompactFontFormatCharset([TupleElementNames(new string[]
		{
			"glyphId",
			"stringId",
			"name"
		})] IReadOnlyList<ValueTuple<int, int, string>> data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("data");
			}
			Dictionary<int, ValueTuple<int, string>> dictionary = new Dictionary<int, ValueTuple<int, string>>();
			dictionary.Add(0, new ValueTuple<int, string>(0, ".notdef"));
			Dictionary<int, ValueTuple<int, string>> dictionary2 = dictionary;
			foreach (ValueTuple<int, int, string> valueTuple in data)
			{
				dictionary2[valueTuple.Item1] = new ValueTuple<int, string>(valueTuple.Item2, valueTuple.Item3);
			}
			this.GlyphIdToStringIdAndName = dictionary2;
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x0001B894 File Offset: 0x00019A94
		public virtual string GetNameByGlyphId(int glyphId)
		{
			return this.GlyphIdToStringIdAndName[glyphId].Item2;
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x0001B8A8 File Offset: 0x00019AA8
		public virtual string GetNameByStringId(int stringId)
		{
			return Enumerable.Single<KeyValuePair<int, ValueTuple<int, string>>>(this.GlyphIdToStringIdAndName, ([TupleElementNames(new string[]
			{
				"stringId",
				"name"
			})] KeyValuePair<int, ValueTuple<int, string>> x) => x.Value.Item1 == stringId).Value.Item2;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x0001B8E8 File Offset: 0x00019AE8
		public virtual int GetStringIdByGlyphId(int glyphId)
		{
			ValueTuple<int, string> valueTuple;
			if (this.GlyphIdToStringIdAndName.TryGetValue(glyphId, ref valueTuple))
			{
				return valueTuple.Item1;
			}
			return 0;
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x0001B910 File Offset: 0x00019B10
		public int GetGlyphIdByName(string characterName)
		{
			foreach (KeyValuePair<int, ValueTuple<int, string>> keyValuePair in this.GlyphIdToStringIdAndName)
			{
				if (string.Equals(keyValuePair.Value.Item2, characterName, 4))
				{
					return keyValuePair.Key;
				}
			}
			return 0;
		}

		// Token: 0x0400036F RID: 879
		[TupleElementNames(new string[]
		{
			"stringId",
			"name"
		})]
		protected readonly IReadOnlyDictionary<int, ValueTuple<int, string>> GlyphIdToStringIdAndName;
	}
}
