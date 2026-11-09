using System.Collections.Generic;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class AttributeAttachableToEntity : IAttachableToEntity
{
	[DocumentAsJson("Required", "", false)]
	public string CategoryCode { get; set; }

	[DocumentAsJson("Recommended", "None", false)]
	public CompositeShape AttachedShape { get; set; }

	[DocumentAsJson("Optional", "None", false)]
	public string[] DisableElements { get; set; }

	[DocumentAsJson("Optional", "None", false)]
	public string[] KeepElements { get; set; }

	[DocumentAsJson("Recommended", "None", false)]
	public string TexturePrefixCode { get; set; }

	[DocumentAsJson("Recommended", "None", false)]
	public Vintagestory.API.Datastructures.OrderedDictionary<string, CompositeShape> AttachedShapeBySlotCode { get; set; }

	[DocumentAsJson("Optional", "0", false)]
	public int RequiresBehindSlots { get; set; }

	public string GetTexturePrefixCode(ItemStack stack)
	{
		return TexturePrefixCode;
	}

	public void CollectTextures(ItemStack stack, Shape gearShape, string texturePrefixCode, Dictionary<string, CompositeTexture> intoDict)
	{
		IAttachableToEntity.CollectTexturesFromCollectible(stack, texturePrefixCode, gearShape, intoDict);
	}

	public CompositeShape GetAttachedShape(ItemStack stack, string slotCode)
	{
		if (AttachedShape != null)
		{
			return AttachedShape;
		}
		if (AttachedShapeBySlotCode != null)
		{
			foreach (KeyValuePair<string, CompositeShape> val in AttachedShapeBySlotCode)
			{
				if (WildcardUtil.Match(val.Key, slotCode))
				{
					return val.Value;
				}
			}
		}
		if (stack.Class != EnumItemClass.Item)
		{
			return stack.Block.Shape;
		}
		return stack.Item.Shape;
	}

	public string GetCategoryCode(ItemStack stack)
	{
		return CategoryCode;
	}

	public string[] GetDisableElements(ItemStack stack)
	{
		return DisableElements;
	}

	public string[] GetKeepElements(ItemStack stack)
	{
		return KeepElements;
	}

	public bool IsAttachable(Entity toEntity, ItemStack itemStack)
	{
		return true;
	}
}
