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
	// Token: 0x02000016 RID: 22
	[NullableContext(2)]
	[Nullable(0)]
	public class MatchlockItem : Item, IHasWeaponLogic, IHasRangedWeaponLogic, IHasIdleAnimations
	{
		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x000049A0 File Offset: 0x00002BA0
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x000049A8 File Offset: 0x00002BA8
		public MatchlockClient ClientLogic { get; private set; }

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000B6 RID: 182 RVA: 0x000049B1 File Offset: 0x00002BB1
		// (set) Token: 0x060000B7 RID: 183 RVA: 0x000049B9 File Offset: 0x00002BB9
		public MuzzleloaderServer ServerLogic { get; private set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000B8 RID: 184 RVA: 0x000049C2 File Offset: 0x00002BC2
		// (set) Token: 0x060000B9 RID: 185 RVA: 0x000049CA File Offset: 0x00002BCA
		public AnimationRequestByCode IdleAnimation { get; private set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000BA RID: 186 RVA: 0x000049D3 File Offset: 0x00002BD3
		// (set) Token: 0x060000BB RID: 187 RVA: 0x000049DB File Offset: 0x00002BDB
		public AnimationRequestByCode ReadyAnimation { get; private set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000BC RID: 188 RVA: 0x000049E4 File Offset: 0x00002BE4
		// (set) Token: 0x060000BD RID: 189 RVA: 0x000049EC File Offset: 0x00002BEC
		public MuzzleloaderStats Stats { get; private set; }

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000BE RID: 190 RVA: 0x000049F5 File Offset: 0x00002BF5
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

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000BF RID: 191 RVA: 0x00004A19 File Offset: 0x00002C19
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

		// Token: 0x060000C0 RID: 192 RVA: 0x00004A40 File Offset: 0x00002C40
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

		// Token: 0x060000C1 RID: 193 RVA: 0x00004B04 File Offset: 0x00002D04
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

		// Token: 0x060000C2 RID: 194 RVA: 0x00004B38 File Offset: 0x00002D38
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
				this._clientApi = clientAPI;
			}
			ICoreServerAPI serverAPI = api as ICoreServerAPI;
			if (serverAPI != null)
			{
				this._serverApi = serverAPI;
				this.TryInitServerLogic(serverAPI);
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00004C12 File Offset: 0x00002E12
		[NullableContext(1)]
		public override void OnCreatedByCrafting(ItemSlot[] allInputslots, ItemSlot outputSlot, IRecipeBase byRecipe)
		{
			base.OnCreatedByCrafting(allInputslots, outputSlot, byRecipe);
			GeneralUtils.MarkItemStack(outputSlot);
			outputSlot.MarkDirty();
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00004C2C File Offset: 0x00002E2C
		[NullableContext(1)]
		private void TryInitClientLogic(ICoreClientAPI clientAPI)
		{
			try
			{
				this.ClientLogic = new MatchlockClient(clientAPI, this);
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

		// Token: 0x060000C5 RID: 197 RVA: 0x00004CAC File Offset: 0x00002EAC
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

		// Token: 0x04000059 RID: 89
		private ICoreClientAPI _clientApi;

		// Token: 0x0400005A RID: 90
		private ICoreServerAPI _serverApi;
	}
}
