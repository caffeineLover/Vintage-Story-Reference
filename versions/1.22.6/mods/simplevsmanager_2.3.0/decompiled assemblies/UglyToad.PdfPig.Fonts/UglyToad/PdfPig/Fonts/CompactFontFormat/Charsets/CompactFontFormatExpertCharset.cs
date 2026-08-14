using System;
using System.Collections.Generic;
using System.Linq;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat.Charsets
{
	// Token: 0x020000A5 RID: 165
	internal class CompactFontFormatExpertCharset : ICompactFontFormatCharset
	{
		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060003F5 RID: 1013 RVA: 0x0001B9BD File Offset: 0x00019BBD
		public static CompactFontFormatExpertCharset Value { get; }

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x0001B9C4 File Offset: 0x00019BC4
		public bool IsCidCharset { get; }

		// Token: 0x060003F7 RID: 1015 RVA: 0x0001B9CC File Offset: 0x00019BCC
		private CompactFontFormatExpertCharset()
		{
			Dictionary<int, KeyValuePair<int, string>> dictionary = new Dictionary<int, KeyValuePair<int, string>>();
			int num = 0;
			foreach (KeyValuePair<int, string> keyValuePair in CompactFontFormatExpertCharset.StringIdToName)
			{
				dictionary[num++] = keyValuePair;
			}
			this.glyphIdToStringIdAndName = dictionary;
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x0001BA34 File Offset: 0x00019C34
		public string GetNameByGlyphId(int glyphId)
		{
			return this.glyphIdToStringIdAndName[glyphId].Value;
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0001BA58 File Offset: 0x00019C58
		public string GetNameByStringId(int stringId)
		{
			return Enumerable.Single<KeyValuePair<int, KeyValuePair<int, string>>>(this.glyphIdToStringIdAndName, (KeyValuePair<int, KeyValuePair<int, string>> x) => x.Value.Key == stringId).Value.Value;
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x0001BA9C File Offset: 0x00019C9C
		public int GetStringIdByGlyphId(int glyphId)
		{
			return this.glyphIdToStringIdAndName[glyphId].Key;
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x0001BAC0 File Offset: 0x00019CC0
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

		// Token: 0x060003FC RID: 1020 RVA: 0x0001BB2C File Offset: 0x00019D2C
		// Note: this type is marked as 'beforefieldinit'.
		static CompactFontFormatExpertCharset()
		{
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			dictionary.Add(0, ".notdef");
			dictionary.Add(1, "space");
			dictionary.Add(229, "exclamsmall");
			dictionary.Add(230, "Hungarumlautsmall");
			dictionary.Add(231, "dollaroldstyle");
			dictionary.Add(232, "dollarsuperior");
			dictionary.Add(233, "ampersandsmall");
			dictionary.Add(234, "Acutesmall");
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
			dictionary.Add(252, "questionsmall");
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
			dictionary.Add(271, "Circumflexsmall");
			dictionary.Add(272, "hyphensuperior");
			dictionary.Add(273, "Gravesmall");
			dictionary.Add(274, "Asmall");
			dictionary.Add(275, "Bsmall");
			dictionary.Add(276, "Csmall");
			dictionary.Add(277, "Dsmall");
			dictionary.Add(278, "Esmall");
			dictionary.Add(279, "Fsmall");
			dictionary.Add(280, "Gsmall");
			dictionary.Add(281, "Hsmall");
			dictionary.Add(282, "Ismall");
			dictionary.Add(283, "Jsmall");
			dictionary.Add(284, "Ksmall");
			dictionary.Add(285, "Lsmall");
			dictionary.Add(286, "Msmall");
			dictionary.Add(287, "Nsmall");
			dictionary.Add(288, "Osmall");
			dictionary.Add(289, "Psmall");
			dictionary.Add(290, "Qsmall");
			dictionary.Add(291, "Rsmall");
			dictionary.Add(292, "Ssmall");
			dictionary.Add(293, "Tsmall");
			dictionary.Add(294, "Usmall");
			dictionary.Add(295, "Vsmall");
			dictionary.Add(296, "Wsmall");
			dictionary.Add(297, "Xsmall");
			dictionary.Add(298, "Ysmall");
			dictionary.Add(299, "Zsmall");
			dictionary.Add(300, "colonmonetary");
			dictionary.Add(301, "onefitted");
			dictionary.Add(302, "rupiah");
			dictionary.Add(303, "Tildesmall");
			dictionary.Add(304, "exclamdownsmall");
			dictionary.Add(305, "centoldstyle");
			dictionary.Add(306, "Lslashsmall");
			dictionary.Add(307, "Scaronsmall");
			dictionary.Add(308, "Zcaronsmall");
			dictionary.Add(309, "Dieresissmall");
			dictionary.Add(310, "Brevesmall");
			dictionary.Add(311, "Caronsmall");
			dictionary.Add(312, "Dotaccentsmall");
			dictionary.Add(313, "Macronsmall");
			dictionary.Add(314, "figuredash");
			dictionary.Add(315, "hypheninferior");
			dictionary.Add(316, "Ogoneksmall");
			dictionary.Add(317, "Ringsmall");
			dictionary.Add(318, "Cedillasmall");
			dictionary.Add(158, "onequarter");
			dictionary.Add(155, "onehalf");
			dictionary.Add(163, "threequarters");
			dictionary.Add(319, "questiondownsmall");
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
			dictionary.Add(347, "Agravesmall");
			dictionary.Add(348, "Aacutesmall");
			dictionary.Add(349, "Acircumflexsmall");
			dictionary.Add(350, "Atildesmall");
			dictionary.Add(351, "Adieresissmall");
			dictionary.Add(352, "Aringsmall");
			dictionary.Add(353, "AEsmall");
			dictionary.Add(354, "Ccedillasmall");
			dictionary.Add(355, "Egravesmall");
			dictionary.Add(356, "Eacutesmall");
			dictionary.Add(357, "Ecircumflexsmall");
			dictionary.Add(358, "Edieresissmall");
			dictionary.Add(359, "Igravesmall");
			dictionary.Add(360, "Iacutesmall");
			dictionary.Add(361, "Icircumflexsmall");
			dictionary.Add(362, "Idieresissmall");
			dictionary.Add(363, "Ethsmall");
			dictionary.Add(364, "Ntildesmall");
			dictionary.Add(365, "Ogravesmall");
			dictionary.Add(366, "Oacutesmall");
			dictionary.Add(367, "Ocircumflexsmall");
			dictionary.Add(368, "Otildesmall");
			dictionary.Add(369, "Odieresissmall");
			dictionary.Add(370, "OEsmall");
			dictionary.Add(371, "Oslashsmall");
			dictionary.Add(372, "Ugravesmall");
			dictionary.Add(373, "Uacutesmall");
			dictionary.Add(374, "Ucircumflexsmall");
			dictionary.Add(375, "Udieresissmall");
			dictionary.Add(376, "Yacutesmall");
			dictionary.Add(377, "Thornsmall");
			dictionary.Add(378, "Ydieresissmall");
			CompactFontFormatExpertCharset.StringIdToName = dictionary;
			CompactFontFormatExpertCharset.Value = new CompactFontFormatExpertCharset();
		}

		// Token: 0x04000373 RID: 883
		private static readonly IReadOnlyDictionary<int, string> StringIdToName;

		// Token: 0x04000375 RID: 885
		private readonly IReadOnlyDictionary<int, KeyValuePair<int, string>> glyphIdToStringIdAndName;
	}
}
