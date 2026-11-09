using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Implementations
{
	// Token: 0x02000093 RID: 147
	[NullableContext(2)]
	[Nullable(0)]
	public class StanceBasedMeleeWeaponStats
	{
		// Token: 0x1700019D RID: 413
		// (get) Token: 0x060007CC RID: 1996 RVA: 0x0002DC2F File Offset: 0x0002BE2F
		// (set) Token: 0x060007CD RID: 1997 RVA: 0x0002DC37 File Offset: 0x0002BE37
		[Nullable(1)]
		public string ProficiencyStat { [NullableContext(1)] get; [NullableContext(1)] set; } = "";

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060007CE RID: 1998 RVA: 0x0002DC40 File Offset: 0x0002BE40
		// (set) Token: 0x060007CF RID: 1999 RVA: 0x0002DC48 File Offset: 0x0002BE48
		public StanceBasedMeleeWeaponGripStats OneHanded { get; set; }

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060007D0 RID: 2000 RVA: 0x0002DC51 File Offset: 0x0002BE51
		// (set) Token: 0x060007D1 RID: 2001 RVA: 0x0002DC59 File Offset: 0x0002BE59
		public StanceBasedMeleeWeaponGripStats TwoHanded { get; set; }

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060007D2 RID: 2002 RVA: 0x0002DC62 File Offset: 0x0002BE62
		// (set) Token: 0x060007D3 RID: 2003 RVA: 0x0002DC6A File Offset: 0x0002BE6A
		public StanceBasedMeleeWeaponGripStats OffHand { get; set; }

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060007D4 RID: 2004 RVA: 0x0002DC73 File Offset: 0x0002BE73
		// (set) Token: 0x060007D5 RID: 2005 RVA: 0x0002DC7B File Offset: 0x0002BE7B
		public bool RenderingOffset { get; set; }

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060007D6 RID: 2006 RVA: 0x0002DC84 File Offset: 0x0002BE84
		// (set) Token: 0x060007D7 RID: 2007 RVA: 0x0002DC8C File Offset: 0x0002BE8C
		public float AnimationStaggerOnHitDurationMs { get; set; } = 100f;
	}
}
