using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using CombatOverhaul.DamageSystems;
using CombatOverhaul.Utils;
using CombatOverhaul.WeaponBuffs;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace CombatOverhaul.RangedSystems
{
	// Token: 0x020000BE RID: 190
	[NullableContext(1)]
	[Nullable(0)]
	public class ProjectileBehavior : CollectibleBehavior
	{
		// Token: 0x1700023C RID: 572
		// (get) Token: 0x060009E8 RID: 2536 RVA: 0x000354CB File Offset: 0x000336CB
		// (set) Token: 0x060009E9 RID: 2537 RVA: 0x000354D3 File Offset: 0x000336D3
		[Nullable(2)]
		public ProjectileStats Stats { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x060009EA RID: 2538 RVA: 0x000354DC File Offset: 0x000336DC
		public ProjectileBehavior(CollectibleObject collObj) : base(collObj)
		{
		}

		// Token: 0x060009EB RID: 2539 RVA: 0x000354E5 File Offset: 0x000336E5
		public override void Initialize(JsonObject properties)
		{
			base.Initialize(properties);
			this.Stats = properties["stats"].AsObject<ProjectileStats>(null);
		}

		// Token: 0x060009EC RID: 2540 RVA: 0x00035508 File Offset: 0x00033708
		public ProjectileStats GetStats(ItemStack stack)
		{
			ItemStackProjectileStats stackStats = ItemStackProjectileStats.FromItemStack(stack);
			ProjectileStats stats = this.Stats.Clone();
			if (this.Stats.DamageStatsByType != null && this.Stats.DamageStatsByType.Count > 0)
			{
				Item item = stack.Item;
				if (((item != null) ? item.Code : null) != null)
				{
					string itemCode = stack.Item.Code.ToString();
					foreach (KeyValuePair<string, ProjectileDamageDataJson> entry in this.Stats.DamageStatsByType)
					{
						if (WildcardUtil.Match(entry.Key, itemCode))
						{
							stats.DamageStats = new ProjectileDamageDataJson
							{
								Damage = entry.Value.Damage,
								DamageType = entry.Value.DamageType
							};
							break;
						}
					}
				}
			}
			stats.DamageStats.Damage *= stackStats.DamageMultiplier * QuenchableStatUtil.GetAttackPowerMultiplier(stack);
			stats.DamageTierBonus += stackStats.DamageTierBonus;
			stats.DropChance = Math.Max(0f, Math.Min(1f, stats.DropChance * stackStats.DropChanceMultiplier));
			stats.Knockback *= stackStats.KnockbackMultiplier;
			stats.PenetrationBonus = Math.Max(0f, stackStats.PenetrationBonus + stats.PenetrationBonus);
			stats.AdditionalDurabilityCost = Math.Max(0, stackStats.AdditionalDurabilityCost + stats.AdditionalDurabilityCost);
			WeaponBuffSystem.ModifyProjectileStats(stack, stats);
			return stats;
		}

		// Token: 0x060009ED RID: 2541 RVA: 0x000356AC File Offset: 0x000338AC
		public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
		{
			if (this.Stats != null)
			{
				ProjectileStats stats = this.GetStats(inSlot.Itemstack);
				string text = "combatoverhaul:iteminfo-projectile";
				object[] array = new object[3];
				int num = 0;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(0, 1);
				defaultInterpolatedStringHandler.AppendFormatted<float>(stats.DamageStats.Damage, "F1");
				array[num] = defaultInterpolatedStringHandler.ToStringAndClear();
				array[1] = Lang.Get("combatoverhaul:damage-type-" + stats.DamageStats.DamageType, Array.Empty<object>());
				int num2 = 2;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
				defaultInterpolatedStringHandler2..ctor(0, 1);
				defaultInterpolatedStringHandler2.AppendFormatted<float>((1f - stats.DropChance) * 100f, "F1");
				array[num2] = defaultInterpolatedStringHandler2.ToStringAndClear();
				dsc.AppendLine(Lang.Get(text, array));
				if (stats.DamageTierBonus != 0)
				{
					dsc.AppendLine(Lang.Get("combatoverhaul:iteminfo-projectile-bonus-damagetier", new object[]
					{
						stats.DamageTierBonus
					}));
				}
				if (stats.AdditionalDurabilityCost != 0)
				{
					dsc.AppendLine(Lang.Get("combatoverhaul:iteminfo-projectile-durability-cost", new object[]
					{
						stats.AdditionalDurabilityCost
					}));
				}
			}
			WeaponBuffSystem.AppendTooltip(inSlot.Itemstack, dsc, world, withDebugInfo);
			base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
		}
	}
}
