using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class BlockBellows : Block
{
	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		BlockFacing.FromCode(Variant["side"]);
		return GetBlockEntity<BlockEntityBellows>(blockSel.Position)?.Interact(byPlayer) ?? false;
	}

	public override void OnEntityCollide(IWorldAccessor world, Entity entity, BlockPos pos, BlockFacing facing, Vec3d collideSpeed, bool isImpact)
	{
		if (world.Rand.NextDouble() < 0.05)
		{
			BlockEntityForge blockEntity = GetBlockEntity<BlockEntityForge>(pos);
			if (blockEntity != null && blockEntity.IsBurning)
			{
				entity.ReceiveDamage(new DamageSource
				{
					Source = EnumDamageSource.Block,
					SourceBlock = this,
					Type = EnumDamageType.Fire,
					SourcePos = pos.ToVec3d()
				}, 0.5f);
			}
		}
		base.OnEntityCollide(world, entity, pos, facing, collideSpeed, isImpact);
	}
}
