using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CombatOverhaul.Animations;
using CombatOverhaul.DamageSystems;
using CombatOverhaul.Inputs;
using CombatOverhaul.Integration;
using CombatOverhaul.RangedSystems;
using CombatOverhaul.WeaponBuffs;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace CombatOverhaul.Implementations
{
	// Token: 0x0200007A RID: 122
	[NullableContext(1)]
	[Nullable(0)]
	public class MeleeWeaponBehavior : CollectibleBehavior, IHasMultipleWeaponLogicModes, IHasWeaponLogic, IHasRangedWeaponLogic, IHasDynamicMoveAnimations, IHasDynamicIdleAnimations, IHasMeleeWeaponActions, IHasServerBlockCallback, ISetsRenderingOffset, IMouseWheelInput, IOnGameTick, IRestrictAction
	{
		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060005BC RID: 1468 RVA: 0x00021B17 File Offset: 0x0001FD17
		[Nullable(2)]
		public MeleeWeaponClient ClientLogic
		{
			[NullableContext(2)]
			get
			{
				MeleeWeaponClientModesCollection clientModes = this.ClientModes;
				if (clientModes == null)
				{
					return null;
				}
				return clientModes.CurrentMode;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060005BD RID: 1469 RVA: 0x00021B2A File Offset: 0x0001FD2A
		// (set) Token: 0x060005BE RID: 1470 RVA: 0x00021B32 File Offset: 0x0001FD32
		[Nullable(2)]
		public MeleeWeaponServer ServerLogic { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060005BF RID: 1471 RVA: 0x00021B3B File Offset: 0x0001FD3B
		// (set) Token: 0x060005C0 RID: 1472 RVA: 0x00021B43 File Offset: 0x0001FD43
		[Nullable(2)]
		public MeleeWeaponClientModesCollection ClientModes { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060005C1 RID: 1473 RVA: 0x00021B4C File Offset: 0x0001FD4C
		[Nullable(2)]
		IClientWeaponLogic IHasWeaponLogic.ClientLogic
		{
			[NullableContext(2)]
			get
			{
				return this.ClientLogic;
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060005C2 RID: 1474 RVA: 0x00021B54 File Offset: 0x0001FD54
		IEnumerable<IClientWeaponLogic> IHasMultipleWeaponLogicModes.ClientLogicModes
		{
			get
			{
				MeleeWeaponClientModesCollection clientModes = this.ClientModes;
				IEnumerable<IClientWeaponLogic> enumerable;
				if (clientModes == null)
				{
					enumerable = null;
				}
				else
				{
					enumerable = Enumerable.Select<KeyValuePair<string, MeleeWeaponClient>, MeleeWeaponClient>(clientModes.Clients, (KeyValuePair<string, MeleeWeaponClient> entry) => entry.Value);
				}
				return enumerable ?? Array.Empty<MeleeWeaponClient>();
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060005C3 RID: 1475 RVA: 0x00021BA0 File Offset: 0x0001FDA0
		[Nullable(2)]
		IServerRangedWeaponLogic IHasRangedWeaponLogic.ServerWeaponLogic
		{
			[NullableContext(2)]
			get
			{
				return this.ServerLogic;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060005C4 RID: 1476 RVA: 0x00021BA8 File Offset: 0x0001FDA8
		// (set) Token: 0x060005C5 RID: 1477 RVA: 0x00021BB0 File Offset: 0x0001FDB0
		public bool RenderingOffset { get; set; }

		// Token: 0x060005C6 RID: 1478 RVA: 0x00021BB9 File Offset: 0x0001FDB9
		public MeleeWeaponBehavior(CollectibleObject collObj) : base(collObj)
		{
			Item item = collObj as Item;
			if (item == null)
			{
				throw new Exception("must be an item");
			}
			this.Item = item;
		}

		// Token: 0x060005C7 RID: 1479 RVA: 0x00021BDD File Offset: 0x0001FDDD
		public bool RestrictRightHandAction()
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			return clientLogic != null && clientLogic.RestrictRightHandAction();
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00021BF0 File Offset: 0x0001FDF0
		public bool RestrictLeftHandAction()
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			return clientLogic != null && clientLogic.RestrictLeftHandAction();
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00021C04 File Offset: 0x0001FE04
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			ICoreClientAPI clientAPI = api as ICoreClientAPI;
			if (clientAPI != null)
			{
				this.LoadClientSide(clientAPI);
			}
			ICoreServerAPI serverAPI = api as ICoreServerAPI;
			if (serverAPI != null)
			{
				this.LoadServerSide(serverAPI);
			}
			this.AltForInteractions = new WorldInteraction
			{
				MouseButton = 255,
				HotKeyCode = "Alt",
				ActionLangCode = "combatoverhaul:interaction-hold-alt"
			};
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00021C68 File Offset: 0x0001FE68
		public AnimationRequestByCode? GetIdleAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			if (clientLogic == null)
			{
				return default(AnimationRequestByCode?);
			}
			return clientLogic.GetIdleAnimation(player, slot, mainHand);
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00021C94 File Offset: 0x0001FE94
		public AnimationRequestByCode? GetReadyAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			if (clientLogic == null)
			{
				return default(AnimationRequestByCode?);
			}
			return clientLogic.GetReadyAnimation(player, slot, mainHand);
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00021CC0 File Offset: 0x0001FEC0
		public AnimationRequestByCode? GetWalkAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			if (clientLogic == null)
			{
				return default(AnimationRequestByCode?);
			}
			return clientLogic.GetWalkAnimation(player, slot, mainHand);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00021CEC File Offset: 0x0001FEEC
		public AnimationRequestByCode? GetRunAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			if (clientLogic == null)
			{
				return default(AnimationRequestByCode?);
			}
			return clientLogic.GetRunAnimation(player, slot, mainHand);
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x00021D18 File Offset: 0x0001FF18
		public AnimationRequestByCode? GetSwimAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			if (clientLogic == null)
			{
				return default(AnimationRequestByCode?);
			}
			return clientLogic.GetSwimAnimation(player, slot, mainHand);
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x00021D44 File Offset: 0x0001FF44
		public AnimationRequestByCode? GetSwimIdleAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			if (clientLogic == null)
			{
				return default(AnimationRequestByCode?);
			}
			return clientLogic.GetSwimIdleAnimation(player, slot, mainHand);
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x00021D6D File Offset: 0x0001FF6D
		public bool CanAttack(EntityPlayer player, bool mainHand)
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			return clientLogic != null && clientLogic.CanAttack(player, mainHand);
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x00021D82 File Offset: 0x0001FF82
		public bool CanBlock(EntityPlayer player, bool mainHand)
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			if (clientLogic == null || !clientLogic.CanBlock(player, mainHand))
			{
				MeleeWeaponClient clientLogic2 = this.ClientLogic;
				return clientLogic2 != null && clientLogic2.CanParry(player, mainHand);
			}
			return true;
		}

		// Token: 0x060005D2 RID: 1490 RVA: 0x00021DAF File Offset: 0x0001FFAF
		public bool CanThrow(EntityPlayer player, bool mainHand)
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			return clientLogic != null && clientLogic.CanThrow(player, mainHand);
		}

		// Token: 0x060005D3 RID: 1491 RVA: 0x00021DC4 File Offset: 0x0001FFC4
		public void OnGameTick(ItemSlot slot, EntityPlayer player, ref int state, bool mainHand)
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			if (clientLogic == null)
			{
				return;
			}
			clientLogic.OnGameTick(slot, player, ref state, mainHand);
		}

		// Token: 0x060005D4 RID: 1492 RVA: 0x00021DDB File Offset: 0x0001FFDB
		public override void OnHeldAttackStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, ref EnumHandHandling handHandling, ref EnumHandling handling)
		{
			handHandling = 3;
			handling = 3;
		}

		// Token: 0x060005D5 RID: 1493 RVA: 0x00021DE5 File Offset: 0x0001FFE5
		public override float OnBlockBreaking(IPlayer player, BlockSelection blockSel, ItemSlot itemslot, float remainingResistance, float dt, int counter, ref EnumHandling handled)
		{
			return remainingResistance;
		}

		// Token: 0x060005D6 RID: 1494 RVA: 0x00021DE9 File Offset: 0x0001FFE9
		public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			if (clientLogic != null)
			{
				clientLogic.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
			}
			WeaponBuffSystem.AppendTooltip(inSlot.Itemstack, dsc, world, withDebugInfo);
			dsc.AppendLine("");
			base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
		}

		// Token: 0x060005D7 RID: 1495 RVA: 0x00021E28 File Offset: 0x00020028
		[return: Nullable(2)]
		public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot, ref EnumHandling handling)
		{
			handling = 1;
			WorldInteraction[] interactionHelp = Array.Empty<WorldInteraction>();
			if (this.ChangeGripInteraction != null)
			{
				interactionHelp = ((interactionHelp != null) ? ArrayExtensions.Append<WorldInteraction>(interactionHelp, this.ChangeGripInteraction) : null);
			}
			MeleeWeaponClientModesCollection clientModes = this.ClientModes;
			if (clientModes != null && clientModes.Modes.Count > 1)
			{
				interactionHelp = ((interactionHelp != null) ? ArrayExtensions.Append<WorldInteraction>(interactionHelp, this.ModesSelectionInteraction) : null);
			}
			if (interactionHelp == null)
			{
				return null;
			}
			return ArrayExtensions.Append<WorldInteraction>(interactionHelp, this.AltForInteractions);
		}

		// Token: 0x060005D8 RID: 1496 RVA: 0x00021E99 File Offset: 0x00020099
		public virtual void BlockCallback(IServerPlayer player, ItemSlot slot, bool mainHand, float damageBlocked, int attackTier, int blockTier)
		{
			this.Item.DamageItem(player.Entity.World, player.Entity, slot, 1, true);
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00021EBA File Offset: 0x000200BA
		public virtual bool OnMouseWheel(ItemSlot slot, IClientPlayer byPlayer, float delta)
		{
			MeleeWeaponClient clientLogic = this.ClientLogic;
			return clientLogic != null && clientLogic.OnMouseWheel(slot, byPlayer, delta);
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00021ED0 File Offset: 0x000200D0
		public override int GetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSelection)
		{
			MeleeWeaponClientModesCollection clientModes = this.ClientModes;
			if (clientModes == null)
			{
				return 0;
			}
			return clientModes.GetToolMode(byPlayer.Entity, slot);
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00021EEC File Offset: 0x000200EC
		[return: Nullable(new byte[]
		{
			2,
			1
		})]
		public override SkillItem[] GetToolModes(ItemSlot slot, IClientPlayer forPlayer, BlockSelection blockSel)
		{
			ICoreClientAPI clientApi = this.ClientApi;
			long? num = (clientApi != null) ? new long?(clientApi.World.Player.Entity.EntityId) : default(long?);
			long entityId = forPlayer.Entity.EntityId;
			if ((num.GetValueOrDefault() == entityId & num != null) && this.ClientModes != null)
			{
				return this.ClientModes.GetToolModes(forPlayer.Entity, slot);
			}
			return null;
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00021F64 File Offset: 0x00020164
		public override void SetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSelection, int toolMode)
		{
			ICoreClientAPI clientApi = this.ClientApi;
			long? num = (clientApi != null) ? new long?(clientApi.World.Player.Entity.EntityId) : default(long?);
			long entityId = byPlayer.Entity.EntityId;
			if ((num.GetValueOrDefault() == entityId & num != null) && this.ClientModes != null)
			{
				this.ClientModes.SetToolMode(byPlayer.Entity, slot, toolMode);
			}
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00021FDC File Offset: 0x000201DC
		protected virtual void LoadClientSide(ICoreClientAPI clientAPI)
		{
			this.ClientApi = clientAPI;
			this.ClientModes = new MeleeWeaponClientModesCollection(clientAPI, this.Item);
			MeleeWeaponStats Stats = this.Item.Attributes.AsObject<MeleeWeaponStats>(null);
			this.RenderingOffset = Stats.RenderingOffset;
			StanceStats oneHandedStance = Stats.OneHandedStance;
			float? num = (oneHandedStance != null) ? new float?(oneHandedStance.GripMinLength) : default(float?);
			StanceStats oneHandedStance2 = Stats.OneHandedStance;
			float? num2 = (oneHandedStance2 != null) ? new float?(oneHandedStance2.GripMaxLength) : default(float?);
			if (num.GetValueOrDefault() == num2.GetValueOrDefault() & num != null == (num2 != null))
			{
				StanceStats twoHandedStance = Stats.TwoHandedStance;
				num2 = ((twoHandedStance != null) ? new float?(twoHandedStance.GripMinLength) : default(float?));
				StanceStats twoHandedStance2 = Stats.TwoHandedStance;
				num = ((twoHandedStance2 != null) ? new float?(twoHandedStance2.GripMaxLength) : default(float?));
				if (num2.GetValueOrDefault() == num.GetValueOrDefault() & num2 != null == (num != null))
				{
					goto IL_11B;
				}
			}
			this.ChangeGripInteraction = new WorldInteraction
			{
				MouseButton = 13,
				ActionLangCode = "combatoverhaul:interaction-grip-change"
			};
			IL_11B:
			this.ModesSelectionInteraction = new WorldInteraction
			{
				ActionLangCode = Lang.Get("combatoverhaul:interaction-mode-selection", Array.Empty<object>()),
				HotKeyCodes = new string[]
				{
					"toolmodeselect"
				},
				MouseButton = 255
			};
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0002214A File Offset: 0x0002034A
		protected virtual void LoadServerSide(ICoreServerAPI serverAPI)
		{
			this.ServerLogic = new MeleeWeaponServer(serverAPI, this.Item);
		}

		// Token: 0x0400027F RID: 639
		[Nullable(2)]
		protected WorldInteraction AltForInteractions;

		// Token: 0x04000280 RID: 640
		[Nullable(2)]
		protected WorldInteraction ChangeGripInteraction;

		// Token: 0x04000281 RID: 641
		[Nullable(2)]
		protected WorldInteraction ModesSelectionInteraction;

		// Token: 0x04000282 RID: 642
		[Nullable(2)]
		protected ICoreClientAPI ClientApi;

		// Token: 0x04000283 RID: 643
		protected Item Item;
	}
}
