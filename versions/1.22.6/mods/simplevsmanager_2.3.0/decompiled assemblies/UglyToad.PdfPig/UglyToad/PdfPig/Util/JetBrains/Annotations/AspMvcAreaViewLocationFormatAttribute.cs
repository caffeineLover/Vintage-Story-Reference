using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x02000052 RID: 82
	[AttributeUsage(385, AllowMultiple = true)]
	internal sealed class AspMvcAreaViewLocationFormatAttribute : Attribute
	{
		// Token: 0x060001A9 RID: 425 RVA: 0x0000E277 File Offset: 0x0000C477
		public AspMvcAreaViewLocationFormatAttribute([NotNull] string format)
		{
			this.Format = format;
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001AA RID: 426 RVA: 0x0000E286 File Offset: 0x0000C486
		// (set) Token: 0x060001AB RID: 427 RVA: 0x0000E28E File Offset: 0x0000C48E
		[NotNull]
		public string Format { get; private set; }
	}
}
