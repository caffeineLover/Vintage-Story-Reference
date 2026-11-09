using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000B3 RID: 179
	internal sealed class Alignment : ContainerElement
	{
		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600055C RID: 1372 RVA: 0x0000D746 File Offset: 0x0000B946
		// (set) Token: 0x0600055D RID: 1373 RVA: 0x0000D74E File Offset: 0x0000B94E
		public VerticalAlignment? Vertical { get; set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600055E RID: 1374 RVA: 0x0000D757 File Offset: 0x0000B957
		// (set) Token: 0x0600055F RID: 1375 RVA: 0x0000D75F File Offset: 0x0000B95F
		public HorizontalAlignment? Horizontal { get; set; }

		// Token: 0x06000560 RID: 1376 RVA: 0x0000D768 File Offset: 0x0000B968
		internal override void Draw(Size availableSpace)
		{
			SpacePlan spacePlan = base.Measure(availableSpace);
			SpacePlanType type = spacePlan.Type;
			bool flag = type <= SpacePlanType.Wrap;
			if (flag)
			{
				return;
			}
			Size size = new Size((this.Horizontal != null) ? spacePlan.Width : availableSpace.Width, (this.Vertical != null) ? spacePlan.Height : availableSpace.Height);
			float topOffset = this.GetTopOffset(availableSpace, size);
			float leftOffset = this.GetLeftOffset(availableSpace, size);
			base.Canvas.Translate(new Position(leftOffset, topOffset));
			base.Draw(size);
			base.Canvas.Translate(new Position(-leftOffset, -topOffset));
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x0000D81C File Offset: 0x0000BA1C
		private float GetTopOffset(Size availableSpace, Size childSize)
		{
			float num = availableSpace.Height - childSize.Height;
			VerticalAlignment? vertical = this.Vertical;
			if (vertical != null)
			{
				switch (vertical.GetValueOrDefault())
				{
				case VerticalAlignment.Top:
					return 0f;
				case VerticalAlignment.Middle:
					return num / 2f;
				case VerticalAlignment.Bottom:
					return num;
				}
			}
			return 0f;
		}

		// Token: 0x06000562 RID: 1378 RVA: 0x0000D880 File Offset: 0x0000BA80
		private float GetLeftOffset(Size availableSpace, Size childSize)
		{
			float num = availableSpace.Width - childSize.Width;
			HorizontalAlignment? horizontal = this.Horizontal;
			if (horizontal != null)
			{
				switch (horizontal.GetValueOrDefault())
				{
				case HorizontalAlignment.Left:
					return 0f;
				case HorizontalAlignment.Center:
					return num / 2f;
				case HorizontalAlignment.Right:
					return num;
				}
			}
			return 0f;
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x0000D8E4 File Offset: 0x0000BAE4
		[NullableContext(2)]
		internal override string GetCompanionHint()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<VerticalAlignment?>(this.Vertical);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted<HorizontalAlignment?>(this.Horizontal);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
