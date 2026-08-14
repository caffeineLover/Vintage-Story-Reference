using System;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace Vintagestory.API.Client;

public class PsychedelicPerceptionEffect : DrunkPerceptionEffect
{
	private float accum;

	private float accum1s;

	private float targetIntensity;

	public PsychedelicPerceptionEffect(ICoreClientAPI capi)
		: base(capi)
	{
	}

	public override void OnBeforeGameRender(float dt)
	{
		if (!capi.IsGamePaused && capi.World.Player.Entity.AnimManager.HeadController != null)
		{
			capi.Render.ShaderUniforms.PerceptionEffectIntensity = Intensity;
			capi.Render.ShaderUniforms.PsychedelicStrength = Intensity * 2f;
			capi.World.Player.Entity.HeadBobbingAmplitude = GameMath.Clamp(1f - Intensity * 5f, 0f, 1f);
			accum1s += dt;
			if (accum1s > 1f)
			{
				accum1s = 0f;
				targetIntensity = capi.World.Player.Entity.WatchedAttributes.GetFloat("psychedelic") / 2f;
			}
			Intensity += (targetIntensity - Intensity) * dt / 3f;
			accum = (float)((double)capi.InWorldEllapsedMilliseconds / 3000.0 % 100.0 * Math.PI);
			float f = Intensity / 250f / 2f / 4f;
			float dizzinessAccum = accum / 4f;
			float dp = (float)(Math.Cos((double)dizzinessAccum / 1.15) + Math.Cos(dizzinessAccum / 1.35f)) * f;
			capi.World.Player.Entity.Pos.Pitch += dp;
			capi.Input.MousePitch += dp;
			capi.Input.MouseYaw += (float)(Math.Sin((double)dizzinessAccum / 1.1) + Math.Sin(dizzinessAccum / 1.5f) + Math.Sin(dizzinessAccum / 5f) * 0.20000000298023224) * f;
			if (!capi.Input.MouseGrabbed)
			{
				capi.World.Player.Entity.Pos.Yaw = capi.Input.MouseYaw;
			}
			IHeadController headController = capi.World.Player.Entity.AnimManager.HeadController;
			headController.YawOffset = (float)(Math.Cos((double)dizzinessAccum / 1.12) + Math.Cos(dizzinessAccum / 1.2f) + Math.Cos(dizzinessAccum / 4f) * 0.20000000298023224) * f * 60f;
			dizzinessAccum /= 2f;
			headController.PitchOffset = (float)(Math.Sin((double)dizzinessAccum / 1.12) + Math.Sin(dizzinessAccum / 1.2f) + Math.Sin(dizzinessAccum / 4f) * 0.20000000298023224) * f * 30f;
			double accum2 = (float)((double)capi.InWorldEllapsedMilliseconds / 9000.0 % 100.0 * Math.PI);
			float intox = capi.Render.ShaderUniforms.PerceptionEffectIntensity;
			float b = GameMath.Clamp((0.4f + (float)Math.Abs(Math.Cos(accum2 / 1.12) + Math.Sin(accum2 / 2.2) + Math.Cos(accum2 * 2.3))) * intox, 0f, 1.5f);
			capi.Render.ShaderUniforms.AmbientBloomLevelAdd[2] = b;
		}
	}
}
