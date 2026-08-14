using System;
using System.Collections.Generic;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class BlockFigurehead : BlockMaterialFromAttributes, IAttachableToEntity, IWrenchOrientable
{
	public override string MeshKey => "Figurehead";

	public int RequiresBehindSlots { get; set; }

	public bool IsAttachable(Entity toEntity, ItemStack itemStack)
	{
		return true;
	}

	public void CollectTextures(ItemStack stack, Shape shape, string texturePrefixCode, Dictionary<string, CompositeTexture> intoDict)
	{
		string material = stack.Attributes.GetString("material", "oak");
		foreach (string key in shape.Textures.Keys)
		{
			CompositeTexture ctex = TexturesBMFA[key].Clone();
			ctex.Base.Path = ctex.Base.Path.Replace("{material}", material);
			ctex.Bake(api.Assets);
			intoDict[texturePrefixCode + key] = ctex;
		}
	}

	public string GetCategoryCode(ItemStack stack)
	{
		return Attributes["attachableToEntity"]["categoryCode"].AsString() ?? throw new ArgumentNullException("categoryCode is null");
	}

	public CompositeShape GetAttachedShape(ItemStack stack, string slotCode)
	{
		return Shape;
	}

	public string[] GetDisableElements(ItemStack stack)
	{
		return Array.Empty<string>();
	}

	public string[] GetKeepElements(ItemStack stack)
	{
		return Array.Empty<string>();
	}

	public string GetTexturePrefixCode(ItemStack stack)
	{
		return string.Empty;
	}

	public override void GetHeldItemInfo(ItemSlot inSlot, StringBuilder dsc, IWorldAccessor world, bool withDebugInfo)
	{
		base.GetHeldItemInfo(inSlot, dsc, world, withDebugInfo);
		string wood = inSlot.Itemstack.Attributes.GetString("material", "oak");
		dsc.AppendLine(Lang.Get("Material: {0}", Lang.Get("material-" + wood)));
	}

	public override string GetHeldItemName(ItemStack itemStack)
	{
		string material = itemStack.Attributes.GetString("material", "oak");
		return Lang.GetMatching("block-" + Code.Path + "-" + material, Lang.Get("material-" + material));
	}

	public override void GetDecal(IWorldAccessor world, BlockPos pos, ITexPositionSource decalTexSource, ref MeshData decalModelData, ref MeshData blockModelData)
	{
		BEBehaviorMaterialFromAttributes beb = GetBEBehavior<BEBehaviorMaterialFromAttributes>(pos);
		if (beb != null && beb.Material != null)
		{
			float[] mat = Matrixf.Create().Translate(0.5f, 0.5f, 0.5f).RotateY(beb.MeshAngleY)
				.Translate(-0.5f, -0.5f, -0.5f)
				.Values;
			blockModelData = GetOrCreateMesh(beb.Material).Clone().MatrixTransform(mat);
			decalModelData = GetOrCreateMesh(beb.Material, decalTexSource).Clone().MatrixTransform(mat);
		}
		else
		{
			base.GetDecal(world, pos, decalTexSource, ref decalModelData, ref blockModelData);
		}
	}

	public void Rotate(EntityAgent byEntity, BlockSelection blockSel, int dir)
	{
		(GetBlockEntity<BlockEntityGeneric>(blockSel.Position)?.GetBehavior<BEBehaviorMaterialFromAttributes>())?.Rotate(byEntity, blockSel, dir);
	}
}
