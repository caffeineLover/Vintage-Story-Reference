using System;

namespace PetAI
{
	// Token: 0x02000026 RID: 38
	public class Difficulty
	{
		// Token: 0x04000063 RID: 99
		public float petMaxHpModifier;

		// Token: 0x04000064 RID: 100
		public float petDamageMultiplier = 1f;

		// Token: 0x04000065 RID: 101
		public float petSpeedMultiplier = 1f;

		// Token: 0x04000066 RID: 102
		public float tamingMultiplier = 1f;

		// Token: 0x04000067 RID: 103
		public float obedienceMultiplier = 1f;

		// Token: 0x04000068 RID: 104
		public float disobedienceMultiplier = 1f;

		// Token: 0x04000069 RID: 105
		public float growingMultiplier = 1f;

		// Token: 0x0400006A RID: 106
		public float tamingMultiplierIncreasePerGen = 0.05f;

		// Token: 0x0400006B RID: 107
		public float obedienceMultiplierIncreasePerGen = 0.05f;

		// Token: 0x0400006C RID: 108
		public float disobedienceMultiplierDecreasePerGen = 0.05f;
	}
}
