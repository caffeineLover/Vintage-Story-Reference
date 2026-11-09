using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ImGuiController_OpenTK;
using ImGuiNET;
using OpenTK.Windowing.Desktop;
using Vintagestory.API.Config;
using VSImGui.API;

namespace VSImGui
{
	// Token: 0x02000005 RID: 5
	[NullableContext(2)]
	[Nullable(0)]
	internal sealed class Controller : ImGuiController
	{
		// Token: 0x06000022 RID: 34 RVA: 0x0000264F File Offset: 0x0000084F
		[NullableContext(1)]
		public Controller(MainGameWindowWrapper window, bool multiViewport = true) : base(window, multiViewport)
		{
			this.SetConfigFolder();
			this.mWindowsManager.OnWindowDestroyed += new Action<NativeWindow>(this.OnWindowDestroyed);
			ImGui.NewFrame();
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000023 RID: 35 RVA: 0x0000267C File Offset: 0x0000087C
		// (remove) Token: 0x06000024 RID: 36 RVA: 0x000026B4 File Offset: 0x000008B4
		public event Action OnWindowMergedIntoMain;

		// Token: 0x06000025 RID: 37 RVA: 0x000026EC File Offset: 0x000008EC
		public unsafe void Update(float deltaSeconds, bool captureInputs)
		{
			try
			{
				ImGui.UpdatePlatformWindows();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return;
			}
			List<IImGuiWindow> windows = base.GetWindows(true);
			foreach (IImGuiWindow imGuiWindow in windows)
			{
				MonitorInfo monitor = Monitors.GetMonitorFromWindow(imGuiWindow.Native);
				*imGuiWindow.Viewport.Pos = new Vector2((float)monitor.ClientArea.Min.X, (float)monitor.ClientArea.Min.Y);
				*imGuiWindow.Viewport.Size = new Vector2((float)monitor.ClientArea.Size.X, (float)monitor.ClientArea.Size.Y);
				imGuiWindow.OnUpdate(deltaSeconds);
			}
			this.UpdateImGuiInput(captureInputs);
			ImGuiController.SetPerFrameImGuiData(deltaSeconds, this.mMainWindow.Native);
			ImGuiController.UpdateMonitors();
			ImGui.NewFrame();
			foreach (IImGuiWindow imGuiWindow2 in windows)
			{
				imGuiWindow2.OnDraw(deltaSeconds);
			}
			ImGui.Render();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x0000285C File Offset: 0x00000A5C
		public void RenderOffWindow(float deltaSeconds)
		{
			foreach (IImGuiWindow imGuiWindow in base.GetWindows(false))
			{
				imGuiWindow.ContextMakeCurrent();
				imGuiWindow.OnRender(deltaSeconds);
				imGuiWindow.ImGuiRenderer.Render();
				imGuiWindow.SwapBuffers();
			}
			this.mMainWindow.Native.MakeCurrent();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000028D8 File Offset: 0x00000AD8
		public void RenderMainWindow(float deltaSeconds)
		{
			this.mMainWindow.ContextMakeCurrent();
			this.mMainWindow.OnRender(deltaSeconds);
			this.mMainWindow.ImGuiRenderer.Render();
			this.mMainWindow.SwapBuffers();
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000290C File Offset: 0x00000B0C
		public static bool IsAnyFocused()
		{
			return ImGui.IsWindowFocused(4);
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002914 File Offset: 0x00000B14
		public unsafe static bool KeyboardCaptured()
		{
			return *ImGui.GetIO().WantCaptureKeyboard;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002930 File Offset: 0x00000B30
		public unsafe static bool MouseCaptured()
		{
			return *ImGui.GetIO().WantCaptureMouse;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x0000294C File Offset: 0x00000B4C
		public unsafe static bool MouseMovesCaptured()
		{
			return ImGui.IsAnyItemHovered() || *ImGui.GetIO().WantCaptureMouse;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002970 File Offset: 0x00000B70
		[NullableContext(1)]
		private void OnWindowDestroyed(NativeWindow window)
		{
			if (this.mMainWindow.Native.IsFocused)
			{
				Action onWindowMergedIntoMain = this.OnWindowMergedIntoMain;
				if (onWindowMergedIntoMain == null)
				{
					return;
				}
				onWindowMergedIntoMain.Invoke();
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002994 File Offset: 0x00000B94
		protected override void LoadFonts()
		{
			FontManager.Load();
		}

		// Token: 0x0600002E RID: 46 RVA: 0x0000299C File Offset: 0x00000B9C
		private unsafe void SetConfigFolder()
		{
			ImGuiIOPtr io = ImGui.GetIO();
			IntPtr ptr = Marshal.StringToHGlobalAnsi(Path.Combine(GamePaths.ModConfig, "imgui.ini"));
			io.NativePtr.IniFilename = (byte*)ptr.ToPointer();
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000029D8 File Offset: 0x00000BD8
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				ImGui.GetIO().Fonts.Destroy();
			}
		}
	}
}
