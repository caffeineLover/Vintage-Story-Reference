using System;

namespace PetAI
{
	// Token: 0x02000025 RID: 37
	public class PetConfig
	{
		// Token: 0x0400005C RID: 92
		public static PetConfig Current;

		// Token: 0x0400005D RID: 93
		public Difficulty Difficulty = new Difficulty();

		// Token: 0x0400005E RID: 94
		public bool PvpOn = true;

		// Token: 0x0400005F RID: 95
		public bool PetDamageableByOwner;

		// Token: 0x04000060 RID: 96
		public bool FalldamageOff = true;

		// Token: 0x04000061 RID: 97
		public bool AllowTeleport;

		// Token: 0x04000062 RID: 98
		public string[] Resurrectors = new string[]
		{
			"game:gear-temporal"
		};
	}
}
