using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CombatOverhaul.DamageSystems;
using CombatOverhaul.MeleeSystems;

namespace CombatOverhaul.Implementations
{
	// Token: 0x02000083 RID: 131
	[NullableContext(2)]
	[Nullable(0)]
	public class StanceStats
	{
		// Token: 0x1700012E RID: 302
		// (get) Token: 0x060006B4 RID: 1716 RVA: 0x0002A730 File Offset: 0x00028930
		// (set) Token: 0x060006B5 RID: 1717 RVA: 0x0002A738 File Offset: 0x00028938
		public bool CanAttack { get; set; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x0002A741 File Offset: 0x00028941
		// (set) Token: 0x060006B7 RID: 1719 RVA: 0x0002A749 File Offset: 0x00028949
		public bool CanParry { get; set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x0002A752 File Offset: 0x00028952
		// (set) Token: 0x060006B9 RID: 1721 RVA: 0x0002A75A File Offset: 0x0002895A
		public bool CanBlock { get; set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060006BA RID: 1722 RVA: 0x0002A763 File Offset: 0x00028963
		// (set) Token: 0x060006BB RID: 1723 RVA: 0x0002A76B File Offset: 0x0002896B
		public bool CanSprint { get; set; } = true;

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060006BC RID: 1724 RVA: 0x0002A774 File Offset: 0x00028974
		// (set) Token: 0x060006BD RID: 1725 RVA: 0x0002A77C File Offset: 0x0002897C
		public bool CanThrow { get; set; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060006BE RID: 1726 RVA: 0x0002A785 File Offset: 0x00028985
		// (set) Token: 0x060006BF RID: 1727 RVA: 0x0002A78D File Offset: 0x0002898D
		public bool CanBash { get; set; }

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060006C0 RID: 1728 RVA: 0x0002A796 File Offset: 0x00028996
		// (set) Token: 0x060006C1 RID: 1729 RVA: 0x0002A79E File Offset: 0x0002899E
		public bool CanRiposte { get; set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060006C2 RID: 1730 RVA: 0x0002A7A7 File Offset: 0x000289A7
		// (set) Token: 0x060006C3 RID: 1731 RVA: 0x0002A7AF File Offset: 0x000289AF
		public float SpeedPenalty { get; set; }

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060006C4 RID: 1732 RVA: 0x0002A7B8 File Offset: 0x000289B8
		// (set) Token: 0x060006C5 RID: 1733 RVA: 0x0002A7C0 File Offset: 0x000289C0
		public float BlockSpeedPenalty { get; set; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060006C6 RID: 1734 RVA: 0x0002A7C9 File Offset: 0x000289C9
		// (set) Token: 0x060006C7 RID: 1735 RVA: 0x0002A7D1 File Offset: 0x000289D1
		public float GripLengthFactor { get; set; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060006C8 RID: 1736 RVA: 0x0002A7DA File Offset: 0x000289DA
		// (set) Token: 0x060006C9 RID: 1737 RVA: 0x0002A7E2 File Offset: 0x000289E2
		public float GripMinLength { get; set; }

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060006CA RID: 1738 RVA: 0x0002A7EB File Offset: 0x000289EB
		// (set) Token: 0x060006CB RID: 1739 RVA: 0x0002A7F3 File Offset: 0x000289F3
		public float GripMaxLength { get; set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060006CC RID: 1740 RVA: 0x0002A7FC File Offset: 0x000289FC
		// (set) Token: 0x060006CD RID: 1741 RVA: 0x0002A804 File Offset: 0x00028A04
		public MeleeAttackStats Attack { get; set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060006CE RID: 1742 RVA: 0x0002A80D File Offset: 0x00028A0D
		// (set) Token: 0x060006CF RID: 1743 RVA: 0x0002A815 File Offset: 0x00028A15
		public MeleeAttackStats Riposte { get; set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x0002A81E File Offset: 0x00028A1E
		// (set) Token: 0x060006D1 RID: 1745 RVA: 0x0002A826 File Offset: 0x00028A26
		public MeleeAttackStats BlockBash { get; set; }

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060006D2 RID: 1746 RVA: 0x0002A82F File Offset: 0x00028A2F
		// (set) Token: 0x060006D3 RID: 1747 RVA: 0x0002A837 File Offset: 0x00028A37
		[Nullable(new byte[]
		{
			2,
			1,
			1
		})]
		public Dictionary<string, MeleeAttackStats> DirectionalAttacks { [return: Nullable(new byte[]
		{
			2,
			1,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1,
			1
		})] set; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x060006D4 RID: 1748 RVA: 0x0002A840 File Offset: 0x00028A40
		// (set) Token: 0x060006D5 RID: 1749 RVA: 0x0002A848 File Offset: 0x00028A48
		[Nullable(new byte[]
		{
			2,
			1,
			1
		})]
		public Dictionary<string, MeleeAttackStats> DirectionalBlockBashes { [return: Nullable(new byte[]
		{
			2,
			1,
			1
		})] get; [param: Nullable(new byte[]
		{
			2,
			1,
			1
		})] set; }

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060006D6 RID: 1750 RVA: 0x0002A851 File Offset: 0x00028A51
		// (set) Token: 0x060006D7 RID: 1751 RVA: 0x0002A859 File Offset: 0x00028A59
		public DamageBlockJson Block { get; set; }

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060006D8 RID: 1752 RVA: 0x0002A862 File Offset: 0x00028A62
		// (set) Token: 0x060006D9 RID: 1753 RVA: 0x0002A86A File Offset: 0x00028A6A
		public DamageBlockJson Parry { get; set; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x0002A873 File Offset: 0x00028A73
		// (set) Token: 0x060006DB RID: 1755 RVA: 0x0002A87B File Offset: 0x00028A7B
		public MeleeAttackStats HandleAttack { get; set; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060006DC RID: 1756 RVA: 0x0002A884 File Offset: 0x00028A84
		// (set) Token: 0x060006DD RID: 1757 RVA: 0x0002A88C File Offset: 0x00028A8C
		public string AttackHitSound { get; set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x0002A895 File Offset: 0x00028A95
		// (set) Token: 0x060006DF RID: 1759 RVA: 0x0002A89D File Offset: 0x00028A9D
		public string BashHitSound { get; set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060006E0 RID: 1760 RVA: 0x0002A8A6 File Offset: 0x00028AA6
		// (set) Token: 0x060006E1 RID: 1761 RVA: 0x0002A8AE File Offset: 0x00028AAE
		public string HandleHitSound { get; set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x0002A8B7 File Offset: 0x00028AB7
		// (set) Token: 0x060006E3 RID: 1763 RVA: 0x0002A8BF File Offset: 0x00028ABF
		public float AttackCooldownMs { get; set; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x0002A8C8 File Offset: 0x00028AC8
		// (set) Token: 0x060006E5 RID: 1765 RVA: 0x0002A8D0 File Offset: 0x00028AD0
		public float BlockCooldownMs { get; set; }

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x0002A8D9 File Offset: 0x00028AD9
		// (set) Token: 0x060006E7 RID: 1767 RVA: 0x0002A8E1 File Offset: 0x00028AE1
		public float ParryCooldownMs { get; set; }

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060006E8 RID: 1768 RVA: 0x0002A8EA File Offset: 0x00028AEA
		// (set) Token: 0x060006E9 RID: 1769 RVA: 0x0002A8F2 File Offset: 0x00028AF2
		public bool ParryWithoutDelay { get; set; } = true;

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x0002A8FB File Offset: 0x00028AFB
		// (set) Token: 0x060006EB RID: 1771 RVA: 0x0002A903 File Offset: 0x00028B03
		[Nullable(1)]
		public string AttackDirectionsType { [NullableContext(1)] get; [NullableContext(1)] set; } = "None";

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x0002A90C File Offset: 0x00028B0C
		// (set) Token: 0x060006ED RID: 1773 RVA: 0x0002A914 File Offset: 0x00028B14
		[Nullable(1)]
		public Dictionary<string, string[]> AttackAnimation { [NullableContext(1)] get; [NullableContext(1)] set; } = new Dictionary<string, string[]>();

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x0002A91D File Offset: 0x00028B1D
		// (set) Token: 0x060006EF RID: 1775 RVA: 0x0002A925 File Offset: 0x00028B25
		[Nullable(1)]
		public Dictionary<string, string[]> BlockBashAnimation { [NullableContext(1)] get; [NullableContext(1)] set; } = new Dictionary<string, string[]>();

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x0002A92E File Offset: 0x00028B2E
		// (set) Token: 0x060006F1 RID: 1777 RVA: 0x0002A936 File Offset: 0x00028B36
		public string ParryAnimation { get; set; }

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x0002A93F File Offset: 0x00028B3F
		// (set) Token: 0x060006F3 RID: 1779 RVA: 0x0002A947 File Offset: 0x00028B47
		public string BlockAnimation { get; set; }

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x0002A950 File Offset: 0x00028B50
		// (set) Token: 0x060006F5 RID: 1781 RVA: 0x0002A958 File Offset: 0x00028B58
		public string RiposteAnimation { get; set; }

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x0002A961 File Offset: 0x00028B61
		// (set) Token: 0x060006F7 RID: 1783 RVA: 0x0002A969 File Offset: 0x00028B69
		public string ReadyAnimation { get; set; }

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060006F8 RID: 1784 RVA: 0x0002A972 File Offset: 0x00028B72
		// (set) Token: 0x060006F9 RID: 1785 RVA: 0x0002A97A File Offset: 0x00028B7A
		public string IdleAnimation { get; set; }

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x0002A983 File Offset: 0x00028B83
		// (set) Token: 0x060006FB RID: 1787 RVA: 0x0002A98B File Offset: 0x00028B8B
		public string WalkAnimation { get; set; }

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x0002A994 File Offset: 0x00028B94
		// (set) Token: 0x060006FD RID: 1789 RVA: 0x0002A99C File Offset: 0x00028B9C
		public string RunAnimation { get; set; }

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x0002A9A5 File Offset: 0x00028BA5
		// (set) Token: 0x060006FF RID: 1791 RVA: 0x0002A9AD File Offset: 0x00028BAD
		public string SwimAnimation { get; set; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x0002A9B6 File Offset: 0x00028BB6
		// (set) Token: 0x06000701 RID: 1793 RVA: 0x0002A9BE File Offset: 0x00028BBE
		public string SwimIdleAnimation { get; set; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x0002A9C7 File Offset: 0x00028BC7
		// (set) Token: 0x06000703 RID: 1795 RVA: 0x0002A9CF File Offset: 0x00028BCF
		public float AttackSpeedMultiplier { get; set; } = 1f;
	}
}
