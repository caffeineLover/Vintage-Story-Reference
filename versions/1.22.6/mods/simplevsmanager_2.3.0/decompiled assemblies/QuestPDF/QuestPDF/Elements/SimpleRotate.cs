using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000F1 RID: 241
	internal sealed class SimpleRotate : ContainerElement
	{
		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x000130C1 File Offset: 0x000112C1
		// (set) Token: 0x060007DA RID: 2010 RVA: 0x000130C9 File Offset: 0x000112C9
		public int TurnCount { get; set; }

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x000130D2 File Offset: 0x000112D2
		public int NormalizedTurnCount
		{
			get
			{
				return (this.TurnCount % 4 + 4) % 4;
			}
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x000130E0 File Offset: 0x000112E0
		internal override SpacePlan Measure(Size availableSpace)
		{
			if (this.NormalizedTurnCount == 0 || this.NormalizedTurnCount == 2)
			{
				return base.Measure(availableSpace);
			}
			availableSpace = new Size(availableSpace.Height, availableSpace.Width);
			SpacePlan spacePlan = base.Measure(availableSpace);
			SpacePlanType type = spacePlan.Type;
			bool flag = type <= SpacePlanType.Wrap;
			if (flag)
			{
				return spacePlan;
			}
			Size size = new Size(spacePlan.Height, spacePlan.Width);
			if (spacePlan.Type == SpacePlanType.FullRender)
			{
				return SpacePlan.FullRender(size);
			}
			if (spacePlan.Type == SpacePlanType.PartialRender)
			{
				return SpacePlan.PartialRender(size);
			}
			throw new ArgumentException();
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x00013170 File Offset: 0x00011370
		internal override void Draw(Size availableSpace)
		{
			Position vector = new Position((this.NormalizedTurnCount == 1 || this.NormalizedTurnCount == 2) ? availableSpace.Width : 0f, (this.NormalizedTurnCount == 2 || this.NormalizedTurnCount == 3) ? availableSpace.Height : 0f);
			int num = this.NormalizedTurnCount * 90;
			base.Canvas.Translate(vector);
			base.Canvas.Rotate((float)num);
			if (this.NormalizedTurnCount == 1 || this.NormalizedTurnCount == 3)
			{
				availableSpace = new Size(availableSpace.Height, availableSpace.Width);
			}
			Element child = base.Child;
			if (child != null)
			{
				child.Draw(availableSpace);
			}
			base.Canvas.Rotate((float)(-(float)num));
			base.Canvas.Translate(vector.Reverse());
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x0001323C File Offset: 0x0001143C
		[NullableContext(2)]
		internal override string GetCompanionHint()
		{
			if (this.TurnCount == 0)
			{
				return "No rotation";
			}
			int num = Math.Abs(this.TurnCount) * 90;
			string text = (this.TurnCount > 0) ? "clockwise" : "counter-clockwise";
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(5, 2);
			defaultInterpolatedStringHandler.AppendFormatted<int>(num);
			defaultInterpolatedStringHandler.AppendLiteral(" deg ");
			defaultInterpolatedStringHandler.AppendFormatted(text);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
