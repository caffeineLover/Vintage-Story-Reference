using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace Cats
{
	// Token: 0x02000002 RID: 2
	public class AiTaskFreezeNear : AiTaskBase
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public AiTaskFreezeNear(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig) : base(entity, taskConfig, aiConfig)
		{
			this.seekingRange = taskConfig["seekingRange"].AsFloat(8f);
			this.entityCodes.AddRange(taskConfig["entityCodes"].AsArray<string>(Array.Empty<string>(), "game"));
			this.partitionUtil = entity.Api.ModLoader.GetModSystem<EntityPartitioning>(true);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020C8 File Offset: 0x000002C8
		public override bool ShouldExecute()
		{
			if (this.lastSearchTotalMs + 3000L > this.entity.World.ElapsedMilliseconds)
			{
				return false;
			}
			this.lastSearchTotalMs = this.entity.World.ElapsedMilliseconds;
			this.targetEntity = (this.partitionUtil.GetNearestInteractableEntity(this.entity.Pos.XYZ, (double)this.seekingRange, (Entity target) => this.entityCodes.Find(delegate(string code)
			{
				if (!code.EndsWith("*"))
				{
					return code == target.Code.Path;
				}
				return target.Code.Path.StartsWith(code.Substring(0, code.Length - 1));
			}) != null) as EntityAgent);
			return this.targetEntity != null;
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002150 File Offset: 0x00000350
		public override bool ContinueExecute(float dt)
		{
			if (this.targetEntity == null)
			{
				return false;
			}
			if (!this.targetEntity.Alive)
			{
				return false;
			}
			Vec3f targetVec = new Vec3f((float)(this.targetEntity.Pos.X - this.entity.Pos.X), (float)(this.targetEntity.Pos.Y - this.entity.Pos.Y), (float)(this.targetEntity.Pos.Z - this.entity.Pos.Z));
			float desiredYaw = (float)Math.Atan2((double)targetVec.X, (double)targetVec.Z);
			float yawDist = GameMath.AngleRadDistance(this.entity.Pos.Yaw, desiredYaw);
			this.entity.Pos.Yaw += GameMath.Clamp(yawDist, -250f * dt, 250f * dt);
			this.entity.Pos.Yaw = this.entity.Pos.Yaw % 6.2831855f;
			return this.entity.Pos.SquareDistanceTo(this.targetEntity.Pos) < this.seekingRange * this.seekingRange;
		}

		// Token: 0x04000001 RID: 1
		private EntityAgent targetEntity;

		// Token: 0x04000002 RID: 2
		private readonly List<string> entityCodes = new List<string>();

		// Token: 0x04000003 RID: 3
		private readonly EntityPartitioning partitionUtil;

		// Token: 0x04000004 RID: 4
		private readonly float seekingRange;

		// Token: 0x04000005 RID: 5
		private long lastSearchTotalMs;
	}
}
