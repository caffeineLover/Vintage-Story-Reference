using System;
using System.Collections.Generic;
using RPVoiceChat.GameContent.BlockEntity;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000030 RID: 48
	public class PrinterSystem : ModSystem
	{
		// Token: 0x0600011E RID: 286 RVA: 0x00008505 File Offset: 0x00006705
		public override void StartServerSide(ICoreServerAPI api)
		{
			this.sapi = api;
			api.Event.RegisterGameTickListener(new Action<float>(this.OnGameTick), 1000, 0);
		}

		// Token: 0x0600011F RID: 287 RVA: 0x0000852C File Offset: 0x0000672C
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

		// Token: 0x06000120 RID: 288 RVA: 0x0000858C File Offset: 0x0000678C
		public void RegisterTelegraphWithPrinter(BlockEntityTelegraph telegraph)
		{
			if (!this.telegraphsWithPrinters.Contains(telegraph))
			{
				this.telegraphsWithPrinters.Add(telegraph);
			}
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000085A8 File Offset: 0x000067A8
		public void UnregisterTelegraphWithPrinter(BlockEntityTelegraph telegraph)
		{
			this.telegraphsWithPrinters.Remove(telegraph);
		}

		// Token: 0x0400006D RID: 109
		private List<BlockEntityTelegraph> telegraphsWithPrinters = new List<BlockEntityTelegraph>();

		// Token: 0x0400006E RID: 110
		private ICoreServerAPI sapi;
	}
}
