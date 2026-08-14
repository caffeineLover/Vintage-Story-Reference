using System;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Functions;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x02000184 RID: 388
	public sealed class FreeFormGouraudShading : Shading
	{
		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000908 RID: 2312 RVA: 0x0002942E File Offset: 0x0002762E
		public int BitsPerCoordinate { get; }

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000909 RID: 2313 RVA: 0x00029436 File Offset: 0x00027636
		public int BitsPerComponent { get; }

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x0600090A RID: 2314 RVA: 0x0002943E File Offset: 0x0002763E
		public int BitsPerFlag { get; }

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x0600090B RID: 2315 RVA: 0x00029446 File Offset: 0x00027646
		public double[] Decode { get; }

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x0600090C RID: 2316 RVA: 0x0002944E File Offset: 0x0002764E
		public override PdfFunction[] Functions { get; }

		// Token: 0x0600090D RID: 2317 RVA: 0x00029456 File Offset: 0x00027656
		public FreeFormGouraudShading(bool antiAlias, StreamToken shadingStream, ColorSpaceDetails colorSpace, PdfRectangle? bbox, double[] background, int bitsPerCoordinate, int bitsPerComponent, int bitsPerFlag, double[] decode, PdfFunction[] functions) : base(ShadingType.FreeFormGouraud, antiAlias, shadingStream.StreamDictionary, colorSpace, bbox, background)
		{
			this.BitsPerCoordinate = bitsPerCoordinate;
			this.BitsPerComponent = bitsPerComponent;
			this.BitsPerFlag = bitsPerFlag;
			this.Decode = decode;
			this.Functions = functions;
		}
	}
}
