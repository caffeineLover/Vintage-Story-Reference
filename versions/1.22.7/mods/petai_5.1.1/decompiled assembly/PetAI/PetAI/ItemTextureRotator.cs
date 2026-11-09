using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;

namespace PetAI
{
	// Token: 0x0200001C RID: 28
	public class ItemTextureRotator : Item
	{
		// Token: 0x060000B2 RID: 178 RVA: 0x00006BB0 File Offset: 0x00004DB0
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handling)
		{
			Entity oldEntity = (entitySel != null) ? entitySel.Entity : null;
			ICoreServerAPI sapi = this.api as ICoreServerAPI;
			if (sapi != null && oldEntity != null && firstEvent)
			{
				int oldtexture = oldEntity.WatchedAttributes.GetInt("textureIndex", 0);
				oldEntity.WatchedAttributes.SetInt("textureIndex", (oldtexture + 1) % (oldEntity.Properties.Client.TexturesAlternatesCount + 1));
				oldEntity.WatchedAttributes.MarkPathDirty("textureIndex");
				sapi.World.SpawnEntity(PetUtil.EntityFromTree(PetUtil.EntityToTree(oldEntity), sapi.World));
				oldEntity.Die(7, null);
			}
			handling = 1;
		}
	}
}
