using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x0200001C RID: 28
	public class BlockFishBasket : Block
	{
		// Token: 0x06000188 RID: 392 RVA: 0x00012EEC File Offset: 0x000110EC
		public MeshData GenMesh(ICoreClientAPI capi, string shapePath, ITexPositionSource texture, int slot, bool alive, ITesselatorAPI tesselator = null)
		{
			tesselator = capi.Tesselator;
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
					y = 0f;
					z = 60f;
				}
				float offY = 0.67f;
				float scale = 1f;
				if (shapePath.Contains("/saltwater/") || shapePath.Contains("/freshwater/"))
				{
					alive = false;
					if (shapePath.Contains("coelacanth") || shapePath.Contains("grouper") || shapePath.Contains("mahi-mahi"))
					{
						offY = 0.62f;
					}
					if (shapePath.Contains("barracuda"))
					{
						offY = 0.62f;
					}
					if (shapePath.Contains("sturgeon"))
					{
						offY = 0.62f;
					}
					if (shapePath.Contains("haddock") || shapePath.Contains("pollock") || shapePath.Contains("gurnard"))
					{
						offY = 0.62f;
					}
					if (shapePath.Contains("herring") || shapePath.Contains("mackerel"))
					{
						offY = 0.8f;
					}
					if (shapePath.Contains("perch"))
					{
						offY = 0.95f;
					}
					if (shapePath.Contains("amberjack") || shapePath.Contains("snapper"))
					{
						offY = 0.62f;
					}
					if (shapePath.Contains("sheatfish") || shapePath.Contains("pike") || shapePath.Contains("pickerel"))
					{
						offY = 0.62f;
					}
					if (shapePath.Contains("catfish") || shapePath.Contains("walleye"))
					{
						offY = 0.8f;
					}
					if (shapePath.Contains("arapaima"))
					{
						offY = 0.6f;
						scale = 0.75f;
					}
				}
				MeshData mesh;
				tesselator.TesselateShape(shapePath, shape, ref mesh, texture, new Vec3f(x, y, z), 0, 0, 0, default(int?), null);
				if (slot == 0)
				{
					mesh.Translate(-0.03f, 0.3f, 0.15f);
				}
				else if (slot == 1)
				{
					if (shapePath.Contains("seashell"))
					{
						mesh.Translate(0.3f, -0.1f, -0.2f);
						mesh.Rotate(new Vec3f(0.5f, 0f, 0.5f), 0f, 0f, 1.0471976f);
					}
					else if (shapePath.Contains("/saltwater/") || shapePath.Contains("/freshwater/"))
					{
						mesh.Translate(0f, 0.85f - offY, 0.15f);
						mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 1.2217305f, 1.3962634f, 4.363323f);
						mesh.Scale(scale, scale, scale);
						if (shapePath.Contains("coelacanth") || shapePath.Contains("grouper") || shapePath.Contains("mahi-mahi"))
						{
							mesh.Scale(new Vec3f(0.5f, 0f, 0.5f), 0.75f, 0.75f, 0.75f);
						}
					}
					else
					{
						mesh.Translate(-0.15f, 0.4f, -0.05f);
						mesh.Rotate(new Vec3f(0.5f, 0f, 0.5f), 1.3962634f, -1.7453293f, 0.17453292f);
					}
				}
				else if (slot == 2)
				{
					if (shapePath.Contains("seashell"))
					{
						mesh.Translate(-0.3f, -0.1f, -0.15f);
						mesh.Rotate(new Vec3f(0.5f, 0f, 0.5f), 0f, 0f, -1.0471976f);
					}
					else if (shapePath.Contains("/saltwater/") || shapePath.Contains("/freshwater/"))
					{
						mesh.Translate(-0.05f, 0.88f - offY, -0.13f);
						mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 1.2217305f, 1.9198622f, 4.276057f);
						mesh.Scale(scale, scale, scale);
						if (shapePath.Contains("coelacanth") || shapePath.Contains("grouper") || shapePath.Contains("mahi-mahi"))
						{
							mesh.Scale(new Vec3f(0.5f, 0f, 0.5f), 0.75f, 0.75f, 0.75f);
						}
						if (shapePath.Contains("arapaima"))
						{
							mesh.Translate(0f, 0f, -0.1f);
						}
					}
					else
					{
						mesh.Translate(-0.2f, 0.45f, 0.05f);
						mesh.Rotate(new Vec3f(0.5f, 0f, 0.5f), 1.2217305f, -1.3962634f, 0f);
					}
				}
				if (alive)
				{
					double flength = 0.75;
					double fheight = 0.37;
					int fishWave = -1912602624;
					for (int vertexNum = 0; vertexNum < mesh.GetVerticesCount(); vertexNum++)
					{
						if ((double)mesh.xyz[3 * vertexNum + 2] < 0.6 - flength + (double)(slot - 1) * 0.05 || (double)mesh.xyz[3 * vertexNum + 1] > fheight + (double)(slot - 1) * 0.05)
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

		// Token: 0x06000189 RID: 393 RVA: 0x00013510 File Offset: 0x00011710
		public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
		{
			world.BlockAccessor.GetBlock(blockSel.Position, 0);
			if (!this.CanPlaceBlock(world, byPlayer, blockSel, ref failureCode))
			{
				return false;
			}
			if (this != null)
			{
				string facing = Block.SuggestedHVOrientation(byPlayer, blockSel)[0].ToString();
				string newPath = this.Code.Path;
				newPath = newPath.Replace("north", facing);
				if (!newPath.Contains("fishbasketinwater"))
				{
					newPath = newPath.Replace("fishbasket", "fishbasketinwater");
				}
				Block blockToPlace = this.api.World.GetBlock(this.CodeWithPath(newPath));
				world.BlockAccessor.SetBlock(blockToPlace.BlockId, blockSel.Position);
				return true;
			}
			return false;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x000135C0 File Offset: 0x000117C0
		public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
		{
			BEFishBasket bedc = world.BlockAccessor.GetBlockEntity(pos) as BEFishBasket;
			if (bedc != null)
			{
				bedc.OnBreak();
			}
			base.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier);
			if (this.FirstCodePart(0) == "fishbasketinwater")
			{
				world.BlockAccessor.GetBlock(pos, 0).OnNeighbourBlockChange(world, pos, pos);
			}
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0001361C File Offset: 0x0001181C
		public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
		{
			BEFishBasket bedc = world.BlockAccessor.GetBlockEntity(blockSel.Position) as BEFishBasket;
			if (bedc != null)
			{
				return bedc.OnInteract(byPlayer, blockSel);
			}
			return base.OnBlockInteractStart(world, byPlayer, blockSel);
		}
	}
}
