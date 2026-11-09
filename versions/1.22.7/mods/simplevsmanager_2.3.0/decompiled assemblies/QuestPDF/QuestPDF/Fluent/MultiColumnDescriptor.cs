using System;
using System.Runtime.CompilerServices;
using QuestPDF.Drawing.Exceptions;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x02000096 RID: 150
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class MultiColumnDescriptor
	{
		// Token: 0x17000107 RID: 263
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x0000AEE1 File Offset: 0x000090E1
		internal MultiColumn MultiColumn { get; } = new MultiColumn();

		// Token: 0x06000423 RID: 1059 RVA: 0x0000AEE9 File Offset: 0x000090E9
		internal MultiColumnDescriptor()
		{
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x0000AEFC File Offset: 0x000090FC
		public void Spacing(float value, Unit unit = Unit.Point)
		{
			this.MultiColumn.Spacing = value.ToPoints(unit);
		}

		// Token: 0x06000425 RID: 1061 RVA: 0x0000AF10 File Offset: 0x00009110
		public void Columns(int value = 2)
		{
			if (value < 2)
			{
				throw new DocumentComposeException("The 'MultiColumn.Columns' value should be higher than 1.");
			}
			this.MultiColumn.ColumnCount = value;
		}

		// Token: 0x06000426 RID: 1062 RVA: 0x0000AF2D File Offset: 0x0000912D
		public void BalanceHeight(bool enable = true)
		{
			this.MultiColumn.BalanceHeight = enable;
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x0000AF3C File Offset: 0x0000913C
		public IContainer Content()
		{
			if (!(this.MultiColumn.Content is Empty))
			{
				throw new DocumentComposeException("The 'MultiColumn.Content' layer has already been defined. Please call this method only once.");
			}
			Container container = new Container();
			this.MultiColumn.Content = container;
			return container;
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0000AF7C File Offset: 0x0000917C
		public IContainer Spacer()
		{
			if (!(this.MultiColumn.Spacer is Empty))
			{
				throw new DocumentComposeException("The 'MultiColumn.Spacer' layer has already been defined. Please call this method only once.");
			}
			Container container = new Container();
			this.MultiColumn.Spacer = container;
			return container.Artifact(-6).Repeat();
		}
	}
}
