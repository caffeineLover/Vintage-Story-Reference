using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing.Exceptions;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x0200008D RID: 141
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ImageDescriptor
	{
		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060003DC RID: 988 RVA: 0x0000A735 File Offset: 0x00008935
		private QuestPDF.Elements.Image ImageElement { get; }

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060003DD RID: 989 RVA: 0x0000A73D File Offset: 0x0000893D
		private AspectRatio AspectRatioElement { get; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060003DE RID: 990 RVA: 0x0000A745 File Offset: 0x00008945
		private float ImageAspectRatio { get; }

		// Token: 0x060003DF RID: 991 RVA: 0x0000A750 File Offset: 0x00008950
		internal ImageDescriptor(QuestPDF.Elements.Image imageElement, AspectRatio aspectRatioElement)
		{
			this.ImageElement = imageElement;
			this.AspectRatioElement = aspectRatioElement;
			ImageSize size = this.ImageElement.DocumentImage.Size;
			this.ImageAspectRatio = (float)size.Width / (float)size.Height;
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x0000A797 File Offset: 0x00008997
		public ImageDescriptor UseOriginalImage(bool value = true)
		{
			this.ImageElement.UseOriginalImage = value;
			return this;
		}

		// Token: 0x060003E1 RID: 993 RVA: 0x0000A7A6 File Offset: 0x000089A6
		public ImageDescriptor WithRasterDpi(int dpi)
		{
			if (dpi <= 0)
			{
				throw new DocumentComposeException("DPI value must be greater than 0.");
			}
			this.ImageElement.TargetDpi = new int?(dpi);
			return this;
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0000A7C9 File Offset: 0x000089C9
		public ImageDescriptor WithCompressionQuality(ImageCompressionQuality quality)
		{
			this.ImageElement.CompressionQuality = new ImageCompressionQuality?(quality);
			return this;
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x0000A7DD File Offset: 0x000089DD
		public ImageDescriptor FitWidth()
		{
			return this.SetAspectRatio(AspectRatioOption.FitWidth);
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x0000A7E6 File Offset: 0x000089E6
		public ImageDescriptor FitHeight()
		{
			return this.SetAspectRatio(AspectRatioOption.FitHeight);
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x0000A7EF File Offset: 0x000089EF
		public ImageDescriptor FitArea()
		{
			return this.SetAspectRatio(AspectRatioOption.FitArea);
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x0000A7F8 File Offset: 0x000089F8
		public ImageDescriptor FitUnproportionally()
		{
			this.AspectRatioElement.Ratio = 0f;
			return this;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x0000A80B File Offset: 0x00008A0B
		internal ImageDescriptor SetAspectRatio(AspectRatioOption option)
		{
			this.AspectRatioElement.Ratio = this.ImageAspectRatio;
			this.AspectRatioElement.Option = option;
			return this;
		}
	}
}
