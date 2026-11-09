using System;

namespace QuestPDF.Skia
{
	// Token: 0x02000019 RID: 25
	internal struct SkRect
	{
		// Token: 0x06000098 RID: 152 RVA: 0x000032F0 File Offset: 0x000014F0
		public SkRect(float left, float top, float right, float bottom)
		{
			this.Left = left;
			this.Top = top;
			this.Right = right;
			this.Bottom = bottom;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000099 RID: 153 RVA: 0x0000330F File Offset: 0x0000150F
		public float Width
		{
			get
			{
				return this.Right - this.Left;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600009A RID: 154 RVA: 0x0000331E File Offset: 0x0000151E
		public float Height
		{
			get
			{
				return this.Bottom - this.Top;
			}
		}

		// Token: 0x04000057 RID: 87
		public float Left;

		// Token: 0x04000058 RID: 88
		public float Top;

		// Token: 0x04000059 RID: 89
		public float Right;

		// Token: 0x0400005A RID: 90
		public float Bottom;
	}
}
