using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Animations;
using CombatOverhaul.Inputs;
using CombatOverhaul.RangedSystems;
using CombatOverhaul.RangedSystems.Aiming;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace Firearms
{
	// Token: 0x02000031 RID: 49
	[NullableContext(1)]
	[Nullable(0)]
	public class ScopedClient : MuzzleloaderClient
	{
		// Token: 0x06000283 RID: 643 RVA: 0x0000D590 File Offset: 0x0000B790
		public ScopedClient(ICoreClientAPI api, Item item) : base(api, item)
		{
			this.IronsidesAimingStats = item.Attributes["IronsidesAiming"].AsObject<AimingStatsJson>(null).ToStats();
			this.ScopeAimingStats = item.Attributes["ScopeAiming"].AsObject<AimingStatsJson>(null).ToStats();
			this.ScopedAimAnimation = item.Attributes["ScopedAimAnimation"].AsString("");
			this.ScopedShootAnimation = item.Attributes["ScopedShootAnimation"].AsObject<string[]>(null);
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000D624 File Offset: 0x0000B824
		[ActionEventHandler(10, 2)]
		protected override bool Aim(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			if (!RangeWeaponClient.CheckState<MuzzleloaderState>(state, new MuzzleloaderState[]
			{
				MuzzleloaderState.Cocked
			}))
			{
				return false;
			}
			if (!mainHand && !base.CanUseOffhand(player))
			{
				return false;
			}
			if (eventData.AltPressed)
			{
				return false;
			}
			if (Enumerable.Contains<EnumEntityAction>(eventData.Modifiers, 13))
			{
				return false;
			}
			base.SetState<MuzzleloaderState>(MuzzleloaderState.Aim, mainHand);
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.Play(mainHand, this.Scoped ? this.ScopedAimAnimation : this.Stats.AimAnimation, base.AnimationCategory(mainHand), 1f, 1f, null, null, true);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				tpAnimationBehavior.Play(mainHand, this.Scoped ? this.ScopedAimAnimation : this.Stats.AimAnimation, base.AnimationCategory(mainHand), 1f, 1f, true);
			}
			FirstPersonAnimationsBehavior animationBehavior2 = this.AnimationBehavior;
			if (animationBehavior2 != null)
			{
				animationBehavior2.StopAllVanillaAnimations(mainHand);
			}
			if (this.TpAnimationBehavior == null)
			{
				FirstPersonAnimationsBehavior animationBehavior3 = this.AnimationBehavior;
				if (animationBehavior3 != null)
				{
					animationBehavior3.PlayVanillaAnimation(this.Stats.AimTpAnimation, mainHand);
				}
			}
			this.AimingSystem.StartAiming(this.Scoped ? this.ScopeAimingStats : this.IronsidesAimingStats);
			this.AimingSystem.AimingState = 3;
			this.AimingAnimationController.Stats = (this.Scoped ? this.ScopeAimingStats : this.IronsidesAimingStats);
			this.AimingAnimationController.Play(mainHand);
			return true;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000D794 File Offset: 0x0000B994
		[ActionEventHandler(10, 1)]
		protected virtual bool ChangeSites(ItemSlot slot, EntityPlayer player, ref int state, ActionEventData eventData, bool mainHand, AttackDirection direction)
		{
			if (!RangeWeaponClient.CheckState<MuzzleloaderState>(state, new MuzzleloaderState[]
			{
				MuzzleloaderState.Cocked
			}))
			{
				return false;
			}
			if (!mainHand && !base.CanUseOffhand(player))
			{
				return false;
			}
			if (eventData.AltPressed)
			{
				return false;
			}
			if (!Enumerable.Contains<EnumEntityAction>(eventData.Modifiers, 13))
			{
				return false;
			}
			this.Scoped = !this.Scoped;
			return true;
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000D7F4 File Offset: 0x0000B9F4
		protected override bool ShootCallback(ItemSlot slot, EntityPlayer player, bool mainHand)
		{
			FirstPersonAnimationsBehavior animationBehavior = this.AnimationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.Play(mainHand, this.Scoped ? this.ScopedAimAnimation : this.Stats.AimAnimation, base.AnimationCategory(mainHand), 1f, 1f, null, null, true);
			}
			ThirdPersonAnimationsBehavior tpAnimationBehavior = this.TpAnimationBehavior;
			if (tpAnimationBehavior != null)
			{
				tpAnimationBehavior.Play(mainHand, this.Scoped ? this.ScopedAimAnimation : this.Stats.AimAnimation, base.AnimationCategory(mainHand), 1f, 1f, true);
			}
			base.SetState<MuzzleloaderState>(MuzzleloaderState.Aim, mainHand);
			return true;
		}

		// Token: 0x04000152 RID: 338
		protected readonly AimingStats IronsidesAimingStats;

		// Token: 0x04000153 RID: 339
		protected readonly AimingStats ScopeAimingStats;

		// Token: 0x04000154 RID: 340
		protected readonly string ScopedAimAnimation;

		// Token: 0x04000155 RID: 341
		protected readonly string[] ScopedShootAnimation;

		// Token: 0x04000156 RID: 342
		protected bool Scoped;
	}
}
