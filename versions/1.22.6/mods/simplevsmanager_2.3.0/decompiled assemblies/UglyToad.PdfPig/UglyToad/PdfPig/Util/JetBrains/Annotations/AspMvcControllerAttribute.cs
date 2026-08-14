using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x02000058 RID: 88
	[AttributeUsage(2112)]
	internal sealed class AspMvcControllerAttribute : Attribute
	{
		// Token: 0x060001BD RID: 445 RVA: 0x0000E347 File Offset: 0x0000C547
		public AspMvcControllerAttribute()
		{
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000E34F File Offset: 0x0000C54F
		public AspMvcControllerAttribute([NotNull] string anonymousProperty)
		{
			this.AnonymousProperty = anonymousProperty;
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060001BF RID: 447 RVA: 0x0000E35E File Offset: 0x0000C55E
		// (set) Token: 0x060001C0 RID: 448 RVA: 0x0000E366 File Offset: 0x0000C566
		[CanBeNull]
		public string AnonymousProperty { get; private set; }
	}
}
