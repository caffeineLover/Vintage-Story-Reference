using System;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Fonts.Encodings
{
	// Token: 0x0200007A RID: 122
	internal class MacExpertEncoding : Encoding
	{
		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0000E768 File Offset: 0x0000C968
		public static MacExpertEncoding Instance { get; } = new MacExpertEncoding();

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x0000E76F File Offset: 0x0000C96F
		public override string EncodingName
		{
			get
			{
				return "MacExpertEncoding";
			}
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x0000E778 File Offset: 0x0000C978
		private MacExpertEncoding()
		{
			foreach (ValueTuple<int, string> valueTuple in MacExpertEncoding.EncodingTable)
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

		// Token: 0x040002EF RID: 751
		private static readonly ValueTuple<int, string>[] EncodingTable = new ValueTuple<int, string>[]
		{
			new ValueTuple<int, string>(276, "AEsmall"),
			new ValueTuple<int, string>(207, "Aacutesmall"),
			new ValueTuple<int, string>(211, "Acircumflexsmall"),
			new ValueTuple<int, string>(47, "Acutesmall"),
			new ValueTuple<int, string>(212, "Adieresissmall"),
			new ValueTuple<int, string>(210, "Agravesmall"),
			new ValueTuple<int, string>(214, "Aringsmall"),
			new ValueTuple<int, string>(141, "Asmall"),
			new ValueTuple<int, string>(213, "Atildesmall"),
			new ValueTuple<int, string>(363, "Brevesmall"),
			new ValueTuple<int, string>(142, "Bsmall"),
			new ValueTuple<int, string>(256, "Caronsmall"),
			new ValueTuple<int, string>(215, "Ccedillasmall"),
			new ValueTuple<int, string>(311, "Cedillasmall"),
			new ValueTuple<int, string>(136, "Circumflexsmall"),
			new ValueTuple<int, string>(143, "Csmall"),
			new ValueTuple<int, string>(254, "Dieresissmall"),
			new ValueTuple<int, string>(372, "Dotaccentsmall"),
			new ValueTuple<int, string>(144, "Dsmall"),
			new ValueTuple<int, string>(216, "Eacutesmall"),
			new ValueTuple<int, string>(220, "Ecircumflexsmall"),
			new ValueTuple<int, string>(221, "Edieresissmall"),
			new ValueTuple<int, string>(217, "Egravesmall"),
			new ValueTuple<int, string>(145, "Esmall"),
			new ValueTuple<int, string>(104, "Ethsmall"),
			new ValueTuple<int, string>(146, "Fsmall"),
			new ValueTuple<int, string>(140, "Gravesmall"),
			new ValueTuple<int, string>(147, "Gsmall"),
			new ValueTuple<int, string>(150, "Hsmall"),
			new ValueTuple<int, string>(42, "Hungarumlautsmall"),
			new ValueTuple<int, string>(222, "Iacutesmall"),
			new ValueTuple<int, string>(224, "Icircumflexsmall"),
			new ValueTuple<int, string>(225, "Idieresissmall"),
			new ValueTuple<int, string>(223, "Igravesmall"),
			new ValueTuple<int, string>(151, "Ismall"),
			new ValueTuple<int, string>(152, "Jsmall"),
			new ValueTuple<int, string>(153, "Ksmall"),
			new ValueTuple<int, string>(302, "Lslashsmall"),
			new ValueTuple<int, string>(154, "Lsmall"),
			new ValueTuple<int, string>(364, "Macronsmall"),
			new ValueTuple<int, string>(155, "Msmall"),
			new ValueTuple<int, string>(156, "Nsmall"),
			new ValueTuple<int, string>(226, "Ntildesmall"),
			new ValueTuple<int, string>(317, "OEsmall"),
			new ValueTuple<int, string>(227, "Oacutesmall"),
			new ValueTuple<int, string>(231, "Ocircumflexsmall"),
			new ValueTuple<int, string>(232, "Odieresissmall"),
			new ValueTuple<int, string>(362, "Ogoneksmall"),
			new ValueTuple<int, string>(230, "Ogravesmall"),
			new ValueTuple<int, string>(277, "Oslashsmall"),
			new ValueTuple<int, string>(157, "Osmall"),
			new ValueTuple<int, string>(233, "Otildesmall"),
			new ValueTuple<int, string>(160, "Psmall"),
			new ValueTuple<int, string>(161, "Qsmall"),
			new ValueTuple<int, string>(373, "Ringsmall"),
			new ValueTuple<int, string>(162, "Rsmall"),
			new ValueTuple<int, string>(247, "Scaronsmall"),
			new ValueTuple<int, string>(163, "Ssmall"),
			new ValueTuple<int, string>(271, "Thornsmall"),
			new ValueTuple<int, string>(176, "Tildesmall"),
			new ValueTuple<int, string>(164, "Tsmall"),
			new ValueTuple<int, string>(234, "Uacutesmall"),
			new ValueTuple<int, string>(236, "Ucircumflexsmall"),
			new ValueTuple<int, string>(237, "Udieresissmall"),
			new ValueTuple<int, string>(235, "Ugravesmall"),
			new ValueTuple<int, string>(165, "Usmall"),
			new ValueTuple<int, string>(166, "Vsmall"),
			new ValueTuple<int, string>(167, "Wsmall"),
			new ValueTuple<int, string>(170, "Xsmall"),
			new ValueTuple<int, string>(264, "Yacutesmall"),
			new ValueTuple<int, string>(330, "Ydieresissmall"),
			new ValueTuple<int, string>(171, "Ysmall"),
			new ValueTuple<int, string>(275, "Zcaronsmall"),
			new ValueTuple<int, string>(172, "Zsmall"),
			new ValueTuple<int, string>(46, "ampersandsmall"),
			new ValueTuple<int, string>(201, "asuperior"),
			new ValueTuple<int, string>(365, "bsuperior"),
			new ValueTuple<int, string>(251, "centinferior"),
			new ValueTuple<int, string>(43, "centoldstyle"),
			new ValueTuple<int, string>(202, "centsuperior"),
			new ValueTuple<int, string>(72, "colon"),
			new ValueTuple<int, string>(173, "colonmonetary"),
			new ValueTuple<int, string>(54, "comma"),
			new ValueTuple<int, string>(262, "commainferior"),
			new ValueTuple<int, string>(370, "commasuperior"),
			new ValueTuple<int, string>(266, "dollarinferior"),
			new ValueTuple<int, string>(44, "dollaroldstyle"),
			new ValueTuple<int, string>(45, "dollarsuperior"),
			new ValueTuple<int, string>(353, "dsuperior"),
			new ValueTuple<int, string>(245, "eightinferior"),
			new ValueTuple<int, string>(70, "eightoldstyle"),
			new ValueTuple<int, string>(241, "eightsuperior"),
			new ValueTuple<int, string>(344, "esuperior"),
			new ValueTuple<int, string>(326, "exclamdownsmall"),
			new ValueTuple<int, string>(41, "exclamsmall"),
			new ValueTuple<int, string>(126, "ff"),
			new ValueTuple<int, string>(131, "ffi"),
			new ValueTuple<int, string>(132, "ffl"),
			new ValueTuple<int, string>(127, "fi"),
			new ValueTuple<int, string>(320, "figuredash"),
			new ValueTuple<int, string>(114, "fiveeighths"),
			new ValueTuple<int, string>(260, "fiveinferior"),
			new ValueTuple<int, string>(65, "fiveoldstyle"),
			new ValueTuple<int, string>(336, "fivesuperior"),
			new ValueTuple<int, string>(130, "fl"),
			new ValueTuple<int, string>(242, "fourinferior"),
			new ValueTuple<int, string>(64, "fouroldstyle"),
			new ValueTuple<int, string>(335, "foursuperior"),
			new ValueTuple<int, string>(57, "fraction"),
			new ValueTuple<int, string>(55, "hyphen"),
			new ValueTuple<int, string>(137, "hypheninferior"),
			new ValueTuple<int, string>(321, "hyphensuperior"),
			new ValueTuple<int, string>(351, "isuperior"),
			new ValueTuple<int, string>(361, "lsuperior"),
			new ValueTuple<int, string>(367, "msuperior"),
			new ValueTuple<int, string>(273, "nineinferior"),
			new ValueTuple<int, string>(71, "nineoldstyle"),
			new ValueTuple<int, string>(341, "ninesuperior"),
			new ValueTuple<int, string>(366, "nsuperior"),
			new ValueTuple<int, string>(53, "onedotenleader"),
			new ValueTuple<int, string>(112, "oneeighth"),
			new ValueTuple<int, string>(174, "onefitted"),
			new ValueTuple<int, string>(110, "onehalf"),
			new ValueTuple<int, string>(301, "oneinferior"),
			new ValueTuple<int, string>(61, "oneoldstyle"),
			new ValueTuple<int, string>(107, "onequarter"),
			new ValueTuple<int, string>(332, "onesuperior"),
			new ValueTuple<int, string>(116, "onethird"),
			new ValueTuple<int, string>(257, "osuperior"),
			new ValueTuple<int, string>(133, "parenleftinferior"),
			new ValueTuple<int, string>(50, "parenleftsuperior"),
			new ValueTuple<int, string>(135, "parenrightinferior"),
			new ValueTuple<int, string>(51, "parenrightsuperior"),
			new ValueTuple<int, string>(56, "period"),
			new ValueTuple<int, string>(263, "periodinferior"),
			new ValueTuple<int, string>(371, "periodsuperior"),
			new ValueTuple<int, string>(300, "questiondownsmall"),
			new ValueTuple<int, string>(77, "questionsmall"),
			new ValueTuple<int, string>(345, "rsuperior"),
			new ValueTuple<int, string>(175, "rupiah"),
			new ValueTuple<int, string>(73, "semicolon"),
			new ValueTuple<int, string>(115, "seveneighths"),
			new ValueTuple<int, string>(246, "seveninferior"),
			new ValueTuple<int, string>(67, "sevenoldstyle"),
			new ValueTuple<int, string>(340, "sevensuperior"),
			new ValueTuple<int, string>(244, "sixinferior"),
			new ValueTuple<int, string>(66, "sixoldstyle"),
			new ValueTuple<int, string>(337, "sixsuperior"),
			new ValueTuple<int, string>(40, "space"),
			new ValueTuple<int, string>(352, "ssuperior"),
			new ValueTuple<int, string>(113, "threeeighths"),
			new ValueTuple<int, string>(243, "threeinferior"),
			new ValueTuple<int, string>(63, "threeoldstyle"),
			new ValueTuple<int, string>(111, "threequarters"),
			new ValueTuple<int, string>(75, "threequartersemdash"),
			new ValueTuple<int, string>(334, "threesuperior"),
			new ValueTuple<int, string>(346, "tsuperior"),
			new ValueTuple<int, string>(52, "twodotenleader"),
			new ValueTuple<int, string>(252, "twoinferior"),
			new ValueTuple<int, string>(62, "twooldstyle"),
			new ValueTuple<int, string>(333, "twosuperior"),
			new ValueTuple<int, string>(117, "twothirds"),
			new ValueTuple<int, string>(274, "zeroinferior"),
			new ValueTuple<int, string>(60, "zerooldstyle"),
			new ValueTuple<int, string>(342, "zerosuperior")
		};
	}
}
