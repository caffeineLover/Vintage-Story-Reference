using System;
using System.ComponentModel;

namespace WinRT
{
	// Token: 0x02000187 RID: 391
	[EditorBrowsable(1)]
	[AttributeUsage(5148, Inherited = false, AllowMultiple = false)]
	internal sealed class WindowsRuntimeTypeAttribute : Attribute
	{
		// Token: 0x06000C44 RID: 3140 RVA: 0x00021BF9 File Offset: 0x0001FDF9
		public WindowsRuntimeTypeAttribute(string sourceMetadata = null)
		{
			this.SourceMetadata = sourceMetadata;
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000C45 RID: 3141 RVA: 0x00021C08 File Offset: 0x0001FE08
		public string SourceMetadata { get; }
	}
}
