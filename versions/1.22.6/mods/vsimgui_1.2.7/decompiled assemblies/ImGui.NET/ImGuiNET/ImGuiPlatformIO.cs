using System;

namespace ImGuiNET
{
	// Token: 0x02000050 RID: 80
	public struct ImGuiPlatformIO
	{
		// Token: 0x040005B4 RID: 1460
		public IntPtr Platform_CreateWindow;

		// Token: 0x040005B5 RID: 1461
		public IntPtr Platform_DestroyWindow;

		// Token: 0x040005B6 RID: 1462
		public IntPtr Platform_ShowWindow;

		// Token: 0x040005B7 RID: 1463
		public IntPtr Platform_SetWindowPos;

		// Token: 0x040005B8 RID: 1464
		public IntPtr Platform_GetWindowPos;

		// Token: 0x040005B9 RID: 1465
		public IntPtr Platform_SetWindowSize;

		// Token: 0x040005BA RID: 1466
		public IntPtr Platform_GetWindowSize;

		// Token: 0x040005BB RID: 1467
		public IntPtr Platform_SetWindowFocus;

		// Token: 0x040005BC RID: 1468
		public IntPtr Platform_GetWindowFocus;

		// Token: 0x040005BD RID: 1469
		public IntPtr Platform_GetWindowMinimized;

		// Token: 0x040005BE RID: 1470
		public IntPtr Platform_SetWindowTitle;

		// Token: 0x040005BF RID: 1471
		public IntPtr Platform_SetWindowAlpha;

		// Token: 0x040005C0 RID: 1472
		public IntPtr Platform_UpdateWindow;

		// Token: 0x040005C1 RID: 1473
		public IntPtr Platform_RenderWindow;

		// Token: 0x040005C2 RID: 1474
		public IntPtr Platform_SwapBuffers;

		// Token: 0x040005C3 RID: 1475
		public IntPtr Platform_GetWindowDpiScale;

		// Token: 0x040005C4 RID: 1476
		public IntPtr Platform_OnChangedViewport;

		// Token: 0x040005C5 RID: 1477
		public IntPtr Platform_CreateVkSurface;

		// Token: 0x040005C6 RID: 1478
		public IntPtr Renderer_CreateWindow;

		// Token: 0x040005C7 RID: 1479
		public IntPtr Renderer_DestroyWindow;

		// Token: 0x040005C8 RID: 1480
		public IntPtr Renderer_SetWindowSize;

		// Token: 0x040005C9 RID: 1481
		public IntPtr Renderer_RenderWindow;

		// Token: 0x040005CA RID: 1482
		public IntPtr Renderer_SwapBuffers;

		// Token: 0x040005CB RID: 1483
		public ImVector Monitors;

		// Token: 0x040005CC RID: 1484
		public ImVector Viewports;
	}
}
