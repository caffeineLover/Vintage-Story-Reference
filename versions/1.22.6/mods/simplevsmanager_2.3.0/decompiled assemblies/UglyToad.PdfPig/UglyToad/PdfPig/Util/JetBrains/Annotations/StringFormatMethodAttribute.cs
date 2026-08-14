using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x0200003C RID: 60
	[AttributeUsage(4320)]
	internal sealed class StringFormatMethodAttribute : Attribute
	{
		// Token: 0x06000163 RID: 355 RVA: 0x0000DFB5 File Offset: 0x0000C1B5
		public StringFormatMethodAttribute([NotNull] string formatParameterName)
		{
			this.FormatParameterName = formatParameterName;
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000164 RID: 356 RVA: 0x0000DFC4 File Offset: 0x0000C1C4
		// (set) Token: 0x06000165 RID: 357 RVA: 0x0000DFCC File Offset: 0x0000C1CC
		[NotNull]
		public string FormatParameterName { get; private set; }
	}
}
