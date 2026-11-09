using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x0200000F RID: 15
	public class BEIrrigationVessel : BlockEntityLiquidContainer
	{
		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000C0 RID: 192 RVA: 0x0000A587 File Offset: 0x00008787
		// (set) Token: 0x060000C1 RID: 193 RVA: 0x0000A58F File Offset: 0x0000878F
		public int CapacityLitres { get; set; } = 50;

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000C2 RID: 194 RVA: 0x0000A598 File Offset: 0x00008798
		public override string InventoryClassName
		{
			get
			{
				return "irrigationvessel";
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x0000A5A0 File Offset: 0x000087A0
		public BEIrrigationVessel()
		{
			this.inventory = new InventoryGeneric(1, null, null, null);
			this.tmpNeibPos = new BlockPos[8];
			for (int i = 0; i < this.tmpNeibPos.Length; i++)
			{
				this.tmpNeibPos[i] = new BlockPos(0);
			}
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x0000A608 File Offset: 0x00008808
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			this.ownBlock = (base.Block as BlockIrrigationVessel);
			if (api.Side == 2)
			{
				this.currentMesh = this.GenMesh();
				this.MarkDirty(true, null);
				return;
			}
			this.RegisterGameTickListener(new Action<float>(this.IrrigationVesselUpdate), (int)(this.updateMinutes * 40000.0), 0);
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x0000A670 File Offset: 0x00008870
		public void IrrigationVesselUpdate(float par)
		{
			if (this.Api.Side == 2)
			{
				return;
			}
			if (this.inventory[0].Empty && !this.Buried)
			{
				this.Api.World.BlockAccessor.SetBlock(0, this.Pos, 2);
				return;
			}
			if (this.inventory[0].Empty || !this.Buried)
			{
				return;
			}
			if (!this.inventory[0].Itemstack.Collectible.Code.Path.Contains("water"))
			{
				return;
			}
			if (this.inventory[0].Itemstack.Collectible.Code.Path.Contains("saltwater"))
			{
				return;
			}
			string assetCode = "game:water-still-6";
			Block waterBlock = this.Api.World.GetBlock(new AssetLocation(assetCode));
			this.Api.World.BlockAccessor.SetBlock(waterBlock.BlockId, this.Pos, 2);
			int baseX = this.Pos.X;
			int baseY = this.Pos.Y;
			int baseZ = this.Pos.Z;
			int dim = this.Pos.dimension;
			BlockPos[] neibPos = this.tmpNeibPos;
			for (int i = 0; i < neibPos.Length; i++)
			{
				neibPos[i].dimension = dim;
			}
			neibPos[0].Set(baseX + 1, baseY, baseZ);
			neibPos[1].Set(baseX + 1, baseY, baseZ - 1);
			neibPos[2].Set(baseX, baseY, baseZ - 1);
			neibPos[3].Set(baseX - 1, baseY, baseZ - 1);
			neibPos[4].Set(baseX, baseY, baseZ + 1);
			neibPos[5].Set(baseX + 1, baseY, baseZ + 1);
			neibPos[6].Set(baseX - 1, baseY, baseZ);
			neibPos[7].Set(baseX - 1, baseY, baseZ + 1);
			double waterUsed = 0.0;
			foreach (BlockPos neib in neibPos)
			{
				BlockEntityFarmland be = this.Api.World.BlockAccessor.GetBlockEntity(neib) as BlockEntityFarmland;
				if (be != null)
				{
					float topUp = 1f - be.MoistureLevel + 0.2f;
					be.WaterFarmland(topUp, true);
					waterUsed += (double)(topUp * 2.5f);
					TreeAttribute tree = new TreeAttribute();
					be.ToTreeAttributes(tree);
				}
			}
			this.inventory[0].TakeOut((int)waterUsed);
			this.MarkDirty(true, null);
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x0000A900 File Offset: 0x00008B00
		public override void OnBlockBroken(IPlayer forPlayer)
		{
			this.Api.World.BlockAccessor.SetBlock(0, this.Pos, 2);
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x0000A91F File Offset: 0x00008B1F
		public override void OnBlockPlaced(ItemStack byItemStack = null)
		{
			this.Buried = false;
			base.OnBlockPlaced(byItemStack);
			if (this.Api.Side == 2)
			{
				this.currentMesh = this.GenMesh();
				this.MarkDirty(true, null);
			}
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x0000A954 File Offset: 0x00008B54
		internal MeshData GenMesh()
		{
			if (this.ownBlock == null)
			{
				return null;
			}
			MeshData mesh = this.ownBlock.GenMesh(this.Api as ICoreClientAPI, base.GetContent(), this.Pos);
			if (mesh.CustomInts != null)
			{
				for (int i = 0; i < mesh.CustomInts.Count; i++)
				{
					mesh.CustomInts.Values[i] |= 134217728;
					mesh.CustomInts.Values[i] |= 67108864;
				}
			}
			return mesh;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x0000A9E4 File Offset: 0x00008BE4
		public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
		{
			string shapePath = "primitivesurvival:shapes/block/clay/irrigationvessel/irrigationvessel";
			BlockIrrigationVessel block = this.Api.World.BlockAccessor.GetBlock(this.Pos, 0) as BlockIrrigationVessel;
			if (block != null)
			{
				ITexPositionSource tmpTextureSource = tesselator.GetTextureSource(block, 0, false);
				MeshData mesh = block.GenMesh(this.Api as ICoreClientAPI, shapePath, tmpTextureSource);
				mesher.AddMeshData(mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 0f, this.MeshAngle, 0f), 1);
				if (this.Buried)
				{
					shapePath = "primitivesurvival:shapes/block/clay/irrigationvessel/soildry";
					if (this.SoilType != null)
					{
						if (this.SoilType == "")
						{
							this.SoilType = "low";
						}
						AssetLocation assetLoc = new AssetLocation("game:farmland-dry-" + this.SoilType);
						if (assetLoc != null)
						{
							Block tempBlock = this.Api.World.GetBlock(assetLoc);
							shapePath = "game:shapes/" + tempBlock.Shape.Base.Path;
							tmpTextureSource = ((ICoreClientAPI)this.Api).Tesselator.GetTextureSource(tempBlock, 0, false);
						}
					}
					if (!this.inventory.Empty && this.inventory[0].Itemstack.Collectible.Code.Path.Contains("water"))
					{
						shapePath = "primitivesurvival:shapes/block/clay/irrigationvessel/soilwet";
						if (this.SoilType != null)
						{
							if (this.SoilType == "")
							{
								this.SoilType = "low";
							}
							AssetLocation assetLoc2 = new AssetLocation("game:farmland-moist-" + this.SoilType);
							if (assetLoc2 != null)
							{
								Block tempBlock2 = this.Api.World.GetBlock(assetLoc2);
								shapePath = "game:shapes/" + tempBlock2.Shape.Base.Path;
								tmpTextureSource = ((ICoreClientAPI)this.Api).Tesselator.GetTextureSource(tempBlock2, 0, false);
							}
						}
					}
					mesh = block.GenMesh(this.Api as ICoreClientAPI, shapePath, tmpTextureSource);
					mesher.AddMeshData(mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 0f, this.MeshAngle, 0f), 1);
				}
			}
			else
			{
				ITexPositionSource tmpTextureSource = tesselator.GetTextureSource(this.ownBlock, 0, false);
				MeshData mesh = this.ownBlock.GenMesh(this.Api as ICoreClientAPI, shapePath, tmpTextureSource);
				mesher.AddMeshData(mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 0f, this.MeshAngle, 0f), 1);
			}
			if (base.GetContent() != null)
			{
				if (base.GetContent().Block != null)
				{
					ITexPositionSource tmpTextureSource = ((ICoreClientAPI)this.Api).Tesselator.GetTextureSource(base.GetContent().Block, 0, false);
					try
					{
						MeshData mesh = this.ownBlock.GenMesh(this.Api as ICoreClientAPI, base.GetContent(), this.Pos);
						mesher.AddMeshData(mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 0f, this.MeshAngle, 0f), 1);
						return true;
					}
					catch
					{
						return true;
					}
				}
				if (base.GetContent().Item != null)
				{
					ITexPositionSource tmpTextureSource = ((ICoreClientAPI)this.Api).Tesselator.GetTextureSource(base.GetContent().Item, false);
					try
					{
						MeshData mesh = this.ownBlock.GenMesh(this.Api as ICoreClientAPI, base.GetContent(), this.Pos);
						mesher.AddMeshData(mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 0f, this.MeshAngle, 0f), 1);
					}
					catch
					{
					}
				}
			}
			return true;
		}

		// Token: 0x060000CA RID: 202 RVA: 0x0000ADD4 File Offset: 0x00008FD4
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
		{
			base.FromTreeAttributes(tree, worldForResolving);
			this.MeshAngle = tree.GetFloat("meshAngle", this.MeshAngle);
			this.Buried = tree.GetBool("buried", this.Buried);
			this.SoilType = tree.GetString("soiltype", this.SoilType);
			if (this.SoilType == null)
			{
				this.SoilType = "low";
			}
			if (this.Api != null && this.Api.Side == 2)
			{
				this.currentMesh = this.GenMesh();
				this.MarkDirty(true, null);
			}
		}

		// Token: 0x060000CB RID: 203 RVA: 0x0000AE6B File Offset: 0x0000906B
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			tree.SetFloat("meshAngle", this.MeshAngle);
			tree.SetBool("buried", this.Buried);
			tree.SetString("soiltype", this.SoilType);
		}

		// Token: 0x04000071 RID: 113
		private MeshData currentMesh;

		// Token: 0x04000072 RID: 114
		private BlockIrrigationVessel ownBlock;

		// Token: 0x04000073 RID: 115
		public float MeshAngle;

		// Token: 0x04000074 RID: 116
		public bool Buried;

		// Token: 0x04000075 RID: 117
		public string SoilType;

		// Token: 0x04000076 RID: 118
		private readonly double updateMinutes = 1.0;

		// Token: 0x04000077 RID: 119
		private readonly BlockPos[] tmpNeibPos;
	}
}
