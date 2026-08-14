using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Vintagestory.API.Common;

/// <summary>
/// Caches results of wildcard pre search among collectibles.
/// Use it to get list of collectibles before applying wildcard search, to reduce number of wildcard evaluations on collectibles codes.
/// </summary>
public sealed class CollectiblePreSearchResultsCache
{
	private readonly ConcurrentDictionary<string, List<CollectibleObject>> cache = new ConcurrentDictionary<string, List<CollectibleObject>>();

	public List<CollectibleObject> GetOrCreate(IWorldAccessor world, AssetLocation code, EnumItemClass collectibleType)
	{
		string codeStartsWith = getCodeBeforeWildcard(code.Path);
		string cacheKey = $"{code.Domain}:{codeStartsWith}:{collectibleType}";
		return cache.GetOrAdd(cacheKey, (string _) => search(world, collectibleType, codeStartsWith));
	}

	public void Clear()
	{
		cache.Clear();
	}

	private static string getCodeBeforeWildcard(string codePath)
	{
		if (string.IsNullOrEmpty(codePath))
		{
			return codePath;
		}
		int wildcardStartIndex = codePath.IndexOfAny(new char[3] { '*', '{', '?' });
		if (wildcardStartIndex >= 0)
		{
			return codePath.Substring(0, wildcardStartIndex);
		}
		return codePath;
	}

	private static List<CollectibleObject> search(IWorldAccessor world, EnumItemClass collectibleType, string codeStartsWith)
	{
		return (from collectible in collectibleType switch
			{
				EnumItemClass.Block => world.Blocks, 
				EnumItemClass.Item => world.Items, 
				_ => throw new NotImplementedException(), 
			}
			where !collectible.IsMissing
			where collectible.Code?.Path != null && collectible.Code.Path.Contains(codeStartsWith)
			select collectible).ToList();
	}
}
