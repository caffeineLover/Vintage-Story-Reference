using System;
using System.Collections.Generic;
using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

[DocumentAsJson]
[AddDocumentationProperty("AttachableFaces", "The faces that this block could be attached from which will prevent it from falling.", "System.String[]", "Optional", "None", false)]
[AddDocumentationProperty("AttachmentAreas", "A list of attachment areas per face that determine what blocks can be attached to.", "System.Collections.Generic.Dictionary{System.String,Vintagestory.API.Datastructures.RotatableCube}", "Optional", "None", false)]
[AddDocumentationProperty("AttachmentArea", "A single attachment area that determine what blocks can be attached to. Used if AttachmentAreas is not supplied.", "Vintagestory.API.Mathtools.Cuboidi", "Optional", "None", false)]
[AddDocumentationProperty("AllowUnstablePlacement", "Can this block be placed in an unstable position?", "System.Boolean", "Optional", "False", true)]
[AddDocumentationProperty("IgnorePlaceTest", "(Obsolete) Please use the AllowUnstablePlacement attribute instead.", "System.Boolean", "Obsolete", "", false)]
public class BlockBehaviorUnstableFalling(Block block) : BlockBehavior(block)
{
	[DocumentAsJson("Optional", "None", false)]
	private AssetLocation[] exceptions;

	[DocumentAsJson("Optional", "False", false)]
	public bool fallSideways;

	[DocumentAsJson("Optional", "0", false)]
	private float dustIntensity;

	[DocumentAsJson("Optional", "0.3", false)]
	private float fallSidewaysChance = 0.3f;

	[DocumentAsJson("Optional", "None", false)]
	private AssetLocation fallSound;

	[DocumentAsJson("Optional", "1", false)]
	private float impactDamageMul;

	[DocumentAsJson("Optional", "None", false)]
	private AssetLocation variantAfterFalling;

	private Cuboidi[] attachmentAreas;

	private BlockFacing[] attachableFaces;

	public override void Initialize(JsonObject properties)
	{
		base.Initialize(properties);
		attachableFaces = null;
		if (properties["attachableFaces"].Exists)
		{
			string[] faces = properties["attachableFaces"].AsArray<string>();
			attachableFaces = new BlockFacing[faces.Length];
			for (int i = 0; i < faces.Length; i++)
			{
				attachableFaces[i] = BlockFacing.FromCode(faces[i]);
			}
		}
		Dictionary<string, RotatableCube> areas = properties["attachmentAreas"].AsObject<Dictionary<string, RotatableCube>>();
		attachmentAreas = new Cuboidi[6];
		if (areas != null)
		{
			foreach (KeyValuePair<string, RotatableCube> val in areas)
			{
				val.Value.Origin.Set(8.0, 8.0, 8.0);
				BlockFacing face = BlockFacing.FromFirstLetter(val.Key[0]);
				attachmentAreas[face.Index] = val.Value.RotatedCopy().ConvertToCuboidi();
			}
		}
		else
		{
			attachmentAreas[4] = properties["attachmentArea"].AsObject<Cuboidi>();
		}
		exceptions = properties["exceptions"].AsObject(Array.Empty<AssetLocation>(), block.Code.Domain);
		fallSideways = properties["fallSideways"].AsBool();
		dustIntensity = properties["dustIntensity"].AsFloat();
		fallSidewaysChance = properties["fallSidewaysChance"].AsFloat(0.3f);
		string sound = properties["fallSound"].AsString();
		if (sound != null)
		{
			fallSound = AssetLocation.Create(sound, block.Code.Domain);
		}
		string variant = properties["variantAfterFalling"].AsString();
		if (variant != null)
		{
			variantAfterFalling = AssetLocation.Create(variant, block.Code.Domain);
		}
		impactDamageMul = properties["impactDamageMul"].AsFloat(1f);
	}

