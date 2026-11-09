using System;
using System.Reflection;
using HarmonyLib;
using Vintagestory.GameContent;

namespace PetAI
{
	// Token: 0x0200001F RID: 31
	public class EntityBehaviorNameTagGetNamePatch
	{
		// Token: 0x060000BE RID: 190 RVA: 0x00006DB5 File Offset: 0x00004FB5
		public static void Patch(Harmony harmony)
		{
			harmony.Patch(EntityBehaviorNameTagGetNamePatch.MethodInfo(), null, new HarmonyMethod(typeof(EntityBehaviorNameTagGetNamePatch).GetMethod("Postfix", 24)), null, null);
		}

		// Token: 0x060000BF RID: 191 RVA: 0x00006DE1 File Offset: 0x00004FE1
		public static void Unpatch(Harmony harmony)
		{
			harmony.Unpatch(EntityBehaviorNameTagGetNamePatch.MethodInfo(), 2, "gerste.petai");
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00006DF4 File Offset: 0x00004FF4
		public static MethodInfo MethodInfo()
		{
			return typeof(EntityBehaviorNameTag).GetMethod("GetName", 20);
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x00006E0C File Offset: 0x0000500C
		public static void Postfix(ref string __result)
		{
			if (string.IsNullOrWhiteSpace(__result))
			{
				__result = null;
			}
		}
	}
}
