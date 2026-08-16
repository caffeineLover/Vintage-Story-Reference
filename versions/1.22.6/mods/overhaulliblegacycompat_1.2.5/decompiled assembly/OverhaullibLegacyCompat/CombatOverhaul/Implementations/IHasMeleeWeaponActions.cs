using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Implementations
{
	// Token: 0x0200007E RID: 126
	[NullableContext(1)]
	public interface IHasMeleeWeaponActions
	{
		// Token: 0x06000669 RID: 1641
		bool CanAttack(EntityPlayer player, bool mainHand);

		// Token: 0x0600066A RID: 1642
		bool CanBlock(EntityPlayer player, bool mainHand);

		// Token: 0x0600066B RID: 1643
		bool CanThrow(EntityPlayer player, bool mainHand);
	}
}
