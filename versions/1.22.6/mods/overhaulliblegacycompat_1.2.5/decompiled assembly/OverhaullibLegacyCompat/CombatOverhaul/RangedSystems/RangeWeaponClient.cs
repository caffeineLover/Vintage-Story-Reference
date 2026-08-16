using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Animations;
using CombatOverhaul.Implementations;
using CombatOverhaul.Inputs;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000D3 RID: 211
	[NullableContext(1)]
	[Nullable(0)]
	public class RangeWeaponClient : IClientWeaponLogic
	{
		// Token: 0x06000AF2 RID: 2802 RVA: 0x0003966C File Offset: 0x0003786C
		public RangeWeaponClient(ICoreClientAPI api, Item item)
		{
			CombatOverhaulSystem system = api.ModLoader.GetModSystem<CombatOverhaulSystem>(true);
			RangedWeaponSystemClient clientRangedWeaponSystem = system.ClientRangedWeaponSystem;
			if (clientRangedWeaponSystem == null)
			{
				throw new Exception();
			}
			this.RangedWeaponSystem = clientRangedWeaponSystem;
			AttachableSystemClient clientAttachmentSystem = system.ClientAttachmentSystem;
			if (clientAttachmentSystem == null)
			{
				throw new Exception();
			}
			this.AttachmentSystem = clientAttachmentSystem;
			this.Item = item;
			this.Api = api;
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x000396CD File Offset: 0x000378CD
		public int ItemId
		{
			get
			{
				return this.Item.Id;
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000AF4 RID: 2804 RVA: 0x000396DA File Offset: 0x000378DA
		public virtual DirectionsConfiguration DirectionsType
		{
			get
			{
				return DirectionsConfiguration.None;
			}
		}

		// Token: 0x06000AF5 RID: 2805 RVA: 0x000396DD File Offset: 0x000378DD
		public virtual void OnDeselected(EntityPlayer player, bool mainHand, ref int state)
		{
		}

		// Token: 0x06000AF6 RID: 2806 RVA: 0x000396DF File Offset: 0x000378DF
		public virtual void OnRegistered(ActionsManagerPlayerBehavior behavior, ICoreClientAPI api)
		{
			this.PlayerBehavior = behavior;
			this.AnimationBehavior = behavior.entity.GetBehavior<FirstPersonAnimationsBehavior>();
			this.TpAnimationBehavior = behavior.entity.GetBehavior<ThirdPersonAnimationsBehavior>();
		}

		// Token: 0x06000AF7 RID: 2807 RVA: 0x0003970A File Offset: 0x0003790A
		public virtual void OnSelected(ItemSlot slot, EntityPlayer player, bool mainHand, ref int state)
		{
		}

		// Token: 0x06000AF8 RID: 2808 RVA: 0x0003970C File Offset: 0x0003790C
		[NullableContext(0)]
		protected static bool CheckState<TState>(int state, [Nullable(new byte[]
		{
			1,
			0
		})] params TState[] statesToCheck) where TState : struct, Enum
		{
			return MemoryExtensions.Contains<TState>(statesToCheck, (TState)((object)Enum.ToObject(typeof(TState), state)), null);
		}

		// Token: 0x06000AF9 RID: 2809 RVA: 0x0003972F File Offset: 0x0003792F
		[NullableContext(0)]
		protected bool CheckState<TState>(bool mainHand, [Nullable(new byte[]
		{
			1,
			0
		})] params TState[] statesToCheck) where TState : struct, Enum
		{
			ReadOnlySpan<TState> readOnlySpan = statesToCheck;
			Type typeFromHandle = typeof(TState);
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			return MemoryExtensions.Contains<TState>(readOnlySpan, (TState)((object)Enum.ToObject(typeFromHandle, (playerBehavior != null) ? playerBehavior.GetState(mainHand) : 0)), null);
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x00039764 File Offset: 0x00037964
		[NullableContext(0)]
		protected void SetState<TState>(TState state, bool mainHand = true) where TState : struct, Enum
		{
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			if (playerBehavior == null)
			{
				return;
			}
			playerBehavior.SetState((int)Enum.ToObject(typeof(TState), state), mainHand);
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x00039791 File Offset: 0x00037991
		[NullableContext(0)]
		protected TState GetState<TState>(bool mainHand = true) where TState : struct, Enum
		{
			Type typeFromHandle = typeof(TState);
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			return (TState)((object)Enum.ToObject(typeFromHandle, (playerBehavior != null) ? playerBehavior.GetState(mainHand) : 0));
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x000397BC File Offset: 0x000379BC
		protected float GetAnimationSpeed(Entity player, string proficiencyStat, float min = 0.5f, float max = 2f)
		{
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			float num = (playerBehavior != null) ? playerBehavior.ManipulationSpeed : 1f;
			float proficiencyBonus = (proficiencyStat == "") ? 0f : (player.Stats.GetBlended(proficiencyStat) - 1f);
			return Math.Clamp(num + proficiencyBonus, min, max);
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x00039810 File Offset: 0x00037A10
		protected virtual float GetAnimationSpeed(Entity player, WeaponStats stats, float min = 0.5f, float max = 2f)
		{
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			float manipulationSpeed = (playerBehavior != null) ? playerBehavior.ManipulationSpeed : 1f;
			float proficiencyBonus = (stats.ProficiencyStat == "") ? 0f : (player.Stats.GetBlended(stats.ProficiencyStat) - 1f);
			foreach (string stat in stats.ProficiencyStats)
			{
				proficiencyBonus += player.Stats.GetBlended(stat) - 1f;
			}
			return Math.Clamp(manipulationSpeed + proficiencyBonus, min, max);
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x000398A0 File Offset: 0x00037AA0
		protected static int GetActionDelayMs(float animationSpeed, int baseMilliseconds)
		{
			return Math.Max(100, (int)((float)baseMilliseconds / Math.Max(0.1f, animationSpeed)));
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x000398B8 File Offset: 0x00037AB8
		protected int GetActionDelayMs(EntityPlayer player, string animationCode, float animationSpeed, int fallbackBaseMilliseconds, [Nullable(2)] string callbackCode = null)
		{
			int baseMilliseconds = fallbackBaseMilliseconds;
			if (!string.IsNullOrEmpty(animationCode))
			{
				CombatOverhaulAnimationsSystem modSystem = this.Api.ModLoader.GetModSystem<CombatOverhaulAnimationsSystem>(true);
				AnimationsManager animationsManager = (modSystem != null) ? modSystem.PlayerAnimationsManager : null;
				Animation animation = (animationsManager != null) ? animationsManager.GetAnimation(animationCode, player, true) : null;
				if (animation != null)
				{
					if (!string.IsNullOrEmpty(callbackCode))
					{
						bool callbackFound = false;
						IEnumerable<CallbackFrame> callbackFrames = animation.CallbackFrames;
						Func<CallbackFrame, bool> <>9__0;
						Func<CallbackFrame, bool> func;
						if ((func = <>9__0) == null)
						{
							func = (<>9__0 = ((CallbackFrame frame) => frame.Code == callbackCode));
						}
						using (IEnumerator<CallbackFrame> enumerator = Enumerable.Where<CallbackFrame>(callbackFrames, func).GetEnumerator())
						{
							if (enumerator.MoveNext())
							{
								CallbackFrame callbackFrame = enumerator.Current;
								baseMilliseconds = (int)(animation.TotalDuration.TotalMilliseconds * (double)callbackFrame.DurationFraction);
								callbackFound = true;
							}
						}
						if (!callbackFound)
						{
							baseMilliseconds = (int)animation.TotalDuration.TotalMilliseconds;
						}
					}
					else
					{
						baseMilliseconds = (int)animation.TotalDuration.TotalMilliseconds;
					}
				}
			}
			return RangeWeaponClient.GetActionDelayMs(animationSpeed, baseMilliseconds);
		}

		// Token: 0x06000B00 RID: 2816 RVA: 0x000399D8 File Offset: 0x00037BD8
		protected bool CheckForOtherHandEmpty(bool mainHand, EntityPlayer player)
		{
			if (!this.RequiresEmptyOtherHand(mainHand, player))
			{
				return true;
			}
			if (mainHand && !player.LeftHandItemSlot.Empty)
			{
				ICoreClientAPI coreClientAPI = player.World.Api as ICoreClientAPI;
				if (coreClientAPI != null)
				{
					coreClientAPI.TriggerIngameError(this, "offhandShouldBeEmpty", Lang.Get("combatoverhaul:message-offhand-empty", Array.Empty<object>()));
				}
				return false;
			}
			if (!mainHand && !player.RightHandItemSlot.Empty)
			{
				ICoreClientAPI coreClientAPI2 = player.World.Api as ICoreClientAPI;
				if (coreClientAPI2 != null)
				{
					coreClientAPI2.TriggerIngameError(this, "mainHandShouldBeEmpty", Lang.Get("combatoverhaul:message-mainhand-empty", Array.Empty<object>()));
				}
				return false;
			}
			return true;
		}

		// Token: 0x06000B01 RID: 2817 RVA: 0x00039A76 File Offset: 0x00037C76
		protected virtual bool RequiresEmptyOtherHand(bool mainHand, EntityPlayer player)
		{
			return this.TwoHanded;
		}

		// Token: 0x0400051F RID: 1311
		protected readonly Item Item;

		// Token: 0x04000520 RID: 1312
		protected readonly ICoreClientAPI Api;

		// Token: 0x04000521 RID: 1313
		protected readonly RangedWeaponSystemClient RangedWeaponSystem;

		// Token: 0x04000522 RID: 1314
		protected readonly AttachableSystemClient AttachmentSystem;

		// Token: 0x04000523 RID: 1315
		[Nullable(2)]
		protected FirstPersonAnimationsBehavior AnimationBehavior;

		// Token: 0x04000524 RID: 1316
		[Nullable(2)]
		protected ThirdPersonAnimationsBehavior TpAnimationBehavior;

		// Token: 0x04000525 RID: 1317
		[Nullable(2)]
		protected ActionsManagerPlayerBehavior PlayerBehavior;

		// Token: 0x04000526 RID: 1318
		protected bool TwoHanded = true;
	}
}
