using System;
using UglyToad.PdfPig.Outline.Destinations;

namespace UglyToad.PdfPig.Actions
{
	// Token: 0x02000207 RID: 519
	public class GoToEAction : AbstractGoToAction
	{
		// Token: 0x1700039C RID: 924
		// (get) Token: 0x06000C66 RID: 3174 RVA: 0x0003DFE9 File Offset: 0x0003C1E9
		public string FileSpecification { get; }

		// Token: 0x06000C67 RID: 3175 RVA: 0x0003DFF1 File Offset: 0x0003C1F1
		public GoToEAction(ExplicitDestination destination, string fileSpecification) : base(ActionType.GoToE, destination)
		{
			this.FileSpecification = fileSpecification;
		}
	}
}
