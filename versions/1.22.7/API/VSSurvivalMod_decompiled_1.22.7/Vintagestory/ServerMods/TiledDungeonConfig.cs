using System.Collections.Generic;
using Newtonsoft.Json;
using Vintagestory.API.Server;

namespace Vintagestory.ServerMods;

public class TiledDungeonConfig : WorldGenStructuresConfigBase
{
	public int minDistance;

	public int storyStructMinDistance;

	[JsonIgnore]
	public int MinDistanceSq;

	[JsonIgnore]
	public int StoryStructMinDistanceSq;

	public TiledDungeon[] Dungeons;

	public Dictionary<string, TiledDungeon> DungeonsByCode = new Dictionary<string, TiledDungeon>();

	public Dictionary<int, Dictionary<int, int>>? resolvedRockTypeRemaps;

	public void Init(ICoreServerAPI api)
	{
		MinDistanceSq = minDistance * minDistance;
		StoryStructMinDistanceSq = storyStructMinDistance * storyStructMinDistance;
		BlockLayerConfig blockLayerConfig = BlockLayerConfig.GetInstance(api);
		ResolveRemaps(api, blockLayerConfig.RockStrata);
		for (int i = 0; i < Dungeons.Length; i++)
		{
			TiledDungeon tiledDungeon = Dungeons[i];
			if (tiledDungeon.Code == null)
			{
				api.Logger.Error("Dungeon code at index: " + i + " is not specified. Will skip initialization");
				continue;
			}
			tiledDungeon.Init(api);
			DungeonsByCode[tiledDungeon.Code] = tiledDungeon;
			foreach (DungeonTile tile in tiledDungeon.Tiles)
			{
				if (tile.RockTypeRemapGroup != null)
				{
					resolvedRockTypeRemaps = resolvedRocktypeRemapGroups[tile.RockTypeRemapGroup];
				}
				else if (tiledDungeon.RockTypeRemapGroup != null)
				{
					resolvedRockTypeRemaps = resolvedRocktypeRemapGroups[tiledDungeon.RockTypeRemapGroup];
				}
				if (tile.RockTypeRemaps == null)
				{
					continue;
				}
				if (resolvedRockTypeRemaps != null)
				{
					Dictionary<int, Dictionary<int, int>> ownRemaps = WorldGenStructuresConfigBase.ResolveRockTypeRemaps(tile.RockTypeRemaps, blockLayerConfig.RockStrata, api);
					foreach (KeyValuePair<int, Dictionary<int, int>> val in resolvedRockTypeRemaps)
					{
						ownRemaps[val.Key] = val.Value;
					}
					resolvedRockTypeRemaps = ownRemaps;
				}
				else
				{
					resolvedRockTypeRemaps = WorldGenStructuresConfigBase.ResolveRockTypeRemaps(tile.RockTypeRemaps, blockLayerConfig.RockStrata, api);
				}
			}
		}
		TiledDungeon[] dungeons = Dungeons;
		for (int j = 0; j < dungeons.Length; j++)
		{
			dungeons[j].resolvedRockTypeRemaps = resolvedRockTypeRemaps;
		}
	}
}
