using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.Common.Collectible.Block;
using Vintagestory.GameContent;

namespace Vintagestory.ServerMods;

public class DungeonTile : WorldGenStructureBase
{
	[JsonProperty]
	public float Chance = 1f;

	[JsonIgnore]
	public BlockSchematicPartial[][] ResolvedSchematics;

	[JsonIgnore]
	public int[] ShuffledIndices;

	[JsonProperty]
	public TiledDungeon TileGenerator;

	[JsonProperty]
	public int Min;

	[JsonProperty]
	public int Max = 9999;

	[JsonIgnore]
	public string[] CachedNames;

	[JsonProperty]
	public string GroupMaxName;

	public void Init(ICoreServerAPI api, BlockLayerConfig blockLayerConfig)
	{
		if (TileGenerator != null)
		{
			TileGenerator.Code = "TileGenerator-" + Code;
			TileGenerator.Init(api);
			List<BlockSchematicPartial[]> schematics = new List<BlockSchematicPartial[]>();
			foreach (DungeonTile t in TileGenerator.Tiles)
			{
				schematics.AddRange(t.ResolvedSchematics);
			}
			ResolvedSchematics = schematics.ToArray();
		}
		else
		{
			ResolvedSchematics = LoadSchematicsWithRotations<BlockSchematicPartial>(api, this, blockLayerConfig, null, null, "dungeontiles/", isDungeon: true);
		}
		ShuffledIndices = new int[ResolvedSchematics.Length];
		for (int i = 0; i < ShuffledIndices.Length; i++)
		{
			ShuffledIndices[i] = i;
		}
		HashSet<string> nameshs = new HashSet<string>();
		BlockSchematicPartial[][] resolvedSchematics = ResolvedSchematics;
		for (int j = 0; j < resolvedSchematics.Length; j++)
		{
			foreach (ConnectorMetaData connector in resolvedSchematics[j][0].Connectors)
			{
				nameshs.Add(connector.Name);
			}
		}
		CachedNames = nameshs.ToArray();
	}

	public void FreshShuffleIndices(LCGRandom lcgRnd)
	{
		for (int i = 0; i < ShuffledIndices.Length; i++)
		{
			ShuffledIndices[i] = i;
		}
		ShuffledIndices.Shuffle(lcgRnd);
	}

	internal void PlaceConnectorsForDebug(IBlockAccessor ba, BlockPos pos, int tileIndex, int i)
	{
		foreach (ConnectorMetaData conn in ResolvedSchematics[tileIndex][i].Connectors)
		{
			BlockPos pathPosition = pos.AddCopy(conn.Position);
			ba.SetBlock(BlockSchematic.ConnectorBlockId, pathPosition);
			BETileConnector blockEntity = ba.GetBlockEntity<BETileConnector>(pathPosition);
			blockEntity.Target = string.Join(",", conn.Targets);
			blockEntity.Name = conn.Name;
			blockEntity.Direction = conn.Facing;
			blockEntity.MarkDirty();
		}
	}
}
