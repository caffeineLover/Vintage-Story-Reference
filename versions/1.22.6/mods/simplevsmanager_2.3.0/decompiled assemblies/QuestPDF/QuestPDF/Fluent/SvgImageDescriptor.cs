using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x020000A3 RID: 163
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class SvgImageDescriptor
	{
		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x0000C0A4 File Offset: 0x0000A2A4
		private QuestPDF.Elements.SvgImage ImageElement { get; }

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060004A4 RID: 1188 RVA: 0x0000C0AC File Offset: 0x0000A2AC
		private AspectRatio AspectRatioElement { get; }

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x0000C0B4 File Offset: 0x0000A2B4
		private float ImageAspectRatio { get; }

		// Token: 0x060004A6 RID: 1190 RVA: 0x0000C0BC File Offset: 0x0000A2BC
		internal SvgImageDescriptor(QuestPDF.Elements.SvgImage imageElement, AspectRatio aspectRatioElement)
		{
			this.ImageElement = imageElement;
			this.AspectRatioElement = aspectRatioElement;
			this.ImageAspectRatio = imageElement.Image.SkSvgImage.AspectRatio;
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x0000C0E8 File Offset: 0x0000A2E8
		public SvgImageDescriptor FitWidth()
		{
			return this.SetAspectRatio(AspectRatioOption.FitWidth);
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x0000C0F1 File Offset: 0x0000A2F1
		public SvgImageDescriptor FitHeight()
		{
			return this.SetAspectRatio(AspectRatioOption.FitHeight);
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x0000C0FA File Offset: 0x0000A2FA
		public SvgImageDescriptor FitArea()
		{
			return this.SetAspectRatio(AspectRatioOption.FitArea);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0000C103 File Offset: 0x0000A303
		internal SvgImageDescriptor SetAspectRatio(AspectRatioOption option)
		{
			this.AspectRatioElement.Ratio = this.ImageAspectRatio;
			this.AspectRatioElement.Option = option;
			return this;
		}
	}
}
