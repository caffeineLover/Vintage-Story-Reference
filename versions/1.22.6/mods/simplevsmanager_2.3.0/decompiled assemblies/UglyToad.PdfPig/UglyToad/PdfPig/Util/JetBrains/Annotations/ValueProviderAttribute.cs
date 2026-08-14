using System;

namespace UglyToad.PdfPig.Util.JetBrains.Annotations
{
	// Token: 0x0200003D RID: 61
	[AttributeUsage(2432, AllowMultiple = true)]
	internal sealed class ValueProviderAttribute : Attribute
	{
		// Token: 0x06000166 RID: 358 RVA: 0x0000DFD5 File Offset: 0x0000C1D5
		public ValueProviderAttribute([NotNull] string name)
		{
			this.Name = name;
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000167 RID: 359 RVA: 0x0000DFE4 File Offset: 0x0000C1E4
		// (set) Token: 0x06000168 RID: 360 RVA: 0x0000DFEC File Offset: 0x0000C1EC
		[NotNull]
		public string Name { get; private set; }
	}
}
