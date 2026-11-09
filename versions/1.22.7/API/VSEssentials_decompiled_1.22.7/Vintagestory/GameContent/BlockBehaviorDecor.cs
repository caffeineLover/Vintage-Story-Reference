using System;
using Vintagestory.API;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

[AddDocumentationProperty("Sides", "A list of sides that this decor block can be placed on.", "System.String[]", "Required", "", false)]
[AddDocumentationProperty("DrawIfCulled", "If true, do not cull even if parent face was culled (used e.g. for medium carpet, which stick out beyond the parent face)", "System.Boolean", "Optional", "False", false)]
[AddDocumentationProperty("AlternateZOffset", "If true, alternates z-offset vertexflag by 1 in odd/even XZ positions to reduce z-fighting (used e.g. for medium carpets overlaying neighbours)", "System.Boolean", "Optional", "False", false)]
[AddDocumentationProperty("NotFullFace", "If true, this decor is NOT (at least) a full opaque face so that the parent block face still needs to be drawn", "System.Boolean", "Optional", "False", false)]
[AddDocumentationProperty("Removable", "If true, this decor is removable using the players hands, without breaking the parent block", "System.Boolean", "Optional", "False", false)]
[AddDocumentationProperty("Thickness", "The thickness of this decor block. Used to adjust selection box of the parent block.", "System.Single", "Optional", "0.03125", false)]
[DocumentAsJson]
public class BlockBehaviorDecor : BlockBehavior
{
	private BlockFacing[] sides;

	[DocumentAsJson("Optional", "False", false)]
	private bool sidedVariants;

	[DocumentAsJson("Optional", "False", false)]
	private bool nwOrientable;

	public BlockBehaviorDecor(Block block)
		: base(block)
	{
		block.decorBehaviorFlags = 1;
	}

	public override void Initialize(JsonObject properties)
	{
		string[] sidenames = properties["sides"].AsArray(Array.Empty<string>());
		sides = new BlockFacing[sidenames.Length];
		for (int i = 0; i < sidenames.Length; i++)
		{
			if (sidenames[i] != null)
			{
				sides[i] = BlockFacing.FromFirstLetter(sidenames[i][0]);
			}
		}
		sidedVariants = properties["sidedVariants"].AsBool();
		nwOrientable = properties["nwOrientable"].AsBool();
		if (properties["drawIfCulled"].AsBool())
		{
			block.decorBehaviorFlags |= 2;
		}
		if (properties["alternateZOffset"].AsBool())
		{
			block.decorBehaviorFlags |= 4;
		}
		if (properties["notFullFace"].AsBool())
		{
			block.decorBehaviorFlags |= 8;
		}
		if (properties["removable"].AsBool())
		{
			block.decorBehaviorFlags |= 16;
		}
		if (properties["canAttachToAnything"].AsBool())
		{
			block.decorBehaviorFlags |= 64;
		}
		if (sidedVariants)
		{
			block.decorBehaviorFlags |= 32;
		}
		block.DecorThickness = properties["thickness"].AsFloat(1f / 32f);
		base.Initialize(properties);
	}

	public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref EnumHandling handling, ref string failureCode)
	{
		handling = EnumHandling.PreventDefault;
		for (int i = 0; i < sides.Length; i++)
		{
			if (sides[i] != blockSel.Face)
			{
				continue;
			}
			BlockPos pos = (blockSel.DidOffset ? blockSel.Position.AddCopy(blockSel.Face.Opposite) : blockSel.Position);
			Block blockToPlace;
			if (sidedVariants)
			{
				blockToPlace = world.BlockAccessor.GetBlock(block.CodeWithParts(blockSel.Face.Opposite.Code));
				if (blockToPlace == null)
				{
					failureCode = "decorvariantnotfound";
					return false;
				}
			}
			else if (nwOrientable)
			{
				string code = ((Block.SuggestedHVOrientation(byPlayer, blockSel)[0].Axis == EnumAxis.X) ? "we" : "ns");
				blockToPlace = world.BlockAccessor.GetBlock(block.CodeWithParts(code));
				if (blockToPlace == null)
				{
					failureCode = "decorvariantnotfound";
					return false;
				}
			}
			else
			{
				blockToPlace = block;
			}
			Block attachingBlock = world.BlockAccessor.GetBlock(pos);
			IAcceptsDecor iad = attachingBlock.GetInterface<IAcceptsDecor>(world, pos);
			if (iad != null && iad.CanAccept(blockToPlace))
			{
				if (byPlayer.WorldData.CurrentGameMode == EnumGameMode.Survival)
				{
					int decorId = iad.GetDecor(blockSel.Face);
					if (decorId > 0)
					{
						Block decor = world.BlockAccessor.GetBlock(decorId);
						ItemStack itemStack = new ItemStack(decor.Id, decor.ItemClass, 1, new TreeAttribute(), world);
						world.SpawnItemEntity(itemStack, pos.AddCopy(blockSel.Face).ToVec3d());
					}
				}
				iad.SetDecor(blockToPlace, blockSel.Face);
				return true;
			}
			EnumBlockMaterial mat = attachingBlock.GetBlockMaterial(world.BlockAccessor, pos);
			bool? attachingBlockCanAllow = null;
			if (attachingBlock.Attributes != null)
			{
				JsonObject attr = attachingBlock.Attributes["allowOverlays"];
				if (attr.Exists)
				{
					attachingBlockCanAllow = attr.AsBool(defaultValue: true);
				}
			}
			if ((((block.decorBehaviorFlags & 0x40) == 0 || (attachingBlock.DrawType != EnumDrawType.JSON && attachingBlock.DrawType != EnumDrawType.JSONAndSnowLayer) || attachingBlock.RenderPass == EnumChunkRenderPass.Transparent || attachingBlockCanAllow == false) && attachingBlockCanAllow != true && !attachingBlock.CanAttachBlockAt(world.BlockAccessor, blockToPlace, pos, blockSel.Face)) || mat == EnumBlockMaterial.Snow || mat == EnumBlockMaterial.Ice)
			{
				failureCode = "decorrequiressolid";
				return false;
			}
			DecorBits decorPosition = new DecorBits(blockSel.Face);
			Block decorBlock = world.BlockAccessor.GetDecor(pos, decorPosition);
			if (world.BlockAccessor.SetDecor(blockToPlace, pos, decorPosition))
			{
				if (byPlayer.WorldData.CurrentGameMode == EnumGameMode.Survival && decorBlock != null && (decorBlock.decorBehaviorFlags & 0x10) != 0)
				{
					ItemStack itemStack2 = decorBlock.OnPickBlock(world, pos);
					world.SpawnItemEntity(itemStack2, pos.AddCopy(blockSel.Face).ToVec3d());
				}
				return true;
			}
			failureCode = "existingdecorinplace";
			return false;
		}
		failureCode = "cannotplacedecorhere";
		return false;
	}

	public override AssetLocation GetRotatedBlockCode(int angle, ref EnumHandling handled)
	{
		if (nwOrientable)
		{
			handled = EnumHandling.PreventDefault;
			string[] angles = new string[2] { "ns", "we" };
			int index = angle / 90;
			if (block.LastCodePart() == "we")
			{
				index++;
			}
			return block.CodeWithParts(angles[index % 2]);
		}
		return base.GetRotatedBlockCode(angle, ref handled);
	}
}
