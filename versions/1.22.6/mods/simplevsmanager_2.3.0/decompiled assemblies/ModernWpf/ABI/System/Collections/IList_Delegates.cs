using System;

namespace ABI.System.Collections
{
	// Token: 0x0200014F RID: 335
	internal static class IList_Delegates
	{
		// Token: 0x0200036E RID: 878
		// (Invoke) Token: 0x060018D5 RID: 6357
		public unsafe delegate int GetAt_0(IntPtr thisPtr, uint index, IntPtr* result);

		// Token: 0x0200036F RID: 879
		// (Invoke) Token: 0x060018D9 RID: 6361
		public unsafe delegate int get_Size_1(IntPtr thisPtr, uint* result);

		// Token: 0x02000370 RID: 880
		// (Invoke) Token: 0x060018DD RID: 6365
		public unsafe delegate int GetView_2(IntPtr thisPtr, IntPtr* result);

		// Token: 0x02000371 RID: 881
		// (Invoke) Token: 0x060018E1 RID: 6369
		public unsafe delegate int IndexOf_3(IntPtr thisPtr, IntPtr value, uint* index, byte* returnValue);

		// Token: 0x02000372 RID: 882
		// (Invoke) Token: 0x060018E5 RID: 6373
		public delegate int SetAt_4(IntPtr thisPtr, uint index, IntPtr value);

		// Token: 0x02000373 RID: 883
		// (Invoke) Token: 0x060018E9 RID: 6377
		public delegate int InsertAt_5(IntPtr thisPtr, uint index, IntPtr value);

		// Token: 0x02000374 RID: 884
		// (Invoke) Token: 0x060018ED RID: 6381
		public delegate int RemoveAt_6(IntPtr thisPtr, uint index);

		// Token: 0x02000375 RID: 885
		// (Invoke) Token: 0x060018F1 RID: 6385
		public delegate int Append_7(IntPtr thisPtr, IntPtr value);

		// Token: 0x02000376 RID: 886
		// (Invoke) Token: 0x060018F5 RID: 6389
		public delegate int RemoveAtEnd_8(IntPtr thisPtr);

		// Token: 0x02000377 RID: 887
		// (Invoke) Token: 0x060018F9 RID: 6393
		public delegate int Clear_9(IntPtr thisPtr);
	}
}
