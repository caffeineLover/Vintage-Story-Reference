using System;
using System.Collections.Generic;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;

namespace Vintagestory.API.Common.Entities;

/// <summary>
/// A base class for entities spawning conditions.
/// </summary>
[DocumentAsJson]
public class BaseSpawnConditions : ClimateSpawnCondition
{
	/// <summary>
	/// The group of the spawn conditions. Vanilla groups are:<br />
	/// - hostile<br />
	/// - neutral<br />
	/// - passive<br />
	/// Hostile creatures should be defined as such here.
	/// This will automatically stop them spawning with a grace timer,
	///     and in locations where hostiles should not spawn.
	/// </summary>
	[DocumentAsJson("Recommended", "None", false)]
	public string Group;

	/// <summary>
	/// The minimum light level for an object to spawn.
	/// </summary>
	[DocumentAsJson("Optional", "0", false)]
	public int MinLightLevel;

	/// <summary>
	/// The maximum light level for an object to spawn.
	/// </summary>
	[DocumentAsJson("Optional", "32", false)]
	public int MaxLightLevel = 32;

	/// <summary>
	/// The type of light counted for spawning purposes.
	/// </summary>
	[DocumentAsJson("Optional", "MaxLight", false)]
	public EnumLightLevelType LightLevelType = EnumLightLevelType.MaxLight;

	/// <summary>
	/// the group size for the spawn.
	/// </summary>
	[DocumentAsJson("Recommended", "1", false)]
	public NatFloat HerdSize = NatFloat.createUniform(1f, 0f);

	/// <summary>
	/// Additional companions for the spawn.
	/// </summary>
	[DocumentAsJson("Optional", "None", false)]
	public AssetLocation[] Companions = Array.Empty<AssetLocation>();

	/// <summary>
	/// The blocks that the object will spawn in.
	/// </summary>
	[DocumentAsJson("Optional", "air", false)]
	public AssetLocation[] InsideBlockCodes = new AssetLocation[1]
	{
		new AssetLocation("air")
	};

	/// <summary>
	/// Checks to see if the object requires solid ground.
	/// </summary>
	[DocumentAsJson("Optional", "True", false)]
	public bool RequireSolidGround = true;

	/// <summary>
	/// checks to see if the object can only spawn on the surface.
	/// </summary>
	[DocumentAsJson("Optional", "False", false)]
	public bool TryOnlySurface;

	/// <summary>
	/// Whether the rain and temperature values are referring to the worldgen values (i.e. yearly averages) or the current values at the moment of spawning.
	/// </summary>
	[DocumentAsJson("Optional", "WorldGenValues", false)]
	public EnumGetClimateMode ClimateValueMode;

	protected HashSet<Block> InsideBlockCodesResolved;

	protected string[] InsideBlockCodesBeginsWith;

	protected string[] InsideBlockCodesExact;

	protected string InsideBlockFirstLetters = "";

	/// <summary>
	/// Obsolete. Use <see cref="F:Vintagestory.API.Common.Entities.BaseSpawnConditions.HerdSize" /> instead.
	/// </summary>
	[DocumentAsJson("Obsolete", "", false)]
	[Obsolete("Use HerdSize instead")]
	public NatFloat GroupSize
	{
		get
		{
			return HerdSize;
		}
		set
		{
			HerdSize = value;
		}
	}

	public bool CanSpawnInside(Block testBlock)
	{
		if (testBlock.Code == null)
		{
			return false;
		}
		string testPath = testBlock.Code.Path;
		if (testPath.Length < 1)
		{
			return false;
		}
		if (InsideBlockFirstLetters.IndexOf(testPath[0]) < 0)
		{
			return false;
		}
		if (PathMatchesInsideBlockCodes(testPath))
		{
			return InsideBlockCodesResolved.Contains(testBlock);
		}
		return false;
	}

	private bool PathMatchesInsideBlockCodes(string testPath)
	{
		for (int i = 0; i < InsideBlockCodesExact.Length; i++)
		{
			if (testPath == InsideBlockCodesExact[i])
			{
				return true;
			}
		}
		for (int j = 0; j < InsideBlockCodesBeginsWith.Length; j++)
		{
			if (testPath.StartsWithOrdinal(InsideBlockCodesBeginsWith[j]))
			{
				return true;
			}
		}
		return false;
	}

	public void Initialise(LCGRandom lcgrnd, IServerWorldAccessor server, string entityName, Dictionary<AssetLocation, Block[]> searchCache)
	{
		if (RandomMapCodePool != null && RandomMapCodePool.Length != 0)
		{
			lcgrnd.InitPositionSeed(entityName.GetHashCode(), 0);
			MapCode = RandomMapCodePool[lcgrnd.NextInt(RandomMapCodePool.Length)];
		}
		if (InsideBlockCodes == null || InsideBlockCodes.Length == 0)
		{
			return;
		}
		bool anyBlockOk = false;
		AssetLocation[] insideBlockCodes = InsideBlockCodes;
		foreach (AssetLocation val in insideBlockCodes)
		{
			if (!searchCache.TryGetValue(val, out var foundBlocks))
			{
				foundBlocks = (searchCache[val] = server.SearchBlocks(val));
			}
			Block[] array2 = foundBlocks;
			foreach (Block b in array2)
			{
				if (InsideBlockCodesResolved == null)
				{
					InsideBlockCodesResolved = new HashSet<Block>();
				}
				InsideBlockCodesResolved.Add(b);
			}
			anyBlockOk |= foundBlocks.Length != 0;
		}
		if (!anyBlockOk)
		{
			server.Logger.Warning("Entity with code {0} has defined InsideBlockCodes for its spawn conditions, but none of these blocks exists, entity is unlikely to spawn.", entityName);
		}
		List<string> targetEntityCodesList = new List<string>();
		List<string> beginswith = new List<string>();
		AssetLocation[] codes = InsideBlockCodes;
		for (int k = 0; k < codes.Length; k++)
		{
			string code = codes[k].Path;
			if (code.EndsWith('*'))
			{
				beginswith.Add(code.Substring(0, code.Length - 1));
			}
			else
			{
				targetEntityCodesList.Add(code);
			}
		}
		InsideBlockCodesBeginsWith = beginswith.ToArray();
		InsideBlockCodesExact = new string[targetEntityCodesList.Count];
		int j2 = 0;
		foreach (string code2 in targetEntityCodesList)
		{
			if (code2.Length != 0)
			{
				InsideBlockCodesExact[j2++] = code2;
				char c = code2[0];
				if (InsideBlockFirstLetters.IndexOf(c) < 0)
				{
					InsideBlockFirstLetters += c;
				}
			}
		}
		string[] insideBlockCodesBeginsWith = InsideBlockCodesBeginsWith;
		foreach (string code3 in insideBlockCodesBeginsWith)
		{
			if (code3.Length != 0)
			{
				char c2 = code3[0];
				if (InsideBlockFirstLetters.IndexOf(c2) < 0)
				{
					InsideBlockFirstLetters += c2;
				}
			}
		}
	}
}
