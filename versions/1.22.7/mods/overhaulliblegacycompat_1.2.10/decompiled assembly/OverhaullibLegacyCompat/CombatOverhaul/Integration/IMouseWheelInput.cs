using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace CombatOverhaul.Integration
{
	// Token: 0x02000043 RID: 67
	[NullableContext(1)]
	public interface IMouseWheelInput
	{
		// Token: 0x06000434 RID: 1076
		bool OnMouseWheel(ItemSlot slot, IClientPlayer byPlayer, float delta);
	}
}
