using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x02000051 RID: 81
	[AttributeUsage(385, AllowMultiple = true)]
	internal sealed class AspMvcAreaPartialViewLocationFormatAttribute : Attribute
	{
		// Token: 0x060001A6 RID: 422 RVA: 0x0000E257 File Offset: 0x0000C457
		public AspMvcAreaPartialViewLocationFormatAttribute([NotNull] string format)
		{
			this.Format = format;
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001A7 RID: 423 RVA: 0x0000E266 File Offset: 0x0000C466
		// (set) Token: 0x060001A8 RID: 424 RVA: 0x0000E26E File Offset: 0x0000C46E
		[NotNull]
		public string Format { get; private set; }
	}
}
