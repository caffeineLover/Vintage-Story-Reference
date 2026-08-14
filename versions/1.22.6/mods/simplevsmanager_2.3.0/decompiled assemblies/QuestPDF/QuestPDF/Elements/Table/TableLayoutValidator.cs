using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing.Exceptions;

namespace QuestPDF.Elements.Table
{
	// Token: 0x0200010B RID: 267
	[NullableContext(1)]
	[Nullable(0)]
	internal static class TableLayoutValidator
	{
		// Token: 0x0600092F RID: 2351 RVA: 0x00016FED File Offset: 0x000151ED
		public static void ValidateCellPositions(this Table table)
		{
			TableLayoutValidator.ValidateCellPositions(table.Columns.Count, table.Cells);
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x00017008 File Offset: 0x00015208
		private static void ValidateCellPositions(int columnsCount, ICollection<TableCell> cells)
		{
			foreach (TableCell tableCell in cells)
			{
				if (tableCell.Column < 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(62, 2);
					defaultInterpolatedStringHandler.AppendFormatted("Detected issue in table cells configuration.");
					defaultInterpolatedStringHandler.AppendLiteral(" A cell column position should be greater or equal to 1. Got ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(tableCell.Column);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new DocumentComposeException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				if (tableCell.Row < 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(59, 2);
					defaultInterpolatedStringHandler2.AppendFormatted("Detected issue in table cells configuration.");
					defaultInterpolatedStringHandler2.AppendLiteral(" A cell row position should be greater or equal to 1. Got ");
					defaultInterpolatedStringHandler2.AppendFormatted<int>(tableCell.Row);
					defaultInterpolatedStringHandler2.AppendLiteral(".");
					throw new DocumentComposeException(defaultInterpolatedStringHandler2.ToStringAndClear());
				}
				if (tableCell.ColumnSpan < 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(44, 2);
					defaultInterpolatedStringHandler3.AppendFormatted("Detected issue in table cells configuration.");
					defaultInterpolatedStringHandler3.AppendLiteral(" A cell must span at least one column. Got ");
					defaultInterpolatedStringHandler3.AppendFormatted<int>(tableCell.ColumnSpan);
					defaultInterpolatedStringHandler3.AppendLiteral(".");
					throw new DocumentComposeException(defaultInterpolatedStringHandler3.ToStringAndClear());
				}
				if (tableCell.RowSpan < 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(41, 2);
					defaultInterpolatedStringHandler4.AppendFormatted("Detected issue in table cells configuration.");
					defaultInterpolatedStringHandler4.AppendLiteral(" A cell must span at least one row. Got ");
					defaultInterpolatedStringHandler4.AppendFormatted<int>(tableCell.RowSpan);
					defaultInterpolatedStringHandler4.AppendLiteral(".");
					throw new DocumentComposeException(defaultInterpolatedStringHandler4.ToStringAndClear());
				}
				if (tableCell.Column > columnsCount)
				{
					throw new DocumentComposeException("Detected issue in table cells configuration. Cell starts at column that does not exist. Cell details: " + TableLayoutValidator.<ValidateCellPositions>g__GetCellDetails|1_0(tableCell) + ".");
				}
				if (tableCell.Column + tableCell.ColumnSpan - 1 > columnsCount)
				{
					throw new DocumentComposeException("Detected issue in table cells configuration. Table cell location is incorrect. Cell spans over columns that do not exist. Cell details: " + TableLayoutValidator.<ValidateCellPositions>g__GetCellDetails|1_0(tableCell) + ".");
				}
			}
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x000171F0 File Offset: 0x000153F0
		[CompilerGenerated]
		internal static string <ValidateCellPositions>g__GetCellDetails|1_0(TableCell cell)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(36, 4);
			defaultInterpolatedStringHandler.AppendLiteral("Row ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(cell.Row);
			defaultInterpolatedStringHandler.AppendLiteral(", Column ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(cell.Column);
			defaultInterpolatedStringHandler.AppendLiteral(", RowSpan ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(cell.RowSpan);
			defaultInterpolatedStringHandler.AppendLiteral(", ColumnSpan ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(cell.ColumnSpan);
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
