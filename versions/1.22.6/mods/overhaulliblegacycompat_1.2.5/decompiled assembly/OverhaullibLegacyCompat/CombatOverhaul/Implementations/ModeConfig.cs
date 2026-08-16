using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Implementations
{
	// Token: 0x0200009C RID: 156
	[NullableContext(1)]
	[Nullable(0)]
	public readonly struct ModeConfig
	{
		// Token: 0x06000854 RID: 2132 RVA: 0x0003060A File Offset: 0x0002E80A
		public ModeConfig(string code, string name, string icon)
		{
			this.Code = code;
			this.Name = name;
			this.Icon = icon;
		}

		// Token: 0x040003CD RID: 973
		public readonly string Code;

		// Token: 0x040003CE RID: 974
		public readonly string Name;

		// Token: 0x040003CF RID: 975
		public readonly string Icon;
	}
}
