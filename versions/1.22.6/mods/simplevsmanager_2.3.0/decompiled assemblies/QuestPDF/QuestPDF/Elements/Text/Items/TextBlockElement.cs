using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements.Text.Items
{
	// Token: 0x020000FE RID: 254
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class TextBlockElement : ITextBlockItem
	{
		// Token: 0x1700020F RID: 527
		// (get) Token: 0x060008A4 RID: 2212 RVA: 0x000158F4 File Offset: 0x00013AF4
		// (set) Token: 0x060008A5 RID: 2213 RVA: 0x000158FC File Offset: 0x00013AFC
		public Element Element { get; set; } = Empty.Instance;

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x060008A6 RID: 2214 RVA: 0x00015905 File Offset: 0x00013B05
		// (set) Token: 0x060008A7 RID: 2215 RVA: 0x0001590D File Offset: 0x00013B0D
		public Size ElementSize { get; set; } = Size.Zero;

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x060008A8 RID: 2216 RVA: 0x00015916 File Offset: 0x00013B16
		// (set) Token: 0x060008A9 RID: 2217 RVA: 0x0001591E File Offset: 0x00013B1E
		public TextInjectedElementAlignment Alignment { get; set; }

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x00015927 File Offset: 0x00013B27
		// (set) Token: 0x060008AB RID: 2219 RVA: 0x0001592F File Offset: 0x00013B2F
		public int ParagraphBlockIndex { get; set; }

		// Token: 0x060008AC RID: 2220 RVA: 0x00015938 File Offset: 0x00013B38
		public void ConfigureElement(IPageContext pageContext, IDrawingCanvas canvas)
		{
			this.Element.VisitChildren(delegate(Element x)
			{
				IStateful stateful = x as IStateful;
				if (stateful == null)
				{
					return;
				}
				stateful.ResetState(true);
			});
			this.Element.InjectDependencies(pageContext, canvas);
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x00015971 File Offset: 0x00013B71
		public void UpdateElementSize()
		{
			this.ElementSize = this.Element.Measure(Size.Max);
		}
	}
}
