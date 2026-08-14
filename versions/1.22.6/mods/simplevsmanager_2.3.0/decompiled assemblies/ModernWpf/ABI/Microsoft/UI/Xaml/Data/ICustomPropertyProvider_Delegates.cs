using System;
using ABI.System;

namespace ABI.Microsoft.UI.Xaml.Data
{
	// Token: 0x02000170 RID: 368
	internal static class ICustomPropertyProvider_Delegates
	{
		// Token: 0x020003AE RID: 942
		// (Invoke) Token: 0x06001A3F RID: 6719
		public unsafe delegate int GetCustomProperty_0(IntPtr thisPtr, IntPtr name, IntPtr* result);

		// Token: 0x020003AF RID: 943
		// (Invoke) Token: 0x06001A43 RID: 6723
		public unsafe delegate int GetIndexedProperty_1(IntPtr thisPtr, IntPtr name, Type type, IntPtr* result);

		// Token: 0x020003B0 RID: 944
		// (Invoke) Token: 0x06001A47 RID: 6727
		public unsafe delegate int GetStringRepresentation_2(IntPtr thisPtr, IntPtr* result);

		// Token: 0x020003B1 RID: 945
		// (Invoke) Token: 0x06001A4B RID: 6731
		public unsafe delegate int get_Type_3(IntPtr thisPtr, Type* value);
	}
}
