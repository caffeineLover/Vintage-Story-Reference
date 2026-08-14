using System;
using System.Runtime.CompilerServices;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x02000176 RID: 374
	public interface IColor
	{
		// Token: 0x17000266 RID: 614
		// (get) Token: 0x060008B2 RID: 2226
		ColorSpace ColorSpace { get; }

		// Token: 0x060008B3 RID: 2227
		[return: TupleElementNames(new string[]
		{
			"r",
			"g",
			"b"
		})]
		ValueTuple<double, double, double> ToRGBValues();
	}
}
