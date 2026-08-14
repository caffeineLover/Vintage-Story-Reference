using System;

namespace UglyToad.PdfPig.Actions
{
	// Token: 0x02000209 RID: 521
	public class PdfAction
	{
		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06000C6A RID: 3178 RVA: 0x0003E01B File Offset: 0x0003C21B
		public ActionType Type { get; }

		// Token: 0x06000C6B RID: 3179 RVA: 0x0003E023 File Offset: 0x0003C223
		protected PdfAction(ActionType type)
		{
			this.Type = type;
		}
	}
}
