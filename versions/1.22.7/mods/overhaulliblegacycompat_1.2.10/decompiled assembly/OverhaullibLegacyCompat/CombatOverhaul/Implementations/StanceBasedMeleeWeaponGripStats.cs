using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CombatOverhaul.DamageSystems;

namespace CombatOverhaul.Implementations
{
	// Token: 0x02000092 RID: 146
	[NullableContext(1)]
	[Nullable(0)]
	public class StanceBasedMeleeWeaponGripStats
	{
		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x0002DAD3 File Offset: 0x0002BCD3
		// (set) Token: 0x060007B0 RID: 1968 RVA: 0x0002DADB File Offset: 0x0002BCDB
		public float GripLengthFactor { get; set; }

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060007B1 RID: 1969 RVA: 0x0002DAE4 File Offset: 0x0002BCE4
		// (set) Token: 0x060007B2 RID: 1970 RVA: 0x0002DAEC File Offset: 0x0002BCEC
		public float GripMinLength { get; set; }

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060007B3 RID: 1971 RVA: 0x0002DAF5 File Offset: 0x0002BCF5
		// (set) Token: 0x060007B4 RID: 1972 RVA: 0x0002DAFD File Offset: 0x0002BCFD
		public float GripMaxLength { get; set; }

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x0002DB06 File Offset: 0x0002BD06
		// (set) Token: 0x060007B6 RID: 1974 RVA: 0x0002DB0E File Offset: 0x0002BD0E
		public string AttackDirectionsType { get; set; } = "None";

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060007B7 RID: 1975 RVA: 0x0002DB17 File Offset: 0x0002BD17
		// (set) Token: 0x060007B8 RID: 1976 RVA: 0x0002DB1F File Offset: 0x0002BD1F
		public string InitialStance { get; set; } = "Top";

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060007B9 RID: 1977 RVA: 0x0002DB28 File Offset: 0x0002BD28
		// (set) Token: 0x060007BA RID: 1978 RVA: 0x0002DB30 File Offset: 0x0002BD30
		[Nullable(2)]
		public StanceBasedMeleeWeaponAttackStats DefaultRightClickAttack { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060007BB RID: 1979 RVA: 0x0002DB39 File Offset: 0x0002BD39
		// (set) Token: 0x060007BC RID: 1980 RVA: 0x0002DB41 File Offset: 0x0002BD41
		public Dictionary<string, StanceBasedMeleeWeaponAttackStats> StanceToStanceRightClickAttacks { get; set; } = new Dictionary<string, StanceBasedMeleeWeaponAttackStats>();

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x0002DB4A File Offset: 0x0002BD4A
		// (set) Token: 0x060007BE RID: 1982 RVA: 0x0002DB52 File Offset: 0x0002BD52
		[Nullable(2)]
		public StanceBasedMeleeWeaponAttackStats DefaultLeftClickAttack { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060007BF RID: 1983 RVA: 0x0002DB5B File Offset: 0x0002BD5B
		// (set) Token: 0x060007C0 RID: 1984 RVA: 0x0002DB63 File Offset: 0x0002BD63
		public Dictionary<string, StanceBasedMeleeWeaponAttackStats> StanceToStanceLeftClickAttacks { get; set; } = new Dictionary<string, StanceBasedMeleeWeaponAttackStats>();

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060007C1 RID: 1985 RVA: 0x0002DB6C File Offset: 0x0002BD6C
		// (set) Token: 0x060007C2 RID: 1986 RVA: 0x0002DB74 File Offset: 0x0002BD74
		[Nullable(2)]
		public DamageBlockJson DefaultBlock { [NullableContext(2)] get; [NullableContext(2)] set; }

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x0002DB7D File Offset: 0x0002BD7D
		// (set) Token: 0x060007C4 RID: 1988 RVA: 0x0002DB85 File Offset: 0x0002BD85
		public Dictionary<string, DamageBlockJson> BlockByStance { get; set; } = new Dictionary<string, DamageBlockJson>();

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060007C5 RID: 1989 RVA: 0x0002DB8E File Offset: 0x0002BD8E
		// (set) Token: 0x060007C6 RID: 1990 RVA: 0x0002DB96 File Offset: 0x0002BD96
		public Dictionary<string, string> StanceAnimations { get; set; } = new Dictionary<string, string>();

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x060007C7 RID: 1991 RVA: 0x0002DB9F File Offset: 0x0002BD9F
		// (set) Token: 0x060007C8 RID: 1992 RVA: 0x0002DBA7 File Offset: 0x0002BDA7
		public Dictionary<string, string[]> RightClickAttacksAnimations { get; set; } = new Dictionary<string, string[]>();

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x060007C9 RID: 1993 RVA: 0x0002DBB0 File Offset: 0x0002BDB0
		// (set) Token: 0x060007CA RID: 1994 RVA: 0x0002DBB8 File Offset: 0x0002BDB8
		public Dictionary<string, string[]> LeftClickAttacksAnimations { get; set; } = new Dictionary<string, string[]>();
	}
}
