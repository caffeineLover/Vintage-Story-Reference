using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000D4 RID: 212
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class Inlined : Element, IContentDirectionAware, IStateful
	{
		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600068E RID: 1678 RVA: 0x0000FDEE File Offset: 0x0000DFEE
		// (set) Token: 0x0600068F RID: 1679 RVA: 0x0000FDF6 File Offset: 0x0000DFF6
		public List<Element> Elements { get; internal set; } = new List<Element>();

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x06000690 RID: 1680 RVA: 0x0000FDFF File Offset: 0x0000DFFF
		// (set) Token: 0x06000691 RID: 1681 RVA: 0x0000FE07 File Offset: 0x0000E007
		public ContentDirection ContentDirection { get; set; }

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x06000692 RID: 1682 RVA: 0x0000FE10 File Offset: 0x0000E010
		// (set) Token: 0x06000693 RID: 1683 RVA: 0x0000FE18 File Offset: 0x0000E018
		internal float VerticalSpacing { get; set; }

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000694 RID: 1684 RVA: 0x0000FE21 File Offset: 0x0000E021
		// (set) Token: 0x06000695 RID: 1685 RVA: 0x0000FE29 File Offset: 0x0000E029
		internal float HorizontalSpacing { get; set; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000696 RID: 1686 RVA: 0x0000FE32 File Offset: 0x0000E032
		// (set) Token: 0x06000697 RID: 1687 RVA: 0x0000FE3A File Offset: 0x0000E03A
		internal InlinedAlignment? ElementsAlignment { get; set; }

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000698 RID: 1688 RVA: 0x0000FE43 File Offset: 0x0000E043
		// (set) Token: 0x06000699 RID: 1689 RVA: 0x0000FE4B File Offset: 0x0000E04B
		internal VerticalAlignment BaselineAlignment { get; set; }

		// Token: 0x0600069A RID: 1690 RVA: 0x0000FE54 File Offset: 0x0000E054
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		internal override IEnumerable<Element> GetChildren()
		{
			return this.Elements;
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x0000FE5C File Offset: 0x0000E05C
		internal override SpacePlan Measure(Size availableSpace)
		{
			this.SetDefaultAlignment();
			if (this.CurrentRenderingIndex == this.Elements.Count)
			{
				return SpacePlan.Empty();
			}
			ICollection<ICollection<InlinedMeasurement>> collection = this.Compose(availableSpace);
			if (!Enumerable.Any<ICollection<InlinedMeasurement>>(collection))
			{
				return SpacePlan.Wrap("The available space is not sufficient to fully render even a single item.");
			}
			List<Size> list = Enumerable.ToList<Size>(Enumerable.Select<ICollection<InlinedMeasurement>, Size>(collection, delegate(ICollection<InlinedMeasurement> line)
			{
				Size lineSize = Inlined.GetLineSize(line);
				return new Size(lineSize.Width + (float)(line.Count - 1) * this.HorizontalSpacing, lineSize.Height);
			}));
			float width = Enumerable.Max<Size>(list, (Size x) => x.Width);
			float height = Enumerable.Sum<Size>(list, (Size x) => x.Height) + (float)(collection.Count - 1) * this.VerticalSpacing;
			Size size = new Size(width, height);
			if (this.CurrentRenderingIndex + Enumerable.Sum<ICollection<InlinedMeasurement>>(collection, (ICollection<InlinedMeasurement> x) => x.Count) != this.Elements.Count)
			{
				return SpacePlan.PartialRender(size);
			}
			return SpacePlan.FullRender(size);
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x0000FF68 File Offset: 0x0000E168
		internal override void Draw(Size availableSpace)
		{
			Inlined.<>c__DisplayClass26_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.availableSpace = availableSpace;
			this.SetDefaultAlignment();
			ICollection<ICollection<InlinedMeasurement>> collection = this.Compose(CS$<>8__locals1.availableSpace);
			float num = 0f;
			foreach (ICollection<InlinedMeasurement> collection2 in collection)
			{
				float num2 = Enumerable.Max<InlinedMeasurement>(collection2, (InlinedMeasurement x) => x.Size.Height);
				this.<Draw>g__DrawLine|26_1(collection2, ref CS$<>8__locals1);
				num += num2 + this.VerticalSpacing;
				base.Canvas.Translate(new Position(0f, num2 + this.VerticalSpacing));
			}
			base.Canvas.Translate(new Position(0f, -num));
			int num3 = Enumerable.Sum<ICollection<InlinedMeasurement>>(collection, (ICollection<InlinedMeasurement> x) => x.Count);
			this.CurrentRenderingIndex += num3;
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00010080 File Offset: 0x0000E280
		private void SetDefaultAlignment()
		{
			if (this.ElementsAlignment != null)
			{
				return;
			}
			this.ElementsAlignment = new InlinedAlignment?((this.ContentDirection == ContentDirection.LeftToRight) ? InlinedAlignment.Left : InlinedAlignment.Right);
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x000100B8 File Offset: 0x0000E2B8
		private static Size GetLineSize(ICollection<InlinedMeasurement> measurements)
		{
			float width = Enumerable.Sum<InlinedMeasurement>(measurements, (InlinedMeasurement x) => x.Size.Width);
			float height = Enumerable.Max<InlinedMeasurement>(measurements, (InlinedMeasurement x) => x.Size.Height);
			return new Size(width, height);
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00010118 File Offset: 0x0000E318
		private ICollection<ICollection<InlinedMeasurement>> Compose(Size availableSize)
		{
			Inlined.<>c__DisplayClass29_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.availableSize = availableSize;
			CS$<>8__locals1.localRenderingIndex = this.CurrentRenderingIndex;
			List<ICollection<InlinedMeasurement>> list = new List<ICollection<InlinedMeasurement>>();
			float num = 0f;
			for (;;)
			{
				ICollection<InlinedMeasurement> collection = this.<Compose>g__GetNextLine|29_0(ref CS$<>8__locals1);
				if (!Enumerable.Any<InlinedMeasurement>(collection))
				{
					break;
				}
				float num2 = Enumerable.Max<InlinedMeasurement>(collection, (InlinedMeasurement x) => x.Size.Height);
				if (num + num2 > CS$<>8__locals1.availableSize.Height + 0.01f)
				{
					break;
				}
				num += num2 + this.VerticalSpacing;
				list.Add(collection);
			}
			return list;
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060006A0 RID: 1696 RVA: 0x000101B3 File Offset: 0x0000E3B3
		// (set) Token: 0x060006A1 RID: 1697 RVA: 0x000101BB File Offset: 0x0000E3BB
		private int CurrentRenderingIndex { get; set; }

		// Token: 0x060006A2 RID: 1698 RVA: 0x000101C4 File Offset: 0x0000E3C4
		public void ResetState(bool hardReset = false)
		{
			this.CurrentRenderingIndex = 0;
		}

		// Token: 0x060006A3 RID: 1699 RVA: 0x000101CD File Offset: 0x0000E3CD
		public object GetState()
		{
			return this.CurrentRenderingIndex;
		}

		// Token: 0x060006A4 RID: 1700 RVA: 0x000101DA File Offset: 0x0000E3DA
		public void SetState(object state)
		{
			this.CurrentRenderingIndex = (int)state;
		}

		// Token: 0x060006A7 RID: 1703 RVA: 0x00010234 File Offset: 0x0000E434
		[CompilerGenerated]
		private void <Draw>g__DrawLine|26_1(ICollection<InlinedMeasurement> lineMeasurements, ref Inlined.<>c__DisplayClass26_0 A_2)
		{
			Inlined.<>c__DisplayClass26_1 CS$<>8__locals1;
			CS$<>8__locals1.lineMeasurements = lineMeasurements;
			CS$<>8__locals1.lineSize = Inlined.GetLineSize(CS$<>8__locals1.lineMeasurements);
			CS$<>8__locals1.elementOffset = this.<Draw>g__ElementOffset|26_3(ref A_2, ref CS$<>8__locals1);
			float num = this.<Draw>g__AlignOffset|26_4(ref A_2, ref CS$<>8__locals1);
			foreach (InlinedMeasurement inlinedMeasurement in CS$<>8__locals1.lineMeasurements)
			{
				Size size = inlinedMeasurement.Size;
				float y = this.<Draw>g__BaselineOffset|26_5(size, CS$<>8__locals1.lineSize.Height, ref A_2);
				if (size.Height == 0f)
				{
					size = new Size(size.Width, CS$<>8__locals1.lineSize.Height);
				}
				Position vector = (this.ContentDirection == ContentDirection.LeftToRight) ? new Position(num, y) : new Position(A_2.availableSpace.Width - size.Width - num, y);
				base.Canvas.Translate(vector);
				inlinedMeasurement.Element.Draw(size);
				base.Canvas.Translate(vector.Reverse());
				num += size.Width + CS$<>8__locals1.elementOffset;
			}
		}

		// Token: 0x060006A8 RID: 1704 RVA: 0x00010370 File Offset: 0x0000E570
		[CompilerGenerated]
		private float <Draw>g__ElementOffset|26_3(ref Inlined.<>c__DisplayClass26_0 A_1, ref Inlined.<>c__DisplayClass26_1 A_2)
		{
			float num = A_1.availableSpace.Width - A_2.lineSize.Width;
			if (A_2.lineMeasurements.Count == 1)
			{
				return 0f;
			}
			InlinedAlignment? elementsAlignment = this.ElementsAlignment;
			if (elementsAlignment != null)
			{
				InlinedAlignment valueOrDefault = elementsAlignment.GetValueOrDefault();
				if (valueOrDefault == InlinedAlignment.Justify)
				{
					return num / (float)(A_2.lineMeasurements.Count - 1);
				}
				if (valueOrDefault == InlinedAlignment.SpaceAround)
				{
					return num / (float)(A_2.lineMeasurements.Count + 1);
				}
			}
			return this.HorizontalSpacing;
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x000103FC File Offset: 0x0000E5FC
		[CompilerGenerated]
		private float <Draw>g__AlignOffset|26_4(ref Inlined.<>c__DisplayClass26_0 A_1, ref Inlined.<>c__DisplayClass26_1 A_2)
		{
			float num = A_1.availableSpace.Width - A_2.lineSize.Width - (float)(A_2.lineMeasurements.Count - 1) * this.HorizontalSpacing;
			InlinedAlignment? elementsAlignment = this.ElementsAlignment;
			if (elementsAlignment != null)
			{
				switch (elementsAlignment.GetValueOrDefault())
				{
				case InlinedAlignment.Left:
					return (this.ContentDirection == ContentDirection.LeftToRight) ? 0f : num;
				case InlinedAlignment.Center:
					return num / 2f;
				case InlinedAlignment.Right:
					return (this.ContentDirection == ContentDirection.LeftToRight) ? num : 0f;
				case InlinedAlignment.Justify:
					return 0f;
				case InlinedAlignment.SpaceAround:
					return A_2.elementOffset;
				}
			}
			return 0f;
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x000104B4 File Offset: 0x0000E6B4
		[CompilerGenerated]
		private float <Draw>g__BaselineOffset|26_5(Size elementSize, float lineHeight, ref Inlined.<>c__DisplayClass26_0 A_3)
		{
			float num = lineHeight - elementSize.Height;
			VerticalAlignment baselineAlignment = this.BaselineAlignment;
			float result;
			if (baselineAlignment != VerticalAlignment.Top)
			{
				if (baselineAlignment != VerticalAlignment.Middle)
				{
					result = num;
				}
				else
				{
					result = num / 2f;
				}
			}
			else
			{
				result = 0f;
			}
			return result;
		}

		// Token: 0x060006AB RID: 1707 RVA: 0x000104F0 File Offset: 0x0000E6F0
		[CompilerGenerated]
		private ICollection<InlinedMeasurement> <Compose>g__GetNextLine|29_0(ref Inlined.<>c__DisplayClass29_0 A_1)
		{
			List<InlinedMeasurement> list = new List<InlinedMeasurement>();
			float num = this.<Compose>g__GetInitialAlignmentOffset|29_1(ref A_1);
			while (A_1.localRenderingIndex != this.Elements.Count)
			{
				Element element = this.Elements[A_1.localRenderingIndex];
				SpacePlan spacePlan = element.Measure(new Size(A_1.availableSize.Width, Size.Max.Height));
				if (spacePlan.Type == SpacePlanType.Wrap || num + spacePlan.Width > A_1.availableSize.Width + 0.01f)
				{
					break;
				}
				int localRenderingIndex = A_1.localRenderingIndex;
				A_1.localRenderingIndex = localRenderingIndex + 1;
				num += spacePlan.Width + this.HorizontalSpacing;
				list.Add(new InlinedMeasurement
				{
					Element = element,
					Size = spacePlan
				});
			}
			return list;
		}

		// Token: 0x060006AC RID: 1708 RVA: 0x000105C0 File Offset: 0x0000E7C0
		[CompilerGenerated]
		private float <Compose>g__GetInitialAlignmentOffset|29_1(ref Inlined.<>c__DisplayClass29_0 A_1)
		{
			float result;
			if (!(this.ElementsAlignment != InlinedAlignment.SpaceAround))
			{
				result = this.HorizontalSpacing * 2f;
			}
			else
			{
				result = 0f;
			}
			return result;
		}
	}
}
