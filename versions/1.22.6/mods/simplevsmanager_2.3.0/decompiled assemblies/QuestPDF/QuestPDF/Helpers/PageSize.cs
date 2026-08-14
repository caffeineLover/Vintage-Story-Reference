using System;
using System.Runtime.CompilerServices;
using QuestPDF.Infrastructure;

namespace QuestPDF.Helpers
{
	// Token: 0x02000077 RID: 119
	public sealed class PageSize
	{
		// Token: 0x060002F6 RID: 758 RVA: 0x00007E94 File Offset: 0x00006094
		public PageSize(float width, float height, Unit unit = Unit.Point)
		{
			if (width < 0f)
			{
				throw new ArgumentOutOfRangeException("width", "Page width must be greater than 0.");
			}
			if (height < 0f)
			{
				throw new ArgumentOutOfRangeException("height", "Page height must be greater than 0.");
			}
			this.Width = width.ToPoints(unit);
			this.Height = height.ToPoints(unit);
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00007EF1 File Offset: 0x000060F1
		[NullableContext(1)]
		public static implicit operator Size(PageSize pageSize)
		{
			return new Size(pageSize.Width, pageSize.Height);
		}

		// Token: 0x04000234 RID: 564
		public readonly float Width;

		// Token: 0x04000235 RID: 565
		public readonly float Height;
	}
}
