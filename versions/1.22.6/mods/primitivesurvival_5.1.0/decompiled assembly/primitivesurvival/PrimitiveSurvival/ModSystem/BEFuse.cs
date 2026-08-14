using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x0200000E RID: 14
	public class BEFuse : BlockEntity
	{
		// Token: 0x060000A9 RID: 169 RVA: 0x000097C4 File Offset: 0x000079C4
		static BEFuse()
		{
			BEFuse.smallSparks.AddPos.Set(0.0, -0.5, 0.0);
			BEFuse.smallSparks.SizeEvolve = new EvolvingNatFloat(1, -0.05f);
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000AA RID: 170 RVA: 0x0000988C File Offset: 0x00007A8C
		public virtual float FuseTimeSeconds
		{
			get
			{
				return 2f;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00009893 File Offset: 0x00007A93
		public virtual EnumBlastType BlastType
		{
			get
			{
				return this.blastType;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000AC RID: 172 RVA: 0x0000989B File Offset: 0x00007A9B
		public virtual float BlastRadius
		{
			get
			{
				return this.blastRadius;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000AD RID: 173 RVA: 0x000098A3 File Offset: 0x00007AA3
		public virtual float InjureRadius
		{
			get
			{
				return this.injureRadius;
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000098AC File Offset: 0x00007AAC
		public override void Initialize(ICoreAPI api)
		{
			base.Initialize(api);
			this.capi = (api as ICoreClientAPI);
			this.RegisterGameTickListener(new Action<float>(this.OnTick), 50, 0);
			if (this.fuseSound == null && api.Side == 2)
			{
				this.fuseSound = ((IClientWorldAccessor)api.World).LoadSound(new SoundParams
				{
					Location = new AssetLocation("game:sounds/effect/fuse"),
					ShouldLoop = true,
					Position = this.Pos.ToVec3f().Add(0.5f, 0.25f, 0.5f),
					DisposeOnFinish = false,
					Volume = 0.1f,
					Range = 16f
				});
			}
			this.blastRadius = (float)base.Block.Attributes["blastRadius"].AsInt(4);
			this.injureRadius = (float)base.Block.Attributes["injureRadius"].AsInt(8);
			this.blastType = base.Block.Attributes["blastType"].AsInt(0);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000099CC File Offset: 0x00007BCC
		public override void OnBlockPlaced(ItemStack byItemStack = null)
		{
			base.OnBlockPlaced(byItemStack);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000099D8 File Offset: 0x00007BD8
		private void OnTick(float dt)
		{
			if (this.IsLit)
			{
				this.remainingSeconds -= dt;
				if (this.Api.Side == 1 && this.remainingSeconds <= 0f)
				{
					this.Combust(dt);
				}
				if (this.Api.Side == 2)
				{
					BEFuse.smallSparks.MinPos.Set((double)this.Pos.X + 0.45, (double)this.Pos.Y + 0.53, (double)this.Pos.Z + 0.45);
					this.Api.World.SpawnParticles(BEFuse.smallSparks, null);
				}
				if (this.Api.Side == 1 && this.remainingSeconds < 1f && !this.NeibChecked)
				{
					this.NeibChecked = true;
					string text = base.Block.LastCodePart(0);
					BlockPos[] neibBlockPos = null;
					string text2 = text;
					for (int i = 0; i < text2.Length; i++)
					{
						char c = text2.get_Chars(i);
						if (c == 'n')
						{
							neibBlockPos = new BlockPos[]
							{
								this.Pos.NorthCopy(1),
								this.Pos.NorthCopy(1).UpCopy(1),
								this.Pos.NorthCopy(1).DownCopy(1)
							};
						}
						else if (c == 's')
						{
							neibBlockPos = new BlockPos[]
							{
								this.Pos.SouthCopy(1),
								this.Pos.SouthCopy(1).UpCopy(1),
								this.Pos.SouthCopy(1).DownCopy(1)
							};
						}
						else if (c == 'e')
						{
							neibBlockPos = new BlockPos[]
							{
								this.Pos.EastCopy(1),
								this.Pos.EastCopy(1).UpCopy(1),
								this.Pos.EastCopy(1).DownCopy(1)
							};
						}
						else if (c == 'w')
						{
							neibBlockPos = new BlockPos[]
							{
								this.Pos.WestCopy(1),
								this.Pos.WestCopy(1).UpCopy(1),
								this.Pos.WestCopy(1).DownCopy(1)
							};
						}
						if (neibBlockPos != null)
						{
							foreach (BlockPos neib in neibBlockPos)
							{
								if (neib != null && this.IgnitedPlayer != null)
								{
									BEFuse befuse = this.IgnitedPlayer.Entity.World.BlockAccessor.GetBlockEntity(neib) as BEFuse;
									if (befuse != null)
									{
										befuse.OnIgnite(this.IgnitedPlayer);
									}
									BEFirework befirework = this.IgnitedPlayer.Entity.World.BlockAccessor.GetBlockEntity(neib) as BEFirework;
									if (befirework != null)
									{
										befirework.OnIgnite(this.IgnitedPlayer);
									}
									BEBombFuse beorebomb = this.IgnitedPlayer.Entity.World.BlockAccessor.GetBlockEntity(neib) as BEBombFuse;
									if (beorebomb != null && beorebomb != null)
									{
										beorebomb.OnIgnite(this.IgnitedPlayer);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00009CF4 File Offset: 0x00007EF4
		private void GenAshes(Vec3d minPos, Vec3d maxPos)
		{
			Vec3f tmp = new Vec3f();
			for (int i = 0; i < 10; i++)
			{
				int color = ColorUtil.ToRgba(255, 30, 30, 30);
				tmp.Set((0.5f - (float)this.Api.World.Rand.NextDouble()) / 3f, 0f, (0.5f - (float)this.Api.World.Rand.NextDouble()) / 3f);
				this.Api.World.SpawnParticles(10f, color, minPos, maxPos, tmp, tmp, (float)this.Api.World.Rand.NextDouble(), 1f, 0.25f + (float)this.Api.World.Rand.NextDouble() * 0.25f, 1, null);
			}
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x00009DD4 File Offset: 0x00007FD4
		private void Combust(float dt)
		{
			Vec3d minPos = this.Pos.ToVec3d().AddCopy(0.5f, 0f, 0.5f);
			Vec3d maxPos = this.Pos.ToVec3d().AddCopy(0.5f, 0f, 0.5f);
			string path = base.Block.LastCodePart(0);
			if (path == "empty")
			{
				maxPos = this.Pos.ToVec3d().AddCopy(0.5f, 0f, 0.5f);
				this.GenAshes(minPos, maxPos);
			}
			else
			{
				string text = path;
				for (int i = 0; i < text.Length; i++)
				{
					char c = text.get_Chars(i);
					if (c == 'n')
					{
						maxPos = this.Pos.ToVec3d().AddCopy(0.5f, 0f, -0.1f);
					}
					else if (c == 's')
					{
						maxPos = this.Pos.ToVec3d().AddCopy(0.5f, 0f, 1.1f);
					}
					else if (c == 'e')
					{
						maxPos = this.Pos.ToVec3d().AddCopy(1.1f, 0f, 0.5f);
					}
					else if (c == 'w')
					{
						maxPos = this.Pos.ToVec3d().AddCopy(-0.1f, 0f, 0.5f);
					}
					this.GenAshes(minPos, maxPos);
				}
			}
			this.Api.World.BlockAccessor.SetBlock(0, this.Pos);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00009F54 File Offset: 0x00008154
		internal void OnBlockExploded(BlockPos pos, string ignitedByPlayerUid)
		{
			if (this.Api.Side == 1 && (!this.IsLit || (double)this.remainingSeconds > 0.1))
			{
				this.Api.World.RegisterCallback(new Action<float>(this.Combust), 250);
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000B4 RID: 180 RVA: 0x00009FAB File Offset: 0x000081AB
		// (set) Token: 0x060000B5 RID: 181 RVA: 0x00009FB3 File Offset: 0x000081B3
		public bool IsLit { get; private set; }

		// Token: 0x060000B6 RID: 182 RVA: 0x00009FBC File Offset: 0x000081BC
		internal void OnIgnite(IPlayer byPlayer)
		{
			if (this.IsLit)
			{
				return;
			}
			if (this.Api.Side == 2)
			{
				this.fuseSound.Start();
			}
			this.IsLit = true;
			this.NeibChecked = false;
			this.remainingSeconds = this.FuseTimeSeconds;
			this.ignitedByPlayerUid = ((byPlayer != null) ? byPlayer.PlayerUID : null);
			this.IgnitedPlayer = byPlayer;
			this.MarkDirty(false, null);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x0000A028 File Offset: 0x00008228
		public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
		{
			base.FromTreeAttributes(tree, worldForResolving);
			this.remainingSeconds = tree.GetFloat("remainingSeconds", 0f);
			this.IsLit = (tree.GetInt("lit", 0) > 0);
			this.ignitedByPlayerUid = tree.GetString("ignitedByPlayerUid", null);
			if (!this.IsLit)
			{
				ICoreAPI api = this.Api;
				if (api != null && api.Side == 2)
				{
					this.fuseSound.Stop();
				}
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x0000A0A4 File Offset: 0x000082A4
		private static BlockPos[] AreaAround(BlockPos pos)
		{
			return new BlockPos[]
			{
				pos.WestCopy(1),
				pos.SouthCopy(1),
				pos.EastCopy(1),
				pos.NorthCopy(1)
			};
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000A0D4 File Offset: 0x000082D4
		private static BlockPos[] AreaAroundUp(BlockPos pos)
		{
			return new BlockPos[]
			{
				pos.WestCopy(1).UpCopy(1),
				pos.SouthCopy(1).UpCopy(1),
				pos.EastCopy(1).UpCopy(1),
				pos.NorthCopy(1).UpCopy(1)
			};
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000A128 File Offset: 0x00008328
		private static BlockPos[] AreaAroundDown(BlockPos pos)
		{
			return new BlockPos[]
			{
				pos.WestCopy(1).DownCopy(1),
				pos.SouthCopy(1).DownCopy(1),
				pos.EastCopy(1).DownCopy(1),
				pos.NorthCopy(1).DownCopy(1)
			};
		}

		// Token: 0x060000BB RID: 187 RVA: 0x0000A17C File Offset: 0x0000837C
		public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tesselator)
		{
			string shapePathUp = "primitivesurvival:shapes/block/fuse/u";
			string shapePathLeadFuse = "primitivesurvival:shapes/block/fuse/leadfuse";
			string shapePathLeadBomb = "primitivesurvival:shapes/block/fuse/leadbomb";
			BlockFuse block = this.Api.World.BlockAccessor.GetBlock(this.Pos, 0) as BlockFuse;
			ITexPositionSource texture = tesselator.GetTextureSource(block, 0, false);
			BlockPos[] array = BEFuse.AreaAround(this.Pos);
			int rot = 0;
			foreach (BlockPos neighbor in array)
			{
				Block blockChk = this.Api.World.BlockAccessor.GetBlock(neighbor, 0);
				if (blockChk != null)
				{
					if (blockChk.Class == "blockfirework")
					{
						MeshData mesh = block.GenMesh(this.Api as ICoreClientAPI, shapePathLeadFuse, texture, rot, 0f);
						mesher.AddMeshData(mesh, 1);
					}
					else if (blockChk.Class == "blockbombfuse")
					{
						MeshData mesh = block.GenMesh(this.Api as ICoreClientAPI, shapePathLeadBomb, texture, rot, 0f);
						mesher.AddMeshData(mesh, 1);
					}
				}
				rot += 90;
			}
			BlockPos[] array3 = BEFuse.AreaAroundUp(this.Pos);
			rot = 0;
			foreach (BlockPos neighbor2 in array3)
			{
				Block blockChk = this.Api.World.BlockAccessor.GetBlock(neighbor2, 0);
				if (blockChk != null)
				{
					if (blockChk.Class == "blockfuse" || blockChk.Class == "blockfirework" || blockChk.Class == "blockbombfuse")
					{
						MeshData mesh = block.GenMesh(this.Api as ICoreClientAPI, shapePathUp, texture, rot, 0f);
						mesher.AddMeshData(mesh, 1);
					}
					if (blockChk.Class == "blockfirework")
					{
						MeshData mesh = block.GenMesh(this.Api as ICoreClientAPI, shapePathLeadFuse, texture, rot, 1f);
						mesher.AddMeshData(mesh, 1);
					}
					else if (blockChk.Class == "blockbombfuse")
					{
						MeshData mesh = block.GenMesh(this.Api as ICoreClientAPI, shapePathLeadBomb, texture, rot, 1f);
						mesher.AddMeshData(mesh, 1);
					}
				}
				rot += 90;
			}
			BlockPos[] array4 = BEFuse.AreaAroundDown(this.Pos);
			rot = 0;
			foreach (BlockPos neighbor3 in array4)
			{
				Block blockChk = this.Api.World.BlockAccessor.GetBlock(neighbor3, 0);
				if (blockChk != null)
				{
					if (blockChk.Class == "blockfuse" || blockChk.Class == "blockfirework" || blockChk.Class == "blockbombfuse")
					{
						MeshData mesh = block.GenMesh(this.Api as ICoreClientAPI, shapePathUp, texture, rot, -1f);
						mesher.AddMeshData(mesh, 1);
					}
					if (blockChk.Class == "blockfirework")
					{
						MeshData mesh = block.GenMesh(this.Api as ICoreClientAPI, shapePathLeadFuse, texture, rot, -1f);
						mesher.AddMeshData(mesh, 1);
					}
					else if (blockChk.Class == "blockbombfuse")
					{
						MeshData mesh = block.GenMesh(this.Api as ICoreClientAPI, shapePathLeadBomb, texture, rot, -1f);
						mesher.AddMeshData(mesh, 1);
					}
				}
				rot += 90;
			}
			return base.OnTesselation(mesher, tesselator);
		}

		// Token: 0x060000BC RID: 188 RVA: 0x0000A4E9 File Offset: 0x000086E9
		public override void ToTreeAttributes(ITreeAttribute tree)
		{
			base.ToTreeAttributes(tree);
			tree.SetFloat("remainingSeconds", this.remainingSeconds);
			tree.SetInt("lit", (this.IsLit > false) ? 1 : 0);
			tree.SetString("ignitedByPlayerUid", this.ignitedByPlayerUid);
		}

		// Token: 0x060000BD RID: 189 RVA: 0x0000A528 File Offset: 0x00008728
		~BEFuse()
		{
			if (this.fuseSound != null)
			{
				this.fuseSound.Dispose();
			}
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000A564 File Offset: 0x00008764
		public override void OnBlockRemoved()
		{
			base.OnBlockRemoved();
			if (this.fuseSound != null)
			{
				this.fuseSound.Stop();
			}
		}

		// Token: 0x04000065 RID: 101
		private float remainingSeconds;

		// Token: 0x04000066 RID: 102
		private string ignitedByPlayerUid;

		// Token: 0x04000067 RID: 103
		private float blastRadius;

		// Token: 0x04000068 RID: 104
		private float injureRadius;

		// Token: 0x04000069 RID: 105
		private EnumBlastType blastType;

		// Token: 0x0400006A RID: 106
		private ILoadedSound fuseSound;

		// Token: 0x0400006B RID: 107
		public static SimpleParticleProperties smallSparks = new SimpleParticleProperties(1f, 1f, ColorUtil.ToRgba(255, 255, 233, 0), new Vec3d(), new Vec3d(), new Vec3f(-3f, 5f, -3f), new Vec3f(3f, 8f, 3f), 0.03f, 1f, 0.05f, 0.15f, 0)
		{
			VertexFlags = 255
		};

		// Token: 0x0400006C RID: 108
		public ICoreClientAPI capi;

		// Token: 0x0400006D RID: 109
		public IPlayer IgnitedPlayer;

		// Token: 0x0400006E RID: 110
		public bool NeibChecked;
	}
}
