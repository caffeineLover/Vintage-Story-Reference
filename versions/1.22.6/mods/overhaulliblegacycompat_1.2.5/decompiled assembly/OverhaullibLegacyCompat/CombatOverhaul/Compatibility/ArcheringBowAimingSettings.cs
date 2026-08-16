using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Compatibility
{
	// Token: 0x02000153 RID: 339
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class ArcheringBowAimingSettings
	{
		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x06001101 RID: 4353 RVA: 0x00052748 File Offset: 0x00050948
		// (set) Token: 0x06001102 RID: 4354 RVA: 0x00052750 File Offset: 0x00050950
		public string MatchedKey { get; set; } = "";

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x06001103 RID: 4355 RVA: 0x00052759 File Offset: 0x00050959
		// (set) Token: 0x06001104 RID: 4356 RVA: 0x00052761 File Offset: 0x00050961
		public float Damage { get; set; }

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06001105 RID: 4357 RVA: 0x0005276A File Offset: 0x0005096A
		// (set) Token: 0x06001106 RID: 4358 RVA: 0x00052772 File Offset: 0x00050972
		public float DefaultDamage { get; set; }

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x06001107 RID: 4359 RVA: 0x0005277B File Offset: 0x0005097B
		// (set) Token: 0x06001108 RID: 4360 RVA: 0x00052783 File Offset: 0x00050983
		public float DamageMult { get; set; } = 1f;

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06001109 RID: 4361 RVA: 0x0005278C File Offset: 0x0005098C
		// (set) Token: 0x0600110A RID: 4362 RVA: 0x00052794 File Offset: 0x00050994
		public float DrawTime { get; set; } = 1.2f;

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x0600110B RID: 4363 RVA: 0x0005279D File Offset: 0x0005099D
		// (set) Token: 0x0600110C RID: 4364 RVA: 0x000527A5 File Offset: 0x000509A5
		public float DrawWeight { get; set; } = 1.5f;

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x0600110D RID: 4365 RVA: 0x000527AE File Offset: 0x000509AE
		// (set) Token: 0x0600110E RID: 4366 RVA: 0x000527B6 File Offset: 0x000509B6
		public float SwayAmplitude { get; set; } = 1f;

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x0600110F RID: 4367 RVA: 0x000527BF File Offset: 0x000509BF
		// (set) Token: 0x06001110 RID: 4368 RVA: 0x000527C7 File Offset: 0x000509C7
		public float SwaySpeed { get; set; } = 1f;

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06001111 RID: 4369 RVA: 0x000527D0 File Offset: 0x000509D0
		// (set) Token: 0x06001112 RID: 4370 RVA: 0x000527D8 File Offset: 0x000509D8
		public float ChargeAmplitude { get; set; } = 1f;

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06001113 RID: 4371 RVA: 0x000527E1 File Offset: 0x000509E1
		// (set) Token: 0x06001114 RID: 4372 RVA: 0x000527E9 File Offset: 0x000509E9
		public float RunPenaltyMultiplier { get; set; } = 9.5f;

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06001115 RID: 4373 RVA: 0x000527F2 File Offset: 0x000509F2
		// (set) Token: 0x06001116 RID: 4374 RVA: 0x000527FA File Offset: 0x000509FA
		public float HoldBreathMultiplier { get; set; } = 0.5f;

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06001117 RID: 4375 RVA: 0x00052803 File Offset: 0x00050A03
		// (set) Token: 0x06001118 RID: 4376 RVA: 0x0005280B File Offset: 0x00050A0B
		public float BreathRecoveryMultiplier { get; set; } = 1.67f;

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06001119 RID: 4377 RVA: 0x00052814 File Offset: 0x00050A14
		// (set) Token: 0x0600111A RID: 4378 RVA: 0x0005281C File Offset: 0x00050A1C
		public float BreathDuration { get; set; } = 2f;

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x0600111B RID: 4379 RVA: 0x00052825 File Offset: 0x00050A25
		// (set) Token: 0x0600111C RID: 4380 RVA: 0x0005282D File Offset: 0x00050A2D
		public float BreathRecoveryDuration { get; set; } = 5f;

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x0600111D RID: 4381 RVA: 0x00052836 File Offset: 0x00050A36
		// (set) Token: 0x0600111E RID: 4382 RVA: 0x0005283E File Offset: 0x00050A3E
		public float DrawWeightDropPower { get; set; } = 0.65f;

		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x0600111F RID: 4383 RVA: 0x00052847 File Offset: 0x00050A47
		// (set) Token: 0x06001120 RID: 4384 RVA: 0x0005284F File Offset: 0x00050A4F
		public float DrawWeightChargePower { get; set; } = 0.85f;

		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x06001121 RID: 4385 RVA: 0x00052858 File Offset: 0x00050A58
		// (set) Token: 0x06001122 RID: 4386 RVA: 0x00052860 File Offset: 0x00050A60
		public bool EnableSwaying { get; set; } = true;

		// Token: 0x17000459 RID: 1113
		// (get) Token: 0x06001123 RID: 4387 RVA: 0x00052869 File Offset: 0x00050A69
		// (set) Token: 0x06001124 RID: 4388 RVA: 0x00052871 File Offset: 0x00050A71
		public bool EnableBowDamageTweaks { get; set; } = true;

		// Token: 0x1700045A RID: 1114
		// (get) Token: 0x06001125 RID: 4389 RVA: 0x0005287A File Offset: 0x00050A7A
		// (set) Token: 0x06001126 RID: 4390 RVA: 0x00052882 File Offset: 0x00050A82
		public bool EnableDrawTimeTweaks { get; set; } = true;

		// Token: 0x1700045B RID: 1115
		// (get) Token: 0x06001127 RID: 4391 RVA: 0x0005288B File Offset: 0x00050A8B
		// (set) Token: 0x06001128 RID: 4392 RVA: 0x00052893 File Offset: 0x00050A93
		public bool EnableDrawTimeArrowDamageTweaks { get; set; } = true;

		// Token: 0x1700045C RID: 1116
		// (get) Token: 0x06001129 RID: 4393 RVA: 0x0005289C File Offset: 0x00050A9C
		// (set) Token: 0x0600112A RID: 4394 RVA: 0x000528A4 File Offset: 0x00050AA4
		public bool EnableHoldBreath { get; set; } = true;

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x0600112B RID: 4395 RVA: 0x000528AD File Offset: 0x00050AAD
		// (set) Token: 0x0600112C RID: 4396 RVA: 0x000528B5 File Offset: 0x00050AB5
		public bool EnableDenockingWithLeftClick { get; set; } = true;

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x0600112D RID: 4397 RVA: 0x000528BE File Offset: 0x00050ABE
		// (set) Token: 0x0600112E RID: 4398 RVA: 0x000528C6 File Offset: 0x00050AC6
		public bool EnableArcheringChargedReticle { get; set; } = true;

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x0600112F RID: 4399 RVA: 0x000528CF File Offset: 0x00050ACF
		// (set) Token: 0x06001130 RID: 4400 RVA: 0x000528D7 File Offset: 0x00050AD7
		public bool EnableSpeedIncreasesWithDrawWeight { get; set; } = true;

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06001131 RID: 4401 RVA: 0x000528E0 File Offset: 0x00050AE0
		// (set) Token: 0x06001132 RID: 4402 RVA: 0x000528E8 File Offset: 0x00050AE8
		public bool EnableArrowBreakTweaks { get; set; } = true;

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x06001133 RID: 4403 RVA: 0x000528F1 File Offset: 0x00050AF1
		// (set) Token: 0x06001134 RID: 4404 RVA: 0x000528F9 File Offset: 0x00050AF9
		public bool DisableVanillaReticle { get; set; } = true;

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x06001135 RID: 4405 RVA: 0x00052902 File Offset: 0x00050B02
		// (set) Token: 0x06001136 RID: 4406 RVA: 0x0005290A File Offset: 0x00050B0A
		public IReadOnlyDictionary<string, float> ArrowBreakChances { get; set; } = new Dictionary<string, float>();

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x06001137 RID: 4407 RVA: 0x00052913 File Offset: 0x00050B13
		public float DrawWeightSpeedMultiplier
		{
			get
			{
				return Math.Max(0.01f, MathF.Pow(this.DrawWeight, this.DrawWeightDropPower));
			}
		}
	}
}
