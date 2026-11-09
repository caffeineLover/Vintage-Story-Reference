using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.ServerMods.NoObf;

namespace Vintagestory.ServerMods;

public class TreeGeneratorsUtil
{
	private ICoreServerAPI sapi;

	public ForestFloorSystem forestFloorSystem;

	public TreeGeneratorsUtil(ICoreServerAPI api)
	{
		sapi = api;
		forestFloorSystem = new ForestFloorSystem(api);
	}

	public void ReloadTreeGenerators()
	{
		int quantity = sapi.Assets.Reload(new AssetLocation("worldgen/treegen"));
		sapi.Server.LogNotification("{0} tree generators reloaded", quantity);
		LoadTreeGenerators();
	}

	public void LoadTreeGenerators()
	{
		Dictionary<AssetLocation, TreeGenConfig> treeGenModelsByTree = sapi.Assets.GetMany<TreeGenConfig>(sapi.Server.Logger, "worldgen/treegen");
		WoodWorldProperty woodWorldProperty = sapi.Assets.Get<WoodWorldProperty>(new AssetLocation("worldproperties/block/wood.json"));
		Dictionary<string, EnumTreeType> treetypes = new Dictionary<string, EnumTreeType>();
		WorldWoodPropertyVariant[] variants = woodWorldProperty.Variants;
		foreach (WorldWoodPropertyVariant val in variants)
		{
			treetypes[val.Code.Path] = val.TreeType;
		}
		bool potatoeMode = sapi.World.Config.GetAsString("potatoeMode", "false").ToBool();
		string names = "";
		foreach (KeyValuePair<AssetLocation, TreeGenConfig> val2 in treeGenModelsByTree)
		{
			AssetLocation name = val2.Key.Clone();
			if (names.Length > 0)
			{
				names += ", ";
			}
			names += name;
			name.Path = val2.Key.Path.Substring("worldgen/treegen/".Length);
			name.RemoveEnding();
			if (potatoeMode)
			{
				val2.Value.treeBlocks.mossDecorCode = null;
			}
			val2.Value.Init(val2.Key, sapi.Server.Logger);
			sapi.RegisterTreeGenerator(name, new TreeGen(val2.Value, sapi.WorldManager.Seed, forestFloorSystem));
			val2.Value.treeBlocks.ResolveBlockNames(sapi, name.Path);
			treetypes.TryGetValue(sapi.World.GetBlock(val2.Value.treeBlocks.logBlockId).Variant["wood"], out val2.Value.Treetype);
		}
		sapi.Server.LogNotification("Reloaded {0} tree generators", treeGenModelsByTree.Count);
	}

	public ITreeGenerator GetGenerator(AssetLocation generatorCode)
	{
		sapi.World.TreeGenerators.TryGetValue(generatorCode, out var gen);
		return gen;
	}

	public KeyValuePair<AssetLocation, ITreeGenerator> GetGenerator(int index)
	{
		AssetLocation key = sapi.World.TreeGenerators.GetKeyAtIndex(index);
		if (key != null)
		{
			return new KeyValuePair<AssetLocation, ITreeGenerator>(key, sapi.World.TreeGenerators[key]);
		}
		return new KeyValuePair<AssetLocation, ITreeGenerator>(null, null);
	}

	public void RunGenerator(AssetLocation treeName, IBlockAccessor api, BlockPos pos, TreeGenParams treeGenParams)
	{
		sapi.World.TreeGenerators[treeName].GrowTree(api, pos, treeGenParams, new NormalRandom());
	}
}
