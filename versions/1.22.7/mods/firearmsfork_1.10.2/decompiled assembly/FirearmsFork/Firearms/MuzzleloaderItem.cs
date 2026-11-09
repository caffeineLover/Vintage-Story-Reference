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
	// Token: 0x02000023 RID: 35
	[NullableContext(2)]
	[Nullable(0)]
	public class MuzzleloaderItem : Item, IHasWeaponLogic, IHasRangedWeaponLogic, IHasDynamicIdleAnimations
	{
		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000196 RID: 406 RVA: 0x0000944A File Offset: 0x0000764A
		// (set) Token: 0x06000197 RID: 407 RVA: 0x00009452 File Offset: 0x00007652
		public MuzzleloaderClient ClientLogic { get; private set; }

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000198 RID: 408 RVA: 0x0000945B File Offset: 0x0000765B
		// (set) Token: 0x06000199 RID: 409 RVA: 0x00009463 File Offset: 0x00007663
		public MuzzleloaderServer ServerLogic { get; private set; }

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600019A RID: 410 RVA: 0x0000946C File Offset: 0x0000766C
		// (set) Token: 0x0600019B RID: 411 RVA: 0x00009474 File Offset: 0x00007674
		public AnimationRequestByCode IdleAnimation { get; private set; }

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600019C RID: 412 RVA: 0x0000947D File Offset: 0x0000767D
		// (set) Token: 0x0600019D RID: 413 RVA: 0x00009485 File Offset: 0x00007685
		public AnimationRequestByCode ReadyAnimation { get; private set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600019E RID: 414 RVA: 0x0000948E File Offset: 0x0000768E
		// (set) Token: 0x0600019F RID: 415 RVA: 0x00009496 File Offset: 0x00007696
		public AnimationRequestByCode IdleAnimationOffhand { get; private set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x0000949F File Offset: 0x0000769F
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x000094A7 File Offset: 0x000076A7
		public AnimationRequestByCode ReadyAnimationOffhand { get; private set; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x000094B0 File Offset: 0x000076B0
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x000094B8 File Offset: 0x000076B8
		public MuzzleloaderStats Stats { get; private set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x000094C1 File Offset: 0x000076C1
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

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001A5 RID: 421 RVA: 0x000094E5 File Offset: 0x000076E5
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

		// Token: 0x060001A6 RID: 422 RVA: 0x00009509 File Offset: 0x00007709
		[NullableContext(1)]
		public AnimationRequestByCode? GetIdleAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			return new AnimationRequestByCode?(mainHand ? this.IdleAnimation : this.IdleAnimationOffhand);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00009521 File Offset: 0x00007721
		[NullableContext(1)]
		public AnimationRequestByCode? GetReadyAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			return new AnimationRequestByCode?(mainHand ? this.ReadyAnimation : this.ReadyAnimationOffhand);
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000953C File Offset: 0x0000773C
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
					this.Stats.BulletDamageMultiplier,
					this.Stats.BulletDamageStrength
				}));
				dsc.AppendLine("");
			}
			base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00009600 File Offset: 0x00007800
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

		// Token: 0x060001AA RID: 426 RVA: 0x00009634 File Offset: 0x00007834
		[NullableContext(1)]
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
			ICoreClientAPI clientAPI = api as ICoreClientAPI;
			if (clientAPI != null)
			{
				this.Stats = this.Attributes.AsObject<MuzzleloaderStats>(null);
				this.IdleAnimation = new AnimationRequestByCode(this.Stats.IdleAnimation, 1f, 1f, "main", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null);
				this.ReadyAnimation = new AnimationRequestByCode(this.Stats.ReadyAnimation, 1f, 1f, "main", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null);
				this.IdleAnimationOffhand = new AnimationRequestByCode(this.Stats.IdleAnimationOffhand, 1f, 1f, "mainOffhand", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null);
				this.ReadyAnimationOffhand = new AnimationRequestByCode(this.Stats.ReadyAnimationOffhand, 1f, 1f, "mainOffhand", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), false, null, null);
				this._clientApi = clientAPI;
			}
			ICoreServerAPI serverAPI = api as ICoreServerAPI;
			if (serverAPI != null)
			{
				this._serverApi = serverAPI;
				this.TryInitServerLogic(serverAPI);
			}
		}

		// Token: 0x060001AB RID: 427 RVA: 0x00009796 File Offset: 0x00007996
		[NullableContext(1)]
		public override void OnCreatedByCrafting(ItemSlot[] allInputslots, ItemSlot outputSlot, IRecipeBase byRecipe)
		{
			base.OnCreatedByCrafting(allInputslots, outputSlot, byRecipe);
			GeneralUtils.MarkItemStack(outputSlot);
			outputSlot.MarkDirty();
		}

		// Token: 0x060001AC RID: 428 RVA: 0x000097B0 File Offset: 0x000079B0
		[NullableContext(1)]
		private void TryInitClientLogic(ICoreClientAPI clientAPI)
		{
			try
			{
				this.ClientLogic = new MuzzleloaderClient(clientAPI, this);
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

		// Token: 0x060001AD RID: 429 RVA: 0x00009830 File Offset: 0x00007A30
		[NullableContext(1)]
		private void TryInitServerLogic(ICoreServerAPI serverAPI)
		{
			try
			{
				this.ServerLogic = new MuzzleloaderServer(serverAPI, this);
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

		// Token: 0x040000E3 RID: 227
		private ICoreClientAPI _clientApi;

		// Token: 0x040000E4 RID: 228
		private ICoreServerAPI _serverApi;
	}
}
