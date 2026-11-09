using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x0200004B RID: 75
	[AttributeUsage(64)]
	internal sealed class MustUseReturnValueAttribute : Attribute
	{
		// Token: 0x06000192 RID: 402 RVA: 0x0000E19C File Offset: 0x0000C39C
		public MustUseReturnValueAttribute()
		{
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000E1A4 File Offset: 0x0000C3A4
		public MustUseReturnValueAttribute([NotNull] string justification)
		{
			this.Justification = justification;
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000194 RID: 404 RVA: 0x0000E1B3 File Offset: 0x0000C3B3
		// (set) Token: 0x06000195 RID: 405 RVA: 0x0000E1BB File Offset: 0x0000C3BB
		[CanBeNull]
		public string Justification { get; private set; }
	}
}
