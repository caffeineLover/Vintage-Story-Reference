using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x0200007C RID: 124
	[AttributeUsage(1, AllowMultiple = true)]
	internal sealed class RazorPageBaseTypeAttribute : Attribute
	{
		// Token: 0x060001FB RID: 507 RVA: 0x0000E59F File Offset: 0x0000C79F
		public RazorPageBaseTypeAttribute([NotNull] string baseType)
		{
			this.BaseType = baseType;
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000E5AE File Offset: 0x0000C7AE
		public RazorPageBaseTypeAttribute([NotNull] string baseType, string pageName)
		{
			this.BaseType = baseType;
			this.PageName = pageName;
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001FD RID: 509 RVA: 0x0000E5C4 File Offset: 0x0000C7C4
		// (set) Token: 0x060001FE RID: 510 RVA: 0x0000E5CC File Offset: 0x0000C7CC
		[NotNull]
		public string BaseType { get; private set; }

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001FF RID: 511 RVA: 0x0000E5D5 File Offset: 0x0000C7D5
		// (set) Token: 0x06000200 RID: 512 RVA: 0x0000E5DD File Offset: 0x0000C7DD
		[CanBeNull]
		public string PageName { get; private set; }
	}
}
