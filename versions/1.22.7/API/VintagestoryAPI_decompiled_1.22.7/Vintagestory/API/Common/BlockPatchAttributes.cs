using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;

namespace Vintagestory.API.Common;

public class BlockPatchAttributes
{
	/// <summary>
	/// List of asset codes for the base (coralblock) types of a coral reef blockpatch
	/// </summary>
	public string[]? CoralBase;

	/// <summary>
	/// List of asset codes for the coral structure types of a coral reef blockpatch
	/// </summary>
	public string[]? CoralStructure;

	/// <summary>
	/// List of asset codes for the coral shelved types of a coral reef blockpatch
	/// These need to have HorizontalOrientable behaviour and only specify one side in here "coralshelf-north"
	/// </summary>
	public string[]? CoralShelve;

	/// <summary>
	/// List of asset codes for the coral decor of a coral reef blockpatch
	/// These will be placed on solid surfaces around coral base blocks and on top of structures
	/// </summary>
	public string[]? CoralDecor;

	public string[]? StructureDecor;

	/// <summary>
	/// List of asset codes for the coral types of a coral reef blockpatch coral-brain, coral-fan ...
	/// </summary>
	public string[]? Coral;

	/// <summary>
	/// Defines the minimum 2D size of the coral reef
	/// </summary>
	public int CoralMinSize = -1;

	/// <summary>
	/// Defines the random size between 0 - X that will be added additionally to the reef
	/// </summary>
	public int CoralRandomSize = -1;

	/// <summary>
	/// Chance for a shelf block to spawn on a under water cliff. The chance is rolled for each height
	/// The patch will try to spawn them until it reaches minWaterDepth
	/// </summary>
	public float CoralVerticalGrowChance = -1f;

	/// <summary>
	/// Chance that any Plant will spawn
	/// </summary>
	public float CoralPlantsChance = -1f;

	/// <summary>
	/// Specifiy which plants should spawn for this blockpatch and their heigh and how often a specific plant should be chosen
	/// </summary>
	public Dictionary<string, CoralPlantConfig>? CoralPlants;

	/// <summary>
	/// Chance that a shelf will spawn instead of a structure on top of a coralblock
	/// </summary>
	public float CoralShelveChance = -1f;

	/// <summary>
	/// Chance that coral generating will replace all other block patches in its area
	/// </summary>
	public float CoralReplaceOtherPatches = -1f;

	/// <summary>
	/// If no shelf was spawned this chance controls how likely a structure will spawn instead of a coral.
	/// If a structure is spawned then a coral will spawn on top
	/// If no shelve nor structure was spawned then also a coral will be spawned
	/// </summary>
	public float CoralStructureChance = -1f;

	/// <summary>
	/// When a coral [brain, fan] gets spawned also try if a decor should spawn below it
	/// Also on vertical growth we try to spawn decors cliffs
	/// </summary>
	public float CoralDecorChance = -1f;

	/// <summary>
	/// Chance to spawn a coral [brain, fan] on top of base blocks or structures
	/// </summary>
	public float CoralChance = 0.5f;

	/// <summary>
	/// How thick the base coral full block layer should be for this patch (goes down into the ground, helpful for cliffs)
	/// 1 -&gt; replace the gravel with coral
	/// 2 -&gt; go 1 block below gravel and also replace and so on
	/// </summary>
	public int CoralBaseHeight;

	/// <summary>
	/// Chance that a BlockCrowfoot will spawn a flower when it reaches the water surface
	/// </summary>
	public float FlowerChance = -1f;

	/// <summary>
	/// Heigh distribution for BlockSeaweed and BlockCrowfoot types
	/// </summary>
	public NatFloat? Height;

	[JsonIgnore]
	public Block[]? CoralBaseBlock;

	[JsonIgnore]
	public Block[]? CoralStructureBlock;

	[JsonIgnore]
	public Block[][]? CoralShelveBlock;

	[JsonIgnore]
	public Block[]? CoralBlock;

	[JsonIgnore]
	public Block[]? CoralDecorBlock;

	[JsonIgnore]
	public Block[]? StructureDecorBlock;

