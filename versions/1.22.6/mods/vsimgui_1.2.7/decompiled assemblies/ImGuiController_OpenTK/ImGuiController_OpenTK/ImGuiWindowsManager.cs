using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ImGuiController_OpenTK
{
	// Token: 0x0200000C RID: 12
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ImGuiWindowsManager : IWindowsManager, IDisposable
	{
		// Token: 0x0600004C RID: 76 RVA: 0x00003D84 File Offset: 0x00001F84
		public unsafe ImGuiWindowsManager(ImGuiWindowsManager.CreateNewWindow windowsMaker, ImGuiViewportPtr mainViewport, IImGuiWindow mainWindow)
		{
			this.mWindowsMaker = windowsMaker;
			this.mMainViewport = mainViewport;
			this.mMainWindow = mainWindow;
			ImGuiPlatformIOPtr platformIO = ImGui.GetPlatformIO();
			this.mMainViewport = *platformIO.Viewports[0];
			this.mCreateWindow = new Platform_CreateWindow(this.CreateWindow);
			this.mDestroyWindow = new Platform_DestroyWindow(this.DestroyWindow);
			this.mShowWindow = new Platform_ShowWindow(this.ShowWindow);
			this.mSetWindowPos = new Platform_SetWindowPos(this.SetWindowPos);
			this.mSetWindowSize = new Platform_SetWindowSize(this.SetWindowSize);
			this.mSetWindowFocus = new Platform_SetWindowFocus(this.SetWindowFocus);
			this.mGetWindowFocus = new Platform_GetWindowFocus(this.GetWindowFocus);
			this.mGetWindowMinimized = new Platform_GetWindowMinimized(this.GetWindowMinimized);
			this.mSetWindowTitle = new Platform_SetWindowTitle(this.SetWindowTitle);
			this.mSetClipboardText = new ImGuiWindowsManager.SetClipboardTextDlg(this.SetClipboardTextFn);
			this.mGetClipboardText = new ImGuiWindowsManager.GetClipboardTextDlg(this.GetClipboardTextFn);
			*platformIO.Platform_CreateWindow = Marshal.GetFunctionPointerForDelegate<Platform_CreateWindow>(this.mCreateWindow);
			*platformIO.Platform_DestroyWindow = Marshal.GetFunctionPointerForDelegate<Platform_DestroyWindow>(this.mDestroyWindow);
			*platformIO.Platform_ShowWindow = Marshal.GetFunctionPointerForDelegate<Platform_ShowWindow>(this.mShowWindow);
			*platformIO.Platform_SetWindowPos = Marshal.GetFunctionPointerForDelegate<Platform_SetWindowPos>(this.mSetWindowPos);
			*platformIO.Platform_SetWindowSize = Marshal.GetFunctionPointerForDelegate<Platform_SetWindowSize>(this.mSetWindowSize);
			*platformIO.Platform_SetWindowFocus = Marshal.GetFunctionPointerForDelegate<Platform_SetWindowFocus>(this.mSetWindowFocus);
			*platformIO.Platform_GetWindowFocus = Marshal.GetFunctionPointerForDelegate<Platform_GetWindowFocus>(this.mGetWindowFocus);
			*platformIO.Platform_GetWindowMinimized = Marshal.GetFunctionPointerForDelegate<Platform_GetWindowMinimized>(this.mGetWindowMinimized);
			*platformIO.Platform_SetWindowTitle = Marshal.GetFunctionPointerForDelegate<Platform_SetWindowTitle>(this.mSetWindowTitle);
			this.mGetWindowPos = new Platform_GetWindowPos(this.GetWindowPos);
			this.mGetWindowSize = new Platform_GetWindowSize(this.GetWindowSize);
			ImGuiNative.ImGuiPlatformIO_Set_Platform_GetWindowPos(platformIO.NativePtr, Marshal.GetFunctionPointerForDelegate<Platform_GetWindowPos>(this.mGetWindowPos));
			ImGuiNative.ImGuiPlatformIO_Set_Platform_GetWindowSize(platformIO.NativePtr, Marshal.GetFunctionPointerForDelegate<Platform_GetWindowSize>(this.mGetWindowSize));
			ImGuiIOPtr io = ImGui.GetIO();
			*io.SetClipboardTextFn = Marshal.GetFunctionPointerForDelegate<ImGuiWindowsManager.SetClipboardTextDlg>(this.mSetClipboardText);
			*io.GetClipboardTextFn = Marshal.GetFunctionPointerForDelegate<ImGuiWindowsManager.GetClipboardTextDlg>(this.mGetClipboardText);
			io.ClipboardUserData = IntPtr.Zero;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003FD4 File Offset: 0x000021D4
		[NullableContext(2)]
		public IImGuiWindow GetWindow(ImGuiViewportPtr viewport)
		{
			if (viewport.NativePtr == this.mMainViewport.NativePtr)
			{
				return this.mMainWindow;
			}
			if (viewport.PlatformUserData == IntPtr.Zero)
			{
				return null;
			}
			return (ImGuiWindow)GCHandle.FromIntPtr(viewport.PlatformUserData).Target;
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600004E RID: 78 RVA: 0x00004028 File Offset: 0x00002228
		// (remove) Token: 0x0600004F RID: 79 RVA: 0x00004060 File Offset: 0x00002260
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public event Action<NativeWindow> OnWindowCreated;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000050 RID: 80 RVA: 0x00004098 File Offset: 0x00002298
		// (remove) Token: 0x06000051 RID: 81 RVA: 0x000040D0 File Offset: 0x000022D0
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public event Action<NativeWindow> OnWindowDestroyed;

		// Token: 0x06000052 RID: 82 RVA: 0x00004108 File Offset: 0x00002308
		private NativeWindow GetWindowImpl(ImGuiViewportPtr viewport)
		{
			if (viewport.NativePtr == this.mMainViewport.NativePtr)
			{
				return this.mMainWindow.Native;
			}
			if (viewport.PlatformUserData == IntPtr.Zero)
			{
				return this.mMainWindow.Native;
			}
			return ((ImGuiWindow)GCHandle.FromIntPtr(viewport.PlatformUserData).Target) ?? this.mMainWindow.Native;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00004178 File Offset: 0x00002378
		private NativeWindow GetWindowImpl()
		{
			foreach (IImGuiWindow window in this.mWindows)
			{
				if (window.Native.IsFocused)
				{
					return window.Native;
				}
			}
			return this.mMainWindow.Native;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000041E8 File Offset: 0x000023E8
		private void SetClipboardTextFn(IntPtr userData, string text)
		{
			GLFW.SetClipboardString(this.GetWindowImpl().WindowPtr, text);
		}

		// Token: 0x06000055 RID: 85 RVA: 0x000041FB File Offset: 0x000023FB
		private IntPtr GetClipboardTextFn(IntPtr userData)
		{
			return Marshal.StringToCoTaskMemUTF8(GLFW.GetClipboardString(this.GetWindowImpl().WindowPtr));
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00004214 File Offset: 0x00002414
		private void CreateWindow(ImGuiViewportPtr viewport)
		{
			IImGuiWindow window = this.mWindowsMaker(viewport);
			this.mWindows.Add(window);
			if (window != null)
			{
				Action<NativeWindow> onWindowCreated = this.OnWindowCreated;
				if (onWindowCreated == null)
				{
					return;
				}
				onWindowCreated.Invoke(window.Native);
			}
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004254 File Offset: 0x00002454
		private void DestroyWindow(ImGuiViewportPtr viewport)
		{
			if (viewport.PlatformUserData == IntPtr.Zero)
			{
				return;
			}
			ImGuiWindow window = (ImGuiWindow)GCHandle.FromIntPtr(viewport.PlatformUserData).Target;
			if (window != null)
			{
				window.Close();
			}
			if (window != null)
			{
				window.Dispose();
			}
			viewport.PlatformUserData = IntPtr.Zero;
			if (window != null && this.mWindows.Contains(window))
			{
				this.mWindows.Remove(window);
			}
			if (window != null)
			{
				Action<NativeWindow> onWindowDestroyed = this.OnWindowDestroyed;
				if (onWindowDestroyed == null)
				{
					return;
				}
				onWindowDestroyed.Invoke(window);
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x000042DA File Offset: 0x000024DA
		private void ShowWindow(ImGuiViewportPtr viewport)
		{
			this.GetWindowImpl(viewport).IsVisible = true;
		}

		// Token: 0x06000059 RID: 89 RVA: 0x000042EC File Offset: 0x000024EC
		[NullableContext(0)]
		private unsafe void GetWindowPos(ImGuiViewportPtr viewport, Vector2* outPos)
		{
			NativeWindow window = this.GetWindowImpl(viewport);
			*outPos = new Vector2((float)window.ClientLocation.X, (float)window.ClientLocation.Y);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00004324 File Offset: 0x00002524
		private void SetWindowPos(ImGuiViewportPtr viewport, Vector2 pos)
		{
			this.GetWindowImpl(viewport).Location = new Vector2i((int)pos.X, (int)pos.Y);
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00004345 File Offset: 0x00002545
		private void SetWindowSize(ImGuiViewportPtr viewport, Vector2 size)
		{
			this.GetWindowImpl(viewport).Size = new Vector2i((int)size.X, (int)size.Y);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00004368 File Offset: 0x00002568
		[NullableContext(0)]
		private unsafe void GetWindowSize(ImGuiViewportPtr viewport, Vector2* outSize)
		{
			NativeWindow window = this.GetWindowImpl(viewport);
			*outSize = new Vector2((float)window.Size.X, (float)window.Size.Y);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(15, 1);
			defaultInterpolatedStringHandler.AppendLiteral("GetWindowSize: ");
			defaultInterpolatedStringHandler.AppendFormatted<Vector2i>(window.Size);
			Console.WriteLine(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600005D RID: 93 RVA: 0x000043CF File Offset: 0x000025CF
		private void SetWindowFocus(ImGuiViewportPtr viewport)
		{
			this.GetWindowImpl(viewport).Focus();
		}

		// Token: 0x0600005E RID: 94 RVA: 0x000043DD File Offset: 0x000025DD
		private byte GetWindowFocus(ImGuiViewportPtr viewport)
		{
			return (this.GetWindowImpl(viewport).IsFocused > false) ? 1 : 0;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x000043EF File Offset: 0x000025EF
		private byte GetWindowMinimized(ImGuiViewportPtr viewport)
		{
			return (this.GetWindowImpl(viewport).WindowState == 1) ? 1 : 0;
		}

		// Token: 0x06000060 RID: 96 RVA: 0x00004404 File Offset: 0x00002604
		private unsafe void SetWindowTitle(ImGuiViewportPtr viewport, IntPtr title)
		{
			NativeWindow window = this.GetWindowImpl(viewport);
			int count = 0;
			while (*(title + (IntPtr)count) != 0)
			{
				count++;
			}
			window.Title = Encoding.ASCII.GetString(title, count);
		}

		// Token: 0x06000061 RID: 97 RVA: 0x0000443C File Offset: 0x0000263C
		public void Dispose()
		{
			if (this.mDisposed)
			{
				return;
			}
			this.mDisposed = true;
			foreach (IImGuiWindow imGuiWindow in Enumerable.Where<IImGuiWindow>(this.mWindows, (IImGuiWindow window) => window != this.mMainWindow))
			{
				imGuiWindow.Dispose();
			}
		}

		// Token: 0x04000034 RID: 52
		private readonly HashSet<IImGuiWindow> mWindows = new HashSet<IImGuiWindow>();

		// Token: 0x04000035 RID: 53
		private readonly ImGuiViewportPtr mMainViewport;

		// Token: 0x04000036 RID: 54
		private readonly ImGuiWindowsManager.CreateNewWindow mWindowsMaker;

		// Token: 0x04000037 RID: 55
		private readonly IImGuiWindow mMainWindow;

		// Token: 0x04000038 RID: 56
		private readonly Platform_CreateWindow mCreateWindow;

		// Token: 0x04000039 RID: 57
		private readonly Platform_DestroyWindow mDestroyWindow;

		// Token: 0x0400003A RID: 58
		private readonly Platform_GetWindowPos mGetWindowPos;

		// Token: 0x0400003B RID: 59
		private readonly Platform_ShowWindow mShowWindow;

		// Token: 0x0400003C RID: 60
		private readonly Platform_SetWindowPos mSetWindowPos;

		// Token: 0x0400003D RID: 61
		private readonly Platform_SetWindowSize mSetWindowSize;

		// Token: 0x0400003E RID: 62
		private readonly Platform_GetWindowSize mGetWindowSize;

		// Token: 0x0400003F RID: 63
		private readonly Platform_SetWindowFocus mSetWindowFocus;

		// Token: 0x04000040 RID: 64
		private readonly Platform_GetWindowFocus mGetWindowFocus;

		// Token: 0x04000041 RID: 65
		private readonly Platform_GetWindowMinimized mGetWindowMinimized;

		// Token: 0x04000042 RID: 66
		private readonly Platform_SetWindowTitle mSetWindowTitle;

		// Token: 0x04000043 RID: 67
		private readonly ImGuiWindowsManager.SetClipboardTextDlg mSetClipboardText;

		// Token: 0x04000044 RID: 68
		private readonly ImGuiWindowsManager.GetClipboardTextDlg mGetClipboardText;

		// Token: 0x04000045 RID: 69
		private bool mDisposed;

		// Token: 0x0200000E RID: 14
		// (Invoke) Token: 0x06000067 RID: 103
		[NullableContext(0)]
		public delegate IImGuiWindow CreateNewWindow(ImGuiViewportPtr viewport);

		// Token: 0x0200000F RID: 15
		// (Invoke) Token: 0x0600006B RID: 107
		[NullableContext(0)]
		private delegate void SetClipboardTextDlg(IntPtr userData, string text);

		// Token: 0x02000010 RID: 16
		// (Invoke) Token: 0x0600006F RID: 111
		[NullableContext(0)]
		private delegate IntPtr GetClipboardTextDlg(IntPtr userData);
	}
}
