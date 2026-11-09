using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using VSImGui;
using VSImGui.API;

namespace ConfigLib
{
	// Token: 0x0200004D RID: 77
	[NullableContext(2)]
	[Nullable(0)]
	internal class GuiManager : IDisposable
	{
		// Token: 0x1400000E RID: 14
		// (add) Token: 0x0600023F RID: 575 RVA: 0x0000CE80 File Offset: 0x0000B080
		// (remove) Token: 0x06000240 RID: 576 RVA: 0x0000CEB8 File Offset: 0x0000B0B8
		public event Action ConfigWindowClosed;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000241 RID: 577 RVA: 0x0000CEF0 File Offset: 0x0000B0F0
		// (remove) Token: 0x06000242 RID: 578 RVA: 0x0000CF28 File Offset: 0x0000B128
		public event Action ConfigWindowOpened;

		// Token: 0x06000243 RID: 579 RVA: 0x0000CF60 File Offset: 0x0000B160
		[NullableContext(1)]
		public GuiManager(ICoreClientAPI api)
		{
			api.Input.RegisterHotKey("configlibconfigs", "(Config lib) Open configs window", 98, 8, false, false, false);
			api.Input.SetHotKeyHandler("configlibconfigs", new ActionConsumable<KeyCombination>(this.ShowConfigWindow));
			this._modSystem = api.ModLoader.GetModSystem<ImGuiModSystem>(true);
			this._modSystem.Draw += new DrawCallbackDelegate(this.Draw);
			this._modSystem.Closed += new Action(this.CloseConfigWindow);
			this._configWindow = new ConfigWindow(api);
			GuiManager._instance = this;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000CFFC File Offset: 0x0000B1FC
		public CallbackGUIStatus Draw(float deltaSeconds)
		{
			if (!this._showConfig)
			{
				return 0;
			}
			if (!this._configWindow.Draw())
			{
				this.CloseConfigWindow();
				return 0;
			}
			return 1;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000D01E File Offset: 0x0000B21E
		public bool ToggleConfigWindow()
		{
			if (!this._showConfig)
			{
				this.OpenConfigWindow();
			}
			else
			{
				this.CloseConfigWindow();
			}
			return true;
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000D037 File Offset: 0x0000B237
		public static bool ShowConfigWindowStatic()
		{
			GuiManager instance = GuiManager._instance;
			if (instance != null)
			{
				instance.ToggleConfigWindow();
			}
			return true;
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000D04B File Offset: 0x0000B24B
		public void OpenConfigWindow()
		{
			if (!this._showConfig)
			{
				Action configWindowOpened = this.ConfigWindowOpened;
				if (configWindowOpened != null)
				{
					configWindowOpened.Invoke();
				}
			}
			this._showConfig = true;
			this._modSystem.Show();
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000D078 File Offset: 0x0000B278
		public void CloseConfigWindow()
		{
			if (this._showConfig)
			{
				Action configWindowClosed = this.ConfigWindowClosed;
				if (configWindowClosed != null)
				{
					configWindowClosed.Invoke();
				}
			}
			this._showConfig = false;
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0000D01E File Offset: 0x0000B21E
		[NullableContext(1)]
		private bool ShowConfigWindow(KeyCombination keyCombination)
		{
			if (!this._showConfig)
			{
				this.OpenConfigWindow();
			}
			else
			{
				this.CloseConfigWindow();
			}
			return true;
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0000D09C File Offset: 0x0000B29C
		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				if (disposing)
				{
					this._modSystem.Draw -= new DrawCallbackDelegate(this.Draw);
					this._modSystem.Closed -= new Action(this.CloseConfigWindow);
				}
				this._disposed = true;
			}
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0000D0E9 File Offset: 0x0000B2E9
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x04000111 RID: 273
		[Nullable(1)]
		private readonly ImGuiModSystem _modSystem;

		// Token: 0x04000112 RID: 274
		[Nullable(1)]
		private readonly ConfigWindow _configWindow;

		// Token: 0x04000113 RID: 275
		private bool _disposed;

		// Token: 0x04000114 RID: 276
		private bool _showConfig;

		// Token: 0x04000115 RID: 277
		private static GuiManager _instance;
	}
}
