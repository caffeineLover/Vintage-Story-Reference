using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;

namespace PetAI
{
	// Token: 0x02000007 RID: 7
	public class AiTaskHappyDance : AiTaskBase
	{
		// Token: 0x06000012 RID: 18 RVA: 0x000026D8 File Offset: 0x000008D8
		public AiTaskHappyDance(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig) : base(entity, taskConfig, aiConfig)
		{
			JsonObject jsonObject = taskConfig["minduration"];
			this.minduration = ((jsonObject != null) ? jsonObject.AsInt(1000) : 1000);
			JsonObject jsonObject2 = taskConfig["maxduration"];
			this.maxduration = ((jsonObject2 != null) ? jsonObject2.AsInt(4000) : 4000);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000273C File Offset: 0x0000093C
		public override bool ShouldExecute()
		{
			if (this.tameable == null)
			{
				this.tameable = this.entity.GetBehavior<EntityBehaviorTameable>();
			}
			EntityBehaviorTameable entityBehaviorTameable = this.tameable;
			EntityPlayer entityPlayer;
			if (entityBehaviorTameable == null)
			{
				entityPlayer = null;
			}
			else
			{
				IPlayer cachedOwner = entityBehaviorTameable.CachedOwner;
				entityPlayer = ((cachedOwner != null) ? cachedOwner.Entity : null);
			}
			this.targetEntity = entityPlayer;
			return this.cooldownUntilMs < this.entity.World.ElapsedMilliseconds && this.targetEntity != null && this.targetEntity.Pos.SquareDistanceTo(this.entity.Pos) <= 25f && this.HasNiceThing(this.targetEntity);
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000027D8 File Offset: 0x000009D8
		public override void StartExecute()
		{
			base.StartExecute();
			this.durationUntilMs = this.entity.World.ElapsedMilliseconds + (long)this.minduration + (long)this.entity.World.Rand.Next(this.maxduration - this.minduration);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x00002830 File Offset: 0x00000A30
		public override bool ContinueExecute(float dt)
		{
			Vec3f targetVec = new Vec3f((float)(this.targetEntity.Pos.X - this.entity.Pos.X), (float)(this.targetEntity.Pos.Y - this.entity.Pos.Y), (float)(this.targetEntity.Pos.Z - this.entity.Pos.Z));
			float desiredYaw = (float)Math.Atan2((double)targetVec.X, (double)targetVec.Z);
			float yawDist = GameMath.AngleRadDistance(this.entity.Pos.Yaw, desiredYaw);
			this.entity.Pos.Yaw += GameMath.Clamp(yawDist, -250f * dt, 250f * dt);
			this.entity.Pos.Yaw = this.entity.Pos.Yaw % 6.2831855f;
			return this.durationUntilMs > this.entity.World.ElapsedMilliseconds && this.HasNiceThing(this.targetEntity) && base.ContinueExecute(dt);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002958 File Offset: 0x00000B58
		private bool HasNiceThing(EntityPlayer owner)
		{
			AiTaskHappyDance.<>c__DisplayClass9_0 CS$<>8__locals1 = new AiTaskHappyDance.<>c__DisplayClass9_0();
			AiTaskHappyDance.<>c__DisplayClass9_0 CS$<>8__locals2 = CS$<>8__locals1;
			string code;
			if (owner == null)
			{
				code = null;
			}
			else
			{
				ItemSlot rightHandItemSlot = owner.RightHandItemSlot;
				if (rightHandItemSlot == null)
				{
					code = null;
				}
				else
				{
					ItemStack itemstack = rightHandItemSlot.Itemstack;
					if (itemstack == null)
					{
						code = null;
					}
					else
					{
						Item item2 = itemstack.Item;
						code = ((item2 != null) ? item2.Code.Path : null);
					}
				}
			}
			CS$<>8__locals2.code = code;
			EntityBehaviorTameable entityBehaviorTameable = this.tameable;
			bool flag;
			if (entityBehaviorTameable == null)
			{
				flag = (null != null);
			}
			else
			{
				List<TamingItem> treatList = entityBehaviorTameable.treatList;
				flag = (((treatList != null) ? treatList.Find((TamingItem item) => item.Name == CS$<>8__locals1.code) : null) != null);
			}
			return flag || CS$<>8__locals1.code == "dogtoy";
		}

		// Token: 0x0400000B RID: 11
		protected EntityPlayer targetEntity;

		// Token: 0x0400000C RID: 12
		protected EntityBehaviorTameable tameable;

		// Token: 0x0400000D RID: 13
		protected int minduration;

		// Token: 0x0400000E RID: 14
		protected int maxduration;

		// Token: 0x0400000F RID: 15
		protected long durationUntilMs;
	}
}
