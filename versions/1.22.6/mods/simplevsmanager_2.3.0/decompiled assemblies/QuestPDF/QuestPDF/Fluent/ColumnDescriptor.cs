using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x0200007D RID: 125
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ColumnDescriptor
	{
		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000358 RID: 856 RVA: 0x00008F6B File Offset: 0x0000716B
		internal Column Column { get; } = new Column();

		// Token: 0x06000359 RID: 857 RVA: 0x00008F73 File Offset: 0x00007173
		internal ColumnDescriptor()
		{
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00008F86 File Offset: 0x00007186
		public void Spacing(float value, Unit unit = Unit.Point)
		{
			if (value < 0f)
			{
				throw new ArgumentOutOfRangeException("value", "The column spacing cannot be negative.");
			}
			this.Column.Spacing = value.ToPoints(unit);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00008FB4 File Offset: 0x000071B4
		public IContainer Item()
		{
			Container container = new Container();
			this.Column.Items.Add(container);
			return container;
		}
	}
}
