using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using RPVoiceChat.GameContent.BlockEntityBehavior;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

// Token: 0x02000007 RID: 7
public class BlockEntityBehaviorRegistry
{
	// Token: 0x06000028 RID: 40 RVA: 0x00002D48 File Offset: 0x00000F48
	public static void RegisterBlockEntityBehaviors(ICoreAPI api)
	{
		api.RegisterBlockEntityBehaviorClass("BERingable", typeof(BEBehaviorRingable));
		api.RegisterBlockEntityBehaviorClass("BELightable", typeof(BEBehaviorLightable));
		api.RegisterBlockEntityBehaviorClass("BESoundable", typeof(BEBehaviorSoundable));
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00002D94 File Offset: 0x00000F94
	public static void AddBlockEntityBehaviors(ICoreAPI api)
	{
		foreach (Block block in api.World.Blocks)
		{
			if (block != null && !(block.Code == null))
			{
				BlockBehavior[] blockBehaviors = block.BlockBehaviors;
				bool flag;
				if (blockBehaviors == null)
				{
					flag = false;
				}
				else
				{
					flag = Enumerable.Any<BlockBehavior>(blockBehaviors, (BlockBehavior behav) => behav.GetType().Name == "BlockBehaviorDoor");
				}
				BlockBehavior[] blockBehaviors2 = block.BlockBehaviors;
				bool flag2;
				if (blockBehaviors2 == null)
				{
					flag2 = false;
				}
				else
				{
					flag2 = Enumerable.Any<BlockBehavior>(blockBehaviors2, (BlockBehavior behav) => behav.GetType().Name == "BlockBehaviorTrapDoor");
				}
				bool isTrapDoor = flag2;
				if (flag || isTrapDoor)
				{
					BlockEntityBehaviorType[] blockEntityBehaviors = block.BlockEntityBehaviors;
					bool flag3;
					if (blockEntityBehaviors == null)
					{
						flag3 = false;
					}
					else
					{
						flag3 = Enumerable.Any<BlockEntityBehaviorType>(blockEntityBehaviors, (BlockEntityBehaviorType b) => b.Name == "BERingable");
					}
					if (!flag3)
					{
						List<BlockEntityBehaviorType> newBehaviors = Enumerable.ToList<BlockEntityBehaviorType>(block.BlockEntityBehaviors ?? new BlockEntityBehaviorType[0]);
						JsonObject jsonProps = new JsonObject(new JObject());
						JsonObject attributes = block.Attributes;
						bool flag4;
						if (attributes == null)
						{
							flag4 = false;
						}
						else
						{
							JsonObject jsonObject = attributes["bellPartCode"];
							flag4 = ((jsonObject != null) ? new bool?(jsonObject.Exists) : default(bool?)).GetValueOrDefault();
						}
						if (flag4)
						{
							jsonProps.Token["bellPartCode"] = block.Attributes["bellPartCode"].AsString(null);
						}
						BlockEntityBehaviorType ringableBehavior = new BlockEntityBehaviorType
						{
							Name = "BERingable",
							properties = jsonProps
						};
						newBehaviors.Add(ringableBehavior);
						block.BlockEntityBehaviors = newBehaviors.ToArray();
					}
				}
			}
		}
	}
}
