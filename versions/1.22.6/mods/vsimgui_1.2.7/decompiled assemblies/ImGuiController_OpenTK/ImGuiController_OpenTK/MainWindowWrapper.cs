using System;
using System.Runtime.CompilerServices;
using ImGuiNET;
using OpenTK.Windowing.Desktop;

namespace ImGuiController_OpenTK
{
	// Token: 0x02000003 RID: 3
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class MainWindowWrapper : IImGuiWindow, IWindow, IDisposable
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002CEB File Offset: 0x00000EEB
		public NativeWindow Native
		{
			get
			{
				return this.mMainWindow.Native;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000016 RID: 22 RVA: 0x00002CF8 File Offset: 0x00000EF8
		public IImGuiRenderer ImGuiRenderer { get; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000017 RID: 23 RVA: 0x00002D00 File Offset: 0x00000F00
		public ImGuiViewportPtr Viewport { get; }

		// Token: 0x06000018 RID: 24 RVA: 0x00002D08 File Offset: 0x00000F08
		public unsafe MainWindowWrapper(IWindow window, IImGuiRenderer renderer)
		{
			this.mMainWindow = window;
			this.Viewport = *ImGui.GetPlatformIO().Viewports[0];
			this.ImGuiRenderer = renderer;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002D4A File Offset: 0x00000F4A
		public void OnDraw(float deltaSeconds)
		{
			this.mMainWindow.OnDraw(deltaSeconds);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002D58 File Offset: 0x00000F58
		public void OnRender(float deltaSeconds)
		{
			this.mMainWindow.OnRender(deltaSeconds);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002D66 File Offset: 0x00000F66
		public void OnUpdate(float deltaSeconds)
		{
			this.mMainWindow.OnUpdate(deltaSeconds);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002D74 File Offset: 0x00000F74
		public void ContextMakeCurrent()
		{
			this.mMainWindow.ContextMakeCurrent();
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002D81 File Offset: 0x00000F81
		public void SwapBuffers()
		{
			this.mMainWindow.SwapBuffers();
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002D8E File Offset: 0x00000F8E
		public void Dispose()
		{
		}

		// Token: 0x0400000A RID: 10
		private readonly IWindow mMainWindow;
	}
}
