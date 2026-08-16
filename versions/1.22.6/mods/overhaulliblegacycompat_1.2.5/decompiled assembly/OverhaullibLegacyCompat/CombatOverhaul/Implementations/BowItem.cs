using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CombatOverhaul.Animations;
using CombatOverhaul.Compatibility;
using CombatOverhaul.Inputs;
using CombatOverhaul.RangedSystems;
using CombatOverhaul.Utils;
using CombatOverhaul.WeaponBuffs;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace CombatOverhaul.Implementations
{
	// Token: 0x02000077 RID: 119
	[NullableContext(2)]
	[Nullable(0)]
	public class BowItem : Item, IHasWeaponLogic, IHasRangedWeaponLogic, IHasMoveAnimations, IHasIdleAnimations
	{
		// Token: 0x1700010E RID: 270
		// (get) Token: 0x06000594 RID: 1428 RVA: 0x00020EF6 File Offset: 0x0001F0F6
		// (set) Token: 0x06000595 RID: 1429 RVA: 0x00020EFE File Offset: 0x0001F0FE
		public BowClient ClientLogic { get; private set; }

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x06000596 RID: 1430 RVA: 0x00020F07 File Offset: 0x0001F107
		// (set) Token: 0x06000597 RID: 1431 RVA: 0x00020F0F File Offset: 0x0001F10F
		public BowServer ServerLogic { get; private set; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x06000598 RID: 1432 RVA: 0x00020F18 File Offset: 0x0001F118
		IClientWeaponLogic IHasWeaponLogic.ClientLogic
		{
			get
			{
				return this.ClientLogic;
			}
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000599 RID: 1433 RVA: 0x00020F20 File Offset: 0x0001F120
		IServerRangedWeaponLogic IHasRangedWeaponLogic.ServerWeaponLogic
		{
			get
			{
				return this.ServerLogic;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x0600059A RID: 1434 RVA: 0x00020F28 File Offset: 0x0001F128
		// (set) Token: 0x0600059B RID: 1435 RVA: 0x00020F30 File Offset: 0x0001F130
		public AnimationRequestByCode IdleAnimation { get; set; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600059C RID: 1436 RVA: 0x00020F39 File Offset: 0x0001F139
		// (set) Token: 0x0600059D RID: 1437 RVA: 0x00020F41 File Offset: 0x0001F141
		public AnimationRequestByCode ReadyAnimation { get; set; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600059E RID: 1438 RVA: 0x00020F4A File Offset: 0x0001F14A
		// (set) Token: 0x0600059F RID: 1439 RVA: 0x00020F52 File Offset: 0x0001F152
		public AnimationRequestByCode WalkAnimation { get; set; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060005A0 RID: 1440 RVA: 0x00020F5B File Offset: 0x0001F15B
		// (set) Token: 0x060005A1 RID: 1441 RVA: 0x00020F63 File Offset: 0x0001F163
		public AnimationRequestByCode RunAnimation { get; set; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060005A2 RID: 1442 RVA: 0x00020F6C File Offset: 0x0001F16C
		// (set) Token: 0x060005A3 RID: 1443 RVA: 0x00020F74 File Offset: 0x0001F174
		public AnimationRequestByCode SwimAnimation { get; set; }

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060005A4 RID: 1444 RVA: 0x00020F7D File Offset: 0x0001F17D
		// (set) Token: 0x060005A5 RID: 1445 RVA: 0x00020F85 File Offset: 0x0001F185
		public AnimationRequestByCode SwimIdleAnimation { get; set; }

		// Token: 0x060005A6 RID: 1446 RVA: 0x00020F90 File Offset: 0x0001F190
		[NullableContext(1)]
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			ICoreClientAPI clientAPI = api as ICoreClientAPI;
			if (clientAPI != null)
			{
				BowStats stats = this.Attributes.AsObject<BowStats>(null);
				this.IdleAnimation = new AnimationRequestByCode(stats.IdleAnimation, 1f, 1f, "main", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null);
				this.ReadyAnimation = new AnimationRequestByCode(stats.ReadyAnimation, 1f, 1f, "main", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null);
				this.WalkAnimation = new AnimationRequestByCode(stats.WalkAnimation, 1f, 1f, "main", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null);
				this.RunAnimation = new AnimationRequestByCode(stats.RunAnimation, 1f, 1f, "main", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null);
				this.SwimAnimation = new AnimationRequestByCode(stats.SwimAnimation, 1f, 1f, "main", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null);
				this.SwimIdleAnimation = new AnimationRequestByCode(stats.SwimIdleAnimation, 1f, 1f, "main", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null);
				this._stats = stats;
				this._ammoSelector = new AmmoSelector(clientAPI, this._stats.ArrowWildcard);
				this._clientApi = clientAPI;
				this._clientArcheringCompat = new ArcheringBowCompat(clientAPI);
				this.ClientLogic = new BowClient(clientAPI, this, this._ammoSelector);
			}
			ICoreServerAPI serverAPI = api as ICoreServerAPI;
			if (serverAPI != null)
			{
				this.ServerLogic = new BowServer(serverAPI, this);
			}
			this._altForInteractions = new WorldInteraction
			{
				MouseButton = 255,
				HotKeyCode = "Alt",
				ActionLangCode = "combatoverhaul:interaction-hold-alt"
			};
			this._ammoSelection = new WorldInteraction
			{
				ActionLangCode = Lang.Get("combatoverhaul:interaction-ammoselection", Array.Empty<object>()),
				HotKeyCodes = new string[]
				{
					"toolmodeselect"
				},
				MouseButton = 255
			};
		}

		// Token: 0x060005A7 RID: 1447 RVA: 0x00021200 File Offset: 0x0001F400
		[NullableContext(1)]
		public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
		{
			if (this._stats != null)
			{
				string[] stats = this._stats.ProficiencyStats;
				if (this._stats.ProficiencyStat != "")
				{
					stats = Enumerable.ToArray<string>(Enumerable.Prepend<string>(stats, this._stats.ProficiencyStat));
				}
				if (stats.Length != 0)
				{
					string statsList = Enumerable.Aggregate<string>(Enumerable.Select<string, string>(stats, (string statName) => Lang.Get("combatoverhaul:proficiency-" + statName, Array.Empty<object>())), (string f, string s) => f + ", " + s);
					string description = Lang.Get("combatoverhaul:iteminfo-proficiency", new object[]
					{
						statsList
					});
					dsc.AppendLine(description);
				}
			}
			WeaponBuffSystem.AppendTooltip(inSlot.Itemstack, dsc, world, withDebugInfo);
			base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
			if (this._stats == null)
			{
				return;
			}
			ItemStackRangedStats stackStats = ItemStackRangedStats.FromItemStack(inSlot.Itemstack);
			string text = "combatoverhaul:iteminfo-range-weapon-damage";
			object[] array = new object[2];
			int num = 0;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			defaultInterpolatedStringHandler..ctor(0, 1);
			defaultInterpolatedStringHandler.AppendFormatted<float>(this._stats.ArrowDamageMultiplier * stackStats.DispersionMultiplier, "F1");
			array[num] = defaultInterpolatedStringHandler.ToStringAndClear();
			array[1] = this._stats.ArrowDamageTier + stackStats.DamageTierBonus;
			dsc.AppendLine(Lang.Get(text, array));
			ArcheringBowCompat clientArcheringCompat = this._clientArcheringCompat;
			ArcheringBowAimingSettings archeringSettings = (clientArcheringCompat != null) ? clientArcheringCompat.GetSettings(inSlot.Itemstack.Item) : null;
			if (archeringSettings != null)
			{
				StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler;
				appendInterpolatedStringHandler..ctor(11, 1, dsc);
				appendInterpolatedStringHandler.AppendFormatted<float>(archeringSettings.DrawTime, "F2");
				appendInterpolatedStringHandler.AppendLiteral("s draw time");
				dsc.AppendLine(ref appendInterpolatedStringHandler);
				appendInterpolatedStringHandler..ctor(12, 1, dsc);
				appendInterpolatedStringHandler.AppendFormatted<float>(archeringSettings.DrawWeight, "F2");
				appendInterpolatedStringHandler.AppendLiteral(" draw weight");
				dsc.AppendLine(ref appendInterpolatedStringHandler);
				appendInterpolatedStringHandler..ctor(14, 1, dsc);
				appendInterpolatedStringHandler.AppendFormatted<float>(archeringSettings.DrawTime / 0.65f * 100f, "F0");
				appendInterpolatedStringHandler.AppendLiteral("% arrow damage");
				dsc.AppendLine(ref appendInterpolatedStringHandler);
			}
		}

		// Token: 0x060005A8 RID: 1448 RVA: 0x00021425 File Offset: 0x0001F625
		[NullableContext(1)]
		public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot)
		{
			return ArrayExtensions.Append<WorldInteraction>(ArrayExtensions.Append<WorldInteraction>(base.GetHeldInteractionHelp(inSlot), this._ammoSelection), this._altForInteractions);
		}

		// Token: 0x060005A9 RID: 1449 RVA: 0x00021444 File Offset: 0x0001F644
		[NullableContext(1)]
		public override int GetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSelection)
		{
			ICoreClientAPI clientApi = this._clientApi;
			long? num = (clientApi != null) ? new long?(clientApi.World.Player.Entity.EntityId) : default(long?);
			long entityId = byPlayer.Entity.EntityId;
			if (!(num.GetValueOrDefault() == entityId & num != null))
			{
				return 0;
			}
			AmmoSelector ammoSelector = this._ammoSelector;
			if (ammoSelector == null)
			{
				return 0;
			}
			return ammoSelector.GetToolMode(slot, byPlayer, blockSelection);
		}

		// Token: 0x060005AA RID: 1450 RVA: 0x000214B8 File Offset: 0x0001F6B8
		[NullableContext(1)]
		public override SkillItem[] GetToolModes(ItemSlot slot, IClientPlayer forPlayer, BlockSelection blockSel)
		{
			ICoreClientAPI clientApi = this._clientApi;
			long? num = (clientApi != null) ? new long?(clientApi.World.Player.Entity.EntityId) : default(long?);
			long entityId = forPlayer.Entity.EntityId;
			if (num.GetValueOrDefault() == entityId & num != null)
			{
				AmmoSelector ammoSelector = this._ammoSelector;
				return ((ammoSelector != null) ? ammoSelector.GetToolModes(slot, forPlayer, blockSel) : null) ?? Array.Empty<SkillItem>();
			}
			return Array.Empty<SkillItem>();
		}

		// Token: 0x060005AB RID: 1451 RVA: 0x00021538 File Offset: 0x0001F738
		[NullableContext(1)]
		public override void SetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSelection, int toolMode)
		{
			ICoreClientAPI clientApi = this._clientApi;
			long? num = (clientApi != null) ? new long?(clientApi.World.Player.Entity.EntityId) : default(long?);
			long entityId = byPlayer.Entity.EntityId;
			if (num.GetValueOrDefault() == entityId & num != null)
			{
				AmmoSelector ammoSelector = this._ammoSelector;
				if (ammoSelector == null)
				{
					return;
				}
				ammoSelector.SetToolMode(slot, byPlayer, blockSelection, toolMode);
			}
		}

		// Token: 0x060005AC RID: 1452 RVA: 0x000215A9 File Offset: 0x0001F7A9
		[NullableContext(1)]
		public override void OnCreatedByCrafting(ItemSlot[] allInputslots, ItemSlot outputSlot, IRecipeBase byRecipe)
		{
			base.OnCreatedByCrafting(allInputslots, outputSlot, byRecipe);
			GeneralUtils.MarkItemStack(outputSlot);
			outputSlot.MarkDirty();
		}

		// Token: 0x060005AD RID: 1453 RVA: 0x000215C1 File Offset: 0x0001F7C1
		[NullableContext(1)]
		public override void OnHeldAttackStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, ref EnumHandHandling handling)
		{
			handling = 4;
		}

		// Token: 0x04000272 RID: 626
		private BowStats _stats;

		// Token: 0x04000273 RID: 627
		private AmmoSelector _ammoSelector;

		// Token: 0x04000274 RID: 628
		private ICoreClientAPI _clientApi;

		// Token: 0x04000275 RID: 629
		private ArcheringBowCompat _clientArcheringCompat;

		// Token: 0x04000276 RID: 630
		private WorldInteraction _altForInteractions;

		// Token: 0x04000277 RID: 631
		private WorldInteraction _ammoSelection;
	}
}
