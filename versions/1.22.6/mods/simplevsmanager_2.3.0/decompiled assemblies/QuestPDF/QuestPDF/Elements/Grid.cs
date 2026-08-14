using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000CF RID: 207
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class Grid : IComponent
	{
		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000657 RID: 1623 RVA: 0x0000F87A File Offset: 0x0000DA7A
		public List<GridElement> Children { get; } = new List<GridElement>();

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000658 RID: 1624 RVA: 0x0000F882 File Offset: 0x0000DA82
		// (set) Token: 0x06000659 RID: 1625 RVA: 0x0000F88A File Offset: 0x0000DA8A
		public Queue<GridElement> ChildrenQueue { get; set; } = new Queue<GridElement>();

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600065A RID: 1626 RVA: 0x0000F893 File Offset: 0x0000DA93
		// (set) Token: 0x0600065B RID: 1627 RVA: 0x0000F89B File Offset: 0x0000DA9B
		public int ColumnsCount { get; set; } = 12;

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x0000F8A4 File Offset: 0x0000DAA4
		// (set) Token: 0x0600065D RID: 1629 RVA: 0x0000F8AC File Offset: 0x0000DAAC
		public HorizontalAlignment Alignment { get; set; }

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600065E RID: 1630 RVA: 0x0000F8B5 File Offset: 0x0000DAB5
		// (set) Token: 0x0600065F RID: 1631 RVA: 0x0000F8BD File Offset: 0x0000DABD
		public float VerticalSpacing { get; set; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000660 RID: 1632 RVA: 0x0000F8C6 File Offset: 0x0000DAC6
		// (set) Token: 0x06000661 RID: 1633 RVA: 0x0000F8CE File Offset: 0x0000DACE
		public float HorizontalSpacing { get; set; }

		// Token: 0x06000662 RID: 1634 RVA: 0x0000F8D7 File Offset: 0x0000DAD7
		public void Compose(IContainer container)
		{
			this.ChildrenQueue = new Queue<GridElement>(this.Children);
			container.Column(delegate(ColumnDescriptor column)
			{
				column.Spacing(this.VerticalSpacing, Unit.Point);
				while (Enumerable.Any<GridElement>(this.ChildrenQueue))
				{
					column.Item().Row(new Action<RowDescriptor>(this.BuildRow));
				}
			});
		}

		// Token: 0x06000663 RID: 1635 RVA: 0x0000F8FC File Offset: 0x0000DAFC
		private IEnumerable<GridElement> GetRowElements()
		{
			Grid.<GetRowElements>d__25 <GetRowElements>d__ = new Grid.<GetRowElements>d__25(-2);
			<GetRowElements>d__.<>4__this = this;
			return <GetRowElements>d__;
		}

		// Token: 0x06000664 RID: 1636 RVA: 0x0000F90C File Offset: 0x0000DB0C
		private void BuildRow(RowDescriptor row)
		{
			row.Spacing(this.HorizontalSpacing, Unit.Point);
			List<GridElement> list = Enumerable.ToList<GridElement>(this.GetRowElements());
			int num = Enumerable.Sum<GridElement>(list, (GridElement x) => x.Columns);
			int num2 = this.ColumnsCount - num;
			bool flag = (float)num2 >= 0.01f;
			if (this.Alignment == HorizontalAlignment.Center)
			{
				num2 /= 2;
			}
			if (flag && this.Alignment != HorizontalAlignment.Left)
			{
				row.RelativeItem((float)num2);
			}
			list.ForEach(delegate(GridElement x)
			{
				row.RelativeItem((float)x.Columns).Element(x.Child);
			});
			if (flag && this.Alignment != HorizontalAlignment.Right)
			{
				row.RelativeItem((float)num2);
			}
		}

		// Token: 0x040002EF RID: 751
		public const int DefaultColumnsCount = 12;
	}
}
