using System;
using System.Text;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

[DocumentAsJson]
[AddDocumentationProperty("TriggerSound", "The sound to play when the trapdoor is opened or closed.", "System.String", "Optional", "sounds/block/door", true)]
public class BlockBehaviorTrapDoor : StrongBlockBehavior, IClaimTraverseable
{
	[DocumentAsJson("Optional", "sounds/block/door", true)]
	public AssetLocation OpenSound;

	[DocumentAsJson("Optional", "sounds/block/door", true)]
	public AssetLocation CloseSound;

	[DocumentAsJson("Optional", "True", true)]
	public bool handopenable;

	[DocumentAsJson("Optional", "True", true)]
	public bool airtight;

	private ICoreAPI api;

	public MeshData animatableOrigMesh;

	public Shape animatableShape;

	public string animatableDictKey;

	public BlockBehaviorTrapDoor(Block block)
		: base(block)
	{
		airtight = block.Attributes["airtight"].AsBool(defaultValue: true);
		handopenable = block.Attributes["handopenable"].AsBool(defaultValue: true);
	}

	public override void OnLoaded(ICoreAPI api)
	{
		this.api = api;
		OpenSound = (CloseSound = AssetLocation.Create(block.Attributes["triggerSound"].AsString("sounds/block/door")));
		if (block.Attributes["openSound"].Exists)
		{
			OpenSound = AssetLocation.Create(block.Attributes["openSound"].AsString("sounds/block/door"));
		}
		if (block.Attributes["closeSound"].Exists)
		{
			CloseSound = AssetLocation.Create(block.Attributes["closeSound"].AsString("sounds/block/door"));
		}
		base.OnLoaded(api);
	}

	public override void Activate(IWorldAccessor world, Caller caller, BlockSelection blockSel, ITreeAttribute activationArgs, ref EnumHandling handled)
	{
		BEBehaviorTrapDoor beh = world.BlockAccessor.GetBlockEntity(blockSel.Position)?.GetBehavior<BEBehaviorTrapDoor>();
		bool opened = !beh.Opened;
		if (activationArgs != null)
		{
			opened = activationArgs.GetBool("opened", opened);
		}
		if (beh.Opened != opened)
		{
			beh.ToggleDoorState(null, opened);
		}
	}

	public override void OnDecalTesselation(IWorldAccessor world, MeshData decalMesh, BlockPos pos, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventDefault;
		BEBehaviorTrapDoor beh = world.BlockAccessor.GetBlockEntity(pos)?.GetBehavior<BEBehaviorTrapDoor>();
		if (beh != null)
		{
			decalMesh.Rotate(0f, beh.RotRad, 0f);
		}
	}

