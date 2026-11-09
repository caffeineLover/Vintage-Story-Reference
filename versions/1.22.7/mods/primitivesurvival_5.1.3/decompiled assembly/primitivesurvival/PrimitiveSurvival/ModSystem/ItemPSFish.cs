using System;
using PrimitiveSurvival.ModConfig;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x0200004B RID: 75
	public class ItemPSFish : Item
	{
		// Token: 0x060002D5 RID: 725 RVA: 0x00021774 File Offset: 0x0001F974
		public override void OnConsumedByCrafting(ItemSlot[] allInputSlots, ItemSlot stackInSlot, IRecipeBase gridRecipe, IRecipeIngredient fromIngredient, IPlayer byPlayer, int quantity)
		{
			base.OnConsumedByCrafting(allInputSlots, stackInSlot, gridRecipe, fromIngredient, byPlayer, quantity);
			if (ItemPSFish.Rnd.Next(100) < this.eggsPercent)
			{
				Item item;
				if (ItemPSFish.Rnd.Next(2) == 0)
				{
					item = this.api.World.GetItem(new AssetLocation("primitivesurvival:fisheggs-raw-normal"));
				}
				else
				{
					item = this.api.World.GetItem(new AssetLocation("primitivesurvival:fisheggs-raw-ovulated"));
				}
				if (item != null)
				{
					ItemStack outStack = new ItemStack(item, 1);
					this.api.World.SpawnItemEntity(outStack, new Vec3d(byPlayer.Entity.Pos.X + 0.5, byPlayer.Entity.Pos.Y + 0.5, byPlayer.Entity.Pos.Z + 0.5), null);
				}
			}
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00021860 File Offset: 0x0001FA60
		public override void OnGroundIdle(EntityItem entityItem)
		{
			base.OnGroundIdle(entityItem);
			IWorldAccessor world = entityItem.World;
			if (world.Side != 1)
			{
				return;
			}
			if (entityItem.Swimming && world.Rand.NextDouble() < 0.1 && entityItem.Itemstack.Item.LastCodePart(0) == "raw")
			{
				PrimitiveSurvivalSystem.UpdateChunkInDictionary(this.api as ICoreServerAPI, entityItem.Pos.AsBlockPos, -this.repleteRate);
				this.GenerateWaterParticles(entityItem.Pos.AsBlockPos, world);
				entityItem.Die(7, null);
			}
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x000218FC File Offset: 0x0001FAFC
		private void GenerateWaterParticles(BlockPos pos, IWorldAccessor world)
		{
			float num = 16f;
			float maxQuantity = 32f;
			int color = ColorUtil.ToRgba(44, 231, 244, 200);
			Vec3d minPos = new Vec3d();
			Vec3d addPos = new Vec3d();
			Vec3f minVelocity = new Vec3f(0.3f, 0f, 0.3f);
			Vec3f maxVelocity = new Vec3f(2f, 0f, 2f);
			float lifeLength = 0.5f;
			float gravityEffect = 0f;
			float minSize = 0.1f;
			float maxSize = 0.3f;
			SimpleParticleProperties waterParticles = new SimpleParticleProperties(num, maxQuantity, color, minPos, addPos, minVelocity, maxVelocity, lifeLength, gravityEffect, minSize, maxSize, 0);
			waterParticles.MinPos.Set(pos.ToVec3d().AddCopy(0.5, 0.0, 0.5));
			waterParticles.AddPos.Set(new Vec3d(0.5, 0.5, 0.5));
			waterParticles.SizeEvolve = new EvolvingNatFloat(1, 0.1f);
			waterParticles.ShouldDieInAir = false;
			waterParticles.SelfPropelled = false;
			world.SpawnParticles(waterParticles, null);
		}

		// Token: 0x0400012B RID: 299
		private readonly int eggsPercent = ModConfig.Loaded.FishChanceOfEggsPercent;

		// Token: 0x0400012C RID: 300
		private readonly int repleteRate = ModConfig.Loaded.FishChunkDepletionRate;

		// Token: 0x0400012D RID: 301
		private static readonly Random Rnd = new Random();
	}
}
