using System;
using UglyToad.PdfPig.Outline.Destinations;

namespace UglyToad.PdfPig.Actions
{
	// Token: 0x02000203 RID: 515
	public abstract class AbstractGoToAction : PdfAction
	{
		// Token: 0x1700039B RID: 923
		// (get) Token: 0x06000C62 RID: 3170 RVA: 0x0003DE7E File Offset: 0x0003C07E
		public ExplicitDestination Destination { get; }

		// Token: 0x06000C63 RID: 3171 RVA: 0x0003DE86 File Offset: 0x0003C086
		protected AbstractGoToAction(ActionType type, ExplicitDestination destination) : base(type)
		{
			this.Destination = destination;
		}
	}
}
