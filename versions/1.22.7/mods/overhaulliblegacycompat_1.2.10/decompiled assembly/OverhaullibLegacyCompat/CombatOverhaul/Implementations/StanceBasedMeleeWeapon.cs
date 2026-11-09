using System;
using System.Runtime.CompilerServices;
using System.Text;
using CombatOverhaul.Animations;
using CombatOverhaul.DamageSystems;
using CombatOverhaul.Inputs;
using CombatOverhaul.Integration;
using CombatOverhaul.Utils;
using CombatOverhaul.WeaponBuffs;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace CombatOverhaul.Implementations
{
	// Token: 0x02000096 RID: 150
	[NullableContext(1)]
	[Nullable(0)]
	public class StanceBasedMeleeWeapon : Item, IHasWeaponLogic, IHasDynamicIdleAnimations, IHasMeleeWeaponActions, IHasServerBlockCallback, ISetsRenderingOffset, IMouseWheelInput, IHeldItemOnMouseWheel, IOnGameTick, IRestrictAction
	{
		// Token: 0x170001BA RID: 442
		// (get) Token: 0x0600081F RID: 2079 RVA: 0x0002F9AB File Offset: 0x0002DBAB
		// (set) Token: 0x06000820 RID: 2080 RVA: 0x0002F9B3 File Offset: 0x0002DBB3
		[Nullable(2)]
		public StanceBasedMeleeWeaponClient ClientLogic { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x0002F9BC File Offset: 0x0002DBBC
		[Nullable(2)]
		IClientWeaponLogic IHasWeaponLogic.ClientLogic
		{
			[NullableContext(2)]
			get
			{
				return this.ClientLogic;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000822 RID: 2082 RVA: 0x0002F9C4 File Offset: 0x0002DBC4
		// (set) Token: 0x06000823 RID: 2083 RVA: 0x0002F9CC File Offset: 0x0002DBCC
		public bool RenderingOffset { get; set; }

		// Token: 0x06000824 RID: 2084 RVA: 0x0002F9D5 File Offset: 0x0002DBD5
		public bool RestrictRightHandAction()
		{
			StanceBasedMeleeWeaponClient clientLogic = this.ClientLogic;
			return clientLogic != null && clientLogic.RestrictRightHandAction();
		}

		// Token: 0x06000825 RID: 2085 RVA: 0x0002F9E8 File Offset: 0x0002DBE8
		public bool RestrictLeftHandAction()
		{
			StanceBasedMeleeWeaponClient clientLogic = this.ClientLogic;
			return clientLogic != null && clientLogic.RestrictLeftHandAction();
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x0002F9FC File Offset: 0x0002DBFC
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			ICoreClientAPI clientAPI = api as ICoreClientAPI;
			if (clientAPI != null)
			{
				this.ClientApi = clientAPI;
				this.ClientLogic = new StanceBasedMeleeWeaponClient(clientAPI, this);
				StanceBasedMeleeWeaponStats Stats = this.Attributes.AsObject<StanceBasedMeleeWeaponStats>(null);
				this.RenderingOffset = Stats.RenderingOffset;
				this.ChangeGripInteraction = new WorldInteraction
				{
					MouseButton = 13,
					ActionLangCode = "combatoverhaul:interaction-grip-change"
				};
			}
			this.AltForInteractions = new WorldInteraction
			{
				MouseButton = 255,
				HotKeyCode = "Alt",
				ActionLangCode = "combatoverhaul:interaction-hold-alt"
			};
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x0002FA94 File Offset: 0x0002DC94
		public AnimationRequestByCode? GetIdleAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			StanceBasedMeleeWeaponClient clientLogic = this.ClientLogic;
			if (clientLogic == null)
			{
				return default(AnimationRequestByCode?);
			}
			return clientLogic.GetIdleAnimation(player, slot, mainHand);
		}

		// Token: 0x06000828 RID: 2088 RVA: 0x0002FAC0 File Offset: 0x0002DCC0
		public AnimationRequestByCode? GetReadyAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			StanceBasedMeleeWeaponClient clientLogic = this.ClientLogic;
			if (clientLogic == null)
			{
				return default(AnimationRequestByCode?);
			}
			return clientLogic.GetReadyAnimation(player, slot, mainHand);
		}

		// Token: 0x06000829 RID: 2089 RVA: 0x0002FAE9 File Offset: 0x0002DCE9
		public override void OnHeldRenderOpaque(ItemSlot inSlot, IClientPlayer byPlayer)
		{
			base.OnHeldRenderOpaque(inSlot, byPlayer);
			if (DebugWindowManager.RenderDebugColliders)
			{
				StanceBasedMeleeWeaponClient clientLogic = this.ClientLogic;
				if (clientLogic == null)
				{
					return;
				}
				clientLogic.RenderDebugCollider(inSlot, byPlayer);
			}
		}

		// Token: 0x0600082A RID: 2090 RVA: 0x0002FB0C File Offset: 0x0002DD0C
		public bool CanAttack(EntityPlayer player, bool mainHand)
		{
			return true;
		}

		// Token: 0x0600082B RID: 2091 RVA: 0x0002FB0F File Offset: 0x0002DD0F
		public bool CanBlock(EntityPlayer player, bool mainHand)
		{
			return true;
		}

		// Token: 0x0600082C RID: 2092 RVA: 0x0002FB12 File Offset: 0x0002DD12
		public bool CanThrow(EntityPlayer player, bool mainHand)
		{
			return false;
		}

		// Token: 0x0600082D RID: 2093 RVA: 0x0002FB15 File Offset: 0x0002DD15
		public void OnGameTick(ItemSlot slot, EntityPlayer player, ref int state, bool mainHand)
		{
			StanceBasedMeleeWeaponClient clientLogic = this.ClientLogic;
			if (clientLogic == null)
			{
				return;
			}
			clientLogic.OnGameTick(slot, player, ref state, mainHand);
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0002FB2C File Offset: 0x0002DD2C
		public override void OnHeldAttackStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, ref EnumHandHandling handling)
		{
			handling = 3;
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x0002FB32 File Offset: 0x0002DD32
		public override float OnBlockBreaking(IPlayer player, BlockSelection blockSel, ItemSlot itemslot, float remainingResistance, float dt, int counter)
		{
			return remainingResistance;
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x0002FB38 File Offset: 0x0002DD38
		public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
		{
			SplitMaterialWeaponUtil.Normalize(inSlot.Itemstack);
			SplitMaterialWeaponUtil.NormalizeDurabilityIfNeeded(inSlot.Itemstack);
			StanceBasedMeleeWeaponClient clientLogic = this.ClientLogic;
			if (clientLogic != null)
			{
				clientLogic.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
			}
			WeaponBuffSystem.AppendTooltip(inSlot.Itemstack, dsc, world, withDebugInfo);
			SplitMaterialWeaponUtil.AppendTooltip(inSlot.Itemstack, dsc);
			dsc.AppendLine("");
			base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0002FBA4 File Offset: 0x0002DDA4
		public override string GetHeldItemName(ItemStack itemStack)
		{
			return SplitMaterialWeaponUtil.GetHeldItemName(itemStack, base.GetHeldItemName(itemStack));
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0002FBB3 File Offset: 0x0002DDB3
		public void BlockCallback(IServerPlayer player, ItemSlot slot, bool mainHand, float damageBlocked, int attackTier, int blockTier)
		{
			this.DamageItem(player.Entity.World, player.Entity, slot, 1, true);
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x0002FBCF File Offset: 0x0002DDCF
		public bool OnMouseWheel(ItemSlot slot, IClientPlayer byPlayer, float delta)
		{
			StanceBasedMeleeWeaponClient clientLogic = this.ClientLogic;
			return clientLogic != null && clientLogic.OnMouseWheel(slot, byPlayer, delta);
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x0002FBE8 File Offset: 0x0002DDE8
		public void OnMouseWheel(EntityPlayer byPlayer, ItemSlot inSlot, MouseWheelEventArgs args)
		{
			ICoreClientAPI clientApi = this.ClientApi;
			if (((clientApi != null) ? clientApi.World.Player : null) == null)
			{
				return;
			}
			float delta = MouseWheelPatch.GetDelta(args);
			if (delta == 0f)
			{
				return;
			}
			if (this.OnMouseWheel(inSlot, this.ClientApi.World.Player, delta))
			{
				args.SetHandled(true);
			}
		}

		// Token: 0x040003B6 RID: 950
		[Nullable(2)]
		protected WorldInteraction AltForInteractions;

		// Token: 0x040003B7 RID: 951
		[Nullable(2)]
		protected WorldInteraction ChangeGripInteraction;

		// Token: 0x040003B8 RID: 952
		[Nullable(2)]
		protected ICoreClientAPI ClientApi;
	}
}
