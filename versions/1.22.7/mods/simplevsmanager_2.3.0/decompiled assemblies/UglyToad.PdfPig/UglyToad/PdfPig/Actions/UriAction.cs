using System;

namespace UglyToad.PdfPig.Actions
{
	// Token: 0x0200020A RID: 522
	public class UriAction : PdfAction
	{
		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06000C6C RID: 3180 RVA: 0x0003E032 File Offset: 0x0003C232
		public string Uri { get; }

		// Token: 0x06000C6D RID: 3181 RVA: 0x0003E03A File Offset: 0x0003C23A
		public UriAction(string uri) : base(ActionType.URI)
		{
			this.Uri = uri;
		}
	}
}
