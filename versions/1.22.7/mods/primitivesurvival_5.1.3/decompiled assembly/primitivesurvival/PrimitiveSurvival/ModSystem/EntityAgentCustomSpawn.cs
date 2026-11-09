using System;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000035 RID: 53
	public class EntityAgentCustomSpawn : EntityAgent
	{
		// Token: 0x06000255 RID: 597 RVA: 0x0001BB8C File Offset: 0x00019D8C
		public override void OnEntitySpawn()
		{
			base.OnEntitySpawn();
			JsonObject attributes = base.Properties.Attributes;
			string[] array;
			if (attributes == null)
			{
				array = null;
			}
			else
			{
				JsonObject jsonObject = attributes["canSpawnOn"];
				array = ((jsonObject != null) ? jsonObject.AsArray<string>(null, "game") : null);
			}
			string[] canspawnon = array;
			if (canspawnon != null)
			{
				string path = this.World.BlockAccessor.GetBlock(base.Pos.AsBlockPos.Add(0, -1, 0), 0).Code.Path;
				bool canSpawn = false;
				foreach (string entry in canspawnon)
				{
					if (path.StartsWith(entry))
					{
						canSpawn = true;
					}
				}
				if (!canSpawn)
				{
					this.Die(2, null);
				}
			}
		}
	}
}