	public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref EnumHandling handling, ref string failureCode)
	{
		handling = EnumHandling.PreventDefault;
		BlockPos pos = blockSel.Position;
		IBlockAccessor ba = world.BlockAccessor;
		if (ba.GetBlock(pos, 1).Id == 0 && block.CanPlaceBlock(world, byPlayer, blockSel, ref failureCode))
		{
			return placeDoor(world, byPlayer, itemstack, blockSel, pos, ba);
		}
		return false;
	}

	public bool placeDoor(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, BlockPos pos, IBlockAccessor ba)
	{
		ba.SetBlock(block.BlockId, pos);
		(ba.GetBlockEntity(pos)?.GetBehavior<BEBehaviorTrapDoor>()).OnBlockPlaced(itemstack, byPlayer, blockSel);
		return true;
	}

	public override Cuboidf[] GetCollisionBoxes(IBlockAccessor blockAccessor, BlockPos pos, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventSubsequent;
		return blockAccessor.GetBlockEntity(pos)?.GetBehavior<BEBehaviorTrapDoor>()?.ColSelBoxes ?? null;
	}

	public override Cuboidf[] GetSelectionBoxes(IBlockAccessor blockAccessor, BlockPos pos, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventSubsequent;
		return blockAccessor.GetBlockEntity(pos)?.GetBehavior<BEBehaviorTrapDoor>()?.ColSelBoxes ?? null;
	}

	public override Cuboidf[] GetParticleCollisionBoxes(IBlockAccessor blockAccessor, BlockPos pos, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventSubsequent;
		return blockAccessor.GetBlockEntity(pos)?.GetBehavior<BEBehaviorTrapDoor>()?.ColSelBoxes ?? null;
	}

	public override Cuboidf GetParticleBreakBox(IBlockAccessor blockAccess, BlockPos pos, BlockFacing facing, ref EnumHandling handled)
	{
		return base.GetParticleBreakBox(blockAccess, pos, facing, ref handled);
	}

	public override void GetDecal(IWorldAccessor world, BlockPos pos, ITexPositionSource decalTexSource, ref MeshData decalModelData, ref MeshData blockModelData, ref EnumHandling handled)
	{
		if ((world.BlockAccessor.GetBlockEntity(pos)?.GetBehavior<BEBehaviorTrapDoor>()).Opened)
		{
			decalModelData = decalModelData.Rotate((float)Math.PI / 2f, 0f, 0f);
			decalModelData = decalModelData.Scale(1f, -1f, 1f);
		}
		base.GetDecal(world, pos, decalTexSource, ref decalModelData, ref blockModelData, ref handled);
	}

	public override void GetHeldItemName(StringBuilder sb, ItemStack itemStack)
	{
		doorNameWithMaterial(sb);
	}

	public override void GetPlacedBlockName(StringBuilder sb, IWorldAccessor world, BlockPos pos)
	{
	}

	private void doorNameWithMaterial(StringBuilder sb)
	{
		if (block.Variant.ContainsKey("wood"))
		{
			string doorname = sb.ToString();
			sb.Clear();
			sb.Append(Lang.Get("doorname-with-material", doorname, Lang.Get("material-" + block.Variant["wood"])));
		}
	}

	public override float GetLiquidBarrierHeightOnSide(BlockFacing face, BlockPos pos, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventDefault;
		BEBehaviorTrapDoor beh = block.GetBEBehavior<BEBehaviorTrapDoor>(pos);
		if (beh == null)
		{
			return 0f;
		}
		if (!beh.IsSideSolid(face))
		{
			return 0f;
		}
		if (!airtight)
		{
			return 0f;
		}
		return 1f;
	}

	public override int GetRetention(BlockPos pos, BlockFacing facing, EnumRetentionType type, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventDefault;
		BEBehaviorTrapDoor beh = block.GetBEBehavior<BEBehaviorTrapDoor>(pos);
		if (beh == null)
		{
			return 0;
		}
		if (type == EnumRetentionType.Sound)
		{
			if (!beh.IsSideSolid(facing))
			{
				return 0;
			}
			return 3;
		}
		if (!airtight)
		{
			return 0;
		}
		if (api.World.Config.GetBool("openDoorsNotSolid"))
		{
			if (!beh.IsSideSolid(facing))
			{
				return 0;
			}
			return getInsulation(pos);
		}
		if (!beh.IsSideSolid(facing) && !beh.IsSideSolid(facing.Opposite))
		{
			return 3;
		}
		return getInsulation(pos);
	}

	private int getInsulation(BlockPos pos)
	{
		EnumBlockMaterial mat = block.GetBlockMaterial(api.World.BlockAccessor, pos);
		if (mat == EnumBlockMaterial.Ore || mat == EnumBlockMaterial.Stone || mat == EnumBlockMaterial.Soil || mat == EnumBlockMaterial.Ceramic)
		{
			return -1;
		}
		return 1;
	}

	public override bool SideIsSolid(BlockPos pos, int faceIndex, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventSubsequent;
		return block.GetBEBehavior<BEBehaviorTrapDoor>(pos)?.IsSideSolid(BlockFacing.ALLFACES[faceIndex]) ?? false;
	}

	public override bool SideIsSolid(IBlockAccessor blockAccess, BlockPos pos, int faceIndex, ref EnumHandling handled)
	{
		handled = EnumHandling.PreventSubsequent;
		return block.GetBEBehavior<BEBehaviorTrapDoor>(pos)?.IsSideSolid(BlockFacing.ALLFACES[faceIndex]) ?? false;
	}
}
