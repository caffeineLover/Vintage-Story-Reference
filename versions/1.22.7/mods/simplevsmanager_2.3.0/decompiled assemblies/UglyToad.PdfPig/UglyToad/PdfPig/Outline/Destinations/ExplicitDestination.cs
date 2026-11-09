using System;

namespace UglyToad.PdfPig.Outline.Destinations
{
	// Token: 0x020000E1 RID: 225
	public class ExplicitDestination
	{
		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x0001A3E3 File Offset: 0x000185E3
		public int PageNumber { get; }

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x0001A3EB File Offset: 0x000185EB
		public ExplicitDestinationType Type { get; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x0001A3F3 File Offset: 0x000185F3
		public ExplicitDestinationCoordinates Coordinates { get; }

		// Token: 0x0600042D RID: 1069 RVA: 0x0001A3FB File Offset: 0x000185FB
		public ExplicitDestination(int pageNumber, ExplicitDestinationType type, ExplicitDestinationCoordinates coordinates)
		{
			this.PageNumber = pageNumber;
			this.Type = type;
			this.Coordinates = coordinates;
		}
	}
}
