using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000E4 RID: 228
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ImpaleAttackStats
	{
		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000BCB RID: 3019 RVA: 0x0003C6A6 File Offset: 0x0003A8A6
		// (set) Token: 0x06000BCC RID: 3020 RVA: 0x0003C6AE File Offset: 0x0003A8AE
		public bool Enabled { get; set; }

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000BCD RID: 3021 RVA: 0x0003C6B7 File Offset: 0x0003A8B7
		// (set) Token: 0x06000BCE RID: 3022 RVA: 0x0003C6BF File Offset: 0x0003A8BF
		public string TargetFilter { get; set; } = "HostileOnly";

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000BCF RID: 3023 RVA: 0x0003C6C8 File Offset: 0x0003A8C8
		// (set) Token: 0x06000BD0 RID: 3024 RVA: 0x0003C6D0 File Offset: 0x0003A8D0
		public float MaxTargetVolume { get; set; } = 3f;

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000BD1 RID: 3025 RVA: 0x0003C6D9 File Offset: 0x0003A8D9
		// (set) Token: 0x06000BD2 RID: 3026 RVA: 0x0003C6E1 File Offset: 0x0003A8E1
		public float[] HoldOffset { get; set; } = new float[]
		{
			default(float),
			default(float),
			1.25f
		};

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000BD3 RID: 3027 RVA: 0x0003C6EA File Offset: 0x0003A8EA
		// (set) Token: 0x06000BD4 RID: 3028 RVA: 0x0003C6F2 File Offset: 0x0003A8F2
		public float[] TargetOffset { get; set; }

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000BD5 RID: 3029 RVA: 0x0003C6FB File Offset: 0x0003A8FB
		// (set) Token: 0x06000BD6 RID: 3030 RVA: 0x0003C703 File Offset: 0x0003A903
		public float ThrowVelocity { get; set; }

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000BD7 RID: 3031 RVA: 0x0003C70C File Offset: 0x0003A90C
		// (set) Token: 0x06000BD8 RID: 3032 RVA: 0x0003C714 File Offset: 0x0003A914
		public float ThrowUpBias { get; set; }

		// Token: 0x06000BD9 RID: 3033 RVA: 0x0003C720 File Offset: 0x0003A920
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
