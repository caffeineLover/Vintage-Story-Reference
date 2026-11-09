using System;

namespace ImGuiNET
{
	// Token: 0x02000042 RID: 66
	public struct ImGuiListClipper
	{
		// Token: 0x0400056F RID: 1391
		public IntPtr Ctx;

		// Token: 0x04000570 RID: 1392
		public int DisplayStart;

		// Token: 0x04000571 RID: 1393
		public int DisplayEnd;

		// Token: 0x04000572 RID: 1394
		public int ItemsCount;

		// Token: 0x04000573 RID: 1395
		public float ItemsHeight;

		// Token: 0x04000574 RID: 1396
		public float StartPosY;

		// Token: 0x04000575 RID: 1397
		public unsafe void* TempData;
	}
}
