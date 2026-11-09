using Vintagestory.API.Common;

namespace Vintagestory.ServerMods;

public static class ForestFloorHelper
{
	private static int[] forestBlockIds;

	private static int maxStage = 8;

	public static int MaxStage => maxStage;

	public static int[] InitialiseForestBlocks(IWorldAccessor world, string blockCodePrefix = "forestfloor-", int stages = 8)
	{
		maxStage = stages;
		forestBlockIds = new int[stages];
		for (int i = 0; i < stages; i++)
		{
			Block block = world.GetBlock(new AssetLocation(blockCodePrefix + i));
			forestBlockIds[i] = block?.Id ?? 0;
		}
		return forestBlockIds;
	}

	public static int[] GetForestBlockIds()
	{
		return forestBlockIds;
	}
}
