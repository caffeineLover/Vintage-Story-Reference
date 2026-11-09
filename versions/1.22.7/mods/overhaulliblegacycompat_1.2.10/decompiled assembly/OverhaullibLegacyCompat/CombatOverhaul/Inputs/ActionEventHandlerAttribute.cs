using System;
using Vintagestory.API.Common;

namespace CombatOverhaul.Inputs
{
	// Token: 0x02000064 RID: 100
	[AttributeUsage(64)]
	public class ActionEventHandlerAttribute : Attribute
	{
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060004FA RID: 1274 RVA: 0x0001D083 File Offset: 0x0001B283
		public ActionEventId Event { get; }

		// Token: 0x060004FB RID: 1275 RVA: 0x0001D08B File Offset: 0x0001B28B
		public ActionEventHandlerAttribute(EnumEntityAction action, ActionState state)
		{
			this.Event = new ActionEventId(action, state);
		}
	}
}
