using System;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Functions;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x02000187 RID: 391
	public sealed class TensorProductPatchMeshesShading : Shading
	{
		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x0600091A RID: 2330 RVA: 0x0002955D File Offset: 0x0002775D
		public int BitsPerCoordinate { get; }

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x0600091B RID: 2331 RVA: 0x00029565 File Offset: 0x00027765
		public int BitsPerComponent { get; }

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x0600091C RID: 2332 RVA: 0x0002956D File Offset: 0x0002776D
		public int BitsPerFlag { get; }

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x0600091D RID: 2333 RVA: 0x00029575 File Offset: 0x00027775
		public double[] Decode { get; }

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x0600091E RID: 2334 RVA: 0x0002957D File Offset: 0x0002777D
		public override PdfFunction[] Functions { get; }

		// Token: 0x0600091F RID: 2335 RVA: 0x00029585 File Offset: 0x00027785
		public TensorProductPatchMeshesShading(bool antiAlias, StreamToken shadingStream, ColorSpaceDetails colorSpace, PdfRectangle? bbox, double[] background, int bitsPerCoordinate, int bitsPerComponent, int bitsPerFlag, double[] decode, PdfFunction[] functions) : base(ShadingType.TensorProductPatch, antiAlias, shadingStream.StreamDictionary, colorSpace, bbox, background)
		{
			this.BitsPerCoordinate = bitsPerCoordinate;
			this.BitsPerComponent = bitsPerComponent;
			this.BitsPerFlag = bitsPerFlag;
			this.Decode = decode;
			this.Functions = functions;
		}
	}
}
