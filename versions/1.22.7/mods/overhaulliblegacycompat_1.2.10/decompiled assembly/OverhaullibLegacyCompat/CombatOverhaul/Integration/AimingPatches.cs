using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Vintagestory.Client.NoObf;

namespace CombatOverhaul.Integration
{
	// Token: 0x02000033 RID: 51
	[NullableContext(1)]
	[Nullable(0)]
	internal static class AimingPatches
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x0600037A RID: 890 RVA: 0x0000FDAE File Offset: 0x0000DFAE
		// (set) Token: 0x0600037B RID: 891 RVA: 0x0000FDB5 File Offset: 0x0000DFB5
		public static bool DrawDefaultReticle { get; set; } = true;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x0600037C RID: 892 RVA: 0x0000FDC0 File Offset: 0x0000DFC0
		// (remove) Token: 0x0600037D RID: 893 RVA: 0x0000FDF4 File Offset: 0x0000DFF4
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public static event AimingPatches.UpdateCameraYawPitchDelegate UpdateCameraYawPitch;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x0600037E RID: 894 RVA: 0x0000FE28 File Offset: 0x0000E028
		// (remove) Token: 0x0600037F RID: 895 RVA: 0x0000FE5C File Offset: 0x0000E05C
		[Nullable(new byte[]
		{
			2,
			1
		})]
		[Nullable(new byte[]
		{
			2,
			1
		})]
		public static event Action<ClientMain> AfterUpdateCameraYawPitch;

		// Token: 0x06000380 RID: 896 RVA: 0x0000FE90 File Offset: 0x0000E090
		public static void Patch(string harmonyId)
		{
			Harmony harmony = new Harmony(harmonyId);
			MethodBase method = typeof(ClientMain).GetMethod("UpdateCameraYawPitch", 20);
			<>A{00001248}<ClientMain, double, double, double, double, float> <>A{00001248};
			if ((<>A{00001248} = AimingPatches.<>O.<0>__UpdateCameraYawPitchPatch) == null)
			{
				<>A{00001248} = (AimingPatches.<>O.<0>__UpdateCameraYawPitchPatch = new <>A{00001248}<ClientMain, double, double, double, double, float>(AimingPatches.UpdateCameraYawPitchPatch));
			}
			HarmonyMethod harmonyMethod = new HarmonyMethod(<>A{00001248});
			Action<ClientMain> action;
			if ((action = AimingPatches.<>O.<1>__UpdateCameraYawPitchPostfix) == null)
			{
				action = (AimingPatches.<>O.<1>__UpdateCameraYawPitchPostfix = new Action<ClientMain>(AimingPatches.UpdateCameraYawPitchPostfix));
			}
			harmony.Patch(method, harmonyMethod, new HarmonyMethod(action), null, null);
			MethodBase method2 = typeof(SystemRenderAim).GetMethod("DrawAim", 36);
			Func<bool> func;
			if ((func = AimingPatches.<>O.<2>__DrawAim) == null)
			{
				func = (AimingPatches.<>O.<2>__DrawAim = new Func<bool>(AimingPatches.DrawAim));
			}
			harmony.Patch(method2, new HarmonyMethod(func), null, null, null);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0000FF44 File Offset: 0x0000E144
		public static void Unpatch(string harmonyId)
		{
			Harmony harmony = new Harmony(harmonyId);
			harmony.Unpatch(typeof(ClientMain).GetMethod("UpdateCameraYawPitch", 20), 1, "*");
			harmony.Unpatch(typeof(ClientMain).GetMethod("UpdateCameraYawPitch", 20), 2, "*");
			harmony.Unpatch(typeof(SystemRenderAim).GetMethod("DrawAim", 36), 1, "*");
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0000FFBC File Offset: 0x0000E1BC
		private static bool DrawAim()
		{
			return AimingPatches.DrawDefaultReticle;
		}

		// Token: 0x06000383 RID: 899 RVA: 0x0000FFC3 File Offset: 0x0000E1C3
		private static void UpdateCameraYawPitchPatch(ClientMain __instance, ref double ___MouseDeltaX, ref double ___MouseDeltaY, ref double ___DelayedMouseDeltaX, ref double ___DelayedMouseDeltaY, float dt)
		{
			AimingPatches.UpdateCameraYawPitchDelegate updateCameraYawPitch = AimingPatches.UpdateCameraYawPitch;
			if (updateCameraYawPitch == null)
			{
				return;
			}
			updateCameraYawPitch(__instance, ref ___MouseDeltaX, ref ___MouseDeltaY, ref ___DelayedMouseDeltaX, ref ___DelayedMouseDeltaY, dt);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0000FFDC File Offset: 0x0000E1DC
		private static void UpdateCameraYawPitchPostfix(ClientMain __instance)
		{
			Action<ClientMain> afterUpdateCameraYawPitch = AimingPatches.AfterUpdateCameraYawPitch;
			if (afterUpdateCameraYawPitch == null)
			{
				return;
			}
			afterUpdateCameraYawPitch.Invoke(__instance);
		}

		// Token: 0x020001CF RID: 463
		// (Invoke) Token: 0x0600153A RID: 5434
		[NullableContext(0)]
		public delegate void UpdateCameraYawPitchDelegate(ClientMain __instance, ref double ___MouseDeltaX, ref double ___MouseDeltaY, ref double ___DelayedMouseDeltaX, ref double ___DelayedMouseDeltaY, float dt);

		// Token: 0x020001D0 RID: 464
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000ADD RID: 2781
			[Nullable(0)]
			public static <>A{00001248}<ClientMain, double, double, double, double, float> <0>__UpdateCameraYawPitchPatch;

			// Token: 0x04000ADE RID: 2782
			[Nullable(new byte[]
			{
				0,
				1
			})]
			public static Action<ClientMain> <1>__UpdateCameraYawPitchPostfix;

			// Token: 0x04000ADF RID: 2783
			[Nullable(0)]
			public static Func<bool> <2>__DrawAim;
		}
	}
}
