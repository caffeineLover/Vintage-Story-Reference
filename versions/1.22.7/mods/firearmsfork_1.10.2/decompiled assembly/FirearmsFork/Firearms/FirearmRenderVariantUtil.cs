using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CombatOverhaul.RangedSystems;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace Firearms
{
	// Token: 0x02000019 RID: 25
	[NullableContext(1)]
	[Nullable(0)]
	internal static class FirearmRenderVariantUtil
	{
		// Token: 0x060000C7 RID: 199 RVA: 0x00004D34 File Offset: 0x00002F34
		public static bool TryGetBayonetRenderVariant(ItemStack firearm, ItemStack bayonet, out int renderVariant)
		{
			renderVariant = FirearmRenderVariantUtil.GetBayonetRenderVariantCandidate(bayonet);
			if (FirearmRenderVariantUtil.IsValidRenderVariant(firearm, renderVariant))
			{
				return true;
			}
			if (FirearmRenderVariantUtil.TryResolveBayonetRenderVariantFromAlternates(firearm, bayonet, out renderVariant))
			{
				return true;
			}
			renderVariant = 0;
			return false;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00004D5C File Offset: 0x00002F5C
		[NullableContext(2)]
		public static bool SanitizeRenderVariant(ItemStack stack)
		{
			if (((stack != null) ? stack.Collectible : null) == null || !stack.Attributes.HasAttribute("renderVariant"))
			{
				return false;
			}
			int renderVariant = stack.Attributes.GetInt("renderVariant", 0);
			if (FirearmRenderVariantUtil.IsValidRenderVariant(stack, renderVariant))
			{
				return false;
			}
			stack.Attributes.RemoveAttribute("renderVariant");
			return true;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00004DBC File Offset: 0x00002FBC
		[NullableContext(2)]
		public static bool SanitizeRenderVariant(ItemSlot slot)
		{
			ItemStack stack = (slot != null) ? slot.Itemstack : null;
			if (((stack != null) ? stack.Collectible : null) == null || !stack.Attributes.HasAttribute("renderVariant"))
			{
				return false;
			}
			int renderVariant = stack.Attributes.GetInt("renderVariant", 0);
			if (FirearmRenderVariantUtil.IsValidRenderVariant(stack, renderVariant))
			{
				return false;
			}
			if (stack.Item is MusketItem && slot != null)
			{
				ItemInventoryBuffer bayonetInventory = new ItemInventoryBuffer();
				bayonetInventory.Read(slot, "bayonet");
				try
				{
					int repairedRenderVariant;
					if (bayonetInventory.Items.Count > 0 && FirearmRenderVariantUtil.TryGetBayonetRenderVariant(stack, bayonetInventory.Items[0], out repairedRenderVariant))
					{
						stack.Attributes.SetInt("renderVariant", repairedRenderVariant);
						return true;
					}
				}
				finally
				{
					bayonetInventory.Clear();
				}
			}
			stack.Attributes.RemoveAttribute("renderVariant");
			return true;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004EA0 File Offset: 0x000030A0
		public static bool IsValidRenderVariant(ItemStack stack, int renderVariant)
		{
			if (renderVariant < 2)
			{
				return true;
			}
			int alternateIndex = renderVariant - 2;
			if (alternateIndex >= 0)
			{
				Item item = stack.Item;
				int? num;
				if (item == null)
				{
					num = default(int?);
				}
				else
				{
					CompositeShape shape = item.Shape;
					if (shape == null)
					{
						num = default(int?);
					}
					else
					{
						CompositeShape[] alternates = shape.Alternates;
						num = ((alternates != null) ? new int?(alternates.Length) : default(int?));
					}
				}
				int? num2 = num;
				return num2.GetValueOrDefault() > alternateIndex;
			}
			return false;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00004F0C File Offset: 0x0000310C
		private static int GetBayonetRenderVariantCandidate(ItemStack bayonet)
		{
			CollectibleObject collectible = bayonet.Collectible;
			string text;
			if (collectible == null)
			{
				text = null;
			}
			else
			{
				AssetLocation code2 = collectible.Code;
				text = ((code2 != null) ? code2.Path : null);
			}
			string text2;
			if ((text2 = text) == null)
			{
				Item item = bayonet.Item;
				string text3;
				if (item == null)
				{
					text3 = null;
				}
				else
				{
					AssetLocation code3 = item.Code;
					text3 = ((code3 != null) ? code3.Path : null);
				}
				text2 = (text3 ?? "");
			}
			string path = text2;
			CollectibleObject collectible2 = bayonet.Collectible;
			string text4;
			if (collectible2 == null)
			{
				text4 = null;
			}
			else
			{
				AssetLocation code4 = collectible2.Code;
				text4 = ((code4 != null) ? code4.ToString() : null);
			}
			string text5;
			if ((text5 = text4) == null)
			{
				Item item2 = bayonet.Item;
				string text6;
				if (item2 == null)
				{
					text6 = null;
				}
				else
				{
					AssetLocation code5 = item2.Code;
					text6 = ((code5 != null) ? code5.ToString() : null);
				}
				text5 = (text6 ?? path);
			}
			string code = text5;
			CollectibleObject collectible3 = bayonet.Collectible;
			Dictionary<string, int> dictionary;
			if (collectible3 == null)
			{
				dictionary = null;
			}
			else
			{
				JsonObject attributes = collectible3.Attributes;
				dictionary = ((attributes != null) ? attributes["musketRenderVariantByType"].AsObject<Dictionary<string, int>>(null) : null);
			}
			Dictionary<string, int> renderVariantByType = dictionary;
			if (renderVariantByType != null)
			{
				foreach (KeyValuePair<string, int> keyValuePair in renderVariantByType)
				{
					string text7;
					int num;
					keyValuePair.Deconstruct(ref text7, ref num);
					string wildcard = text7;
					int renderVariant = num;
					if (WildcardUtil.Match(wildcard, path) || WildcardUtil.Match(wildcard, code))
					{
						return Math.Max(0, renderVariant);
					}
				}
			}
			if (WildcardUtil.Match("*-plug-plain", path))
			{
				return 2;
			}
			if (WildcardUtil.Match("*-plug-decorated", path))
			{
				return 3;
			}
			if (WildcardUtil.Match("*-socket-plain", path))
			{
				return 4;
			}
			if (WildcardUtil.Match("*-socket-decorated", path))
			{
				return 5;
			}
			return 0;
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00005084 File Offset: 0x00003284
		private static bool TryResolveBayonetRenderVariantFromAlternates(ItemStack firearm, ItemStack bayonet, out int renderVariant)
		{
			FirearmRenderVariantUtil.<>c__DisplayClass7_0 CS$<>8__locals1 = new FirearmRenderVariantUtil.<>c__DisplayClass7_0();
			renderVariant = 0;
			FirearmRenderVariantUtil.<>c__DisplayClass7_0 CS$<>8__locals2 = CS$<>8__locals1;
			CollectibleObject collectible = bayonet.Collectible;
			string text;
			if (collectible == null)
			{
				text = null;
			}
			else
			{
				AssetLocation code = collectible.Code;
				text = ((code != null) ? code.Path : null);
			}
			string bayonetPath;
			if ((bayonetPath = text) == null)
			{
				Item item = bayonet.Item;
				string text2;
				if (item == null)
				{
					text2 = null;
				}
				else
				{
					AssetLocation code2 = item.Code;
					text2 = ((code2 != null) ? code2.Path : null);
				}
				bayonetPath = (text2 ?? "");
			}
			CS$<>8__locals2.bayonetPath = bayonetPath;
			string suffix = Enumerable.FirstOrDefault<string>(FirearmRenderVariantUtil.BayonetVariantSuffixes, (string element) => CS$<>8__locals1.bayonetPath.Contains(element, 5)) ?? "";
			if (suffix == "")
			{
				return false;
			}
			Item item2 = firearm.Item;
			CompositeShape[] array;
			if (item2 == null)
			{
				array = null;
			}
			else
			{
				CompositeShape shape = item2.Shape;
				array = ((shape != null) ? shape.Alternates : null);
			}
			CompositeShape[] alternates = array;
			if (alternates == null || alternates.Length == 0)
			{
				return false;
			}
			for (int index = 0; index < alternates.Length; index++)
			{
				AssetLocation @base = alternates[index].Base;
				string text3;
				if ((text3 = ((@base != null) ? @base.Path : null)) == null)
				{
					AssetLocation base2 = alternates[index].Base;
					text3 = (((base2 != null) ? base2.ToString() : null) ?? "");
				}
				if (text3.Contains(suffix, 5))
				{
					renderVariant = index + 2;
					return true;
				}
			}
			return false;
		}

		// Token: 0x0400006F RID: 111
		private const string BayonetInventoryId = "bayonet";

		// Token: 0x04000070 RID: 112
		private static readonly string[] BayonetVariantSuffixes = new string[]
		{
			"plug-plain",
			"plug-decorated",
			"socket-plain",
			"socket-decorated",
			"plain",
			"decorated"
		};
	}
}
