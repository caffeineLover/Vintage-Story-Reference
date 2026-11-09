using System;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Util;

// Token: 0x02000004 RID: 4
public class BlockBehaviorRegistry
{
	// Token: 0x0600000C RID: 12 RVA: 0x000025E5 File Offset: 0x000007E5
	public static void RegisterBlockEntityBehaviors(ICoreAPI api)
	{
		api.RegisterBlockBehaviorClass("Ringable", typeof(BehaviorRingable));
		api.RegisterBlockBehaviorClass("AttachWithDirection", typeof(BehaviorAttachWithDirection));
	}

	// Token: 0x0600000D RID: 13 RVA: 0x00002614 File Offset: 0x00000814
	public static void AddBehaviors(ICoreAPI api)
	{
		BehaviorRingable behaviour;
		foreach (Block block in Enumerable.Where<Block>(api.World.Blocks, delegate(Block x)
		{
			if (x != null && x.Code != null)
			{
				return Enumerable.Any<BlockBehavior>(x.BlockBehaviors, (BlockBehavior behaviour) => behaviour.GetType().Name == "BlockBehaviorDoor" || behaviour.GetType().Name == "BlockBehaviorTrapDoor");
			}
			return false;
		}))
		{
			behaviour = new BehaviorRingable(block);
			block.CollectibleBehaviors = ArrayExtensions.Append<CollectibleBehavior>(block.CollectibleBehaviors, behaviour);
			block.BlockBehaviors = ArrayExtensions.Append<BlockBehavior>(block.BlockBehaviors, behaviour);
		}
	}
}
