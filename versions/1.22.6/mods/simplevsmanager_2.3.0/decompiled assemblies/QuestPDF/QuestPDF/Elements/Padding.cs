using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000DC RID: 220
	internal sealed class Padding : ContainerElement
	{
		// Token: 0x1700019A RID: 410
		// (get) Token: 0x0600070A RID: 1802 RVA: 0x000111B4 File Offset: 0x0000F3B4
		// (set) Token: 0x0600070B RID: 1803 RVA: 0x000111BC File Offset: 0x0000F3BC
		public float Top { get; set; }

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x0600070C RID: 1804 RVA: 0x000111C5 File Offset: 0x0000F3C5
		// (set) Token: 0x0600070D RID: 1805 RVA: 0x000111CD File Offset: 0x0000F3CD
		public float Right { get; set; }

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x0600070E RID: 1806 RVA: 0x000111D6 File Offset: 0x0000F3D6
		// (set) Token: 0x0600070F RID: 1807 RVA: 0x000111DE File Offset: 0x0000F3DE
		public float Bottom { get; set; }

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000710 RID: 1808 RVA: 0x000111E7 File Offset: 0x0000F3E7
		// (set) Token: 0x06000711 RID: 1809 RVA: 0x000111EF File Offset: 0x0000F3EF
		public float Left { get; set; }

		// Token: 0x06000712 RID: 1810 RVA: 0x000111F8 File Offset: 0x0000F3F8
		internal override SpacePlan Measure(Size availableSpace)
		{
			Size size = this.InternalSpace(availableSpace);
			if (size.IsNegative())
			{
				if (!base.Child.IsEmpty())
				{
					return SpacePlan.Wrap("The available space is negative.");
				}
				return SpacePlan.Empty();
			}
			else
			{
				SpacePlan spacePlan = base.Measure(size);
				SpacePlanType type = spacePlan.Type;
				bool flag = type <= SpacePlanType.Wrap;
				if (flag)
				{
					return spacePlan;
				}
				float width = Math.Max(0f, spacePlan.Width + this.Left + this.Right);
				float height = Math.Max(0f, spacePlan.Height + this.Top + this.Bottom);
				Size size2 = new Size(width, height);
				if (spacePlan.Type == SpacePlanType.PartialRender)
				{
					return SpacePlan.PartialRender(size2);
				}
				return SpacePlan.FullRender(size2);
			}
		}

		// Token: 0x06000713 RID: 1811 RVA: 0x000112B8 File Offset: 0x0000F4B8
		internal override void Draw(Size availableSpace)
		{
			Size availableSpace2 = this.InternalSpace(availableSpace);
			base.Canvas.Translate(new Position(this.Left, this.Top));
			base.Draw(availableSpace2);
			base.Canvas.Translate(new Position(-this.Left, -this.Top));
		}

		// Token: 0x06000714 RID: 1812 RVA: 0x0001130E File Offset: 0x0000F50E
		private Size InternalSpace(Size availableSpace)
		{
			return new Size(availableSpace.Width - this.Left - this.Right, availableSpace.Height - this.Top - this.Bottom);
		}

		// Token: 0x06000715 RID: 1813 RVA: 0x00011340 File Offset: 0x0000F540
		[NullableContext(2)]
		internal override string GetCompanionHint()
		{
			return string.Join("   ", Enumerable.Select<ValueTuple<string, float>, string>(Enumerable.Where<ValueTuple<string, float>>(this.<GetCompanionHint>g__GetOptions|19_2(), ([TupleElementNames(new string[]
			{
				"Label",
				"value"
			})] ValueTuple<string, float> x) => x.Item2 != 0f), ([TupleElementNames(new string[]
			{
				"Label",
				"value"
			})] ValueTuple<string, float> x) => x.Item1 + "=" + x.Item2.FormatAsCompanionNumber()));
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x000113AD File Offset: 0x0000F5AD
		[CompilerGenerated]
		[return: TupleElementNames(new string[]
		{
			"Label",
			"value"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		private IEnumerable<ValueTuple<string, float>> <GetCompanionHint>g__GetOptions|19_2()
		{
			Padding.<<GetCompanionHint>g__GetOptions|19_2>d <<GetCompanionHint>g__GetOptions|19_2>d = new Padding.<<GetCompanionHint>g__GetOptions|19_2>d(-2);
			<<GetCompanionHint>g__GetOptions|19_2>d.<>4__this = this;
			return <<GetCompanionHint>g__GetOptions|19_2>d;
		}
	}
}
