using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000CB RID: 203
	internal static class ProjectileCatch
	{
		// Token: 0x06000A5E RID: 2654 RVA: 0x00036F58 File Offset: 0x00035158
		[NullableContext(1)]
		internal static bool TryCatch(ProjectileEntity projectile, Entity owner)
		{
			ItemStack stack = projectile.ProjectileStack;
			if (stack == null || stack.StackSize <= 0)
			{
				return false;
			}
			ItemStack returnedStack = stack.Clone();
			if (!owner.TryGiveItemStack(returnedStack))
			{
				return false;
			}
			projectile.ProjectileStack = null;
			projectile.CanBeCollected = false;
			projectile.Die(3, null);
			return true;
		}
	}
}
