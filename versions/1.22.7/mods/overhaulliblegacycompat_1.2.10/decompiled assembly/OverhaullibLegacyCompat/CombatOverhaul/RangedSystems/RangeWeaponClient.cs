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
	// Token: 0x020000DE RID: 222
	[NullableContext(1)]
	[Nullable(0)]
	public class RangeWeaponClient : IClientWeaponLogic
	{
		// Token: 0x06000B4E RID: 2894 RVA: 0x0003A65C File Offset: 0x0003885C
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

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000B4F RID: 2895 RVA: 0x0003A6BD File Offset: 0x000388BD
		public int ItemId
		{
			get
			{
				return this.Item.Id;
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000B50 RID: 2896 RVA: 0x0003A6CA File Offset: 0x000388CA
		public virtual DirectionsConfiguration DirectionsType
		{
			get
			{
				return DirectionsConfiguration.None;
			}
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x0003A6CD File Offset: 0x000388CD
		public virtual void OnDeselected(EntityPlayer player, bool mainHand, ref int state)
		{
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x0003A6CF File Offset: 0x000388CF
		public virtual void OnRegistered(ActionsManagerPlayerBehavior behavior, ICoreClientAPI api)
		{
			this.PlayerBehavior = behavior;
			this.AnimationBehavior = behavior.entity.GetBehavior<FirstPersonAnimationsBehavior>();
			this.TpAnimationBehavior = behavior.entity.GetBehavior<ThirdPersonAnimationsBehavior>();
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x0003A6FA File Offset: 0x000388FA
		public virtual void OnSelected(ItemSlot slot, EntityPlayer player, bool mainHand, ref int state)
		{
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x0003A6FC File Offset: 0x000388FC
		[NullableContext(0)]
		protected static bool CheckState<TState>(int state, [Nullable(new byte[]
		{
			1,
			0
		})] params TState[] statesToCheck) where TState : struct, Enum
		{
			return MemoryExtensions.Contains<TState>(statesToCheck, (TState)((object)Enum.ToObject(typeof(TState), state)), null);
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x0003A71F File Offset: 0x0003891F
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

		// Token: 0x06000B56 RID: 2902 RVA: 0x0003A754 File Offset: 0x00038954
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

		// Token: 0x06000B57 RID: 2903 RVA: 0x0003A781 File Offset: 0x00038981
		[NullableContext(0)]
		protected TState GetState<TState>(bool mainHand = true) where TState : struct, Enum
		{
			Type typeFromHandle = typeof(TState);
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			return (TState)((object)Enum.ToObject(typeFromHandle, (playerBehavior != null) ? playerBehavior.GetState(mainHand) : 0));
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x0003A7AC File Offset: 0x000389AC
		protected float GetAnimationSpeed(Entity player, string proficiencyStat, float min = 0.5f, float max = 2f)
		{
			ActionsManagerPlayerBehavior playerBehavior = this.PlayerBehavior;
			float num = (playerBehavior != null) ? playerBehavior.ManipulationSpeed : 1f;
			float proficiencyBonus = (proficiencyStat == "") ? 0f : (player.Stats.GetBlended(proficiencyStat) - 1f);
			return Math.Clamp(num + proficiencyBonus, min, max);
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x0003A800 File Offset: 0x00038A00
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

		// Token: 0x06000B5A RID: 2906 RVA: 0x0003A890 File Offset: 0x00038A90
		protected static int GetActionDelayMs(float animationSpeed, int baseMilliseconds)
		{
			return Math.Max(100, (int)((float)baseMilliseconds / Math.Max(0.1f, animationSpeed)));
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x0003A8A8 File Offset: 0x00038AA8
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

		// Token: 0x06000B5C RID: 2908 RVA: 0x0003A9C8 File Offset: 0x00038BC8
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

		// Token: 0x06000B5D RID: 2909 RVA: 0x0003AA66 File Offset: 0x00038C66
		protected virtual bool RequiresEmptyOtherHand(bool mainHand, EntityPlayer player)
		{
			return this.TwoHanded;
		}

		// Token: 0x04000541 RID: 1345
		protected readonly Item Item;

		// Token: 0x04000542 RID: 1346
		protected readonly ICoreClientAPI Api;

		// Token: 0x04000543 RID: 1347
		protected readonly RangedWeaponSystemClient RangedWeaponSystem;

		// Token: 0x04000544 RID: 1348
		protected readonly AttachableSystemClient AttachmentSystem;

		// Token: 0x04000545 RID: 1349
		[Nullable(2)]
		protected FirstPersonAnimationsBehavior AnimationBehavior;

		// Token: 0x04000546 RID: 1350
		[Nullable(2)]
		protected ThirdPersonAnimationsBehavior TpAnimationBehavior;

		// Token: 0x04000547 RID: 1351
		[Nullable(2)]
		protected ActionsManagerPlayerBehavior PlayerBehavior;

		// Token: 0x04000548 RID: 1352
		protected bool TwoHanded = true;
	}
}
