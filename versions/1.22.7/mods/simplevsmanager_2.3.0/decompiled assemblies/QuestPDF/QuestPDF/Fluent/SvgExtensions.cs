using System;
using System.IO;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x020000A4 RID: 164
	[NullableContext(1)]
	[Nullable(0)]
	public static class SvgExtensions
	{
		// Token: 0x060004AB RID: 1195 RVA: 0x0000C123 File Offset: 0x0000A323
		internal static void SvgPath(this IContainer container, string svgPath, Color color)
		{
			container.Element(new SvgPath
			{
				Path = svgPath,
				FillColor = color
			});
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0000C140 File Offset: 0x0000A340
		public static SvgImageDescriptor Svg(this IContainer container, string svg)
		{
			QuestPDF.Infrastructure.SvgImage svgImage = Path.GetExtension(svg).Equals(".svg", 5) ? QuestPDF.Infrastructure.SvgImage.FromFile(svg) : QuestPDF.Infrastructure.SvgImage.FromText(svg);
			svgImage.IsShared = false;
			return container.Svg(svgImage);
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x0000C180 File Offset: 0x0000A380
		public static SvgImageDescriptor Svg(this IContainer parent, QuestPDF.Infrastructure.SvgImage image)
		{
			QuestPDF.Elements.SvgImage svgImage = new QuestPDF.Elements.SvgImage
			{
				Image = image
			};
			AspectRatio aspectRatio = new AspectRatio
			{
				Child = svgImage
			};
			parent.Element(aspectRatio);
			AspectRatioOption bestAspectRatioOptionFromParent = ImageExtensions.GetBestAspectRatioOptionFromParent(parent);
			return new SvgImageDescriptor(svgImage, aspectRatio).SetAspectRatio(bestAspectRatioOptionFromParent);
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x0000C1C4 File Offset: 0x0000A3C4
		public static void Svg(this IContainer element, GenerateDynamicSvgDelegate dynamicSvgSource)
		{
			DynamicSvgImage child = new DynamicSvgImage
			{
				SvgSource = dynamicSvgSource
			};
			element.Element(child);
		}
	}
}
