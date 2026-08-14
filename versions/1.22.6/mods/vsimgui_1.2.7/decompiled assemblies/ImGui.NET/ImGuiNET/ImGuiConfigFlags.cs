using System;

namespace ImGuiNET
{
	// Token: 0x02000033 RID: 51
	[Flags]
	public enum ImGuiConfigFlags
	{
		// Token: 0x0400015D RID: 349
		None = 0,
		// Token: 0x0400015E RID: 350
		NavEnableKeyboard = 1,
		// Token: 0x0400015F RID: 351
		NavEnableGamepad = 2,
		// Token: 0x04000160 RID: 352
		NavEnableSetMousePos = 4,
		// Token: 0x04000161 RID: 353
		NavNoCaptureKeyboard = 8,
		// Token: 0x04000162 RID: 354
		NoMouse = 16,
		// Token: 0x04000163 RID: 355
		NoMouseCursorChange = 32,
		// Token: 0x04000164 RID: 356
		DockingEnable = 64,
		// Token: 0x04000165 RID: 357
		ViewportsEnable = 1024,
		// Token: 0x04000166 RID: 358
		DpiEnableScaleViewports = 16384,
		// Token: 0x04000167 RID: 359
		DpiEnableScaleFonts = 32768,
		// Token: 0x04000168 RID: 360
		IsSRGB = 1048576,
		// Token: 0x04000169 RID: 361
		IsTouchScreen = 2097152
	}
}
