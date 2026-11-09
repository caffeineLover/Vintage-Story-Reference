using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x02000041 RID: 65
	[AttributeUsage(32767)]
	internal sealed class LocalizationRequiredAttribute : Attribute
	{
		// Token: 0x06000174 RID: 372 RVA: 0x0000E067 File Offset: 0x0000C267
		public LocalizationRequiredAttribute() : this(true)
		{
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000E070 File Offset: 0x0000C270
		public LocalizationRequiredAttribute(bool required)
		{
			this.Required = required;
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000176 RID: 374 RVA: 0x0000E07F File Offset: 0x0000C27F
		// (set) Token: 0x06000177 RID: 375 RVA: 0x0000E087 File Offset: 0x0000C287
		public bool Required { get; private set; }
	}
}
