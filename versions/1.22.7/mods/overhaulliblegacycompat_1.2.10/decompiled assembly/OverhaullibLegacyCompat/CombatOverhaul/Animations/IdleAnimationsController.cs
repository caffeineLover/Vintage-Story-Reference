using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000197 RID: 407
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class IdleAnimationsController
	{
		// Token: 0x060013CA RID: 5066 RVA: 0x00061728 File Offset: 0x0005F928
		public IdleAnimationsController(EntityPlayer player, Action<AnimationRequestByCode> playAnimationCallback, Action stopAnimationCallback, Func<ItemSlot> slotGetter, bool mainHand)
		{
			this._player = player;
			this._playAnimationCallback = playAnimationCallback;
			this._stopAnimationCallback = stopAnimationCallback;
			this._slotGetter = slotGetter;
			this._mainHand = mainHand;
		}

		// Token: 0x060013CB RID: 5067 RVA: 0x0006175C File Offset: 0x0005F95C
		public void Start()
		{
			this._currentItemId = this.GetItemId();
			this._currentAnimation = IdleAnimationsController.EnumAnimationType.New;
			this.PlayNextAnimation();
		}

		// Token: 0x060013CC RID: 5068 RVA: 0x00061777 File Offset: 0x0005F977
		public void Pause()
		{
			if (this._currentAnimation == IdleAnimationsController.EnumAnimationType.None)
			{
				return;
			}
			this._currentItemId = 0;
			this._currentAnimation = IdleAnimationsController.EnumAnimationType.None;
		}

		// Token: 0x060013CD RID: 5069 RVA: 0x00061791 File Offset: 0x0005F991
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

		// Token: 0x060013CE RID: 5070 RVA: 0x000617BB File Offset: 0x0005F9BB
		public void Update()
		{
			if (!this.NeedsUpdate())
			{
				return;
			}
			this.PlayNextAnimation();
		}

		// Token: 0x060013CF RID: 5071 RVA: 0x000617CC File Offset: 0x0005F9CC
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

		// Token: 0x060013D0 RID: 5072 RVA: 0x00061878 File Offset: 0x0005FA78
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

		// Token: 0x060013D1 RID: 5073 RVA: 0x000618C8 File Offset: 0x0005FAC8
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

		// Token: 0x060013D2 RID: 5074 RVA: 0x00061924 File Offset: 0x0005FB24
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

		// Token: 0x060013D3 RID: 5075 RVA: 0x000619AC File Offset: 0x0005FBAC
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

		// Token: 0x060013D4 RID: 5076 RVA: 0x00061A00 File Offset: 0x0005FC00
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

		// Token: 0x060013D5 RID: 5077 RVA: 0x00061A4C File Offset: 0x0005FC4C
		private static float GetWalkAnimationSpeed(EntityPlayer player)
		{
			EntityControls controls = player.Controls;
			return (float)((double)controls.MovespeedMultiplier * player.GetWalkSpeedMultiplier(0.3) * (controls.Sprint ? 0.9 : 1.2) * (double)(controls.Sneak ? 1.2f : 1f) / 1.2000000476837158 / 0.949999988079071);
		}

		// Token: 0x060013D6 RID: 5078 RVA: 0x00061AC0 File Offset: 0x0005FCC0
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

		// Token: 0x060013D7 RID: 5079 RVA: 0x00061B60 File Offset: 0x0005FD60
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

		// Token: 0x060013D8 RID: 5080 RVA: 0x00061CFC File Offset: 0x0005FEFC
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

		// Token: 0x060013D9 RID: 5081 RVA: 0x00061E94 File Offset: 0x00060094
		private static IdleAnimationsController.EnumAnimationType GetNextExistingAnimationType(EntityPlayer player, ItemSlot slot, bool mainHand, IdleAnimationsController.EnumAnimationType animationType)
		{
			IdleAnimationsController.EnumAnimationType result = IdleAnimationsController.GetNextAnimationType(player, animationType);
			while (!IdleAnimationsController.HasAnimation(player, slot, mainHand, result) && result != IdleAnimationsController.EnumAnimationType.None)
			{
				result = IdleAnimationsController.NextAnimationTypeIfNotExists(result);
			}
			return result;
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x00061EC4 File Offset: 0x000600C4
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

		// Token: 0x060013DB RID: 5083 RVA: 0x00061F20 File Offset: 0x00060120
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

		// Token: 0x060013DC RID: 5084 RVA: 0x00061F94 File Offset: 0x00060194
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

		// Token: 0x040009C7 RID: 2503
		private static readonly IdleAnimationsController.EnumAnimationType[] _repeatedAnimations = new IdleAnimationsController.EnumAnimationType[]
		{
			IdleAnimationsController.EnumAnimationType.Walk,
			IdleAnimationsController.EnumAnimationType.Run,
			IdleAnimationsController.EnumAnimationType.Swim,
			IdleAnimationsController.EnumAnimationType.SwimIdle,
			IdleAnimationsController.EnumAnimationType.Ready
		};

		// Token: 0x040009C8 RID: 2504
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

		// Token: 0x040009C9 RID: 2505
		private readonly EntityPlayer _player;

		// Token: 0x040009CA RID: 2506
		private readonly Action<AnimationRequestByCode> _playAnimationCallback;

		// Token: 0x040009CB RID: 2507
		private readonly Action _stopAnimationCallback;

		// Token: 0x040009CC RID: 2508
		private readonly Func<ItemSlot> _slotGetter;

		// Token: 0x040009CD RID: 2509
		private readonly bool _mainHand;

		// Token: 0x040009CE RID: 2510
		private IdleAnimationsController.EnumAnimationType _currentAnimation = IdleAnimationsController.EnumAnimationType.None;

		// Token: 0x040009CF RID: 2511
		private int _currentItemId;

		// Token: 0x020002D8 RID: 728
		[NullableContext(0)]
		private enum EnumAnimationType
		{
			// Token: 0x04000E6B RID: 3691
			None = -1,
			// Token: 0x04000E6C RID: 3692
			New,
			// Token: 0x04000E6D RID: 3693
			Idle,
			// Token: 0x04000E6E RID: 3694
			Ready,
			// Token: 0x04000E6F RID: 3695
			Walk,
			// Token: 0x04000E70 RID: 3696
			Run,
			// Token: 0x04000E71 RID: 3697
			SwimIdle,
			// Token: 0x04000E72 RID: 3698
			Swim
		}

		// Token: 0x020002D9 RID: 729
		[NullableContext(0)]
		private enum EnumPlayerState
		{
			// Token: 0x04000E74 RID: 3700
			Idle,
			// Token: 0x04000E75 RID: 3701
			Walk,
			// Token: 0x04000E76 RID: 3702
			Run,
			// Token: 0x04000E77 RID: 3703
			SwimIdle,
			// Token: 0x04000E78 RID: 3704
			Swim
		}
	}
}
