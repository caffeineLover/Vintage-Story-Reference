using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace CombatOverhaul.Animations
{
	// Token: 0x02000183 RID: 387
	public readonly struct PlayerFrame
	{
		// Token: 0x060012FF RID: 4863 RVA: 0x0005D218 File Offset: 0x0005B418
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

		// Token: 0x06001300 RID: 4864 RVA: 0x0005D308 File Offset: 0x0005B508
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

		// Token: 0x06001301 RID: 4865 RVA: 0x0005D580 File Offset: 0x0005B780
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

		// Token: 0x06001302 RID: 4866 RVA: 0x0005D920 File Offset: 0x0005BB20
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

		// Token: 0x06001303 RID: 4867 RVA: 0x0005D988 File Offset: 0x0005BB88
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

		// Token: 0x0400094D RID: 2381
		public readonly RightHandFrame? RightHand;

		// Token: 0x0400094E RID: 2382
		public readonly LeftHandFrame? LeftHand;

		// Token: 0x0400094F RID: 2383
		public readonly OtherPartsFrame? OtherParts;

		// Token: 0x04000950 RID: 2384
		public readonly AnimationElement? UpperTorso;

		// Token: 0x04000951 RID: 2385
		public readonly AnimationElement? DetachedAnchorFrame;

		// Token: 0x04000952 RID: 2386
		public readonly AnimationElement? LowerTorso;

		// Token: 0x04000953 RID: 2387
		public readonly bool DetachedAnchor;

		// Token: 0x04000954 RID: 2388
		public readonly bool SwitchArms;

		// Token: 0x04000955 RID: 2389
		public readonly float PitchFollow;

		// Token: 0x04000956 RID: 2390
		public readonly float FovMultiplier;

		// Token: 0x04000957 RID: 2391
		public readonly float BobbingAmplitude;

		// Token: 0x04000958 RID: 2392
		public readonly float DetachedAnchorFollow;

		// Token: 0x04000959 RID: 2393
		public const float DefaultPitchFollow = 0.8f;

		// Token: 0x0400095A RID: 2394
		public const float PerfectPitchFollow = 1f;

		// Token: 0x0400095B RID: 2395
		public const float Epsilon = 1E-06f;

		// Token: 0x0400095C RID: 2396
		public const float EyeHeightToAnimationDistanceMultiplier = 16.1f;

		// Token: 0x0400095D RID: 2397
		public const float PitchAngleMin = -45f;

		// Token: 0x0400095E RID: 2398
		public const float PitchAngleMax = 75f;

		// Token: 0x0400095F RID: 2399
		public static readonly PlayerFrame Zero = new PlayerFrame(new RightHandFrame?(RightHandFrame.Zero), new LeftHandFrame?(LeftHandFrame.Zero), new OtherPartsFrame?(OtherPartsFrame.Zero), default(AnimationElement?), default(AnimationElement?), false, false, 0.8f, 1f, 1f, default(float?), default(AnimationElement?));

		// Token: 0x04000960 RID: 2400
		public static readonly PlayerFrame Empty = new PlayerFrame(default(RightHandFrame?), default(LeftHandFrame?), default(OtherPartsFrame?), default(AnimationElement?), default(AnimationElement?), false, false, 0.8f, 1f, 1f, default(float?), default(AnimationElement?));

		// Token: 0x020002C2 RID: 706
		private struct PlayerFrameComposition
		{
			// Token: 0x06001872 RID: 6258 RVA: 0x0006CFEC File Offset: 0x0006B1EC
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

			// Token: 0x06001873 RID: 6259 RVA: 0x0006D290 File Offset: 0x0006B490
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

			// Token: 0x04000DED RID: 3565
			private bool _hasAny;

			// Token: 0x04000DEE RID: 3566
			private bool _hasRightHand;

			// Token: 0x04000DEF RID: 3567
			private bool _hasLeftHand;

			// Token: 0x04000DF0 RID: 3568
			private bool _hasOtherParts;

			// Token: 0x04000DF1 RID: 3569
			private bool _hasUpperTorso;

			// Token: 0x04000DF2 RID: 3570
			private bool _hasDetachedAnchorFrame;

			// Token: 0x04000DF3 RID: 3571
			private bool _hasLowerTorso;

			// Token: 0x04000DF4 RID: 3572
			private bool _detachedAnchor;

			// Token: 0x04000DF5 RID: 3573
			private bool _switchArms;

			// Token: 0x04000DF6 RID: 3574
			private bool _pitchFollowSet;

			// Token: 0x04000DF7 RID: 3575
			private float _pitchFollow;

			// Token: 0x04000DF8 RID: 3576
			private float _fovMultiplier;

			// Token: 0x04000DF9 RID: 3577
			private float _bobbingAmplitude;

			// Token: 0x04000DFA RID: 3578
			private float _detachedAnchorFollow;

			// Token: 0x04000DFB RID: 3579
			private AnimationElement.Accumulator _rightItemAnchor;

			// Token: 0x04000DFC RID: 3580
			private AnimationElement.Accumulator _rightLowerArm;

			// Token: 0x04000DFD RID: 3581
			private AnimationElement.Accumulator _rightUpperArm;

			// Token: 0x04000DFE RID: 3582
			private AnimationElement.Accumulator _leftItemAnchor;

			// Token: 0x04000DFF RID: 3583
			private AnimationElement.Accumulator _leftLowerArm;

			// Token: 0x04000E00 RID: 3584
			private AnimationElement.Accumulator _leftUpperArm;

			// Token: 0x04000E01 RID: 3585
			private AnimationElement.Accumulator _neck;

			// Token: 0x04000E02 RID: 3586
			private AnimationElement.Accumulator _head;

			// Token: 0x04000E03 RID: 3587
			private AnimationElement.Accumulator _upperFootR;

			// Token: 0x04000E04 RID: 3588
			private AnimationElement.Accumulator _upperFootL;

			// Token: 0x04000E05 RID: 3589
			private AnimationElement.Accumulator _lowerFootR;

			// Token: 0x04000E06 RID: 3590
			private AnimationElement.Accumulator _lowerFootL;

			// Token: 0x04000E07 RID: 3591
			private AnimationElement.Accumulator _upperTorso;

			// Token: 0x04000E08 RID: 3592
			private AnimationElement.Accumulator _detachedAnchorFrame;

			// Token: 0x04000E09 RID: 3593
			private AnimationElement.Accumulator _lowerTorso;
		}
	}
}
