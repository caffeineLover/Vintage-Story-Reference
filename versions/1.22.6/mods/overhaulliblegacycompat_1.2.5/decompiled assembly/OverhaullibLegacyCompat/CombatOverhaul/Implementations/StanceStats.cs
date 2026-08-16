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
		// (get) Token: 0x060006B4 RID: 1716 RVA: 0x0002A6F8 File Offset: 0x000288F8
		// (set) Token: 0x060006B5 RID: 1717 RVA: 0x0002A700 File Offset: 0x00028900
		public bool CanAttack { get; set; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x0002A709 File Offset: 0x00028909
		// (set) Token: 0x060006B7 RID: 1719 RVA: 0x0002A711 File Offset: 0x00028911
		public bool CanParry { get; set; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x0002A71A File Offset: 0x0002891A
		// (set) Token: 0x060006B9 RID: 1721 RVA: 0x0002A722 File Offset: 0x00028922
		public bool CanBlock { get; set; }

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060006BA RID: 1722 RVA: 0x0002A72B File Offset: 0x0002892B
		// (set) Token: 0x060006BB RID: 1723 RVA: 0x0002A733 File Offset: 0x00028933
		public bool CanSprint { get; set; } = true;

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060006BC RID: 1724 RVA: 0x0002A73C File Offset: 0x0002893C
		// (set) Token: 0x060006BD RID: 1725 RVA: 0x0002A744 File Offset: 0x00028944
		public bool CanThrow { get; set; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x060006BE RID: 1726 RVA: 0x0002A74D File Offset: 0x0002894D
		// (set) Token: 0x060006BF RID: 1727 RVA: 0x0002A755 File Offset: 0x00028955
		public bool CanBash { get; set; }

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x060006C0 RID: 1728 RVA: 0x0002A75E File Offset: 0x0002895E
		// (set) Token: 0x060006C1 RID: 1729 RVA: 0x0002A766 File Offset: 0x00028966
		public bool CanRiposte { get; set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x060006C2 RID: 1730 RVA: 0x0002A76F File Offset: 0x0002896F
		// (set) Token: 0x060006C3 RID: 1731 RVA: 0x0002A777 File Offset: 0x00028977
		public float SpeedPenalty { get; set; }

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x060006C4 RID: 1732 RVA: 0x0002A780 File Offset: 0x00028980
		// (set) Token: 0x060006C5 RID: 1733 RVA: 0x0002A788 File Offset: 0x00028988
		public float BlockSpeedPenalty { get; set; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x060006C6 RID: 1734 RVA: 0x0002A791 File Offset: 0x00028991
		// (set) Token: 0x060006C7 RID: 1735 RVA: 0x0002A799 File Offset: 0x00028999
		public float GripLengthFactor { get; set; }

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x060006C8 RID: 1736 RVA: 0x0002A7A2 File Offset: 0x000289A2
		// (set) Token: 0x060006C9 RID: 1737 RVA: 0x0002A7AA File Offset: 0x000289AA
		public float GripMinLength { get; set; }

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x060006CA RID: 1738 RVA: 0x0002A7B3 File Offset: 0x000289B3
		// (set) Token: 0x060006CB RID: 1739 RVA: 0x0002A7BB File Offset: 0x000289BB
		public float GripMaxLength { get; set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x060006CC RID: 1740 RVA: 0x0002A7C4 File Offset: 0x000289C4
		// (set) Token: 0x060006CD RID: 1741 RVA: 0x0002A7CC File Offset: 0x000289CC
		public MeleeAttackStats Attack { get; set; }

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x060006CE RID: 1742 RVA: 0x0002A7D5 File Offset: 0x000289D5
		// (set) Token: 0x060006CF RID: 1743 RVA: 0x0002A7DD File Offset: 0x000289DD
		public MeleeAttackStats Riposte { get; set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x0002A7E6 File Offset: 0x000289E6
		// (set) Token: 0x060006D1 RID: 1745 RVA: 0x0002A7EE File Offset: 0x000289EE
		public MeleeAttackStats BlockBash { get; set; }

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x060006D2 RID: 1746 RVA: 0x0002A7F7 File Offset: 0x000289F7
		// (set) Token: 0x060006D3 RID: 1747 RVA: 0x0002A7FF File Offset: 0x000289FF
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
		// (get) Token: 0x060006D4 RID: 1748 RVA: 0x0002A808 File Offset: 0x00028A08
		// (set) Token: 0x060006D5 RID: 1749 RVA: 0x0002A810 File Offset: 0x00028A10
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
		// (get) Token: 0x060006D6 RID: 1750 RVA: 0x0002A819 File Offset: 0x00028A19
		// (set) Token: 0x060006D7 RID: 1751 RVA: 0x0002A821 File Offset: 0x00028A21
		public DamageBlockJson Block { get; set; }

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060006D8 RID: 1752 RVA: 0x0002A82A File Offset: 0x00028A2A
		// (set) Token: 0x060006D9 RID: 1753 RVA: 0x0002A832 File Offset: 0x00028A32
		public DamageBlockJson Parry { get; set; }

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x0002A83B File Offset: 0x00028A3B
		// (set) Token: 0x060006DB RID: 1755 RVA: 0x0002A843 File Offset: 0x00028A43
		public MeleeAttackStats HandleAttack { get; set; }

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060006DC RID: 1756 RVA: 0x0002A84C File Offset: 0x00028A4C
		// (set) Token: 0x060006DD RID: 1757 RVA: 0x0002A854 File Offset: 0x00028A54
		public string AttackHitSound { get; set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060006DE RID: 1758 RVA: 0x0002A85D File Offset: 0x00028A5D
		// (set) Token: 0x060006DF RID: 1759 RVA: 0x0002A865 File Offset: 0x00028A65
		public string BashHitSound { get; set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060006E0 RID: 1760 RVA: 0x0002A86E File Offset: 0x00028A6E
		// (set) Token: 0x060006E1 RID: 1761 RVA: 0x0002A876 File Offset: 0x00028A76
		public string HandleHitSound { get; set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060006E2 RID: 1762 RVA: 0x0002A87F File Offset: 0x00028A7F
		// (set) Token: 0x060006E3 RID: 1763 RVA: 0x0002A887 File Offset: 0x00028A87
		public float AttackCooldownMs { get; set; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060006E4 RID: 1764 RVA: 0x0002A890 File Offset: 0x00028A90
		// (set) Token: 0x060006E5 RID: 1765 RVA: 0x0002A898 File Offset: 0x00028A98
		public float BlockCooldownMs { get; set; }

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060006E6 RID: 1766 RVA: 0x0002A8A1 File Offset: 0x00028AA1
		// (set) Token: 0x060006E7 RID: 1767 RVA: 0x0002A8A9 File Offset: 0x00028AA9
		public float ParryCooldownMs { get; set; }

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060006E8 RID: 1768 RVA: 0x0002A8B2 File Offset: 0x00028AB2
		// (set) Token: 0x060006E9 RID: 1769 RVA: 0x0002A8BA File Offset: 0x00028ABA
		public bool ParryWithoutDelay { get; set; } = true;

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x0002A8C3 File Offset: 0x00028AC3
		// (set) Token: 0x060006EB RID: 1771 RVA: 0x0002A8CB File Offset: 0x00028ACB
		[Nullable(1)]
		public string AttackDirectionsType { [NullableContext(1)] get; [NullableContext(1)] set; } = "None";

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x0002A8D4 File Offset: 0x00028AD4
		// (set) Token: 0x060006ED RID: 1773 RVA: 0x0002A8DC File Offset: 0x00028ADC
		[Nullable(1)]
		public Dictionary<string, string[]> AttackAnimation { [NullableContext(1)] get; [NullableContext(1)] set; } = new Dictionary<string, string[]>();

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x0002A8E5 File Offset: 0x00028AE5
		// (set) Token: 0x060006EF RID: 1775 RVA: 0x0002A8ED File Offset: 0x00028AED
		[Nullable(1)]
		public Dictionary<string, string[]> BlockBashAnimation { [NullableContext(1)] get; [NullableContext(1)] set; } = new Dictionary<string, string[]>();

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x0002A8F6 File Offset: 0x00028AF6
		// (set) Token: 0x060006F1 RID: 1777 RVA: 0x0002A8FE File Offset: 0x00028AFE
		public string ParryAnimation { get; set; }

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x0002A907 File Offset: 0x00028B07
		// (set) Token: 0x060006F3 RID: 1779 RVA: 0x0002A90F File Offset: 0x00028B0F
		public string BlockAnimation { get; set; }

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x0002A918 File Offset: 0x00028B18
		// (set) Token: 0x060006F5 RID: 1781 RVA: 0x0002A920 File Offset: 0x00028B20
		public string RiposteAnimation { get; set; }

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x0002A929 File Offset: 0x00028B29
		// (set) Token: 0x060006F7 RID: 1783 RVA: 0x0002A931 File Offset: 0x00028B31
		public string ReadyAnimation { get; set; }

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060006F8 RID: 1784 RVA: 0x0002A93A File Offset: 0x00028B3A
		// (set) Token: 0x060006F9 RID: 1785 RVA: 0x0002A942 File Offset: 0x00028B42
		public string IdleAnimation { get; set; }

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x0002A94B File Offset: 0x00028B4B
		// (set) Token: 0x060006FB RID: 1787 RVA: 0x0002A953 File Offset: 0x00028B53
		public string WalkAnimation { get; set; }

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x0002A95C File Offset: 0x00028B5C
		// (set) Token: 0x060006FD RID: 1789 RVA: 0x0002A964 File Offset: 0x00028B64
		public string RunAnimation { get; set; }

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x0002A96D File Offset: 0x00028B6D
		// (set) Token: 0x060006FF RID: 1791 RVA: 0x0002A975 File Offset: 0x00028B75
		public string SwimAnimation { get; set; }

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x0002A97E File Offset: 0x00028B7E
		// (set) Token: 0x06000701 RID: 1793 RVA: 0x0002A986 File Offset: 0x00028B86
		public string SwimIdleAnimation { get; set; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x0002A98F File Offset: 0x00028B8F
		// (set) Token: 0x06000703 RID: 1795 RVA: 0x0002A997 File Offset: 0x00028B97
		public float AttackSpeedMultiplier { get; set; } = 1f;
	}
}
