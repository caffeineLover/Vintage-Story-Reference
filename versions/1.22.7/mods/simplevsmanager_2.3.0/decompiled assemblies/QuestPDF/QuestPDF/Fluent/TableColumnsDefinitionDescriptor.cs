using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using QuestPDF.Elements.Table;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x020000A5 RID: 165
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class TableColumnsDefinitionDescriptor
	{
		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x0000C1E6 File Offset: 0x0000A3E6
		internal List<TableColumnDefinition> Columns { get; } = new List<TableColumnDefinition>();

		// Token: 0x060004B0 RID: 1200 RVA: 0x0000C1EE File Offset: 0x0000A3EE
		internal TableColumnsDefinitionDescriptor()
		{
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x0000C201 File Offset: 0x0000A401
		public void ConstantColumn(float width, Unit unit = Unit.Point)
		{
			if (width <= 0f)
			{
				throw new ArgumentOutOfRangeException("width", "Constant column width must be greater than zero.");
			}
			this.ComplexColumn(width.ToPoints(unit), 0f);
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x0000C22D File Offset: 0x0000A42D
		public void RelativeColumn(float width = 1f)
		{
			if (width <= 0f)
			{
				throw new ArgumentOutOfRangeException("width", "Relative column width must be greater than zero.");
			}
			this.ComplexColumn(0f, width);
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0000C254 File Offset: 0x0000A454
		private void ComplexColumn(float constantWidth = 0f, float relativeWidth = 0f)
		{
			TableColumnDefinition tableColumnDefinition = new TableColumnDefinition(constantWidth, relativeWidth);
			this.Columns.Add(tableColumnDefinition);
		}
	}
}
