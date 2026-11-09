using System;
using Vintagestory.API.Common;

namespace CombatOverhaul.Inputs
{
	// Token: 0x02000061 RID: 97
	public readonly struct ActionEventId
	{
		// Token: 0x060004D7 RID: 1239 RVA: 0x0001C585 File Offset: 0x0001A785
		public ActionEventId(EnumEntityAction action, ActionState state)
		{
			this.Action = action;
			this.State = state;
		}

		// Token: 0x040001CE RID: 462
		public readonly EnumEntityAction Action;

		// Token: 0x040001CF RID: 463
		public readonly ActionState State;
	}
}
