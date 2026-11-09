using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Drawing.Exceptions;
using QuestPDF.Elements.Text.Items;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;
using QuestPDF.Skia.Text;

namespace QuestPDF.Elements.Text
{
	// Token: 0x020000FC RID: 252
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class TextBlock : Element, IStateful, IContentDirectionAware, IDisposable
	{
		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x06000858 RID: 2136 RVA: 0x00014496 File Offset: 0x00012696
		// (set) Token: 0x06000859 RID: 2137 RVA: 0x0001449E File Offset: 0x0001269E
		public List<ITextBlockItem> Items { get; set; } = new List<ITextBlockItem>();

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x0600085A RID: 2138 RVA: 0x000144A7 File Offset: 0x000126A7
		// (set) Token: 0x0600085B RID: 2139 RVA: 0x000144AF File Offset: 0x000126AF
		public TextHorizontalAlignment? Alignment { get; set; }

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x0600085C RID: 2140 RVA: 0x000144B8 File Offset: 0x000126B8
		// (set) Token: 0x0600085D RID: 2141 RVA: 0x000144C0 File Offset: 0x000126C0
		public ContentDirection ContentDirection { get; set; }

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x0600085E RID: 2142 RVA: 0x000144C9 File Offset: 0x000126C9
		// (set) Token: 0x0600085F RID: 2143 RVA: 0x000144D1 File Offset: 0x000126D1
		public int? LineClamp { get; set; }

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000860 RID: 2144 RVA: 0x000144DA File Offset: 0x000126DA
		// (set) Token: 0x06000861 RID: 2145 RVA: 0x000144E2 File Offset: 0x000126E2
		public string LineClampEllipsis { get; set; }

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000862 RID: 2146 RVA: 0x000144EB File Offset: 0x000126EB
		// (set) Token: 0x06000863 RID: 2147 RVA: 0x000144F3 File Offset: 0x000126F3
		public float ParagraphSpacing { get; set; }

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06000864 RID: 2148 RVA: 0x000144FC File Offset: 0x000126FC
		// (set) Token: 0x06000865 RID: 2149 RVA: 0x00014504 File Offset: 0x00012704
		public float ParagraphFirstLineIndentation { get; set; }

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x06000866 RID: 2150 RVA: 0x0001450D File Offset: 0x0001270D
		// (set) Token: 0x06000867 RID: 2151 RVA: 0x00014515 File Offset: 0x00012715
		public TextStyle DefaultTextStyle { get; set; } = TextStyle.Default;

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06000868 RID: 2152 RVA: 0x0001451E File Offset: 0x0001271E
		// (set) Token: 0x06000869 RID: 2153 RVA: 0x00014526 File Offset: 0x00012726
		private bool RebuildParagraphForEveryPage { get; set; }

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x0600086A RID: 2154 RVA: 0x0001452F File Offset: 0x0001272F
		// (set) Token: 0x0600086B RID: 2155 RVA: 0x00014537 File Offset: 0x00012737
		private bool AreParagraphMetricsValid { get; set; }

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x0600086C RID: 2156 RVA: 0x00014540 File Offset: 0x00012740
		// (set) Token: 0x0600086D RID: 2157 RVA: 0x00014548 File Offset: 0x00012748
		private bool AreParagraphItemsTransformedWithSpacingAndIndentation { get; set; }

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x0600086E RID: 2158 RVA: 0x00014551 File Offset: 0x00012751
		// (set) Token: 0x0600086F RID: 2159 RVA: 0x00014559 File Offset: 0x00012759
		private SkSize[] LineMetrics { get; set; }

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000870 RID: 2160 RVA: 0x00014562 File Offset: 0x00012762
		// (set) Token: 0x06000871 RID: 2161 RVA: 0x0001456A File Offset: 0x0001276A
		private float WidthForLineMetricsCalculation { get; set; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000872 RID: 2162 RVA: 0x00014573 File Offset: 0x00012773
		// (set) Token: 0x06000873 RID: 2163 RVA: 0x0001457B File Offset: 0x0001277B
		private float MaximumWidth { get; set; }

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000874 RID: 2164 RVA: 0x00014584 File Offset: 0x00012784
		// (set) Token: 0x06000875 RID: 2165 RVA: 0x0001458C File Offset: 0x0001278C
		private SkRect[] PlaceholderPositions { get; set; }

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000876 RID: 2166 RVA: 0x00014595 File Offset: 0x00012795
		// (set) Token: 0x06000877 RID: 2167 RVA: 0x0001459D File Offset: 0x0001279D
		private bool? ContainsOnlyWhiteSpace { get; set; }

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000878 RID: 2168 RVA: 0x000145A6 File Offset: 0x000127A6
		// (set) Token: 0x06000879 RID: 2169 RVA: 0x000145AE File Offset: 0x000127AE
		private SkParagraph Paragraph { get; set; }

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x0600087A RID: 2170 RVA: 0x000145B7 File Offset: 0x000127B7
		// (set) Token: 0x0600087B RID: 2171 RVA: 0x000145BF File Offset: 0x000127BF
		internal bool ClearInternalCacheAfterFullRender { get; set; } = true;

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x0600087C RID: 2172 RVA: 0x000145C8 File Offset: 0x000127C8
		public string Text
		{
			get
			{
				return string.Join(" ", Enumerable.Select<TextBlockSpan, string>(Enumerable.OfType<TextBlockSpan>(this.Items), (TextBlockSpan x) => x.Text));
			}
		}

		// Token: 0x0600087D RID: 2173 RVA: 0x00014604 File Offset: 0x00012804
		~TextBlock()
		{
			if (this.Paragraph != null)
			{
				this.WarnThatFinalizerIsReached<TextBlock>();
				this.Dispose();
			}
		}

		// Token: 0x0600087E RID: 2174 RVA: 0x00014640 File Offset: 0x00012840
		public void Dispose()
		{
			SkParagraph paragraph = this.Paragraph;
			if (paragraph != null)
			{
				paragraph.Dispose();
			}
			foreach (TextBlockElement textBlockElement in Enumerable.OfType<TextBlockElement>(this.Items))
			{
				textBlockElement.Element.ReleaseDisposableChildren();
			}
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x000146AC File Offset: 0x000128AC
		internal override SpacePlan Measure(Size availableSpace)
		{
			if (this.Items.Count == 0)
			{
				return SpacePlan.Empty();
			}
			if (this.IsRendered)
			{
				return SpacePlan.Empty();
			}
			if (availableSpace.IsNegative())
			{
				return SpacePlan.Wrap("The available space is negative.");
			}
			bool? containsOnlyWhiteSpace = this.ContainsOnlyWhiteSpace;
			bool flag = containsOnlyWhiteSpace.GetValueOrDefault();
			if (containsOnlyWhiteSpace == null)
			{
				flag = this.CheckIfContainsOnlyWhiteSpace();
				bool? containsOnlyWhiteSpace2 = new bool?(flag);
				this.ContainsOnlyWhiteSpace = containsOnlyWhiteSpace2;
			}
			if (this.ContainsOnlyWhiteSpace.GetValueOrDefault())
			{
				float num = this.MeasureHeightOfParagraphContainingOnlyWhiteSpace();
				if (num >= availableSpace.Height + 0.01f)
				{
					return SpacePlan.Wrap("The available vertical space is not sufficient to render even a single line of text.");
				}
				return SpacePlan.FullRender(0f, num);
			}
			else
			{
				if (availableSpace.Width < 0.01f || availableSpace.Height < 0.01f)
				{
					return SpacePlan.Wrap("The available space is not sufficient to render even a single line of text.");
				}
				this.Initialize();
				this.CalculateParagraphMetrics(availableSpace);
				if (availableSpace.Width < this.MaximumWidth - 0.01f)
				{
					return SpacePlan.Wrap("The available space is not sufficient to render even a single character.");
				}
				if (this.MaximumWidth == 0f)
				{
					return SpacePlan.FullRender(Size.Zero);
				}
				float num2 = 0f;
				int num3 = 0;
				for (int i = this.CurrentLineIndex; i < this.LineMetrics.Length; i++)
				{
					SkSize skSize = this.LineMetrics[i];
					float num4 = num2 + skSize.Height;
					if (num4 > availableSpace.Height + 0.01f)
					{
						break;
					}
					num2 = num4;
					num3++;
				}
				if (num3 == 0)
				{
					return SpacePlan.Wrap("The available space is not sufficient to render even a single line of text.");
				}
				Size size = new Size(Math.Min(this.MaximumWidth, availableSpace.Width), Math.Min(num2, availableSpace.Height));
				if (this.CurrentLineIndex + num3 < this.LineMetrics.Length)
				{
					return SpacePlan.PartialRender(size);
				}
				return SpacePlan.FullRender(size);
			}
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x00014870 File Offset: 0x00012A70
		internal override void Draw(Size availableSpace)
		{
			TextBlock.<>c__DisplayClass77_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.availableSpace = availableSpace;
			if (this.Items.Count == 0)
			{
				return;
			}
			if (this.IsRendered)
			{
				return;
			}
			if (this.ContainsOnlyWhiteSpace.GetValueOrDefault())
			{
				return;
			}
			this.CalculateParagraphMetrics(CS$<>8__locals1.availableSpace);
			if (this.MaximumWidth == 0f)
			{
				return;
			}
			ValueTuple<int, float> valueTuple = this.<Draw>g__DetermineLinesToDraw|77_0(ref CS$<>8__locals1);
			CS$<>8__locals1.linesToDraw = valueTuple.Item1;
			CS$<>8__locals1.takenHeight = valueTuple.Item2;
			this.<Draw>g__DrawParagraph|77_1(ref CS$<>8__locals1);
			this.CurrentLineIndex += CS$<>8__locals1.linesToDraw;
			this.CurrentTopOffset += CS$<>8__locals1.takenHeight;
			if (this.CurrentLineIndex == this.LineMetrics.Length)
			{
				this.IsRendered = true;
			}
			if (this.IsRendered && this.ClearInternalCacheAfterFullRender)
			{
				SkParagraph paragraph = this.Paragraph;
				if (paragraph != null)
				{
					paragraph.Dispose();
				}
				this.Paragraph = null;
			}
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00014960 File Offset: 0x00012B60
		private void Initialize()
		{
			if (this.Paragraph != null && !this.RebuildParagraphForEveryPage)
			{
				return;
			}
			if (!this.AreParagraphItemsTransformedWithSpacingAndIndentation)
			{
				this.Items = Enumerable.ToList<ITextBlockItem>(this.ApplyParagraphSpacingToTextBlockItems());
				this.AreParagraphItemsTransformedWithSpacingAndIndentation = true;
			}
			this.RebuildParagraphForEveryPage = Enumerable.Any<ITextBlockItem>(this.Items, (ITextBlockItem x) => x is TextBlockPageNumber);
			this.BuildParagraph();
			this.AreParagraphMetricsValid = false;
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x000149DC File Offset: 0x00012BDC
		private void BuildParagraph()
		{
			TextHorizontalAlignment? alignment = this.Alignment;
			TextHorizontalAlignment textHorizontalAlignment = alignment.GetValueOrDefault();
			if (alignment == null)
			{
				textHorizontalAlignment = TextHorizontalAlignment.Start;
				TextHorizontalAlignment? alignment2 = new TextHorizontalAlignment?(textHorizontalAlignment);
				this.Alignment = alignment2;
			}
			ParagraphStyle paragraphStyle = new ParagraphStyle();
			paragraphStyle.Alignment = TextBlock.<BuildParagraph>g__MapAlignment|79_0(this.Alignment.Value);
			paragraphStyle.Direction = TextBlock.<BuildParagraph>g__MapDirection|79_1(this.ContentDirection);
			paragraphStyle.MaxLinesVisible = this.LineClamp.GetValueOrDefault(1000000);
			paragraphStyle.LineClampEllipsis = this.LineClampEllipsis;
			if (this.Paragraph != null)
			{
				this.Paragraph.Dispose();
				this.Paragraph = null;
			}
			SkParagraphBuilder builder = SkParagraphBuilderPoolManager.Get(paragraphStyle);
			try
			{
				this.Paragraph = this.<BuildParagraph>g__CreateParagraph|79_3(builder);
			}
			finally
			{
				SkParagraphBuilderPoolManager.Return(builder);
			}
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00014AB0 File Offset: 0x00012CB0
		private IEnumerable<ITextBlockItem> ApplyParagraphSpacingToTextBlockItems()
		{
			TextBlock.<>c__DisplayClass80_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			if (this.ParagraphSpacing < 0.01f && this.ParagraphFirstLineIndentation < 0.01f)
			{
				return this.Items;
			}
			CS$<>8__locals1.result = new List<ITextBlockItem>();
			this.<ApplyParagraphSpacingToTextBlockItems>g__AddParagraphFirstLineIndentation|80_2(ref CS$<>8__locals1);
			foreach (ITextBlockItem textBlockItem in this.Items)
			{
				TextBlockSpan textBlockSpan = textBlockItem as TextBlockSpan;
				if (textBlockSpan == null)
				{
					CS$<>8__locals1.result.Add(textBlockItem);
				}
				else if (textBlockItem is TextBlockPageNumber)
				{
					CS$<>8__locals1.result.Add(textBlockItem);
				}
				else if (textBlockSpan.Text == "\n")
				{
					this.<ApplyParagraphSpacingToTextBlockItems>g__AddParagraphSpacing|80_1(ref CS$<>8__locals1);
					this.<ApplyParagraphSpacingToTextBlockItems>g__AddParagraphFirstLineIndentation|80_2(ref CS$<>8__locals1);
				}
				else
				{
					string[] array = textBlockSpan.Text.Split('\n', 0);
					foreach (string text in array)
					{
						this.<ApplyParagraphSpacingToTextBlockItems>g__AddClonedTextBlockSpanWithTextFragment|80_0(textBlockSpan, text, ref CS$<>8__locals1);
						if (!(text == Enumerable.Last<string>(array)))
						{
							this.<ApplyParagraphSpacingToTextBlockItems>g__AddParagraphSpacing|80_1(ref CS$<>8__locals1);
							this.<ApplyParagraphSpacingToTextBlockItems>g__AddParagraphFirstLineIndentation|80_2(ref CS$<>8__locals1);
						}
					}
				}
			}
			return CS$<>8__locals1.result;
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x00014BF4 File Offset: 0x00012DF4
		private void CalculateParagraphMetrics(Size availableSpace)
		{
			if (Math.Abs(this.WidthForLineMetricsCalculation - availableSpace.Width) > 0.01f)
			{
				this.AreParagraphMetricsValid = false;
			}
			if (this.AreParagraphMetricsValid)
			{
				return;
			}
			this.WidthForLineMetricsCalculation = availableSpace.Width;
			object planLayoutLock = TextBlock.PlanLayoutLock;
			lock (planLayoutLock)
			{
				this.Paragraph.PlanLayout(availableSpace.Width);
			}
			this.CheckUnresolvedGlyphs();
			this.LineMetrics = this.Paragraph.GetLineMetrics();
			this.PlaceholderPositions = this.Paragraph.GetPlaceholderPositions();
			float maximumWidth;
			if (!Enumerable.Any<SkSize>(this.LineMetrics))
			{
				maximumWidth = 0f;
			}
			else
			{
				maximumWidth = Enumerable.Max<SkSize>(this.LineMetrics, (SkSize x) => x.Width);
			}
			this.MaximumWidth = maximumWidth;
			this.AreParagraphMetricsValid = true;
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x00014CE8 File Offset: 0x00012EE8
		private void CheckUnresolvedGlyphs()
		{
			if (!Settings.CheckIfAllTextGlyphsAreAvailable)
			{
				return;
			}
			int[] unresolvedCodepoints = this.Paragraph.GetUnresolvedCodepoints();
			if (!Enumerable.Any<int>(unresolvedCodepoints))
			{
				return;
			}
			IEnumerable<string> enumerable = Enumerable.Select<int, string>(unresolvedCodepoints, delegate(int codepoint)
			{
				string text2 = char.ConvertFromUtf32(codepoint);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(5, 2);
				defaultInterpolatedStringHandler2.AppendLiteral("U-");
				defaultInterpolatedStringHandler2.AppendFormatted<int>(codepoint, "X4");
				defaultInterpolatedStringHandler2.AppendLiteral(" '");
				defaultInterpolatedStringHandler2.AppendFormatted(text2);
				defaultInterpolatedStringHandler2.AppendLiteral("'");
				return defaultInterpolatedStringHandler2.ToStringAndClear();
			});
			string text = string.Join("\n", enumerable);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(549, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Could not find an appropriate font fallback for the following glyphs: \n");
			defaultInterpolatedStringHandler.AppendLiteral("$");
			defaultInterpolatedStringHandler.AppendFormatted(text);
			defaultInterpolatedStringHandler.AppendLiteral(" \n\n");
			defaultInterpolatedStringHandler.AppendLiteral("Possible solutions: \n");
			defaultInterpolatedStringHandler.AppendLiteral("1) Install fonts that contain missing glyphs in your runtime environment. \n");
			defaultInterpolatedStringHandler.AppendLiteral("2) Configure the fallback TextStyle using the 'TextStyle.FontFamilyFallback' method. \n");
			defaultInterpolatedStringHandler.AppendLiteral("3) Register additional application specific fonts using the 'FontManager.RegisterFont' method. \n\n");
			defaultInterpolatedStringHandler.AppendLiteral("You can disable this check by setting the 'Settings.CheckIfAllTextGlyphsAreAvailable' option to 'false'. \n");
			defaultInterpolatedStringHandler.AppendLiteral("However, this may result with text glyphs being incorrectly rendered without any warning.");
			throw new DocumentDrawingException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000886 RID: 2182 RVA: 0x00014DD1 File Offset: 0x00012FD1
		private static ConcurrentDictionary<int, float> ParagraphContainingOnlyWhiteSpaceHeightCache { get; } = new ConcurrentDictionary<int, float>();

		// Token: 0x06000887 RID: 2183 RVA: 0x00014DD8 File Offset: 0x00012FD8
		private bool CheckIfContainsOnlyWhiteSpace()
		{
			foreach (ITextBlockItem textBlockItem in this.Items)
			{
				if (textBlockItem is TextBlockPageNumber)
				{
					return false;
				}
				TextBlockSpan textBlockSpan = textBlockItem as TextBlockSpan;
				if (textBlockSpan != null && !string.IsNullOrWhiteSpace(textBlockSpan.Text))
				{
					return false;
				}
				if (textBlockItem is TextBlockElement)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x00014E5C File Offset: 0x0001305C
		private float MeasureHeightOfParagraphContainingOnlyWhiteSpace()
		{
			return Enumerable.Max(Enumerable.DefaultIfEmpty<float>(Enumerable.Select<TextBlockSpan, float>(Enumerable.OfType<TextBlockSpan>(this.Items), delegate(TextBlockSpan x)
			{
				ConcurrentDictionary<int, float> paragraphContainingOnlyWhiteSpaceHeightCache = TextBlock.ParagraphContainingOnlyWhiteSpaceHeightCache;
				int id = x.Style.Id;
				Func<int, float> func;
				if ((func = TextBlock.<>O.<0>__Measure) == null)
				{
					func = (TextBlock.<>O.<0>__Measure = new Func<int, float>(TextBlock.<MeasureHeightOfParagraphContainingOnlyWhiteSpace>g__Measure|88_1));
				}
				return paragraphContainingOnlyWhiteSpaceHeightCache.GetOrAdd(id, func);
			}), 0f));
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000889 RID: 2185 RVA: 0x00014E9C File Offset: 0x0001309C
		// (set) Token: 0x0600088A RID: 2186 RVA: 0x00014EA4 File Offset: 0x000130A4
		private bool IsRendered { get; set; }

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x0600088B RID: 2187 RVA: 0x00014EAD File Offset: 0x000130AD
		// (set) Token: 0x0600088C RID: 2188 RVA: 0x00014EB5 File Offset: 0x000130B5
		private int CurrentLineIndex { get; set; }

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x0600088D RID: 2189 RVA: 0x00014EBE File Offset: 0x000130BE
		// (set) Token: 0x0600088E RID: 2190 RVA: 0x00014EC6 File Offset: 0x000130C6
		private float CurrentTopOffset { get; set; }

		// Token: 0x0600088F RID: 2191 RVA: 0x00014ECF File Offset: 0x000130CF
		public void ResetState(bool hardReset = false)
		{
			this.IsRendered = false;
			this.CurrentLineIndex = 0;
			this.CurrentTopOffset = 0f;
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x00014EEC File Offset: 0x000130EC
		public object GetState()
		{
			return new TextBlock.TextBlockState
			{
				IsRendered = this.IsRendered,
				CurrentLineIndex = this.CurrentLineIndex,
				CurrentTopOffset = this.CurrentTopOffset
			};
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x00014F30 File Offset: 0x00013130
		public void SetState(object state)
		{
			TextBlock.TextBlockState textBlockState = (TextBlock.TextBlockState)state;
			this.IsRendered = textBlockState.IsRendered;
			this.CurrentLineIndex = textBlockState.CurrentLineIndex;
			this.CurrentTopOffset = textBlockState.CurrentTopOffset;
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x00014F68 File Offset: 0x00013168
		[NullableContext(2)]
		internal override string GetCompanionHint()
		{
			return this.Text.Substring(0, Math.Min(this.Text.Length, 50));
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x00014F88 File Offset: 0x00013188
		[NullableContext(2)]
		internal override string GetCompanionSearchableContent()
		{
			return this.Text;
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x00014FCC File Offset: 0x000131CC
		[NullableContext(0)]
		[CompilerGenerated]
		[return: TupleElementNames(new string[]
		{
			"linesToDraw",
			"takenHeight"
		})]
		private ValueTuple<int, float> <Draw>g__DetermineLinesToDraw|77_0(ref TextBlock.<>c__DisplayClass77_0 A_1)
		{
			int num = 0;
			float num2 = 0f;
			for (int i = this.CurrentLineIndex; i < this.LineMetrics.Length; i++)
			{
				SkSize skSize = this.LineMetrics[i];
				float num3 = num2 + skSize.Height;
				if (num3 > A_1.availableSpace.Height + 0.01f)
				{
					break;
				}
				num2 = num3;
				num++;
			}
			return new ValueTuple<int, float>(num, num2);
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x00015034 File Offset: 0x00013234
		[CompilerGenerated]
		private void <Draw>g__DrawParagraph|77_1(ref TextBlock.<>c__DisplayClass77_0 A_1)
		{
			bool flag = A_1.linesToDraw != this.LineMetrics.Length;
			if (flag)
			{
				base.Canvas.Save();
				base.Canvas.Translate(new Position(0f, -this.CurrentTopOffset));
			}
			base.Canvas.DrawParagraph(this.Paragraph, this.CurrentLineIndex, this.CurrentLineIndex + A_1.linesToDraw - 1);
			if (flag)
			{
				base.Canvas.ClipRectangle(new SkRect(0f, this.CurrentTopOffset, A_1.availableSpace.Width, A_1.takenHeight + this.CurrentTopOffset));
			}
			this.<Draw>g__DrawInjectedElements|77_2(ref A_1);
			this.<Draw>g__DrawHyperlinks|77_3(ref A_1);
			this.<Draw>g__DrawSectionLinks|77_4(ref A_1);
			if (flag)
			{
				base.Canvas.Restore();
			}
		}

		// Token: 0x06000898 RID: 2200 RVA: 0x000150FC File Offset: 0x000132FC
		[CompilerGenerated]
		private void <Draw>g__DrawInjectedElements|77_2(ref TextBlock.<>c__DisplayClass77_0 A_1)
		{
			foreach (TextBlockElement textBlockElement in Enumerable.OfType<TextBlockElement>(this.Items))
			{
				SkRect skRect = this.PlaceholderPositions[textBlockElement.ParagraphBlockIndex];
				textBlockElement.ConfigureElement(base.PageContext, base.Canvas);
				Position position = new Position(skRect.Left, skRect.Top);
				if (this.<Draw>g__IsPositionVisible|77_5(position, ref A_1))
				{
					base.Canvas.Translate(position);
					textBlockElement.Element.Draw(new Size(skRect.Width, skRect.Height));
					base.Canvas.Translate(position.Reverse());
				}
			}
		}

		// Token: 0x06000899 RID: 2201 RVA: 0x000151CC File Offset: 0x000133CC
		[CompilerGenerated]
		private void <Draw>g__DrawHyperlinks|77_3(ref TextBlock.<>c__DisplayClass77_0 A_1)
		{
			foreach (TextBlockHyperlink textBlockHyperlink in Enumerable.OfType<TextBlockHyperlink>(this.Items))
			{
				foreach (SkRect skRect in this.Paragraph.GetTextRangePositions(textBlockHyperlink.ParagraphBeginIndex, textBlockHyperlink.ParagraphBeginIndex + textBlockHyperlink.Text.Length))
				{
					Position position = new Position(skRect.Left, skRect.Top);
					if (this.<Draw>g__IsPositionVisible|77_5(position, ref A_1))
					{
						base.Canvas.Translate(position);
						base.Canvas.DrawHyperlink(new Size(skRect.Width, skRect.Height), textBlockHyperlink.Url, textBlockHyperlink.Text);
						base.Canvas.Translate(position.Reverse());
					}
				}
			}
		}

		// Token: 0x0600089A RID: 2202 RVA: 0x000152C4 File Offset: 0x000134C4
		[CompilerGenerated]
		private void <Draw>g__DrawSectionLinks|77_4(ref TextBlock.<>c__DisplayClass77_0 A_1)
		{
			foreach (TextBlockSectionLink textBlockSectionLink in Enumerable.OfType<TextBlockSectionLink>(this.Items))
			{
				SkRect[] textRangePositions = this.Paragraph.GetTextRangePositions(textBlockSectionLink.ParagraphBeginIndex, textBlockSectionLink.ParagraphBeginIndex + textBlockSectionLink.Text.Length);
				string documentLocationName = base.PageContext.GetDocumentLocationName(textBlockSectionLink.SectionName);
				foreach (SkRect skRect in textRangePositions)
				{
					Position position = new Position(skRect.Left, skRect.Top);
					if (this.<Draw>g__IsPositionVisible|77_5(position, ref A_1))
					{
						base.Canvas.Translate(position);
						base.Canvas.DrawSectionLink(new Size(skRect.Width, skRect.Height), documentLocationName, textBlockSectionLink.Text);
						base.Canvas.Translate(position.Reverse());
					}
				}
			}
		}

		// Token: 0x0600089B RID: 2203 RVA: 0x000153CC File Offset: 0x000135CC
		[CompilerGenerated]
		private bool <Draw>g__IsPositionVisible|77_5(Position position, ref TextBlock.<>c__DisplayClass77_0 A_2)
		{
			return this.CurrentTopOffset <= position.Y || position.Y <= this.CurrentTopOffset + A_2.takenHeight;
		}

		// Token: 0x0600089C RID: 2204 RVA: 0x000153F8 File Offset: 0x000135F8
		[CompilerGenerated]
		internal static ParagraphStyleConfiguration.TextAlign <BuildParagraph>g__MapAlignment|79_0(TextHorizontalAlignment alignment)
		{
			ParagraphStyleConfiguration.TextAlign result;
			switch (alignment)
			{
			case TextHorizontalAlignment.Left:
				result = ParagraphStyleConfiguration.TextAlign.Left;
				break;
			case TextHorizontalAlignment.Center:
				result = ParagraphStyleConfiguration.TextAlign.Center;
				break;
			case TextHorizontalAlignment.Right:
				result = ParagraphStyleConfiguration.TextAlign.Right;
				break;
			case TextHorizontalAlignment.Justify:
				result = ParagraphStyleConfiguration.TextAlign.Justify;
				break;
			case TextHorizontalAlignment.Start:
				result = ParagraphStyleConfiguration.TextAlign.Start;
				break;
			case TextHorizontalAlignment.End:
				result = ParagraphStyleConfiguration.TextAlign.End;
				break;
			default:
				throw new Exception();
			}
			return result;
		}

		// Token: 0x0600089D RID: 2205 RVA: 0x00015444 File Offset: 0x00013644
		[CompilerGenerated]
		internal static ParagraphStyleConfiguration.TextDirection <BuildParagraph>g__MapDirection|79_1(ContentDirection direction)
		{
			ParagraphStyleConfiguration.TextDirection result;
			if (direction != ContentDirection.LeftToRight)
			{
				if (direction != ContentDirection.RightToLeft)
				{
					throw new Exception();
				}
				result = ParagraphStyleConfiguration.TextDirection.Rtl;
			}
			else
			{
				result = ParagraphStyleConfiguration.TextDirection.Ltr;
			}
			return result;
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x0001546C File Offset: 0x0001366C
		[CompilerGenerated]
		internal static SkPlaceholderStyle.PlaceholderAlignment <BuildParagraph>g__MapInjectedTextAlignment|79_2(TextInjectedElementAlignment alignment)
		{
			SkPlaceholderStyle.PlaceholderAlignment result;
			switch (alignment)
			{
			case TextInjectedElementAlignment.AboveBaseline:
				result = SkPlaceholderStyle.PlaceholderAlignment.AboveBaseline;
				break;
			case TextInjectedElementAlignment.BelowBaseline:
				result = SkPlaceholderStyle.PlaceholderAlignment.BelowBaseline;
				break;
			case TextInjectedElementAlignment.Top:
				result = SkPlaceholderStyle.PlaceholderAlignment.Top;
				break;
			case TextInjectedElementAlignment.Bottom:
				result = SkPlaceholderStyle.PlaceholderAlignment.Bottom;
				break;
			case TextInjectedElementAlignment.Middle:
				result = SkPlaceholderStyle.PlaceholderAlignment.Middle;
				break;
			default:
				throw new Exception();
			}
			return result;
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x000154B0 File Offset: 0x000136B0
		[CompilerGenerated]
		private SkParagraph <BuildParagraph>g__CreateParagraph|79_3(SkParagraphBuilder builder)
		{
			int num = 0;
			int num2 = 0;
			if (!Enumerable.Any<ITextBlockItem>(this.Items, (ITextBlockItem x) => x is TextBlockSpan))
			{
				builder.AddText("​", this.DefaultTextStyle.GetSkTextStyle());
			}
			foreach (ITextBlockItem textBlockItem in this.Items)
			{
				TextBlockSpan textBlockSpan = textBlockItem as TextBlockSpan;
				if (textBlockSpan != null)
				{
					TextBlockSectionLink textBlockSectionLink = textBlockItem as TextBlockSectionLink;
					if (textBlockSectionLink != null)
					{
						textBlockSectionLink.ParagraphBeginIndex = num;
					}
					else
					{
						TextBlockHyperlink textBlockHyperlink = textBlockItem as TextBlockHyperlink;
						if (textBlockHyperlink != null)
						{
							textBlockHyperlink.ParagraphBeginIndex = num;
						}
						else
						{
							TextBlockPageNumber textBlockPageNumber = textBlockItem as TextBlockPageNumber;
							if (textBlockPageNumber != null)
							{
								textBlockPageNumber.UpdatePageNumberText(base.PageContext);
							}
						}
					}
					SkTextStyle skTextStyle = textBlockSpan.Style.GetSkTextStyle();
					string text = textBlockSpan.Text;
					string text2 = ((text != null) ? text.Replace("\r", "") : null) ?? "";
					builder.AddText(text2, skTextStyle);
					num += text2.Length;
				}
				else
				{
					TextBlockElement textBlockElement = textBlockItem as TextBlockElement;
					if (textBlockElement != null)
					{
						textBlockElement.ConfigureElement(base.PageContext, base.Canvas);
						textBlockElement.UpdateElementSize();
						textBlockElement.ParagraphBlockIndex = num2;
						builder.AddPlaceholder(new SkPlaceholderStyle
						{
							Width = textBlockElement.ElementSize.Width,
							Height = textBlockElement.ElementSize.Height,
							Alignment = TextBlock.<BuildParagraph>g__MapInjectedTextAlignment|79_2(textBlockElement.Alignment),
							Baseline = SkPlaceholderStyle.PlaceholderBaseline.Alphabetic,
							BaselineOffset = 0f
						});
						num++;
						num2++;
					}
					else
					{
						TextBlockParagraphSpacing textBlockParagraphSpacing = textBlockItem as TextBlockParagraphSpacing;
						if (textBlockParagraphSpacing != null)
						{
							builder.AddPlaceholder(new SkPlaceholderStyle
							{
								Width = textBlockParagraphSpacing.Width,
								Height = textBlockParagraphSpacing.Height,
								Alignment = SkPlaceholderStyle.PlaceholderAlignment.Middle,
								Baseline = SkPlaceholderStyle.PlaceholderBaseline.Alphabetic,
								BaselineOffset = 0f
							});
							num++;
							num2++;
						}
					}
				}
			}
			return builder.CreateParagraph();
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x000156F0 File Offset: 0x000138F0
		[CompilerGenerated]
		private void <ApplyParagraphSpacingToTextBlockItems>g__AddClonedTextBlockSpanWithTextFragment|80_0(TextBlockSpan originalSpan, string textFragment, ref TextBlock.<>c__DisplayClass80_0 A_3)
		{
			TextBlockSectionLink textBlockSectionLink = originalSpan as TextBlockSectionLink;
			TextBlockSpan textBlockSpan;
			if (textBlockSectionLink != null)
			{
				textBlockSpan = new TextBlockSectionLink
				{
					SectionName = textBlockSectionLink.SectionName
				};
			}
			else
			{
				TextBlockHyperlink textBlockHyperlink = originalSpan as TextBlockHyperlink;
				if (textBlockHyperlink != null)
				{
					textBlockSpan = new TextBlockHyperlink
					{
						Url = textBlockHyperlink.Url
					};
				}
				else
				{
					TextBlockPageNumber textBlockPageNumber = originalSpan as TextBlockPageNumber;
					if (textBlockPageNumber != null)
					{
						textBlockSpan = textBlockPageNumber;
					}
					else
					{
						textBlockSpan = new TextBlockSpan();
					}
				}
			}
			textBlockSpan.Text = textFragment;
			textBlockSpan.Style = originalSpan.Style;
			A_3.result.Add(textBlockSpan);
		}

		// Token: 0x060008A1 RID: 2209 RVA: 0x0001576C File Offset: 0x0001396C
		[CompilerGenerated]
		private void <ApplyParagraphSpacingToTextBlockItems>g__AddParagraphSpacing|80_1(ref TextBlock.<>c__DisplayClass80_0 A_1)
		{
			if (this.ParagraphSpacing <= 0.01f)
			{
				return;
			}
			A_1.result.Add(new TextBlockSpan
			{
				Text = "\n ",
				Style = TextStyle.ParagraphSpacing
			});
			A_1.result.Add(new TextBlockParagraphSpacing(0f, this.ParagraphSpacing));
			A_1.result.Add(new TextBlockSpan
			{
				Text = " \n",
				Style = TextStyle.ParagraphSpacing
			});
		}

		// Token: 0x060008A2 RID: 2210 RVA: 0x000157F0 File Offset: 0x000139F0
		[CompilerGenerated]
		private void <ApplyParagraphSpacingToTextBlockItems>g__AddParagraphFirstLineIndentation|80_2(ref TextBlock.<>c__DisplayClass80_0 A_1)
		{
			if (this.ParagraphFirstLineIndentation <= 0.01f)
			{
				return;
			}
			A_1.result.Add(new TextBlockSpan
			{
				Text = "\n",
				Style = TextStyle.ParagraphSpacing
			});
			A_1.result.Add(new TextBlockParagraphSpacing(this.ParagraphFirstLineIndentation, 0f));
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x0001584C File Offset: 0x00013A4C
		[CompilerGenerated]
		internal static float <MeasureHeightOfParagraphContainingOnlyWhiteSpace>g__Measure|88_1(int textStyleId)
		{
			SkParagraphBuilder skParagraphBuilder = SkParagraphBuilderPoolManager.Get(new ParagraphStyle
			{
				Alignment = ParagraphStyleConfiguration.TextAlign.Start,
				Direction = ParagraphStyleConfiguration.TextDirection.Ltr,
				MaxLinesVisible = 1000000,
				LineClampEllipsis = string.Empty
			});
			float height;
			try
			{
				SkTextStyle skTextStyle = TextStyleManager.GetTextStyle(textStyleId).GetSkTextStyle();
				skParagraphBuilder.AddText("\u00a0", skTextStyle);
				using (SkParagraph skParagraph = skParagraphBuilder.CreateParagraph())
				{
					skParagraph.PlanLayout(1000f);
					height = Enumerable.First<SkSize>(skParagraph.GetLineMetrics()).Height;
				}
			}
			finally
			{
				SkParagraphBuilderPoolManager.Return(skParagraphBuilder);
			}
			return height;
		}

		// Token: 0x0400039B RID: 923
		private static readonly object PlanLayoutLock = new object();

		// Token: 0x020001D0 RID: 464
		[NullableContext(0)]
		public struct TextBlockState
		{
			// Token: 0x040006CB RID: 1739
			public bool IsRendered;

			// Token: 0x040006CC RID: 1740
			public int CurrentLineIndex;

			// Token: 0x040006CD RID: 1741
			public float CurrentTopOffset;
		}

		// Token: 0x020001D1 RID: 465
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x040006CE RID: 1742
			[Nullable(0)]
			public static Func<int, float> <0>__Measure;
		}
	}
}
