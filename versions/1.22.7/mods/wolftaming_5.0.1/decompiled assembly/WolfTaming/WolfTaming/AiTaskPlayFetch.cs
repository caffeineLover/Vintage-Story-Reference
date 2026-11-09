using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace WolfTaming
{
	// Token: 0x02000002 RID: 2
	public class AiTaskPlayFetch : AiTaskBase
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002058 File Offset: 0x00000258
		public EntityItem DogToy { get; set; }

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002061 File Offset: 0x00000261
		// (set) Token: 0x06000004 RID: 4 RVA: 0x00002069 File Offset: 0x00000269
		private Entity Owner { get; set; }

		// Token: 0x06000005 RID: 5 RVA: 0x00002072 File Offset: 0x00000272
		public AiTaskPlayFetch(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig) : base(entity, taskConfig, aiConfig)
		{
			if (taskConfig["movespeed"] != null)
			{
				this.moveSpeed = taskConfig["movespeed"].AsFloat(0.03f);
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020B0 File Offset: 0x000002B0
		public override bool ShouldExecute()
		{
			if (this.DogToy == null || !this.DogToy.Alive || this.DogToy.ShouldDespawn)
			{
				return false;
			}
			if (this.entity.Pos.SquareDistanceTo(this.DogToy.Pos) > 900f)
			{
				this.DogToy = null;
			}
			return this.DogToy != null;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002114 File Offset: 0x00000314
		public override void StartExecute()
		{
			base.StartExecute();
			float size = this.DogToy.CollisionBox.XSize;
			this.pathTraverser.WalkTowards(this.DogToy.Pos.XYZ, this.moveSpeed, size + 0.2f, new Action(this.OnGoalReached), new Action(this.OnStuck), 0);
			this.stuck = false;
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002181 File Offset: 0x00000381
		public override bool ContinueExecute(float dt)
		{
			if (this.DogToy != null)
			{
				return this.GetToy();
			}
			return this.BringToy();
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002198 File Offset: 0x00000398
		private bool GetToy()
		{
			if (this.DogToy == null || !this.DogToy.Alive || this.stuck)
			{
				this.pathTraverser.Stop();
				return false;
			}
			double x = this.DogToy.Pos.X;
			double y = this.DogToy.Pos.Y;
			double z = this.DogToy.Pos.Z;
			this.pathTraverser.CurrentTarget.X = x;
			this.pathTraverser.CurrentTarget.Y = y;
			this.pathTraverser.CurrentTarget.Z = z;
			if (this.entity.Pos.SquareDistanceTo(x, y, z) < 2f)
			{
				if (new DummySlot(this.DogToy.Itemstack).TryPutInto(this.entity.World, this.entity.LeftHandItemSlot, 1) > 0)
				{
					this.DogToy.Die(3, null);
				}
				this.DogToy = null;
				this.IncreaseObedience();
				return true;
			}
			return this.pathTraverser.Active;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000022A8 File Offset: 0x000004A8
		private void IncreaseObedience()
		{
			ITreeAttribute domesticationStatus = this.entity.WatchedAttributes.GetOrAddTreeAttribute("domesticationstatus");
			domesticationStatus.SetFloat("obedience", domesticationStatus.GetFloat("obedience", 0f) + 0.03f);
			this.entity.WatchedAttributes.MarkPathDirty("domesticationstatus");
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002304 File Offset: 0x00000504
		private bool BringToy()
		{
			if (this.Owner == null || !this.Owner.Alive || this.Owner.ShouldDespawn)
			{
				this.Owner = this.entity.World.GetNearestEntity(this.entity.Pos.XYZ, 50f, 10f, (Entity entity) => entity is EntityPlayer);
				if (this.Owner == null)
				{
					return false;
				}
			}
			double x = this.Owner.Pos.X;
			double y = this.Owner.Pos.Y;
			double z = this.Owner.Pos.Z;
			this.pathTraverser.CurrentTarget.X = x;
			this.pathTraverser.CurrentTarget.Y = y;
			this.pathTraverser.CurrentTarget.Z = z;
			if (this.entity.Pos.SquareDistanceTo(x, y, z) < 2f)
			{
				this.pathTraverser.Stop();
				this.Owner = null;
				return false;
			}
			return this.Owner.Alive && !this.stuck && this.pathTraverser.Active;
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002442 File Offset: 0x00000642
		private void OnStuck()
		{
			this.stuck = true;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000244B File Offset: 0x0000064B
		private void OnGoalReached()
		{
		}

		// Token: 0x04000003 RID: 3
		public float moveSpeed = 0.03f;

		// Token: 0x04000004 RID: 4
		private bool stuck;
	}
}
