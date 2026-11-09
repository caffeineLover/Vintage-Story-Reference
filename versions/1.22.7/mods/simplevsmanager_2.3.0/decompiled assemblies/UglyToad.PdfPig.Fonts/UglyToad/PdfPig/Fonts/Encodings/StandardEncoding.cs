using System;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Fonts.Encodings
{
	// Token: 0x0200007D RID: 125
	public class StandardEncoding : Encoding
	{
		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060002DD RID: 733 RVA: 0x00010C39 File Offset: 0x0000EE39
		public static StandardEncoding Instance { get; } = new StandardEncoding();

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060002DE RID: 734 RVA: 0x00010C40 File Offset: 0x0000EE40
		public override string EncodingName
		{
			get
			{
				return "StandardEncoding";
			}
		}

		// Token: 0x060002DF RID: 735 RVA: 0x00010C48 File Offset: 0x0000EE48
		private StandardEncoding()
		{
			foreach (ValueTuple<int, string> valueTuple in StandardEncoding.EncodingTable)
			{
				int item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(0, 1);
				defaultInterpolatedStringHandler.AppendFormatted<int>(item);
				int code = Convert.ToInt32(defaultInterpolatedStringHandler.ToStringAndClear(), 8);
				base.Add(code, item2);
			}
		}

		// Token: 0x040002F5 RID: 757
		private static readonly ValueTuple<int, string>[] EncodingTable = new ValueTuple<int, string>[]
		{
			new ValueTuple<int, string>(101, "A"),
			new ValueTuple<int, string>(341, "AE"),
			new ValueTuple<int, string>(102, "B"),
			new ValueTuple<int, string>(103, "C"),
			new ValueTuple<int, string>(104, "D"),
			new ValueTuple<int, string>(105, "E"),
			new ValueTuple<int, string>(106, "F"),
			new ValueTuple<int, string>(107, "G"),
			new ValueTuple<int, string>(110, "H"),
			new ValueTuple<int, string>(111, "I"),
			new ValueTuple<int, string>(112, "J"),
			new ValueTuple<int, string>(113, "K"),
			new ValueTuple<int, string>(114, "L"),
			new ValueTuple<int, string>(350, "Lslash"),
			new ValueTuple<int, string>(115, "M"),
			new ValueTuple<int, string>(116, "N"),
			new ValueTuple<int, string>(117, "O"),
			new ValueTuple<int, string>(352, "OE"),
			new ValueTuple<int, string>(351, "Oslash"),
			new ValueTuple<int, string>(120, "P"),
			new ValueTuple<int, string>(121, "Q"),
			new ValueTuple<int, string>(122, "R"),
			new ValueTuple<int, string>(123, "S"),
			new ValueTuple<int, string>(124, "T"),
			new ValueTuple<int, string>(125, "U"),
			new ValueTuple<int, string>(126, "V"),
			new ValueTuple<int, string>(127, "W"),
			new ValueTuple<int, string>(130, "X"),
			new ValueTuple<int, string>(131, "Y"),
			new ValueTuple<int, string>(132, "Z"),
			new ValueTuple<int, string>(141, "a"),
			new ValueTuple<int, string>(302, "acute"),
			new ValueTuple<int, string>(361, "ae"),
			new ValueTuple<int, string>(46, "ampersand"),
			new ValueTuple<int, string>(136, "asciicircum"),
			new ValueTuple<int, string>(176, "asciitilde"),
			new ValueTuple<int, string>(52, "asterisk"),
			new ValueTuple<int, string>(100, "at"),
			new ValueTuple<int, string>(142, "b"),
			new ValueTuple<int, string>(134, "backslash"),
			new ValueTuple<int, string>(174, "bar"),
			new ValueTuple<int, string>(173, "braceleft"),
			new ValueTuple<int, string>(175, "braceright"),
			new ValueTuple<int, string>(133, "bracketleft"),
			new ValueTuple<int, string>(135, "bracketright"),
			new ValueTuple<int, string>(306, "breve"),
			new ValueTuple<int, string>(267, "bullet"),
			new ValueTuple<int, string>(143, "c"),
			new ValueTuple<int, string>(317, "caron"),
			new ValueTuple<int, string>(313, "cedilla"),
			new ValueTuple<int, string>(242, "cent"),
			new ValueTuple<int, string>(303, "circumflex"),
			new ValueTuple<int, string>(72, "colon"),
			new ValueTuple<int, string>(54, "comma"),
			new ValueTuple<int, string>(250, "currency"),
			new ValueTuple<int, string>(144, "d"),
			new ValueTuple<int, string>(262, "dagger"),
			new ValueTuple<int, string>(263, "daggerdbl"),
			new ValueTuple<int, string>(310, "dieresis"),
			new ValueTuple<int, string>(44, "dollar"),
			new ValueTuple<int, string>(307, "dotaccent"),
			new ValueTuple<int, string>(365, "dotlessi"),
			new ValueTuple<int, string>(145, "e"),
			new ValueTuple<int, string>(70, "eight"),
			new ValueTuple<int, string>(274, "ellipsis"),
			new ValueTuple<int, string>(320, "emdash"),
			new ValueTuple<int, string>(261, "endash"),
			new ValueTuple<int, string>(75, "equal"),
			new ValueTuple<int, string>(41, "exclam"),
			new ValueTuple<int, string>(241, "exclamdown"),
			new ValueTuple<int, string>(146, "f"),
			new ValueTuple<int, string>(256, "fi"),
			new ValueTuple<int, string>(65, "five"),
			new ValueTuple<int, string>(257, "fl"),
			new ValueTuple<int, string>(246, "florin"),
			new ValueTuple<int, string>(64, "four"),
			new ValueTuple<int, string>(244, "fraction"),
			new ValueTuple<int, string>(147, "g"),
			new ValueTuple<int, string>(373, "germandbls"),
			new ValueTuple<int, string>(301, "grave"),
			new ValueTuple<int, string>(76, "greater"),
			new ValueTuple<int, string>(253, "guillemotleft"),
			new ValueTuple<int, string>(273, "guillemotright"),
			new ValueTuple<int, string>(254, "guilsinglleft"),
			new ValueTuple<int, string>(255, "guilsinglright"),
			new ValueTuple<int, string>(150, "h"),
			new ValueTuple<int, string>(315, "hungarumlaut"),
			new ValueTuple<int, string>(55, "hyphen"),
			new ValueTuple<int, string>(151, "i"),
			new ValueTuple<int, string>(152, "j"),
			new ValueTuple<int, string>(153, "k"),
			new ValueTuple<int, string>(154, "l"),
			new ValueTuple<int, string>(74, "less"),
			new ValueTuple<int, string>(370, "lslash"),
			new ValueTuple<int, string>(155, "m"),
			new ValueTuple<int, string>(305, "macron"),
			new ValueTuple<int, string>(156, "n"),
			new ValueTuple<int, string>(71, "nine"),
			new ValueTuple<int, string>(43, "numbersign"),
			new ValueTuple<int, string>(157, "o"),
			new ValueTuple<int, string>(372, "oe"),
			new ValueTuple<int, string>(316, "ogonek"),
			new ValueTuple<int, string>(61, "one"),
			new ValueTuple<int, string>(343, "ordfeminine"),
			new ValueTuple<int, string>(353, "ordmasculine"),
			new ValueTuple<int, string>(371, "oslash"),
			new ValueTuple<int, string>(160, "p"),
			new ValueTuple<int, string>(266, "paragraph"),
			new ValueTuple<int, string>(50, "parenleft"),
			new ValueTuple<int, string>(51, "parenright"),
			new ValueTuple<int, string>(45, "percent"),
			new ValueTuple<int, string>(56, "period"),
			new ValueTuple<int, string>(264, "periodcentered"),
			new ValueTuple<int, string>(275, "perthousand"),
			new ValueTuple<int, string>(53, "plus"),
			new ValueTuple<int, string>(161, "q"),
			new ValueTuple<int, string>(77, "question"),
			new ValueTuple<int, string>(277, "questiondown"),
			new ValueTuple<int, string>(42, "quotedbl"),
			new ValueTuple<int, string>(271, "quotedblbase"),
			new ValueTuple<int, string>(252, "quotedblleft"),
			new ValueTuple<int, string>(272, "quotedblright"),
			new ValueTuple<int, string>(140, "quoteleft"),
			new ValueTuple<int, string>(47, "quoteright"),
			new ValueTuple<int, string>(270, "quotesinglbase"),
			new ValueTuple<int, string>(251, "quotesingle"),
			new ValueTuple<int, string>(162, "r"),
			new ValueTuple<int, string>(312, "ring"),
			new ValueTuple<int, string>(163, "s"),
			new ValueTuple<int, string>(247, "section"),
			new ValueTuple<int, string>(73, "semicolon"),
			new ValueTuple<int, string>(67, "seven"),
			new ValueTuple<int, string>(66, "six"),
			new ValueTuple<int, string>(57, "slash"),
			new ValueTuple<int, string>(40, "space"),
			new ValueTuple<int, string>(243, "sterling"),
			new ValueTuple<int, string>(164, "t"),
			new ValueTuple<int, string>(63, "three"),
			new ValueTuple<int, string>(304, "tilde"),
			new ValueTuple<int, string>(62, "two"),
			new ValueTuple<int, string>(165, "u"),
			new ValueTuple<int, string>(137, "underscore"),
			new ValueTuple<int, string>(166, "v"),
			new ValueTuple<int, string>(167, "w"),
			new ValueTuple<int, string>(170, "x"),
			new ValueTuple<int, string>(171, "y"),
			new ValueTuple<int, string>(245, "yen"),
			new ValueTuple<int, string>(172, "z"),
			new ValueTuple<int, string>(60, "zero")
		};
	}
}
