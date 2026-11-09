using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x02000201 RID: 513
	[StructLayout(0)]
	internal class WINDOWPLACEMENT
	{
		// Token: 0x04000324 RID: 804
		public int length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));

		// Token: 0x04000325 RID: 805
		public int flags;

		// Token: 0x04000326 RID: 806
		public SW showCmd;

		// Token: 0x04000327 RID: 807
		public POINT ptMinPosition;

		// Token: 0x04000328 RID: 808
		public POINT ptMaxPosition;

		// Token: 0x04000329 RID: 809
		public RECT rcNormalPosition;
	}
}
