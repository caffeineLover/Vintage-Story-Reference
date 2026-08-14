using System;
using PrimitiveSurvival.ModConfig;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000006 RID: 6
	public class RightClickPickupSpawnWorm : BlockBehavior
	{
		// Token: 0x06000017 RID: 23 RVA: 0x00002898 File Offset: 0x00000A98
		public RightClickPickupSpawnWorm(Block block) : base(block)
		{
		}

		// Token: 0x06000018 RID: 24 RVA: 0x000028A4 File Offset: 0x00000AA4
		public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
		{
			Block block = world.BlockAccessor.GetBlock(blockSel.Position, 0);
			int wormOdds = ModConfig.Loaded.WormFoundPercentRock;
			double coachWhipNum = ModConfig.Loaded.SpawnMultiplierSnakeCoachWhip;
			double pitViperNum = ModConfig.Loaded.SpawnMultiplierSnakeCoachWhip;
			if (!block.Code.Path.Contains("flint") && !block.Code.Path.Contains("stick"))
			{
				wormOdds = ModConfig.Loaded.WormFoundPercentStickFlint;
			}
			if (RightClickPickupSpawnWorm.Rnd.Next(100) < wormOdds && byPlayer.InventoryManager.ActiveHotbarSlot.Empty && world.Side == 1)
			{
				BlockSelection blockSelBelow = blockSel.Clone();
				blockSelBelow.Position.Y--;
				Block blockBelow = world.BlockAccessor.GetBlock(blockSelBelow.Position, 0);
				ClimateCondition conds = world.BlockAccessor.GetClimateAt(blockSelBelow.Position, 1, 0.0);
				if ((blockBelow.Code.Path.Contains("soil") || blockBelow.Code.Path.Contains("forestfloor")) && conds.Temperature > 0f && conds.Temperature < 35f)
				{
					BlockPos pos = blockSel.Position;
					EntityProperties type = world.GetEntityType(new AssetLocation("primitivesurvival:nightcrawler"));
					if (RightClickPickupSpawnWorm.Rnd.Next(5) == 0)
					{
						if (RightClickPickupSpawnWorm.Rnd.Next(2) == 0 && coachWhipNum > 0.0)
						{
							type = world.GetEntityType(new AssetLocation("primitivesurvival:coachwhip"));
						}
						else if (pitViperNum > 0.0)
						{
							type = world.GetEntityType(new AssetLocation("primitivesurvival:pitviper"));
						}
					}
					if (type != null)
					{
						Entity entity = world.ClassRegistry.CreateEntity(type);
						if (entity != null)
						{
							entity.Pos.X = (double)((float)pos.X + 0.5f);
							entity.Pos.Y = (double)((float)pos.Y + 0f);
							entity.Pos.Z = (double)((float)pos.Z + 0.5f);
							entity.Pos.Yaw = (float)RightClickPickupSpawnWorm.Rnd.NextDouble() * 2f * 3.1415927f;
							world.SpawnEntity(entity);
							handling = 2;
						}
					}
				}
			}
			return true;
		}

		// Token: 0x04000009 RID: 9
		protected static readonly Random Rnd = new Random();
	}
}
