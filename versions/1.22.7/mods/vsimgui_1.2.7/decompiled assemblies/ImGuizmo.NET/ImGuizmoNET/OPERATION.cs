using System;

namespace ImGuizmoNET
{
	// Token: 0x02000005 RID: 5
	public enum OPERATION
	{
		// Token: 0x04000005 RID: 5
		TRANSLATE_X = 1,
		// Token: 0x04000006 RID: 6
		TRANSLATE_Y,
		// Token: 0x04000007 RID: 7
		TRANSLATE_Z = 4,
		// Token: 0x04000008 RID: 8
		ROTATE_X = 8,
		// Token: 0x04000009 RID: 9
		ROTATE_Y = 16,
		// Token: 0x0400000A RID: 10
		ROTATE_Z = 32,
		// Token: 0x0400000B RID: 11
		ROTATE_SCREEN = 64,
		// Token: 0x0400000C RID: 12
		SCALE_X = 128,
		// Token: 0x0400000D RID: 13
		SCALE_Y = 256,
		// Token: 0x0400000E RID: 14
		SCALE_Z = 512,
		// Token: 0x0400000F RID: 15
		BOUNDS = 1024,
		// Token: 0x04000010 RID: 16
		TRANSLATE = 7,
		// Token: 0x04000011 RID: 17
		ROTATE = 120,
		// Token: 0x04000012 RID: 18
		SCALE = 896
	}
}
