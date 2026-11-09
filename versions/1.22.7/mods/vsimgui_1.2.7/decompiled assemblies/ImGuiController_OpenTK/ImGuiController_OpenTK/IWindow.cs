using System;
using System.Runtime.CompilerServices;
using OpenTK.Windowing.Desktop;

namespace ImGuiController_OpenTK
{
	// Token: 0x02000008 RID: 8
	[NullableContext(1)]
	public interface IWindow : IDisposable
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000034 RID: 52
		NativeWindow Native { get; }

		// Token: 0x06000035 RID: 53
		void OnRender(float deltaSeconds);

		// Token: 0x06000036 RID: 54
		void OnDraw(float deltaSeconds);

		// Token: 0x06000037 RID: 55
		void OnUpdate(float deltaSeconds);

		// Token: 0x06000038 RID: 56
		void SwapBuffers();

		// Token: 0x06000039 RID: 57
		void ContextMakeCurrent();
	}
}
