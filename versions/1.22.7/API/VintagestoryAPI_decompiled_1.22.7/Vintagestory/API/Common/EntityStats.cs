using System.Collections;
using System.Collections.Generic;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace Vintagestory.API.Common;

public class EntityStats : IEnumerable<KeyValuePair<string, EntityFloatStats>>, IEnumerable
{
	private IDictionary<string, EntityFloatStats> floatStats = new FastSmallDictionary<string, EntityFloatStats>(0);

	private Entity entity;

	private bool ignoreChange;

	public EntityFloatStats this[string key]
	{
		get
		{
			return floatStats[key];
		}
		set
		{
			floatStats[key] = value;
		}
	}

	public EntityStats(Entity entity)
	{
		this.entity = entity;
	}

	public void Initialize(ICoreAPI api)
	{
		if (api.Side == EnumAppSide.Client)
		{
			entity.WatchedAttributes.RegisterModifiedListener("stats", onStatsChanged);
		}
	}

	private void onStatsChanged()
	{
		if (!ignoreChange)
		{
			FromTreeAttributes(entity.WatchedAttributes);
		}
	}

	public IEnumerator<KeyValuePair<string, EntityFloatStats>> GetEnumerator()
	{
		return floatStats.GetEnumerator();
	}

	IEnumerator<KeyValuePair<string, EntityFloatStats>> IEnumerable<KeyValuePair<string, EntityFloatStats>>.GetEnumerator()
	{
		return floatStats.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return floatStats.GetEnumerator();
	}

	public void ToTreeAttributes(ITreeAttribute tree, bool forClient)
	{
		if (floatStats.Count == 0)
		{
			return;
		}
		TreeAttribute statstree;
		if (tree.TryGetAttribute("stats", out var attr))
		{
			statstree = attr as TreeAttribute;
			if (statstree != null)
			{
				statstree.Clear();
				goto IL_0041;
			}
		}
		statstree = (TreeAttribute)(tree["stats"] = new TreeAttribute());
		goto IL_0041;
		IL_0041:
		foreach (KeyValuePair<string, EntityFloatStats> stats in floatStats)
		{
			TreeAttribute subtree = new TreeAttribute();
			stats.Value.ToTreeAttributes(subtree, forClient);
			statstree[stats.Key] = subtree;
		}
	}

	public void FromTreeAttributes(ITreeAttribute tree)
	{
		if (!(tree["stats"] is ITreeAttribute subtree))
		{
			return;
		}
		foreach (KeyValuePair<string, IAttribute> val in subtree)
		{
			EntityFloatStats stats = new EntityFloatStats();
			stats.FromTreeAttributes(val.Value as ITreeAttribute);
			floatStats[val.Key] = stats;
		}
	}

	/// <summary>
	/// Set up a stat. Its not required to call this method, you can go straight to doing .Set() if your blend type is weighted sum. Also initializes a base value of 1.
	/// </summary>
	/// <param name="category"></param>
	/// <param name="blendType"></param>
	/// <returns></returns>
	public EntityStats Register(string category, EnumStatBlendType blendType = EnumStatBlendType.WeightedSum)
	{
		EntityFloatStats stats = (floatStats[category] = new EntityFloatStats());
		stats.BlendType = blendType;
		return this;
	}

	/// <summary>
	/// Set a stat value, if the stat catgory does not exist, it will create a new default one. Initializes a base value of 1 when creating a new stat.
	/// </summary>
	/// <param name="category"></param>
	/// <param name="code"></param>
	/// <param name="value"></param>
	/// <param name="persistent"></param>
	/// <returns></returns>
	public EntityStats Set(string category, string code, float value, bool persistent = false)
	{
		ignoreChange = true;
		if (!floatStats.TryGetValue(category, out var stats))
		{
			stats = (floatStats[category] = new EntityFloatStats());
		}
		stats.Set(code, value, persistent);
		ToTreeAttributes(entity.WatchedAttributes, forClient: true);
		entity.WatchedAttributes.MarkPathDirty("stats");
		ignoreChange = false;
		return this;
	}

	/// <summary>
	/// Remove a stat value
	/// </summary>
	/// <param name="category"></param>
	/// <param name="code"></param>
	/// <returns></returns>
	public EntityStats Remove(string category, string code)
	{
		ignoreChange = true;
		if (floatStats.TryGetValue(category, out var stats))
		{
			stats.Remove(code);
		}
		ToTreeAttributes(entity.WatchedAttributes, forClient: true);
		entity.WatchedAttributes.MarkPathDirty("stats");
		ignoreChange = false;
		return this;
	}

	/// <summary>
	/// Get the final stat value, blended by the stats blend type
	/// </summary>
	/// <param name="category"></param>
	/// <returns></returns>
	public float GetBlended(string category)
	{
		if (floatStats.TryGetValue(category, out var stats))
		{
			return stats.GetBlended();
		}
		return 1f;
	}
}
