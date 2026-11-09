using System;
using System.Collections.Generic;
using System.Linq;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;
using UglyToad.PdfPig.Graphics.Colors;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Graphics
{
	// Token: 0x02000103 RID: 259
	internal class ColorSpaceContext : IColorSpaceContext, IDeepCloneable<IColorSpaceContext>
	{
		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060004EE RID: 1262 RVA: 0x0001D7F3 File Offset: 0x0001B9F3
		// (set) Token: 0x060004EF RID: 1263 RVA: 0x0001D7FB File Offset: 0x0001B9FB
		public ColorSpaceDetails CurrentStrokingColorSpace { get; private set; } = DeviceGrayColorSpaceDetails.Instance;

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060004F0 RID: 1264 RVA: 0x0001D804 File Offset: 0x0001BA04
		// (set) Token: 0x060004F1 RID: 1265 RVA: 0x0001D80C File Offset: 0x0001BA0C
		public ColorSpaceDetails CurrentNonStrokingColorSpace { get; private set; } = DeviceGrayColorSpaceDetails.Instance;

		// Token: 0x060004F2 RID: 1266 RVA: 0x0001D818 File Offset: 0x0001BA18
		public ColorSpaceContext(Func<CurrentGraphicsState> currentStateFunc, IResourceStore resourceStore)
		{
			if (currentStateFunc == null)
			{
				throw new ArgumentNullException("currentStateFunc");
			}
			this.currentStateFunc = currentStateFunc;
			if (resourceStore == null)
			{
				throw new ArgumentNullException("resourceStore");
			}
			this.resourceStore = resourceStore;
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x0001D86D File Offset: 0x0001BA6D
		public void SetStrokingColorspace(NameToken colorspace, DictionaryToken dictionary = null)
		{
			this.CurrentStrokingColorSpace = this.resourceStore.GetColorSpaceDetails(colorspace, dictionary);
			if (this.CurrentStrokingColorSpace is UnsupportedColorSpaceDetails)
			{
				return;
			}
			this.currentStateFunc.Invoke().CurrentStrokingColor = this.CurrentStrokingColorSpace.GetInitializeColor();
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0001D8AC File Offset: 0x0001BAAC
		public void SetStrokingColor(IReadOnlyList<decimal> operands, NameToken patternName)
		{
			if (this.CurrentStrokingColorSpace is UnsupportedColorSpaceDetails)
			{
				return;
			}
			if (patternName != null && this.CurrentStrokingColorSpace.Type == ColorSpace.Pattern)
			{
				this.currentStateFunc.Invoke().CurrentStrokingColor = ((PatternColorSpaceDetails)this.CurrentStrokingColorSpace).GetColor(patternName);
				return;
			}
			this.currentStateFunc.Invoke().CurrentStrokingColor = this.CurrentStrokingColorSpace.GetColor(Enumerable.ToArray<double>(Enumerable.Select<decimal, double>(operands, (decimal v) => (double)v)));
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0001D945 File Offset: 0x0001BB45
		public void SetStrokingColorGray(decimal gray)
		{
			this.CurrentStrokingColorSpace = DeviceGrayColorSpaceDetails.Instance;
			this.currentStateFunc.Invoke().CurrentStrokingColor = this.CurrentStrokingColorSpace.GetColor(new double[]
			{
				(double)gray
			});
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001D980 File Offset: 0x0001BB80
		public void SetStrokingColorRgb(decimal r, decimal g, decimal b)
		{
			this.CurrentStrokingColorSpace = DeviceRgbColorSpaceDetails.Instance;
			this.currentStateFunc.Invoke().CurrentStrokingColor = this.CurrentStrokingColorSpace.GetColor(new double[]
			{
				(double)r,
				(double)g,
				(double)b
			});
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0001D9D8 File Offset: 0x0001BBD8
		public void SetStrokingColorCmyk(decimal c, decimal m, decimal y, decimal k)
		{
			this.CurrentStrokingColorSpace = DeviceCmykColorSpaceDetails.Instance;
			this.currentStateFunc.Invoke().CurrentStrokingColor = this.CurrentStrokingColorSpace.GetColor(new double[]
			{
				(double)c,
				(double)m,
				(double)y,
				(double)k
			});
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0001DA3A File Offset: 0x0001BC3A
		public void SetNonStrokingColorspace(NameToken colorspace, DictionaryToken dictionary = null)
		{
			this.CurrentNonStrokingColorSpace = this.resourceStore.GetColorSpaceDetails(colorspace, dictionary);
			if (this.CurrentNonStrokingColorSpace is UnsupportedColorSpaceDetails)
			{
				return;
			}
			this.currentStateFunc.Invoke().CurrentNonStrokingColor = this.CurrentNonStrokingColorSpace.GetInitializeColor();
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0001DA78 File Offset: 0x0001BC78
		public void SetNonStrokingColor(IReadOnlyList<decimal> operands, NameToken patternName)
		{
			if (this.CurrentNonStrokingColorSpace is UnsupportedColorSpaceDetails)
			{
				return;
			}
			if (patternName != null && this.CurrentNonStrokingColorSpace.Type == ColorSpace.Pattern)
			{
				this.currentStateFunc.Invoke().CurrentNonStrokingColor = ((PatternColorSpaceDetails)this.CurrentNonStrokingColorSpace).GetColor(patternName);
				return;
			}
			this.currentStateFunc.Invoke().CurrentNonStrokingColor = this.CurrentNonStrokingColorSpace.GetColor(Enumerable.ToArray<double>(Enumerable.Select<decimal, double>(operands, (decimal v) => (double)v)));
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0001DB11 File Offset: 0x0001BD11
		public void SetNonStrokingColorGray(decimal gray)
		{
			this.CurrentNonStrokingColorSpace = DeviceGrayColorSpaceDetails.Instance;
			this.currentStateFunc.Invoke().CurrentNonStrokingColor = this.CurrentNonStrokingColorSpace.GetColor(new double[]
			{
				(double)gray
			});
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0001DB4C File Offset: 0x0001BD4C
		public void SetNonStrokingColorRgb(decimal r, decimal g, decimal b)
		{
			this.CurrentNonStrokingColorSpace = DeviceRgbColorSpaceDetails.Instance;
			this.currentStateFunc.Invoke().CurrentNonStrokingColor = this.CurrentNonStrokingColorSpace.GetColor(new double[]
			{
				(double)r,
				(double)g,
				(double)b
			});
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0001DBA4 File Offset: 0x0001BDA4
		public void SetNonStrokingColorCmyk(decimal c, decimal m, decimal y, decimal k)
		{
			this.CurrentNonStrokingColorSpace = DeviceCmykColorSpaceDetails.Instance;
			this.currentStateFunc.Invoke().CurrentNonStrokingColor = this.CurrentNonStrokingColorSpace.GetColor(new double[]
			{
				(double)c,
				(double)m,
				(double)y,
				(double)k
			});
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0001DC06 File Offset: 0x0001BE06
		public IColorSpaceContext DeepClone()
		{
			return new ColorSpaceContext(this.currentStateFunc, this.resourceStore)
			{
				CurrentStrokingColorSpace = this.CurrentStrokingColorSpace,
				CurrentNonStrokingColorSpace = this.CurrentNonStrokingColorSpace
			};
		}

		// Token: 0x040002A5 RID: 677
		private readonly Func<CurrentGraphicsState> currentStateFunc;

		// Token: 0x040002A6 RID: 678
		private readonly IResourceStore resourceStore;
	}
}
