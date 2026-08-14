using System;
using HarmonyLib;
using Jaunt.Behaviors;
using Jaunt.Config;
using Jaunt.Entities;
using Jaunt.Hud;
using Jaunt.Items;
using Jaunt.Systems;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.Client.NoObf;

namespace Jaunt
{
	// Token: 0x02000003 RID: 3
	public class JauntModSystem : ModSystem
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x00002074 File Offset: 0x00000274
		public string ModId
		{
			get
			{
				return base.Mod.Info.ModID;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002086 File Offset: 0x00000286
		public ILogger Logger
		{
			get
			{
				return base.Mod.Logger;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000004 RID: 4 RVA: 0x00002093 File Offset: 0x00000293
		// (set) Token: 0x06000005 RID: 5 RVA: 0x0000209B File Offset: 0x0000029B
		public ICoreAPI api { get; private set; }

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000006 RID: 6 RVA: 0x000020A4 File Offset: 0x000002A4
		// (set) Token: 0x06000007 RID: 7 RVA: 0x000020AC File Offset: 0x000002AC
		public ICoreClientAPI capi { get; private set; }

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000020B5 File Offset: 0x000002B5
		// (set) Token: 0x06000009 RID: 9 RVA: 0x000020BD File Offset: 0x000002BD
		public JauntConfig Config { get; private set; }

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000020C6 File Offset: 0x000002C6
		// (set) Token: 0x0600000B RID: 11 RVA: 0x000020CD File Offset: 0x000002CD
		public static JauntModSystem Instance { get; private set; }

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000C RID: 12 RVA: 0x000020D5 File Offset: 0x000002D5
		// (set) Token: 0x0600000D RID: 13 RVA: 0x000020DC File Offset: 0x000002DC
		public static Harmony HarmonyInstance { get; private set; }

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000E RID: 14 RVA: 0x000020E4 File Offset: 0x000002E4
		internal bool DebugMode
		{
			get
			{
				return this.Config.GlobalDebugMode;
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000020F4 File Offset: 0x000002F4
		public override void Start(ICoreAPI api)
		{
			JauntModSystem.Instance = this;
			this.api = api;
			api.RegisterItemClass(this.ModId + ":iteminstrument", typeof(ItemJauntInstrument));
			api.RegisterEntity(this.ModId + ":entityflyingagent", typeof(EntityFlyingAgent));
			api.RegisterEntity(this.ModId + ":entityjauntagent", typeof(EntityJauntAgent));
			api.RegisterEntityBehaviorClass(this.ModId + ":gait", typeof(EntityBehaviorJauntGait));
			api.RegisterEntityBehaviorClass(this.ModId + ":rideable", typeof(EntityBehaviorJauntRideable));
			api.RegisterEntityBehaviorClass(this.ModId + ":stamina", typeof(EntityBehaviorJauntStamina));
			api.RegisterEntityBehaviorClass(this.ModId + ":armor", typeof(EntityBehaviorJauntArmor));
			api.RegisterEntityBehaviorClass(this.ModId + ":revivable", typeof(EntityBehaviorJauntRevivable));
			api.RegisterEntityBehaviorClass(this.ModId + ":walkspeed", typeof(EntityBehaviorJauntWalkspeed));
			api.RegisterEntityBehaviorClass(this.ModId + ":aicontrol", typeof(EntityBehaviorJauntAiControl));
			api.RegisterCollectibleBehaviorClass(this.ModId + ":attachments", typeof(CollectibleBehaviorJauntAttachments));
			api.RegisterItemClass(this.ModId + ".iteminstrument", typeof(ItemJauntInstrument));
			api.RegisterEntity(this.ModId + ".entityflyingagent", typeof(EntityFlyingAgent));
			api.RegisterEntity(this.ModId + ".entityjauntagent", typeof(EntityJauntAgent));
			api.RegisterEntityBehaviorClass(this.ModId + ".gait", typeof(EntityBehaviorJauntGait));
			api.RegisterEntityBehaviorClass(this.ModId + ".rideable", typeof(EntityBehaviorJauntRideable));
			api.RegisterEntityBehaviorClass(this.ModId + ".stamina", typeof(EntityBehaviorJauntStamina));
			api.RegisterEntityBehaviorClass(this.ModId + ".armor", typeof(EntityBehaviorJauntArmor));
			api.RegisterEntityBehaviorClass(this.ModId + ".revivable", typeof(EntityBehaviorJauntRevivable));
			api.RegisterEntityBehaviorClass(this.ModId + ".walkspeed", typeof(EntityBehaviorJauntWalkspeed));
			api.RegisterEntityBehaviorClass(this.ModId + ".aicontrol", typeof(EntityBehaviorJauntAiControl));
			api.RegisterCollectibleBehaviorClass(this.ModId + ".attachments", typeof(CollectibleBehaviorJauntAttachments));
			JauntModSystem.HarmonyInstance = new Harmony(this.ModId);
			JauntModSystem.HarmonyInstance.PatchAll();
			this.ReloadConfig();
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000023EE File Offset: 0x000005EE
		public override void StartServerSide(ICoreServerAPI api)
		{
			api.Event.OnEntityLoaded += new EntityDelegate(this.AddFatigueHandlers);
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00002408 File Offset: 0x00000608
		public override void StartClientSide(ICoreClientAPI api)
		{
			this.capi = api;
			if (this.Config.EnableStamina)
			{
				this.customHudListenerId = api.Event.RegisterGameTickListener(new Action<float>(this.CheckAndInitializeCustomHud), 20, 0);
			}
			if (this.Config.ShowGaitIcon)
			{
				this.hudIconRenderer = new HudIconRenderer(api);
				this.hudIconRenderer.Initialize();
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002470 File Offset: 0x00000670
		private void CheckAndInitializeCustomHud(float dt)
		{
			HudStatbar vanillaHudStatbar = this.GetVanillaStatbarHud();
			if (vanillaHudStatbar != null && vanillaHudStatbar.IsOpened())
			{
				this._staminaHud = new HudElementStaminaBar(this.capi);
				this.capi.Event.RegisterGameTickListener(new Action<float>(this._staminaHud.OnGameTick), 1000, 0);
				this.capi.Gui.RegisterDialog(new GuiDialog[]
				{
					this._staminaHud
				});
				this.capi.Event.UnregisterGameTickListener(this.customHudListenerId);
			}
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002500 File Offset: 0x00000700
		private HudStatbar GetVanillaStatbarHud()
		{
			foreach (GuiDialog guiDialog in this.capi.Gui.OpenedGuis)
			{
				HudStatbar statbar = guiDialog as HudStatbar;
				if (statbar != null)
				{
					return statbar;
				}
			}
			return null;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002568 File Offset: 0x00000768
		private void AddFatigueHandlers(Entity entity)
		{
			EntityBehaviorJauntStamina ebs = entity.GetBehavior<EntityBehaviorJauntStamina>();
			if (ebs == null)
			{
				return;
			}
			ebs.OnFatigued += ((float ftg, FatigueSource ftgSource) => this.HandleFatigued(entity as EntityAgent, ftg, ftgSource));
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000025AB File Offset: 0x000007AB
		public float HandleFatigued(EntityAgent eagent, float fatigue, FatigueSource ftgSource)
		{
			fatigue = this.ApplyFatigueProtection(eagent, fatigue, ftgSource);
			return fatigue;
		}

		// Token: 0x06000016 RID: 22 RVA: 0x000025B9 File Offset: 0x000007B9
		public float ApplyFatigueProtection(EntityAgent eagent, float fatigue, FatigueSource ftgSource)
		{
			return fatigue;
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000025BC File Offset: 0x000007BC
		public void ReloadConfig()
		{
			try
			{
				JauntConfig config = this.api.LoadModConfig<JauntConfig>(this.ModId + ".json");
				if (config == null)
				{
					this.Logger.Warning("Missing config! Using default.");
					config = new JauntConfig();
					this.api.StoreModConfig<JauntConfig>(config, this.ModId + ".json");
				}
				if (config.ConfigVersion == null)
				{
					this.Logger.Warning("Missing config version! Using default.");
					config.ConfigVersion = "0.0.0";
					this.api.StoreModConfig<JauntConfig>(config, this.ModId + ".json");
				}
				if (GameVersion.IsNewerVersionThan(base.Mod.Info.Version, config.ConfigVersion))
				{
					this.Logger.Warning("Config outdated, migrating from version " + config.ConfigVersion + " to version " + base.Mod.Info.Version);
					config = JauntModSystem.MigrateConfig(config, config.ConfigVersion, base.Mod.Info.Version);
					this.api.StoreModConfig<JauntConfig>(config, this.ModId + ".json");
				}
				this.Config = config;
			}
			catch (Exception ex)
			{
				this.Logger.Error("Could not load " + this.ModId + " config!");
				this.Logger.Error(ex);
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002734 File Offset: 0x00000934
		public static JauntConfig MigrateConfig(JauntConfig config, string oldVersion, string newVersion)
		{
			if (GameVersion.IsNewerVersionThan("1.1.7", oldVersion))
			{
				config.GlobalDebugMode = false;
			}
			if (GameVersion.IsNewerVersionThan("2.0.0-rc.8", oldVersion))
			{
				config.IconOffsetX = -385f;
				config.IconOffsetY = -105f;
				config.IconSize = 75f;
			}
			config.ConfigVersion = newVersion;
			return config;
		}

		// Token: 0x04000001 RID: 1
		private HudElementStaminaBar _staminaHud;

		// Token: 0x04000002 RID: 2
		private long customHudListenerId;

		// Token: 0x04000003 RID: 3
		internal HudIconRenderer hudIconRenderer;
	}
}
