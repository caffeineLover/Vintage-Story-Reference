using System;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Functions;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x02000182 RID: 386
	public sealed class AxialShading : Shading
	{
		// Token: 0x1700028A RID: 650
		// (get) Token: 0x060008FE RID: 2302 RVA: 0x0002938E File Offset: 0x0002758E
		public double[] Coords { get; }

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x060008FF RID: 2303 RVA: 0x00029396 File Offset: 0x00027596
		public double[] Domain { get; }

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000900 RID: 2304 RVA: 0x0002939E File Offset: 0x0002759E
		public override PdfFunction[] Functions { get; }

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000901 RID: 2305 RVA: 0x000293A6 File Offset: 0x000275A6
		public bool[] Extend { get; }

		// Token: 0x06000902 RID: 2306 RVA: 0x000293AE File Offset: 0x000275AE
		public AxialShading(bool antiAlias, DictionaryToken shadingDictionary, ColorSpaceDetails colorSpace, PdfRectangle? bbox, double[] background, double[] coords, double[] domain, PdfFunction[] functions, bool[] extend) : base(ShadingType.Axial, antiAlias, shadingDictionary, colorSpace, bbox, background)
		{
			this.Coords = coords;
			this.Domain = domain;
			this.Functions = functions;
			this.Extend = extend;
		}
	}
}
