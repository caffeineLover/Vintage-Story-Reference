using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x02000178 RID: 376
	public sealed class TilingPatternColor : PatternColor, IEquatable<TilingPatternColor>
	{
		// Token: 0x1700026C RID: 620
		// (get) Token: 0x060008BC RID: 2236 RVA: 0x0002825B File Offset: 0x0002645B
		public StreamToken PatternStream { get; }

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x060008BD RID: 2237 RVA: 0x00028263 File Offset: 0x00026463
		public PatternPaintType PaintType { get; }

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x060008BE RID: 2238 RVA: 0x0002826B File Offset: 0x0002646B
		public PatternTilingType TilingType { get; }

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x060008BF RID: 2239 RVA: 0x00028273 File Offset: 0x00026473
		public PdfRectangle BBox { get; }

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x0002827B File Offset: 0x0002647B
		public double XStep { get; }

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x060008C1 RID: 2241 RVA: 0x00028283 File Offset: 0x00026483
		public double YStep { get; }

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x060008C2 RID: 2242 RVA: 0x0002828B File Offset: 0x0002648B
		public DictionaryToken Resources { get; }

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x060008C3 RID: 2243 RVA: 0x00028293 File Offset: 0x00026493
		public IReadOnlyList<byte> Data { get; }

		// Token: 0x060008C4 RID: 2244 RVA: 0x0002829C File Offset: 0x0002649C
		public TilingPatternColor(TransformationMatrix matrix, DictionaryToken extGState, StreamToken patternStream, PatternPaintType paintType, PatternTilingType tilingType, PdfRectangle bbox, double xStep, double yStep, DictionaryToken resources, IReadOnlyList<byte> data) : base(PatternType.Tiling, patternStream.StreamDictionary, extGState, matrix)
		{
			this.PatternStream = patternStream;
			this.PaintType = paintType;
			this.TilingType = tilingType;
			this.BBox = bbox;
			this.XStep = xStep;
			this.YStep = yStep;
			this.Resources = resources;
			this.Data = data;
		}

		// Token: 0x060008C5 RID: 2245 RVA: 0x000282F8 File Offset: 0x000264F8
		public override bool Equals(object obj)
		{
			TilingPatternColor tilingPatternColor = obj as TilingPatternColor;
			return tilingPatternColor != null && this.Equals(tilingPatternColor);
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x00028318 File Offset: 0x00026518
		public bool Equals(TilingPatternColor other)
		{
			return base.PatternType.Equals(other.PatternType) && base.Matrix.Equals(other.Matrix) && base.ExtGState.Equals(other.ExtGState) && this.PaintType.Equals(other.PaintType) && this.TilingType.Equals(other.TilingType) && this.BBox.Equals(other.BBox) && this.XStep.Equals(other.XStep) && this.YStep.Equals(other.YStep) && this.Resources.Equals(other.Resources) && Enumerable.SequenceEqual<byte>(this.Data, other.Data);
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x00028440 File Offset: 0x00026640
		public override int GetHashCode()
		{
			return new ValueTuple<PatternType, TransformationMatrix, DictionaryToken, PatternPaintType, PatternTilingType, PdfRectangle, double, ValueTuple<double, DictionaryToken, IReadOnlyList<byte>>>(base.PatternType, base.Matrix, base.ExtGState, this.PaintType, this.TilingType, this.BBox, this.XStep, new ValueTuple<double, DictionaryToken, IReadOnlyList<byte>>(this.YStep, this.Resources, this.Data)).GetHashCode();
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x000284A4 File Offset: 0x000266A4
		public override string ToString()
		{
			string text = base.ToString();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(4, 2);
			defaultInterpolatedStringHandler.AppendLiteral("[");
			defaultInterpolatedStringHandler.AppendFormatted<PatternPaintType>(this.PaintType);
			defaultInterpolatedStringHandler.AppendLiteral("][");
			defaultInterpolatedStringHandler.AppendFormatted<PatternTilingType>(this.TilingType);
			defaultInterpolatedStringHandler.AppendLiteral("]");
			return text + defaultInterpolatedStringHandler.ToStringAndClear();
		}

		// Token: 0x060008C9 RID: 2249 RVA: 0x0002850A File Offset: 0x0002670A
		public static bool operator ==(TilingPatternColor color1, TilingPatternColor color2)
		{
			return EqualityComparer<TilingPatternColor>.Default.Equals(color1, color2);
		}

		// Token: 0x060008CA RID: 2250 RVA: 0x00028518 File Offset: 0x00026718
		public static bool operator !=(TilingPatternColor color1, TilingPatternColor color2)
		{
			return !(color1 == color2);
		}
	}
}
