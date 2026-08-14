using System;
using System.Runtime.InteropServices;

namespace QuestPDF.Skia.Text
{
	// Token: 0x0200002D RID: 45
	internal sealed class SkTextStyle : IDisposable
	{
		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00004101 File Offset: 0x00002301
		// (set) Token: 0x06000103 RID: 259 RVA: 0x00004109 File Offset: 0x00002309
		public IntPtr Instance { get; private set; }

		// Token: 0x06000104 RID: 260 RVA: 0x00004112 File Offset: 0x00002312
		public SkTextStyle(TextStyleConfiguration textStyleConfiguration)
		{
			this.Instance = SkTextStyle.API.text_style_create(textStyleConfiguration);
			SkiaAPI.EnsureNotNull(this.Instance);
		}

		// Token: 0x06000105 RID: 261 RVA: 0x00004134 File Offset: 0x00002334
		~SkTextStyle()
		{
			this.WarnThatFinalizerIsReached<SkTextStyle>();
			this.Dispose();
		}

		// Token: 0x06000106 RID: 262 RVA: 0x00004168 File Offset: 0x00002368
		public void Dispose()
		{
			if (this.Instance == IntPtr.Zero)
			{
				return;
			}
			SkTextStyle.API.text_style_delete(this.Instance);
			this.Instance = IntPtr.Zero;
			GC.SuppressFinalize(this);
		}

		// Token: 0x02000153 RID: 339
		private static class API
		{
			// Token: 0x06000B39 RID: 2873
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr text_style_create(TextStyleConfiguration textStyleConfiguration);

			// Token: 0x06000B3A RID: 2874
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void text_style_delete(IntPtr textStyle);
		}
	}
}
