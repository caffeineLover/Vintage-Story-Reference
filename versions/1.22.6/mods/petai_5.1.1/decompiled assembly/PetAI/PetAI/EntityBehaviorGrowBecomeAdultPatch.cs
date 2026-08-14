using System;
using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;

namespace PetAI
{
	// Token: 0x02000020 RID: 32
	public class EntityBehaviorGrowBecomeAdultPatch
	{
		// Token: 0x060000C3 RID: 195 RVA: 0x00006E22 File Offset: 0x00005022
		public static void Patch(Harmony harmony)
		{
			harmony.Patch(EntityBehaviorGrowBecomeAdultPatch.MethodInfo(), null, new HarmonyMethod(typeof(EntityBehaviorGrowBecomeAdultPatch).GetMethod("Postfix", 24)), null, null);
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00006E4E File Offset: 0x0000504E
		public static void Unpatch(Harmony harmony)
		{
			harmony.Unpatch(EntityBehaviorGrowBecomeAdultPatch.MethodInfo(), 2, "gerste.petai");
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00006E61 File Offset: 0x00005061
		public static MethodInfo MethodInfo()
		{
			return typeof(EntityBehaviorGrow).GetMethod("BecomeAdult", 36);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00006E7C File Offset: 0x0000507C
		public static void Postfix(EntityBehaviorGrow __instance, Entity adult)
		{
			Entity entity = __instance.entity;
			if (adult.HasBehavior<EntityBehaviorTameable>())
			{
				adult.GetBehavior<EntityBehaviorTameable>().DomesticationStatus = entity.GetBehavior<EntityBehaviorTameable>().DomesticationStatus;
			}
			EntityBehaviorNameTag behavior = adult.GetBehavior<EntityBehaviorNameTag>();
			if (behavior == null)
			{
				return;
			}
			EntityBehaviorNameTag behavior2 = entity.GetBehavior<EntityBehaviorNameTag>();
			behavior.SetName((behavior2 != null) ? behavior2.DisplayName : null);
		}
	}
}
