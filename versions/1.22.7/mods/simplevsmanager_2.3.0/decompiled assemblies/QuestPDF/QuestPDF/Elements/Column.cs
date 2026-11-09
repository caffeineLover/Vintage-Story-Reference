using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000B7 RID: 183
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class Column : Element, IStateful
	{
		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600057D RID: 1405 RVA: 0x0000DC52 File Offset: 0x0000BE52
		internal List<Element> Items { get; } = new List<Element>();

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600057E RID: 1406 RVA: 0x0000DC5A File Offset: 0x0000BE5A
		// (set) Token: 0x0600057F RID: 1407 RVA: 0x0000DC62 File Offset: 0x0000BE62
		internal float Spacing { get; set; }

		// Token: 0x06000580 RID: 1408 RVA: 0x0000DC6B File Offset: 0x0000BE6B
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		internal override IEnumerable<Element> GetChildren()
		{
			return this.Items;
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0000DC74 File Offset: 0x0000BE74
		internal override void CreateProxy([Nullable(new byte[]
		{
			1,
			2,
			2
		})] Func<Element, Element> create)
		{
			for (int i = 0; i < this.Items.Count; i++)
			{
				this.Items[i] = create.Invoke(this.Items[i]);
			}
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0000DCB8 File Offset: 0x0000BEB8
		internal override SpacePlan Measure(Size availableSpace)
		{
			if (!Enumerable.Any<Element>(this.Items))
			{
				return SpacePlan.Empty();
			}
			if (this.CurrentRenderingIndex == this.Items.Count)
			{
				return SpacePlan.Empty();
			}
			if (availableSpace.IsNegative())
			{
				return SpacePlan.Wrap("The available space is negative.");
			}
			List<ColumnItemRenderingCommand> list = this.PlanLayout(availableSpace);
			if (!Enumerable.Any<ColumnItemRenderingCommand>(list))
			{
				return SpacePlan.Wrap("The available space is not sufficient for even partially rendering a single item.");
			}
			float num = Enumerable.Max<ColumnItemRenderingCommand>(list, (ColumnItemRenderingCommand x) => x.Measurement.Width);
			float num2 = Enumerable.Last<ColumnItemRenderingCommand>(list).Offset.Y + Enumerable.Last<ColumnItemRenderingCommand>(list).Measurement.Height;
			Size size = new Size(num, num2);
			if (num > availableSpace.Width + 0.01f)
			{
				return SpacePlan.Wrap("The content requires more horizontal space than available.");
			}
			if (num2 > availableSpace.Height + 0.01f)
			{
				return SpacePlan.Wrap("The content requires more vertical space than available.");
			}
			if (this.CurrentRenderingIndex + Enumerable.Count<ColumnItemRenderingCommand>(list, delegate(ColumnItemRenderingCommand x)
			{
				SpacePlanType type = x.Measurement.Type;
				return type == SpacePlanType.Empty || type == SpacePlanType.FullRender;
			}) != this.Items.Count)
			{
				return SpacePlan.PartialRender(size);
			}
			return SpacePlan.FullRender(size);
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0000DDEC File Offset: 0x0000BFEC
		internal override void Draw(Size availableSpace)
		{
			List<ColumnItemRenderingCommand> list = this.PlanLayout(availableSpace);
			foreach (ColumnItemRenderingCommand columnItemRenderingCommand in list)
			{
				Size availableSpace2 = new Size(availableSpace.Width, columnItemRenderingCommand.Measurement.Height);
				base.Canvas.Translate(columnItemRenderingCommand.Offset);
				columnItemRenderingCommand.Element.Draw(availableSpace2);
				base.Canvas.Translate(columnItemRenderingCommand.Offset.Reverse());
			}
			int num = Enumerable.Count<ColumnItemRenderingCommand>(list, delegate(ColumnItemRenderingCommand x)
			{
				SpacePlanType type = x.Measurement.Type;
				return type == SpacePlanType.Empty || type == SpacePlanType.FullRender;
			});
			this.CurrentRenderingIndex += num;
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0000DEC0 File Offset: 0x0000C0C0
		private List<ColumnItemRenderingCommand> PlanLayout(Size availableSpace)
		{
			float num = 0f;
			List<ColumnItemRenderingCommand> list = new List<ColumnItemRenderingCommand>();
			foreach (Element element in Enumerable.Skip<Element>(this.Items, this.CurrentRenderingIndex))
			{
				bool flag = list.Count == 0;
				float num2 = availableSpace.Height - num;
				if (num2 < -0.01f)
				{
					break;
				}
				num2 = Math.Max(0f, num2);
				if (!flag)
				{
					num2 -= this.Spacing;
				}
				bool flag2 = num2 < 0.01f;
				Size availableSpace2 = flag2 ? Size.Zero : new Size(availableSpace.Width, num2);
				SpacePlan spacePlan = element.Measure(availableSpace2);
				if (spacePlan.Type == SpacePlanType.Wrap)
				{
					break;
				}
				bool flag3 = !Size.Equal(spacePlan, Size.Zero);
				if (flag2 && flag3)
				{
					break;
				}
				if (!flag && flag3)
				{
					num += this.Spacing;
				}
				list.Add(new ColumnItemRenderingCommand
				{
					Element = element,
					Measurement = spacePlan,
					Offset = new Position(0f, num)
				});
				if (spacePlan.Type == SpacePlanType.PartialRender)
				{
					break;
				}
				num += spacePlan.Height;
			}
			return list;
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000585 RID: 1413 RVA: 0x0000E024 File Offset: 0x0000C224
		// (set) Token: 0x06000586 RID: 1414 RVA: 0x0000E02C File Offset: 0x0000C22C
		internal int CurrentRenderingIndex { get; set; }

		// Token: 0x06000587 RID: 1415 RVA: 0x0000E035 File Offset: 0x0000C235
		public void ResetState(bool hardReset = false)
		{
			this.CurrentRenderingIndex = 0;
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0000E03E File Offset: 0x0000C23E
		public object GetState()
		{
			return this.CurrentRenderingIndex;
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0000E04B File Offset: 0x0000C24B
		public void SetState(object state)
		{
			this.CurrentRenderingIndex = (int)state;
		}
	}
}
