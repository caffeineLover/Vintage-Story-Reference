using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x02000055 RID: 85
	[AttributeUsage(385, AllowMultiple = true)]
	internal sealed class AspMvcViewLocationFormatAttribute : Attribute
	{
		// Token: 0x060001B2 RID: 434 RVA: 0x0000E2D7 File Offset: 0x0000C4D7
		public AspMvcViewLocationFormatAttribute([NotNull] string format)
		{
			this.Format = format;
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x060001B3 RID: 435 RVA: 0x0000E2E6 File Offset: 0x0000C4E6
		// (set) Token: 0x060001B4 RID: 436 RVA: 0x0000E2EE File Offset: 0x0000C4EE
		[NotNull]
		public string Format { get; private set; }
	}
}
