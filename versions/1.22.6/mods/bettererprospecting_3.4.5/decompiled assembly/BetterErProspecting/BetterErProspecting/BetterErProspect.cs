using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using BetterErProspecting.Config;
using BetterErProspecting.Item;
using ConfigLib;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace BetterErProspecting
{
	// Token: 0x02000002 RID: 2
	[NullableContext(1)]
	[Nullable(0)]
	public class BetterErProspect : ModSystem
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002057 File Offset: 0x00000257
		public static ILogger Logger { get; private set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x0000205F File Offset: 0x0000025F
		// (set) Token: 0x06000004 RID: 4 RVA: 0x00002066 File Offset: 0x00000266
		public static ICoreAPI Api { get; private set; }

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000005 RID: 5 RVA: 0x0000206E File Offset: 0x0000026E
		public static ModConfig Config
		{
			get
			{
				return ModConfig.Instance;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000006 RID: 6 RVA: 0x00002075 File Offset: 0x00000275
		// (set) Token: 0x06000007 RID: 7 RVA: 0x0000207C File Offset: 0x0000027C
		private static Harmony harmony { get; set; }

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000008 RID: 8 RVA: 0x00002084 File Offset: 0x00000284
		// (remove) Token: 0x06000009 RID: 9 RVA: 0x000020B8 File Offset: 0x000002B8
		public static event Action ReloadTools;

		// Token: 0x0600000A RID: 10 RVA: 0x000020EC File Offset: 0x000002EC
		public override void Start(ICoreAPI api)
		{
			base.Start(api);
			BetterErProspect.harmony = new Harmony(base.Mod.Info.ModID);
			BetterErProspect.Api = api;
			BetterErProspect.Logger = base.Mod.Logger;
			try
			{
				ModConfig.Instance = (api.LoadModConfig<ModConfig>(ModConfig.ConfigName) ?? new ModConfig());
				api.StoreModConfig<ModConfig>(BetterErProspect.Config, ModConfig.ConfigName);
			}
			catch (Exception)
			{
				ModConfig.Instance = new ModConfig();
			}
			if (api.ModLoader.IsModEnabled("configlib"))
			{
				this.SubscribeToConfigChange();
			}
			this.PatchUnpatch();
			api.RegisterItemClass("ItemProspectingPick", typeof(ItemBetterErProspectingPick));
			BetterErProspect.Logger.Debug("ItemProspectingPick item re-registered to mod's implementation");
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021BC File Offset: 0x000003BC
		private void SubscribeToConfigChange()
		{
			ConfigLibModSystem system = BetterErProspect.Api.ModLoader.GetModSystem<ConfigLibModSystem>(true);
			system.SettingChanged += delegate(string domain, IConfig _, ISetting setting)
			{
				if (domain != "bettererprospecting")
				{
					return;
				}
				setting.AssignSettingValue(BetterErProspect.Config);
				if (MemoryExtensions.Contains<string>(ModConfig.SettingsForceLoad, setting.YamlCode))
				{
					Action reloadTools = BetterErProspect.ReloadTools;
					if (reloadTools != null)
					{
						reloadTools.Invoke();
					}
				}
				if (MemoryExtensions.Contains<string>(ModConfig.SettingsPatch, setting.YamlCode))
				{
					this.PatchUnpatch();
				}
			};
			if (BetterErProspect.Api.Side != 1)
			{
				ICoreClientAPI coreClientAPI = BetterErProspect.Api as ICoreClientAPI;
				if (coreClientAPI == null || !coreClientAPI.IsSinglePlayer)
				{
					system.ConfigsLoaded += delegate()
					{
						Config config = (Config)system.GetConfig("bettererprospecting");
						base.<SubscribeToConfigChange>g__ApplyConfigChange|2(config);
						config.ConfigSaved -= new Action<Config>(base.<SubscribeToConfigChange>g__ApplyConfigChange|2);
						config.ConfigSaved += new Action<Config>(base.<SubscribeToConfigChange>g__ApplyConfigChange|2);
					};
					return;
				}
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002240 File Offset: 0x00000440
		public override void Dispose()
		{
			Harmony harmony = BetterErProspect.harmony;
			if (harmony != null)
			{
				harmony.UnpatchAll(base.Mod.Info.ModID);
			}
			ModConfig.Instance = null;
			BetterErProspect.harmony = null;
			BetterErProspect.Logger = null;
			BetterErProspect.Api = null;
			base.Dispose();
		}

		// Token: 0x0600000D RID: 13 RVA: 0x00002280 File Offset: 0x00000480
		private void PatchUnpatch()
		{
			BetterErProspect.harmony.UnpatchAll(base.Mod.Info.ModID);
			BetterErProspect.harmony.PatchCategory("Always");
			this.handleProspectTogether();
			if (ModConfig.Instance.NewDensityMode)
			{
				BetterErProspect.harmony.PatchCategory("NewDensity");
			}
			if (ModConfig.Instance.StoneSearchCreatesReadings)
			{
				BetterErProspect.harmony.PatchCategory("StoneReadings");
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000022F4 File Offset: 0x000004F4
		private void handleProspectTogether()
		{
			if (!BetterErProspect.Api.ModLoader.IsModEnabled("prospecttogether"))
			{
				return;
			}
			MethodInfo original = AccessTools.Method(typeof(OreMapLayer), "OnDataFromServer", null, null);
			Patches info = Harmony.GetPatchInfo(original);
			if (((info != null) ? info.Prefixes : null) != null)
			{
				foreach (Patch patch in Enumerable.ToList<Patch>(info.Prefixes))
				{
					if (patch.owner == "prospecttogether")
					{
						BetterErProspect.harmony.Unpatch(original, patch.PatchMethod);
					}
				}
			}
			BetterErProspect.harmony.PatchCategory("ProspectTogetherCompat");
		}

		// Token: 0x02000018 RID: 24
		[NullableContext(0)]
		public enum PatchCategory
		{
			// Token: 0x04000049 RID: 73
			Always,
			// Token: 0x0400004A RID: 74
			NewDensity,
			// Token: 0x0400004B RID: 75
			StoneReadings,
			// Token: 0x0400004C RID: 76
			ProspectTogetherCompat
		}
	}
}
