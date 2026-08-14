using System.Collections.Generic;
using Vintagestory.API.Datastructures;

namespace Vintagestory.API.Common.Entities;

public class EntityServerProperties : EntitySidedProperties
{
	/// <summary>
	/// The conditions for spawning the entity.
	/// </summary>
	public SpawnConditions SpawnConditions;

	public EntityServerProperties(JsonObject[] behaviors, Dictionary<string, JsonObject> commonConfigs)
		: base(behaviors, commonConfigs)
	{
	}

	/// <summary>
	/// Makes a copy of this EntiyServerProperties type
	/// </summary>
	/// <returns></returns>
	public override EntitySidedProperties Clone()
	{
		return new EntityServerProperties(BehaviorsAsJsonObj, null)
		{
			Attributes = Attributes,
			SpawnConditions = SpawnConditions
		};
	}

	/// <summary>
	/// Returns a list of all entity behaviors that return true for ShouldEarlyLoadCollectibleMappings
	/// <br />Note, these behaviors are temporarily created in this list only, not attached to the entity, and should be discarded by the caller
	/// </summary>
	/// <param name="entity"></param>
	/// <param name="world"></param>
	/// <returns></returns>
	public List<EntityBehaviorAndConfig> BehaviorsWithEarlyLoadCollectibleMappings(Entity entity, IWorldAccessor world)
	{
		if (BehaviorsAsJsonObj == null)
		{
			return null;
		}
		List<EntityBehaviorAndConfig> behaviors = new List<EntityBehaviorAndConfig>();
		for (int i = 0; i < BehaviorsAsJsonObj.Length; i++)
		{
			JsonObject jobj = BehaviorsAsJsonObj[i];
			string code = jobj["code"].AsString();
			if (world.ClassRegistry.GetEntityBehaviorClass(code) != null)
			{
				EntityBehavior behavior = world.ClassRegistry.CreateEntityBehavior(entity, code);
				if (behavior.ShouldEarlyLoadCollectibleMappings())
				{
					behavior.FromBytes(isSync: false);
					behaviors.Add(new EntityBehaviorAndConfig(behavior, jobj));
				}
			}
		}
		return behaviors;
	}
}
