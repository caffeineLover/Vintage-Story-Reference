using System;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace Cats
{
	// Token: 0x02000003 RID: 3
	public class AiTaskPlayWithEntity : AiTaskSeekEntity
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000022C0 File Offset: 0x000004C0
		public AiTaskPlayWithEntity(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig) : base(entity, taskConfig, aiConfig)
		{
			this.playAnimations = taskConfig["playAnimations"].AsArray<string>(Array.Empty<string>(), "game");
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000022EC File Offset: 0x000004EC
		public override bool ShouldExecute()
		{
			long ellapsedMs = this.entity.World.ElapsedMilliseconds;
			if (this.lastCheck + this.cooldownUntilMs < ellapsedMs)
			{
				this.lastCheck = ellapsedMs;
				this.targetEntity = this.partitionUtil.GetNearestInteractableEntity(this.entity.Pos.XYZ, (double)this.seekingRange, (Entity e) => this.IsTargetableEntity(e, this.seekingRange));
				if (this.targetEntity != null)
				{
					this.targetPos = this.targetEntity.Pos.XYZ;
					this.finished = false;
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002380 File Offset: 0x00000580
		public override bool ContinueExecute(float dt)
		{
			long elapsedMs = this.entity.World.ElapsedMilliseconds;
			if (this.finished)
			{
				return this.finishedAt + 1000L >= elapsedMs;
			}
			if (this.entity.Pos.SquareHorDistanceTo(this.targetEntity.Pos.XYZ) < (double)(base.MinDistanceToTarget() * 2f))
			{
				this.finished = true;
				this.finishedAt = elapsedMs;
				this.pathTraverser.Stop();
				if (this.playAnimations.Length != 0)
				{
					string playAnimCode = this.playAnimations[this.entity.World.Rand.Next(0, this.playAnimations.Length)];
					AnimationMetaData playAnim = new AnimationMetaData
					{
						Code = playAnimCode.ToLowerInvariant(),
						Animation = playAnimCode.ToLowerInvariant(),
						AnimationSpeed = 1f
					}.Init();
					this.entity.AnimManager.StartAnimation(playAnim);
				}
				this.entity.AnimManager.StopAnimation(this.animMeta.Code);
				return true;
			}
			return base.ContinueExecute(dt);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002498 File Offset: 0x00000698
		public override bool IsTargetableEntity(Entity e, float range)
		{
			if (!e.Alive || e.EntityId == this.entity.EntityId || !this.CanSense(e, (double)range))
			{
				return false;
			}
			if (Enumerable.Contains<string>(this.targetEntityCodesExact, e.Code.Path))
			{
				return true;
			}
			for (int i = 0; i < this.targetEntityCodesBeginsWith.Length; i++)
			{
				if (e.Code.Path.StartsWith(this.targetEntityCodesBeginsWith[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04000006 RID: 6
		private long lastCheck;

		// Token: 0x04000007 RID: 7
		private readonly string[] playAnimations;

		// Token: 0x04000008 RID: 8
		private bool finished;

		// Token: 0x04000009 RID: 9
		private long finishedAt;
	}
}
