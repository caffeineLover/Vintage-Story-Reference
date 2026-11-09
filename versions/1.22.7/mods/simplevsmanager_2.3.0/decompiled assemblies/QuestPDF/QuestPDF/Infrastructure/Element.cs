using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;

namespace QuestPDF.Infrastructure
{
	// Token: 0x02000040 RID: 64
	[NullableContext(1)]
	[Nullable(0)]
	internal abstract class Element : IElement
	{
		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600019C RID: 412 RVA: 0x00004E93 File Offset: 0x00003093
		// (set) Token: 0x0600019D RID: 413 RVA: 0x00004E9B File Offset: 0x0000309B
		internal IPageContext PageContext { get; set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600019E RID: 414 RVA: 0x00004EA4 File Offset: 0x000030A4
		// (set) Token: 0x0600019F RID: 415 RVA: 0x00004EAC File Offset: 0x000030AC
		internal IDrawingCanvas Canvas { get; set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x00004EB5 File Offset: 0x000030B5
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x00004EBD File Offset: 0x000030BD
		internal SourceCodePath? CodeLocation { get; set; }

		// Token: 0x060001A2 RID: 418 RVA: 0x00004EC6 File Offset: 0x000030C6
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		internal virtual IEnumerable<Element> GetChildren()
		{
			return new Element.<GetChildren>d__12(-2);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00004ECF File Offset: 0x000030CF
		internal virtual void CreateProxy([Nullable(new byte[]
		{
			1,
			2,
			2
		})] Func<Element, Element> create)
		{
		}

		// Token: 0x060001A4 RID: 420
		internal abstract SpacePlan Measure(Size availableSpace);

		// Token: 0x060001A5 RID: 421
		internal abstract void Draw(Size availableSpace);

		// Token: 0x060001A6 RID: 422 RVA: 0x00004ED1 File Offset: 0x000030D1
		[NullableContext(2)]
		internal virtual string GetCompanionHint()
		{
			return null;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00004ED4 File Offset: 0x000030D4
		[NullableContext(2)]
		internal virtual string GetCompanionSearchableContent()
		{
			return null;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00004ED7 File Offset: 0x000030D7
		[return: Nullable(new byte[]
		{
			2,
			0,
			1,
			1
		})]
		internal virtual IEnumerable<KeyValuePair<string, string>> GetCompanionProperties()
		{
			return null;
		}
	}
}
