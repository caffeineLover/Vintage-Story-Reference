using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class BlockReeds : BlockPlant
{
	private WorldInteraction[] interactions;

	private string climateColorMapInt;

	private string seasonColorMapInt;

	private int maxWaterDepth;

	private string habitatBlockCode;

	public override string ClimateColorMapForMap => climateColorMapInt;

	public override string SeasonColorMapForMap => seasonColorMapInt;

	public override string RemapToLiquidsLayer => habitatBlockCode;

	public override void OnCollectTextures(ICoreAPI api, ITextureLocationDictionary textureDict)
	{
		base.OnCollectTextures(api, textureDict);
		climateColorMapInt = Attributes["climateColorMapForMap"].AsString();
		seasonColorMapInt = Attributes["seasonColorMapForMap"].AsString();
	}

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		isWaterPlant = true;
		maxWaterDepth = Attributes["maxWaterDepth"].AsInt(1);
		string hab = Variant["habitat"];
		if (hab == "water")
		{
			habitatBlockCode = "water-still-7";
		}
		else if (hab == "ice")
		{
			habitatBlockCode = "lakeice";
		}
		interactions = ObjectCacheUtil.GetOrCreate(api, "reedsBlockInteractions", () => new WorldInteraction[2]
		{
			new WorldInteraction
			{
				ActionLangCode = "blockhelp-reeds-harvest",
				MouseButton = EnumMouseButton.Left,
				Itemstacks = ObjectCacheUtil.GetToolStacks(api, EnumTool.Knife),
				GetMatchingStacks = (WorldInteraction wi, BlockSelection bs, EntitySelection es) => (api.World.BlockAccessor.GetBlock(bs.Position).Variant["state"] == "harvested") ? null : wi.Itemstacks
			},
			new WorldInteraction
			{
				ActionLangCode = "blockhelp-reeds-dig-roots",
				MouseButton = EnumMouseButton.Left,
				Itemstacks = ObjectCacheUtil.GetToolStacks(api, EnumTool.Shovel),
				GetMatchingStacks = (WorldInteraction wi, BlockSelection bs, EntitySelection es) => (api.World.BlockAccessor.GetBlock(bs.Position).Variant["state"] != "harvested") ? null : wi.Itemstacks
			}
		});
	}

	public override bool TryPlaceBlock(IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref string failureCode)
	{
		if (!CanPlaceBlock(world, byPlayer, blockSel, ref failureCode))
		{
			return false;
		}
		if (CanPlantStay(world.BlockAccessor, blockSel.Position))
		{
			world.BlockAccessor.SetBlock(BlockId, blockSel.Position);
			return true;
		}
		failureCode = "requirefertileground";
		return false;
	}

	public override float OnGettingBroken(IPlayer player, BlockSelection blockSel, ItemSlot itemslot, float remainingResistance, float dt, int counter)
	{
		float mul2;
		if (Variant["state"] == "harvested")
		{
			dt /= 2f;
			if (player.InventoryManager.ActiveTool == EnumTool.Shovel)
			{
				EnumBlockMaterial blockMaterial = player.Entity.World.BlockAccessor.GetBlockBelow(blockSel.Position).BlockMaterial;
				if (itemslot.Itemstack.Collectible.GetMiningSpeeds(itemslot).TryGetValue(blockMaterial, out var mul))
				{
					dt *= mul;
				}
			}
		}
		else if (player.InventoryManager.ActiveTool != EnumTool.Knife)
		{
			dt /= 3f;
		}
		else if (itemslot.Itemstack.Collectible.GetMiningSpeeds(itemslot).TryGetValue(EnumBlockMaterial.Plant, out mul2))
		{
			dt *= mul2;
		}
		float resistance = ((GetRequiredMiningTier(api.World, blockSel.Position) == 0) ? (remainingResistance - dt) : remainingResistance);
		if (counter % 5 == 0 || resistance <= 0f)
		{
			double posx = (double)blockSel.Position.X + blockSel.HitPosition.X;
			double posy = (double)blockSel.Position.InternalY + blockSel.HitPosition.Y;
			double posz = (double)blockSel.Position.Z + blockSel.HitPosition.Z;
			int dim = blockSel.Position.dimension;
			player.Entity.World.PlaySoundAt((resistance > 0f) ? Sounds.GetHitSound(player) : Sounds.GetBreakSound(player), posx, posy, posz, dim, player);
		}
		return resistance;
	}

	public override ItemStack OnPickBlock(IWorldAccessor world, BlockPos pos)
	{
		AssetLocation loc = CodeWithVariants(new string[2] { "habitat", "cover" }, new string[2] { "land", "free" });
		return new ItemStack(world.GetBlock(loc));
	}

	public override BlockDropItemStack[] GetDropsForHandbook(ItemStack handbookStack, IPlayer forPlayer)
	{
		Block harvestedBlock = api.World.GetBlock(CodeWithVariant("state", "harvested"));
		return api.World.GetBlock(CodeWithVariant("state", "normal")).Drops.Append(harvestedBlock.Drops);
	}

	public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, float dropQuantityMultiplier = 1f)
	{
		if (world.Side == EnumAppSide.Server && (byPlayer == null || byPlayer.WorldData.CurrentGameMode != EnumGameMode.Creative))
		{
			BlockDropItemStack[] drops = Drops;
			for (int i = 0; i < drops.Length; i++)
			{
				ItemStack drop = drops[i].GetNextItemStack();
				if (drop != null)
				{
					world.SpawnItemEntity(drop, pos);
				}
			}
			world.PlaySoundAt(Sounds.GetBreakSound(byPlayer), pos, -0.5, byPlayer);
		}
		if (byPlayer != null && Variant["state"] == "normal" && (byPlayer.InventoryManager.ActiveTool == EnumTool.Knife || byPlayer.InventoryManager.ActiveTool == EnumTool.Sickle || byPlayer.InventoryManager.ActiveTool == EnumTool.Scythe))
		{
			world.BlockAccessor.SetBlock(world.GetBlock(CodeWithVariants(new string[2] { "habitat", "state" }, new string[2] { "land", "harvested" })).BlockId, pos);
		}
		else
		{
			SpawnBlockBrokenParticles(pos);
			world.BlockAccessor.SetBlock(0, pos);
		}
	}

	public override bool TryPlaceBlockForWorldGen(IBlockAccessor blockAccessor, BlockPos pos, BlockFacing onBlockFace, IRandom worldGenRand, BlockPatchAttributes attributes = null)
	{
		if (!blockAccessor.GetBlock(pos).IsReplacableBy(this))
		{
			return false;
		}
		bool canPlace = true;
		BlockPos tmpPos = pos.Copy();
		for (int x = -1; x < 2; x++)
		{
			for (int z = -1; z < 2; z++)
			{
				tmpPos.Set(pos.X + x, pos.Y, pos.Z + z);
				if (blockAccessor.GetBlock(tmpPos, 1) is BlockWaterLilyGiant)
				{
					canPlace = false;
				}
			}
		}
		if (blockAccessor.GetBlock(pos, 1) is BlockPlant)
		{
			canPlace = false;
		}
		if (!canPlace)
		{
			return false;
		}
		int depth = 0;
		Block belowBlock = blockAccessor.GetBlockBelow(pos);
		while (belowBlock.LiquidCode == "water")
		{
			if (++depth > maxWaterDepth)
			{
				return false;
			}
			belowBlock = blockAccessor.GetBlockBelow(pos, depth + 1);
		}
		if (belowBlock.Fertility > 0)
		{
			return TryGen(blockAccessor, pos.DownCopy(depth));
		}
		return false;
	}

	private bool TryGen(IBlockAccessor blockAccessor, BlockPos pos)
	{
		Block placingBlock = blockAccessor.GetBlock(CodeWithVariant("habitat", "land"));
		if (placingBlock == null)
		{
			return false;
		}
		blockAccessor.SetBlock(placingBlock.BlockId, pos);
		return true;
	}

	public override int GetRandomColor(ICoreClientAPI capi, BlockPos pos, BlockFacing facing, int rndIndex = -1)
	{
		return capi.World.ApplyColorMapOnRgba(ClimateColorMapForMap, SeasonColorMapForMap, capi.BlockTextureAtlas.GetRandomColor(Textures.Last().Value.Baked.TextureSubId, rndIndex), pos.X, pos.Y, pos.Z);
	}

	public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
	{
		return interactions.Append(base.GetPlacedBlockInteractionHelp(world, selection, forPlayer));
	}
}
