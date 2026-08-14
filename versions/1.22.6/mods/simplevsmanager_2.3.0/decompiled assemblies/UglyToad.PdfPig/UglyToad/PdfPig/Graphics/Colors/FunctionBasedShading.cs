using System;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Functions;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Graphics.Colors
{
	// Token: 0x02000181 RID: 385
	public sealed class FunctionBasedShading : Shading
	{
		// Token: 0x17000287 RID: 647
		// (get) Token: 0x060008FA RID: 2298 RVA: 0x0002934E File Offset: 0x0002754E
		public double[] Domain { get; }

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x060008FB RID: 2299 RVA: 0x00029356 File Offset: 0x00027556
		public TransformationMatrix Matrix { get; }

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x060008FC RID: 2300 RVA: 0x0002935E File Offset: 0x0002755E
		public override PdfFunction[] Functions { get; }

		// Token: 0x060008FD RID: 2301 RVA: 0x00029366 File Offset: 0x00027566
		public FunctionBasedShading(bool antiAlias, DictionaryToken shadingDictionary, ColorSpaceDetails colorSpace, PdfRectangle? bbox, double[] background, double[] domain, TransformationMatrix matrix, PdfFunction[] functions) : base(ShadingType.FunctionBased, antiAlias, shadingDictionary, colorSpace, bbox, background)
		{
			this.Domain = domain;
			this.Matrix = matrix;
			this.Functions = functions;
		}
	}
}
