using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Elements.Text;
using QuestPDF.Elements.Text.Items;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x020000AF RID: 175
	[NullableContext(1)]
	[Nullable(0)]
	public static class TextExtensions
	{
		// Token: 0x060004FF RID: 1279 RVA: 0x0000CC08 File Offset: 0x0000AE08
		public static void Text(this IContainer element, Action<TextDescriptor> content)
		{
			TextDescriptor textDescriptor = new TextDescriptor();
			Alignment alignment = element as Alignment;
			if (alignment != null)
			{
				textDescriptor.TextBlock.Alignment = TextExtensions.MapAlignment(alignment.Horizontal);
			}
			if (content != null)
			{
				content.Invoke(textDescriptor);
			}
			textDescriptor.Compose(element);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0000CC4C File Offset: 0x0000AE4C
		[Obsolete("This method has been deprecated since version 2022.3. Please use the overload that returns a TextSpanDescriptor object which allows to specify text style.")]
		[ExcludeFromCodeCoverage]
		public static void Text(this IContainer element, [Nullable(2)] object text, TextStyle style)
		{
			element.Text(text).Style(style);
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0000CC5C File Offset: 0x0000AE5C
		[Obsolete("This method has been deprecated since version 2022.12. Please use an overload where the text parameter is passed explicitly as a string.")]
		[ExcludeFromCodeCoverage]
		public static TextSpanDescriptor Text(this IContainer element, [Nullable(2)] object text)
		{
			return element.Text((text != null) ? text.ToString() : null);
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0000CC70 File Offset: 0x0000AE70
		public static TextBlockDescriptor Text(this IContainer container, [Nullable(2)] string text)
		{
			if (text == null)
			{
				return new TextBlockDescriptor(new TextBlock(), new TextBlockSpan());
			}
			TextBlock textBlock = new TextBlock();
			container.Element(textBlock);
			Alignment alignment = container as Alignment;
			if (alignment != null)
			{
				textBlock.Alignment = TextExtensions.MapAlignment(alignment.Horizontal);
			}
			TextBlockSpan textBlockSpan = new TextBlockSpan
			{
				Text = text
			};
			textBlock.Items.Add(textBlockSpan);
			return new TextBlockDescriptor(textBlock, textBlockSpan);
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0000CCDC File Offset: 0x0000AEDC
		private static TextHorizontalAlignment? MapAlignment(HorizontalAlignment? alignment)
		{
			if (alignment != null)
			{
				switch (alignment.GetValueOrDefault())
				{
				case HorizontalAlignment.Left:
					return new TextHorizontalAlignment?(TextHorizontalAlignment.Left);
				case HorizontalAlignment.Center:
					return new TextHorizontalAlignment?(TextHorizontalAlignment.Center);
				case HorizontalAlignment.Right:
					return new TextHorizontalAlignment?(TextHorizontalAlignment.Right);
				}
			}
			return default(TextHorizontalAlignment?);
		}
	}
}
