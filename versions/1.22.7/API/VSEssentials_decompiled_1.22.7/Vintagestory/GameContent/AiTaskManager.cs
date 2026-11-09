using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Util;

namespace Vintagestory.GameContent;

public sealed class AiTaskManager(Entity entity)
{
	public delegate bool OnExecuteTaskDelegate(IAiTask taskToExecute, IAiTask? activeTask);

	public const int ActiveTasksSlotsNumber = 8;

	private readonly Entity entity = entity;

	private readonly List<IAiTask> tasks = new List<IAiTask>();

	private readonly IAiTask?[] activeTasksBySlot = new IAiTask[8];

	private bool wasRunAiTasks;

	public bool Shuffle { get; set; }

	public IAiTask?[] ActiveTasksBySlot => activeTasksBySlot;

	public List<IAiTask> AllTasks => tasks;

	public event Action<IAiTask>? OnTaskStarted;

	public event Action<IAiTask>? OnTaskStopped;

	public event ActionBoolReturn<IAiTask>? OnShouldExecuteTask;

	public event OnExecuteTaskDelegate? OnExecuteTask;

	public void OnGameTick(float dt)
	{
		if (!AiRuntimeConfig.RunAiTasks)
		{
			if (wasRunAiTasks)
			{
				IAiTask[] array = activeTasksBySlot;
				for (int i = 0; i < array.Length; i++)
				{
					array[i]?.FinishExecute(cancelled: true);
				}
			}
			wasRunAiTasks = false;
		}
		else
		{
			wasRunAiTasks = true;
			if (Shuffle)
			{
				tasks.Shuffle(entity.World.Rand);
			}
			StartNewTasks();
			ProcessRunningTasks(dt);
			LogRunningTasks();
		}
	}

	public void AddTask(IAiTask task)
	{
		tasks.Add(task);
		task.ProfilerName = ("task-startexecute-" + AiTaskRegistry.TaskCodes[task.GetType()]).DeDuplicate();
	}

	public void RemoveTask(IAiTask task)
	{
		tasks.Remove(task);
	}

	public void AfterInitialize()
	{
		foreach (IAiTask task in tasks)
		{
			task.AfterInitialize();
		}
	}

	public void ExecuteTask(IAiTask task, int slot)
	{
		IAiTask activeTask = activeTasksBySlot[slot];
		if (this.OnExecuteTask != null)
		{
			bool execute = true;
			foreach (OnExecuteTaskDelegate dele in this.OnExecuteTask.GetInvocationList().Cast<OnExecuteTaskDelegate>())
			{
				execute &= dele(task, activeTask);
			}
			if (!execute)
			{
				return;
			}
		}
		if (activeTask != null)
		{
			activeTask.FinishExecute(cancelled: true);
			this.OnTaskStopped?.Invoke(activeTask);
		}
		task.StartExecute();
		activeTasksBySlot[slot] = task;
		this.OnTaskStarted?.Invoke(task);
		if (entity.World.FrameProfiler.Enabled)
		{
			entity.World.FrameProfiler.Mark(task.ProfilerName);
		}
	}

	public void ExecuteTask<TTask>() where TTask : IAiTask
	{
		TTask taskToExecute = GetTask<TTask>();
		if (taskToExecute != null)
		{
			ExecuteTask(taskToExecute, taskToExecute.Slot);
		}
	}

	public void ExecuteTask(string id)
	{
		IAiTask taskToExecute = GetTask(id);
		if (taskToExecute != null)
		{
			ExecuteTask(taskToExecute, taskToExecute.Slot);
		}
	}

	public TTask? GetTask<TTask>() where TTask : IAiTask
	{
		return (TTask)tasks.Find((IAiTask task) => task is TTask);
	}

	public IAiTask? GetTask(string id)
	{
		return tasks.Find((IAiTask task) => task.Id == id);
	}

	public IEnumerable<TTask> GetTasks<TTask>() where TTask : IAiTask
	{
		return tasks.OfType<TTask>();
	}

	public void StopTask(Type taskType)
	{
		for (int i = 0; i < activeTasksBySlot.Length; i++)
		{
			if (activeTasksBySlot[i]?.GetType() == taskType)
			{
				StopTask(i);
				break;
			}
		}
	}

	public void StopTask<TTask>() where TTask : IAiTask
	{
		for (int i = 0; i < activeTasksBySlot.Length; i++)
		{
			if (activeTasksBySlot[i] is TTask)
			{
				StopTask(i);
				break;
			}
		}
	}

	public void StopTask(string id)
	{
		for (int i = 0; i < activeTasksBySlot.Length; i++)
		{
			if (activeTasksBySlot[i]?.Id == id)
			{
				StopTask(i);
				break;
			}
		}
	}

	public void StopTask(int slot)
	{
		IAiTask task = activeTasksBySlot[slot];
		if (task != null)
		{
			task.FinishExecute(cancelled: true);
			this.OnTaskStopped?.Invoke(task);
			activeTasksBySlot[slot] = null;
			entity.World.FrameProfiler.Mark("finishexecute");
		}
	}

	public void StopTasks()
	{
		IAiTask[] array = activeTasksBySlot;
		foreach (IAiTask task in array)
		{
			if (task != null)
			{
				task.FinishExecute(cancelled: true);
				this.OnTaskStopped?.Invoke(task);
				activeTasksBySlot[task.Slot] = null;
			}
		}
	}

