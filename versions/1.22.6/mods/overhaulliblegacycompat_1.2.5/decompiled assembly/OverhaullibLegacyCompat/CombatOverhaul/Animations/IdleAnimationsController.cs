using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000189 RID: 393
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class IdleAnimationsController
	{
		// Token: 0x06001352 RID: 4946 RVA: 0x0005FCA8 File Offset: 0x0005DEA8
		public IdleAnimationsController(EntityPlayer player, Action<AnimationRequestByCode> playAnimationCallback, Action stopAnimationCallback, Func<ItemSlot> slotGetter, bool mainHand)
		{
			this._player = player;
			this._playAnimationCallback = playAnimationCallback;
			this._stopAnimationCallback = stopAnimationCallback;
			this._slotGetter = slotGetter;
			this._mainHand = mainHand;
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x0005FCDC File Offset: 0x0005DEDC
		public void Start()
		{
			this._currentItemId = this.GetItemId();
			this._currentAnimation = IdleAnimationsController.EnumAnimationType.New;
			this.PlayNextAnimation();
		}

		// Token: 0x06001354 RID: 4948 RVA: 0x0005FCF7 File Offset: 0x0005DEF7
		public void Pause()
		{
			if (this._currentAnimation == IdleAnimationsController.EnumAnimationType.None)
			{
				return;
			}
			this._currentItemId = 0;
			this._currentAnimation = IdleAnimationsController.EnumAnimationType.None;
		}

		// Token: 0x06001355 RID: 4949 RVA: 0x0005FD11 File Offset: 0x0005DF11
		public void Stop()
		{
			if (this._currentAnimation == IdleAnimationsController.EnumAnimationType.None)
			{
				return;
			}
			this._currentItemId = 0;
			this._currentAnimation = IdleAnimationsController.EnumAnimationType.None;
			Action stopAnimationCallback = this._stopAnimationCallback;
			if (stopAnimationCallback == null)
			{
				return;
			}
			stopAnimationCallback.Invoke();
		}

		// Token: 0x06001356 RID: 4950 RVA: 0x0005FD3B File Offset: 0x0005DF3B
		public void Update()
		{
			if (!this.NeedsUpdate())
			{
				return;
			}
			this.PlayNextAnimation();
		}

		// Token: 0x06001357 RID: 4951 RVA: 0x0005FD4C File Offset: 0x0005DF4C
		[NullableContext(0)]
		[return: TupleElementNames(new string[]
		{
			"request",
			"animationType"
		})]
		private ValueTuple<AnimationRequestByCode?, IdleAnimationsController.EnumAnimationType> GetNextAnimation()
		{
			ItemSlot slot = this._slotGetter.Invoke();
			if (!IdleAnimationsController.HasAnyAnimations(slot))
			{
				return new ValueTuple<AnimationRequestByCode?, IdleAnimationsController.EnumAnimationType>(default(AnimationRequestByCode?), IdleAnimationsController.EnumAnimationType.None);
			}
			IdleAnimationsController.EnumAnimationType nextAnimationType = IdleAnimationsController.GetNextExistingAnimationType(this._player, slot, this._mainHand, this._currentAnimation);
			AnimationRequestByCode? animationRequest = IdleAnimationsController.GetAnimation(this._player, slot, this._mainHand, nextAnimationType);
			if (animationRequest == null)
			{
				return new ValueTuple<AnimationRequestByCode?, IdleAnimationsController.EnumAnimationType>(default(AnimationRequestByCode?), nextAnimationType);
			}
			float animationSpeed = IdleAnimationsController.GetAnimationSpeed(this._player, nextAnimationType);
			return new ValueTuple<AnimationRequestByCode?, IdleAnimationsController.EnumAnimationType>(new AnimationRequestByCode?(new AnimationRequestByCode(animationRequest.Value, animationSpeed, new Func<bool>(this.AnimationCallback), null)), nextAnimationType);
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x0005FDF8 File Offset: 0x0005DFF8
		private bool AnimationCallback()
		{
			if (this._currentItemId != this.GetItemId())
			{
				this._currentItemId = this.GetItemId();
				this.Stop();
				return true;
			}
			if (!MemoryExtensions.Contains<IdleAnimationsController.EnumAnimationType>(IdleAnimationsController._repeatedAnimations, this._currentAnimation, null))
			{
				return true;
			}
			this.PlayNextAnimation();
			return true;
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x0005FE48 File Offset: 0x0005E048
		private void PlayNextAnimation()
		{
			ValueTuple<AnimationRequestByCode?, IdleAnimationsController.EnumAnimationType> nextAnimation = this.GetNextAnimation();
			AnimationRequestByCode? request = nextAnimation.Item1;
			IdleAnimationsController.EnumAnimationType animationType = nextAnimation.Item2;
			if (request != null)
			{
				this._playAnimationCallback.Invoke(request.Value);
				this._currentAnimation = animationType;
				return;
			}
			Action stopAnimationCallback = this._stopAnimationCallback;
			if (stopAnimationCallback != null)
			{
				stopAnimationCallback.Invoke();
			}
			this._currentAnimation = IdleAnimationsController.EnumAnimationType.None;
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x0005FEA4 File Offset: 0x0005E0A4
		private bool NeedsUpdate()
		{
			ItemSlot slot = this._slotGetter.Invoke();
			if (!IdleAnimationsController.HasAnyAnimations(slot))
			{
				return false;
			}
			IdleAnimationsController.EnumAnimationType nextAnimationType = IdleAnimationsController.GetNextExistingAnimationType(this._player, slot, this._mainHand, this._currentAnimation);
			return !MemoryExtensions.Contains<ValueTuple<IdleAnimationsController.EnumAnimationType, IdleAnimationsController.EnumAnimationType>>(IdleAnimationsController._consecutiveAnimations, new ValueTuple<IdleAnimationsController.EnumAnimationType, IdleAnimationsController.EnumAnimationType>(this._currentAnimation, nextAnimationType)) && nextAnimationType != this._currentAnimation && IdleAnimationsController.GetAnimation(this._player, slot, this._mainHand, nextAnimationType) != null;
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x0005FF2C File Offset: 0x0005E12C
		private int GetItemId()
		{
			ItemStack itemstack = this._slotGetter.Invoke().Itemstack;
			int? num;
			if (itemstack == null)
			{
				num = default(int?);
			}
			else
			{
				Item item = itemstack.Item;
				num = ((item != null) ? new int?(item.Id) : default(int?));
			}
			int? num2 = num;
			return num2.GetValueOrDefault();
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x0005FF80 File Offset: 0x0005E180
		private static float GetAnimationSpeed(EntityPlayer player, IdleAnimationsController.EnumAnimationType animationType)
		{
			switch (animationType)
			{
			case IdleAnimationsController.EnumAnimationType.Walk:
				return IdleAnimationsController.GetWalkAnimationSpeed(player);
			case IdleAnimationsController.EnumAnimationType.Run:
				return IdleAnimationsController.GetWalkAnimationSpeed(player);
			case IdleAnimationsController.EnumAnimationType.Swim:
				return IdleAnimationsController.GetWalkAnimationSpeed(player);
			}
			return 1f;
		}

		// Token: 0x0600135D RID: 4957 RVA: 0x0005FFCC File Offset: 0x0005E1CC
		private static float GetWalkAnimationSpeed(EntityPlayer player)
		{
			EntityControls controls = player.Controls;
			return (float)((double)controls.MovespeedMultiplier * player.GetWalkSpeedMultiplier(0.3) * (controls.Sprint ? 0.9 : 1.2) * (double)(controls.Sneak ? 1.2f : 1f) / 1.2000000476837158 / 0.949999988079071);
		}

		// Token: 0x0600135E RID: 4958 RVA: 0x00060040 File Offset: 0x0005E240
		private static bool HasAnyAnimations(ItemSlot slot)
		{
			if (!slot.Empty)
			{
				ItemStack itemstack = slot.Itemstack;
				bool flag;
				if (itemstack == null)
				{
					flag = (null != null);
				}
				else
				{
					CollectibleObject collectible = itemstack.Collectible;
					flag = (((collectible != null) ? collectible.GetCollectibleInterface<IHasIdleAnimations>() : null) != null);
				}
				if (!flag)
				{
					ItemStack itemstack2 = slot.Itemstack;
					bool flag2;
					if (itemstack2 == null)
					{
						flag2 = (null != null);
					}
					else
					{
						CollectibleObject collectible2 = itemstack2.Collectible;
						flag2 = (((collectible2 != null) ? collectible2.GetCollectibleInterface<IHasMoveAnimations>() : null) != null);
					}
					if (!flag2)
					{
						ItemStack itemstack3 = slot.Itemstack;
						bool flag3;
						if (itemstack3 == null)
						{
							flag3 = (null != null);
						}
						else
						{
							CollectibleObject collectible3 = itemstack3.Collectible;
							flag3 = (((collectible3 != null) ? collectible3.GetCollectibleInterface<IHasDynamicIdleAnimations>() : null) != null);
						}
						if (!flag3)
						{
							ItemStack itemstack4 = slot.Itemstack;
							object obj;
							if (itemstack4 == null)
							{
								obj = null;
							}
							else
							{
								CollectibleObject collectible4 = itemstack4.Collectible;
								obj = ((collectible4 != null) ? collectible4.GetCollectibleInterface<IHasDynamicMoveAnimations>() : null);
							}
							return obj != null;
						}
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x0600135F RID: 4959 RVA: 0x000600E0 File Offset: 0x0005E2E0
		private static bool HasAnimation(EntityPlayer player, ItemSlot slot, bool mainHand, IdleAnimationsController.EnumAnimationType animationType)
		{
			IHasIdleAnimations idleAnimationsProvider = slot.Itemstack.Collectible.GetCollectibleInterface<IHasIdleAnimations>();
			IHasMoveAnimations moveAnimationsProvider = slot.Itemstack.Collectible.GetCollectibleInterface<IHasMoveAnimations>();
			IHasDynamicIdleAnimations idleDynamicAnimationsProvider = slot.Itemstack.Collectible.GetCollectibleInterface<IHasDynamicIdleAnimations>();
			IHasDynamicMoveAnimations moveDynamicAnimationsProvider = slot.Itemstack.Collectible.GetCollectibleInterface<IHasDynamicMoveAnimations>();
			AnimationRequestByCode? animationRequestByCode;
			switch (animationType)
			{
			case IdleAnimationsController.EnumAnimationType.Idle:
				animationRequestByCode = ((idleAnimationsProvider != null) ? new AnimationRequestByCode?(idleAnimationsProvider.IdleAnimation) : ((idleDynamicAnimationsProvider != null) ? idleDynamicAnimationsProvider.GetIdleAnimation(player, slot, mainHand) : default(AnimationRequestByCode?)));
				break;
			case IdleAnimationsController.EnumAnimationType.Ready:
				animationRequestByCode = ((idleAnimationsProvider != null) ? new AnimationRequestByCode?(idleAnimationsProvider.ReadyAnimation) : ((idleDynamicAnimationsProvider != null) ? idleDynamicAnimationsProvider.GetReadyAnimation(player, slot, mainHand) : default(AnimationRequestByCode?)));
				break;
			case IdleAnimationsController.EnumAnimationType.Walk:
				animationRequestByCode = ((moveAnimationsProvider != null) ? new AnimationRequestByCode?(moveAnimationsProvider.WalkAnimation) : ((moveDynamicAnimationsProvider != null) ? moveDynamicAnimationsProvider.GetWalkAnimation(player, slot, mainHand) : default(AnimationRequestByCode?)));
				break;
			case IdleAnimationsController.EnumAnimationType.Run:
				animationRequestByCode = ((moveAnimationsProvider != null) ? new AnimationRequestByCode?(moveAnimationsProvider.RunAnimation) : ((moveDynamicAnimationsProvider != null) ? moveDynamicAnimationsProvider.GetRunAnimation(player, slot, mainHand) : default(AnimationRequestByCode?)));
				break;
			case IdleAnimationsController.EnumAnimationType.SwimIdle:
				animationRequestByCode = ((moveAnimationsProvider != null) ? new AnimationRequestByCode?(moveAnimationsProvider.SwimIdleAnimation) : ((moveDynamicAnimationsProvider != null) ? moveDynamicAnimationsProvider.GetSwimIdleAnimation(player, slot, mainHand) : default(AnimationRequestByCode?)));
				break;
			case IdleAnimationsController.EnumAnimationType.Swim:
				animationRequestByCode = ((moveAnimationsProvider != null) ? new AnimationRequestByCode?(moveAnimationsProvider.SwimAnimation) : ((moveDynamicAnimationsProvider != null) ? moveDynamicAnimationsProvider.GetSwimAnimation(player, slot, mainHand) : default(AnimationRequestByCode?)));
				break;
			default:
				animationRequestByCode = default(AnimationRequestByCode?);
				break;
			}
			return animationRequestByCode != null;
		}

		// Token: 0x06001360 RID: 4960 RVA: 0x0006027C File Offset: 0x0005E47C
		private static AnimationRequestByCode? GetAnimation(EntityPlayer player, ItemSlot slot, bool mainHand, IdleAnimationsController.EnumAnimationType animationType)
		{
			IHasIdleAnimations idleAnimationsProvider = slot.Itemstack.Collectible.GetCollectibleInterface<IHasIdleAnimations>();
			IHasMoveAnimations moveAnimationsProvider = slot.Itemstack.Collectible.GetCollectibleInterface<IHasMoveAnimations>();
			IHasDynamicIdleAnimations idleDynamicAnimationsProvider = slot.Itemstack.Collectible.GetCollectibleInterface<IHasDynamicIdleAnimations>();
			IHasDynamicMoveAnimations moveDynamicAnimationsProvider = slot.Itemstack.Collectible.GetCollectibleInterface<IHasDynamicMoveAnimations>();
			AnimationRequestByCode? result;
			switch (animationType)
			{
			case IdleAnimationsController.EnumAnimationType.Idle:
				result = ((idleAnimationsProvider != null) ? new AnimationRequestByCode?(idleAnimationsProvider.IdleAnimation) : ((idleDynamicAnimationsProvider != null) ? idleDynamicAnimationsProvider.GetIdleAnimation(player, slot, mainHand) : default(AnimationRequestByCode?)));
				break;
			case IdleAnimationsController.EnumAnimationType.Ready:
				result = ((idleAnimationsProvider != null) ? new AnimationRequestByCode?(idleAnimationsProvider.ReadyAnimation) : ((idleDynamicAnimationsProvider != null) ? idleDynamicAnimationsProvider.GetReadyAnimation(player, slot, mainHand) : default(AnimationRequestByCode?)));
				break;
			case IdleAnimationsController.EnumAnimationType.Walk:
				result = ((moveAnimationsProvider != null) ? new AnimationRequestByCode?(moveAnimationsProvider.WalkAnimation) : ((moveDynamicAnimationsProvider != null) ? moveDynamicAnimationsProvider.GetWalkAnimation(player, slot, mainHand) : default(AnimationRequestByCode?)));
				break;
			case IdleAnimationsController.EnumAnimationType.Run:
				result = ((moveAnimationsProvider != null) ? new AnimationRequestByCode?(moveAnimationsProvider.RunAnimation) : ((moveDynamicAnimationsProvider != null) ? moveDynamicAnimationsProvider.GetRunAnimation(player, slot, mainHand) : default(AnimationRequestByCode?)));
				break;
			case IdleAnimationsController.EnumAnimationType.SwimIdle:
				result = ((moveAnimationsProvider != null) ? new AnimationRequestByCode?(moveAnimationsProvider.SwimIdleAnimation) : ((moveDynamicAnimationsProvider != null) ? moveDynamicAnimationsProvider.GetSwimIdleAnimation(player, slot, mainHand) : default(AnimationRequestByCode?)));
				break;
			case IdleAnimationsController.EnumAnimationType.Swim:
				result = ((moveAnimationsProvider != null) ? new AnimationRequestByCode?(moveAnimationsProvider.SwimAnimation) : ((moveDynamicAnimationsProvider != null) ? moveDynamicAnimationsProvider.GetSwimAnimation(player, slot, mainHand) : default(AnimationRequestByCode?)));
				break;
			default:
				result = default(AnimationRequestByCode?);
				break;
			}
			return result;
		}

		// Token: 0x06001361 RID: 4961 RVA: 0x00060414 File Offset: 0x0005E614
		private static IdleAnimationsController.EnumAnimationType GetNextExistingAnimationType(EntityPlayer player, ItemSlot slot, bool mainHand, IdleAnimationsController.EnumAnimationType animationType)
		{
			IdleAnimationsController.EnumAnimationType result = IdleAnimationsController.GetNextAnimationType(player, animationType);
			while (!IdleAnimationsController.HasAnimation(player, slot, mainHand, result) && result != IdleAnimationsController.EnumAnimationType.None)
			{
				result = IdleAnimationsController.NextAnimationTypeIfNotExists(result);
			}
			return result;
		}

		// Token: 0x06001362 RID: 4962 RVA: 0x00060444 File Offset: 0x0005E644
		private static IdleAnimationsController.EnumAnimationType NextAnimationTypeIfNotExists(IdleAnimationsController.EnumAnimationType animationType)
		{
			IdleAnimationsController.EnumAnimationType result;
			switch (animationType)
			{
			case IdleAnimationsController.EnumAnimationType.None:
				result = IdleAnimationsController.EnumAnimationType.None;
				break;
			case IdleAnimationsController.EnumAnimationType.New:
				result = IdleAnimationsController.EnumAnimationType.Ready;
				break;
			case IdleAnimationsController.EnumAnimationType.Idle:
				result = IdleAnimationsController.EnumAnimationType.None;
				break;
			case IdleAnimationsController.EnumAnimationType.Ready:
				result = IdleAnimationsController.EnumAnimationType.Idle;
				break;
			case IdleAnimationsController.EnumAnimationType.Walk:
				result = IdleAnimationsController.EnumAnimationType.Idle;
				break;
			case IdleAnimationsController.EnumAnimationType.Run:
				result = IdleAnimationsController.EnumAnimationType.Walk;
				break;
			case IdleAnimationsController.EnumAnimationType.SwimIdle:
				result = IdleAnimationsController.EnumAnimationType.Swim;
				break;
			case IdleAnimationsController.EnumAnimationType.Swim:
				result = IdleAnimationsController.EnumAnimationType.Idle;
				break;
			default:
				result = IdleAnimationsController.EnumAnimationType.None;
				break;
			}
			return result;
		}

		// Token: 0x06001363 RID: 4963 RVA: 0x000604A0 File Offset: 0x0005E6A0
		private static IdleAnimationsController.EnumAnimationType GetNextAnimationType(EntityPlayer player, IdleAnimationsController.EnumAnimationType currentAnimation)
		{
			IdleAnimationsController.EnumPlayerState playerState = IdleAnimationsController.GetPlayerState(player);
			switch (currentAnimation)
			{
			case IdleAnimationsController.EnumAnimationType.None:
				return IdleAnimationsController.EnumAnimationType.None;
			case IdleAnimationsController.EnumAnimationType.New:
				return IdleAnimationsController.EnumAnimationType.Ready;
			case IdleAnimationsController.EnumAnimationType.Ready:
				if (playerState != IdleAnimationsController.EnumPlayerState.Idle)
				{
					goto IL_29;
				}
				return IdleAnimationsController.EnumAnimationType.Idle;
			}
			if (playerState == IdleAnimationsController.EnumPlayerState.Idle)
			{
				return IdleAnimationsController.EnumAnimationType.Idle;
			}
			IL_29:
			IdleAnimationsController.EnumAnimationType result;
			switch (playerState)
			{
			case IdleAnimationsController.EnumPlayerState.Walk:
				result = IdleAnimationsController.EnumAnimationType.Walk;
				break;
			case IdleAnimationsController.EnumPlayerState.Run:
				result = IdleAnimationsController.EnumAnimationType.Run;
				break;
			case IdleAnimationsController.EnumPlayerState.SwimIdle:
				result = IdleAnimationsController.EnumAnimationType.SwimIdle;
				break;
			case IdleAnimationsController.EnumPlayerState.Swim:
				result = IdleAnimationsController.EnumAnimationType.Swim;
				break;
			default:
				result = IdleAnimationsController.EnumAnimationType.None;
				break;
			}
			return result;
		}

		// Token: 0x06001364 RID: 4964 RVA: 0x00060514 File Offset: 0x0005E714
		private static IdleAnimationsController.EnumPlayerState GetPlayerState(EntityPlayer player)
		{
			bool triesToMove = player.Controls.Forward || player.Controls.Right || player.Controls.Left;
			bool triesToRun = player.Controls.Sprint && triesToMove;
			bool swimming = player.Swimming;
			IdleAnimationsController.EnumPlayerState result;
			if (!triesToMove)
			{
				if (!triesToRun)
				{
					if (!swimming)
					{
						return IdleAnimationsController.EnumPlayerState.Idle;
					}
				}
				else if (!swimming)
				{
					return IdleAnimationsController.EnumPlayerState.Idle;
				}
				result = IdleAnimationsController.EnumPlayerState.SwimIdle;
			}
			else
			{
				if (!triesToRun)
				{
					if (!swimming)
					{
						return IdleAnimationsController.EnumPlayerState.Walk;
					}
				}
				else if (!swimming)
				{
					return IdleAnimationsController.EnumPlayerState.Run;
				}
				result = IdleAnimationsController.EnumPlayerState.Swim;
			}
			return result;
		}

		// Token: 0x0400099F RID: 2463
		private static readonly IdleAnimationsController.EnumAnimationType[] _repeatedAnimations = new IdleAnimationsController.EnumAnimationType[]
		{
			IdleAnimationsController.EnumAnimationType.Walk,
			IdleAnimationsController.EnumAnimationType.Run,
			IdleAnimationsController.EnumAnimationType.Swim,
			IdleAnimationsController.EnumAnimationType.SwimIdle,
			IdleAnimationsController.EnumAnimationType.Ready
		};

		// Token: 0x040009A0 RID: 2464
		[TupleElementNames(new string[]
		{
			"from",
			"to"
		})]
		[Nullable(new byte[]
		{
			1,
			0
		})]
		private static readonly ValueTuple<IdleAnimationsController.EnumAnimationType, IdleAnimationsController.EnumAnimationType>[] _consecutiveAnimations = new ValueTuple<IdleAnimationsController.EnumAnimationType, IdleAnimationsController.EnumAnimationType>[]
		{
			new ValueTuple<IdleAnimationsController.EnumAnimationType, IdleAnimationsController.EnumAnimationType>(IdleAnimationsController.EnumAnimationType.Ready, IdleAnimationsController.EnumAnimationType.Idle)
		};

		// Token: 0x040009A1 RID: 2465
		private readonly EntityPlayer _player;

		// Token: 0x040009A2 RID: 2466
		private readonly Action<AnimationRequestByCode> _playAnimationCallback;

		// Token: 0x040009A3 RID: 2467
		private readonly Action _stopAnimationCallback;

		// Token: 0x040009A4 RID: 2468
		private readonly Func<ItemSlot> _slotGetter;

		// Token: 0x040009A5 RID: 2469
		private readonly bool _mainHand;

		// Token: 0x040009A6 RID: 2470
		private IdleAnimationsController.EnumAnimationType _currentAnimation = IdleAnimationsController.EnumAnimationType.None;

		// Token: 0x040009A7 RID: 2471
		private int _currentItemId;

		// Token: 0x020002C5 RID: 709
		[NullableContext(0)]
		private enum EnumAnimationType
		{
			// Token: 0x04000E1F RID: 3615
			None = -1,
			// Token: 0x04000E20 RID: 3616
			New,
			// Token: 0x04000E21 RID: 3617
			Idle,
			// Token: 0x04000E22 RID: 3618
			Ready,
			// Token: 0x04000E23 RID: 3619
			Walk,
			// Token: 0x04000E24 RID: 3620
			Run,
			// Token: 0x04000E25 RID: 3621
			SwimIdle,
			// Token: 0x04000E26 RID: 3622
			Swim
		}

		// Token: 0x020002C6 RID: 710
		[NullableContext(0)]
		private enum EnumPlayerState
		{
			// Token: 0x04000E28 RID: 3624
			Idle,
			// Token: 0x04000E29 RID: 3625
			Walk,
			// Token: 0x04000E2A RID: 3626
			Run,
			// Token: 0x04000E2B RID: 3627
			SwimIdle,
			// Token: 0x04000E2C RID: 3628
			Swim
		}
	}
}
