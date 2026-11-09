using System;
using System.Collections.Generic;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000031 RID: 49
	public class PrinterSystem : ModSystem
	{
		// Token: 0x0600012A RID: 298 RVA: 0x00008F21 File Offset: 0x00007121
		public override void StartServerSide(ICoreServerAPI api)
		{
			this.sapi = api;
			api.Event.RegisterGameTickListener(new Action<float>(this.OnGameTick), 1000, 0);
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00008F48 File Offset: 0x00007148
		private void OnGameTick(float dt)
		{
			for (int i = this.telegraphsWithPrinters.Count - 1; i >= 0; i--)
			{
				BlockEntityTelegraph telegraph = this.telegraphsWithPrinters[i];
				bool flag;
				if (telegraph == null)
				{
					flag = (null != null);
				}
				else
				{
					ICoreAPI api = telegraph.Api;
					flag = (((api != null) ? api.World : null) != null);
				}
				if (!flag)
				{
					this.telegraphsWithPrinters.RemoveAt(i);
				}
				else
				{
					telegraph.CheckAutoSave();
				}
			}
		}

		// Token: 0x0600012C RID: 300 RVA: 0x00008FA8 File Offset: 0x000071A8
		public void RegisterTelegraphWithPrinter(BlockEntityTelegraph telegraph)
		{
			if (!this.telegraphsWithPrinters.Contains(telegraph))
			{
				this.telegraphsWithPrinters.Add(telegraph);
			}
		}

		// Token: 0x0600012D RID: 301 RVA: 0x00008FC4 File Offset: 0x000071C4
		public void UnregisterTelegraphWithPrinter(BlockEntityTelegraph telegraph)
		{
			this.telegraphsWithPrinters.Remove(telegraph);
		}

		// Token: 0x0400007B RID: 123
		private List<BlockEntityTelegraph> telegraphsWithPrinters = new List<BlockEntityTelegraph>();

		// Token: 0x0400007C RID: 124
		private ICoreServerAPI sapi;
	}
}
