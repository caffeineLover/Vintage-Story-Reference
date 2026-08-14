using System;
using System.Text;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;

namespace Vintagestory.GameContent;

public class BlockEntityAntlerMount : BlockEntityDisplay
{
	private InventoryGeneric inv;

	private BEBehaviorShapeMaterialFromAttributes? bh;

	public override InventoryBase Inventory => inv;

	public override string InventoryClassName => "antlermount";

	public override string AttributeTransformCode => "onAntlerMountTransform";

	public float MeshAngleRad
	{
		get
		{
			return bh?.MeshAngleY ?? 0f;
		}
		set
		{
			bh.MeshAngleY = value;
		}
	}

	public string? Type => bh?.Type;

	public string? Material => bh?.Material;

	public BlockEntityAntlerMount()
	{
		inv = new InventoryGeneric(1, "antlermount-0", null);
	}

	public override void Initialize(ICoreAPI api)
	{
		bh = GetBehavior<BEBehaviorShapeMaterialFromAttributes>();
		base.Initialize(api);
	}

	internal bool OnInteract(IPlayer byPlayer, BlockSelection blockSel)
	{
		ItemSlot slot = byPlayer.InventoryManager.ActiveHotbarSlot;
		CollectibleObject colObj = slot.Itemstack?.Collectible;
		bool shelvable = colObj?.Attributes != null && colObj.Attributes["antlerMountable"].AsBool();
		if (slot.Empty || !shelvable)
		{
			if (TryTake(byPlayer, blockSel))
			{
				return true;
			}
			return false;
		}
		if (shelvable)
		{
			SoundAttributes? sound = slot.Itemstack?.Block?.Sounds?.Place;
			AssetLocation stackCode = slot.Itemstack?.Collectible.Code;
			if (TryPut(slot))
			{
				Api.World.PlaySoundAt(sound ?? GlobalConstants.DefaultBuildSound, byPlayer.Entity, byPlayer);
				Api.World.Logger.Audit("{0} Put 1x{1} on to AntlerMount at {2}.", byPlayer.PlayerName, stackCode, blockSel.Position);
				return true;
			}
		}
		return false;
	}

	private bool TryPut(ItemSlot slot)
	{
		int invIndex = 0;
		if (inv[invIndex].Empty)
		{
			int num = slot.TryPutInto(Api.World, inv[invIndex]);
			MarkDirty();
			return num > 0;
		}
		return false;
	}

	private bool TryTake(IPlayer byPlayer, BlockSelection blockSel)
	{
		int invIndex = 0;
		if (!inv[invIndex].Empty)
		{
			ItemStack stack = inv[invIndex].TakeOut(1);
			if (byPlayer.InventoryManager.TryGiveItemstack(stack))
			{
				SoundAttributes? obj = stack.Block?.Sounds?.Place;
				Api.World.PlaySoundAt(obj ?? GlobalConstants.DefaultBuildSound, byPlayer.Entity, byPlayer);
			}
			if (stack.StackSize > 0)
			{
				Api.World.SpawnItemEntity(stack, Pos);
			}
			Api.World.Logger.Audit("{0} Took 1x{1} from AntlerMount at {2}.", byPlayer.PlayerName, stack.Collectible.Code, blockSel.Position);
			MarkDirty();
			return true;
		}
		return false;
	}

	protected override float[][] genTransformationMatrices()
	{
		tfMatrices = new float[Inventory.Count][];
		for (int i = 0; i < Inventory.Count; i++)
		{
			tfMatrices[i] = new Matrixf().Translate(0.5f, 0f, 0.5f).RotateY(MeshAngleRad - (float)Math.PI / 2f).Translate(-0.5f, 0f, -0.5f)
				.Values;
		}
		return tfMatrices;
	}

	public override void FromTreeAttributes(ITreeAttribute tree, IWorldAccessor worldForResolving)
	{
		base.FromTreeAttributes(tree, worldForResolving);
		bh?.Init();
		RedrawAfterReceivingTreeAttributes(worldForResolving);
	}

	public override void GetBlockInfo(IPlayer forPlayer, StringBuilder sb)
	{
		if (forPlayer.CurrentBlockSelection == null)
		{
			base.GetBlockInfo(forPlayer, sb);
			return;
		}
		int index = 0;
		if (index >= inv.Count)
		{
			base.GetBlockInfo(forPlayer, sb);
		}
		else if (inv[index].Empty)
		{
			sb.AppendLine(Lang.Get("Empty"));
		}
	}
}
