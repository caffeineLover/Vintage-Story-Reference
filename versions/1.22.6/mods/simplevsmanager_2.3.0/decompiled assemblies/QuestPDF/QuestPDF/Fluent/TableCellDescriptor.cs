using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using QuestPDF.Elements.Table;

namespace QuestPDF.Fluent
{
	// Token: 0x020000A6 RID: 166
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TableCellDescriptor
	{
		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x0000C275 File Offset: 0x0000A475
		private ICollection<TableCell> Cells { get; }

		// Token: 0x060004B5 RID: 1205 RVA: 0x0000C27D File Offset: 0x0000A47D
		internal TableCellDescriptor(ICollection<TableCell> cells)
		{
			this.Cells = cells;
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x0000C28C File Offset: 0x0000A48C
		public ITableCellContainer Cell()
		{
			TableCell tableCell = new TableCell();
			this.Cells.Add(tableCell);
			return tableCell;
		}
	}
}
