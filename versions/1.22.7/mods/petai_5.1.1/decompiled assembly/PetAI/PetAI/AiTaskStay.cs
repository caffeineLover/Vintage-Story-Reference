using System;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace PetAI
{
	// Token: 0x0200000B RID: 11
	public class AiTaskStay : AiTaskBase
	{
		// Token: 0x06000029 RID: 41 RVA: 0x000032D4 File Offset: 0x000014D4
		public AiTaskStay(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig) : base(entity, taskConfig, aiConfig)
		{
			if (taskConfig["movespeed"] != null)
			{
				this.moveSpeed = taskConfig["movespeed"].AsFloat(0.01f);
			}
			if (taskConfig["maxdistance"] != null)
			{
				this.maxDistance = taskConfig["maxdistance"].AsFloat(10f);
			}
			if (taskConfig["command"] != null)
			{
				this.commandName = taskConfig["command"].AsString("stay");
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00003384 File Offset: 0x00001584
		public override bool ShouldExecute()
		{
			EntityAgent entity = this.entity;
			bool flag;
			if (entity == null)
			{
				flag = false;
			}
			else
			{
				EntityBehaviorRideable behavior = entity.GetBehavior<EntityBehaviorRideable>();
				flag = ((behavior != null) ? new bool?(behavior.AnyMounted()) : default(bool?)).GetValueOrDefault();
			}
			if (flag)
			{
				this.x = default(double?);
				this.y = default(double?);
				this.z = default(double?);
				EntityAgent entity2 = this.entity;
				bool flag2;
				if (entity2 == null)
				{
					flag2 = (null != null);
				}
				else
				{
					SyncedTreeAttribute watchedAttributes = entity2.WatchedAttributes;
					flag2 = (((watchedAttributes != null) ? watchedAttributes.GetTreeAttribute("staylocation") : null) != null);
				}
				if (flag2)
				{
					EntityAgent entity3 = this.entity;
					if (entity3 != null)
					{
						SyncedTreeAttribute watchedAttributes2 = entity3.WatchedAttributes;
						if (watchedAttributes2 != null)
						{
							watchedAttributes2.RemoveAttribute("staylocation");
						}
					}
				}
				return false;
			}
			EntityAgent entity4 = this.entity;
			string text;
			if (entity4 == null)
			{
				text = null;
			}
			else
			{
				EntityBehaviorReceiveCommand behavior2 = entity4.GetBehavior<EntityBehaviorReceiveCommand>();
				text = ((behavior2 != null) ? behavior2.ComplexCommand : null);
			}
			if (text != this.commandName)
			{
				this.x = default(double?);
				this.y = default(double?);
				this.z = default(double?);
				return false;
			}
			if (this.x == null || this.y == null || this.z == null)
			{
				EntityAgent entity5 = this.entity;
				ITreeAttribute treeAttribute;
				if (entity5 == null)
				{
					treeAttribute = null;
				}
				else
				{
					SyncedTreeAttribute watchedAttributes3 = entity5.WatchedAttributes;
					treeAttribute = ((watchedAttributes3 != null) ? watchedAttributes3.GetTreeAttribute("staylocation") : null);
				}
				ITreeAttribute home = treeAttribute;
				this.x = ((home != null) ? home.TryGetDouble("x") : default(double?));
				this.y = ((home != null) ? home.TryGetDouble("y") : default(double?));
				this.z = ((home != null) ? home.TryGetDouble("z") : default(double?));
				return false;
			}
			return this.entity.GetBehavior<EntityBehaviorReceiveCommand>().ComplexCommand == this.commandName && this.entity.Pos.SquareDistanceTo((float)this.x.Value, (float)this.y.Value, (float)this.z.Value) > this.maxDistance * this.maxDistance;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00003594 File Offset: 0x00001794
		public override void StartExecute()
		{
			base.StartExecute();
			if (this.x != null && this.y != null && this.z != null)
			{
				this.pathTraverser.WalkTowards(new Vec3d(this.x.Value, this.y.Value, this.z.Value), this.moveSpeed, this.maxDistance / 2f, new Action(this.OnGoalReached), new Action(this.OnStuck), 0);
			}
			this.stuck = false;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00003638 File Offset: 0x00001838
		public override bool ContinueExecute(float dt)
		{
			if (this.x == null || this.y == null || this.z == null)
			{
				return false;
			}
			if (this.entity.Pos.SquareDistanceTo(this.x.Value, this.y.Value, this.z.Value) < this.maxDistance * this.maxDistance / 4f)
			{
				this.pathTraverser.Stop();
				return false;
			}
			return !this.stuck && this.pathTraverser.Active;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000036D9 File Offset: 0x000018D9
		private void OnStuck()
		{
			this.stuck = true;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x000036E2 File Offset: 0x000018E2
		private void OnGoalReached()
		{
		}

		// Token: 0x0400001D RID: 29
		private double? x;

		// Token: 0x0400001E RID: 30
		private double? y;

		// Token: 0x0400001F RID: 31
		private double? z;

		// Token: 0x04000020 RID: 32
		private readonly float moveSpeed = 0.01f;

		// Token: 0x04000021 RID: 33
		private readonly float maxDistance = 10f;

		// Token: 0x04000022 RID: 34
		private bool stuck;

		// Token: 0x04000023 RID: 35
		private readonly string commandName = "stay";
	}
}
