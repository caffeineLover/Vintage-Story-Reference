using System;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace PetAI
{
	// Token: 0x0200000C RID: 12
	public class AiTaskTrick : AiTaskIdle
	{
		// Token: 0x0600002F RID: 47 RVA: 0x000036E4 File Offset: 0x000018E4
		public AiTaskTrick(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig) : base(entity, taskConfig, aiConfig)
		{
			this.commandName = taskConfig["command"].AsString(null);
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000030 RID: 48 RVA: 0x00003706 File Offset: 0x00001906
		public override int Slot
		{
			get
			{
				return base.Slot;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000031 RID: 49 RVA: 0x0000370E File Offset: 0x0000190E
		public override float Priority
		{
			get
			{
				return base.Priority;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000032 RID: 50 RVA: 0x00003716 File Offset: 0x00001916
		public override float PriorityForCancel
		{
			get
			{
				return base.PriorityForCancel;
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000371E File Offset: 0x0000191E
		public override bool ShouldExecute()
		{
			EntityBehaviorReceiveCommand behavior = this.entity.GetBehavior<EntityBehaviorReceiveCommand>();
			return ((behavior != null) ? behavior.SimpleCommand : null) == this.commandName;
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003742 File Offset: 0x00001942
		public override void StartExecute()
		{
			EntityBehaviorReceiveCommand behavior = this.entity.GetBehavior<EntityBehaviorReceiveCommand>();
			if (behavior != null)
			{
				behavior.SetCommand(new Command(EnumCommandType.SIMPLE, null), null);
			}
			base.StartExecute();
		}

		// Token: 0x04000024 RID: 36
		public string commandName;
	}
}
