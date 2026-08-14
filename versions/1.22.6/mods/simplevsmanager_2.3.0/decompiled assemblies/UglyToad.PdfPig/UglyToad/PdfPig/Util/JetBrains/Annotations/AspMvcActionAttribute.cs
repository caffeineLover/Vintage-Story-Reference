using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x02000056 RID: 86
	[AttributeUsage(2112)]
	internal sealed class AspMvcActionAttribute : Attribute
	{
		// Token: 0x060001B5 RID: 437 RVA: 0x0000E2F7 File Offset: 0x0000C4F7
		public AspMvcActionAttribute()
		{
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000E2FF File Offset: 0x0000C4FF
		public AspMvcActionAttribute([NotNull] string anonymousProperty)
		{
			this.AnonymousProperty = anonymousProperty;
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000E30E File Offset: 0x0000C50E
		// (set) Token: 0x060001B8 RID: 440 RVA: 0x0000E316 File Offset: 0x0000C516
		[CanBeNull]
		public string AnonymousProperty { get; private set; }
	}
}
