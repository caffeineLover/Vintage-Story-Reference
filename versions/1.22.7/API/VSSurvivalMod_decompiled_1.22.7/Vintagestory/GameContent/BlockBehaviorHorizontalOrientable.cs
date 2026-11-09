using System;
using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class BlockBehaviorHorizontalOrientable : BlockBehavior, ILookAwarePlacement
{
	[DocumentAsJson("Optional", "north", false)]
	protected string dropBlockFace = "north";

	[DocumentAsJson("Optional", "None", false)]
	protected JsonItemStack drop;

	[DocumentAsJson("Optional", "true", false)]
	protected bool handleDrop = true;

	protected string variantCode = "horizontalorientation";

	public BlockBehaviorHorizontalOrientable(Block block)
		: base(block)
	{
		if (!block.Variant.ContainsKey("horizontalorientation"))
		{
			variantCode = "side";
		}
	}

	public override void Initialize(JsonObject properties)
	{
		base.Initialize(properties);
		if (properties["dropBlockFace"].Exists)
		{
			dropBlockFace = properties["dropBlockFace"].AsString();
		}
		if (properties["drop"].Exists)
		{
			drop = properties["drop"].AsObject<JsonItemStack>(null, block.Code.Domain);
		}
		handleDrop = properties["handleDrop"].AsBool(defaultValue: true);
	}

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		drop?.Resolve(api.World, "HorizontalOrientable drop for " + block.Code);
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
		BlockFacing[] horVer = Block.SuggestedHVOrientation(byPlayer, blockSel);
		AssetLocation blockCode = block.CodeWithVariant(variantCode, horVer[0].Code);
		return byPlayer.Entity.World.BlockAccessor.GetBlock(blockCode) ?? throw new NullReferenceException(string.Concat("Unable to to find a rotated block with code ", blockCode, ", you're maybe missing the side variant group of have a dash in your block code"));
	}

	public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref float dropQuantityMultiplier, ref EnumHandling handled)
	{
		if (!handleDrop)
		{
			return null;
		}
		handled = EnumHandling.PreventDefault;
		if (drop?.ResolvedItemstack != null)
		{
			return new ItemStack[1] { drop?.ResolvedItemstack.Clone() };
		}
		ItemStack stack = new ItemStack(world.BlockAccessor.GetBlock(block.CodeWithVariant(variantCode, dropBlockFace)));
		stack.StackSize = GameMath.RoundRandom(world.Rand, (float)stack.StackSize * dropQuantityMultiplier);
		if (stack.StackSize <= 0)
		{
			return Array.Empty<ItemStack>();
		}
		return new ItemStack[1] { stack };
	}

	public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventDefault;
		if (drop != null)
		{
			return drop?.ResolvedItemstack.Clone();
		}
		return new ItemStack(world.BlockAccessor.GetBlock(block.CodeWithVariant(variantCode, dropBlockFace)));
	}

	public override AssetLocation GetRotatedBlockCode(int angle, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventDefault;
		int rotatedIndex = GameMath.Mod(BlockFacing.FromCode(block.Variant[variantCode]).HorizontalAngleIndex - angle / 90, 4);
		BlockFacing nowFacing = BlockFacing.HORIZONTALS_ANGLEORDER[rotatedIndex];
		return block.CodeWithVariant(variantCode, nowFacing.Code);
	}

	public override AssetLocation GetHorizontallyFlippedBlockCode(EnumAxis axis, ref EnumHandling handling)
	{
		handling = EnumHandling.PreventDefault;
		BlockFacing facing = BlockFacing.FromCode(block.Variant[variantCode]);
		if (facing.Axis == axis)
		{
			return block.CodeWithVariant(variantCode, facing.Opposite.Code);
		}
		return block.Code;
	}
}
