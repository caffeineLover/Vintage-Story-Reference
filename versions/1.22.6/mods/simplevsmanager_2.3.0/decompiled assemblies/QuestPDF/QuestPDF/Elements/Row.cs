using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000E6 RID: 230
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class Row : Element, IStateful, IContentDirectionAware
	{
		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000777 RID: 1911 RVA: 0x00011CEA File Offset: 0x0000FEEA
		// (set) Token: 0x06000778 RID: 1912 RVA: 0x00011CF2 File Offset: 0x0000FEF2
		public ContentDirection ContentDirection { get; set; }

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000779 RID: 1913 RVA: 0x00011CFB File Offset: 0x0000FEFB
		internal List<RowItem> Items { get; } = new List<RowItem>();

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x00011D03 File Offset: 0x0000FF03
		// (set) Token: 0x0600077B RID: 1915 RVA: 0x00011D0B File Offset: 0x0000FF0B
		internal float Spacing { get; set; }

		// Token: 0x0600077C RID: 1916 RVA: 0x00011D14 File Offset: 0x0000FF14
		[return: Nullable(new byte[]
		{
			1,
			2
		})]
		internal override IEnumerable<Element> GetChildren()
		{
			return this.Items;
		}

		// Token: 0x0600077D RID: 1917 RVA: 0x00011D1C File Offset: 0x0000FF1C
		internal override SpacePlan Measure(Size availableSpace)
		{
			if (!Enumerable.Any<RowItem>(this.Items))
			{
				return SpacePlan.Empty();
			}
			if (Enumerable.All<RowItem>(this.Items, (RowItem x) => x.IsRendered))
			{
				return SpacePlan.Empty();
			}
			this.UpdateItemsWidth(availableSpace.Width);
			if (Enumerable.Any<RowItem>(this.Items, (RowItem x) => x.Width.IsLessThan(0f)))
			{
				return SpacePlan.Wrap("One of the items has a negative size, indicating insufficient horizontal space. Usually, constant items require more space than is available, potentially causing other content to overflow.");
			}
			ICollection<RowItemRenderingCommand> collection = this.PlanLayout(availableSpace);
			if (Enumerable.Any<RowItemRenderingCommand>(collection, (RowItemRenderingCommand x) => !x.RowItem.IsRendered && x.Measurement.Type == SpacePlanType.Wrap))
			{
				return SpacePlan.Wrap("One of the items does not fit (even partially) in the available space.");
			}
			float num = Enumerable.Last<RowItemRenderingCommand>(collection).Offset.X + Enumerable.Last<RowItemRenderingCommand>(collection).Size.Width;
			float num2 = Enumerable.Max<RowItemRenderingCommand>(collection, (RowItemRenderingCommand x) => x.Size.Height);
			Size size = new Size(num, num2);
			if (num.IsGreaterThan(availableSpace.Width))
			{
				return SpacePlan.Wrap("The content requires more horizontal space than available.");
			}
			if (num2.IsGreaterThan(availableSpace.Height))
			{
				return SpacePlan.Wrap("The content requires more vertical space than available.");
			}
			if (Enumerable.Any<RowItemRenderingCommand>(collection, (RowItemRenderingCommand x) => !x.RowItem.IsRendered && x.Measurement.Type == SpacePlanType.PartialRender))
			{
				return SpacePlan.PartialRender(size);
			}
			return SpacePlan.FullRender(size);
		}

		// Token: 0x0600077E RID: 1918 RVA: 0x00011EA4 File Offset: 0x000100A4
		internal override void Draw(Size availableSpace)
		{
			if (!Enumerable.Any<RowItem>(this.Items))
			{
				return;
			}
			if (Enumerable.All<RowItem>(this.Items, (RowItem x) => x.IsRendered))
			{
				return;
			}
			this.UpdateItemsWidth(availableSpace.Width);
			foreach (RowItemRenderingCommand rowItemRenderingCommand in this.PlanLayout(availableSpace))
			{
				SpacePlanType type = rowItemRenderingCommand.Measurement.Type;
				bool flag = type == SpacePlanType.Empty || type == SpacePlanType.FullRender;
				if (flag)
				{
					rowItemRenderingCommand.RowItem.IsRendered = true;
				}
				if (rowItemRenderingCommand.Measurement.Type != SpacePlanType.Wrap)
				{
					Position vector = (this.ContentDirection == ContentDirection.LeftToRight) ? rowItemRenderingCommand.Offset : new Position(availableSpace.Width - rowItemRenderingCommand.Offset.X - rowItemRenderingCommand.Size.Width, 0f);
					Size size = new Size(rowItemRenderingCommand.Size.Width, availableSpace.Height);
					if (!size.Width.IsLessThan(0f))
					{
						base.Canvas.Translate(vector);
						rowItemRenderingCommand.RowItem.Draw(size);
						base.Canvas.Translate(vector.Reverse());
					}
				}
			}
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x00012004 File Offset: 0x00010204
		private void UpdateItemsWidth(float availableWidth)
		{
			foreach (RowItem rowItem in Enumerable.Where<RowItem>(this.Items, (RowItem x) => x.Type == RowItemType.Auto && x.Size == 0f))
			{
				rowItem.Size = rowItem.Measure(Size.Max).Width;
			}
			float num = Enumerable.Sum<RowItem>(Enumerable.Where<RowItem>(this.Items, (RowItem x) => x.Type != RowItemType.Relative), (RowItem x) => x.Size);
			float num2 = Enumerable.Sum<RowItem>(Enumerable.Where<RowItem>(this.Items, (RowItem x) => x.Type == RowItemType.Relative), (RowItem x) => x.Size);
			float num3 = (float)(this.Items.Count - 1) * this.Spacing;
			foreach (RowItem rowItem2 in Enumerable.Where<RowItem>(this.Items, (RowItem x) => x.Type != RowItemType.Relative))
			{
				rowItem2.Width = rowItem2.Size;
			}
			if (num2 <= 0f)
			{
				return;
			}
			float num4 = (availableWidth - num - num3) / num2;
			foreach (RowItem rowItem3 in Enumerable.Where<RowItem>(this.Items, (RowItem x) => x.Type == RowItemType.Relative))
			{
				rowItem3.Size *= num4;
			}
		}

		// Token: 0x06000780 RID: 1920 RVA: 0x00012220 File Offset: 0x00010420
		private ICollection<RowItemRenderingCommand> PlanLayout(Size availableSpace)
		{
			float num = 0f;
			List<RowItemRenderingCommand> list = new List<RowItemRenderingCommand>();
			foreach (RowItem rowItem in this.Items)
			{
				Size size = new Size(rowItem.Width, availableSpace.Height);
				RowItemRenderingCommand rowItemRenderingCommand = new RowItemRenderingCommand
				{
					RowItem = rowItem,
					Size = size,
					Measurement = rowItem.Measure(size),
					Offset = new Position(num, 0f)
				};
				list.Add(rowItemRenderingCommand);
				num += rowItem.Width + this.Spacing;
			}
			if (Enumerable.Any<RowItemRenderingCommand>(list, (RowItemRenderingCommand x) => x.Measurement.Type == SpacePlanType.Wrap))
			{
				return list;
			}
			float height = Enumerable.Max(Enumerable.DefaultIfEmpty<float>(Enumerable.Select<RowItemRenderingCommand, float>(Enumerable.Where<RowItemRenderingCommand>(list, (RowItemRenderingCommand x) => !x.RowItem.IsRendered), (RowItemRenderingCommand x) => x.Measurement.Height), 0f));
			foreach (RowItemRenderingCommand rowItemRenderingCommand2 in list)
			{
				rowItemRenderingCommand2.Size = new Size(rowItemRenderingCommand2.Size.Width, height);
				rowItemRenderingCommand2.Measurement = rowItemRenderingCommand2.RowItem.Measure(rowItemRenderingCommand2.Size);
			}
			return list;
		}

		// Token: 0x06000781 RID: 1921 RVA: 0x000123CC File Offset: 0x000105CC
		public void ResetState(bool hardReset = false)
		{
			foreach (RowItem rowItem in this.Items)
			{
				rowItem.IsRendered = false;
				if (rowItem.Type == RowItemType.Auto)
				{
					rowItem.Size = 0f;
				}
			}
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x00012434 File Offset: 0x00010634
		public object GetState()
		{
			bool[] array = new bool[this.Items.Count];
			for (int i = 0; i < this.Items.Count; i++)
			{
				array[i] = this.Items[i].IsRendered;
			}
			return array;
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x00012480 File Offset: 0x00010680
		public void SetState(object state)
		{
			bool[] array = (bool[])state;
			for (int i = 0; i < this.Items.Count; i++)
			{
				this.Items[i].IsRendered = array[i];
			}
		}
	}
}
