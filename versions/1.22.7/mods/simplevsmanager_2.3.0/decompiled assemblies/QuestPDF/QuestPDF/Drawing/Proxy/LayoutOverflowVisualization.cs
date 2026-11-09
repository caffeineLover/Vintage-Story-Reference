using System;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Skia;

namespace QuestPDF.Drawing.Proxy
{
	// Token: 0x02000116 RID: 278
	internal sealed class LayoutOverflowVisualization : ElementProxy, IContentDirectionAware
	{
		// Token: 0x17000250 RID: 592
		// (get) Token: 0x060009A0 RID: 2464 RVA: 0x000195DC File Offset: 0x000177DC
		// (set) Token: 0x060009A1 RID: 2465 RVA: 0x000195E4 File Offset: 0x000177E4
		public ContentDirection ContentDirection { get; set; }

		// Token: 0x060009A2 RID: 2466 RVA: 0x000195F0 File Offset: 0x000177F0
		internal override SpacePlan Measure(Size availableSpace)
		{
			if (Size.Equal(availableSpace, Size.Zero))
			{
				return SpacePlan.Wrap("There is no available space.");
			}
			SpacePlan spacePlan = base.Measure(availableSpace);
			if (spacePlan.Type == SpacePlanType.FullRender)
			{
				return spacePlan;
			}
			SpacePlan spacePlan2 = base.Child.TryMeasureWithOverflow(availableSpace);
			if (spacePlan2.Type == SpacePlanType.Wrap)
			{
				return spacePlan2;
			}
			float width = Math.Min(availableSpace.Width, spacePlan2.Width);
			float height = Math.Min(availableSpace.Height, spacePlan2.Height);
			return new SpacePlan(spacePlan2.Type, width, height, null);
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x00019674 File Offset: 0x00017874
		internal override void Draw(Size availableSpace)
		{
			SpacePlanType type = base.Measure(availableSpace).Type;
			bool flag = type == SpacePlanType.Empty || type == SpacePlanType.FullRender;
			if (!flag)
			{
				base.Canvas = base.Child.Canvas;
				SpacePlan spacePlan = base.Child.TryMeasureWithOverflow(availableSpace);
				Size size = (spacePlan.Type == SpacePlanType.Wrap) ? Size.Max : spacePlan;
				Position vector = (this.ContentDirection == ContentDirection.RightToLeft) ? new Position(availableSpace.Width - size.Width, 0f) : Position.Zero;
				base.Canvas.Translate(vector);
				Element child = base.Child;
				if (child != null)
				{
					child.Draw(size);
				}
				base.Canvas.Translate(vector.Reverse());
				Position vector2 = (this.ContentDirection == ContentDirection.RightToLeft) ? new Position(availableSpace.Width, 0f) : Position.Zero;
				int num = (this.ContentDirection == ContentDirection.RightToLeft) ? -1 : 1;
				base.Canvas.Translate(vector2);
				base.Canvas.Scale((float)num, 1f);
				this.DrawOverflowArea(availableSpace, size);
				base.Canvas.Scale((float)num, 1f);
				base.Canvas.Translate(vector2.Reverse());
				return;
			}
			Element child2 = base.Child;
			if (child2 == null)
			{
				return;
			}
			child2.Draw(availableSpace);
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x000197C0 File Offset: 0x000179C0
		private void DrawOverflowArea(Size availableSpace, Size contentSize)
		{
			using (SkPaint skPaint = new SkPaint())
			{
				skPaint.SetSolidColor(this.AvailableAreaColor.WithAlpha(64));
				base.Canvas.DrawRectangle(Position.Zero, availableSpace, skPaint);
				base.Canvas.Save();
				base.Canvas.ClipOverflowArea(new SkRect(0f, 0f, availableSpace.Width, availableSpace.Height), new SkRect(0f, 0f, contentSize.Width, contentSize.Height));
				base.Canvas.DrawOverflowArea(new SkRect(0f, 0f, contentSize.Width, contentSize.Height));
				base.Canvas.Restore();
				using (SkPaint skPaint2 = new SkPaint())
				{
					skPaint2.SetStroke(1.5f);
					skPaint2.SetSolidColor(this.LineColor);
					base.Canvas.DrawRectangle(Position.Zero, contentSize, skPaint2);
				}
			}
		}

		// Token: 0x040003E8 RID: 1000
		private const float BorderThickness = 1.5f;

		// Token: 0x040003E9 RID: 1001
		private readonly Color LineColor = Colors.Red.Medium;

		// Token: 0x040003EA RID: 1002
		private readonly Color AvailableAreaColor = Colors.Green.Medium;

		// Token: 0x040003EB RID: 1003
		private const byte AreaOpacity = 64;
	}
}
