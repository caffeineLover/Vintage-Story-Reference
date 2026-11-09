using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000190 RID: 400
	public readonly struct PlayerFrame
	{
		// Token: 0x06001376 RID: 4982 RVA: 0x0005EC6C File Offset: 0x0005CE6C
		public PlayerFrame(RightHandFrame? rightHand = null, LeftHandFrame? leftHand = null, OtherPartsFrame? otherParts = null, AnimationElement? upperTorso = null, AnimationElement? detachedAnchorFrame = null, bool detachedAnchor = false, bool switchArms = false, float pitchFollow = 0.8f, float fovMultiplier = 1f, float bobbingAmplitude = 1f, float? detachedAnchorFollow = null, AnimationElement? lowerTorso = null)
		{
			this.OtherParts = default(OtherPartsFrame?);
			this.UpperTorso = default(AnimationElement?);
			this.DetachedAnchorFrame = default(AnimationElement?);
			this.LowerTorso = default(AnimationElement?);
			this.DetachedAnchor = false;
			this.SwitchArms = false;
			this.PitchFollow = 0.8f;
			this.FovMultiplier = 1f;
			this.BobbingAmplitude = 1f;
			this.DetachedAnchorFollow = 1f;
			this.RightHand = rightHand;
			this.LeftHand = leftHand;
			this.OtherParts = otherParts;
			this.UpperTorso = upperTorso;
			this.DetachedAnchorFrame = detachedAnchorFrame;
			this.DetachedAnchor = detachedAnchor;
			this.SwitchArms = switchArms;
			this.PitchFollow = pitchFollow;
			this.FovMultiplier = fovMultiplier;
			this.BobbingAmplitude = bobbingAmplitude;
			this.LowerTorso = lowerTorso;
			this.DetachedAnchorFollow = (detachedAnchorFollow ?? (!detachedAnchor));
		}

		// Token: 0x06001377 RID: 4983 RVA: 0x0005ED5C File Offset: 0x0005CF5C
		[NullableContext(1)]
		public void Apply(ElementPose pose, EnumAnimatedElement element, Vector3 eyePosition, float eyeHeight, float cameraPitch, bool applyCameraPitch, bool overrideTorso)
		{
			switch (element)
			{
			case EnumAnimatedElement.DetachedAnchor:
				if (this.DetachedAnchorFrame == null)
				{
					return;
				}
				this.DetachedAnchorFrame.GetValueOrDefault().Apply(pose);
				return;
			case EnumAnimatedElement.UpperTorso:
				if (this.UpperTorso != null)
				{
					this.UpperTorso.GetValueOrDefault().Apply(pose);
				}
				if (applyCameraPitch)
				{
					pose.degZ += GameMath.Clamp(cameraPitch * 57.295776f * this.PitchFollow * this.DetachedAnchorFollow, -45f, 75f);
					return;
				}
				break;
			case EnumAnimatedElement.LowerTorso:
			{
				if (!overrideTorso)
				{
					if (this.LowerTorso != null)
					{
						this.LowerTorso.GetValueOrDefault().Apply(pose);
					}
					pose.translateY = (eyePosition.Y - eyeHeight) * 16.1f / 16f;
					return;
				}
				if (this.LowerTorso != null)
				{
					AnimationElement torso = new AnimationElement(this.LowerTorso.Value.OffsetX, new float?((eyePosition.Y - eyeHeight) * 16.1f), this.LowerTorso.Value.OffsetZ, this.LowerTorso.Value.RotationX, this.LowerTorso.Value.RotationY, this.LowerTorso.Value.RotationZ);
					torso.Apply(pose);
					return;
				}
				AnimationElement torso2 = new AnimationElement(new float?(0f), new float?((eyePosition.Y - eyeHeight) * 16.1f), new float?(0f), new float?(0f), new float?(0f), new float?(0f));
				torso2.Apply(pose);
				return;
			}
			case EnumAnimatedElement.Neck:
				if (this.OtherParts != null)
				{
					this.OtherParts.GetValueOrDefault().Apply(pose, element);
				}
				if (applyCameraPitch)
				{
					pose.degZ = -GameMath.Clamp(cameraPitch * 57.295776f * this.PitchFollow * this.DetachedAnchorFollow, -45f, 75f) / 2f;
					return;
				}
				break;
			default:
				if (this.OtherParts != null)
				{
					this.OtherParts.GetValueOrDefault().Apply(pose, element);
				}
				if (this.RightHand != null)
				{
					this.RightHand.GetValueOrDefault().Apply(pose, element);
				}
				if (this.LeftHand == null)
				{
					return;
				}
				this.LeftHand.GetValueOrDefault().Apply(pose, element);
				break;
			}
		}

		// Token: 0x06001378 RID: 4984 RVA: 0x0005EFD4 File Offset: 0x0005D1D4
		public static PlayerFrame Interpolate(PlayerFrame from, PlayerFrame to, float progress)
		{
			RightHandFrame? righthand = default(RightHandFrame?);
			if (from.RightHand == null && to.RightHand != null)
			{
				righthand = default(RightHandFrame?);
			}
			else if (from.RightHand != null && to.RightHand == null)
			{
				righthand = default(RightHandFrame?);
			}
			else if (from.RightHand != null && to.RightHand != null)
			{
				righthand = new RightHandFrame?(RightHandFrame.Interpolate(from.RightHand.Value, to.RightHand.Value, progress));
			}
			LeftHandFrame? leftHand = default(LeftHandFrame?);
			if (from.LeftHand == null && to.LeftHand != null)
			{
				leftHand = default(LeftHandFrame?);
			}
			else if (from.LeftHand != null && to.LeftHand == null)
			{
				leftHand = default(LeftHandFrame?);
			}
			else if (from.LeftHand != null && to.LeftHand != null)
			{
				leftHand = new LeftHandFrame?(LeftHandFrame.Interpolate(from.LeftHand.Value, to.LeftHand.Value, progress));
			}
			OtherPartsFrame? otherParts = default(OtherPartsFrame?);
			if (from.OtherParts == null && to.OtherParts != null)
			{
				otherParts = default(OtherPartsFrame?);
			}
			else if (from.OtherParts != null && to.OtherParts == null)
			{
				otherParts = default(OtherPartsFrame?);
			}
			else if (from.OtherParts != null && to.OtherParts != null)
			{
				otherParts = new OtherPartsFrame?(OtherPartsFrame.Interpolate(from.OtherParts.Value, to.OtherParts.Value, progress));
			}
			AnimationElement? anchor = new AnimationElement?(AnimationElement.Interpolate(from.DetachedAnchorFrame ?? AnimationElement.Zero, to.DetachedAnchorFrame ?? AnimationElement.Zero, progress));
			AnimationElement? torso = new AnimationElement?(AnimationElement.Interpolate(from.UpperTorso ?? AnimationElement.Zero, to.UpperTorso ?? AnimationElement.Zero, progress));
			AnimationElement? lowerTorso = new AnimationElement?(AnimationElement.Interpolate(from.LowerTorso ?? AnimationElement.Zero, to.LowerTorso ?? AnimationElement.Zero, progress));
			if (from.DetachedAnchorFrame == null && to.DetachedAnchorFrame == null)
			{
				anchor = default(AnimationElement?);
			}
			if (from.UpperTorso == null && to.UpperTorso == null)
			{
				torso = default(AnimationElement?);
			}
			if (from.LowerTorso == null && to.LowerTorso == null)
			{
				lowerTorso = default(AnimationElement?);
			}
			float pitchFollow = from.PitchFollow + (to.PitchFollow - from.PitchFollow) * progress;
			float fov = from.FovMultiplier + (to.FovMultiplier - from.FovMultiplier) * progress;
			float bobbing = from.BobbingAmplitude + (to.BobbingAmplitude - from.BobbingAmplitude) * progress;
			float detachedAnchorFollow = from.DetachedAnchorFollow + (to.DetachedAnchorFollow - from.DetachedAnchorFollow) * progress;
			return new PlayerFrame(righthand, leftHand, otherParts, torso, anchor, to.DetachedAnchor, to.SwitchArms, pitchFollow, fov, bobbing, new float?(detachedAnchorFollow), lowerTorso);
		}

		// Token: 0x06001379 RID: 4985 RVA: 0x0005F374 File Offset: 0x0005D574
		public static PlayerFrame Compose([TupleElementNames(new string[]
		{
			"element",
			"weight"
		})] [Nullable(new byte[]
		{
			1,
			0
		})] IEnumerable<ValueTuple<PlayerFrame, float>> frames)
		{
			PlayerFrame.PlayerFrameComposition composition = default(PlayerFrame.PlayerFrameComposition);
			foreach (ValueTuple<PlayerFrame, float> valueTuple in frames)
			{
				PlayerFrame element = valueTuple.Item1;
				float weight = valueTuple.Item2;
				composition.Add(element, weight);
			}
			return composition.ToFrame();
		}

		// Token: 0x0600137A RID: 4986 RVA: 0x0005F3DC File Offset: 0x0005D5DC
		public static PlayerFrame Compose([TupleElementNames(new string[]
		{
			"element",
			"weight"
		})] [Nullable(new byte[]
		{
			1,
			0
		})] IReadOnlyList<ValueTuple<PlayerItemFrame, float>> frames)
		{
			PlayerFrame.PlayerFrameComposition composition = default(PlayerFrame.PlayerFrameComposition);
			foreach (ValueTuple<PlayerItemFrame, float> valueTuple in frames)
			{
				PlayerItemFrame element = valueTuple.Item1;
				float weight = valueTuple.Item2;
				composition.Add(element.Player, weight);
			}
			return composition.ToFrame();
		}

		// Token: 0x04000975 RID: 2421
		public readonly RightHandFrame? RightHand;

		// Token: 0x04000976 RID: 2422
		public readonly LeftHandFrame? LeftHand;

		// Token: 0x04000977 RID: 2423
		public readonly OtherPartsFrame? OtherParts;

		// Token: 0x04000978 RID: 2424
		public readonly AnimationElement? UpperTorso;

		// Token: 0x04000979 RID: 2425
		public readonly AnimationElement? DetachedAnchorFrame;

		// Token: 0x0400097A RID: 2426
		public readonly AnimationElement? LowerTorso;

		// Token: 0x0400097B RID: 2427
		public readonly bool DetachedAnchor;

		// Token: 0x0400097C RID: 2428
		public readonly bool SwitchArms;

		// Token: 0x0400097D RID: 2429
		public readonly float PitchFollow;

		// Token: 0x0400097E RID: 2430
		public readonly float FovMultiplier;

		// Token: 0x0400097F RID: 2431
		public readonly float BobbingAmplitude;

		// Token: 0x04000980 RID: 2432
		public readonly float DetachedAnchorFollow;

		// Token: 0x04000981 RID: 2433
		public const float DefaultPitchFollow = 0.8f;

		// Token: 0x04000982 RID: 2434
		public const float PerfectPitchFollow = 1f;

		// Token: 0x04000983 RID: 2435
		public const float Epsilon = 1E-06f;

		// Token: 0x04000984 RID: 2436
		public const float EyeHeightToAnimationDistanceMultiplier = 16.1f;

		// Token: 0x04000985 RID: 2437
		public const float PitchAngleMin = -45f;

		// Token: 0x04000986 RID: 2438
		public const float PitchAngleMax = 75f;

		// Token: 0x04000987 RID: 2439
		public static readonly PlayerFrame Zero = new PlayerFrame(new RightHandFrame?(RightHandFrame.Zero), new LeftHandFrame?(LeftHandFrame.Zero), new OtherPartsFrame?(OtherPartsFrame.Zero), default(AnimationElement?), default(AnimationElement?), false, false, 0.8f, 1f, 1f, default(float?), default(AnimationElement?));

		// Token: 0x04000988 RID: 2440
		public static readonly PlayerFrame Empty = new PlayerFrame(default(RightHandFrame?), default(LeftHandFrame?), default(OtherPartsFrame?), default(AnimationElement?), default(AnimationElement?), false, false, 0.8f, 1f, 1f, default(float?), default(AnimationElement?));

		// Token: 0x020002D5 RID: 725
		private struct PlayerFrameComposition
		{
			// Token: 0x06001917 RID: 6423 RVA: 0x0006F588 File Offset: 0x0006D788
			public void Add(PlayerFrame frame, float weight)
			{
				if (!this._hasAny)
				{
					this._fovMultiplier = frame.FovMultiplier;
					this._bobbingAmplitude = frame.BobbingAmplitude;
					this._detachedAnchorFollow = frame.DetachedAnchorFollow;
					this._hasAny = true;
				}
				else
				{
					this._fovMultiplier = Math.Min(this._fovMultiplier, frame.FovMultiplier);
					this._bobbingAmplitude = Math.Min(this._bobbingAmplitude, frame.BobbingAmplitude);
					this._detachedAnchorFollow = Math.Min(this._detachedAnchorFollow, frame.DetachedAnchorFollow);
				}
				this._detachedAnchor |= frame.DetachedAnchor;
				this._switchArms |= frame.SwitchArms;
				if (!this._pitchFollowSet && Math.Abs(frame.PitchFollow - 0.8f) > 1E-06f)
				{
					this._pitchFollow = frame.PitchFollow;
					this._pitchFollowSet = true;
				}
				RightHandFrame? rightHand2 = frame.RightHand;
				if (rightHand2 != null)
				{
					RightHandFrame rightHand = rightHand2.GetValueOrDefault();
					this._hasRightHand = true;
					this._rightItemAnchor.Add(rightHand.ItemAnchor, weight);
					this._rightLowerArm.Add(rightHand.LowerArmR, weight);
					this._rightUpperArm.Add(rightHand.UpperArmR, weight);
				}
				LeftHandFrame? leftHand2 = frame.LeftHand;
				if (leftHand2 != null)
				{
					LeftHandFrame leftHand = leftHand2.GetValueOrDefault();
					this._hasLeftHand = true;
					this._leftItemAnchor.Add(leftHand.ItemAnchorL, weight);
					this._leftLowerArm.Add(leftHand.LowerArmL, weight);
					this._leftUpperArm.Add(leftHand.UpperArmL, weight);
				}
				OtherPartsFrame? otherParts2 = frame.OtherParts;
				if (otherParts2 != null)
				{
					OtherPartsFrame otherParts = otherParts2.GetValueOrDefault();
					this._hasOtherParts = true;
					this._neck.Add(otherParts.Neck, weight);
					this._head.Add(otherParts.Head, weight);
					this._upperFootR.Add(otherParts.UpperFootR, weight);
					this._upperFootL.Add(otherParts.UpperFootL, weight);
					this._lowerFootR.Add(otherParts.LowerFootR, weight);
					this._lowerFootL.Add(otherParts.LowerFootL, weight);
				}
				AnimationElement? animationElement = frame.UpperTorso;
				if (animationElement != null)
				{
					AnimationElement upperTorso = animationElement.GetValueOrDefault();
					this._hasUpperTorso = true;
					this._upperTorso.Add(upperTorso, weight);
				}
				animationElement = frame.DetachedAnchorFrame;
				if (animationElement != null)
				{
					AnimationElement detachedAnchorFrame = animationElement.GetValueOrDefault();
					this._hasDetachedAnchorFrame = true;
					this._detachedAnchorFrame.Add(detachedAnchorFrame, weight);
				}
				animationElement = frame.LowerTorso;
				if (animationElement != null)
				{
					AnimationElement lowerTorso = animationElement.GetValueOrDefault();
					this._hasLowerTorso = true;
					this._lowerTorso.Add(lowerTorso, weight);
				}
			}

			// Token: 0x06001918 RID: 6424 RVA: 0x0006F82C File Offset: 0x0006DA2C
			public PlayerFrame ToFrame()
			{
				if (!this._hasAny)
				{
					return PlayerFrame.Empty;
				}
				RightHandFrame? rightHand = this._hasRightHand ? new RightHandFrame?(new RightHandFrame(this._rightItemAnchor.ToElement(), this._rightLowerArm.ToElement(), this._rightUpperArm.ToElement())) : default(RightHandFrame?);
				LeftHandFrame? leftHand = this._hasLeftHand ? new LeftHandFrame?(new LeftHandFrame(this._leftItemAnchor.ToElement(), this._leftLowerArm.ToElement(), this._leftUpperArm.ToElement())) : default(LeftHandFrame?);
				OtherPartsFrame? otherParts = this._hasOtherParts ? new OtherPartsFrame?(new OtherPartsFrame(this._neck.ToElement(), this._head.ToElement(), this._upperFootR.ToElement(), this._upperFootL.ToElement(), this._lowerFootR.ToElement(), this._lowerFootL.ToElement())) : default(OtherPartsFrame?);
				return new PlayerFrame(rightHand, leftHand, otherParts, this._hasUpperTorso ? new AnimationElement?(this._upperTorso.ToElement()) : default(AnimationElement?), this._hasDetachedAnchorFrame ? new AnimationElement?(this._detachedAnchorFrame.ToElement()) : default(AnimationElement?), this._detachedAnchor, this._switchArms, this._pitchFollowSet ? this._pitchFollow : 0.8f, this._fovMultiplier, this._bobbingAmplitude, new float?(this._detachedAnchorFollow), this._hasLowerTorso ? new AnimationElement?(this._lowerTorso.ToElement()) : default(AnimationElement?));
			}

			// Token: 0x04000E39 RID: 3641
			private bool _hasAny;

			// Token: 0x04000E3A RID: 3642
			private bool _hasRightHand;

			// Token: 0x04000E3B RID: 3643
			private bool _hasLeftHand;

			// Token: 0x04000E3C RID: 3644
			private bool _hasOtherParts;

			// Token: 0x04000E3D RID: 3645
			private bool _hasUpperTorso;

			// Token: 0x04000E3E RID: 3646
			private bool _hasDetachedAnchorFrame;

			// Token: 0x04000E3F RID: 3647
			private bool _hasLowerTorso;

			// Token: 0x04000E40 RID: 3648
			private bool _detachedAnchor;

			// Token: 0x04000E41 RID: 3649
			private bool _switchArms;

			// Token: 0x04000E42 RID: 3650
			private bool _pitchFollowSet;

			// Token: 0x04000E43 RID: 3651
			private float _pitchFollow;

			// Token: 0x04000E44 RID: 3652
			private float _fovMultiplier;

			// Token: 0x04000E45 RID: 3653
			private float _bobbingAmplitude;

			// Token: 0x04000E46 RID: 3654
			private float _detachedAnchorFollow;

			// Token: 0x04000E47 RID: 3655
			private AnimationElement.Accumulator _rightItemAnchor;

			// Token: 0x04000E48 RID: 3656
			private AnimationElement.Accumulator _rightLowerArm;

			// Token: 0x04000E49 RID: 3657
			private AnimationElement.Accumulator _rightUpperArm;

			// Token: 0x04000E4A RID: 3658
			private AnimationElement.Accumulator _leftItemAnchor;

			// Token: 0x04000E4B RID: 3659
			private AnimationElement.Accumulator _leftLowerArm;

			// Token: 0x04000E4C RID: 3660
			private AnimationElement.Accumulator _leftUpperArm;

			// Token: 0x04000E4D RID: 3661
			private AnimationElement.Accumulator _neck;

			// Token: 0x04000E4E RID: 3662
			private AnimationElement.Accumulator _head;

			// Token: 0x04000E4F RID: 3663
			private AnimationElement.Accumulator _upperFootR;

			// Token: 0x04000E50 RID: 3664
			private AnimationElement.Accumulator _upperFootL;

			// Token: 0x04000E51 RID: 3665
			private AnimationElement.Accumulator _lowerFootR;

			// Token: 0x04000E52 RID: 3666
			private AnimationElement.Accumulator _lowerFootL;

			// Token: 0x04000E53 RID: 3667
			private AnimationElement.Accumulator _upperTorso;

			// Token: 0x04000E54 RID: 3668
			private AnimationElement.Accumulator _detachedAnchorFrame;

			// Token: 0x04000E55 RID: 3669
			private AnimationElement.Accumulator _lowerTorso;
		}
	}
}
