using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;

namespace Firearms
{
	// Token: 0x02000005 RID: 5
	[NullableContext(1)]
	[Nullable(0)]
	internal static class FirearmsRecoilSystem
	{
		// Token: 0x06000047 RID: 71 RVA: 0x000027E4 File Offset: 0x000009E4
		public static void Start(ICoreClientAPI api)
		{
			FirearmsRecoilSystem._api = api;
			if (FirearmsRecoilSystem._harmony == null)
			{
				FirearmsRecoilSystem._harmony = new Harmony("firearmsfork.recoil");
			}
			MethodInfo method = AccessTools.Method(typeof(ClientMain), "UpdateCameraYawPitch", new Type[]
			{
				typeof(float)
			}, null);
			FirearmsRecoilSystem._harmony.Patch(method, null, new HarmonyMethod(typeof(FirearmsRecoilSystem), "UpdateCameraYawPitchPostfix", null), null, null);
		}

		// Token: 0x06000048 RID: 72 RVA: 0x0000285C File Offset: 0x00000A5C
		public static void Stop()
		{
			Harmony harmony = FirearmsRecoilSystem._harmony;
			if (harmony != null)
			{
				harmony.UnpatchAll("firearmsfork.recoil");
			}
			FirearmsRecoilSystem._harmony = null;
			FirearmsRecoilSystem._api = null;
			FirearmsRecoilSystem._pendingPitchKick = 0f;
			FirearmsRecoilSystem._pendingYawKick = 0f;
			FirearmsRecoilSystem._pendingPitchRecovery = 0f;
			FirearmsRecoilSystem._pendingYawRecovery = 0f;
			FirearmsRecoilSystem._recoveryDelayRemaining = 0f;
			FirearmsRecoilSystem._riseDuration = 0.18f;
			FirearmsRecoilSystem._recoveryDuration = 0.55f;
			FirearmsRecoilSystem._maxPendingPitchKick = 0.31415927f;
			FirearmsRecoilSystem._maxPendingYawKick = 0.05235988f;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000028E4 File Offset: 0x00000AE4
		public static void AddRecoil(float verticalDeg, float horizontalDeg, RecoilStats recoil)
		{
			if (FirearmsRecoilSystem._api == null || !recoil.Enabled)
			{
				return;
			}
			long now = FirearmsRecoilSystem._api.World.ElapsedMilliseconds;
			if (now - FirearmsRecoilSystem._lastRecoilMs < (long)Math.Max(0, recoil.MinIntervalMs))
			{
				return;
			}
			FirearmsRecoilSystem._lastRecoilMs = now;
			FirearmsRecoilSystem._riseDuration = Math.Max(0.001f, recoil.RiseDurationSec);
			FirearmsRecoilSystem._recoveryDuration = Math.Max(0.001f, recoil.SpringBackDurationSec);
			FirearmsRecoilSystem._maxPendingPitchKick = Math.Max(0.1f, recoil.MaxQueuedVerticalDeg) * 0.017453292f;
			FirearmsRecoilSystem._maxPendingYawKick = Math.Max(0.1f, recoil.MaxQueuedHorizontalDeg) * 0.017453292f;
			float vertical = Math.Max(0f, verticalDeg) * 0.017453292f;
			float horizontal = Math.Max(0f, horizontalDeg) * 0.017453292f;
			float yawImpulse = ((float)FirearmsRecoilSystem.Random.NextDouble() * 2f - 1f) * horizontal;
			FirearmsRecoilSystem._pendingPitchKick = GameMath.Clamp(FirearmsRecoilSystem._pendingPitchKick - vertical, -FirearmsRecoilSystem._maxPendingPitchKick, FirearmsRecoilSystem._maxPendingPitchKick);
			FirearmsRecoilSystem._pendingYawKick = GameMath.Clamp(FirearmsRecoilSystem._pendingYawKick + yawImpulse, -FirearmsRecoilSystem._maxPendingYawKick, FirearmsRecoilSystem._maxPendingYawKick);
			if (recoil.SpringBack && recoil.SpringBackFraction > 0f)
			{
				float springBackFraction = Math.Max(0f, recoil.SpringBackFraction);
				FirearmsRecoilSystem._pendingPitchRecovery = GameMath.Clamp(FirearmsRecoilSystem._pendingPitchRecovery + vertical * springBackFraction, -FirearmsRecoilSystem._maxPendingPitchKick, FirearmsRecoilSystem._maxPendingPitchKick);
				FirearmsRecoilSystem._pendingYawRecovery = GameMath.Clamp(FirearmsRecoilSystem._pendingYawRecovery - yawImpulse * springBackFraction, -FirearmsRecoilSystem._maxPendingYawKick, FirearmsRecoilSystem._maxPendingYawKick);
				FirearmsRecoilSystem._recoveryDelayRemaining = Math.Max(FirearmsRecoilSystem._recoveryDelayRemaining, Math.Max(0f, recoil.SpringBackDelaySec));
			}
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002A88 File Offset: 0x00000C88
		private static void UpdateCameraYawPitchPostfix(ClientMain __instance, float dt, ref float ___mousePitch, ref float ___mouseYaw)
		{
			if (FirearmsRecoilSystem._api != null)
			{
				EntityPlayer entityPlayer = __instance.EntityPlayer;
				if (((entityPlayer != null) ? entityPlayer.Pos : null) != null)
				{
					dt = GameMath.Clamp(dt, 0f, 0.1f);
					if (dt <= 0f)
					{
						return;
					}
					float pitchDelta = FirearmsRecoilSystem.ConsumeKick(ref FirearmsRecoilSystem._pendingPitchKick, dt, FirearmsRecoilSystem._riseDuration);
					float yawDelta = FirearmsRecoilSystem.ConsumeKick(ref FirearmsRecoilSystem._pendingYawKick, dt, FirearmsRecoilSystem._riseDuration);
					if (Math.Abs(FirearmsRecoilSystem._pendingPitchKick) < 0.0005f && Math.Abs(FirearmsRecoilSystem._pendingYawKick) < 0.0005f)
					{
						FirearmsRecoilSystem._recoveryDelayRemaining = Math.Max(0f, FirearmsRecoilSystem._recoveryDelayRemaining - dt);
						if (FirearmsRecoilSystem._recoveryDelayRemaining <= 0f)
						{
							pitchDelta += FirearmsRecoilSystem.ConsumeKick(ref FirearmsRecoilSystem._pendingPitchRecovery, dt, FirearmsRecoilSystem._recoveryDuration);
							yawDelta += FirearmsRecoilSystem.ConsumeKick(ref FirearmsRecoilSystem._pendingYawRecovery, dt, FirearmsRecoilSystem._recoveryDuration);
						}
					}
					if (Math.Abs(pitchDelta) < 1E-06f && Math.Abs(yawDelta) < 1E-06f)
					{
						return;
					}
					__instance.EntityPlayer.Pos.Pitch = GameMath.Clamp(__instance.EntityPlayer.Pos.Pitch + pitchDelta, 1.5857964f, 4.697389f);
					__instance.EntityPlayer.Pos.Yaw = GameMath.Mod(__instance.EntityPlayer.Pos.Yaw + yawDelta, 6.2831855f);
					___mousePitch = __instance.EntityPlayer.Pos.Pitch;
					___mouseYaw = __instance.EntityPlayer.Pos.Yaw;
					return;
				}
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002BF4 File Offset: 0x00000DF4
		private static float ConsumeKick(ref float pendingKick, float dt, float duration)
		{
			float absPending = Math.Abs(pendingKick);
			if (absPending < 1E-06f)
			{
				return 0f;
			}
			float delta = MathF.CopySign(Math.Min(absPending, absPending * dt / duration), pendingKick);
			pendingKick -= delta;
			return delta;
		}

		// Token: 0x0400001C RID: 28
		private const string HarmonyId = "firearmsfork.recoil";

		// Token: 0x0400001D RID: 29
		private const float MinCameraPitch = 1.5857964f;

		// Token: 0x0400001E RID: 30
		private const float MaxCameraPitch = 4.697389f;

		// Token: 0x0400001F RID: 31
		private static readonly Random Random = new Random();

		// Token: 0x04000020 RID: 32
		[Nullable(2)]
		private static Harmony _harmony;

		// Token: 0x04000021 RID: 33
		[Nullable(2)]
		private static ICoreClientAPI _api;

		// Token: 0x04000022 RID: 34
		private static float _pendingPitchKick;

		// Token: 0x04000023 RID: 35
		private static float _pendingYawKick;

		// Token: 0x04000024 RID: 36
		private static float _pendingPitchRecovery;

		// Token: 0x04000025 RID: 37
		private static float _pendingYawRecovery;

		// Token: 0x04000026 RID: 38
		private static float _recoveryDelayRemaining;

		// Token: 0x04000027 RID: 39
		private static float _riseDuration = 0.18f;

		// Token: 0x04000028 RID: 40
		private static float _recoveryDuration = 0.55f;

		// Token: 0x04000029 RID: 41
		private static float _maxPendingPitchKick = 0.31415927f;

		// Token: 0x0400002A RID: 42
		private static float _maxPendingYawKick = 0.05235988f;

		// Token: 0x0400002B RID: 43
		private static long _lastRecoilMs = -1000L;
	}
}
