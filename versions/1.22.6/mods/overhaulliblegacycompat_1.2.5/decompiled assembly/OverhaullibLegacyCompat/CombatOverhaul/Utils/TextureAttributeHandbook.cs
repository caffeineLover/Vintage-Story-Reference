using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace CombatOverhaul.Utils
{
	// Token: 0x0200002F RID: 47
	[NullableContext(1)]
	[Nullable(0)]
	public static class TextureAttributeHandbook
	{
		// Token: 0x0600032F RID: 815 RVA: 0x0000EBC8 File Offset: 0x0000CDC8
		public static string[] GetTextureAttributes(CollectibleObject collectible)
		{
			CollectibleBehavior[] collectibleBehaviors = collectible.CollectibleBehaviors;
			string[] array;
			if (collectibleBehaviors == null)
			{
				array = null;
			}
			else
			{
				array = Enumerable.ToArray<string>(Enumerable.Distinct<string>(Enumerable.Where<string>(Enumerable.SelectMany<CollectibleBehavior, string>(collectibleBehaviors, delegate(CollectibleBehavior behavior)
				{
					TexturesFromAttributes textures = behavior as TexturesFromAttributes;
					string[] result;
					if (textures == null)
					{
						TextureFromAttributes texture = behavior as TextureFromAttributes;
						if (texture == null)
						{
							result = Array.Empty<string>();
						}
						else
						{
							result = texture.TextureAttributes;
						}
					}
					else
					{
						result = textures.TextureAttributes;
					}
					return result;
				}), (string attribute) => !string.IsNullOrWhiteSpace(attribute)), StringComparer.Ordinal));
			}
			return array ?? Array.Empty<string>();
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000EC44 File Offset: 0x0000CE44
		public static string PageCodeForStack(ItemStack stack, IEnumerable<string> attributesToIgnore)
		{
			string[] attributes = TextureAttributeHandbook.Normalize(attributesToIgnore);
			if (attributes.Length == 0)
			{
				return GuiHandbookItemStackPage.PageCodeForStack(stack);
			}
			ItemStack itemStack = stack.Clone();
			TextureAttributeHandbook.RemoveAttributes(itemStack.Attributes, attributes);
			return GuiHandbookItemStackPage.PageCodeForStack(itemStack);
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000EC7C File Offset: 0x0000CE7C
		public static string VisualAttributePageCodeForStack(ItemStack stack)
		{
			string[] attributes = TextureAttributeHandbook.GetVisualAttributes(stack);
			if (attributes.Length != 0)
			{
				return TextureAttributeHandbook.PageCodeForStack(stack, attributes);
			}
			return TextureAttributeHandbook.VisualGroupPageCodeForStack(stack) ?? GuiHandbookItemStackPage.PageCodeForStack(stack);
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000ECAC File Offset: 0x0000CEAC
		[NullableContext(2)]
		public static bool MatchesIgnoringTextureAttributes(ItemStack thisStack, ItemStack otherStack)
		{
			return ((thisStack != null) ? thisStack.Collectible : null) != null && ((otherStack != null) ? otherStack.Collectible : null) != null && TextureAttributeHandbook.ShouldUseGroupedHandbookPage(thisStack) && TextureAttributeHandbook.ShouldUseGroupedHandbookPage(otherStack) && TextureAttributeHandbook.VisualAttributePageCodeForStack(thisStack) == TextureAttributeHandbook.VisualAttributePageCodeForStack(otherStack);
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000ECFC File Offset: 0x0000CEFC
		[return: Nullable(2)]
		public static string GroupPageCodeForStack(ItemStack stack)
		{
			AssetLocation groupCode = TextureAttributeHandbook.GroupCodeForStack(stack);
			if (!(groupCode == null))
			{
				return ItemClassMethods.Name(stack.Class) + "-" + groupCode.ToShortString();
			}
			return null;
		}

		// Token: 0x06000334 RID: 820 RVA: 0x0000ED38 File Offset: 0x0000CF38
		[return: Nullable(2)]
		public static AssetLocation GroupCodeForStack(ItemStack stack)
		{
			AssetLocation assetLocation;
			if (stack == null)
			{
				assetLocation = null;
			}
			else
			{
				CollectibleObject collectible = stack.Collectible;
				assetLocation = ((collectible != null) ? collectible.Code : null);
			}
			if (assetLocation == null)
			{
				return null;
			}
			string[] groups = TextureAttributeHandbook.GetHandbookGroupBy(stack.Collectible);
			if (groups.Length == 0)
			{
				return null;
			}
			string wildcard = TextureAttributeHandbook.NormalizeHandbookGroupWildcard(groups[0], stack);
			if (string.IsNullOrWhiteSpace(wildcard))
			{
				return null;
			}
			if (!wildcard.Contains(':'))
			{
				return new AssetLocation(stack.Collectible.Code.Domain, wildcard);
			}
			return new AssetLocation(wildcard);
		}

		// Token: 0x06000335 RID: 821 RVA: 0x0000EDB4 File Offset: 0x0000CFB4
		public static string[] GetHandbookGroupBy(CollectibleObject collectible)
		{
			JsonObject attributes = collectible.Attributes;
			string[] array;
			if (attributes == null)
			{
				array = null;
			}
			else
			{
				JsonObject jsonObject = attributes["handbook"];
				array = ((jsonObject != null) ? jsonObject["groupBy"].AsArray<string>(null, "game") : null);
			}
			return array ?? Array.Empty<string>();
		}

		// Token: 0x06000336 RID: 822 RVA: 0x0000EDF2 File Offset: 0x0000CFF2
		[NullableContext(2)]
		public static bool ShouldUseGroupedHandbookPage(ItemStack stack)
		{
			return ((stack != null) ? stack.Collectible : null) != null && (TextureAttributeHandbook.GetVisualAttributes(stack).Length != 0 || TextureAttributeHandbook.VisualGroupPageCodeForStack(stack) != null);
		}

		// Token: 0x06000337 RID: 823 RVA: 0x0000EE18 File Offset: 0x0000D018
		public static string[] GetVisualAttributes([Nullable(2)] ItemStack stack)
		{
			if (((stack != null) ? stack.Collectible : null) == null)
			{
				return Array.Empty<string>();
			}
			return TextureAttributeHandbook.Normalize(Enumerable.Concat<string>(Enumerable.Concat<string>(TextureAttributeHandbook.GetTextureAttributes(stack.Collectible), TextureAttributeHandbook.GetConfiguredVisualAttributes(stack.Collectible)), TextureAttributeHandbook.GetImplicitVisualAttributes(stack)));
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000EE64 File Offset: 0x0000D064
		[return: Nullable(2)]
		public static string VisualGroupPageCodeForStack(ItemStack stack)
		{
			AssetLocation groupCode = TextureAttributeHandbook.VisualGroupCodeForStack(stack);
			if (!(groupCode == null))
			{
				return ItemClassMethods.Name(stack.Class) + "-" + groupCode.ToShortString();
			}
			return null;
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000EEA0 File Offset: 0x0000D0A0
		public static string ResolveHandbookGroupWildcard(string wildcard, ItemStack stack)
		{
			if (((stack != null) ? stack.Collectible : null) == null || string.IsNullOrWhiteSpace(wildcard))
			{
				return wildcard;
			}
			foreach (KeyValuePair<string, string> keyValuePair in stack.Collectible.Variant)
			{
				string text;
				string text2;
				keyValuePair.Deconstruct(ref text, ref text2);
				string key = text;
				string value = text2;
				wildcard = wildcard.Replace("{" + key + "}", value, 4);
			}
			foreach (string key2 in TextureAttributeHandbook.GetPlaceholders(wildcard))
			{
				ITreeAttribute attributes = stack.Attributes;
				string value2 = (attributes != null) ? attributes.GetAsString(key2, null) : null;
				if (!string.IsNullOrWhiteSpace(value2))
				{
					wildcard = wildcard.Replace("{" + key2 + "}", value2, 4);
				}
			}
			return wildcard;
		}

		// Token: 0x0600033A RID: 826 RVA: 0x0000EFA8 File Offset: 0x0000D1A8
		public static string NormalizeHandbookGroupWildcard(string wildcard, ItemStack stack)
		{
			return TextureAttributeHandbook.NarrowLeadingWildcard(TextureAttributeHandbook.ResolveHandbookGroupWildcard(wildcard, stack), stack);
		}

		// Token: 0x0600033B RID: 827 RVA: 0x0000EFB8 File Offset: 0x0000D1B8
		public static bool MatchesHandbookGroup(ItemStack groupStack, ItemStack candidate)
		{
			AssetLocation assetLocation;
			if (groupStack == null)
			{
				assetLocation = null;
			}
			else
			{
				CollectibleObject collectible = groupStack.Collectible;
				assetLocation = ((collectible != null) ? collectible.Code : null);
			}
			if (!(assetLocation == null))
			{
				AssetLocation assetLocation2;
				if (candidate == null)
				{
					assetLocation2 = null;
				}
				else
				{
					CollectibleObject collectible2 = candidate.Collectible;
					assetLocation2 = ((collectible2 != null) ? collectible2.Code : null);
				}
				if (!(assetLocation2 == null))
				{
					string[] handbookGroupBy = TextureAttributeHandbook.GetHandbookGroupBy(groupStack.Collectible);
					for (int i = 0; i < handbookGroupBy.Length; i++)
					{
						string wildcard = TextureAttributeHandbook.NormalizeHandbookGroupWildcard(handbookGroupBy[i], groupStack);
						AssetLocation assetLocation3 = wildcard.Contains(':') ? new AssetLocation(wildcard) : new AssetLocation(groupStack.Collectible.Code.Domain, wildcard);
						IHandbookGrouping collectibleInterface = candidate.Collectible.GetCollectibleInterface<IHandbookGrouping>();
						AssetLocation candidateCode = ((collectibleInterface != null) ? collectibleInterface.GetCodeForHandbookGrouping(candidate) : null) ?? candidate.Collectible.Code;
						if (WildcardUtil.Match(assetLocation3, candidateCode))
						{
							return true;
						}
					}
					return false;
				}
			}
			return false;
		}

		// Token: 0x0600033C RID: 828 RVA: 0x0000F088 File Offset: 0x0000D288
		[NullableContext(2)]
		public static bool SatisfiesIgnoringAttributes(IWorldAccessor world, ItemStack thisStack, ItemStack otherStack, [Nullable(1)] IEnumerable<string> attributesToIgnore)
		{
			string[] attributes = TextureAttributeHandbook.Normalize(attributesToIgnore);
			if (attributes.Length == 0 || world == null || thisStack == null || otherStack == null)
			{
				return false;
			}
			if (thisStack.Class != otherStack.Class || thisStack.Id != otherStack.Id)
			{
				return false;
			}
			ITreeAttribute treeAttribute = thisStack.Attributes.Clone();
			ITreeAttribute otherAttributes = otherStack.Attributes.Clone();
			TextureAttributeHandbook.RemoveAttributes(treeAttribute, attributes);
			TextureAttributeHandbook.RemoveAttributes(otherAttributes, attributes);
			return treeAttribute.IsSubSetOf(world, otherAttributes);
		}

		// Token: 0x0600033D RID: 829 RVA: 0x0000F0F8 File Offset: 0x0000D2F8
		private static void RemoveAttributes(ITreeAttribute attributes, IEnumerable<string> attributesToRemove)
		{
			foreach (string attribute in attributesToRemove)
			{
				attributes.RemoveAttribute(attribute);
			}
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000F140 File Offset: 0x0000D340
		private static string[] Normalize(IEnumerable<string> attributes)
		{
			return Enumerable.ToArray<string>(Enumerable.Distinct<string>(Enumerable.Where<string>(attributes, (string attribute) => !string.IsNullOrWhiteSpace(attribute)), StringComparer.Ordinal));
		}

		// Token: 0x0600033F RID: 831 RVA: 0x0000F176 File Offset: 0x0000D376
		private static string[] GetConfiguredVisualAttributes(CollectibleObject collectible)
		{
			JsonObject attributes = collectible.Attributes;
			string[] array;
			if (attributes == null)
			{
				array = null;
			}
			else
			{
				JsonObject jsonObject = attributes["handbook"];
				array = ((jsonObject != null) ? jsonObject["visualAttributes"].AsArray<string>(null, "game") : null);
			}
			return array ?? Array.Empty<string>();
		}

		// Token: 0x06000340 RID: 832 RVA: 0x0000F1B4 File Offset: 0x0000D3B4
		private static IEnumerable<string> GetImplicitVisualAttributes(ItemStack stack)
		{
			TextureAttributeHandbook.<GetImplicitVisualAttributes>d__17 <GetImplicitVisualAttributes>d__ = new TextureAttributeHandbook.<GetImplicitVisualAttributes>d__17(-2);
			<GetImplicitVisualAttributes>d__.<>3__stack = stack;
			return <GetImplicitVisualAttributes>d__;
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000F1C4 File Offset: 0x0000D3C4
		[return: Nullable(2)]
		private static AssetLocation VisualGroupCodeForStack(ItemStack stack)
		{
			AssetLocation assetLocation;
			if (stack == null)
			{
				assetLocation = null;
			}
			else
			{
				CollectibleObject collectible = stack.Collectible;
				assetLocation = ((collectible != null) ? collectible.Code : null);
			}
			if (assetLocation == null)
			{
				return null;
			}
			string[] configuredVisualGroupBy = TextureAttributeHandbook.GetConfiguredVisualGroupBy(stack.Collectible);
			int i = 0;
			while (i < configuredVisualGroupBy.Length)
			{
				string wildcard = TextureAttributeHandbook.NormalizeHandbookGroupWildcard(configuredVisualGroupBy[i], stack);
				if (!string.IsNullOrWhiteSpace(wildcard))
				{
					if (!wildcard.Contains(':'))
					{
						return new AssetLocation(stack.Collectible.Code.Domain, wildcard);
					}
					return new AssetLocation(wildcard);
				}
				else
				{
					i++;
				}
			}
			if (TextureAttributeHandbook.IsTailoredGambeson(stack))
			{
				string bodyPart = stack.Collectible.Variant["bodypart"];
				return new AssetLocation(stack.Collectible.Code.Domain, "armor-" + bodyPart + "-tailored-*-linen");
			}
			return null;
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000F28C File Offset: 0x0000D48C
		private static string[] GetConfiguredVisualGroupBy(CollectibleObject collectible)
		{
			JsonObject attributes = collectible.Attributes;
			string[] array;
			if (attributes == null)
			{
				array = null;
			}
			else
			{
				JsonObject jsonObject = attributes["handbook"];
				array = ((jsonObject != null) ? jsonObject["visualGroupBy"].AsArray<string>(null, "game") : null);
			}
			return array ?? Array.Empty<string>();
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000F2CA File Offset: 0x0000D4CA
		private static bool IsQuiversVisualStack(ItemStack stack)
		{
			CollectibleObject collectible = stack.Collectible;
			string text;
			if (collectible == null)
			{
				text = null;
			}
			else
			{
				AssetLocation code = collectible.Code;
				text = ((code != null) ? code.Domain : null);
			}
			return text == "quiversandsheaths" && TextureAttributeHandbook.GetHandbookGroupBy(stack.Collectible).Length != 0;
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000F308 File Offset: 0x0000D508
		private static bool IsTailoredGambeson(ItemStack stack)
		{
			CollectibleObject collectible = stack.Collectible;
			string text;
			if (collectible == null)
			{
				text = null;
			}
			else
			{
				AssetLocation code = collectible.Code;
				text = ((code != null) ? code.Domain : null);
			}
			if (text != "game")
			{
				return false;
			}
			if (!stack.Collectible.Code.Path.StartsWith("armor-", 4))
			{
				return false;
			}
			RelaxedReadOnlyDictionary<string, string> variant = stack.Collectible.Variant;
			string bodyPart;
			string construction;
			string material;
			return variant.TryGetValue("bodypart", ref bodyPart) && !string.IsNullOrWhiteSpace(bodyPart) && variant.TryGetValue("construction", ref construction) && construction.StartsWith("tailored-", 4) && variant.TryGetValue("material", ref material) && material == "linen";
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000F3BC File Offset: 0x0000D5BC
		private static string NarrowLeadingWildcard(string wildcard, ItemStack stack)
		{
			if (wildcard.StartsWith('*'))
			{
				AssetLocation assetLocation;
				if (stack == null)
				{
					assetLocation = null;
				}
				else
				{
					CollectibleObject collectible = stack.Collectible;
					assetLocation = ((collectible != null) ? collectible.Code : null);
				}
				if (!(assetLocation == null))
				{
					int nextWildcard = wildcard.IndexOf('*', 1);
					string literal = (nextWildcard >= 0) ? wildcard.Substring(1, nextWildcard - 1) : wildcard.Substring(1, wildcard.Length - 1);
					if (string.IsNullOrEmpty(literal))
					{
						return wildcard;
					}
					string path = stack.Collectible.Code.Path;
					int literalIndex = path.IndexOf(literal, 4);
					if (literalIndex <= 0)
					{
						return wildcard;
					}
					return path.Substring(0, literalIndex) + wildcard.Substring(1, wildcard.Length - 1);
				}
			}
			return wildcard;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000F46F File Offset: 0x0000D66F
		private static IEnumerable<string> GetPlaceholders(string text)
		{
			TextureAttributeHandbook.<GetPlaceholders>d__23 <GetPlaceholders>d__ = new TextureAttributeHandbook.<GetPlaceholders>d__23(-2);
			<GetPlaceholders>d__.<>3__text = text;
			return <GetPlaceholders>d__;
		}
	}
}
