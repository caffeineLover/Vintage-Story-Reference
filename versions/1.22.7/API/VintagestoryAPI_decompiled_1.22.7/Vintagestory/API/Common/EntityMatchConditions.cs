using System.Collections.Generic;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace Vintagestory.API.Common;

public class EntityMatchConditions
{
	public AssetLocation Type;

	public string Classstr;

	public string Tagstr;

	public float? Range;

	public Cuboidi Box;

	public string Name;

	public bool? Alive;

	public long? Id;

	public int ConditionCount;

	public static EntityMatchConditions FromArgs(Dictionary<string, string> subargs)
	{
		return new EntityMatchConditions().LoadFromArgs(subargs);
	}

	public EntityMatchConditions LoadFromArgs(Dictionary<string, string> subargs)
	{
		if (subargs.TryGetValue("range", out var strrange))
		{
			Range = strrange.ToFloat();
			subargs.Remove("range");
			ConditionCount++;
		}
		if (subargs.TryGetValue("type", out var typestr))
		{
			Type = new AssetLocation(typestr);
			subargs.Remove("type");
			ConditionCount++;
		}
		if (subargs.TryGetValue("class", out var classstr))
		{
			Classstr = classstr.ToLowerInvariant();
			subargs.Remove("class");
			ConditionCount++;
		}
		if (subargs.TryGetValue("tag", out var tagstr))
		{
			Tagstr = tagstr.ToLowerInvariant();
			subargs.Remove("tag");
			ConditionCount++;
		}
		if (subargs.TryGetValue("name", out var name))
		{
			Name = name;
			subargs.Remove("name");
			ConditionCount++;
		}
		if (subargs.TryGetValue("alive", out var stralive))
		{
			Alive = stralive.ToBool();
			subargs.Remove("alive");
			ConditionCount++;
		}
		if (subargs.TryGetValue("id", out var strid))
		{
			Id = strid.ToLong(0L);
			subargs.Remove("id");
			ConditionCount++;
		}
		return this;
	}
}
