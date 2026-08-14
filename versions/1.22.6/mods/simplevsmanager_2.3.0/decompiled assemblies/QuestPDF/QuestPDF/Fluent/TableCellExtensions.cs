using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements.Table;

namespace QuestPDF.Fluent
{
	// Token: 0x020000A9 RID: 169
	[NullableContext(1)]
	[Nullable(0)]
	public static class TableCellExtensions
	{
		// Token: 0x060004C4 RID: 1220 RVA: 0x0000C544 File Offset: 0x0000A744
		public static ITableCellContainer Column(this ITableCellContainer tableCellContainer, uint value)
		{
			TableCell tableCell = tableCellContainer as TableCell;
			if (tableCell != null)
			{
				tableCell.Column = (int)value;
			}
			return tableCellContainer;
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x0000C564 File Offset: 0x0000A764
		public static ITableCellContainer ColumnSpan(this ITableCellContainer tableCellContainer, uint value)
		{
			TableCell tableCell = tableCellContainer as TableCell;
			if (tableCell != null)
			{
				tableCell.ColumnSpan = (int)value;
			}
			return tableCellContainer;
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0000C584 File Offset: 0x0000A784
		public static ITableCellContainer Row(this ITableCellContainer tableCellContainer, uint value)
		{
			TableCell tableCell = tableCellContainer as TableCell;
			if (tableCell != null)
			{
				tableCell.Row = (int)value;
			}
			return tableCellContainer;
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0000C5A4 File Offset: 0x0000A7A4
		public static ITableCellContainer RowSpan(this ITableCellContainer tableCellContainer, uint value)
		{
			TableCell tableCell = tableCellContainer as TableCell;
			if (tableCell != null)
			{
				tableCell.RowSpan = (int)value;
			}
			return tableCellContainer;
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0000C5C4 File Offset: 0x0000A7C4
		public static ITableCellContainer AsSemanticHorizontalHeader(this ITableCellContainer tableCellContainer)
		{
			TableCell tableCell = tableCellContainer as TableCell;
			if (tableCell != null)
			{
				tableCell.IsSemanticHorizontalHeader = true;
			}
			return tableCellContainer;
		}
	}
}
