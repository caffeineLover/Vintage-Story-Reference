using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000E7 RID: 231
	internal sealed class Scale : ContainerElement
	{
		// Token: 0x170001BF RID: 447
		// (get) Token: 0x06000785 RID: 1925 RVA: 0x000124D1 File Offset: 0x000106D1
		// (set) Token: 0x06000786 RID: 1926 RVA: 0x000124D9 File Offset: 0x000106D9
		public float ScaleX { get; set; } = 1f;

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x06000787 RID: 1927 RVA: 0x000124E2 File Offset: 0x000106E2
		// (set) Token: 0x06000788 RID: 1928 RVA: 0x000124EA File Offset: 0x000106EA
		public float ScaleY { get; set; } = 1f;

		// Token: 0x06000789 RID: 1929 RVA: 0x000124F4 File Offset: 0x000106F4
		internal override SpacePlan Measure(Size availableSpace)
		{
			Size availableSpace2 = new Size(Math.Abs(availableSpace.Width / this.ScaleX), Math.Abs(availableSpace.Height / this.ScaleY));
			SpacePlan spacePlan = base.Measure(availableSpace2);
			SpacePlanType type = spacePlan.Type;
			bool flag = type <= SpacePlanType.Wrap;
			if (flag)
			{
				return spacePlan;
			}
			Size size = new Size(Math.Abs(spacePlan.Width * this.ScaleX), Math.Abs(spacePlan.Height * this.ScaleY));
			if (spacePlan.Type == SpacePlanType.PartialRender)
			{
				return SpacePlan.PartialRender(size);
			}
			if (spacePlan.Type == SpacePlanType.FullRender)
			{
				return SpacePlan.FullRender(size);
			}
			throw new ArgumentException();
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x000125A0 File Offset: 0x000107A0
		internal override void Draw(Size availableSpace)
		{
			Size availableSpace2 = new Size(Math.Abs(availableSpace.Width / this.ScaleX), Math.Abs(availableSpace.Height / this.ScaleY));
			Position vector = new Position((this.ScaleX < 0f) ? availableSpace.Width : 0f, (this.ScaleY < 0f) ? availableSpace.Height : 0f);
			base.Canvas.Translate(vector);
			base.Canvas.Scale(this.ScaleX, this.ScaleY);
			Element child = base.Child;
			if (child != null)
			{
				child.Draw(availableSpace2);
			}
			base.Canvas.Scale(1f / this.ScaleX, 1f / this.ScaleY);
			base.Canvas.Translate(vector.Reverse());
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x00012680 File Offset: 0x00010880
		[NullableContext(2)]
		internal override string GetCompanionHint()
		{
			return string.Join("   ", Enumerable.Select<ValueTuple<string, float>, string>(Enumerable.Where<ValueTuple<string, float>>(this.<GetCompanionHint>g__GetOptions|10_2(), ([TupleElementNames(new string[]
			{
				"Label",
				"value"
			})] ValueTuple<string, float> x) => x.Item2 != 1f), ([TupleElementNames(new string[]
			{
				"Label",
				"value"
			})] ValueTuple<string, float> x) => x.Item1 + "=" + x.Item2.FormatAsCompanionNumber()));
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x00012703 File Offset: 0x00010903
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
		private IEnumerable<ValueTuple<string, float>> <GetCompanionHint>g__GetOptions|10_2()
		{
			Scale.<<GetCompanionHint>g__GetOptions|10_2>d <<GetCompanionHint>g__GetOptions|10_2>d = new Scale.<<GetCompanionHint>g__GetOptions|10_2>d(-2);
			<<GetCompanionHint>g__GetOptions|10_2>d.<>4__this = this;
			return <<GetCompanionHint>g__GetOptions|10_2>d;
		}
	}
}
