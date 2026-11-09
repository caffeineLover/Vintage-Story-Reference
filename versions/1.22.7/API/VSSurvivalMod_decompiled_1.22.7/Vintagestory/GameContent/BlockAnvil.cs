using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockAnvil : Block
{
	public int MetalTier { get; private set; }

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		if (api.ModLoader.GetModSystem<SurvivalCoreSystem>().metalsByCode.TryGetValue(Variant["metal"] ?? LastCodePart(), out var metalVariant))
		{
			MetalTier = metalVariant.Tier;
		}
	}

	public override void OnDecalTesselation(IWorldAccessor world, MeshData decalMesh, BlockPos pos)
	{
		base.OnDecalTesselation(world, decalMesh, pos);
		if (world.BlockAccessor.GetBlockEntity(pos) is BlockEntityAnvil bect)
		{
			decalMesh.Rotate(0f, bect.MeshAngle, 0f);
		}
	}

	public override Cuboidf[] GetSelectionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
	{
		if (blockAccessor.GetBlockEntity(pos) is BlockEntityAnvil bea)
		{
			Cuboidf[] selectionBoxes = bea.GetSelectionBoxes(blockAccessor, pos);
			float angledeg = Math.Abs(bea.MeshAngle * (180f / (float)Math.PI));
			selectionBoxes[0] = ((angledeg == 0f || angledeg == 180f) ? SelectionBoxes[0] : SelectionBoxes[1]);
			return selectionBoxes;
		}
		return base.GetSelectionBoxes(blockAccessor, pos);
	}

	public override Cuboidf[] GetCollisionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
	{
		return GetSelectionBoxes(blockAccessor, pos);
	}

	public override Cuboidf[] GetParticleCollisionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
	{
		return GetSelectionBoxes(blockAccessor, pos);
	}

	public override bool DoPartialSelection(IWorldAccessor world, BlockPos pos)
	{
		return true;
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		if (world.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityAnvil bea)
		{
			if (bea.OnPlayerInteract(world, byPlayer, blockSel))
			{
				return true;
			}
			return false;
		}
		return base.OnBlockInteractStart(world, byPlayer, blockSel);
	}

	public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
	{
		return ObjectCacheUtil.GetOrCreate(api, "anvilBlockInteractions" + MetalTier, delegate
		{
			List<ItemStack> list = new List<ItemStack>();
			List<ItemStack> list2 = new List<ItemStack>();
			foreach (Item current in api.World.Items)
			{
				if (!(current.Code == null))
				{
					IAnvilWorkable workable = current.GetCollectibleInterface<IAnvilWorkable>();
					if (workable != null)
					{
						list.AddRange(current.GetHandBookStacks(api as ICoreClientAPI)?.Where((ItemStack stack) => workable.GetRequiredAnvilTier(stack) <= MetalTier) ?? Array.Empty<ItemStack>());
					}
					if (current is ItemHammer)
					{
						list2.Add(new ItemStack(current));
					}
				}
			}
			return new WorldInteraction[6]
			{
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-anvil-takeworkable",
					MouseButton = EnumMouseButton.Right,
					ShouldApply = (WorldInteraction wi, BlockSelection bs, EntitySelection es) => GetBlockEntity<BlockEntityAnvil>(bs.Position)?.WorkItemStack != null
				},
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-anvil-placeworkable",
					HotKeyCode = "shift",
					MouseButton = EnumMouseButton.Right,
					Itemstacks = list.ToArray(),
					GetMatchingStacks = (WorldInteraction wi, BlockSelection bs, EntitySelection es) => (GetBlockEntity<BlockEntityAnvil>(bs.Position)?.WorkItemStack != null) ? null : wi.Itemstacks
				},
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-anvil-smith",
					MouseButton = EnumMouseButton.Left,
					Itemstacks = list2.ToArray(),
					GetMatchingStacks = (WorldInteraction wi, BlockSelection bs, EntitySelection es) => (GetBlockEntity<BlockEntityAnvil>(bs.Position)?.WorkItemStack != null) ? wi.Itemstacks : null
				},
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-anvil-rotateworkitem",
					MouseButton = EnumMouseButton.Right,
					Itemstacks = list2.ToArray(),
					GetMatchingStacks = (WorldInteraction wi, BlockSelection bs, EntitySelection es) => (GetBlockEntity<BlockEntityAnvil>(bs.Position)?.WorkItemStack != null) ? wi.Itemstacks : null
				},
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-selecttoolmode",
					HotKeyCode = "toolmodeselect",
					MouseButton = EnumMouseButton.None,
					Itemstacks = list2.ToArray(),
					GetMatchingStacks = (WorldInteraction wi, BlockSelection bs, EntitySelection es) => (GetBlockEntity<BlockEntityAnvil>(bs.Position)?.WorkItemStack != null) ? wi.Itemstacks : null
				},
				new WorldInteraction
				{
					ActionLangCode = "blockhelp-anvil-addvoxels",
					HotKeyCode = "shift",
					MouseButton = EnumMouseButton.Right,
					Itemstacks = list.ToArray(),
					GetMatchingStacks = delegate(WorldInteraction wi, BlockSelection bs, EntitySelection es)
					{
						BlockEntityAnvil blockEntityAnvil = api.World.BlockAccessor.GetBlockEntity(bs.Position) as BlockEntityAnvil;
						return (blockEntityAnvil?.WorkItemStack != null) ? new ItemStack[1] { blockEntityAnvil.WorkItemStack.Collectible.GetCollectibleInterface<IAnvilWorkable>().GetBaseMaterial(blockEntityAnvil.WorkItemStack) } : null;
					}
				}
			};
		}).Append(base.GetPlacedBlockInteractionHelp(world, selection, forPlayer));
	}

	public override bool DoPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ItemStack byItemStack)
	{
		bool num = base.DoPlaceBlock(world, byPlayer, blockSel, byItemStack);
		if (num && world.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityAnvil bect)
		{
			BlockPos targetPos = (blockSel.DidOffset ? blockSel.Position.AddCopy(blockSel.Face.Opposite) : blockSel.Position);
			double y = byPlayer.Entity.Pos.X - ((double)targetPos.X + blockSel.HitPosition.X);
			double dz = byPlayer.Entity.Pos.Z - ((double)targetPos.Z + blockSel.HitPosition.Z);
			float num2 = (float)Math.Atan2(y, dz);
			float deg22dot5rad = (float)Math.PI / 8f;
			float roundRad = (float)(int)Math.Round(num2 / deg22dot5rad) * deg22dot5rad;
			bect.MeshAngle = roundRad;
		}
		return num;
	}
}
