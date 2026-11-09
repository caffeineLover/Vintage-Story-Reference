using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;
using Vintagestory.ServerMods;

namespace Vintagestory.GameContent;

public class StoryStructures : IStructureProvider, IEnumerable<KeyValuePair<string, StoryStructureLocation>>, IEnumerable
{
	public Vintagestory.API.Datastructures.OrderedDictionary<string, StoryStructureLocation> values = new Vintagestory.API.Datastructures.OrderedDictionary<string, StoryStructureLocation>();

	private StoryStructureLocation[] allStructures = Array.Empty<StoryStructureLocation>();

	public StoryStructureLocation Get(string code)
	{
		values.TryGetValue(code, out var location);
		return location;
	}

	protected void GenCache()
	{
		allStructures = values.Values.ToArray();
		StoryStructureLocation[] array = allStructures;
		foreach (StoryStructureLocation structure in array)
		{
			if (structure.SkipGenerationFlags != null)
			{
				int max = structure.SkipGenerationFlags.Max((KeyValuePair<int, int> flag) => flag.Value);
				structure.MaxSkipGenerationRadiusSq = max * max;
			}
		}
	}

	public static IStructureLocation GetBlockingStructureAt(int x, int z, int category, IEnumerable<StoryStructureLocation> locations)
	{
		foreach (StoryStructureLocation loc in locations)
		{
			int checkRadius = loc.LandformRadius;
			if (category == -1 || loc.SkipGenerationFlags.TryGetValue(category, out checkRadius))
			{
				if (checkRadius == 0 && loc.Location.Contains(x, z))
				{
					return loc;
				}
				float distSq = loc.CenterPos.HorDistanceSqTo(x, z);
				if (checkRadius > 0 && distSq < (float)(checkRadius * checkRadius))
				{
					return loc;
				}
			}
		}
		return null;
	}

	public IStructureLocation GetBlockingStructureAt(int x, int z, int category)
	{
		return GetBlockingStructureAt(x, z, category, allStructures);
	}

	public List<string> GetMissingStructures(List<string> attemptedToGen)
	{
		List<string> missingStructures = new List<string>();
		if (attemptedToGen != null)
		{
			foreach (string storyCode in attemptedToGen)
			{
				if (!values.ContainsKey(storyCode))
				{
					missingStructures.Add(storyCode);
				}
			}
		}
		return missingStructures;
	}

	public void Set(string code, StoryStructureLocation strucloc)
	{
		values[code] = strucloc;
		GenCache();
	}

	public IEnumerator<KeyValuePair<string, StoryStructureLocation>> GetEnumerator()
	{
		return values.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return values.GetEnumerator();
	}

	public void Load(Vintagestory.API.Datastructures.OrderedDictionary<string, StoryStructureLocation> strucs)
	{
		values = strucs;
		GenCache();
	}

	public byte[] ToBytes()
	{
		return SerializerUtil.Serialize(values);
	}
}
