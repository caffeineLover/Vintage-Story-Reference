using System;
using System.Collections.Generic;
using System.Linq;

namespace UglyToad.PdfPig.Fonts.CompactFontFormat.Charsets
{
	// Token: 0x020000AA RID: 170
	internal class CompactFontFormatIsoAdobeCharset : ICompactFontFormatCharset
	{
		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x0001CC88 File Offset: 0x0001AE88
		public static CompactFontFormatIsoAdobeCharset Value { get; }

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x0001CC8F File Offset: 0x0001AE8F
		public bool IsCidCharset { get; }

		// Token: 0x0600040A RID: 1034 RVA: 0x0001CC98 File Offset: 0x0001AE98
		private CompactFontFormatIsoAdobeCharset()
		{
			Dictionary<int, KeyValuePair<int, string>> dictionary = new Dictionary<int, KeyValuePair<int, string>>();
			int num = 0;
			foreach (KeyValuePair<int, string> keyValuePair in CompactFontFormatIsoAdobeCharset.StringIdToName)
			{
				dictionary[num++] = keyValuePair;
			}
			this.glyphIdToStringIdAndName = dictionary;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x0001CD00 File Offset: 0x0001AF00
		public string GetNameByGlyphId(int glyphId)
		{
			return this.glyphIdToStringIdAndName[glyphId].Value;
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x0001CD24 File Offset: 0x0001AF24
		public string GetNameByStringId(int stringId)
		{
			return Enumerable.Single<KeyValuePair<int, KeyValuePair<int, string>>>(this.glyphIdToStringIdAndName, (KeyValuePair<int, KeyValuePair<int, string>> x) => x.Value.Key == stringId).Value.Value;
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0001CD68 File Offset: 0x0001AF68
		public int GetStringIdByGlyphId(int glyphId)
		{
			return this.glyphIdToStringIdAndName[glyphId].Key;
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0001CD8C File Offset: 0x0001AF8C
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

		// Token: 0x0600040F RID: 1039 RVA: 0x0001CDF8 File Offset: 0x0001AFF8
		// Note: this type is marked as 'beforefieldinit'.
		static CompactFontFormatIsoAdobeCharset()
		{
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			dictionary.Add(0, ".notdef");
			dictionary.Add(1, "space");
			dictionary.Add(2, "exclam");
			dictionary.Add(3, "quotedbl");
			dictionary.Add(4, "numbersign");
			dictionary.Add(5, "dollar");
			dictionary.Add(6, "percent");
			dictionary.Add(7, "ampersand");
			dictionary.Add(8, "quoteright");
			dictionary.Add(9, "parenleft");
			dictionary.Add(10, "parenright");
			dictionary.Add(11, "asterisk");
			dictionary.Add(12, "plus");
			dictionary.Add(13, "comma");
			dictionary.Add(14, "hyphen");
			dictionary.Add(15, "period");
			dictionary.Add(16, "slash");
			dictionary.Add(17, "zero");
			dictionary.Add(18, "one");
			dictionary.Add(19, "two");
			dictionary.Add(20, "three");
			dictionary.Add(21, "four");
			dictionary.Add(22, "five");
			dictionary.Add(23, "six");
			dictionary.Add(24, "seven");
			dictionary.Add(25, "eight");
			dictionary.Add(26, "nine");
			dictionary.Add(27, "colon");
			dictionary.Add(28, "semicolon");
			dictionary.Add(29, "less");
			dictionary.Add(30, "equal");
			dictionary.Add(31, "greater");
			dictionary.Add(32, "question");
			dictionary.Add(33, "at");
			dictionary.Add(34, "A");
			dictionary.Add(35, "B");
			dictionary.Add(36, "C");
			dictionary.Add(37, "D");
			dictionary.Add(38, "E");
			dictionary.Add(39, "F");
			dictionary.Add(40, "G");
			dictionary.Add(41, "H");
			dictionary.Add(42, "I");
			dictionary.Add(43, "J");
			dictionary.Add(44, "K");
			dictionary.Add(45, "L");
			dictionary.Add(46, "M");
			dictionary.Add(47, "N");
			dictionary.Add(48, "O");
			dictionary.Add(49, "P");
			dictionary.Add(50, "Q");
			dictionary.Add(51, "R");
			dictionary.Add(52, "S");
			dictionary.Add(53, "T");
			dictionary.Add(54, "U");
			dictionary.Add(55, "V");
			dictionary.Add(56, "W");
			dictionary.Add(57, "X");
			dictionary.Add(58, "Y");
			dictionary.Add(59, "Z");
			dictionary.Add(60, "bracketleft");
			dictionary.Add(61, "backslash");
			dictionary.Add(62, "bracketright");
			dictionary.Add(63, "asciicircum");
			dictionary.Add(64, "underscore");
			dictionary.Add(65, "quoteleft");
			dictionary.Add(66, "a");
			dictionary.Add(67, "b");
			dictionary.Add(68, "c");
			dictionary.Add(69, "d");
			dictionary.Add(70, "e");
			dictionary.Add(71, "f");
			dictionary.Add(72, "g");
			dictionary.Add(73, "h");
			dictionary.Add(74, "i");
			dictionary.Add(75, "j");
			dictionary.Add(76, "k");
			dictionary.Add(77, "l");
			dictionary.Add(78, "m");
			dictionary.Add(79, "n");
			dictionary.Add(80, "o");
			dictionary.Add(81, "p");
			dictionary.Add(82, "q");
			dictionary.Add(83, "r");
			dictionary.Add(84, "s");
			dictionary.Add(85, "t");
			dictionary.Add(86, "u");
			dictionary.Add(87, "v");
			dictionary.Add(88, "w");
			dictionary.Add(89, "x");
			dictionary.Add(90, "y");
			dictionary.Add(91, "z");
			dictionary.Add(92, "braceleft");
			dictionary.Add(93, "bar");
			dictionary.Add(94, "braceright");
			dictionary.Add(95, "asciitilde");
			dictionary.Add(96, "exclamdown");
			dictionary.Add(97, "cent");
			dictionary.Add(98, "sterling");
			dictionary.Add(99, "fraction");
			dictionary.Add(100, "yen");
			dictionary.Add(101, "florin");
			dictionary.Add(102, "section");
			dictionary.Add(103, "currency");
			dictionary.Add(104, "quotesingle");
			dictionary.Add(105, "quotedblleft");
			dictionary.Add(106, "guillemotleft");
			dictionary.Add(107, "guilsinglleft");
			dictionary.Add(108, "guilsinglright");
			dictionary.Add(109, "fi");
			dictionary.Add(110, "fl");
			dictionary.Add(111, "endash");
			dictionary.Add(112, "dagger");
			dictionary.Add(113, "daggerdbl");
			dictionary.Add(114, "periodcentered");
			dictionary.Add(115, "paragraph");
			dictionary.Add(116, "bullet");
			dictionary.Add(117, "quotesinglbase");
			dictionary.Add(118, "quotedblbase");
			dictionary.Add(119, "quotedblright");
			dictionary.Add(120, "guillemotright");
			dictionary.Add(121, "ellipsis");
			dictionary.Add(122, "perthousand");
			dictionary.Add(123, "questiondown");
			dictionary.Add(124, "grave");
			dictionary.Add(125, "acute");
			dictionary.Add(126, "circumflex");
			dictionary.Add(127, "tilde");
			dictionary.Add(128, "macron");
			dictionary.Add(129, "breve");
			dictionary.Add(130, "dotaccent");
			dictionary.Add(131, "dieresis");
			dictionary.Add(132, "ring");
			dictionary.Add(133, "cedilla");
			dictionary.Add(134, "hungarumlaut");
			dictionary.Add(135, "ogonek");
			dictionary.Add(136, "caron");
			dictionary.Add(137, "emdash");
			dictionary.Add(138, "AE");
			dictionary.Add(139, "ordfeminine");
			dictionary.Add(140, "Lslash");
			dictionary.Add(141, "Oslash");
			dictionary.Add(142, "OE");
			dictionary.Add(143, "ordmasculine");
			dictionary.Add(144, "ae");
			dictionary.Add(145, "dotlessi");
			dictionary.Add(146, "lslash");
			dictionary.Add(147, "oslash");
			dictionary.Add(148, "oe");
			dictionary.Add(149, "germandbls");
			dictionary.Add(150, "onesuperior");
			dictionary.Add(151, "logicalnot");
			dictionary.Add(152, "mu");
			dictionary.Add(153, "trademark");
			dictionary.Add(154, "Eth");
			dictionary.Add(155, "onehalf");
			dictionary.Add(156, "plusminus");
			dictionary.Add(157, "Thorn");
			dictionary.Add(158, "onequarter");
			dictionary.Add(159, "divide");
			dictionary.Add(160, "brokenbar");
			dictionary.Add(161, "degree");
			dictionary.Add(162, "thorn");
			dictionary.Add(163, "threequarters");
			dictionary.Add(164, "twosuperior");
			dictionary.Add(165, "registered");
			dictionary.Add(166, "minus");
			dictionary.Add(167, "eth");
			dictionary.Add(168, "multiply");
			dictionary.Add(169, "threesuperior");
			dictionary.Add(170, "copyright");
			dictionary.Add(171, "Aacute");
			dictionary.Add(172, "Acircumflex");
			dictionary.Add(173, "Adieresis");
			dictionary.Add(174, "Agrave");
			dictionary.Add(175, "Aring");
			dictionary.Add(176, "Atilde");
			dictionary.Add(177, "Ccedilla");
			dictionary.Add(178, "Eacute");
			dictionary.Add(179, "Ecircumflex");
			dictionary.Add(180, "Edieresis");
			dictionary.Add(181, "Egrave");
			dictionary.Add(182, "Iacute");
			dictionary.Add(183, "Icircumflex");
			dictionary.Add(184, "Idieresis");
			dictionary.Add(185, "Igrave");
			dictionary.Add(186, "Ntilde");
			dictionary.Add(187, "Oacute");
			dictionary.Add(188, "Ocircumflex");
			dictionary.Add(189, "Odieresis");
			dictionary.Add(190, "Ograve");
			dictionary.Add(191, "Otilde");
			dictionary.Add(192, "Scaron");
			dictionary.Add(193, "Uacute");
			dictionary.Add(194, "Ucircumflex");
			dictionary.Add(195, "Udieresis");
			dictionary.Add(196, "Ugrave");
			dictionary.Add(197, "Yacute");
			dictionary.Add(198, "Ydieresis");
			dictionary.Add(199, "Zcaron");
			dictionary.Add(200, "aacute");
			dictionary.Add(201, "acircumflex");
			dictionary.Add(202, "adieresis");
			dictionary.Add(203, "agrave");
			dictionary.Add(204, "aring");
			dictionary.Add(205, "atilde");
			dictionary.Add(206, "ccedilla");
			dictionary.Add(207, "eacute");
			dictionary.Add(208, "ecircumflex");
			dictionary.Add(209, "edieresis");
			dictionary.Add(210, "egrave");
			dictionary.Add(211, "iacute");
			dictionary.Add(212, "icircumflex");
			dictionary.Add(213, "idieresis");
			dictionary.Add(214, "igrave");
			dictionary.Add(215, "ntilde");
			dictionary.Add(216, "oacute");
			dictionary.Add(217, "ocircumflex");
			dictionary.Add(218, "odieresis");
			dictionary.Add(219, "ograve");
			dictionary.Add(220, "otilde");
			dictionary.Add(221, "scaron");
			dictionary.Add(222, "uacute");
			dictionary.Add(223, "ucircumflex");
			dictionary.Add(224, "udieresis");
			dictionary.Add(225, "ugrave");
			dictionary.Add(226, "yacute");
			dictionary.Add(227, "ydieresis");
			dictionary.Add(228, "zcaron");
			CompactFontFormatIsoAdobeCharset.StringIdToName = dictionary;
			CompactFontFormatIsoAdobeCharset.Value = new CompactFontFormatIsoAdobeCharset();
		}

		// Token: 0x0400037B RID: 891
		private static readonly IReadOnlyDictionary<int, string> StringIdToName;

		// Token: 0x0400037D RID: 893
		private readonly IReadOnlyDictionary<int, KeyValuePair<int, string>> glyphIdToStringIdAndName;
	}
}
