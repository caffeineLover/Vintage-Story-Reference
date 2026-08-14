using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace QuestPDF.Skia.Text
{
	// Token: 0x0200002B RID: 43
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class SkParagraphBuilder : IDisposable
	{
		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00003F65 File Offset: 0x00002165
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00003F6D File Offset: 0x0000216D
		public IntPtr Instance { get; private set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00003F76 File Offset: 0x00002176
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00003F7E File Offset: 0x0000217E
		public ParagraphStyle Style { get; private set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00003F87 File Offset: 0x00002187
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00003F8F File Offset: 0x0000218F
		private SkFontCollection FontCollection { get; set; }

		// Token: 0x060000FA RID: 250 RVA: 0x00003F98 File Offset: 0x00002198
		public static SkParagraphBuilder Create(ParagraphStyle style, SkFontCollection fontCollection)
		{
			SkParagraphBuilder result;
			using (SkText skText = new SkText(style.LineClampEllipsis))
			{
				IntPtr instance = SkParagraphBuilder.API.paragraph_builder_create(new ParagraphStyleConfiguration
				{
					Alignment = style.Alignment,
					Direction = style.Direction,
					MaxLinesVisible = style.MaxLinesVisible,
					LineClampEllipsis = skText.Instance
				}, SkUnicode.Global.Instance, fontCollection.Instance);
				SkiaAPI.EnsureNotNull(instance);
				result = new SkParagraphBuilder
				{
					Instance = instance,
					Style = style,
					FontCollection = fontCollection
				};
			}
			return result;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00004044 File Offset: 0x00002244
		public void AddText(string text, SkTextStyle textStyle)
		{
			SkParagraphBuilder.API.paragraph_builder_add_text(this.Instance, text, textStyle.Instance);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00004058 File Offset: 0x00002258
		public void AddPlaceholder(SkPlaceholderStyle placeholderStyle)
		{
			SkParagraphBuilder.API.paragraph_builder_add_placeholder(this.Instance, placeholderStyle);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00004066 File Offset: 0x00002266
		public SkParagraph CreateParagraph()
		{
			return new SkParagraph(SkParagraphBuilder.API.paragraph_builder_create_paragraph(this.Instance));
		}

		// Token: 0x060000FE RID: 254 RVA: 0x00004078 File Offset: 0x00002278
		public void Reset()
		{
			SkParagraphBuilder.API.paragraph_builder_reset(this.Instance);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00004088 File Offset: 0x00002288
		~SkParagraphBuilder()
		{
			this.WarnThatFinalizerIsReached<SkParagraphBuilder>();
			this.Dispose();
		}

		// Token: 0x06000100 RID: 256 RVA: 0x000040BC File Offset: 0x000022BC
		public void Dispose()
		{
			if (this.Instance == IntPtr.Zero)
			{
				return;
			}
			SkFontCollection fontCollection = this.FontCollection;
			if (fontCollection != null)
			{
				fontCollection.Dispose();
			}
			SkParagraphBuilder.API.paragraph_builder_delete(this.Instance);
			this.Instance = IntPtr.Zero;
			GC.SuppressFinalize(this);
		}

		// Token: 0x0200014D RID: 333
		[NullableContext(0)]
		private static class API
		{
			// Token: 0x06000B33 RID: 2867
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr paragraph_builder_create(ParagraphStyleConfiguration paragraphStyleConfiguration, IntPtr unicode, IntPtr fontCollection);

			// Token: 0x06000B34 RID: 2868
			[NullableContext(1)]
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void paragraph_builder_add_text(IntPtr paragraphBuilder, [MarshalAs(44, MarshalTypeRef = QuestPDF.Skia.Utf8StringMarshaller)] string text, IntPtr textStyle);

			// Token: 0x06000B35 RID: 2869
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void paragraph_builder_add_placeholder(IntPtr paragraphBuilder, SkPlaceholderStyle placeholderStyle);

			// Token: 0x06000B36 RID: 2870
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern IntPtr paragraph_builder_create_paragraph(IntPtr paragraphBuilder);

			// Token: 0x06000B37 RID: 2871
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void paragraph_builder_reset(IntPtr paragraphBuilder);

			// Token: 0x06000B38 RID: 2872
			[DllImport("QuestPdfSkia", CallingConvention = 2)]
			public static extern void paragraph_builder_delete(IntPtr paragraphBuilder);
		}
	}
}
