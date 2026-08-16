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
		// (get) Token: 0x0600015C RID: 348 RVA: 0x000076AB File Offset: 0x000058AB
		// (set) Token: 0x0600015D RID: 349 RVA: 0x000076B3 File Offset: 0x000058B3
		public float DirectionsCursorTransparency { get; set; } = 0.5f;

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x0600015E RID: 350 RVA: 0x000076BC File Offset: 0x000058BC
		// (set) Token: 0x0600015F RID: 351 RVA: 0x000076C4 File Offset: 0x000058C4
		public float DirectionsCursorScale { get; set; } = 0.5f;

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000160 RID: 352 RVA: 0x000076CD File Offset: 0x000058CD
		// (set) Token: 0x06000161 RID: 353 RVA: 0x000076D5 File Offset: 0x000058D5
		public string BowsAimingCursorType { get; set; } = "Fixed";

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000162 RID: 354 RVA: 0x000076DE File Offset: 0x000058DE
		// (set) Token: 0x06000163 RID: 355 RVA: 0x000076E6 File Offset: 0x000058E6
		public float BowsAimingHorizontalLimit { get; set; } = 0.125f;

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000164 RID: 356 RVA: 0x000076EF File Offset: 0x000058EF
		// (set) Token: 0x06000165 RID: 357 RVA: 0x000076F7 File Offset: 0x000058F7
		public float BowsAimingVerticalLimit { get; set; } = 0.35f;

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000166 RID: 358 RVA: 0x00007700 File Offset: 0x00005900
		// (set) Token: 0x06000167 RID: 359 RVA: 0x00007708 File Offset: 0x00005908
		public bool BowTwoHanded { get; set; } = true;

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000168 RID: 360 RVA: 0x00007711 File Offset: 0x00005911
		// (set) Token: 0x06000169 RID: 361 RVA: 0x00007719 File Offset: 0x00005919
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
		// (get) Token: 0x0600016A RID: 362 RVA: 0x00007722 File Offset: 0x00005922
		// (set) Token: 0x0600016B RID: 363 RVA: 0x0000772A File Offset: 0x0000592A
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
		// (get) Token: 0x0600016C RID: 364 RVA: 0x00007733 File Offset: 0x00005933
		// (set) Token: 0x0600016D RID: 365 RVA: 0x0000773B File Offset: 0x0000593B
		public string ThrownWeaponsCursorType { get; set; } = "Fixed";

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600016E RID: 366 RVA: 0x00007744 File Offset: 0x00005944
		// (set) Token: 0x0600016F RID: 367 RVA: 0x0000774C File Offset: 0x0000594C
		public float ThrownWeaponsAimingHorizontalLimit { get; set; } = 0.125f;

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x06000170 RID: 368 RVA: 0x00007755 File Offset: 0x00005955
		// (set) Token: 0x06000171 RID: 369 RVA: 0x0000775D File Offset: 0x0000595D
		public float ThrownWeaponsAimingVerticalLimit { get; set; } = 0.25f;

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06000172 RID: 370 RVA: 0x00007766 File Offset: 0x00005966
		// (set) Token: 0x06000173 RID: 371 RVA: 0x0000776E File Offset: 0x0000596E
		public string SlingsAimingCursorType { get; set; } = "Fixed";

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000174 RID: 372 RVA: 0x00007777 File Offset: 0x00005977
		// (set) Token: 0x06000175 RID: 373 RVA: 0x0000777F File Offset: 0x0000597F
		public float SlingsAimingHorizontalLimit { get; set; } = 0.125f;

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000176 RID: 374 RVA: 0x00007788 File Offset: 0x00005988
		// (set) Token: 0x06000177 RID: 375 RVA: 0x00007790 File Offset: 0x00005990
		public float SlingsAimingVerticalLimit { get; set; } = 0.35f;

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000178 RID: 376 RVA: 0x00007799 File Offset: 0x00005999
		// (set) Token: 0x06000179 RID: 377 RVA: 0x000077A1 File Offset: 0x000059A1
		public bool PrintRangeHits { get; set; }

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600017A RID: 378 RVA: 0x000077AA File Offset: 0x000059AA
		// (set) Token: 0x0600017B RID: 379 RVA: 0x000077B2 File Offset: 0x000059B2
		public bool PrintMeleeHits { get; set; }

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600017C RID: 380 RVA: 0x000077BB File Offset: 0x000059BB
		// (set) Token: 0x0600017D RID: 381 RVA: 0x000077C3 File Offset: 0x000059C3
		public bool PrintPlayerHits { get; set; }

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600017E RID: 382 RVA: 0x000077CC File Offset: 0x000059CC
		// (set) Token: 0x0600017F RID: 383 RVA: 0x000077D4 File Offset: 0x000059D4
		public float DirectionsSensitivity { get; set; } = 1f;

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000180 RID: 384 RVA: 0x000077DD File Offset: 0x000059DD
		// (set) Token: 0x06000181 RID: 385 RVA: 0x000077E5 File Offset: 0x000059E5
		public bool DirectionsInvert { get; set; }

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000182 RID: 386 RVA: 0x000077EE File Offset: 0x000059EE
		// (set) Token: 0x06000183 RID: 387 RVA: 0x000077F6 File Offset: 0x000059F6
		public bool FlipDirectionAfterAttack { get; set; } = true;

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000184 RID: 388 RVA: 0x000077FF File Offset: 0x000059FF
		// (set) Token: 0x06000185 RID: 389 RVA: 0x00007807 File Offset: 0x00005A07
		public bool HandsYawSmoothing { get; set; }

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00007810 File Offset: 0x00005A10
		// (set) Token: 0x06000187 RID: 391 RVA: 0x00007818 File Offset: 0x00005A18
		public bool VanillaActionsWhileBlocking { get; set; } = true;

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000188 RID: 392 RVA: 0x00007821 File Offset: 0x00005A21
		// (set) Token: 0x06000189 RID: 393 RVA: 0x00007829 File Offset: 0x00005A29
		public bool VanillaArmorGridRecipes { get; set; } = true;

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600018A RID: 394 RVA: 0x00007832 File Offset: 0x00005A32
		// (set) Token: 0x0600018B RID: 395 RVA: 0x0000783A File Offset: 0x00005A3A
		public bool ToolsmithIntegrationEnabled { get; set; } = true;

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600018C RID: 396 RVA: 0x00007843 File Offset: 0x00005A43
		// (set) Token: 0x0600018D RID: 397 RVA: 0x0000784B File Offset: 0x00005A4B
		public float CollisionRadius { get; set; } = 16f;

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600018E RID: 398 RVA: 0x00007854 File Offset: 0x00005A54
		// (set) Token: 0x0600018F RID: 399 RVA: 0x0000785C File Offset: 0x00005A5C
		public float DefaultColliderPenetrationResistance { get; set; } = 5f;

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00007865 File Offset: 0x00005A65
		// (set) Token: 0x06000191 RID: 401 RVA: 0x0000786D File Offset: 0x00005A6D
		public bool DirectionsMovementControls { get; set; }

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00007876 File Offset: 0x00005A76
		// (set) Token: 0x06000193 RID: 403 RVA: 0x0000787E File Offset: 0x00005A7E
		public bool DirectionsHotkeysControls { get; set; }

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00007887 File Offset: 0x00005A87
		// (set) Token: 0x06000195 RID: 405 RVA: 0x0000788F File Offset: 0x00005A8F
		public bool DisableAllAnimations { get; set; }

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00007898 File Offset: 0x00005A98
		// (set) Token: 0x06000197 RID: 407 RVA: 0x000078A0 File Offset: 0x00005AA0
		public bool DisableThirdPersonAnimations { get; set; }

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000198 RID: 408 RVA: 0x000078A9 File Offset: 0x00005AA9
		// (set) Token: 0x06000199 RID: 409 RVA: 0x000078B1 File Offset: 0x00005AB1
		public bool MeleeWeaponStopOnTerrainHit { get; set; } = true;

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x0600019A RID: 410 RVA: 0x000078BA File Offset: 0x00005ABA
		// (set) Token: 0x0600019B RID: 411 RVA: 0x000078C2 File Offset: 0x00005AC2
		public bool MeleeWeaponIgnoreTerrainBehind { get; set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x0600019C RID: 412 RVA: 0x000078CB File Offset: 0x00005ACB
		// (set) Token: 0x0600019D RID: 413 RVA: 0x000078D3 File Offset: 0x00005AD3
		public float MeleeWeaponAttackSpeedMultiplier { get; set; } = 1f;

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x0600019E RID: 414 RVA: 0x000078DC File Offset: 0x00005ADC
		// (set) Token: 0x0600019F RID: 415 RVA: 0x000078E4 File Offset: 0x00005AE4
		public int GlobalAttackCooldownMs { get; set; } = 1000;

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x000078ED File Offset: 0x00005AED
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x000078F5 File Offset: 0x00005AF5
		public bool SecondChanceAvailable { get; set; } = true;

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x000078FE File Offset: 0x00005AFE
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x00007906 File Offset: 0x00005B06
		public bool SecondChanceParticles { get; set; } = true;

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x0000790F File Offset: 0x00005B0F
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x00007917 File Offset: 0x00005B17
		public bool DebugHitParticles { get; set; }

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00007920 File Offset: 0x00005B20
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x00007928 File Offset: 0x00005B28
		public bool DebugWeaponTrailParticles { get; set; }

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00007931 File Offset: 0x00005B31
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x00007939 File Offset: 0x00005B39
		public float DebugWeaponTrailParticlesSize { get; set; } = 0.5f;

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001AA RID: 426 RVA: 0x00007942 File Offset: 0x00005B42
		// (set) Token: 0x060001AB RID: 427 RVA: 0x0000794A File Offset: 0x00005B4A
		public bool DebugProjectilesTrailsParticles { get; set; }

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001AC RID: 428 RVA: 0x00007953 File Offset: 0x00005B53
		// (set) Token: 0x060001AD RID: 429 RVA: 0x0000795B File Offset: 0x00005B5B
		public bool DebugBlockAnglesParticles { get; set; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00007964 File Offset: 0x00005B64
		// (set) Token: 0x060001AF RID: 431 RVA: 0x0000796C File Offset: 0x00005B6C
		public float EntityProtectionMultiplier { get; set; } = 0.5f;

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00007975 File Offset: 0x00005B75
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x0000797D File Offset: 0x00005B7D
		public float WeaponQuenchDamageMultiplier { get; set; } = 1f;

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x00007986 File Offset: 0x00005B86
		// (set) Token: 0x060001B3 RID: 435 RVA: 0x0000798E File Offset: 0x00005B8E
		public bool RangedWeaponsDamageSupport { get; set; } = true;

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001B4 RID: 436 RVA: 0x00007997 File Offset: 0x00005B97
		// (set) Token: 0x060001B5 RID: 437 RVA: 0x0000799F File Offset: 0x00005B9F
		public bool SwitchFromImmersiveFirstPerson { get; set; } = true;

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001B6 RID: 438 RVA: 0x000079A8 File Offset: 0x00005BA8
		// (set) Token: 0x060001B7 RID: 439 RVA: 0x000079B0 File Offset: 0x00005BB0
		public float FueledItemUpdateInGameHours { get; set; } = 0.1f;

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001B8 RID: 440 RVA: 0x000079B9 File Offset: 0x00005BB9
		// (set) Token: 0x060001B9 RID: 441 RVA: 0x000079C1 File Offset: 0x00005BC1
		public bool ShortEntityInfo { get; set; } = true;

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001BA RID: 442 RVA: 0x000079CA File Offset: 0x00005BCA
		// (set) Token: 0x060001BB RID: 443 RVA: 0x000079D2 File Offset: 0x00005BD2
		public bool SlowDiagnosticsEnabled { get; set; }

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001BC RID: 444 RVA: 0x000079DB File Offset: 0x00005BDB
		// (set) Token: 0x060001BD RID: 445 RVA: 0x000079E3 File Offset: 0x00005BE3
		public int SlowDiagnosticsNetworkThresholdMs { get; set; } = 50;

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060001BE RID: 446 RVA: 0x000079EC File Offset: 0x00005BEC
		// (set) Token: 0x060001BF RID: 447 RVA: 0x000079F4 File Offset: 0x00005BF4
		public int SlowDiagnosticsRangedStatusThresholdMs { get; set; } = 10;

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x000079FD File Offset: 0x00005BFD
		// (set) Token: 0x060001C1 RID: 449 RVA: 0x00007A05 File Offset: 0x00005C05
		public int SlowDiagnosticsLogCooldownMs { get; set; } = 5000;
	}
}
