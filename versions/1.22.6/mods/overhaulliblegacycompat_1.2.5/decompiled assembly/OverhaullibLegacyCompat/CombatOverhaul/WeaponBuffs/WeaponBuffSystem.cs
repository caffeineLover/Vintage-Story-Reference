using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using CombatOverhaul.Implementations;
using CombatOverhaul.RangedSystems;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;

namespace CombatOverhaul.WeaponBuffs
{
	// Token: 0x020000AE RID: 174
	[NullableContext(1)]
	[Nullable(0)]
	public sealed class WeaponBuffSystem : ModSystem
	{
		// Token: 0x0600091D RID: 2333 RVA: 0x00030F91 File Offset: 0x0002F191
		public override void Start(ICoreAPI api)
		{
			this._api = api;
			WeaponBuffSystem.Current = this;
		}

		// Token: 0x0600091E RID: 2334 RVA: 0x00030FA0 File Offset: 0x0002F1A0
		public override void Dispose()
		{
			if (WeaponBuffSystem.Current == this)
			{
				WeaponBuffSystem.Current = null;
			}
			WeaponBuffSystem._providers.Clear();
			WeaponBuffSystem._providerErrors.Clear();
		}

		// Token: 0x0600091F RID: 2335 RVA: 0x00030FC4 File Offset: 0x0002F1C4
		public WeaponBuffInstance ApplyBuff(ItemSlot slot, WeaponBuffDefinition definition, [Nullable(2)] WeaponBuffApplyOptions options = null)
		{
			if (slot.Itemstack == null)
			{
				throw new ArgumentException("Cannot apply a weapon buff to an empty slot.", "slot");
			}
			WeaponBuffInstance result = this.ApplyBuff(slot.Itemstack, definition, options);
			if (options == null || options.MarkDirty)
			{
				slot.MarkDirty();
			}
			return result;
		}

		// Token: 0x06000920 RID: 2336 RVA: 0x00031000 File Offset: 0x0002F200
		public WeaponBuffInstance ApplyProjectileBuff(ItemSlot slot, WeaponBuffDefinition definition, [Nullable(2)] WeaponBuffApplyOptions options = null)
		{
			if (slot.Itemstack == null)
			{
				throw new ArgumentException("Cannot apply a projectile buff to an empty slot.", "slot");
			}
			WeaponBuffSystem.ValidateProjectileStack(slot.Itemstack, "slot");
			WeaponBuffInstance result = this.ApplyBuff(slot.Itemstack, definition, options);
			if (options == null || options.MarkDirty)
			{
				slot.MarkDirty();
			}
			return result;
		}

		// Token: 0x06000921 RID: 2337 RVA: 0x00031057 File Offset: 0x0002F257
		public WeaponBuffInstance ApplyProjectileBuff(ItemStack stack, WeaponBuffDefinition definition, [Nullable(2)] WeaponBuffApplyOptions options = null)
		{
			WeaponBuffSystem.ValidateProjectileStack(stack, "stack");
			return this.ApplyBuff(stack, definition, options);
		}

		// Token: 0x06000922 RID: 2338 RVA: 0x00031070 File Offset: 0x0002F270
		public WeaponBuffInstance ApplyBuff(ItemStack stack, WeaponBuffDefinition definition, [Nullable(2)] WeaponBuffApplyOptions options = null)
		{
			if (string.IsNullOrWhiteSpace(definition.Code))
			{
				throw new ArgumentException("Weapon buff code is required.", "definition");
			}
			if (options == null)
			{
				options = new WeaponBuffApplyOptions();
			}
			WeaponBuffSystem.EnsureAttributes(stack);
			if (options.ReplaceExisting)
			{
				this.RemoveBuff(stack, definition.Code, definition.SourceModId);
			}
			WeaponBuffInstance instance = WeaponBuffSystem.CreateInstance(definition, options);
			WeaponBuffSystem.GetOrCreateRoot(stack)[instance.InstanceId] = WeaponBuffSystem.ToTree(instance);
			return instance;
		}

		// Token: 0x06000923 RID: 2339 RVA: 0x000310E6 File Offset: 0x0002F2E6
		public int RemoveBuff(ItemSlot slot, string code, [Nullable(2)] string sourceModId = null)
		{
			int num = this.RemoveBuff(slot.Itemstack, code, sourceModId);
			if (num > 0)
			{
				slot.MarkDirty();
			}
			return num;
		}

		// Token: 0x06000924 RID: 2340 RVA: 0x00031100 File Offset: 0x0002F300
		public int RemoveProjectileBuff(ItemSlot slot, string code, [Nullable(2)] string sourceModId = null)
		{
			if (slot.Itemstack != null)
			{
				WeaponBuffSystem.ValidateProjectileStack(slot.Itemstack, "slot");
			}
			return this.RemoveBuff(slot, code, sourceModId);
		}

		// Token: 0x06000925 RID: 2341 RVA: 0x00031123 File Offset: 0x0002F323
		[NullableContext(2)]
		public int RemoveProjectileBuff(ItemStack stack, [Nullable(1)] string code, string sourceModId = null)
		{
			if (stack != null)
			{
				WeaponBuffSystem.ValidateProjectileStack(stack, "stack");
			}
			return this.RemoveBuff(stack, code, sourceModId);
		}

