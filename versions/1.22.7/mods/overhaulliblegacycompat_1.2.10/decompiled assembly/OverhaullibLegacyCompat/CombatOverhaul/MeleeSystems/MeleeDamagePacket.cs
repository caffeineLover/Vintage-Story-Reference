using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul.MeleeSystems
{
	// Token: 0x020000F8 RID: 248
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public class MeleeDamagePacket
	{
		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000C94 RID: 3220 RVA: 0x0003F071 File Offset: 0x0003D271
		// (set) Token: 0x06000C95 RID: 3221 RVA: 0x0003F079 File Offset: 0x0003D279
		public string DamageType { get; set; } = "";

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000C96 RID: 3222 RVA: 0x0003F082 File Offset: 0x0003D282
		// (set) Token: 0x06000C97 RID: 3223 RVA: 0x0003F08A File Offset: 0x0003D28A
		public int Tier { get; set; }

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000C98 RID: 3224 RVA: 0x0003F093 File Offset: 0x0003D293
		// (set) Token: 0x06000C99 RID: 3225 RVA: 0x0003F09B File Offset: 0x0003D29B
		public int ArmorPiercingTier { get; set; }

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000C9A RID: 3226 RVA: 0x0003F0A4 File Offset: 0x0003D2A4
		// (set) Token: 0x06000C9B RID: 3227 RVA: 0x0003F0AC File Offset: 0x0003D2AC
		public float Damage { get; set; }

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000C9C RID: 3228 RVA: 0x0003F0B5 File Offset: 0x0003D2B5
		// (set) Token: 0x06000C9D RID: 3229 RVA: 0x0003F0BD File Offset: 0x0003D2BD
		public float Knockback { get; set; }

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000C9E RID: 3230 RVA: 0x0003F0C6 File Offset: 0x0003D2C6
		// (set) Token: 0x06000C9F RID: 3231 RVA: 0x0003F0CE File Offset: 0x0003D2CE
		public double[] Position { get; set; } = Array.Empty<double>();

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000CA0 RID: 3232 RVA: 0x0003F0D7 File Offset: 0x0003D2D7
		// (set) Token: 0x06000CA1 RID: 3233 RVA: 0x0003F0DF File Offset: 0x0003D2DF
		public string Collider { get; set; } = "";

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000CA2 RID: 3234 RVA: 0x0003F0E8 File Offset: 0x0003D2E8
		// (set) Token: 0x06000CA3 RID: 3235 RVA: 0x0003F0F0 File Offset: 0x0003D2F0
		public int ColliderType { get; set; }

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000CA4 RID: 3236 RVA: 0x0003F0F9 File Offset: 0x0003D2F9
		// (set) Token: 0x06000CA5 RID: 3237 RVA: 0x0003F101 File Offset: 0x0003D301
		public long AttackerEntityId { get; set; }

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000CA6 RID: 3238 RVA: 0x0003F10A File Offset: 0x0003D30A
		// (set) Token: 0x06000CA7 RID: 3239 RVA: 0x0003F112 File Offset: 0x0003D312
		public long TargetEntityId { get; set; }

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000CA8 RID: 3240 RVA: 0x0003F11B File Offset: 0x0003D31B
		// (set) Token: 0x06000CA9 RID: 3241 RVA: 0x0003F123 File Offset: 0x0003D323
		public int DurabilityDamage { get; set; }

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000CAA RID: 3242 RVA: 0x0003F12C File Offset: 0x0003D32C
		// (set) Token: 0x06000CAB RID: 3243 RVA: 0x0003F134 File Offset: 0x0003D334
		public bool MainHand { get; set; }

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000CAC RID: 3244 RVA: 0x0003F13D File Offset: 0x0003D33D
		// (set) Token: 0x06000CAD RID: 3245 RVA: 0x0003F145 File Offset: 0x0003D345
		public int StaggerTimeMs { get; set; }

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000CAE RID: 3246 RVA: 0x0003F14E File Offset: 0x0003D34E
		// (set) Token: 0x06000CAF RID: 3247 RVA: 0x0003F156 File Offset: 0x0003D356
		public int StaggerTier { get; set; }

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000CB0 RID: 3248 RVA: 0x0003F15F File Offset: 0x0003D35F
		// (set) Token: 0x06000CB1 RID: 3249 RVA: 0x0003F167 File Offset: 0x0003D367
		public int AttackDirection { get; set; }

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000CB2 RID: 3250 RVA: 0x0003F170 File Offset: 0x0003D370
		// (set) Token: 0x06000CB3 RID: 3251 RVA: 0x0003F178 File Offset: 0x0003D378
		public bool ImpaleEnabled { get; set; }

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000CB4 RID: 3252 RVA: 0x0003F181 File Offset: 0x0003D381
		// (set) Token: 0x06000CB5 RID: 3253 RVA: 0x0003F189 File Offset: 0x0003D389
		public string ImpaleTargetFilter { get; set; } = "HostileOnly";

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x0003F192 File Offset: 0x0003D392
		// (set) Token: 0x06000CB7 RID: 3255 RVA: 0x0003F19A File Offset: 0x0003D39A
		public float ImpaleMaxTargetVolume { get; set; } = 3f;

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x0003F1A3 File Offset: 0x0003D3A3
		// (set) Token: 0x06000CB9 RID: 3257 RVA: 0x0003F1AB File Offset: 0x0003D3AB
		public float[] ImpaleHoldOffset { get; set; } = Array.Empty<float>();

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000CBA RID: 3258 RVA: 0x0003F1B4 File Offset: 0x0003D3B4
		// (set) Token: 0x06000CBB RID: 3259 RVA: 0x0003F1BC File Offset: 0x0003D3BC
		public float[] ImpaleTargetOffset { get; set; } = Array.Empty<float>();

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000CBC RID: 3260 RVA: 0x0003F1C5 File Offset: 0x0003D3C5
		// (set) Token: 0x06000CBD RID: 3261 RVA: 0x0003F1CD File Offset: 0x0003D3CD
		public float ImpaleThrowVelocity { get; set; } = 14f;

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000CBE RID: 3262 RVA: 0x0003F1D6 File Offset: 0x0003D3D6
		// (set) Token: 0x06000CBF RID: 3263 RVA: 0x0003F1DE File Offset: 0x0003D3DE
		public float ImpaleThrowUpBias { get; set; } = 0.15f;
	}
}
