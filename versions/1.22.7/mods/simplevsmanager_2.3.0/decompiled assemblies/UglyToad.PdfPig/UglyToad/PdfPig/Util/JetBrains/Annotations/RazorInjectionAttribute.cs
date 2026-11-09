using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x0200007A RID: 122
	[AttributeUsage(1, AllowMultiple = true)]
	internal sealed class RazorInjectionAttribute : Attribute
	{
		// Token: 0x060001F3 RID: 499 RVA: 0x0000E547 File Offset: 0x0000C747
		public RazorInjectionAttribute([NotNull] string type, [NotNull] string fieldName)
		{
			this.Type = type;
			this.FieldName = fieldName;
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000E55D File Offset: 0x0000C75D
		// (set) Token: 0x060001F5 RID: 501 RVA: 0x0000E565 File Offset: 0x0000C765
		[NotNull]
		public string Type { get; private set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x0000E56E File Offset: 0x0000C76E
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x0000E576 File Offset: 0x0000C776
		[NotNull]
		public string FieldName { get; private set; }
	}
}
