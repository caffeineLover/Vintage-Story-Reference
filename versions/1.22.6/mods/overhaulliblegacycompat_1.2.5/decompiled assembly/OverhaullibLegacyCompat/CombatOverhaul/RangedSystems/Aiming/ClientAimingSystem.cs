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
	// Token: 0x020000E0 RID: 224
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ClientAimingSystem : IDisposable
	{
		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000B92 RID: 2962 RVA: 0x0003B0D1 File Offset: 0x000392D1
		// (set) Token: 0x06000B93 RID: 2963 RVA: 0x0003B0D9 File Offset: 0x000392D9
		public bool Aiming { get; set; }

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000B94 RID: 2964 RVA: 0x0003B0E2 File Offset: 0x000392E2
		// (set) Token: 0x06000B95 RID: 2965 RVA: 0x0003B0EA File Offset: 0x000392EA
		public bool ShowVanillaReticle { get; private set; } = true;

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000B96 RID: 2966 RVA: 0x0003B0F3 File Offset: 0x000392F3
		// (set) Token: 0x06000B97 RID: 2967 RVA: 0x0003B0FB File Offset: 0x000392FB
		public bool ShowBullseyeReticle { get; private set; }

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000B98 RID: 2968 RVA: 0x0003B104 File Offset: 0x00039304
		// (set) Token: 0x06000B99 RID: 2969 RVA: 0x0003B10C File Offset: 0x0003930C
		public Vector3 TargetVec { get; private set; }

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000B9A RID: 2970 RVA: 0x0003B115 File Offset: 0x00039315
		// (set) Token: 0x06000B9B RID: 2971 RVA: 0x0003B11D File Offset: 0x0003931D
		public Vector2 Aim { get; private set; }

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000B9C RID: 2972 RVA: 0x0003B126 File Offset: 0x00039326
		// (set) Token: 0x06000B9D RID: 2973 RVA: 0x0003B12E File Offset: 0x0003932E
		public Vector2 AimOffset { get; private set; }

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000B9E RID: 2974 RVA: 0x0003B137 File Offset: 0x00039337
		// (set) Token: 0x06000B9F RID: 2975 RVA: 0x0003B13F File Offset: 0x0003933F
		public float DriftMultiplier { get; set; } = 1f;

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000BA0 RID: 2976 RVA: 0x0003B148 File Offset: 0x00039348
		// (set) Token: 0x06000BA1 RID: 2977 RVA: 0x0003B150 File Offset: 0x00039350
		public float TwitchMultiplier { get; set; } = 1f;

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x0003B159 File Offset: 0x00039359
		// (set) Token: 0x06000BA3 RID: 2979 RVA: 0x0003B166 File Offset: 0x00039366
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
		// (add) Token: 0x06000BA4 RID: 2980 RVA: 0x0003B180 File Offset: 0x00039380
		// (remove) Token: 0x06000BA5 RID: 2981 RVA: 0x0003B1B8 File Offset: 0x000393B8
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public event Action OnAimPointChange;

		// Token: 0x06000BA6 RID: 2982 RVA: 0x0003B1F0 File Offset: 0x000393F0
		public ClientAimingSystem(ICoreClientAPI api, ReticleRenderer renderer)
		{
			this._clientApi = api;
			this._unprojectionTool = new Unproject();
			this.ResetAim();
			this._reticleRenderer = renderer;
			AimingPatches.UpdateCameraYawPitch += this.UpdateAimPoint;
			AimingPatches.AfterUpdateCameraYawPitch += new Action<ClientMain>(this.RefreshAimAfterCameraUpdate);
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x0003B2C8 File Offset: 0x000394C8
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

		// Token: 0x06000BA8 RID: 2984 RVA: 0x0003B408 File Offset: 0x00039608
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

		// Token: 0x06000BA9 RID: 2985 RVA: 0x0003B47C File Offset: 0x0003967C
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

		// Token: 0x06000BAA RID: 2986 RVA: 0x0003B580 File Offset: 0x00039780
		public void ResetAim()
		{
			this.Aim = new Vector2(0f, 0f);
			this.ResetAimOffset();
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x0003B5A0 File Offset: 0x000397A0
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

		// Token: 0x06000BAC RID: 2988 RVA: 0x0003B65E File Offset: 0x0003985E
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

		// Token: 0x06000BAD RID: 2989 RVA: 0x0003B692 File Offset: 0x00039892
		public void RefreshTargetVec()
		{
			if (this.SetMountedAimFromPlayerView())
			{
				return;
			}
			this.SetAimFromRenderMatrices();
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x0003B6A4 File Offset: 0x000398A4
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

		// Token: 0x06000BAF RID: 2991 RVA: 0x0003B747 File Offset: 0x00039947
		private void ResetAimOffset()
		{
			this.AimOffset = new Vector2(0f, 0f);
			this._twitch = new Vector2(0f, 0f);
			this.ShowVanillaReticle = true;
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x0003B77A File Offset: 0x0003997A
		private void UpdateAimOffsetSimple(ClientMain __instance, float dt)
		{
			this.UpdateAimOffsetSimpleDrift(__instance, dt);
			this.UpdateAimOffsetSimpleTwitch(__instance, dt);
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x0003B78C File Offset: 0x0003998C
		private void UpdateAimOffsetSimpleDrift(ClientMain __instance, float dt)
		{
			float xNoise = (float)this._noiseGenerator.Noise((double)((float)__instance.ElapsedMilliseconds * this._aimingStats.AimDriftFrequency), 1000.0) - 0.5f;
			float yNoise = (float)this._noiseGenerator.Noise(-1000.0, (double)((float)__instance.ElapsedMilliseconds * this._aimingStats.AimDriftFrequency)) - 0.5f;
			float maxDrift = GameMath.Max(this._aimingStats.AimDrift * 1.1f * this.DriftMultiplier, 1f) * this._currentFovRatio;
			float aimOffsetX = this.AimOffset.X + (xNoise - this.AimOffset.X / maxDrift) * this._aimingStats.AimDrift * this.DriftMultiplier * dt * this._currentFovRatio;
			float aimOffsetY = this.AimOffset.Y + (yNoise - this.AimOffset.Y / maxDrift) * this._aimingStats.AimDrift * this.DriftMultiplier * dt * this._currentFovRatio;
			this.AimOffset = new Vector2(aimOffsetX, aimOffsetY);
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x0003B8A4 File Offset: 0x00039AA4
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

		// Token: 0x06000BB3 RID: 2995 RVA: 0x0003BB38 File Offset: 0x00039D38
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

		// Token: 0x06000BB4 RID: 2996 RVA: 0x0003BC40 File Offset: 0x00039E40
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

		// Token: 0x06000BB5 RID: 2997 RVA: 0x0003BD8E File Offset: 0x00039F8E
		private void RefreshAimAfterCameraUpdate(ClientMain __instance)
		{
			if (!this.Aiming)
			{
				return;
			}
			this.RefreshTargetVec();
		}

		// Token: 0x06000BB6 RID: 2998 RVA: 0x0003BD9F File Offset: 0x00039F9F
		private void SetAim()
		{
			this.SetAimFromRenderMatrices();
		}

		// Token: 0x06000BB7 RID: 2999 RVA: 0x0003BDA8 File Offset: 0x00039FA8
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

		// Token: 0x06000BB8 RID: 3000 RVA: 0x0003BF70 File Offset: 0x0003A170
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

		// Token: 0x0400058E RID: 1422
		private readonly NormalizedSimplexNoise _noiseGenerator = NormalizedSimplexNoise.FromDefaultOctaves(4, 1.0, 0.9, 123L);

		// Token: 0x0400058F RID: 1423
		private readonly Random _random = new Random();

		// Token: 0x04000590 RID: 1424
		private readonly ICoreClientAPI _clientApi;

		// Token: 0x04000591 RID: 1425
		private readonly ReticleRenderer _reticleRenderer;

		// Token: 0x04000592 RID: 1426
		private readonly Unproject _unprojectionTool;

		// Token: 0x04000593 RID: 1427
		private AimingStats _aimingStats = new AimingStats();

		// Token: 0x04000594 RID: 1428
		private long _twitchLastChangeMilliseconds;

		// Token: 0x04000595 RID: 1429
		private long _twitchLastStepMilliseconds;

		// Token: 0x04000596 RID: 1430
		private Vector2 _twitch;

		// Token: 0x04000597 RID: 1431
		private readonly double[] _viewport = new double[4];

		// Token: 0x04000598 RID: 1432
		private readonly double[] _rayStart = new double[4];

		// Token: 0x04000599 RID: 1433
		private readonly double[] _rayEnd = new double[4];

		// Token: 0x0400059A RID: 1434
		private float _aimingDt;

		// Token: 0x0400059B RID: 1435
		private long _lastAimingEndTime;

		// Token: 0x0400059C RID: 1436
		private const long _aimResetTime = 5000L;

		// Token: 0x0400059D RID: 1437
		private const float _aimStartInterpolationTime = 0.3f;

		// Token: 0x0400059E RID: 1438
		private Vector2 _currentAim;

		// Token: 0x0400059F RID: 1439
		private float _currentFovRatio;

		// Token: 0x040005A0 RID: 1440
		private float _difficultyMultiplier = 1f;

		// Token: 0x040005A1 RID: 1441
		private const float _noCursorDifficultyMultiplier = 0.75f;

		// Token: 0x040005A2 RID: 1442
		private bool _disposed;
	}
}
