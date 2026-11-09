using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x02000065 RID: 101
	[AttributeUsage(2432)]
	internal sealed class HtmlAttributeValueAttribute : Attribute
	{
		// Token: 0x060001D0 RID: 464 RVA: 0x0000E3EF File Offset: 0x0000C5EF
		public HtmlAttributeValueAttribute([NotNull] string name)
		{
			this.Name = name;
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x0000E3FE File Offset: 0x0000C5FE
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x0000E406 File Offset: 0x0000C606
		[NotNull]
		public string Name { get; private set; }
	}
}
