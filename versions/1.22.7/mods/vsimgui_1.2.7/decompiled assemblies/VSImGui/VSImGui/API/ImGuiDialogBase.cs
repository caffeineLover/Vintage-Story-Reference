using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;

namespace VSImGui.API
{
	// Token: 0x0200002A RID: 42
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class ImGuiDialogBase : IDisposable
	{
		// Token: 0x06000292 RID: 658 RVA: 0x0000D662 File Offset: 0x0000B862
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000293 RID: 659 RVA: 0x0000D671 File Offset: 0x0000B871
		public void Close()
		{
			if (this.OnClose())
			{
				this.Opened = false;
			}
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000D682 File Offset: 0x0000B882
		public void Open()
		{
			if (this.OnOpen())
			{
				this.Opened = true;
				this._system.Show();
			}
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000D6A0 File Offset: 0x0000B8A0
		protected ImGuiDialogBase(ICoreClientAPI api)
		{
			this.ClientApi = api;
			this._system = api.ModLoader.GetModSystem<ImGuiModSystem>(true);
			this._system.Draw += this.Draw;
			this._system.Closed += new Action(this.Close);
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000296 RID: 662 RVA: 0x0000D6FB File Offset: 0x0000B8FB
		protected ICoreClientAPI ClientApi { get; }

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000297 RID: 663 RVA: 0x0000D703 File Offset: 0x0000B903
		// (set) Token: 0x06000298 RID: 664 RVA: 0x0000D70B File Offset: 0x0000B90B
		private protected bool Opened { protected get; private set; }

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000299 RID: 665 RVA: 0x0000D714 File Offset: 0x0000B914
		// (set) Token: 0x0600029A RID: 666 RVA: 0x0000D71C File Offset: 0x0000B91C
		private protected bool GrabMouse { protected get; private set; }

		// Token: 0x0600029B RID: 667
		protected abstract bool OnDraw();

		// Token: 0x0600029C RID: 668 RVA: 0x0000D725 File Offset: 0x0000B925
		protected virtual bool OnClose()
		{
			return true;
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000D728 File Offset: 0x0000B928
		protected virtual bool OnOpen()
		{
			return true;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000D72C File Offset: 0x0000B92C
		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				if (disposing)
				{
					this._system.Draw -= this.Draw;
					this._system.Closed -= new Action(this.Close);
				}
				this._disposed = true;
			}
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000D77A File Offset: 0x0000B97A
		protected virtual CallbackGUIStatus Draw(float deltaSeconds)
		{
			if (!this.Opened)
			{
				return this.DrawStatus();
			}
			if (!this.OnDraw())
			{
				this.Close();
			}
			return this.DrawStatus();
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x0000D79F File Offset: 0x0000B99F
		protected CallbackGUIStatus DrawStatus()
		{
			if (!this.Opened)
			{
				return CallbackGUIStatus.Closed;
			}
			if (!this.GrabMouse)
			{
				return CallbackGUIStatus.DontGrabMouse;
			}
			return CallbackGUIStatus.GrabMouse;
		}

		// Token: 0x04000056 RID: 86
		private bool _disposed;

		// Token: 0x04000057 RID: 87
		private readonly ImGuiModSystem _system;
	}
}
