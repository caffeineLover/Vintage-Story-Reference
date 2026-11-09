using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.DamageSystems;
using CombatOverhaul.Inputs;
using CombatOverhaul.MeleeSystems;
using Vintagestory.API.Client;
using Vintagestory.API.Common;

namespace CombatOverhaul.Implementations
{
	// Token: 0x02000094 RID: 148
	[NullableContext(2)]
	[Nullable(0)]
	public class GripSpecificStats
	{
		// Token: 0x060007D9 RID: 2009 RVA: 0x0002DCB4 File Offset: 0x0002BEB4
		[NullableContext(1)]
		public GripSpecificStats(ICoreClientAPI api, Item item, StanceBasedMeleeWeaponGripStats stats)
		{
			this.Stats = stats;
			this.LeftClickAttacks = new Dictionary<ValueTuple<AttackDirection, AttackDirection>, MeleeAttack>();
			this.LeftClickHandleAttacks = new Dictionary<ValueTuple<AttackDirection, AttackDirection>, MeleeAttack>();
			this.LeftClickParries = new Dictionary<ValueTuple<AttackDirection, AttackDirection>, DamageBlockJson>();
			this.LeftClickAttackStats = new Dictionary<ValueTuple<AttackDirection, AttackDirection>, StanceBasedMeleeWeaponAttackStats>();
			this.LeftClickAttacksAnimations = new Dictionary<ValueTuple<AttackDirection, AttackDirection>, string[]>();
			this.RightClickAttacks = new Dictionary<ValueTuple<AttackDirection, AttackDirection>, MeleeAttack>();
			this.RightClickHandleAttacks = new Dictionary<ValueTuple<AttackDirection, AttackDirection>, MeleeAttack>();
			this.RightClickParries = new Dictionary<ValueTuple<AttackDirection, AttackDirection>, DamageBlockJson>();
			this.RightClickAttackStats = new Dictionary<ValueTuple<AttackDirection, AttackDirection>, StanceBasedMeleeWeaponAttackStats>();
			this.RightClickAttacksAnimations = new Dictionary<ValueTuple<AttackDirection, AttackDirection>, string[]>();
			this.Blocks = new Dictionary<AttackDirection, DamageBlockJson>();
			this.DirectionsType = Enum.Parse<DirectionsConfiguration>(stats.AttackDirectionsType);
			this.InitialStance = Enum.Parse<AttackDirection>(stats.InitialStance);
			IEnumerable<AttackDirection> directions = Enumerable.Select<int, AttackDirection>(DirectionController.Configurations[this.DirectionsType], (int element) => (AttackDirection)element);
			foreach (AttackDirection fromStance in directions)
			{
				foreach (AttackDirection toStance in directions)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(1, 2);
					defaultInterpolatedStringHandler.AppendFormatted<AttackDirection>(fromStance);
					defaultInterpolatedStringHandler.AppendLiteral("-");
					defaultInterpolatedStringHandler.AppendFormatted<AttackDirection>(toStance);
					string attackCode = defaultInterpolatedStringHandler.ToStringAndClear();
					if (stats.StanceToStanceLeftClickAttacks.ContainsKey(attackCode))
					{
						this.LeftClickAttacks.Add(new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance), new MeleeAttack(api, stats.StanceToStanceLeftClickAttacks[attackCode]));
						this.LeftClickHandleAttacks.Add(new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance), (stats.StanceToStanceLeftClickAttacks[attackCode].HandleAttack == null) ? null : new MeleeAttack(api, stats.StanceToStanceLeftClickAttacks[attackCode].HandleAttack));
						this.LeftClickParries.Add(new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance), stats.StanceToStanceLeftClickAttacks[attackCode].Parry);
						this.LeftClickAttackStats.Add(new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance), stats.StanceToStanceLeftClickAttacks[attackCode]);
						this.LeftClickAttacksAnimations.Add(new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance), stats.LeftClickAttacksAnimations[attackCode]);
						string item2 = item.Code.ToString();
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(12, 2);
						defaultInterpolatedStringHandler2.AppendLiteral("left-click-");
						defaultInterpolatedStringHandler2.AppendFormatted<AttackDirection>(fromStance);
						defaultInterpolatedStringHandler2.AppendLiteral("-");
						defaultInterpolatedStringHandler2.AppendFormatted<AttackDirection>(toStance);
						GripSpecificStats.RegisterCollider(item2, defaultInterpolatedStringHandler2.ToStringAndClear(), this.LeftClickAttacks[new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance)]);
						if (this.LeftClickHandleAttacks[new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance)] != null)
						{
							string item3 = item.Code.ToString();
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler3 = new DefaultInterpolatedStringHandler(19, 2);
							defaultInterpolatedStringHandler3.AppendLiteral("left-click-handle-");
							defaultInterpolatedStringHandler3.AppendFormatted<AttackDirection>(fromStance);
							defaultInterpolatedStringHandler3.AppendLiteral("-");
							defaultInterpolatedStringHandler3.AppendFormatted<AttackDirection>(toStance);
							GripSpecificStats.RegisterCollider(item3, defaultInterpolatedStringHandler3.ToStringAndClear(), this.LeftClickHandleAttacks[new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance)]);
						}
					}
					else
					{
						this.LeftClickAttacks.Add(new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance), new MeleeAttack(api, stats.DefaultLeftClickAttack));
						this.LeftClickHandleAttacks.Add(new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance), (stats.DefaultLeftClickAttack.HandleAttack == null) ? null : new MeleeAttack(api, stats.DefaultLeftClickAttack.HandleAttack));
						this.LeftClickParries.Add(new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance), stats.DefaultLeftClickAttack.Parry);
						this.LeftClickAttackStats.Add(new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance), stats.DefaultLeftClickAttack);
						this.LeftClickAttacksAnimations.Add(new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance), stats.LeftClickAttacksAnimations[attackCode]);
						string item4 = item.Code.ToString();
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler4 = new DefaultInterpolatedStringHandler(12, 2);
						defaultInterpolatedStringHandler4.AppendLiteral("left-click-");
						defaultInterpolatedStringHandler4.AppendFormatted<AttackDirection>(fromStance);
						defaultInterpolatedStringHandler4.AppendLiteral("-");
						defaultInterpolatedStringHandler4.AppendFormatted<AttackDirection>(toStance);
						GripSpecificStats.RegisterCollider(item4, defaultInterpolatedStringHandler4.ToStringAndClear(), this.LeftClickAttacks[new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance)]);
						if (this.LeftClickHandleAttacks[new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance)] != null)
						{
							string item5 = item.Code.ToString();
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler5 = new DefaultInterpolatedStringHandler(19, 2);
							defaultInterpolatedStringHandler5.AppendLiteral("left-click-handle-");
							defaultInterpolatedStringHandler5.AppendFormatted<AttackDirection>(fromStance);
							defaultInterpolatedStringHandler5.AppendLiteral("-");
							defaultInterpolatedStringHandler5.AppendFormatted<AttackDirection>(toStance);
							GripSpecificStats.RegisterCollider(item5, defaultInterpolatedStringHandler5.ToStringAndClear(), this.LeftClickHandleAttacks[new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance)]);
						}
					}
					if (stats.StanceToStanceRightClickAttacks.ContainsKey(attackCode))
					{
						this.RightClickAttacks.Add(new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance), new MeleeAttack(api, stats.StanceToStanceRightClickAttacks[attackCode]));
						this.RightClickHandleAttacks.Add(new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance), (stats.StanceToStanceRightClickAttacks[attackCode].HandleAttack == null) ? null : new MeleeAttack(api, stats.StanceToStanceRightClickAttacks[attackCode].HandleAttack));
						this.RightClickParries.Add(new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance), stats.StanceToStanceRightClickAttacks[attackCode].Parry);
						this.RightClickAttackStats.Add(new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance), stats.StanceToStanceRightClickAttacks[attackCode]);
						this.RightClickAttacksAnimations.Add(new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance), stats.RightClickAttacksAnimations[attackCode]);
						string item6 = item.Code.ToString();
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler6 = new DefaultInterpolatedStringHandler(13, 2);
						defaultInterpolatedStringHandler6.AppendLiteral("right-click-");
						defaultInterpolatedStringHandler6.AppendFormatted<AttackDirection>(fromStance);
						defaultInterpolatedStringHandler6.AppendLiteral("-");
						defaultInterpolatedStringHandler6.AppendFormatted<AttackDirection>(toStance);
						GripSpecificStats.RegisterCollider(item6, defaultInterpolatedStringHandler6.ToStringAndClear(), this.RightClickAttacks[new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance)]);
						if (this.RightClickHandleAttacks[new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance)] != null)
						{
							string item7 = item.Code.ToString();
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler7 = new DefaultInterpolatedStringHandler(20, 2);
							defaultInterpolatedStringHandler7.AppendLiteral("right-click-handle-");
							defaultInterpolatedStringHandler7.AppendFormatted<AttackDirection>(fromStance);
							defaultInterpolatedStringHandler7.AppendLiteral("-");
							defaultInterpolatedStringHandler7.AppendFormatted<AttackDirection>(toStance);
							GripSpecificStats.RegisterCollider(item7, defaultInterpolatedStringHandler7.ToStringAndClear(), this.RightClickHandleAttacks[new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance)]);
						}
					}
					else
					{
						this.RightClickAttacks.Add(new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance), new MeleeAttack(api, stats.DefaultRightClickAttack));
						this.RightClickHandleAttacks.Add(new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance), (stats.DefaultRightClickAttack.HandleAttack == null) ? null : new MeleeAttack(api, stats.DefaultRightClickAttack.HandleAttack));
						this.RightClickParries.Add(new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance), stats.DefaultRightClickAttack.Parry);
						this.RightClickAttackStats.Add(new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance), stats.DefaultRightClickAttack);
						this.RightClickAttacksAnimations.Add(new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance), stats.RightClickAttacksAnimations[attackCode]);
						string item8 = item.Code.ToString();
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler8 = new DefaultInterpolatedStringHandler(13, 2);
						defaultInterpolatedStringHandler8.AppendLiteral("right-click-");
						defaultInterpolatedStringHandler8.AppendFormatted<AttackDirection>(fromStance);
						defaultInterpolatedStringHandler8.AppendLiteral("-");
						defaultInterpolatedStringHandler8.AppendFormatted<AttackDirection>(toStance);
						GripSpecificStats.RegisterCollider(item8, defaultInterpolatedStringHandler8.ToStringAndClear(), this.RightClickAttacks[new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance)]);
						if (this.RightClickHandleAttacks[new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance)] != null)
						{
							string item9 = item.Code.ToString();
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler9 = new DefaultInterpolatedStringHandler(20, 2);
							defaultInterpolatedStringHandler9.AppendLiteral("right-click-handle-");
							defaultInterpolatedStringHandler9.AppendFormatted<AttackDirection>(fromStance);
							defaultInterpolatedStringHandler9.AppendLiteral("-");
							defaultInterpolatedStringHandler9.AppendFormatted<AttackDirection>(toStance);
							GripSpecificStats.RegisterCollider(item9, defaultInterpolatedStringHandler9.ToStringAndClear(), this.RightClickHandleAttacks[new ValueTuple<AttackDirection, AttackDirection>(fromStance, toStance)]);
						}
					}
				}
			}
			foreach (AttackDirection fromStance2 in Enum.GetValues<AttackDirection>())
			{
				if (stats.BlockByStance.ContainsKey(fromStance2.ToString()))
				{
					this.Blocks.Add(fromStance2, stats.BlockByStance[fromStance2.ToString()]);
				}
				else
				{
					this.Blocks.Add(fromStance2, stats.DefaultBlock);
				}
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060007DA RID: 2010 RVA: 0x0002E538 File Offset: 0x0002C738
		// (set) Token: 0x060007DB RID: 2011 RVA: 0x0002E540 File Offset: 0x0002C740
		public virtual DirectionsConfiguration DirectionsType { get; protected set; } = DirectionsConfiguration.None;

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060007DC RID: 2012 RVA: 0x0002E549 File Offset: 0x0002C749
		[Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		public Dictionary<ValueTuple<AttackDirection, AttackDirection>, MeleeAttack> RightClickAttacks { [return: Nullable(new byte[]
		{
			1,
			0,
			1
		})] get; }

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x0002E551 File Offset: 0x0002C751
		[Nullable(new byte[]
		{
			1,
			0,
			2
		})]
		public Dictionary<ValueTuple<AttackDirection, AttackDirection>, MeleeAttack> RightClickHandleAttacks { [return: Nullable(new byte[]
		{
			1,
			0,
			2
		})] get; }

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060007DE RID: 2014 RVA: 0x0002E559 File Offset: 0x0002C759
		[Nullable(new byte[]
		{
			1,
			0,
			2
		})]
		public Dictionary<ValueTuple<AttackDirection, AttackDirection>, DamageBlockJson> RightClickParries { [return: Nullable(new byte[]
		{
			1,
			0,
			2
		})] get; }

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060007DF RID: 2015 RVA: 0x0002E561 File Offset: 0x0002C761
		[Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		public Dictionary<ValueTuple<AttackDirection, AttackDirection>, StanceBasedMeleeWeaponAttackStats> RightClickAttackStats { [return: Nullable(new byte[]
		{
			1,
			0,
			1
		})] get; }

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060007E0 RID: 2016 RVA: 0x0002E569 File Offset: 0x0002C769
		[Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})]
		public Dictionary<ValueTuple<AttackDirection, AttackDirection>, string[]> RightClickAttacksAnimations { [return: Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})] get; }

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x0002E571 File Offset: 0x0002C771
		[Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		public Dictionary<ValueTuple<AttackDirection, AttackDirection>, MeleeAttack> LeftClickAttacks { [return: Nullable(new byte[]
		{
			1,
			0,
			1
		})] get; }

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060007E2 RID: 2018 RVA: 0x0002E579 File Offset: 0x0002C779
		[Nullable(new byte[]
		{
			1,
			0,
			2
		})]
		public Dictionary<ValueTuple<AttackDirection, AttackDirection>, MeleeAttack> LeftClickHandleAttacks { [return: Nullable(new byte[]
		{
			1,
			0,
			2
		})] get; }

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060007E3 RID: 2019 RVA: 0x0002E581 File Offset: 0x0002C781
		[Nullable(new byte[]
		{
			1,
			0,
			2
		})]
		public Dictionary<ValueTuple<AttackDirection, AttackDirection>, DamageBlockJson> LeftClickParries { [return: Nullable(new byte[]
		{
			1,
			0,
			2
		})] get; }

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060007E4 RID: 2020 RVA: 0x0002E589 File Offset: 0x0002C789
		[Nullable(new byte[]
		{
			1,
			0,
			1
		})]
		public Dictionary<ValueTuple<AttackDirection, AttackDirection>, StanceBasedMeleeWeaponAttackStats> LeftClickAttackStats { [return: Nullable(new byte[]
		{
			1,
			0,
			1
		})] get; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060007E5 RID: 2021 RVA: 0x0002E591 File Offset: 0x0002C791
		[Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})]
		public Dictionary<ValueTuple<AttackDirection, AttackDirection>, string[]> LeftClickAttacksAnimations { [return: Nullable(new byte[]
		{
			1,
			0,
			1,
			1
		})] get; }

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060007E6 RID: 2022 RVA: 0x0002E599 File Offset: 0x0002C799
		[Nullable(new byte[]
		{
			1,
			2
		})]
		public Dictionary<AttackDirection, DamageBlockJson> Blocks { [return: Nullable(new byte[]
		{
			1,
			2
		})] get; }

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060007E7 RID: 2023 RVA: 0x0002E5A1 File Offset: 0x0002C7A1
		public AttackDirection InitialStance { get; }

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060007E8 RID: 2024 RVA: 0x0002E5A9 File Offset: 0x0002C7A9
		// (set) Token: 0x060007E9 RID: 2025 RVA: 0x0002E5B1 File Offset: 0x0002C7B1
		public bool ParryButtonReleased { get; set; } = true;

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060007EA RID: 2026 RVA: 0x0002E5BA File Offset: 0x0002C7BA
		// (set) Token: 0x060007EB RID: 2027 RVA: 0x0002E5C2 File Offset: 0x0002C7C2
		public int AttackCounter { get; set; }

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060007EC RID: 2028 RVA: 0x0002E5CB File Offset: 0x0002C7CB
		// (set) Token: 0x060007ED RID: 2029 RVA: 0x0002E5D3 File Offset: 0x0002C7D3
		public bool HandleHitTerrain { get; set; }

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060007EE RID: 2030 RVA: 0x0002E5DC File Offset: 0x0002C7DC
		// (set) Token: 0x060007EF RID: 2031 RVA: 0x0002E5E4 File Offset: 0x0002C7E4
		public AttackDirection CurrentStance { get; set; }

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060007F0 RID: 2032 RVA: 0x0002E5ED File Offset: 0x0002C7ED
		// (set) Token: 0x060007F1 RID: 2033 RVA: 0x0002E5F5 File Offset: 0x0002C7F5
		public MeleeAttack CurrentAttack { get; set; }

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060007F2 RID: 2034 RVA: 0x0002E5FE File Offset: 0x0002C7FE
		// (set) Token: 0x060007F3 RID: 2035 RVA: 0x0002E606 File Offset: 0x0002C806
		public MeleeAttack CurrentHandleAttack { get; set; }

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060007F4 RID: 2036 RVA: 0x0002E60F File Offset: 0x0002C80F
		// (set) Token: 0x060007F5 RID: 2037 RVA: 0x0002E617 File Offset: 0x0002C817
		public StanceBasedMeleeWeaponAttackStats CurrentAttackStats { get; set; }

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060007F6 RID: 2038 RVA: 0x0002E620 File Offset: 0x0002C820
		// (set) Token: 0x060007F7 RID: 2039 RVA: 0x0002E628 File Offset: 0x0002C828
		[Nullable(1)]
		public StanceBasedMeleeWeaponGripStats Stats { [NullableContext(1)] get; [NullableContext(1)] set; }

		// Token: 0x060007F8 RID: 2040 RVA: 0x0002E631 File Offset: 0x0002C831
		[NullableContext(1)]
		protected static void RegisterCollider(string item, string type, MeleeAttack attack)
		{
		}

		// Token: 0x04000390 RID: 912
		public GripController GripController;
	}
}
