using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class BlockBehaviorPillar(Block block) : BlockBehavior(block), ILookAwarePlacement, ICustomChiselMaterialName
{
	[DocumentAsJson("Optional", "False", false)]
	protected bool invertedPlacement;

	[DocumentAsJson("Optional", "rotation", false)]
	protected string rotationVariantCode = "rotation";

	public override void Initialize(JsonObject properties)
	{
		base.Initialize(properties);
		invertedPlacement = properties["invertedPlacement"].AsBool();
		rotationVariantCode = properties["rotationVariantCode"].AsString("rotation");
	}

	public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref EnumHandling handling, ref string failureCode)
	{
		handling = EnumHandling.PreventDefault;
		Block orientedBlock = GetLookAwareBlockVariant(byPlayer, itemstack, blockSel);
		if (orientedBlock.CanPlaceBlock(world, byPlayer, blockSel, ref failureCode))
		{
			orientedBlock.DoPlaceBlock(world, byPlayer, blockSel, itemstack);
			return true;
		}
		return false;
	}

	public Block GetLookAwareBlockVariant(IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel)
	{
		string rotation = null;
		switch (blockSel.Face.Axis)
		{
		case EnumAxis.X:
			rotation = "we";
			break;
		case EnumAxis.Y:
			rotation = "ud";
			break;
		case EnumAxis.Z:
			rotation = "ns";
			break;
		}
		if (invertedPlacement)
		{
			BlockFacing[] horVer = Block.SuggestedHVOrientation(byPlayer, blockSel);
			rotation = ((!blockSel.Face.IsVertical) ? "ud" : ((horVer[0].Axis == EnumAxis.X) ? "we" : "ns"));
		}
		return byPlayer.Entity.World.BlockAccessor.GetBlock(block.CodeWithParts(rotation));
	}

	public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos, ref EnumHandling handling)
	{
		handling = EnumHandling.PreventDefault;
		return new ItemStack(world.GetBlock(block.CodeWithParts("ud")));
	}

	public override AssetLocation GetRotatedBlockCode(int angle, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventDefault;
		if (block.LastCodePart() == "ud")
		{
			return block.Code;
		}
		string[] angles = new string[2] { "ns", "we" };
		if (angle < 0)
		{
			angle += 360;
		}
		int index = angle / 90;
		if (block.LastCodePart() == "we")
		{
			index++;
		}
		return block.CodeWithParts(angles[index % 2]);
	}

	string ICustomChiselMaterialName.GetName(ItemStack itemStack)
	{
		return Lang.Get("chiselmaterialnamewithorientation-" + block.Variant[rotationVariantCode], itemStack.GetName());
	}
}