		// Token: 0x06000926 RID: 2342 RVA: 0x0003113C File Offset: 0x0002F33C
		[NullableContext(2)]
		public int RemoveBuff(ItemStack stack, [Nullable(1)] string code, string sourceModId = null)
		{
			if (((stack != null) ? stack.Attributes : null) == null)
			{
				return 0;
			}
			ITreeAttribute root = stack.Attributes.GetTreeAttribute("combatOverhaul:weaponBuffs");
			if (root == null)
			{
				return 0;
			}
			int removed = 0;
			foreach (KeyValuePair<string, IAttribute> entry in Enumerable.ToArray<KeyValuePair<string, IAttribute>>(root))
			{
				ITreeAttribute tree = entry.Value as ITreeAttribute;
				if (tree != null)
				{
					string buffCode = tree.GetString("Code", "");
					string buffSource = tree.GetString("SourceModId", "");
					if (string.Equals(buffCode, code, 4) && (sourceModId == null || string.Equals(buffSource, sourceModId, 4)))
					{
						root.RemoveAttribute(entry.Key);
						removed++;
					}
				}
			}
			WeaponBuffSystem.RemoveRootIfEmpty(stack, root);
			return removed;
		}

		// Token: 0x06000927 RID: 2343 RVA: 0x000311F9 File Offset: 0x0002F3F9
		public IReadOnlyList<WeaponBuffInstance> GetBuffs([Nullable(2)] ItemStack stack, bool activeOnly = true)
		{
			if (!WeaponBuffSystem.HasBuffRoot(stack))
			{
				return Array.Empty<WeaponBuffInstance>();
			}
			return Enumerable.ToArray<WeaponBuffInstance>(WeaponBuffSystem.OrderBuffs(WeaponBuffSystem.GetBuffsInternal(stack, activeOnly)));
		}

		// Token: 0x06000928 RID: 2344 RVA: 0x0003121A File Offset: 0x0002F41A
		[NullableContext(2)]
		public bool IsProjectileBuffTarget(ItemStack stack)
		{
			return WeaponBuffSystem.IsProjectileStack(stack);
		}

		// Token: 0x06000929 RID: 2345 RVA: 0x00031222 File Offset: 0x0002F422
		public void RegisterProvider(IWeaponBuffProvider provider)
		{
			if (!WeaponBuffSystem._providers.Contains(provider))
			{
				WeaponBuffSystem._providers.Add(provider);
			}
		}

		// Token: 0x0600092A RID: 2346 RVA: 0x0003123C File Offset: 0x0002F43C
		public void UnregisterProvider(IWeaponBuffProvider provider)
		{
			WeaponBuffSystem._providers.Remove(provider);
		}

		// Token: 0x0600092B RID: 2347 RVA: 0x0003124A File Offset: 0x0002F44A
		[NullableContext(2)]
		public int Consume(ItemSlot slot, WeaponBuffConsumptionTrigger trigger)
		{
			if (((slot != null) ? slot.Itemstack : null) == null)
			{
				return 0;
			}
			int num = this.Consume(slot.Itemstack, trigger);
			if (num > 0)
			{
				slot.MarkDirty();
			}
			return num;
		}

		// Token: 0x0600092C RID: 2348 RVA: 0x00031273 File Offset: 0x0002F473
		[NullableContext(2)]
		public int Consume(ItemStack stack, WeaponBuffConsumptionTrigger trigger)
		{
			return WeaponBuffSystem.ConsumeInternal(stack, trigger);
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x0600092D RID: 2349 RVA: 0x0003127C File Offset: 0x0002F47C
		// (set) Token: 0x0600092E RID: 2350 RVA: 0x00031283 File Offset: 0x0002F483
		[Nullable(2)]
		public static WeaponBuffSystem Current { [NullableContext(2)] get; [NullableContext(2)] private set; }

		// Token: 0x0600092F RID: 2351 RVA: 0x0003128C File Offset: 0x0002F48C
		internal static ItemStackMeleeWeaponStats ComposeMeleeStats(ItemStack stack, WeaponBuffMeleeStats stats)
		{
			if (!WeaponBuffSystem.HasBuffRoot(stack))
			{
				return stats.ToReadonly();
			}
			WeaponBuffQueryContext context = new WeaponBuffQueryContext(stack, "melee", null, null, null);
			using (IEnumerator<WeaponBuffInstance> enumerator = WeaponBuffSystem.GetActiveBuffs(stack).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					WeaponBuffInstance buff = enumerator.Current;
					WeaponBuffSystem.ApplyStatModifiers(buff, stats);
					WeaponBuffSystem.ApplyProvider(context, buff, delegate(IWeaponBuffProvider provider)
					{
						provider.ModifyMeleeStats(context, buff, stats);
					});
				}
			}
			return stats.ToReadonly();
		}

		// Token: 0x06000930 RID: 2352 RVA: 0x0003135C File Offset: 0x0002F55C
		internal static ItemStackRangedStats ComposeRangedStats(ItemStack stack, WeaponBuffRangedStats stats)
		{
			if (!WeaponBuffSystem.HasBuffRoot(stack))
			{
				return stats.ToReadonly();
			}
			WeaponBuffQueryContext context = new WeaponBuffQueryContext(stack, "ranged", null, null, null);
			using (IEnumerator<WeaponBuffInstance> enumerator = WeaponBuffSystem.GetActiveBuffs(stack).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					WeaponBuffInstance buff = enumerator.Current;
					WeaponBuffSystem.ApplyStatModifiers(buff, stats);
					WeaponBuffSystem.ApplyProvider(context, buff, delegate(IWeaponBuffProvider provider)
					{
						provider.ModifyRangedStats(context, buff, stats);
					});
				}
			}
			return stats.ToReadonly();
		}

