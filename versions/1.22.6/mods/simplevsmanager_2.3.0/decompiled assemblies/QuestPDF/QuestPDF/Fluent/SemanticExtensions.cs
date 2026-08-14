using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x0200009F RID: 159
	[NullableContext(1)]
	[Nullable(0)]
	public static class SemanticExtensions
	{
		// Token: 0x06000461 RID: 1121 RVA: 0x0000B6C0 File Offset: 0x000098C0
		internal static IContainer Artifact(this IContainer container, int nodeId)
		{
			return container.Element(new ArtifactTag
			{
				Id = nodeId
			});
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0000B6D4 File Offset: 0x000098D4
		public static IContainer SemanticIgnore(this IContainer container)
		{
			return container.Artifact(-1);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x0000B6DD File Offset: 0x000098DD
		internal static IContainer SemanticTag(this IContainer container, string type, [Nullable(2)] string alternativeText = null, [Nullable(2)] string language = null)
		{
			return container.Element(new SemanticTag
			{
				TagType = type,
				Alt = alternativeText,
				Lang = language
			});
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x0000B6FF File Offset: 0x000098FF
		public static IContainer SemanticArticle(this IContainer container)
		{
			return container.SemanticTag("Art", null, null);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x0000B70E File Offset: 0x0000990E
		public static IContainer SemanticSection(this IContainer container)
		{
			return container.SemanticTag("Sect", null, null);
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x0000B71D File Offset: 0x0000991D
		public static IContainer SemanticDivision(this IContainer container)
		{
			return container.SemanticTag("Div", null, null);
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x0000B72C File Offset: 0x0000992C
		public static IContainer SemanticBlockQuotation(this IContainer container)
		{
			return container.SemanticTag("BlockQuote", null, null);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x0000B73B File Offset: 0x0000993B
		public static IContainer SemanticCaption(this IContainer container)
		{
			return container.SemanticTag("Caption", null, null);
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0000B74A File Offset: 0x0000994A
		public static IContainer SemanticIndex(this IContainer container)
		{
			return container.SemanticTag("Index", null, null);
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x0000B759 File Offset: 0x00009959
		public static IContainer SemanticLanguage(this IContainer container, string language)
		{
			return container.SemanticTag("NonStruct", null, language);
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x0000B768 File Offset: 0x00009968
		public static IContainer SemanticTableOfContents(this IContainer container)
		{
			return container.SemanticTag("TOC", null, null);
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x0000B777 File Offset: 0x00009977
		public static IContainer SemanticTableOfContentsItem(this IContainer container)
		{
			return container.SemanticTag("TOCI", null, null);
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x0000B788 File Offset: 0x00009988
		private static IContainer SemanticHeader(this IContainer container, int level)
		{
			if (level < 1 || level > 6)
			{
				throw new ArgumentOutOfRangeException("level", "Header level must be between 1 and 6.");
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 1);
			defaultInterpolatedStringHandler.AppendLiteral("H");
			defaultInterpolatedStringHandler.AppendFormatted<int>(level);
			return container.SemanticTag(defaultInterpolatedStringHandler.ToStringAndClear(), null, null);
		}

		// Token: 0x0600046E RID: 1134 RVA: 0x0000B7D9 File Offset: 0x000099D9
		public static IContainer SemanticHeader1(this IContainer container)
		{
			return container.SemanticHeader(1);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x0000B7E2 File Offset: 0x000099E2
		public static IContainer SemanticHeader2(this IContainer container)
		{
			return container.SemanticHeader(2);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x0000B7EB File Offset: 0x000099EB
		public static IContainer SemanticHeader3(this IContainer container)
		{
			return container.SemanticHeader(3);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0000B7F4 File Offset: 0x000099F4
		public static IContainer SemanticHeader4(this IContainer container)
		{
			return container.SemanticHeader(4);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x0000B7FD File Offset: 0x000099FD
		public static IContainer SemanticHeader5(this IContainer container)
		{
			return container.SemanticHeader(5);
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x0000B806 File Offset: 0x00009A06
		public static IContainer SemanticHeader6(this IContainer container)
		{
			return container.SemanticHeader(6);
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x0000B80F File Offset: 0x00009A0F
		public static IContainer SemanticParagraph(this IContainer container)
		{
			return container.SemanticTag("P", null, null);
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x0000B81E File Offset: 0x00009A1E
		public static IContainer SemanticList(this IContainer container)
		{
			return container.SemanticTag("L", null, null);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x0000B82D File Offset: 0x00009A2D
		public static IContainer SemanticListItem(this IContainer container)
		{
			return container.SemanticTag("LI", null, null);
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0000B83C File Offset: 0x00009A3C
		public static IContainer SemanticListLabel(this IContainer container)
		{
			return container.SemanticTag("Lbl", null, null);
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x0000B84B File Offset: 0x00009A4B
		public static IContainer SemanticListItemBody(this IContainer container)
		{
			return container.SemanticTag("LBody", null, null);
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x0000B85A File Offset: 0x00009A5A
		public static IContainer SemanticTable(this IContainer container)
		{
			return container.SemanticTag("Table", null, null);
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x0000B869 File Offset: 0x00009A69
		public static IContainer SemanticSpan(this IContainer container, [Nullable(2)] string alternativeText = null)
		{
			return container.SemanticTag("Span", alternativeText, null);
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x0000B878 File Offset: 0x00009A78
		public static IContainer SemanticQuote(this IContainer container)
		{
			return container.SemanticTag("Quote", null, null);
		}

		// Token: 0x0600047C RID: 1148 RVA: 0x0000B887 File Offset: 0x00009A87
		public static IContainer SemanticCode(this IContainer container)
		{
			return container.SemanticTag("Code", null, null);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x0000B896 File Offset: 0x00009A96
		public static IContainer SemanticLink(this IContainer container, string alternativeText)
		{
			return container.SemanticTag("Link", alternativeText, null);
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x0000B8A5 File Offset: 0x00009AA5
		public static IContainer SemanticFigure(this IContainer container, string alternativeText)
		{
			return container.SemanticTag("Figure", alternativeText, null);
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x0000B8B4 File Offset: 0x00009AB4
		public static IContainer SemanticImage(this IContainer container, string alternativeText)
		{
			return container.SemanticFigure(alternativeText);
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0000B8BD File Offset: 0x00009ABD
		public static IContainer SemanticFormula(this IContainer container, string alternativeText)
		{
			return container.SemanticTag("Formula", alternativeText, null);
		}
	}
}
