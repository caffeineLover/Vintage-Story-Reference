using System.Collections.Generic;
using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace Vintagestory.GameContent;

public class BlockBehaviorFruitingBush(Block block) : BlockBehavior(block)
{
	[DocumentAsJson("Recommended", "0", false)]
	public float harvestTime;

	[DocumentAsJson("Recommended", "0", false)]
	public float cuttingTime;

	[DocumentAsJson("Required", "", false)]
	public BlockDropItemStack[]? harvestedStacks;

	public float PauseGrowthBelowTemperature;

	public float PauseGrowthAboveTemperature;

	public float ResetGrowthBelowTemperature;

	public float ResetGrowthAboveTemperature;

	public float GoDormantBelowTemperature;

	public float LeaveDormantAboveTemperature;

	public string[]? CreatureDietFoodTags;

	public JsonObject? GrowthProperties;

	public float GrowthRateMul = 1f;

	public AssetLocation? HarvestingSound;

	public Dictionary<string, NpkNutrients> nutrientUseByHealthState = new Dictionary<string, NpkNutrients>
	{
		["bountiful"] = new NpkNutrients(10f, 10f, 10f),
		["healthy"] = new NpkNutrients(7f, 7f, 7f),
		["struggling"] = new NpkNutrients(4f, 4f, 4f),
		["barren"] = new NpkNutrients(1f, 1f, 1f)
	};

	public NatFloat[] growthStageMonths;

	public override void OnLoaded(ICoreAPI api)
	{
		base.OnLoaded(api);
		GrowthRateMul = (float)api.World.Config.GetDecimal("cropGrowthRateMul", GrowthRateMul);
		GrowthProperties = block.Attributes["growthProperties"];
		PauseGrowthBelowTemperature = GrowthProperties["pauseGrowthBelowTemperature"].AsFloat(-999f);
		PauseGrowthAboveTemperature = GrowthProperties["pauseGrowthAboveTemperature"].AsFloat(999f);
		ResetGrowthBelowTemperature = GrowthProperties["resetGrowthBelowTemperature"].AsFloat(-999f);
		ResetGrowthAboveTemperature = GrowthProperties["resetGrowthAboveTemperature"].AsFloat(999f);
		GoDormantBelowTemperature = GrowthProperties["goDormantBelowTemperature"].AsFloat(-999f);
		LeaveDormantAboveTemperature = GrowthProperties["leaveDormantAboveTemperature"].AsFloat(999f);
		nutrientUseByHealthState = GrowthProperties["nutrientUseByHealthState"].AsObject(nutrientUseByHealthState);
		growthStageMonths = new NatFloat[6]
		{
			GrowthProperties["youngStageMonths"].AsObject(new NatFloat(3f, 1f, EnumDistribution.UNIFORM)),
			GrowthProperties["emptyStageMonths"].AsObject(new NatFloat(0.85f, 0.1f, EnumDistribution.UNIFORM)),
			GrowthProperties["floweringStageMonths"].AsObject(new NatFloat(0.35f, 0.1f, EnumDistribution.UNIFORM)),
			GrowthProperties["ripeningStageMonths"].AsObject(new NatFloat(0.85f, 0.1f, EnumDistribution.UNIFORM)),
			GrowthProperties["ripeStageMonths"].AsObject(new NatFloat(1f, 0.1f, EnumDistribution.UNIFORM)),
			null
		};
		CreatureDietFoodTags = block.Attributes["foodTags"].AsArray<string>();
		string code = block.Attributes["harvestingSound"].AsString("game:sounds/block/leafy-picking");
		if (code != null)
		{
			HarvestingSound = AssetLocation.Create(code, block.Code.Domain);
		}
		harvestTime = block.Attributes["harvestTime"].AsFloat(0.5f);
		cuttingTime = block.Attributes["cuttingTime"].AsFloat(2f);
		harvestedStacks = block.Attributes["harvestedStacks"].AsObject<BlockDropItemStack[]>();
		BlockDropItemStack[] array = harvestedStacks;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Resolve(api.World, "harvested stack of fruiting bush", code);
		}
	}

	public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref float dropChanceMultiplier, ref EnumHandling handling)
	{
		BEBehaviorFruitingBush beh = block.GetBEBehavior<BEBehaviorFruitingBush>(pos);
		if (beh == null || beh.BState.Growthstate != EnumFruitingBushGrowthState.Ripe)
		{
			return base.GetDrops(world, pos, byPlayer, ref dropChanceMultiplier, ref handling);
		}
		handling = EnumHandling.PassThrough;
		return beh.GetRipeDrops(byPlayer);
	}
}
