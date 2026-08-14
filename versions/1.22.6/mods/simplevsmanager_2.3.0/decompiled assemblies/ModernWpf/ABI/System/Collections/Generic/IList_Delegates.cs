using System;

namespace ABI.System.Collections.Generic
{
	// Token: 0x02000162 RID: 354
	internal static class IList_Delegates
	{
		// Token: 0x02000390 RID: 912
		// (Invoke) Token: 0x060019AD RID: 6573
		public delegate int GetView_2(IntPtr thisPtr, out IntPtr __return_value__);

		// Token: 0x02000391 RID: 913
		// (Invoke) Token: 0x060019B1 RID: 6577
		public delegate int RemoveAt_6(IntPtr thisPtr, uint index);

		// Token: 0x02000392 RID: 914
		// (Invoke) Token: 0x060019B5 RID: 6581
		public delegate int RemoveAtEnd_8(IntPtr thisPtr);

		// Token: 0x02000393 RID: 915
		// (Invoke) Token: 0x060019B9 RID: 6585
		public delegate int Clear_9(IntPtr thisPtr);

		// Token: 0x02000394 RID: 916
		// (Invoke) Token: 0x060019BD RID: 6589
		public delegate int GetMany_10(IntPtr thisPtr, uint startIndex, int __itemsSize, IntPtr items, out uint __return_value__);

		// Token: 0x02000395 RID: 917
		// (Invoke) Token: 0x060019C1 RID: 6593
		public delegate int ReplaceAll_11(IntPtr thisPtr, int __itemsSize, IntPtr items);
	}
}
