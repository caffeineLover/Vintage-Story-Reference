using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class BlockBehaviorRotateablePlaceable(Block block) : StrongBlockBehavior(block)
{
	protected ICoreAPI api;

	protected float intervalRad;

	public float OffsetRad;

	private Dictionary<BlockPos, float> recentPlacementRad = new Dictionary<BlockPos, float>();

	public override void Initialize(JsonObject properties)
	{
		base.Initialize(properties);
		intervalRad = (float)Math.PI / 180f * properties["intervalDeg"].AsFloat(45f);
		OffsetRad = (float)Math.PI / 180f * properties["offsetDeg"].AsFloat();
	}

	public override void OnLoaded(ICoreAPI api)
	{
		this.api = api;
	}

	public override bool DoPartialSelection(IWorldAccessor world, BlockPos pos, ref EnumHandling handled)
	{
		IRotatablePlaceable bem = block.GetInterface<IRotatablePlaceable>(world, pos);
		if (bem != null)
		{
			handled = EnumHandling.PreventDefault;
			return bem.DoPartialSelection();
		}
		return base.DoPartialSelection(world, pos, ref handled);
	}

	public override Cuboidf[] GetCollisionBoxes(IBlockAccessor blockAccessor, BlockPos pos, ref EnumHandling handled)
	{
		IRotatablePlaceable bem = block.GetInterface<IRotatablePlaceable>(api.World, pos);
		if (bem != null)
		{
			handled = EnumHandling.PreventDefault;
			return bem.GetCollisionBoxes();
		}
		return null;
	}

	public override Cuboidf[] GetSelectionBoxes(IBlockAccessor blockAccessor, BlockPos pos, ref EnumHandling handled)
	{
		IRotatablePlaceable bem = block.GetInterface<IRotatablePlaceable>(api.World, pos);
		if (bem != null)
		{
			handled = EnumHandling.PreventDefault;
			return bem.GetSelectionBoxes();
		}
		return null;
	}

	public override Cuboidf[] GetParticleCollisionBoxes(IBlockAccessor blockAccessor, BlockPos pos, ref EnumHandling handled)
	{
		return GetCollisionBoxes(blockAccessor, pos, ref handled);
	}

	public override Cuboidf GetParticleBreakBox(IBlockAccessor blockAccess, BlockPos pos, BlockFacing facing, ref EnumHandling handled)
	{
		return GetCollisionBoxes(blockAccess, pos, ref handled)[0];
	}

	public override void OnBlockPlaced(IWorldAccessor world, BlockPos blockPos, ref EnumHandling handling)
	{
		if (recentPlacementRad.TryGetValue(blockPos, out var angleRad))
		{
			recentPlacementRad.Remove(blockPos);
			block.GetInterface<IRotatablePlaceable>(api.World, blockPos).MeshAngleRad = angleRad;
		}
	}

	public override bool DoPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ItemStack byItemStack, ref EnumHandling handling)
	{
		BlockPos targetPos = (blockSel.DidOffset ? blockSel.Position.AddCopy(blockSel.Face.Opposite) : blockSel.Position);
		double y = byPlayer.Entity.Pos.X - ((double)targetPos.X + blockSel.HitPosition.X);
		double dz = (double)(float)byPlayer.Entity.Pos.Z - ((double)targetPos.Z + blockSel.HitPosition.Z);
		float roundRad = (float)(int)Math.Round((float)Math.Atan2(y, dz) / intervalRad) * intervalRad;
		recentPlacementRad[blockSel.Position] = roundRad;
		return base.DoPlaceBlock(world, byPlayer, blockSel, byItemStack, ref handling);
	}
}
