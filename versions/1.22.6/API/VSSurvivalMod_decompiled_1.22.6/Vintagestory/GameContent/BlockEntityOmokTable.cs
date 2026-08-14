using System;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent;

public class BlockEntityOmokTable : BlockEntityDisplay
{
	private InventoryGeneric inv;

	private int size = 15;

	public override InventoryBase Inventory => inv;

	public override string InventoryClassName => "omoktable";

	public override string AttributeTransformCode => "onOmokTransform";

	public float MeshAngleRad { get; set; }

	public BlockEntityOmokTable()
	{
		inv = new InventoryGeneric(size * size, "omoktable-0", null);
	}

	public override void Initialize(ICoreAPI api)
	{
		base.Initialize(api);
	}

	internal bool OnInteract(IPlayer byPlayer, BlockSelection blockSel)
	{
		ItemSlot slot = byPlayer.InventoryManager.ActiveHotbarSlot;
		CollectibleObject colObj = slot.Itemstack?.Collectible;
		bool placeable = colObj?.Attributes != null && colObj.Attributes["omokpiece"].AsBool();
		if (slot.Empty || !placeable)
		{
			if (TryTake(byPlayer, blockSel))
			{
				return true;
			}
			return false;
		}
		if (placeable)
		{
			SoundAttributes sound = slot.Itemstack?.Block?.Sounds?.Place ?? GlobalConstants.DefaultBuildSound;
			if (TryPut(slot, blockSel))
			{
				Api.World.PlaySoundAt(sound, byPlayer.Entity, byPlayer);
				return true;
			}
			return false;
		}
		return false;
	}

	private bool TryPut(ItemSlot slot, BlockSelection blockSel)
	{
		int index = blockSel.SelectionBoxIndex;
		if (index < 0 || index >= inv.Count)
		{
			return false;
		}
		if (inv[index].Empty)
		{
			int num = slot.TryPutInto(Api.World, inv[index]);
			MarkDirty();
			return num > 0;
		}
		return false;
	}

	private bool TryTake(IPlayer byPlayer, BlockSelection blockSel)
	{
		int index = blockSel.SelectionBoxIndex;
		if (index < 0 || index >= inv.Count)
		{
			return false;
		}
		if (!inv[index].Empty)
		{
			ItemStack stack = inv[index].TakeOut(1);
			if (byPlayer.InventoryManager.TryGiveItemstack(stack))
			{
				SoundAttributes sound = stack.Block?.Sounds?.Place ?? GlobalConstants.DefaultBuildSound;
				Api.World.PlaySoundAt(sound, byPlayer.Entity, byPlayer);
			}
			if (stack.StackSize > 0)
			{
				Api.World.SpawnItemEntity(stack, Pos);
			}
			MarkDirty();
			return true;
		}
		return false;
	}

	public override void ToTreeAttributes(ITreeAttribute tree)
	{
		base.ToTreeAttributes(tree);
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
	{
		base.FromTreeAttributes(tree, worldForResolving);
		RedrawAfterReceivingTreeAttributes(worldForResolving);
	}

	public override bool OnTesselation(ITerrainMeshPool mesher, ITesselatorAPI tessThreadTesselator)
	{
		Matrixf mat = new Matrixf();
		for (int i = 0; i < size * size; i++)
		{
			ItemSlot slot = Inventory[i];
			if (!slot.Empty)
			{
				mat.Identity();
				int dx = i % size;
				int dz = i / size;
				mat.Translate((0.6f + (float)dx) / 16f, 0f, (0.6f + (float)dz) / 16f);
				mesher.AddMeshData(getMesh(slot), mat.Values);
			}
		}
		return false;
	}

	public override void MarkMeshesDirty()
	{
		for (int i = 0; i < DisplayedItems; i++)
		{
			updateMesh(i);
		}
	}

	protected override float[][] genTransformationMatrices()
	{
		throw new NotImplementedException();
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder dsc)
	{
	}
}