	public override bool CanPlaceBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling, ref string failureCode)
	{
		handling = EnumHandling.PassThrough;
		JsonObject attributes = block.Attributes;
		if (attributes != null && attributes["allowUnstablePlacement"].AsBool())
		{
			return true;
		}
		Cuboidi attachmentArea = attachmentAreas[4];
		BlockPos pos = blockSel.Position.DownCopy();
		Block onBlock = world.BlockAccessor.GetBlock(pos);
		if (blockSel != null && !IsAttached(world.BlockAccessor, blockSel.Position) && !onBlock.CanAttachBlockAt(world.BlockAccessor, block, pos, BlockFacing.UP, attachmentArea) && !onBlock.WildCardMatch(exceptions))
		{
			handling = EnumHandling.PreventSubsequent;
			failureCode = "requiresolidground";
			return false;
		}
		return true;
	}

	public override void OnBlockPlaced(IWorldAccessor world, BlockPos blockPos, ref EnumHandling handling)
	{
		TryFalling(world, blockPos, ref handling);
	}

	public override void OnNeighbourBlockChange(IWorldAccessor world, BlockPos pos, BlockPos neibpos, ref EnumHandling handling)
	{
		base.OnNeighbourBlockChange(world, pos, neibpos, ref handling);
		if (world.Side != EnumAppSide.Client)
		{
			EnumHandling bla = EnumHandling.PassThrough;
			TryFalling(world, pos, ref bla);
		}
	}

	private bool TryFalling(IWorldAccessor world, BlockPos pos, ref EnumHandling handling)
	{
		if (world.Side != EnumAppSide.Server)
		{
			return false;
		}
		if (!fallSideways && IsAttached(world.BlockAccessor, pos))
		{
			return false;
		}
		ICoreServerAPI sapi = (world as IServerWorldAccessor).Api as ICoreServerAPI;
		if (!sapi.World.Config.GetBool("allowFallingBlocks"))
		{
			return false;
		}
		if (IsReplacableBeneath(world, pos) || (fallSideways && world.Rand.NextDouble() < (double)fallSidewaysChance && IsReplacableBeneathAndSideways(world, pos)))
		{
			BlockPos ourPos = pos.Copy();
			sapi.Event.EnqueueMainThreadTask(delegate
			{
				createFallingBlock(world, ourPos);
			}, "falling");
			handling = EnumHandling.PreventSubsequent;
			return true;
		}
		handling = EnumHandling.PassThrough;
		return false;
	}

	public void createFallingBlock(IWorldAccessor world, BlockPos ourPos)
	{
		Block block = world.BlockAccessor.GetBlock(ourPos);
		if (base.block == block && !FallingEntityAlreadyExists(world, ourPos))
		{
			if (variantAfterFalling != null)
			{
				block = world.BlockAccessor.GetBlock(variantAfterFalling);
			}
			BlockEntity be = world.BlockAccessor.GetBlockEntity(ourPos);
			EntityBlockFalling entityBf = new EntityBlockFalling(block, be, ourPos, fallSound, impactDamageMul, canFallSideways: true, dustIntensity);
			world.SpawnEntity(entityBf);
		}
	}

	public bool FallingEntityAlreadyExists(IWorldAccessor world, BlockPos ourPos)
	{
		return world.GetNearestEntity(ourPos.ToVec3d().Add(0.5, 0.5, 0.5), 1f, 1.5f, (Entity e) => e is EntityBlockFalling entityBlockFalling && entityBlockFalling.initialPos.Equals(ourPos)) != null;
	}

	public virtual bool IsAttached(IBlockAccessor blockAccessor, BlockPos pos)
	{
		BlockPos tmpPos;
		if (attachableFaces == null)
		{
			tmpPos = pos.DownCopy();
			return blockAccessor.GetBlock(tmpPos).CanAttachBlockAt(blockAccessor, block, tmpPos, BlockFacing.UP, attachmentAreas[5]);
		}
		tmpPos = new BlockPos(pos.dimension);
		for (int i = 0; i < attachableFaces.Length; i++)
		{
			BlockFacing face = attachableFaces[i];
			tmpPos.Set(pos).Add(face);
			if (blockAccessor.GetBlock(tmpPos).CanAttachBlockAt(blockAccessor, block, tmpPos, face.Opposite, attachmentAreas[face.Index]))
			{
				return true;
			}
		}
		return false;
	}

	private bool IsReplacableBeneathAndSideways(IWorldAccessor world, BlockPos pos)
	{
		for (int i = 0; i < 4; i++)
		{
			BlockFacing facing = BlockFacing.HORIZONTALS[i];
			Block nBlock = world.BlockAccessor.GetBlockOrNull(pos.X + facing.Normali.X, pos.InternalY + facing.Normali.Y, pos.Z + facing.Normali.Z);
			if (nBlock != null && nBlock.Replaceable >= 6000)
			{
				nBlock = world.BlockAccessor.GetBlockOrNull(pos.X + facing.Normali.X, pos.InternalY + facing.Normali.Y - 1, pos.Z + facing.Normali.Z);
				if (nBlock != null && nBlock.Replaceable >= 6000)
				{
					return true;
				}
			}
		}
		return false;
	}

	private bool IsReplacableBeneath(IWorldAccessor world, BlockPos pos)
	{
		return world.BlockAccessor.GetBlockBelow(pos).Replaceable > 6000;
	}
}
