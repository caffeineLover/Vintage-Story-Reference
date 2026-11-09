using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x02000078 RID: 120
	[AttributeUsage(128)]
	internal sealed class AspTypePropertyAttribute : Attribute
	{
		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000E507 File Offset: 0x0000C707
		// (set) Token: 0x060001EE RID: 494 RVA: 0x0000E50F File Offset: 0x0000C70F
		public bool CreateConstructorReferences { get; private set; }

		// Token: 0x060001EF RID: 495 RVA: 0x0000E518 File Offset: 0x0000C718
		public AspTypePropertyAttribute(bool createConstructorReferences)
		{
			this.CreateConstructorReferences = createConstructorReferences;
		}
	}
}
