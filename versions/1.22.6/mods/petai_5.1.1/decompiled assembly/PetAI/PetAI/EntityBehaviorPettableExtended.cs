using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace PetAI
{
	// Token: 0x02000012 RID: 18
	public class EntityBehaviorPettableExtended : EntityBehaviorPettable
	{
		// Token: 0x06000049 RID: 73 RVA: 0x00003AC0 File Offset: 0x00001CC0
		public EntityBehaviorPettableExtended(Entity entity) : base(entity)
		{
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00003ACC File Offset: 0x00001CCC
		public override void OnInteract(EntityAgent byEntity, ItemSlot itemslot, Vec3d hitPosition, EnumInteractMode mode, ref EnumHandling handled)
		{
			base.OnInteract(byEntity, itemslot, hitPosition, mode, ref handled);
			if (this.lastObedienceIncrease + 1000L < byEntity.World.ElapsedMilliseconds && this.entity.HasBehavior<EntityBehaviorTameable>())
			{
				this.lastObedienceIncrease = byEntity.World.ElapsedMilliseconds;
				this.entity.GetBehavior<EntityBehaviorTameable>().Obedience += 0.004f;
			}
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00003B3A File Offset: 0x00001D3A
		public override string PropertyName()
		{
			return "pettableextended";
		}

		// Token: 0x04000033 RID: 51
		private long lastObedienceIncrease;
	}
}
