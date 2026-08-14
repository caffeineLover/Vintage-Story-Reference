using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x0200006A RID: 106
	[AttributeUsage(2048)]
	internal sealed class AssertionConditionAttribute : Attribute
	{
		// Token: 0x060001D8 RID: 472 RVA: 0x0000E43F File Offset: 0x0000C63F
		public AssertionConditionAttribute(AssertionConditionType conditionType)
		{
			this.ConditionType = conditionType;
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x0000E44E File Offset: 0x0000C64E
		// (set) Token: 0x060001DA RID: 474 RVA: 0x0000E456 File Offset: 0x0000C656
		public AssertionConditionType ConditionType { get; private set; }
	}
}
