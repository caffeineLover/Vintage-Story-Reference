using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace CombatOverhaul.Compatibility
{
	// Token: 0x0200015F RID: 351
	[NullableContext(1)]
	[Nullable(0)]
	internal sealed class ArcheringBowAimingSettings
	{
		// Token: 0x1700046A RID: 1130
		// (get) Token: 0x06001175 RID: 4469 RVA: 0x00054050 File Offset: 0x00052250
		// (set) Token: 0x06001176 RID: 4470 RVA: 0x00054058 File Offset: 0x00052258
		public string MatchedKey { get; set; } = "";

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06001177 RID: 4471 RVA: 0x00054061 File Offset: 0x00052261
		// (set) Token: 0x06001178 RID: 4472 RVA: 0x00054069 File Offset: 0x00052269
		public float Damage { get; set; }

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06001179 RID: 4473 RVA: 0x00054072 File Offset: 0x00052272
		// (set) Token: 0x0600117A RID: 4474 RVA: 0x0005407A File Offset: 0x0005227A
		public float DefaultDamage { get; set; }

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x0600117B RID: 4475 RVA: 0x00054083 File Offset: 0x00052283
		// (set) Token: 0x0600117C RID: 4476 RVA: 0x0005408B File Offset: 0x0005228B
		public float DamageMult { get; set; } = 1f;

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x0600117D RID: 4477 RVA: 0x00054094 File Offset: 0x00052294
		// (set) Token: 0x0600117E RID: 4478 RVA: 0x0005409C File Offset: 0x0005229C
		public float DrawTime { get; set; } = 1.2f;

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x0600117F RID: 4479 RVA: 0x000540A5 File Offset: 0x000522A5
		// (set) Token: 0x06001180 RID: 4480 RVA: 0x000540AD File Offset: 0x000522AD
		public float DrawWeight { get; set; } = 1.5f;

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06001181 RID: 4481 RVA: 0x000540B6 File Offset: 0x000522B6
		// (set) Token: 0x06001182 RID: 4482 RVA: 0x000540BE File Offset: 0x000522BE
		public float SwayAmplitude { get; set; } = 1f;

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06001183 RID: 4483 RVA: 0x000540C7 File Offset: 0x000522C7
		// (set) Token: 0x06001184 RID: 4484 RVA: 0x000540CF File Offset: 0x000522CF
		public float SwaySpeed { get; set; } = 1f;

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06001185 RID: 4485 RVA: 0x000540D8 File Offset: 0x000522D8
		// (set) Token: 0x06001186 RID: 4486 RVA: 0x000540E0 File Offset: 0x000522E0
		public float ChargeAmplitude { get; set; } = 1f;

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06001187 RID: 4487 RVA: 0x000540E9 File Offset: 0x000522E9
		// (set) Token: 0x06001188 RID: 4488 RVA: 0x000540F1 File Offset: 0x000522F1
		public float RunPenaltyMultiplier { get; set; } = 9.5f;

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x06001189 RID: 4489 RVA: 0x000540FA File Offset: 0x000522FA
		// (set) Token: 0x0600118A RID: 4490 RVA: 0x00054102 File Offset: 0x00052302
		public float HoldBreathMultiplier { get; set; } = 0.5f;

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x0600118B RID: 4491 RVA: 0x0005410B File Offset: 0x0005230B
		// (set) Token: 0x0600118C RID: 4492 RVA: 0x00054113 File Offset: 0x00052313
		public float BreathRecoveryMultiplier { get; set; } = 1.67f;

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x0600118D RID: 4493 RVA: 0x0005411C File Offset: 0x0005231C
		// (set) Token: 0x0600118E RID: 4494 RVA: 0x00054124 File Offset: 0x00052324
		public float BreathDuration { get; set; } = 2f;

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x0600118F RID: 4495 RVA: 0x0005412D File Offset: 0x0005232D
		// (set) Token: 0x06001190 RID: 4496 RVA: 0x00054135 File Offset: 0x00052335
		public float BreathRecoveryDuration { get; set; } = 5f;

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06001191 RID: 4497 RVA: 0x0005413E File Offset: 0x0005233E
		// (set) Token: 0x06001192 RID: 4498 RVA: 0x00054146 File Offset: 0x00052346
		public float DrawWeightDropPower { get; set; } = 0.65f;

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06001193 RID: 4499 RVA: 0x0005414F File Offset: 0x0005234F
		// (set) Token: 0x06001194 RID: 4500 RVA: 0x00054157 File Offset: 0x00052357
		public float DrawWeightChargePower { get; set; } = 0.85f;

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x06001195 RID: 4501 RVA: 0x00054160 File Offset: 0x00052360
		// (set) Token: 0x06001196 RID: 4502 RVA: 0x00054168 File Offset: 0x00052368
		public bool EnableSwaying { get; set; } = true;

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x06001197 RID: 4503 RVA: 0x00054171 File Offset: 0x00052371
		// (set) Token: 0x06001198 RID: 4504 RVA: 0x00054179 File Offset: 0x00052379
		public bool EnableBowDamageTweaks { get; set; } = true;

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06001199 RID: 4505 RVA: 0x00054182 File Offset: 0x00052382
		// (set) Token: 0x0600119A RID: 4506 RVA: 0x0005418A File Offset: 0x0005238A
		public bool EnableDrawTimeTweaks { get; set; } = true;

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x0600119B RID: 4507 RVA: 0x00054193 File Offset: 0x00052393
		// (set) Token: 0x0600119C RID: 4508 RVA: 0x0005419B File Offset: 0x0005239B
		public bool EnableDrawTimeArrowDamageTweaks { get; set; } = true;

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x0600119D RID: 4509 RVA: 0x000541A4 File Offset: 0x000523A4
		// (set) Token: 0x0600119E RID: 4510 RVA: 0x000541AC File Offset: 0x000523AC
		public bool EnableHoldBreath { get; set; } = true;

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x0600119F RID: 4511 RVA: 0x000541B5 File Offset: 0x000523B5
		// (set) Token: 0x060011A0 RID: 4512 RVA: 0x000541BD File Offset: 0x000523BD
		public bool EnableDenockingWithLeftClick { get; set; } = true;

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x060011A1 RID: 4513 RVA: 0x000541C6 File Offset: 0x000523C6
		// (set) Token: 0x060011A2 RID: 4514 RVA: 0x000541CE File Offset: 0x000523CE
		public bool EnableArcheringChargedReticle { get; set; } = true;

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x060011A3 RID: 4515 RVA: 0x000541D7 File Offset: 0x000523D7
		// (set) Token: 0x060011A4 RID: 4516 RVA: 0x000541DF File Offset: 0x000523DF
		public bool EnableSpeedIncreasesWithDrawWeight { get; set; } = true;

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x060011A5 RID: 4517 RVA: 0x000541E8 File Offset: 0x000523E8
		// (set) Token: 0x060011A6 RID: 4518 RVA: 0x000541F0 File Offset: 0x000523F0
		public bool EnableArrowBreakTweaks { get; set; } = true;

		// Token: 0x17000483 RID: 1155
		// (get) Token: 0x060011A7 RID: 4519 RVA: 0x000541F9 File Offset: 0x000523F9
		// (set) Token: 0x060011A8 RID: 4520 RVA: 0x00054201 File Offset: 0x00052401
		public bool DisableVanillaReticle { get; set; } = true;

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x060011A9 RID: 4521 RVA: 0x0005420A File Offset: 0x0005240A
		// (set) Token: 0x060011AA RID: 4522 RVA: 0x00054212 File Offset: 0x00052412
		public IReadOnlyDictionary<string, float> ArrowBreakChances { get; set; } = new Dictionary<string, float>();

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x060011AB RID: 4523 RVA: 0x0005421B File Offset: 0x0005241B
		public float DrawWeightSpeedMultiplier
		{
			get
			{
				return Math.Max(0.01f, MathF.Pow(this.DrawWeight, this.DrawWeightDropPower));
			}
		}
	}
}
