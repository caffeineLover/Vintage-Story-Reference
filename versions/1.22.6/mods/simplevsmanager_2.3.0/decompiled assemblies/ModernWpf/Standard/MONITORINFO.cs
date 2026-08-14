using System;
using System.Runtime.InteropServices;

namespace Standard
{
	// Token: 0x020001FD RID: 509
	[StructLayout(0)]
	internal class MONITORINFO
	{
		// Token: 0x04000318 RID: 792
		public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));

		// Token: 0x04000319 RID: 793
		public RECT rcMonitor;

		// Token: 0x0400031A RID: 794
		public RECT rcWork;

		// Token: 0x0400031B RID: 795
		public int dwFlags;
	}
}
