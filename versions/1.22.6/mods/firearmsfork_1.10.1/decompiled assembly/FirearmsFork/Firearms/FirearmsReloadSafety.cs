using System;
using System.Runtime.CompilerServices;
using CombatOverhaul.Inputs;

namespace Firearms
{
	// Token: 0x02000006 RID: 6
	internal static class FirearmsReloadSafety
	{
		// Token: 0x0600004D RID: 77 RVA: 0x00002C70 File Offset: 0x00000E70
		public static float AnimationSpeed(float speed)
		{
			if (float.IsNaN(speed) || float.IsInfinity(speed) || speed <= 0f)
			{
				return 1f;
			}
			return Math.Clamp(speed, 0.05f, 10f);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002CA0 File Offset: 0x00000EA0
		[NullableContext(1)]
		public static void SetReloadWalkspeedPenalty([Nullable(2)] ActionsManagerPlayerBehavior playerBehavior, bool mainHand, string mainHandCategory, string offHandCategory, float penalty)
		{
			if (float.IsNaN(penalty) || float.IsInfinity(penalty))
			{
				penalty = 0f;
			}
			if (playerBehavior != null)
			{
				playerBehavior.SetStat("walkspeed", mainHand ? mainHandCategory : offHandCategory, Math.Clamp(penalty, -0.95f, 0f));
			}
		}

		// Token: 0x0400002C RID: 44
		private const float MinAnimationSpeed = 0.05f;

		// Token: 0x0400002D RID: 45
		private const float MaxAnimationSpeed = 10f;

		// Token: 0x0400002E RID: 46
		private const float MinWalkspeedPenalty = -0.95f;

		// Token: 0x0400002F RID: 47
		private const float MaxWalkspeedPenalty = 0f;
	}
}
