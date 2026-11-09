using System;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000020 RID: 32
	public class BlockHandOfTheDead : Block
	{
		// Token: 0x060001B6 RID: 438 RVA: 0x000149B8 File Offset: 0x00012BB8
		public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
		{
			world.BlockAccessor.GetBlock(blockSel.Position, 0);
			string face = blockSel.Face.ToString();
			string newPath;
			Block blockToPlace;
			if (!(face != "up") || !(face != "down"))
			{
				if (this != null)
				{
					BlockPos targetPos = blockSel.DidOffset ? blockSel.Position.AddCopy(blockSel.Face.Opposite) : blockSel.Position;
					double num = byPlayer.Entity.Pos.X - ((double)targetPos.X + blockSel.HitPosition.X);
					double dz = byPlayer.Entity.Pos.Z - ((double)targetPos.Z + blockSel.HitPosition.Z);
					int halfQuarter = Convert.ToInt32((Math.Atan2(num, dz) + 3.141592653589793) / 0.7853981633974483);
					halfQuarter %= 8;
					string facing;
					if (halfQuarter == 4)
					{
						facing = "south";
					}
					else if (halfQuarter == 6)
					{
						facing = "east";
					}
					else if (halfQuarter == 2)
					{
						facing = "west";
					}
					else if (halfQuarter == 7)
					{
						facing = "northeast";
					}
					else if (halfQuarter == 1)
					{
						facing = "northwest";
					}
					else if (halfQuarter == 5)
					{
						facing = "southeast";
					}
					else if (halfQuarter == 3)
					{
						facing = "southwest";
					}
					else
					{
						facing = "north";
					}
					newPath = this.Code.Path;
					newPath = newPath.Replace("north", facing);
					newPath = newPath.Replace("candle-", "candleplaced-");
					blockToPlace = this.api.World.GetBlock(this.CodeWithPath(newPath));
					if (blockToPlace != null)
					{
						world.BlockAccessor.SetBlock(blockToPlace.BlockId, blockSel.Position);
						return true;
					}
				}
				return false;
			}
			newPath = this.Code.Path;
			face = blockSel.Face.Opposite.ToString();
			newPath = newPath.Replace("north", face);
			blockToPlace = this.api.World.GetBlock(this.CodeWithPath(newPath));
			if (blockToPlace != null)
			{
				world.BlockAccessor.SetBlock(blockToPlace.BlockId, blockSel.Position);
				return true;
			}
			return false;
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x00014BCD File Offset: 0x00012DCD
		internal virtual bool HasSolidGround(IBlockAccessor blockAccessor, BlockPos pos)
		{
			return blockAccessor.GetBlock(pos.DownCopy(1), 0).CanAttachBlockAt(blockAccessor, this, pos, BlockFacing.UP, null);
		}
	}
}
