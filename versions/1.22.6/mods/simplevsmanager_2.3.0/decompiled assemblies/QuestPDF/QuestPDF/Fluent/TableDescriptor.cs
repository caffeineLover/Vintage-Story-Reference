using System;
using System.Linq;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing.Exceptions;
using QuestPDF.Elements;
using QuestPDF.Elements.Table;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x020000A7 RID: 167
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TableDescriptor
	{
		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x0000C2AC File Offset: 0x0000A4AC
		// (set) Token: 0x060004B8 RID: 1208 RVA: 0x0000C2B4 File Offset: 0x0000A4B4
		internal bool EnableAutomatedSemanticTagging { get; set; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x0000C2BD File Offset: 0x0000A4BD
		private Table HeaderTable { get; } = new Table();

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060004BA RID: 1210 RVA: 0x0000C2C5 File Offset: 0x0000A4C5
		private Table ContentTable { get; } = new Table();

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x0000C2CD File Offset: 0x0000A4CD
		private Table FooterTable { get; } = new Table();

		// Token: 0x060004BC RID: 1212 RVA: 0x0000C2D5 File Offset: 0x0000A4D5
		internal TableDescriptor()
		{
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0000C300 File Offset: 0x0000A500
		public void ColumnsDefinition(Action<TableColumnsDefinitionDescriptor> handler)
		{
			if (Enumerable.Any<TableColumnDefinition>(this.ContentTable.Columns))
			{
				throw new DocumentComposeException("Table columns have already been defined. Please call the 'Table.ColumnsDefinition' method only once.");
			}
			TableColumnsDefinitionDescriptor tableColumnsDefinitionDescriptor = new TableColumnsDefinitionDescriptor();
			handler.Invoke(tableColumnsDefinitionDescriptor);
			this.HeaderTable.Columns = tableColumnsDefinitionDescriptor.Columns;
			this.ContentTable.Columns = tableColumnsDefinitionDescriptor.Columns;
			this.FooterTable.Columns = tableColumnsDefinitionDescriptor.Columns;
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x0000C36A File Offset: 0x0000A56A
		public void ExtendLastCellsToTableBottom()
		{
			this.ContentTable.ExtendLastCellsToTableBottom = true;
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x0000C378 File Offset: 0x0000A578
		public void Header(Action<TableCellDescriptor> handler)
		{
			if (Enumerable.Any<TableCell>(this.HeaderTable.Cells))
			{
				throw new DocumentComposeException("The 'Table.Header' layer has already been defined. Please call this method only once.");
			}
			TableCellDescriptor tableCellDescriptor = new TableCellDescriptor(this.HeaderTable.Cells);
			handler.Invoke(tableCellDescriptor);
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0000C3BC File Offset: 0x0000A5BC
		public void Footer(Action<TableCellDescriptor> handler)
		{
			if (Enumerable.Any<TableCell>(this.FooterTable.Cells))
			{
				throw new DocumentComposeException("The 'Table.Footer' layer has already been defined. Please call this method only once.");
			}
			TableCellDescriptor tableCellDescriptor = new TableCellDescriptor(this.FooterTable.Cells);
			handler.Invoke(tableCellDescriptor);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0000C400 File Offset: 0x0000A600
		public ITableCellContainer Cell()
		{
			TableCell tableCell = new TableCell();
			this.ContentTable.Cells.Add(tableCell);
			return tableCell;
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0000C428 File Offset: 0x0000A628
		internal IElement CreateElement()
		{
			TableDescriptor.<>c__DisplayClass19_0 CS$<>8__locals1 = new TableDescriptor.<>c__DisplayClass19_0();
			CS$<>8__locals1.<>4__this = this;
			Container container = new Container();
			CS$<>8__locals1.hasHeader = Enumerable.Any<TableCell>(this.HeaderTable.Cells);
			CS$<>8__locals1.hasFooter = Enumerable.Any<TableCell>(this.FooterTable.Cells);
			CS$<>8__locals1.<CreateElement>g__ConfigureTable|1(this.HeaderTable, Table.TablePartType.Header);
			CS$<>8__locals1.<CreateElement>g__ConfigureTable|1(this.ContentTable, Table.TablePartType.Body);
			CS$<>8__locals1.<CreateElement>g__ConfigureTable|1(this.FooterTable, Table.TablePartType.Footer);
			bool tableRequiresAdvancedHeaderTagging = Table.DoesTableBodyRequireExtendedHeaderTagging(this.HeaderTable.Cells, this.ContentTable.Cells);
			this.HeaderTable.TableRequiresAdvancedHeaderTagging = tableRequiresAdvancedHeaderTagging;
			this.ContentTable.TableRequiresAdvancedHeaderTagging = tableRequiresAdvancedHeaderTagging;
			this.ContentTable.HeaderCells = this.HeaderTable.Cells;
			container.Decoration(delegate(DecorationDescriptor decoration)
			{
				IContainer parent = decoration.Before().ShowIf(CS$<>8__locals1.hasHeader);
				Func<IContainer, IContainer> handler;
				if ((handler = CS$<>8__locals1.<>9__2) == null)
				{
					handler = (CS$<>8__locals1.<>9__2 = delegate(IContainer x)
					{
						if (!CS$<>8__locals1.<>4__this.EnableAutomatedSemanticTagging)
						{
							return x;
						}
						return x.SemanticTag("THead", null, null);
					});
				}
				parent.NonTrackingElement(handler).Element(CS$<>8__locals1.<>4__this.HeaderTable);
				IContainer parent2 = decoration.Content();
				Func<IContainer, IContainer> handler2;
				if ((handler2 = CS$<>8__locals1.<>9__3) == null)
				{
					handler2 = (CS$<>8__locals1.<>9__3 = delegate(IContainer x)
					{
						if (!CS$<>8__locals1.<>4__this.EnableAutomatedSemanticTagging)
						{
							return x;
						}
						return x.SemanticTag("TBody", null, null);
					});
				}
				parent2.NonTrackingElement(handler2).ShowIf(Enumerable.Any<TableCell>(CS$<>8__locals1.<>4__this.ContentTable.Cells)).Element(CS$<>8__locals1.<>4__this.ContentTable);
				IContainer parent3 = decoration.After().ShowIf(CS$<>8__locals1.hasFooter);
				Func<IContainer, IContainer> handler3;
				if ((handler3 = CS$<>8__locals1.<>9__4) == null)
				{
					handler3 = (CS$<>8__locals1.<>9__4 = delegate(IContainer x)
					{
						if (!CS$<>8__locals1.<>4__this.EnableAutomatedSemanticTagging)
						{
							return x;
						}
						return x.SemanticTag("TFoot", null, null);
					});
				}
				parent3.NonTrackingElement(handler3).Element(CS$<>8__locals1.<>4__this.FooterTable);
			});
			return container;
		}
	}
}