	public void Init(ICoreServerAPI sapi, int i)
	{
		List<Block> foundBlocksList = new List<Block>();
		string[] coralBase;
		if (CoralBase != null)
		{
			coralBase = CoralBase;
			foreach (string code in coralBase)
			{
				Block[] searchBlocks = sapi.World.SearchBlocks(new AssetLocation(code));
				if (searchBlocks != null)
				{
					foundBlocksList.AddRange(searchBlocks);
					continue;
				}
				sapi.World.Logger.Warning("Block patch Nr. {0}: Unable to resolve CoralBaseBlocks block with code {1}. Will ignore.", i, code);
			}
			CoralBaseBlock = foundBlocksList.ToArray();
			foundBlocksList.Clear();
		}
		if (CoralStructure != null)
		{
			coralBase = CoralStructure;
			foreach (string code2 in coralBase)
			{
				Block[] searchBlocks2 = sapi.World.SearchBlocks(new AssetLocation(code2));
				if (searchBlocks2 != null)
				{
					foundBlocksList.AddRange(searchBlocks2);
					continue;
				}
				sapi.World.Logger.Warning("Block patch Nr. {0}: Unable to resolve CoralStructure block with code {1}. Will ignore.", i, code2);
			}
			CoralStructureBlock = foundBlocksList.ToArray();
			foundBlocksList.Clear();
		}
		if (CoralShelve != null)
		{
			List<Block[]> foundShelveBlocks = new List<Block[]>();
			coralBase = CoralShelve;
			foreach (string code3 in coralBase)
			{
				Block[] searchBlocks3 = sapi.World.SearchBlocks(new AssetLocation(code3));
				if (searchBlocks3 != null)
				{
					List<Block[]> bockList = new List<Block[]>();
					Block[] array = searchBlocks3;
					foreach (Block block in array)
					{
						string codeWithoutParts = block.CodeWithoutParts(1);
						if (!foundShelveBlocks.Any((Block[] c) => c[0].Code.Path.Equals(codeWithoutParts + "-north")))
						{
							Block[] blocks = new Block[4]
							{
								sapi.World.BlockAccessor.GetBlock(new AssetLocation(codeWithoutParts + "-north")),
								sapi.World.BlockAccessor.GetBlock(new AssetLocation(codeWithoutParts + "-east")),
								sapi.World.BlockAccessor.GetBlock(new AssetLocation(codeWithoutParts + "-south")),
								sapi.World.BlockAccessor.GetBlock(new AssetLocation(codeWithoutParts + "-west"))
							};
							bockList.Add(blocks);
						}
					}
					foundShelveBlocks.AddRange(bockList);
				}
				else
				{
					sapi.World.Logger.Warning("Block patch Nr. {0}: Unable to resolve CoralShelve block with code {1}. Will ignore.", i, code3);
				}
			}
			CoralShelveBlock = foundShelveBlocks.ToArray();
		}
		if (Coral != null)
		{
			coralBase = Coral;
			foreach (string code4 in coralBase)
			{
				Block[] searchBlocks4 = sapi.World.SearchBlocks(new AssetLocation(code4));
				if (searchBlocks4 != null)
				{
					foundBlocksList.AddRange(searchBlocks4);
					continue;
				}
				sapi.World.Logger.Warning("Block patch Nr. {0}: Unable to resolve Coral block with code {1}. Will ignore.", i, code4);
			}
			CoralBlock = foundBlocksList.ToArray();
			foundBlocksList.Clear();
		}
		if (CoralPlants != null)
		{
			foreach (KeyValuePair<string, CoralPlantConfig> coralPlant in CoralPlants)
			{
				coralPlant.Deconstruct(out var key, out var value);
				string code5 = key;
				CoralPlantConfig config = value;
				Block[] searchBlocks5 = sapi.World.SearchBlocks(new AssetLocation(code5));
				if (searchBlocks5 != null)
				{
					config.Block = searchBlocks5;
					continue;
				}
				sapi.World.Logger.Warning("Block patch Nr. {0}: Unable to resolve CoralPlants block with code {1}. Will ignore.", i, code5);
			}
		}
		if (CoralDecor != null)
		{
			coralBase = CoralDecor;
			foreach (string code6 in coralBase)
			{
				Block[] searchBlocks6 = sapi.World.SearchBlocks(new AssetLocation(code6));
				if (searchBlocks6 != null)
				{
					foundBlocksList.AddRange(searchBlocks6);
					continue;
				}
				sapi.World.Logger.Warning("Block patch Nr. {0}: Unable to resolve CoralDecor block with code {1}. Will ignore.", i, code6);
			}
			CoralDecorBlock = foundBlocksList.ToArray();
			foundBlocksList.Clear();
		}
		if (StructureDecor == null)
		{
			return;
		}
		coralBase = StructureDecor;
		foreach (string code7 in coralBase)
		{
			Block[] searchBlocks7 = sapi.World.SearchBlocks(new AssetLocation(code7));
			if (searchBlocks7 != null)
			{
				foundBlocksList.AddRange(searchBlocks7);
				continue;
			}
			sapi.World.Logger.Warning("Block patch Nr. {0}: Unable to resolve StructureDecor block with code {1}. Will ignore.", i, code7);
		}
		StructureDecorBlock = foundBlocksList.ToArray();
		foundBlocksList.Clear();
	}
}
