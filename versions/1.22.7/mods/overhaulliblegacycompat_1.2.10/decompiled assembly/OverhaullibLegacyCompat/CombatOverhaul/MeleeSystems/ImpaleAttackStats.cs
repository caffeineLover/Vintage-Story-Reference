using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000EF RID: 239
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ImpaleAttackStats
	{
		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000C27 RID: 3111 RVA: 0x0003D696 File Offset: 0x0003B896
		// (set) Token: 0x06000C28 RID: 3112 RVA: 0x0003D69E File Offset: 0x0003B89E
		public bool Enabled { get; set; }

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000C29 RID: 3113 RVA: 0x0003D6A7 File Offset: 0x0003B8A7
		// (set) Token: 0x06000C2A RID: 3114 RVA: 0x0003D6AF File Offset: 0x0003B8AF
		public string TargetFilter { get; set; } = "HostileOnly";

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000C2B RID: 3115 RVA: 0x0003D6B8 File Offset: 0x0003B8B8
		// (set) Token: 0x06000C2C RID: 3116 RVA: 0x0003D6C0 File Offset: 0x0003B8C0
		public float MaxTargetVolume { get; set; } = 3f;

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000C2D RID: 3117 RVA: 0x0003D6C9 File Offset: 0x0003B8C9
		// (set) Token: 0x06000C2E RID: 3118 RVA: 0x0003D6D1 File Offset: 0x0003B8D1
		public float[] HoldOffset { get; set; } = new float[]
		{
			default(float),
			default(float),
			1.25f
		};

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000C2F RID: 3119 RVA: 0x0003D6DA File Offset: 0x0003B8DA
		// (set) Token: 0x06000C30 RID: 3120 RVA: 0x0003D6E2 File Offset: 0x0003B8E2
		public float[] TargetOffset { get; set; }

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000C31 RID: 3121 RVA: 0x0003D6EB File Offset: 0x0003B8EB
		// (set) Token: 0x06000C32 RID: 3122 RVA: 0x0003D6F3 File Offset: 0x0003B8F3
		public float ThrowVelocity { get; set; }

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000C33 RID: 3123 RVA: 0x0003D6FC File Offset: 0x0003B8FC
		// (set) Token: 0x06000C34 RID: 3124 RVA: 0x0003D704 File Offset: 0x0003B904
		public float ThrowUpBias { get; set; }

		// Token: 0x06000C35 RID: 3125 RVA: 0x0003D710 File Offset: 0x0003B910
		public ImpaleAttackStats()
		{
			float[] array = new float[3];
			array[1] = -0.6f;
			this.TargetOffset = array;
			this.ThrowVelocity = 5f;
			this.ThrowUpBias = 0.15f;
			base..ctor();
		}
	}
}
