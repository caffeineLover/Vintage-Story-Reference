using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x02000050 RID: 80
	[AttributeUsage(385, AllowMultiple = true)]
	internal sealed class AspMvcAreaMasterLocationFormatAttribute : Attribute
	{
		// Token: 0x060001A3 RID: 419 RVA: 0x0000E237 File Offset: 0x0000C437
		public AspMvcAreaMasterLocationFormatAttribute([NotNull] string format)
		{
			this.Format = format;
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x0000E246 File Offset: 0x0000C446
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x0000E24E File Offset: 0x0000C44E
		[NotNull]
		public string Format { get; private set; }
	}
}
