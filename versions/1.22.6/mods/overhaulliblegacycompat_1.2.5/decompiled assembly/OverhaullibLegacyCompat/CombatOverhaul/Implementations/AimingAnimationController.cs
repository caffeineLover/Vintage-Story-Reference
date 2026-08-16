using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Animations;
using CombatOverhaul.RangedSystems.Aiming;
using OpenTK.Mathematics;
using Vintagestory.Client.NoObf;

namespace CombatOverhaul.Implementations
{
	// Token: 0x02000098 RID: 152
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class AimingAnimationController
	{
		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000839 RID: 2105 RVA: 0x0002FC53 File Offset: 0x0002DE53
		// (set) Token: 0x0600083A RID: 2106 RVA: 0x0002FC5B File Offset: 0x0002DE5B
		public AimingStats Stats { get; set; }

		// Token: 0x0600083B RID: 2107 RVA: 0x0002FC64 File Offset: 0x0002DE64
		public AimingAnimationController(ClientAimingSystem aimingSystem, [Nullable(2)] FirstPersonAnimationsBehavior animationBehavior, AimingStats stats)
		{
			this._aimingSystem = aimingSystem;
			this._animationBehavior = animationBehavior;
			this.Stats = stats;
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x0002FCB8 File Offset: 0x0002DEB8
		public void Play(bool mainHand)
		{
			AnimationRequest request = new AnimationRequest(this._cursorFollowAnimation, 1f, 0f, "aiming", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), true, null, null);
			FirstPersonAnimationsBehavior animationBehavior = this._animationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.Play(request, mainHand);
			}
			this._aimingSystem.OnAimPointChange += new Action(this.UpdateCursorFollowAnimation);
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x0002FD2C File Offset: 0x0002DF2C
		public void Stop(bool mainHand)
		{
			this._cursorStopFollowAnimation.PlayerKeyFrames[0] = new PLayerKeyFrame(PlayerFrame.Zero, TimeSpan.FromMilliseconds(500L), EasingFunctionType.CosShifted);
			AnimationRequest request = new AnimationRequest(this._cursorStopFollowAnimation, 1f, 0f, "aiming", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.2), true, null, null);
			FirstPersonAnimationsBehavior animationBehavior = this._animationBehavior;
			if (animationBehavior != null)
			{
				animationBehavior.Play(request, mainHand);
			}
			this._aimingSystem.OnAimPointChange -= new Action(this.UpdateCursorFollowAnimation);
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0002FDC8 File Offset: 0x0002DFC8
		private PLayerKeyFrame GetAimingFrame()
		{
			Vector2 currentAim = this._aimingSystem.GetCurrentAim();
			float fovAdjustment = 1f - MathF.Cos((float)ClientSettings.FieldOfView * 0.017453292f) * this._fovMultiplier;
			float yaw = 0f - currentAim.X * 0.01f * this.Stats.AnimationFollowX * fovAdjustment + this.Stats.AnimationOffsetX;
			float pitch = currentAim.Y * 0.01f * this.Stats.AnimationFollowY * fovAdjustment + this.Stats.AnimationOffsetY;
			AnimationElement element = new AnimationElement(new float?(0f), new float?(0f), new float?(0f), new float?(0f), new float?(yaw), new float?(pitch));
			AnimationElement? upperTorso = new AnimationElement?(element);
			return new PLayerKeyFrame(new PlayerFrame(default(RightHandFrame?), default(LeftHandFrame?), default(OtherPartsFrame?), upperTorso, default(AnimationElement?), false, false, 0.8f, 1f, 1f, default(float?), default(AnimationElement?)), TimeSpan.Zero, EasingFunctionType.Linear);
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0002FEF7 File Offset: 0x0002E0F7
		private void UpdateCursorFollowAnimation()
		{
			this._cursorFollowAnimation.PlayerKeyFrames[0] = this.GetAimingFrame();
			this._cursorFollowAnimation.Hold = true;
		}

		// Token: 0x040003BA RID: 954
		private readonly Animation _cursorFollowAnimation = Animation.Zero.Clone();

		// Token: 0x040003BB RID: 955
		private readonly Animation _cursorStopFollowAnimation = Animation.Zero.Clone();

		// Token: 0x040003BC RID: 956
		private const float _animationFollowMultiplier = 0.01f;

		// Token: 0x040003BD RID: 957
		private readonly ClientAimingSystem _aimingSystem;

		// Token: 0x040003BE RID: 958
		[Nullable(2)]
		private readonly FirstPersonAnimationsBehavior _animationBehavior;

		// Token: 0x040003BF RID: 959
		private float _fovMultiplier = 0.79f;
	}
}
