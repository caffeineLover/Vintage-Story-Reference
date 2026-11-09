using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x02000079 RID: 121
	[AttributeUsage(1, AllowMultiple = true)]
	internal sealed class RazorImportNamespaceAttribute : Attribute
	{
		// Token: 0x060001F0 RID: 496 RVA: 0x0000E527 File Offset: 0x0000C727
		public RazorImportNamespaceAttribute([NotNull] string name)
		{
			this.Name = name;
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x0000E536 File Offset: 0x0000C736
		// (set) Token: 0x060001F2 RID: 498 RVA: 0x0000E53E File Offset: 0x0000C73E
		[NotNull]
		public string Name { get; private set; }
	}
}
