using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Implementations
{
	// Token: 0x02000085 RID: 133
	[NullableContext(2)]
	[Nullable(0)]
	public class MeleeWeaponStats : WeaponStats
	{
		// Token: 0x17000160 RID: 352
		// (get) Token: 0x0600071A RID: 1818 RVA: 0x0002AB37 File Offset: 0x00028D37
		// (set) Token: 0x0600071B RID: 1819 RVA: 0x0002AB3F File Offset: 0x00028D3F
		public StanceStats OneHandedStance { get; set; }

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x0600071C RID: 1820 RVA: 0x0002AB48 File Offset: 0x00028D48
		// (set) Token: 0x0600071D RID: 1821 RVA: 0x0002AB50 File Offset: 0x00028D50
		public StanceStats TwoHandedStance { get; set; }

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x0002AB59 File Offset: 0x00028D59
		// (set) Token: 0x0600071F RID: 1823 RVA: 0x0002AB61 File Offset: 0x00028D61
		public StanceStats OffHandStance { get; set; }

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000720 RID: 1824 RVA: 0x0002AB6A File Offset: 0x00028D6A
		// (set) Token: 0x06000721 RID: 1825 RVA: 0x0002AB72 File Offset: 0x00028D72
		[Nullable(1)]
		public Dictionary<string, StanceStats> MainHandDualWieldStances { [NullableContext(1)] get; [NullableContext(1)] set; } = new Dictionary<string, StanceStats>();

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x06000722 RID: 1826 RVA: 0x0002AB7B File Offset: 0x00028D7B
		// (set) Token: 0x06000723 RID: 1827 RVA: 0x0002AB83 File Offset: 0x00028D83
		[Nullable(1)]
		public Dictionary<string, StanceStats> OffHandDualWieldStances { [NullableContext(1)] get; [NullableContext(1)] set; } = new Dictionary<string, StanceStats>();

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x06000724 RID: 1828 RVA: 0x0002AB8C File Offset: 0x00028D8C
		// (set) Token: 0x06000725 RID: 1829 RVA: 0x0002AB94 File Offset: 0x00028D94
		public ThrowWeaponStats ThrowAttack { get; set; }

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000726 RID: 1830 RVA: 0x0002AB9D File Offset: 0x00028D9D
		// (set) Token: 0x06000727 RID: 1831 RVA: 0x0002ABA5 File Offset: 0x00028DA5
		public float ScreenShakeStrength { get; set; } = 0.25f;

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000728 RID: 1832 RVA: 0x0002ABAE File Offset: 0x00028DAE
		// (set) Token: 0x06000729 RID: 1833 RVA: 0x0002ABB6 File Offset: 0x00028DB6
		public float ThrowScreenShakeStrength { get; set; } = 0.15f;

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x0600072A RID: 1834 RVA: 0x0002ABBF File Offset: 0x00028DBF
		// (set) Token: 0x0600072B RID: 1835 RVA: 0x0002ABC7 File Offset: 0x00028DC7
		public bool RenderingOffset { get; set; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x0002ABD0 File Offset: 0x00028DD0
		// (set) Token: 0x0600072D RID: 1837 RVA: 0x0002ABD8 File Offset: 0x00028DD8
		public float AnimationStaggerOnHitDurationMs { get; set; } = 100f;
	}
}
