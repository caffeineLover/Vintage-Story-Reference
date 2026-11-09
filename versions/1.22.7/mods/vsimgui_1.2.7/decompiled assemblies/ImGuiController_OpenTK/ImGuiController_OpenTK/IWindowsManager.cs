using System;
using System.Runtime.CompilerServices;
using ImGuiNET;

namespace ImGuiController_OpenTK
{
	// Token: 0x0200000B RID: 11
	public interface IWindowsManager : IDisposable
	{
		// Token: 0x0600004B RID: 75
		[NullableContext(2)]
		IImGuiWindow GetWindow(ImGuiViewportPtr viewport);
	}
}
