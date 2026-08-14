using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x0200000C RID: 12
	public class BEFloatingDock : BlockEntityDisplayCase, ITexPositionSource
	{
		// Token: 0x06000084 RID: 132 RVA: 0x0000819C File Offset: 0x0000639C
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			this.capi = (api as ICoreClientAPI);
			if (base.Block.Attributes != null)
			{
				if (base.Block.Attributes["sideShape"].Exists)
				{
					this.sideShape = base.Block.Attributes["sideShape"].AsString(null);
				}
				if (base.Block.Attributes["baseShape"].Exists)
				{
					this.baseShape = base.Block.Attributes["baseShape"].AsString(null);
				}
				if (base.Block.Attributes["renderSides"].Exists)
				{
					this.renderSides = base.Block.Attributes["renderSides"].AsString(null);
				}
			}
			if (EnumAppSideExtensions.IsServer(api.Side))
			{
				this.soundTick = this.RegisterGameTickListener(new Action<float>(this.soundUpdate), 500, 0);
			}
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000082AE File Offset: 0x000064AE
		protected virtual bool shouldPlaySound()
		{
			return BEFloatingDock.rand.NextDouble() < 0.02;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x000082C8 File Offset: 0x000064C8
		public void soundUpdate(float par)
		{
			if (this.playerIsWalking)
			{
				BlockSelection blockSelection = new BlockSelection();
				blockSelection.Position = this.Pos;
				blockSelection.Face = BlockFacing.UP;
				SoundAttributes soundWalkLoc = base.Block.Sounds.Walk;
				this.Api.World.PlaySoundAt(soundWalkLoc, (double)this.Pos.X, (double)this.Pos.Y, (double)this.Pos.Z, 0, null, 1f);
			}
			AssetLocation soundWalkLoc2 = new AssetLocation("game", "sounds/block/woodcreak_" + this.Api.World.Rand.Next(1, 4).ToString());
			if (this.shouldPlaySound() && this.IsFloating())
			{
				this.Api.World.PlaySoundAt(soundWalkLoc2, (double)this.Pos.X, (double)this.Pos.Y, (double)this.Pos.Z, null, (float)this.Api.World.Rand.NextDouble() * 0.5f + 0.7f, 4f, 0.85f);
			}
			this.playerIsWalking = false;
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000083F5 File Offset: 0x000065F5
		public override void OnBlockPlaced(ItemStack byItemStack = null)
		{
			this.currentConnections = "";
			base.OnBlockPlaced(byItemStack);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x00008409 File Offset: 0x00006609
		internal string GetConnections()
		{
			return this.currentConnections;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00008411 File Offset: 0x00006611
		internal void PlayerIsWalking(bool walking)
		{
			this.playerIsWalking = walking;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x0000841C File Offset: 0x0000661C
		internal bool AddConnection(BlockPos pos, BlockFacing facing)
		{
			char face = char.ToLower(facing.ToString().get_Chars(0));
			if (!this.currentConnections.Contains(face))
			{
				ReadOnlySpan<char> readOnlySpan = this.currentConnections;
				char c = face;
				this.currentConnections = readOnlySpan + new ReadOnlySpan<char>(ref c);
				return true;
			}
			return false;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000846C File Offset: 0x0000666C
		internal bool RemoveConnection(BlockPos pos, BlockFacing facing)
		{
			string face = char.ToLower(facing.ToString().get_Chars(0)).ToString();
			if (this.currentConnections.Contains(face))
			{
				this.currentConnections = this.currentConnections.Replace(face, string.Empty);
				return true;
			}
			return false;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000084BC File Offset: 0x000066BC
		public virtual MeshData GenBaseMesh(ICoreClientAPI capi, string shapePath, ITexPositionSource texture)
		{
			ITesselatorAPI tesselator = capi.Tesselator;
			Shape shape = capi.Assets.TryGet(shapePath + ".json", true).ToObject<Shape>(null);
			MeshData mesh;
			tesselator.TesselateShape(shapePath, shape, ref mesh, texture, null, 0, 0, 0, default(int?), null);
			if (this.Api.World.BlockAccessor.GetBlock(this.Pos, 0).LastCodePart(0) == "we")
			{
				mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 0f, 1.5707964f, 0f);
			}
			return mesh;
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00008564 File Offset: 0x00006764
		public virtual MeshData GenMesh(ICoreClientAPI capi, string shapePath, ITexPositionSource texture, char side)
		{
			ITesselatorAPI tesselator = capi.Tesselator;
			Shape shape = capi.Assets.TryGet(shapePath + ".json", true).ToObject<Shape>(null);
			MeshData mesh;
			tesselator.TesselateShape(shapePath, shape, ref mesh, texture, null, 0, 0, 0, default(int?), null);
			if (mesh != null)
			{
				mesh.Translate(0f, 0f, 0f);
				if (side <= 'n')
				{
					if (side != 'e')
					{
						if (side == 'n')
						{
							mesh.Translate(new Vec3f(0.001f, 0.001f, -0.435f));
						}
					}
					else
					{
						mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 0f, 1.5707964f, 0f);
						mesh.Translate(new Vec3f(0.435f, 0.0018f, 0.0018f));
					}
				}
				else if (side != 's')
				{
					if (side == 'w')
					{
						mesh.Rotate(new Vec3f(0.5f, 0.5f, 0.5f), 0f, 1.5707964f, 0f);
						mesh.Translate(new Vec3f(-0.435f, 0.0015f, 0.0015f));
					}
				}
				else
				{
					mesh.Translate(new Vec3f(0.0013f, 0.0013f, 0.435f));
				}
			}
			return mesh;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000086C4 File Offset: 0x000068C4
		public virtual string BuildShapePath(string partialPath)
		{
			if (partialPath == "")
			{
				return "";
			}
			string fullPath = "";
			if (!partialPath.Contains(":"))
			{
				fullPath = base.Block.Code.Domain + ":";
			}
			if (!partialPath.Contains("shapes/"))
			{
				fullPath += "shapes/";
			}
			return fullPath + partialPath;
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00008734 File Offset: 0x00006934
		private bool IsFloating()
		{
			Block belowBlock = this.Api.World.BlockAccessor.GetBlock(this.Pos.DownCopy(1), 2);
			return belowBlock != null && belowBlock.Code.Path.Contains("water-");
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00008784 File Offset: 0x00006984
		public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
		{
			BlockPos posBelow = this.Pos.DownCopy(1);
			this.Api.World.BlockAccessor.GetBlock(posBelow, 2);
			bool floating = this.IsFloating();
			ITexPositionSource texture = tesselator.GetTextureSource(base.Block, 0, false);
			string shapePath = this.BuildShapePath(this.baseShape);
			MeshData mesh = this.GenBaseMesh(this.capi, shapePath, texture);
			if (mesh != null)
			{
				if (floating)
				{
					int waterWave = 201326592;
					for (int vertexNum = 0; vertexNum < mesh.GetVerticesCount(); vertexNum++)
					{
						mesh.Flags[vertexNum] |= waterWave;
					}
				}
				mesher.AddMeshData(mesh, 1);
			}
			if (floating)
			{
				shapePath = this.BuildShapePath(this.sideShape);
				string text = this.renderSides;
				for (int i = 0; i < text.Length; i++)
				{
					char side = text.get_Chars(i);
					mesh = null;
					if (!this.currentConnections.Contains(side) && shapePath != "")
					{
						mesh = this.GenMesh(this.capi, shapePath, texture, side);
					}
					if (mesh != null)
					{
						int waterWave2 = 201326592;
						for (int vertexNum2 = 0; vertexNum2 < mesh.GetVerticesCount(); vertexNum2++)
						{
							mesh.Flags[vertexNum2] |= waterWave2;
						}
						mesher.AddMeshData(mesh, 1);
					}
				}
			}
			return true;
		}

		// Token: 0x06000091 RID: 145 RVA: 0x000088D3 File Offset: 0x00006AD3
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
		{
			base.FromTreeAttributes(tree, worldForResolving);
			this.currentConnections = tree.GetString("currentConnections", null);
		}

		// Token: 0x06000092 RID: 146 RVA: 0x000088EF File Offset: 0x00006AEF
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			tree.SetString("currentConnections", this.currentConnections);
		}

		// Token: 0x0400004A RID: 74
		public ICoreClientAPI capi;

		// Token: 0x0400004B RID: 75
		public ICoreServerAPI sapi;

		// Token: 0x0400004C RID: 76
		private bool playerIsWalking;

		// Token: 0x0400004D RID: 77
		protected static Random rand = new Random();

		// Token: 0x0400004E RID: 78
		private string renderSides = "nsew";

		// Token: 0x0400004F RID: 79
		private string sideShape = "";

		// Token: 0x04000050 RID: 80
		private string baseShape = "";

		// Token: 0x04000051 RID: 81
		private long soundTick;

		// Token: 0x04000052 RID: 82
		private string currentConnections = "";
	}
}
