using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x02000053 RID: 83
	[AttributeUsage(385, AllowMultiple = true)]
	internal sealed class AspMvcMasterLocationFormatAttribute : Attribute
	{
		// Token: 0x060001AC RID: 428 RVA: 0x0000E297 File Offset: 0x0000C497
		public AspMvcMasterLocationFormatAttribute([NotNull] string format)
		{
			this.Format = format;
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x060001AD RID: 429 RVA: 0x0000E2A6 File Offset: 0x0000C4A6
		// (set) Token: 0x060001AE RID: 430 RVA: 0x0000E2AE File Offset: 0x0000C4AE
		[NotNull]
		public string Format { get; private set; }
	}
}
