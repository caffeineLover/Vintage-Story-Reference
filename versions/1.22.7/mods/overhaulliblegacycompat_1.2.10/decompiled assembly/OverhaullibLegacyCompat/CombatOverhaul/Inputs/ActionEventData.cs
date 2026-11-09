using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;

namespace CombatOverhaul.Inputs
{
	// Token: 0x02000062 RID: 98
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct ActionEventData
	{
		// Token: 0x060004D8 RID: 1240 RVA: 0x0001C595 File Offset: 0x0001A795
		public ActionEventData(ActionEventId action, IEnumerable<EnumEntityAction> modifiers, bool altPressed)
		{
			this.Action = action;
			this.Modifiers = modifiers;
			this.AltPressed = altPressed;
		}

		// Token: 0x040001D0 RID: 464
		public readonly ActionEventId Action;

		// Token: 0x040001D1 RID: 465
		public readonly IEnumerable<EnumEntityAction> Modifiers;

		// Token: 0x040001D2 RID: 466
		public readonly bool AltPressed;
	}
}
