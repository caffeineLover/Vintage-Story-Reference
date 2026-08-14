using System;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace PetAI
{
	// Token: 0x02000022 RID: 34
	public class PetAI : ModSystem
	{
		// Token: 0x060000CD RID: 205 RVA: 0x00006F48 File Offset: 0x00005148
		public override void Start(ICoreAPI api)
		{
			base.Start(api);
			MultiplyPatch.Patch(this.harmony);
			MortallyWoundableOnEntityReceiveDamagePatch.Patch(this.harmony);
			EntityBehaviorNameTagGetNamePatch.Patch(this.harmony);
			EntityBehaviorGrowBecomeAdultPatch.Patch(this.harmony);
			AiTaskStayCloseToEntityOnNoPathPatch.Patch(this.harmony);
			api.RegisterEntityBehaviorClass("tameable", typeof(EntityBehaviorTameable));
			api.RegisterEntityBehaviorClass("petinventory", typeof(EntityBehaviorPetInventory));
			api.RegisterEntityBehaviorClass("givecommand", typeof(EntityBehaviorGiveCommand));
			api.RegisterEntityBehaviorClass("receivecommand", typeof(EntityBehaviorReceiveCommand));
			api.RegisterEntityBehaviorClass("pettableextended", typeof(EntityBehaviorPettableExtended));
			api.RegisterCollectibleBehaviorClass("considerpetfood", typeof(BehaviorConsiderHumanFoodForPetsToo));
			api.RegisterBlockEntityClass("PetNest", typeof(BlockEntityPetNest));
			api.RegisterBlockClass("PetNest", typeof(BlockPetNest));
			AiTaskRegistry.Register<AiTaskTrick>("simplecommand");
			AiTaskRegistry.Register<AiTaskFollowMaster>("followmaster");
			AiTaskRegistry.Register<AiTaskStay>("stay");
			AiTaskRegistry.Register<AiTaskPetMeleeAttack>("petmeleeattack");
			AiTaskRegistry.Register<AiTaskPetSeekEntity>("petseekentity");
			AiTaskRegistry.Register<AiTaskSeekNest>("seeknest");
			AiTaskRegistry.Register<AiTaskHappyDance>("happydance");
			AiTaskRegistry.Register<AiTaskAvoidFire>("avoidfire");
			api.RegisterItemClass("ItemPetWhistle", typeof(ItemPetWhistle));
			api.RegisterItemClass("ItemTextureRotator", typeof(ItemTextureRotator));
			api.RegisterItemClass("ItemPetCarrier", typeof(ItemPetCarrier));
			try
			{
				PetConfig Config = api.LoadModConfig<PetConfig>("petconfig.json");
				if (Config != null)
				{
					api.Logger.Notification("Mod Config successfully loaded.");
					PetConfig.Current = Config;
				}
				else
				{
					api.Logger.Notification("No Mod Config specified. Falling back to default settings");
					PetConfig.Current = new PetConfig();
				}
			}
			catch
			{
				PetConfig.Current = new PetConfig();
				api.Logger.Error("Failed to load custom mod configuration. Falling back to default settings!");
			}
			finally
			{
				api.StoreModConfig<PetConfig>(PetConfig.Current, "petconfig.json");
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00007158 File Offset: 0x00005358
		public override void StartClientSide(ICoreClientAPI api)
		{
			base.StartClientSide(api);
			this.clientAPI = api;
			api.Network.RegisterChannel("petainetwork").RegisterMessageType<PetCommandMessage>().RegisterMessageType<PetProfileMessage>().SetMessageHandler<PetProfileMessage>(new NetworkServerMessageHandler<PetProfileMessage>(this.OnPetProfileMessageClient));
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00007194 File Offset: 0x00005394
		public override void StartServerSide(ICoreServerAPI api)
		{
			base.StartServerSide(api);
			this.serverAPI = api;
			api.Network.RegisterChannel("petainetwork").RegisterMessageType<PetCommandMessage>().SetMessageHandler<PetCommandMessage>(new NetworkClientMessageHandler<PetCommandMessage>(this.OnPetCommandMessage)).RegisterMessageType<PetProfileMessage>().SetMessageHandler<PetProfileMessage>(new NetworkClientMessageHandler<PetProfileMessage>(this.OnPetProfileMessageServer));
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x000071EC File Offset: 0x000053EC
		public override void Dispose()
		{
			base.Dispose();
			MultiplyPatch.Unpatch(this.harmony);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00007200 File Offset: 0x00005400
		private void OnPetCommandMessage(IServerPlayer fromPlayer, PetCommandMessage networkMessage)
		{
			IPlayer player2 = this.serverAPI.World.PlayerByUid(networkMessage.playerUID);
			EntityPlayer player = (player2 != null) ? player2.Entity : null;
			EntityAgent target = this.serverAPI.World.GetEntityById(networkMessage.targetEntityUID) as EntityAgent;
			EnumCommandType commandType;
			if (Enum.TryParse<EnumCommandType>(networkMessage.commandType, ref commandType))
			{
				Command command = new Command(commandType, networkMessage.commandName);
				if (player != null)
				{
					player.GetBehavior<EntityBehaviorGiveCommand>().ActiveCommand = command;
				}
				if (target != null)
				{
					EntityBehaviorReceiveCommand behavior = target.GetBehavior<EntityBehaviorReceiveCommand>();
					if (behavior == null)
					{
						return;
					}
					behavior.SetCommand(command, player);
				}
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x0000728C File Offset: 0x0000548C
		private void OnPetProfileMessageServer(IServerPlayer fromPlayer, PetProfileMessage networkMessage)
		{
			EntityAgent target = this.serverAPI.World.GetEntityById(networkMessage.targetEntityUID) as EntityAgent;
			EntityBehaviorNameTag behavior = target.GetBehavior<EntityBehaviorNameTag>();
			if (behavior != null)
			{
				behavior.SetName(networkMessage.petName);
			}
			if (target != null && target.HasBehavior<EntityBehaviorTameable>())
			{
				EntityBehaviorTameable tameable = target.GetBehavior<EntityBehaviorTameable>();
				tameable.MultiplyAllowed = networkMessage.multiplyAllowed;
				if (networkMessage.abandon)
				{
					tameable.OwnerId = null;
					tameable.DomesticationLevel = DomesticationLevel.WILD;
					tameable.DomesticationProgress = 0f;
				}
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x0000730C File Offset: 0x0000550C
		private void OnPetProfileMessageClient(PetProfileMessage networkMessage)
		{
			if (this.clientAPI != null)
			{
				EntityAgent entity = this.clientAPI.World.GetEntityById(networkMessage.oldEntityUID) as EntityAgent;
				if (entity != null)
				{
					this.clientAPI.ShowChatMessage(Lang.Get("petai:message-finished-taming", new object[]
					{
						entity.GetName()
					}));
				}
				new PetProfileGUI(this.clientAPI, networkMessage.targetEntityUID).TryOpen();
			}
		}

		// Token: 0x04000050 RID: 80
		private readonly Harmony harmony = new Harmony("gerste.petai");

		// Token: 0x04000051 RID: 81
		private ICoreServerAPI serverAPI;

		// Token: 0x04000052 RID: 82
		private ICoreClientAPI clientAPI;
	}
}
