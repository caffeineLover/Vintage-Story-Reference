using System;
using System.Linq;
using System.Text;
using PrimitiveSurvival.ModConfig;
using Vintagestory.API.Client.Tesselation;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x0200001D RID: 29
	public class BlockFloatingDock : Block, IDrawYAdjustable
	{
		// Token: 0x0600018D RID: 397 RVA: 0x0001365D File Offset: 0x0001185D
		public override void OnLoaded(ICoreAPI api)
		{
			base.OnLoaded(api);
		}

		// Token: 0x0600018E RID: 398 RVA: 0x00013668 File Offset: 0x00011868
		public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
		{
			BlockFacing[] horVer = Block.SuggestedHVOrientation(byPlayer, blockSel);
			string code = "ns";
			if (horVer[0].Index == 1 || horVer[0].Index == 3)
			{
				code = "we";
			}
			if (this.IsWater(world.BlockAccessor, blockSel.Position.UpCopy(1)))
			{
				blockSel = blockSel.Clone();
				blockSel.Position = blockSel.Position.Up(1);
				bool flag = base.TryPlaceBlock(world, byPlayer, itemstack, blockSel, ref failureCode);
				if (flag)
				{
					Block block = this.api.World.BlockAccessor.GetBlock(blockSel.Position, 0);
					string newPath = block.Code.Path;
					newPath = newPath.Replace("ns", code);
					block = this.api.World.GetBlock(block.CodeWithPath(newPath));
					this.api.World.BlockAccessor.SetBlock(block.BlockId, blockSel.Position);
				}
				return flag;
			}
			if (blockSel.Face.IsHorizontal)
			{
				return false;
			}
			if (this.api.World.BlockAccessor.GetBlock(blockSel.Position, 0).Code.Path.Contains("water") && !this.IsWater(this.api.World.BlockAccessor, blockSel.Position))
			{
				return false;
			}
			Block testBlock2 = this.api.World.BlockAccessor.GetBlock(blockSel.Position.DownCopy(1), 0);
			if (testBlock2.Code.FirstCodePart() == "floatingdock" || testBlock2.Code.FirstCodePart() == "raft")
			{
				return false;
			}
			bool flag2 = base.TryPlaceBlock(world, byPlayer, itemstack, blockSel, ref failureCode);
			if (flag2)
			{
				Block block2 = this.api.World.BlockAccessor.GetBlock(blockSel.Position, 0);
				string newPath2 = block2.Code.Path;
				newPath2 = newPath2.Replace("ns", code);
				block2 = this.api.World.GetBlock(block2.CodeWithPath(newPath2));
				this.api.World.BlockAccessor.SetBlock(block2.BlockId, blockSel.Position);
			}
			return flag2;
		}

		// Token: 0x0600018F RID: 399 RVA: 0x000138A3 File Offset: 0x00011AA3
		public override void OnBlockPlaced(IWorldAccessor world, BlockPos pos, ItemStack byItemStack = null)
		{
			base.OnBlockPlaced(world, pos, byItemStack);
			world.BlockAccessor.TriggerNeighbourBlockUpdate(pos);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x000138BA File Offset: 0x00011ABA
		public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
		{
			base.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier);
			world.BlockAccessor.TriggerNeighbourBlockUpdate(pos);
		}

		// Token: 0x06000191 RID: 401 RVA: 0x000138D4 File Offset: 0x00011AD4
		private bool IsWater(IBlockAccessor blockAccessor, BlockPos pos)
		{
			Block waterblock = blockAccessor.GetBlock(pos.DownCopy(1), 2);
			Block upblock = blockAccessor.GetBlock(pos, 2);
			return StringUtil.ToInt(waterblock.LastCodePart(0), 0) > 4 && (waterblock.IsLiquid() && waterblock.LiquidCode.Contains("water")) && upblock.Id == 0;
		}

		// Token: 0x06000192 RID: 402 RVA: 0x00013930 File Offset: 0x00011B30
		public float AdjustYPosition(BlockPos pos, Block[] chunkExtBlocks, int extIndex3d)
		{
			Cuboidf[] boxes = chunkExtBlocks[extIndex3d + TileSideEnum.MoveIndex[5]].CollisionBoxes;
			float boxHeight = 0f;
			if (boxes != null && boxes.Length != 0)
			{
				boxHeight = Enumerable.Max<Cuboidf>(boxes, (Cuboidf c) => c.Y2);
			}
			this.adjHeight = ((boxHeight < 0.8f) ? -0.19f : 0f);
			return this.adjHeight;
		}

		// Token: 0x06000193 RID: 403 RVA: 0x000139A4 File Offset: 0x00011BA4
		public override void OnEntityCollide(IWorldAccessor world, Entity entity, BlockPos pos, BlockFacing facing, Vec3d collideSpeed, bool isImpact)
		{
			base.OnEntityCollide(world, entity, pos, facing, collideSpeed, isImpact);
			if (entity is EntityPlayer)
			{
				EntityAgent player = entity as EntityAgent;
				if (player is EntityPlayer)
				{
					double num = Math.Abs(player.Pos.Motion.X);
					double msY = Math.Abs(player.Pos.Motion.Y);
					double msZ = Math.Abs(player.Pos.Motion.Z);
					double msMax = Math.Max(num, Math.Max(msY, msZ));
					BEFloatingDock be = world.BlockAccessor.GetBlockEntity(pos) as BEFloatingDock;
					if (be != null && msMax >= 0.03 && player.Pos.Motion.Y <= 0.0)
					{
						be.PlayerIsWalking(true);
					}
				}
			}
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00013A71 File Offset: 0x00011C71
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
		{
			handling = 4;
			base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handling);
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00013A88 File Offset: 0x00011C88
		public override void OnNeighbourBlockChange(IWorldAccessor world, BlockPos pos, BlockPos neibpos)
		{
			if (pos == neibpos)
			{
				return;
			}
			world.BlockAccessor.GetBlock(pos, 2);
			world.BlockAccessor.GetBlock(neibpos, 2);
			Block thisBlock = world.BlockAccessor.GetBlock(pos, 0);
			Block neibBlock = world.BlockAccessor.GetBlock(neibpos, 0);
			BEFloatingDock beBlock = world.BlockAccessor.GetBlockEntity(pos) as BEFloatingDock;
			if (beBlock != null)
			{
				bool updated;
				if (thisBlock.FirstCodePart(0) == neibBlock.FirstCodePart(0))
				{
					updated = beBlock.AddConnection(pos, neibpos.FacingFrom(pos));
				}
				else
				{
					updated = beBlock.RemoveConnection(pos, neibpos.FacingFrom(pos));
				}
				if (updated)
				{
					beBlock.MarkDirty(true, null);
				}
			}
			BEFloatingDock bebBlock = world.BlockAccessor.GetBlockEntity(neibpos) as BEFloatingDock;
			if (bebBlock != null)
			{
				bool updated2;
				if (thisBlock.FirstCodePart(0) == neibBlock.FirstCodePart(0))
				{
					updated2 = bebBlock.AddConnection(neibpos, pos.FacingFrom(neibpos));
				}
				else
				{
					updated2 = bebBlock.RemoveConnection(neibpos, pos.FacingFrom(neibpos));
				}
				if (updated2)
				{
					bebBlock.MarkDirty(true, null);
				}
			}
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00013B8C File Offset: 0x00011D8C
		public override string GetPlacedBlockInfo(IWorldAccessor world, BlockPos pos, IPlayer forPlayer)
		{
			StringBuilder dsc = new StringBuilder();
			world.BlockAccessor.GetBlock(pos, 0);
			dsc.AppendLine(Lang.GetMatching("primitivesurvival:blockdesc-floatingdock-*", Array.Empty<object>()));
			if (ModConfig.Loaded.ShowModNameInHud)
			{
				dsc.AppendLine("\n<font color=\"#D8EAA3\"><i>" + Lang.GetMatching("game:tabname-primitive", Array.Empty<object>()) + "</i></font>").AppendLine();
			}
			return dsc.ToString();
		}

		// Token: 0x040000C5 RID: 197
		private static Random rand = new Random();

		// Token: 0x040000C6 RID: 198
		private float adjHeight;
	}
}
