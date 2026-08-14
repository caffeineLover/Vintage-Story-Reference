using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ImGuiController_OpenTK
{
	// Token: 0x0200000A RID: 10
	[NullableContext(1)]
	[Nullable(0)]
	public class ImGuiWindow : GameWindow, IImGuiWindow, IWindow, IDisposable
	{
		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600003C RID: 60 RVA: 0x00003AF1 File Offset: 0x00001CF1
		public NativeWindow Native
		{
			get
			{
				return this;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600003D RID: 61 RVA: 0x00003AF4 File Offset: 0x00001CF4
		public IImGuiRenderer ImGuiRenderer
		{
			get
			{
				return this.mImGuiRenderer;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600003E RID: 62 RVA: 0x00003AFC File Offset: 0x00001CFC
		public ImGuiViewportPtr Viewport
		{
			get
			{
				return this.mViewport;
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00003B04 File Offset: 0x00001D04
		public void ContextMakeCurrent()
		{
			base.Context.MakeCurrent();
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003B14 File Offset: 0x00001D14
		public unsafe ImGuiWindow(ImGuiViewportPtr viewport, NativeWindow mainWindow, ImGuiRenderer renderer, ImGuiController controller) : base(GameWindowSettings.Default, new NativeWindowSettings
		{
			SharedContext = mainWindow.Context,
			WindowBorder = ImGuiWindow.GetBorderSettings(viewport),
			StartVisible = false,
			Location = new Vector2i?(new Vector2i((int)viewport.Pos.X, (int)viewport.Pos.Y)),
			Size = new Vector2i((int)viewport.Size.X, (int)viewport.Size.Y),
			APIVersion = new Version(3, 3)
		})
		{
			this.mViewport = viewport;
			GCHandle gcHandle = GCHandle.Alloc(this);
			this.mViewport.PlatformUserData = (IntPtr)gcHandle;
			this.mImGuiRenderer = new ImGuiRenderer(renderer, viewport, this);
			base.Resize += delegate(ResizeEventArgs _)
			{
				*this.mViewport.PlatformRequestResize = true;
			};
			base.Move += delegate(WindowPositionEventArgs _)
			{
				*this.mViewport.PlatformRequestMove = true;
			};
			base.Closing += delegate(CancelEventArgs _)
			{
				*this.mViewport.PlatformRequestClose = true;
			};
			base.MouseWheel += new Action<MouseWheelEventArgs>(controller.OnMouseScroll);
			base.TextInput += new Action<TextInputEventArgs>(controller.OnTextInput);
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003C35 File Offset: 0x00001E35
		public void OnRender(float deltaSeconds)
		{
			base.OnRenderFrame(new FrameEventArgs((double)deltaSeconds));
			GL.ClearColor(new Color4(0, 32, 48, byte.MaxValue));
			GL.Clear(17664);
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003C62 File Offset: 0x00001E62
		public void OnUpdate(float deltaSeconds)
		{
			base.NewInputFrame();
			NativeWindow.ProcessWindowEvents(base.IsEventDriven);
			base.UpdateTime = (double)deltaSeconds;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003C7D File Offset: 0x00001E7D
		public void OnDraw(float deltaSeconds)
		{
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003C80 File Offset: 0x00001E80
		public void SetMouseButton(MouseButton button, bool value)
		{
			MouseState state = base.MouseState;
			FieldInfo field = typeof(MouseState).GetField("_buttons", 36);
			BitArray buttons = (BitArray)((field != null) ? field.GetValue(state) : null);
			if (buttons == null)
			{
				return;
			}
			buttons[button] = value;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003CC9 File Offset: 0x00001EC9
		public void ReleaseLeftMouseButton()
		{
			this.SetMouseButton(0, false);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003CD3 File Offset: 0x00001ED3
		protected unsafe override void OnResize(ResizeEventArgs e)
		{
			base.OnResize(e);
			Window* currentContext = GLFW.GetCurrentContext();
			this.Native.Context.MakeCurrent();
			GL.Viewport(0, 0, base.ClientSize.X, base.ClientSize.Y);
			GLFW.MakeContextCurrent(currentContext);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003D13 File Offset: 0x00001F13
		protected unsafe static WindowBorder GetBorderSettings(ImGuiViewportPtr viewport)
		{
			if ((*viewport.Flags & 8) != 0)
			{
				return 2;
			}
			return 0;
		}

		// Token: 0x04000030 RID: 48
		protected readonly ImGuiRenderer mImGuiRenderer;

		// Token: 0x04000031 RID: 49
		protected readonly ImGuiViewportPtr mViewport;
	}
}
