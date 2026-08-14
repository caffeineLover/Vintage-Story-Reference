using Vintagestory.API;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace Vintagestory.GameContent;

[DocumentAsJson]
public class EntityBehaviorSemiTamed : EntityBehavior
{
	public EntityBehaviorSemiTamed(Entity entity)
		: base(entity)
	{
	}

	public override void AfterInitialized(bool onFirstSpawn)
	{
		base.AfterInitialized(onFirstSpawn);
		entity.GetBehavior<EntityBehaviorTaskAI>().TaskManager.OnShouldExecuteTask += TaskManager_OnShouldExecuteTask;
	}

	private bool TaskManager_OnShouldExecuteTask(IAiTask t)
	{
		if (t is AiTaskFleeEntity fleetask)
		{
			if (fleetask.WhenInEmotionStates == null)
			{
				return !(fleetask.targetEntity is EntityPlayer);
			}
			return false;
		}
		return true;
	}

	public override string PropertyName()
	{
		return "semitamed";
	}
}
