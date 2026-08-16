using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Colliders
{
	// Token: 0x0200015B RID: 347
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class ColliderTypesJson
	{
		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06001165 RID: 4453 RVA: 0x00053974 File Offset: 0x00051B74
		// (set) Token: 0x06001166 RID: 4454 RVA: 0x0005397C File Offset: 0x00051B7C
		public string[] Torso { get; set; } = Array.Empty<string>();

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06001167 RID: 4455 RVA: 0x00053985 File Offset: 0x00051B85
		// (set) Token: 0x06001168 RID: 4456 RVA: 0x0005398D File Offset: 0x00051B8D
		public string[] Head { get; set; } = Array.Empty<string>();

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06001169 RID: 4457 RVA: 0x00053996 File Offset: 0x00051B96
		// (set) Token: 0x0600116A RID: 4458 RVA: 0x0005399E File Offset: 0x00051B9E
		public string[] Arm { get; set; } = Array.Empty<string>();

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x0600116B RID: 4459 RVA: 0x000539A7 File Offset: 0x00051BA7
		// (set) Token: 0x0600116C RID: 4460 RVA: 0x000539AF File Offset: 0x00051BAF
		public string[] Leg { get; set; } = Array.Empty<string>();

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x0600116D RID: 4461 RVA: 0x000539B8 File Offset: 0x00051BB8
		// (set) Token: 0x0600116E RID: 4462 RVA: 0x000539C0 File Offset: 0x00051BC0
		public string[] Critical { get; set; } = Array.Empty<string>();

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x0600116F RID: 4463 RVA: 0x000539C9 File Offset: 0x00051BC9
		// (set) Token: 0x06001170 RID: 4464 RVA: 0x000539D1 File Offset: 0x00051BD1
		public string[] Resistant { get; set; } = Array.Empty<string>();
	}
}
