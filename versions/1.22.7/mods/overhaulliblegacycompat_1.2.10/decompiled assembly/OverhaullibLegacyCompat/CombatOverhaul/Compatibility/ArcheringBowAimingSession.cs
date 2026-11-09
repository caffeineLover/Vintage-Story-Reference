using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Integration;
using OpenTK.Mathematics;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;

namespace CombatOverhaul.Compatibility
{
	// Token: 0x02000160 RID: 352
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class ArcheringBowAimingSession : IRenderer, IDisposable
	{
		// Token: 0x060011AD RID: 4525 RVA: 0x00054338 File Offset: 0x00052538
		public ArcheringBowAimingSession(ICoreClientAPI api)
		{
			this._api = api;
			this._chargedReticle = new LoadedTexture(api);
			api.Render.GetOrLoadTexture(new AssetLocation("combatoverhaul", "gui/aiming/default-full.png"), ref this._chargedReticle);
			this._breathHud = new ArcheringBowBreathHud(api);
			this._breathHud.TryOpen();
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x060011AE RID: 4526 RVA: 0x000543B7 File Offset: 0x000525B7
		public double RenderOrder
		{
			get
			{
				return 0.98;
			}
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x060011AF RID: 4527 RVA: 0x000543C2 File Offset: 0x000525C2
		public int RenderRange
		{
			get
			{
				return 9999;
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x060011B0 RID: 4528 RVA: 0x000543C9 File Offset: 0x000525C9
		public bool Active
		{
			get
			{
				return this._settings != null;
			}
		}

		// Token: 0x060011B1 RID: 4529 RVA: 0x000543D4 File Offset: 0x000525D4
		public void Start(ArcheringBowAimingSettings settings, EntityPlayer player, Func<bool> shouldContinue)
		{
			this.Stop();
			this._settings = settings;
			this._player = player;
			this._shouldContinue = shouldContinue;
			this._charged = false;
			this._flashUntilMs = 0L;
			this._startMs = this._api.World.ElapsedMilliseconds;
			this._oldPos = Vector2.Zero;
			this._stanceMultiplier = 0f;
			float drawWeight = Math.Max(settings.DrawWeight, 0.05f);
			float intensity = 3f * ArcheringBowAimingSession.Rational(settings.DrawTime, 2.5f, 3.75f) * 0.015f * MathF.Pow(drawWeight + 0.5f, settings.DrawWeightChargePower) * 1.35f * settings.ChargeAmplitude;
			float minAngle = 0.7330383f;
			float maxAngle = 0.83775806f;
			float angle = minAngle + (float)this._random.NextDouble() * (maxAngle - minAngle);
			this._targetOffset = new Vector2(-MathF.Cos(angle) * intensity, -MathF.Sin(angle) * intensity);
			this._controlOffset = new Vector2(this._targetOffset.X / 2f + this._targetOffset.Y * 1.2f * 0.9f, this._targetOffset.Y / 2f - this._targetOffset.X * 1.2f * 1.2f * MathF.Sqrt(drawWeight + 1.5f) * 1.1f);
			AimingPatches.UpdateCameraYawPitch += this.OnCameraYawPitch;
			this._cameraHookRegistered = true;
			if (settings.EnableArcheringChargedReticle)
			{
				this._api.Event.RegisterRenderer(this, 10, null);
				this._reticleRegistered = true;
			}
		}

		// Token: 0x060011B2 RID: 4530 RVA: 0x00054574 File Offset: 0x00052774
		public void Stop()
		{
			if (this._cameraHookRegistered)
			{
				AimingPatches.UpdateCameraYawPitch -= this.OnCameraYawPitch;
				this._cameraHookRegistered = false;
			}
			if (this._reticleRegistered)
			{
				this._api.Event.UnregisterRenderer(this, 10);
				this._reticleRegistered = false;
			}
			this._settings = null;
			this._player = null;
			this._shouldContinue = null;
			this._isHoldingBreath = false;
			this._charged = false;
			this._breathHud.UpdateBar(1f, 1f, false, false);
		}

		// Token: 0x060011B3 RID: 4531 RVA: 0x000545FD File Offset: 0x000527FD
		public void Dispose()
		{
			this.Stop();
			this._chargedReticle.Dispose();
			this._breathHud.TryClose();
			this._breathHud.Dispose();
		}

		// Token: 0x060011B4 RID: 4532 RVA: 0x00054627 File Offset: 0x00052827
		public void SetCharged()
		{
			if (this._charged)
			{
				return;
			}
			this._charged = true;
			this._flashUntilMs = this._api.World.ElapsedMilliseconds + 250L;
		}

		// Token: 0x060011B5 RID: 4533 RVA: 0x00054656 File Offset: 0x00052856
		public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
		{
			if (this._settings == null || this._player == null || stage != 10)
			{
				return;
			}
			this.RenderChargeFlash();
		}

		// Token: 0x060011B6 RID: 4534 RVA: 0x00054674 File Offset: 0x00052874
		private void OnCameraYawPitch(ClientMain __instance, ref double mouseDeltaX, ref double mouseDeltaY, ref double delayedMouseDeltaX, ref double delayedMouseDeltaY, float dt)
		{
			if (this._settings == null || this._player == null)
			{
				return;
			}
			if (!this._api.IsGamePaused)
			{
				Func<bool> shouldContinue = this._shouldContinue;
				if (shouldContinue != null && shouldContinue.Invoke())
				{
					this.ApplyHoldBreath(dt);
					this.ApplyCameraSway(dt);
					return;
				}
			}
			this.Stop();
		}

		// Token: 0x060011B7 RID: 4535 RVA: 0x000546D0 File Offset: 0x000528D0
		private void RenderChargeFlash()
		{
			if (this._api.World.ElapsedMilliseconds > this._flashUntilMs)
			{
				return;
			}
			float scale = RuntimeEnv.GUIScale;
			this._api.Render.Render2DTexture(this._chargedReticle.TextureId, (float)this._api.Render.FrameWidth / 2f - (float)this._chargedReticle.Width * scale / 2f, (float)this._api.Render.FrameHeight / 2f - (float)this._chargedReticle.Height * scale / 2f, (float)this._chargedReticle.Width * scale, (float)this._chargedReticle.Height * scale, 10000f, null);
		}

		// Token: 0x060011B8 RID: 4536 RVA: 0x00054794 File Offset: 0x00052994
		private void ApplyHoldBreath(float deltaTime)
		{
			if (this._settings == null || this._player == null || !this._settings.EnableHoldBreath)
			{
				this._isHoldingBreath = false;
				this._breathHud.UpdateBar(1f, 1f, false, false);
				return;
			}
			bool wantsHoldBreath = this._player.Controls.Sprint || this._player.Controls.Sneak;
			this._isRecovering = (this._currentBreath != 1f && this._isRecovering);
			if (this._player.Controls.RightMouseDown && wantsHoldBreath && this._currentBreath > 0f && !this._isRecovering && this._charged)
			{
				this._isHoldingBreath = true;
				this._currentBreath = Math.Max(0f, this._currentBreath - deltaTime / this._settings.BreathDuration);
				this._breathHud.UpdateBar(this._currentBreath, 1f, this._isRecovering, this._currentBreath < 1f);
				return;
			}
			this._isHoldingBreath = false;
			this._isRecovering = (this._currentBreath == 0f || this._isRecovering);
			this._currentBreath = Math.Min(1f, this._currentBreath + deltaTime / this._settings.BreathRecoveryDuration);
			if (this._currentBreath >= 1f)
			{
				this._isRecovering = false;
			}
			this._breathHud.UpdateBar(this._currentBreath, 1f, this._isRecovering, this._currentBreath < 1f);
		}

		// Token: 0x060011B9 RID: 4537 RVA: 0x0005492C File Offset: 0x00052B2C
		private void ApplyCameraSway(float deltaTime)
		{
			if (this._settings == null || this._player == null || !this._settings.EnableSwaying)
			{
				return;
			}
			float elapsedSec = (float)(this._api.World.ElapsedMilliseconds - this._startMs) / 1000f;
			float worldTimeSec = (float)this._api.World.ElapsedMilliseconds / 1000f;
			float charge = Math.Min(elapsedSec / this._settings.DrawTime, 1f);
			float targetMultiplier = 1f;
			if (this._player.Controls.TriesToMove || this._player.Controls.Jump)
			{
				targetMultiplier *= this._settings.RunPenaltyMultiplier + (this._player.Controls.Sprint ? 2.5f : 0f);
			}
			else if (this._isHoldingBreath)
			{
				targetMultiplier *= this._settings.HoldBreathMultiplier;
			}
			else if (this._isRecovering)
			{
				targetMultiplier *= this._settings.BreathRecoveryMultiplier;
			}
			Vector2 chargeBezierDisplacement = this.GetChargeBezierDisplacement(elapsedSec);
			Vector2 sway = this.GetLissajousCurveDisplacement(worldTimeSec) * MathF.Sqrt(Math.Max(this._settings.DrawWeight - 0.5f, 0.01f)) * 1.2f;
			this._stanceMultiplier = GameMath.Lerp(this._stanceMultiplier, targetMultiplier, deltaTime * 5f);
			float stanceAtCharge = GameMath.SmoothStep(Math.Clamp((charge - 0.3f) / 0.7f, 0f, 1f));
			float swayMultiplier = GameMath.Lerp(Math.Max(0f, this._stanceMultiplier - 1f) * (this._settings.RunPenaltyMultiplier / Math.Max(0.001f, this._settings.RunPenaltyMultiplier - 1f)), this._stanceMultiplier, stanceAtCharge);
			Vector2 nextPos = chargeBezierDisplacement + sway / 1.5f * swayMultiplier;
			Vector2 delta = nextPos - this._oldPos;
			this._api.Input.MouseYaw += delta.X;
			this._api.Input.MousePitch += delta.Y;
			this._player.Pos.Yaw += delta.X;
			this._player.Pos.Pitch += delta.Y;
			this._oldPos = nextPos;
		}

		// Token: 0x060011BA RID: 4538 RVA: 0x00054BA0 File Offset: 0x00052DA0
		private Vector2 GetChargeBezierDisplacement(float elapsedSec)
		{
			if (this._settings == null)
			{
				return Vector2.Zero;
			}
			float num = Math.Min(elapsedSec / this._settings.DrawTime, 1f);
			float smoothCharge = GameMath.SmoothStep(num);
			float charge = GameMath.Lerp(num, smoothCharge, smoothCharge);
			float inverse = 1f - charge;
			return new Vector2(2f * inverse * charge * this._controlOffset.X + charge * charge * this._targetOffset.X, 2f * inverse * charge * this._controlOffset.Y + charge * charge * this._targetOffset.Y);
		}

		// Token: 0x060011BB RID: 4539 RVA: 0x00054C38 File Offset: 0x00052E38
		private Vector2 GetLissajousCurveDisplacement(float timeSec)
		{
			if (this._settings == null)
			{
				return Vector2.Zero;
			}
			float speed = 0.5f * this._settings.SwaySpeed;
			float amplitude = 0.05f * this._settings.SwayAmplitude;
			return new Vector2(MathF.Sin(4f * speed * timeSec + 1.5707964f) * amplitude, MathF.Sin(3f * speed * timeSec) * amplitude);
		}

		// Token: 0x060011BC RID: 4540 RVA: 0x00054CA2 File Offset: 0x00052EA2
		private static float Rational(float x, float k, float max = 1f)
		{
			return x / (x + k) * max;
		}

		// Token: 0x04000828 RID: 2088
		private readonly ICoreClientAPI _api;

		// Token: 0x04000829 RID: 2089
		private readonly Random _random = new Random();

		// Token: 0x0400082A RID: 2090
		private readonly LoadedTexture _chargedReticle;

		// Token: 0x0400082B RID: 2091
		private readonly ArcheringBowBreathHud _breathHud;

		// Token: 0x0400082C RID: 2092
		[Nullable(2)]
		private ArcheringBowAimingSettings _settings;

		// Token: 0x0400082D RID: 2093
		[Nullable(2)]
		private EntityPlayer _player;

		// Token: 0x0400082E RID: 2094
		private Vector2 _oldPos;

		// Token: 0x0400082F RID: 2095
		private Vector2 _targetOffset;

		// Token: 0x04000830 RID: 2096
		private Vector2 _controlOffset;

		// Token: 0x04000831 RID: 2097
		private long _startMs;

		// Token: 0x04000832 RID: 2098
		private long _flashUntilMs;

		// Token: 0x04000833 RID: 2099
		private float _stanceMultiplier = 1f;

		// Token: 0x04000834 RID: 2100
		private float _currentBreath = 1f;

		// Token: 0x04000835 RID: 2101
		private bool _isRecovering;

		// Token: 0x04000836 RID: 2102
		private bool _isHoldingBreath;

		// Token: 0x04000837 RID: 2103
		private bool _charged;

		// Token: 0x04000838 RID: 2104
		private bool _reticleRegistered;

		// Token: 0x04000839 RID: 2105
		private bool _cameraHookRegistered;

		// Token: 0x0400083A RID: 2106
		[Nullable(2)]
		private Func<bool> _shouldContinue;
	}
}
