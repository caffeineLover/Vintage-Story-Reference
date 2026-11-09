using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using VSImGui.API;
using VSImGui.Debug;

namespace VSImGui
{
	// Token: 0x02000004 RID: 4
	[NullableContext(2)]
	[Nullable(0)]
	public class ImGuiModSystem : ModSystem, IImGuiRenderer
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000C RID: 12 RVA: 0x0000219B File Offset: 0x0000039B
		// (set) Token: 0x0600000D RID: 13 RVA: 0x000021A3 File Offset: 0x000003A3
		public Style DefaultStyle { get; private set; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x0600000E RID: 14 RVA: 0x000021AC File Offset: 0x000003AC
		// (remove) Token: 0x0600000F RID: 15 RVA: 0x000021C2 File Offset: 0x000003C2
		[Nullable(1)]
		public event DrawCallbackDelegate Draw
		{
			[NullableContext(1)]
			add
			{
				if (this._guiManager != null)
				{
					this._guiManager.DrawCallback += value;
				}
			}
			[NullableContext(1)]
			remove
			{
				if (this._guiManager != null)
				{
					this._guiManager.DrawCallback -= value;
				}
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000021D8 File Offset: 0x000003D8
		public void Show()
		{
			VSImGuiDialog dialog = this._dialog;
			if (dialog == null)
			{
				return;
			}
			dialog.TryOpen();
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000011 RID: 17 RVA: 0x000021EC File Offset: 0x000003EC
		// (remove) Token: 0x06000012 RID: 18 RVA: 0x00002224 File Offset: 0x00000424
		public event Action Closed;

		// Token: 0x06000013 RID: 19 RVA: 0x0000225C File Offset: 0x0000045C
		[NullableContext(1)]
		public override void StartPre(ICoreAPI api)
		{
			ICoreClientAPI clientApi = api as ICoreClientAPI;
			if (clientApi == null)
			{
				return;
			}
			if (!NativesLoader.Load(api.Logger, this))
			{
				return;
			}
			ImGuiConfig config = api.LoadModConfig<ImGuiConfig>("imgui.json");
			if (config == null || config.Version < 2)
			{
				this._config = new ImGuiConfig();
				this._config.Version = 2;
				if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
				{
					this._config.MultiViewportSupport = false;
				}
				else
				{
					this._config.MultiViewportSupport = true;
				}
				api.StoreModConfig<ImGuiConfig>(this._config, "imgui.json");
			}
			else
			{
				this._config = config;
			}
			this._guiManager = new DrawCallbacksManager();
			this._mainWindowWrapper = new MainGameWindowWrapper(clientApi);
			this._controller = new Controller(this._mainWindowWrapper, this._config.MultiViewportSupport);
			this._dialog = new VSImGuiDialog(clientApi, this._controller, this._mainWindowWrapper, this._guiManager);
			this._dialog.TryOpen();
			this._dialog.OnClosed += delegate()
			{
				Action closed = this.Closed;
				if (closed == null)
				{
					return;
				}
				closed.Invoke();
			};
			this._controller.OnWindowMergedIntoMain += delegate()
			{
				this._dialog.TryOpen();
			};
			this._offWindowRenderer = new OffWindowRenderer(this._dialog);
			clientApi.Event.RegisterRenderer(this._offWindowRenderer, 10, null);
			clientApi.Input.RegisterHotKey("imguitoggle", Lang.Get("vsimgui:imgui-toggle", Array.Empty<object>()), 98, 2, false, true, false);
			this.Draw += this.DrawDebugWindow;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000023D9 File Offset: 0x000005D9
		public override double ExecuteOrder()
		{
			return 0.0;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000023E4 File Offset: 0x000005E4
		public override bool ShouldLoad(EnumAppSide forSide)
		{
			return forSide == 2;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000023EC File Offset: 0x000005EC
		[NullableContext(1)]
		public override void AssetsLoaded(ICoreAPI api)
		{
			ICoreClientAPI clientApi = api as ICoreClientAPI;
			if (clientApi == null)
			{
				return;
			}
			byte[] data = api.Assets.Get("vsimgui:config/defaultstyle.json").Data;
			string serializedStyle = Encoding.UTF8.GetString(data);
			this.DefaultStyle = JsonConvert.DeserializeObject<Style>(serializedStyle);
			Style defaultStyle = this.DefaultStyle;
			if (defaultStyle != null)
			{
				defaultStyle.Push();
			}
			clientApi.Input.RegisterHotKey("imguiincfont", Lang.Get("vsimgui:imgui-increase-font-size", Array.Empty<object>()), 121, 2, false, true, false);
			clientApi.Input.RegisterHotKey("imguidecfont", Lang.Get("vsimgui:imgui-decrease-font-size", Array.Empty<object>()), 120, 2, false, true, false);
			clientApi.Input.SetHotKeyHandler("imguiincfont", (KeyCombination _) => this.IncreaseFontSize());
			clientApi.Input.SetHotKeyHandler("imguidecfont", (KeyCombination _) => this.DecreaseFontSize());
			this.Draw += this.DrawCallbackForFontChange;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000024DC File Offset: 0x000006DC
		public override void Dispose()
		{
			DebugWindowsManager.Clear();
			Controller controller = this._controller;
			if (controller != null)
			{
				controller.Dispose();
			}
			MainGameWindowWrapper mainWindowWrapper = this._mainWindowWrapper;
			if (mainWindowWrapper != null)
			{
				mainWindowWrapper.Dispose();
			}
			VSImGuiDialog dialog = this._dialog;
			if (dialog != null)
			{
				dialog.Dispose();
			}
			OffWindowRenderer offWindowRenderer = this._offWindowRenderer;
			if (offWindowRenderer != null)
			{
				offWindowRenderer.Dispose();
			}
			base.Dispose();
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002538 File Offset: 0x00000738
		private bool IncreaseFontSize()
		{
			this._fontSizeChange++;
			return true;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002549 File Offset: 0x00000749
		private bool DecreaseFontSize()
		{
			this._fontSizeChange--;
			return true;
		}

		// Token: 0x0600001A RID: 26 RVA: 0x0000255C File Offset: 0x0000075C
		private CallbackGUIStatus DrawCallbackForFontChange(float deltaSeconds)
		{
			Style defaultStyle = this.DefaultStyle;
			if (defaultStyle != null)
			{
				defaultStyle.Pop();
			}
			if (this._fontSizeChange != 0 && this.DefaultStyle != null)
			{
				this.DefaultStyle.FontSize = this.GetFontSize(this.DefaultStyle.FontSize, this._fontSizeChange);
				this._fontSizeChange = 0;
			}
			Style defaultStyle2 = this.DefaultStyle;
			if (defaultStyle2 != null)
			{
				defaultStyle2.Push();
			}
			return CallbackGUIStatus.Closed;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000025C8 File Offset: 0x000007C8
		private int GetFontSize(int current, int change)
		{
			int newIndex = GameMath.Clamp(FontManager.Sizes.IndexOf(current) + change, 0, FontManager.Sizes.Count - 1);
			return FontManager.Sizes[newIndex];
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002600 File Offset: 0x00000800
		private CallbackGUIStatus DrawDebugWindow(float timeSeconds)
		{
			if (!DebugWindowsManager.Draw())
			{
				return CallbackGUIStatus.Closed;
			}
			return CallbackGUIStatus.DontGrabMouse;
		}

		// Token: 0x04000007 RID: 7
		private Controller _controller;

		// Token: 0x04000008 RID: 8
		private MainGameWindowWrapper _mainWindowWrapper;

		// Token: 0x04000009 RID: 9
		private VSImGuiDialog _dialog;

		// Token: 0x0400000A RID: 10
		private DrawCallbacksManager _guiManager;

		// Token: 0x0400000B RID: 11
		private OffWindowRenderer _offWindowRenderer;

		// Token: 0x0400000C RID: 12
		private int _fontSizeChange;

		// Token: 0x0400000D RID: 13
		[Nullable(1)]
		private ImGuiConfig _config = new ImGuiConfig();
	}
}
