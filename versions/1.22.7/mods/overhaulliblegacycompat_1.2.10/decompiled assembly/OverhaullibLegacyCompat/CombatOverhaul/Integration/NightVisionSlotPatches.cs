using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.GameContent;

namespace CombatOverhaul.Integration
{
	// Token: 0x02000045 RID: 69
	[NullableContext(1)]
	[Nullable(0)]
	internal static class NightVisionSlotPatches
	{
		// Token: 0x0600043A RID: 1082 RVA: 0x00018324 File Offset: 0x00016524
		public static void Patch(string harmonyId, ICoreClientAPI api)
		{
			Harmony harmony = new Harmony(harmonyId);
			NightVisionSlotPatches.PatchMethod(harmony, NightVisionSlotPatches.ResolveVanillaRenderMethod());
			if (api.ModLoader.IsModEnabled("betterjonasdevicesfixedagain") || api.ModLoader.IsModEnabled("betterjonasdevicesfixed"))
			{
				NightVisionSlotPatches.PatchMethod(harmony, NightVisionSlotPatches.ResolveBetterJonasApplyEffectMethod());
			}
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x00018374 File Offset: 0x00016574
		public static void Unpatch(string harmonyId)
		{
			Harmony harmony = new Harmony(harmonyId);
			NightVisionSlotPatches.UnpatchMethod(harmony, NightVisionSlotPatches.ResolveVanillaRenderMethod(), harmonyId);
			NightVisionSlotPatches.UnpatchMethod(harmony, NightVisionSlotPatches.ResolveBetterJonasApplyEffectMethod(), harmonyId);
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x00018393 File Offset: 0x00016593
		private static bool PatchMethod(Harmony harmony, [Nullable(2)] MethodInfo method)
		{
			if (method == null)
			{
				return false;
			}
			harmony.Patch(method, null, null, new HarmonyMethod(typeof(NightVisionSlotPatches), "InventorySlotIndexTranspiler", null), null);
			return true;
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x000183C1 File Offset: 0x000165C1
		private static void UnpatchMethod(Harmony harmony, [Nullable(2)] MethodInfo method, string harmonyId)
		{
			if (method == null)
			{
				return;
			}
			harmony.Unpatch(method, 3, harmonyId);
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x000183D6 File Offset: 0x000165D6
		[NullableContext(2)]
		private static MethodInfo ResolveVanillaRenderMethod()
		{
			return AccessTools.Method(typeof(ModSystemNightVision), "OnRenderFrame", null, null);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x000183F0 File Offset: 0x000165F0
		[NullableContext(2)]
		private static MethodInfo ResolveBetterJonasApplyEffectMethod()
		{
			Type rendererType = AccessTools.TypeByName("BetterJonasDevices.NightVisionRenderer");
			if (!(rendererType == null))
			{
				return AccessTools.Method(rendererType, "ApplyEffect", null, null);
			}
			return null;
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x00018420 File Offset: 0x00016620
		private static IEnumerable<CodeInstruction> InventorySlotIndexTranspiler(IEnumerable<CodeInstruction> instructions)
		{
			NightVisionSlotPatches.<InventorySlotIndexTranspiler>d__6 <InventorySlotIndexTranspiler>d__ = new NightVisionSlotPatches.<InventorySlotIndexTranspiler>d__6(-2);
			<InventorySlotIndexTranspiler>d__.<>3__instructions = instructions;
			return <InventorySlotIndexTranspiler>d__;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x00018430 File Offset: 0x00016630
		private static bool LoadsVanillaNightVisionSlot(CodeInstruction instruction)
		{
			return (instruction.opcode == OpCodes.Ldc_I4_S && Convert.ToInt32(instruction.operand) == 12) || (instruction.opcode == OpCodes.Ldc_I4 && Convert.ToInt32(instruction.operand) == 12);
		}
	}
}
