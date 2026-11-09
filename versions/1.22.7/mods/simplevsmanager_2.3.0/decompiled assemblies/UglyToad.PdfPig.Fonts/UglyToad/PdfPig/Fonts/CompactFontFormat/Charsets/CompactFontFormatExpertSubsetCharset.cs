using System;
using System.Collections.Generic;
using System.Linq;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat.Charsets
{
	// Token: 0x020000A6 RID: 166
	internal class CompactFontFormatExpertSubsetCharset : ICompactFontFormatCharset
	{
		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060003FD RID: 1021 RVA: 0x0001C58D File Offset: 0x0001A78D
		public static CompactFontFormatExpertSubsetCharset Value { get; }

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060003FE RID: 1022 RVA: 0x0001C594 File Offset: 0x0001A794
		public bool IsCidCharset { get; }

		// Token: 0x060003FF RID: 1023 RVA: 0x0001C59C File Offset: 0x0001A79C
		private CompactFontFormatExpertSubsetCharset()
		{
			Dictionary<int, KeyValuePair<int, string>> dictionary = new Dictionary<int, KeyValuePair<int, string>>();
			int num = 0;
			foreach (KeyValuePair<int, string> keyValuePair in CompactFontFormatExpertSubsetCharset.StringIdToName)
			{
				dictionary[num++] = keyValuePair;
			}
			this.glyphIdToStringIdAndName = dictionary;
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x0001C604 File Offset: 0x0001A804
		public string GetNameByGlyphId(int glyphId)
		{
			return this.glyphIdToStringIdAndName[glyphId].Value;
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x0001C628 File Offset: 0x0001A828
		public string GetNameByStringId(int stringId)
		{
			return Enumerable.Single<KeyValuePair<int, KeyValuePair<int, string>>>(this.glyphIdToStringIdAndName, (KeyValuePair<int, KeyValuePair<int, string>> x) => x.Value.Key == stringId).Value.Value;
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x0001C66C File Offset: 0x0001A86C
		public int GetStringIdByGlyphId(int glyphId)
		{
			return this.glyphIdToStringIdAndName[glyphId].Key;
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x0001C690 File Offset: 0x0001A890
		public int GetGlyphIdByName(string characterName)
		{
			foreach (KeyValuePair<int, KeyValuePair<int, string>> keyValuePair in this.glyphIdToStringIdAndName)
			{
				if (string.Equals(keyValuePair.Value.Value, characterName, 4))
				{
					return keyValuePair.Key;
				}
			}
			return 0;
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x0001C6FC File Offset: 0x0001A8FC
		// Note: this type is marked as 'beforefieldinit'.
		static CompactFontFormatExpertSubsetCharset()
		{
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			dictionary.Add(0, ".notdef");
			dictionary.Add(1, "space");
			dictionary.Add(231, "dollaroldstyle");
			dictionary.Add(232, "dollarsuperior");
			dictionary.Add(235, "parenleftsuperior");
			dictionary.Add(236, "parenrightsuperior");
			dictionary.Add(237, "twodotenleader");
			dictionary.Add(238, "onedotenleader");
			dictionary.Add(13, "comma");
			dictionary.Add(14, "hyphen");
			dictionary.Add(15, "period");
			dictionary.Add(99, "fraction");
			dictionary.Add(239, "zerooldstyle");
			dictionary.Add(240, "oneoldstyle");
			dictionary.Add(241, "twooldstyle");
			dictionary.Add(242, "threeoldstyle");
			dictionary.Add(243, "fouroldstyle");
			dictionary.Add(244, "fiveoldstyle");
			dictionary.Add(245, "sixoldstyle");
			dictionary.Add(246, "sevenoldstyle");
			dictionary.Add(247, "eightoldstyle");
			dictionary.Add(248, "nineoldstyle");
			dictionary.Add(27, "colon");
			dictionary.Add(28, "semicolon");
			dictionary.Add(249, "commasuperior");
			dictionary.Add(250, "threequartersemdash");
			dictionary.Add(251, "periodsuperior");
			dictionary.Add(253, "asuperior");
			dictionary.Add(254, "bsuperior");
			dictionary.Add(255, "centsuperior");
			dictionary.Add(256, "dsuperior");
			dictionary.Add(257, "esuperior");
			dictionary.Add(258, "isuperior");
			dictionary.Add(259, "lsuperior");
			dictionary.Add(260, "msuperior");
			dictionary.Add(261, "nsuperior");
			dictionary.Add(262, "osuperior");
			dictionary.Add(263, "rsuperior");
			dictionary.Add(264, "ssuperior");
			dictionary.Add(265, "tsuperior");
			dictionary.Add(266, "ff");
			dictionary.Add(109, "fi");
			dictionary.Add(110, "fl");
			dictionary.Add(267, "ffi");
			dictionary.Add(268, "ffl");
			dictionary.Add(269, "parenleftinferior");
			dictionary.Add(270, "parenrightinferior");
			dictionary.Add(272, "hyphensuperior");
			dictionary.Add(300, "colonmonetary");
			dictionary.Add(301, "onefitted");
			dictionary.Add(302, "rupiah");
			dictionary.Add(305, "centoldstyle");
			dictionary.Add(314, "figuredash");
			dictionary.Add(315, "hypheninferior");
			dictionary.Add(158, "onequarter");
			dictionary.Add(155, "onehalf");
			dictionary.Add(163, "threequarters");
			dictionary.Add(320, "oneeighth");
			dictionary.Add(321, "threeeighths");
			dictionary.Add(322, "fiveeighths");
			dictionary.Add(323, "seveneighths");
			dictionary.Add(324, "onethird");
			dictionary.Add(325, "twothirds");
			dictionary.Add(326, "zerosuperior");
			dictionary.Add(150, "onesuperior");
			dictionary.Add(164, "twosuperior");
			dictionary.Add(169, "threesuperior");
			dictionary.Add(327, "foursuperior");
			dictionary.Add(328, "fivesuperior");
			dictionary.Add(329, "sixsuperior");
			dictionary.Add(330, "sevensuperior");
			dictionary.Add(331, "eightsuperior");
			dictionary.Add(332, "ninesuperior");
			dictionary.Add(333, "zeroinferior");
			dictionary.Add(334, "oneinferior");
			dictionary.Add(335, "twoinferior");
			dictionary.Add(336, "threeinferior");
			dictionary.Add(337, "fourinferior");
			dictionary.Add(338, "fiveinferior");
			dictionary.Add(339, "sixinferior");
			dictionary.Add(340, "seveninferior");
			dictionary.Add(341, "eightinferior");
			dictionary.Add(342, "nineinferior");
			dictionary.Add(343, "centinferior");
			dictionary.Add(344, "dollarinferior");
			dictionary.Add(345, "periodinferior");
			dictionary.Add(346, "commainferior");
			CompactFontFormatExpertSubsetCharset.StringIdToName = dictionary;
			CompactFontFormatExpertSubsetCharset.Value = new CompactFontFormatExpertSubsetCharset();
		}

		// Token: 0x04000377 RID: 887
		private static readonly IReadOnlyDictionary<int, string> StringIdToName;

		// Token: 0x04000379 RID: 889
		private readonly IReadOnlyDictionary<int, KeyValuePair<int, string>> glyphIdToStringIdAndName;
	}
}
