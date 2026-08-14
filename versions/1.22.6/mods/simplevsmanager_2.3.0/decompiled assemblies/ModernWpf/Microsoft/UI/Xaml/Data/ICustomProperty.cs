using System;
using System.Runtime.InteropServices;
using WinRT;

namespace Microsoft.UI.Xaml.Data
{
	// Token: 0x0200017B RID: 379
	[WindowsRuntimeType(null)]
	[Guid("30DA92C0-23E8-42A0-AE7C-734A0E5D2782")]
	internal interface ICustomProperty
	{
		// Token: 0x06000C0D RID: 3085
		object GetValue(object target);

		// Token: 0x06000C0E RID: 3086
		void SetValue(object target, object value);

		// Token: 0x06000C0F RID: 3087
		object GetIndexedValue(object target, object index);

		// Token: 0x06000C10 RID: 3088
		void SetIndexedValue(object target, object value, object index);

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000C11 RID: 3089
		bool CanRead { get; }

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000C12 RID: 3090
		bool CanWrite { get; }

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000C13 RID: 3091
		string Name { get; }

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000C14 RID: 3092
		Type Type { get; }
	}
}
