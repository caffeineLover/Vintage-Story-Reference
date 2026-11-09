using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Animations;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Implementations
{
	// Token: 0x02000078 RID: 120
	[NullableContext(2)]
	[Nullable(0)]
	public sealed class GripController
	{
		// Token: 0x060005AF RID: 1455 RVA: 0x00021607 File Offset: 0x0001F807
		public GripController(FirstPersonAnimationsBehavior animationBehavior)
		{
			this._animationBehavior = animationBehavior;
		}

		// Token: 0x060005B0 RID: 1456 RVA: 0x00021628 File Offset: 0x0001F828
		public void ChangeGrip(float delta, bool mainHand, float gripFactor, float min, float max)
		{
			GripController.NormalizeRange(ref min, ref max);
			if (min == 0f && max == 0f)
			{
				this.StopAnimation(mainHand);
				return;
			}
			this._grip = GameMath.Clamp(this._grip + delta * gripFactor, min, max);
			this.PlayAnimation(mainHand);
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x00021678 File Offset: 0x0001F878
		public void ResetGrip(bool mainHand)
		{
			this._grip = 0f;
			FirstPersonAnimationsBehavior animationBehavior = this._animationBehavior;
			if (animationBehavior == null)
			{
				return;
			}
			animationBehavior.Stop("grip");
		}

		// Token: 0x060005B2 RID: 1458 RVA: 0x0002169A File Offset: 0x0001F89A
		public void StopAnimation(bool mainHand)
		{
			FirstPersonAnimationsBehavior animationBehavior = this._animationBehavior;
			if (animationBehavior == null)
			{
				return;
			}
			animationBehavior.Stop("grip");
		}

		// Token: 0x060005B3 RID: 1459 RVA: 0x000216B1 File Offset: 0x0001F8B1
		public void AdjustGrip(bool mainHand, float min, float max)
		{
			GripController.NormalizeRange(ref min, ref max);
			if (min == 0f && max == 0f)
			{
				this.StopAnimation(mainHand);
				return;
			}
			this._grip = GameMath.Clamp(this._grip, min, max);
			this.PlayAnimation(mainHand);
		}

		// Token: 0x060005B4 RID: 1460 RVA: 0x000216F0 File Offset: 0x0001F8F0
		private static void NormalizeRange(ref float min, ref float max)
		{
			if (min <= max)
			{
				return;
			}
			float num = max;
			float num2 = min;
			min = num;
			max = num2;
		}

		// Token: 0x060005B5 RID: 1461 RVA: 0x00021710 File Offset: 0x0001F910
		private PLayerKeyFrame GetAimingFrame(bool mainHand)
		{
			AnimationElement element = new AnimationElement(new float?(this._grip), default(float?), default(float?), default(float?), default(float?), default(float?));
			AnimationElement nullElement = new AnimationElement(default(float?), default(float?), default(float?), default(float?), default(float?), default(float?));
			PlayerFrame frame;
			if (!mainHand)
			{
				LeftHandFrame? leftHand = new LeftHandFrame?(new LeftHandFrame(element, nullElement, nullElement));
				frame = new PlayerFrame(default(RightHandFrame?), leftHand, default(OtherPartsFrame?), default(AnimationElement?), default(AnimationElement?), false, false, 0.8f, 1f, 1f, default(float?), default(AnimationElement?));
			}
			else
			{
				RightHandFrame? rightHand = new RightHandFrame?(new RightHandFrame(element, nullElement, nullElement));
				LeftHandFrame? leftHand = default(LeftHandFrame?);
				frame = new PlayerFrame(rightHand, leftHand, default(OtherPartsFrame?), default(AnimationElement?), default(AnimationElement?), false, false, 0.8f, 1f, 1f, default(float?), default(AnimationElement?));
			}
			return new PLayerKeyFrame(frame, TimeSpan.Zero, EasingFunctionType.Linear);
		}

		// Token: 0x060005B6 RID: 1462 RVA: 0x00021868 File Offset: 0x0001FA68
		private void PlayAnimation(bool mainHand)
		{
			this._gripAnimation.PlayerKeyFrames[0] = this.GetAimingFrame(mainHand);
			this._gripAnimation.Hold = true;
			AnimationRequest request = new AnimationRequest(this._gripAnimation, 1f, 0f, "grip", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), true, null, null);
			FirstPersonAnimationsBehavior animationBehavior = this._animationBehavior;
			if (animationBehavior == null)
			{
				return;
			}
			animationBehavior.Play(request, mainHand);
		}

		// Token: 0x04000278 RID: 632
		private float _grip;

		// Token: 0x04000279 RID: 633
		[Nullable(1)]
		private readonly Animation _gripAnimation = Animation.Zero.Clone();

		// Token: 0x0400027A RID: 634
		private readonly FirstPersonAnimationsBehavior _animationBehavior;
	}
}
