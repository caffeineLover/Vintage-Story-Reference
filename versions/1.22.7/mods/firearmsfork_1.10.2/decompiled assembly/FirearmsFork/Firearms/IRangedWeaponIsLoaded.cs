using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace Firearms
{
	// Token: 0x02000024 RID: 36
	[NullableContext(1)]
	public interface IRangedWeaponIsLoaded
	{
		// Token: 0x060001AF RID: 431
		bool GetIsLoaded(EntityPlayer player, ItemSlot slot);
	}
}
