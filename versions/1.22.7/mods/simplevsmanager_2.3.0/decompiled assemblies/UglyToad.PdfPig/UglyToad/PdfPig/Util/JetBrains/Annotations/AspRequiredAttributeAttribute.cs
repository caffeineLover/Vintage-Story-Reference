using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x02000077 RID: 119
	[AttributeUsage(4, AllowMultiple = true)]
	internal sealed class AspRequiredAttributeAttribute : Attribute
	{
		// Token: 0x060001EA RID: 490 RVA: 0x0000E4E7 File Offset: 0x0000C6E7
		public AspRequiredAttributeAttribute([NotNull] string attribute)
		{
			this.Attribute = attribute;
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001EB RID: 491 RVA: 0x0000E4F6 File Offset: 0x0000C6F6
		// (set) Token: 0x060001EC RID: 492 RVA: 0x0000E4FE File Offset: 0x0000C6FE
		[NotNull]
		public string Attribute { get; private set; }
	}
}
