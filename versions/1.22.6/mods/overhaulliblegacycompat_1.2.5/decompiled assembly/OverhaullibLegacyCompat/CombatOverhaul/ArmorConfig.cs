using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul
{
	// Token: 0x02000013 RID: 19
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ArmorConfig
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000BE RID: 190 RVA: 0x0000449E File Offset: 0x0000269E
		// (set) Token: 0x060000BF RID: 191 RVA: 0x000044A6 File Offset: 0x000026A6
		public int MaxAttackTier { get; set; } = 9;

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x000044AF File Offset: 0x000026AF
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x000044B7 File Offset: 0x000026B7
		public int MaxArmorTier { get; set; } = 24;

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x000044C0 File Offset: 0x000026C0
		// (set) Token: 0x060000C3 RID: 195 RVA: 0x000044C8 File Offset: 0x000026C8
		public float[][] DamageReduction { get; set; } = new float[][]
		{
			new float[]
			{
				0.5f,
				1f,
				1f,
				1f,
				1f,
				1f,
				1f,
				1f,
				1f
			},
			new float[]
			{
				0.25f,
				0.5f,
				1f,
				1f,
				1f,
				1f,
				1f,
				1f,
				1f
			},
			new float[]
			{
				0.2f,
				0.25f,
				0.5f,
				1f,
				1f,
				1f,
				1f,
				1f,
				1f
			},
			new float[]
			{
				0.15f,
				0.2f,
				0.25f,
				0.5f,
				1f,
				1f,
				1f,
				1f,
				1f
			},
			new float[]
			{
				0.12f,
				0.15f,
				0.2f,
				0.25f,
				0.5f,
				1f,
				1f,
				1f,
				1f
			},
			new float[]
			{
				0.1f,
				0.12f,
				0.15f,
				0.2f,
				0.25f,
				0.5f,
				1f,
				1f,
				1f
			},
			new float[]
			{
				0.08f,
				0.1f,
				0.12f,
				0.15f,
				0.2f,
				0.3f,
				0.6f,
				1f,
				1f
			},
			new float[]
			{
				0.06f,
				0.08f,
				0.1f,
				0.12f,
				0.15f,
				0.25f,
				0.5f,
				0.75f,
				1f
			},
			new float[]
			{
				0.04f,
				0.06f,
				0.08f,
				0.1f,
				0.12f,
				0.2f,
				0.4f,
				0.6f,
				0.9f
			},
			new float[]
			{
				0.02f,
				0.04f,
				0.06f,
				0.08f,
				0.1f,
				0.15f,
				0.3f,
				0.5f,
				0.8f
			},
			new float[]
			{
				0.01f,
				0.02f,
				0.04f,
				0.06f,
				0.08f,
				0.1f,
				0.2f,
				0.4f,
				0.7f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.02f,
				0.04f,
				0.06f,
				0.08f,
				0.16f,
				0.3f,
				0.6f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.01f,
				0.02f,
				0.04f,
				0.06f,
				0.12f,
				0.25f,
				0.5f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.02f,
				0.04f,
				0.08f,
				0.12f,
				0.25f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.02f,
				0.04f,
				0.08f,
				0.12f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.02f,
				0.04f,
				0.08f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.02f,
				0.04f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.02f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f
			},
			new float[]
			{
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f,
				0.01f
			}
		};
	}
}
