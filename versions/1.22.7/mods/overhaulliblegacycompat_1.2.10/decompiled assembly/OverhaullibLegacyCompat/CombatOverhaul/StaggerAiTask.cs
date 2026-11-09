using System;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace CombatOverhaul
{
	// Token: 0x0200000B RID: 11
	[NullableContext(1)]
	[Nullable(0)]
	public class StaggerAiTask : AiTaskBaseR
	{
		// Token: 0x06000083 RID: 131 RVA: 0x0000386A File Offset: 0x00001A6A
		public StaggerAiTask(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig) : base(entity, taskConfig, aiConfig)
		{
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00003880 File Offset: 0x00001A80
		public StaggerAiTask(EntityAgent entity, AiTaskBaseConfig config) : base(entity)
		{
			this.baseConfig = config;
		}

		// Token: 0x06000085 RID: 133 RVA: 0x0000389B File Offset: 0x00001A9B
		public override bool ShouldExecute()
		{
			return false;
		}

		// Token: 0x06000086 RID: 134 RVA: 0x0000389E File Offset: 0x00001A9E
		public override bool ContinueExecute(float dt)
		{
			return this.Staggered();
		}

		// Token: 0x06000087 RID: 135 RVA: 0x000038A6 File Offset: 0x00001AA6
		public override void FinishExecute(bool cancelled)
		{
			base.FinishExecute(cancelled);
		}

		// Token: 0x06000088 RID: 136 RVA: 0x000038AF File Offset: 0x00001AAF
		public virtual void AddStaggerTime(TimeSpan duration)
		{
			if (this.StaggerUntil < this.CurrentTime)
			{
				this.StaggerUntil = this.CurrentTime + duration;
				return;
			}
			this.StaggerUntil += duration;
		}

		// Token: 0x06000089 RID: 137 RVA: 0x000038E9 File Offset: 0x00001AE9
		public virtual void SetStaggerTime(TimeSpan duration)
		{
			this.StaggerUntil = this.CurrentTime + duration;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x000038FD File Offset: 0x00001AFD
		public virtual void ResetStaggerTime()
		{
			this.StaggerUntil = TimeSpan.Zero;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x0000390A File Offset: 0x00001B0A
		public virtual bool Staggered()
		{
			return this.StaggerUntil > this.CurrentTime;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600008C RID: 140 RVA: 0x0000391D File Offset: 0x00001B1D
		protected TimeSpan CurrentTime
		{
			get
			{
				return TimeSpan.FromMilliseconds(this.entity.Api.World.ElapsedMilliseconds);
			}
		}

		// Token: 0x04000023 RID: 35
		protected TimeSpan StaggerUntil = TimeSpan.Zero;
	}
}