		// Token: 0x06000931 RID: 2353 RVA: 0x0003142C File Offset: 0x0002F62C
		internal static ItemStackProjectileStats ComposeProjectileStackStats(ItemStack stack, WeaponBuffProjectileStats stats)
		{
			if (!WeaponBuffSystem.HasBuffRoot(stack))
			{
				return stats.ToReadonly();
			}
			WeaponBuffQueryContext context = new WeaponBuffQueryContext(stack, "projectile", null, null, null);
			using (IEnumerator<WeaponBuffInstance> enumerator = WeaponBuffSystem.GetActiveBuffs(stack).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					WeaponBuffInstance buff = enumerator.Current;
					WeaponBuffSystem.ApplyStatModifiers(buff, stats);
					WeaponBuffSystem.ApplyProvider(context, buff, delegate(IWeaponBuffProvider provider)
					{
						provider.ModifyProjectileStackStats(context, buff, stats);
					});
				}
			}
			return stats.ToReadonly();
		}

		// Token: 0x06000932 RID: 2354 RVA: 0x000314FC File Offset: 0x0002F6FC
		internal static void ModifyProjectileStats(ItemStack stack, ProjectileStats stats)
		{
			if (!WeaponBuffSystem.HasBuffRoot(stack))
			{
				return;
			}
			WeaponBuffQueryContext context = new WeaponBuffQueryContext(stack, "projectile-stats", null, null, null);
			using (IEnumerator<WeaponBuffInstance> enumerator = WeaponBuffSystem.GetActiveBuffs(stack).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					WeaponBuffInstance buff = enumerator.Current;
					WeaponBuffSystem.ApplyProvider(context, buff, delegate(IWeaponBuffProvider provider)
					{
						provider.ModifyProjectileStats(context, buff, stats);
					});
				}
			}
		}

		// Token: 0x06000933 RID: 2355 RVA: 0x000315A0 File Offset: 0x0002F7A0
		internal static void ModifyProjectileSpawn(ProjectileStats projectileStats, ref ProjectileSpawnStats spawnStats, ItemStack projectileStack, [Nullable(2)] ItemStack weaponStack, Entity shooter, Entity target)
		{
			WeaponBuffProjectileSpawnContext context = new WeaponBuffProjectileSpawnContext(projectileStats, spawnStats, projectileStack, weaponStack, shooter, target);
			using (IEnumerator<WeaponBuffInstance> enumerator = WeaponBuffSystem.GetActiveBuffs(projectileStack).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					WeaponBuffInstance buff = enumerator.Current;
					WeaponBuffSystem.ApplyProvider(new WeaponBuffQueryContext(projectileStack, "projectile-spawn", null, shooter, target), buff, delegate(IWeaponBuffProvider provider)
					{
						provider.ModifyProjectileSpawn(context, buff);
					});
				}
			}
			using (IEnumerator<WeaponBuffInstance> enumerator = WeaponBuffSystem.GetActiveBuffs(weaponStack).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					WeaponBuffInstance buff = enumerator.Current;
					WeaponBuffSystem.ApplyProvider(new WeaponBuffQueryContext(weaponStack, "weapon-projectile-spawn", null, shooter, target), buff, delegate(IWeaponBuffProvider provider)
					{
						provider.ModifyProjectileSpawn(context, buff);
					});
				}
			}
			spawnStats = context.SpawnStats;
		}

		// Token: 0x06000934 RID: 2356 RVA: 0x000316BC File Offset: 0x0002F8BC
		internal static void ModifyMeleeDamage(Entity target, DamageSource damageSource, [Nullable(2)] ItemSlot slot, ref float damage)
		{
			ItemStack weaponStack = (slot != null) ? slot.Itemstack : null;
			WeaponBuffDamageContext damageContext = new WeaponBuffDamageContext(target, damageSource, slot, weaponStack, damage, null);
			WeaponBuffQueryContext queryContext = new WeaponBuffQueryContext(weaponStack, "melee-damage", slot, damageSource.SourceEntity, target);
			using (IEnumerator<WeaponBuffInstance> enumerator = WeaponBuffSystem.GetActiveBuffs(weaponStack).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					WeaponBuffInstance buff = enumerator.Current;
					WeaponBuffSystem.ApplyProvider(queryContext, buff, delegate(IWeaponBuffProvider provider)
					{
						provider.ModifyMeleeDamage(damageContext, buff);
					});
				}
			}
			damage = damageContext.Damage;
		}

		// Token: 0x06000935 RID: 2357 RVA: 0x00031778 File Offset: 0x0002F978
		internal static void ModifyRangedDamage(Entity target, DamageSource damageSource, [Nullable(2)] ItemStack weaponStack, ref float damage)
		{
			WeaponBuffSystem.ModifyRangedDamage(target, damageSource, weaponStack, null, ref damage);
		}

		// Token: 0x06000936 RID: 2358 RVA: 0x00031784 File Offset: 0x0002F984
		internal static void ModifyRangedDamage(Entity target, DamageSource damageSource, [Nullable(2)] ItemStack weaponStack, [Nullable(2)] ItemStack projectileStack, ref float damage)
		{
			WeaponBuffDamageContext damageContext = new WeaponBuffDamageContext(target, damageSource, null, weaponStack, damage, projectileStack);
			Entity actor = damageSource.CauseEntity ?? damageSource.SourceEntity;
			using (IEnumerator<WeaponBuffInstance> enumerator = WeaponBuffSystem.GetActiveBuffs(weaponStack).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					WeaponBuffInstance buff = enumerator.Current;
					WeaponBuffSystem.ApplyProvider(new WeaponBuffQueryContext(weaponStack, "ranged-damage", null, actor, target), buff, delegate(IWeaponBuffProvider provider)
					{
						provider.ModifyRangedDamage(damageContext, buff);
					});
				}
			}
			using (IEnumerator<WeaponBuffInstance> enumerator = WeaponBuffSystem.GetActiveBuffs(projectileStack).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					WeaponBuffInstance buff = enumerator.Current;
					WeaponBuffSystem.ApplyProvider(new WeaponBuffQueryContext(projectileStack, "projectile-ranged-damage", null, actor, target), buff, delegate(IWeaponBuffProvider provider)
					{
						provider.ModifyRangedDamage(damageContext, buff);
					});
				}
			}
			damage = damageContext.Damage;
		}

		// Token: 0x06000937 RID: 2359 RVA: 0x000318AC File Offset: 0x0002FAAC
		public static void AppendTooltip([Nullable(2)] ItemStack stack, StringBuilder description, IWorldAccessor world, bool withDebugInfo)
		{
			if (stack == null)
			{
				return;
			}
			if (!WeaponBuffSystem.HasBuffRoot(stack))
			{
				return;
			}
			WeaponBuffTooltipContext context = new WeaponBuffTooltipContext(stack, description, world, withDebugInfo);
			WeaponBuffQueryContext queryContext = new WeaponBuffQueryContext(stack, "tooltip", null, null, null);
			using (IEnumerator<WeaponBuffInstance> enumerator = WeaponBuffSystem.GetActiveBuffs(stack).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					WeaponBuffInstance buff = enumerator.Current;
					WeaponBuffSystem.AppendDefaultTooltip(context, buff);
					WeaponBuffSystem.ApplyProvider(queryContext, buff, delegate(IWeaponBuffProvider provider)
					{
						provider.AppendTooltip(context, buff);
					});
				}
			}
		}

		// Token: 0x06000938 RID: 2360 RVA: 0x00031964 File Offset: 0x0002FB64
		[NullableContext(2)]
		internal static int ConsumeInternal(ItemStack stack, WeaponBuffConsumptionTrigger trigger)
		{
			ItemStack stack2 = stack;
			if (((stack2 != null) ? stack2.Attributes : null) == null || trigger == WeaponBuffConsumptionTrigger.None)
			{
				return 0;
			}
			ITreeAttribute root = stack.Attributes.GetTreeAttribute("combatOverhaul:weaponBuffs");
			if (root == null)
			{
				return 0;
			}
			int changed = 0;
			WeaponBuffSystem weaponBuffSystem = WeaponBuffSystem.Current;
			double? num;
			if (weaponBuffSystem == null)
			{
				num = default(double?);
			}
			else
			{
				ICoreAPI api = weaponBuffSystem._api;
				num = ((api != null) ? new double?(api.World.Calendar.TotalHours) : default(double?));
			}
			double? now = num;
			KeyValuePair<string, IAttribute>[] array = Enumerable.ToArray<KeyValuePair<string, IAttribute>>(root);
			for (int i = 0; i < array.Length; i++)
			{
				KeyValuePair<string, IAttribute> entry = array[i];
				ITreeAttribute tree = entry.Value as ITreeAttribute;
				if (tree != null)
				{
					WeaponBuffInstance buff = WeaponBuffSystem.FromTree(entry.Key, tree);
					if (!WeaponBuffSystem.IsExpired(buff, now))
					{
						int? usesRemaining = buff.UsesRemaining;
						int j = 0;
						if (!(usesRemaining.GetValueOrDefault() <= j & usesRemaining != null))
						{
							if (!WeaponBuffSystem.ShouldConsume(buff, trigger) || buff.UsesRemaining == null)
							{
								goto IL_275;
							}
							buff.UsesRemaining = new int?(Math.Max(0, buff.UsesRemaining.Value - 1));
							changed++;
							WeaponBuffQueryContext context = new WeaponBuffQueryContext(stack, "consume", null, null, null);
							IWeaponBuffProvider[] array2 = WeaponBuffSystem._providers.ToArray();
							for (j = 0; j < array2.Length; j++)
							{
								IWeaponBuffProvider provider = array2[j];
								if (WeaponBuffSystem.ProviderHandles(provider, context, buff))
								{
									WeaponBuffSystem.TryProvider(provider, delegate
									{
										provider.OnConsumed(stack, buff, trigger);
									});
								}
							}
							usesRemaining = buff.UsesRemaining;
							j = 0;
							if (usesRemaining.GetValueOrDefault() <= j & usesRemaining != null)
							{
								root.RemoveAttribute(entry.Key);
								goto IL_275;
							}
							root[entry.Key] = WeaponBuffSystem.ToTree(buff);
							goto IL_275;
						}
					}
					root.RemoveAttribute(entry.Key);
					changed++;
				}
				IL_275:;
			}
			WeaponBuffSystem.RemoveRootIfEmpty(stack, root);
			return changed;
		}

		// Token: 0x06000939 RID: 2361 RVA: 0x00031C04 File Offset: 0x0002FE04
		private static void ValidateProjectileStack(ItemStack stack, string paramName)
		{
			if (!WeaponBuffSystem.IsProjectileStack(stack))
			{
				CollectibleObject collectible = stack.Collectible;
				string text;
				if (collectible == null)
				{
					text = null;
				}
				else
				{
					AssetLocation code2 = collectible.Code;
					text = ((code2 != null) ? code2.ToString() : null);
				}
				string code = text ?? "unknown";
				throw new ArgumentException("Item stack '" + code + "' is not a Combat Overhaul projectile stack.", paramName);
			}
		}

		// Token: 0x0600093A RID: 2362 RVA: 0x00031C58 File Offset: 0x0002FE58
		[NullableContext(2)]
		private static bool IsProjectileStack(ItemStack stack)
		{
			object obj;
			if (stack == null)
			{
				obj = null;
			}
			else
			{
				CollectibleObject collectible = stack.Collectible;
				obj = ((collectible != null) ? collectible.GetCollectibleBehavior<ProjectileBehavior>(true) : null);
			}
			return obj != null;
		}

		// Token: 0x0600093B RID: 2363 RVA: 0x00031C78 File Offset: 0x0002FE78
		private static WeaponBuffInstance CreateInstance(WeaponBuffDefinition definition, WeaponBuffApplyOptions options)
		{
			double? durationHoursOverride = options.DurationHoursOverride;
			double? duration = (durationHoursOverride != null) ? durationHoursOverride : definition.DurationHours;
			int? usesOverride = options.UsesOverride;
			int? uses = (usesOverride != null) ? usesOverride : definition.Uses;
			WeaponBuffSystem weaponBuffSystem = WeaponBuffSystem.Current;
			double? num;
			if (weaponBuffSystem == null)
			{
				num = default(double?);
			}
			else
			{
				ICoreAPI api = weaponBuffSystem._api;
				num = ((api != null) ? new double?(api.World.Calendar.TotalHours) : default(double?));
			}
			double? now = num;
			WeaponBuffInstance weaponBuffInstance = new WeaponBuffInstance();
			weaponBuffInstance.InstanceId = (string.IsNullOrWhiteSpace(options.InstanceId) ? Guid.NewGuid().ToString("N") : options.InstanceId);
			weaponBuffInstance.Code = definition.Code;
			weaponBuffInstance.SourceModId = definition.SourceModId;
			weaponBuffInstance.DisplayName = definition.DisplayName;
			weaponBuffInstance.DisplayNameLangCode = definition.DisplayNameLangCode;
			weaponBuffInstance.Description = definition.Description;
			weaponBuffInstance.DescriptionLangCode = definition.DescriptionLangCode;
			weaponBuffInstance.ExpiresAtTotalHours = ((duration != null && now != null) ? new double?(now.Value + Math.Max(0.0, duration.Value)) : default(double?));
			weaponBuffInstance.UsesRemaining = uses;
			weaponBuffInstance.Priority = definition.Priority;
			IEnumerable<WeaponStatModifier> modifiers = definition.Modifiers;
			Func<WeaponStatModifier, WeaponStatModifier> func;
			if ((func = WeaponBuffSystem.<>O.<0>__CloneModifier) == null)
			{
				func = (WeaponBuffSystem.<>O.<0>__CloneModifier = new Func<WeaponStatModifier, WeaponStatModifier>(WeaponBuffSystem.CloneModifier));
			}
			weaponBuffInstance.Modifiers = Enumerable.ToList<WeaponStatModifier>(Enumerable.Select<WeaponStatModifier, WeaponStatModifier>(modifiers, func));
			weaponBuffInstance.ConsumeOn = Enumerable.ToList<WeaponBuffConsumptionTrigger>(definition.ConsumeOn);
			weaponBuffInstance.Data = definition.Data;
			return weaponBuffInstance;
		}

		// Token: 0x0600093C RID: 2364 RVA: 0x00031E19 File Offset: 0x00030019
		private static IEnumerable<WeaponBuffInstance> GetBuffsInternal([Nullable(2)] ItemStack stack, bool activeOnly)
		{
			WeaponBuffSystem.<GetBuffsInternal>d__37 <GetBuffsInternal>d__ = new WeaponBuffSystem.<GetBuffsInternal>d__37(-2);
			<GetBuffsInternal>d__.<>3__stack = stack;
			<GetBuffsInternal>d__.<>3__activeOnly = activeOnly;
			return <GetBuffsInternal>d__;
		}

		// Token: 0x0600093D RID: 2365 RVA: 0x00031E30 File Offset: 0x00030030
		private static IEnumerable<WeaponBuffInstance> GetActiveBuffs([Nullable(2)] ItemStack stack)
		{
			if (!WeaponBuffSystem.HasBuffRoot(stack))
			{
				return Array.Empty<WeaponBuffInstance>();
			}
			return WeaponBuffSystem.OrderBuffs(WeaponBuffSystem.GetBuffsInternal(stack, true));
		}

		// Token: 0x0600093E RID: 2366 RVA: 0x00031E4C File Offset: 0x0003004C
		[NullableContext(2)]
		private static bool HasBuffRoot(ItemStack stack)
		{
			object obj;
			if (stack == null)
			{
				obj = null;
			}
			else
			{
				ITreeAttribute attributes = stack.Attributes;
				obj = ((attributes != null) ? attributes.GetTreeAttribute("combatOverhaul:weaponBuffs") : null);
			}
			return obj != null;
		}

		// Token: 0x0600093F RID: 2367 RVA: 0x00031E70 File Offset: 0x00030070
		private static IOrderedEnumerable<WeaponBuffInstance> OrderBuffs(IEnumerable<WeaponBuffInstance> buffs)
		{
			return Enumerable.ThenBy<WeaponBuffInstance, string>(Enumerable.ThenBy<WeaponBuffInstance, string>(Enumerable.ThenBy<WeaponBuffInstance, string>(Enumerable.OrderBy<WeaponBuffInstance, int>(buffs, (WeaponBuffInstance buff) => buff.Priority), (WeaponBuffInstance buff) => buff.SourceModId, StringComparer.Ordinal), (WeaponBuffInstance buff) => buff.Code, StringComparer.Ordinal), (WeaponBuffInstance buff) => buff.InstanceId, StringComparer.Ordinal);
		}

		// Token: 0x06000940 RID: 2368 RVA: 0x00031F20 File Offset: 0x00030120
		private static ITreeAttribute GetOrCreateRoot(ItemStack stack)
		{
			WeaponBuffSystem.EnsureAttributes(stack);
			ITreeAttribute root = stack.Attributes.GetTreeAttribute("combatOverhaul:weaponBuffs");
			if (root != null)
			{
				return root;
			}
			root = new TreeAttribute();
			stack.Attributes["combatOverhaul:weaponBuffs"] = root;
			return root;
		}

		// Token: 0x06000941 RID: 2369 RVA: 0x00031F64 File Offset: 0x00030164
		private static void EnsureAttributes(ItemStack stack)
		{
			if (stack.Attributes == null)
			{
				stack.Attributes = new TreeAttribute();
			}
		}

		// Token: 0x06000942 RID: 2370 RVA: 0x00031F88 File Offset: 0x00030188
		private static void RemoveRootIfEmpty(ItemStack stack, ITreeAttribute root)
		{
			if (root.Count == 0)
			{
				stack.Attributes.RemoveAttribute("combatOverhaul:weaponBuffs");
			}
		}

		// Token: 0x06000943 RID: 2371 RVA: 0x00031FA4 File Offset: 0x000301A4
		private static ITreeAttribute ToTree(WeaponBuffInstance instance)
		{
			TreeAttribute tree = new TreeAttribute();
			tree.SetString("InstanceId", instance.InstanceId);
			tree.SetString("Code", instance.Code);
			tree.SetString("SourceModId", instance.SourceModId);
			tree.SetString("DisplayName", instance.DisplayName);
			tree.SetString("DisplayNameLangCode", instance.DisplayNameLangCode);
			tree.SetString("Description", instance.Description);
			tree.SetString("DescriptionLangCode", instance.DescriptionLangCode);
			tree.SetInt("Priority", instance.Priority);
			if (instance.ExpiresAtTotalHours != null)
			{
				tree.SetDouble("ExpiresAtTotalHours", instance.ExpiresAtTotalHours.Value);
			}
			if (instance.UsesRemaining != null)
			{
				tree.SetInt("UsesRemaining", instance.UsesRemaining.Value);
			}
			if (instance.ConsumeOn.Count > 0)
			{
				tree.SetString("ConsumeOn", string.Join(",", Enumerable.Select<WeaponBuffConsumptionTrigger, string>(instance.ConsumeOn, (WeaponBuffConsumptionTrigger trigger) => trigger.ToString())));
			}
			TreeAttribute modifiers = new TreeAttribute();
			for (int index = 0; index < instance.Modifiers.Count; index++)
			{
				WeaponStatModifier modifier = instance.Modifiers[index];
				TreeAttribute modifierTree = new TreeAttribute();
				modifierTree.SetString("StatCode", modifier.StatCode);
				modifierTree.SetFloat("Add", modifier.Add);
				modifierTree.SetFloat("Multiply", modifier.Multiply);
				modifierTree.SetInt("Priority", modifier.Priority);
				modifiers.SetAttribute(index.ToString(), modifierTree);
			}
			tree.SetAttribute("Modifiers", modifiers);
			if (instance.Data != null)
			{
				tree.SetAttribute("Data", instance.Data);
			}
			return tree;
		}

		// Token: 0x06000944 RID: 2372 RVA: 0x00032198 File Offset: 0x00030398
		private static WeaponBuffInstance FromTree(string key, ITreeAttribute tree)
		{
			WeaponBuffInstance instance = new WeaponBuffInstance
			{
				InstanceId = tree.GetString("InstanceId", key),
				Code = tree.GetString("Code", ""),
				SourceModId = tree.GetString("SourceModId", ""),
				DisplayName = tree.GetString("DisplayName", ""),
				DisplayNameLangCode = tree.GetString("DisplayNameLangCode", ""),
				Description = tree.GetString("Description", ""),
				DescriptionLangCode = tree.GetString("DescriptionLangCode", ""),
				Priority = tree.GetInt("Priority", 0),
				Data = tree.GetTreeAttribute("Data")
			};
			double expires = tree.GetDouble("ExpiresAtTotalHours", double.NaN);
			if (!double.IsNaN(expires))
			{
				instance.ExpiresAtTotalHours = new double?(expires);
			}
			int uses = tree.GetInt("UsesRemaining", int.MinValue);
			if (uses != -2147483648)
			{
				instance.UsesRemaining = new int?(uses);
			}
			string consumeOn = tree.GetString("ConsumeOn", "");
			if (!string.IsNullOrWhiteSpace(consumeOn))
			{
				string[] array = consumeOn.Split(',', 3);
				for (int i = 0; i < array.Length; i++)
				{
					WeaponBuffConsumptionTrigger trigger;
					if (Enum.TryParse<WeaponBuffConsumptionTrigger>(array[i], true, ref trigger))
					{
						instance.ConsumeOn.Add(trigger);
					}
				}
			}
			ITreeAttribute modifiers = tree.GetTreeAttribute("Modifiers");
			if (modifiers != null)
			{
				IAttribute[] values = modifiers.Values;
				for (int i = 0; i < values.Length; i++)
				{
					ITreeAttribute modifierTree = values[i] as ITreeAttribute;
					if (modifierTree != null)
					{
						string statCode = modifierTree.GetString("StatCode", "");
						if (!string.IsNullOrWhiteSpace(statCode))
						{
							instance.Modifiers.Add(new WeaponStatModifier
							{
								StatCode = statCode,
								Add = modifierTree.GetFloat("Add", 0f),
								Multiply = modifierTree.GetFloat("Multiply", 1f),
								Priority = modifierTree.GetInt("Priority", 0)
							});
						}
					}
				}
				if (instance.Modifiers.Count > 1)
				{
					instance.Modifiers.Sort((WeaponStatModifier left, WeaponStatModifier right) => left.Priority.CompareTo(right.Priority));
				}
			}
			return instance;
		}

		// Token: 0x06000945 RID: 2373 RVA: 0x000323F9 File Offset: 0x000305F9
		private static WeaponStatModifier CloneModifier(WeaponStatModifier modifier)
		{
			return new WeaponStatModifier
			{
				StatCode = modifier.StatCode,
				Add = modifier.Add,
				Multiply = modifier.Multiply,
				Priority = modifier.Priority
			};
		}

		// Token: 0x06000946 RID: 2374 RVA: 0x00032430 File Offset: 0x00030630
		private static bool IsExpired(WeaponBuffInstance buff, double? now)
		{
			return buff.ExpiresAtTotalHours != null && now != null && now.Value >= buff.ExpiresAtTotalHours.Value;
		}

		// Token: 0x06000947 RID: 2375 RVA: 0x00032472 File Offset: 0x00030672
		private static bool ShouldConsume(WeaponBuffInstance buff, WeaponBuffConsumptionTrigger trigger)
		{
			return buff.ConsumeOn.Contains(WeaponBuffConsumptionTrigger.Any) || buff.ConsumeOn.Contains(trigger);
		}

		// Token: 0x06000948 RID: 2376 RVA: 0x00032490 File Offset: 0x00030690
		private static void ApplyStatModifiers(WeaponBuffInstance buff, WeaponBuffMeleeStats stats)
		{
			stats.DamageMultiplier = WeaponBuffSystem.ApplyFloat(buff, "damageMultiplier", stats.DamageMultiplier);
			stats.DamageBonus = WeaponBuffSystem.ApplyFloat(buff, "damageBonus", stats.DamageBonus);
			stats.DamageTierBonus = WeaponBuffSystem.ApplyInt(buff, "damageTierBonus", stats.DamageTierBonus);
			stats.AttackSpeed = WeaponBuffSystem.ApplyFloat(buff, "attackSpeed", stats.AttackSpeed);
			stats.BlockTierBonus = WeaponBuffSystem.ApplyInt(buff, "blockTierBonus", stats.BlockTierBonus);
			stats.ParryTierBonus = WeaponBuffSystem.ApplyInt(buff, "parryTierBonus", stats.ParryTierBonus);
			stats.ThrownDamageMultiplier = WeaponBuffSystem.ApplyFloat(buff, "thrownDamageMultiplier", stats.ThrownDamageMultiplier);
			stats.ThrownDamageTierBonus = WeaponBuffSystem.ApplyInt(buff, "thrownDamageTierBonus", stats.ThrownDamageTierBonus);
			stats.ThrownAimingDifficulty = WeaponBuffSystem.ApplyFloat(buff, "thrownAimingDifficulty", stats.ThrownAimingDifficulty);
			stats.ThrownProjectileSpeedMultiplier = WeaponBuffSystem.ApplyFloat(buff, "thrownProjectileSpeedMultiplier", stats.ThrownProjectileSpeedMultiplier);
			stats.KnockbackMultiplier = WeaponBuffSystem.ApplyFloat(buff, "knockbackMultiplier", stats.KnockbackMultiplier);
			stats.ArmorPiercingBonus = WeaponBuffSystem.ApplyInt(buff, "armorPiercingBonus", stats.ArmorPiercingBonus);
		}

		// Token: 0x06000949 RID: 2377 RVA: 0x000325B4 File Offset: 0x000307B4
		private static void ApplyStatModifiers(WeaponBuffInstance buff, WeaponBuffRangedStats stats)
		{
			stats.ReloadSpeed = WeaponBuffSystem.ApplyFloat(buff, "reloadSpeed", stats.ReloadSpeed);
			stats.DamageMultiplier = WeaponBuffSystem.ApplyFloat(buff, "damageMultiplier", stats.DamageMultiplier);
			stats.DamageTierBonus = WeaponBuffSystem.ApplyInt(buff, "damageTierBonus", stats.DamageTierBonus);
			stats.ProjectileSpeed = WeaponBuffSystem.ApplyFloat(buff, "projectileSpeed", stats.ProjectileSpeed);
			stats.DispersionMultiplier = WeaponBuffSystem.ApplyFloat(buff, "dispersionMultiplier", stats.DispersionMultiplier);
			stats.AimingDifficulty = WeaponBuffSystem.ApplyFloat(buff, "aimingDifficulty", stats.AimingDifficulty);
		}

		// Token: 0x0600094A RID: 2378 RVA: 0x0003264C File Offset: 0x0003084C
		private static void ApplyStatModifiers(WeaponBuffInstance buff, WeaponBuffProjectileStats stats)
		{
			stats.DamageMultiplier = WeaponBuffSystem.ApplyFloat(buff, "damageMultiplier", stats.DamageMultiplier);
			stats.DamageTierBonus = WeaponBuffSystem.ApplyInt(buff, "damageTierBonus", stats.DamageTierBonus);
			stats.KnockbackMultiplier = WeaponBuffSystem.ApplyFloat(buff, "knockbackMultiplier", stats.KnockbackMultiplier);
			stats.DropChanceMultiplier = WeaponBuffSystem.ApplyFloat(buff, "dropChanceMultiplier", stats.DropChanceMultiplier);
			stats.PenetrationBonus = WeaponBuffSystem.ApplyFloat(buff, "penetrationBonus", stats.PenetrationBonus);
			stats.AdditionalDurabilityCost = WeaponBuffSystem.ApplyInt(buff, "additionalDurabilityCost", stats.AdditionalDurabilityCost);
		}

		// Token: 0x0600094B RID: 2379 RVA: 0x000326E4 File Offset: 0x000308E4
		private static float ApplyFloat(WeaponBuffInstance buff, string statCode, float value)
		{
			List<WeaponStatModifier> modifiers = buff.Modifiers;
			for (int index = 0; index < modifiers.Count; index++)
			{
				WeaponStatModifier modifier = modifiers[index];
				if (string.Equals(modifier.StatCode, statCode, 5))
				{
					float next = (value + modifier.Add) * modifier.Multiply;
					if (float.IsFinite(next))
					{
						value = next;
					}
				}
			}
			return value;
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0003273C File Offset: 0x0003093C
		private static int ApplyInt(WeaponBuffInstance buff, string statCode, int value)
		{
			float modified = WeaponBuffSystem.ApplyFloat(buff, statCode, (float)value);
			if (!float.IsFinite(modified))
			{
				return value;
			}
			return (int)MathF.Round(modified);
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x00032764 File Offset: 0x00030964
		private static void ApplyProvider(WeaponBuffQueryContext context, WeaponBuffInstance buff, Action<IWeaponBuffProvider> action)
		{
			IWeaponBuffProvider[] array = WeaponBuffSystem._providers.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				IWeaponBuffProvider provider = array[i];
				if (WeaponBuffSystem.ProviderHandles(provider, context, buff))
				{
					WeaponBuffSystem.TryProvider(provider, delegate
					{
						action.Invoke(provider);
					});
				}
			}
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x000327D4 File Offset: 0x000309D4
		private static bool ProviderHandles(IWeaponBuffProvider provider, WeaponBuffQueryContext context, WeaponBuffInstance buff)
		{
			bool handles = false;
			WeaponBuffSystem.TryProvider(provider, delegate
			{
				handles = provider.Handles(context, buff);
			});
			return handles;
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x00032820 File Offset: 0x00030A20
		private static void TryProvider(IWeaponBuffProvider provider, Action action)
		{
			try
			{
				action.Invoke();
			}
			catch (Exception exception)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(2, 3);
				defaultInterpolatedStringHandler.AppendFormatted(provider.GetType().FullName);
				defaultInterpolatedStringHandler.AppendLiteral(":");
				defaultInterpolatedStringHandler.AppendFormatted(exception.GetType().FullName);
				defaultInterpolatedStringHandler.AppendLiteral(":");
				defaultInterpolatedStringHandler.AppendFormatted(exception.Message);
				string key = defaultInterpolatedStringHandler.ToStringAndClear();
				if (WeaponBuffSystem._providerErrors.Add(key))
				{
					WeaponBuffSystem weaponBuffSystem = WeaponBuffSystem.Current;
					if (weaponBuffSystem != null)
					{
						ICoreAPI api = weaponBuffSystem._api;
						if (api != null)
						{
							ILogger logger = api.Logger;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2;
							defaultInterpolatedStringHandler2..ctor(37, 2);
							defaultInterpolatedStringHandler2.AppendLiteral("[WeaponBuffSystem] Provider ");
							defaultInterpolatedStringHandler2.AppendFormatted(provider.GetType().FullName);
							defaultInterpolatedStringHandler2.AppendLiteral(" failed: ");
							defaultInterpolatedStringHandler2.AppendFormatted<Exception>(exception);
							logger.Warning(defaultInterpolatedStringHandler2.ToStringAndClear());
						}
					}
				}
			}
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x00032918 File Offset: 0x00030B18
		private static void AppendDefaultTooltip(WeaponBuffTooltipContext context, WeaponBuffInstance buff)
		{
			string name = WeaponBuffSystem.ResolveText(buff.DisplayNameLangCode, buff.DisplayName, buff.Code);
			if (!string.IsNullOrWhiteSpace(name))
			{
				context.Description.AppendLine(name);
			}
			string description = WeaponBuffSystem.ResolveText(buff.DescriptionLangCode, buff.Description, "");
			if (!string.IsNullOrWhiteSpace(description))
			{
				context.Description.AppendLine(description);
			}
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x00032980 File Offset: 0x00030B80
		private static string ResolveText(string langCode, string literal, string fallback)
		{
			if (!string.IsNullOrWhiteSpace(langCode))
			{
				string translated = Lang.Get(langCode, Array.Empty<object>());
				if (!string.Equals(translated, langCode, 4))
				{
					return translated;
				}
			}
			if (!string.IsNullOrWhiteSpace(literal))
			{
				return literal;
			}
			return fallback;
		}

		// Token: 0x04000447 RID: 1095
		public const string AttributeKey = "combatOverhaul:weaponBuffs";

		// Token: 0x04000449 RID: 1097
		private static readonly List<IWeaponBuffProvider> _providers = new List<IWeaponBuffProvider>();

		// Token: 0x0400044A RID: 1098
		private static readonly HashSet<string> _providerErrors = new HashSet<string>();

		// Token: 0x0400044B RID: 1099
		[Nullable(2)]
		private ICoreAPI _api;

		// Token: 0x02000226 RID: 550
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000BF8 RID: 3064
			[Nullable(0)]
			public static Func<WeaponStatModifier, WeaponStatModifier> <0>__CloneModifier;
		}
	}
}
