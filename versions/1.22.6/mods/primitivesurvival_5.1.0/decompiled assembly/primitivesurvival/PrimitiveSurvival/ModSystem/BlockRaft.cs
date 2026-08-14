using System;
using System.Linq;
using PrimitiveSurvival.ModConfig;
using Vintagestory.API.Client;
using Vintagestory.API.Client.Tesselation;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000025 RID: 37
	public class BlockRaft : Block, IDrawYAdjustable
	{
		// Token: 0x060001D6 RID: 470 RVA: 0x000161B4 File Offset: 0x000143B4
		private void GenerateParticles(BlockPos pos)
		{
			int color = ColorUtil.ToRgba(BlockRaft.rand.Next(0, 255), 0, 50, BlockRaft.rand.Next(80, 140));
			SimpleParticleProperties particles = new SimpleParticleProperties(30f, 30f, color, new Vec3d(1.0, 0.3, 1.0), new Vec3d(), new Vec3f(0.5f, 0.5f, 0.5f), new Vec3f(), (float)(BlockRaft.rand.NextDouble() * 1.0 + 1.0), 0f, 0.2f, 0.8f, 1);
			particles.MinPos.Add(pos);
			particles.AddPos.Set(new Vec3d(-1.0, 0.0, -1.0));
			particles.AddVelocity.Set(new Vec3f(-1f, -0.5f, -1f));
			particles.SizeEvolve = new EvolvingNatFloat(3, 1f);
			particles.VertexFlags = 255;
			this.api.World.SpawnParticles(particles, null);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x000162F0 File Offset: 0x000144F0
		public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
		{
			string facing = Block.SuggestedHVOrientation(byPlayer, blockSel)[0].ToString();
			if (this.isWater(world.BlockAccessor, blockSel.Position.UpCopy(1)))
			{
				blockSel = blockSel.Clone();
				blockSel.Position = blockSel.Position.Up(1);
				bool flag = base.TryPlaceBlock(world, byPlayer, itemstack, blockSel, ref failureCode);
				if (flag)
				{
					Block block = this.api.World.BlockAccessor.GetBlock(blockSel.Position, 0);
					string newPath = block.Code.Path;
					newPath = newPath.Replace("north", facing);
					block = this.api.World.GetBlock(block.CodeWithPath(newPath));
					this.api.World.BlockAccessor.SetBlock(block.BlockId, blockSel.Position);
				}
				return flag;
			}
			Block testBlock = this.api.World.BlockAccessor.GetBlock(blockSel.Position, 0);
			if (blockSel.Face.IsHorizontal || testBlock.Code.Path.Contains("water"))
			{
				return false;
			}
			Block testBlock2 = this.api.World.BlockAccessor.GetBlock(blockSel.Position.DownCopy(1), 0);
			if (testBlock2.Code.FirstCodePart() == "floatingdock" || testBlock2.Code.FirstCodePart() == "raft")
			{
				return false;
			}
			if (testBlock2.Code.Path.Contains("lakeice"))
			{
				return false;
			}
			bool flag2 = base.TryPlaceBlock(world, byPlayer, itemstack, blockSel, ref failureCode);
			if (flag2)
			{
				Block block2 = this.api.World.BlockAccessor.GetBlock(blockSel.Position, 0);
				string newPath2 = block2.Code.Path;
				newPath2 = newPath2.Replace("north", facing);
				block2 = this.api.World.GetBlock(block2.CodeWithPath(newPath2));
				this.api.World.BlockAccessor.SetBlock(block2.BlockId, blockSel.Position);
			}
			return flag2;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0001650C File Offset: 0x0001470C
		public bool isWater(IBlockAccessor blockAccessor, BlockPos pos)
		{
			Block waterblock = blockAccessor.GetBlock(pos.DownCopy(1), 2);
			Block upblock = blockAccessor.GetBlock(pos, 2);
			return waterblock.IsLiquid() && waterblock.LiquidLevel == 7 && waterblock.LiquidCode.Contains("water") && upblock.Id == 0;
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x00016560 File Offset: 0x00014760
		public float AdjustYPosition(BlockPos pos, Block[] chunkExtBlocks, int extIndex3d)
		{
			Cuboidf[] boxes = chunkExtBlocks[extIndex3d + TileSideEnum.MoveIndex[5]].CollisionBoxes;
			float boxHeight = 0f;
			if (boxes != null && boxes.Length != 0)
			{
				boxHeight = Enumerable.Max<Cuboidf>(boxes, (Cuboidf c) => c.Y2);
			}
			if (boxHeight >= 0.8f)
			{
				return 0f;
			}
			return -0.28f;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x000165C4 File Offset: 0x000147C4
		public override void OnJsonTesselation(ref MeshData sourceMesh, ref int[] lightRgbsByCorner, BlockPos pos, Block[] chunkExtBlocks, int extIndex3d)
		{
			sourceMesh.XyzInstanced = true;
			base.OnJsonTesselation(ref sourceMesh, ref lightRgbsByCorner, pos, chunkExtBlocks, extIndex3d);
			BlockPos below = pos.DownCopy(1);
			Block downSolidBlock = this.api.World.BlockAccessor.GetBlock(below, 1);
			Cuboidf[] boxes = downSolidBlock.CollisionBoxes;
			float boxHeight = 0f;
			if (boxes != null && boxes.Length != 0)
			{
				boxHeight = Enumerable.Max<Cuboidf>(boxes, (Cuboidf c) => c.Y2);
			}
			RegistryObject block = this.api.World.BlockAccessor.GetBlock(below, 2);
			int windData = (int)this.VertexFlags.Normal;
			if (block.Code.Path.Contains("water") && (downSolidBlock.Id == 0 || boxHeight < 0.8f))
			{
				windData = ((int)((ushort)this.VertexFlags.Normal) | 201326592 | (int)this.VertexFlags.ZOffset);
			}
			for (int i = 0; i < sourceMesh.FlagsCount; i++)
			{
				sourceMesh.Flags[i] = windData;
			}
		}

		// Token: 0x060001DB RID: 475 RVA: 0x000166CC File Offset: 0x000148CC
		public override void OnNeighbourBlockChange(IWorldAccessor world, BlockPos pos, BlockPos neibpos)
		{
			if (world.BlockAccessor.GetBlock(new BlockPos(pos.X, pos.Y - 1, pos.Z, 0)).Id == 0)
			{
				world.BlockAccessor.BreakBlock(pos, null, 1f);
				return;
			}
			base.OnNeighbourBlockChange(world, pos, neibpos);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x00016724 File Offset: 0x00014924
		public override void OnBeingLookedAt(IPlayer byPlayer, BlockSelection blockSel, bool firstTick)
		{
			Block block = this.api.World.BlockAccessor.GetBlock(blockSel.Position, 0);
			string newPath = block.Code.Path;
			if (newPath.Contains("raft-") && this.api.Side == 1)
			{
				if (firstTick)
				{
					this.lookCount = 0L;
				}
				else
				{
					this.lookCount += 1L;
					if (this.lookCount > 200L)
					{
						int rafttype = this.api.World.Rand.Next(Enumerable.Count<string>(this.raftTypes));
						newPath = newPath.Replace("raft", this.raftTypes[rafttype]);
						block = this.api.World.GetBlock(block.CodeWithPath(newPath));
						this.api.World.BlockAccessor.SetBlock(block.BlockId, blockSel.Position);
						this.api.World.PlaySoundAt(new AssetLocation("game:sounds/block/metaldoor-place"), (double)blockSel.Position.X + 0.5, (double)blockSel.Position.Y + 0.5, (double)blockSel.Position.Z + 0.5, null, true, 32f, 1f);
						this.GenerateParticles(blockSel.Position);
						this.lookCount = 0L;
					}
				}
			}
			base.OnBeingLookedAt(byPlayer, blockSel, firstTick);
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0001689E File Offset: 0x00014A9E
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
		{
			handling = 4;
			base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handling);
		}

		// Token: 0x060001DE RID: 478 RVA: 0x000168B4 File Offset: 0x00014AB4
		public override void OnHeldIdle(ItemSlot slot, EntityAgent byEntity)
		{
			if (byEntity.World is IClientWorldAccessor && this.wsound == null)
			{
				byEntity.World.Api.ObjectCache["raftSound"] = (this.wsound = (byEntity.World as IClientWorldAccessor).LoadSound(new SoundParams
				{
					Location = new AssetLocation("sounds/environment/largesplash1.ogg"),
					ShouldLoop = true,
					DisposeOnFinish = false,
					Volume = 0.3f,
					Pitch = 1f
				}));
			}
			if (byEntity.World.Side == 2)
			{
				byEntity.World.UnregisterCallback(this.handlerId);
				this.handlerId = byEntity.World.RegisterCallback(new Action<float>(this.AfterAwhile), 1350);
			}
			if (byEntity.World is IClientWorldAccessor)
			{
				byEntity.StopAnimation("swim");
				if (byEntity.IsEyesSubmerged() || byEntity.FeetInLiquid)
				{
					byEntity.StartAnimation("swim");
					this.wsound.SetPosition(byEntity.Pos.XYZ.ToVec3f());
					if (!this.wsound.IsPlaying)
					{
						this.wsound.Start();
					}
					IClientPlayer player = (this.api as ICoreClientAPI).World.Player;
					if (player != null)
					{
						bool flag = player.CameraMode == 0;
					}
					double tickInterval = 1.0;
					if (true)
					{
						tickInterval = ((double)byEntity.World.ElapsedMilliseconds - this.prevms) / 30.0;
						if (tickInterval > 10.0)
						{
							tickInterval = 1.01;
						}
						this.prevms = (double)byEntity.World.ElapsedMilliseconds;
					}
					if (byEntity.IsEyesSubmerged())
					{
						Vec3d pos = byEntity.Pos.HorizontalAheadCopy(0.009999999776482582).XYZ;
						double newX = byEntity.Pos.X - pos.X;
						double newZ = byEntity.Pos.Z - pos.Z;
						byEntity.Pos.Motion.X -= newX;
						byEntity.Pos.Motion.Z -= newZ;
						byEntity.Pos.Motion.Y += (double)ModConfig.Loaded.RaftFlotationModifier / 1.8 * tickInterval;
						return;
					}
					Vec3d pos2 = byEntity.Pos.HorizontalAheadCopy(0.05000000074505806).XYZ;
					double newX2 = byEntity.Pos.X - pos2.X;
					double newZ2 = byEntity.Pos.Z - pos2.Z;
					byEntity.Pos.Motion.X -= newX2 * (double)ModConfig.Loaded.RaftWaterSpeedModifier * tickInterval;
					byEntity.Pos.Motion.Z -= newZ2 * (double)ModConfig.Loaded.RaftWaterSpeedModifier * tickInterval;
					return;
				}
				else
				{
					this.CancelRaft(byEntity);
				}
			}
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00016BB0 File Offset: 0x00014DB0
		private void CancelRaft(EntityAgent byEntity)
		{
			byEntity.StopAnimation("swim");
			if (byEntity.World is IClientWorldAccessor && this.wsound != null && this.wsound.IsPlaying)
			{
				ILoadedSound loadedSound = this.wsound;
				if (loadedSound != null)
				{
					loadedSound.Stop();
				}
				ILoadedSound loadedSound2 = this.wsound;
				if (loadedSound2 != null)
				{
					loadedSound2.Dispose();
				}
				this.wsound = null;
			}
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x00016C14 File Offset: 0x00014E14
		private void AfterAwhile(float dt)
		{
			EntityPlayer byEntity = (this.api as ICoreClientAPI).World.Player.Entity;
			string stackname = "null";
			if (byEntity.RightHandItemSlot.Itemstack != null)
			{
				stackname = byEntity.RightHandItemSlot.Itemstack.GetName();
			}
			if (stackname != "Raft")
			{
				this.CancelRaft(byEntity);
			}
		}

		// Token: 0x040000D2 RID: 210
		private long handlerId;

		// Token: 0x040000D3 RID: 211
		public long lookCount;

		// Token: 0x040000D4 RID: 212
		private static Random rand = new Random();

		// Token: 0x040000D5 RID: 213
		private ILoadedSound wsound;

		// Token: 0x040000D6 RID: 214
		private readonly string[] raftTypes = new string[]
		{
			"raftps",
			"raftcrab",
			"raftdolphin",
			"raftshark",
			"rafttuna",
			"raftps"
		};

		// Token: 0x040000D7 RID: 215
		private double prevms;
	}
}
