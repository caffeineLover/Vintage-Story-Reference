using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000F7 RID: 247
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class SvgPath : Element, IStateful
	{
		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x06000836 RID: 2102 RVA: 0x0001411E File Offset: 0x0001231E
		// (set) Token: 0x06000837 RID: 2103 RVA: 0x00014126 File Offset: 0x00012326
		public string Path { get; set; } = string.Empty;

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x06000838 RID: 2104 RVA: 0x0001412F File Offset: 0x0001232F
		// (set) Token: 0x06000839 RID: 2105 RVA: 0x00014137 File Offset: 0x00012337
		public Color FillColor { get; set; } = Colors.Black;

		// Token: 0x0600083A RID: 2106 RVA: 0x00014140 File Offset: 0x00012340
		internal override SpacePlan Measure(Size availableSpace)
		{
			if (this.IsRendered)
			{
				return SpacePlan.Empty();
			}
			if (availableSpace.IsNegative())
			{
				return SpacePlan.Wrap("The available space is negative.");
			}
			return SpacePlan.FullRender(Size.Zero);
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x0001416D File Offset: 0x0001236D
		internal override void Draw(Size availableSpace)
		{
			if (this.IsRendered)
			{
				return;
			}
			base.Canvas.DrawSvgPath(this.Path, this.FillColor);
			this.IsRendered = true;
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x0600083C RID: 2108 RVA: 0x00014196 File Offset: 0x00012396
		// (set) Token: 0x0600083D RID: 2109 RVA: 0x0001419E File Offset: 0x0001239E
		private bool IsRendered { get; set; }

		// Token: 0x0600083E RID: 2110 RVA: 0x000141A7 File Offset: 0x000123A7
		public void ResetState(bool hardReset = false)
		{
			this.IsRendered = false;
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x000141B0 File Offset: 0x000123B0
		public object GetState()
		{
			return this.IsRendered;
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x000141BD File Offset: 0x000123BD
		public void SetState(object state)
		{
			this.IsRendered = (bool)state;
		}
	}
}
