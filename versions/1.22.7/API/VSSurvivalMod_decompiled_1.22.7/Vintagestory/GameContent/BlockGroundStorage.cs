using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockGroundStorage : Block, ICombustible, IIgnitable
{
	protected ItemStack[] groundStorablesQuadrants;

	protected ItemStack[] groundStorablesHalves;

	public ModelTransform[] Messy12Transforms;

	public static bool IsUsingContainedBlock;

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		PlacedPriorityInteract = true;
		Vec3f[] positions = Attributes?["messy12Position"].AsObject<Vec3f[]>();
		Messy12Transforms = new ModelTransform[12];
		Random rnd = new Random(0);
		for (int i = 0; i < 12; i++)
		{
			float rndx = ((positions != null) ? (positions[i].X - 0.5f) : ((float)rnd.NextDouble() * 0.9f - 0.45f));
			float rndz = ((positions != null) ? (positions[i].Z - 0.5f) : ((float)rnd.NextDouble() * 0.9f - 0.45f));
			float rot = (float)rnd.NextDouble();
			float rndscale = 1f + ((float)rnd.NextDouble() * 0.02f - 0.01f);
			Messy12Transforms[i] = new ModelTransform
			{
				Origin = new FastVec3f(0.5f, 0f, 0.5f),
				Rotation = new FastVec3f(0f, rot, 0f),
				Scale = rndscale,
				Translation = new FastVec3f(rndx, 0f, rndz)
			};
		}
		ItemStack[][] stacks = ObjectCacheUtil.GetOrCreate(api, "groundStorablesQuadrands", delegate
		{
			List<ItemStack> list = new List<ItemStack>();
			List<ItemStack> list2 = new List<ItemStack>();
			foreach (CollectibleObject current in api.World.Collectibles)
			{
				CollectibleBehaviorGroundStorable behavior = current.GetBehavior<CollectibleBehaviorGroundStorable>();
				if (behavior != null && behavior.StorageProps.Layout == EnumGroundStorageLayout.Quadrants)
				{
					list.Add(new ItemStack(current));
				}
				if (behavior != null && behavior.StorageProps.Layout == EnumGroundStorageLayout.Halves)
				{
					list2.Add(new ItemStack(current));
				}
			}
			return new ItemStack[2][]
			{
				list.ToArray(),
				list2.ToArray()
			};
		});
		groundStorablesQuadrants = stacks[0];
		groundStorablesHalves = stacks[1];
		if (api.Side == EnumAppSide.Client)
		{
			(api as ICoreClientAPI).Event.MouseUp += Event_MouseUp;
		}
	}

	private void Event_MouseUp(MouseEvent e)
	{
		IsUsingContainedBlock = false;
	}

	public override Cuboidf[] GetCollisionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
	{
		if (blockAccessor.GetBlockEntity(pos) is BlockEntityGroundStorage beg)
		{
			return beg.GetCollisionBoxes();
		}
		return base.GetCollisionBoxes(blockAccessor, pos);
	}

	public override Cuboidf[] GetParticleCollisionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
	{
		if (blockAccessor.GetBlockEntity(pos) is BlockEntityGroundStorage beg)
		{
			return beg.GetCollisionBoxes();
		}
		return base.GetCollisionBoxes(blockAccessor, pos);
	}

	public override Cuboidf[] GetSelectionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
	{
		BlockEntityGroundStorage be = blockAccessor.GetBlockEntity<BlockEntityGroundStorage>(pos);
		if (be != null)
		{
			return be.GetSelectionBoxes();
		}
		return base.GetSelectionBoxes(blockAccessor, pos);
	}

	public override bool CanAttachBlockAt(IBlockAccessor blockAccessor, Block block, BlockPos pos, BlockFacing blockFace, Cuboidi attachmentArea = null)
	{
		return blockAccessor.GetBlockEntity<BlockEntityGroundStorage>(pos)?.CanAttachBlockAt(blockFace, attachmentArea) ?? base.CanAttachBlockAt(blockAccessor, block, pos, blockFace, attachmentArea);
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		if (api.Side == EnumAppSide.Client && IsUsingContainedBlock)
		{
			return false;
		}
		if (!world.Claims.TryAccess(byPlayer, blockSel.Position, EnumBlockAccessFlags.Use))
		{
			byPlayer.InventoryManager.ActiveHotbarSlot.MarkDirty();
			return false;
		}
		if (world.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityGroundStorage beg)
		{
			return beg.OnPlayerInteractStart(byPlayer, blockSel);
		}
		return base.OnBlockInteractStart(world, byPlayer, blockSel);
	}

	public override bool OnBlockInteractStep(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		if (world.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityGroundStorage beg)
		{
			return beg.OnPlayerInteractStep(secondsUsed, byPlayer, blockSel);
		}
		return base.OnBlockInteractStep(secondsUsed, world, byPlayer, blockSel);
	}

	public override void OnBlockInteractStop(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		if (world.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityGroundStorage beg)
		{
			beg.OnPlayerInteractStop(secondsUsed, byPlayer, blockSel);
		}
		else
		{
			base.OnBlockInteractStop(secondsUsed, world, byPlayer, blockSel);
		}
	}

	public override bool OnBlockInteractCancel(float secondsUsed, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, EnumItemUseCancelReason cancelReason)
	{
		if (world.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityGroundStorage beg)
		{
			return beg.OnPlayerInteractCancel(secondsUsed, byPlayer, blockSel, cancelReason);
		}
		return base.OnBlockInteractCancel(secondsUsed, world, byPlayer, blockSel, cancelReason);
	}

	public override EnumBlockMaterial GetBlockMaterial(IBlockAccessor blockAccessor, BlockPos pos, ItemStack stack = null)
	{
		return base.GetBlockMaterial(blockAccessor, pos, stack);
	}

	public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
	{
		if (world.BlockAccessor.GetBlockEntity(pos) is BlockEntityGroundStorage beg)
		{
			List<ItemStack> stacks = new List<ItemStack>();
			foreach (ItemSlot slot in beg.Inventory)
			{
				if (!slot.Empty)
				{
					stacks.Add(slot.Itemstack);
				}
			}
			return stacks.ToArray();
		}
		return base.GetDrops(world, pos, byPlayer, dropQuantityMultiplier);
	}

	public float FillLevel(IBlockAccessor blockAccessor, BlockPos pos)
	{
		if (blockAccessor.GetBlockEntity(pos) is BlockEntityGroundStorage beg)
		{
			return (int)Math.Ceiling((float)beg.TotalStackSize / (float)beg.Capacity);
		}
		return 1f;
	}

	public bool CreateStorage(IWorldAccessor world, BlockSelection blockSel, IPlayer player)
	{
		if (!world.Claims.TryAccess(player, blockSel.Position, EnumBlockAccessFlags.BuildOrBreak))
		{
			player.InventoryManager.ActiveHotbarSlot.MarkDirty();
			return false;
		}
		BlockPos pos = blockSel.Position;
		if (blockSel.Face != null)
		{
			pos = pos.AddCopy(blockSel.Face);
		}
		if (pos.Y >= api.World.BlockAccessor.MapSizeY)
		{
			return false;
		}
		BlockPos posBelow = pos.DownCopy();
		if (!world.BlockAccessor.GetBlock(posBelow).CanAttachBlockAt(world.BlockAccessor, this, posBelow, BlockFacing.UP))
		{
			return false;
		}
		GroundStorageProperties storageProps = player.InventoryManager.ActiveHotbarSlot.Itemstack.Collectible.GetBehavior<CollectibleBehaviorGroundStorable>()?.StorageProps;
		if (storageProps != null && storageProps.CtrlKey && !player.Entity.Controls.CtrlKey)
		{
			return false;
		}
		BlockPos targetPos = (blockSel.DidOffset ? blockSel.Position.AddCopy(blockSel.Face.Opposite) : blockSel.Position);
		double y = player.Entity.Pos.X - ((double)targetPos.X + blockSel.HitPosition.X);
		double dz = (double)(float)player.Entity.Pos.Z - ((double)targetPos.Z + blockSel.HitPosition.Z);
		float num = (float)Math.Atan2(y, dz);
		float deg90 = (float)Math.PI / 2f;
		float roundRad = (float)(int)Math.Round(num / deg90) * deg90;
		BlockFacing attachFace = null;
		if (storageProps.Layout == EnumGroundStorageLayout.WallHalves)
		{
			attachFace = Block.SuggestedHVOrientation(player, blockSel)[0];
			BlockPos npos = pos.AddCopy(attachFace).Up(storageProps.WallOffY - 1);
			if (!world.BlockAccessor.GetBlock(npos).CanAttachBlockAt(world.BlockAccessor, this, npos, attachFace.Opposite))
			{
				attachFace = null;
				BlockFacing[] hORIZONTALS = BlockFacing.HORIZONTALS;
				foreach (BlockFacing face in hORIZONTALS)
				{
					npos = pos.AddCopy(face).Up(storageProps.WallOffY - 1);
					if (world.BlockAccessor.GetBlock(npos).CanAttachBlockAt(world.BlockAccessor, this, npos, face.Opposite))
					{
						attachFace = face;
						break;
					}
				}
			}
			if (attachFace == null)
			{
				if (storageProps.WallOffY > 1)
				{
					(api as ICoreClientAPI)?.TriggerIngameError(this, "requireswall", Lang.Get("placefailure-requirestallwall", storageProps.WallOffY));
				}
				else
				{
					(api as ICoreClientAPI)?.TriggerIngameError(this, "requireswall", Lang.Get("placefailure-requireswall"));
				}
				return false;
			}
			roundRad = (float)Math.Atan2(attachFace.Normali.X, attachFace.Normali.Z);
		}
		world.BlockAccessor.SetBlock(BlockId, pos);
		if (world.BlockAccessor.GetBlockEntity(pos) is BlockEntityGroundStorage beg)
		{
			beg.MeshAngle = roundRad;
			beg.AttachFace = attachFace;
			beg.clientsideFirstPlacement = world.Side == EnumAppSide.Client;
			beg.OnPlayerInteractStart(player, blockSel);
		}
		if (CollisionTester.AabbIntersect(GetCollisionBoxes(world.BlockAccessor, pos)[0], pos.X, pos.Y, pos.Z, player.Entity.SelectionBox, player.Entity.Pos.XYZ))
		{
			player.Entity.Pos.Y += GetCollisionBoxes(world.BlockAccessor, pos)[0].Y2;
		}
		(player as IClientPlayer)?.TriggerFpAnimation(EnumHandInteract.HeldItemInteract);
		return true;
	}

	public override void OnNeighbourBlockChange(IWorldAccessor world, BlockPos pos, BlockPos neibpos)
	{
		BlockEntityGroundStorage beg = world.BlockAccessor.GetBlockEntity(pos) as BlockEntityGroundStorage;
		if (beg?.StorageProps != null && beg.StorageProps.Layout == EnumGroundStorageLayout.WallHalves)
		{
			BlockFacing facing = beg.AttachFace;
			BlockPos bpos = pos.AddCopy(facing.Normali.X, beg.StorageProps.WallOffY - 1, facing.Normali.Z);
			if (!world.BlockAccessor.GetBlock(bpos).CanAttachBlockAt(world.BlockAccessor, this, bpos, facing.Opposite))
			{
				world.BlockAccessor.BreakBlock(pos, null);
			}
			if (!world.BlockAccessor.GetBlock(pos.DownCopy()).CanAttachBlockAt(world.BlockAccessor, this, pos.DownCopy(), BlockFacing.UP))
			{
				world.BlockAccessor.BreakBlock(pos, null);
			}
			return;
		}
		if (api.World.BlockAccessor.GetBlockEntity(pos) is BlockEntityGroundStorage { IsBurning: not false } begs)
		{
			if (!world.BlockAccessor.GetBlock(pos.DownCopy()).CanAttachBlockAt(world.BlockAccessor, this, pos.DownCopy(), BlockFacing.UP))
			{
				world.BlockAccessor.BreakBlock(pos, null);
				return;
			}
			Block block = world.BlockAccessor.GetBlock(neibpos);
			Block neibliqBlock = world.BlockAccessor.GetBlock(neibpos, 2);
			JsonObject attributes = block.Attributes;
			if (attributes == null || !attributes.IsTrue("smothersFire"))
			{
				JsonObject attributes2 = neibliqBlock.Attributes;
				if (attributes2 == null || !attributes2.IsTrue("smothersFire"))
				{
					goto IL_01a1;
				}
			}
			begs?.Extinguish();
		}
		goto IL_01a1;
		IL_01a1:
		base.OnNeighbourBlockChange(world, pos, neibpos);
	}

	public override byte[] GetLightHsv(IBlockAccessor blockAccessor, BlockPos pos, ItemStack stack = null)
	{
		if (pos != null && blockAccessor.GetBlockEntity(pos) is BlockEntityGroundStorage beg)
		{
			return beg.GetLightHsv();
		}
		return base.GetLightHsv(blockAccessor, pos, stack);
	}

	public override int GetColorWithoutTint(ICoreClientAPI capi, BlockPos pos)
	{
		if (capi.World.BlockAccessor.GetBlockEntity(pos) is BlockEntityGroundStorage beg)
		{
			ItemSlot slot = beg.Inventory.ToArray().Shuffle(capi.World.Rand).FirstOrDefault((ItemSlot s) => !s.Empty);
			if (slot != null)
			{
				return slot.Itemstack.Collectible.GetRandomColor(capi, slot.Itemstack);
			}
		}
		return base.GetColorWithoutTint(capi, pos);
	}

	public override int GetRandomColor(ICoreClientAPI capi, BlockPos pos, BlockFacing facing, int rndIndex = -1)
	{
		if (capi.World.BlockAccessor.GetBlockEntity(pos) is BlockEntityGroundStorage beg)
		{
			ItemSlot slot = beg.Inventory.ToArray().Shuffle(capi.World.Rand).FirstOrDefault((ItemSlot s) => !s.Empty);
			return slot?.Itemstack.Collectible.GetRandomColor(capi, slot.Itemstack) ?? 0;
		}
		return base.GetRandomColor(capi, pos, facing, rndIndex);
	}

	public override int GetRandomColor(ICoreClientAPI capi, ItemStack stack)
	{
		return base.GetRandomColor(capi, stack);
	}

	public override string GetPlacedBlockName(IWorldAccessor world, BlockPos pos)
	{
		if (world.BlockAccessor.GetBlockEntity(pos) is BlockEntityGroundStorage beg)
		{
			return beg.GetBlockName();
		}
		return OnPickBlock(world, pos)?.GetName();
	}

	public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos)
	{
		if (world.BlockAccessor.GetBlockEntity(pos) is BlockEntityGroundStorage beg)
		{
			return beg.Inventory.FirstNonEmptySlot?.Itemstack.Clone();
		}
		return null;
	}

	public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
	{
		BlockEntityGroundStorage beg = world.BlockAccessor.GetBlockEntity(selection.Position) as BlockEntityGroundStorage;
		if (beg?.StorageProps != null)
		{
			ItemSlot selSlot = beg.GetSlotAt(selection);
			WorldInteraction[] containedInteractions = selSlot?.Itemstack?.Collectible.GetCollectibleInterface<IContainedInteractable>()?.GetContainedInteractionHelp(beg, selSlot, forPlayer, selection) ?? Array.Empty<WorldInteraction>();
			int bulkquantity = beg.StorageProps.BulkTransferQuantity;
			if (beg.StorageProps.Layout == EnumGroundStorageLayout.Stacking && !beg.Inventory.Empty)
			{
				ItemStack[] canIgniteStacks = BlockBehaviorCanIgnite.CanIgniteStacks(api, withFirestarter: true).ToArray();
				CollectibleObject collObj = beg.Inventory[0].Itemstack?.Collectible;
				if (collObj == null)
				{
					return base.GetPlacedBlockInteractionHelp(world, selection, forPlayer);
				}
				List<WorldInteraction> list = new List<WorldInteraction>();
				list.Add(new WorldInteraction
				{
					ActionLangCode = "blockhelp-firepit-ignite",
					MouseButton = EnumMouseButton.Right,
					HotKeyCode = "shift",
					Itemstacks = canIgniteStacks,
					GetMatchingStacks = (WorldInteraction wi, BlockSelection bs, EntitySelection es) => (api.World.BlockAccessor.GetBlockEntity(bs.Position) is BlockEntityGroundStorage { IsBurning: false } blockEntityGroundStorage && blockEntityGroundStorage != null && blockEntityGroundStorage.CanIgnite) ? wi.Itemstacks : null
				});
				list.Add(new WorldInteraction
				{
					ActionLangCode = "blockhelp-groundstorage-addone",
					MouseButton = EnumMouseButton.Right,
					HotKeyCode = "shift",
					Itemstacks = new ItemStack[1]
					{
						new ItemStack(collObj)
					}
				});
				list.Add(new WorldInteraction
				{
					ActionLangCode = "blockhelp-groundstorage-removeone",
					MouseButton = EnumMouseButton.Right,
					HotKeyCode = null
				});
				list.Add(new WorldInteraction
				{
					ActionLangCode = "blockhelp-groundstorage-addbulk",
					MouseButton = EnumMouseButton.Right,
					HotKeyCodes = new string[2] { "ctrl", "shift" },
					Itemstacks = new ItemStack[1]
					{
						new ItemStack(collObj, bulkquantity)
					}
				});
				list.Add(new WorldInteraction
				{
					ActionLangCode = "blockhelp-groundstorage-removebulk",
					HotKeyCode = "ctrl",
					MouseButton = EnumMouseButton.Right
				});
				list.AddRange(containedInteractions);
				list.AddRange(base.GetPlacedBlockInteractionHelp(world, selection, forPlayer));
				return list.ToArray();
			}
			WorldInteraction[] array2;
			WorldInteraction[] placedBlockInteractionHelp;
			WorldInteraction[] array;
			ReadOnlySpan<WorldInteraction> readOnlySpan2;
			ReadOnlySpan<WorldInteraction> readOnlySpan;
			int num;
			if (beg.StorageProps.Layout == EnumGroundStorageLayout.SingleCenter)
			{
				WorldInteraction worldInteraction = new WorldInteraction
				{
					ActionLangCode = "blockhelp-behavior-rightclickpickup",
					MouseButton = EnumMouseButton.Right
				};
				array = containedInteractions;
				placedBlockInteractionHelp = base.GetPlacedBlockInteractionHelp(world, selection, forPlayer);
				num = 0;
				array2 = new WorldInteraction[1 + (array.Length + placedBlockInteractionHelp.Length)];
				array2[num] = worldInteraction;
				num++;
				readOnlySpan = new ReadOnlySpan<WorldInteraction>(array);
				readOnlySpan.CopyTo(new Span<WorldInteraction>(array2).Slice(num, readOnlySpan.Length));
				num += readOnlySpan.Length;
				readOnlySpan2 = new ReadOnlySpan<WorldInteraction>(placedBlockInteractionHelp);
				readOnlySpan2.CopyTo(new Span<WorldInteraction>(array2).Slice(num, readOnlySpan2.Length));
				num += readOnlySpan2.Length;
				return array2;
			}
			if (beg.StorageProps.Layout == EnumGroundStorageLayout.Halves || beg.StorageProps.Layout == EnumGroundStorageLayout.Quadrants)
			{
				List<WorldInteraction> list2 = new List<WorldInteraction>();
				list2.Add(new WorldInteraction
				{
					ActionLangCode = "blockhelp-groundstorage-add",
					MouseButton = EnumMouseButton.Right,
					HotKeyCode = "shift",
					Itemstacks = ((beg.StorageProps.Layout == EnumGroundStorageLayout.Halves) ? groundStorablesHalves : groundStorablesQuadrants)
				});
				list2.Add(new WorldInteraction
				{
					ActionLangCode = "blockhelp-groundstorage-remove",
					MouseButton = EnumMouseButton.Right,
					HotKeyCode = null
				});
				list2.AddRange(containedInteractions);
				list2.AddRange(base.GetPlacedBlockInteractionHelp(world, selection, forPlayer));
				return list2.ToArray();
			}
			array2 = containedInteractions;
			placedBlockInteractionHelp = base.GetPlacedBlockInteractionHelp(world, selection, forPlayer);
			num = 0;
			array = new WorldInteraction[array2.Length + placedBlockInteractionHelp.Length];
			readOnlySpan2 = new ReadOnlySpan<WorldInteraction>(array2);
			readOnlySpan2.CopyTo(new Span<WorldInteraction>(array).Slice(num, readOnlySpan2.Length));
			num += readOnlySpan2.Length;
			readOnlySpan = new ReadOnlySpan<WorldInteraction>(placedBlockInteractionHelp);
			readOnlySpan.CopyTo(new Span<WorldInteraction>(array).Slice(num, readOnlySpan.Length));
			num += readOnlySpan.Length;
			return array;
		}
		return base.GetPlacedBlockInteractionHelp(world, selection, forPlayer);
	}

	public float GetBurnDuration(IWorldAccessor world, BlockPos pos)
	{
		if (world.BlockAccessor.GetBlockEntity(pos) is BlockEntityGroundStorage beg)
		{
			ItemStack stack = beg.Inventory.FirstNonEmptySlot?.Itemstack;
			CombustibleProperties combustibleProps = stack?.Collectible?.GetCombustibleProperties(world, stack, null);
			if (stack == null || combustibleProps == null)
			{
				return 0f;
			}
			float dur = combustibleProps.BurnDuration;
			if (dur == 0f)
			{
				return 0f;
			}
			return GameMath.Clamp(dur * (float)Math.Log(stack.StackSize), 1f, 120f);
		}
		return 0f;
	}

	public override void OnUnloaded(ICoreAPI api)
	{
		base.OnUnloaded(api);
		Dictionary<string, MultiTextureMeshRef> groundStorageMeshRefs = ObjectCacheUtil.TryGet<Dictionary<string, MultiTextureMeshRef>>(api, "groundStorageUMC");
		if (groundStorageMeshRefs == null)
		{
			return;
		}
		foreach (MultiTextureMeshRef meshRef in groundStorageMeshRefs.Values)
		{
			if (meshRef != null && !meshRef.Disposed)
			{
				meshRef.Dispose();
			}
		}
		ObjectCacheUtil.Delete(api, "groundStorageUMC");
	}

	EnumIgniteState IIgnitable.OnTryIgniteStack(EntityAgent byEntity, BlockPos pos, ItemSlot slot, float secondsIgniting)
	{
		return EnumIgniteState.NotIgnitable;
	}

	public EnumIgniteState OnTryIgniteBlock(EntityAgent byEntity, BlockPos pos, float secondsIgniting)
	{
		if (!(byEntity.World.BlockAccessor.GetBlockEntity(pos) is BlockEntityGroundStorage { CanIgnite: not false }))
		{
			return EnumIgniteState.NotIgnitablePreventDefault;
		}
		if (secondsIgniting > 0.25f && (int)(30f * secondsIgniting) % 9 == 1)
		{
			Random rand = byEntity.World.Rand;
			Vec3d dpos = new Vec3d((double)((float)pos.X + 0.25f) + 0.5 * rand.NextDouble(), (float)pos.InternalY + 0.875f, (double)((float)pos.Z + 0.25f) + 0.5 * rand.NextDouble());
			Block blockFire = byEntity.World.GetBlock(new AssetLocation("fire"));
			AdvancedParticleProperties props = blockFire.ParticleProperties[blockFire.ParticleProperties.Length - 1];
			props.basePos = dpos;
			props.Quantity.avg = 1f;
			IPlayer byPlayer = null;
			if (byEntity is EntityPlayer)
			{
				byPlayer = byEntity.World.PlayerByUid(((EntityPlayer)byEntity).PlayerUID);
			}
			byEntity.World.SpawnParticles(props, byPlayer);
			props.Quantity.avg = 0f;
		}
		if (secondsIgniting >= 1.5f)
		{
			return EnumIgniteState.IgniteNow;
		}
		return EnumIgniteState.Ignitable;
	}

	public void OnTryIgniteBlockOver(EntityAgent byEntity, BlockPos pos, float secondsIgniting, ref EnumHandling handling)
	{
		if (!(secondsIgniting < 1.45f))
		{
			handling = EnumHandling.PreventDefault;
			IPlayer byPlayer = null;
			if (byEntity is EntityPlayer)
			{
				byPlayer = byEntity.World.PlayerByUid(((EntityPlayer)byEntity).PlayerUID);
			}
			if (byPlayer != null)
			{
				(byEntity.World.BlockAccessor.GetBlockEntity(pos) as BlockEntityGroundStorage)?.TryIgnite();
			}
		}
	}

	public override bool ShouldReceiveClientParticleTicks(IWorldAccessor world, IPlayer player, BlockPos pos, out bool isWindAffected)
	{
		if (!base.ShouldReceiveClientParticleTicks(world, player, pos, out isWindAffected))
		{
			return world.BlockAccessor.GetBlockEntity<BlockEntityGroundStorage>(pos)?.Inventory.Any((ItemSlot slot) => slot.Itemstack?.Collectible?.GetCollectibleInterface<IGroundStoredParticleEmitter>() != null) ?? false;
		}
		return true;
	}

	public override void OnAsyncClientParticleTick(IAsyncParticleManager manager, BlockPos pos, float windAffectednessAtPos, float secondsTicking)
	{
		if (manager.BlockAccess.GetBlockEntity(pos) is BlockEntityGroundStorage { StorageProps: not null } begs && !begs.Inventory.Empty)
		{
			Vec3f[] offs = new Vec3f[begs.DisplayedItems];
			begs.GetLayoutOffset(offs);
			foreach (ItemSlot slot in begs.Inventory)
			{
				IGroundStoredParticleEmitter gsParticleEmitter = slot?.Itemstack?.Collectible.GetCollectibleInterface<IGroundStoredParticleEmitter>();
				if (gsParticleEmitter == null)
				{
					continue;
				}
				int slotId = begs.Inventory.GetSlotId(slot);
				if (slotId >= 0 && slotId < offs.Length)
				{
					Vec3f offset = new Matrixf().RotateY(begs.MeshAngle).TransformVector(new Vec4f(offs[slotId].X, offs[slotId].Y, offs[slotId].Z, 1f)).XYZ;
					if (gsParticleEmitter.ShouldSpawnGSParticles(begs.Api.World, slot.Itemstack))
					{
						gsParticleEmitter.DoSpawnGSParticles(manager, pos, offset);
					}
				}
			}
		}
		base.OnAsyncClientParticleTick(manager, pos, windAffectednessAtPos, secondsTicking);
	}
}
