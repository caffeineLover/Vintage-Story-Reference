using System;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x0200008A RID: 138
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class GridDescriptor
	{
		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060003CB RID: 971 RVA: 0x0000A564 File Offset: 0x00008764
		internal Grid Grid { get; } = new Grid();

		// Token: 0x060003CC RID: 972 RVA: 0x0000A56C File Offset: 0x0000876C
		internal GridDescriptor()
		{
		}

		// Token: 0x060003CD RID: 973 RVA: 0x0000A57F File Offset: 0x0000877F
		public void Spacing(float value, Unit unit = Unit.Point)
		{
			this.VerticalSpacing(value, unit);
			this.HorizontalSpacing(value, unit);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x0000A591 File Offset: 0x00008791
		public void VerticalSpacing(float value, Unit unit = Unit.Point)
		{
			if (value < 0f)
			{
				throw new ArgumentOutOfRangeException("value", "The Grid vertical spacing cannot be negative.");
			}
			this.Grid.VerticalSpacing = value.ToPoints(unit);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x0000A5BD File Offset: 0x000087BD
		public void HorizontalSpacing(float value, Unit unit = Unit.Point)
		{
			if (value < 0f)
			{
				throw new ArgumentOutOfRangeException("value", "The Grid horizontal spacing cannot be negative.");
			}
			this.Grid.HorizontalSpacing = value.ToPoints(unit);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x0000A5E9 File Offset: 0x000087E9
		public void Columns(int value = 12)
		{
			if (value < 1)
			{
				throw new ArgumentOutOfRangeException("value", "The Grid columns count cannot be less than 1.");
			}
			this.Grid.ColumnsCount = value;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0000A60B File Offset: 0x0000880B
		public void Alignment(HorizontalAlignment alignment)
		{
			this.Grid.Alignment = alignment;
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000A619 File Offset: 0x00008819
		public void AlignLeft()
		{
			this.Alignment(HorizontalAlignment.Left);
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000A622 File Offset: 0x00008822
		public void AlignCenter()
		{
			this.Alignment(HorizontalAlignment.Center);
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000A62B File Offset: 0x0000882B
		public void AlignRight()
		{
			this.Alignment(HorizontalAlignment.Right);
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x0000A634 File Offset: 0x00008834
		public IContainer Item(int columns = 1)
		{
			if (columns < 1)
			{
				throw new ArgumentOutOfRangeException("columns", "The Grid item cannot span less than 1 column.");
			}
			Container container = new Container();
			GridElement gridElement = new GridElement
			{
				Columns = columns,
				Child = container
			};
			this.Grid.Children.Add(gridElement);
			return container;
		}
	}
}
