using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x02000057 RID: 87
	[AttributeUsage(2048)]
	internal sealed class AspMvcAreaAttribute : Attribute
	{
		// Token: 0x060001B9 RID: 441 RVA: 0x0000E31F File Offset: 0x0000C51F
		public AspMvcAreaAttribute()
		{
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000E327 File Offset: 0x0000C527
		public AspMvcAreaAttribute([NotNull] string anonymousProperty)
		{
			this.AnonymousProperty = anonymousProperty;
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x060001BB RID: 443 RVA: 0x0000E336 File Offset: 0x0000C536
		// (set) Token: 0x060001BC RID: 444 RVA: 0x0000E33E File Offset: 0x0000C53E
		[CanBeNull]
		public string AnonymousProperty { get; private set; }
	}
}
