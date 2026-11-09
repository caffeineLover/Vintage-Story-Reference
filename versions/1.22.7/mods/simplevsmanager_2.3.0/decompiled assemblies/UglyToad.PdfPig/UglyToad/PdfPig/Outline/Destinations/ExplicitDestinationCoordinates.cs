using System;

namespace UglyToad.PdfPig.Outline.Destinations
{
	// Token: 0x020000E2 RID: 226
	public class ExplicitDestinationCoordinates
	{
		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x0001A418 File Offset: 0x00018618
		public static ExplicitDestinationCoordinates Empty { get; } = new ExplicitDestinationCoordinates(default(decimal?), default(decimal?), default(decimal?), default(decimal?));

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x0001A41F File Offset: 0x0001861F
		public decimal? Left { get; }

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x0001A427 File Offset: 0x00018627
		public decimal? Top { get; }

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000431 RID: 1073 RVA: 0x0001A42F File Offset: 0x0001862F
		public decimal? Right { get; }

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000432 RID: 1074 RVA: 0x0001A437 File Offset: 0x00018637
		public decimal? Bottom { get; }

		// Token: 0x06000433 RID: 1075 RVA: 0x0001A43F File Offset: 0x0001863F
		public ExplicitDestinationCoordinates(decimal? left)
		{
			this.Left = left;
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x0001A44E File Offset: 0x0001864E
		public ExplicitDestinationCoordinates(decimal? left, decimal? top)
		{
			this.Left = left;
			this.Top = top;
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x0001A464 File Offset: 0x00018664
		public ExplicitDestinationCoordinates(decimal? left, decimal? top, decimal? right, decimal? bottom)
		{
			this.Left = left;
			this.Top = top;
			this.Right = right;
			this.Bottom = bottom;
		}
	}
}
