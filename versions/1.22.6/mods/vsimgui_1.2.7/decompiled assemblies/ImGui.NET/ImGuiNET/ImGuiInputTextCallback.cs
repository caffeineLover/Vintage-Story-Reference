using System;
using System.Runtime.InteropServices;

namespace ImGuiNET
{
	// Token: 0x0200007F RID: 127
	// (Invoke) Token: 0x06000911 RID: 2321
	[UnmanagedFunctionPointer(2)]
	public unsafe delegate int ImGuiInputTextCallback(ImGuiInputTextCallbackData* data);
}
