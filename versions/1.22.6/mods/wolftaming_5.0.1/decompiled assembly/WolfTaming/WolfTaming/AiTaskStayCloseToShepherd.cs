using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace WolfTaming
{
	// Token: 0x02000003 RID: 3
	public class AiTaskStayCloseToShepherd : AiTaskStayCloseToEntity
	{
		// Token: 0x0600000E RID: 14 RVA: 0x0000244D File Offset: 0x0000064D
		public AiTaskStayCloseToShepherd(EntityAgent entity, JsonObject taskConfig, JsonObject aiConfig) : base(entity, taskConfig, aiConfig)
		{
		}

		// Token: 0x0600000F RID: 15 RVA: 0x00002458 File Offset: 0x00000658
		public override bool ShouldExecute()
		{
			if (this.lastCheck + 20000L < this.entity.Api.World.ElapsedMilliseconds && this.entity.WatchedAttributes.GetInt("generation", 0) > 0)
			{
				this.lastCheck = this.entity.World.ElapsedMilliseconds;
				Entity candidate = this.entity.World.GetNearestEntity(this.entity.Pos.XYZ, this.range, 2f, delegate(Entity e)
				{
					if (e == null)
					{
						return false;
					}
					EntityProperties properties = e.Properties;
					bool? flag;
					if (properties == null)
					{
						flag = default(bool?);
					}
					else
					{
						JsonObject attributes = properties.Attributes;
						flag = ((attributes != null) ? new bool?(attributes["IsShepherd"].AsBool(false)) : default(bool?));
					}
					bool? flag2 = flag;
					return flag2.GetValueOrDefault();
				});
				if (candidate != null)
				{
					this.targetEntity = candidate;
				}
			}
			if (this.targetEntity == null)
			{
				return false;
			}
			double x = this.targetEntity.Pos.X;
			double y = this.targetEntity.Pos.Y;
			double z = this.targetEntity.Pos.Z;
			double dist = (double)this.entity.Pos.SquareDistanceTo(x, y, z);
			if (dist > (double)(this.maxDistance * this.maxDistance * 4f))
			{
				this.targetEntity = null;
				return false;
			}
			return dist > (double)(this.maxDistance * this.maxDistance);
		}

		// Token: 0x04000005 RID: 5
		public Entity shepherd;

		// Token: 0x04000006 RID: 6
		private long lastCheck;
	}
}
