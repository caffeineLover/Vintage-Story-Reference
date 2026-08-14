using System;
using System.IO;
using System.Text;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace PetAI
{
	// Token: 0x02000027 RID: 39
	public class PetUtil
	{
		// Token: 0x060000D9 RID: 217 RVA: 0x00007450 File Offset: 0x00005650
		public static ITreeAttribute EntityToTree(Entity entity)
		{
			TreeAttribute entityTree = new TreeAttribute();
			entityTree.SetString("class", entity.Api.World.ClassRegistry.GetEntityClassName(entity.GetType()));
			using (MemoryStream ms = new MemoryStream())
			{
				using (BinaryWriter writer = new BinaryWriter(ms, Encoding.UTF8))
				{
					entity.ToBytes(writer, false);
					writer.Flush();
					entityTree.SetBytes("pet", ms.ToArray());
				}
			}
			return entityTree;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x000074F0 File Offset: 0x000056F0
		public static Entity EntityFromTree(ITreeAttribute entityTree, IWorldAccessor world)
		{
			if (entityTree == null)
			{
				return null;
			}
			Entity result;
			using (MemoryStream ms = new MemoryStream(entityTree.GetBytes("pet", null)))
			{
				using (BinaryReader reader = new BinaryReader(ms, Encoding.UTF8))
				{
					Entity entity = world.ClassRegistry.CreateEntity(entityTree.GetString("class", null));
					entity.FromBytes(reader, false);
					result = entity;
				}
			}
			return result;
		}
	}
}
