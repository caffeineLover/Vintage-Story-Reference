using System;
using PrimitiveSurvival.ModConfig;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000044 RID: 68
	public class ItemFishEggs : Item
	{
		// Token: 0x060002AD RID: 685 RVA: 0x0001F480 File Offset: 0x0001D680
		public override void OnGroundIdle(EntityItem entityItem)
		{
			base.OnGroundIdle(entityItem);
			IWorldAccessor world = entityItem.World;
			if (world.Side != 1)
			{
				return;
			}
			if (entityItem.Swimming && world.Rand.NextDouble() < 0.01 && entityItem.Itemstack.Item.LastCodePart(0) == "ovulated")
			{
				PrimitiveSurvivalSystem.UpdateChunkInDictionary(this.api as ICoreServerAPI, entityItem.Pos.AsBlockPos, -ModConfig.Loaded.FishEggsChunkRepletionRate);
				this.GenerateWaterParticles(entityItem.Pos.AsBlockPos, world);
				entityItem.Die(7, null);
			}
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0001F520 File Offset: 0x0001D720
		private void GenerateWaterParticles(BlockPos pos, IWorldAccessor world)
		{
			float num = 6f;
			float maxQuantity = 12f;
			int color = ColorUtil.ToRgba(44, 255, 196, 170);
			Vec3d minPos = new Vec3d();
			Vec3d addPos = new Vec3d();
			Vec3f minVelocity = new Vec3f(0f, 0f, 0f);
			Vec3f maxVelocity = new Vec3f(0.3f, 0.3f, 0.3f);
			float lifeLength = 2f;
			float gravityEffect = 0.03f;
			float minSize = 0.2f;
			float maxSize = 0.2f;
			SimpleParticleProperties waterParticles = new SimpleParticleProperties(num, maxQuantity, color, minPos, addPos, minVelocity, maxVelocity, lifeLength, gravityEffect, minSize, maxSize, 0);
			waterParticles.MinPos.Set(pos.ToVec3d().AddCopy(0.5, 0.0, 0.5));
			waterParticles.SizeEvolve = new EvolvingNatFloat(0, 0.3f);
			waterParticles.ShouldDieInAir = false;
			waterParticles.SelfPropelled = true;
			world.SpawnParticles(waterParticles, null);
		}
	}
}
