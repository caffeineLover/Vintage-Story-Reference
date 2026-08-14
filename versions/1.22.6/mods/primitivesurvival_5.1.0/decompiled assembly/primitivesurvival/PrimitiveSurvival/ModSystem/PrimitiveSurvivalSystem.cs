using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using HarmonyLib;
using PrimitiveSurvival.ModConfig;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000051 RID: 81
	public class PrimitiveSurvivalSystem : ModSystem
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060002FB RID: 763 RVA: 0x00022F58 File Offset: 0x00021158
		// (set) Token: 0x060002FC RID: 764 RVA: 0x00022F60 File Offset: 0x00021160
		public IShaderProgram EntityGenericShaderProgram { get; private set; }

		// Token: 0x060002FD RID: 765 RVA: 0x00022F6C File Offset: 0x0002116C
		public override void StartClientSide(ICoreClientAPI api)
		{
			base.StartClientSide(api);
			MethodInfo PSPumpkinPatchOriginal = typeof(PumpkinCropBehavior).GetMethod("CanSupportPumpkin");
			MethodInfo PSPumpkinPatchPrefix = typeof(PrimitiveSurvivalSystem.PS_CanSupportPumpkin_Patch).GetMethod("PSCanSupportPumpkinPrefix");
			this.harmony.Patch(PSPumpkinPatchOriginal, new HarmonyMethod(PSPumpkinPatchPrefix), null, null, null);
			MethodInfo originalSpawn = typeof(ItemWormGrunter).GetMethod("spawnWorm", 52);
			MethodInfo grunterPatchPrefix = typeof(PrimitiveSurvivalSystem.PS_WormSpawn_Patch).GetMethod("Prefix");
			if (originalSpawn != null && grunterPatchPrefix != null)
			{
				this.harmony.Patch(originalSpawn, new HarmonyMethod(grunterPatchPrefix), null, null, null);
			}
			if (ModConfig.Loaded.ShowModNameInHud)
			{
				MethodInfo PSBlockGetPlacedBlockInfoOriginal = typeof(Block).GetMethod("GetPlacedBlockInfo");
				MethodInfo PSBlockGetPlacedBlockInfoPostfix = typeof(PrimitiveSurvivalSystem.PS_BlockGetPlacedBlockInfo_Patch).GetMethod("PSBlockGetPlacedBlockInfoPostfix");
				this.harmony.Patch(PSBlockGetPlacedBlockInfoOriginal, null, new HarmonyMethod(PSBlockGetPlacedBlockInfoPostfix), null, null);
			}
			if (ModConfig.Loaded.ShowModNameInGuis)
			{
				MethodInfo PSCollectibleGetHeldItemInfoOriginal = typeof(CollectibleObject).GetMethod("GetHeldItemInfo");
				MethodInfo PSCollectibleGetHeldItemInfoPostfix = typeof(PrimitiveSurvivalSystem.PS_CollectibleGetHeldItemInfo_Patch).GetMethod("PSCollectibleGetHeldItemInfoPostfix");
				this.harmony.Patch(PSCollectibleGetHeldItemInfoOriginal, null, new HarmonyMethod(PSCollectibleGetHeldItemInfoPostfix), null, null);
			}
			this.capi = api;
			this.capi.Event.ReloadShader += new ActionBoolReturn(this.LoadCustomShaders);
			this.LoadCustomShaders();
			this.vrenderer = new VenomOverlayRenderer(api);
			api.Event.RegisterRenderer(this.vrenderer, 10, null);
			(this.capi.World as ClientMain).RegisterDialog(new GuiDialog[]
			{
				new GuiDialogHollowTransform(this.capi)
			});
			this.capi.Network.RegisterChannel("primitivesurvival").RegisterMessageType<SyncClientPacket>().SetMessageHandler<SyncClientPacket>(delegate(SyncClientPacket packet)
			{
				ModConfig.Loaded.DeadfallBaitStolenPercent = packet.DeadfallBaitStolenPercent;
				ModConfig.Loaded.DeadfallMaxAnimalHeight = packet.DeadfallMaxAnimalHeight;
				ModConfig.Loaded.DeadfallMaxDamageSet = packet.DeadfallMaxDamageSet;
				ModConfig.Loaded.DeadfallMaxDamageBaited = packet.DeadfallMaxDamageBaited;
				ModConfig.Loaded.DeadfallTrippedPercent = packet.DeadfallTrippedPercent;
				ModConfig.Loaded.FallDamageMultiplierWoodSpikes = packet.FallDamageMultiplierWoodSpikes;
				ModConfig.Loaded.FallDamageMultiplierMetalSpikes = packet.FallDamageMultiplierMetalSpikes;
				ModConfig.Loaded.FishBasketCatchPercent = packet.FishBasketCatchPercent;
				ModConfig.Loaded.FishBasketBaitedCatchPercent = packet.FishBasketBaitedCatchPercent;
				ModConfig.Loaded.FishBasketBaitStolenPercent = packet.FishBasketBaitStolenPercent;
				ModConfig.Loaded.FishBasketEscapePercent = packet.FishBasketEscapePercent;
				ModConfig.Loaded.FishBasketUpdateMinutes = packet.FishBasketUpdateMinutes;
				ModConfig.Loaded.FishBasketRotRemovedPercent = packet.FishBasketRotRemovedPercent;
				ModConfig.Loaded.FishChanceOfEggsPercent = packet.FishChanceOfEggsPercent;
				ModConfig.Loaded.FishChunkDepletionRate = packet.FishChunkDepletionRate;
				ModConfig.Loaded.FishChunkRepletionRate = packet.FishChunkRepletionRate;
				ModConfig.Loaded.FishChunkRepletionMinutes = packet.FishChunkRepletionMinutes;
				ModConfig.Loaded.FishEggsChunkRepletionRate = packet.FishEggsChunkRepletionRate;
				ModConfig.Loaded.FishChunkMaxDepletionPercent = packet.FishChunkMaxDepletionPercent;
				ModConfig.Loaded.FurrowedLandUpdateFrequency = packet.FurrowedLandUpdateFrequency;
				ModConfig.Loaded.FurrowedLandBlockageChancePercent = packet.FurrowedLandBlockageChancePercent;
				ModConfig.Loaded.FurrowedLandMinMoistureClose = packet.FurrowedLandMinMoistureClose;
				ModConfig.Loaded.FurrowedLandMinMoistureFar = packet.FurrowedLandMinMoistureFar;
				ModConfig.Loaded.LimbTrotlineCatchPercent = packet.LimbTrotlineCatchPercent;
				ModConfig.Loaded.LimbTrotlineBaitedCatchPercent = packet.LimbTrotlineBaitedCatchPercent;
				ModConfig.Loaded.LimbTrotlineLuredCatchPercent = packet.LimbTrotlineLuredCatchPercent;
				ModConfig.Loaded.LimbTrotlineBaitedLuredCatchPercent = packet.LimbTrotlineBaitedLuredCatchPercent;
				ModConfig.Loaded.LimbTrotlineBaitStolenPercent = packet.LimbTrotlineBaitStolenPercent;
				ModConfig.Loaded.LimbTrotlineUpdateMinutes = packet.LimbTrotlineUpdateMinutes;
				ModConfig.Loaded.LimbTrotlineRotRemovedPercent = packet.LimbTrotlineRotRemovedPercent;
				ModConfig.Loaded.MonkeyBridgeMaxLength = packet.MonkeyBridgeMaxLength;
				ModConfig.Loaded.RaftWaterSpeedModifier = packet.RaftWaterSpeedModifier;
				ModConfig.Loaded.RaftFlotationModifier = packet.RaftFlotationModifier;
				ModConfig.Loaded.SnareBaitStolenPercent = packet.SnareBaitStolenPercent;
				ModConfig.Loaded.SnareMaxAnimalHeight = packet.SnareMaxAnimalHeight;
				ModConfig.Loaded.SnareMaxDamageSet = packet.SnareMaxDamageSet;
				ModConfig.Loaded.SnareMaxDamageBaited = packet.SnareMaxDamageBaited;
				ModConfig.Loaded.SnareTrippedPercent = packet.SnareTrippedPercent;
				ModConfig.Loaded.SpawnMultiplierBioluminescentGlobe = packet.SpawnMultiplierBioluminescentGlobe;
				ModConfig.Loaded.SpawnMultiplierBioluminescentJelly = packet.SpawnMultiplierBioluminescentJelly;
				ModConfig.Loaded.SpawnMultiplierBioluminescentOrangeJelly = packet.SpawnMultiplierBioluminescentOrangeJelly;
				ModConfig.Loaded.SpawnMultiplierBioluminescentWorm = packet.SpawnMultiplierBioluminescentWorm;
				ModConfig.Loaded.SpawnMultiplierCrabBairdi = packet.SpawnMultiplierCrabBairdi;
				ModConfig.Loaded.SpawnMultiplierCrabLand = packet.SpawnMultiplierCrabLand;
				ModConfig.Loaded.SpawnMultiplierLivingDead = packet.SpawnMultiplierLivingDead;
				ModConfig.Loaded.SpawnMultiplierSnakeBlackRat = packet.SpawnMultiplierSnakeBlackRat;
				ModConfig.Loaded.SpawnMultiplierSnakeChainViper = packet.SpawnMultiplierSnakeChainViper;
				ModConfig.Loaded.SpawnMultiplierSnakeCoachWhip = packet.SpawnMultiplierSnakeCoachWhip;
				ModConfig.Loaded.SpawnMultiplierSnakePitViper = packet.SpawnMultiplierSnakePitViper;
				ModConfig.Loaded.SpawnMultiplierWillowispGreen = packet.SpawnMultiplierWillowispGreen;
				ModConfig.Loaded.SpawnMultiplierWillowispWhite = packet.SpawnMultiplierWillowispWhite;
				ModConfig.Loaded.SpawnMultiplierWillowispYellow = packet.SpawnMultiplierWillowispYellow;
				ModConfig.Loaded.TreeHollowsMaxItems = packet.TreeHollowsMaxItems;
				ModConfig.Loaded.TreeHollowsEnableDeveloperTools = packet.TreeHollowsEnableDeveloperTools;
				ModConfig.Loaded.TreeHollowsMaxPerChunk = packet.TreeHollowsMaxPerChunk;
				ModConfig.Loaded.TreeHollowsSpawnProbability = packet.TreeHollowsSpawnProbability;
				ModConfig.Loaded.TreeHollowsUpdateMinutes = packet.TreeHollowsUpdateMinutes;
				ModConfig.Loaded.WeirTrapCatchPercent = packet.WeirTrapCatchPercent;
				ModConfig.Loaded.WeirTrapEscapePercent = packet.WeirTrapEscapePercent;
				ModConfig.Loaded.WeirTrapUpdateMinutes = packet.WeirTrapUpdateMinutes;
				ModConfig.Loaded.WeirTrapRotRemovedPercent = packet.WeirTrapRotRemovedPercent;
				ModConfig.Loaded.WormFoundPercentRock = packet.WormFoundPercentRock;
				ModConfig.Loaded.WormFoundPercentStickFlint = packet.WormFoundPercentStickFlint;
			});
		}

		// Token: 0x060002FE RID: 766 RVA: 0x00023168 File Offset: 0x00021368
		public override void StartPre(ICoreAPI api)
		{
			string cfgFileName = this.thisModID + ".json";
			try
			{
				ModConfig fromDisk;
				if ((fromDisk = api.LoadModConfig<ModConfig>(cfgFileName)) == null)
				{
					api.StoreModConfig<ModConfig>(ModConfig.Loaded, cfgFileName);
				}
				else
				{
					ModConfig.Loaded = fromDisk;
				}
			}
			catch
			{
				api.StoreModConfig<ModConfig>(ModConfig.Loaded, cfgFileName);
			}
			api.World.Config.SetBool("FurrowedLandEnabled", ModConfig.Loaded.FurrowedLandEnabled);
			api.World.Config.SetBool("RaftEnabled", ModConfig.Loaded.RaftEnabled);
			base.StartPre(api);
			if (api.Side != 2 && ClientSettings.Inst.Int["maxTextureAtlasHeight"] < 4096)
			{
				try
				{
					ClientSettings.Inst.Int["maxTextureAtlasHeight"] = 4096;
				}
				catch
				{
				}
			}
		}

		// Token: 0x060002FF RID: 767 RVA: 0x0002325C File Offset: 0x0002145C
		public bool LoadCustomShaders()
		{
			this.EntityGenericShaderProgram = this.capi.Shader.NewShaderProgram();
			(this.EntityGenericShaderProgram as ShaderProgram).AssetDomain = "game";
			this.capi.Shader.RegisterFileShaderProgram("entityanimated", this.EntityGenericShaderProgram);
			this.EntityGenericShaderProgram.Compile();
			return true;
		}

		// Token: 0x06000300 RID: 768 RVA: 0x000232C0 File Offset: 0x000214C0
		public void RegisterClasses(ICoreAPI api)
		{
			api.RegisterEntity("entitynightcrawler", typeof(EntityNightcrawler));
			api.RegisterEntity("entityfireflies", typeof(EntityFireflies));
			api.RegisterEntity("entitypsglowingagent", typeof(EntityPSGlowingAgent));
			api.RegisterEntity("entitypsagent", typeof(EntityPSAgent));
			api.RegisterEntity("entitylivingdead", typeof(EntityLivingDead));
			api.RegisterEntity("entitygenericglowingagent", typeof(EntityGenericGlowingAgent));
			api.RegisterEntity("entityskullofthedead", typeof(EntitySkullOfTheDead));
			api.RegisterEntity("entitywillowisp", typeof(EntityWillowisp));
			api.RegisterEntity("entitybioluminescent", typeof(EntityBioluminescent));
			api.RegisterEntityBehaviorClass("carryable", typeof(EntityBehaviorCarryable));
			AiTaskRegistry.Register<AiTaskMeleeAttackVenomous>("meleeattackvenomous");
			AiTaskRegistry.Register<AiTaskMeleeAttackCrab>("meleeattackcrab");
			api.RegisterCollectibleBehaviorClass("inTreeHollowTransform", typeof(BehaviorInTreeHollowTransform));
			api.RegisterBlockBehaviorClass("RightClickPickupSpawnWorm", typeof(RightClickPickupSpawnWorm));
			api.RegisterBlockBehaviorClass("RightClickPickupRaft", typeof(RightClickPickupRaft));
			api.RegisterBlockBehaviorClass("RightClickPickupFloatingDock", typeof(RightClickPickupFloatingDock));
			api.RegisterBlockBehaviorClass("RightClickPickupFireflies", typeof(RightClickPickupFireflies));
			api.RegisterBlockEntityClass("bedeadfall", typeof(BEDeadfall));
			api.RegisterBlockEntityClass("besnare", typeof(BESnare));
			api.RegisterBlockEntityClass("belimbtrotlinelure", typeof(BELimbTrotLineLure));
			api.RegisterBlockEntityClass("befishbasket", typeof(BEFishBasket));
			api.RegisterBlockEntityClass("beweirtrap", typeof(BEWeirTrap));
			api.RegisterBlockEntityClass("bewoodsupportspikes", typeof(BEWoodSupportSpikes));
			api.RegisterBlockEntityClass("befireflies", typeof(BEFireflies));
			api.RegisterBlockEntityClass("befirework", typeof(BEFirework));
			api.RegisterBlockEntityClass("befuse", typeof(BEFuse));
			api.RegisterBlockEntityClass("betreehollowgrown", typeof(BETreeHollowGrown));
			api.RegisterBlockEntityClass("betreehollowplaced", typeof(BETreeHollowPlaced));
			api.RegisterBlockEntityClass("bebombfuse", typeof(BEBombFuse));
			api.RegisterBlockEntityClass("beirrigationvessel", typeof(BEIrrigationVessel));
			api.RegisterBlockEntityClass("besmoker", typeof(BESmoker));
			api.RegisterBlockEntityClass("befurrowedland", typeof(BEFurrowedLand));
			api.RegisterBlockEntityClass("befloatingdock", typeof(BEFloatingDock));
			api.RegisterBlockClass("blockearthwormcastings", typeof(BlockEarthwormCastings));
			api.RegisterBlockClass("blockfuse", typeof(BlockFuse));
			api.RegisterBlockClass("blockstakeinwater", typeof(BlockStakeInWater));
			api.RegisterBlockClass("blockdeadfall", typeof(BlockDeadfall));
			api.RegisterBlockClass("blocksnare", typeof(BlockSnare));
			api.RegisterBlockClass("blocklimbtrotlinelure", typeof(BlockLimbTrotLineLure));
			api.RegisterBlockClass("blockfishbasket", typeof(BlockFishBasket));
			api.RegisterBlockClass("blockweirtrap", typeof(BlockWeirTrap));
			api.RegisterBlockClass("blockspiketrap", typeof(BlockSpikeTrap));
			api.RegisterBlockClass("blockwoodsupportspikes", typeof(BlockWoodSupportSpikes));
			api.RegisterBlockClass("blocknsew", typeof(BlockNSEW));
			api.RegisterBlockClass("blockmonkeybridge", typeof(BlockMonkeyBridge));
			api.RegisterBlockClass("blockraft", typeof(BlockRaft));
			api.RegisterBlockClass("blockfireflies", typeof(BlockFireflies));
			api.RegisterBlockClass("blockfirework", typeof(BlockFirework));
			api.RegisterBlockClass("blocktreehollowgrown", typeof(BlockTreeHollowGrown));
			api.RegisterBlockClass("blocktreehollowplaced", typeof(BlockTreeHollowPlaced));
			api.RegisterBlockClass("blockhandofthedead", typeof(BlockHandOfTheDead));
			api.RegisterBlockClass("blockskullofthedead", typeof(BlockSkullOfTheDead));
			api.RegisterBlockClass("blockbombfuse", typeof(BlockBombFuse));
			api.RegisterBlockClass("blockirrigationvessel", typeof(BlockIrrigationVessel));
			api.RegisterBlockClass("blocksmoker", typeof(BlockSmoker));
			api.RegisterBlockClass("blockfurrowedland", typeof(BlockFurrowedLand));
			api.RegisterBlockClass("blockfloatingdock", typeof(BlockFloatingDock));
			api.RegisterItemClass("itemcordage", typeof(ItemCordage));
			api.RegisterItemClass("itemfuse", typeof(ItemFuse));
			api.RegisterItemClass("itemwoodspikebundle", typeof(ItemWoodSpikeBundle));
			api.RegisterItemClass("itemmonkeybridge", typeof(ItemMonkeyBridge));
			api.RegisterItemClass("itemnightcrawler", typeof(ItemNightcrawler));
			api.RegisterItemClass("itemsnake", typeof(ItemSnake));
			api.RegisterItemClass("itemcrab", typeof(ItemCrab));
			api.RegisterItemClass("itemfisheggs", typeof(ItemFishEggs));
			api.RegisterItemClass("itemlivingdead", typeof(ItemLivingDead));
			api.RegisterItemClass("itemwillowisp", typeof(ItemWillowisp));
			api.RegisterItemClass("itembioluminescent", typeof(ItemBioluminescent));
			api.RegisterItemClass("itemspearfromattributes", typeof(ItemSpearFromAttributes));
			api.RegisterItemClass("ItemHoeExtended", typeof(ItemHoeExtended));
			api.RegisterItemClass("itemfishingspear", typeof(ItemFishingSpear));
			api.RegisterCollectibleBehaviorClass("fishDepletionProbe", typeof(fishDepletionProbe));
		}

		// Token: 0x06000301 RID: 769 RVA: 0x000238A0 File Offset: 0x00021AA0
		public void UpdateSpawnRate(ICoreAPI api, string entityCode, double multiplier)
		{
			if (multiplier == 1.0)
			{
				return;
			}
			if (multiplier >= 10.0)
			{
				multiplier = 10.0;
			}
			int i = api.World.EntityTypes.Count - 1;
			while (i >= 0)
			{
				if (api.World.EntityTypes[i].Code.Path == entityCode)
				{
					EntityServerProperties server = api.World.EntityTypes[i].Server;
					bool flag;
					if (server == null)
					{
						flag = (null != null);
					}
					else
					{
						SpawnConditions spawnConditions = server.SpawnConditions;
						flag = (((spawnConditions != null) ? spawnConditions.Runtime : null) != null);
					}
					if (flag)
					{
						api.World.EntityTypes[i].Server.SpawnConditions.Runtime.Chance *= multiplier;
					}
					EntityServerProperties server2 = api.World.EntityTypes[i].Server;
					bool flag2;
					if (server2 == null)
					{
						flag2 = (null != null);
					}
					else
					{
						SpawnConditions spawnConditions2 = server2.SpawnConditions;
						flag2 = (((spawnConditions2 != null) ? spawnConditions2.Worldgen : null) != null);
					}
					if (flag2)
					{
						api.World.EntityTypes[i].Server.SpawnConditions.Worldgen.TriesPerChunk.avg *= (float)multiplier;
						return;
					}
					break;
				}
				else
				{
					i--;
				}
			}
		}

		// Token: 0x06000302 RID: 770 RVA: 0x000239DC File Offset: 0x00021BDC
		public void UpdateSpawnRates(ICoreAPI api)
		{
			this.UpdateSpawnRate(api, "bioluminescent-globe", ModConfig.Loaded.SpawnMultiplierBioluminescentGlobe);
			this.UpdateSpawnRate(api, "bioluminescent-jelly", ModConfig.Loaded.SpawnMultiplierBioluminescentJelly);
			this.UpdateSpawnRate(api, "bioluminescent-orangejelly", ModConfig.Loaded.SpawnMultiplierBioluminescentOrangeJelly);
			this.UpdateSpawnRate(api, "bioluminescent-worm", ModConfig.Loaded.SpawnMultiplierBioluminescentWorm);
			this.UpdateSpawnRate(api, "bairdicrab", ModConfig.Loaded.SpawnMultiplierCrabBairdi);
			this.UpdateSpawnRate(api, "landcrab", ModConfig.Loaded.SpawnMultiplierCrabLand);
			this.UpdateSpawnRate(api, "livingdead-normal", ModConfig.Loaded.SpawnMultiplierLivingDead);
			this.UpdateSpawnRate(api, "blackrat", ModConfig.Loaded.SpawnMultiplierSnakeBlackRat);
			this.UpdateSpawnRate(api, "chainviper", ModConfig.Loaded.SpawnMultiplierSnakeChainViper);
			this.UpdateSpawnRate(api, "coachwhip", ModConfig.Loaded.SpawnMultiplierSnakeCoachWhip);
			this.UpdateSpawnRate(api, "pitviper", ModConfig.Loaded.SpawnMultiplierSnakePitViper);
			this.UpdateSpawnRate(api, "willowisp-green", ModConfig.Loaded.SpawnMultiplierWillowispGreen);
			this.UpdateSpawnRate(api, "willowisp-white", ModConfig.Loaded.SpawnMultiplierWillowispWhite);
			this.UpdateSpawnRate(api, "willowisp-yellow", ModConfig.Loaded.SpawnMultiplierWillowispYellow);
		}

		// Token: 0x06000303 RID: 771 RVA: 0x00023B20 File Offset: 0x00021D20
		public override void StartServerSide(ICoreServerAPI api)
		{
			this.prevChunksLoaded = false;
			base.StartServerSide(this.sapi);
			this.sapi = api;
			api.Event.SaveGameLoaded += new Action(this.OnSaveGameLoading);
			api.Event.GameWorldSave += new Action(this.OnSaveGameSaving);
			api.Event.RegisterGameTickListener(new Action<float>(this.RepleteFishStocks), 60000 * ModConfig.Loaded.FishChunkRepletionMinutes, 0);
			this.sapi.Event.PlayerJoin += new PlayerDelegate(this.OnPlayerJoin);
			this.serverChannel = this.sapi.Network.RegisterChannel("primitivesurvival").RegisterMessageType<SyncClientPacket>().SetMessageHandler<SyncClientPacket>(delegate(IServerPlayer player, SyncClientPacket packet)
			{
			});
		}

		// Token: 0x06000304 RID: 772 RVA: 0x00023C00 File Offset: 0x00021E00
		private void OnPlayerJoin(IServerPlayer player)
		{
			this.serverChannel.SendPacket<SyncClientPacket>(new SyncClientPacket
			{
				DeadfallBaitStolenPercent = ModConfig.Loaded.DeadfallBaitStolenPercent,
				DeadfallMaxAnimalHeight = ModConfig.Loaded.DeadfallMaxAnimalHeight,
				DeadfallMaxDamageSet = ModConfig.Loaded.DeadfallMaxDamageSet,
				DeadfallMaxDamageBaited = ModConfig.Loaded.DeadfallMaxDamageBaited,
				DeadfallTrippedPercent = ModConfig.Loaded.DeadfallTrippedPercent,
				FallDamageMultiplierWoodSpikes = ModConfig.Loaded.FallDamageMultiplierWoodSpikes,
				FallDamageMultiplierMetalSpikes = ModConfig.Loaded.FallDamageMultiplierMetalSpikes,
				FishBasketCatchPercent = ModConfig.Loaded.FishBasketCatchPercent,
				FishBasketBaitedCatchPercent = ModConfig.Loaded.FishBasketBaitedCatchPercent,
				FishBasketBaitStolenPercent = ModConfig.Loaded.FishBasketBaitStolenPercent,
				FishBasketEscapePercent = ModConfig.Loaded.FishBasketEscapePercent,
				FishBasketUpdateMinutes = ModConfig.Loaded.FishBasketUpdateMinutes,
				FishBasketRotRemovedPercent = ModConfig.Loaded.FishBasketRotRemovedPercent,
				FishChanceOfEggsPercent = ModConfig.Loaded.FishChanceOfEggsPercent,
				FishChunkDepletionRate = ModConfig.Loaded.FishChunkDepletionRate,
				FishChunkRepletionRate = ModConfig.Loaded.FishChunkRepletionRate,
				FishChunkRepletionMinutes = ModConfig.Loaded.FishChunkRepletionMinutes,
				FishEggsChunkRepletionRate = ModConfig.Loaded.FishEggsChunkRepletionRate,
				FishChunkMaxDepletionPercent = ModConfig.Loaded.FishChunkMaxDepletionPercent,
				FurrowedLandUpdateFrequency = ModConfig.Loaded.FurrowedLandUpdateFrequency,
				FurrowedLandBlockageChancePercent = ModConfig.Loaded.FurrowedLandBlockageChancePercent,
				FurrowedLandMinMoistureClose = ModConfig.Loaded.FurrowedLandMinMoistureClose,
				FurrowedLandMinMoistureFar = ModConfig.Loaded.FurrowedLandMinMoistureFar,
				LimbTrotlineCatchPercent = ModConfig.Loaded.LimbTrotlineCatchPercent,
				LimbTrotlineBaitedCatchPercent = ModConfig.Loaded.LimbTrotlineBaitedCatchPercent,
				LimbTrotlineLuredCatchPercent = ModConfig.Loaded.LimbTrotlineLuredCatchPercent,
				LimbTrotlineBaitedLuredCatchPercent = ModConfig.Loaded.LimbTrotlineBaitedLuredCatchPercent,
				LimbTrotlineBaitStolenPercent = ModConfig.Loaded.LimbTrotlineBaitStolenPercent,
				LimbTrotlineUpdateMinutes = ModConfig.Loaded.LimbTrotlineUpdateMinutes,
				LimbTrotlineRotRemovedPercent = ModConfig.Loaded.LimbTrotlineRotRemovedPercent,
				MonkeyBridgeMaxLength = ModConfig.Loaded.MonkeyBridgeMaxLength,
				RaftWaterSpeedModifier = ModConfig.Loaded.RaftWaterSpeedModifier,
				RaftFlotationModifier = ModConfig.Loaded.RaftFlotationModifier,
				SnareBaitStolenPercent = ModConfig.Loaded.SnareBaitStolenPercent,
				SnareMaxAnimalHeight = ModConfig.Loaded.SnareMaxAnimalHeight,
				SnareMaxDamageSet = ModConfig.Loaded.SnareMaxDamageSet,
				SnareMaxDamageBaited = ModConfig.Loaded.SnareMaxDamageBaited,
				SnareTrippedPercent = ModConfig.Loaded.SnareTrippedPercent,
				SpawnMultiplierBioluminescentGlobe = ModConfig.Loaded.SpawnMultiplierBioluminescentGlobe,
				SpawnMultiplierBioluminescentJelly = ModConfig.Loaded.SpawnMultiplierBioluminescentJelly,
				SpawnMultiplierBioluminescentOrangeJelly = ModConfig.Loaded.SpawnMultiplierBioluminescentOrangeJelly,
				SpawnMultiplierBioluminescentWorm = ModConfig.Loaded.SpawnMultiplierBioluminescentWorm,
				SpawnMultiplierCrabBairdi = ModConfig.Loaded.SpawnMultiplierCrabBairdi,
				SpawnMultiplierCrabLand = ModConfig.Loaded.SpawnMultiplierCrabLand,
				SpawnMultiplierLivingDead = ModConfig.Loaded.SpawnMultiplierLivingDead,
				SpawnMultiplierSnakeBlackRat = ModConfig.Loaded.SpawnMultiplierSnakeBlackRat,
				SpawnMultiplierSnakeChainViper = ModConfig.Loaded.SpawnMultiplierSnakeChainViper,
				SpawnMultiplierSnakeCoachWhip = ModConfig.Loaded.SpawnMultiplierSnakeCoachWhip,
				SpawnMultiplierSnakePitViper = ModConfig.Loaded.SpawnMultiplierSnakePitViper,
				SpawnMultiplierWillowispGreen = ModConfig.Loaded.SpawnMultiplierWillowispGreen,
				SpawnMultiplierWillowispWhite = ModConfig.Loaded.SpawnMultiplierWillowispWhite,
				SpawnMultiplierWillowispYellow = ModConfig.Loaded.SpawnMultiplierWillowispYellow,
				TreeHollowsMaxItems = ModConfig.Loaded.TreeHollowsMaxItems,
				TreeHollowsEnableDeveloperTools = ModConfig.Loaded.TreeHollowsEnableDeveloperTools,
				TreeHollowsMaxPerChunk = ModConfig.Loaded.TreeHollowsMaxPerChunk,
				TreeHollowsSpawnProbability = ModConfig.Loaded.TreeHollowsSpawnProbability,
				TreeHollowsUpdateMinutes = ModConfig.Loaded.TreeHollowsUpdateMinutes,
				WeirTrapCatchPercent = ModConfig.Loaded.WeirTrapCatchPercent,
				WeirTrapEscapePercent = ModConfig.Loaded.WeirTrapEscapePercent,
				WeirTrapUpdateMinutes = ModConfig.Loaded.WeirTrapUpdateMinutes,
				WeirTrapRotRemovedPercent = ModConfig.Loaded.WeirTrapRotRemovedPercent,
				WormFoundPercentRock = ModConfig.Loaded.WormFoundPercentRock,
				WormFoundPercentStickFlint = ModConfig.Loaded.WormFoundPercentStickFlint
			}, new IServerPlayer[]
			{
				player
			});
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00024017 File Offset: 0x00022217
		public override void AssetsFinalize(ICoreAPI api)
		{
			base.AssetsFinalize(api);
			this.UpdateSpawnRates(api);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00024027 File Offset: 0x00022227
		public override void Start(ICoreAPI api)
		{
			base.Start(api);
			this.RegisterClasses(api);
		}

		// Token: 0x06000307 RID: 775 RVA: 0x00024038 File Offset: 0x00022238
		private void OnSaveGameLoading()
		{
			PrimitiveSurvivalSystem.fishingChunks = new Dictionary<IServerChunk, int>();
			byte[] data = this.sapi.WorldManager.SaveGame.GetData("chunklist");
			PrimitiveSurvivalSystem.chunkList = ((data == null) ? new List<string>() : SerializerUtil.Deserialize<List<string>>(data));
			PrimitiveSurvivalSystem.chunkSet = new HashSet<string>(PrimitiveSurvivalSystem.chunkList);
		}

		// Token: 0x06000308 RID: 776 RVA: 0x00024090 File Offset: 0x00022290
		private void OnSaveGameSaving()
		{
			int chunkcount = 0;
			foreach (KeyValuePair<IServerChunk, int> chunk in PrimitiveSurvivalSystem.fishingChunks)
			{
				if (chunk.Value != 0)
				{
					chunk.Key.SetServerModdata("primitivesurvival5", SerializerUtil.Serialize<int>(chunk.Value));
					chunkcount++;
				}
			}
			this.sapi.WorldManager.SaveGame.StoreData("chunklist", SerializerUtil.Serialize<List<string>>(PrimitiveSurvivalSystem.chunkList));
		}

		// Token: 0x06000309 RID: 777 RVA: 0x0002412C File Offset: 0x0002232C
		private static void AddChunkToDictionary(IServerChunk chunk)
		{
			byte[] data = chunk.GetServerModdata("primitivesurvival");
			int fishing = (data == null) ? 0 : SerializerUtil.Deserialize<int>(data);
			PrimitiveSurvivalSystem.fishingChunks.Add(chunk, fishing);
		}

		// Token: 0x0600030A RID: 778 RVA: 0x00024160 File Offset: 0x00022360
		public static void UpdateChunkInDictionary(ICoreServerAPI api, BlockPos pos, int rate)
		{
			IServerChunk chunk = api.WorldManager.GetChunk(pos);
			if (!PrimitiveSurvivalSystem.fishingChunks.ContainsKey(chunk))
			{
				PrimitiveSurvivalSystem.AddChunkToDictionary(chunk);
			}
			string chunkindex = string.Concat(new string[]
			{
				pos.X.ToString(),
				",",
				pos.Y.ToString(),
				",",
				pos.Z.ToString()
			});
			if (PrimitiveSurvivalSystem.chunkSet == null)
			{
				PrimitiveSurvivalSystem.chunkSet = ((PrimitiveSurvivalSystem.chunkList != null) ? new HashSet<string>(PrimitiveSurvivalSystem.chunkList) : new HashSet<string>());
			}
			if (!PrimitiveSurvivalSystem.chunkSet.Contains(chunkindex))
			{
				PrimitiveSurvivalSystem.chunkList.Add(chunkindex);
				PrimitiveSurvivalSystem.chunkSet.Add(chunkindex);
			}
			int newvalue = rate;
			byte[] getdata = api.WorldManager.GetChunk(pos).GetServerModdata("primitivesurvival5");
			if (getdata != null)
			{
				if (getdata.Length != 0)
				{
					newvalue = (int)getdata[getdata.Length - 1] + rate;
				}
			}
			else
			{
				newvalue = new Random().Next(0, 60);
			}
			if (newvalue < 0)
			{
				newvalue = 0;
			}
			if (newvalue > ModConfig.Loaded.FishChunkMaxDepletionPercent)
			{
				newvalue = ModConfig.Loaded.FishChunkMaxDepletionPercent;
			}
			chunk.SetServerModdata("primitivesurvival5", SerializerUtil.Serialize<int>(newvalue));
			chunk.MarkModified();
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0002428C File Offset: 0x0002248C
		private void RepleteFishStocks(float par)
		{
			HashSet<int> uniqueNumbers = new HashSet<int>();
			if (!this.prevChunksLoaded)
			{
				foreach (string text in PrimitiveSurvivalSystem.chunkList)
				{
					string[] coords = text.Split(',', 0);
					BlockPos pos = new BlockPos(StringUtil.ToInt(coords[0], 0), StringUtil.ToInt(coords[1], 0), StringUtil.ToInt(coords[2], 0), 0);
					if (!(pos == null))
					{
						IServerChunk getchunk = this.sapi.WorldManager.GetChunk(pos);
						if (getchunk != null && uniqueNumbers.Add(getchunk.GetHashCode()) && getchunk.GetServerModdata("primitivesurvival5") != null)
						{
							PrimitiveSurvivalSystem.UpdateChunkInDictionary(this.sapi, pos, -ModConfig.Loaded.FishChunkRepletionRate);
						}
					}
				}
			}
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0002436C File Offset: 0x0002256C
		public static int FishDepletedPercent(ICoreServerAPI api, BlockPos pos)
		{
			int rate = 0;
			byte[] getdata = api.WorldManager.GetChunk(pos).GetServerModdata("primitivesurvival5");
			if (getdata == null)
			{
				PrimitiveSurvivalSystem.UpdateChunkInDictionary(api, pos, rate);
				getdata = api.WorldManager.GetChunk(pos).GetServerModdata("primitivesurvival5");
			}
			if (getdata.Length != 0)
			{
				rate = (int)getdata[getdata.Length - 1];
			}
			return rate;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x000243C1 File Offset: 0x000225C1
		public override void Dispose()
		{
			Harmony harmony = this.harmony;
			if (harmony != null)
			{
				harmony.UnpatchAll("com.spearandfang.primitivesurvival");
			}
			base.Dispose();
		}

		// Token: 0x04000138 RID: 312
		private readonly string thisModID = "primitivesurvival5";

		// Token: 0x04000139 RID: 313
		private static Dictionary<IServerChunk, int> fishingChunks;

		// Token: 0x0400013A RID: 314
		public static List<string> chunkList;

		// Token: 0x0400013B RID: 315
		private static HashSet<string> chunkSet;

		// Token: 0x0400013C RID: 316
		private bool prevChunksLoaded;

		// Token: 0x0400013D RID: 317
		private ICoreServerAPI sapi;

		// Token: 0x0400013E RID: 318
		private ICoreClientAPI capi;

		// Token: 0x0400013F RID: 319
		private VenomOverlayRenderer vrenderer;

		// Token: 0x04000140 RID: 320
		private readonly Harmony harmony = new Harmony("com.spearandfang.primitivesurvival");

		// Token: 0x04000141 RID: 321
		private IServerNetworkChannel serverChannel;

		// Token: 0x02000076 RID: 118
		public class PS_CanSupportPumpkin_Patch
		{
			// Token: 0x06000405 RID: 1029 RVA: 0x00026008 File Offset: 0x00024208
			[HarmonyPrefix]
			public static bool PSCanSupportPumpkinPrefix(ref bool __result, ICoreAPI api, BlockPos pos)
			{
				string bclass = api.World.BlockAccessor.GetBlock(pos, 1).Class;
				if (bclass == "blockfurrowedland" || bclass == "blockirrigationvessel")
				{
					__result = true;
					return false;
				}
				return true;
			}
		}

		// Token: 0x02000077 RID: 119
		public class PS_BlockGetPlacedBlockInfo_Patch
		{
			// Token: 0x06000407 RID: 1031 RVA: 0x00026058 File Offset: 0x00024258
			[HarmonyPostfix]
			public static void PSBlockGetPlacedBlockInfoPostfix(ref string __result, IPlayer forPlayer)
			{
				EntityPlayer entity = forPlayer.Entity;
				string text;
				if (entity == null)
				{
					text = null;
				}
				else
				{
					BlockSelection blockSelection = entity.BlockSelection;
					if (blockSelection == null)
					{
						text = null;
					}
					else
					{
						Block block = blockSelection.Block;
						if (block == null)
						{
							text = null;
						}
						else
						{
							AssetLocation code = block.Code;
							text = ((code != null) ? code.Domain : null);
						}
					}
				}
				string domain = text;
				if (domain != null && domain == "primitivesurvival")
				{
					__result += "\n\n<font color=\"#D8EAA3\"><i>Primitive Survival</i></font>\n\n";
				}
			}
		}

		// Token: 0x02000078 RID: 120
		public class PS_CollectibleGetHeldItemInfo_Patch
		{
			// Token: 0x06000409 RID: 1033 RVA: 0x000260C4 File Offset: 0x000242C4
			[HarmonyPostfix]
			public static void PSCollectibleGetHeldItemInfoPostfix(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world)
			{
				ItemStack itemstack = inSlot.Itemstack;
				string text;
				if (itemstack == null)
				{
					text = null;
				}
				else
				{
					CollectibleObject collectible = itemstack.Collectible;
					if (collectible == null)
					{
						text = null;
					}
					else
					{
						AssetLocation code = collectible.Code;
						text = ((code != null) ? code.Domain : null);
					}
				}
				string domain = text;
				if (domain != null && domain == "primitivesurvival")
				{
					dsc.AppendLine("\n<font color=\"#D8EAA3\"><i>" + Lang.GetMatching("game:tabname-primitive", Array.Empty<object>()) + "</i></font>");
				}
			}
		}

		// Token: 0x02000079 RID: 121
		public class PS_WormSpawn_Patch
		{
			// Token: 0x0600040B RID: 1035 RVA: 0x0002613C File Offset: 0x0002433C
			[HarmonyPrefix]
			public static bool Prefix(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, int amount)
			{
				if (byEntity.Api.Side != 1)
				{
					return true;
				}
				if (byEntity.World.Rand.NextDouble() > 0.5)
				{
					return true;
				}
				AssetLocation nightcrawlerLoc = new AssetLocation("primitivesurvival:nightcrawler");
				EntityProperties type = byEntity.World.GetEntityType(nightcrawlerLoc);
				if (type == null)
				{
					return true;
				}
				for (int i = 0; i < amount; i++)
				{
					Entity nightcrawler = byEntity.World.ClassRegistry.CreateEntity(type);
					nightcrawler.Pos.X = (double)((float)blockSel.Position.X + (float)byEntity.World.Rand.NextDouble());
					nightcrawler.Pos.Y = (double)((float)blockSel.Position.Y + 1.1f);
					nightcrawler.Pos.Z = (double)((float)blockSel.Position.Z + (float)byEntity.World.Rand.NextDouble());
					nightcrawler.Pos.Yaw = (float)byEntity.World.Rand.NextDouble() * 2f * 3.1415927f;
					byEntity.World.SpawnEntity(nightcrawler);
				}
				return false;
			}
		}
	}
}
