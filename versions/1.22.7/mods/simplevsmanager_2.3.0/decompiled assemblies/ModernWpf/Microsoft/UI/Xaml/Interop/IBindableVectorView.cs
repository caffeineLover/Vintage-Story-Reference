using System;
using System.Collections;
using System.Runtime.InteropServices;
using WinRT;

namespace Microsoft.UI.Xaml.Interop
{
	// Token: 0x02000182 RID: 386
	[WindowsRuntimeType(null)]
	[Guid("346DD6E7-976E-4BC3-815D-ECE243BC0F33")]
	internal interface IBindableVectorView : IEnumerable
	{
		// Token: 0x06000C2E RID: 3118
		object GetAt(uint index);

		// Token: 0x06000C2F RID: 3119
		bool IndexOf(object value, out uint index);

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000C30 RID: 3120
		uint Size { get; }
	}
}
