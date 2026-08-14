using System;
using System.Collections;
using System.Runtime.InteropServices;
using WinRT;

namespace Microsoft.UI.Xaml.Interop
{
	// Token: 0x02000181 RID: 385
	[WindowsRuntimeType(null)]
	[Guid("393DE7DE-6FD0-4C0D-BB71-47244A113E93")]
	internal interface IBindableVector : IEnumerable
	{
		// Token: 0x06000C24 RID: 3108
		object GetAt(uint index);

		// Token: 0x06000C25 RID: 3109
		IBindableVectorView GetView();

		// Token: 0x06000C26 RID: 3110
		bool IndexOf(object value, out uint index);

		// Token: 0x06000C27 RID: 3111
		void SetAt(uint index, object value);

		// Token: 0x06000C28 RID: 3112
		void InsertAt(uint index, object value);

		// Token: 0x06000C29 RID: 3113
		void RemoveAt(uint index);

		// Token: 0x06000C2A RID: 3114
		void Append(object value);

		// Token: 0x06000C2B RID: 3115
		void RemoveAtEnd();

		// Token: 0x06000C2C RID: 3116
		void Clear();

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000C2D RID: 3117
		uint Size { get; }
	}
}
