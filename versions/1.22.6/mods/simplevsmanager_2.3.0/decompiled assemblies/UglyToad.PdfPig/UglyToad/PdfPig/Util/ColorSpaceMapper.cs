using System;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Graphics.Colors;
using UglyToad.PdfPig.Tokens;

namespace UglyToad.PdfPig.Util
{
	// Token: 0x02000027 RID: 39
	internal static class ColorSpaceMapper
	{
		// Token: 0x06000114 RID: 276 RVA: 0x0000A674 File Offset: 0x00008874
		public static bool TryMap(NameToken name, IResourceStore resourceStore, out ColorSpace colorSpaceResult)
		{
			ResourceColorSpace resourceColorSpace;
			return name.TryMapToColorSpace(out colorSpaceResult) || (resourceStore.TryGetNamedColorSpace(name, out resourceColorSpace) && resourceColorSpace.Name.TryMapToColorSpace(out colorSpaceResult));
		}
	}
}
