using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x0200007B RID: 123
	[AttributeUsage(1, AllowMultiple = true)]
	internal sealed class RazorDirectiveAttribute : Attribute
	{
		// Token: 0x060001F8 RID: 504 RVA: 0x0000E57F File Offset: 0x0000C77F
		public RazorDirectiveAttribute([NotNull] string directive)
		{
			this.Directive = directive;
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001F9 RID: 505 RVA: 0x0000E58E File Offset: 0x0000C78E
		// (set) Token: 0x060001FA RID: 506 RVA: 0x0000E596 File Offset: 0x0000C796
		[NotNull]
		public string Directive { get; private set; }
	}
}
