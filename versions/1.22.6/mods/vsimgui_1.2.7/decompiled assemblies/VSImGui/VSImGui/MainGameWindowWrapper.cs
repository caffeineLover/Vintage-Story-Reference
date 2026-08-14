using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using ImGuiController_OpenTK;
using OpenTK.Windowing.Desktop;
using Vintagestory.API.Client;
using Vintagestory.Client.NoObf;

namespace VSImGui
{
	// Token: 0x02000006 RID: 6
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class MainGameWindowWrapper : IWindow, IDisposable
	{
		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000030 RID: 48 RVA: 0x000029FD File Offset: 0x00000BFD
		public NativeWindow Native
		{
			get
			{
				return this._window;
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000031 RID: 49 RVA: 0x00002A08 File Offset: 0x00000C08
		// (remove) Token: 0x06000032 RID: 50 RVA: 0x00002A40 File Offset: 0x00000C40
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		internal event Action<float> Draw;

		// Token: 0x06000033 RID: 51 RVA: 0x00002A78 File Offset: 0x00000C78
		public MainGameWindowWrapper(ICoreClientAPI clientApi)
		{
			FieldInfo field = typeof(ClientMain).GetField("Platform", 52);
			ClientPlatformWindows platform = (ClientPlatformWindows)((field != null) ? field.GetValue(clientApi.World as ClientMain) : null);
			GameWindowNative gameWindowNative = (platform != null) ? platform.window : null;
			if (gameWindowNative == null)
			{
				throw new InvalidOperationException("Unable to get game native window from client api");
			}
			this._window = gameWindowNative;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002ADF File Offset: 0x00000CDF
		public void ContextMakeCurrent()
		{
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002AE1 File Offset: 0x00000CE1
		public void Dispose()
		{
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002AE3 File Offset: 0x00000CE3
		public void OnDraw(float deltaSeconds)
		{
			Action<float> draw = this.Draw;
			if (draw == null)
			{
				return;
			}
			draw.Invoke(deltaSeconds);
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002AF6 File Offset: 0x00000CF6
		public void OnRender(float deltaSeconds)
		{
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00002AF8 File Offset: 0x00000CF8
		public void OnUpdate(float deltaSeconds)
		{
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00002AFA File Offset: 0x00000CFA
		public void SwapBuffers()
		{
		}

		// Token: 0x04000010 RID: 16
		private readonly GameWindowNative _window;
	}
}
