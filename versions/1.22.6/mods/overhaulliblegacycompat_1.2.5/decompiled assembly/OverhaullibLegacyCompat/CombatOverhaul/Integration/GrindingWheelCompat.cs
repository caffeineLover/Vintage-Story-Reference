using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.Implementations;
using CombatOverhaul.Inputs;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace CombatOverhaul.Integration
{
	// Token: 0x02000039 RID: 57
	[NullableContext(1)]
	[Nullable(0)]
	public static class GrindingWheelCompat
	{
		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060003C4 RID: 964 RVA: 0x0001174C File Offset: 0x0000F94C
		// (remove) Token: 0x060003C5 RID: 965 RVA: 0x00011780 File Offset: 0x0000F980
		[Nullable(2)]
		[method: NullableContext(2)]
		[Nullable(2)]
		public static event GrindingWheelCompat.ExtraWeaponCriticalHitChanceDelegate ExtraWeaponCriticalHitChance;

		// Token: 0x060003C6 RID: 966 RVA: 0x000117B3 File Offset: 0x0000F9B3
		public static void SetApi(ICoreAPI api)
		{
			GrindingWheelCompat._api = api;
			CriticalHitFeedback.Register(api);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x000117C1 File Offset: 0x0000F9C1
		public static void Dispose()
		{
			CriticalHitFeedback.Dispose();
			GrindingWheelCompat._api = null;
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x000117D0 File Offset: 0x0000F9D0
		public static void EnsureWeaponBuffableBehavior(ICoreAPI api)
		{
			GrindingWheelCompat.SetApi(api);
			int eligible = 0;
			int patched = 0;
			foreach (Item item in api.World.Items)
			{
				if (!(((item != null) ? item.Code : null) == null) && GrindingWheelCompat.IsSharpenableWeapon(api, item))
				{
					eligible++;
					if (item.GetCollectibleBehavior<CollectibleBehaviorBuffable>(true) == null)
					{
						GrindingWheelCompat.AddBuffableBehavior(item, api);
						patched++;
					}
				}
			}
			if (patched > 0)
			{
				ILogger logger = api.Logger;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
				defaultInterpolatedStringHandler..ctor(120, 2);
				defaultInterpolatedStringHandler.AppendLiteral("[OverhaullibLegacyCompat] Enabled vanilla grinding wheel sharpening for ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(eligible);
				defaultInterpolatedStringHandler.AppendLiteral(" weapon items; added Buffable to ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(patched);
				defaultInterpolatedStringHandler.AppendLiteral(" missing items.");
				logger.Notification(defaultInterpolatedStringHandler.ToStringAndClear());
			}
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x000118B0 File Offset: 0x0000FAB0
		public static float ApplyBuffableDamage([Nullable(2)] ItemStack weaponStack, Entity target, float damage)
		{
			return GrindingWheelCompat.ApplyBuffableDamage(weaponStack, target, damage, null, true);
		}

		// Token: 0x060003CA RID: 970 RVA: 0x000118BC File Offset: 0x0000FABC
		[NullableContext(2)]
		public static float ApplyBuffableDamage(ItemStack weaponStack, [Nullable(1)] Entity target, float damage, DamageSource damageSource)
		{
			return GrindingWheelCompat.ApplyBuffableDamage(weaponStack, target, damage, damageSource, true);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x000118C8 File Offset: 0x0000FAC8
		[NullableContext(2)]
		public static float ApplyBuffableDamage(ItemStack weaponStack, [Nullable(1)] Entity target, float damage, DamageSource damageSource, bool allowCriticalHit)
		{
			if (damage <= 0f || ((weaponStack != null) ? weaponStack.Collectible : null) == null)
			{
				return damage;
			}
			CollectibleBehaviorBuffable behavior = weaponStack.Collectible.GetCollectibleBehavior<CollectibleBehaviorBuffable>(true);
			float criticalHitChance = allowCriticalHit ? GrindingWheelCompat.GetCombinedCriticalHitChance(weaponStack, target, damage, behavior) : 0f;
			if (criticalHitChance > 0f && target.World.Rand.NextDouble() < (double)criticalHitChance)
			{
				damage *= 2f;
				CriticalHitFeedback.Trigger(damageSource);
			}
			if (behavior == null)
			{
				return damage;
			}
			return GrindingWheelCompat.ApplyBuffableDamageWithoutGrindingWheelCriticalRoll(weaponStack, target, damage, behavior);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00011948 File Offset: 0x0000FB48
		private static float ApplyBuffableDamageWithoutGrindingWheelCriticalRoll(ItemStack weaponStack, Entity target, float damage, CollectibleBehaviorBuffable behavior)
		{
			List<AppliedCollectibleBuff> originalBuffs;
			try
			{
				originalBuffs = GrindingWheelCompat.CloneBuffs(behavior.GetItemBuffs(weaponStack));
			}
			catch
			{
				bool fallbackCriticalHit = false;
				return weaponStack.Collectible.GetDamageToEntity(damage, target, weaponStack, ref fallbackCriticalHit);
			}
			IEnumerable<AppliedCollectibleBuff> enumerable = originalBuffs;
			Func<AppliedCollectibleBuff, bool> func;
			if ((func = GrindingWheelCompat.<>O.<0>__IsGrindingWheelCriticalHitBuff) == null)
			{
				func = (GrindingWheelCompat.<>O.<0>__IsGrindingWheelCriticalHitBuff = new Func<AppliedCollectibleBuff, bool>(GrindingWheelCompat.IsGrindingWheelCriticalHitBuff));
			}
			if (!Enumerable.Any<AppliedCollectibleBuff>(enumerable, func))
			{
				bool nonCriticalHit = false;
				return weaponStack.Collectible.GetDamageToEntity(damage, target, weaponStack, ref nonCriticalHit);
			}
			List<AppliedCollectibleBuff> nonRollingBuffs = GrindingWheelCompat.CloneBuffs(originalBuffs);
			foreach (AppliedCollectibleBuff buff in nonRollingBuffs)
			{
				if (GrindingWheelCompat.IsGrindingWheelCriticalHitBuff(buff))
				{
					buff.Multiplier = ((buff.Multiplier > 1f) ? 1f : 0f);
				}
			}
			float damageToEntity;
			try
			{
				behavior.StoreItemBuffs(weaponStack, nonRollingBuffs);
				bool nonCriticalHit2 = false;
				damageToEntity = weaponStack.Collectible.GetDamageToEntity(damage, target, weaponStack, ref nonCriticalHit2);
			}
			finally
			{
				behavior.StoreItemBuffs(weaponStack, originalBuffs);
			}
			return damageToEntity;
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00011A64 File Offset: 0x0000FC64
		private static float GetCombinedCriticalHitChance(ItemStack weaponStack, Entity target, float damage, [Nullable(2)] CollectibleBehaviorBuffable behavior)
		{
			float chance = GrindingWheelCompat.GetGrindingWheelCriticalHitChance(weaponStack, behavior);
			GrindingWheelCompat.ExtraWeaponCriticalHitChanceDelegate handlers = GrindingWheelCompat.ExtraWeaponCriticalHitChance;
			if (handlers != null)
			{
				foreach (GrindingWheelCompat.ExtraWeaponCriticalHitChanceDelegate handler in Enumerable.Cast<GrindingWheelCompat.ExtraWeaponCriticalHitChanceDelegate>(handlers.GetInvocationList()))
				{
					try
					{
						chance += Math.Max(0f, handler(weaponStack, target, damage));
					}
					catch (Exception exception)
					{
						ICoreAPI api = GrindingWheelCompat._api;
						if (api != null)
						{
							ILogger logger = api.Logger;
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(70, 1);
							defaultInterpolatedStringHandler.AppendLiteral("[OverhaullibLegacyCompat] Weapon critical hit chance provider failed: ");
							defaultInterpolatedStringHandler.AppendFormatted<Exception>(exception);
							logger.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
						}
					}
				}
			}
			return Math.Clamp(chance, 0f, 1f);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x00011B38 File Offset: 0x0000FD38
		private static float GetGrindingWheelCriticalHitChance(ItemStack weaponStack, [Nullable(2)] CollectibleBehaviorBuffable behavior)
		{
			if (behavior == null)
			{
				return 0f;
			}
			float result;
			try
			{
				float chance = 0f;
				foreach (AppliedCollectibleBuff buff in behavior.GetItemBuffs(weaponStack))
				{
					if (GrindingWheelCompat.IsGrindingWheelCriticalHitBuff(buff))
					{
						chance += GrindingWheelCompat.NormalizeCriticalHitChance(buff.Multiplier);
					}
				}
				result = Math.Clamp(chance, 0f, 1f);
			}
			catch
			{
				result = 0f;
			}
			return result;
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00011BD4 File Offset: 0x0000FDD4
		private static bool IsGrindingWheelCriticalHitBuff(AppliedCollectibleBuff buff)
		{
			return buff.Code.Equals("sharpened", 5) && buff.StatCode.Equals("critchance", 5) && GrindingWheelCompat.NormalizeCriticalHitChance(buff.Multiplier) > 0f;
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00011C10 File Offset: 0x0000FE10
		private static float NormalizeCriticalHitChance(float multiplier)
		{
			if (multiplier <= 0f)
			{
				return 0f;
			}
			if (multiplier <= 1f)
			{
				return multiplier;
			}
			return multiplier - 1f;
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00011C31 File Offset: 0x0000FE31
		private static List<AppliedCollectibleBuff> CloneBuffs(IEnumerable<AppliedCollectibleBuff> buffs)
		{
			return Enumerable.ToList<AppliedCollectibleBuff>(Enumerable.Select<AppliedCollectibleBuff, AppliedCollectibleBuff>(buffs, (AppliedCollectibleBuff buff) => new AppliedCollectibleBuff
			{
				Code = buff.Code,
				StatCode = buff.StatCode,
				Multiplier = buff.Multiplier,
				FlatChange = buff.FlatChange,
				RemainingDurability = buff.RemainingDurability
			}));
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00011C60 File Offset: 0x0000FE60
		[NullableContext(2)]
		public static bool TryEnableGrindingWheelBuff(ItemSlot slot)
		{
			object obj;
			if (slot == null)
			{
				obj = null;
			}
			else
			{
				ItemStack itemstack = slot.Itemstack;
				obj = ((itemstack != null) ? itemstack.Collectible : null);
			}
			Item item = obj as Item;
			if (item == null || !GrindingWheelCompat.IsSharpenableWeapon(GrindingWheelCompat._api, item))
			{
				return false;
			}
			CollectibleBehaviorBuffable behavior = item.GetBehavior<CollectibleBehaviorBuffable>();
			if (behavior == null)
			{
				behavior = GrindingWheelCompat.AddBuffableBehavior(item, GrindingWheelCompat._api);
			}
			AppliedCollectibleBuff sharpened = Enumerable.FirstOrDefault<AppliedCollectibleBuff>(behavior.GetItemBuffs(slot.Itemstack), (AppliedCollectibleBuff buff) => buff.Code == "sharpened");
			return sharpened == null || sharpened.Multiplier < 1.099f;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00011CF6 File Offset: 0x0000FEF6
		public static bool PlayerTriesToUseGrindingWheel(EntityPlayer player, [Nullable(2)] ItemSlot slot, ActionEventData eventData, bool mainHand)
		{
			if (!mainHand || eventData.Action.Action != 10)
			{
				return false;
			}
			BlockSelection blockSelection = player.BlockSelection;
			return GrindingWheelCompat.IsGrindingWheel((blockSelection != null) ? blockSelection.Block : null) && GrindingWheelCompat.TryEnableGrindingWheelBuff(slot);
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x00011D2D File Offset: 0x0000FF2D
		[NullableContext(2)]
		public static bool IsGrindingWheel(Block block)
		{
			if (block == null)
			{
				return false;
			}
			if (block is BlockGrindingWheel)
			{
				return true;
			}
			AssetLocation code = block.Code;
			return (((code != null) ? code.Path : null) ?? "").StartsWith("grindingwheel-", 5);
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x00011D64 File Offset: 0x0000FF64
		[NullableContext(2)]
		public static bool TryStartGrindingWeapon(IWorldAccessor world, IPlayer player, BlockSelection blockSelection)
		{
			if (world == null || player == null || blockSelection == null)
			{
				return false;
			}
			IPlayerInventoryManager inventoryManager = player.InventoryManager;
			if (!GrindingWheelCompat.TryEnableGrindingWheelBuff((inventoryManager != null) ? inventoryManager.ActiveHotbarSlot : null))
			{
				return false;
			}
			BlockEntityGrindingWheel blockEntityGrindingWheel = world.BlockAccessor.GetBlockEntity(blockSelection.Position) as BlockEntityGrindingWheel;
			return blockEntityGrindingWheel != null && blockEntityGrindingWheel.OnInteractStart(player, blockSelection);
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00011DBC File Offset: 0x0000FFBC
		private static CollectibleBehaviorBuffable AddBuffableBehavior(Item item, [Nullable(2)] ICoreAPI api)
		{
			CollectibleBehaviorBuffable behavior = new CollectibleBehaviorBuffable(item);
			behavior.Initialize(new JsonObject(new JObject()));
			if (api != null)
			{
				behavior.OnLoaded(api);
			}
			item.CollectibleBehaviors = Enumerable.ToArray<CollectibleBehavior>(Enumerable.Append<CollectibleBehavior>(item.CollectibleBehaviors ?? Array.Empty<CollectibleBehavior>(), behavior));
			return behavior;
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00011E0C File Offset: 0x0001000C
		private static bool IsSharpenableWeapon([Nullable(2)] ICoreAPI api, Item item)
		{
			if (api != null && GrindingWheelCompat.HasTag(api, item, "weapon-melee"))
			{
				return true;
			}
			bool flag = item is MeleeWeapon || item is StanceBasedMeleeWeapon;
			return flag || item.GetCollectibleBehavior<MeleeWeaponBehavior>(true) != null || GrindingWheelCompat.HasCombatOverhaulMeleeStats(item) || GrindingWheelCompat.LooksLikeMeleeWeaponCode(item.Code.Path);
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00011E6C File Offset: 0x0001006C
		private static bool HasCombatOverhaulMeleeStats(Item item)
		{
			JsonObject attributes = item.Attributes;
			return attributes != null && (attributes["OneHandedStance"].Exists || attributes["TwoHandedStance"].Exists || attributes["OffHandStance"].Exists || attributes["MainHandStance"].Exists || attributes["ThrowAttack"].Exists);
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00011EE0 File Offset: 0x000100E0
		private static bool HasTag(ICoreAPI api, Item item, string tag)
		{
			if (item.Tags.IsEmpty)
			{
				return false;
			}
			bool result;
			try
			{
				ITagRegistry<TagSet> collectibleTagRegistry = api.CollectibleTagRegistry;
				string text = tag;
				TagSet tagSet;
				collectibleTagRegistry.TryRegisterAndCreateTagSet(ref tagSet, new ReadOnlySpan<string>(ref text));
				result = (!tagSet.IsEmpty && tagSet.IsFullyContainedIn(ref item.Tags));
			}
			catch
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00011F48 File Offset: 0x00010148
		private static bool LooksLikeMeleeWeaponCode(string path)
		{
			return !path.Contains("firearm", 5) && !path.Contains("bullet", 5) && !path.Contains("cartridge", 5) && !path.Contains("arrow", 5) && !path.Contains("bolt", 5) && !path.Contains("bow", 5) && !path.Contains("sling", 5) && (path.Contains("blade", 5) || path.Contains("sword", 5) || path.Contains("sabre", 5) || path.Contains("dagger", 5) || path.Contains("spear", 5) || path.Contains("javelin", 5) || path.Contains("halberd", 5) || path.Contains("poleaxe", 5) || path.Contains("pike", 5) || path.Contains("mace", 5) || path.Contains("club", 5) || path.Contains("warhammer", 5) || path.Contains("battleaxe", 5) || path.Contains("longaxe", 5));
		}

		// Token: 0x0400015A RID: 346
		private const string GrindingWheelCritStatCode = "critchance";

		// Token: 0x0400015B RID: 347
		private const string GrindingWheelSharpenedBuffCode = "sharpened";

		// Token: 0x0400015C RID: 348
		private const float CriticalHitDamageMultiplier = 2f;

		// Token: 0x0400015D RID: 349
		[Nullable(2)]
		private static ICoreAPI _api;

		// Token: 0x020001C9 RID: 457
		// (Invoke) Token: 0x060014D0 RID: 5328
		[NullableContext(0)]
		public delegate float ExtraWeaponCriticalHitChanceDelegate(ItemStack weaponStack, Entity target, float damage);

		// Token: 0x020001CA RID: 458
		[CompilerGenerated]
		private static class <>O
		{
			// Token: 0x04000AC3 RID: 2755
			[Nullable(0)]
			public static Func<AppliedCollectibleBuff, bool> <0>__IsGrindingWheelCriticalHitBuff;
		}
	}
}
