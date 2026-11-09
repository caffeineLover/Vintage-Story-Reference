using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing.Exceptions;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x0200008C RID: 140
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class DynamicImageDescriptor
	{
		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x0000A6D8 File Offset: 0x000088D8
		private DynamicImage ImageElement { get; }

		// Token: 0x060003D8 RID: 984 RVA: 0x0000A6E0 File Offset: 0x000088E0
		internal DynamicImageDescriptor(DynamicImage imageElement)
		{
			this.ImageElement = imageElement;
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x0000A6EF File Offset: 0x000088EF
		public DynamicImageDescriptor UseOriginalImage(bool value = true)
		{
			this.ImageElement.UseOriginalImage = value;
			return this;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x0000A6FE File Offset: 0x000088FE
		public DynamicImageDescriptor WithRasterDpi(int dpi)
		{
			if (dpi <= 0)
			{
				throw new DocumentComposeException("DPI value must be greater than 0.");
			}
			this.ImageElement.TargetDpi = new int?(dpi);
			return this;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x0000A721 File Offset: 0x00008921
		public DynamicImageDescriptor WithCompressionQuality(ImageCompressionQuality quality)
		{
			this.ImageElement.CompressionQuality = new ImageCompressionQuality?(quality);
			return this;
		}
	}
}
