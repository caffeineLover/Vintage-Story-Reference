using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000C0 RID: 192
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class Decoration : Element, IContentDirectionAware
	{
		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060005B9 RID: 1465 RVA: 0x0000E48E File Offset: 0x0000C68E
		// (set) Token: 0x060005BA RID: 1466 RVA: 0x0000E496 File Offset: 0x0000C696
		public ContentDirection ContentDirection { get; set; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060005BB RID: 1467 RVA: 0x0000E49F File Offset: 0x0000C69F
		// (set) Token: 0x060005BC RID: 1468 RVA: 0x0000E4A7 File Offset: 0x0000C6A7
		internal Element Before { get; set; } = new DebugPointer(DebugPointerType.ElementStructure, "Before");

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060005BD RID: 1469 RVA: 0x0000E4B0 File Offset: 0x0000C6B0
		// (set) Token: 0x060005BE RID: 1470 RVA: 0x0000E4B8 File Offset: 0x0000C6B8
		internal Element Content { get; set; } = new DebugPointer(DebugPointerType.ElementStructure, "Content");

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x0000E4C1 File Offset: 0x0000C6C1
		// (set) Token: 0x060005C0 RID: 1472 RVA: 0x0000E4C9 File Offset: 0x0000C6C9
		internal Element After { get; set; } = new DebugPointer(DebugPointerType.ElementStructure, "After");

		// Token: 0x060005C1 RID: 1473 RVA: 0x0000E4D2 File Offset: 0x0000C6D2
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		internal override IEnumerable<Element> GetChildren()
		{
			Decoration.<GetChildren>d__16 <GetChildren>d__ = new Decoration.<GetChildren>d__16(-2);
			<GetChildren>d__.<>4__this = this;
			return <GetChildren>d__;
		}

		// Token: 0x060005C2 RID: 1474 RVA: 0x0000E4E2 File Offset: 0x0000C6E2
		internal override void CreateProxy([Nullable(new byte[]
		{
			1,
			2,
			2
		})] Func<Element, Element> create)
		{
			this.Before = create.Invoke(this.Before);
			this.Content = create.Invoke(this.Content);
			this.After = create.Invoke(this.After);
		}

		// Token: 0x060005C3 RID: 1475 RVA: 0x0000E51C File Offset: 0x0000C71C
		internal override SpacePlan Measure(Size availableSpace)
		{
			DecorationElementLayout decorationElementLayout = this.PlanLayout(availableSpace);
			if (decorationElementLayout.Content.Measurement.Type == SpacePlanType.Empty)
			{
				return SpacePlan.Empty();
			}
			if (decorationElementLayout.Content.Measurement.Type == SpacePlanType.Wrap)
			{
				return SpacePlan.Wrap("The primary content does not fit on the page.");
			}
			if (decorationElementLayout.Before.Measurement.Type == SpacePlanType.Wrap)
			{
				return decorationElementLayout.Before.Measurement;
			}
			if (decorationElementLayout.After.Measurement.Type == SpacePlanType.Wrap)
			{
				return decorationElementLayout.After.Measurement;
			}
			SpacePlan[] array = new SpacePlan[]
			{
				decorationElementLayout.Before.Measurement,
				decorationElementLayout.Content.Measurement,
				decorationElementLayout.After.Measurement
			};
			float num = Enumerable.Max<SpacePlan>(array, (SpacePlan x) => x.Width);
			float num2 = Enumerable.Sum<SpacePlan>(array, (SpacePlan x) => x.Height);
			Size size = new Size(num, num2);
			if (num > availableSpace.Width + 0.01f)
			{
				return SpacePlan.Wrap("The content slot requires more horizontal space than available.");
			}
			if (num2 > availableSpace.Height + 0.01f)
			{
				return SpacePlan.Wrap("The content slot requires more vertical space than available.");
			}
			if (!Enumerable.All<SpacePlan>(array, delegate(SpacePlan x)
			{
				SpacePlanType type = x.Type;
				return type == SpacePlanType.Empty || type == SpacePlanType.FullRender;
			}))
			{
				return SpacePlan.PartialRender(size);
			}
			return SpacePlan.FullRender(size);
		}

		// Token: 0x060005C4 RID: 1476 RVA: 0x0000E6A4 File Offset: 0x0000C8A4
		internal override void Draw(Size availableSpace)
		{
			DecorationElementLayout decorationElementLayout = this.PlanLayout(availableSpace);
			DecorationElementLayout.ItemCommand[] array = new DecorationElementLayout.ItemCommand[]
			{
				decorationElementLayout.Before,
				decorationElementLayout.Content,
				decorationElementLayout.After
			};
			float num = Enumerable.Max<DecorationElementLayout.ItemCommand>(array, (DecorationElementLayout.ItemCommand x) => x.Measurement.Width);
			foreach (DecorationElementLayout.ItemCommand itemCommand in array)
			{
				Size availableSpace2 = new Size(num, itemCommand.Measurement.Height);
				Position vector = (this.ContentDirection == ContentDirection.LeftToRight) ? itemCommand.Offset : new Position(availableSpace.Width - num, itemCommand.Offset.Y);
				base.Canvas.Translate(vector);
				itemCommand.Element.Draw(availableSpace2);
				base.Canvas.Translate(vector.Reverse());
			}
		}

		// Token: 0x060005C5 RID: 1477 RVA: 0x0000E794 File Offset: 0x0000C994
		private DecorationElementLayout PlanLayout(Size availableSpace)
		{
			Decoration.<>c__DisplayClass20_0 CS$<>8__locals1;
			CS$<>8__locals1.availableSpace = availableSpace;
			SpacePlan spacePlan = Decoration.<PlanLayout>g__GetDecorationMeasurement|20_0(this.Before, ref CS$<>8__locals1);
			SpacePlan spacePlan2 = Decoration.<PlanLayout>g__GetDecorationMeasurement|20_0(this.After, ref CS$<>8__locals1);
			Size availableSpace2 = new Size(CS$<>8__locals1.availableSpace.Width, CS$<>8__locals1.availableSpace.Height - spacePlan.Height - spacePlan2.Height);
			SpacePlan spacePlan3 = this.Content.Measure(availableSpace2);
			return new DecorationElementLayout
			{
				Before = new DecorationElementLayout.ItemCommand
				{
					Element = this.Before,
					Measurement = spacePlan,
					Offset = Position.Zero
				},
				Content = new DecorationElementLayout.ItemCommand
				{
					Element = this.Content,
					Measurement = spacePlan3,
					Offset = new Position(0f, spacePlan.Height)
				},
				After = new DecorationElementLayout.ItemCommand
				{
					Element = this.After,
					Measurement = spacePlan2,
					Offset = new Position(0f, spacePlan.Height + spacePlan3.Height)
				}
			};
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x0000E8F0 File Offset: 0x0000CAF0
		[CompilerGenerated]
		internal static SpacePlan <PlanLayout>g__GetDecorationMeasurement|20_0(Element element, ref Decoration.<>c__DisplayClass20_0 A_1)
		{
			SpacePlan spacePlan = element.Measure(A_1.availableSpace);
			SpacePlanType type = spacePlan.Type;
			bool flag = type - SpacePlanType.Wrap <= 1;
			if (flag)
			{
				return SpacePlan.Wrap("Decoration slot (before or after) does not fit fully on the page.");
			}
			return spacePlan;
		}
	}
}
