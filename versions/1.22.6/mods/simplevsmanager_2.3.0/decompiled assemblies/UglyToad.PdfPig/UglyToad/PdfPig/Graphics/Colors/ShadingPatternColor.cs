using System;
using System.Collections.Generic;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x02000179 RID: 377
	public sealed class ShadingPatternColor : PatternColor, IEquatable<ShadingPatternColor>
	{
		// Token: 0x17000274 RID: 628
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x00028524 File Offset: 0x00026724
		public Shading Shading { get; }

		// Token: 0x060008CC RID: 2252 RVA: 0x0002852C File Offset: 0x0002672C
		public ShadingPatternColor(TransformationMatrix matrix, DictionaryToken extGState, DictionaryToken patternDictionary, Shading shading) : base(PatternType.Shading, patternDictionary, extGState, matrix)
		{
			this.Shading = shading;
		}

		// Token: 0x060008CD RID: 2253 RVA: 0x00028540 File Offset: 0x00026740
		public override bool Equals(object obj)
		{
			ShadingPatternColor shadingPatternColor = obj as ShadingPatternColor;
			return shadingPatternColor != null && this.Equals(shadingPatternColor);
		}

		// Token: 0x060008CE RID: 2254 RVA: 0x00028560 File Offset: 0x00026760
		public bool Equals(ShadingPatternColor other)
		{
			return base.PatternType.Equals(other.PatternType) && base.Matrix.Equals(other.Matrix) && base.ExtGState.Equals(other.ExtGState) && this.Shading.Equals(other.Shading);
		}

		// Token: 0x060008CF RID: 2255 RVA: 0x000285D8 File Offset: 0x000267D8
		public override int GetHashCode()
		{
			return new ValueTuple<PatternType, TransformationMatrix, DictionaryToken, Shading>(base.PatternType, base.Matrix, base.ExtGState, this.Shading).GetHashCode();
		}

		// Token: 0x060008D0 RID: 2256 RVA: 0x00028610 File Offset: 0x00026810
		public static bool operator ==(ShadingPatternColor color1, ShadingPatternColor color2)
		{
			return EqualityComparer<ShadingPatternColor>.Default.Equals(color1, color2);
		}

		// Token: 0x060008D1 RID: 2257 RVA: 0x0002861E File Offset: 0x0002681E
		public static bool operator !=(ShadingPatternColor color1, ShadingPatternColor color2)
		{
			return !(color1 == color2);
		}
	}
}
