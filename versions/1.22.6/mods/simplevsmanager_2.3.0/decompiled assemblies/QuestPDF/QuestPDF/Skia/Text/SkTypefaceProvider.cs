using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace QuestPDF.Skia.Text
{
	// Token: 0x0200002F RID: 47
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class SkTypefaceProvider : IDisposable
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00004220 File Offset: 0x00002420
		// (set) Token: 0x0600010D RID: 269 RVA: 0x00004228 File Offset: 0x00002428
		public IntPtr Instance { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00004231 File Offset: 0x00002431
		private List<SkTypeface> Typefaces { get; } = new List<SkTypeface>();

		// Token: 0x0600010F RID: 271 RVA: 0x00004239 File Offset: 0x00002439
		public SkTypefaceProvider()
		{
			this.Instance = SkTypefaceProvider.API.typeface_font_provider_create();
			SkiaAPI.EnsureNotNull(this.Instance);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00004264 File Offset: 0x00002464
		public void AddTypefaceFromData(SkData data, [Nullable(2)] string alias = null)
		{
			SkTypeface skTypeface = SkFontManager.Global.CreateTypeface(data);
			this.Typefaces.Add(skTypeface);
			if (alias == null)
			{
				SkTypefaceProvider.API.typeface_font_provider_add_typeface(this.Instance, skTypeface.Instance);
				return;
			}
			SkTypefaceProvider.API.typeface_font_provider_add_typeface_with_custom_alias(this.Instance, skTypeface.Instance, alias);
		}

		// Token: 0x06000111 RID: 273 RVA: 0x000042B0 File Offset: 0x000024B0
		~SkTypefaceProvider()
		{
			this.WarnThatFinalizerIsReached<SkTypefaceProvider>();
			this.Dispose();
		}

		// Token: 0x06000112 RID: 274 RVA: 0x000042E4 File Offset: 0x000024E4
		public void Dispose()
		{
			if (this.Instance == IntPtr.Zero)
			{
				return;
			}
			foreach (SkTypeface skTypeface in this.Typefaces)
			{
				skTypeface.Dispose();
			}
			SkTypefaceProvider.API.typeface_font_provider_unref(this.Instance);
			this.Instance = IntPtr.Zero;
			GC.SuppressFinalize(this);
		}

		// Token: 0x02000155 RID: 341
		[NullableContext(0)]
		private static class API
		{
			// Token: 0x06000B3C RID: 2876
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr typeface_font_provider_create();

			// Token: 0x06000B3D RID: 2877
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void typeface_font_provider_add_typeface(IntPtr typefaceProvider, IntPtr typeface);

			// Token: 0x06000B3E RID: 2878
			[NullableContext(1)]
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void typeface_font_provider_add_typeface_with_custom_alias(IntPtr typefaceProvider, IntPtr typeface, [MarshalAs(44, MarshalTypeRef = QuestPDF.Skia.Utf8StringMarshaller)] string alias);

			// Token: 0x06000B3F RID: 2879
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void typeface_font_provider_unref(IntPtr typefaceProvider);
		}
	}
}
