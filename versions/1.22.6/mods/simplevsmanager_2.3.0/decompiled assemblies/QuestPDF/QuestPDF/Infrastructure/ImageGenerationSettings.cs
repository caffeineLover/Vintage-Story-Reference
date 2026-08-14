using System;
using System.Runtime.CompilerServices;

namespace QuestPDF.Infrastructure
{
	// Token: 0x02000055 RID: 85
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ImageGenerationSettings
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000202 RID: 514 RVA: 0x000053CC File Offset: 0x000035CC
		// (set) Token: 0x06000203 RID: 515 RVA: 0x000053D4 File Offset: 0x000035D4
		public ImageFormat ImageFormat { get; set; } = ImageFormat.Png;

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000204 RID: 516 RVA: 0x000053DD File Offset: 0x000035DD
		// (set) Token: 0x06000205 RID: 517 RVA: 0x000053E5 File Offset: 0x000035E5
		public ImageCompressionQuality ImageCompressionQuality { get; set; } = ImageCompressionQuality.High;

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000206 RID: 518 RVA: 0x000053EE File Offset: 0x000035EE
		// (set) Token: 0x06000207 RID: 519 RVA: 0x000053F6 File Offset: 0x000035F6
		public int RasterDpi { get; set; } = 288;

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000208 RID: 520 RVA: 0x000053FF File Offset: 0x000035FF
		public static ImageGenerationSettings Default
		{
			get
			{
				return new ImageGenerationSettings();
			}
		}
	}
}