	public bool IsTaskActive(string id)
	{
		IAiTask[] array = activeTasksBySlot;
		foreach (IAiTask task in array)
		{
			if (task != null && task.Id == id)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsTaskActive<TTask>()
	{
		IAiTask[] array = activeTasksBySlot;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] is TTask)
			{
				return true;
			}
		}
		return false;
	}

	public void Notify(string key, object data)
	{
		if (key == "starttask")
		{
			string taskId = (string)data;
			if (activeTasksBySlot.FirstOrDefault((IAiTask aiTask) => aiTask?.Id == taskId) != null)
			{
				return;
			}
			IAiTask task = GetTask(taskId);
			if (task != null)
			{
				IAiTask activeTask = activeTasksBySlot[task.Slot];
				if (activeTask != null)
				{
					activeTask.FinishExecute(cancelled: true);
					this.OnTaskStopped?.Invoke(activeTask);
				}
				activeTasksBySlot[task.Slot] = null;
				ExecuteTask(task, task.Slot);
			}
			return;
		}
		if (key == "stoptask")
		{
			string taskId2 = (string)data;
			IAiTask task2 = activeTasksBySlot.FirstOrDefault((IAiTask aiTask) => aiTask?.Id == taskId2);
			if (task2 != null)
			{
				task2.FinishExecute(cancelled: true);
				this.OnTaskStopped?.Invoke(task2);
				activeTasksBySlot[task2.Slot] = null;
			}
			return;
		}
		for (int taskIndex = 0; taskIndex < tasks.Count; taskIndex++)
		{
			IAiTask task3 = tasks[taskIndex];
			if (!task3.Notify(key, data))
			{
				continue;
			}
			int slot = tasks[taskIndex].Slot;
			IAiTask activeTask2 = activeTasksBySlot[slot];
			if (activeTask2 == null || task3.Priority > activeTask2.PriorityForCancel)
			{
				if (activeTask2 != null)
				{
					activeTask2.FinishExecute(cancelled: true);
					this.OnTaskStopped?.Invoke(activeTask2);
				}
				activeTasksBySlot[slot] = task3;
				task3.StartExecute();
				this.OnTaskStarted?.Invoke(task3);
			}
		}
	}

	public void OnStateChanged(EnumEntityState beforeState)
	{
		foreach (IAiTask task in tasks)
		{
			task.OnStateChanged(beforeState);
		}
	}

	public void OnEntitySpawn()
	{
		foreach (IAiTask task in tasks)
		{
			task.OnEntitySpawn();
		}
	}

	public void OnEntityLoaded()
	{
		foreach (IAiTask task in tasks)
		{
			task.OnEntityLoaded();
		}
	}

	public void OnEntityDespawn(EntityDespawnData reason)
	{
		foreach (IAiTask task in tasks)
		{
			task.OnEntityDespawn(reason);
		}
	}

	public void OnEntityHurt(DamageSource source, float damage)
	{
		foreach (IAiTask task in tasks)
		{
			task.OnEntityHurt(source, damage);
		}
	}

	private void StartNewTasks()
	{
		foreach (IAiTask task in tasks)
		{
			if (task.Priority < 0f)
			{
				continue;
			}
			int slot = task.Slot;
			IAiTask oldTask = activeTasksBySlot[slot];
			if ((oldTask == null || task.Priority > oldTask.PriorityForCancel) && task.ShouldExecute() && ShouldExecuteTask(task))
			{
				oldTask?.FinishExecute(cancelled: true);
				if (oldTask != null)
				{
					this.OnTaskStopped?.Invoke(oldTask);
				}
				activeTasksBySlot[slot] = task;
				task.StartExecute();
				this.OnTaskStarted?.Invoke(task);
			}
			if (entity.World.FrameProfiler.Enabled)
			{
				entity.World.FrameProfiler.Mark(task.ProfilerName);
			}
		}
	}

	private bool ShouldExecuteTask(IAiTask task)
	{
		if (this.OnShouldExecuteTask == null)
		{
			return true;
		}
		bool exec = true;
		Delegate[] invocationList = this.OnShouldExecuteTask.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			ActionBoolReturn<IAiTask> dele = (ActionBoolReturn<IAiTask>)invocationList[i];
			exec &= dele(task);
		}
		return exec;
	}

	private void ProcessRunningTasks(float dt)
	{
		for (int index = 0; index < activeTasksBySlot.Length; index++)
		{
			IAiTask task = activeTasksBySlot[index];
			if (task != null && task.CanContinueExecute())
			{
				if (!task.ContinueExecute(dt))
				{
					task.FinishExecute(cancelled: false);
					this.OnTaskStopped?.Invoke(task);
					activeTasksBySlot[index] = null;
				}
				if (entity.World.FrameProfiler.Enabled)
				{
					entity.World.FrameProfiler.Mark("task-continueexec-", AiTaskRegistry.TaskCodes[task.GetType()]);
				}
			}
		}
	}

	private void LogRunningTasks()
	{
		if (!entity.World.EntityDebugMode)
		{
			return;
		}
		StringBuilder tasksInfo = new StringBuilder();
		int taskIndex = 0;
		for (int slotIndex = 0; slotIndex < activeTasksBySlot.Length; slotIndex++)
		{
			IAiTask task = activeTasksBySlot[slotIndex];
			if (task != null)
			{
				if (taskIndex++ > 0)
				{
					tasksInfo.Append(", ");
				}
				AiTaskRegistry.TaskCodes.TryGetValue(task.GetType(), out string code);
				code = code + "/" + task.Id;
				StringBuilder stringBuilder = tasksInfo;
				StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(8, 3, stringBuilder);
				handler.AppendFormatted(code);
				handler.AppendLiteral("(p");
				handler.AppendFormatted(task.Priority);
				handler.AppendLiteral(", pc ");
				handler.AppendFormatted(task.PriorityForCancel);
				handler.AppendLiteral(")");
				stringBuilder.Append(ref handler);
			}
		}
		entity.DebugAttributes.SetString("AI Tasks", (tasksInfo.Length > 0) ? tasksInfo.ToString() : "-");
	}
}
