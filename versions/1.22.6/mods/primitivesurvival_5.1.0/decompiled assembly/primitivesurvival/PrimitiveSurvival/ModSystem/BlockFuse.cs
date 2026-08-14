using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x0200001F RID: 31
	public class BlockFuse : Block, IIgnitable
	{
		// Token: 0x060001A5 RID: 421 RVA: 0x00014278 File Offset: 0x00012478
		public override void OnLoaded(ICoreAPI api)
		{
			if (api.Side != 2)
			{
				return;
			}
			ICoreClientAPI capi = api as ICoreClientAPI;
			InteractionStacksDelegate <>9__1;
			this.interactions = ObjectCacheUtil.GetOrCreate<WorldInteraction[]>(api, "fuseInteractions", delegate()
			{
				List<ItemStack> canIgniteStacks = new List<ItemStack>();
				foreach (CollectibleObject obj in api.World.Collectibles)
				{
					if (obj is Block && (obj as Block).HasBehavior<BlockBehaviorCanIgnite>(false))
					{
						List<ItemStack> stacks = obj.GetHandBookStacks(capi);
						if (stacks != null)
						{
							canIgniteStacks.AddRange(stacks);
						}
					}
				}
				WorldInteraction[] array = new WorldInteraction[1];
				int num = 0;
				WorldInteraction worldInteraction = new WorldInteraction();
				worldInteraction.MouseButton = 2;
				worldInteraction.ActionLangCode = "primitivesurvival:blockhelp-firework-ignite";
				worldInteraction.Itemstacks = canIgniteStacks.ToArray();
				InteractionStacksDelegate getMatchingStacks;
				if ((getMatchingStacks = <>9__1) == null)
				{
					getMatchingStacks = (<>9__1 = delegate(WorldInteraction wi, BlockSelection bs, EntitySelection es)
					{
						BEFuse befuse = api.World.BlockAccessor.GetBlockEntity(bs.Position) as BEFuse;
						if (befuse != null && !befuse.IsLit)
						{
							return wi.Itemstacks;
						}
						return null;
					});
				}
				worldInteraction.GetMatchingStacks = getMatchingStacks;
				array[num] = worldInteraction;
				return array;
			});
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x000142D4 File Offset: 0x000124D4
		EnumIgniteState IIgnitable.OnTryIgniteStack(EntityAgent byEntity, BlockPos pos, ItemSlot slot, float secondsIgniting)
		{
			if (secondsIgniting <= 2f)
			{
				return 2;
			}
			return 3;
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x000142E4 File Offset: 0x000124E4
		public EnumIgniteState OnTryIgniteBlock(EntityAgent byEntity, BlockPos pos, float secondsIgniting)
		{
			BEFuse befuse = byEntity.World.BlockAccessor.GetBlockEntity(pos) as BEFuse;
			if (befuse == null || befuse.IsLit)
			{
				return 1;
			}
			if (secondsIgniting > 0.75f)
			{
				return 3;
			}
			return 2;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x00014320 File Offset: 0x00012520
		public void OnTryIgniteBlockOver(EntityAgent byEntity, BlockPos pos, float secondsIgniting, ref EnumHandling handling)
		{
			if (secondsIgniting < 0.7f)
			{
				return;
			}
			handling = 2;
			IPlayer byPlayer = null;
			EntityPlayer player = byEntity as EntityPlayer;
			if (player != null)
			{
				byPlayer = byEntity.World.PlayerByUid(player.PlayerUID);
			}
			if (byPlayer == null)
			{
				return;
			}
			BEFuse befuse = byPlayer.Entity.World.BlockAccessor.GetBlockEntity(pos) as BEFuse;
			if (befuse == null)
			{
				return;
			}
			befuse.OnIgnite(byPlayer);
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x00014382 File Offset: 0x00012582
		public override void OnBlockExploded(IWorldAccessor world, BlockPos pos, BlockPos explosionCenter, EnumBlastType blastType, string ignitedByPlayerUid)
		{
			BEFuse befuse = world.BlockAccessor.GetBlockEntity(pos) as BEFuse;
			if (befuse == null)
			{
				return;
			}
			befuse.OnBlockExploded(pos, ignitedByPlayerUid);
		}

		// Token: 0x060001AA RID: 426 RVA: 0x000143A2 File Offset: 0x000125A2
		public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
		{
			return ArrayExtensions.Append<WorldInteraction>(this.interactions, base.GetPlacedBlockInteractionHelp(world, selection, forPlayer));
		}

		// Token: 0x060001AB RID: 427 RVA: 0x000143B8 File Offset: 0x000125B8
		public override void OnJsonTesselation(ref MeshData sourceMesh, ref int[] lightRgbsByCorner, BlockPos pos, Block[] chunkExtBlocks, int extIndex3d)
		{
		}

		// Token: 0x060001AC RID: 428 RVA: 0x000143BC File Offset: 0x000125BC
		public string GetOrientations(IWorldAccessor world, BlockPos pos)
		{
			string orientations = this.GetFuseCode(world, pos, BlockFacing.NORTH) + this.GetFuseCode(world, pos, BlockFacing.EAST) + this.GetFuseCode(world, pos, BlockFacing.SOUTH) + this.GetFuseCode(world, pos, BlockFacing.WEST);
			if (orientations.Length == 0)
			{
				orientations = "empty";
			}
			return orientations;
		}

		// Token: 0x060001AD RID: 429 RVA: 0x00014414 File Offset: 0x00012614
		private string GetFuseCode(IWorldAccessor world, BlockPos pos, BlockFacing facing)
		{
			if (this.ShouldConnectAt(world, pos, facing) || this.ShouldConnectAt(world, pos.UpCopy(1), facing) || this.ShouldConnectAt(world, pos.DownCopy(1), facing))
			{
				return facing.Code.get_Chars(0).ToString() ?? "";
			}
			return "";
		}

		// Token: 0x060001AE RID: 430 RVA: 0x00014471 File Offset: 0x00012671
		private static BlockPos[] AboveBelow(BlockPos pos)
		{
			return new BlockPos[]
			{
				pos,
				pos.UpCopy(1),
				pos.DownCopy(1)
			};
		}

		// Token: 0x060001AF RID: 431 RVA: 0x00014494 File Offset: 0x00012694
		public override void OnNeighbourBlockChange(IWorldAccessor world, BlockPos pos, BlockPos neibpos)
		{
			BlockPos[] around = BlockFuse.AboveBelow(pos);
			string orientations = this.GetOrientations(world, pos);
			AssetLocation newBlockCode = base.CodeWithVariant("dir", orientations);
			if (!this.Code.Equals(newBlockCode))
			{
				Block block = world.BlockAccessor.GetBlock(newBlockCode);
				if (block == null)
				{
					return;
				}
				if (block.Code.Path.Contains("fuse"))
				{
					world.BlockAccessor.SetBlock(block.BlockId, pos);
				}
				foreach (BlockPos neighbor in around)
				{
					world.BlockAccessor.TriggerNeighbourBlockUpdate(neighbor);
				}
			}
			base.OnNeighbourBlockChange(world, pos, neibpos);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x00014539 File Offset: 0x00012739
		public override BlockDropItemStack[] GetDropsForHandbook(ItemStack handbookStack, IPlayer forPlayer)
		{
			return new BlockDropItemStack[]
			{
				new BlockDropItemStack(handbookStack, 1f)
			};
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x00014550 File Offset: 0x00012750
		public bool ShouldConnectAt(IWorldAccessor world, BlockPos ownPos, BlockFacing side)
		{
			Block block = world.BlockAccessor.GetBlock(ownPos.AddCopy(side), 0);
			return block.FirstCodePart(0) == base.FirstCodePart(0) || block.FirstCodePart(0) == "firework" || block.Code.Path.StartsWith("bomb-") || block.FirstCodePart(0) == "rockblastingbomb" || block.Class == "blockbombfuse";
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x000145D8 File Offset: 0x000127D8
		static BlockFuse()
		{
			BlockFuse.AngleGroups["n"] = new KeyValuePair<string[], int>(BlockFuse.OneDir, 0);
			BlockFuse.AngleGroups["e"] = new KeyValuePair<string[], int>(BlockFuse.OneDir, 1);
			BlockFuse.AngleGroups["s"] = new KeyValuePair<string[], int>(BlockFuse.OneDir, 2);
			BlockFuse.AngleGroups["w"] = new KeyValuePair<string[], int>(BlockFuse.OneDir, 3);
			BlockFuse.AngleGroups["ns"] = new KeyValuePair<string[], int>(BlockFuse.TwoDir, 0);
			BlockFuse.AngleGroups["ew"] = new KeyValuePair<string[], int>(BlockFuse.TwoDir, 1);
			BlockFuse.AngleGroups["ne"] = new KeyValuePair<string[], int>(BlockFuse.AngledDir, 0);
			BlockFuse.AngleGroups["nw"] = new KeyValuePair<string[], int>(BlockFuse.AngledDir, 1);
			BlockFuse.AngleGroups["es"] = new KeyValuePair<string[], int>(BlockFuse.AngledDir, 2);
			BlockFuse.AngleGroups["sw"] = new KeyValuePair<string[], int>(BlockFuse.AngledDir, 3);
			BlockFuse.AngleGroups["nes"] = new KeyValuePair<string[], int>(BlockFuse.ThreeDir, 0);
			BlockFuse.AngleGroups["new"] = new KeyValuePair<string[], int>(BlockFuse.ThreeDir, 1);
			BlockFuse.AngleGroups["nsw"] = new KeyValuePair<string[], int>(BlockFuse.ThreeDir, 2);
			BlockFuse.AngleGroups["esw"] = new KeyValuePair<string[], int>(BlockFuse.ThreeDir, 3);
			BlockFuse.AngleGroups["egw"] = new KeyValuePair<string[], int>(BlockFuse.GateLeft, 0);
			BlockFuse.AngleGroups["ngs"] = new KeyValuePair<string[], int>(BlockFuse.GateLeft, 1);
			BlockFuse.AngleGroups["gew"] = new KeyValuePair<string[], int>(BlockFuse.GateRight, 0);
			BlockFuse.AngleGroups["gns"] = new KeyValuePair<string[], int>(BlockFuse.GateRight, 1);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x00014898 File Offset: 0x00012A98
		public override AssetLocation GetRotatedBlockCode(int angle)
		{
			string type = this.Variant["dir"];
			if (type == "empty" || type == "nesw")
			{
				return this.Code;
			}
			int angleIndex = angle / 90;
			KeyValuePair<string[], int> val = BlockFuse.AngleGroups[type];
			string newFacing = val.Key[(angleIndex + val.Value) % val.Key.Length];
			return base.CodeWithVariant("dir", newFacing);
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x00014910 File Offset: 0x00012B10
		public MeshData GenMesh(ICoreClientAPI capi, string shapePath, ITexPositionSource texture, int rot, float updown)
		{
			ITesselatorAPI tesselator = capi.Tesselator;
			Shape shape = capi.Assets.TryGet(shapePath + ".json", true).ToObject<Shape>(null);
			MeshData mesh;
			tesselator.TesselateShape(shapePath, shape, ref mesh, texture, new Vec3f(0f, 0f, 0f), 0, 0, 0, default(int?), null);
			if (shapePath.Contains("lead"))
			{
				mesh.Translate(-1f, updown, 0f);
			}
			else if (updown == -1f)
			{
				mesh.Translate(-0.02f, -1f, 0f);
			}
			mesh.Rotate(new Vec3f(0.5f, 0f, 0.5f), 0f, (float)rot * 0.017453292f, 0f);
			return mesh;
		}

		// Token: 0x040000C7 RID: 199
		private WorldInteraction[] interactions;

		// Token: 0x040000C8 RID: 200
		private static readonly string[] OneDir = new string[]
		{
			"n",
			"e",
			"s",
			"w"
		};

		// Token: 0x040000C9 RID: 201
		private static readonly string[] TwoDir = new string[]
		{
			"ns",
			"ew"
		};

		// Token: 0x040000CA RID: 202
		private static readonly string[] AngledDir = new string[]
		{
			"ne",
			"es",
			"sw",
			"nw"
		};

		// Token: 0x040000CB RID: 203
		private static readonly string[] ThreeDir = new string[]
		{
			"nes",
			"new",
			"nsw",
			"esw"
		};

		// Token: 0x040000CC RID: 204
		private static readonly string[] GateLeft = new string[]
		{
			"egw",
			"ngs"
		};

		// Token: 0x040000CD RID: 205
		private static readonly string[] GateRight = new string[]
		{
			"gew",
			"gns"
		};

		// Token: 0x040000CE RID: 206
		private static readonly Dictionary<string, KeyValuePair<string[], int>> AngleGroups = new Dictionary<string, KeyValuePair<string[], int>>();
	}
}
