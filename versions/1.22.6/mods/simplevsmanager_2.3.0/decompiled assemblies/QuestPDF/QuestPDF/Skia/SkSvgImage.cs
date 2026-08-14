using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using QuestPDF.Skia.Text;

namespace QuestPDF.Skia
{
	// Token: 0x02000020 RID: 32
	internal sealed class SkSvgImage : IDisposable
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x000033ED File Offset: 0x000015ED
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x000033F5 File Offset: 0x000015F5
		public IntPtr Instance { get; private set; }

		// Token: 0x060000A8 RID: 168 RVA: 0x00003400 File Offset: 0x00001600
		[NullableContext(1)]
		public SkSvgImage(string svgString, SkResourceProvider resourceProvider, SkFontManager fontManager)
		{
			using (SkData skData = SkData.FromBinary(Encoding.UTF8.GetBytes(svgString)))
			{
				this.Instance = SkSvgImage.API.svg_create(skData.Instance, resourceProvider.Instance, fontManager.Instance);
				if (this.Instance == IntPtr.Zero)
				{
					throw new Exception("Cannot decode the provided SVG image.");
				}
				SkSvgImage.API.svg_get_size(this.Instance, out this.Size, out this.ViewBox);
			}
		}

		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000A9 RID: 169 RVA: 0x0000348C File Offset: 0x0000168C
		internal float AspectRatio
		{
			get
			{
				if (this.Size.WidthUnit == SkSvgImageSize.Unit.Percentage || this.Size.HeightUnit == SkSvgImageSize.Unit.Percentage)
				{
					return this.ViewBox.Width / this.ViewBox.Height;
				}
				return this.Size.Width / this.Size.Height;
			}
		}

		// Token: 0x060000AA RID: 170 RVA: 0x000034E4 File Offset: 0x000016E4
		~SkSvgImage()
		{
			this.WarnThatFinalizerIsReached<SkSvgImage>();
			this.Dispose();
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00003518 File Offset: 0x00001718
		public void Dispose()
		{
			if (this.Instance == IntPtr.Zero)
			{
				return;
			}
			SkSvgImage.API.svg_unref(this.Instance);
			this.Instance = IntPtr.Zero;
			GC.SuppressFinalize(this);
		}

		// Token: 0x04000073 RID: 115
		public SkSvgImageSize Size;

		// Token: 0x04000074 RID: 116
		public SkRect ViewBox;

		// Token: 0x02000142 RID: 322
		private static class API
		{
			// Token: 0x06000B1C RID: 2844
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr svg_create(IntPtr data, IntPtr resourceProvider, IntPtr fontManager);

			// Token: 0x06000B1D RID: 2845
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void svg_unref(IntPtr svg);

			// Token: 0x06000B1E RID: 2846
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void svg_get_size(IntPtr svg, out SkSvgImageSize size, out SkRect viewBox);
		}
	}
}
