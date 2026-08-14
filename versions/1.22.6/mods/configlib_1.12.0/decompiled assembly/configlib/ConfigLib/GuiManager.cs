using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using VSImGui;
using VSImGui.API;

namespace ConfigLib
{
	// Token: 0x0200004B RID: 75
	[NullableContext(2)]
	[Nullable(0)]
	internal class GuiManager : IDisposable
	{
		// Token: 0x1400000E RID: 14
		// (add) Token: 0x0600021A RID: 538 RVA: 0x0000CB64 File Offset: 0x0000AD64
		// (remove) Token: 0x0600021B RID: 539 RVA: 0x0000CB9C File Offset: 0x0000AD9C
		public event Action ConfigWindowClosed;

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x0600021C RID: 540 RVA: 0x0000CBD4 File Offset: 0x0000ADD4
		// (remove) Token: 0x0600021D RID: 541 RVA: 0x0000CC0C File Offset: 0x0000AE0C
		public event Action ConfigWindowOpened;

		// Token: 0x0600021E RID: 542 RVA: 0x0000CC44 File Offset: 0x0000AE44
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

		// Token: 0x0600021F RID: 543 RVA: 0x0000CCE0 File Offset: 0x0000AEE0
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

		// Token: 0x06000220 RID: 544 RVA: 0x0000CD02 File Offset: 0x0000AF02
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

		// Token: 0x06000221 RID: 545 RVA: 0x0000CD1B File Offset: 0x0000AF1B
		public static bool ShowConfigWindowStatic()
		{
			GuiManager instance = GuiManager._instance;
			if (instance != null)
			{
				instance.ToggleConfigWindow();
			}
			return true;
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000CD2F File Offset: 0x0000AF2F
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

		// Token: 0x06000223 RID: 547 RVA: 0x0000CD5C File Offset: 0x0000AF5C
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

		// Token: 0x06000224 RID: 548 RVA: 0x0000CD02 File Offset: 0x0000AF02
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

		// Token: 0x06000225 RID: 549 RVA: 0x0000CD80 File Offset: 0x0000AF80
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

		// Token: 0x06000226 RID: 550 RVA: 0x0000CDCD File Offset: 0x0000AFCD
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0400010E RID: 270
		[Nullable(1)]
		private readonly ImGuiModSystem _modSystem;

		// Token: 0x0400010F RID: 271
		[Nullable(1)]
		private readonly ConfigWindow _configWindow;

		// Token: 0x04000110 RID: 272
		private bool _disposed;

		// Token: 0x04000111 RID: 273
		private bool _showConfig;

		// Token: 0x04000112 RID: 274
		private static GuiManager _instance;
	}
}
