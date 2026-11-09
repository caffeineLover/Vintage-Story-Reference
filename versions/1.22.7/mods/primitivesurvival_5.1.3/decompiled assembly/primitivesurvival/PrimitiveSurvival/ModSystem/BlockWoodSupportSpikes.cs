using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x0200002E RID: 46
	public class BlockWoodSupportSpikes : Block
	{
		// Token: 0x06000230 RID: 560 RVA: 0x0001A3F8 File Offset: 0x000185F8
		public MeshData GenMesh(ICoreClientAPI capi, string shapePath, ITexPositionSource texture, int slot)
		{
			ITesselatorAPI tesselator = capi.Tesselator;
			Shape shape = capi.Assets.TryGet(shapePath + ".json", true).ToObject<Shape>(null);
			MeshData mesh;
			tesselator.TesselateShape(shapePath, shape, ref mesh, texture, new Vec3f(0f, 0f, 0f), 0, 0, 0, default(int?), null);
			if (slot == -1)
			{
				mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), this.Shape.rotateX * 0.017453292f, this.Shape.rotateY * 0.017453292f, this.Shape.rotateZ * 0.017453292f);
			}
			return mesh;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0001A4B0 File Offset: 0x000186B0
		public override void OnEntityCollide(IWorldAccessor world, Entity entity, BlockPos pos, BlockFacing facing, Vec3d collideSpeed, bool isImpact)
		{
			if (entity.Code.Path.StartsWith("butterfly"))
			{
				return;
			}
			base.OnEntityCollide(world, entity, pos, facing, collideSpeed, isImpact);
			if (world.Side == 1)
			{
				world.BlockAccessor.BreakBlock(pos, null, 1f);
			}
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0001A500 File Offset: 0x00018700
		public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
		{
			BEWoodSupportSpikes be = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BEWoodSupportSpikes;
			if (be != null)
			{
				return be.OnInteract(byPlayer);
			}
			return base.OnBlockInteractStart(world, byPlayer, blockSel);
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0001A538 File Offset: 0x00018738
		public override string GetPlacedBlockName(IWorldAccessor world, BlockPos pos)
		{
			BEWoodSupportSpikes be = this.api.World.BlockAccessor.GetBlockEntity(pos) as BEWoodSupportSpikes;
			if (be != null)
			{
				return be.GetBlockName(world, pos);
			}
			return base.GetPlacedBlockName(world, pos);
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0001A578 File Offset: 0x00018778
		public override void OnNeighbourBlockChange(IWorldAccessor world, BlockPos pos, BlockPos neibpos)
		{
			if (world.BlockAccessor.GetBlock(neibpos, 0).BlockId <= 0)
			{
				if (world.BlockAccessor.GetBlock(neibpos.NorthCopy(1), 0).FirstCodePart(0) == "woodsupportspikes")
				{
					world.BlockAccessor.BreakBlock(neibpos.NorthCopy(1), null, 1f);
				}
				if (world.BlockAccessor.GetBlock(neibpos.SouthCopy(1), 0).FirstCodePart(0) == "woodsupportspikes")
				{
					world.BlockAccessor.BreakBlock(neibpos.SouthCopy(1), null, 1f);
				}
				if (world.BlockAccessor.GetBlock(neibpos.EastCopy(1), 0).FirstCodePart(0) == "woodsupportspikes")
				{
					world.BlockAccessor.BreakBlock(neibpos.EastCopy(1), null, 1f);
				}
				if (world.BlockAccessor.GetBlock(neibpos.WestCopy(1), 0).FirstCodePart(0) == "woodsupportspikes")
				{
					world.BlockAccessor.BreakBlock(neibpos.WestCopy(1), null, 1f);
					return;
				}
			}
			else if (world.BlockAccessor.GetBlock(neibpos.DownCopy(1), 0).FirstCodePart(0) == "woodsupportspikes")
			{
				bool breakDontPlace = true;
				Block aboveblock = world.BlockAccessor.GetBlock(pos.UpCopy(1), 0);
				BEWoodSupportSpikes be = this.api.World.BlockAccessor.GetBlockEntity(pos) as BEWoodSupportSpikes;
				if (be != null)
				{
					ItemStack grassStack = be.Inventory[2].Itemstack;
					ItemSlot dirtStack = be.Inventory[3];
					if (((grassStack != null) ? grassStack.Collectible.Code.Path : null) == "drygrass" && dirtStack.Empty && aboveblock.Fertility > 0)
					{
						breakDontPlace = false;
						world.BlockAccessor.SetBlock(0, pos.UpCopy(1));
						ItemStack tempStack = new ItemStack(aboveblock, 1);
						be.Inventory[3].Itemstack = tempStack;
					}
				}
				if (breakDontPlace)
				{
					world.BlockAccessor.BreakBlock(neibpos.DownCopy(1), null, 1f);
					world.BlockAccessor.BreakBlock(neibpos, null, 1f);
				}
			}
		}
	}
}
