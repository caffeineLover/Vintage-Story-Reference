using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x02000054 RID: 84
	[AttributeUsage(385, AllowMultiple = true)]
	internal sealed class AspMvcPartialViewLocationFormatAttribute : Attribute
	{
		// Token: 0x060001AF RID: 431 RVA: 0x0000E2B7 File Offset: 0x0000C4B7
		public AspMvcPartialViewLocationFormatAttribute([NotNull] string format)
		{
			this.Format = format;
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x0000E2C6 File Offset: 0x0000C4C6
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x0000E2CE File Offset: 0x0000C4CE
		[NotNull]
		public string Format { get; private set; }
	}
}
