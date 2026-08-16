using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000ED RID: 237
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public class MeleeDamagePacket
	{
		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000C38 RID: 3128 RVA: 0x0003E081 File Offset: 0x0003C281
		// (set) Token: 0x06000C39 RID: 3129 RVA: 0x0003E089 File Offset: 0x0003C289
		public string DamageType { get; set; } = "";

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000C3A RID: 3130 RVA: 0x0003E092 File Offset: 0x0003C292
		// (set) Token: 0x06000C3B RID: 3131 RVA: 0x0003E09A File Offset: 0x0003C29A
		public int Tier { get; set; }

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000C3C RID: 3132 RVA: 0x0003E0A3 File Offset: 0x0003C2A3
		// (set) Token: 0x06000C3D RID: 3133 RVA: 0x0003E0AB File Offset: 0x0003C2AB
		public int ArmorPiercingTier { get; set; }

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000C3E RID: 3134 RVA: 0x0003E0B4 File Offset: 0x0003C2B4
		// (set) Token: 0x06000C3F RID: 3135 RVA: 0x0003E0BC File Offset: 0x0003C2BC
		public float Damage { get; set; }

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000C40 RID: 3136 RVA: 0x0003E0C5 File Offset: 0x0003C2C5
		// (set) Token: 0x06000C41 RID: 3137 RVA: 0x0003E0CD File Offset: 0x0003C2CD
		public float Knockback { get; set; }

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000C42 RID: 3138 RVA: 0x0003E0D6 File Offset: 0x0003C2D6
		// (set) Token: 0x06000C43 RID: 3139 RVA: 0x0003E0DE File Offset: 0x0003C2DE
		public double[] Position { get; set; } = Array.Empty<double>();

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000C44 RID: 3140 RVA: 0x0003E0E7 File Offset: 0x0003C2E7
		// (set) Token: 0x06000C45 RID: 3141 RVA: 0x0003E0EF File Offset: 0x0003C2EF
		public string Collider { get; set; } = "";

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000C46 RID: 3142 RVA: 0x0003E0F8 File Offset: 0x0003C2F8
		// (set) Token: 0x06000C47 RID: 3143 RVA: 0x0003E100 File Offset: 0x0003C300
		public int ColliderType { get; set; }

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000C48 RID: 3144 RVA: 0x0003E109 File Offset: 0x0003C309
		// (set) Token: 0x06000C49 RID: 3145 RVA: 0x0003E111 File Offset: 0x0003C311
		public long AttackerEntityId { get; set; }

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000C4A RID: 3146 RVA: 0x0003E11A File Offset: 0x0003C31A
		// (set) Token: 0x06000C4B RID: 3147 RVA: 0x0003E122 File Offset: 0x0003C322
		public long TargetEntityId { get; set; }

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000C4C RID: 3148 RVA: 0x0003E12B File Offset: 0x0003C32B
		// (set) Token: 0x06000C4D RID: 3149 RVA: 0x0003E133 File Offset: 0x0003C333
		public int DurabilityDamage { get; set; }

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000C4E RID: 3150 RVA: 0x0003E13C File Offset: 0x0003C33C
		// (set) Token: 0x06000C4F RID: 3151 RVA: 0x0003E144 File Offset: 0x0003C344
		public bool MainHand { get; set; }

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000C50 RID: 3152 RVA: 0x0003E14D File Offset: 0x0003C34D
		// (set) Token: 0x06000C51 RID: 3153 RVA: 0x0003E155 File Offset: 0x0003C355
		public int StaggerTimeMs { get; set; }

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000C52 RID: 3154 RVA: 0x0003E15E File Offset: 0x0003C35E
		// (set) Token: 0x06000C53 RID: 3155 RVA: 0x0003E166 File Offset: 0x0003C366
		public int StaggerTier { get; set; }

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000C54 RID: 3156 RVA: 0x0003E16F File Offset: 0x0003C36F
		// (set) Token: 0x06000C55 RID: 3157 RVA: 0x0003E177 File Offset: 0x0003C377
		public int AttackDirection { get; set; }

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000C56 RID: 3158 RVA: 0x0003E180 File Offset: 0x0003C380
		// (set) Token: 0x06000C57 RID: 3159 RVA: 0x0003E188 File Offset: 0x0003C388
		public bool ImpaleEnabled { get; set; }

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000C58 RID: 3160 RVA: 0x0003E191 File Offset: 0x0003C391
		// (set) Token: 0x06000C59 RID: 3161 RVA: 0x0003E199 File Offset: 0x0003C399
		public string ImpaleTargetFilter { get; set; } = "HostileOnly";

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000C5A RID: 3162 RVA: 0x0003E1A2 File Offset: 0x0003C3A2
		// (set) Token: 0x06000C5B RID: 3163 RVA: 0x0003E1AA File Offset: 0x0003C3AA
		public float ImpaleMaxTargetVolume { get; set; } = 3f;

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000C5C RID: 3164 RVA: 0x0003E1B3 File Offset: 0x0003C3B3
		// (set) Token: 0x06000C5D RID: 3165 RVA: 0x0003E1BB File Offset: 0x0003C3BB
		public float[] ImpaleHoldOffset { get; set; } = Array.Empty<float>();

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000C5E RID: 3166 RVA: 0x0003E1C4 File Offset: 0x0003C3C4
		// (set) Token: 0x06000C5F RID: 3167 RVA: 0x0003E1CC File Offset: 0x0003C3CC
		public float[] ImpaleTargetOffset { get; set; } = Array.Empty<float>();

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000C60 RID: 3168 RVA: 0x0003E1D5 File Offset: 0x0003C3D5
		// (set) Token: 0x06000C61 RID: 3169 RVA: 0x0003E1DD File Offset: 0x0003C3DD
		public float ImpaleThrowVelocity { get; set; } = 14f;

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000C62 RID: 3170 RVA: 0x0003E1E6 File Offset: 0x0003C3E6
		// (set) Token: 0x06000C63 RID: 3171 RVA: 0x0003E1EE File Offset: 0x0003C3EE
		public float ImpaleThrowUpBias { get; set; } = 0.15f;
	}
}
