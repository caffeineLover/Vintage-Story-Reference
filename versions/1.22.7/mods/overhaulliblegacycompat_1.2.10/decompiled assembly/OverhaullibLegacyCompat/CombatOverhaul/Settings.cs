using System;
using System.Runtime.CompilerServices;

namespace CombatOverhaul
{
	// Token: 0x02000019 RID: 25
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class Settings
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600015C RID: 348 RVA: 0x000076EB File Offset: 0x000058EB
		// (set) Token: 0x0600015D RID: 349 RVA: 0x000076F3 File Offset: 0x000058F3
		public float DirectionsCursorTransparency { get; set; } = 0.5f;

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600015E RID: 350 RVA: 0x000076FC File Offset: 0x000058FC
		// (set) Token: 0x0600015F RID: 351 RVA: 0x00007704 File Offset: 0x00005904
		public float DirectionsCursorScale { get; set; } = 0.5f;

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000160 RID: 352 RVA: 0x0000770D File Offset: 0x0000590D
		// (set) Token: 0x06000161 RID: 353 RVA: 0x00007715 File Offset: 0x00005915
		public string BowsAimingCursorType { get; set; } = "Fixed";

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000162 RID: 354 RVA: 0x0000771E File Offset: 0x0000591E
		// (set) Token: 0x06000163 RID: 355 RVA: 0x00007726 File Offset: 0x00005926
		public float BowsAimingHorizontalLimit { get; set; } = 0.125f;

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000164 RID: 356 RVA: 0x0000772F File Offset: 0x0000592F
		// (set) Token: 0x06000165 RID: 357 RVA: 0x00007737 File Offset: 0x00005937
		public float BowsAimingVerticalLimit { get; set; } = 0.35f;

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00007740 File Offset: 0x00005940
		// (set) Token: 0x06000167 RID: 359 RVA: 0x00007748 File Offset: 0x00005948
		public bool BowTwoHanded { get; set; } = true;

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00007751 File Offset: 0x00005951
		// (set) Token: 0x06000169 RID: 361 RVA: 0x00007759 File Offset: 0x00005959
		public float BowAimingHorisontalLimit
		{
			get
			{
				return this.BowsAimingHorizontalLimit;
			}
			set
			{
				this.BowsAimingHorizontalLimit = value;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00007762 File Offset: 0x00005962
		// (set) Token: 0x0600016B RID: 363 RVA: 0x0000776A File Offset: 0x0000596A
		public float BowAimingVerticalLimit
		{
			get
			{
				return this.BowsAimingVerticalLimit;
			}
			set
			{
				this.BowsAimingVerticalLimit = value;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00007773 File Offset: 0x00005973
		// (set) Token: 0x0600016D RID: 365 RVA: 0x0000777B File Offset: 0x0000597B
		public string ThrownWeaponsCursorType { get; set; } = "Fixed";

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00007784 File Offset: 0x00005984
		// (set) Token: 0x0600016F RID: 367 RVA: 0x0000778C File Offset: 0x0000598C
		public float ThrownWeaponsAimingHorizontalLimit { get; set; } = 0.125f;

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00007795 File Offset: 0x00005995
		// (set) Token: 0x06000171 RID: 369 RVA: 0x0000779D File Offset: 0x0000599D
		public float ThrownWeaponsAimingVerticalLimit { get; set; } = 0.25f;

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000172 RID: 370 RVA: 0x000077A6 File Offset: 0x000059A6
		// (set) Token: 0x06000173 RID: 371 RVA: 0x000077AE File Offset: 0x000059AE
		public string SlingsAimingCursorType { get; set; } = "Fixed";

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000174 RID: 372 RVA: 0x000077B7 File Offset: 0x000059B7
		// (set) Token: 0x06000175 RID: 373 RVA: 0x000077BF File Offset: 0x000059BF
		public float SlingsAimingHorizontalLimit { get; set; } = 0.125f;

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000176 RID: 374 RVA: 0x000077C8 File Offset: 0x000059C8
		// (set) Token: 0x06000177 RID: 375 RVA: 0x000077D0 File Offset: 0x000059D0
		public float SlingsAimingVerticalLimit { get; set; } = 0.35f;

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000178 RID: 376 RVA: 0x000077D9 File Offset: 0x000059D9
		// (set) Token: 0x06000179 RID: 377 RVA: 0x000077E1 File Offset: 0x000059E1
		public bool PrintRangeHits { get; set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600017A RID: 378 RVA: 0x000077EA File Offset: 0x000059EA
		// (set) Token: 0x0600017B RID: 379 RVA: 0x000077F2 File Offset: 0x000059F2
		public bool PrintMeleeHits { get; set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600017C RID: 380 RVA: 0x000077FB File Offset: 0x000059FB
		// (set) Token: 0x0600017D RID: 381 RVA: 0x00007803 File Offset: 0x00005A03
		public bool PrintPlayerHits { get; set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600017E RID: 382 RVA: 0x0000780C File Offset: 0x00005A0C
		// (set) Token: 0x0600017F RID: 383 RVA: 0x00007814 File Offset: 0x00005A14
		public float DirectionsSensitivity { get; set; } = 1f;

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000180 RID: 384 RVA: 0x0000781D File Offset: 0x00005A1D
		// (set) Token: 0x06000181 RID: 385 RVA: 0x00007825 File Offset: 0x00005A25
		public bool DirectionsInvert { get; set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000182 RID: 386 RVA: 0x0000782E File Offset: 0x00005A2E
		// (set) Token: 0x06000183 RID: 387 RVA: 0x00007836 File Offset: 0x00005A36
		public bool FlipDirectionAfterAttack { get; set; } = true;

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000184 RID: 388 RVA: 0x0000783F File Offset: 0x00005A3F
		// (set) Token: 0x06000185 RID: 389 RVA: 0x00007847 File Offset: 0x00005A47
		public bool HandsYawSmoothing { get; set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00007850 File Offset: 0x00005A50
		// (set) Token: 0x06000187 RID: 391 RVA: 0x00007858 File Offset: 0x00005A58
		public bool VanillaActionsWhileBlocking { get; set; } = true;

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00007861 File Offset: 0x00005A61
		// (set) Token: 0x06000189 RID: 393 RVA: 0x00007869 File Offset: 0x00005A69
		public bool VanillaArmorGridRecipes { get; set; } = true;

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00007872 File Offset: 0x00005A72
		// (set) Token: 0x0600018B RID: 395 RVA: 0x0000787A File Offset: 0x00005A7A
		public bool ToolsmithIntegrationEnabled { get; set; } = true;

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600018C RID: 396 RVA: 0x00007883 File Offset: 0x00005A83
		// (set) Token: 0x0600018D RID: 397 RVA: 0x0000788B File Offset: 0x00005A8B
		public float CollisionRadius { get; set; } = 16f;

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600018E RID: 398 RVA: 0x00007894 File Offset: 0x00005A94
		// (set) Token: 0x0600018F RID: 399 RVA: 0x0000789C File Offset: 0x00005A9C
		public float DefaultColliderPenetrationResistance { get; set; } = 5f;

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000190 RID: 400 RVA: 0x000078A5 File Offset: 0x00005AA5
		// (set) Token: 0x06000191 RID: 401 RVA: 0x000078AD File Offset: 0x00005AAD
		public bool DirectionsMovementControls { get; set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000192 RID: 402 RVA: 0x000078B6 File Offset: 0x00005AB6
		// (set) Token: 0x06000193 RID: 403 RVA: 0x000078BE File Offset: 0x00005ABE
		public bool DirectionsHotkeysControls { get; set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000194 RID: 404 RVA: 0x000078C7 File Offset: 0x00005AC7
		// (set) Token: 0x06000195 RID: 405 RVA: 0x000078CF File Offset: 0x00005ACF
		public bool DisableAllAnimations { get; set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000196 RID: 406 RVA: 0x000078D8 File Offset: 0x00005AD8
		// (set) Token: 0x06000197 RID: 407 RVA: 0x000078E0 File Offset: 0x00005AE0
		public bool DisableThirdPersonAnimations { get; set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000198 RID: 408 RVA: 0x000078E9 File Offset: 0x00005AE9
		// (set) Token: 0x06000199 RID: 409 RVA: 0x000078F1 File Offset: 0x00005AF1
		public bool MeleeWeaponStopOnTerrainHit { get; set; } = true;

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600019A RID: 410 RVA: 0x000078FA File Offset: 0x00005AFA
		// (set) Token: 0x0600019B RID: 411 RVA: 0x00007902 File Offset: 0x00005B02
		public bool MeleeWeaponIgnoreTerrainBehind { get; set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600019C RID: 412 RVA: 0x0000790B File Offset: 0x00005B0B
		// (set) Token: 0x0600019D RID: 413 RVA: 0x00007913 File Offset: 0x00005B13
		public float MeleeWeaponAttackSpeedMultiplier { get; set; } = 1f;

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600019E RID: 414 RVA: 0x0000791C File Offset: 0x00005B1C
		// (set) Token: 0x0600019F RID: 415 RVA: 0x00007924 File Offset: 0x00005B24
		public int GlobalAttackCooldownMs { get; set; } = 1000;

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x0000792D File Offset: 0x00005B2D
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x00007935 File Offset: 0x00005B35
		public bool SecondChanceAvailable { get; set; } = true;

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x0000793E File Offset: 0x00005B3E
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x00007946 File Offset: 0x00005B46
		public bool SecondChanceParticles { get; set; } = true;

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x0000794F File Offset: 0x00005B4F
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x00007957 File Offset: 0x00005B57
		public bool DebugHitParticles { get; set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00007960 File Offset: 0x00005B60
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x00007968 File Offset: 0x00005B68
		public bool DebugWeaponTrailParticles { get; set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00007971 File Offset: 0x00005B71
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x00007979 File Offset: 0x00005B79
		public float DebugWeaponTrailParticlesSize { get; set; } = 0.5f;

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00007982 File Offset: 0x00005B82
		// (set) Token: 0x060001AB RID: 427 RVA: 0x0000798A File Offset: 0x00005B8A
		public bool DebugProjectilesTrailsParticles { get; set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00007993 File Offset: 0x00005B93
		// (set) Token: 0x060001AD RID: 429 RVA: 0x0000799B File Offset: 0x00005B9B
		public bool DebugBlockAnglesParticles { get; set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001AE RID: 430 RVA: 0x000079A4 File Offset: 0x00005BA4
		// (set) Token: 0x060001AF RID: 431 RVA: 0x000079AC File Offset: 0x00005BAC
		public float EntityProtectionMultiplier { get; set; } = 0.5f;

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x000079B5 File Offset: 0x00005BB5
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x000079BD File Offset: 0x00005BBD
		public float WeaponQuenchDamageMultiplier { get; set; } = 1f;

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x000079C6 File Offset: 0x00005BC6
		// (set) Token: 0x060001B3 RID: 435 RVA: 0x000079CE File Offset: 0x00005BCE
		public bool RangedWeaponsDamageSupport { get; set; } = true;

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x000079D7 File Offset: 0x00005BD7
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x000079DF File Offset: 0x00005BDF
		public bool SwitchFromImmersiveFirstPerson { get; set; } = true;

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x000079E8 File Offset: 0x00005BE8
		// (set) Token: 0x060001B7 RID: 439 RVA: 0x000079F0 File Offset: 0x00005BF0
		public float FueledItemUpdateInGameHours { get; set; } = 0.1f;

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x000079F9 File Offset: 0x00005BF9
		// (set) Token: 0x060001B9 RID: 441 RVA: 0x00007A01 File Offset: 0x00005C01
		public bool ShortEntityInfo { get; set; } = true;

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001BA RID: 442 RVA: 0x00007A0A File Offset: 0x00005C0A
		// (set) Token: 0x060001BB RID: 443 RVA: 0x00007A12 File Offset: 0x00005C12
		public bool SlowDiagnosticsEnabled { get; set; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001BC RID: 444 RVA: 0x00007A1B File Offset: 0x00005C1B
		// (set) Token: 0x060001BD RID: 445 RVA: 0x00007A23 File Offset: 0x00005C23
		public int SlowDiagnosticsNetworkThresholdMs { get; set; } = 50;

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001BE RID: 446 RVA: 0x00007A2C File Offset: 0x00005C2C
		// (set) Token: 0x060001BF RID: 447 RVA: 0x00007A34 File Offset: 0x00005C34
		public int SlowDiagnosticsRangedStatusThresholdMs { get; set; } = 10;

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x00007A3D File Offset: 0x00005C3D
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x00007A45 File Offset: 0x00005C45
		public int SlowDiagnosticsLogCooldownMs { get; set; } = 5000;
	}
}
