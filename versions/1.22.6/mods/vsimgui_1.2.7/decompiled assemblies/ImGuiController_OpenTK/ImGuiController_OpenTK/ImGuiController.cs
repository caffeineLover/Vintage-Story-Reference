using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ImGuiNET;
using ImGuizmoNET;
using imnodesNET;
using ImPlotNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ImGuiController_OpenTK
{
	// Token: 0x02000002 RID: 2
	[NullableContext(1)]
	[Nullable(0)]
	public class ImGuiController : IDisposable
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public unsafe ImGuiController(IWindow mainWindow, bool multiViewport = true)
		{
			ImGuiController.SetImGuiContext();
			this.SetImGuiParameters(multiViewport);
			this.mMainViewport = *ImGui.GetPlatformIO().Viewports[0];
			this.mMainImGuiRenderer = new ImGuiRenderer(this.mMainViewport, mainWindow.Native);
			this.mMainWindow = new MainWindowWrapper(mainWindow, this.mMainImGuiRenderer);
			this.mWindowsManager = new ImGuiWindowsManager(new ImGuiWindowsManager.CreateNewWindow(this.CreateWindow), this.mMainViewport, this.mMainWindow);
			this.mMainWindow.Native.MouseWheel += new Action<MouseWheelEventArgs>(this.OnMouseScroll);
			this.mMainWindow.Native.TextInput += new Action<TextInputEventArgs>(this.OnTextInput);
			ImGuiController.SetPerFrameImGuiData(0.016666668f, this.mMainWindow.Native);
			ImGuiController.UpdateMonitors();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002140 File Offset: 0x00000340
		public unsafe virtual void Render(float deltaSeconds, bool mainWindowIncluded = true)
		{
			ImGui.UpdatePlatformWindows();
			List<IImGuiWindow> windows = this.GetWindows(mainWindowIncluded);
			foreach (IImGuiWindow imGuiWindow in windows)
			{
				MonitorInfo monitor = Monitors.GetMonitorFromWindow(imGuiWindow.Native);
				*imGuiWindow.Viewport.Pos = new Vector2((float)monitor.ClientArea.Min.X, (float)monitor.ClientArea.Min.Y);
				*imGuiWindow.Viewport.Size = new Vector2((float)monitor.ClientArea.Size.X, (float)monitor.ClientArea.Size.Y);
				imGuiWindow.OnUpdate(deltaSeconds);
			}
			this.UpdateImGuiInput(mainWindowIncluded);
			ImGuiController.SetPerFrameImGuiData(deltaSeconds, this.mMainWindow.Native);
			ImGuiController.UpdateMonitors();
			ImGui.NewFrame();
			foreach (IImGuiWindow imGuiWindow2 in windows)
			{
				imGuiWindow2.OnDraw(deltaSeconds);
			}
			ImGui.Render();
			foreach (IImGuiWindow imGuiWindow3 in windows)
			{
				imGuiWindow3.ContextMakeCurrent();
				imGuiWindow3.OnRender(deltaSeconds);
				imGuiWindow3.ImGuiRenderer.Render();
				imGuiWindow3.SwapBuffers();
			}
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000022E4 File Offset: 0x000004E4
		// (set) Token: 0x06000004 RID: 4 RVA: 0x000022EC File Offset: 0x000004EC
		public bool CanUpdateInputs { get; set; } = true;

		// Token: 0x06000005 RID: 5 RVA: 0x000022F8 File Offset: 0x000004F8
		protected unsafe List<IImGuiWindow> GetWindows(bool includeMain = true)
		{
			List<IImGuiWindow> windows = new List<IImGuiWindow>();
			ImVector<ImGuiViewportPtr> viewports = ImGui.GetPlatformIO().Viewports;
			for (int index = 0; index < viewports.Size; index++)
			{
				ImGuiViewportPtr viewport = *viewports[index];
				IImGuiWindow window = this.mWindowsManager.GetWindow(viewport);
				if (window != null && (includeMain || viewport.NativePtr != this.mMainViewport.NativePtr))
				{
					windows.Add(window);
				}
			}
			return windows;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x0000236C File Offset: 0x0000056C
		protected virtual void LoadFonts()
		{
			ImGui.GetIO().Fonts.AddFontDefault();
		}

		// Token: 0x06000007 RID: 7 RVA: 0x0000238F File Offset: 0x0000058F
		private IImGuiWindow CreateWindow(ImGuiViewportPtr viewport)
		{
			return new ImGuiWindow(viewport, this.mMainWindow.Native, this.mMainImGuiRenderer, this);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000023AC File Offset: 0x000005AC
		private unsafe void SetImGuiParameters(bool multiViewport = true)
		{
			ImGuiIOPtr io = ImGui.GetIO();
			this.LoadFonts();
			if (multiViewport)
			{
				*io.ConfigFlags |= 1024;
				*io.BackendFlags |= 8;
				*io.ConfigFlags |= 64;
			}
			*io.BackendFlags |= 2;
			*io.BackendFlags |= 4;
			*io.BackendFlags |= 1024;
			*io.BackendFlags |= 4096;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002429 File Offset: 0x00000629
		private static void SetImGuiContext()
		{
			IntPtr intPtr = ImGui.CreateContext();
			ImGui.SetCurrentContext(intPtr);
			ImPlot.SetCurrentContext(ImPlot.CreateContext());
			ImPlot.SetImGuiContext(intPtr);
			ImGuizmo.SetImGuiContext(intPtr);
			imnodes.SetCurrentContext(imnodes.CreateContext());
			imnodes.SetImGuiContext(intPtr);
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000A RID: 10 RVA: 0x0000245B File Offset: 0x0000065B
		public List<char> PressedCharacters { get; } = new List<char>();

		// Token: 0x0600000B RID: 11 RVA: 0x00002463 File Offset: 0x00000663
		public void OnTextInput(TextInputEventArgs args)
		{
			this.PressedCharacters.Add((char)args.Unicode);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002478 File Offset: 0x00000678
		public unsafe void OnMouseScroll(MouseWheelEventArgs args)
		{
			ImGuiIOPtr io = ImGui.GetIO();
			*io.MouseWheel = args.Offset.Y;
			*io.MouseWheelH = args.Offset.X;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000024B4 File Offset: 0x000006B4
		protected unsafe static void SetPerFrameImGuiData(float deltaSeconds, NativeWindow window)
		{
			ImGuiIOPtr io = ImGui.GetIO();
			*io.DisplaySize = new Vector2((float)(window.ClientSize.X / 1), (float)(window.ClientSize.Y / 1));
			*io.DisplayFramebufferScale = new Vector2(1f, 1f);
			*io.DeltaTime = deltaSeconds;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00002518 File Offset: 0x00000718
		protected unsafe static void UpdateMonitors()
		{
			ImGuiPlatformIOPtr platformIO = ImGui.GetPlatformIO();
			List<MonitorInfo> monitors = Monitors.GetMonitors();
			IntPtr data = Marshal.AllocHGlobal(Unsafe.SizeOf<ImGuiPlatformMonitor>() * monitors.Count);
			Marshal.FreeHGlobal(platformIO.NativePtr.Monitors.Data);
			platformIO.NativePtr.Monitors = new ImVector(monitors.Count, monitors.Count, data);
			for (int i = 0; i < monitors.Count; i++)
			{
				Box2i clientArea = monitors[i].ClientArea;
				ImGuiPlatformMonitorPtr monitor = platformIO.Monitors[i];
				*monitor.DpiScale = 1f;
				*monitor.MainPos = new Vector2((float)clientArea.Min.X, (float)clientArea.Min.Y);
				*monitor.MainSize = new Vector2((float)clientArea.Size.X, (float)clientArea.Size.Y);
				*monitor.WorkPos = new Vector2((float)clientArea.Min.X, (float)clientArea.Min.Y);
				*monitor.WorkSize = new Vector2((float)clientArea.Size.X, (float)clientArea.Size.Y);
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002668 File Offset: 0x00000868
		protected unsafe void CollectKeysInputs(bool mainWindowIncluded = true)
		{
			ImGuiIOPtr io = ImGui.GetIO();
			bool mouseLeft = false;
			bool mouseRight = false;
			bool mouseMiddle = false;
			bool mouseButton4 = false;
			bool mouseButton5 = false;
			bool keyCtrl = false;
			bool keyAlt = false;
			bool keyShift = false;
			bool keySuper = false;
			foreach (NativeWindow nativeWindow in Enumerable.Select<IImGuiWindow, NativeWindow>(this.GetWindows(mainWindowIncluded), (IImGuiWindow window) => window.Native))
			{
				MouseState mouseState = nativeWindow.MouseState;
				KeyboardState keyboardState = nativeWindow.KeyboardState;
				mouseLeft |= mouseState[0];
				mouseRight |= mouseState[1];
				mouseMiddle |= mouseState[2];
				mouseButton4 |= mouseState[3];
				mouseButton5 |= mouseState[4];
				keyCtrl |= (keyboardState.IsKeyDown(341) || keyboardState.IsKeyDown(345));
				keyAlt |= (keyboardState.IsKeyDown(342) || keyboardState.IsKeyDown(346));
				keyShift |= (keyboardState.IsKeyDown(340) || keyboardState.IsKeyDown(344));
				keySuper |= (keyboardState.IsKeyDown(343) || keyboardState.IsKeyDown(347));
				foreach (object obj in Enum.GetValues(typeof(Keys)))
				{
					Keys key = (Keys)obj;
					if (key != -1)
					{
						io.AddKeyEvent(ImGuiController.TranslateKey(key), keyboardState.IsKeyDown(key));
					}
				}
			}
			*io.MouseDown[0] = mouseLeft;
			*io.MouseDown[1] = mouseRight;
			*io.MouseDown[2] = mouseMiddle;
			*io.MouseDown[3] = mouseButton4;
			*io.MouseDown[4] = mouseButton5;
			*io.KeyCtrl = keyCtrl;
			*io.KeyAlt = keyAlt;
			*io.KeyShift = keyShift;
			*io.KeySuper = keySuper;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000028DC File Offset: 0x00000ADC
		protected Vector2 CollectMousePosition(bool outsideOfMainWinodw = false)
		{
			NativeWindow mainWindow = this.mMainWindow.Native;
			Vector2i screenPoint;
			screenPoint..ctor((int)mainWindow.MouseState.X, (int)mainWindow.MouseState.Y);
			Vector2i point = mainWindow.ClientLocation + screenPoint;
			return new Vector2((float)point.X, (float)point.Y);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002934 File Offset: 0x00000B34
		protected unsafe virtual void UpdateImGuiInput(bool mainWindowIncluded = true)
		{
			if (!this.CanUpdateInputs)
			{
				this.PressedCharacters.Clear();
				return;
			}
			ImGuiIOPtr io = ImGui.GetIO();
			this.CollectKeysInputs(mainWindowIncluded);
			*io.MousePos = this.CollectMousePosition(!mainWindowIncluded);
			foreach (char character in this.PressedCharacters)
			{
				io.AddInputCharacter((uint)character);
			}
			this.PressedCharacters.Clear();
		}

		// Token: 0x06000012 RID: 18 RVA: 0x000029CC File Offset: 0x00000BCC
		public static ImGuiKey TranslateKey(Keys key)
		{
			if (key >= 48 && key <= 57)
			{
				return key - 48 + 536;
			}
			if (key >= 65 && key <= 90)
			{
				return key - 65 + 546;
			}
			if (key >= 320 && key <= 329)
			{
				return key - 320 + 600;
			}
			if (key >= 290 && key <= 301)
			{
				return key - 290 + 583;
			}
			if (key <= 59)
			{
				if (key == 32)
				{
					return 524;
				}
				switch (key)
				{
				case 39:
					return 584;
				case 40:
				case 41:
				case 42:
				case 43:
					break;
				case 44:
					return 585;
				case 45:
					return 586;
				case 46:
					return 587;
				case 47:
					return 588;
				default:
					if (key == 59)
					{
						return 589;
					}
					break;
				}
			}
			else if (key <= 96)
			{
				if (key == 61)
				{
					return 590;
				}
				switch (key)
				{
				case 91:
					return 591;
				case 92:
					return 592;
				case 93:
					return 593;
				case 96:
					return 594;
				}
			}
			else
			{
				switch (key)
				{
				case 256:
					return 526;
				case 257:
					return 525;
				case 258:
					return 512;
				case 259:
					return 523;
				case 260:
					return 521;
				case 261:
					return 522;
				case 262:
					return 514;
				case 263:
					return 513;
				case 264:
					return 516;
				case 265:
					return 515;
				case 266:
					return 517;
				case 267:
					return 518;
				case 268:
					return 519;
				case 269:
					return 520;
				case 270:
				case 271:
				case 272:
				case 273:
				case 274:
				case 275:
				case 276:
				case 277:
				case 278:
				case 279:
					break;
				case 280:
					return 595;
				case 281:
					return 596;
				case 282:
					return 597;
				case 283:
					return 598;
				case 284:
					return 599;
				default:
					switch (key)
					{
					case 330:
						return 610;
					case 331:
						return 611;
					case 332:
						return 612;
					case 333:
						return 613;
					case 334:
						return 614;
					case 335:
						return 615;
					case 336:
						return 616;
					case 340:
						return 528;
					case 341:
						return 527;
					case 342:
						return 529;
					case 343:
						return 530;
					case 344:
						return 532;
					case 345:
						return 531;
					case 346:
						return 533;
					case 347:
						return 534;
					case 348:
						return 535;
					}
					break;
				}
			}
			return 0;
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002CB1 File Offset: 0x00000EB1
		protected virtual void Dispose(bool disposing)
		{
			if (this.mDisposed)
			{
				return;
			}
			if (disposing)
			{
				this.mWindowsManager.Dispose();
				this.mMainImGuiRenderer.Dispose();
			}
			this.mDisposed = true;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002CDC File Offset: 0x00000EDC
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x04000002 RID: 2
		protected readonly ImGuiWindowsManager mWindowsManager;

		// Token: 0x04000003 RID: 3
		protected readonly IImGuiWindow mMainWindow;

		// Token: 0x04000004 RID: 4
		protected readonly ImGuiViewportPtr mMainViewport;

		// Token: 0x04000005 RID: 5
		protected readonly ImGuiRenderer mMainImGuiRenderer;

		// Token: 0x04000007 RID: 7
		private bool mDisposed;
	}
}
