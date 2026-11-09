using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul
{
	// Token: 0x02000007 RID: 7
	[NullableContext(1)]
	public interface IWearableLightSource
	{
		// Token: 0x06000052 RID: 82
		byte[] GetLightHsv(EntityPlayer player, ItemSlot slot);
	}
}
