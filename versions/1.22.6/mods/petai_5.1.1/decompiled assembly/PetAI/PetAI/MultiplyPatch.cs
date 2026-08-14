using System;
using System.Reflection;
using HarmonyLib;
using Vintagestory.GameContent;

namespace PetAI
{
	// Token: 0x0200001D RID: 29
	public class MultiplyPatch
	{
		// Token: 0x060000B4 RID: 180 RVA: 0x00006C5F File Offset: 0x00004E5F
		public static void Patch(Harmony harmony)
		{
			harmony.Patch(MultiplyPatch.MethodInfo(), new HarmonyMethod(typeof(MultiplyPatch).GetMethod("Prefix", 24)), null, null, null);
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x00006C8B File Offset: 0x00004E8B
		public static void Unpatch(Harmony harmony)
		{
			harmony.Unpatch(MultiplyPatch.MethodInfo(), 1, "gerste.petai");
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00006C9E File Offset: 0x00004E9E
		public static MethodInfo MethodInfo()
		{
			return typeof(EntityBehaviorMultiply).GetMethod("TryGetPregnant", 36);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00006CB8 File Offset: 0x00004EB8
		public static bool Prefix(ref bool __result, EntityBehaviorMultiply __instance)
		{
			EntityBehaviorTameable behavior = __instance.entity.GetBehavior<EntityBehaviorTameable>();
			bool? multiplyAllowed = (behavior != null) ? new bool?(behavior.MultiplyAllowed) : default(bool?);
			if (multiplyAllowed == null || multiplyAllowed.GetValueOrDefault())
			{
				return true;
			}
			__result = false;
			return false;
		}
	}
}
