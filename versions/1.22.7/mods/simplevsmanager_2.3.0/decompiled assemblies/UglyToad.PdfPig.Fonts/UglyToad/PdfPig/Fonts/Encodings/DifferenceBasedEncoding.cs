using System;
using System.Collections.Generic;
using System.Linq;

namespace UglyToad.PdfPig.Fonts.Encodings
{
	// Token: 0x02000078 RID: 120
	public class DifferenceBasedEncoding : Encoding
	{
		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060002C6 RID: 710 RVA: 0x0000E520 File Offset: 0x0000C720
		public override string EncodingName { get; } = "Difference Encoding";

		// Token: 0x060002C7 RID: 711 RVA: 0x0000E528 File Offset: 0x0000C728
		public DifferenceBasedEncoding(Encoding baseEncoding, IReadOnlyList<ValueTuple<int, string>> differences)
		{
			if (baseEncoding == null)
			{
				throw new ArgumentNullException("baseEncoding");
			}
			if (differences == null)
			{
				throw new ArgumentNullException("differences");
			}
			this.EncodingName = "Difference " + baseEncoding.EncodingName;
			foreach (ValueTuple<int, string> valueTuple in differences)
			{
				base.Add(valueTuple.Item1, valueTuple.Item2);
			}
			using (IEnumerator<KeyValuePair<int, string>> enumerator2 = baseEncoding.CodeToNameMap.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<int, string> pair = enumerator2.Current;
					if (Enumerable.All<ValueTuple<int, string>>(differences, (ValueTuple<int, string> x) => x.Item1 != pair.Key))
					{
						base.Add(pair.Key, pair.Value);
					}
				}
			}
		}
	}
}
