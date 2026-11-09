using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x02000064 RID: 100
	[AttributeUsage(2432)]
	internal sealed class HtmlElementAttributesAttribute : Attribute
	{
		// Token: 0x060001CC RID: 460 RVA: 0x0000E3C7 File Offset: 0x0000C5C7
		public HtmlElementAttributesAttribute()
		{
		}

		// Token: 0x060001CD RID: 461 RVA: 0x0000E3CF File Offset: 0x0000C5CF
		public HtmlElementAttributesAttribute([NotNull] string name)
		{
			this.Name = name;
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001CE RID: 462 RVA: 0x0000E3DE File Offset: 0x0000C5DE
		// (set) Token: 0x060001CF RID: 463 RVA: 0x0000E3E6 File Offset: 0x0000C5E6
		[CanBeNull]
		public string Name { get; private set; }
	}
}
