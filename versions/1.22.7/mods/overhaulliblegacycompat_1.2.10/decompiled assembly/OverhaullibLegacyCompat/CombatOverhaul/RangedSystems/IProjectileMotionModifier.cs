using System;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000C2 RID: 194
	public interface IProjectileMotionModifier
	{
		// Token: 0x1700024D RID: 589
		// (get) Token: 0x06000A19 RID: 2585 RVA: 0x000363B3 File Offset: 0x000345B3
		int Priority
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06000A1A RID: 2586
		void ModifyMotion(ProjectileMotionContext context, float dt);
	}
}
