using System;

namespace CommunityToolkit.Mvvm.ComponentModel
{
	// Token: 0x0200002D RID: 45
	[AttributeUsage(4, AllowMultiple = false, Inherited = false)]
	public sealed class INotifyPropertyChangedAttribute : Attribute
	{
		// Token: 0x1700002D RID: 45
		// (get) Token: 0x0600011F RID: 287 RVA: 0x0000579F File Offset: 0x0000399F
		// (set) Token: 0x06000120 RID: 288 RVA: 0x000057A7 File Offset: 0x000039A7
		public bool IncludeAdditionalHelperMethods { get; set; } = true;
	}
}
