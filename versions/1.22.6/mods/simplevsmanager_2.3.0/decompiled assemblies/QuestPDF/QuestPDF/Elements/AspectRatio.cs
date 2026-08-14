using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000B5 RID: 181
	internal sealed class AspectRatio : ContainerElement, IContentDirectionAware
	{
		// Token: 0x1700011A RID: 282
		// (get) Token: 0x0600056B RID: 1387 RVA: 0x0000D9B2 File Offset: 0x0000BBB2
		// (set) Token: 0x0600056C RID: 1388 RVA: 0x0000D9BA File Offset: 0x0000BBBA
		public ContentDirection ContentDirection { get; set; }

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x0600056D RID: 1389 RVA: 0x0000D9C3 File Offset: 0x0000BBC3
		// (set) Token: 0x0600056E RID: 1390 RVA: 0x0000D9CB File Offset: 0x0000BBCB
		public float Ratio { get; set; }

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x0600056F RID: 1391 RVA: 0x0000D9D4 File Offset: 0x0000BBD4
		// (set) Token: 0x06000570 RID: 1392 RVA: 0x0000D9DC File Offset: 0x0000BBDC
		public AspectRatioOption Option { get; set; }

		// Token: 0x06000571 RID: 1393 RVA: 0x0000D9E8 File Offset: 0x0000BBE8
		internal override SpacePlan Measure(Size availableSpace)
		{
			if (this.Ratio == 0f)
			{
				return SpacePlan.FullRender(0f, 0f);
			}
			if (base.Child.IsEmpty())
			{
				return SpacePlan.Empty();
			}
			if (availableSpace.IsCloseToZero())
			{
				return SpacePlan.Wrap("The available space is zero.");
			}
			Size targetSize = this.GetTargetSize(availableSpace);
			if (targetSize.Height > availableSpace.Height + 0.01f)
			{
				return SpacePlan.Wrap("To preserve the target aspect ratio, the content requires more vertical space than available.");
			}
			if (targetSize.Width > availableSpace.Width + 0.01f)
			{
				return SpacePlan.Wrap("To preserve the target aspect ratio, the content requires more horizontal space than available.");
			}
			SpacePlan spacePlan = base.Measure(targetSize);
			if (spacePlan.Type == SpacePlanType.Wrap)
			{
				return spacePlan;
			}
			if (spacePlan.Type == SpacePlanType.PartialRender)
			{
				return SpacePlan.PartialRender(targetSize);
			}
			if (spacePlan.Type == SpacePlanType.FullRender)
			{
				return SpacePlan.FullRender(targetSize);
			}
			throw new NotSupportedException();
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x0000DAB8 File Offset: 0x0000BCB8
		internal override void Draw(Size availableSpace)
		{
			Size targetSize = this.GetTargetSize(availableSpace);
			Position vector = (this.ContentDirection == ContentDirection.LeftToRight) ? Position.Zero : new Position(availableSpace.Width - targetSize.Width, 0f);
			base.Canvas.Translate(vector);
			base.Draw(targetSize);
			base.Canvas.Translate(vector.Reverse());
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x0000DB1C File Offset: 0x0000BD1C
		private Size GetTargetSize(Size availableSpace)
		{
			if (this.Ratio == 0f)
			{
				return availableSpace;
			}
			float num = availableSpace.Width / availableSpace.Height;
			Size size = new Size(availableSpace.Height * this.Ratio, availableSpace.Height);
			Size size2 = new Size(availableSpace.Width, availableSpace.Width / this.Ratio);
			Size result;
			switch (this.Option)
			{
			case AspectRatioOption.FitWidth:
				result = size2;
				break;
			case AspectRatioOption.FitHeight:
				result = size;
				break;
			case AspectRatioOption.FitArea:
				result = ((this.Ratio < num) ? size : size2);
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
			return result;
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x0000DBB8 File Offset: 0x0000BDB8
		[NullableContext(2)]
		internal override string GetCompanionHint()
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(12, 2);
			defaultInterpolatedStringHandler.AppendFormatted(this.Option.ToString());
			defaultInterpolatedStringHandler.AppendLiteral(" with ratio ");
			defaultInterpolatedStringHandler.AppendFormatted<float>(this.Ratio, "F1");
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
