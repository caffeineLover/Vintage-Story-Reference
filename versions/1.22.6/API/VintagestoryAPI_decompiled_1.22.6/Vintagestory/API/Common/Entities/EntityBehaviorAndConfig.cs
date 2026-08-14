using Vintagestory.API.Datastructures;

namespace Vintagestory.API.Common.Entities;

public class EntityBehaviorAndConfig
{
	public EntityBehavior Behavior;

	public JsonObject BehaviorConfig;

	public EntityBehaviorAndConfig(EntityBehavior behavior, JsonObject behaviorConfig)
	{
		Behavior = behavior;
		BehaviorConfig = behaviorConfig;
	}
}
