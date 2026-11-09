using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x0200004F RID: 79
	[AttributeUsage(2112, AllowMultiple = true)]
	internal sealed class MacroAttribute : Attribute
	{
		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600019C RID: 412 RVA: 0x0000E1FC File Offset: 0x0000C3FC
		// (set) Token: 0x0600019D RID: 413 RVA: 0x0000E204 File Offset: 0x0000C404
		[CanBeNull]
		public string Expression { get; set; }

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x0600019E RID: 414 RVA: 0x0000E20D File Offset: 0x0000C40D
		// (set) Token: 0x0600019F RID: 415 RVA: 0x0000E215 File Offset: 0x0000C415
		public int Editable { get; set; }

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x0000E21E File Offset: 0x0000C41E
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x0000E226 File Offset: 0x0000C426
		[CanBeNull]
		public string Target { get; set; }
	}
}
