using System;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x02000177 RID: 375
	public abstract class PatternColor : IColor
	{
		// Token: 0x17000267 RID: 615
		// (get) Token: 0x060008B4 RID: 2228 RVA: 0x000281B6 File Offset: 0x000263B6
		public PatternType PatternType { get; }

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x060008B5 RID: 2229 RVA: 0x000281BE File Offset: 0x000263BE
		public DictionaryToken PatternDictionary { get; }

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x060008B6 RID: 2230 RVA: 0x000281C6 File Offset: 0x000263C6
		public DictionaryToken ExtGState { get; }

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x060008B7 RID: 2231 RVA: 0x000281CE File Offset: 0x000263CE
		public TransformationMatrix Matrix { get; }

		// Token: 0x060008B8 RID: 2232 RVA: 0x000281D6 File Offset: 0x000263D6
		protected internal PatternColor(PatternType patternType, DictionaryToken patternDictionary, DictionaryToken extGState, TransformationMatrix matrix)
		{
			this.PatternType = patternType;
			this.PatternDictionary = patternDictionary;
			this.ExtGState = extGState;
			this.Matrix = matrix;
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x060008B9 RID: 2233 RVA: 0x00028202 File Offset: 0x00026402
		public ColorSpace ColorSpace { get; } = 8;

		// Token: 0x060008BA RID: 2234 RVA: 0x0002820A File Offset: 0x0002640A
		[return: TupleElementNames(new string[]
		{
			"r",
			"g",
			"b"
		})]
		public ValueTuple<double, double, double> ToRGBValues()
		{
			throw new InvalidOperationException("Cannot call ToRGBValues in a Pattern color.");
		}

		// Token: 0x060008BB RID: 2235 RVA: 0x00028218 File Offset: 0x00026418
		public override string ToString()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(11, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Pattern: (");
			defaultInterpolatedStringHandler.AppendFormatted<PatternType>(this.PatternType);
			defaultInterpolatedStringHandler.AppendLiteral(")");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
