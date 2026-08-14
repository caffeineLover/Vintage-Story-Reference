using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Util;

namespace PrimitiveSurvival.ModSystem
{
	// Token: 0x02000041 RID: 65
	public class ItemBioluminescent : Item
	{
		// Token: 0x0600029D RID: 669 RVA: 0x0001E8EA File Offset: 0x0001CAEA
		public override string GetHeldTpUseAnimation(ItemSlot activeHotbarSlot, Entity byEntity)
		{
			return null;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0001E8F0 File Offset: 0x0001CAF0
		public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling)
		{
			if (byEntity.Controls.Sneak && blockSel != null)
			{
				IPlayer player = byEntity.World.PlayerByUid((byEntity as EntityPlayer).PlayerUID);
				if (!byEntity.World.Claims.TryAccess(player, blockSel.Position, 1))
				{
					return;
				}
				if (!(byEntity is EntityPlayer) || player.WorldData.CurrentGameMode != 2)
				{
					slot.TakeOut(1);
					slot.MarkDirty();
				}
				AssetLocation location = new AssetLocation(this.Code.Domain, "bioluminescent-" + base.CodeEndWithoutParts(1));
				EntityProperties type = byEntity.World.GetEntityType(location);
				if (type == null)
				{
					byEntity.World.Logger.Error("ItemCreature: No such entity - {0}", new object[]
					{
						location
					});
					if (this.api.World.Side == 2)
					{
						(this.api as ICoreClientAPI).TriggerIngameError(this, "nosuchentity", "No such entity '{0}' loaded.");
					}
					return;
				}
				Entity entity = byEntity.World.ClassRegistry.CreateEntity(type);
				if (entity != null)
				{
					entity.Pos.X = (double)((float)(blockSel.Position.X + (blockSel.DidOffset ? 0 : blockSel.Face.Normali.X)) + 0.5f);
					entity.Pos.Y = (double)(blockSel.Position.Y + (blockSel.DidOffset ? 0 : blockSel.Face.Normali.Y));
					entity.Pos.Z = (double)((float)(blockSel.Position.Z + (blockSel.DidOffset ? 0 : blockSel.Face.Normali.Z)) + 0.5f);
					entity.Pos.Yaw = (float)byEntity.World.Rand.NextDouble() * 2f * 3.1415927f;
					entity.Pos.SetFrom(entity.Pos);
					entity.PositionBeforeFalling.Set(entity.Pos.X, entity.Pos.Y, entity.Pos.Z);
					entity.Attributes.SetString("origin", "playerplaced");
					byEntity.World.SpawnEntity(entity);
					handHandling = 3;
					return;
				}
			}
			else
			{
				base.OnHeldInteractStart(slot, byEntity, blockSel, entitySel, firstEvent, ref handHandling);
			}
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0001EB44 File Offset: 0x0001CD44
		public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot)
		{
			return ArrayExtensions.Append<WorldInteraction>(new WorldInteraction[]
			{
				new WorldInteraction
				{
					HotKeyCode = "sneak",
					ActionLangCode = "heldhelp-place",
					MouseButton = 2
				}
			}, base.GetHeldInteractionHelp(inSlot));
		}
	}
}
