using System;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public class EntityBehaviorActivityDriven : EntityBehavior
{
	public delegate EnumInteruptionType ActivitySystemInterruptionHandler();

	private ICoreAPI Api;

	public EntityActivitySystem ActivitySystem;

	private bool active = true;

	private bool wasRunAiActivities;

	public event ActivitySystemInterruptionHandler OnShouldRunActivitySystem;

	public EntityBehaviorActivityDriven(Entity entity)
		: base(entity)
	{
		Api = entity.Api;
		if (!(entity is EntityAgent))
		{
			throw new InvalidOperationException("ActivityDriven behavior only avaialble for EntityAgent classes.");
		}
		ActivitySystem = new EntityActivitySystem(entity as EntityAgent);
	}

	public override void Initialize(EntityProperties properties, JsonObject attributes)
	{
		base.Initialize(properties, attributes);
		string path = attributes?["activityCollectionPath"]?.AsString();
		load(path);
	}

	public bool load(string p)
	{
		return ActivitySystem.Load((p == null) ? null : AssetLocation.Create(p, entity.Code.Domain));
	}

	public override void OnEntitySpawn()
	{
		base.OnEntitySpawn();
		if (Api.Side == EnumAppSide.Server)
		{
			setupTaskBlocker();
		}
	}

	public override void OnEntityLoaded()
	{
		base.OnEntityLoaded();
		if (Api.Side == EnumAppSide.Server)
		{
			setupTaskBlocker();
		}
	}

	private void setupTaskBlocker()
	{
		EntityAgent eagent = entity as EntityAgent;
		EntityBehaviorTaskAI taskAi = entity.GetBehavior<EntityBehaviorTaskAI>();
		if (taskAi != null)
		{
			taskAi.TaskManager.OnShouldExecuteTask += delegate(IAiTask task)
			{
				if (task is AiTaskGotoEntity)
				{
					return true;
				}
				return eagent.MountedOn == null && ActivitySystem.ActiveActivitiesBySlot.Values.Any((IEntityActivity a) => a.CurrentAction?.Type == "standardai");
			};
		}
		EntityBehaviorConversable ebc = entity.GetBehavior<EntityBehaviorConversable>();
		if (ebc != null)
		{
			ebc.CanConverse += Ebc_CanConverse;
		}
	}

	private bool Ebc_CanConverse(out string errorMessage)
	{
		bool canTalk = !Api.ModLoader.GetModSystem<VariablesModSystem>().GetVariable(EnumActivityVariableScope.Entity, "tooBusyToTalk", entity).ToBool();
		errorMessage = (canTalk ? null : "cantconverse-toobusy");
		return canTalk;
	}

	public override void OnGameTick(float deltaTime)
	{
		if (!entity.Alive)
		{
			return;
		}
		if (!AiRuntimeConfig.RunAiActivities)
		{
			if (wasRunAiActivities)
			{
				ActivitySystem.CancelAll();
			}
			wasRunAiActivities = false;
			return;
		}
		wasRunAiActivities = AiRuntimeConfig.RunAiActivities;
		base.OnGameTick(deltaTime);
		if (this.OnShouldRunActivitySystem != null)
		{
			bool wasActive = active;
			active = true;
			EnumInteruptionType interuptionType = EnumInteruptionType.None;
			Delegate[] invocationList = this.OnShouldRunActivitySystem.GetInvocationList();
			for (int i = 0; i < invocationList.Length; i++)
			{
				EnumInteruptionType result = ((ActivitySystemInterruptionHandler)invocationList[i])();
				if (result > interuptionType)
				{
					interuptionType = result;
				}
			}
			active = interuptionType == EnumInteruptionType.None;
			if (wasActive && !active)
			{
				ActivitySystem.Pause(interuptionType);
			}
			if (!wasActive && active)
			{
				ActivitySystem.Resume();
			}
		}
		Api.World.FrameProfiler.Mark("behavior-activitydriven-checks");
		if (active)
		{
			ActivitySystem.OnTick(deltaTime);
		}
	}

	public override string PropertyName()
	{
		return "activitydriven";
	}
}
