using System;
using System.Collections;
using System.Runtime.CompilerServices;
using CombatOverhaul.Implementations;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace CombatOverhaul.Utils
{
	// Token: 0x02000020 RID: 32
	[NullableContext(2)]
	[Nullable(0)]
	public static class CollectibleClassifier
	{
		// Token: 0x06000228 RID: 552 RVA: 0x0000949C File Offset: 0x0000769C
		public static bool IsDagger(ItemSlot slot)
		{
			return CollectibleClassifier.IsDagger((slot != null) ? slot.Itemstack : null);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x000094AF File Offset: 0x000076AF
		public static bool IsDagger(ItemStack stack)
		{
			if (CollectibleClassifier.HasClassification(stack, "isDagger", "dagger"))
			{
				return true;
			}
			AssetLocation code;
			if (stack == null)
			{
				code = null;
			}
			else
			{
				CollectibleObject collectible = stack.Collectible;
				code = ((collectible != null) ? collectible.Code : null);
			}
			return CollectibleClassifier.IsDaggerCode(code);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x000094E2 File Offset: 0x000076E2
		public static bool IsDaggerCode(AssetLocation code)
		{
			return CollectibleClassifier.ContainsCodePart(code, "dagger");
		}

		// Token: 0x0600022B RID: 555 RVA: 0x000094F0 File Offset: 0x000076F0
		public static bool IsTongs(ItemStack stack)
		{
			if (CollectibleClassifier.HasClassification(stack, "isTongs", "tongs"))
			{
				return true;
			}
			string text;
			if (stack == null)
			{
				text = null;
			}
			else
			{
				CollectibleObject collectible = stack.Collectible;
				if (collectible == null)
				{
					text = null;
				}
				else
				{
					AssetLocation code = collectible.Code;
					text = ((code != null) ? code.Path : null);
				}
			}
			string path = text ?? "";
			return path.StartsWith("tongs", 5) || path.Contains("tongsmetal", 5);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000955B File Offset: 0x0000775B
		public static bool IsBow(ItemStack stack)
		{
			return CollectibleClassifier.IsBow(stack, (stack != null) ? stack.Collectible : null);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000956F File Offset: 0x0000776F
		public static bool IsBow(CollectibleObject collectible)
		{
			return CollectibleClassifier.IsBow(null, collectible);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00009578 File Offset: 0x00007778
		public static bool IsCrossbow(ItemStack stack)
		{
			return CollectibleClassifier.IsCrossbow(stack, (stack != null) ? stack.Collectible : null);
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000958C File Offset: 0x0000778C
		public static bool IsCrossbow(CollectibleObject collectible)
		{
			return CollectibleClassifier.IsCrossbow(null, collectible);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x00009598 File Offset: 0x00007798
		public static bool IsFirearm(CollectibleObject collectible)
		{
			if (collectible == null)
			{
				return false;
			}
			if (CollectibleClassifier.HasClassification(null, collectible, "isFirearm", "firearm"))
			{
				return true;
			}
			Type collectibleType = collectible.GetType();
			string fullName = collectibleType.FullName;
			if (fullName != null && fullName.StartsWith("Firearms.", 4))
			{
				return true;
			}
			string name = collectibleType.Assembly.GetName().Name;
			return name != null && name.Contains("Firearms", 5);
		}

		// Token: 0x06000231 RID: 561 RVA: 0x00009603 File Offset: 0x00007803
		public static bool IsShield(ItemStack stack)
		{
			return CollectibleClassifier.IsShield(stack, (stack != null) ? stack.Collectible : null);
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00009617 File Offset: 0x00007817
		public static bool IsShield(CollectibleObject collectible)
		{
			return CollectibleClassifier.IsShield(null, collectible);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x00009620 File Offset: 0x00007820
		private static bool IsShield(ItemStack stack, CollectibleObject collectible)
		{
			return collectible != null && (CollectibleClassifier.HasClassification(stack, collectible, "isShield", "shield") || collectible is ItemShield || collectible.Tool.GetValueOrDefault() == 23 || CollectibleClassifier.IsShieldCode(collectible.Code));
		}

		// Token: 0x06000234 RID: 564 RVA: 0x00009670 File Offset: 0x00007870
		private static bool IsBow(ItemStack stack, CollectibleObject collectible)
		{
			if (collectible == null)
			{
				return false;
			}
			if (CollectibleClassifier.IsFirearm(collectible) || CollectibleClassifier.IsCrossbow(stack, collectible))
			{
				return false;
			}
			if (CollectibleClassifier.HasClassification(stack, collectible, "isBow", "bow"))
			{
				return true;
			}
			string typeName = collectible.GetType().FullName ?? "";
			return typeName.EndsWith(".BowItem", 4) || typeName.EndsWith(".ItemBow", 4) || CollectibleClassifier.IsBowCode(collectible.Code);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x000096E8 File Offset: 0x000078E8
		private static bool IsCrossbow(ItemStack stack, CollectibleObject collectible)
		{
			if (collectible == null)
			{
				return false;
			}
			if (CollectibleClassifier.HasClassification(stack, collectible, "isCrossbow", "crossbow"))
			{
				return true;
			}
			Type collectibleType = collectible.GetType();
			if ((collectibleType.FullName ?? "").Contains("Crossbow", 4))
			{
				return true;
			}
			string name = collectibleType.Assembly.GetName().Name;
			return (name != null && name.Contains("Crossbows", 5) && CollectibleClassifier.IsCrossbowCode(collectible.Code)) || CollectibleClassifier.IsCrossbowCode(collectible.Code);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00009774 File Offset: 0x00007974
		private static bool IsBowCode(AssetLocation code)
		{
			string path = ((code != null) ? code.Path : null) ?? "";
			return path.Equals("bow", 5) || path.StartsWith("bow-", 5);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x000097B4 File Offset: 0x000079B4
		private static bool IsCrossbowCode(AssetLocation code)
		{
			string path = ((code != null) ? code.Path : null) ?? "";
			return path.Equals("crossbow", 5) || path.StartsWith("crossbow-", 5);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x000097F3 File Offset: 0x000079F3
		public static bool IsVanillaItemShield(Item item)
		{
			return CollectibleClassifier.IsUnpatchedVanillaItemShield(item);
		}

		// Token: 0x06000239 RID: 569 RVA: 0x000097FB File Offset: 0x000079FB
		public static bool IsUnpatchedVanillaItemShield(Item item)
		{
			return !(((item != null) ? item.GetType().FullName : null) != "Vintagestory.GameContent.ItemShield") && item.GetCollectibleInterface<IHasMeleeWeaponActions>() == null;
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00009825 File Offset: 0x00007A25
		public static bool HasMeleeWeaponActions(CollectibleObject collectible)
		{
			return ((collectible != null) ? collectible.GetCollectibleInterface<IHasMeleeWeaponActions>() : null) != null;
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00009838 File Offset: 0x00007A38
		private static bool IsShieldCode(AssetLocation code)
		{
			string path = ((code != null) ? code.Path : null) ?? "";
			return path.Equals("shield", 5) || path.StartsWith("shield-", 5) || path.Equals("roundshield", 5) || path.StartsWith("roundshield-", 5);
		}

		// Token: 0x0600023C RID: 572 RVA: 0x00009893 File Offset: 0x00007A93
		[NullableContext(1)]
		private static bool HasClassification([Nullable(2)] ItemStack stack, string attributeName, string tag)
		{
			return CollectibleClassifier.HasClassification(stack, (stack != null) ? stack.Collectible : null, attributeName, tag);
		}

		// Token: 0x0600023D RID: 573 RVA: 0x000098AC File Offset: 0x00007AAC
		[NullableContext(1)]
		private static bool HasClassification([Nullable(2)] ItemStack stack, [Nullable(2)] CollectibleObject collectible, string attributeName, string tag)
		{
			string qualifiedAttribute = "combatoverhaul:" + attributeName;
			return CollectibleClassifier.AttributeTrue((stack != null) ? stack.ItemAttributes : null, qualifiedAttribute, attributeName) || CollectibleClassifier.AttributeTrue((collectible != null) ? collectible.Attributes : null, qualifiedAttribute, attributeName) || CollectibleClassifier.HasTag((stack != null) ? stack.Item : null, tag) || CollectibleClassifier.HasTag(collectible, tag);
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000990C File Offset: 0x00007B0C
		[NullableContext(1)]
		private static bool AttributeTrue([Nullable(2)] JsonObject attributes, string qualifiedName, string shortName)
		{
			if ((attributes != null && attributes[qualifiedName].AsBool(false)) || (attributes != null && attributes[shortName].AsBool(false)))
			{
				return true;
			}
			if (attributes == null)
			{
				return false;
			}
			JsonObject jsonObject = attributes["combatoverhaul"];
			return ((jsonObject != null) ? new bool?(jsonObject[shortName].AsBool(false)) : default(bool?)).GetValueOrDefault();
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00009978 File Offset: 0x00007B78
		[NullableContext(1)]
		private static bool HasTag([Nullable(2)] CollectibleObject collectible, string tag)
		{
			Item item = collectible as Item;
			return item != null && CollectibleClassifier.HasTag(item, tag);
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00009998 File Offset: 0x00007B98
		[NullableContext(1)]
		private static bool HasTag([Nullable(2)] Item item, string tag)
		{
			IEnumerable tags = ((item != null) ? new TagSet?(item.Tags) : default(TagSet?)) as IEnumerable;
			if (tags == null)
			{
				return false;
			}
			foreach (object obj in tags)
			{
				if ((((obj != null) ? obj.ToString() : null) ?? "").Equals(tag, 5))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x00009A30 File Offset: 0x00007C30
		[NullableContext(1)]
		private static bool ContainsCodePart([Nullable(2)] AssetLocation code, string part)
		{
			return !(code == null) && (code.Path.Contains(part, 5) || code.Domain.Contains(part, 5));
		}
	}
}
