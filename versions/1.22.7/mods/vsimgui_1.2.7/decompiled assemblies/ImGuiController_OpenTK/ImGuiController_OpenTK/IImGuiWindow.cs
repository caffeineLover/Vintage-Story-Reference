using System;
using System.Runtime.CompilerServices;
using ImGuiNET;

namespace ImGuiController_OpenTK
{
	// Token: 0x02000009 RID: 9
	public interface IImGuiWindow : IWindow, IDisposable
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600003A RID: 58
		[Nullable(1)]
		IImGuiRenderer ImGuiRenderer { [NullableContext(1)] get; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600003B RID: 59
		ImGuiViewportPtr Viewport { get; }
	}
}
