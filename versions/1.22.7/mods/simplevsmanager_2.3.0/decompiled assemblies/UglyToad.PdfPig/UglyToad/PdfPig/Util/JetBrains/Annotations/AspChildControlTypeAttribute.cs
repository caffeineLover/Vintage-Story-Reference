using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x02000073 RID: 115
	[AttributeUsage(4, AllowMultiple = true)]
	internal sealed class AspChildControlTypeAttribute : Attribute
	{
		// Token: 0x060001E2 RID: 482 RVA: 0x0000E497 File Offset: 0x0000C697
		public AspChildControlTypeAttribute([NotNull] string tagName, [NotNull] Type controlType)
		{
			this.TagName = tagName;
			this.ControlType = controlType;
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x0000E4AD File Offset: 0x0000C6AD
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x0000E4B5 File Offset: 0x0000C6B5
		[NotNull]
		public string TagName { get; private set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x0000E4BE File Offset: 0x0000C6BE
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x0000E4C6 File Offset: 0x0000C6C6
		[NotNull]
		public Type ControlType { get; private set; }
	}
}
