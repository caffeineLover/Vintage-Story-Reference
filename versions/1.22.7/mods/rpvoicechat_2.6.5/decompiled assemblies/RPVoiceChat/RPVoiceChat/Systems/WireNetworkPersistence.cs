using System;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace RPVoiceChat.Systems
{
	// Token: 0x02000055 RID: 85
	public class WireNetworkPersistence : ModSystem
	{
		// Token: 0x0600021C RID: 540 RVA: 0x0000E302 File Offset: 0x0000C502
		public override bool ShouldLoad(EnumAppSide forSide)
		{
			return forSide == 1;
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0000E308 File Offset: 0x0000C508
		public override void StartServerSide(ICoreServerAPI api)
		{
			this.sapi = api;
			api.Event.SaveGameLoaded += new Action(this.OnSaveGameLoaded);
			api.Event.GameWorldSave += new Action(this.OnGameWorldSave);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0000E340 File Offset: 0x0000C540
		private void OnSaveGameLoaded()
		{
			WireTopologyRegistry.LoadFromSave(this.sapi.WorldManager.SaveGame.GetData("rpvc:wire-topology"));
			WirelessTopologyRegistry.LoadFromSave(this.sapi.WorldManager.SaveGame.GetData("rpvc:wireless-topology"));
			RadioRfPresenceRegistry.LoadFromSave(this.sapi.WorldManager.SaveGame.GetData("rpvc:radio-rf-presence"));
			byte[] networkBytes = this.sapi.WorldManager.SaveGame.GetData("rpvc:wire-networks");
			if (networkBytes == null)
			{
				return;
			}
			byte[] nextIdBytes = this.sapi.WorldManager.SaveGame.GetData("rpvc:wire-network-nextid");
			WireNetworkHandler.InitializeFromSave(networkBytes, nextIdBytes);
			WireNetworkHandler.RejoinLoadedNodes(this.sapi);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0000E3F8 File Offset: 0x0000C5F8
		private void OnGameWorldSave()
		{
			WireNetworkHandler.PersistToSaveGame(this.sapi.WorldManager.SaveGame);
			this.sapi.WorldManager.SaveGame.StoreData("rpvc:wire-topology", WireTopologyRegistry.ToSaveBytes());
			this.sapi.WorldManager.SaveGame.StoreData("rpvc:wireless-topology", WirelessTopologyRegistry.ToSaveBytes());
			this.sapi.WorldManager.SaveGame.StoreData("rpvc:radio-rf-presence", RadioRfPresenceRegistry.ToSaveBytes());
		}

		// Token: 0x040000DA RID: 218
		public const string NetworksDataKey = "rpvc:wire-networks";

		// Token: 0x040000DB RID: 219
		public const string NextNetworkIdDataKey = "rpvc:wire-network-nextid";

		// Token: 0x040000DC RID: 220
		private ICoreServerAPI sapi;
	}
}
