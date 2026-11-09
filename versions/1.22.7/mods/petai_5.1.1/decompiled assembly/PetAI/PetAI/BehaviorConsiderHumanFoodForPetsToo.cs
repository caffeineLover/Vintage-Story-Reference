using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace PetAI
{
	// Token: 0x02000019 RID: 25
	internal class BehaviorConsiderHumanFoodForPetsToo : CollectibleBehavior
	{
		// Token: 0x060000A8 RID: 168 RVA: 0x0000645A File Offset: 0x0000465A
		public BehaviorConsiderHumanFoodForPetsToo(CollectibleObject collObj) : base(collObj)
		{
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00006463 File Offset: 0x00004663
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling, ref EnumHandling handling)
		{
			object obj;
			if (entitySel == null)
			{
				obj = null;
			}
			else
			{
				Entity entity = entitySel.Entity;
				obj = ((entity != null) ? entity.GetBehavior<EntityBehaviorTameable>() : null);
			}
			object obj2 = obj;
			if (obj2 != null)
			{
				obj2.OnInteract(byEntity, slot, null, 1, ref handling);
			}
			base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handHandling, ref handling);
		}
	}
}
