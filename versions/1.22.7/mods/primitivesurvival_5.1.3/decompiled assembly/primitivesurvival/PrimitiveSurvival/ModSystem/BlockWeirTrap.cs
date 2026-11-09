using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x0200002D RID: 45
	public class BlockWeirTrap : Block
	{
		// Token: 0x0600022B RID: 555 RVA: 0x00019CD8 File Offset: 0x00017ED8
		public MeshData GenMesh(ICoreClientAPI capi, string shapePath, ITexPositionSource texture, int slot, bool alive)
		{
			ITesselatorAPI tesselator = capi.Tesselator;
			IAsset shapeAsset = capi.Assets.TryGet(shapePath + ".json", true);
			if (shapeAsset != null)
			{
				Shape shape = shapeAsset.ToObject<Shape>(null);
				float x = 0f;
				float y = 0f;
				float z = 0f;
				if (shapePath.Contains("/saltwater/") || shapePath.Contains("/freshwater/"))
				{
					x = 0f;
					y = 180f;
					z = -60f;
				}
				float offY = -0.83f;
				float scale = 1f;
				if (shapePath.Contains("/saltwater/") || shapePath.Contains("/freshwater/"))
				{
					if (shapePath.Contains("coelacanth") || shapePath.Contains("grouper") || shapePath.Contains("mahi-mahi"))
					{
						offY = -1.1f;
					}
					if (shapePath.Contains("barracuda"))
					{
						offY = -1f;
					}
					if (shapePath.Contains("sturgeon"))
					{
						offY = -1f;
					}
					if (shapePath.Contains("haddock") || shapePath.Contains("pollock") || shapePath.Contains("gurnard"))
					{
						offY = -0.88f;
					}
					if (shapePath.Contains("herring") || shapePath.Contains("mackerel"))
					{
						offY = -0.7f;
					}
					if (shapePath.Contains("perch"))
					{
						offY = -0.55f;
					}
					if (shapePath.Contains("amberjack") || shapePath.Contains("snapper"))
					{
						offY = -0.88f;
					}
					if (shapePath.Contains("sheatfish") || shapePath.Contains("pike") || shapePath.Contains("pickerel"))
					{
						offY = -1.1f;
					}
					if (shapePath.Contains("catfish") || shapePath.Contains("walleye"))
					{
						offY = -1f;
					}
					if (shapePath.Contains("arapaima"))
					{
						offY = -1.1f;
						scale = 0.8f;
						alive = false;
					}
				}
				MeshData mesh;
				tesselator.TesselateShape(shapePath, shape, ref mesh, texture, new Vec3f(x, y, z), 0, 0, 0, default(int?), null);
				if (slot == 0)
				{
					if (shapePath.Contains("seashell"))
					{
						mesh.Translate(-0.4f, 0.1f, -0.2f);
					}
					else if (shapePath.Contains("/saltwater/") || shapePath.Contains("/freshwater/"))
					{
						mesh.Translate(-0.1f, 1.4f + offY, -0.6f);
						mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 1.2217305f, 1.7453293f, 4.0142574f);
						mesh.Scale(scale, scale, scale);
					}
					else
					{
						mesh.Translate(0.5f, 1.4f, -0.6f);
						mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 1.2217305f, 1.7453293f, 4.0142574f);
					}
				}
				else if (slot == 1)
				{
					if (shapePath.Contains("seashell"))
					{
						mesh.Translate(0.6f, 0.1f, 0f);
					}
					else if (shapePath.Contains("/saltwater/") || shapePath.Contains("/freshwater/"))
					{
						mesh.Translate(-0.1f, 1.3f + offY, 0.7f);
						mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 1.2217305f, 1.3962634f, 4.0142574f);
						mesh.Scale(scale, scale, scale);
					}
					else
					{
						mesh.Translate(0.5f, 1.4f, 0.7f);
						mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 1.2217305f, 1.3962634f, 4.0142574f);
					}
				}
				if (alive)
				{
					double flength = 0.45;
					double fheight = 0.45;
					if (shapePath.Contains("/saltwater/") || shapePath.Contains("/freshwater/"))
					{
						flength = (double)(offY * -1f - 0.4f);
					}
					int fishWave = -1912602624;
					for (int vertexNum = 0; vertexNum < mesh.GetVerticesCount(); vertexNum++)
					{
						if ((double)mesh.xyz[3 * vertexNum + 2] > flength || (double)mesh.xyz[3 * vertexNum + 1] > fheight)
						{
							mesh.Flags[vertexNum] |= fishWave;
						}
						else
						{
							mesh.Flags[vertexNum] |= 6144;
						}
					}
				}
				mesh.Rotate(new Vec3f(0.5f, 0f, 0.5f), 0f, this.Shape.rotateY * 0.017453292f, 0f);
				return mesh;
			}
			return null;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0001A1B0 File Offset: 0x000183B0
		public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
		{
			BEWeirTrap bedc = world.BlockAccessor.GetBlockEntity(pos) as BEWeirTrap;
			if (bedc != null)
			{
				bedc.OnBreak();
			}
			base.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier);
			if (world.BlockAccessor.GetBlock(pos, 2).Code.Path.Contains("saltwater"))
			{
				world.BlockAccessor.SetBlock(world.GetBlock(new AssetLocation("saltwater-still-7")).BlockId, pos);
			}
			else
			{
				world.BlockAccessor.SetBlock(world.GetBlock(new AssetLocation("water-still-7")).BlockId, pos);
			}
			world.BlockAccessor.GetBlock(pos, 0).OnNeighbourBlockChange(world, pos, pos);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0001A260 File Offset: 0x00018460
		public override void OnNeighbourBlockChange(IWorldAccessor world, BlockPos pos, BlockPos neibpos)
		{
			Block neibBlock = world.BlockAccessor.GetBlock(neibpos, 0);
			if (neibBlock != null && (neibBlock.BlockId == 0 || neibBlock.Code.Path.StartsWith("water") || neibBlock.Code.Path.StartsWith("saltwater")))
			{
				foreach (BlockPos neighbor in new BlockPos[]
				{
					pos.EastCopy(1),
					pos.WestCopy(1),
					pos.NorthCopy(1),
					pos.SouthCopy(1)
				})
				{
					Block testBlock = world.BlockAccessor.GetBlock(neighbor, 0);
					if (testBlock.Code.Path.Contains("open") && testBlock.Code.Path.Contains("stake-"))
					{
						world.BlockAccessor.BreakBlock(pos, null, 1f);
						string newPath = testBlock.Code.Path.Replace("open", "").Replace("we", "ew").Replace("sn", "ns");
						testBlock = world.GetBlock(testBlock.CodeWithPath(newPath));
						if (testBlock != null)
						{
							world.BlockAccessor.SetBlock(testBlock.BlockId, neighbor);
						}
					}
				}
			}
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0001A3B4 File Offset: 0x000185B4
		public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
		{
			BEWeirTrap bedc = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BEWeirTrap;
			if (bedc != null)
			{
				return bedc.OnInteract(byPlayer, blockSel);
			}
			return base.OnBlockInteractStart(world, byPlayer, blockSel);
		}
	}
}
