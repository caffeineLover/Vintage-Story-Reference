using System;
using System.Runtime.CompilerServices;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace QuestPDF.Elements
{
	// Token: 0x020000E4 RID: 228
	internal sealed class RowItem : ContainerElement
	{
		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x00011BF9 File Offset: 0x0000FDF9
		// (set) Token: 0x06000765 RID: 1893 RVA: 0x00011C01 File Offset: 0x0000FE01
		public bool IsRendered { get; set; }

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x00011C0A File Offset: 0x0000FE0A
		// (set) Token: 0x06000767 RID: 1895 RVA: 0x00011C12 File Offset: 0x0000FE12
		public float Width { get; set; }

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000768 RID: 1896 RVA: 0x00011C1B File Offset: 0x0000FE1B
		// (set) Token: 0x06000769 RID: 1897 RVA: 0x00011C23 File Offset: 0x0000FE23
		public RowItemType Type { get; set; }

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600076A RID: 1898 RVA: 0x00011C2C File Offset: 0x0000FE2C
		// (set) Token: 0x0600076B RID: 1899 RVA: 0x00011C34 File Offset: 0x0000FE34
		public float Size { get; set; }

		// Token: 0x0600076C RID: 1900 RVA: 0x00011C40 File Offset: 0x0000FE40
		[NullableContext(2)]
		internal override string GetCompanionHint()
		{
			if (this.Type == RowItemType.Auto)
			{
				return "Auto";
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(1, 2);
			defaultInterpolatedStringHandler.AppendFormatted<RowItemType>(this.Type);
			defaultInterpolatedStringHandler.AppendLiteral(" ");
			defaultInterpolatedStringHandler.AppendFormatted(this.Size.FormatAsCompanionNumber());
			return defaultInterpolatedStringHandler.ToStringAndClear();
		}
	}
}
