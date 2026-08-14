using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000023 RID: 35
	public class BlockMonkeyBridge : Block
	{
		// Token: 0x060001D0 RID: 464 RVA: 0x00015D3C File Offset: 0x00013F3C
		public void BreakAbove(IWorldAccessor world, BlockPos neibpos)
		{
			Block block = world.BlockAccessor.GetBlock(neibpos.UpCopy(1), 1);
			if (block.FirstCodePart(0) == "monkeybridge" && block.FirstCodePart(1) == "null")
			{
				world.BlockAccessor.SetBlock(0, neibpos.UpCopy(1));
			}
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00015D98 File Offset: 0x00013F98
		public override void OnNeighbourBlockChange(IWorldAccessor world, BlockPos pos, BlockPos neibpos)
		{
			Block block = world.BlockAccessor.GetBlock(neibpos, 1);
			Block thisblock = world.BlockAccessor.GetBlock(pos, 1);
			if (block.BlockId <= 0)
			{
				if (thisblock.Code.Path.Contains("monkeybridge-null") && pos.Y == neibpos.Y)
				{
					BlockPos belowpos = neibpos.DownCopy(1);
					if (world.BlockAccessor.GetBlock(belowpos, 1).Code.Path.Contains("monkeybridge-middle"))
					{
						return;
					}
				}
				if (pos.Y != neibpos.Y && !thisblock.Code.Path.Contains("monkeybridge-end"))
				{
					return;
				}
				block = world.BlockAccessor.GetBlock(neibpos.NorthCopy(1), 1);
				if (block.FirstCodePart(0) == "monkeybridge" && block.LastCodePart(0) != "east" && block.LastCodePart(0) != "west")
				{
					float dropQty;
					if (block.FirstCodePart(1) != "null")
					{
						dropQty = 1f;
					}
					else
					{
						dropQty = 0f;
					}
					world.BlockAccessor.BreakBlock(neibpos.NorthCopy(1), null, dropQty);
					this.BreakAbove(world, neibpos.NorthCopy(1));
				}
				block = world.BlockAccessor.GetBlock(neibpos.SouthCopy(1), 1);
				if (block.FirstCodePart(0) == "monkeybridge" && block.LastCodePart(0) != "east" && block.LastCodePart(0) != "west")
				{
					float dropQty;
					if (block.FirstCodePart(1) != "null")
					{
						dropQty = 1f;
					}
					else
					{
						dropQty = 0f;
					}
					world.BlockAccessor.BreakBlock(neibpos.SouthCopy(1), null, dropQty);
					this.BreakAbove(world, neibpos.SouthCopy(1));
				}
				block = world.BlockAccessor.GetBlock(neibpos.EastCopy(1), 1);
				if (block.FirstCodePart(0) == "monkeybridge" && block.LastCodePart(0) != "north" && block.LastCodePart(0) != "south")
				{
					float dropQty;
					if (block.FirstCodePart(1) != "null")
					{
						dropQty = 1f;
					}
					else
					{
						dropQty = 0f;
					}
					world.BlockAccessor.BreakBlock(neibpos.EastCopy(1), null, dropQty);
					this.BreakAbove(world, neibpos.EastCopy(1));
				}
				block = world.BlockAccessor.GetBlock(neibpos.WestCopy(1), 1);
				if (block.FirstCodePart(0) == "monkeybridge" && block.LastCodePart(0) != "north" && block.LastCodePart(0) != "south")
				{
					float dropQty;
					if (block.FirstCodePart(1) != "null")
					{
						dropQty = 1f;
					}
					else
					{
						dropQty = 0f;
					}
					world.BlockAccessor.BreakBlock(neibpos.WestCopy(1), null, dropQty);
					this.BreakAbove(world, neibpos.WestCopy(1));
				}
				block = world.BlockAccessor.GetBlock(neibpos.DownCopy(1), 1);
				if (block.FirstCodePart(0) == "monkeybridge" && thisblock.FirstCodePart(0) == "monkeybridge")
				{
					float dropQty;
					if (block.FirstCodePart(1) != "null")
					{
						dropQty = 1f;
					}
					else
					{
						dropQty = 0f;
					}
					world.BlockAccessor.BreakBlock(neibpos.DownCopy(1), null, dropQty);
					this.BreakAbove(world, neibpos.DownCopy(1));
				}
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00016101 File Offset: 0x00014301
		public override void OnEntityCollide(IWorldAccessor world, Entity entity, BlockPos pos, BlockFacing facing, Vec3d collideSpeed, bool isImpact)
		{
		}
	}
}
