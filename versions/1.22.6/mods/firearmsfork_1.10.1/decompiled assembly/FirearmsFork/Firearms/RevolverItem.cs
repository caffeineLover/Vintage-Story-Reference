using System;
using System.Runtime.CompilerServices;
using System.Text;
using CombatOverhaul.Animations;
using CombatOverhaul.Inputs;
using CombatOverhaul.RangedSystems;
using CombatOverhaul.Utils;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

namespace Firearms
{
	// Token: 0x02000030 RID: 48
	[NullableContext(2)]
	[Nullable(0)]
	public class RevolverItem : Item, IHasWeaponLogic, IHasRangedWeaponLogic, IHasDynamicIdleAnimations
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600026A RID: 618 RVA: 0x0000D1A1 File Offset: 0x0000B3A1
		// (set) Token: 0x0600026B RID: 619 RVA: 0x0000D1A9 File Offset: 0x0000B3A9
		public RevolverClient ClientLogic { get; private set; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600026C RID: 620 RVA: 0x0000D1B2 File Offset: 0x0000B3B2
		// (set) Token: 0x0600026D RID: 621 RVA: 0x0000D1BA File Offset: 0x0000B3BA
		public RevolverServer ServerLogic { get; private set; }

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600026E RID: 622 RVA: 0x0000D1C3 File Offset: 0x0000B3C3
		// (set) Token: 0x0600026F RID: 623 RVA: 0x0000D1CB File Offset: 0x0000B3CB
		public AnimationRequestByCode IdleAnimation { get; private set; }

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000270 RID: 624 RVA: 0x0000D1D4 File Offset: 0x0000B3D4
		// (set) Token: 0x06000271 RID: 625 RVA: 0x0000D1DC File Offset: 0x0000B3DC
		public AnimationRequestByCode ReadyAnimation { get; private set; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000272 RID: 626 RVA: 0x0000D1E5 File Offset: 0x0000B3E5
		// (set) Token: 0x06000273 RID: 627 RVA: 0x0000D1ED File Offset: 0x0000B3ED
		public AnimationRequestByCode IdleAnimationOffhand { get; private set; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000274 RID: 628 RVA: 0x0000D1F6 File Offset: 0x0000B3F6
		// (set) Token: 0x06000275 RID: 629 RVA: 0x0000D1FE File Offset: 0x0000B3FE
		public AnimationRequestByCode ReadyAnimationOffhand { get; private set; }

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000276 RID: 630 RVA: 0x0000D207 File Offset: 0x0000B407
		// (set) Token: 0x06000277 RID: 631 RVA: 0x0000D20F File Offset: 0x0000B40F
		public RevolverStats Stats { get; private set; }

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000278 RID: 632 RVA: 0x0000D218 File Offset: 0x0000B418
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

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000279 RID: 633 RVA: 0x0000D23C File Offset: 0x0000B43C
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

		// Token: 0x0600027A RID: 634 RVA: 0x0000D260 File Offset: 0x0000B460
		[NullableContext(1)]
		public AnimationRequestByCode? GetIdleAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			return new AnimationRequestByCode?(mainHand ? this.IdleAnimation : this.IdleAnimationOffhand);
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000D278 File Offset: 0x0000B478
		[NullableContext(1)]
		public AnimationRequestByCode? GetReadyAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			return new AnimationRequestByCode?(mainHand ? this.ReadyAnimation : this.ReadyAnimationOffhand);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000D290 File Offset: 0x0000B490
		[NullableContext(1)]
		public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
		{
			if (this.Stats != null && this.Stats.ProficiencyStat != "")
			{
				string description = Lang.Get("combatoverhaul:iteminfo-proficiency", new object[]
				{
					Lang.Get("combatoverhaul:proficiency-" + this.Stats.ProficiencyStat, Array.Empty<object>())
				});
				dsc.AppendLine(description);
			}
			if (this.Stats != null)
			{
				dsc.AppendLine(Lang.Get("combatoverhaul:iteminfo-range-weapon-damage", new object[]
				{
					this.Stats.FiringStats.BulletDamageMultiplier,
					this.Stats.FiringStats.BulletDamageTier
				}));
				dsc.AppendLine("");
			}
			base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000D360 File Offset: 0x0000B560
		[NullableContext(1)]
		public override int GetRemainingDurability(ItemStack itemstack)
		{
			int durability = base.GetRemainingDurability(itemstack);
			int maxDurability = this.GetMaxDurability(itemstack);
			if (durability > maxDurability)
			{
				itemstack.Attributes.RemoveAttribute("durability");
				return maxDurability;
			}
			return durability;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000D394 File Offset: 0x0000B594
		[NullableContext(1)]
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			ICoreClientAPI clientAPI = api as ICoreClientAPI;
			if (clientAPI != null)
			{
				this.Stats = this.Attributes.AsObject<RevolverStats>(null);
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

		// Token: 0x0600027F RID: 639 RVA: 0x0000D46E File Offset: 0x0000B66E
		[NullableContext(1)]
		public override void OnCreatedByCrafting(ItemSlot[] allInputslots, ItemSlot outputSlot, IRecipeBase byRecipe)
		{
			base.OnCreatedByCrafting(allInputslots, outputSlot, byRecipe);
			GeneralUtils.MarkItemStack(outputSlot);
			outputSlot.MarkDirty();
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000D488 File Offset: 0x0000B688
		[NullableContext(1)]
		private void TryInitClientLogic(ICoreClientAPI clientAPI)
		{
			try
			{
				this.ClientLogic = new RevolverClient(clientAPI, this);
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

		// Token: 0x06000281 RID: 641 RVA: 0x0000D508 File Offset: 0x0000B708
		[NullableContext(1)]
		private void TryInitServerLogic(ICoreServerAPI serverAPI)
		{
			try
			{
				this.ServerLogic = new RevolverServer(serverAPI, this);
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

		// Token: 0x04000150 RID: 336
		private ICoreClientAPI _clientApi;

		// Token: 0x04000151 RID: 337
		private ICoreServerAPI _serverApi;
	}
}
