using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CombatOverhaul.Animations;
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
	// Token: 0x0200008E RID: 142
	[NullableContext(2)]
	[Nullable(0)]
	public class SlingItem : Item, IHasWeaponLogic, IHasRangedWeaponLogic, IHasMoveAnimations, IHasIdleAnimations
	{
		// Token: 0x17000181 RID: 385
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x0002D456 File Offset: 0x0002B656
		// (set) Token: 0x0600078C RID: 1932 RVA: 0x0002D45E File Offset: 0x0002B65E
		public SlingClient ClientLogic { get; private set; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x0600078D RID: 1933 RVA: 0x0002D467 File Offset: 0x0002B667
		// (set) Token: 0x0600078E RID: 1934 RVA: 0x0002D46F File Offset: 0x0002B66F
		public SlingServer ServerLogic { get; private set; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x0600078F RID: 1935 RVA: 0x0002D478 File Offset: 0x0002B678
		IClientWeaponLogic IHasWeaponLogic.ClientLogic
		{
			get
			{
				return this.ClientLogic;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000790 RID: 1936 RVA: 0x0002D480 File Offset: 0x0002B680
		IServerRangedWeaponLogic IHasRangedWeaponLogic.ServerWeaponLogic
		{
			get
			{
				return this.ServerLogic;
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000791 RID: 1937 RVA: 0x0002D488 File Offset: 0x0002B688
		// (set) Token: 0x06000792 RID: 1938 RVA: 0x0002D490 File Offset: 0x0002B690
		public AnimationRequestByCode IdleAnimation { get; set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x0002D499 File Offset: 0x0002B699
		// (set) Token: 0x06000794 RID: 1940 RVA: 0x0002D4A1 File Offset: 0x0002B6A1
		public AnimationRequestByCode ReadyAnimation { get; set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000795 RID: 1941 RVA: 0x0002D4AA File Offset: 0x0002B6AA
		// (set) Token: 0x06000796 RID: 1942 RVA: 0x0002D4B2 File Offset: 0x0002B6B2
		public AnimationRequestByCode WalkAnimation { get; set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000797 RID: 1943 RVA: 0x0002D4BB File Offset: 0x0002B6BB
		// (set) Token: 0x06000798 RID: 1944 RVA: 0x0002D4C3 File Offset: 0x0002B6C3
		public AnimationRequestByCode RunAnimation { get; set; }

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000799 RID: 1945 RVA: 0x0002D4CC File Offset: 0x0002B6CC
		// (set) Token: 0x0600079A RID: 1946 RVA: 0x0002D4D4 File Offset: 0x0002B6D4
		public AnimationRequestByCode SwimAnimation { get; set; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x0600079B RID: 1947 RVA: 0x0002D4DD File Offset: 0x0002B6DD
		// (set) Token: 0x0600079C RID: 1948 RVA: 0x0002D4E5 File Offset: 0x0002B6E5
		public AnimationRequestByCode SwimIdleAnimation { get; set; }

		// Token: 0x0600079D RID: 1949 RVA: 0x0002D4F0 File Offset: 0x0002B6F0
		[NullableContext(1)]
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			ICoreClientAPI clientAPI = api as ICoreClientAPI;
			if (clientAPI != null)
			{
				SlingStats stats = this.Attributes.AsObject<SlingStats>(null);
				this.IdleAnimation = new AnimationRequestByCode(stats.IdleAnimation, 1f, 1f, "main", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null);
				this.ReadyAnimation = new AnimationRequestByCode(stats.ReadyAnimation, 1f, 1f, "main", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null);
				this.WalkAnimation = new AnimationRequestByCode(stats.WalkAnimation, 1f, 1f, "main", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null);
				this.RunAnimation = new AnimationRequestByCode(stats.RunAnimation, 1f, 1f, "main", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null);
				this.SwimAnimation = new AnimationRequestByCode(stats.SwimAnimation, 1f, 1f, "main", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null);
				this.SwimIdleAnimation = new AnimationRequestByCode(stats.SwimIdleAnimation, 1f, 1f, "main", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null);
				this._stats = stats;
				this._ammoSelector = new AmmoSelector(clientAPI, this._stats.BulletWildcard);
				this._clientApi = clientAPI;
				this.ClientLogic = new SlingClient(clientAPI, this, this._ammoSelector);
			}
			ICoreServerAPI serverAPI = api as ICoreServerAPI;
			if (serverAPI != null)
			{
				this.ServerLogic = new SlingServer(serverAPI, this);
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

		// Token: 0x0600079E RID: 1950 RVA: 0x0002D754 File Offset: 0x0002B954
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
			defaultInterpolatedStringHandler.AppendFormatted<float>(this._stats.BulletDamageMultiplier * stackStats.DispersionMultiplier, "F1");
			array[num] = defaultInterpolatedStringHandler.ToStringAndClear();
			array[1] = this._stats.BulletDamageTier + stackStats.DamageTierBonus;
			dsc.AppendLine(Lang.Get(text, array));
		}

		// Token: 0x0600079F RID: 1951 RVA: 0x0002D8A2 File Offset: 0x0002BAA2
		[NullableContext(1)]
		public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot)
		{
			return ArrayExtensions.Append<WorldInteraction>(ArrayExtensions.Append<WorldInteraction>(base.GetHeldInteractionHelp(inSlot), this._ammoSelection), this._altForInteractions);
		}

		// Token: 0x060007A0 RID: 1952 RVA: 0x0002D8C4 File Offset: 0x0002BAC4
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

		// Token: 0x060007A1 RID: 1953 RVA: 0x0002D938 File Offset: 0x0002BB38
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

		// Token: 0x060007A2 RID: 1954 RVA: 0x0002D9B8 File Offset: 0x0002BBB8
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

		// Token: 0x060007A3 RID: 1955 RVA: 0x0002DA29 File Offset: 0x0002BC29
		[NullableContext(1)]
		public override void OnCreatedByCrafting(ItemSlot[] allInputslots, ItemSlot outputSlot, IRecipeBase byRecipe)
		{
			base.OnCreatedByCrafting(allInputslots, outputSlot, byRecipe);
			GeneralUtils.MarkItemStack(outputSlot);
			outputSlot.MarkDirty();
		}

		// Token: 0x060007A4 RID: 1956 RVA: 0x0002DA41 File Offset: 0x0002BC41
		[NullableContext(1)]
		public override void OnHeldAttackStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, ref EnumHandHandling handling)
		{
			handling = 4;
		}

		// Token: 0x04000369 RID: 873
		private SlingStats _stats;

		// Token: 0x0400036A RID: 874
		private AmmoSelector _ammoSelector;

		// Token: 0x0400036B RID: 875
		private ICoreClientAPI _clientApi;

		// Token: 0x0400036C RID: 876
		private WorldInteraction _altForInteractions;

		// Token: 0x0400036D RID: 877
		private WorldInteraction _ammoSelection;
	}
}
