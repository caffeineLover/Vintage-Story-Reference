using System;
using PrimitiveSurvival.ModConfig;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000029 RID: 41
	public class BlockSpikeTrap : Block
	{
		// Token: 0x060001F9 RID: 505 RVA: 0x00017D00 File Offset: 0x00015F00
		public override void OnEntityInside(IWorldAccessor world, Entity entity, BlockPos pos)
		{
			base.OnEntityInside(world, entity, pos);
			if (entity.Alive && entity.Class != "entitynightcrawler" && entity.Class != "EntityItem")
			{
				float motion = (float)Math.Abs(entity.Pos.Motion.X + entity.Pos.Motion.Y + entity.Pos.Motion.Z);
				if (motion > 0f)
				{
					entity.ReceiveDamage(new DamageSource
					{
						Source = 0,
						SourceBlock = this,
						Type = 4,
						SourcePos = pos.ToVec3d()
					}, motion + 1f);
				}
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00017DC0 File Offset: 0x00015FC0
		public override void OnEntityCollide(IWorldAccessor world, Entity entity, BlockPos pos, BlockFacing facing, Vec3d collideSpeed, bool isImpact)
		{
			if (world.Side == 1 && isImpact && facing.Axis == 1)
			{
				base.OnEntityCollide(world, entity, pos, facing, collideSpeed, isImpact);
				if (entity.Alive)
				{
					double fallIntoDamageMul = (double)ModConfig.Loaded.FallDamageMultiplierMetalSpikes;
					if (world.BlockAccessor.GetBlock(pos, 0).Code.Path.Contains("woodspikes"))
					{
						fallIntoDamageMul = (double)ModConfig.Loaded.FallDamageMultiplierWoodSpikes;
					}
					float dmg = (float)Math.Abs(collideSpeed.Y * fallIntoDamageMul);
					entity.ReceiveDamage(new DamageSource
					{
						Source = 0,
						SourceBlock = this,
						Type = 4,
						SourcePos = pos.ToVec3d()
					}, dmg);
				}
			}
		}
	}
}
