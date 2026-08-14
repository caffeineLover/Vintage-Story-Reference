using System;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Functions;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x02000186 RID: 390
	public class CoonsPatchMeshesShading : Shading
	{
		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000914 RID: 2324 RVA: 0x000294F8 File Offset: 0x000276F8
		public int BitsPerCoordinate { get; }

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000915 RID: 2325 RVA: 0x00029500 File Offset: 0x00027700
		public int BitsPerComponent { get; }

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000916 RID: 2326 RVA: 0x00029508 File Offset: 0x00027708
		public int BitsPerFlag { get; }

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000917 RID: 2327 RVA: 0x00029510 File Offset: 0x00027710
		public double[] Decode { get; }

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000918 RID: 2328 RVA: 0x00029518 File Offset: 0x00027718
		public override PdfFunction[] Functions { get; }

		// Token: 0x06000919 RID: 2329 RVA: 0x00029520 File Offset: 0x00027720
		public CoonsPatchMeshesShading(bool antiAlias, StreamToken shadingStream, ColorSpaceDetails colorSpace, PdfRectangle? bbox, double[] background, int bitsPerCoordinate, int bitsPerComponent, int bitsPerFlag, double[] decode, PdfFunction[] functions) : base(ShadingType.CoonsPatch, antiAlias, shadingStream.StreamDictionary, colorSpace, bbox, background)
		{
			this.BitsPerCoordinate = bitsPerCoordinate;
			this.BitsPerComponent = bitsPerComponent;
			this.BitsPerFlag = bitsPerFlag;
			this.Decode = decode;
			this.Functions = functions;
		}
	}
}
