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
	// Token: 0x02000154 RID: 340
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class ArcheringBowAimingSession : IRenderer, IDisposable
	{
		// Token: 0x06001139 RID: 4409 RVA: 0x00052A30 File Offset: 0x00050C30
		public ArcheringBowAimingSession(ICoreClientAPI api)
		{
			this._api = api;
			this._chargedReticle = new LoadedTexture(api);
			api.Render.GetOrLoadTexture(new AssetLocation("combatoverhaul", "gui/aiming/default-full.png"), ref this._chargedReticle);
			this._breathHud = new ArcheringBowBreathHud(api);
			this._breathHud.TryOpen();
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x0600113A RID: 4410 RVA: 0x00052AAF File Offset: 0x00050CAF
		public double RenderOrder
		{
			get
			{
				return 0.98;
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x0600113B RID: 4411 RVA: 0x00052ABA File Offset: 0x00050CBA
		public int RenderRange
		{
			get
			{
				return 9999;
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x0600113C RID: 4412 RVA: 0x00052AC1 File Offset: 0x00050CC1
		public bool Active
		{
			get
			{
				return this._settings != null;
			}
		}

		// Token: 0x0600113D RID: 4413 RVA: 0x00052ACC File Offset: 0x00050CCC
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

		// Token: 0x0600113E RID: 4414 RVA: 0x00052C6C File Offset: 0x00050E6C
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

		// Token: 0x0600113F RID: 4415 RVA: 0x00052CF5 File Offset: 0x00050EF5
		public void Dispose()
		{
			this.Stop();
			this._chargedReticle.Dispose();
			this._breathHud.TryClose();
			this._breathHud.Dispose();
		}

		// Token: 0x06001140 RID: 4416 RVA: 0x00052D1F File Offset: 0x00050F1F
		public void SetCharged()
		{
			if (this._charged)
			{
				return;
			}
			this._charged = true;
			this._flashUntilMs = this._api.World.ElapsedMilliseconds + 250L;
		}

		// Token: 0x06001141 RID: 4417 RVA: 0x00052D4E File Offset: 0x00050F4E
		public void OnRenderFrame(float deltaTime, EnumRenderStage stage)
		{
			if (this._settings == null || this._player == null || stage != 10)
			{
				return;
			}
			this.RenderChargeFlash();
		}

		// Token: 0x06001142 RID: 4418 RVA: 0x00052D6C File Offset: 0x00050F6C
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

		// Token: 0x06001143 RID: 4419 RVA: 0x00052DC8 File Offset: 0x00050FC8
		private void RenderChargeFlash()
		{
			if (this._api.World.ElapsedMilliseconds > this._flashUntilMs)
			{
				return;
			}
			float scale = RuntimeEnv.GUIScale;
			this._api.Render.Render2DTexture(this._chargedReticle.TextureId, (float)this._api.Render.FrameWidth / 2f - (float)this._chargedReticle.Width * scale / 2f, (float)this._api.Render.FrameHeight / 2f - (float)this._chargedReticle.Height * scale / 2f, (float)this._chargedReticle.Width * scale, (float)this._chargedReticle.Height * scale, 10000f, null);
		}

		// Token: 0x06001144 RID: 4420 RVA: 0x00052E8C File Offset: 0x0005108C
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

		// Token: 0x06001145 RID: 4421 RVA: 0x00053024 File Offset: 0x00051224
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

		// Token: 0x06001146 RID: 4422 RVA: 0x00053298 File Offset: 0x00051498
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

		// Token: 0x06001147 RID: 4423 RVA: 0x00053330 File Offset: 0x00051530
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

		// Token: 0x06001148 RID: 4424 RVA: 0x0005339A File Offset: 0x0005159A
		private static float Rational(float x, float k, float max = 1f)
		{
			return x / (x + k) * max;
		}

		// Token: 0x04000802 RID: 2050
		private readonly ICoreClientAPI _api;

		// Token: 0x04000803 RID: 2051
		private readonly Random _random = new Random();

		// Token: 0x04000804 RID: 2052
		private readonly LoadedTexture _chargedReticle;

		// Token: 0x04000805 RID: 2053
		private readonly ArcheringBowBreathHud _breathHud;

		// Token: 0x04000806 RID: 2054
		[Nullable(2)]
		private ArcheringBowAimingSettings _settings;

		// Token: 0x04000807 RID: 2055
		[Nullable(2)]
		private EntityPlayer _player;

		// Token: 0x04000808 RID: 2056
		private Vector2 _oldPos;

		// Token: 0x04000809 RID: 2057
		private Vector2 _targetOffset;

		// Token: 0x0400080A RID: 2058
		private Vector2 _controlOffset;

		// Token: 0x0400080B RID: 2059
		private long _startMs;

		// Token: 0x0400080C RID: 2060
		private long _flashUntilMs;

		// Token: 0x0400080D RID: 2061
		private float _stanceMultiplier = 1f;

		// Token: 0x0400080E RID: 2062
		private float _currentBreath = 1f;

		// Token: 0x0400080F RID: 2063
		private bool _isRecovering;

		// Token: 0x04000810 RID: 2064
		private bool _isHoldingBreath;

		// Token: 0x04000811 RID: 2065
		private bool _charged;

		// Token: 0x04000812 RID: 2066
		private bool _reticleRegistered;

		// Token: 0x04000813 RID: 2067
		private bool _cameraHookRegistered;

		// Token: 0x04000814 RID: 2068
		[Nullable(2)]
		private Func<bool> _shouldContinue;
	}
}
