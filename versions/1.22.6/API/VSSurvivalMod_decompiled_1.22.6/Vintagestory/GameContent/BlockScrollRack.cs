using System;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockScrollRack : BlockShapeMaterialFromAttributes
{
	public Cuboidf[] slotsHitBoxes;

	public string[] slotSide;

	public int[] oppositeSlotIndex;

	public Dictionary<string, int[]> slotsBySide = new Dictionary<string, int[]>();

	public override string MeshKey { get; } = "ScrollrackMeshes";

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		LoadTypes();
	}

	public override void LoadTypes()
	{
		base.LoadTypes();
		slotsHitBoxes = Attributes["slotsHitBoxes"].AsObject<Cuboidf[]>();
		ArgumentNullException.ThrowIfNull(slotsHitBoxes, "slotsHitBoxes");
		slotSide = Attributes["slotSide"].AsObject<string[]>();
		ArgumentNullException.ThrowIfNull(slotSide, "slotSide");
		oppositeSlotIndex = Attributes["oppositeSlotIndex"].AsObject<int[]>();
		ArgumentNullException.ThrowIfNull(oppositeSlotIndex, "oppositeSlotIndex");
		for (int i = 0; i < slotSide.Length; i++)
		{
			string side = slotSide[i];
			int[] slots = ((!slotsBySide.TryGetValue(side, out slots)) ? new int[1] { i } : slots.Append(i));
			slotsBySide[side] = slots;
		}
	}

	public override Cuboidf[] GetSelectionBoxes(IBlockAccessor blockAccessor, BlockPos pos)
	{
		return GetBlockEntity<BlockEntityScrollRack>(pos)?.getOrCreateSelectionBoxes() ?? base.GetSelectionBoxes(blockAccessor, pos);
	}

	public override void GetDecal(IWorldAccessor world, BlockPos pos, ITexPositionSource decalTexSource, ref MeshData decalModelData, ref MeshData blockModelData)
	{
		BlockEntityScrollRack beb = GetBlockEntity<BlockEntityScrollRack>(pos);
		if (beb != null)
		{
			float[] mat = Matrixf.Create().Translate(0.5f, 0.5f, 0.5f).RotateY(beb.MeshAngleRad)
				.Translate(-0.5f, -0.5f, -0.5f)
				.Values;
			blockModelData = GetOrCreateMesh(beb.Type, beb.Material).Clone().MatrixTransform(mat);
			decalModelData = GetOrCreateMesh(beb.Type, beb.Material, null, decalTexSource).Clone().MatrixTransform(mat);
		}
		else
		{
			base.GetDecal(world, pos, decalTexSource, ref decalModelData, ref blockModelData);
		}
	}

	public override void OnNeighbourBlockChange(IWorldAccessor world, BlockPos pos, BlockPos neibpos)
	{
		GetBlockEntity<BlockEntityScrollRack>(pos)?.clearUsableSlots();
		base.OnNeighbourBlockChange(world, pos, neibpos);
	}

	public override bool DoPartialSelection(IWorldAccessor world, BlockPos pos)
	{
		return true;
	}

	public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
	{
		if (world.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityScrollRack beshelf)
		{
			return beshelf.OnInteract(byPlayer, blockSel);
		}
		return base.OnBlockInteractStart(world, byPlayer, blockSel);
	}

	public override string GetHeldItemName(ItemStack itemStack)
	{
		return Lang.Get("block-scrollrack-" + itemStack.Attributes.GetString("material"));
	}
}
