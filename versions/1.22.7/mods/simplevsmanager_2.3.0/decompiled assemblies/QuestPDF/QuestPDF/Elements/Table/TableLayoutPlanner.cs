using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace QuestPDF.Elements.Table
{
	// Token: 0x0200010A RID: 266
	[NullableContext(1)]
	[Nullable(0)]
	internal static class TableLayoutPlanner
	{
		// Token: 0x06000928 RID: 2344 RVA: 0x00016D67 File Offset: 0x00014F67
		public static void PlanCellPositions(this Table table)
		{
			TableLayoutPlanner.PlanCellPositions(table.Columns.Count, table.Cells);
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x00016D80 File Offset: 0x00014F80
		private static void PlanCellPositions(int columnsCount, ICollection<TableCell> cells)
		{
			List<TableCell> list = new List<TableCell>();
			ValueTuple<int, int> currentLocation = new ValueTuple<int, int>(1, 1);
			int num = 0;
			Func<TableCell, bool> <>9__0;
			foreach (TableCell tableCell in cells)
			{
				tableCell.ZIndex = num;
				num++;
				if (list.Count > Math.Max(columnsCount, 16))
				{
					IEnumerable<TableCell> enumerable = list;
					Func<TableCell, bool> func;
					if ((func = <>9__0) == null)
					{
						func = (<>9__0 = ((TableCell x) => x.Row + x.RowSpan > currentLocation.Item2));
					}
					list = Enumerable.ToList<TableCell>(Enumerable.Where<TableCell>(enumerable, func));
				}
				tableCell.SetPartialLocation();
				if (tableCell.HasLocation())
				{
					list.Add(tableCell);
					currentLocation = new ValueTuple<int, int>(tableCell.Column, tableCell.Row);
				}
				else
				{
					foreach (ValueTuple<int, int> valueTuple in TableLayoutPlanner.GenerateCoordinates(columnsCount, currentLocation))
					{
						if (valueTuple.Item1 + tableCell.ColumnSpan - 1 <= columnsCount)
						{
							tableCell.Column = valueTuple.Item1;
							tableCell.Row = valueTuple.Item2;
							if (!tableCell.CollidesWithAnyOf(list))
							{
								list.Add(tableCell);
								currentLocation = new ValueTuple<int, int>(tableCell.Column, tableCell.Row);
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x00016F18 File Offset: 0x00015118
		[NullableContext(0)]
		[return: TupleElementNames(new string[]
		{
			"x",
			"y"
		})]
		[return: Nullable(new byte[]
		{
			1,
			0
		})]
		private static IEnumerable<ValueTuple<int, int>> GenerateCoordinates(int columnsCount, [TupleElementNames(new string[]
		{
			"x",
			"y"
		})] ValueTuple<int, int> startPosition)
		{
			TableLayoutPlanner.<GenerateCoordinates>d__2 <GenerateCoordinates>d__ = new TableLayoutPlanner.<GenerateCoordinates>d__2(-2);
			<GenerateCoordinates>d__.<>3__columnsCount = columnsCount;
			<GenerateCoordinates>d__.<>3__startPosition = startPosition;
			return <GenerateCoordinates>d__;
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x00016F30 File Offset: 0x00015130
		private static bool CollidesWith(this TableCell cell, TableCell neighbour)
		{
			return cell.Column < neighbour.Column + neighbour.ColumnSpan && cell.Column + cell.ColumnSpan > neighbour.Column && cell.Row < neighbour.Row + neighbour.RowSpan && cell.RowSpan + cell.Row > neighbour.Row;
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x00016F93 File Offset: 0x00015193
		private static bool CollidesWithAnyOf(this TableCell cell, ICollection<TableCell> neighbours)
		{
			return Enumerable.Any<TableCell>(neighbours, new Func<TableCell, bool>(cell.CollidesWith));
		}

		// Token: 0x0600092D RID: 2349 RVA: 0x00016FA7 File Offset: 0x000151A7
		private static void SetPartialLocation(this TableCell cell)
		{
			if (cell.Row == 0 && cell.Column == 0)
			{
				return;
			}
			if (cell.Row == 0)
			{
				cell.Row = 1;
			}
			if (cell.Column == 0)
			{
				cell.Column = 1;
			}
		}

		// Token: 0x0600092E RID: 2350 RVA: 0x00016FD8 File Offset: 0x000151D8
		private static bool HasLocation(this TableCell cell)
		{
			return cell.Row != 0 && cell.Column != 0;
		}
	}
}
