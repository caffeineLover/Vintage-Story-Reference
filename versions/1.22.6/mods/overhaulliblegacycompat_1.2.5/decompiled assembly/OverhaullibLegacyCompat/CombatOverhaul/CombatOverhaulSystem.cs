using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Cairo;
using CombatOverhaul.Animations;
using CombatOverhaul.Armor;
using CombatOverhaul.Colliders;
using CombatOverhaul.DamageSystems;
using CombatOverhaul.Implementations;
using CombatOverhaul.Inputs;
using CombatOverhaul.Integration;
using CombatOverhaul.Integration.Transpilers;
using CombatOverhaul.MeleeSystems;
using CombatOverhaul.RangedSystems;
using CombatOverhaul.RangedSystems.Aiming;
using CombatOverhaul.Utils;
using CombatOverhaul.Vanity;
using Newtonsoft.Json.Linq;
using OpenTK.Mathematics;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;
using Vintagestory.Server;

namespace CombatOverhaul
{
	// Token: 0x02000016 RID: 22
	[NullableContext(2)]
	[Nullable(0)]
	public class CombatOverhaulSystem : ModSystem
	{
		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060000CB RID: 203 RVA: 0x00004850 File Offset: 0x00002A50
		// (remove) Token: 0x060000CC RID: 204 RVA: 0x00004888 File Offset: 0x00002A88
		public event Action OnDispose;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060000CD RID: 205 RVA: 0x000048C0 File Offset: 0x00002AC0
		// (remove) Token: 0x060000CE RID: 206 RVA: 0x000048F8 File Offset: 0x00002AF8
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public event Action<Settings> SettingsLoaded;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060000CF RID: 207 RVA: 0x00004930 File Offset: 0x00002B30
		// (remove) Token: 0x060000D0 RID: 208 RVA: 0x00004968 File Offset: 0x00002B68
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public event Action<Settings> SettingsChanged;

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000D1 RID: 209 RVA: 0x0000499D File Offset: 0x00002B9D
		// (set) Token: 0x060000D2 RID: 210 RVA: 0x000049A5 File Offset: 0x00002BA5
		[Nullable(1)]
		public Settings Settings { [NullableContext(1)] get; [NullableContext(1)] set; } = new Settings();

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000D3 RID: 211 RVA: 0x000049AE File Offset: 0x00002BAE
		// (set) Token: 0x060000D4 RID: 212 RVA: 0x000049B6 File Offset: 0x00002BB6
		public bool Disposed { get; private set; }

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060000D5 RID: 213 RVA: 0x000049C0 File Offset: 0x00002BC0
		// (remove) Token: 0x060000D6 RID: 214 RVA: 0x000049F4 File Offset: 0x00002BF4
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public static event Action<ICoreAPI> OnSettingsChange;

		// Token: 0x060000D7 RID: 215 RVA: 0x00004A28 File Offset: 0x00002C28
		[NullableContext(1)]
		public override void StartPre(ICoreAPI api)
		{
			ServerCoreAPI serverCoreAPI = api as ServerCoreAPI;
			if (serverCoreAPI != null)
			{
				serverCoreAPI.ClassRegistryNative.RegisterInventoryClass("character", typeof(ArmorInventory));
			}
			ServerCoreAPI serverCoreAPI2 = api as ServerCoreAPI;
			if (serverCoreAPI2 != null)
			{
				serverCoreAPI2.ClassRegistryNative.RegisterInventoryClass("backpack", typeof(InventoryPlayerBackPacksCombatOverhaul));
			}
			ClientCoreAPI clientCoreAPI = api as ClientCoreAPI;
			if (clientCoreAPI != null)
			{
				clientCoreAPI.ClassRegistryNative.RegisterInventoryClass("character", typeof(ArmorInventory));
			}
			ClientCoreAPI clientCoreAPI2 = api as ClientCoreAPI;
			if (clientCoreAPI2 != null)
			{
				clientCoreAPI2.ClassRegistryNative.RegisterInventoryClass("backpack", typeof(InventoryPlayerBackPacksCombatOverhaul));
			}
			api.RegisterBlockClass("CombatOverhaul:GenericDisplayBlock", typeof(GenericDisplayBlock));
			api.RegisterBlockClass("CombatOverhaul:HandbookAntlerMount", typeof(HandbookAntlerMount));
			api.RegisterBlockEntityClass("CombatOverhaul:GenericDisplayBlockEntity", typeof(GenericDisplayBlockEntity));
			CombatOverhaulSystem.RegisterClassMappings(api);
		}

