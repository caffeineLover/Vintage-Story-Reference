using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Skia.Text;

namespace QuestPDF.Elements.Text
{
	// Token: 0x020000FB RID: 251
	[NullableContext(1)]
	[Nullable(0)]
	internal static class SkParagraphBuilderPoolManager
	{
		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x06000853 RID: 2131 RVA: 0x00014409 File Offset: 0x00012609
		private static ConcurrentDictionary<ParagraphStyle, ConcurrentBag<SkParagraphBuilder>> ObjectPool { get; } = new ConcurrentDictionary<ParagraphStyle, ConcurrentBag<SkParagraphBuilder>>();

		// Token: 0x06000854 RID: 2132 RVA: 0x00014410 File Offset: 0x00012610
		public static SkParagraphBuilder Get(ParagraphStyle style)
		{
			SkParagraphBuilder result;
			if (SkParagraphBuilderPoolManager.GetPool(style).TryTake(ref result))
			{
				return result;
			}
			SkFontCollection fontCollection = SkFontCollection.Create(FontManager.TypefaceProvider, FontManager.CurrentFontManager);
			return SkParagraphBuilder.Create(style, fontCollection);
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x00014445 File Offset: 0x00012645
		public static void Return(SkParagraphBuilder builder)
		{
			builder.Reset();
			SkParagraphBuilderPoolManager.GetPool(builder.Style).Add(builder);
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x0001445E File Offset: 0x0001265E
		private static ConcurrentBag<SkParagraphBuilder> GetPool(ParagraphStyle style)
		{
			return SkParagraphBuilderPoolManager.ObjectPool.GetOrAdd(style, (ParagraphStyle _) => new ConcurrentBag<SkParagraphBuilder>());
		}
	}
}
