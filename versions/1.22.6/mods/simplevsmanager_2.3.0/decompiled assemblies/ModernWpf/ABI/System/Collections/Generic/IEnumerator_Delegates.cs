using System;

namespace ABI.System.Collections.Generic
{
	// Token: 0x0200015E RID: 350
	internal static class IEnumerator_Delegates
	{
		// Token: 0x0200038C RID: 908
		// (Invoke) Token: 0x06001978 RID: 6520
		public delegate int MoveNext_2(IntPtr thisPtr, out byte __return_value__);

		// Token: 0x0200038D RID: 909
		// (Invoke) Token: 0x0600197C RID: 6524
		public delegate int GetMany_3(IntPtr thisPtr, int __itemsSize, IntPtr items, out uint __return_value__);
	}
}
