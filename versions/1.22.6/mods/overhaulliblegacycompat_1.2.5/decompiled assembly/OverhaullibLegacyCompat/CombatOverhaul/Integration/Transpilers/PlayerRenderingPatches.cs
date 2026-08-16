using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using HarmonyLib;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;

namespace CombatOverhaul.Integration.Transpilers
{
	// Token: 0x0200005F RID: 95
	internal static class PlayerRenderingPatches
	{
		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060004CD RID: 1229 RVA: 0x0001C4A8 File Offset: 0x0001A6A8
		// (set) Token: 0x060004CE RID: 1230 RVA: 0x0001C4AF File Offset: 0x0001A6AF
		public static float HandsFovMultiplier { get; set; } = 1f;

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x0001C4B7 File Offset: 0x0001A6B7
		// (set) Token: 0x060004D0 RID: 1232 RVA: 0x0001C4BE File Offset: 0x0001A6BE
		public static float FpHandsOffset { get; set; } = -0.3f;

		// Token: 0x060004D1 RID: 1233 RVA: 0x0001C4C6 File Offset: 0x0001A6C6
		public static float ResetOffset()
		{
			return PlayerRenderingPatches.FpHandsOffset = -0.3f;
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0001C4D3 File Offset: 0x0001A6D3
		public static float SetOffset(float offset)
		{
			PlayerRenderingPatches.FpHandsOffset = offset;
			return offset;
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0001C4DC File Offset: 0x0001A6DC
		[NullableContext(1)]
		public static float GetOffset(ModSystemFpHands modSys)
		{
			return PlayerRenderingPatches.FpHandsOffset + GameMath.Max(0f, (float)ClientSettings.FieldOfView / 90f - 1f) / 2f;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0001C506 File Offset: 0x0001A706
		[NullableContext(1)]
		public static float GetOffsetAdjusted(ModSystemFpHands modSys)
		{
			return PlayerRenderingPatches.FpHandsOffset + GameMath.Max(0f, (float)ClientSettings.FieldOfView / 90f - 1f) / 2f;
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0001C530 File Offset: 0x0001A730
		public static float GetMultiplier()
		{
			return PlayerRenderingPatches.HandsFovMultiplier;
		}

		// Token: 0x040001C7 RID: 455
		public const float DefaultFpHandsOffset = -0.3f;

		// Token: 0x020001F2 RID: 498
		[HarmonyPatch(typeof(EntityPlayerShapeRenderer), "DoRender3DOpaque")]
		[HarmonyPatchCategory("combatoverhaul")]
		public class EntityShapeRendererPatch
		{
			// Token: 0x06001571 RID: 5489 RVA: 0x00068AB4 File Offset: 0x00066CB4
			[NullableContext(1)]
			[HarmonyTranspiler]
			public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
			{
				List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
				for (int i = 0; i < codes.Count; i++)
				{
					if (codes[i].opcode == OpCodes.Callvirt)
					{
						MethodInfo propertyInfo = codes[i].operand as MethodInfo;
						if (propertyInfo != null && propertyInfo.Name == "get_HandRenderFov")
						{
							codes.Insert(i + 1, new CodeInstruction(OpCodes.Call, typeof(PlayerRenderingPatches).GetMethod("GetMultiplier")));
							codes.Insert(i + 2, new CodeInstruction(OpCodes.Mul, null));
						}
					}
					if (codes[i].opcode == OpCodes.Ldc_R4 && (float)codes[i].operand == -0.3f)
					{
						codes[i].opcode = OpCodes.Call;
						codes[i].operand = typeof(PlayerRenderingPatches).GetMethod("GetOffsetAdjusted");
						codes.Insert(i, new CodeInstruction(OpCodes.Ldarg_0, null));
						codes.Insert(i + 1, new CodeInstruction(OpCodes.Ldfld, typeof(EntityPlayerShapeRenderer).GetField("modSys", 36)));
						break;
					}
				}
				return codes;
			}
		}

		// Token: 0x020001F3 RID: 499
		[HarmonyPatch(typeof(EntityPlayerShapeRenderer), "getReadyShader")]
		[HarmonyPatchCategory("combatoverhaul")]
		public class EntityShapeRendererShaderPatch
		{
			// Token: 0x06001573 RID: 5491 RVA: 0x00068C04 File Offset: 0x00066E04
			[NullableContext(1)]
			[HarmonyTranspiler]
			public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
			{
				List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
				for (int i = 0; i < codes.Count; i++)
				{
					if (codes[i].opcode == OpCodes.Ldc_R4 && (float)codes[i].operand == -0.3f)
					{
						codes[i].opcode = OpCodes.Call;
						codes[i].operand = typeof(PlayerRenderingPatches).GetMethod("GetOffset");
						codes.Insert(i, new CodeInstruction(OpCodes.Ldarg_0, null));
						codes.Insert(i + 1, new CodeInstruction(OpCodes.Ldfld, typeof(EntityPlayerShapeRenderer).GetField("modSys", 36)));
						break;
					}
				}
				return codes;
			}
		}
	}
}
