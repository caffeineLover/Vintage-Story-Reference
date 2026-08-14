using System;
using UglyToad.PdfPig.Outline.Destinations;

namespace UglyToad.PdfPig.Actions
{
	// Token: 0x02000208 RID: 520
	public class GoToRAction : AbstractGoToAction
	{
		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06000C68 RID: 3176 RVA: 0x0003E002 File Offset: 0x0003C202
		public string Filename { get; }

		// Token: 0x06000C69 RID: 3177 RVA: 0x0003E00A File Offset: 0x0003C20A
		public GoToRAction(ExplicitDestination destination, string filename) : base(ActionType.GoToR, destination)
		{
			this.Filename = filename;
		}
	}
}
