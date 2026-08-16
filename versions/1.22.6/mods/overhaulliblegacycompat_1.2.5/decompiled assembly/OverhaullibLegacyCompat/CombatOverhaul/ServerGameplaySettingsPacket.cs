using System;
using System.Runtime.CompilerServices;
using ProtoBuf;

namespace CombatOverhaul
{
	// Token: 0x0200001A RID: 26
	[NullableContext(1)]
	[Nullable(0)]
	[ProtoContract(ImplicitFields = 1)]
	public sealed class ServerGameplaySettingsPacket
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00007B67 File Offset: 0x00005D67
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x00007B6F File Offset: 0x00005D6F
		public float BowsAimingHorizontalLimit { get; set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00007B78 File Offset: 0x00005D78
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x00007B80 File Offset: 0x00005D80
		public float BowsAimingVerticalLimit { get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00007B89 File Offset: 0x00005D89
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x00007B91 File Offset: 0x00005D91
		public bool BowTwoHanded { get; set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00007B9A File Offset: 0x00005D9A
		// (set) Token: 0x060001CA RID: 458 RVA: 0x00007BA2 File Offset: 0x00005DA2
		public float ThrownWeaponsAimingHorizontalLimit { get; set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001CB RID: 459 RVA: 0x00007BAB File Offset: 0x00005DAB
		// (set) Token: 0x060001CC RID: 460 RVA: 0x00007BB3 File Offset: 0x00005DB3
		public float ThrownWeaponsAimingVerticalLimit { get; set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00007BBC File Offset: 0x00005DBC
		// (set) Token: 0x060001CE RID: 462 RVA: 0x00007BC4 File Offset: 0x00005DC4
		public float SlingsAimingHorizontalLimit { get; set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001CF RID: 463 RVA: 0x00007BCD File Offset: 0x00005DCD
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x00007BD5 File Offset: 0x00005DD5
		public float SlingsAimingVerticalLimit { get; set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x00007BDE File Offset: 0x00005DDE
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x00007BE6 File Offset: 0x00005DE6
		public bool VanillaActionsWhileBlocking { get; set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x00007BEF File Offset: 0x00005DEF
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x00007BF7 File Offset: 0x00005DF7
		public float CollisionRadius { get; set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x00007C00 File Offset: 0x00005E00
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x00007C08 File Offset: 0x00005E08
		public float DefaultColliderPenetrationResistance { get; set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x00007C11 File Offset: 0x00005E11
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x00007C19 File Offset: 0x00005E19
		public bool MeleeWeaponStopOnTerrainHit { get; set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x00007C22 File Offset: 0x00005E22
		// (set) Token: 0x060001DA RID: 474 RVA: 0x00007C2A File Offset: 0x00005E2A
		public bool MeleeWeaponIgnoreTerrainBehind { get; set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001DB RID: 475 RVA: 0x00007C33 File Offset: 0x00005E33
		// (set) Token: 0x060001DC RID: 476 RVA: 0x00007C3B File Offset: 0x00005E3B
		public float MeleeWeaponAttackSpeedMultiplier { get; set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001DD RID: 477 RVA: 0x00007C44 File Offset: 0x00005E44
		// (set) Token: 0x060001DE RID: 478 RVA: 0x00007C4C File Offset: 0x00005E4C
		public int GlobalAttackCooldownMs { get; set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001DF RID: 479 RVA: 0x00007C55 File Offset: 0x00005E55
		// (set) Token: 0x060001E0 RID: 480 RVA: 0x00007C5D File Offset: 0x00005E5D
		public bool SecondChanceAvailable { get; set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x00007C66 File Offset: 0x00005E66
		// (set) Token: 0x060001E2 RID: 482 RVA: 0x00007C6E File Offset: 0x00005E6E
		public float EntityProtectionMultiplier { get; set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x00007C77 File Offset: 0x00005E77
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x00007C7F File Offset: 0x00005E7F
		public float WeaponQuenchDamageMultiplier { get; set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x00007C88 File Offset: 0x00005E88
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x00007C90 File Offset: 0x00005E90
		public bool RangedWeaponsDamageSupport { get; set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x00007C99 File Offset: 0x00005E99
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x00007CA1 File Offset: 0x00005EA1
		public float FueledItemUpdateInGameHours { get; set; }

		// Token: 0x060001E9 RID: 489 RVA: 0x00007CAC File Offset: 0x00005EAC
		public static ServerGameplaySettingsPacket From(Settings settings)
		{
			return new ServerGameplaySettingsPacket
			{
				BowsAimingHorizontalLimit = settings.BowsAimingHorizontalLimit,
				BowsAimingVerticalLimit = settings.BowsAimingVerticalLimit,
				BowTwoHanded = settings.BowTwoHanded,
				ThrownWeaponsAimingHorizontalLimit = settings.ThrownWeaponsAimingHorizontalLimit,
				ThrownWeaponsAimingVerticalLimit = settings.ThrownWeaponsAimingVerticalLimit,
				SlingsAimingHorizontalLimit = settings.SlingsAimingHorizontalLimit,
				SlingsAimingVerticalLimit = settings.SlingsAimingVerticalLimit,
				VanillaActionsWhileBlocking = settings.VanillaActionsWhileBlocking,
				CollisionRadius = settings.CollisionRadius,
				DefaultColliderPenetrationResistance = settings.DefaultColliderPenetrationResistance,
				MeleeWeaponStopOnTerrainHit = settings.MeleeWeaponStopOnTerrainHit,
				MeleeWeaponIgnoreTerrainBehind = settings.MeleeWeaponIgnoreTerrainBehind,
				MeleeWeaponAttackSpeedMultiplier = settings.MeleeWeaponAttackSpeedMultiplier,
				GlobalAttackCooldownMs = settings.GlobalAttackCooldownMs,
				SecondChanceAvailable = settings.SecondChanceAvailable,
				EntityProtectionMultiplier = settings.EntityProtectionMultiplier,
				WeaponQuenchDamageMultiplier = settings.WeaponQuenchDamageMultiplier,
				RangedWeaponsDamageSupport = settings.RangedWeaponsDamageSupport,
				FueledItemUpdateInGameHours = settings.FueledItemUpdateInGameHours
			};
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00007DA4 File Offset: 0x00005FA4
		public void ApplyTo(Settings settings)
		{
			settings.BowsAimingHorizontalLimit = this.BowsAimingHorizontalLimit;
			settings.BowsAimingVerticalLimit = this.BowsAimingVerticalLimit;
			settings.BowTwoHanded = this.BowTwoHanded;
			settings.ThrownWeaponsAimingHorizontalLimit = this.ThrownWeaponsAimingHorizontalLimit;
			settings.ThrownWeaponsAimingVerticalLimit = this.ThrownWeaponsAimingVerticalLimit;
			settings.SlingsAimingHorizontalLimit = this.SlingsAimingHorizontalLimit;
			settings.SlingsAimingVerticalLimit = this.SlingsAimingVerticalLimit;
			settings.VanillaActionsWhileBlocking = this.VanillaActionsWhileBlocking;
			settings.CollisionRadius = this.CollisionRadius;
			settings.DefaultColliderPenetrationResistance = this.DefaultColliderPenetrationResistance;
			settings.MeleeWeaponStopOnTerrainHit = this.MeleeWeaponStopOnTerrainHit;
			settings.MeleeWeaponIgnoreTerrainBehind = this.MeleeWeaponIgnoreTerrainBehind;
			settings.MeleeWeaponAttackSpeedMultiplier = this.MeleeWeaponAttackSpeedMultiplier;
			settings.GlobalAttackCooldownMs = this.GlobalAttackCooldownMs;
			settings.SecondChanceAvailable = this.SecondChanceAvailable;
			settings.EntityProtectionMultiplier = this.EntityProtectionMultiplier;
			settings.WeaponQuenchDamageMultiplier = this.WeaponQuenchDamageMultiplier;
			settings.RangedWeaponsDamageSupport = this.RangedWeaponsDamageSupport;
			settings.FueledItemUpdateInGameHours = this.FueledItemUpdateInGameHours;
		}
	}
}
