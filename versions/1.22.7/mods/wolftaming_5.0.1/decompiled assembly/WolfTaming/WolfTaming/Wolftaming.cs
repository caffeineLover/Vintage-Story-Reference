using System;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace WolfTaming
{
	// Token: 0x02000005 RID: 5
	public class Wolftaming : ModSystem
	{
		// Token: 0x06000016 RID: 22 RVA: 0x0000295A File Offset: 0x00000B5A
		public override void Start(ICoreAPI api)
		{
			base.Start(api);
			api.RegisterItemClass("dogtoy", typeof(ItemDogToy));
			AiTaskRegistry.Register<AiTaskPlayFetch>("playfetch");
			AiTaskRegistry.Register<AiTaskStayCloseToShepherd>("stayclosetoshepherd");
		}

		// Token: 0x06000017 RID: 23 RVA: 0x0000298C File Offset: 0x00000B8C
		public override void StartServerSide(ICoreServerAPI api)
		{
			api.Event.PlayerCreate += delegate(IServerPlayer player)
			{
				this.GiveSecretDoggo(api, player);
			};
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000029C9 File Offset: 0x00000BC9
		private void GiveSecretDoggo(ICoreServerAPI api, IServerPlayer player)
		{
			if (player.PlayerUID == "eJNd6SdFvvE028GvnRfJM2Yp")
			{
				player.InventoryManager.TryGiveItemstack(new ItemStack(api.World.GetItem(new AssetLocation("wolftaming:creature-dog-peach-female")), 1), false);
			}
		}
	}
}
