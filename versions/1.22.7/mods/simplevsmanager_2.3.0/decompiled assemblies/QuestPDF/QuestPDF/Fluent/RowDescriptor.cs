using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using QuestPDF.Elements;
using QuestPDF.Infrastructure;

namespace QuestPDF.Fluent
{
	// Token: 0x0200009C RID: 156
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class RowDescriptor
	{
		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000450 RID: 1104 RVA: 0x0000B4CA File Offset: 0x000096CA
		internal Row Row { get; } = new Row();

		// Token: 0x06000451 RID: 1105 RVA: 0x0000B4D2 File Offset: 0x000096D2
		internal RowDescriptor()
		{
		}

		// Token: 0x06000452 RID: 1106 RVA: 0x0000B4E5 File Offset: 0x000096E5
		public void Spacing(float spacing, Unit unit = Unit.Point)
		{
			if (spacing < 0f)
			{
				throw new ArgumentOutOfRangeException("spacing", "The row spacing cannot be negative.");
			}
			this.Row.Spacing = spacing.ToPoints(unit);
		}

		// Token: 0x06000453 RID: 1107 RVA: 0x0000B514 File Offset: 0x00009714
		private IContainer Item(RowItemType type, float size = 0f)
		{
			RowItem rowItem = new RowItem
			{
				Type = type,
				Size = size
			};
			this.Row.Items.Add(rowItem);
			return rowItem;
		}

		// Token: 0x06000454 RID: 1108 RVA: 0x0000B547 File Offset: 0x00009747
		[Obsolete("This element has been renamed since version 2022.2. Please use the RelativeItem method.")]
		[ExcludeFromCodeCoverage]
		public IContainer RelativeColumn(float size = 1f)
		{
			return this.Item(RowItemType.Relative, size);
		}

		// Token: 0x06000455 RID: 1109 RVA: 0x0000B551 File Offset: 0x00009751
		[Obsolete("This element has been renamed since version 2022.2. Please use the ConstantItem method.")]
		[ExcludeFromCodeCoverage]
		public IContainer ConstantColumn(float size)
		{
			return this.Item(RowItemType.Constant, size);
		}

		// Token: 0x06000456 RID: 1110 RVA: 0x0000B55B File Offset: 0x0000975B
		public IContainer RelativeItem(float size = 1f)
		{
			if (size <= 0f)
			{
				throw new ArgumentOutOfRangeException("size", "The relative item size must be greater than zero.");
			}
			return this.Item(RowItemType.Relative, size);
		}

		// Token: 0x06000457 RID: 1111 RVA: 0x0000B57D File Offset: 0x0000977D
		public IContainer ConstantItem(float size, Unit unit = Unit.Point)
		{
			if (size < 0f)
			{
				throw new ArgumentOutOfRangeException("size", "The constant item size cannot be negative.");
			}
			return this.Item(RowItemType.Constant, size.ToPoints(unit));
		}

		// Token: 0x06000458 RID: 1112 RVA: 0x0000B5A5 File Offset: 0x000097A5
		public IContainer AutoItem()
		{
			return this.Item(RowItemType.Auto, 0f);
		}
	}
}
