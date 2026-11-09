using System;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Functions;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x02000183 RID: 387
	public sealed class RadialShading : Shading
	{
		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000903 RID: 2307 RVA: 0x000293DE File Offset: 0x000275DE
		public double[] Coords { get; }

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x000293E6 File Offset: 0x000275E6
		public double[] Domain { get; }

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000905 RID: 2309 RVA: 0x000293EE File Offset: 0x000275EE
		public override PdfFunction[] Functions { get; }

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000906 RID: 2310 RVA: 0x000293F6 File Offset: 0x000275F6
		public bool[] Extend { get; }

		// Token: 0x06000907 RID: 2311 RVA: 0x000293FE File Offset: 0x000275FE
		public RadialShading(bool antiAlias, DictionaryToken shadingDictionary, ColorSpaceDetails colorSpace, PdfRectangle? bbox, double[] background, double[] coords, double[] domain, PdfFunction[] functions, bool[] extend) : base(ShadingType.Radial, antiAlias, shadingDictionary, colorSpace, bbox, background)
		{
			this.Coords = coords;
			this.Domain = domain;
			this.Functions = functions;
			this.Extend = extend;
		}
	}
}
