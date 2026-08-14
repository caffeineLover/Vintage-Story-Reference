using System;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace RPVoiceChat.Systems
{
	// Token: 0x0200003B RID: 59
	public class WireNetworkPersistence : ModSystem
	{
		// Token: 0x0600018B RID: 395 RVA: 0x0000AA12 File Offset: 0x00008C12
		public override bool ShouldLoad(EnumAppSide forSide)
		{
			return forSide == 1;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000AA18 File Offset: 0x00008C18
		public override void StartServerSide(ICoreServerAPI api)
		{
			this.sapi = api;
			api.Event.SaveGameLoaded += new Action(this.OnSaveGameLoaded);
			api.Event.GameWorldSave += new Action(this.OnGameWorldSave);
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000AA50 File Offset: 0x00008C50
		private void OnSaveGameLoaded()
		{
			WireTopologyRegistry.LoadFromSave(this.sapi.WorldManager.SaveGame.GetData("rpvc:wire-topology"));
			WirelessTopologyRegistry.LoadFromSave(this.sapi.WorldManager.SaveGame.GetData("rpvc:wireless-topology"));
			byte[] networkBytes = this.sapi.WorldManager.SaveGame.GetData("rpvc:wire-networks");
			if (networkBytes == null)
			{
				return;
			}
			byte[] nextIdBytes = this.sapi.WorldManager.SaveGame.GetData("rpvc:wire-network-nextid");
			WireNetworkHandler.InitializeFromSave(networkBytes, nextIdBytes);
			WireNetworkHandler.RejoinLoadedNodes(this.sapi);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000AAE8 File Offset: 0x00008CE8
		private void OnGameWorldSave()
		{
			WireNetworkHandler.PersistToSaveGame(this.sapi.WorldManager.SaveGame);
			this.sapi.WorldManager.SaveGame.StoreData("rpvc:wire-topology", WireTopologyRegistry.ToSaveBytes());
			this.sapi.WorldManager.SaveGame.StoreData("rpvc:wireless-topology", WirelessTopologyRegistry.ToSaveBytes());
		}

		// Token: 0x0400008F RID: 143
		public const string NetworksDataKey = "rpvc:wire-networks";

		// Token: 0x04000090 RID: 144
		public const string NextNetworkIdDataKey = "rpvc:wire-network-nextid";

		// Token: 0x04000091 RID: 145
		private ICoreServerAPI sapi;
	}
}
