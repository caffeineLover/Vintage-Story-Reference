using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000022 RID: 34
	public class BlockLimbTrotLineLure : Block
	{
		// Token: 0x060001C9 RID: 457 RVA: 0x000154AC File Offset: 0x000136AC
		public MeshData GenMesh(ICoreClientAPI capi, string shapePath, ITexPositionSource texture, bool alive)
		{
			ITesselatorAPI tesselator = capi.Tesselator;
			IAsset shapeAsset = capi.Assets.TryGet(shapePath + ".json", true);
			if (shapeAsset != null)
			{
				Shape shape = shapeAsset.ToObject<Shape>(null);
				float x = this.Shape.rotateX;
				float y = this.Shape.rotateY;
				float z = this.Shape.rotateZ;
				if (shapePath.Contains("/saltwater/") || shapePath.Contains("/freshwater/"))
				{
					x = 0f;
					y = 180f + this.Shape.rotateY;
					z = 0f;
					if (y == 270f)
					{
						x = 60f;
					}
					else
					{
						z = -60f;
					}
				}
				MeshData mesh;
				tesselator.TesselateShape(shapePath, shape, ref mesh, texture, new Vec3f(x, y, z), 0, 0, 0, default(int?), null);
				float offY = -0.83f;
				float offL = 0.47f;
				if (shapePath.Contains("/saltwater/") || shapePath.Contains("/freshwater/"))
				{
					if (shapePath.Contains("coelacanth") || shapePath.Contains("grouper") || shapePath.Contains("mahi-mahi"))
					{
						offY = -1.1f;
						offL = 0.56f;
					}
					if (shapePath.Contains("barracuda"))
					{
						offY = -1f;
						offL = 0.6f;
					}
					if (shapePath.Contains("sturgeon"))
					{
						offY = -1f;
						offL = 0.66f;
					}
					if (shapePath.Contains("haddock") || shapePath.Contains("pollock") || shapePath.Contains("gurnard"))
					{
						offY = -0.88f;
						offL = 0.5f;
					}
					if (shapePath.Contains("herring") || shapePath.Contains("mackerel"))
					{
						offY = -0.7f;
						offL = 0.45f;
					}
					if (shapePath.Contains("perch-pacific"))
					{
						offY = -0.55f;
						offL = 0.375f;
					}
					if (shapePath.Contains("amberjack") || shapePath.Contains("snapper"))
					{
						offY = -0.88f;
						offL = 0.5f;
					}
					if (shapePath.Contains("arapaima"))
					{
						offY = -1.2f;
						offL = 0.56f;
						float scale = 0.8f;
						mesh.Scale(scale, scale, scale);
					}
					if (shapePath.Contains("pike"))
					{
						offY = -1f;
						offL = 0.59f;
					}
					if (shapePath.Contains("sheatfish"))
					{
						offY = -0.98f;
						offL = 0.5f;
					}
					if (shapePath.Contains("walleye") || shapePath.Contains("pickerel"))
					{
						offY = -0.95f;
						offL = 0.55f;
					}
					if (shapePath.Contains("trout") || shapePath.Contains("catfish") || shapePath.Contains("bass") || shapePath.Contains("carp") || shapePath.Contains("salmon"))
					{
						offY = -0.88f;
						offL = 0.5f;
					}
					if (shapePath.Contains("piranha"))
					{
						offY = -0.7f;
						offL = 0.45f;
					}
					if (shapePath.Contains("perch"))
					{
						offY = -0.55f;
						offL = 0.375f;
					}
					if (shapePath.Contains("crappie") || shapePath.Contains("tilapia"))
					{
						offY = -0.55f;
						offL = 0.375f;
					}
					if (shapePath.Contains("alewife") || shapePath.Contains("chub"))
					{
						offY = -0.55f;
						offL = 0.375f;
					}
					if (y == 270f)
					{
						mesh.Translate(-0.06f, offY, offL);
					}
					else if (y == 450f)
					{
						mesh.Translate(0.06f, offY, -offL);
					}
					else if (y == 180f)
					{
						mesh.Translate(-offL, offY, -0.06f);
					}
					else if (y == 360f)
					{
						mesh.Translate(offL, offY, 0.06f);
					}
				}
				if (shapePath.Contains("lure"))
				{
					int rando = BlockLimbTrotLineLure.Rnd.Next(10);
					if (rando == 0)
					{
						mesh.Rotate(new Vec3f(0.5f, 0f, 0.5f), 0f, 0f, 0.08726646f);
					}
					else if (rando == 1)
					{
						mesh.Rotate(new Vec3f(0.5f, 0f, 0.5f), 0f, 0f, 6.1959186f);
					}
				}
				if (alive)
				{
					double flength = 0.7;
					if (shapePath.Contains("/saltwater/") || shapePath.Contains("/freshwater/"))
					{
						flength = (double)(offY * -1f - 0.2f);
					}
					int fishWave = -1912602624;
					for (int vertexNum = 0; vertexNum < mesh.GetVerticesCount(); vertexNum++)
					{
						if ((double)mesh.xyz[3 * vertexNum + 1] < -0.2 - flength)
						{
							mesh.Flags[vertexNum] |= fishWave;
						}
						else
						{
							mesh.Flags[vertexNum] |= 6144;
						}
					}
				}
				else
				{
					for (int vertexNum2 = 0; vertexNum2 < mesh.GetVerticesCount(); vertexNum2++)
					{
						mesh.Flags[vertexNum2] |= 6144;
					}
				}
				if ((this.Shape.rotateY == 0f || this.Shape.rotateY == 90f) && !shapePath.Contains("-end") && !shapePath.Contains("-middle"))
				{
					mesh.Rotate(new Vec3f(0.5f, 0f, 0.5f), 0f, 3.1415927f, 0f);
				}
				return mesh;
			}
			return null;
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00015A4C File Offset: 0x00013C4C
		public override void OnNeighbourBlockChange(IWorldAccessor world, BlockPos pos, BlockPos neibpos)
		{
			Block block = world.BlockAccessor.GetBlock(neibpos, 0);
			if (block.BlockId <= 0)
			{
				block = world.BlockAccessor.GetBlock(neibpos.NorthCopy(1), 0);
				if (block.FirstCodePart(0) == "limbtrotlinelure" && block.LastCodePart(0) != "east" && block.LastCodePart(0) != "west")
				{
					world.BlockAccessor.BreakBlock(neibpos.NorthCopy(1), null, 1f);
				}
				block = world.BlockAccessor.GetBlock(neibpos.SouthCopy(1), 0);
				if (block.FirstCodePart(0) == "limbtrotlinelure" && block.LastCodePart(0) != "east" && block.LastCodePart(0) != "west")
				{
					world.BlockAccessor.BreakBlock(neibpos.SouthCopy(1), null, 1f);
				}
				block = world.BlockAccessor.GetBlock(neibpos.EastCopy(1), 0);
				if (block.FirstCodePart(0) == "limbtrotlinelure" && block.LastCodePart(0) != "north" && block.LastCodePart(0) != "south")
				{
					world.BlockAccessor.BreakBlock(neibpos.EastCopy(1), null, 1f);
				}
				block = world.BlockAccessor.GetBlock(neibpos.WestCopy(1), 0);
				if (block.FirstCodePart(0) == "limbtrotlinelure" && block.LastCodePart(0) != "north" && block.LastCodePart(0) != "south")
				{
					world.BlockAccessor.BreakBlock(neibpos.WestCopy(1), null, 1f);
				}
			}
		}

		// Token: 0x060001CB RID: 459 RVA: 0x00015C08 File Offset: 0x00013E08
		public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
		{
			BELimbTrotLineLure bedc = world.BlockAccessor.GetBlockEntity(pos) as BELimbTrotLineLure;
			if (bedc != null)
			{
				bedc.OnBreak();
			}
			base.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x00015C3C File Offset: 0x00013E3C
		public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
		{
			blockSel = blockSel.Clone();
			Block block = this.api.World.BlockAccessor.GetBlock(blockSel.Position, 0);
			block = this.api.World.GetBlock(block.CodeWithPath(block.Code.Path));
			this.api.World.BlockAccessor.SetBlock(block.BlockId, blockSel.Position);
			return false;
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00015CB8 File Offset: 0x00013EB8
		public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
		{
			BELimbTrotLineLure bedc = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BELimbTrotLineLure;
			if (bedc != null)
			{
				return bedc.OnInteract(byPlayer, blockSel);
			}
			return base.OnBlockInteractStart(world, byPlayer, blockSel);
		}

		// Token: 0x040000D1 RID: 209
		private static readonly Random Rnd = new Random();
	}
}
