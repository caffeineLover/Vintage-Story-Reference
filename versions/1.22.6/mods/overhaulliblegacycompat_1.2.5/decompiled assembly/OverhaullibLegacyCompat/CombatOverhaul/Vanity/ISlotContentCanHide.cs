using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Vanity
{
	// Token: 0x020000B1 RID: 177
	[NullableContext(2)]
	public interface ISlotContentCanHide
	{
		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000963 RID: 2403
		// (set) Token: 0x06000964 RID: 2404
		bool Hide { get; set; }

		// Token: 0x0400044E RID: 1102
		public const string HiddenSlotColor = "#776666";

		// Token: 0x0400044F RID: 1103
		public const string ActiveSlotColor = null;
	}
}
