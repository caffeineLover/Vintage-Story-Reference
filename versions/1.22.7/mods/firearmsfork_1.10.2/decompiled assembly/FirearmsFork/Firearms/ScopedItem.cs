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
	// Token: 0x02000032 RID: 50
	[NullableContext(2)]
	[Nullable(0)]
	public class ScopedItem : Item, IHasWeaponLogic, IHasRangedWeaponLogic, IHasDynamicIdleAnimations
	{
		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000287 RID: 647 RVA: 0x0000D88A File Offset: 0x0000BA8A
		// (set) Token: 0x06000288 RID: 648 RVA: 0x0000D892 File Offset: 0x0000BA92
		public ScopedClient ClientLogic { get; private set; }

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000289 RID: 649 RVA: 0x0000D89B File Offset: 0x0000BA9B
		// (set) Token: 0x0600028A RID: 650 RVA: 0x0000D8A3 File Offset: 0x0000BAA3
		public MuzzleloaderServer ServerLogic { get; private set; }

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600028B RID: 651 RVA: 0x0000D8AC File Offset: 0x0000BAAC
		// (set) Token: 0x0600028C RID: 652 RVA: 0x0000D8B4 File Offset: 0x0000BAB4
		public AnimationRequestByCode IdleAnimation { get; private set; }

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600028D RID: 653 RVA: 0x0000D8BD File Offset: 0x0000BABD
		// (set) Token: 0x0600028E RID: 654 RVA: 0x0000D8C5 File Offset: 0x0000BAC5
		public AnimationRequestByCode ReadyAnimation { get; private set; }

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x0600028F RID: 655 RVA: 0x0000D8CE File Offset: 0x0000BACE
		// (set) Token: 0x06000290 RID: 656 RVA: 0x0000D8D6 File Offset: 0x0000BAD6
		public AnimationRequestByCode IdleAnimationOffhand { get; private set; }

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000291 RID: 657 RVA: 0x0000D8DF File Offset: 0x0000BADF
		// (set) Token: 0x06000292 RID: 658 RVA: 0x0000D8E7 File Offset: 0x0000BAE7
		public AnimationRequestByCode ReadyAnimationOffhand { get; private set; }

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000293 RID: 659 RVA: 0x0000D8F0 File Offset: 0x0000BAF0
		// (set) Token: 0x06000294 RID: 660 RVA: 0x0000D8F8 File Offset: 0x0000BAF8
		public MuzzleloaderStats Stats { get; private set; }

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000295 RID: 661 RVA: 0x0000D901 File Offset: 0x0000BB01
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

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x06000296 RID: 662 RVA: 0x0000D925 File Offset: 0x0000BB25
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

		// Token: 0x06000297 RID: 663 RVA: 0x0000D949 File Offset: 0x0000BB49
		[NullableContext(1)]
		public AnimationRequestByCode? GetIdleAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			return new AnimationRequestByCode?(mainHand ? this.IdleAnimation : this.IdleAnimationOffhand);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000D961 File Offset: 0x0000BB61
		[NullableContext(1)]
		public AnimationRequestByCode? GetReadyAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			return new AnimationRequestByCode?(mainHand ? this.ReadyAnimation : this.ReadyAnimationOffhand);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000D97C File Offset: 0x0000BB7C
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

		// Token: 0x0600029A RID: 666 RVA: 0x0000D9E8 File Offset: 0x0000BBE8
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

		// Token: 0x0600029B RID: 667 RVA: 0x0000DB4A File Offset: 0x0000BD4A
		[NullableContext(1)]
		public override void OnCreatedByCrafting(ItemSlot[] allInputslots, ItemSlot outputSlot, IRecipeBase byRecipe)
		{
			base.OnCreatedByCrafting(allInputslots, outputSlot, byRecipe);
			GeneralUtils.MarkItemStack(outputSlot);
			outputSlot.MarkDirty();
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000DB64 File Offset: 0x0000BD64
		[NullableContext(1)]
		private void TryInitClientLogic(ICoreClientAPI clientAPI)
		{
			try
			{
				this.ClientLogic = new ScopedClient(clientAPI, this);
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

		// Token: 0x0600029D RID: 669 RVA: 0x0000DBE4 File Offset: 0x0000BDE4
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

		// Token: 0x0400015E RID: 350
		private ICoreClientAPI _clientApi;

		// Token: 0x0400015F RID: 351
		private ICoreServerAPI _serverApi;
	}
}
