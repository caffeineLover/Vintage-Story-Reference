using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000B8 RID: 184
	internal sealed class Constrained : ContainerElement, IContentDirectionAware
	{
		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600058B RID: 1419 RVA: 0x0000E06C File Offset: 0x0000C26C
		// (set) Token: 0x0600058C RID: 1420 RVA: 0x0000E074 File Offset: 0x0000C274
		public ContentDirection ContentDirection { get; set; }

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x0600058D RID: 1421 RVA: 0x0000E07D File Offset: 0x0000C27D
		// (set) Token: 0x0600058E RID: 1422 RVA: 0x0000E085 File Offset: 0x0000C285
		public float? MinWidth { get; set; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x0600058F RID: 1423 RVA: 0x0000E08E File Offset: 0x0000C28E
		// (set) Token: 0x06000590 RID: 1424 RVA: 0x0000E096 File Offset: 0x0000C296
		public float? MaxWidth { get; set; }

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000591 RID: 1425 RVA: 0x0000E09F File Offset: 0x0000C29F
		// (set) Token: 0x06000592 RID: 1426 RVA: 0x0000E0A7 File Offset: 0x0000C2A7
		public float? MinHeight { get; set; }

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000593 RID: 1427 RVA: 0x0000E0B0 File Offset: 0x0000C2B0
		// (set) Token: 0x06000594 RID: 1428 RVA: 0x0000E0B8 File Offset: 0x0000C2B8
		public float? MaxHeight { get; set; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000595 RID: 1429 RVA: 0x0000E0C1 File Offset: 0x0000C2C1
		// (set) Token: 0x06000596 RID: 1430 RVA: 0x0000E0C9 File Offset: 0x0000C2C9
		public bool EnforceSizeWhenEmpty { get; set; }

		// Token: 0x06000597 RID: 1431 RVA: 0x0000E0D4 File Offset: 0x0000C2D4
		internal override SpacePlan Measure(Size availableSpace)
		{
			if (!this.EnforceSizeWhenEmpty && base.Child.IsEmpty())
			{
				return SpacePlan.Empty();
			}
			float? num = this.MinWidth;
			float num2 = availableSpace.Width + 0.01f;
			if (num.GetValueOrDefault() > num2 & num != null)
			{
				return SpacePlan.Wrap("The available horizontal space is less than the minimum width.");
			}
			num = this.MinHeight;
			num2 = availableSpace.Height + 0.01f;
			if (num.GetValueOrDefault() > num2 & num != null)
			{
				return SpacePlan.Wrap("The available vertical space is less than the minimum height.");
			}
			Size availableSpace2 = new Size(Constrained.Min(this.MaxWidth, availableSpace.Width), Constrained.Min(this.MaxHeight, availableSpace.Height));
			SpacePlan spacePlan = base.Measure(availableSpace2);
			if (spacePlan.Type == SpacePlanType.Wrap)
			{
				return spacePlan;
			}
			Size size = new Size(Constrained.Max(this.MinWidth, spacePlan.Width), Constrained.Max(this.MinHeight, spacePlan.Height));
			if (spacePlan.Type == SpacePlanType.Empty)
			{
				if (!this.EnforceSizeWhenEmpty)
				{
					return SpacePlan.Empty();
				}
				return SpacePlan.FullRender(size);
			}
			else
			{
				if (spacePlan.Type == SpacePlanType.FullRender)
				{
					return SpacePlan.FullRender(size);
				}
				if (spacePlan.Type == SpacePlanType.PartialRender)
				{
					return SpacePlan.PartialRender(size);
				}
				throw new NotSupportedException();
			}
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x0000E210 File Offset: 0x0000C410
		internal override void Draw(Size availableSpace)
		{
			Size size = new Size(Constrained.Min(this.MaxWidth, availableSpace.Width), Constrained.Min(this.MaxHeight, availableSpace.Height));
			Position vector = (this.ContentDirection == ContentDirection.LeftToRight) ? Position.Zero : new Position(availableSpace.Width - size.Width, 0f);
			base.Canvas.Translate(vector);
			base.Draw(size);
			base.Canvas.Translate(vector.Reverse());
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x0000E292 File Offset: 0x0000C492
		private static float Min(float? x, float y)
		{
			if (x == null)
			{
				return y;
			}
			return Math.Min(x.Value, y);
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0000E2AC File Offset: 0x0000C4AC
		private static float Max(float? x, float y)
		{
			if (x == null)
			{
				return y;
			}
			return Math.Max(x.Value, y);
		}

		// Token: 0x0600059B RID: 1435 RVA: 0x0000E2C8 File Offset: 0x0000C4C8
		[NullableContext(2)]
		internal override string GetCompanionHint()
		{
			IEnumerable<string> enumerable = Constrained.<GetCompanionHint>g__FormatRange|28_0("W", this.MinWidth, this.MaxWidth);
			IEnumerable<string> enumerable2 = Constrained.<GetCompanionHint>g__FormatRange|28_0("H", this.MinHeight, this.MaxHeight);
			return string.Join("   ", Enumerable.Concat<string>(enumerable, enumerable2));
		}

		// Token: 0x0600059D RID: 1437 RVA: 0x0000E31C File Offset: 0x0000C51C
		[NullableContext(1)]
		[CompilerGenerated]
		internal static IEnumerable<string> <GetCompanionHint>g__FormatRange|28_0(string prefix, float? min, float? max)
		{
			Constrained.<<GetCompanionHint>g__FormatRange|28_0>d <<GetCompanionHint>g__FormatRange|28_0>d = new Constrained.<<GetCompanionHint>g__FormatRange|28_0>d(-2);
			<<GetCompanionHint>g__FormatRange|28_0>d.<>3__prefix = prefix;
			<<GetCompanionHint>g__FormatRange|28_0>d.<>3__min = min;
			<<GetCompanionHint>g__FormatRange|28_0>d.<>3__max = max;
			return <<GetCompanionHint>g__FormatRange|28_0>d;
		}
	}
}
