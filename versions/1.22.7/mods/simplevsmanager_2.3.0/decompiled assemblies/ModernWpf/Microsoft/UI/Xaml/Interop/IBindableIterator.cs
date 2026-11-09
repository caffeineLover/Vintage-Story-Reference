using System;
using System.Runtime.InteropServices;
using WinRT;

namespace Microsoft.UI.Xaml.Interop
{
	// Token: 0x02000180 RID: 384
	[WindowsRuntimeType(null)]
	[Guid("6A1D6C07-076D-49F2-8314-F52C9C9A8331")]
	internal interface IBindableIterator
	{
		// Token: 0x06000C20 RID: 3104
		bool MoveNext();

		// Token: 0x06000C21 RID: 3105
		uint GetMany(ref object[] items);

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000C22 RID: 3106
		object Current { get; }

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000C23 RID: 3107
		bool HasCurrent { get; }
	}
}
