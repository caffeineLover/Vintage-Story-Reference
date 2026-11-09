using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UglyToad.PdfPig.Core
{
	// Token: 0x0200000A RID: 10
	public static class OtherEncodings
	{
		// Token: 0x06000023 RID: 35 RVA: 0x000024C9 File Offset: 0x000006C9
		public static byte[] StringAsLatin1Bytes(string s)
		{
			if (s == null)
			{
				return null;
			}
			return OtherEncodings.Iso88591.GetBytes(s);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000024DC File Offset: 0x000006DC
		public static string BytesAsLatin1String(IReadOnlyList<byte> bytes)
		{
			if (bytes == null)
			{
				return null;
			}
			byte[] array = bytes as byte[];
			if (array != null)
			{
				return OtherEncodings.BytesAsLatin1String(array);
			}
			return OtherEncodings.BytesAsLatin1String(Enumerable.ToArray<byte>(bytes));
		}

		// Token: 0x06000025 RID: 37 RVA: 0x0000250A File Offset: 0x0000070A
		public static string BytesAsLatin1String(byte[] bytes)
		{
			if (bytes == null)
			{
				return null;
			}
			return OtherEncodings.Iso88591.GetString(bytes);
		}

		// Token: 0x0400000C RID: 12
		public static Encoding Iso88591 = Encoding.GetEncoding("ISO-8859-1");

		// Token: 0x02000017 RID: 23
		public static class PdfDocEncoding
		{
			// Token: 0x060000B3 RID: 179 RVA: 0x00005190 File Offset: 0x00003390
			public static bool ContainsChar(char c)
			{
				return Enumerable.Any<KeyValuePair<byte, char>>(OtherEncodings.PdfDocEncoding.CodeToUnicode, (KeyValuePair<byte, char> x) => x.Value == c);
			}

			// Token: 0x060000B4 RID: 180 RVA: 0x000051C0 File Offset: 0x000033C0
			// Note: this type is marked as 'beforefieldinit'.
			static PdfDocEncoding()
			{
				Dictionary<byte, char> dictionary = new Dictionary<byte, char>();
				dictionary.Add(0, '\0');
				dictionary.Add(1, '\u0001');
				dictionary.Add(2, '\u0002');
				dictionary.Add(3, '\u0003');
				dictionary.Add(4, '\u0004');
				dictionary.Add(5, '\u0005');
				dictionary.Add(6, '\u0006');
				dictionary.Add(7, '\a');
				dictionary.Add(8, '\b');
				dictionary.Add(9, '\t');
				dictionary.Add(10, '\n');
				dictionary.Add(11, '\v');
				dictionary.Add(12, '\f');
				dictionary.Add(13, '\r');
				dictionary.Add(14, '\u000e');
				dictionary.Add(15, '\u000f');
				dictionary.Add(16, '\u0010');
				dictionary.Add(17, '\u0011');
				dictionary.Add(18, '\u0012');
				dictionary.Add(19, '\u0013');
				dictionary.Add(20, '\u0014');
				dictionary.Add(21, '\u0015');
				dictionary.Add(22, '\u0017');
				dictionary.Add(23, '\u0017');
				dictionary.Add(24, '˘');
				dictionary.Add(25, 'ˇ');
				dictionary.Add(26, 'ˆ');
				dictionary.Add(27, '˙');
				dictionary.Add(28, '˝');
				dictionary.Add(29, '˛');
				dictionary.Add(30, '˚');
				dictionary.Add(31, '˜');
				dictionary.Add(32, ' ');
				dictionary.Add(33, '!');
				dictionary.Add(34, '"');
				dictionary.Add(35, '#');
				dictionary.Add(36, '$');
				dictionary.Add(37, '%');
				dictionary.Add(38, '&');
				dictionary.Add(39, '\'');
				dictionary.Add(40, '(');
				dictionary.Add(41, ')');
				dictionary.Add(42, '*');
				dictionary.Add(43, '+');
				dictionary.Add(44, ',');
				dictionary.Add(45, '-');
				dictionary.Add(46, '.');
				dictionary.Add(47, '/');
				dictionary.Add(48, '0');
				dictionary.Add(49, '1');
				dictionary.Add(50, '2');
				dictionary.Add(51, '3');
				dictionary.Add(52, '4');
				dictionary.Add(53, '5');
				dictionary.Add(54, '6');
				dictionary.Add(55, '7');
				dictionary.Add(56, '8');
				dictionary.Add(57, '9');
				dictionary.Add(58, ':');
				dictionary.Add(59, ';');
				dictionary.Add(60, '<');
				dictionary.Add(61, '=');
				dictionary.Add(62, '>');
				dictionary.Add(63, '?');
				dictionary.Add(64, '@');
				dictionary.Add(65, 'A');
				dictionary.Add(66, 'B');
				dictionary.Add(67, 'C');
				dictionary.Add(68, 'D');
				dictionary.Add(69, 'E');
				dictionary.Add(70, 'F');
				dictionary.Add(71, 'G');
				dictionary.Add(72, 'H');
				dictionary.Add(73, 'I');
				dictionary.Add(74, 'J');
				dictionary.Add(75, 'K');
				dictionary.Add(76, 'L');
				dictionary.Add(77, 'M');
				dictionary.Add(78, 'N');
				dictionary.Add(79, 'O');
				dictionary.Add(80, 'P');
				dictionary.Add(81, 'Q');
				dictionary.Add(82, 'R');
				dictionary.Add(83, 'S');
				dictionary.Add(84, 'T');
				dictionary.Add(85, 'U');
				dictionary.Add(86, 'V');
				dictionary.Add(87, 'W');
				dictionary.Add(88, 'X');
				dictionary.Add(89, 'Y');
				dictionary.Add(90, 'Z');
				dictionary.Add(91, '[');
				dictionary.Add(92, '\\');
				dictionary.Add(93, ']');
				dictionary.Add(94, '^');
				dictionary.Add(95, '_');
				dictionary.Add(96, '`');
				dictionary.Add(97, 'a');
				dictionary.Add(98, 'b');
				dictionary.Add(99, 'c');
				dictionary.Add(100, 'd');
				dictionary.Add(101, 'e');
				dictionary.Add(102, 'f');
				dictionary.Add(103, 'g');
				dictionary.Add(104, 'h');
				dictionary.Add(105, 'i');
				dictionary.Add(106, 'j');
				dictionary.Add(107, 'k');
				dictionary.Add(108, 'l');
				dictionary.Add(109, 'm');
				dictionary.Add(110, 'n');
				dictionary.Add(111, 'o');
				dictionary.Add(112, 'p');
				dictionary.Add(113, 'q');
				dictionary.Add(114, 'r');
				dictionary.Add(115, 's');
				dictionary.Add(116, 't');
				dictionary.Add(117, 'u');
				dictionary.Add(118, 'v');
				dictionary.Add(119, 'w');
				dictionary.Add(120, 'x');
				dictionary.Add(121, 'y');
				dictionary.Add(122, 'z');
				dictionary.Add(123, '{');
				dictionary.Add(124, '|');
				dictionary.Add(125, '}');
				dictionary.Add(126, '~');
				dictionary.Add(128, '•');
				dictionary.Add(129, '†');
				dictionary.Add(130, '‡');
				dictionary.Add(131, '…');
				dictionary.Add(132, '—');
				dictionary.Add(133, '–');
				dictionary.Add(134, 'ƒ');
				dictionary.Add(135, '⁄');
				dictionary.Add(136, '‹');
				dictionary.Add(137, '›');
				dictionary.Add(138, '−');
				dictionary.Add(139, '‰');
				dictionary.Add(140, '„');
				dictionary.Add(141, '“');
				dictionary.Add(142, '”');
				dictionary.Add(143, '‘');
				dictionary.Add(144, '’');
				dictionary.Add(145, '‚');
				dictionary.Add(146, '™');
				dictionary.Add(147, 'ﬁ');
				dictionary.Add(148, 'ﬂ');
				dictionary.Add(149, 'Ł');
				dictionary.Add(150, 'Œ');
				dictionary.Add(151, 'Š');
				dictionary.Add(152, 'Ÿ');
				dictionary.Add(153, 'Ž');
				dictionary.Add(154, 'ı');
				dictionary.Add(155, 'ł');
				dictionary.Add(156, 'œ');
				dictionary.Add(157, 'š');
				dictionary.Add(158, 'ž');
				dictionary.Add(160, '€');
				dictionary.Add(161, '¡');
				dictionary.Add(162, '¢');
				dictionary.Add(163, '£');
				dictionary.Add(164, '¤');
				dictionary.Add(165, '¥');
				dictionary.Add(166, '¦');
				dictionary.Add(167, '§');
				dictionary.Add(168, '¨');
				dictionary.Add(169, '©');
				dictionary.Add(170, 'ª');
				dictionary.Add(171, '«');
				dictionary.Add(172, '¬');
				dictionary.Add(174, '®');
				dictionary.Add(175, '¯');
				dictionary.Add(176, '°');
				dictionary.Add(177, '±');
				dictionary.Add(178, '²');
				dictionary.Add(179, '³');
				dictionary.Add(180, '´');
				dictionary.Add(181, 'µ');
				dictionary.Add(182, '¶');
				dictionary.Add(183, '·');
				dictionary.Add(184, '¸');
				dictionary.Add(185, '¹');
				dictionary.Add(186, 'º');
				dictionary.Add(187, '»');
				dictionary.Add(188, '¼');
				dictionary.Add(189, '½');
				dictionary.Add(190, '¾');
				dictionary.Add(191, '¿');
				dictionary.Add(192, 'À');
				dictionary.Add(193, 'Á');
				dictionary.Add(194, 'Â');
				dictionary.Add(195, 'Ã');
				dictionary.Add(196, 'Ä');
				dictionary.Add(197, 'Å');
				dictionary.Add(198, 'Æ');
				dictionary.Add(199, 'Ç');
				dictionary.Add(200, 'È');
				dictionary.Add(201, 'É');
				dictionary.Add(202, 'Ê');
				dictionary.Add(203, 'Ë');
				dictionary.Add(204, 'Ì');
				dictionary.Add(205, 'Í');
				dictionary.Add(206, 'Î');
				dictionary.Add(207, 'Ï');
				dictionary.Add(208, 'Ð');
				dictionary.Add(209, 'Ñ');
				dictionary.Add(210, 'Ò');
				dictionary.Add(211, 'Ó');
				dictionary.Add(212, 'Ô');
				dictionary.Add(213, 'Õ');
				dictionary.Add(214, 'Ö');
				dictionary.Add(215, '×');
				dictionary.Add(216, 'Ø');
				dictionary.Add(217, 'Ù');
				dictionary.Add(218, 'Ú');
				dictionary.Add(219, 'Û');
				dictionary.Add(220, 'Ü');
				dictionary.Add(221, 'Ý');
				dictionary.Add(222, 'Þ');
				dictionary.Add(223, 'ß');
				dictionary.Add(224, 'à');
				dictionary.Add(225, 'á');
				dictionary.Add(226, 'â');
				dictionary.Add(227, 'ã');
				dictionary.Add(228, 'ä');
				dictionary.Add(229, 'å');
				dictionary.Add(230, 'æ');
				dictionary.Add(231, 'ç');
				dictionary.Add(232, 'è');
				dictionary.Add(233, 'é');
				dictionary.Add(234, 'ê');
				dictionary.Add(235, 'ë');
				dictionary.Add(236, 'ì');
				dictionary.Add(237, 'í');
				dictionary.Add(238, 'î');
				dictionary.Add(239, 'ï');
				dictionary.Add(240, 'ð');
				dictionary.Add(241, 'ñ');
				dictionary.Add(242, 'ò');
				dictionary.Add(243, 'ó');
				dictionary.Add(244, 'ô');
				dictionary.Add(245, 'õ');
				dictionary.Add(246, 'ö');
				dictionary.Add(247, '÷');
				dictionary.Add(248, 'ø');
				dictionary.Add(249, 'ù');
				dictionary.Add(250, 'ú');
				dictionary.Add(251, 'û');
				dictionary.Add(252, 'ü');
				dictionary.Add(253, 'ý');
				dictionary.Add(254, 'þ');
				dictionary.Add(byte.MaxValue, 'ÿ');
				dictionary.Add(127, '�');
				dictionary.Add(159, '�');
				dictionary.Add(173, '�');
				OtherEncodings.PdfDocEncoding.CodeToUnicode = dictionary;
			}

			// Token: 0x0400003B RID: 59
			private static readonly IReadOnlyDictionary<byte, char> CodeToUnicode;
		}
	}
}
