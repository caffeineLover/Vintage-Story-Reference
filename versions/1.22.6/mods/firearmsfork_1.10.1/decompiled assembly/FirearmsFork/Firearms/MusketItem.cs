using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using CombatOverhaul.Animations;
using CombatOverhaul.Inputs;
using CombatOverhaul.MeleeSystems;
using CombatOverhaul.RangedSystems;
using CombatOverhaul.Utils;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace Firearms
{
	// Token: 0x0200001D RID: 29
	[NullableContext(2)]
	[Nullable(0)]
	public class MusketItem : Item, IHasWeaponLogic, IHasRangedWeaponLogic, IHasIdleAnimations, IOnGameTick, IHasServerMeleeAttacks
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00005BAE File Offset: 0x00003DAE
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x00005BB6 File Offset: 0x00003DB6
		public MusketClient ClientLogic { get; private set; }

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00005BBF File Offset: 0x00003DBF
		// (set) Token: 0x060000EA RID: 234 RVA: 0x00005BC7 File Offset: 0x00003DC7
		public MusketServer ServerLogic { get; private set; }

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00005BD0 File Offset: 0x00003DD0
		// (set) Token: 0x060000EC RID: 236 RVA: 0x00005BD8 File Offset: 0x00003DD8
		public AnimationRequestByCode IdleAnimation { get; private set; }

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00005BE1 File Offset: 0x00003DE1
		// (set) Token: 0x060000EE RID: 238 RVA: 0x00005BE9 File Offset: 0x00003DE9
		public AnimationRequestByCode ReadyAnimation { get; private set; }

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00005BF2 File Offset: 0x00003DF2
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x00005BFA File Offset: 0x00003DFA
		public MuzzleloaderStats Stats { get; private set; }

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00005C03 File Offset: 0x00003E03
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x00005C0B File Offset: 0x00003E0B
		public MusketStats StatsMusket { get; private set; }

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00005C14 File Offset: 0x00003E14
		IClientWeaponLogic IHasWeaponLogic.ClientLogic
		{
			get
			{
				if (this.ClientLogic == null && this._clientApi != null)
				{
					this.TryInitClientLogic(this._clientApi);
				}
				return this.ClientLogic;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00005C38 File Offset: 0x00003E38
		IServerRangedWeaponLogic IHasRangedWeaponLogic.ServerWeaponLogic
		{
			get
			{
				if (this.ServerLogic == null && this._serverApi != null)
				{
					this.TryInitServerLogic(this._serverApi);
				}
				return this.ServerLogic;
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00005C5C File Offset: 0x00003E5C
		[NullableContext(1)]
		public void OnGameTick(ItemSlot slot, EntityPlayer player, ref int state, bool mainHand)
		{
			MusketClient clientLogic = this.ClientLogic;
			if (clientLogic == null)
			{
				return;
			}
			clientLogic.OnGameTick(slot, player, ref state, mainHand);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00005C73 File Offset: 0x00003E73
		[NullableContext(1)]
		public IEnumerable<ServerMeleeAttackStats> GetServerMeleeAttacks(EntityPlayer attacker, Entity target, ItemSlot weaponSlot)
		{
			MusketItem.<GetServerMeleeAttacks>d__29 <GetServerMeleeAttacks>d__ = new MusketItem.<GetServerMeleeAttacks>d__29(-2);
			<GetServerMeleeAttacks>d__.<>4__this = this;
			<GetServerMeleeAttacks>d__.<>3__attacker = attacker;
			<GetServerMeleeAttacks>d__.<>3__weaponSlot = weaponSlot;
			return <GetServerMeleeAttacks>d__;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00005C94 File Offset: 0x00003E94
		[NullableContext(1)]
		public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
		{
			if (this.Stats != null)
			{
				dsc.AppendLine(Lang.Get("combatoverhaul:iteminfo-range-weapon-damage", new object[]
				{
					this.Stats.BulletDamageMultiplier,
					this.Stats.BulletDamageStrength
				}));
				dsc.AppendLine("");
			}
			base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00005D00 File Offset: 0x00003F00
		[NullableContext(1)]
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			this.StatsMusket = this.Attributes.AsObject<MusketStats>(null);
			ICoreClientAPI clientAPI = api as ICoreClientAPI;
			if (clientAPI != null)
			{
				this.Stats = this.Attributes.AsObject<MuzzleloaderStats>(null);
				this.IdleAnimation = new AnimationRequestByCode(this.Stats.IdleAnimation, 1f, 1f, "main", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null);
				this.ReadyAnimation = new AnimationRequestByCode(this.Stats.ReadyAnimation, 1f, 1f, "main", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null);
				this._clientApi = clientAPI;
			}
			ICoreServerAPI serverAPI = api as ICoreServerAPI;
			if (serverAPI != null)
			{
				this._serverApi = serverAPI;
				this.TryInitServerLogic(serverAPI);
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00005DEC File Offset: 0x00003FEC
		[NullableContext(1)]
		public override void OnHeldRenderOpaque(ItemSlot inSlot, IClientPlayer byPlayer)
		{
			FirearmRenderVariantUtil.SanitizeRenderVariant(inSlot.Itemstack);
			base.OnHeldRenderOpaque(inSlot, byPlayer);
			if (DebugWindowManager.RenderDebugColliders)
			{
				MusketClient clientLogic = this.ClientLogic;
				if (clientLogic == null)
				{
					return;
				}
				clientLogic.RenderDebugCollider(inSlot, byPlayer);
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00005E1C File Offset: 0x0000401C
		[NullableContext(1)]
		public override int GetRemainingDurability(ItemStack itemstack)
		{
			FirearmRenderVariantUtil.SanitizeRenderVariant(itemstack);
			int durability = base.GetRemainingDurability(itemstack);
			int maxDurability = this.GetMaxDurability(itemstack);
			if (durability > maxDurability)
			{
				itemstack.Attributes.RemoveAttribute("durability");
				return maxDurability;
			}
			return durability;
		}

		// Token: 0x060000FB RID: 251 RVA: 0x00005E57 File Offset: 0x00004057
		[NullableContext(1)]
		public override void OnCreatedByCrafting(ItemSlot[] allInputslots, ItemSlot outputSlot, IRecipeBase byRecipe)
		{
			base.OnCreatedByCrafting(allInputslots, outputSlot, byRecipe);
			GeneralUtils.MarkItemStack(outputSlot);
			outputSlot.MarkDirty();
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00005E70 File Offset: 0x00004070
		[NullableContext(1)]
		private void TryInitClientLogic(ICoreClientAPI clientAPI)
		{
			try
			{
				this.ClientLogic = new MusketClient(clientAPI, this);
			}
			catch (Exception exception)
			{
				ILogger logger = clientAPI.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(50, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[maltiezfirearms] Delayed client logic init for ");
				defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(this.Code);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(exception.Message);
				logger.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00005EF0 File Offset: 0x000040F0
		[NullableContext(1)]
		private void TryInitServerLogic(ICoreServerAPI serverAPI)
		{
			try
			{
				this.ServerLogic = new MusketServer(serverAPI, this);
			}
			catch (Exception exception)
			{
				ILogger logger = serverAPI.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(50, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[maltiezfirearms] Delayed server logic init for ");
				defaultInterpolatedStringHandler.AppendFormatted<AssetLocation>(this.Code);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(exception.Message);
				logger.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x04000082 RID: 130
		private ICoreClientAPI _clientApi;

		// Token: 0x04000083 RID: 131
		private ICoreServerAPI _serverApi;

		// Token: 0x04000084 RID: 132
		[Nullable(1)]
		private const string BayonetInventoryId = "bayonet";
	}
}
