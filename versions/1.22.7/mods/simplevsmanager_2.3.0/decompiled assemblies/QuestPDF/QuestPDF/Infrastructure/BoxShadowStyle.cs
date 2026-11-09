using System;
using QuestPDF.Helpers;

namespace QuestPDF.Infrastructure
{
	// Token: 0x02000038 RID: 56
	public sealed class BoxShadowStyle
	{
		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000147 RID: 327 RVA: 0x000049C6 File Offset: 0x00002BC6
		// (set) Token: 0x06000148 RID: 328 RVA: 0x000049CE File Offset: 0x00002BCE
		public float OffsetX { get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000149 RID: 329 RVA: 0x000049D7 File Offset: 0x00002BD7
		// (set) Token: 0x0600014A RID: 330 RVA: 0x000049DF File Offset: 0x00002BDF
		public float OffsetY { get; set; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600014B RID: 331 RVA: 0x000049E8 File Offset: 0x00002BE8
		// (set) Token: 0x0600014C RID: 332 RVA: 0x000049F0 File Offset: 0x00002BF0
		public float Spread { get; set; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x0600014D RID: 333 RVA: 0x000049F9 File Offset: 0x00002BF9
		// (set) Token: 0x0600014E RID: 334 RVA: 0x00004A01 File Offset: 0x00002C01
		public float Blur { get; set; }

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600014F RID: 335 RVA: 0x00004A0A File Offset: 0x00002C0A
		// (set) Token: 0x06000150 RID: 336 RVA: 0x00004A12 File Offset: 0x00002C12
		public Color Color { get; set; } = Colors.Grey.Medium;

		// Token: 0x06000151 RID: 337 RVA: 0x00004A1B File Offset: 0x00002C1B
		internal void Validate()
		{
			if (this.Blur < 0f)
			{
				throw new ArgumentException("Shadow blur radius cannot be negative.");
			}
		}
	}
}
