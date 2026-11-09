using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace CombatOverhaul
{
	// Token: 0x0200000A RID: 10
	[NullableContext(1)]
	[Nullable(0)]
	public class StaggerBehavior : EntityBehavior
	{
		// Token: 0x0600007C RID: 124 RVA: 0x00003683 File Offset: 0x00001883
		public StaggerBehavior(Entity entity) : base(entity)
		{
		}

		// Token: 0x0600007D RID: 125 RVA: 0x0000368C File Offset: 0x0000188C
		public override string PropertyName()
		{
			return "StaggerBehavior";
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003694 File Offset: 0x00001894
		public override void Initialize(EntityProperties properties, JsonObject attributes)
		{
			EntityAgent agent = this.entity as EntityAgent;
			if (agent == null)
			{
				return;
			}
			JsonObject jsonObject = attributes["taskConfig"];
			this.AiTaskConfig = ((jsonObject != null) ? jsonObject.AsObject<AiTaskBaseConfig>(null) : null);
			this.ResistanceTier = attributes["resistanceTier"].AsInt(0);
			AiTaskBaseConfig aiTaskConfig = this.AiTaskConfig;
			if (aiTaskConfig == null)
			{
				return;
			}
			aiTaskConfig.Init(agent);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x000036F8 File Offset: 0x000018F8
		public override void AfterInitialized(bool onFirstSpawn)
		{
			EntityBehaviorTaskAI behavior = this.entity.GetBehavior<EntityBehaviorTaskAI>();
			if (behavior != null && this.AiTaskConfig != null)
			{
				EntityAgent agent = this.entity as EntityAgent;
				if (agent != null)
				{
					behavior.TaskManager.AddTask(new StaggerAiTask(agent, this.AiTaskConfig));
					return;
				}
			}
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00003744 File Offset: 0x00001944
		public void TriggerStagger(TimeSpan duration, int tier)
		{
			if (!this.entity.Alive)
			{
				return;
			}
			EntityBehaviorTaskAI behavior = this.entity.GetBehavior<EntityBehaviorTaskAI>();
			if (behavior == null)
			{
				return;
			}
			AiTaskManager taskManager = behavior.TaskManager;
			StaggerAiTask task = (taskManager != null) ? taskManager.GetTask<StaggerAiTask>() : null;
			if (task == null)
			{
				return;
			}
			foreach (string animation in this.entity.AnimManager.ActiveAnimationsByAnimCode.Keys)
			{
				this.entity.AnimManager.StopAnimation(animation);
			}
			task.SetStaggerTime(duration * (double)this.ApplyResistance(tier));
			AiTaskManager taskManager2 = behavior.TaskManager;
			if (taskManager2 == null)
			{
				return;
			}
			taskManager2.ExecuteTask<StaggerAiTask>();
		}

		// Token: 0x06000081 RID: 129 RVA: 0x0000380C File Offset: 0x00001A0C
		public void ClearStagger()
		{
			EntityBehaviorTaskAI behavior = this.entity.GetBehavior<EntityBehaviorTaskAI>();
			if (behavior == null)
			{
				return;
			}
			AiTaskManager taskManager = behavior.TaskManager;
			StaggerAiTask task = (taskManager != null) ? taskManager.GetTask<StaggerAiTask>() : null;
			if (task == null)
			{
				return;
			}
			task.ResetStaggerTime();
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00003846 File Offset: 0x00001A46
		protected virtual float ApplyResistance(int tier)
		{
			if (this.ResistanceTier <= tier)
			{
				return 1f;
			}
			return MathF.Pow(0.5f, (float)(tier - this.ResistanceTier));
		}

		// Token: 0x04000021 RID: 33
		[Nullable(2)]
		protected AiTaskBaseConfig AiTaskConfig;

		// Token: 0x04000022 RID: 34
		protected int ResistanceTier;
	}
}
