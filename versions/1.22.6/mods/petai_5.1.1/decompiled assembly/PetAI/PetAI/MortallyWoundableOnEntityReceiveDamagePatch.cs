using System;
using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace PetAI
{
	// Token: 0x0200001E RID: 30
	public class MortallyWoundableOnEntityReceiveDamagePatch
	{
		// Token: 0x060000B9 RID: 185 RVA: 0x00006D0A File Offset: 0x00004F0A
		public static void Patch(Harmony harmony)
		{
			harmony.Patch(MortallyWoundableOnEntityReceiveDamagePatch.MethodInfo(), new HarmonyMethod(typeof(MortallyWoundableOnEntityReceiveDamagePatch).GetMethod("Prefix", 24)), null, null, null);
		}

		// Token: 0x060000BA RID: 186 RVA: 0x00006D36 File Offset: 0x00004F36
		public static void Unpatch(Harmony harmony)
		{
			harmony.Unpatch(MortallyWoundableOnEntityReceiveDamagePatch.MethodInfo(), 1, "gerste.petai");
		}

		// Token: 0x060000BB RID: 187 RVA: 0x00006D49 File Offset: 0x00004F49
		public static MethodInfo MethodInfo()
		{
			return typeof(EntityBehaviorMortallyWoundable).GetMethod("OnEntityReceiveDamage", 20);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00006D64 File Offset: 0x00004F64
		public static bool Prefix(EntityBehaviorMortallyWoundable __instance, DamageSource damageSource, ref float damage)
		{
			EntityBehaviorTameable tameable = __instance.entity.GetBehavior<EntityBehaviorTameable>();
			if (tameable != null && string.IsNullOrEmpty(tameable.OwnerId))
			{
				return false;
			}
			if (tameable == null || __instance.HealthState == null || damageSource.Type == 6)
			{
				return true;
			}
			damage = 0f;
			return false;
		}
	}
}
