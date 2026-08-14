using System;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Fonts.Encodings
{
	// Token: 0x0200007B RID: 123
	public class MacOsRomanEncoding : MacRomanEncoding
	{
		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x0000F6BD File Offset: 0x0000D8BD
		public new static MacOsRomanEncoding Instance { get; } = new MacOsRomanEncoding();

		// Token: 0x060002D7 RID: 727 RVA: 0x0000F6C4 File Offset: 0x0000D8C4
		private MacOsRomanEncoding()
		{
			foreach (ValueTuple<int, string> valueTuple in MacOsRomanEncoding.EncodingTable)
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

		// Token: 0x040002F1 RID: 753
		private static readonly ValueTuple<int, string>[] EncodingTable = new ValueTuple<int, string>[]
		{
			new ValueTuple<int, string>(255, "notequal"),
			new ValueTuple<int, string>(260, "infinity"),
			new ValueTuple<int, string>(262, "lessequal"),
			new ValueTuple<int, string>(263, "greaterequal"),
			new ValueTuple<int, string>(266, "partialdiff"),
			new ValueTuple<int, string>(267, "summation"),
			new ValueTuple<int, string>(270, "product"),
			new ValueTuple<int, string>(271, "pi"),
			new ValueTuple<int, string>(272, "integral"),
			new ValueTuple<int, string>(275, "Omega"),
			new ValueTuple<int, string>(303, "radical"),
			new ValueTuple<int, string>(305, "approxequal"),
			new ValueTuple<int, string>(306, "Delta"),
			new ValueTuple<int, string>(327, "lozenge"),
			new ValueTuple<int, string>(333, "Euro"),
			new ValueTuple<int, string>(360, "apple")
		};
	}
}
