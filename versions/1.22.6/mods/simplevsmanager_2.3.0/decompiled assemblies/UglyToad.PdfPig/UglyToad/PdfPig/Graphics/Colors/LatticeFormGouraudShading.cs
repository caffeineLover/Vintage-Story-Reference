using System;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Functions;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x02000185 RID: 389
	public sealed class LatticeFormGouraudShading : Shading
	{
		// Token: 0x17000297 RID: 663
		// (get) Token: 0x0600090E RID: 2318 RVA: 0x00029493 File Offset: 0x00027693
		public int BitsPerCoordinate { get; }

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x0600090F RID: 2319 RVA: 0x0002949B File Offset: 0x0002769B
		public int BitsPerComponent { get; }

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000910 RID: 2320 RVA: 0x000294A3 File Offset: 0x000276A3
		public int VerticesPerRow { get; }

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000911 RID: 2321 RVA: 0x000294AB File Offset: 0x000276AB
		public double[] Decode { get; }

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000912 RID: 2322 RVA: 0x000294B3 File Offset: 0x000276B3
		public override PdfFunction[] Functions { get; }

		// Token: 0x06000913 RID: 2323 RVA: 0x000294BB File Offset: 0x000276BB
		public LatticeFormGouraudShading(bool antiAlias, StreamToken shadingStream, ColorSpaceDetails colorSpace, PdfRectangle? bbox, double[] background, int bitsPerCoordinate, int bitsPerComponent, int verticesPerRow, double[] decode, PdfFunction[] functions) : base(ShadingType.LatticeFormGouraud, antiAlias, shadingStream.StreamDictionary, colorSpace, bbox, background)
		{
			this.BitsPerCoordinate = bitsPerCoordinate;
			this.BitsPerComponent = bitsPerComponent;
			this.VerticesPerRow = verticesPerRow;
			this.Decode = decode;
			this.Functions = functions;
		}
	}
}
