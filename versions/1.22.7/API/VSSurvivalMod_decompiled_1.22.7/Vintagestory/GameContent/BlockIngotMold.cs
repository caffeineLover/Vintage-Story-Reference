using System;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockIngotMold : Block
{
	private WorldInteraction[] interactionsLeft;

	private WorldInteraction[] interactionsRight;

	private readonly Cuboidf[] oneMoldBoxes = new Cuboidf[1]
	{
		new Cuboidf(0f, 0f, 0f, 1f, 0.1875f, 1f)
	};

	private readonly Cuboidf[] twoMoldBoxesNS = new Cuboidf[2]
	{
		new Cuboidf(0f, 0f, 0f, 0.5f, 0.1875f, 1f),
		new Cuboidf(0.5f, 0f, 0f, 1f, 0.1875f, 1f)
	};

	private readonly Cuboidf[] twoMoldBoxesEW = new Cuboidf[2]
	{
		new Cuboidf(0f, 0f, 0f, 1f, 0.1875f, 0.5f),
		new Cuboidf(0f, 0f, 0.5f, 1f, 0.1875f, 1f)
	};

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		if (api.Side != EnumAppSide.Client || LastCodePart() == "raw")
		{
			return;
		}
		interactionsLeft = ObjectCacheUtil.GetOrCreate(api, "ingotmoldBlockInteractionsLeft", delegate
		{
			ItemStack[] itemstacks = (from block in api.World.Blocks
				where block is BlockSmeltedContainer
				select new ItemStack(block)).ToArray();
			return new WorldInteraction[5]
			{
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-ingotmold-pour",
					HotKeyCode = "shift",
					MouseButton = EnumMouseButton.Right,
					Itemstacks = itemstacks,
					GetMatchingStacks = (WorldInteraction wi, BlockSelection bs, EntitySelection es) => (!(api.World.BlockAccessor.GetBlockEntity(bs.Position) is BlockEntityIngotMold { IsFullLeft: false, ShatteredLeft: false })) ? null : wi.Itemstacks
				},
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-ingotmold-takeingot",
					HotKeyCode = null,
					MouseButton = EnumMouseButton.Right,
					ShouldApply = (WorldInteraction wi, BlockSelection bs, EntitySelection es) => api.World.BlockAccessor.GetBlockEntity(bs.Position) is BlockEntityIngotMold { IsFullLeft: not false, IsHardenedLeft: not false } blockEntityIngotMold && !blockEntityIngotMold.ShatteredLeft
				},
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-ingotmold-chiselmoldforbits",
					HotKeyCode = null,
					MouseButton = EnumMouseButton.Left,
					Itemstacks = ObjectCacheUtil.GetToolStacks(api, EnumTool.Chisel),
					GetMatchingStacks = (WorldInteraction wi, BlockSelection bs, EntitySelection es) => (!(api.World.BlockAccessor.GetBlockEntity(bs.Position) is BlockEntityIngotMold { FillLevelLeft: >0, IsHardenedLeft: not false, ShatteredLeft: false })) ? null : wi.Itemstacks
				},
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-ingotmold-pickup",
					HotKeyCode = null,
					RequireFreeHand = true,
					MouseButton = EnumMouseButton.Right,
					ShouldApply = (WorldInteraction wi, BlockSelection bs, EntitySelection es) => api.World.BlockAccessor.GetBlockEntity(bs.Position) is BlockEntityIngotMold { ContentsLeft: null } blockEntityIngotMold && !blockEntityIngotMold.ShatteredLeft
				},
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-ingotmold-placemold",
					HotKeyCode = "shift",
					Itemstacks = new ItemStack[1]
					{
						new ItemStack(this)
					},
					MouseButton = EnumMouseButton.Right,
					GetMatchingStacks = (WorldInteraction wi, BlockSelection bs, EntitySelection es) => (!(api.World.BlockAccessor.GetBlockEntity(bs.Position) is BlockEntityIngotMold { QuantityMolds: <2 })) ? null : wi.Itemstacks
				}
			};
		});
		interactionsRight = ObjectCacheUtil.GetOrCreate(api, "ingotmoldBlockInteractionsRight", delegate
		{
			ItemStack[] itemstacks = (from block in api.World.Blocks
				where block is BlockSmeltedContainer
				select new ItemStack(block)).ToArray();
			return new WorldInteraction[4]
			{
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-ingotmold-pour",
					HotKeyCode = "shift",
					MouseButton = EnumMouseButton.Right,
					Itemstacks = itemstacks,
					GetMatchingStacks = (WorldInteraction wi, BlockSelection bs, EntitySelection es) => (!(api.World.BlockAccessor.GetBlockEntity(bs.Position) is BlockEntityIngotMold { QuantityMolds: >1, IsFullRight: false, ShatteredRight: false })) ? null : wi.Itemstacks
				},
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-ingotmold-takeingot",
					HotKeyCode = null,
					MouseButton = EnumMouseButton.Right,
					ShouldApply = (WorldInteraction wi, BlockSelection bs, EntitySelection es) => api.World.BlockAccessor.GetBlockEntity(bs.Position) is BlockEntityIngotMold { QuantityMolds: >1, IsFullRight: not false, IsHardenedRight: not false } blockEntityIngotMold && !blockEntityIngotMold.ShatteredRight
				},
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-ingotmold-chiselmoldforbits",
					HotKeyCode = null,
					MouseButton = EnumMouseButton.Left,
					Itemstacks = ObjectCacheUtil.GetToolStacks(api, EnumTool.Chisel),
					GetMatchingStacks = (WorldInteraction wi, BlockSelection bs, EntitySelection es) => (!(api.World.BlockAccessor.GetBlockEntity(bs.Position) is BlockEntityIngotMold { QuantityMolds: >1, FillLevelRight: >0, IsHardenedRight: not false, ShatteredRight: false })) ? null : wi.Itemstacks
				},
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-ingotmold-pickup",
					HotKeyCode = null,
					RequireFreeHand = true,
					MouseButton = EnumMouseButton.Right,
					ShouldApply = (WorldInteraction wi, BlockSelection bs, EntitySelection es) => api.World.BlockAccessor.GetBlockEntity(bs.Position) is BlockEntityIngotMold { QuantityMolds: >1, ContentsRight: null } blockEntityIngotMold && !blockEntityIngotMold.ShatteredRight
				}
			};
		});
	}

	public override bool DoPartialSelection(IWorldAccessor world, BlockPos pos)
	{
		return true;
	}

	public override Cuboidf[] GetSelectionBoxes(IBlockAccessor world, BlockPos pos)
	{
		if (!(api.World.BlockAccessor.GetBlockEntity(pos) is BlockEntityIngotMold { QuantityMolds: not 1 } beim))
		{
			return oneMoldBoxes;
		}
		int index = BlockFacing.HorizontalFromAngle(beim.MeshAngle).Index;
		if (index == 0 || index == 2)
		{
			return twoMoldBoxesEW;
		}
		return twoMoldBoxesNS;
	}

	public override Cuboidf[] GetCollisionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
	{
		return GetSelectionBoxes(blockAccessor, pos);
	}

	public override void GetDecal(IWorldAccessor world, BlockPos pos, ITexPositionSource decalTexSource, ref MeshData decalModelData, ref MeshData blockModelData)
	{
		BlockEntityIngotMold be = GetBlockEntity<BlockEntityIngotMold>(pos);
		if (be != null)
		{
			blockModelData = be.GetCurrentDecalMesh(be);
			decalModelData = be.GetCurrentDecalMesh(decalTexSource);
			if (be.QuantityMolds == 2)
			{
				Vec3f side = (be.IsRightSideSelected ? BlockEntityIngotMold.right : BlockEntityIngotMold.left);
				blockModelData.Translate(side);
				decalModelData.Translate(side);
			}
			blockModelData.Rotate(Vec3f.Half, 0f, be.MeshAngle, 0f);
			decalModelData.Rotate(Vec3f.Half, 0f, be.MeshAngle, 0f);
		}
		else
		{
			base.GetDecal(world, pos, decalTexSource, ref decalModelData, ref blockModelData);
		}
	}

	public override void OnHeldInteractStart(ItemSlot itemslot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
	{
		if (blockSel == null)
		{
			base.OnHeldInteractStart(itemslot, byEntity, blockSel, entitySel, firstEvent, ref handling);
		}
		else if (byEntity.World.BlockAccessor.GetBlockEntity(blockSel.Position.AddCopy(blockSel.Face.Opposite)) is BlockEntityIngotMold beim)
		{
			IPlayer byPlayer = byEntity.World.PlayerByUid((byEntity as EntityPlayer)?.PlayerUID);
			if (byPlayer != null && beim.OnPlayerInteract(byPlayer, blockSel.Face, blockSel.HitPosition))
			{
				handling = EnumHandHandling.PreventDefault;
			}
		}
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		if (world.BlockAccessor.GetBlockEntity(blockSel?.Position) is BlockEntityIngotMold beim)
		{
			return beim.OnPlayerInteract(byPlayer, blockSel.Face, blockSel.HitPosition);
		}
		return base.OnBlockInteractStart(world, byPlayer, blockSel);
	}

	public override void OnEntityCollide(IWorldAccessor world, Entity entity, BlockPos pos, BlockFacing facing, Vec3d collideSpeed, bool isImpact)
	{
		if (world.Rand.NextDouble() > 0.05)
		{
			base.OnEntityCollide(world, entity, pos, facing, collideSpeed, isImpact);
			return;
		}
		BlockEntityIngotMold beim = GetBlockEntity<BlockEntityIngotMold>(pos);
		if ((beim != null && beim.TemperatureLeft > 300f) || (beim != null && beim.TemperatureRight > 300f))
		{
			entity.ReceiveDamage(new DamageSource
			{
				Source = EnumDamageSource.Block,
				SourceBlock = this,
				Type = EnumDamageType.Fire,
				SourcePos = pos.ToVec3d()
			}, 0.5f);
		}
		base.OnEntityCollide(world, entity, pos, facing, collideSpeed, isImpact);
	}

	public override float GetTraversalCost(BlockPos pos, EnumAICreatureType creatureType)
	{
		if (creatureType == EnumAICreatureType.LandCreature || creatureType == EnumAICreatureType.Humanoid)
		{
			BlockEntityIngotMold beim = GetBlockEntity<BlockEntityIngotMold>(pos);
			if ((beim != null && beim.TemperatureLeft > 300f) || (beim != null && beim.TemperatureRight > 300f))
			{
				return 10000f;
			}
		}
		return 0f;
	}

	public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
	{
		if (!byPlayer.Entity.Controls.ShiftKey)
		{
			failureCode = "onlywhensneaking";
			return false;
		}
		if (!world.BlockAccessor.GetBlock(blockSel.Position.DownCopy()).CanAttachBlockAt(world.BlockAccessor, this, blockSel.Position.DownCopy(), BlockFacing.UP))
		{
			failureCode = "requiresolidground";
			return false;
		}
		return base.TryPlaceBlock(world, byPlayer, itemstack, blockSel, ref failureCode);
	}

	public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos)
	{
		return GetBlockEntity<BlockEntityIngotMold>(pos)?.SelectedMold ?? base.OnPickBlock(world, pos);
	}

	public override BlockSounds GetSounds(IBlockAccessor blockAccessor, BlockSelection blockSel, ItemStack? stack = null)
	{
		return GetBlockEntity<BlockEntityIngotMold>(blockSel.Position)?.SelectedMold?.Block?.Sounds ?? base.GetSounds(blockAccessor, blockSel, stack);
	}

	public override float OnGettingBroken(IPlayer player, BlockSelection blockSel, ItemSlot itemslot, float remainingResistance, float dt, int counter)
	{
		BlockEntityIngotMold blockEntity = GetBlockEntity<BlockEntityIngotMold>(blockSel.Position);
		IPlayerInventoryManager invMan = player?.InventoryManager;
		int beingChiseled;
		if (invMan != null)
		{
			EnumTool? offhandTool = invMan.OffhandTool;
			if (offhandTool.HasValue && offhandTool == EnumTool.Hammer)
			{
				offhandTool = invMan.ActiveTool;
				beingChiseled = ((offhandTool.HasValue && offhandTool == EnumTool.Chisel) ? 1 : 0);
				goto IL_0057;
			}
		}
		beingChiseled = 0;
		goto IL_0057;
		IL_0057:
		blockEntity.BeingChiseled = (byte)beingChiseled != 0;
		return base.OnGettingBroken(player, blockSel, itemslot, remainingResistance, dt, counter);
	}

	public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
	{
		if (world.BlockAccessor.GetBlockEntity(pos) is BlockEntityIngotMold beim && (byPlayer == null || byPlayer.WorldData.CurrentGameMode != EnumGameMode.Creative))
		{
			BlockSelection blockSel = byPlayer?.CurrentBlockSelection;
			if (blockSel != null)
			{
				beim.SetSelectedSide(blockSel.HitPosition);
				IPlayerInventoryManager invMan = byPlayer?.InventoryManager;
				if (invMan != null)
				{
					EnumTool? offhandTool = invMan.OffhandTool;
					if (offhandTool.HasValue && offhandTool == EnumTool.Hammer)
					{
						offhandTool = invMan.ActiveTool;
						if (offhandTool.HasValue && offhandTool == EnumTool.Chisel)
						{
							ItemStack drop = beim.GetChiseledStack(beim.SelectedContents, beim.SelectedFillLevel, beim.SelectedShattered, beim.SelectedIsHardened);
							if (drop != null)
							{
								if (SplitDropStacks)
								{
									for (int k = 0; k < drop.StackSize; k++)
									{
										ItemStack stack = drop.Clone();
										stack.StackSize = 1;
										world.SpawnItemEntity(stack, pos);
									}
								}
								else
								{
									world.SpawnItemEntity(drop, pos);
								}
								world.PlaySoundAt(new AssetLocation("sounds/block/ingot"), pos, 0.0, byPlayer);
								beim.SelectedContents = null;
								beim.SelectedFillLevel = 0;
								DamageItem(world, byPlayer.Entity, invMan.ActiveHotbarSlot);
								DamageItem(world, byPlayer.Entity, byPlayer.Entity?.LeftHandItemSlot);
								return;
							}
							goto IL_032a;
						}
					}
				}
				ItemStack[] drops = beim.GetStateAwareMoldSided(beim.SelectedMold, beim.SelectedShattered);
				if (drops.Length != 0)
				{
					ItemStack[] array = drops;
					foreach (ItemStack drop2 in array)
					{
						if (SplitDropStacks)
						{
							for (int j = 0; j < drop2.StackSize; j++)
							{
								ItemStack stack2 = drop2.Clone();
								stack2.StackSize = 1;
								world.SpawnItemEntity(stack2, pos);
							}
						}
						else
						{
							world.SpawnItemEntity(drop2, pos);
						}
					}
					if (!beim.IsRightSideSelected && beim.QuantityMolds > 1)
					{
						if (beim.MoldRight == null)
						{
							beim.QuantityMolds--;
							beim.ContentsRight = null;
							beim.FillLevelRight = 0;
							beim.ShatteredRight = false;
							return;
						}
						beim.MoldLeft = beim.MoldRight;
						beim.MoldMeshLeft = beim.MoldMeshRight;
						beim.ContentsLeft = beim.ContentsRight;
						beim.FillLevelLeft = beim.FillLevelRight;
						beim.ShatteredLeft = beim.ShatteredRight;
						world.BlockAccessor.ExchangeBlock(beim.MoldLeft.Block.BlockId, pos);
						beim.MoldRight = null;
						beim.ContentsRight = null;
						beim.FillLevelRight = 0;
						beim.ShatteredRight = false;
					}
					beim.QuantityMolds--;
					if (beim.QuantityMolds <= 0)
					{
						world.BlockAccessor.SetBlock(0, pos);
					}
					else
					{
						beim.MoldRight = null;
						beim.MarkDirty(redrawOnClient: true);
					}
					BlockSounds sounds = beim.SelectedMold?.Block.Sounds;
					if (sounds != null)
					{
						world.PlaySoundAt(sounds.GetBreakSound(byPlayer), pos, 0.0, byPlayer);
					}
					return;
				}
			}
		}
		goto IL_032a;
		IL_032a:
		base.OnBlockBroken(world, pos, byPlayer, dropQuantityMultiplier);
	}

	public override BlockDropItemStack[] GetDropsForHandbook(ItemStack handbookStack, IPlayer forPlayer)
	{
		return Drops;
	}

	public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
	{
		if (!(world.BlockAccessor.GetBlockEntity(pos) is BlockEntityIngotMold beim))
		{
			return new ItemStack[1]
			{
				new ItemStack(this)
			};
		}
		ItemStack[] stateAwareMolds = beim.GetStateAwareMolds();
		ItemStack[] stateAwareMoldedStacks = beim.GetStateAwareMoldedStacks();
		int num = 0;
		ItemStack[] array = new ItemStack[stateAwareMolds.Length + stateAwareMoldedStacks.Length];
		ReadOnlySpan<ItemStack> readOnlySpan = new ReadOnlySpan<ItemStack>(stateAwareMolds);
		readOnlySpan.CopyTo(new Span<ItemStack>(array).Slice(num, readOnlySpan.Length));
		num += readOnlySpan.Length;
		ReadOnlySpan<ItemStack> readOnlySpan2 = new ReadOnlySpan<ItemStack>(stateAwareMoldedStacks);
		readOnlySpan2.CopyTo(new Span<ItemStack>(array).Slice(num, readOnlySpan2.Length));
		num += readOnlySpan2.Length;
		return array;
	}

	public override string GetPlacedBlockName(IWorldAccessor world, BlockPos pos)
	{
		if (world.BlockAccessor.GetBlockEntity(pos) is BlockEntityIngotMold beim)
		{
			if (beim.SelectedMold == null)
			{
				return base.GetPlacedBlockName(world, pos);
			}
			if (!beim.SelectedShattered)
			{
				return beim.SelectedMold.GetName();
			}
			return Lang.Get("ceramicblock-blockname-shattered", beim.SelectedMold.GetName());
		}
		return base.GetPlacedBlockName(world, pos);
	}

	public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
	{
		if (world.BlockAccessor.GetBlockEntity(selection.Position) is BlockEntityIngotMold beim)
		{
			beim.SetSelectedSide(selection.HitPosition);
			return (beim.IsRightSideSelected ? interactionsRight : interactionsLeft).Append<WorldInteraction>(base.GetPlacedBlockInteractionHelp(world, selection, forPlayer));
		}
		return ((selection.SelectionBoxIndex == 0) ? interactionsLeft : interactionsRight).Append<WorldInteraction>(base.GetPlacedBlockInteractionHelp(world, selection, forPlayer));
	}

	public override bool DoPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ItemStack byItemStack)
	{
		bool num = base.DoPlaceBlock(world, byPlayer, blockSel, byItemStack);
		if (num && world.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityIngotMold beim)
		{
			beim.MoldLeft = byItemStack?.Clone() ?? byPlayer.InventoryManager.ActiveHotbarSlot.Itemstack?.Clone() ?? new ItemStack(this);
			if (beim.MoldLeft != null)
			{
				beim.MoldLeft.StackSize = 1;
			}
			BlockPos targetPos = (blockSel.DidOffset ? blockSel.Position.AddCopy(blockSel.Face.Opposite) : blockSel.Position);
			double y = byPlayer.Entity.Pos.X - ((double)targetPos.X + blockSel.HitPosition.X);
			double dz = byPlayer.Entity.Pos.Z - ((double)targetPos.Z + blockSel.HitPosition.Z);
			float roundRad = (float)(int)Math.Round((float)Math.Atan2(y, dz) / ((float)Math.PI / 2f)) * ((float)Math.PI / 2f);
			beim.MeshAngle = roundRad;
			beim.MarkDirty();
		}
		return num;
	}
}
