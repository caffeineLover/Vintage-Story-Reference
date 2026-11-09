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
		// (get) Token: 0x060004CD RID: 1229 RVA: 0x0001C4E0 File Offset: 0x0001A6E0
		// (set) Token: 0x060004CE RID: 1230 RVA: 0x0001C4E7 File Offset: 0x0001A6E7
		public static float HandsFovMultiplier { get; set; } = 1f;

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060004CF RID: 1231 RVA: 0x0001C4EF File Offset: 0x0001A6EF
		// (set) Token: 0x060004D0 RID: 1232 RVA: 0x0001C4F6 File Offset: 0x0001A6F6
		public static float FpHandsOffset { get; set; } = -0.3f;

		// Token: 0x060004D1 RID: 1233 RVA: 0x0001C4FE File Offset: 0x0001A6FE
		public static float ResetOffset()
		{
			return PlayerRenderingPatches.FpHandsOffset = -0.3f;
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x0001C50B File Offset: 0x0001A70B
		public static float SetOffset(float offset)
		{
			PlayerRenderingPatches.FpHandsOffset = offset;
			return offset;
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x0001C514 File Offset: 0x0001A714
		[NullableContext(1)]
		public static float GetOffset(ModSystemFpHands modSys)
		{
			return PlayerRenderingPatches.FpHandsOffset + GameMath.Max(0f, (float)ClientSettings.FieldOfView / 90f - 1f) / 2f;
		}

		// Token: 0x060004D4 RID: 1236 RVA: 0x0001C53E File Offset: 0x0001A73E
		[NullableContext(1)]
		public static float GetOffsetAdjusted(ModSystemFpHands modSys)
		{
			return PlayerRenderingPatches.FpHandsOffset + GameMath.Max(0f, (float)ClientSettings.FieldOfView / 90f - 1f) / 2f;
		}

		// Token: 0x060004D5 RID: 1237 RVA: 0x0001C568 File Offset: 0x0001A768
		public static float GetMultiplier()
		{
			return PlayerRenderingPatches.HandsFovMultiplier;
		}

		// Token: 0x040001C7 RID: 455
		public const float DefaultFpHandsOffset = -0.3f;

		// Token: 0x02000200 RID: 512
		[HarmonyPatch(typeof(EntityPlayerShapeRenderer), "DoRender3DOpaque")]
		[HarmonyPatchCategory("combatoverhaul")]
		public class EntityShapeRendererPatch
		{
			// Token: 0x060015E9 RID: 5609 RVA: 0x0006A55C File Offset: 0x0006875C
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

		// Token: 0x02000201 RID: 513
		[HarmonyPatch(typeof(EntityPlayerShapeRenderer), "getReadyShader")]
		[HarmonyPatchCategory("combatoverhaul")]
		public class EntityShapeRendererShaderPatch
		{
			// Token: 0x060015EB RID: 5611 RVA: 0x0006A6AC File Offset: 0x000688AC
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
