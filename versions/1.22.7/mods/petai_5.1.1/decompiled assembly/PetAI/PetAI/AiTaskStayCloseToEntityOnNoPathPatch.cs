using System;
using System.Reflection;
using HarmonyLib;
using Vintagestory.GameContent;

namespace PetAI
{
	// Token: 0x02000021 RID: 33
	public class AiTaskStayCloseToEntityOnNoPathPatch
	{
		// Token: 0x060000C8 RID: 200 RVA: 0x00006ED8 File Offset: 0x000050D8
		public static void Patch(Harmony harmony)
		{
			harmony.Patch(AiTaskStayCloseToEntityOnNoPathPatch.MethodInfo(), null, new HarmonyMethod(typeof(AiTaskStayCloseToEntityOnNoPathPatch).GetMethod("Postfix", 24)), null, null);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00006F04 File Offset: 0x00005104
		public static void Unpatch(Harmony harmony)
		{
			harmony.Unpatch(AiTaskStayCloseToEntityOnNoPathPatch.MethodInfo(), 2, "gerste.petai");
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00006F17 File Offset: 0x00005117
		public static MethodInfo MethodInfo()
		{
			return typeof(AiTaskStayCloseToEntity).GetMethod("OnNoPath", 20, Array.Empty<Type>());
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00006F34 File Offset: 0x00005134
		public static void Postfix(AiTaskStayCloseToEntity __instance)
		{
			__instance.OnNoPath(null);
		}
	}
}
