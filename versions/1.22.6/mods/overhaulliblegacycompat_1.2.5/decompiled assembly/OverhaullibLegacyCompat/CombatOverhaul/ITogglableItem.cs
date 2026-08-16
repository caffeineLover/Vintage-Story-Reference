using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul
{
	// Token: 0x02000010 RID: 16
	[NullableContext(1)]
	public interface ITogglableItem
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000A0 RID: 160
		string HotKeyCode { get; }

		// Token: 0x060000A1 RID: 161
		bool TurnedOn(IPlayer player, ItemSlot slot);

		// Token: 0x060000A2 RID: 162
		void TurnOn(IPlayer player, ItemSlot slot);

		// Token: 0x060000A3 RID: 163
		void TurnOff(IPlayer player, ItemSlot slot);

		// Token: 0x060000A4 RID: 164
		void Toggle(IPlayer player, ItemSlot slot);
	}
}
