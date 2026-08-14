using System;
using System.Text;
using PrimitiveSurvival.ModConfig;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x0200001E RID: 30
	public class BlockFurrowedLand : Block
	{
		// Token: 0x06000199 RID: 409 RVA: 0x00013C14 File Offset: 0x00011E14
		public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
		{
			BEFurrowedLand be = world.BlockAccessor.GetBlockEntity(selection.Position) as BEFurrowedLand;
			if (be != null && !be.OtherSlot.Empty)
			{
				return ArrayExtensions.Append<WorldInteraction>(new WorldInteraction[]
				{
					new WorldInteraction
					{
						ActionLangCode = "primitivesurvival:blockhelp-furrowedland-removedebris",
						MouseButton = 2,
						HotKeyCode = null
					}
				}, base.GetPlacedBlockInteractionHelp(world, selection, forPlayer));
			}
			return base.GetPlacedBlockInteractionHelp(world, selection, forPlayer);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00013C88 File Offset: 0x00011E88
		public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
		{
			BEFurrowedLand be = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BEFurrowedLand;
			if (be != null)
			{
				bool flag = be.OnInteract(byPlayer, blockSel);
				if (flag && world.Api.Side == 1 && !be.OtherSlot.Empty)
				{
					IServerPlayer serverPlayer = byPlayer as IServerPlayer;
					if (serverPlayer == null)
					{
						return flag;
					}
					serverPlayer.SendIngameError("debris-error", Lang.Get("primitivesurvival:ingameerror-debris", Array.Empty<object>()), Array.Empty<object>());
				}
				return flag;
			}
			return base.OnBlockInteractStart(world, byPlayer, blockSel);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00013D08 File Offset: 0x00011F08
		public BlockPos[] AreaAround(BlockPos pos)
		{
			return new BlockPos[]
			{
				pos.WestCopy(1),
				pos.SouthCopy(1),
				pos.EastCopy(1),
				pos.NorthCopy(1)
			};
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00013D38 File Offset: 0x00011F38
		public bool WaterAbove(BlockPos pos)
		{
			BlockPos upPos = pos.UpCopy(1);
			return this.api.World.BlockAccessor.GetBlock(upPos, 0).Code.Path.Contains("water-");
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00013D7D File Offset: 0x00011F7D
		public bool FullWaterBlock(Block block)
		{
			return block.FirstCodePart(0).Contains("water") && block.LastCodePart(0) == "7";
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00013DA8 File Offset: 0x00011FA8
		public void FillPlacedBlock(IWorldAccessor world, BlockPos pos)
		{
			if (this.WaterAbove(pos))
			{
				Block thisWaterBlock = world.BlockAccessor.GetBlock(pos, 2);
				if (!thisWaterBlock.Code.Path.Contains("ice"))
				{
					string assetCode = "game:" + thisWaterBlock.FirstCodePart(0) + "-still-7";
					Block waterBlock = world.GetBlock(new AssetLocation(assetCode));
					if (waterBlock != null)
					{
						world.BlockAccessor.SetBlock(waterBlock.BlockId, pos, 2);
					}
					world.BlockAccessor.TriggerNeighbourBlockUpdate(pos);
				}
			}
			foreach (BlockPos waterPos in this.AreaAround(pos))
			{
				Block blockChk = this.api.World.BlockAccessor.GetBlock(waterPos, 2);
				if (this.FullWaterBlock(blockChk))
				{
					string assetCode2 = "game:" + blockChk.FirstCodePart(0) + "-still-7";
					Block waterBlock2 = world.GetBlock(new AssetLocation(assetCode2));
					if (waterBlock2 != null)
					{
						world.BlockAccessor.SetBlock(waterBlock2.BlockId, pos, 2);
					}
					world.BlockAccessor.TriggerNeighbourBlockUpdate(waterPos);
				}
			}
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00013EC0 File Offset: 0x000120C0
		public override void OnEntityCollide(IWorldAccessor world, Entity entity, BlockPos pos, BlockFacing facing, Vec3d collideSpeed, bool isImpact)
		{
			if (isImpact && facing.Axis == 1 && entity.Pos.Motion.Y > -0.03 && entity.Pos.Motion.Y < 0.0)
			{
				entity.Pos.Motion.Y *= -2.0;
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00013F31 File Offset: 0x00012131
		public override void OnBlockPlaced(IWorldAccessor world, BlockPos pos, ItemStack byItemStack = null)
		{
			base.OnBlockPlaced(world, pos, byItemStack);
			world.BlockAccessor.TriggerNeighbourBlockUpdate(pos);
			this.FillPlacedBlock(world, pos);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00013F50 File Offset: 0x00012150
		public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
		{
			base.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier);
			world.BlockAccessor.TriggerNeighbourBlockUpdate(pos);
			bool waterFound = false;
			BlockPos[] array = this.AreaAround(pos);
			ArrayExtensions.Append<BlockPos>(array, pos.Copy());
			string blockFound = "water";
			foreach (BlockPos waterPos in array)
			{
				Block blockChk = this.api.World.BlockAccessor.GetBlock(waterPos, 2);
				if (this.FullWaterBlock(blockChk))
				{
					waterFound = true;
					blockFound = blockChk.FirstCodePart(0);
				}
			}
			if (!waterFound)
			{
				world.BlockAccessor.SetBlock(0, pos, 2);
				return;
			}
			string assetCode = "game:" + blockFound + "-still-3";
			Block waterBlock = world.GetBlock(new AssetLocation(assetCode));
			if (waterBlock != null)
			{
				world.BlockAccessor.SetBlock(waterBlock.Id, pos, 0);
			}
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00014020 File Offset: 0x00012220
		public override void OnNeighbourBlockChange(IWorldAccessor world, BlockPos pos, BlockPos neibpos)
		{
			if (pos == neibpos)
			{
				return;
			}
			Block thisWaterBlock = world.BlockAccessor.GetBlock(pos, 2);
			Block neibWaterBlock = world.BlockAccessor.GetBlock(neibpos, 2);
			Block thisBlock = world.BlockAccessor.GetBlock(pos, 0);
			Block neibBlock = world.BlockAccessor.GetBlock(neibpos, 0);
			BEFurrowedLand beBlock = world.BlockAccessor.GetBlockEntity(pos) as BEFurrowedLand;
			if (beBlock != null)
			{
				bool updated;
				if (thisBlock.FirstCodePart(0) == neibBlock.FirstCodePart(0) || this.FullWaterBlock(neibWaterBlock))
				{
					updated = beBlock.AddConnection(pos, neibpos.FacingFrom(pos));
					if (this.FullWaterBlock(neibWaterBlock) && !thisWaterBlock.Code.Path.Contains("ice"))
					{
						string assetCode = "game:" + thisWaterBlock.FirstCodePart(0) + "-still-7";
						Block waterBlock = world.GetBlock(new AssetLocation(assetCode));
						if (waterBlock != null)
						{
							world.BlockAccessor.SetBlock(waterBlock.BlockId, pos, 2);
						}
					}
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
			BEFurrowedLand bebBlock = world.BlockAccessor.GetBlockEntity(neibpos) as BEFurrowedLand;
			if (bebBlock != null)
			{
				bool updated2;
				if (thisBlock.FirstCodePart(0) == neibBlock.FirstCodePart(0) || this.FullWaterBlock(thisWaterBlock))
				{
					updated2 = bebBlock.AddConnection(neibpos, pos.FacingFrom(neibpos));
					if (this.FullWaterBlock(thisWaterBlock) && !neibWaterBlock.Code.Path.Contains("ice"))
					{
						string assetCode2 = "game:" + thisWaterBlock.FirstCodePart(0) + "-still-7";
						Block waterBlock2 = world.GetBlock(new AssetLocation(assetCode2));
						if (waterBlock2 != null)
						{
							world.BlockAccessor.SetBlock(waterBlock2.BlockId, pos, 2);
						}
						world.BlockAccessor.SetBlock(waterBlock2.BlockId, neibpos, 2);
					}
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

		// Token: 0x060001A3 RID: 419 RVA: 0x0001421C File Offset: 0x0001241C
		public override string GetPlacedBlockInfo(IWorldAccessor world, BlockPos pos, IPlayer forPlayer)
		{
			StringBuilder dsc = new StringBuilder();
			if (ModConfig.Loaded.ShowModNameInHud)
			{
				dsc.AppendLine("\n<font color=\"#D8EAA3\"><i>" + Lang.GetMatching("game:tabname-primitive", Array.Empty<object>()) + "</i></font>").AppendLine();
			}
			return dsc.AppendLine().ToString();
		}
	}
}
