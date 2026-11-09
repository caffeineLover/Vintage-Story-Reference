using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Animations;
using CombatOverhaul.Integration;
using OpenTK.Mathematics;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;

namespace CombatOverhaul.RangedSystems.Aiming
{
	// Token: 0x020000EB RID: 235
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ClientAimingSystem : IDisposable
	{
		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000BEE RID: 3054 RVA: 0x0003C0C1 File Offset: 0x0003A2C1
		// (set) Token: 0x06000BEF RID: 3055 RVA: 0x0003C0C9 File Offset: 0x0003A2C9
		public bool Aiming { get; set; }

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000BF0 RID: 3056 RVA: 0x0003C0D2 File Offset: 0x0003A2D2
		// (set) Token: 0x06000BF1 RID: 3057 RVA: 0x0003C0DA File Offset: 0x0003A2DA
		public bool ShowVanillaReticle { get; private set; } = true;

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000BF2 RID: 3058 RVA: 0x0003C0E3 File Offset: 0x0003A2E3
		// (set) Token: 0x06000BF3 RID: 3059 RVA: 0x0003C0EB File Offset: 0x0003A2EB
		public bool ShowBullseyeReticle { get; private set; }

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000BF4 RID: 3060 RVA: 0x0003C0F4 File Offset: 0x0003A2F4
		// (set) Token: 0x06000BF5 RID: 3061 RVA: 0x0003C0FC File Offset: 0x0003A2FC
		public Vector3 TargetVec { get; private set; }

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000BF6 RID: 3062 RVA: 0x0003C105 File Offset: 0x0003A305
		// (set) Token: 0x06000BF7 RID: 3063 RVA: 0x0003C10D File Offset: 0x0003A30D
		public Vector2 Aim { get; private set; }

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000BF8 RID: 3064 RVA: 0x0003C116 File Offset: 0x0003A316
		// (set) Token: 0x06000BF9 RID: 3065 RVA: 0x0003C11E File Offset: 0x0003A31E
		public Vector2 AimOffset { get; private set; }

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000BFA RID: 3066 RVA: 0x0003C127 File Offset: 0x0003A327
		// (set) Token: 0x06000BFB RID: 3067 RVA: 0x0003C12F File Offset: 0x0003A32F
		public float DriftMultiplier { get; set; } = 1f;

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000BFC RID: 3068 RVA: 0x0003C138 File Offset: 0x0003A338
		// (set) Token: 0x06000BFD RID: 3069 RVA: 0x0003C140 File Offset: 0x0003A340
		public float TwitchMultiplier { get; set; } = 1f;

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000BFE RID: 3070 RVA: 0x0003C149 File Offset: 0x0003A349
		// (set) Token: 0x06000BFF RID: 3071 RVA: 0x0003C156 File Offset: 0x0003A356
		public WeaponAimingState AimingState
		{
			get
			{
				return this._reticleRenderer.AimingState;
			}
			set
			{
				this._reticleRenderer.AimingState = (this.ShowBullseyeReticle ? value : WeaponAimingState.None);
			}
		}

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x06000C00 RID: 3072 RVA: 0x0003C170 File Offset: 0x0003A370
		// (remove) Token: 0x06000C01 RID: 3073 RVA: 0x0003C1A8 File Offset: 0x0003A3A8
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event Action OnAimPointChange;

		// Token: 0x06000C02 RID: 3074 RVA: 0x0003C1E0 File Offset: 0x0003A3E0
		public ClientAimingSystem(ICoreClientAPI api, ReticleRenderer renderer)
		{
			this._clientApi = api;
			this._unprojectionTool = new Unproject();
			this.ResetAim();
			this._reticleRenderer = renderer;
			AimingPatches.UpdateCameraYawPitch += this.UpdateAimPoint;
			AimingPatches.AfterUpdateCameraYawPitch += new Action<ClientMain>(this.RefreshAimAfterCameraUpdate);
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x0003C2B8 File Offset: 0x0003A4B8
		public void StartAiming(AimingStats stats)
		{
			this._aimingStats = stats;
			if (this._clientApi.World.ElapsedMilliseconds - this._lastAimingEndTime > 5000L)
			{
				this.ResetAim();
			}
			else
			{
				this.ResetAimOffset();
			}
			if (this._aimingStats.CursorType == AimingCursorType.Moving || this._aimingStats.CursorType == AimingCursorType.MovingNone)
			{
				this.SetFixedAimPoint(this._clientApi.Render.FrameWidth, this._clientApi.Render.FrameHeight);
			}
			this.Aiming = true;
			this._aimingDt = 0f;
			this.ShowVanillaReticle = (this._aimingStats.CursorType == AimingCursorType.Vanilla);
			this.ShowBullseyeReticle = (this._aimingStats.CursorType != AimingCursorType.None && this._aimingStats.CursorType != AimingCursorType.Vanilla && this._aimingStats.CursorType != AimingCursorType.MovingNone);
			this._difficultyMultiplier = (this.ShowBullseyeReticle ? 1f : 0.75f);
			IClientPlayer player = this._clientApi.World.Player;
			if (player != null)
			{
				AimingAccuracyBehavior behavior = player.Entity.GetBehavior<AimingAccuracyBehavior>();
				if (behavior != null)
				{
					behavior.StartAim(stats);
				}
			}
			this._reticleRenderer.SetReticleTextures(stats.CursorTextureNotReady, stats.CursorTextureReady, "");
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x0003C3F8 File Offset: 0x0003A5F8
		public void StopAiming()
		{
			if (this.Aiming)
			{
				this.RefreshTargetVec();
			}
			this.Aiming = false;
			this._lastAimingEndTime = this._clientApi.World.ElapsedMilliseconds;
			this.ShowVanillaReticle = true;
			this._reticleRenderer.AimingState = WeaponAimingState.None;
			IClientPlayer player = this._clientApi.World.Player;
			if (player == null)
			{
				return;
			}
			AimingAccuracyBehavior behavior = player.Entity.GetBehavior<AimingAccuracyBehavior>();
			if (behavior == null)
			{
				return;
			}
			behavior.StopAim();
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x0003C46C File Offset: 0x0003A66C
		public Vector2 GetCurrentAim()
		{
			float offsetMagnitude = Math.Clamp(this._aimingStats.AimDifficulty, 0f, 2f) * this._difficultyMultiplier;
			IClientPlayer player = this._clientApi.World.Player;
			if (((player != null) ? player.Entity : null) != null)
			{
				offsetMagnitude /= GameMath.Max(this._clientApi.World.Player.Entity.Stats.GetBlended("rangedWeaponsAcc"), 0.001f);
			}
			float interpolation = GameMath.Sqrt(GameMath.Min(this._aimingDt / 0.3f, 1f));
			this._currentAim.X = (this.Aim.X + this.AimOffset.X * offsetMagnitude * this._aimingStats.HorizontalAccuracyMultiplier) * interpolation;
			this._currentAim.Y = (this.Aim.Y + this.AimOffset.Y * offsetMagnitude * this._aimingStats.VerticalAccuracyMultiplier) * interpolation;
			return this._currentAim;
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x0003C570 File Offset: 0x0003A770
		public void ResetAim()
		{
			this.Aim = new Vector2(0f, 0f);
			this.ResetAimOffset();
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x0003C590 File Offset: 0x0003A790
		public void UpdateAimPoint(ClientMain __instance, ref double ___MouseDeltaX, ref double ___MouseDeltaY, ref double ___DelayedMouseDeltaX, ref double ___DelayedMouseDeltaY, float dt)
		{
			AimingPatches.DrawDefaultReticle = this.ShowVanillaReticle;
			if (!this.Aiming)
			{
				return;
			}
			this._currentFovRatio = (float)__instance.Width / 1920f * (GameMath.Tan(0.61086524f) / GameMath.Tan(FirstPersonAnimationsBehavior.CurrentFov / 2f * 0.017453292f));
			this._aimingDt += dt;
			this.UpdateAimOffsetSimple(__instance, dt);
			if (this._aimingStats.CursorType == AimingCursorType.Moving || this._aimingStats.CursorType == AimingCursorType.MovingNone)
			{
				this.UpdateMouseDelta(__instance, ref ___MouseDeltaX, ref ___MouseDeltaY, ref ___DelayedMouseDeltaX, ref ___DelayedMouseDeltaY);
			}
			this.SetAim();
			this._reticleRenderer.AimingPoint = this.GetCurrentAim();
			Action onAimPointChange = this.OnAimPointChange;
			if (onAimPointChange == null)
			{
				return;
			}
			onAimPointChange.Invoke();
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x0003C64E File Offset: 0x0003A84E
		public void Dispose()
		{
			if (this._disposed)
			{
				return;
			}
			this._disposed = true;
			AimingPatches.UpdateCameraYawPitch -= this.UpdateAimPoint;
			AimingPatches.AfterUpdateCameraYawPitch -= new Action<ClientMain>(this.RefreshAimAfterCameraUpdate);
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x0003C682 File Offset: 0x0003A882
		public void RefreshTargetVec()
		{
			if (this.SetMountedAimFromPlayerView())
			{
				return;
			}
			this.SetAimFromRenderMatrices();
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x0003C694 File Offset: 0x0003A894
		public static Vector3 Zeroing(Vector3 direction, float ZeroingAngle)
		{
			Vector3 vertical;
			vertical..ctor(0f, 1f, 0f);
			Vector3 zeroingAxis = Vector3.Cross(direction, vertical);
			double[] matrix = Mat4d.Create();
			Mat4d.Rotate(matrix, matrix, (double)(ZeroingAngle * 0.017453292f), new double[]
			{
				(double)zeroingAxis.X,
				(double)zeroingAxis.Y,
				(double)zeroingAxis.Z
			});
			double[] array = new double[4];
			array[0] = (double)direction.X;
			array[1] = (double)direction.Y;
			array[2] = (double)direction.Z;
			double[] matrixVec = array;
			matrixVec = Mat4d.MulWithVec4(matrix, matrixVec);
			return new Vector3((float)matrixVec[0], (float)matrixVec[1], (float)matrixVec[2]);
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x0003C737 File Offset: 0x0003A937
		private void ResetAimOffset()
		{
			this.AimOffset = new Vector2(0f, 0f);
			this._twitch = new Vector2(0f, 0f);
			this.ShowVanillaReticle = true;
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x0003C76A File Offset: 0x0003A96A
		private void UpdateAimOffsetSimple(ClientMain __instance, float dt)
		{
			this.UpdateAimOffsetSimpleDrift(__instance, dt);
			this.UpdateAimOffsetSimpleTwitch(__instance, dt);
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x0003C77C File Offset: 0x0003A97C
		private void UpdateAimOffsetSimpleDrift(ClientMain __instance, float dt)
		{
			float xNoise = (float)this._noiseGenerator.Noise((double)((float)__instance.ElapsedMilliseconds * this._aimingStats.AimDriftFrequency), 1000.0) - 0.5f;
			float yNoise = (float)this._noiseGenerator.Noise(-1000.0, (double)((float)__instance.ElapsedMilliseconds * this._aimingStats.AimDriftFrequency)) - 0.5f;
			float maxDrift = GameMath.Max(this._aimingStats.AimDrift * 1.1f * this.DriftMultiplier, 1f) * this._currentFovRatio;
			float aimOffsetX = this.AimOffset.X + (xNoise - this.AimOffset.X / maxDrift) * this._aimingStats.AimDrift * this.DriftMultiplier * dt * this._currentFovRatio;
			float aimOffsetY = this.AimOffset.Y + (yNoise - this.AimOffset.Y / maxDrift) * this._aimingStats.AimDrift * this.DriftMultiplier * dt * this._currentFovRatio;
			this.AimOffset = new Vector2(aimOffsetX, aimOffsetY);
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x0003C894 File Offset: 0x0003AA94
		private void UpdateAimOffsetSimpleTwitch(ClientMain __instance, float dt)
		{
			float fovModAimOffsetX = this.AimOffset.X * this._currentFovRatio;
			float fovModAimOffsetY = this.AimOffset.Y * this._currentFovRatio;
			if (__instance.Api.World.ElapsedMilliseconds > this._twitchLastChangeMilliseconds + (long)this._aimingStats.AimTwitchDuration)
			{
				this._twitchLastChangeMilliseconds = __instance.Api.World.ElapsedMilliseconds;
				this._twitchLastStepMilliseconds = __instance.Api.World.ElapsedMilliseconds;
				float twitchMax = GameMath.Max(this._aimingStats.AimTwitch * 0.14285715f * this.TwitchMultiplier, 1f) * this._currentFovRatio;
				this._twitch.X = ((float)this._random.NextDouble() - 0.5f) * 2f * twitchMax - fovModAimOffsetX / twitchMax;
				this._twitch.Y = ((float)this._random.NextDouble() - 0.5f) * 2f * twitchMax - fovModAimOffsetY / twitchMax;
				float twitchLength = GameMath.Max(GameMath.Sqrt(this._twitch.X * this._twitch.X + this._twitch.Y * this._twitch.Y), 1f);
				this._twitch.X = this._twitch.X / twitchLength;
				this._twitch.Y = this._twitch.Y / twitchLength;
			}
			float lastStep = (float)(this._twitchLastStepMilliseconds - this._twitchLastChangeMilliseconds) / (float)this._aimingStats.AimTwitchDuration;
			float currentStep = (float)(__instance.Api.World.ElapsedMilliseconds - this._twitchLastChangeMilliseconds) / (float)this._aimingStats.AimTwitchDuration;
			float stepSize = (1f - lastStep) * (1f - lastStep) - (1f - currentStep) * (1f - currentStep);
			float aimOffsetX = this.AimOffset.X + this._twitch.X * stepSize * (this._aimingStats.AimTwitch * this.TwitchMultiplier * dt) * (float)(this._aimingStats.AimTwitchDuration / 20) * this._currentFovRatio;
			float aimOffsetY = this.AimOffset.Y + this._twitch.Y * stepSize * (this._aimingStats.AimTwitch * this.TwitchMultiplier * dt) * (float)(this._aimingStats.AimTwitchDuration / 20) * this._currentFovRatio;
			this.AimOffset = new Vector2(aimOffsetX, aimOffsetY);
			this._twitchLastStepMilliseconds = __instance.Api.World.ElapsedMilliseconds;
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x0003CB28 File Offset: 0x0003AD28
		private void UpdateMouseDelta(ClientMain __instance, ref double ___MouseDeltaX, ref double ___MouseDeltaY, ref double ___DelayedMouseDeltaX, ref double ___DelayedMouseDeltaY)
		{
			float horizontalAimLimit = (float)__instance.Width / 2f * this._aimingStats.HorizontalLimit;
			float verticalAimLimit = (float)__instance.Height / 2f * this._aimingStats.VerticalLimit;
			float verticalAimOffset = (float)__instance.Height / 2f * this._aimingStats.VerticalOffset;
			float yInversionFactor = (float)(this._aimingStats.InvertMouseYAxis ? -1 : 1);
			float deltaX = (float)(___MouseDeltaX - ___DelayedMouseDeltaX);
			float deltaY = (float)(___MouseDeltaY - ___DelayedMouseDeltaY) * yInversionFactor;
			float aimX = this.Aim.X;
			float aimY = this.Aim.Y;
			if (Math.Abs(aimX + deltaX) > horizontalAimLimit)
			{
				aimX = ((aimX > 0f) ? horizontalAimLimit : (-horizontalAimLimit));
			}
			else
			{
				aimX += deltaX;
				___DelayedMouseDeltaX = ___MouseDeltaX;
			}
			if (Math.Abs(aimY + deltaY - verticalAimOffset) > verticalAimLimit)
			{
				aimY = ((aimY > 0f) ? verticalAimLimit : (-verticalAimLimit)) + verticalAimOffset;
			}
			else
			{
				aimY += deltaY;
				___DelayedMouseDeltaY = ___MouseDeltaY;
			}
			this.Aim = new Vector2(aimX, aimY);
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x0003CC30 File Offset: 0x0003AE30
		private void SetFixedAimPoint(int screenWidth, int screenHeight)
		{
			float difficultyModifier = GameMath.Clamp(this._aimingStats.AimDifficulty, 0f, 2f) * this._difficultyMultiplier;
			float horizontalLimit = GameMath.Min(this._aimingStats.HorizontalLimit, 0.25f);
			float verticalLimit = GameMath.Min(this._aimingStats.VerticalLimit, 0.25f);
			float verticalOffset = GameMath.Clamp(this._aimingStats.VerticalOffset, -0.05f, 0.1f);
			float horizontalAimLimit = (float)screenWidth / 2f * horizontalLimit;
			float verticalAimLimit = (float)screenHeight / 2f * verticalLimit;
			float verticalAimOffset = (float)screenHeight / 2f * verticalOffset;
			float maxHorizontalShift = horizontalAimLimit / 2.25f;
			float maxVerticalShift = verticalAimLimit / 2.25f;
			maxHorizontalShift = GameMath.Max(maxHorizontalShift, GameMath.Min(maxVerticalShift, horizontalAimLimit));
			maxVerticalShift = GameMath.Max(maxVerticalShift, GameMath.Min(maxHorizontalShift, verticalAimLimit));
			float num = GameMath.Clamp(this.Aim.X, -maxHorizontalShift, maxHorizontalShift);
			float verticalCenter = GameMath.Clamp(this.Aim.Y, -maxVerticalShift, maxVerticalShift);
			float aimX = (num + (-maxHorizontalShift + (float)this._random.NextDouble() * maxHorizontalShift * 2f)) * difficultyModifier;
			float aimY = (verticalCenter + (-maxVerticalShift + (float)this._random.NextDouble() * maxVerticalShift * 2f) + verticalAimOffset) * difficultyModifier;
			this.Aim = new Vector2(aimX, aimY);
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x0003CD7E File Offset: 0x0003AF7E
		private void RefreshAimAfterCameraUpdate(ClientMain __instance)
		{
			if (!this.Aiming)
			{
				return;
			}
			this.RefreshTargetVec();
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x0003CD8F File Offset: 0x0003AF8F
		private void SetAim()
		{
			this.SetAimFromRenderMatrices();
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x0003CD98 File Offset: 0x0003AF98
		private bool SetMountedAimFromPlayerView()
		{
			IClientWorldAccessor world = this._clientApi.World;
			EntityPlayer entityPlayer;
			if (world == null)
			{
				entityPlayer = null;
			}
			else
			{
				IClientPlayer player2 = world.Player;
				entityPlayer = ((player2 != null) ? player2.Entity : null);
			}
			EntityPlayer player = entityPlayer;
			if (((player != null) ? player.MountedOn : null) == null)
			{
				return false;
			}
			Vector2 currentAim = this.GetCurrentAim();
			float pitch = player.Pos.Pitch;
			float yaw = player.Pos.Yaw;
			float cosPitch = GameMath.Cos(pitch);
			Vector3 forward;
			forward..ctor(-cosPitch * GameMath.Sin(yaw), GameMath.Sin(pitch), -cosPitch * GameMath.Cos(yaw));
			if (forward.LengthSquared < 1E-06f)
			{
				return false;
			}
			forward = Vector3.Normalize(forward);
			Vector3 worldUp = Vector3.UnitY;
			Vector3 right = Vector3.Cross(forward, worldUp);
			if (right.LengthSquared < 1E-06f)
			{
				right = Vector3.UnitX;
			}
			else
			{
				right = Vector3.Normalize(right);
			}
			Vector3 up = Vector3.Cross(right, forward);
			if (up.LengthSquared < 1E-06f)
			{
				up = worldUp;
			}
			else
			{
				up = Vector3.Normalize(up);
			}
			int frameWidth = Math.Max(this._clientApi.Render.FrameWidth, 1);
			int frameHeight = Math.Max(this._clientApi.Render.FrameHeight, 1);
			float verticalTan = MathF.Tan(Math.Clamp(FirstPersonAnimationsBehavior.CurrentFov, 1f, 179f) * 0.017453292f / 2f);
			float horizontalTan = verticalTan * (float)frameWidth / (float)frameHeight;
			float xOffset = currentAim.X / ((float)frameWidth / 2f) * horizontalTan;
			float yOffset = -currentAim.Y / ((float)frameHeight / 2f) * verticalTan;
			Vector3 direction = forward + right * xOffset + up * yOffset;
			if (direction.LengthSquared < 1E-06f)
			{
				return false;
			}
			this.TargetVec = Vector3.Normalize(direction);
			return true;
		}

		// Token: 0x06000C14 RID: 3092 RVA: 0x0003CF60 File Offset: 0x0003B160
		private void SetAimFromRenderMatrices()
		{
			Vector2 currentAim = this.GetCurrentAim();
			int mouseCurrentX = (int)currentAim.X + this._clientApi.Render.FrameWidth / 2;
			int mouseCurrentY = (int)currentAim.Y + this._clientApi.Render.FrameHeight / 2;
			this._viewport[0] = 0.0;
			this._viewport[1] = 0.0;
			this._viewport[2] = (double)this._clientApi.Render.FrameWidth;
			this._viewport[3] = (double)this._clientApi.Render.FrameHeight;
			if (!(true | this._unprojectionTool.UnProject(mouseCurrentX, this._clientApi.Render.FrameHeight - mouseCurrentY, 1, this._clientApi.Render.MvMatrix.Top, this._clientApi.Render.PMatrix.Top, this._viewport, this._rayEnd) | this._unprojectionTool.UnProject(mouseCurrentX, this._clientApi.Render.FrameHeight - mouseCurrentY, 0, this._clientApi.Render.MvMatrix.Top, this._clientApi.Render.PMatrix.Top, this._viewport, this._rayStart)))
			{
				return;
			}
			double offsetX = this._rayEnd[0] - this._rayStart[0];
			double offsetY = this._rayEnd[1] - this._rayStart[1];
			double offsetZ = this._rayEnd[2] - this._rayStart[2];
			float length = (float)Math.Sqrt(offsetX * offsetX + offsetY * offsetY + offsetZ * offsetZ);
			if (length == 0f)
			{
				return;
			}
			offsetX /= (double)length;
			offsetY /= (double)length;
			offsetZ /= (double)length;
			this.TargetVec = new Vector3((float)offsetX, (float)offsetY, (float)offsetZ);
		}

		// Token: 0x040005B0 RID: 1456
		private readonly NormalizedSimplexNoise _noiseGenerator = NormalizedSimplexNoise.FromDefaultOctaves(4, 1.0, 0.9, 123L);

		// Token: 0x040005B1 RID: 1457
		private readonly Random _random = new Random();

		// Token: 0x040005B2 RID: 1458
		private readonly ICoreClientAPI _clientApi;

		// Token: 0x040005B3 RID: 1459
		private readonly ReticleRenderer _reticleRenderer;

		// Token: 0x040005B4 RID: 1460
		private readonly Unproject _unprojectionTool;

		// Token: 0x040005B5 RID: 1461
		private AimingStats _aimingStats = new AimingStats();

		// Token: 0x040005B6 RID: 1462
		private long _twitchLastChangeMilliseconds;

		// Token: 0x040005B7 RID: 1463
		private long _twitchLastStepMilliseconds;

		// Token: 0x040005B8 RID: 1464
		private Vector2 _twitch;

		// Token: 0x040005B9 RID: 1465
		private readonly double[] _viewport = new double[4];

		// Token: 0x040005BA RID: 1466
		private readonly double[] _rayStart = new double[4];

		// Token: 0x040005BB RID: 1467
		private readonly double[] _rayEnd = new double[4];

		// Token: 0x040005BC RID: 1468
		private float _aimingDt;

		// Token: 0x040005BD RID: 1469
		private long _lastAimingEndTime;

		// Token: 0x040005BE RID: 1470
		private const long _aimResetTime = 5000L;

		// Token: 0x040005BF RID: 1471
		private const float _aimStartInterpolationTime = 0.3f;

		// Token: 0x040005C0 RID: 1472
		private Vector2 _currentAim;

		// Token: 0x040005C1 RID: 1473
		private float _currentFovRatio;

		// Token: 0x040005C2 RID: 1474
		private float _difficultyMultiplier = 1f;

		// Token: 0x040005C3 RID: 1475
		private const float _noCursorDifficultyMultiplier = 0.75f;

		// Token: 0x040005C4 RID: 1476
		private bool _disposed;
	}
}