		// Token: 0x060000D8 RID: 216 RVA: 0x00004B10 File Offset: 0x00002D10
		[NullableContext(1)]
		public override void Start(ICoreAPI api)
		{
			GrindingWheelCompat.SetApi(api);
			SplitMaterialWeaponUtil.SetApi(api);
			HarmonyPatchesManager.Patch(api);
			if (api.Side == 2)
			{
				HarmonyPatches.ClientSettings = this.Settings;
				AnimationPatches.ClientSettings = this.Settings;
			}
			else
			{
				HarmonyPatches.ServerSettings = this.Settings;
				AnimationPatches.ServerSettings = this.Settings;
			}
			AiTaskRegistry.Register<AiTaskCOTurretMode>("CombatOverhaul:TurretMode");
			AiTaskRegistry.Register<StaggerAiTask>("CombatOverhaul:Stagger");
			this.StartConfigLibSubscription(api);
			this.ApplyConfigFileSettings(api);
			CombatOverhaulSystem.ApplyRuntimeSettings(this.Settings);
			HarmonyPatchesManager.ConfigureDiagnostics(api, this.Settings);
			if (api.ModLoader.IsModEnabled("combatoverhaul") || api.ModLoader.IsModEnabled("combatoverhaulfork"))
			{
				EidolonSlam_KnockbackMultiplierPatch.KnockbackMultiplier = 0.2f;
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00004BD0 File Offset: 0x00002DD0
		[NullableContext(1)]
		private static void RegisterClassMappings(ICoreAPI api)
		{
			api.RegisterEntityBehaviorClass("CombatOverhaul:FirstPersonAnimations", typeof(FirstPersonAnimationsBehavior));
			api.RegisterEntityBehaviorClass("CombatOverhaul:ThirdPersonAnimations", typeof(ThirdPersonAnimationsBehavior));
			api.RegisterEntityBehaviorClass("CombatOverhaul:EntityColliders", typeof(CollidersEntityBehavior));
			api.RegisterEntityBehaviorClass("CombatOverhaul:EntityDamageModel", typeof(EntityDamageModelBehavior));
			api.RegisterEntityBehaviorClass("CombatOverhaul:PlayerDamageModel", typeof(PlayerDamageModelBehavior));
			api.RegisterEntityBehaviorClass("CombatOverhaul:ActionsManager", typeof(ActionsManagerPlayerBehavior));
			api.RegisterEntityBehaviorClass("CombatOverhaul:AimingAccuracy", typeof(AimingAccuracyBehavior));
			api.RegisterEntityBehaviorClass("CombatOverhaul:WearableStats", typeof(WearableStatsBehavior));
			api.RegisterEntityBehaviorClass("CombatOverhaul:InInventory", typeof(InInventoryPlayerBehavior));
			api.RegisterEntityBehaviorClass("CombatOverhaul:ProjectilePhysics", typeof(ProjectilePhysicsBehavior));
			api.RegisterEntityBehaviorClass("CombatOverhaul:Stagger", typeof(StaggerBehavior));
			api.RegisterEntityBehaviorClass("CombatOverhaul:PositionBeforeFalling", typeof(PositionBeforeFallingBehavior));
			api.RegisterEntityBehaviorClass("CombatOverhaul:ArmorStandInventory", typeof(EntityBehaviorCOArmorStandInventory));
			api.RegisterCollectibleBehaviorClass("CombatOverhaul:Animatable", typeof(Animatable));
			api.RegisterCollectibleBehaviorClass("CombatOverhaul:AnimatableAttachable", typeof(AnimatableAttachable));
			api.RegisterCollectibleBehaviorClass("AnimationsLib:Animatable", typeof(Animatable));
			api.RegisterCollectibleBehaviorClass("AnimationsLib:AnimatableAttachable", typeof(AnimatableAttachable));
			api.RegisterCollectibleBehaviorClass("CombatOverhaul:Projectile", typeof(ProjectileBehavior));
			api.RegisterCollectibleBehaviorClass("CombatOverhaul:Armor", typeof(ArmorBehavior));
			api.RegisterCollectibleBehaviorClass("CombatOverhaul:WearableArmor", typeof(WearableArmorBehavior));
			api.RegisterCollectibleBehaviorClass("CombatOverhaul:WearableWithStats", typeof(WearableWithStatsBehavior));
			api.RegisterCollectibleBehaviorClass("CombatOverhaul:GearEquipableBag", typeof(GearEquipableBag));
			api.RegisterCollectibleBehaviorClass("CombatOverhaul:ToolBag", typeof(ToolBag));
			api.RegisterCollectibleBehaviorClass("CombatOverhaul:TextureFromAttributes", typeof(TextureFromAttributes));
			api.RegisterCollectibleBehaviorClass("CombatOverhaul:TexturesFromAttributes", typeof(TexturesFromAttributes));
			api.RegisterCollectibleBehaviorClass("CombatOverhaul:AdditionalSlots", typeof(AdditionalSlotsBehavior));
			api.RegisterCollectibleBehaviorClass("CombatOverhaul:GoesIntoSlotsInfo", typeof(GoesIntoSlotsInfo));
			api.RegisterCollectibleBehaviorClass("CombatOverhaul:MeleeWeaponBehavior", typeof(MeleeWeaponBehavior));
			api.RegisterItemClass("CombatOverhaul:Bow", typeof(BowItem));
			api.RegisterItemClass("CombatOverhaul:Sling", typeof(SlingItem));
			api.RegisterItemClass("CombatOverhaul:MeleeWeapon", typeof(MeleeWeapon));
			api.RegisterItemClass("CombatOverhaul:StanceBasedMeleeWeapon", typeof(StanceBasedMeleeWeapon));
			api.RegisterItemClass("CombatOverhaul:TextureAttributedItem", typeof(TextureAttributedItem));
			api.RegisterItemClass("CombatOverhaul:VanillaShield", typeof(VanillaShield));
			api.RegisterItemClass("CombatOverhaul:WearableArmor", typeof(ItemWearableArmor));
			api.RegisterItemClass("CombatOverhaul:WearableFueledLightSource", typeof(WearableFueledLightSource));
			api.RegisterEntity("CombatOverhaul:Projectile", typeof(ProjectileEntity));
			api.RegisterEntity("CombatOverhaul:ArmorStand", typeof(EntityCOArmorStand));
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00004EFC File Offset: 0x000030FC
		[NullableContext(1)]
		public override void StartServerSide(ICoreServerAPI api)
		{
			this._serverApi = api;
			this.ServerProjectileSystem = new ProjectileSystemServer(api);
			this.ServerRangedWeaponSystem = new RangedWeaponSystemServer(api);
			this.ServerSoundsSynchronizer = new SoundsSynchronizerServer(api);
			this.ServerMeleeSystem = new MeleeSystemServer(api);
			this.ServerImpaleSystem = new ImpaleSystemServer(api);
			this.ServerBlockSystem = new MeleeBlockSystemServer(api);
			this.ServerStatsSystem = new StatsSystemServer(api);
			this.ServerAttachmentSystem = new AttachableSystemServer(api);
			this.ServerToolBagSystem = new ToolBagSystemServer(api);
			this.ServerVanitySystem = new VanitySystemServer(api);
			this._serverToggleChannel = api.Network.RegisterChannel("combatOverhaulToggleItem").RegisterMessageType<TogglePacket>().SetMessageHandler<TogglePacket>(new NetworkClientMessageHandler<TogglePacket>(this.ToggleWearableItem));
			this._serverGameplaySettingsChannel = api.Network.RegisterChannel("CombatOverhaul:server-gameplay-settings").RegisterMessageType<ServerGameplaySettingsPacket>();
			this._playerNowPlayingSettingsHandler = new PlayerDelegate(this.SendGameplaySettings);
			api.Event.PlayerNowPlaying += this._playerNowPlayingSettingsHandler;
			if (ShieldAutoPatcher.IsCombatOverhaulEnabled(api))
			{
				ShieldAutoPatcher.Patch(api);
				this._shieldAutoPatchServerListener = api.Event.RegisterGameTickListener(new Action<float>(this.PatchShieldsOnServerTick), 50, 50);
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00005024 File Offset: 0x00003224
		[NullableContext(1)]
		public override void StartClientSide(ICoreClientAPI api)
		{
			this._clientApi = api;
			this.ClientProjectileSystem = new ProjectileSystemClient(api, api.ModLoader.GetModSystem<EntityPartitioning>(true));
			this.ActionListener = new ActionListener(api);
			this.DirectionCursorRenderer = new DirectionCursorRenderer(api, this.Settings);
			this.ReticleRenderer = new ReticleRenderer(api);
			this.DirectionController = new DirectionController(api, this.DirectionCursorRenderer, this.Settings);
			this.ClientRangedWeaponSystem = new RangedWeaponSystemClient(api);
			this.ClientSoundsSynchronizer = new SoundsSynchronizerClient(api);
			this.AimingSystem = new ClientAimingSystem(api, this.ReticleRenderer);
			this.ClientMeleeSystem = new MeleeSystemClient(api);
			this.ClientImpaleSystem = new ImpaleSystemClient(api);
			this.ClientBlockSystem = new MeleeBlockSystemClient(api);
			this.ClientStatsSystem = new StatsSystemClient(api);
			this.ClientAttachmentSystem = new AttachableSystemClient(api);
			this.ClientToolBagSystem = new ToolBagSystemClient(api);
			this.ClientToolBagSelectionSystem = new ToolBagSelectionSystemClient(api, this.ClientToolBagSystem);
			this.ClientVanitySystem = new VanitySystemClient(api);
			api.Event.RegisterRenderer(this.ReticleRenderer, 10, null);
			api.Event.RegisterRenderer(this.DirectionCursorRenderer, 10, null);
			this._clientToggleChannel = api.Network.RegisterChannel("combatOverhaulToggleItem").RegisterMessageType<TogglePacket>();
			this._clientGameplaySettingsChannel = api.Network.RegisterChannel("CombatOverhaul:server-gameplay-settings").RegisterMessageType<ServerGameplaySettingsPacket>().SetMessageHandler<ServerGameplaySettingsPacket>(new NetworkServerMessageHandler<ServerGameplaySettingsPacket>(this.HandleServerGameplaySettings));
			if (!api.IsSinglePlayer)
			{
				api.Event.EnqueueMainThreadTask(delegate()
				{
					Action<ICoreAPI> onSettingsChange = CombatOverhaulSystem.OnSettingsChange;
					if (onSettingsChange == null)
					{
						return;
					}
					onSettingsChange.Invoke(api);
				}, "game");
			}
			api.Input.RegisterHotKey("toggleWearableLight", "Toggle wearable light source", 94, 4, false, false, false);
			api.Input.SetHotKeyHandler("toggleWearableLight", (KeyCombination _) => this.ToggleWearableItem(api.World.Player, "toggleWearableLight"));
			api.Input.RegisterHotKey("toggleTpAnimations", "Toggle CO third person animations", 57, 4, false, true, false);
			api.Input.RegisterHotKey("toggleAllAnimations", "Toggle all CO animations", 56, 4, false, true, false);
			api.Input.SetHotKeyHandler("toggleAllAnimations", delegate(KeyCombination _)
			{
				this.Settings.DisableAllAnimations = !this.Settings.DisableAllAnimations;
				if (this.Settings.DisableAllAnimations)
				{
					LoggerUtil.Notify(api, this, "Animations disabled");
					api.TriggerIngameError(this, "animationsDisabled", "Overhaul lib animations are DISABLED");
				}
				else
				{
					LoggerUtil.Notify(api, this, "Animations enabled");
					api.TriggerIngameError(this, "animationsDisabled", "Overhaul lib animations are ENABLED");
				}
				return true;
			});
			api.Input.SetHotKeyHandler("toggleTpAnimations", delegate(KeyCombination _)
			{
				this.Settings.DisableThirdPersonAnimations = !this.Settings.DisableThirdPersonAnimations;
				if (this.Settings.DisableThirdPersonAnimations)
				{
					LoggerUtil.Notify(api, this, "Third person animations disabled");
					api.TriggerIngameError(this, "animationsDisabled", "Third person Overhaul lib animations are DISABLED");
				}
				else
				{
					this.Settings.DisableAllAnimations = false;
					LoggerUtil.Notify(api, this, "All animations enabled");
					api.TriggerIngameError(this, "animationsDisabled", "All Overhaul lib animations are ENABLED");
				}
				return true;
			});
			api.Event.PlayerEntitySpawn += new PlayerEventDelegate(this.EnsureOwnPlayerAnimationBehaviors);
			api.Event.LevelFinalize += new Action(this.EnsureOwnPlayerAnimationBehaviors);
			if (ShieldAutoPatcher.IsCombatOverhaulEnabled(api))
			{
				api.Event.LevelFinalize += new Action(this.PatchShieldsOnClientLevelFinalize);
				ShieldAutoPatcher.Patch(api);
			}
			this._ensureAnimationBehaviorsListener = api.Event.RegisterGameTickListener(delegate(float _)
			{
				this.EnsureOwnPlayerAnimationBehaviors();
			}, 1000, 1000);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000539C File Offset: 0x0000359C
		[NullableContext(1)]
		public override void AssetsLoaded(ICoreAPI api)
		{
			this.StartConfigLibSubscription(api);
			this.ApplyConfigFileSettings(api);
			CombatOverhaulSystem.ApplyRuntimeSettings(this.Settings);
			HarmonyPatchesManager.ConfigureDiagnostics(api, this.Settings);
			QuenchablePatchGate.DisableCustomQuenchRecipeAssetsIfDisabled(api);
			ICoreClientAPI clientApi = api as ICoreClientAPI;
			if (clientApi == null)
			{
				return;
			}
			foreach (ArmorLayers layer in Enum.GetValues<ArmorLayers>())
			{
				foreach (DamageZone zone in Enum.GetValues<DamageZone>())
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(45, 2);
					defaultInterpolatedStringHandler.AppendLiteral("combatoverhaul:textures/gui/icons/armor-");
					defaultInterpolatedStringHandler.AppendFormatted<ArmorLayers>(layer);
					defaultInterpolatedStringHandler.AppendLiteral("-");
					defaultInterpolatedStringHandler.AppendFormatted<DamageZone>(zone);
					defaultInterpolatedStringHandler.AppendLiteral(".svg");
					string iconPath = defaultInterpolatedStringHandler.ToStringAndClear();
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
					defaultInterpolatedStringHandler2..ctor(22, 2);
					defaultInterpolatedStringHandler2.AppendLiteral("combatoverhaul-armor-");
					defaultInterpolatedStringHandler2.AppendFormatted<ArmorLayers>(layer);
					defaultInterpolatedStringHandler2.AppendLiteral("-");
					defaultInterpolatedStringHandler2.AppendFormatted<DamageZone>(zone);
					string iconCode = defaultInterpolatedStringHandler2.ToStringAndClear();
					if (clientApi.Assets.Exists(new AssetLocation(iconPath)))
					{
						this.RegisterCustomIcon(clientApi, iconCode, iconPath);
					}
				}
			}
			foreach (IAsset icon in clientApi.Assets.GetManyInCategory("textures", "sloticons", null, false))
			{
				string iconPath2 = icon.Location.ToString();
				string domain = icon.Location.Domain;
				string text = ":";
				string path = icon.Location.Path;
				int i = "textures/sloticons/".Length;
				string iconCode2 = domain + text + path.Substring(i, path.Length - 4 - i).ToLowerInvariant();
				if (!iconPath2.ToLowerInvariant().EndsWith(".svg"))
				{
					LoggerUtil.Verbose(clientApi, this, "Icon should have '.svg' format, skipping. Path: " + iconPath2);
					break;
				}
				this.RegisterCustomIcon(clientApi, iconCode2, iconPath2);
			}
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000055AC File Offset: 0x000037AC
		[NullableContext(1)]
		public override void AssetsFinalize(ICoreAPI api)
		{
			QuenchablePatchGate.RemoveCustomQuenchRecipesIfDisabled(api);
			ArmorConfig armorConfig = JsonObject.FromJson(api.Assets.Get("combatoverhaul:config/armor-config.json").ToText()).AsObject<ArmorConfig>(null);
			DamageResistData.MaxAttackTier = armorConfig.MaxAttackTier;
			DamageResistData.MaxArmorTier = armorConfig.MaxArmorTier;
			DamageResistData.DamageReduction = armorConfig.DamageReduction;
			ICoreClientAPI clientApi = api as ICoreClientAPI;
			if (clientApi != null)
			{
				this.DetermineSlotsStatus(clientApi);
			}
			CombatOverhaulSystem.EnsureTongsTransformsForForgableItems(api);
			GrindingWheelCompat.EnsureWeaponBuffableBehavior(api);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00005624 File Offset: 0x00003824
		[NullableContext(1)]
		private static void EnsureTongsTransformsForForgableItems(ICoreAPI api)
		{
			int patched = 0;
			foreach (Item item in api.World.Items)
			{
				string text;
				if (item == null)
				{
					text = null;
				}
				else
				{
					AssetLocation code = item.Code;
					text = ((code != null) ? code.Domain : null);
				}
				string text2 = text;
				bool flag = text2 == "armory" || text2 == "combatoverhaul";
				if (flag)
				{
					JsonObject attributes = item.Attributes;
					bool flag2;
					if (attributes == null)
					{
						flag2 = true;
					}
					else
					{
						JsonObject jsonObject = attributes["forgable"];
						flag2 = !((jsonObject != null) ? new bool?(jsonObject.AsBool(false)) : default(bool?)).GetValueOrDefault();
					}
					if (!flag2)
					{
						JsonObject attributes2 = item.Attributes;
						JObject jobject = ((attributes2 != null) ? attributes2.Token : null) as JObject;
						JObject attrs = (((jobject != null) ? jobject.DeepClone() : null) as JObject) ?? new JObject();
						bool changed = false;
						if (attrs["onTongTransform"] == null)
						{
							attrs["onTongTransform"] = CombatOverhaulSystem.DefaultOnTongTransform.DeepClone();
							changed = true;
						}
						if (attrs["onMetalTongTransform"] == null)
						{
							attrs["onMetalTongTransform"] = CombatOverhaulSystem.DefaultOnMetalTongTransform.DeepClone();
							changed = true;
						}
						if (changed)
						{
							item.Attributes = new JsonObject(attrs);
							patched++;
						}
					}
				}
			}
			if (patched > 0)
			{
				ILogger logger = api.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(80, 1);
				defaultInterpolatedStringHandler.AppendLiteral("[OverhaullibLegacyCompat] Applied tongs transforms to ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(patched);
				defaultInterpolatedStringHandler.AppendLiteral(" forgable CO/Armory items.");
				logger.Notification(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x000057E0 File Offset: 0x000039E0
		public override void Dispose()
		{
			if (this.Disposed)
			{
				return;
			}
			HarmonyPatchesManager.Unpatch();
			GrindingWheelCompat.Dispose();
			this.StopConfigLibSubscriptionRetry();
			this.UnsubscribeFromConfigChange();
			this._configLibSubscriptionApi = null;
			if (this._serverApi != null && this._playerNowPlayingSettingsHandler != null)
			{
				this._serverApi.Event.PlayerNowPlaying -= this._playerNowPlayingSettingsHandler;
			}
			if (this._serverApi != null && this._shieldAutoPatchServerListener != 0L)
			{
				this._serverApi.Event.UnregisterGameTickListener(this._shieldAutoPatchServerListener);
				this._shieldAutoPatchServerListener = 0L;
			}
			this._serverApi = null;
			this._playerNowPlayingSettingsHandler = null;
			ClientThreadCleanup.DisposeRenderer(this._clientApi, this.ReticleRenderer, 10, "combat-overhaul-reticle-dispose");
			ClientThreadCleanup.DisposeRenderer(this._clientApi, this.DirectionCursorRenderer, 10, "combat-overhaul-direction-cursor-dispose");
			if (this._clientApi != null)
			{
				this._clientApi.Event.PlayerEntitySpawn -= new PlayerEventDelegate(this.EnsureOwnPlayerAnimationBehaviors);
				this._clientApi.Event.LevelFinalize -= new Action(this.EnsureOwnPlayerAnimationBehaviors);
				this._clientApi.Event.LevelFinalize -= new Action(this.PatchShieldsOnClientLevelFinalize);
				if (this._ensureAnimationBehaviorsListener != 0L)
				{
					this._clientApi.Event.UnregisterGameTickListener(this._ensureAnimationBehaviorsListener);
					this._ensureAnimationBehaviorsListener = 0L;
				}
			}
			ActionListener actionListener = this.ActionListener;
			if (actionListener != null)
			{
				actionListener.Dispose();
			}
			ClientAimingSystem aimingSystem = this.AimingSystem;
			if (aimingSystem != null)
			{
				aimingSystem.Dispose();
			}
			Action onDispose = this.OnDispose;
			if (onDispose != null)
			{
				onDispose.Invoke();
			}
			this.OnDispose = null;
			ICoreClientAPI clientApi = this._clientApi;
			if (clientApi != null)
			{
				clientApi.World.UnregisterGameTickListener(this._cacheMissesReportedListener);
			}
			this.Disposed = true;
			ImpaleSystemServer serverImpaleSystem = this.ServerImpaleSystem;
			if (serverImpaleSystem != null)
			{
				serverImpaleSystem.Dispose();
			}
			VanitySystemServer serverVanitySystem = this.ServerVanitySystem;
			if (serverVanitySystem == null)
			{
				return;
			}
			serverVanitySystem.Dispose();
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x000059A8 File Offset: 0x00003BA8
		[NullableContext(1)]
		public bool ToggleWearableItem(IPlayer player, string hotkeyCode)
		{
			EntityBehaviorPlayerInventory behavior = player.Entity.GetBehavior<EntityBehaviorPlayerInventory>();
			IInventory gearInventory = (behavior != null) ? behavior.Inventory : null;
			if (gearInventory == null)
			{
				return false;
			}
			bool toggled = false;
			foreach (ItemSlot slot in gearInventory)
			{
				ITogglableItem togglableItem2;
				if (slot == null)
				{
					togglableItem2 = null;
				}
				else
				{
					ItemStack itemstack = slot.Itemstack;
					if (itemstack == null)
					{
						togglableItem2 = null;
					}
					else
					{
						CollectibleObject collectible = itemstack.Collectible;
						togglableItem2 = ((collectible != null) ? collectible.GetCollectibleInterface<ITogglableItem>() : null);
					}
				}
				ITogglableItem togglableItem = togglableItem2;
				if (togglableItem != null && togglableItem.HotKeyCode == hotkeyCode)
				{
					togglableItem.Toggle(player, slot);
					toggled = true;
				}
			}
			if (player is IClientPlayer)
			{
				IClientNetworkChannel clientToggleChannel = this._clientToggleChannel;
				if (clientToggleChannel != null)
				{
					clientToggleChannel.SendPacket<TogglePacket>(new TogglePacket
					{
						HotKeyCode = hotkeyCode
					});
				}
			}
			return toggled;
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00005A74 File Offset: 0x00003C74
		[NullableContext(1)]
		public void ToggleWearableItem(IServerPlayer player, TogglePacket packet)
		{
			this.ToggleWearableItem(player, packet.HotKeyCode);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00005A84 File Offset: 0x00003C84
		[NullableContext(1)]
		private void SendGameplaySettings(IServerPlayer player)
		{
			IServerNetworkChannel serverGameplaySettingsChannel = this._serverGameplaySettingsChannel;
			if (serverGameplaySettingsChannel == null)
			{
				return;
			}
			serverGameplaySettingsChannel.SendPacket<ServerGameplaySettingsPacket>(ServerGameplaySettingsPacket.From(this.Settings), new IServerPlayer[]
			{
				player
			});
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00005AAB File Offset: 0x00003CAB
		private void BroadcastGameplaySettings()
		{
			IServerNetworkChannel serverGameplaySettingsChannel = this._serverGameplaySettingsChannel;
			if (serverGameplaySettingsChannel == null)
			{
				return;
			}
			serverGameplaySettingsChannel.BroadcastPacket<ServerGameplaySettingsPacket>(ServerGameplaySettingsPacket.From(this.Settings), Array.Empty<IServerPlayer>());
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00005ACD File Offset: 0x00003CCD
		[NullableContext(1)]
		private void HandleServerGameplaySettings(ServerGameplaySettingsPacket packet)
		{
			this._lastServerGameplaySettings = packet;
			packet.ApplyTo(this.Settings);
			CombatOverhaulSystem.ApplyRuntimeSettings(this.Settings);
			Action<Settings> settingsChanged = this.SettingsChanged;
			if (settingsChanged == null)
			{
				return;
			}
			settingsChanged.Invoke(this.Settings);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00005B03 File Offset: 0x00003D03
		private void ReapplyServerGameplaySettingsIfNeeded(ICoreAPI api)
		{
			if (api == null || api.Side != 2 || this._lastServerGameplaySettings == null)
			{
				return;
			}
			this._lastServerGameplaySettings.ApplyTo(this.Settings);
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000E6 RID: 230 RVA: 0x00005B33 File Offset: 0x00003D33
		// (set) Token: 0x060000E7 RID: 231 RVA: 0x00005B3B File Offset: 0x00003D3B
		public ProjectileSystemClient ClientProjectileSystem { get; private set; }

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000E8 RID: 232 RVA: 0x00005B44 File Offset: 0x00003D44
		// (set) Token: 0x060000E9 RID: 233 RVA: 0x00005B4C File Offset: 0x00003D4C
		public ProjectileSystemServer ServerProjectileSystem { get; private set; }

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00005B55 File Offset: 0x00003D55
		// (set) Token: 0x060000EB RID: 235 RVA: 0x00005B5D File Offset: 0x00003D5D
		public ActionListener ActionListener { get; private set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000EC RID: 236 RVA: 0x00005B66 File Offset: 0x00003D66
		// (set) Token: 0x060000ED RID: 237 RVA: 0x00005B6E File Offset: 0x00003D6E
		public DirectionCursorRenderer DirectionCursorRenderer { get; private set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000EE RID: 238 RVA: 0x00005B77 File Offset: 0x00003D77
		// (set) Token: 0x060000EF RID: 239 RVA: 0x00005B7F File Offset: 0x00003D7F
		public ReticleRenderer ReticleRenderer { get; private set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000F0 RID: 240 RVA: 0x00005B88 File Offset: 0x00003D88
		// (set) Token: 0x060000F1 RID: 241 RVA: 0x00005B90 File Offset: 0x00003D90
		public ClientAimingSystem AimingSystem { get; private set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000F2 RID: 242 RVA: 0x00005B99 File Offset: 0x00003D99
		// (set) Token: 0x060000F3 RID: 243 RVA: 0x00005BA1 File Offset: 0x00003DA1
		public DirectionController DirectionController { get; private set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00005BAA File Offset: 0x00003DAA
		// (set) Token: 0x060000F5 RID: 245 RVA: 0x00005BB2 File Offset: 0x00003DB2
		public RangedWeaponSystemClient ClientRangedWeaponSystem { get; private set; }

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000F6 RID: 246 RVA: 0x00005BBB File Offset: 0x00003DBB
		// (set) Token: 0x060000F7 RID: 247 RVA: 0x00005BC3 File Offset: 0x00003DC3
		public RangedWeaponSystemServer ServerRangedWeaponSystem { get; private set; }

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000F8 RID: 248 RVA: 0x00005BCC File Offset: 0x00003DCC
		// (set) Token: 0x060000F9 RID: 249 RVA: 0x00005BD4 File Offset: 0x00003DD4
		public SoundsSynchronizerClient ClientSoundsSynchronizer { get; private set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000FA RID: 250 RVA: 0x00005BDD File Offset: 0x00003DDD
		// (set) Token: 0x060000FB RID: 251 RVA: 0x00005BE5 File Offset: 0x00003DE5
		public SoundsSynchronizerServer ServerSoundsSynchronizer { get; private set; }

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000FC RID: 252 RVA: 0x00005BEE File Offset: 0x00003DEE
		// (set) Token: 0x060000FD RID: 253 RVA: 0x00005BF6 File Offset: 0x00003DF6
		public MeleeSystemClient ClientMeleeSystem { get; private set; }

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000FE RID: 254 RVA: 0x00005BFF File Offset: 0x00003DFF
		// (set) Token: 0x060000FF RID: 255 RVA: 0x00005C07 File Offset: 0x00003E07
		public MeleeSystemServer ServerMeleeSystem { get; private set; }

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000100 RID: 256 RVA: 0x00005C10 File Offset: 0x00003E10
		// (set) Token: 0x06000101 RID: 257 RVA: 0x00005C18 File Offset: 0x00003E18
		public ImpaleSystemClient ClientImpaleSystem { get; private set; }

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000102 RID: 258 RVA: 0x00005C21 File Offset: 0x00003E21
		// (set) Token: 0x06000103 RID: 259 RVA: 0x00005C29 File Offset: 0x00003E29
		public ImpaleSystemServer ServerImpaleSystem { get; private set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000104 RID: 260 RVA: 0x00005C32 File Offset: 0x00003E32
		// (set) Token: 0x06000105 RID: 261 RVA: 0x00005C3A File Offset: 0x00003E3A
		public MeleeBlockSystemClient ClientBlockSystem { get; private set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000106 RID: 262 RVA: 0x00005C43 File Offset: 0x00003E43
		// (set) Token: 0x06000107 RID: 263 RVA: 0x00005C4B File Offset: 0x00003E4B
		public MeleeBlockSystemServer ServerBlockSystem { get; private set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000108 RID: 264 RVA: 0x00005C54 File Offset: 0x00003E54
		// (set) Token: 0x06000109 RID: 265 RVA: 0x00005C5C File Offset: 0x00003E5C
		public StatsSystemClient ClientStatsSystem { get; private set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x0600010A RID: 266 RVA: 0x00005C65 File Offset: 0x00003E65
		// (set) Token: 0x0600010B RID: 267 RVA: 0x00005C6D File Offset: 0x00003E6D
		public StatsSystemServer ServerStatsSystem { get; private set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x0600010C RID: 268 RVA: 0x00005C76 File Offset: 0x00003E76
		// (set) Token: 0x0600010D RID: 269 RVA: 0x00005C7E File Offset: 0x00003E7E
		public AttachableSystemClient ClientAttachmentSystem { get; private set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x0600010E RID: 270 RVA: 0x00005C87 File Offset: 0x00003E87
		// (set) Token: 0x0600010F RID: 271 RVA: 0x00005C8F File Offset: 0x00003E8F
		public AttachableSystemServer ServerAttachmentSystem { get; private set; }

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000110 RID: 272 RVA: 0x00005C98 File Offset: 0x00003E98
		// (set) Token: 0x06000111 RID: 273 RVA: 0x00005CA0 File Offset: 0x00003EA0
		public ToolBagSystemClient ClientToolBagSystem { get; private set; }

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000112 RID: 274 RVA: 0x00005CA9 File Offset: 0x00003EA9
		// (set) Token: 0x06000113 RID: 275 RVA: 0x00005CB1 File Offset: 0x00003EB1
		public ToolBagSystemServer ServerToolBagSystem { get; private set; }

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000114 RID: 276 RVA: 0x00005CBA File Offset: 0x00003EBA
		// (set) Token: 0x06000115 RID: 277 RVA: 0x00005CC2 File Offset: 0x00003EC2
		public ToolBagSelectionSystemClient ClientToolBagSelectionSystem { get; private set; }

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000116 RID: 278 RVA: 0x00005CCB File Offset: 0x00003ECB
		// (set) Token: 0x06000117 RID: 279 RVA: 0x00005CD3 File Offset: 0x00003ED3
		public VanitySystemClient ClientVanitySystem { get; private set; }

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000118 RID: 280 RVA: 0x00005CDC File Offset: 0x00003EDC
		// (set) Token: 0x06000119 RID: 281 RVA: 0x00005CE4 File Offset: 0x00003EE4
		public VanitySystemServer ServerVanitySystem { get; private set; }

		// Token: 0x0600011A RID: 282 RVA: 0x00005CF0 File Offset: 0x00003EF0
		[NullableContext(1)]
		private void RegisterCustomIcon(ICoreClientAPI api, string key, string path)
		{
			AssetLocation location = new AssetLocation(path);
			IAsset svgAsset = CombatOverhaulSystem.TryGetLoadedSvgAsset(api, location);
			api.Gui.Icons.CustomIcons[key] = delegate(Context ctx, int x, int y, float w, float h, double[] rgba)
			{
				svgAsset = this.EnsureLoadedSvgAsset(api, location, svgAsset);
				if (svgAsset == null)
				{
					return;
				}
				int value = ColorUtil.ColorFromRgba(75, 75, 75, 255);
				if (rgba.Length == 4)
				{
					value = ColorUtil.ColorFromRgba(rgba);
				}
				if (rgba.Length >= 4 && rgba[0] == 0.0 && rgba[1] == 0.0 && rgba[2] == 0.0 && rgba[3] == 0.2)
				{
					value = ColorUtil.ColorFromRgba(75, 75, 75, 190);
				}
				Surface target = ctx.GetTarget();
				int xNew = x + (int)(w * this._iconScale.X);
				int yNew = y + (int)(h * this._iconScale.Y);
				int wNew = (int)(w * this._iconScale.W);
				int hNew = (int)(h * this._iconScale.Z);
				api.Gui.DrawSvg(svgAsset, (ImageSurface)((target is ImageSurface) ? target : null), xNew, yNew, wNew, hNew, new int?(value));
			};
		}

		// Token: 0x0600011B RID: 283 RVA: 0x00005D5C File Offset: 0x00003F5C
		[NullableContext(1)]
		[return: Nullable(2)]
		private IAsset EnsureLoadedSvgAsset(ICoreClientAPI api, AssetLocation location, [Nullable(2)] IAsset asset)
		{
			if (asset != null && asset.IsLoaded() && asset.Data != null)
			{
				return asset;
			}
			asset = CombatOverhaulSystem.TryGetLoadedSvgAsset(api, location);
			if (asset != null)
			{
				return asset;
			}
			if (this._reportedMissingSvgIcons.Add(location))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(69, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to draw custom GUI icon '");
				defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(location);
				defaultInterpolatedStringHandler.AppendLiteral("': SVG asset is missing or not loaded");
				LoggerUtil.Warn(api, this, defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return null;
		}

		// Token: 0x0600011C RID: 284 RVA: 0x00005DD4 File Offset: 0x00003FD4
		[NullableContext(1)]
		[return: Nullable(2)]
		private static IAsset TryGetLoadedSvgAsset(ICoreClientAPI api, AssetLocation location)
		{
			IAsset asset = api.Assets.TryGet(location, true);
			if (asset == null || !asset.IsLoaded() || asset.Data == null)
			{
				return null;
			}
			return asset;
		}

		// Token: 0x0600011D RID: 285 RVA: 0x00005E08 File Offset: 0x00004008
		[NullableContext(1)]
		private void StartConfigLibSubscription(ICoreAPI api)
		{
			if (this._configLibSubscribed || this._reportedConfigLibIntegrationError)
			{
				return;
			}
			if (!api.ModLoader.IsModEnabled("configlib"))
			{
				return;
			}
			this._configLibSubscriptionApi = api;
			if (this.SubscribeToConfigChange(api))
			{
				return;
			}
			if (this._reportedConfigLibIntegrationError || this._configLibSubscribeRetryListener != 0L)
			{
				return;
			}
			this._configLibSubscribeRetryListener = api.Event.RegisterGameTickListener(delegate(float _)
			{
				if (this._reportedConfigLibIntegrationError)
				{
					this.StopConfigLibSubscriptionRetry();
					return;
				}
				if (!this.SubscribeToConfigChange(api))
				{
					return;
				}
				this.StopConfigLibSubscriptionRetry();
			}, 500, 500);
		}

		// Token: 0x0600011E RID: 286 RVA: 0x00005EAA File Offset: 0x000040AA
		private void StopConfigLibSubscriptionRetry()
		{
			if (this._configLibSubscribeRetryListener == 0L)
			{
				return;
			}
			ICoreAPI configLibSubscriptionApi = this._configLibSubscriptionApi;
			if (configLibSubscriptionApi != null)
			{
				configLibSubscriptionApi.Event.UnregisterGameTickListener(this._configLibSubscribeRetryListener);
			}
			this._configLibSubscribeRetryListener = 0L;
		}

		// Token: 0x0600011F RID: 287 RVA: 0x00005EDC File Offset: 0x000040DC
		[NullableContext(1)]
		private bool SubscribeToConfigChange(ICoreAPI api)
		{
			if (this._configLibSubscribed)
			{
				return true;
			}
			bool result;
			try
			{
				Type configLibSystemType = CombatOverhaulSystem.FindLoadedType("ConfigLib.ConfigLibModSystem");
				if (configLibSystemType == null)
				{
					result = false;
				}
				else
				{
					object system = CombatOverhaulSystem.GetModSystem(api, configLibSystemType);
					if (system == null)
					{
						result = false;
					}
					else
					{
						this._configLibSystem = system;
						this._configGetConfigMethod = configLibSystemType.GetMethod("GetConfig", 20, null, new Type[]
						{
							typeof(string)
						}, null);
						this._configSettingChangedEvent = configLibSystemType.GetEvent("SettingChanged", 20);
						this._configLoadedEvent = configLibSystemType.GetEvent("ConfigsLoaded", 20);
						EventInfo configSettingChangedEvent = this._configSettingChangedEvent;
						if (((configSettingChangedEvent != null) ? configSettingChangedEvent.EventHandlerType : null) != null)
						{
							this._configSettingChangedHandler = this.CreateConfigSettingChangedHandler(this._configSettingChangedEvent.EventHandlerType);
							if (this._configSettingChangedHandler != null)
							{
								this._configSettingChangedEvent.AddEventHandler(system, this._configSettingChangedHandler);
							}
						}
						EventInfo configLoadedEvent = this._configLoadedEvent;
						if (((configLoadedEvent != null) ? configLoadedEvent.EventHandlerType : null) != null)
						{
							this._configLoadedHandler = this.CreateConfigLoadedHandler(this._configLoadedEvent.EventHandlerType);
							if (this._configLoadedHandler != null)
							{
								this._configLoadedEvent.AddEventHandler(system, this._configLoadedHandler);
							}
						}
						this._configLibSubscribed = true;
						this.OnConfigLoaded();
						result = true;
					}
				}
			}
			catch (Exception exception)
			{
				this.ReportConfigLibIntegrationError(api, exception);
				this.UnsubscribeFromConfigChange();
				result = false;
			}
			return result;
		}

		// Token: 0x06000120 RID: 288 RVA: 0x00006050 File Offset: 0x00004250
		[NullableContext(1)]
		private void OnConfigSettingChanged(string domain, object setting)
		{
			if (domain != "combatoverhaul" && domain != "combatoverhaulfork" && domain != "bullseyecontinued" && domain != "overhaullib")
			{
				return;
			}
			bool applied = this.TryInvokeConfigAssignment(setting, "AssignSettingValue");
			if (!applied)
			{
				applied = this.ApplyConfigFileSettings(this._configLibSubscriptionApi);
			}
			if (applied)
			{
				this.ReapplyServerGameplaySettingsIfNeeded(this._configLibSubscriptionApi);
				CombatOverhaulSystem.ApplyRuntimeSettings(this.Settings);
				if (this._configLibSubscriptionApi != null)
				{
					HarmonyPatchesManager.ConfigureDiagnostics(this._configLibSubscriptionApi, this.Settings);
				}
				ICoreAPI configLibSubscriptionApi = this._configLibSubscriptionApi;
				if (configLibSubscriptionApi != null && configLibSubscriptionApi.Side == 1)
				{
					this.BroadcastGameplaySettings();
				}
				Action<Settings> settingsChanged = this.SettingsChanged;
				if (settingsChanged == null)
				{
					return;
				}
				settingsChanged.Invoke(this.Settings);
			}
		}

		// Token: 0x06000121 RID: 289 RVA: 0x00006118 File Offset: 0x00004318
		private void OnConfigLoaded()
		{
			this.AssignConfigSettings("combatoverhaul");
			this.AssignConfigSettings("combatoverhaulfork");
			this.AssignConfigSettings("bullseyecontinued");
			this.AssignConfigSettings("overhaullib");
			this.ApplyConfigFileSettings(this._configLibSubscriptionApi);
			this.ReapplyServerGameplaySettingsIfNeeded(this._configLibSubscriptionApi);
			CombatOverhaulSystem.ApplyRuntimeSettings(this.Settings);
			if (this._configLibSubscriptionApi != null)
			{
				HarmonyPatchesManager.ConfigureDiagnostics(this._configLibSubscriptionApi, this.Settings);
			}
			ICoreAPI configLibSubscriptionApi = this._configLibSubscriptionApi;
			if (configLibSubscriptionApi != null && configLibSubscriptionApi.Side == 1)
			{
				this.BroadcastGameplaySettings();
			}
			Action<Settings> settingsLoaded = this.SettingsLoaded;
			if (settingsLoaded == null)
			{
				return;
			}
			settingsLoaded.Invoke(this.Settings);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000061C4 File Offset: 0x000043C4
		private void UnsubscribeFromConfigChange()
		{
			if (this._configLibSystem != null)
			{
				if (this._configSettingChangedHandler != null)
				{
					EventInfo configSettingChangedEvent = this._configSettingChangedEvent;
					if (configSettingChangedEvent != null)
					{
						configSettingChangedEvent.RemoveEventHandler(this._configLibSystem, this._configSettingChangedHandler);
					}
				}
				if (this._configLoadedHandler != null)
				{
					EventInfo configLoadedEvent = this._configLoadedEvent;
					if (configLoadedEvent != null)
					{
						configLoadedEvent.RemoveEventHandler(this._configLibSystem, this._configLoadedHandler);
					}
				}
			}
			this._configLibSystem = null;
			this._configSettingChangedEvent = null;
			this._configLoadedEvent = null;
			this._configGetConfigMethod = null;
			this._configSettingChangedHandler = null;
			this._configLoadedHandler = null;
			this._configLibSubscribed = false;
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00006254 File Offset: 0x00004454
		[NullableContext(1)]
		private void AssignConfigSettings(string domain)
		{
			if (this._configLibSystem == null || this._configGetConfigMethod == null)
			{
				return;
			}
			object config = this._configGetConfigMethod.Invoke(this._configLibSystem, new object[]
			{
				domain
			});
			if (config == null)
			{
				return;
			}
			this.TryInvokeConfigAssignment(config, "AssignSettingsValues");
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000062A8 File Offset: 0x000044A8
		private bool ApplyConfigFileSettings(ICoreAPI api)
		{
			string configPath = Path.Combine(GamePaths.ModConfig, "combatoverhaul.yaml");
			if (!File.Exists(configPath))
			{
				return false;
			}
			List<string> appliedSettings = new List<string>();
			try
			{
				foreach (string line2 in File.ReadLines(configPath))
				{
					string line = CombatOverhaulSystem.StripYamlComment(line2).Trim();
					if (line.Length != 0)
					{
						int separator = line.IndexOf(':');
						if (separator > 0)
						{
							string key = line.Substring(0, separator).Trim();
							string text = line;
							int num = separator + 1;
							string value = text.Substring(num, text.Length - num).Trim();
							string appliedSetting;
							if (key.Length != 0 && value.Length != 0 && this.TryAssignSettingsPropertyFromString(key, value, out appliedSetting) && appliedSetting != null)
							{
								appliedSettings.Add(appliedSetting);
							}
						}
					}
				}
			}
			catch (Exception exception)
			{
				ICoreAPI api2 = api ?? this._clientApi;
				Type typeFromHandle = typeof(CombatOverhaulSystem);
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(56, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Could not apply Combat Overhaul runtime config file '");
				defaultInterpolatedStringHandler.AppendFormatted(configPath);
				defaultInterpolatedStringHandler.AppendLiteral("':\n");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
				LoggerUtil.Warn(api2, typeFromHandle, defaultInterpolatedStringHandler.ToStringAndClear());
				return false;
			}
			if (appliedSettings.Count == 0)
			{
				return false;
			}
			string summary = string.Join(", ", Enumerable.OrderBy<string, string>(appliedSettings, (string setting) => setting));
			if (!string.Equals(summary, this._lastAppliedConfigFileSettingsSummary, 4))
			{
				this._lastAppliedConfigFileSettingsSummary = summary;
			}
			return true;
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00006458 File Offset: 0x00004658
		[NullableContext(1)]
		private bool TryAssignSettingsPropertyFromString(string key, string value, [Nullable(2)] out string appliedSetting)
		{
			appliedSetting = null;
			string normalizedKey = CombatOverhaulSystem.NormalizeConfigName(key);
			PropertyInfo property2 = Enumerable.FirstOrDefault<PropertyInfo>(typeof(Settings).GetProperties(20), (PropertyInfo property) => CombatOverhaulSystem.NormalizeConfigName(property.Name) == normalizedKey);
			if (property2 == null || !property2.CanWrite)
			{
				return false;
			}
			object converted;
			if (!CombatOverhaulSystem.TryConvertConfigValue(value, property2.PropertyType, out converted))
			{
				return false;
			}
			property2.SetValue(this.Settings, converted);
			appliedSetting = key + "=" + CombatOverhaulSystem.FormatConfigValue(converted);
			return true;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x000064E4 File Offset: 0x000046E4
		[NullableContext(1)]
		private static bool TryConvertConfigValue(string value, Type targetType, [Nullable(2)] out object converted)
		{
			converted = null;
			value = CombatOverhaulSystem.UnquoteConfigScalar(value);
			if (targetType == typeof(bool))
			{
				bool boolValue;
				if (!bool.TryParse(value, ref boolValue))
				{
					return false;
				}
				converted = boolValue;
				return true;
			}
			else if (targetType == typeof(int))
			{
				int intValue;
				if (!int.TryParse(value, 7, CultureInfo.InvariantCulture, ref intValue))
				{
					return false;
				}
				converted = intValue;
				return true;
			}
			else if (targetType == typeof(float))
			{
				float floatValue;
				if (!float.TryParse(value, 167, CultureInfo.InvariantCulture, ref floatValue))
				{
					return false;
				}
				converted = floatValue;
				return true;
			}
			else
			{
				if (targetType == typeof(string))
				{
					converted = value;
					return true;
				}
				return false;
			}
		}

		// Token: 0x06000127 RID: 295 RVA: 0x0000659C File Offset: 0x0000479C
		[NullableContext(1)]
		private static string FormatConfigValue([Nullable(2)] object value)
		{
			string result;
			if (value is bool)
			{
				result = ((bool)value).ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
			}
			else if (value is float)
			{
				result = ((float)value).ToString(CultureInfo.InvariantCulture);
			}
			else
			{
				IFormattable formattable = value as IFormattable;
				if (formattable == null)
				{
					result = (((value != null) ? value.ToString() : null) ?? "");
				}
				else
				{
					result = formattable.ToString(null, CultureInfo.InvariantCulture);
				}
			}
			return result;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00006620 File Offset: 0x00004820
		[NullableContext(1)]
		private static string UnquoteConfigScalar(string value)
		{
			value = value.Trim();
			if (value.Length >= 2)
			{
				if (value.get_Chars(0) == '"')
				{
					string text = value;
					if (text.get_Chars(text.Length - 1) == '"')
					{
						goto IL_4B;
					}
				}
				if (value.get_Chars(0) != '\'')
				{
					return value;
				}
				string text2 = value;
				if (text2.get_Chars(text2.Length - 1) != '\'')
				{
					return value;
				}
				IL_4B:
				string text3 = value;
				return text3.Substring(1, text3.Length - 1 - 1);
			}
			return value;
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00006690 File Offset: 0x00004890
		[NullableContext(1)]
		private static string StripYamlComment(string line)
		{
			bool inSingleQuote = false;
			bool inDoubleQuote = false;
			for (int index = 0; index < line.Length; index++)
			{
				char character = line.get_Chars(index);
				if (character == '\'' && !inDoubleQuote)
				{
					inSingleQuote = !inSingleQuote;
				}
				else if (character == '"' && !inSingleQuote)
				{
					inDoubleQuote = !inDoubleQuote;
				}
				else if (character == '#' && !inSingleQuote && !inDoubleQuote)
				{
					return line.Substring(0, index);
				}
			}
			return line;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x000066F0 File Offset: 0x000048F0
		[NullableContext(1)]
		private unsafe static string NormalizeConfigName(string name)
		{
			int i = name.Length;
			Span<char> buffer = new Span<char>(stackalloc byte[checked(unchecked((UIntPtr)i) * 2)], i);
			int length = 0;
			for (i = 0; i < name.Length; i++)
			{
				char character = name.get_Chars(i);
				if (char.IsLetterOrDigit(character))
				{
					*buffer[length++] = char.ToLowerInvariant(character);
				}
			}
			return new string(buffer.Slice(0, length));
		}

		// Token: 0x0600012B RID: 299 RVA: 0x00006760 File Offset: 0x00004960
		[NullableContext(1)]
		private bool TryInvokeConfigAssignment(object target, string methodName)
		{
			try
			{
				if (this.TryInvokeInstanceConfigAssignment(target, methodName))
				{
					return true;
				}
				if (this.TryInvokeExtensionConfigAssignment(target, methodName))
				{
					return true;
				}
			}
			catch (Exception exception)
			{
				this.ReportConfigLibIntegrationError(null, exception);
			}
			return false;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x000067AC File Offset: 0x000049AC
		[NullableContext(1)]
		private bool TryInvokeInstanceConfigAssignment(object target, string methodName)
		{
			MethodInfo method2 = Enumerable.FirstOrDefault<MethodInfo>(target.GetType().GetMethods(20), (MethodInfo method) => CombatOverhaulSystem.IsConfigAssignmentMethod(method, methodName, false, target.GetType()));
			if (method2 == null)
			{
				return false;
			}
			method2.Invoke(target, new object[]
			{
				this.Settings
			});
			return true;
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000681C File Offset: 0x00004A1C
		[NullableContext(1)]
		private bool TryInvokeExtensionConfigAssignment(object target, string methodName)
		{
			Type targetType = target.GetType();
			Type settingsType = this.Settings.GetType();
			foreach (Type type in CombatOverhaulSystem.GetLoadableTypes(targetType.Assembly))
			{
				if (type.IsAbstract && type.IsSealed)
				{
					foreach (MethodInfo method in type.GetMethods(24))
					{
						if (method.IsDefined(typeof(ExtensionAttribute), false) && CombatOverhaulSystem.IsConfigAssignmentMethod(method, methodName, true, targetType) && method.GetParameters()[1].ParameterType.IsAssignableFrom(settingsType))
						{
							method.Invoke(null, new object[]
							{
								target,
								this.Settings
							});
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00006914 File Offset: 0x00004B14
		[NullableContext(1)]
		private static bool IsConfigAssignmentMethod(MethodInfo method, string methodName, bool extensionMethod, Type targetType)
		{
			if (method.Name != methodName)
			{
				return false;
			}
			ParameterInfo[] parameters = method.GetParameters();
			if (extensionMethod)
			{
				return parameters.Length == 2 && parameters[0].ParameterType.IsAssignableFrom(targetType);
			}
			return parameters.Length == 1;
		}

		// Token: 0x0600012F RID: 303 RVA: 0x00006958 File Offset: 0x00004B58
		[NullableContext(1)]
		[return: Nullable(2)]
		private Delegate CreateConfigSettingChangedHandler(Type eventHandlerType)
		{
			MethodInfo method = eventHandlerType.GetMethod("Invoke");
			ParameterInfo[] parametersInfo = (method != null) ? method.GetParameters() : null;
			if (parametersInfo == null || parametersInfo.Length != 3)
			{
				return null;
			}
			ParameterExpression[] parameters = Enumerable.ToArray<ParameterExpression>(Enumerable.Select<ParameterInfo, ParameterExpression>(parametersInfo, (ParameterInfo parameter) => Expression.Parameter(parameter.ParameterType, parameter.Name)));
			MethodInfo method2 = base.GetType().GetMethod("OnConfigSettingChanged", 36);
			if (method2 == null)
			{
				throw new MissingMethodException("OnConfigSettingChanged");
			}
			MethodInfo targetMethod = method2;
			MethodCallExpression body = Expression.Call(Expression.Constant(this), targetMethod, Expression.Convert(parameters[0], typeof(string)), Expression.Convert(parameters[2], typeof(object)));
			return Expression.Lambda(eventHandlerType, body, parameters).Compile();
		}

		// Token: 0x06000130 RID: 304 RVA: 0x00006A1C File Offset: 0x00004C1C
		[NullableContext(1)]
		[return: Nullable(2)]
		private Delegate CreateConfigLoadedHandler(Type eventHandlerType)
		{
			MethodInfo method = eventHandlerType.GetMethod("Invoke");
			ParameterInfo[] array = (method != null) ? method.GetParameters() : null;
			if (array == null || array.Length != 0)
			{
				return null;
			}
			MethodInfo method2 = base.GetType().GetMethod("OnConfigLoaded", 36);
			if (method2 == null)
			{
				throw new MissingMethodException("OnConfigLoaded");
			}
			MethodInfo targetMethod = method2;
			return Delegate.CreateDelegate(eventHandlerType, this, targetMethod, false);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x00006A7C File Offset: 0x00004C7C
		[NullableContext(1)]
		[return: Nullable(2)]
		private static object GetModSystem(ICoreAPI api, Type modSystemType)
		{
			MethodInfo methodInfo = Enumerable.FirstOrDefault<MethodInfo>(api.ModLoader.GetType().GetMethods(20), (MethodInfo method) => method.Name == "GetModSystem" && method.IsGenericMethodDefinition && method.GetParameters().Length == 0);
			if (methodInfo == null)
			{
				return null;
			}
			return methodInfo.MakeGenericMethod(new Type[]
			{
				modSystemType
			}).Invoke(api.ModLoader, null);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00006AE0 File Offset: 0x00004CE0
		[NullableContext(1)]
		[return: Nullable(2)]
		private static Type FindLoadedType(string fullName)
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				Type type = assemblies[i].GetType(fullName, false);
				if (type != null)
				{
					return type;
				}
			}
			return null;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00006B20 File Offset: 0x00004D20
		[NullableContext(1)]
		private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
		{
			IEnumerable<Type> result;
			try
			{
				result = assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				result = Enumerable.Cast<Type>(Enumerable.Where<Type>(ex.Types, (Type type) => type != null));
			}
			return result;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00006B78 File Offset: 0x00004D78
		[NullableContext(1)]
		private void ReportConfigLibIntegrationError([Nullable(2)] ICoreAPI api, Exception exception)
		{
			if (this._reportedConfigLibIntegrationError)
			{
				return;
			}
			this._reportedConfigLibIntegrationError = true;
			ICoreAPI api2 = api ?? this._clientApi;
			Type typeFromHandle = typeof(CombatOverhaulSystem);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(56, 1);
			defaultInterpolatedStringHandler.AppendLiteral("ConfigLib optional integration failed and was disabled:\n");
			defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
			LoggerUtil.Warn(api2, typeFromHandle, defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00006BD4 File Offset: 0x00004DD4
		[NullableContext(1)]
		private static void ApplyRuntimeSettings(Settings settings)
		{
			DamageResistData.EntityProtectionFactor = settings.EntityProtectionMultiplier;
			QuenchableStatUtil.WeaponDamageMultiplier = Math.Max(0f, settings.WeaponQuenchDamageMultiplier);
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00006BF8 File Offset: 0x00004DF8
		[NullableContext(1)]
		private void EnsureOwnPlayerAnimationBehaviors(IClientPlayer player)
		{
			EntityPlayer entity = player.Entity;
			long? num = (entity != null) ? new long?(entity.EntityId) : default(long?);
			ICoreClientAPI clientApi = this._clientApi;
			long? num2;
			if (clientApi == null)
			{
				num2 = default(long?);
			}
			else
			{
				IClientWorldAccessor world = clientApi.World;
				if (world == null)
				{
					num2 = default(long?);
				}
				else
				{
					IClientPlayer player2 = world.Player;
					if (player2 == null)
					{
						num2 = default(long?);
					}
					else
					{
						EntityPlayer entity2 = player2.Entity;
						num2 = ((entity2 != null) ? new long?(entity2.EntityId) : default(long?));
					}
				}
			}
			long? num3 = num2;
			if (num.GetValueOrDefault() == num3.GetValueOrDefault() & num != null == (num3 != null))
			{
				this.EnsureOwnPlayerAnimationBehaviors();
			}
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00006CAC File Offset: 0x00004EAC
		private void EnsureOwnPlayerAnimationBehaviors()
		{
			try
			{
				ICoreClientAPI clientApi = this._clientApi;
				EntityPlayer entityPlayer;
				if (clientApi == null)
				{
					entityPlayer = null;
				}
				else
				{
					IClientWorldAccessor world = clientApi.World;
					if (world == null)
					{
						entityPlayer = null;
					}
					else
					{
						IClientPlayer player = world.Player;
						entityPlayer = ((player != null) ? player.Entity : null);
					}
				}
				EntityPlayer playerEntity = entityPlayer;
				if (playerEntity != null)
				{
					if (playerEntity.GetBehavior<FirstPersonAnimationsBehavior>() == null || playerEntity.GetBehavior<ThirdPersonAnimationsBehavior>() == null || playerEntity.GetBehavior<WearableStatsBehavior>() == null)
					{
						bool added = false;
						JsonObject emptyAttributes = new JsonObject(new JObject());
						if (playerEntity.GetBehavior<FirstPersonAnimationsBehavior>() == null)
						{
							FirstPersonAnimationsBehavior firstPerson = new FirstPersonAnimationsBehavior(playerEntity);
							playerEntity.AddBehavior(firstPerson);
							firstPerson.Initialize(playerEntity.Properties, emptyAttributes);
							firstPerson.AfterInitialized(false);
							added = true;
						}
						if (playerEntity.GetBehavior<ThirdPersonAnimationsBehavior>() == null)
						{
							ThirdPersonAnimationsBehavior thirdPerson = new ThirdPersonAnimationsBehavior(playerEntity);
							playerEntity.AddBehavior(thirdPerson);
							thirdPerson.Initialize(playerEntity.Properties, emptyAttributes);
							thirdPerson.AfterInitialized(false);
							added = true;
						}
						if (playerEntity.GetBehavior<WearableStatsBehavior>() == null)
						{
							WearableStatsBehavior wearableStats = new WearableStatsBehavior(playerEntity);
							playerEntity.AddBehavior(wearableStats);
							wearableStats.Initialize(playerEntity.Properties, emptyAttributes);
							wearableStats.AfterInitialized(false);
							wearableStats.RefreshStatsNow();
							added = true;
						}
						if (added && !this._reportedAnimationBehaviorFallback)
						{
							this._reportedAnimationBehaviorFallback = true;
							LoggerUtil.Warn(this._clientApi, this, "Attached missing OverhaulLib player client behaviors at runtime.");
						}
					}
				}
			}
			catch (Exception exception)
			{
				if (!this._reportedAnimationBehaviorFallbackError)
				{
					this._reportedAnimationBehaviorFallbackError = true;
					ICoreAPI clientApi2 = this._clientApi;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
					defaultInterpolatedStringHandler..ctor(68, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Could not attach OverhaulLib player animation behaviors at runtime: ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
					LoggerUtil.Warn(clientApi2, this, defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00006E30 File Offset: 0x00005030
		private void PatchShieldsOnClientLevelFinalize()
		{
			if (this._clientApi != null)
			{
				ShieldAutoPatcher.Patch(this._clientApi);
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00006E45 File Offset: 0x00005045
		private void PatchShieldsOnServerTick(float dt)
		{
			if (this._serverApi == null)
			{
				return;
			}
			if (this._shieldAutoPatchServerListener != 0L)
			{
				this._serverApi.Event.UnregisterGameTickListener(this._shieldAutoPatchServerListener);
				this._shieldAutoPatchServerListener = 0L;
			}
			ShieldAutoPatcher.Patch(this._serverApi);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x00006E84 File Offset: 0x00005084
		[NullableContext(1)]
		private void DetermineSlotsStatus(ICoreClientAPI api)
		{
			foreach (Item item in api.World.Items)
			{
				object obj;
				if (item == null)
				{
					obj = null;
				}
				else
				{
					JsonObject attributes = item.Attributes;
					obj = ((attributes != null) ? attributes["clothescategory"].AsString(null) : null);
				}
				object obj2;
				if ((obj2 = obj) == null)
				{
					if (item == null)
					{
						obj2 = null;
					}
					else
					{
						JsonObject attributes2 = item.Attributes;
						obj2 = ((attributes2 != null) ? attributes2["attachableToEntity"]["categoryCode"].AsString(null) : null);
					}
				}
				string stackDressType = obj2;
				object obj3;
				if (item == null)
				{
					obj3 = null;
				}
				else
				{
					JsonObject attributes3 = item.Attributes;
					obj3 = ((attributes3 != null) ? attributes3["clothescategories"].AsObject<string[]>(null) : null);
				}
				object obj4;
				if ((obj4 = obj3) == null)
				{
					if (item == null)
					{
						obj4 = null;
					}
					else
					{
						JsonObject attributes4 = item.Attributes;
						obj4 = ((attributes4 != null) ? attributes4["attachableToEntity"]["categoryCodes"].AsObject<string[]>(null) : null);
					}
				}
				string[] stackDressTypes = obj4;
				if (stackDressType != null)
				{
					CombatOverhaulSystem.SetSlotsStatus(stackDressType);
				}
				if (stackDressTypes != null)
				{
					string[] array = stackDressTypes;
					for (int i = 0; i < array.Length; i++)
					{
						CombatOverhaulSystem.SetSlotsStatus(array[i]);
					}
				}
			}
		}

		// Token: 0x0600013B RID: 315 RVA: 0x00006FAC File Offset: 0x000051AC
		[NullableContext(1)]
		private static void SetSlotsStatus(string gearType)
		{
			if (gearType != null)
			{
				switch (gearType.Length)
				{
				case 8:
				{
					char c = gearType.get_Chars(0);
					if (c != 'b')
					{
						if (c != 'h')
						{
							if (c != 'm')
							{
								return;
							}
							if (!(gearType == "miscgear"))
							{
								return;
							}
							CharacterTabPatch.SlotsStatus.Misc = true;
							return;
						}
						else
						{
							if (!(gearType == "headgear"))
							{
								return;
							}
							CharacterTabPatch.SlotsStatus.Headgear = true;
							return;
						}
					}
					else
					{
						if (!(gearType == "backgear"))
						{
							return;
						}
						CharacterTabPatch.SlotsStatus.BackGear = true;
						return;
					}
					break;
				}
				case 9:
				{
					char c = gearType.get_Chars(0);
					if (c != 'f')
					{
						if (c != 'w')
						{
							return;
						}
						if (!(gearType == "waistgear"))
						{
							return;
						}
						CharacterTabPatch.SlotsStatus.WaistHear = true;
						return;
					}
					else
					{
						if (!(gearType == "frontgear"))
						{
							return;
						}
						CharacterTabPatch.SlotsStatus.FrontGear = true;
						return;
					}
					break;
				}
				case 10:
				case 13:
				case 14:
				case 15:
					break;
				case 11:
				{
					char c = gearType.get_Chars(7);
					if (c != 'B')
					{
						if (c != 'L')
						{
							return;
						}
						if (!(gearType == "addBeltLeft"))
						{
							return;
						}
						CharacterTabPatch.SlotsStatus.Belt = true;
						return;
					}
					else
					{
						if (!(gearType == "addBeltBack"))
						{
							return;
						}
						CharacterTabPatch.SlotsStatus.Belt = true;
						return;
					}
					break;
				}
				case 12:
				{
					char c = gearType.get_Chars(11);
					switch (c)
					{
					case '1':
						if (!(gearType == "addBackpack1"))
						{
							return;
						}
						CharacterTabPatch.SlotsStatus.Backpack = true;
						return;
					case '2':
						if (!(gearType == "addBackpack2"))
						{
							return;
						}
						CharacterTabPatch.SlotsStatus.Backpack = true;
						return;
					case '3':
						if (!(gearType == "addBackpack3"))
						{
							return;
						}
						CharacterTabPatch.SlotsStatus.Backpack = true;
						return;
					case '4':
						if (!(gearType == "addBackpack4"))
						{
							return;
						}
						CharacterTabPatch.SlotsStatus.Backpack = true;
						break;
					default:
						if (c != 't')
						{
							return;
						}
						if (gearType == "addBeltRight")
						{
							CharacterTabPatch.SlotsStatus.Belt = true;
							return;
						}
						if (!(gearType == "addBeltFront"))
						{
							return;
						}
						CharacterTabPatch.SlotsStatus.Belt = true;
						return;
					}
					break;
				}
				case 16:
					if (!(gearType == "leftshouldergear"))
					{
						return;
					}
					CharacterTabPatch.SlotsStatus.LeftShoulderGear = true;
					return;
				case 17:
					if (!(gearType == "rightshouldergear"))
					{
						return;
					}
					CharacterTabPatch.SlotsStatus.RightShoulderGear = true;
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x04000059 RID: 89
		[Nullable(1)]
		public const string VanityInventoryCode = "combatoverhaul:vanity";

		// Token: 0x0400005A RID: 90
		[Nullable(1)]
		private static readonly JObject DefaultOnTongTransform = JObject.Parse("{\n  \"translation\": { \"x\": -0.9, \"y\": -0.74, \"z\": -0.39 },\n  \"rotation\": { \"x\": -102, \"y\": -109, \"z\": 16 }\n}");

		// Token: 0x0400005B RID: 91
		[Nullable(1)]
		private static readonly JObject DefaultOnMetalTongTransform = JObject.Parse("{\n  \"translation\": { \"x\": -1.2, \"y\": -0.74, \"z\": -0.59 },\n  \"rotation\": { \"x\": -90, \"y\": -102, \"z\": -92 },\n  \"origin\": { \"x\": 0.5, \"y\": 0.6, \"z\": 0.5 },\n  \"scale\": 0.93\n}");

		// Token: 0x04000076 RID: 118
		private ICoreClientAPI _clientApi;

		// Token: 0x04000077 RID: 119
		private readonly Vector4 _iconScale = new Vector4(-0.1f, -0.1f, 1.2f, 1.2f);

		// Token: 0x04000078 RID: 120
		private IClientNetworkChannel _clientToggleChannel;

		// Token: 0x04000079 RID: 121
		private IServerNetworkChannel _serverToggleChannel;

		// Token: 0x0400007A RID: 122
		private IClientNetworkChannel _clientGameplaySettingsChannel;

		// Token: 0x0400007B RID: 123
		private IServerNetworkChannel _serverGameplaySettingsChannel;

		// Token: 0x0400007C RID: 124
		private ICoreServerAPI _serverApi;

		// Token: 0x0400007D RID: 125
		private PlayerDelegate _playerNowPlayingSettingsHandler;

		// Token: 0x0400007E RID: 126
		private ServerGameplaySettingsPacket _lastServerGameplaySettings;

		// Token: 0x0400007F RID: 127
		[Nullable(1)]
		private const string GameplaySettingsChannelId = "CombatOverhaul:server-gameplay-settings";

		// Token: 0x04000080 RID: 128
		[Nullable(1)]
		private const string _iconsFolder = "sloticons";

		// Token: 0x04000081 RID: 129
		[Nullable(1)]
		private const string _iconsPath = "textures/sloticons/";

		// Token: 0x04000082 RID: 130
		private long _cacheMissesReportedListener;

		// Token: 0x04000083 RID: 131
		private long _ensureAnimationBehaviorsListener;

		// Token: 0x04000084 RID: 132
		private long _shieldAutoPatchServerListener;

		// Token: 0x04000085 RID: 133
		private bool _reportedAnimationBehaviorFallback;

		// Token: 0x04000086 RID: 134
		private bool _reportedAnimationBehaviorFallbackError;

		// Token: 0x04000087 RID: 135
		[Nullable(1)]
		private readonly HashSet<AssetLocation> _reportedMissingSvgIcons = new HashSet<AssetLocation>();

		// Token: 0x04000088 RID: 136
		private object _configLibSystem;

		// Token: 0x04000089 RID: 137
		private EventInfo _configSettingChangedEvent;

		// Token: 0x0400008A RID: 138
		private EventInfo _configLoadedEvent;

		// Token: 0x0400008B RID: 139
		private MethodInfo _configGetConfigMethod;

		// Token: 0x0400008C RID: 140
		private Delegate _configSettingChangedHandler;

		// Token: 0x0400008D RID: 141
		private Delegate _configLoadedHandler;

		// Token: 0x0400008E RID: 142
		private ICoreAPI _configLibSubscriptionApi;

		// Token: 0x0400008F RID: 143
		private long _configLibSubscribeRetryListener;

		// Token: 0x04000090 RID: 144
		private bool _configLibSubscribed;

		// Token: 0x04000091 RID: 145
		private bool _reportedConfigLibIntegrationError;

		// Token: 0x04000092 RID: 146
		private string _lastAppliedConfigFileSettingsSummary;

		// Token: 0x04000093 RID: 147
		private const int ConfigLibSubscribeRetryIntervalMs = 500;
	}
}
