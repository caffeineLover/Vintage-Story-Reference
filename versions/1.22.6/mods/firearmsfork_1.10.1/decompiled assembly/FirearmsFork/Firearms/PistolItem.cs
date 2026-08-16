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
	// Token: 0x02000028 RID: 40
	[NullableContext(2)]
	[Nullable(0)]
	public class PistolItem : Item, IHasWeaponLogic, IHasRangedWeaponLogic, IHasDynamicIdleAnimations, IRangedWeaponIsLoaded
	{
		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001BD RID: 445 RVA: 0x00009BFE File Offset: 0x00007DFE
		// (set) Token: 0x060001BE RID: 446 RVA: 0x00009C06 File Offset: 0x00007E06
		public PistolClient ClientLogic { get; private set; }

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001BF RID: 447 RVA: 0x00009C0F File Offset: 0x00007E0F
		// (set) Token: 0x060001C0 RID: 448 RVA: 0x00009C17 File Offset: 0x00007E17
		public PistolServer ServerLogic { get; private set; }

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x00009C20 File Offset: 0x00007E20
		// (set) Token: 0x060001C2 RID: 450 RVA: 0x00009C28 File Offset: 0x00007E28
		public AnimationRequestByCode IdleAnimation { get; private set; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00009C31 File Offset: 0x00007E31
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x00009C39 File Offset: 0x00007E39
		public AnimationRequestByCode ReadyAnimation { get; private set; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00009C42 File Offset: 0x00007E42
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x00009C4A File Offset: 0x00007E4A
		public AnimationRequestByCode IdleAnimationOffhand { get; private set; }

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00009C53 File Offset: 0x00007E53
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x00009C5B File Offset: 0x00007E5B
		public AnimationRequestByCode ReadyAnimationOffhand { get; private set; }

		// Token: 0x060001C9 RID: 457 RVA: 0x00009C64 File Offset: 0x00007E64
		[NullableContext(1)]
		public AnimationRequestByCode? GetIdleAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			return new AnimationRequestByCode?(mainHand ? this.IdleAnimation : this.IdleAnimationOffhand);
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00009C7C File Offset: 0x00007E7C
		[NullableContext(1)]
		public AnimationRequestByCode? GetReadyAnimation(EntityPlayer player, ItemSlot slot, bool mainHand)
		{
			return new AnimationRequestByCode?(mainHand ? this.ReadyAnimation : this.ReadyAnimationOffhand);
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001CB RID: 459 RVA: 0x00009C94 File Offset: 0x00007E94
		// (set) Token: 0x060001CC RID: 460 RVA: 0x00009C9C File Offset: 0x00007E9C
		public MuzzleloaderStats Stats { get; private set; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00009CA5 File Offset: 0x00007EA5
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

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001CE RID: 462 RVA: 0x00009CC9 File Offset: 0x00007EC9
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

		// Token: 0x060001CF RID: 463 RVA: 0x00009CED File Offset: 0x00007EED
		[NullableContext(1)]
		public override void OnCreatedByCrafting(ItemSlot[] allInputslots, ItemSlot outputSlot, IRecipeBase byRecipe)
		{
			base.OnCreatedByCrafting(allInputslots, outputSlot, byRecipe);
			GeneralUtils.MarkItemStack(outputSlot);
			outputSlot.MarkDirty();
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00009D08 File Offset: 0x00007F08
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

		// Token: 0x060001D1 RID: 465 RVA: 0x00009DCB File Offset: 0x00007FCB
		[NullableContext(1)]
		public bool GetIsLoaded(EntityPlayer player, ItemSlot slot)
		{
			PistolServer serverLogic = this.ServerLogic;
			return serverLogic != null && serverLogic.GetIsLoaded(player, slot);
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00009DE0 File Offset: 0x00007FE0
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

		// Token: 0x060001D3 RID: 467 RVA: 0x00009F44 File Offset: 0x00008144
		[NullableContext(1)]
		private void TryInitClientLogic(ICoreClientAPI clientAPI)
		{
			try
			{
				this.ClientLogic = new PistolClient(clientAPI, this);
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

		// Token: 0x060001D4 RID: 468 RVA: 0x00009FC4 File Offset: 0x000081C4
		[NullableContext(1)]
		private void TryInitServerLogic(ICoreServerAPI serverAPI)
		{
			try
			{
				this.ServerLogic = new PistolServer(serverAPI, this);
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

		// Token: 0x040000EF RID: 239
		private ICoreClientAPI _clientApi;

		// Token: 0x040000F0 RID: 240
		private ICoreServerAPI _serverApi;
	}
}
