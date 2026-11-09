using System;
using UglyToad.PdfPig.Outline.Destinations;

namespace UglyToad.PdfPig.Actions
{
	// Token: 0x02000206 RID: 518
	public class GoToAction : AbstractGoToAction
	{
		// Token: 0x06000C65 RID: 3173 RVA: 0x0003DFDF File Offset: 0x0003C1DF
		public GoToAction(ExplicitDestination destination) : base(ActionType.GoTo, destination)
		{
		}
	}
}
