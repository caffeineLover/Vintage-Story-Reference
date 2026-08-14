using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace PetAI
{
	// Token: 0x0200000A RID: 10
	public class AiTaskSeekNest : AiTaskBase
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00002FCF File Offset: 0x000011CF
		// (set) Token: 0x06000021 RID: 33 RVA: 0x00002FD7 File Offset: 0x000011D7
		private BlockEntityPetNest Nest { get; set; }

		// Token: 0x06000022 RID: 34 RVA: 0x00002FE0 File Offset: 0x000011E0
		public AiTaskSeekNest(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig) : base(entity, taskConfig, aiConfig)
		{
			this.range = taskConfig["horRange"].AsInt(15);
			this.moveSpeed = taskConfig["movespeed"].AsFloat(0.02f);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000303C File Offset: 0x0000123C
		public override bool ShouldExecute()
		{
			if (this.lastCheck + 10000L > this.entity.World.ElapsedMilliseconds)
			{
				return false;
			}
			this.lastCheck = this.entity.World.ElapsedMilliseconds;
			if (this.duringDayTimeFrames.Length != 0)
			{
				double hourOfDay = (double)(this.entity.World.Calendar.HourOfDay / this.entity.World.Calendar.HoursPerDay * 24f) + (this.entity.World.Rand.NextDouble() * 0.30000001192092896 - 0.15000000596046448);
				if (!Array.Exists<DayTimeFrame>(this.duringDayTimeFrames, (DayTimeFrame frame) => frame.Matches(hourOfDay)))
				{
					return false;
				}
			}
			if (this.Nest == null || this.entity.Pos.SquareDistanceTo(this.Nest.Position) > 50.0)
			{
				this.Nest = (this.entity.Api.ModLoader.GetModSystem<POIRegistry>(true).GetNearestPoi(this.entity.Pos.XYZ, (float)this.range, new PoiMatcher(this.IsValidNonOccupiedNest)) as BlockEntityPetNest);
			}
			return this.Nest != null && this.entity.Pos.SquareDistanceTo(this.Nest.Pos.ToVec3d()) > 2.0;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000031BC File Offset: 0x000013BC
		private bool IsValidNonOccupiedNest(IPointOfInterest poi)
		{
			BlockEntityPetNest nest = poi as BlockEntityPetNest;
			if (nest != null)
			{
				if ((nest.Block as BlockPetNest).nestSize < this.entity.GetBehavior<EntityBehaviorTameable>().Size)
				{
					return false;
				}
				if (this.entity.World.GetEntitiesAround(nest.Position, 3f, 3f, delegate(Entity occupier)
				{
					EntityBehaviorTaskAI behavior = occupier.GetBehavior<EntityBehaviorTaskAI>();
					BlockEntityPetNest blockEntityPetNest;
					if (behavior == null)
					{
						blockEntityPetNest = null;
					}
					else
					{
						AiTaskManager taskManager = behavior.TaskManager;
						if (taskManager == null)
						{
							blockEntityPetNest = null;
						}
						else
						{
							AiTaskSeekNest task = taskManager.GetTask<AiTaskSeekNest>();
							blockEntityPetNest = ((task != null) ? task.Nest : null);
						}
					}
					return blockEntityPetNest == nest;
				}).Length == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00003244 File Offset: 0x00001444
		public override void StartExecute()
		{
			base.StartExecute();
			this.stuck = false;
			this.pathTraverser.WalkTowards(this.Nest.MiddlePostion, this.moveSpeed, 0.12f, delegate()
			{
			}, delegate()
			{
				this.stuck = true;
			}, 0);
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000032AC File Offset: 0x000014AC
		public override bool ContinueExecute(float dt)
		{
			return !this.stuck && this.pathTraverser.Active;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000032C3 File Offset: 0x000014C3
		public override string ToString()
		{
			return base.ToString();
		}

		// Token: 0x04000019 RID: 25
		private readonly int range = 15;

		// Token: 0x0400001A RID: 26
		private long lastCheck;

		// Token: 0x0400001B RID: 27
		private bool stuck;

		// Token: 0x0400001C RID: 28
		private readonly float moveSpeed = 0.02f;
	}
}
