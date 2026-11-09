using System;

namespace ImGuiNET
{
	// Token: 0x0200002D RID: 45
	[Flags]
	public enum ImGuiBackendFlags
	{
		// Token: 0x040000E6 RID: 230
		None = 0,
		// Token: 0x040000E7 RID: 231
		HasGamepad = 1,
		// Token: 0x040000E8 RID: 232
		HasMouseCursors = 2,
		// Token: 0x040000E9 RID: 233
		HasSetMousePos = 4,
		// Token: 0x040000EA RID: 234
		RendererHasVtxOffset = 8,
		// Token: 0x040000EB RID: 235
		PlatformHasViewports = 1024,
		// Token: 0x040000EC RID: 236
		HasMouseHoveredViewport = 2048,
		// Token: 0x040000ED RID: 237
		RendererHasViewports = 4096
	}
}
