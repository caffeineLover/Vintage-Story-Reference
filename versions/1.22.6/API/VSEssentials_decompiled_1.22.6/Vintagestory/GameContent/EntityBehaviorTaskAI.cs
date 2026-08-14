using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.Essentials;

namespace Vintagestory.GameContent;

public class EntityBehaviorTaskAI(Entity entity) : EntityBehavior(entity)
{
	public AiTaskManager TaskManager = new AiTaskManager(entity);

	public WaypointsTraverser? PathTraverser;

	public override string PropertyName()
	{
		return "taskai";
	}

	public override void OnEntitySpawn()
	{
		base.OnEntitySpawn();
		TaskManager.OnEntitySpawn();
	}

	public override void OnEntityLoaded()
	{
		base.OnEntityLoaded();
		TaskManager.OnEntityLoaded();
	}

	public override void OnEntityDespawn(EntityDespawnData despawn)
	{
		base.OnEntityDespawn(despawn);
		TaskManager.OnEntityDespawn(despawn);
	}

	public override void OnEntityReceiveDamage(DamageSource damageSource, ref float damage)
	{
		base.OnEntityReceiveDamage(damageSource, ref damage);
		TaskManager.OnEntityHurt(damageSource, damage);
	}

	public override void Initialize(EntityProperties properties, JsonObject attributes)
	{
		ILogger logger = entity.World.Logger;
		if (!(entity is EntityAgent entityAgent))
		{
			logger.Error($"The task ai currently only works on entities inheriting from EntityAgent. Will ignore loading tasks for entity {entity.Code}.");
			return;
		}
		TaskManager.Shuffle = attributes["shuffle"].AsBool();
		string creatureTypeStr = attributes["aiCreatureType"].AsString("Default");
		if (!Enum.TryParse<EnumAICreatureType>(creatureTypeStr, out var creatureType))
		{
			creatureType = EnumAICreatureType.Default;
			logger.Warning($"Entity {entity.Code} Task AI, invalid aiCreatureType '{creatureTypeStr}'. Will default to 'Default'.");
		}
		PathTraverser = new WaypointsTraverser(entityAgent, creatureType);
		JsonObject[] tasks = attributes["aitasks"]?.AsArray();
		if (tasks == null)
		{
			return;
		}
		JsonObject[] array = tasks;
		foreach (JsonObject taskConfig in array)
		{
			JsonObject jsonObject = taskConfig["enabled"];
			if (jsonObject != null && !jsonObject.AsBool(defaultValue: true))
			{
				continue;
			}
			string taskCode = taskConfig["code"]?.AsString();
			if (taskCode == null)
			{
				logger.Error($"Task does not have 'code' specified, for entity '{entity.Code}', will skip it.");
				continue;
			}
			if (!AiTaskRegistry.TaskTypes.TryGetValue(taskCode, out Type taskType))
			{
				logger.Error($"Task with code {taskCode} for entity {entity.Code} does not exist, will skip it.");
				continue;
			}
			IAiTask task;
			try
			{
				task = (IAiTask)Activator.CreateInstance(taskType, entityAgent, taskConfig, attributes);
			}
			catch
			{
				logger.Error($"Task with code '{taskCode}' for entity '{entity.Code}': failed to instantiate task, possible error in task config json.");
				throw;
			}
			if (task != null)
			{
				TaskManager.AddTask(task);
				continue;
			}
			logger.Error($"Task with code {taskCode} for entity {entity.Code}: failed to instantiate task.");
		}
	}

	public override void AfterInitialized(bool onFirstSpawn)
	{
		TaskManager.AfterInitialize();
	}

	public override void OnGameTick(float deltaTime)
	{
		if (entity.State == EnumEntityState.Active && entity.Alive)
		{
			entity.World.FrameProfiler.Mark("ai-init");
			PathTraverser?.OnGameTick(deltaTime);
			entity.World.FrameProfiler.Mark("ai-pathfinding");
			entity.World.FrameProfiler.Enter("ai-tasks");
			TaskManager.OnGameTick(deltaTime);
			entity.World.FrameProfiler.Leave();
		}
	}

	public override void OnStateChanged(EnumEntityState beforeState, ref EnumHandling handling)
	{
		TaskManager.OnStateChanged(beforeState);
	}

	public override void Notify(string key, object data)
	{
		TaskManager.Notify(key, data);
	}
}
