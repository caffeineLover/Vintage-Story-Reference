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
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00007BA7 File Offset: 0x00005DA7
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x00007BAF File Offset: 0x00005DAF
		public float BowsAimingHorizontalLimit { get; set; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00007BB8 File Offset: 0x00005DB8
		// (set) Token: 0x060001C6 RID: 454 RVA: 0x00007BC0 File Offset: 0x00005DC0
		public float BowsAimingVerticalLimit { get; set; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x00007BC9 File Offset: 0x00005DC9
		// (set) Token: 0x060001C8 RID: 456 RVA: 0x00007BD1 File Offset: 0x00005DD1
		public bool BowTwoHanded { get; set; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x00007BDA File Offset: 0x00005DDA
		// (set) Token: 0x060001CA RID: 458 RVA: 0x00007BE2 File Offset: 0x00005DE2
		public float ThrownWeaponsAimingHorizontalLimit { get; set; }

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060001CB RID: 459 RVA: 0x00007BEB File Offset: 0x00005DEB
		// (set) Token: 0x060001CC RID: 460 RVA: 0x00007BF3 File Offset: 0x00005DF3
		public float ThrownWeaponsAimingVerticalLimit { get; set; }

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001CD RID: 461 RVA: 0x00007BFC File Offset: 0x00005DFC
		// (set) Token: 0x060001CE RID: 462 RVA: 0x00007C04 File Offset: 0x00005E04
		public float SlingsAimingHorizontalLimit { get; set; }

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060001CF RID: 463 RVA: 0x00007C0D File Offset: 0x00005E0D
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x00007C15 File Offset: 0x00005E15
		public float SlingsAimingVerticalLimit { get; set; }

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x00007C1E File Offset: 0x00005E1E
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x00007C26 File Offset: 0x00005E26
		public bool VanillaActionsWhileBlocking { get; set; }

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x00007C2F File Offset: 0x00005E2F
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x00007C37 File Offset: 0x00005E37
		public float CollisionRadius { get; set; }

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x00007C40 File Offset: 0x00005E40
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x00007C48 File Offset: 0x00005E48
		public float DefaultColliderPenetrationResistance { get; set; }

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x00007C51 File Offset: 0x00005E51
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x00007C59 File Offset: 0x00005E59
		public bool MeleeWeaponStopOnTerrainHit { get; set; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001D9 RID: 473 RVA: 0x00007C62 File Offset: 0x00005E62
		// (set) Token: 0x060001DA RID: 474 RVA: 0x00007C6A File Offset: 0x00005E6A
		public bool MeleeWeaponIgnoreTerrainBehind { get; set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001DB RID: 475 RVA: 0x00007C73 File Offset: 0x00005E73
		// (set) Token: 0x060001DC RID: 476 RVA: 0x00007C7B File Offset: 0x00005E7B
		public float MeleeWeaponAttackSpeedMultiplier { get; set; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001DD RID: 477 RVA: 0x00007C84 File Offset: 0x00005E84
		// (set) Token: 0x060001DE RID: 478 RVA: 0x00007C8C File Offset: 0x00005E8C
		public int GlobalAttackCooldownMs { get; set; }

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001DF RID: 479 RVA: 0x00007C95 File Offset: 0x00005E95
		// (set) Token: 0x060001E0 RID: 480 RVA: 0x00007C9D File Offset: 0x00005E9D
		public bool SecondChanceAvailable { get; set; }

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x00007CA6 File Offset: 0x00005EA6
		// (set) Token: 0x060001E2 RID: 482 RVA: 0x00007CAE File Offset: 0x00005EAE
		public float EntityProtectionMultiplier { get; set; }

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x00007CB7 File Offset: 0x00005EB7
		// (set) Token: 0x060001E4 RID: 484 RVA: 0x00007CBF File Offset: 0x00005EBF
		public float WeaponQuenchDamageMultiplier { get; set; }

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060001E5 RID: 485 RVA: 0x00007CC8 File Offset: 0x00005EC8
		// (set) Token: 0x060001E6 RID: 486 RVA: 0x00007CD0 File Offset: 0x00005ED0
		public bool RangedWeaponsDamageSupport { get; set; }

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060001E7 RID: 487 RVA: 0x00007CD9 File Offset: 0x00005ED9
		// (set) Token: 0x060001E8 RID: 488 RVA: 0x00007CE1 File Offset: 0x00005EE1
		public float FueledItemUpdateInGameHours { get; set; }

		// Token: 0x060001E9 RID: 489 RVA: 0x00007CEC File Offset: 0x00005EEC
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

		// Token: 0x060001EA RID: 490 RVA: 0x00007DE4 File Offset: 0x00005FE4
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
