using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x02000067 RID: 103
	[AttributeUsage(224)]
	internal sealed class CollectionAccessAttribute : Attribute
	{
		// Token: 0x060001D4 RID: 468 RVA: 0x0000E417 File Offset: 0x0000C617
		public CollectionAccessAttribute(CollectionAccessType collectionAccessType)
		{
			this.CollectionAccessType = collectionAccessType;
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x0000E426 File Offset: 0x0000C626
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x0000E42E File Offset: 0x0000C62E
		public CollectionAccessType CollectionAccessType { get; private set; }
	}
}
