using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul
{
	// Token: 0x0200000F RID: 15
	[NullableContext(1)]
	public interface IFueledItem
	{
		// Token: 0x0600009D RID: 157
		double GetFuelHours(IPlayer player, ItemSlot slot);

		// Token: 0x0600009E RID: 158
		void AddFuelHours(IPlayer player, ItemSlot slot, double hours);

		// Token: 0x0600009F RID: 159
		bool ConsumeFuelWhenSleeping(IPlayer player, ItemSlot slot);
	}
}
