using System;
using ABI.System;

namespace ABI.Microsoft.UI.Xaml.Data
{
	// Token: 0x0200016D RID: 365
	internal static class ICustomProperty_Delegates
	{
		// Token: 0x020003A6 RID: 934
		// (Invoke) Token: 0x06001A1F RID: 6687
		public unsafe delegate int get_Type_0(IntPtr thisPtr, Type* value);

		// Token: 0x020003A7 RID: 935
		// (Invoke) Token: 0x06001A23 RID: 6691
		public unsafe delegate int get_Name_1(IntPtr thisPtr, IntPtr* value);

		// Token: 0x020003A8 RID: 936
		// (Invoke) Token: 0x06001A27 RID: 6695
		public unsafe delegate int GetValue_2(IntPtr thisPtr, IntPtr target, IntPtr* result);

		// Token: 0x020003A9 RID: 937
		// (Invoke) Token: 0x06001A2B RID: 6699
		public delegate int SetValue_3(IntPtr thisPtr, IntPtr target, IntPtr value);

		// Token: 0x020003AA RID: 938
		// (Invoke) Token: 0x06001A2F RID: 6703
		public unsafe delegate int GetIndexedValue_4(IntPtr thisPtr, IntPtr target, IntPtr index, IntPtr* result);

		// Token: 0x020003AB RID: 939
		// (Invoke) Token: 0x06001A33 RID: 6707
		public delegate int SetIndexedValue_5(IntPtr thisPtr, IntPtr target, IntPtr value, IntPtr index);

		// Token: 0x020003AC RID: 940
		// (Invoke) Token: 0x06001A37 RID: 6711
		public unsafe delegate int get_CanWrite_6(IntPtr thisPtr, byte* value);

		// Token: 0x020003AD RID: 941
		// (Invoke) Token: 0x06001A3B RID: 6715
		public unsafe delegate int get_CanRead_7(IntPtr thisPtr, byte* value);
	}
}
